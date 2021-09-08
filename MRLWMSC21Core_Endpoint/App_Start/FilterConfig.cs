using System.Web;
using System.Web.Mvc;

namespace MRLWMSC21_Endpoint
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
