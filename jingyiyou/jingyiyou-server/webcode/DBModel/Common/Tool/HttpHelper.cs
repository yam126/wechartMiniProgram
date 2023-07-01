
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
namespace ncc2019.Common.Tool
{
    public class HttpHelper
    {
        private CookieContainer _cc = new CookieContainer();
        private int _delayTime;
        private string _lastUrl = string.Empty;
        private WebProxy _proxy;
        private int _timeout = 0x1d4c0;
        private int _tryTimes = 3;
        public string reqUserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
        public Encoding ReponseEncoding = Encoding.UTF8;

        public HttpHelper()
        {
            //this.PostContentType = "text/xml";
            this.PostContentType = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        }

        public string CurrentRedirectLocation { get; set; }

        public void Download(string url, string localfile)
        {
            new WebClient().DownloadFile(url, localfile);
        }

        public static string EncodePostData(string data)
        {
            return HttpUtility.UrlEncode(data);
        }

        public string Get(string url)
        {
            string str = this.Get(url, this._lastUrl);
            this._lastUrl = url;
            return str;
        }
        public string Get(string url, string referer)
        {
            return Get(url, referer, null);
        }
        private string CheckUrl(string url)
        {
            if (!url.Contains("http://") && !url.Contains("https://"))
            {
                url = "http://" + url;
                url = url.Replace("////", "//");
            }
            return url;
        }
        public string Get(string url, string referer, Cookie cookie)
        {
            int num = this._tryTimes;
            while (num-- > 0)
            {
                try
                {
                    if (this._delayTime > 0)
                    {
                        Thread.Sleep((int)(this._delayTime * 0x3e8));
                    }
                    url = CheckUrl(url);
                    HttpWebRequest request = null;
                    //如果是发送HTTPS请求  
                    if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                    {
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                        request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                        request.ProtocolVersion = HttpVersion.Version10;
                    }
                    else
                    {
                        request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                    }

                    request.AllowAutoRedirect = false;
                    request.UserAgent = this.reqUserAgent;
                    request.CookieContainer = this._cc;
                    request.Referer = referer;
                    request.Method = "GET";                   
                    request.Timeout = this._timeout;

                    //request.Connection = "Keep-Alive";
                    if (cookie != null)
                    {
                        CookieContainer cc = new CookieContainer();
                        cc.Add(cookie);
                        request.CookieContainer = cc;


                    }

                 
                    if ((this._proxy != null) && (null != this._proxy.Credentials))
                    {
                        request.UseDefaultCredentials = true;
                        request.Proxy = this._proxy;
                    }
                  
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    this._cc.Add(response.Cookies);
                    StreamReader reader = new StreamReader(response.GetResponseStream(), ReponseEncoding);
                    if (response.StatusCode == HttpStatusCode.Redirect)
                    {
                        CurrentRedirectLocation = response.Headers.Get("location");
                        if (!string.IsNullOrEmpty(CurrentRedirectLocation))
                        {
                            return Get(CurrentRedirectLocation);
                        }
                    }

                    return reader.ReadToEnd();
                }
                catch (Exception exception)
                {
                    LoggerHelper.Debug("http-exception:" + exception);
                    //TraceLog.Error("HTTP GET Error: " + exception.Message, new object[0]);
                    //TraceLog.Error("Url: " + url, new object[0]);
                }
            }
            return string.Empty;
        }

        public string Post(string url, string content)
        {
            string str = this.Post(url, content, this._lastUrl);
            this._lastUrl = url;
            return str;
        }

        public string Post(string url, string content, string referer)
        {
            int num = this._tryTimes;
            while (num-- > 0)
            {
                try
                {
                    if (this._delayTime > 0)
                    {
                        Thread.Sleep((int)(this._delayTime * 0x3e8));
                    }
                    url = CheckUrl(url);
                    HttpWebRequest request = null;
                    //如果是发送HTTPS请求  
                    if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                    {                       
                        //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                        //request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                        //request.ProtocolVersion = HttpVersion.Version10;
                        //string cert = @"D:\webroot\zgg.m\key\apiclient_cert.p12";
                        string cert = System.Web.HttpContext.Current.Server.MapPath("/key/apiclient_cert.p12");//@"D:\八零创想\项目\智裹裹\temp\apiclient_cert.p12";
                        string password = "1424703802";
                        ServicePointManager.ServerCertificateValidationCallback = new
                        RemoteCertificateValidationCallback(CheckValidationResult);
                        X509Certificate cer = new X509Certificate(cert, password);
                        request = (HttpWebRequest)HttpWebRequest.Create(url);
                        request.ClientCertificates.Add(cer);


                    }
                    else
                    {
                        request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                    }

                    request.Headers.Add(HttpRequestHeader.KeepAlive, "true");
                    request.UserAgent = this.reqUserAgent;
                    request.CookieContainer = this._cc;
                    request.Referer = referer;
                    byte[] bytes = Encoding.UTF8.GetBytes(content);
                    request.Method = "POST";
                    request.Timeout = this._timeout;
                    request.ContentType = this.PostContentType;
                    request.ContentLength = bytes.Length;
                    //request.Connection = "Keep-Alive";
                    //request.Headers.Add("ContentLength ", bytes.Length.ToString());  

                    if ((this._proxy != null) && (null != this._proxy.Credentials))
                    {
                        request.UseDefaultCredentials = true;
                    }
                    if (null != this._proxy)
                    {
                        request.Proxy = this._proxy;
                    }
                   

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    // CookieCollection cc1 = new CookieCollection();
                    this._cc.Add(response.Cookies);

                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    if (response.StatusCode == HttpStatusCode.Redirect)
                    {
                        CurrentRedirectLocation = response.Headers.Get("location");
                    }
                    return reader.ReadToEnd();
                }
                catch (Exception exception)
                {
                    LoggerHelper.Debug("HTTP:"+exception.ToString());
                    throw exception;
                    //TraceLog.Error("HTTP POST Error: " + exception.Message, new object[0]);
                    //TraceLog.Error("Url: " + url, new object[0]);
                    //TraceLog.Error("Data: " + content, new object[0]);
                }
            }
            return string.Empty;
        }
        public string PostEx(string url, string content, string referer="")
        {
            int num = this._tryTimes;
            while (num-- > 0)
            {
                try
                {
                    if (this._delayTime > 0)
                    {
                        Thread.Sleep((int)(this._delayTime * 0x3e8));
                    }
                    url = CheckUrl(url);
                    HttpWebRequest request = null;
                    //如果是发送HTTPS请求  
                    if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                    {
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                        request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                        request.ProtocolVersion = HttpVersion.Version10;



                    }
                    else
                    {
                        request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                    }

                    request.UserAgent = this.reqUserAgent;
                    request.CookieContainer = this._cc;
                    //request.Referer = referer;
                    byte[] bytes = Encoding.UTF8.GetBytes(content);
                    request.Method = "POST";
                    request.Timeout = this._timeout;
                    request.ContentType = this.PostContentType;
                    request.ContentLength = bytes.Length;
                    //request.Connection = "Keep-Alive";
                    //request.Headers.Add("ContentLength ", bytes.Length.ToString());  

                    if ((this._proxy != null) && (null != this._proxy.Credentials))
                    {
                        request.UseDefaultCredentials = true;
                    }
                    if (null != this._proxy)
                    {
                        request.Proxy = this._proxy;
                    }

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    // CookieCollection cc1 = new CookieCollection();
                    this._cc.Add(response.Cookies);

                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    if (response.StatusCode == HttpStatusCode.Redirect)
                    {
                        CurrentRedirectLocation = response.Headers.Get("location");
                    }
                    return reader.ReadToEnd();
                }
                catch (Exception exception)
                {
                    throw exception;
                    //TraceLog.Error("HTTP POST Error: " + exception.Message, new object[0]);
                    //TraceLog.Error("Url: " + url, new object[0]);
                    //TraceLog.Error("Data: " + content, new object[0]);
                }
            }
            return string.Empty;
        }
        public System.Drawing.Bitmap GetImage(string url, string referer)
        {
            int num = this._tryTimes;
            while (num-- > 0)
            {
                try
                {
                    if (this._delayTime > 0)
                    {
                        Thread.Sleep((int)(this._delayTime * 0x3e8));
                    }
                    url = CheckUrl(url);
                    HttpWebRequest request = null;
                    //如果是发送HTTPS请求  
                    if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                    {
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                        request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                        request.ProtocolVersion = HttpVersion.Version10;
                    }
                    else
                    {
                        request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                    }

                    request.UserAgent = this.reqUserAgent;
                    request.CookieContainer = this._cc;
                    request.Referer = referer;
                    //byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(content);
                    request.Method = "GET";
                    request.Timeout = this._timeout;
                    //request.ContentType = this.PostContentType;
                    //request.ContentLength = bytes.Length;
                    //request.Connection = "Keep-Alive";
                    //request.Headers.Add("ContentLength ", bytes.Length.ToString());  

                    if ((this._proxy != null) && (null != this._proxy.Credentials))
                    {
                        request.UseDefaultCredentials = true;
                    }
                    if (null != this._proxy)
                    {
                        request.Proxy = this._proxy;
                    }


                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    // CookieCollection cc1 = new CookieCollection();
                    this._cc.Add(response.Cookies);

                    //StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    //if (response.StatusCode == HttpStatusCode.Redirect)
                    //{
                    //    CurrentRedirectLocation = response.Headers.Get("location");
                    //}
                    return new System.Drawing.Bitmap(response.GetResponseStream());
                }
                catch (Exception exception)
                {
                    //throw exception;
                    //TraceLog.Error("HTTP POST Error: " + exception.Message, new object[0]);
                    //TraceLog.Error("Url: " + url, new object[0]);
                    //TraceLog.Error("Data: " + content, new object[0]);
                }
            }
            return null;
        }

        public System.Drawing.Bitmap GetImageByPost(string url, string content)
        {
            int num = this._tryTimes;
            while (num-- > 0)
            {
                try
                {
                    if (this._delayTime > 0)
                    {
                        Thread.Sleep((int)(this._delayTime * 0x3e8));
                    }
                    url = CheckUrl(url);
                    HttpWebRequest request = null;
                    //如果是发送HTTPS请求  
                    if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                    {
                        //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                        //request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                        //request.ProtocolVersion = HttpVersion.Version10;
                        //string cert = @"D:\webroot\zgg.m\key\apiclient_cert.p12";
                        string cert = System.Web.HttpContext.Current.Server.MapPath("~/key/apiclient_cert.p12"); //@"D:\八零创想\项目\智裹裹\temp\apiclient_cert.p12";
                        string password = "1466759502";
                        LoggerHelper.Debug("=================" + cert.ToString());
                        ServicePointManager.ServerCertificateValidationCallback = new
                        RemoteCertificateValidationCallback(CheckValidationResult);
                        X509Certificate cer = new X509Certificate(@"D:\tool\key\apiclient_cert.p12", password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                        request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                        request.ClientCertificates.Add(cer);


                    }
                    else
                    {
                        request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                    }

                    request.UserAgent = this.reqUserAgent;
                    request.CookieContainer = this._cc;
                    //request.Referer = referer;
                    byte[] bytes = Encoding.UTF8.GetBytes(content);
                    request.Method = "POST";
                    request.Timeout = this._timeout;
                    request.ContentType = this.PostContentType;
                    request.ContentLength = bytes.Length;
                    //request.Connection = "Keep-Alive";
                    //request.Headers.Add("ContentLength ", bytes.Length.ToString());  

                    if ((this._proxy != null) && (null != this._proxy.Credentials))
                    {
                        request.UseDefaultCredentials = true;
                    }
                    if (null != this._proxy)
                    {
                        request.Proxy = this._proxy;
                    }

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    // CookieCollection cc1 = new CookieCollection();
                    this._cc.Add(response.Cookies);

                    return new System.Drawing.Bitmap(response.GetResponseStream());

                }
                catch (Exception exception)
                {
                    //throw exception;
                    //TraceLog.Error("HTTP POST Error: " + exception.Message, new object[0]);
                    //TraceLog.Error("Url: " + url, new object[0]);
                    //TraceLog.Error("Data: " + content, new object[0]);
                    LoggerHelper.Debug(exception.ToString());
                }
            }
            return null;
        }
        public byte[] GetByteByPost(string url, string content)
        {
            int num = this._tryTimes;
            while (num-- > 0)
            {
                try
                {
                    if (this._delayTime > 0)
                    {
                        Thread.Sleep((int)(this._delayTime * 0x3e8));
                    }
                    url = CheckUrl(url);
                    HttpWebRequest request = null;
                    //如果是发送HTTPS请求  
                    if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                    {
                        //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                        //request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                        //request.ProtocolVersion = HttpVersion.Version10;
                        //string cert = @"D:\webroot\zgg.m\key\apiclient_cert.p12";
                        string cert = System.Web.HttpContext.Current.Server.MapPath("~/key/apiclient_cert.p12"); //@"D:\八零创想\项目\智裹裹\temp\apiclient_cert.p12";
                        string password = "1466759502";
                        LoggerHelper.Debug("=================" + cert.ToString());
                        ServicePointManager.ServerCertificateValidationCallback = new
                        RemoteCertificateValidationCallback(CheckValidationResult);
                        X509Certificate cer = new X509Certificate(cert, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                        request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                        request.ClientCertificates.Add(cer);


                    }
                    else
                    {
                        request = WebRequest.Create(new Uri(url)) as HttpWebRequest;
                    }

                    request.UserAgent = this.reqUserAgent;
                    request.CookieContainer = this._cc;
                    //request.Referer = referer;
                    byte[] bytes = Encoding.UTF8.GetBytes(content);
                    request.Method = "POST";
                    request.Timeout = this._timeout;
                    request.ContentType = this.PostContentType;
                    request.ContentLength = bytes.Length;
                    //request.Connection = "Keep-Alive";
                    //request.Headers.Add("ContentLength ", bytes.Length.ToString());  

                    if ((this._proxy != null) && (null != this._proxy.Credentials))
                    {
                        request.UseDefaultCredentials = true;
                    }
                    if (null != this._proxy)
                    {
                        request.Proxy = this._proxy;
                    }

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    // CookieCollection cc1 = new CookieCollection();
                    this._cc.Add(response.Cookies);
                    

                    Stream responseStream = response.GetResponseStream();
                    //byte[] buffer = new byte[response.ContentLength];                    
                    //responseStream.Read(buffer, 0, buffer.Length);

                    int count = (int)response.ContentLength;
                    int offset = 0;
                    byte[] buffer = new byte[count];
                    while (count > 0)
                    {
                        int n = responseStream.Read(buffer, offset, count);
                        if (n == 0) break;
                        count -= n;
                        offset += n;
                       
                    }

                    return buffer;

                }
                catch (Exception exception)
                {
                    //throw exception;
                    //TraceLog.Error("HTTP POST Error: " + exception.Message, new object[0]);
                    //TraceLog.Error("Url: " + url, new object[0]);
                    //TraceLog.Error("Data: " + content, new object[0]);
                    LoggerHelper.Debug(exception.ToString());
                }
            }
            return null;
        }
        public void SetDelayConnect(int delayTime)
        {
            this._delayTime = delayTime;
        }

        public void SetProxy(string server, int port, string username, string password)
        {
            if ((server != null) && (port > 0))
            {
                this._proxy = new WebProxy(server, port);
                if ((username != null) && (null != password))
                {
                    this._proxy.Credentials = new NetworkCredential(username, password);
                    this._proxy.BypassProxyOnLocal = true;
                }
            }
        }

        public void SetTimeOut(int timeout)
        {
            if (timeout > 0)
            {
                this._timeout = timeout;
            }
        }

        public void SetTryTimes(int times)
        {
            if (times > 0)
            {
                this._tryTimes = times;
            }
        }
        //  private CookieCollection _cc;

        public CookieContainer CookieContainer
        {
            get { return _cc; }
            set { _cc = value; }
        }

        private CookieCollection rcc;

        public CookieCollection ResponseCookieCollection
        {
            get { return rcc; }
            set { rcc = value; }
        }
        public string PostContentType { get; set; }

        // private Stream curStream;

        //public Stream CurStream
        //{
        //    get { return curStream; }
        //    set { curStream = value; }
        //}
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

    }

}