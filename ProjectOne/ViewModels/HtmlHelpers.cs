using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ProjectOne.ViewModels
{
    public static class HtmlHelpers
    {
        public static HtmlString ContactLink(this HtmlHelper html, string label="mysalah@outlook.com")
        {
            return new HtmlString(string.Format("<a href=mailto:mysalah@outlook.com>{0}</a>",label));
        }
    }
}