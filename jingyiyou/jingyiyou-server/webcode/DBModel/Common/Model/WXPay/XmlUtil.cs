using System;
using System.IO;
using System.Xml.Serialization;

public class XmlUtil
{
    public static T XmlToObect<T>(string xml)
    {
        using (StringReader sr = new StringReader(xml))
        {
            XmlSerializer xmldes = new XmlSerializer(typeof(T));
            return (T)xmldes.Deserialize(sr);
        }
    }

}