using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using Microsoft.AspNet.Identity.Owin;
using ThreeColor.Data.Models;
using TreeColor.Models;
using TreeColor.Utils;

namespace TreeColor.Controllers
{
    public class HomeController : BaseController
    {
        private ApplicationUserManager _userManager;
        private Random _random = new Random();

        private ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        [AllowAnonymous]
        public ActionResult ForDevelopers()
        {

            ViewBag.Server = ConfigManager.ServerName;
            ViewBag.Server =
            ViewBag.Server =
            return View();
        }

        [Authorize]
        public async Task<ActionResult> Index(bool showResults = false)
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
                {
                    ViewBag.Tests = tests.Data;

                    var identityUser = UserManager.FindByNameAsync(User.Identity.Name).Result;
                    var user = await HttpUtil.GetAsync<Users>("api/Account/User/" + identityUser.Id);
                    if (user.IsSuccess)
                        ViewBag.CurrentUser = user.Data;
                    else
                    {
                        FillReturnDataModel(user);
                        return View();
                    }
                }
                else
                {
                    FillReturnDataModel(tests);
                    return View();
                }

                if(showResults)
                {
                    var results = await HttpUtil.GetAsync<List<Results>>("api/Result/Last");
                    ViewBag.CurrentTest = (Tests)Session["CurrentTest"];
                    if (!results.IsSuccess)
                    {
                        FillReturnDataModel(results);
                        return View();
                    }
                    else
                    {
                        if (results.Data.Count > 0)
                            ViewBag.Result = results.Data.Average(r => r.Time);
                        ViewBag.Average = (await HttpUtil.GetAsync<double>("api/Result/AverageByLastTest")).Data;
                        ViewBag.ErrorAmount = results.Data.Count(r => r.ErrorCode != 0);
                    }
                }
                else
                {
                    ViewBag.CurrentTest = (Tests)Session["CurrentTest"];

                    if(((Tests)Session["CurrentTest"]) != null)
                    {
                        var pointsForTest = await HttpUtil.GetAsync<List<Points>>("api/Point/" + ((Tests)Session["CurrentTest"]).Id + "/false");
                        if (pointsForTest.IsSuccess)
                            ViewBag.PointsForCurrentTest = pointsForTest.Data;
                        else
                        {
                            ViewBag.ErrorMessage = pointsForTest.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View();
        }

        [Authorize]
        public async Task<ActionResult> ChangeTest(int id = 0)
        {
            if (id == 0)
                Session["CurrentTest"] = null;
            else
            {
                var test = await  HttpUtil.GetAsync<Tests>("api/Test/" + id);
                if(!test.IsSuccess)
                {
                    ViewBag.Error = test.Message;
                    return View();
                }
                Session["CurrentTest"] = test.Data;
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
        public async Task<JsonResult> PutResult(Results res)
        {
            try
            {
                res.Date = DateTime.Now;
                var putResult = await HttpUtil.PostAsync(res, "api/Result/Add");
                if (putResult.IsSuccess)
                    return Json(true);
                else
                    return Json(putResult.Exception);
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

        public ActionResult CreateChart(int width, int height)
        {
            List<Results> allResults = HttpUtil.GetAsync<List<Results>>("api/Result/All").Result.Data;
            var allTests = HttpUtil.GetAsync<List<Tests>>("api/Test/All").Result.Data;
            var allPoints = HttpUtil.GetAsync<List<Points>>("api/Point/All").Result.Data;
            if (allTests != null && allPoints != null)
            {
                Chart chart = new Chart();
                chart.Width = width;
                chart.Height = height;
                chart.BackColor = Color.White;
                chart.BorderlineDashStyle = ChartDashStyle.Solid;
                chart.BackSecondaryColor = Color.White;
                chart.BackGradientStyle = GradientStyle.TopBottom;
                chart.BorderlineWidth = 1;
                chart.Palette = ChartColorPalette.SeaGreen;
                chart.BorderlineColor = Color.FromArgb(26, 59, 105);
                chart.RenderType = RenderType.BinaryStreaming;
                chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
                chart.AntiAliasing = AntiAliasingStyles.All;
                chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
                chart.Titles.Add(CreateTitle());
                chart.Legends.Add(CreateLegend());

                List<double> allRes = allResults
                    .GroupBy(r => r.TestingNumber)
                    .Select(r => r.Average(t => t.Time))
                    .ToList();

                var beg = allRes.Min();
                var end = allRes.Max();

                var h = (end - beg) / (allRes.Count > 30 ? 30 : allRes.Count);

                double lastResult;

                chart.Series.Add(CreateSeriesForLastTest(allTests, allPoints, allResults, h, out lastResult));
                CreateSeriesForAllTests(allResults, h).ForEach(s =>
                    chart.Series.Add(s));
                chart.ChartAreas.Add(CreateChartArea());
                chart.ChartAreas[0].AxisX.Interval = h;
                chart.ChartAreas[0].AxisX.LabelStyle.Format = "#.#";

                VerticalLineAnnotation myLine = new VerticalLineAnnotation();

                myLine.AxisX = chart.ChartAreas[0].AxisX;
                myLine.IsInfinitive = true;
                myLine.ClipToChartArea = chart.ChartAreas[0].Name;
                myLine.Name = "Your Result";
                myLine.LineColor = Color.Red;
                myLine.LineWidth = 1;
                myLine.X = lastResult;
                chart.Annotations.Add(myLine);

                MemoryStream ms = new MemoryStream();
                chart.SaveImage(ms);
                return File(ms.GetBuffer(), @"image/png");
            }
            return null;
        }

        [NonAction]
        public Legend CreateLegend()
        {
            var legend = new Legend();
            legend.Name = "Result Chart";
            legend.Docking = Docking.Top;
            legend.Alignment = StringAlignment.Center;
            legend.BackColor = Color.Transparent;
            legend.Font = new Font(new FontFamily("Trebuchet MS"), 9);
            legend.LegendStyle = LegendStyle.Row;

            return legend;
        }

        [NonAction]
        public Title CreateTitle()
        {
            Title title = new Title();
            title.Text = "Среднее время реакции";
            title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            title.Font = new Font("Trebuchet MS", 14F, FontStyle.Bold);
            title.ShadowOffset = 3;
            title.ForeColor = Color.FromArgb(26, 59, 105);
            return title;
        }

        [NonAction]
        public Series CreateSeriesForLastTest(List<Tests> allTest, List<Points> allPoints, List<Results> results, double interval, out double lastResult)
        {
            var maxNumber = results.Max(t => t.TestingNumber);
            var pointId = results.Where(r => r.TestingNumber == maxNumber).First().PointId;
            var testId = allPoints.Where(p => p.Id == pointId).First().TestId;
            var pointsByTest = allPoints.Where(p => p.TestId == testId).ToList();
            lastResult = results
                .Where(r => r.TestingNumber == maxNumber)
                .Average(r => r.Time);

            Series seriesDetail = new Series();
            seriesDetail.Name = "Для вашего теста";
            seriesDetail.IsValueShownAsLabel = false;
            seriesDetail.Color = Color.FromArgb(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255));
            seriesDetail.ChartType = SeriesChartType.Line;
            seriesDetail.BorderWidth = 1;
            DataPoint point;

            List<double> allRes = results
                .GroupBy(r => r.TestingNumber)
                .Select(r => r.Average(t => t.Time))
                .ToList();

            List<double> resByTest = results
                .Where(r => pointsByTest.Any(p => p.Id == r.PointId))
                .GroupBy(r => r.TestingNumber)
                .Select(r => r.Average(t => t.Time))
                .ToList();

            var beg = allRes.Min();
            var end = allRes.Max();

            for (double i = beg; i < end; i += interval)
            {
                point = new DataPoint();
                point.SetValueXY(Math.Round(i, 2), resByTest.Count(r => r > i && r < i + interval));
                seriesDetail.Points.Add(point);
            }

            seriesDetail.ChartArea = "Result Chart";
            return seriesDetail;
        }

        [NonAction]
        public List<Series> CreateSeriesForAllTests(List<Results> results, double interval)
        {
            Series firstErrorColumns = new Series();
            firstErrorColumns.Name = "количество ошибок первого типа(неверная клавиша)";
            firstErrorColumns.IsValueShownAsLabel = false;
            firstErrorColumns.Color = Color.FromArgb((int)(255*0.5), _random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255));
            firstErrorColumns.ChartType = SeriesChartType.Column;

            Series secondErrorColumns = new Series();
            secondErrorColumns.Name = "количество ошибок первого второго типа(преждевременная реакция)";
            secondErrorColumns.IsValueShownAsLabel = false;
            secondErrorColumns.Color = Color.FromArgb((int)(255 * 0.5), _random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255));
            secondErrorColumns.ChartType = SeriesChartType.Column;

            Series seriesDetail = new Series();
            seriesDetail.Name = "Для всех тестов";
            seriesDetail.IsValueShownAsLabel = false;
            seriesDetail.Color = Color.FromArgb(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255));
            seriesDetail.ChartType = SeriesChartType.Line;
            seriesDetail.BorderWidth = 2;
            DataPoint point;

            List<ResultViewModel> array = results
                .GroupBy(r => r.TestingNumber)
                .Select(r => new ResultViewModel()
                {
                    Time = r.Average(t => t.Time),
                    FirstErrorCount = r.Count(t => t.ErrorCode == ErrorCodes.WrongKey),
                    SecondErrorCount = r.Count(t => t.ErrorCode == ErrorCodes.PrematureReaction)
                })
                .ToList();

            var beg = array.Min(r => r.Time);
            var end = array.Max(r => r.Time);

            for(double i = beg; i < end; i+= interval)
            {
                List<ResultViewModel> currentSequence = array.Where(r => r.Time > i && r.Time < i + interval).ToList();
                point = new DataPoint();
                point.SetValueXY(Math.Round(i, 2) , currentSequence.Count());
                seriesDetail.Points.Add(point);

                point = new DataPoint();
                point.SetValueXY(Math.Round(i, 2), currentSequence.Sum(r => r.FirstErrorCount));
                firstErrorColumns.Points.Add(point);

                point = new DataPoint();
                point.SetValueXY(Math.Round(i, 2), currentSequence.Sum(r => r.SecondErrorCount));
                secondErrorColumns.Points.Add(point);
            }

            seriesDetail.ChartArea = "Result Chart";
            return new List<Series>()
            {
                seriesDetail,
                firstErrorColumns,
                secondErrorColumns
            };
        }

        [NonAction]
        public ChartArea CreateChartArea()
        {
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "Result Chart";
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.Interval = 1;

            return chartArea;
        }
    }
}