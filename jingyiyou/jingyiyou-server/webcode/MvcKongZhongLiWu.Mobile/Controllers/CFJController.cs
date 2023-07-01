using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ncc2019.Common.Enum;
using ncc2019.Common.Tool;
using ncc2019.Common.BLL;

namespace ncc2019.Controllers
{
    public class CFJController : ControllerBaseNoCheck
    {
        public ActionResult Index(string mcode)
        {

            ViewBag.mcode = mcode;
            return View();
        }

        public JsonResult GetQrCodeUrl(string mcode, string key)
        {
            string guid_code = "";
            string url = "";

            guid_code = mcode.Trim();


            url = "http://" + SettingBLL.MobileDomain + "/CFJ/QrCode?key=" + DESEncrypt.Encrypt(guid_code);

            return Json(new { state = "ok", key = guid_code, url = url });
        }

        public JsonResult GetQrCodeUrl_back(string mcode, string key)
        {
            string guid_code = "";
            string url = "";
            if (key.Trim() == "")
            {
                //生成一个新的Code
                guid_code = CreateCode(mcode);

            }
            else
            {
                guid_code = key.Trim();
                //去数据库查询是否使用过
                CFJQrCode qrcode = db.CFJQrCode.Where(c => c.Code == key && c.IsUsed == (int)ShiFouStatus.否).FirstOrDefault();
                if (qrcode == null)
                {
                    //生成一个新的Code
                    guid_code = CreateCode(mcode);
                }
            }

            url = "http://" + SettingBLL.MobileDomain + "/CFJ/QrCode?key=" + DESEncrypt.Encrypt(guid_code);

            return Json(new { state = "ok", key = guid_code, url = url });
        }

        private string CreateCode(string mcode)
        {
            string guid_code = Guid.NewGuid().ToString().Replace("-", "");
            var machine = db.CFJMachine.Where(c => c.MachineCode == mcode).FirstOrDefault();
            CFJQrCode qrcode = new CFJQrCode()
            {
                AddDate = DateTime.Now,
                Code = guid_code,
                IsUsed = (int)ShiFouStatus.否,
                MachineCode = mcode,
                GroupCode = machine.GroupCode
            };
            db.CFJQrCode.Add(qrcode);
            db.SaveChanges();

            return guid_code;
        }
        private void UseCode(string code)
        {

            CFJQrCode qrcode = db.CFJQrCode.Where(c => c.Code == code && c.IsUsed == (int)ShiFouStatus.否).FirstOrDefault();
            qrcode.IsUsed = (int)ShiFouStatus.是;
            qrcode.UsedDate = DateTime.Now;
            db.SaveChanges();


        }


        public FileResult QrCode(string key)
        {
            //返回图像
            QrCodeHelper qrHelper = new QrCodeHelper();
            qrHelper.Content = "http://" + SettingBLL.MobileDomain + "/CFJWX/MyInfo?key=" + key;
            byte[] buffer = qrHelper.Render();
            return File(buffer, "image/png");
        }

        public ActionResult OK()
        {
            return View();
        }

        public string SetCode(string code, string groupcode)
        {
            CFJMachine machine = db.CFJMachine.AsNoTracking().Where(c => c.GroupCode == groupcode).FirstOrDefault();
            if (machine != null)
            {
                //修改重复的code
                var machine2 = db.CFJMachine.AsNoTracking().Where(c => c.MachineCode == code).FirstOrDefault();
                if (machine2!=null)
                {
                    machine2.MachineCode = "_" + machine2.MachineCode;
                }

                machine.MachineCode = code;

                
                int cont = db.SaveChanges();
                return "ok";
            }
            else
            {
                return "error";
            }
        }

        public string WriteLog(string log, string machinecode)
        {
            string result = "error";
            try
            {
                CommonLog clog = new CommonLog()
                {
                    AddDate = DateTime.Now,
                    Content = log,
                    Agent = machinecode
                };
                db.CommonLog.Add(clog);
                db.SaveChanges();
                result = "ok";
            }
            catch (Exception)
            {


            }

            return result;

        }

        public string GetCommonParam(string name)
        {
            string value = "";
            //var param = db.CommonParam.Where(c => c.Name == name).FirstOrDefault();
            //if (param != null)
            //{
            //    value = param.Value;
            //}
            return value;
        }

        public string ExeSuccess(string machineCode,string cfjcid)
        {
            string result = "error";
            if (cfjcid!="")
            {
                try
                {
                    int int_cfjcid = int.Parse(cfjcid);
                    CFJControl control = db.CFJControl.Where(c => c.CFJControlID == int_cfjcid).FirstOrDefault();
                    var member = db.Members.Where(c => c.WechatOpenid == control.WXOpenID).FirstOrDefault();
                    member.CFJDayCount++;
                    control.IsRun = 1;
                    control.ExeTime = DateTime.Now;

                    db.SaveChanges();
                    result = "ok";
                }
                catch (Exception)
                {

                }
              
            }
            return result;
        }
    }
}
