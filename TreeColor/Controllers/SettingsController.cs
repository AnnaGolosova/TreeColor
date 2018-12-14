using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ThreeColor.Data.Models;
using TreeColor.Models;
using TreeColor.Utils;

namespace TreeColor.Controllers
 {
    [Authorize(Roles = ("Admin"))]
    public class SettingsController : BaseController
    {
        public ActionResult TestList(string errorMessage = null, string successMessage = null)
        {
            ViewBag.ErrorMessage = errorMessage;
            ViewBag.SuccessMessage = successMessage;
            var tests = HttpUtil.GetAsync<List<Tests>>("api/Test/All").Result;
            if(!tests.IsSuccess)
            {
                ViewBag.ErrorMessage = tests.Message;
                return View();
            }
            return View(tests.Data);
        }

        [HttpGet]
        public ActionResult SettingsTest(int id = 0, bool newTest = false, string errorMessage = null)
        {
            ViewBag.ErrorMessage = errorMessage;
            if (newTest)
            {
                ViewBag.NewTest = newTest;
                ViewBag.Points = new List<Points>()
                {
                    new Points()
                    {
                        Color = "000000",
                        Key = "W"
                    }
                };
                return View(new Tests()
                {
                    FieldColor = "FFFFFF",
                    IsDeleted = 0,
                    MaxInterval = 0,
                    MinInterval = 0,
                    Name = "",
                    PointSize = 0,
                    Speed = 1
                });
            }
            var test = HttpUtil.GetAsync<Tests>("api/Test/" + id).Result;
            if (!test.IsSuccess)
            {
                ViewBag.ErrorMessage = test.Message;
                return View();
            }
            else if (test.Data != null)
            {
                ViewBag.fieldColor = test.Data.FieldColor;

                var points = HttpUtil.GetAsync<List<Points>>("api/Point/" + id + "/false").Result;
                if (!points.IsSuccess)
                {
                    ViewBag.ErrorMessage = points.Message;
                    return View();
                }

                ViewBag.NewTest = newTest;
                ViewBag.Points = points.Data;

                return View(test.Data);
            }
            return RedirectToAction("TestList");
        }

        [HttpPost]
        public ActionResult SettingsTest(Tests model, List<Points> Points, bool newTest = false)
        {
            if (newTest)
            {
                var addTestResult = HttpUtil.PostAsync<int>(model, "api/Test/Add").Result;
                if(!addTestResult.IsSuccess)
                {
                    ViewBag.ErrorMessage = addTestResult.Message;
                    return View();
                }
                Points.ForEach(point =>  point.TestId = addTestResult.Data);
                HttpUtil.PostAsync(Points, "api/Point/Add");

                return RedirectToAction("TestList", new { message = "Изменения успешно сохранены!" });
            }

            var updateTestResult = HttpUtil.PutAsync(model, "api/Test/Update").Result;
            HttpUtil.PutAsync(Points, "api/Point/Update");

            if (!updateTestResult.IsSuccess)
            {
                ViewBag.ErrorMessage = updateTestResult.Message;
                return View();
            }
            return RedirectToAction("TestList", new { successMessage = "Изменения успешно сохранены!" });
        }

        [HttpGet]
        public ActionResult CreateTest()
        {
            return RedirectToAction("SettingsTest", new { id = 0, newTest = true });
        }

        [HttpPost]
        public ActionResult CreateTest(Tests model)
        {
            return RedirectToAction("SettingsTest", new { model = model, newTest = true });
        }

        public async Task<ActionResult> DeleteTest(int testId)
        {
            var result = await HttpUtil.PutAsync(testId,"api/Test/delete/" + testId);
            if(!result.IsSuccess)
            {
                return RedirectToAction("SettingsTest", new { id = testId, errorMessage = "Тест не может быть удален по причине: " + result.Message });
            }
            return RedirectToAction("TestList", new { successMessage = "Тест был успешно удален!" });
        }
    }
}