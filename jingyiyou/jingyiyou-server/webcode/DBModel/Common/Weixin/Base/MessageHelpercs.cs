using ncc2019.Common.Tool;
using ncc2019.Common.Weixin.QrCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ncc2019.Common.Weixin.Base
{
    public class MessageHelpercs
    {
        public static MessageBase DoMessage(string xmlStr)
        {
            try
            {
                MessageBase message = InitMessage(xmlStr);

                if (message.MsgType == "event")
                {
                    LoggerHelper.Info(message.MsgType);
                    Enum.WXMessageType thetype = Enum.EnumTool.GetType<Enum.WXMessageType>(message.Event);
                    LoggerHelper.Info(message.Event);
                    switch (thetype)
                    {
                        case Enum.WXMessageType.subscribe:
                            {

                                GuanZhuMessage gzmessage = GuanZhuMessage.InitMessage(message, xmlStr);
                                gzmessage.DoMessage();
                                //QrCodeMessage qrmessage = QrCodeMessage.InitMessage(message, xmlStr);
                                //qrmessage.DoMessage();
                                break;
                            }
                        case ncc2019.Common.Enum.WXMessageType.SCAN:
                            {
                                //QrCodeMessage qrmessage = QrCodeMessage.InitMessage(message, xmlStr);
                                //qrmessage.DoMessage();
                                break;
                            }
                        case Enum.WXMessageType.unsubscribe:
                            {

                                GuanZhuMessage gzmessage = GuanZhuMessage.InitMessage(message, xmlStr);
                                gzmessage.DoMessage();


                                break;
                            }
                        case Enum.WXMessageType.LOCATION:
                            {
                                LoggerHelper.Info("LOCATION  InitMessage");
                                LocationMessage locmessage= LocationMessage.InitMessage(message, xmlStr);
                                LoggerHelper.Info("LOCATION  DoMessage");
                                locmessage.DoMessage();
                                break;
                            }
                        default:
                            break;
                    }
                }

            }
            catch (Exception error)
            {

                LoggerHelper.Debug(error.ToString());
            }
          

            return null;
        }

        private static MessageBase InitMessage(string xmlStr)
        {

            StringReader sr = new StringReader(xmlStr);
            XmlReader reader = XmlReader.Create(sr);
            var doc = XDocument.Load(reader);
            MessageBase message = (from c in doc.Descendants("xml")
                                   select new MessageBase()
                                   {

                                       ToUserName = c.Element("ToUserName") == null ? "0" : c.Element("ToUserName").Value,
                                       FromUserName = c.Element("FromUserName") == null ? "0" : c.Element("FromUserName").Value,
                                       MsgType = c.Element("MsgType") == null ? "0" : c.Element("MsgType").Value,
                                       Event = c.Element("Event") == null ? "0" : c.Element("Event").Value,
                                       EventKey = c.Element("EventKey") == null ? "0" : c.Element("EventKey").Value
                                   }).First();

            sr.Close();
            reader.Close();
            return message;
        }
    }


}
