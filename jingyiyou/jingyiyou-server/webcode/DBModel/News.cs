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
    
    public partial class News
    {
        public int NewsID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public Nullable<int> UserID { get; set; }
        public string ImgURL { get; set; }
        public string Author { get; set; }
        public Nullable<int> IsPublish { get; set; }
    }
}
