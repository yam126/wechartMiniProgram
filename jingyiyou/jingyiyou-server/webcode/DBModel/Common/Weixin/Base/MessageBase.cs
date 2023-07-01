using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Common.Weixin.Base
{
    public class MessageBase
    {
        protected ncc2019Entities db = new ncc2019Entities();
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        //public DateTime CreateTime { get; set; }
        public string MsgType { get; set; }
        public string Event { get; set; }
        public string EventKey { get; set; }

     

        public void MakeMessage(MessageBase message)
        {
            message.ToUserName = this.ToUserName;
            message.FromUserName = this.FromUserName;
            message.MsgType = this.MsgType;
            message.Event = this.Event;
            message.EventKey = this.EventKey;

        }
    }
}
