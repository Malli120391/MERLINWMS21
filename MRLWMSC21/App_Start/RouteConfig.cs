using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace MRLWMSC21
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
           routes.EnableFriendlyUrls();
        }
    }
}
