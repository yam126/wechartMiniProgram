using ncc2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class MainController : ControllerBase
    {
        //
        // GET: /Main/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {

            ViewBag.openid = this.CurMemberInfo.WeChatOpenid;
            ViewBag.js_json = TenPayManager.MakeUpJsParam();//jsconfig 参数

            return View();
        }
        /// <summary>
        /// 获取箱子列表
        /// </summary>
        /// <returns></returns>
        public ActionResult BoxList()
        {
            ViewBag.gzhid = System.Configuration.ConfigurationManager.AppSettings["gzhid"];
            ViewBag.js_json = TenPayManager.MakeUpJsParam();//jsconfig 参数
            return View();
        }
        /// <summary>
        /// 获取本地箱子列表
        /// </summary>
        /// <returns></returns>
        public ActionResult LocBoxList()
        {

            ZGGLocation locationModel = db.ZGGLocation.Where(c => c.WXOpenID == this.CurMemberInfo.WeChatOpenid).OrderByDescending(c => c.CreateTime).FirstOrDefault();
            double dLongitude = 0.001;
            double dLatitude = 0.001;
            if (locationModel != null)
            {
                List<ZGGMachine> machineList = db.ZGGMachine.Where(c => locationModel.Longitude >= c.Longitude - dLongitude && locationModel.Longitude <= c.Longitude + dLongitude
                && locationModel.Latitude <= c.Latitude + dLatitude && locationModel.Latitude >= c.Latitude - dLatitude).ToList();


                return View(machineList);

            }
            else
            {
                return View(new List<ZGGMachine>());
            }
        }

        /// <summary>
        /// 选择用户角色
        /// </summary>
        /// <returns></returns>
        public ActionResult UserChoice(string mid)
        {
            int midInt = int.Parse(Common.Tool.DESEncrypt.Decrypt(mid));
            ZGGMachine machineModel = db.ZGGMachine.Find(midInt);
            if (machineModel != null)
            {
                CurMemberInfo.CurMeachineID = machineModel.MachineID;
                //绑定机器
                base.BindMachine(machineModel.BackCode);
            }

            ViewBag.title = "我是谁？";
            ViewBag.btn1name = "我是快递员";
            ViewBag.btn2name = "我是邻居";
            ViewBag.btn1url = "/main/courier";
            ViewBag.btn2url = "/main/neighbour";
            return View("~/views/main/com2button.cshtml");
        }
        /// <summary>
        /// 邻居入口
        /// </summary>
        /// <returns></returns>
        public ActionResult Neighbour()
        {
            this.CurMemberInfo.MemberType = Common.Enum.MemberType.邻居;
            ViewBag.title = "我要做什么？";
            ViewBag.btn1name = "取快递";
            ViewBag.btn2name = "寄快递";
            ViewBag.btn1url = "/main/getmygg";
            ViewBag.btn2url = "/main/sendgg";
            return View("~/views/main/com2button.cshtml");
        }
        /// <summary>
        /// 快递员入口
        /// </summary>
        /// <returns></returns>
        public ActionResult Courier()
        {
            this.CurMemberInfo.MemberType = Common.Enum.MemberType.快递员;
            ViewBag.title = "我要做什么？";
            ViewBag.btn1name = "存快递包裹";
            //ViewBag.btn2name = "临时代存";
            ViewBag.btn2name = "取待邮寄包裹";
            ViewBag.btn1url = "/main/putgg";
            ViewBag.btn2url = "/main/getgg";
            //ViewBag.btn3url = "/main/";
            return View("~/views/main/com2button.cshtml");
        }
        /// <summary>
        /// 取我的包裹
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMyGG()
        {
            this.CurMemberInfo.EndingControl = true;
            return View();
        }
        [HttpPost]
        public ActionResult CheckPass(string passinput)
        {
            //this.CurMemberInfo.CurMeachineID = 1;
            //TODO:删除以上测试信息
            ZGGMachine machineModel = db.ZGGMachine.Find(this.CurMemberInfo.CurMeachineID);
            var controlModel = db.ZGGUseControl.Find(machineModel.ZGGUseControlID);
            if (controlModel.PassWord == passinput)
            {

                this.CurMemberInfo.ZGGLastOpenTime = DateTime.Now.AddMinutes(10);
                return Redirect("/Pay/My/?type=zgg");
            }
            else
            {
                ShowAlertMessage("密码错误！");
                return View("~/views/main/getmygg.cshtml");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ZggPayDetail()
        {
            this.CheckUser();
            ZGGMachine machineModel = db.ZGGMachine.Find(this.CurMemberInfo.CurMeachineID);
            ZGGUseControl controlModel = db.ZGGUseControl.Find(machineModel.ZGGUseControlID);
            int days = DateTime.Now.Subtract(controlModel.PutInDate.Value).Days + (DateTime.Now.Subtract(controlModel.PutInDate.Value).Minutes + 59) / 60;
            decimal payment = machineModel.UserPayment.Value * days;
            ZGGPay payModel = new ZGGPay()
            {
                AddDate = DateTime.Now,
                MemberID = CurMemberInfo.MemnerID,
                NeedPay = payment,
                Payment = payment,
                State = (int)Common.Enum.PayStatus.未支付

            };

            db.ZGGPay.Add(payModel);
            db.SaveChanges();

            controlModel.ZGGPayID = payModel.ZGGPayID;
            db.SaveChanges();

            ViewBag.payid = payModel.ZGGPayID;
            ViewBag.totalpay = payment;
            ViewBag.days = days;

            ViewBag.paydata = Newtonsoft.Json.JsonConvert.SerializeObject(TenPayManager.MakeUpJsParam(CurMemberInfo.WeChatOpenid, payModel));


            return View("~/views/main/zggpaydetail.cshtml");
        }

        public ActionResult ZggScan(string mid)
        {
            Common.Tool.LoggerHelper.Info(mid + "===" + Common.Tool.DESEncrypt.Decrypt(mid));
            int midInt = int.Parse(Common.Tool.DESEncrypt.Decrypt(mid));
            this.CurMemberInfo.CurMeachineID = midInt;


            var machineModel = db.ZGGMachine.Find(midInt);
            //绑定设备
            base.BindMachine(machineModel.BackCode);

            Common.Tool.LoggerHelper.Info(this.CurMemberInfo.WeChatOpenid + "===" + machineModel.WxOpenID);
            // return Redirect("http://we.qq.com/d/AQAhKKXBGmzC-9qyBcRRcIZZckoaxHg3yOzojmpc");
            if (machineModel != null && this.CurMemberInfo.WeChatOpenid == machineModel.WxOpenID)
            {
                Common.Tool.LoggerHelper.Info("init");
                this.CurMemberInfo.IsOwner = true;
                //直接开箱
                return Redirect("/main/zggopen");
            }


            //验证是否可以开门
            if (DateTime.Now > this.CurMemberInfo.ZGGLastOpenTime)
            {
                return Redirect("/main/UserChoice?mid=" + mid);
            }
            return Redirect("/main/zggopen");
        }
        public string Check()
        {
            string result = "error";
            if (this.CurMemberInfo.CurMeachineID > 0)
            {
                var machine = db.ZGGMachine.Find(this.CurMemberInfo.CurMeachineID);
                if (machine != null)
                {
                    var control = db.ZGGUseControl.Find(machine.ZGGUseControlID);
                    if (control != null)
                    {
                        result = "ok";
                    }
                }
                if (machine.WxOpenID == this.CurMemberInfo.WeChatOpenid)
                {
                    result = "ok";
                }
            }
            return result;
        }
        /// <summary>
        /// 打开智裹裹箱门
        /// </summary>
        /// <returns></returns>
        public ActionResult ZggOpen()
        {
            //try
            //{
            //    Common.Tool.HttpHelper http = new Common.Tool.HttpHelper();
            //    string command = Common.Tool.Base64Helper.EncodeBase64("command=#open2close*");
            //    //byte[] bytes = Encoding.Default.GetBytes("command=#open2close*");
            //    //string str = Convert.ToBase64String(bytes);
            //    string resultStr = http.Post("https://api.weixin.qq.com/device/transmsg?access_token=" + Common.Tool.TokenHelper.GetToken()
            //        , "{" + string.Format("\"device_type\":\"{0}\",\"device_id\":\"{1}\",\"open_id\": \"{2}\",\"content\": \"{3}\""
            //        , "gh_1ecefda99708", "gh_1ecefda99708_d4d0833990d5d22b", "oISilwcZBXZ7WZ4IygxTs6RCQyyY", "command=#open2close*") + "}");

            //    Common.Tool.LoggerHelper.Info(resultStr);
            //    //this.CurMemberInfo.CurMeachineID = int.Parse(mid);
            //}
            //catch (Exception error)
            //{

            //    ViewBag.error = error.ToString();
            //}

            //验证是否可以开门
            if (DateTime.Now > this.CurMemberInfo.ZGGLastOpenTime && this.CurMemberInfo.IsOwner != true)
            {
                return Redirect("/main/UserChoice");
            }
            //this.CurMemberInfo.CurMeachineID = int.Parse(Common.Tool.DESEncrypt.Decrypt(mid));

            ZGGMachine machineModel = db.ZGGMachine.Find(this.CurMemberInfo.CurMeachineID);

            //ViewBag.type = type;
            ViewBag.deveiceNo = machineModel.BackCode;
            var jresult = TenPayManager.MakeUpJsParam();
            ViewBag.js_json = jresult;//jsconfig 参数
            ViewBag.gzhid = System.Configuration.ConfigurationManager.AppSettings["gzhid"];
            //ViewBag.info = jresult.appId;

            return View();
        }
        /// <summary>
        /// 箱门已经被打开
        /// </summary>
        /// <returns></returns>
        public ActionResult ZggOpened()
        {
            //找到相应上下文流程控制
            ZGGMachine machineModel = db.ZGGMachine.Find(this.CurMemberInfo.CurMeachineID);
            var controlModel = db.ZGGUseControl.Find(machineModel.ZGGUseControlID);
            if (controlModel != null)
            {
                //若没有开门流程存在则跳转走
                if (this.CurMemberInfo.MemberType == Common.Enum.MemberType.快递员)
                {
                    ViewBag.memberType = "kd";
                    controlModel.PutInDate = DateTime.Now;
                    controlModel.KDUserID = CurMemberInfo.MemnerID;//快递员ID
                }
                else
                {
                    controlModel.GetOutDate = DateTime.Now;
                    controlModel.NeighborUserID = CurMemberInfo.MemnerID;//取件人ID

                }

                if (this.CurMemberInfo.EndingControl == true)
                {
                    //清除流程信息
                    machineModel.ZGGUseControlID = null;
                    machineModel.IsLocked = (int)Common.Enum.ShiFouStatus.否;
                }


                db.SaveChanges();
            }

            return View();
        }
        /// <summary>
        /// 邮寄快递
        /// </summary>
        /// <returns></returns>
        public ActionResult SendGG()
        {
            return View();
        }
        /// <summary>
        /// 寄件信息填写
        /// </summary>
        /// <returns></returns>
        public ActionResult SendDetail()
        {
            return View();
        }
        /// <summary>
        /// 存入包裹
        /// </summary>
        /// <returns></returns>
        public ActionResult PutGG()
        {
            this.CurMemberInfo.EndingControl = false;
            ViewBag.title = "打开箱门";
            ViewBag.btn1name = "已知密码，直接开箱";
            ViewBag.lbmemo1 = "使用<strong>快递单号</strong>或者<strong>收件人手机号</strong>";
            ViewBag.btn2name = "未知密码，付费开箱";
            ViewBag.lbmemo2 = "若为他人存入包裹则需要付费0.01元";
            ViewBag.btn1url = "/main/passopen";
            ViewBag.btn2url = "/Pay/My/?type=kdpay";
            return View("~/views/main/com2button.cshtml");
        }
        public ActionResult GetGG()
        {
            return View();
        }
        /// <summary>
        /// 快递员通过密码存包裹
        /// </summary>
        /// <returns></returns>
        public ActionResult PassOpen()
        {
            this.CurMemberInfo.EndingControl = true;
            return View();
        }
        [HttpPost]
        public ActionResult CheckPassKD(string passinput)
        {
            try
            {
                if (this.CurMemberInfo.CurMeachineID <= 0)
                {
                    return Redirect("/main/locboxlist");
                }
                //this.CurMemberInfo.CurMeachineID = 1;
                //TODO:删除以上测试信息
                //ViewBag.info = this.CurMemberInfo.CurMeachineID;
                ZGGMachine machineModel = db.ZGGMachine.Find(this.CurMemberInfo.CurMeachineID);
                if (machineModel.ZGGUseControlID == null || machineModel.ZGGUseControlID.Value <= 0)
                {
                    return Redirect("/common/message");
                }
                var controlModel = db.ZGGUseControl.Find(machineModel.ZGGUseControlID);
                if (controlModel.PassWord == passinput)
                {
                    ////添加控制信息
                    //ZGGUseControl controlModel = new ZGGUseControl()
                    //{
                    //    IsPrivate = 1,
                    //    KDUserID = this.CurMemberInfo.MemnerID,
                    //    Payed = 0,
                    //    PayState = (int)Common.Enum.PayStatus.未支付,
                    //    PaymentType = (int)Common.Enum.PaymentType.双方支付,
                    //    PutInDate = DateTime.Now,
                    //    ZGGMachineID = machineModel.MachineID

                    //};
                    //db.ZGGUseControl.Add(controlModel);
                    //db.SaveChanges();

                    this.CurMemberInfo.ZGGLastOpenTime = DateTime.Now.AddMinutes(10);

                    return Redirect("/main/ZggOpen?type=kd");
                }
                else
                {
                    ShowAlertMessage("密码错误！");
                    return View("~/views/main/passopen.cshtml");
                }
            }
            catch (Exception error)
            {

                ViewBag.error = error.ToString();
                return View("~/views/main/passopen.cshtml");
            }


        }
        /// <summary>
        /// 快递员通过支付存包裹
        /// </summary>
        /// <returns></returns>
        public ActionResult PayOpen()
        {
            //this.CurMemberInfo.CurMeachineID = 1;
            //this.CheckUser();
            ZGGMachine machineModel = db.ZGGMachine.Find(this.CurMemberInfo.CurMeachineID);
            //添加控制信息
            ZGGUseControl controlModel = new ZGGUseControl()
            {
                IsPrivate = 1,
                KDUserID = this.CurMemberInfo.MemnerID,
                Payed = 0,
                PayState = (int)Common.Enum.PayStatus.未支付,
                PaymentType = (int)Common.Enum.PaymentType.双方支付,
                PutInDate = DateTime.Now,
                ZGGMachineID = machineModel.MachineID
            };

            decimal payment = machineModel.CompanyPayment.Value;
            ZGGPay payModel = new ZGGPay()
            {
                AddDate = DateTime.Now,
                MemberID = CurMemberInfo.MemnerID,
                NeedPay = payment,
                Payment = payment,
                State = (int)Common.Enum.PayStatus.未支付

            };

            db.ZGGUseControl.Add(controlModel);
            db.ZGGPay.Add(payModel);
            db.SaveChanges();

            controlModel.ZGGPayID = payModel.ZGGPayID;
            machineModel.ZGGUseControlID = controlModel.ZGGUseControlID;
            db.SaveChanges();

            ViewBag.payid = payModel.ZGGPayID;
            ViewBag.totalpay = payment;


            ViewBag.paydata = Newtonsoft.Json.JsonConvert.SerializeObject(TenPayManager.MakeUpJsParam(CurMemberInfo.WeChatOpenid, payModel));


            return View("~/views/main/payopen.cshtml");
        }
        /// <summary>
        /// 为取件者设置开箱密码
        /// </summary>
        /// <returns></returns>
        public ActionResult SetPass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoSetPass(string passinput)
        {
            ZGGMachine machineModel = db.ZGGMachine.Find(CurMemberInfo.CurMeachineID);
            var controlModel = db.ZGGUseControl.Find(machineModel.ZGGUseControlID);
            controlModel.PassWord = passinput;

            db.SaveChanges();

            return Redirect("/main/zggopen?type=kd");
        }

        public ActionResult MyBoxList()
        {

            List<ZGGMachine> machineList = db.ZGGMachine.Where(c => c.WxOpenID == this.CurMemberInfo.WeChatOpenid).ToList();


            return View(machineList);
        }

        public ActionResult CommandList(string mid)
        {
            ViewBag.mid = mid;
            return View();
        }

        /// <summary>
        /// 跳转--微信打开箱门按钮
        /// </summary>
        /// <returns></returns>
        public ActionResult Jump()
        {
            if (this.CurMemberInfo.CurMeachineID <= 0)
            {
                return Redirect("/main/locboxlist");
            }


            return Redirect("/main/zggscan?mid=" + Common.Tool.DESEncrypt.Encrypt(this.CurMemberInfo.CurMeachineID));
        }
        public ActionResult AddControl(string mid)
        {
            ViewBag.mid = mid;
            int midInt = int.Parse(Common.Tool.DESEncrypt.Decrypt(mid));
            var machineModel = db.ZGGMachine.Find(midInt);
            if (machineModel.ZGGUseControlID != null)
            {
                ViewBag.title = "不可设置密码";
                ViewBag.message = "箱子正在使用中！";
            }
            else
            {
                ViewBag.title = "设置密码";
                ViewBag.message = "";
            }

            return View();
        }

        [HttpPost]
        public ActionResult DoMySetPass(string passinput, string mid)
        {
            int midInt = int.Parse(Common.Tool.DESEncrypt.Decrypt(mid));
            var machineModel = db.ZGGMachine.Find(midInt);

            ZGGUseControl controlModel = new ZGGUseControl()
            {
                IsPrivate = 1,
                KDUserID = this.CurMemberInfo.MemnerID,
                Payed = 0,
                PayState = (int)Common.Enum.PayStatus.未支付,
                PaymentType = (int)Common.Enum.PaymentType.双方支付,
                PutInDate = DateTime.Now,
                ZGGMachineID = midInt,
                PassWord = passinput
            };
            db.ZGGUseControl.Add(controlModel);
            db.SaveChanges();

            machineModel.ZGGUseControlID = controlModel.ZGGUseControlID;
            db.SaveChanges();

            return Redirect("/main/SetMyPassResult");
        }

        public ActionResult SetMyPassResult()
        {
            return View();
        }

        public ActionResult CallMaster()
        {
            return View();
        }

        public ActionResult NoRight()
        {
            return View();
        }
        public ActionResult UnDo()
        {
            return View();
        }
    }
}
