using OpenQA.Selenium;

namespace EnaxisSelenium.TestSortingHelpers
{
    public class FilterManager
    {
        private readonly IFilterHandler dropdownFilterHandler;
        private readonly IFilterHandler searchBoxFilterHandler;

        public FilterManager(string tableUrl)
        {
            dropdownFilterHandler = new DropdownFilterHandler(tableUrl);
            searchBoxFilterHandler = new SearchBoxFilterHandler(tableUrl);
        }

        /// <summary>
        /// Handles filtering for a specific filter row using the appropriate filter handler based on the filter type.
        /// </summary>
        /// <param name="webdriver">The web driver used for interacting with the web page.</param>
        /// <param name="filterRow">The filter row element to be processed.</param>
        public void HandleFilter(IWebDriver webdriver, IWebElement filterRow)
        {
            var filterParentElements = filterRow.FindElements(By.ClassName("ms-parent"));

            if (filterParentElements.Count > 0)
            {
                Console.WriteLine("--- Filter type: Select ---");

                dropdownFilterHandler.HandleFilter(webdriver, filterRow);
            }
            else
            {
                filterParentElements = filterRow.FindElements(By.Id("CTRL_SQLFilters_TXTFilterSearch_1"));
                if (filterParentElements.Count > 0)
                {
                    Console.WriteLine("--- Filter type: Searchbox ---");
                    searchBoxFilterHandler.HandleFilter(webdriver, filterRow);
                }
                else
                {
                    Console.WriteLine("Warning: Could not recognize filter type. Skipping filtering...");
                }
            }
        }
    }
}
