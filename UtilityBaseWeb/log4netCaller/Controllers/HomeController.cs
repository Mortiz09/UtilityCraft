using log4netPlay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace log4netPlay.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string sysInfo = string.Format("log4netPlay.Web.{0}.{1}", System.Reflection.MethodInfo.GetCurrentMethod().ReflectedType.Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            
            Dictionary<string, string> extProperties = new Dictionary<string, string>();
            extProperties["Name"] = "王大明";
            extProperties["ID"] = "Cus001001001";

            //LogHandler.GetInstance().WriteFileLog(Guid.NewGuid().ToString(), "127.0.0.1", sysInfo, "hey", "yo ~", "Info", extProperties);
            LogHandler.GetInstance().WriteLog(Guid.NewGuid(), "127.0.0.1", sysInfo, "hey", "yo ~", "Info", extProperties);

            return View();
        }

    }
}
