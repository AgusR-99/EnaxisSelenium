using EnaxisSelenium.Helpers;
using OpenQA.Selenium;

namespace EnaxisSelenium.TestSortingHelpers
{
    public class SearchBoxFilterHandler : IFilterHandler
    {
        private readonly string tableUrl;

        public SearchBoxFilterHandler(string tableUrl)
        {
            this.tableUrl = tableUrl;
        }

        /// <summary>
        /// Handles filtering using the search box filter for a specific filter row.
        /// </summary>
        /// <param name="webDriver">The web driver used for interacting with the web page.</param>
        /// <param name="filterRow">The filter row element to be processed.</param>
        public void HandleFilter(IWebDriver webDriver, IWebElement filterRow)
        {
            // Find the search input element for the filter row
            var filterSearchInputElement = webDriver.FindElement(By.Id("CTRL_SQLFilters_TXTFilterSearch_1"));

            // Get the number of columns in the table
            var columnCount = CommonHelpers.GetColumnCount(webDriver);
            int columnIndex;
            string columnHeaderText = "";

            // Loop through each column in the table
            for (columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                // Find the header element for the current column
                var columnHeader = webDriver.FindElement(By.XPath($"//*[@id='ctl00_FiltersDataGrid']/tbody/tr[2]/td[{columnIndex + 1}]"));
                columnHeaderText = columnHeader.Text;

                if (columnHeader.Displayed && !string.IsNullOrWhiteSpace(columnHeader.Text))
                {
                    // Enter the column header text into the search input element
                    filterSearchInputElement.SendKeys(columnHeaderText);
                    Console.WriteLine($"  - Filtering by string '{columnHeaderText}'");

                    // Click the apply filters button to trigger the search
                    var applyFiltersButton = webDriver.FindElement(By.Id("CTRL_SQLFilters_FiltroButton"));
                    applyFiltersButton.Click();
                    Console.WriteLine($"    - OK: Filtered by string '{columnHeaderText}'");

                    try
                    {
                        var recordCount = CommonHelpers.GetRecordCount(webDriver);
                        Console.WriteLine($"      - Returned {recordCount} records");
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"      - Warning: Could not retrieve record count");
                    }

                    // Navigate back to the table URL to clear the search filter
                    webDriver.Navigate().GoToUrl(tableUrl);

                    // Refresh the search input element
                    filterSearchInputElement = webDriver.FindElement(By.Id("CTRL_SQLFilters_TXTFilterSearch_1"));
                }
            }
        }
    }
}
