using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ThreeColor.Data.Models;
using TreeColor.Utils;

namespace TreeColor.Controllers
{
    [Authorize(Roles = ("Admin"))]
    public class ReportController : BaseController
    {
        private readonly ExcelHelper _excelHelper;

        public ReportController()
        {
            _excelHelper = new ExcelHelper();
        }

        public async Task<ActionResult> LoadExcel()
        {
            var results = await HttpUtil.GetAsync<List<Results>>("api/Result/All");
            if(!results.IsSuccess)
            {
                return RedirectToAction("TestList", "Settings", new { errorMessage = "Ошибка во время загрузки отчета: " + results.Message });
            }

            byte[] fileContents;

            try
            {
                fileContents = _excelHelper.GenerateReport(results.Data);
            }
            catch(Exception ex)
            {
                return RedirectToAction("TestList", "Settings", new { errorMessage = "Ошибка во время загрузки отчета: " + ex.Message });
            }

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ThreeColor_Data_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
        }
    }
}
