using EnaxisSelenium.ColumnSorterHandlers.Contracts;
using EnaxisSelenium.Helpers;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Diagnostics;
using EnaxisSelenium.SummaryHandlers;

namespace EnaxisSelenium.ColumnSorterHandlers
{
    public class SortingTestExecutor : ISortingTestExecutor
    {
        public void Execute(IWebDriver webDriver, TestSummary summary)
        {
            var defaultWaitHelper = new DefaultWaitHelper();
            var columnSorter = new DefaultColumnSorter(defaultWaitHelper);

            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));

            var sortableTable = new SortableTable(webDriver, wait);

            int columnCount;
            int sortableColumnCount;

            var MainTitle = GetMainTitle(webDriver);
            summary.LogMessage($"-In {MainTitle}-");

            try
            {
                try
                {
                    columnCount = CommonHelpers.GetColumnCount(webDriver);
                    sortableColumnCount = CommonHelpers.GetSortableColumnCount(webDriver);
                }
                catch (Exception)
                {
                    Console.WriteLine("Unable to locate table component");
                    throw;
                }

                Console.WriteLine("Number of columns in the table: " + columnCount);
                Console.WriteLine("Number of sortable columns in the table: " + sortableColumnCount);

                Assert.Multiple(() =>
                {
                    summary.LogMessage($"Starting Sorting Test with {sortableColumnCount} sortable columns");
                    var sortingTimer = Stopwatch.StartNew();

                    for (int i = 0; i < columnCount; i++)
                    {
                        columnSorter.SortColumn(webDriver, sortableTable, i);
                    }

                    sortingTimer.Stop();
                    summary.LogTime("Sorting Test", sortingTimer.Elapsed);
                });
            }
            catch (Exception)
            {
                Console.WriteLine($"\nFAILED: Sorting Test - {MainTitle}");
                throw;
            }

            Console.WriteLine($"\nOK: Sorting Test - {MainTitle}");
        }

        private string GetMainTitle(IWebDriver webDriver)
        {
            return webDriver.FindElement(By.XPath("//*[@id=\"TextoTituloPantalla\"]")).Text;
        }
    }
}
