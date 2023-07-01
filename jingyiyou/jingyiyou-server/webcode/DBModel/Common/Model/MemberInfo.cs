using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ncc2019.Common.Model
{
    /// <summary>
    /// 会员登陆后的信息
    /// </summary>
    public class MemberInfo
    {
        public int MemnerID { get; set; }
        /// <summary>
        /// 微信的openid
        /// </summary>
        public string WeChatOpenid { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 用户等级
        /// </summary>
        public int UserLevel { get; set; }
        /// <summary>
        /// 头像信息
        /// </summary>
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 是否关注微信
        /// </summary>
        public bool IsGuanZhu { get; set; }

        /// <summary>
        /// 吹风机用户类型
        /// </summary>
        public int? CFJMemberTypeID { get; set; }
        /// <summary>
        /// 是否为吹风机用户
        /// </summary>
        public int? ISCFJUser { get; set; }

        public int CurMeachineID { get; set; }

        public DateTime ZGGLastOpenTime { get; set; }

        public Enum.MemberType MemberType { get; set; }

        public bool IsOwner { get; set; }
        /// <summary>
        /// 是否结束流程
        /// </summary>
        public bool EndingControl { get; set; }
        /// <summary>
        /// 个人账户
        /// </summary>
        public decimal Balance
        {
            get
            {
                return new ncc2019Entities().Members.Find(MemnerID).Balance.Value;

            }
        }

        public static string GetMemberName(int? memberid)
        {
            if (memberid == null)
            {
                return "";
            }
            return new ncc2019Entities().Members.Find(memberid).Name;
        }

        public static MemberInfo BuildMemberInfo(Members member)
        {
            MemberInfo info = new MemberInfo()
            {
                MemnerID = member.MemberID,
                Name = member.Name,
                WeChatOpenid = member.WechatOpenid,
                HeadImgUrl = member.HeadImgUrl,
                UserLevel = member.UserLevel.Value,
                IsGuanZhu = member.IsGuanZhu == (int)Common.Enum.ShiFouStatus.是 ? true : false,
                CFJMemberTypeID = member.CFJMemberTypeID,
                ISCFJUser = member.ISCFJUser
            };
            return info;
        }


        /// <summary>
        /// 实时获取体力值
        /// </summary>
        public int TiLiNum
        {
            get
            {
                return new ncc2019Entities().Members.Find(MemnerID).TiLiNum.Value;

            }
        }

    }
}