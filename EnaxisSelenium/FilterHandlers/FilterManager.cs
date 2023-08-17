using EnaxisSelenium.FilterHandlers.Contracts;
using OpenQA.Selenium;

namespace EnaxisSelenium.FilterHandlers
{
    public class FilterManager
    {
        private readonly IDropdownFilterOptionHandler dropdownFilterHandler;
        private readonly IFilterHandler searchBoxFilterHandler;
        private readonly IWebDriver webdriver;
        private readonly bool isFastVariation;

        public FilterManager(IWebDriver webdriver, string tableUrl, bool isFastVariation = false)
        {
            dropdownFilterHandler = new DropdownFilterOptionHandler(webdriver);
            searchBoxFilterHandler = new SearchBoxFilterHandler(webdriver);
            this.webdriver = webdriver;
            this.isFastVariation = isFastVariation;
        }

        /// <summary>
        /// Handles filtering for a specific filter row using the appropriate filter handler based on the filter type.
        /// </summary>
        /// <param name="webdriver">The web driver used for interacting with the web page.</param>
        /// <param name="filterRow">The filter row element to be processed.</param>
        public void HandleFilter(IWebElement filterRow)
        {
            var filterDropDownList = filterRow.FindElements(By.ClassName("ms-parent"));

            if (filterDropDownList.Count > 0)
            {
                /*Console.WriteLine("--- Filter type: Select ---");

                if (isFastVariation)
                {
                    dropdownFilterHandler.HandleFastFilterOption(filterRow);
                }
                else
                {
                    dropdownFilterHandler.HandleFullFilterOption(filterRow);
                }
                webdriver.Navigate().Refresh();*/

            }
            else
            {
                var filterParentElements = filterRow.FindElements(By.Id("CTRL_SQLFilters_TXTFilterSearch_1"));
                if (filterParentElements.Count > 0)
                {
                    Console.WriteLine("--- Filter type: Searchbox ---");
                    searchBoxFilterHandler.HandleFilter(filterRow);
                    webdriver.Navigate().Refresh();
                }
                else
                {
                    Console.WriteLine("Warning: Could not recognize filter type. Skipping filtering...");
                }
            }
            
        }
    }
}
