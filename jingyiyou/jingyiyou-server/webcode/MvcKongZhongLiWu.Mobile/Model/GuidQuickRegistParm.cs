using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ncc2019.Model
{
    public class GuidQuickRegistParm
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        public string PhoneNumber { get; set; }

        /// <summary>
        /// 用户头像URL
        /// </summary>
        public string UserFaceUrl { get; set; }
    }
}