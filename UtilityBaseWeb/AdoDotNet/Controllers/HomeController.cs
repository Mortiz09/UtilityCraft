using AdoDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdoDotNet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            int msgId = 1;
            string msg = "Hey ~";

            AdoManager.Insert(msgId, msg);

            return View();
        }

    }
}
