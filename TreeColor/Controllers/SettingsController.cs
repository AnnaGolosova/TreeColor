using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TreeColor.Controllers
{
    public class SettingsController : BaseController
    {
        public ActionResult TestList()
        {
            return View(DBcontext.Tests.ToList());
        }

        public ActionResult SettingsTest(int id = 0)
        {
            return View();
        }
    }
}