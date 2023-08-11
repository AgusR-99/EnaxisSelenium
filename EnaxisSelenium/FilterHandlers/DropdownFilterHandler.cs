using EnaxisSelenium.FilterHandlers.Contracts;
using EnaxisSelenium.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;

namespace EnaxisSelenium.FilterHandlers
{
    public class DropdownFilterHandler : IFilterHandler
    {
        private readonly string tableUrl;

        public DropdownFilterHandler(string tableUrl)
        {
            this.tableUrl = tableUrl;
        }

        /// <summary>
        /// Handles filtering using the dropdown filter for a specific filter row.
        /// </summary>
        /// <param name="webDriver">The web driver used for interacting with the web page.</param>
        /// <param name="filterRow">The filter row element to be processed.</param>
        public void HandleFilter(IWebDriver webDriver, IWebElement filterRow)
        {
            var filterDropDown = filterRow.FindElement(By.ClassName("ms-parent"));
            var options = filterDropDown.FindElements(By.TagName("li"));

            bool uncheckedCheckboxes = false;

            var selectElement = filterRow.FindElement(By.TagName("select"));
            string multipleAttributeValue = selectElement.GetAttribute("multiple");
            bool isMultiple = !string.IsNullOrEmpty(multipleAttributeValue) && multipleAttributeValue.ToLower() == "true";

            Console.WriteLine($"  - Filtering by '{filterRow.Text}...");
            for (int j = 0; j < options.Count; j++)
            {
                filterDropDown.Click();

                if (isMultiple)
                {
                    // Uncheck previous option if it was checked
                    if (j > 0)
                    {
                        var previousOption = filterDropDown.FindElements(By.TagName("li"))[j - 1];
                        previousOption.Click();
                    }
                    if (!uncheckedCheckboxes)
                    {
                        var checkedCheckboxes = filterRow.FindElements(By.ClassName("selected"));
                        UncheckAllCheckboxes(checkedCheckboxes);
                        uncheckedCheckboxes = true;
                    }
                }

                var option = filterDropDown.FindElements(By.TagName("li"))[j];

                if (option.Displayed)
                {
                    var optionText = option.Text;
                    Console.WriteLine($"    - With option '{optionText}'...");

                    option.Click();

                    if (isMultiple)
                    {
                        // Close element to avoid element click interception
                        filterDropDown.Click();
                    }

                    var applyFiltersButton = webDriver.FindElement(By.Id("CTRL_SQLFilters_FiltroButton"));

                    string filterRowId = "";

                    filterRowId = filterRow.GetAttribute("id");

                    applyFiltersButton.Click();

                    var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
                    wait.Until(ExpectedConditions.StalenessOf(filterRow));
                    Console.WriteLine($"     - OK: filtered with option '{optionText}'...");

                    try
                    {

                        var recordCount = CommonHelpers.GetRecordCount(webDriver);
                        Console.WriteLine($"       - Returned {recordCount} records");
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"       - Warning: Could not retrieve record count");
                    }

                    // Refresh element
                    try
                    {
                        filterRow = webDriver.FindElement(By.Id(filterRowId));

                        if (filterRow != null)
                        {
                            filterDropDown = filterRow.FindElement(By.ClassName("ms-parent"));
                        }
                        else
                        {
                            Console.WriteLine($"    - Warning: Filter row '{optionText}' not found after refresh.");
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("COMPATIBILITY ERROR: Select Filter Id does not exist. Cannot continue test.");
                        throw;
                    }

                }
            }

            // Refresh page
            webDriver.Navigate().GoToUrl(tableUrl);
        }

        private void UncheckAllCheckboxes(ReadOnlyCollection<IWebElement> checkboxes)
        {
            foreach (var checkbox in checkboxes)
            {
                checkbox.Click();
            }
        }
    }

}
