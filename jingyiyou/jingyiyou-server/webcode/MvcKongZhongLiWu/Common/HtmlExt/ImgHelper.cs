﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcKongZhongLiWu.Common.HtmlExt
{
    public static class ImgHelper
    {
        /// <summary>
        /// 自定义一个@html.Image()
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="src">src属性</param>
        /// <param name="alt">alt属性</param>
        /// <returns></returns>
        public static MvcHtmlString Image(this HtmlHelper helper, string src, string alt)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", src);
            builder.MergeAttribute("alt", alt);
            builder.ToString(TagRenderMode.SelfClosing);
            return MvcHtmlString.Create(builder.ToString());
        }
    }
}