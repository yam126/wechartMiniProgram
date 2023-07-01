using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Common.Tool
{
    public class SmtpHelper
    {
        private string FormEmail = "";
        public SmtpHelper()
        {

        }
        public static bool Send(string toemail, string url)
        {
            //简单邮件传输协议类
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = "smtp.exmail.qq.com";//邮件服务器
            client.Port = 25;//smtp主机上的端口号,默认是25.
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;//邮件发送方式:通过网络发送到SMTP服务器
            client.Credentials = new System.Net.NetworkCredential("password@kongzhongliwu.com", "killer007");//凭证,发件人登录邮箱的用户名和密码

            //电子邮件信息类
            System.Net.Mail.MailAddress fromAddress = new System.Net.Mail.MailAddress("password@kongzhongliwu.com", "密码管理");//发件人Email,在邮箱是这样显示的,[发件人:小明<panthervic@163.com>;]
            System.Net.Mail.MailAddress toAddress = new System.Net.Mail.MailAddress(toemail);//收件人Email,在邮箱是这样显示的, [收件人:小红<43327681@163.com>;]
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage(fromAddress, toAddress);//创建一个电子邮件类
            mailMessage.Subject = "找回密码";
            //string filePath = System.Web.HttpContext.Current.Server.MapPath("/index.html");//邮件的内容可以是一个html文本.
            //System.IO.StreamReader read = new System.IO.StreamReader(filePath, System.Text.Encoding.GetEncoding("GB2312"));
            //string mailBody = read.ReadToEnd();
            //read.Close();
            string mailBody = "请您点击下面的链接完成密码修改，如果不能点击请将以下链接复制到浏览器中进行打开。<br /> <a href='%url%' >%url%</a>";
            mailBody = mailBody.Replace("%url%", url);
            mailMessage.Body = mailBody;//可为html格式文本
            //mailMessage.Body = "邮件的内容";//可为html格式文本
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;//邮件主题编码
            mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("GB2312");//邮件内容编码
            mailMessage.IsBodyHtml = true;//邮件内容是否为html格式
            mailMessage.Priority = System.Net.Mail.MailPriority.High;//邮件的优先级,有三个值:高(在邮件主题前有一个红色感叹号,表示紧急),低(在邮件主题前有一个蓝色向下箭头,表示缓慢),正常(无显示).
            bool result = false;
            try
            {
                client.Send(mailMessage);//发送邮件
                //client.SendAsync(mailMessage, "ojb");异步方法发送邮件,不会阻塞线程.
                result = true;
            }
            catch (Exception)
            {
            }

            return result;
        }
        public static bool SendByPayTest(string toemail, string content)
        {
            //简单邮件传输协议类
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = "smtp.exmail.qq.com";//邮件服务器
            client.Port = 25;//smtp主机上的端口号,默认是25.
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;//邮件发送方式:通过网络发送到SMTP服务器
            client.Credentials = new System.Net.NetworkCredential("password@kongzhongliwu.com", "killer007");//凭证,发件人登录邮箱的用户名和密码

            //电子邮件信息类
            System.Net.Mail.MailAddress fromAddress = new System.Net.Mail.MailAddress("password@kongzhongliwu.com", "吹风机器人");//发件人Email,在邮箱是这样显示的,[发件人:小明<panthervic@163.com>;]
            System.Net.Mail.MailAddress toAddress = new System.Net.Mail.MailAddress(toemail);//收件人Email,在邮箱是这样显示的, [收件人:小红<43327681@163.com>;]
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage(fromAddress, toAddress);//创建一个电子邮件类
            mailMessage.Subject = "支付成功";
            //string filePath = System.Web.HttpContext.Current.Server.MapPath("/index.html");//邮件的内容可以是一个html文本.
            //System.IO.StreamReader read = new System.IO.StreamReader(filePath, System.Text.Encoding.GetEncoding("GB2312"));
            //string mailBody = read.ReadToEnd();
            //read.Close();
            string mailBody = "恭喜您成功支付了0.01元，吹风功能开启中，请稍等！" + content;
            //mailBody = mailBody.Replace("%url%", url);
            mailMessage.Body = mailBody;//可为html格式文本
            //mailMessage.Body = "邮件的内容";//可为html格式文本
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;//邮件主题编码
            mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("GB2312");//邮件内容编码
            mailMessage.IsBodyHtml = true;//邮件内容是否为html格式
            mailMessage.Priority = System.Net.Mail.MailPriority.High;//邮件的优先级,有三个值:高(在邮件主题前有一个红色感叹号,表示紧急),低(在邮件主题前有一个蓝色向下箭头,表示缓慢),正常(无显示).
            bool result = false;
            try
            {
                client.Send(mailMessage);//发送邮件
                //client.SendAsync(mailMessage, "ojb");异步方法发送邮件,不会阻塞线程.
                result = true;
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}
