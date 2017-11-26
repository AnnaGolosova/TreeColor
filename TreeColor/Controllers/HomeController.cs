using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TreeColor.Models;

namespace TreeColor.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            try
            {
                ViewBag.CurrentTest = (Tests)Session["CurrentTest"];
                ViewBag.Tests = DBcontext.Tests.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View();
        }

        public ActionResult ChangeTest(int id = 0)
        {
            if (id == 0)
                Session["CurrentTest"] = null;
            else
            {
                Tests test = DBcontext.Tests.AsNoTracking().Where(t => t.id == id).FirstOrDefault();
                Session["CurrentTest"] = test;
            }
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult PutResult(Results res)
        {
            try
            {
                DBcontext.Results.Add(res);
                DBcontext.Entry(res).State = System.Data.Entity.EntityState.Added;
                DBcontext.SaveChanges();
                return Json(true);
            }
            catch(Exception e)
            {
                return Json(false);
            }
        }
    }
}