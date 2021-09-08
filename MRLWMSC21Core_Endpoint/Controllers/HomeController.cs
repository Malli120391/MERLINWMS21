using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MRLWMSC21Core.Library;

namespace MRLWMSC21_Endpoint.Controllers
{
    public class HomeController : Controller
    {
        private string _ClassCode = string.Empty;

        public HomeController()
        {
            _ClassCode = ExceptionHandling.GetClassExceptionCode(ExceptionHandling.ExcpConstants_API_Enpoint.HomeController);
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
