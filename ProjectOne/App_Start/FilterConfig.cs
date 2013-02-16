using System.Web;
using System.Web.Mvc;

namespace ProjectOne
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           // Since we are using elmah.mvc this is not required
           // filters.Add(new HandleErrorAttribute());
        }
    }
}