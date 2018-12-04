using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThreeColor.Data.Models;
using TreeColor.Utils;

namespace TreeColor.Controllers
{
    public class HomeController : BaseController
    {
        [Authorize]
        public async ActionResult Index(bool showResults = false)
        {
            try
            {
                var testingNumber = await HttpUtil.GetAsync<int>("api/Test/LastNumber");
                if(testingNumber.IsSuccess)
                    ViewBag.TestingNumber = testingNumber.Data;
                else
                {
                    FillReturnDataModel(testingNumber);
                    return View();
                }

                var tests = await HttpUtil.GetAsync<List<Tests>>("api/Test/All");
                if (tests.IsSuccess)
                    ViewBag.Tests = tests.Data;
                else
                {
                    FillReturnDataModel(testingNumber);
                    return View();
                }

                if(showResults)
                {
                    var results = await HttpUtil.GetAsync<List<Results>>("api/Result/Last");
                    if (!tests.IsSuccess)
                    {
                        FillReturnDataModel(testingNumber);
                        return View();
                    }
                    else
                    {

                        if (results.Data.Count > 0)
                            ViewBag.Result = results.Data.Average(r => r.Time);
                        //AverageTime/{testId}
                        ViewBag.Average = await HttpUtil.GetAsync<List<Results>>("api/Result/AverageTime/" + );
                        ViewBag.ErrorAmount = DBcontext.Results.Where(r => r.testingnumber == DBcontext.Results.Max(res => res.testingnumber)).Count(r => r.error > 0);
                    }
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

        private void FillReturnDataModel(ReturnDataModel data)
        {
            ViewBag.ErrorMessage = data.Message;
            ViewBag.Exception = data.Exception;
        }
    }
}