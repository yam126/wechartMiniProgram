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
    
    public partial class ZGGMachine
    {
        public int MachineID { get; set; }
        public string Name { get; set; }
        public string ShowCode { get; set; }
        public string BackCode { get; set; }
        public string WxOpenID { get; set; }
        public string QrUrl { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string OwnerName { get; set; }
        public Nullable<int> IsShared { get; set; }
        public Nullable<int> IsLocked { get; set; }
        public Nullable<int> OpenUserID { get; set; }
        public string PassWord { get; set; }
        public string GuiGe { get; set; }
        public Nullable<decimal> UserPayment { get; set; }
        public Nullable<decimal> CompanyPayment { get; set; }
        public Nullable<int> StrategyPayment { get; set; }
        public Nullable<int> ZGGUseControlID { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<double> Precision { get; set; }
    }
}
