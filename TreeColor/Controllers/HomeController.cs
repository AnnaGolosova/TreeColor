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
        [Authorize]
        public ActionResult Index(bool showResults = false)
        {
            try
            {
                ViewBag.TestingNumber = DBcontext.Results.Max(r => r.testingnumber) + 1 ?? 1;
                ViewBag.Tests = DBcontext.Tests.AsNoTracking().ToList();

                if(showResults)
                {
                    if(ViewBag.Result = DBcontext.Results
                        .Any(r => r.testingnumber == DBcontext.Results.Max(res => res.testingnumber)))
                        ViewBag.Result = DBcontext.Results
                            .Where(r => r.testingnumber == DBcontext.Results.Max(res => res.testingnumber))
                            .Average(r => r.tim);
                    ViewBag.Average = DBcontext.Results.Where(r => r.Points.testid == DBcontext.Results.Where(res => res.testingnumber == DBcontext.Results.Max(rr => rr.testingnumber)).FirstOrDefault().Points.testid).Average(r => r.tim);
                    ViewBag.ErrorAmount = DBcontext.Results.Where(r => r.testingnumber == DBcontext.Results.Max(res => res.testingnumber)).Count(r => r.error > 0);
                }
                else
                {
                    ViewBag.CurrentTest = (Tests)Session["CurrentTest"];
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View();
        }

        [Authorize]
        public ActionResult ChangeTest(int id = 0)
        {
            if (id == 0)
                Session["CurrentTest"] = null;
            else
            {
                Tests test = DBcontext.Tests.Where(t => t.id == id).FirstOrDefault();
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

        [Authorize]
        public JsonResult PutResult(Results res)
        {
            try
            {
                int pointId = DBcontext.Tests.Where(t => t.id == res.userid).FirstOrDefault().Points.ToList()[(int)res.pointid].id;
                res.pointid = pointId;
                res.userid = null;
                res.CDate = DateTime.Now;
                if(User.Identity.IsAuthenticated)
                {
                    string userId = UserDB.Users.Where(nu => nu.UserName == User.Identity.Name).FirstOrDefault().Id;
                    res.userid = DBcontext.Users.Where(u => String.Compare(u.NewId, userId) == 0).FirstOrDefault().id;
                }

                DBcontext.Results.Add(res);
                DBcontext.Entry(res).State = System.Data.Entity.EntityState.Added;
                DBcontext.SaveChanges();

                return Json(true);
            }
            catch(Exception e)
            {
                return Json(e.Message);
            }
        }
    }
}