//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ncc2019
{
    using System;
    using System.Collections.Generic;
    
    public partial class ZGGPay
    {
        public int ZGGPayID { get; set; }
        public Nullable<decimal> Payment { get; set; }
        public Nullable<int> MemberID { get; set; }
        public Nullable<int> MemberTypeID { get; set; }
        public Nullable<int> State { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<decimal> NeedPay { get; set; }
        public string HeadImgUrl { get; set; }
        public string Name { get; set; }
        public Nullable<int> UseControlID { get; set; }
    }
}
