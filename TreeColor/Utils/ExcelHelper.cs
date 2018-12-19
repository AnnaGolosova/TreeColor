using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ThreeColor.Data.Models;

namespace TreeColor.Utils
{
    public class ExcelHelper
    {
        public byte[] GenerateReport(List<Results> data)
        {
            var package = new ExcelPackage();
            int tableStartRow = 5;

            var worksheet = package.Workbook.Worksheets.Add("ThreeColor");

            worksheet.Cells[1, 1].Value = "Примечания:";
            worksheet.Cells[2, 1].Value = "* - каждое тестирование имеет свой уникальный код. Используется для группировки результатов в одно тестирование";
            worksheet.Cells[3, 1].Value = "** - 0: ошибок не произошло; 1: неверная клавиша; 2: преждевеменная реакция";
            worksheet.Cells[4, 1].Value = "*** - результаты удаленных тестов или точек, которые были отредактированы";

            var titleCell = worksheet.Cells[tableStartRow - 1, 5];
            titleCell.Style.Font.Bold = true;
            titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //GY blue
            var mainColor = System.Drawing.ColorTranslator.FromHtml("#009688");
            var darkenColor = System.Drawing.ColorTranslator.FromHtml("#00695c");
            var header = worksheet.Cells[tableStartRow, 1, tableStartRow, 14];
            header.Style.Fill.PatternType = ExcelFillStyle.Solid;
            header.Style.Font.Size = 12;
            header.Style.Font.Bold = true;
            header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            header.Style.Fill.BackgroundColor.SetColor(darkenColor);
            header.Style.Font.Color.SetColor(System.Drawing.Color.White);
            var subHeader = worksheet.Cells[tableStartRow + 1, 1, tableStartRow + 1, 14];
            subHeader.Style.Fill.PatternType = ExcelFillStyle.Solid;
            subHeader.Style.Font.Size = 12;
            subHeader.Style.Font.Bold = true;
            subHeader.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            subHeader.Style.Fill.BackgroundColor.SetColor(mainColor);
            subHeader.Style.Font.Color.SetColor(System.Drawing.Color.White);
            for (int r = tableStartRow; r < tableStartRow + 2; r++)
            {
                for (int c = 1; c <= 14; c++)
                {
                    worksheet.Cells[r, c].Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.White);
                    worksheet.Cells[r, c].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }
            }

            worksheet.Cells[tableStartRow, 1].Value = "О результатах тестирования";
            worksheet.Cells[tableStartRow, 1, tableStartRow, 4].Merge = true;
            worksheet.Cells[tableStartRow, 5].Value = "О тесте";
            worksheet.Cells[tableStartRow, 5, tableStartRow, 11].Merge = true;
            worksheet.Cells[tableStartRow, 12].Value = "О точке";
            worksheet.Cells[tableStartRow, 12, tableStartRow, 14].Merge = true;
            worksheet.Cells[tableStartRow + 1, 1].Value = "Номер тестирования *";
            worksheet.Cells[tableStartRow + 1, 2].Value = "Дата проведения теста";
            worksheet.Cells[tableStartRow + 1, 3].Value = "Время реакции";
            worksheet.Cells[tableStartRow + 1, 4].Value = "Код ошибки **";
            worksheet.Cells[tableStartRow + 1, 5].Value = "Название теста";
            worksheet.Cells[tableStartRow + 1, 6].Value = "Цвет фона для теста";
            worksheet.Cells[tableStartRow + 1, 7].Value = "Начальный размер точки для теста";
            worksheet.Cells[tableStartRow + 1, 8].Value = "Скорость роста точки для теста";
            worksheet.Cells[tableStartRow + 1, 9].Value = "Минимальное время между появлением точек";
            worksheet.Cells[tableStartRow + 1, 10].Value = "Максимальное время между появлением точек";
            worksheet.Cells[tableStartRow + 1, 11].Value = "Удален ли тест ***";
            worksheet.Cells[tableStartRow + 1, 12].Value = "Клавиша для точки";
            worksheet.Cells[tableStartRow + 1, 13].Value = "Цвет точки";
            worksheet.Cells[tableStartRow + 1, 14].Value = "Удалена ли точка ***";

            int rowIndex = tableStartRow + 2;
            if (data != null)
            {
                foreach (var line in data as List<Results>)
                {
                    worksheet.Cells[rowIndex, 1].Value = line.TestingNumber;
                    worksheet.Cells[rowIndex, 2].Value = line.Date;
                    worksheet.Cells[rowIndex, 3].Value = line.Time;
                    worksheet.Cells[rowIndex, 4].Value = (int)line.ErrorCode;
                    worksheet.Cells[rowIndex, 5].Value = line.Point?.Test?.Name;
                    worksheet.Cells[rowIndex, 6].Value = line.Point?.Test?.FieldColor;
                    worksheet.Cells[rowIndex, 7].Value = line.Point?.Test?.PointSize;
                    worksheet.Cells[rowIndex, 8].Value = line.Point?.Test?.Speed;
                    worksheet.Cells[rowIndex, 9].Value = line.Point?.Test?.MinInterval;
                    worksheet.Cells[rowIndex, 10].Value = line.Point?.Test?.MaxInterval;
                    worksheet.Cells[rowIndex, 11].Value = (line.Point?.Test?.IsDeleted == 1 ? "Да" : "Нет");
                    worksheet.Cells[rowIndex, 12].Value = line.Point?.Key;
                    worksheet.Cells[rowIndex, 13].Value = line.Point?.Color;
                    worksheet.Cells[rowIndex, 14].Value = (line.Point?.IsDeleted == 1 ? "Да" : "Нет");

                    rowIndex++;
                }
            }
            worksheet.Row(tableStartRow).Height = 30;
            worksheet.Row(tableStartRow + 1).Height = 20;
            worksheet.Column(2).AutoFit();
            worksheet.Column(3).AutoFit();
            worksheet.Column(4).AutoFit();
            worksheet.Column(5).AutoFit();
            worksheet.Column(6).AutoFit();
            worksheet.Column(7).AutoFit();
            worksheet.Column(8).AutoFit();
            worksheet.Column(9).AutoFit();
            worksheet.Column(10).AutoFit();
            worksheet.Column(11).AutoFit();
            worksheet.Column(12).AutoFit();
            worksheet.Column(13).AutoFit();
            worksheet.Column(14).AutoFit();

            // Finally when you're done, export it to byte array. [it closes package stream]
            var fileContents = package.GetAsByteArray();

            return fileContents;
        }
    }
}