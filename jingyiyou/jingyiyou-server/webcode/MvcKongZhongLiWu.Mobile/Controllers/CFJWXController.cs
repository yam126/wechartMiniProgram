using ncc2019.Common;
using ncc2019.Common.Enum;
using ncc2019.Common.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Controllers
{
    public class CFJWXController : ControllerBaseVIP
    {
        //
        // GET: /CFJWX/
        public ActionResult Info()
        {

            return View();
        }

        public ActionResult ShowSel(string mcode)
        {
            ViewBag.mcode = mcode;
            return View();
        }
        public ActionResult ShowMessage()
        {
            ViewBag.message = "恭喜您扫描成功，5秒后设备将自动打开，请耐心等待。";
            return View();
        }
        public ActionResult OpenCFj(string mcode)
        {

            var machine = db.CFJMachine.AsNoTracking().Where(c => c.GroupCode == mcode).FirstOrDefault();
            //var member = db.Members.AsNoTracking().FirstOrDefault(c => c.MemberID == CurMemberInfo.MemnerID);//1833
            //var member = db.Members.AsNoTracking().FirstOrDefault(c => c.MemberID == 1833);
           
            if (machine != null)
            {
                CFJControl control = new CFJControl();
                control.DelayTime = machine.DelayTime;
                control.Command = "open";
                control.AddDate = DateTime.Now;
                control.CFJQrCode = machine.MachineCode;//qrcode.Code;
                control.MachineCode = machine.MachineCode; //qrcode.MachineCode;
                control.Param = "1,2";
                control.AddMemberID = CurMemberInfo.MemnerID;
                control.IsRun = (int)ShiFouStatus.否;
                control.CZIP = machine.CZIP;
                control.GroupCode = machine.GroupCode;


                db.CFJControl.Add(control);
                db.SaveChanges();
                //return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }



            return Redirect("/cfjwx/ShowMessage");
        }
        public ActionResult XYJSel(string mcode)
        {
            ViewBag.mcode = mcode;
            return View();
        }
        public ActionResult OpenXYJ(string mcode, int xytype)
        {


            //var member = db.Members.AsNoTracking().FirstOrDefault(c => c.MemberID == CurMemberInfo.MemnerID);
            //var member = db.Members.AsNoTracking().FirstOrDefault(c => c.MemberID == 1833);
            var machine = db.CFJMachine.AsNoTracking().Where(c => c.GroupCode == mcode).FirstOrDefault();
            if (machine != null)
            {
                CFJControl control = new CFJControl();
                control.DelayTime = xytype;// 60 * xytype;
                control.Command = "open";
                control.AddDate = DateTime.Now;
                control.CFJQrCode = machine.MachineCode;//qrcode.Code;
                control.MachineCode = machine.MachineCode; //qrcode.MachineCode;
                control.Param = "6,7";
                control.AddMemberID = CurMemberInfo.MemnerID;
                control.IsRun = (int)ShiFouStatus.否;
                control.CZIP = machine.CZIP;
                control.GroupCode = machine.GroupCode;
                control.Func = xytype.ToString();


                db.CFJControl.Add(control);
                db.SaveChanges();
                //return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }



            return Redirect("/cfjwx/ShowMessage");
        }

        public ActionResult Index(int flag)
        {
            //if (flag != 1)
            //{
            //    Redirect("/cfjwx/info");
            //}
            var list = db.CFJMemberType.Where(c => c.IsPublic == (int)ShiFouStatus.是);
            return View(list);
        }
        [HttpPost]
        public ActionResult Index(string paytype)
        {
            if (paytype != "")
            {
                int paytypeInt = int.Parse(paytype);
                var membertype = db.CFJMemberType.Where(c => c.CFJMemberTypeID == paytypeInt).FirstOrDefault();
                //生成订单
                CFJPay pay = new CFJPay()
                {
                    AddDate = DateTime.Now,
                    MemberID = CurMemberInfo.MemnerID,
                    HeadImgUrl = CurMemberInfo.HeadImgUrl,
                    Payment = membertype.Payment,
                    MemberTypeID = membertype.CFJMemberTypeID,
                    Name = CurMemberInfo.Name,
                    State = (int)PayStatus.未支付,
                    NeedPay = membertype.Payment
                };
                db.CFJPay.Add(pay);
                db.SaveChanges();

                return Redirect("/CFJWX/getinfo?orderid=" + pay.CFJPayID);
            }
            else
            {
                ShowAlertMessage("出错啦，请稍后再试！");
                return View();
            }
        }

        public ActionResult MyInfo(string key)
        {


            //string de_key = DESEncrypt.Decrypt(key);
            //string mcode = de_key;
            string mcode = key;
            int memberType = 4;

            var machine = db.CFJMachine.AsNoTracking().Where(c => c.MachineCode == mcode).FirstOrDefault();
            //如果学校名称是00 则直接扫描成功 打开吹风机
            if (machine.UniversityName != "00")
            {
                var member = db.Members.AsNoTracking().FirstOrDefault(c => c.MemberID == CurMemberInfo.MemnerID);
                if (member != null && member.ISCFJUser != (int)ShiFouStatus.是)
                {
                    //return Redirect("/cfjwx/");
                    return Redirect("/cfjwx/info");//2016-4-16
                }
                else
                {
                    memberType = member.CFJMemberTypeID.Value;
                }
            }

            //如果没有找到还未执行的指令

            var control_unrun = db.CFJControl.AsNoTracking().Where(c => (c.MachineCode == mcode && c.IsRun == (int)ShiFouStatus.否)).FirstOrDefault();


            if (control_unrun != null)
            {
                //ViewBag.message = "扫码后发生错误(100)，请稍后再试！";
                ViewBag.message = "您的扫码过于频繁，请稍后再试！错误(100)";
                ViewBag.message = "恭喜您扫描成功，5秒后吹风机将自动打开，请耐心等待。";
            }
            else
            {
                CFJMemberType membertype = db.CFJMemberType.AsNoTracking().Where(c => c.CFJMemberTypeID == memberType).FirstOrDefault();
                //CFJMemberType membertype = db.CFJMemberType.AsNoTracking().Where(c => c.CFJMemberTypeID == member.CFJMemberTypeID).FirstOrDefault();2016-4-16
                //var lasttime = DateTime.Now.AddSeconds(-1 * membertype.DelayTime.Value);
                var lasttime = DateTime.Now.AddSeconds(-1);
                var control_runing = db.CFJControl.AsNoTracking().Where(c => (c.MachineCode == mcode && c.IsRun == (int)ShiFouStatus.是 && lasttime < c.ExeTime))
                    .OrderByDescending(c => c.CFJControlID).FirstOrDefault();
                if (control_runing != null)
                {
                    //ViewBag.message = "您的扫码过于频繁，请稍后再试！";
                    ViewBag.message = "恭喜您扫描成功，3秒后吹风机将自动打开，请耐心等待。";
                }
                else
                {

                    if (mcode != "")
                    {

                        CFJControl control = new CFJControl();
                        //打开吹风机
                        //CFJMachine machine = db.CFJMachine.Where(c => c.MachineCode == mcode).FirstOrDefault();

                        var Curmember = db.Members.AsNoTracking().Where(c => c.MemberID == CurMemberInfo.MemnerID).FirstOrDefault();
                        // CFJMemberType membertype = db.CFJMemberType.AsNoTracking().Where(c => c.CFJMemberTypeID == Curmember.CFJMemberTypeID).FirstOrDefault();
                        if (membertype != null)
                        {
                            control.DelayTime = membertype.DelayTime;

                            //检查吹风机的使用次数
                            DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                            DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 23, 59);
                            int daycount = db.CFJControl.AsNoTracking().Count(c => c.IsRun == (int)ShiFouStatus.是 && c.WXOpenID == CurMemberInfo.WeChatOpenid && c.ExeTime >= startTime && c.ExeTime <= endTime);

                            DateTime startTime_month = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                            DateTime endTime_month = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1, 0, 0, 0);
                            int monthcount = db.CFJControl.AsNoTracking().Count(c => c.IsRun == (int)ShiFouStatus.是 && c.WXOpenID == CurMemberInfo.WeChatOpenid && c.ExeTime >= startTime_month && c.ExeTime < endTime_month);

                            if (membertype.DayCount <= daycount)
                            {
                                //使用次数已经超过了
                                ViewBag.message = "对不起，今天的使用次数您已经用完了！";
                            }
                            else if (membertype.MonthCount <= monthcount)
                            {
                                ViewBag.message = "对不起，这个月的使用次数您已经用完了！";
                            }
                            else
                            {
                                control.Command = "open";
                                control.AddDate = DateTime.Now;
                                control.CFJQrCode = machine.MachineCode;//qrcode.Code;
                                control.MachineCode = machine.MachineCode; //qrcode.MachineCode;
                                control.WXOpenID = CurMemberInfo.WeChatOpenid;
                                control.IsRun = (int)ShiFouStatus.否;
                                control.CZIP = machine.CZIP;
                                control.GroupCode = machine.GroupCode;

                                //qrcode.IsUsed = (int)ShiFouStatus.是;
                                //qrcode.UsedDate = DateTime.Now;


                                db.CFJControl.Add(control);
                                db.SaveChanges();

                                ViewBag.message = string.Format("今天您的使用了{0}次，还剩{1}次，<br/>", daycount + 1, membertype.DayCount - daycount - 1);
                                ViewBag.message += string.Format("这个月您使用了{0}次，还剩{1}次。", monthcount + 1, membertype.MonthCount - monthcount - 1);
                            }
                        }
                        else
                        {
                            //ViewBag.message = "扫码后发生错误(110)，请稍后再试！";
                            ViewBag.message = "恭喜您扫描成功，6秒后吹风机将自动打开，请耐心等待。";
                        }
                    }
                }


            }

            return View();
        }

        public ActionResult MyInfo_back(string key)
        {
            var member = db.Members.AsNoTracking().FirstOrDefault(c => c.MemberID == CurMemberInfo.MemnerID);
            if (member != null && member.ISCFJUser != (int)ShiFouStatus.是)
            {
                return Redirect("/cfjwx/");
            }

            string de_key = DESEncrypt.Decrypt(key);
            string mcode = de_key;
            //去数据库查询是否使用过
            //CFJQrCode qrcode = db.CFJQrCode.Where(c => c.Code == de_key && c.IsUsed == (int)ShiFouStatus.否).FirstOrDefault();
            //if (qrcode == null)
            //{
            //    ViewBag.message = "扫码后发生错误，请稍后再试！";

            //}
            //else
            //{
            //    mcode = qrcode.MachineCode;

            //}


            if (mcode != "")
            {



                CFJControl control = new CFJControl();
                //打开吹风机
                CFJMachine machine = db.CFJMachine.Where(c => c.MachineCode == mcode).FirstOrDefault();

                CFJMemberType membertype = db.CFJMemberType.Where(c => c.CFJMemberTypeID == CurMemberInfo.CFJMemberTypeID).FirstOrDefault();
                if (membertype != null)
                {
                    control.DelayTime = membertype.DelayTime;

                    //检查吹风机的使用次数
                    DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 23, 59);
                    int daycount = db.CFJControl.AsNoTracking().Count(c => c.IsRun == (int)ShiFouStatus.是 && c.WXOpenID == CurMemberInfo.WeChatOpenid && c.ExeTime >= startTime && c.ExeTime <= endTime);

                    DateTime startTime_month = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                    DateTime endTime_month = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1, 0, 0, 0);
                    int monthcount = db.CFJControl.AsNoTracking().Count(c => c.IsRun == (int)ShiFouStatus.是 && c.WXOpenID == CurMemberInfo.WeChatOpenid && c.ExeTime >= startTime_month && c.ExeTime < endTime_month);

                    if (membertype.DayCount <= daycount || membertype.MonthCount <= monthcount)
                    {
                        //使用次数已经超过了
                        ViewBag.message = "对不起，今天的使用次数您已经用完了！";
                    }
                    else
                    {
                        control.Command = "open";
                        control.AddDate = DateTime.Now;
                        control.CFJQrCode = machine.MachineCode;//qrcode.Code;
                        control.MachineCode = machine.MachineCode; //qrcode.MachineCode;
                        control.WXOpenID = CurMemberInfo.WeChatOpenid;
                        control.IsRun = (int)ShiFouStatus.否;
                        control.CZIP = machine.CZIP;
                        control.GroupCode = machine.GroupCode;

                        //qrcode.IsUsed = (int)ShiFouStatus.是;
                        //qrcode.UsedDate = DateTime.Now;


                        db.CFJControl.Add(control);
                        db.SaveChanges();

                        ViewBag.message = string.Format("今天您的使用了{0}次，还剩{1}次，<br/>", daycount + 1, membertype.DayCount - daycount - 1);
                        ViewBag.message += string.Format("这个月您使用了{0}次，还剩{1}次。", monthcount + 1, membertype.MonthCount - monthcount - 1);
                    }
                }
                else
                {
                    ViewBag.message = "扫码后发生错误，请稍后再试！";
                    ViewBag.message = "恭喜您扫描成功，5秒后吹风机将自动打开，请耐心等待。";
                }



            }

            return View();
        }

        public ActionResult GetInfo(string orderid)
        {
            Members member = db.Members.Find(CurMemberInfo.MemnerID);
            if (member != null)
            {
                ViewBag.name = member.Name;
                ViewBag.phone = member.Phone;
                ViewBag.orderid = orderid;
            }
            return View();

        }
        [HttpPost]
        public ActionResult GetInfo(string orderid, string name, string phone)
        {
            if (name != "" && phone != "")
            {
                Members member = db.Members.Find(CurMemberInfo.MemnerID);
                member.Phone = phone;
                member.Name = name;
                db.SaveChanges();
            }
            return Redirect("/Pay/My?type=cfj&orderid=" + orderid);
        }

        public ActionResult Pay(int orderid)
        {
            CFJPay pay = db.CFJPay.Where(c => c.CFJPayID == orderid).FirstOrDefault();
            if (pay != null)
            {
                var membertype = db.CFJMemberType.Find(pay.MemberTypeID);
                ViewBag.paydata = Newtonsoft.Json.JsonConvert.SerializeObject(TenPayManager.MakeUpJsParam(CurMemberInfo.WeChatOpenid, membertype.Payment.Value, orderid));

                ViewBag.payment = membertype.Payment;
                ViewBag.memo = membertype.Memo;
                ViewBag.name = membertype.Name;
            }



            return View("/views/cfjwx/pay.cshtml");
        }


    }
}
