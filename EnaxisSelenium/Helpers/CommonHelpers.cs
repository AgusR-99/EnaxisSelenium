using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace EnaxisSelenium.Helpers
{
    public static class CommonHelpers
    {
        /// <summary>
        /// Gets the number of columns in a table identified by the specified web driver.
        /// </summary>
        /// <param name="webDriver">The web driver used to locate the table.</param>
        /// <returns>The number of columns in the table.</returns>
        public static int GetColumnCount(IWebDriver webDriver)
        {
            // Locate the table element by its ID
            var tableElement = webDriver.FindElement(By.Id("ctl00_FiltersDataGrid"));

            // Find all header cells within the first row of the table
            var headerCells = tableElement.FindElements(By.XPath(".//tr[1]/th"));

            // Count the number of header cells, which represents the number of columns in the table
            int columnCount = headerCells.Count;

            return columnCount;
        }

        public static int GetSortableColumnCount(IWebDriver webDriver)
        {
            var tableElement = webDriver.FindElement(By.Id("ctl00_FiltersDataGrid"));

            var headerCells = tableElement.FindElements(By.XPath(".//tr[1]/th/a"));

            int columnCount = headerCells.Count;

            return columnCount;
        }

        public static int GetRecordCount(IWebDriver webDriver)
        {
            var recordElement = webDriver.FindElement(By.XPath("//*[@id=\"ctl00_TituloListado\"]/i")).Text;

            return int.Parse(Regex.Replace(recordElement, "[^0-9]", ""));
        }

        public static string GetCurrentMethodName()
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            var frame = stackTrace.GetFrame(1);
            var method = frame.GetMethod();
            return method.Name;
        }
    }
}
