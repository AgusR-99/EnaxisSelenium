using EnaxisSelenium.SummaryHandlers.Contracts;
using OfficeOpenXml;

namespace EnaxisSelenium.SummaryHandlers
{
    public class ExcelTestSummaryExporter : ITestSummaryExporter
    {
        public void Export(List<(TimeSpan ElapsedTime, string Action, TimeSpan? TimeTaken)> logMessages, string outputPath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Summary");
                var startRow = 2;

                // Insert a row at the top for filter headers
                worksheet.InsertRow(1, 1);
                worksheet.Cells["A1"].Value = "Elapsed Time";
                worksheet.Cells["B1"].Value = "Action";
                worksheet.Cells["C1"].Value = "Time Taken";
                worksheet.Cells["A1:C1"].Style.Font.Bold = true;
                worksheet.Cells["A1:C1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["A1:C1"].AutoFilter = true;

                var timeTakenFormat = "0.00"; // Format for time taken

                foreach (var log in logMessages)
                {
                    var elapsedTimeFormatted = $"{log.ElapsedTime:hh\\:mm\\:ss\\.ffffff}";

                    worksheet.Cells[startRow, 1].Value = elapsedTimeFormatted;
                    worksheet.Cells[startRow, 2].Value = log.Action;

                    if (log.TimeTaken.HasValue)
                    {
                        worksheet.Cells[startRow, 3].Value = log.TimeTaken.Value.TotalSeconds;
                        worksheet.Cells[startRow, 3].Style.Numberformat.Format = timeTakenFormat;
                    }

                    startRow++;
                }

                // Autofit columns after the data is populated
                worksheet.Cells.AutoFitColumns();

                File.WriteAllBytes(outputPath, excelPackage.GetAsByteArray());
            }

            Console.WriteLine($"Test summary has been saved to {outputPath}");
        }
    }
}
