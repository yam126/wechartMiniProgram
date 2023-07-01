using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ncc2019.Common.Enum
{
    public enum MemmberStatus
    {
        正常 = 1,
        禁用 = 2
    }
    public enum SellerStatus
    {
        正常 = 1,
        禁用 = 2,
        待审核 = 3
    }
    public enum UserLevel
    {
        管理员 = 1,
        普通账户 = 2,
        校园代理 = 3
    }
    public enum ShiFouStatus
    {
        是 = 1,
        否 = 0
    }
    public enum PayStatus
    {
        已支付 = 1,
        未支付 = 2,
        已退款 = 3
    }
    /// <summary>
    /// 支付类型
    /// </summary>
    public enum PaymentType
    {
        双方支付 = 1,
        取件人支付 = 2,
        快递员支付 = 3
    }

    public enum PayType
    {
        自己账户 = 0,
        Web支付宝 = 1,
        微信 = 2,
        手机支付宝 = 3,
        系统账户 = 9

    }

    public enum MemberType
    {
        邻居 = 1,
        快递员 = 2

    }
    public enum PayDirection
    {
        购买 = 1,
        充值 = 2,
        副账户充值 = 3,
        副账户购买 = 4,
        退款 = 5,
        彩票销售结算 = 6,
        销售网点推荐提成 = 7,
        中奖后打款 = 8,
        中奖后收款 = 9,
        销售彩票款 = 10,
        销售彩票提成 = 11,
        取现到银行卡 = 99

    }

    public enum GivenStatus
    {
        未送出 = 1,
        已送出 = 2,
        已经打开 = 3,
        //开始邮寄 = 4,
        //接到礼物 = 5

    }
    public enum OrderStatus
    {
        正常 = 1,
        退款 = 2,
        未开启众筹 = 3,
        众筹中 = 4,
        众筹成功 = 5,
        众筹失败 = 6
    }
    public enum TransferStatus
    {
        未发货 = 1,
        已发货 = 2,
        已签收 = 3
    }

    public enum AdminPageType
    {
        Member = 1,
        GoodList = 2,
        Address = 3,
        InfoList = 4,
        OrderList = 5,
        GoodSortList = 6,
        ScanList = 7,
        MachineList = 8
    }
    public enum MyPageType
    {
        ChangePass = 1,
        MyInfo = 2,
        Payment = 3

    }

    public enum GoodStatus
    {
        上架 = 1,
        下架 = 0,
        挂起 = 2
    }
    /// <summary>
    /// 是否启用
    /// </summary>
    public enum Enabled
    {
        启用 = 1,
        禁用 = 0
    }
    /// <summary>
    /// 券类型
    /// </summary>
    public enum QuanType
    {
        一个微信号生成一个 = 1,
        任意数量代金券 = 2
    }


    //public enum QrAction
    //{
    //    获取礼物 = 1,
    //    关联订单 = 2
    //}
    /// <summary>
    /// 微信消息类型
    /// </summary>
    public enum WXMessageType
    {
        SCAN = 1,//扫码
        subscribe = 2,//关注
        unsubscribe = 3,//取消关注
        LOCATION = 4//位置
    }
    /// <summary>
    /// 操作流程
    /// </summary>
    public enum AtionType
    {
        打开礼物 = 10,
        填写地址 = 20,
        回复留言 = 30,
        发送礼物 = 40,
        收到礼物 = 50
    }
    /// <summary>
    /// 产品的类型
    /// </summary>
    public enum GoodType
    {
        普通实体 = 1,
        贺卡 = 2,
        自动降价 = 3
    }
    /// <summary>
    /// 
    /// </summary>
    public enum PrizeType
    {
        中奖啦 = 1,
        谢谢参与 = 2
    }
    /// <summary>
    /// 
    /// </summary>
    public enum OrderType
    {
        普通订单 = 1,
        众筹订单 = 2,
        书签类二维码 = 3,
        书签类二维码测试 = 4,
        自动降价 = 5
    }
    public enum SMSSendStatus
    {
        未发送 = 1,
        发送中 = 2
    }
    public enum PrizeStatus
    {
        未开奖 = 0,
        已中奖 = 1,
        未中奖 = 2
    }
    public enum LotteryStatus
    {
        异常 = 0,
        正常 = 1,
        已支付 = 2,
        已经退款 = 3
    }
    public class EnumTool
    {
        public static string GetEnumName(Type type, int? value)
        {
            if (value == null)
            {
                return "";
            }
            return System.Enum.GetName(type, value);
        }

        public static T GetType<T>(string name)
        {
            Array values = System.Enum.GetValues(typeof(T));
            foreach (var item in values)
            {
                if (item.ToString() == name)
                {
                    return (T)item;
                }

            }
            return (T)new object();
        }
        /// <summary>
        /// 将枚举转换为下拉框元素
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="selectValue">默认选中的值</param>
        /// <returns></returns>
        public static List<SelectListItem> ConvertList(Type type, string selectValue)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            Array values = System.Enum.GetValues(type);
            string[] names = System.Enum.GetNames(type);
            foreach (var item in values)
            {
                SelectListItem selitem = new SelectListItem() { Text = item.ToString(), Value = System.Enum.Format(type, System.Enum.Parse(type, item.ToString()), "d") };
                if (selectValue == item.ToString())
                {
                    selitem.Selected = true;
                }
                list.Add(selitem);
            }

            return list;

        }
        public static List<SelectListItem> ConvertList(Type type, string selectValue, bool insertALL)
        {
            List<SelectListItem> list = ConvertList(type, selectValue);
            if (insertALL)
            {
                list.Insert(0, new SelectListItem() { Text = "全部", Value = "0" });
            }
            return list;
        }
        public static List<SelectListItem> ConvertList(Type type, string selectValue, string text, string value)
        {
            List<SelectListItem> list = ConvertList(type, selectValue);

            list.Insert(0, new SelectListItem() { Text = text, Value = value });

            return list;
        }
    }
}