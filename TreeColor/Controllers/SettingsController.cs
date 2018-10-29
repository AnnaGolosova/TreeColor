using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TreeColor.Models;

namespace TreeColor.Controllers
 {
    [Authorize(Roles =("Admin"))]
    public class SettingsController : BaseController
    {
        public ActionResult TestList(string message = null)
        {
            ViewBag.Message = message;
            return View(DBcontext.Tests.ToList());
        }

        [HttpGet]
        public ActionResult SettingsTest(int id = 0, bool newTest = false)
        {
            Tests test = new Tests();
            if (DBcontext.Tests.Any(t => t.id == id))
                test = DBcontext.Tests.Where(t => t.id == id).First();

            string fieldColor = test.field_color;
            ViewBag.fieldColor = fieldColor;

            List<string> pointColors = new List<string>();
            var points = test.Points.ToList();
            for (int i = 0; i < points.Count; i++)
            {
                pointColors.Add(points[i].color);
            }
            ViewBag.NewTest = newTest;
            ViewBag.pointColors = pointColors;

            return View(test);
        }

        [HttpPost]
        public ActionResult SettingsTest(Tests model, bool newTest = false)
        {
            if(newTest)
            {
                DBcontext.Tests.Add(model);
                DBcontext.SaveChanges();

                return RedirectToAction("TestList", new { message = "Изменения успешно сохранены!" });
            }
            Tests oldTest = DBcontext.Tests.Where(t => t.id == model.id).FirstOrDefault();
            model.id = oldTest.id;
            oldTest = model;

            DBcontext.SaveChanges();
            return RedirectToAction("TestList", new { message = "Изменения успешно сохранены!" });
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
    }
}