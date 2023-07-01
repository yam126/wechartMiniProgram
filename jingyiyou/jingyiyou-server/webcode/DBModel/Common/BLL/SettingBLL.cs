using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ncc2019.Common.BLL
{
    public class SettingBLL
    {
        static Model.Setting settingModel = new Model.Setting();
        /// <summary>
        /// 获取快递费
        /// </summary>
        /// <returns></returns>
        public static decimal GetDeliverFee()
        {
            return settingModel.DeliverFee;
        }

        public static string AppID { get { return System.Configuration.ConfigurationManager.AppSettings["AppId"]; } }
        public static string AppSecret { get { return System.Configuration.ConfigurationManager.AppSettings["AppSecret"]; } }
        public static string TenPayV3_MchId { get { return System.Configuration.ConfigurationManager.AppSettings["TenPayV3_MchId"]; } }
        public static string TenPayV3_Key { get { return System.Configuration.ConfigurationManager.AppSettings["TenPayV3_Key"]; } }

        public static string AppID_XCX { get { return System.Configuration.ConfigurationManager.AppSettings["AppID_XCX"]; } }
        public static string AppSecret_XCX { get { return System.Configuration.ConfigurationManager.AppSettings["AppSecret_XCX"]; } }
        /// <summary>
        /// 是否进行送券活动
        /// </summary>
        public static bool IsQunGiven { get { return System.Configuration.ConfigurationManager.AppSettings["IsQunGiven"] == "1" ? true : false; } }
        /// <summary>
        /// 进行送券活动，券的价格
        /// </summary>
        public static decimal QuanPrice { get { return decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["QuanPrice"]); } }
        /// <summary>
        /// 优惠券介绍
        /// </summary>
        public static string QuanIntro { get { return System.Configuration.ConfigurationManager.AppSettings["QuanIntro"]; } }

        /// <summary>
        /// 实际的web域名
        /// </summary>
        public static string WebDomain
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["webdomain"];
            }
        }
        /// <summary>
        /// 实际的手机域名
        /// </summary>
        public static string MobileDomain
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["mobiledomain"];
            }
        }
        /// <summary>
        /// 系统人员ID
        /// </summary>
        public static int SysMemberID
        {
            get
            {
                return 1437;
            }
        }
        public static int SysSellerID
        {
            get
            {
                return 1;
            }
        }

    }
}
