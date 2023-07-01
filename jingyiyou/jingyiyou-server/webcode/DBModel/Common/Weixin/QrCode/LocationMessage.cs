using ncc2019.Common.Weixin.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ncc2019.Common.Weixin.QrCode
{
    public class LocationMessage : MessageBase
    {
        public string Latitude { get; set; }//纬度
        public string Longitude { get; set; }//经度
        public string Precision { get; set; }//精度
        public static LocationMessage InitMessage(MessageBase message, string xmlStr)
        {
            LocationMessage locmessage = new LocationMessage();
            try
            {
                
                StringReader sr = new StringReader(xmlStr);
                XmlReader reader = XmlReader.Create(sr);
                var doc = XDocument.Load(reader);
                locmessage = (from c in doc.Descendants("xml")
                             select new LocationMessage()
                             {
                                 Latitude = c.Element("Latitude") == null ? "0" : c.Element("Latitude").Value,
                                 Longitude = c.Element("Longitude") == null ? "0" : c.Element("Longitude").Value,
                                 Precision = c.Element("Precision") == null ? "0" : c.Element("Precision").Value
                             }).First();

                sr.Close();
                reader.Close();

                message.MakeMessage(locmessage);
            }
            catch (Exception error)
            {

                Common.Tool.LoggerHelper.Debug(error.ToString());
            }

            return locmessage;
        }

        public void DoMessage()
        {
            Common.Tool.LoggerHelper.Debug(Latitude+"===="+ Latitude+"====="+ Precision);
            ZGGLocation locModel = new ZGGLocation()
            {
                CreateTime = DateTime.Now,
                WXOpenID = FromUserName,
                Latitude = double.Parse(Latitude),
                Longitude = double.Parse(Longitude),
                Precision = double.Parse(Precision)
            };
            db.ZGGLocation.Add(locModel);
            db.SaveChanges();
            

        }
    }
}
