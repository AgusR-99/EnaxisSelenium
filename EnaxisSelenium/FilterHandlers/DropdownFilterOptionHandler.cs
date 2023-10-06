using EnaxisSelenium.Helpers;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using EnaxisSelenium.FilterHandlers.Contracts;

namespace EnaxisSelenium.FilterHandlers
{
    public class DropdownFilterOptionHandler : IDropdownFilterOptionHandler
    {
        private readonly IWebDriver webDriver;

        public DropdownFilterOptionHandler(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        public void HandleFastFilterOption(IWebElement filterRow)
        {
            var filterDropDown = filterRow.FindElement(By.ClassName("ms-parent"));

            filterDropDown.Click();

            var options = filterDropDown.FindElements(By.TagName("li"));

            IWebElement option;

            if (options.Count() > 2)
            {
                // Choose second to last option
                option = options[options.Count - 2];
            }
            else
            {
                option = options[0];
            }

            if (option.Displayed)
            {
                var optionText = ClickOption(option);

                if (IsMultiple(filterRow))
                {
                    // Close element to avoid element click interception
                    filterDropDown.Click();
                }

                ApplyFilters(filterRow);

                Console.WriteLine($"     - OK: filtered with option '{optionText}'...");

                LogRecordCount();
            }
        }

        public void HandleFullFilterOption(IWebElement filterRow)
        {
            var isMultiple = IsMultiple(filterRow);

            var filterDropDown = filterRow.FindElement(By.ClassName("ms-parent"));
            var options = filterDropDown.FindElements(By.TagName("li"));
            bool uncheckedCheckboxes = false;

            for (int j = 0; j < options.Count; j++)
            {
                filterDropDown.Click();

                if (isMultiple)
                {
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
                    var optionText = ClickOption(option);

                    if (isMultiple)
                    {
                        // Close element to avoid element click interception
                        filterDropDown.Click();
                    }

                    string filterRowId = filterRow.GetAttribute("id");

                    ApplyFilters(filterRow);
                    Console.WriteLine($"     - OK: filtered with option '{optionText}'...");

                    LogRecordCount();

                    filterRow = webDriver.FindElement(By.Id(filterRowId));

                    filterDropDown = RefreshElement(filterDropDown, filterRow, optionText);
                }
            }
        }

        private static bool IsMultiple(IWebElement filterRow)
        {
            var selectElement = filterRow.FindElement(By.TagName("select"));
            string multipleAttributeValue = selectElement.GetAttribute("multiple");
            return !string.IsNullOrEmpty(multipleAttributeValue) && multipleAttributeValue.ToLower() == "true";
        }

        private static void UncheckAllCheckboxes(ReadOnlyCollection<IWebElement> checkboxes)
        {
            foreach (var checkbox in checkboxes)
            {
                checkbox.Click();
            }
        }

        private void ApplyFilters(IWebElement filterRow)
        {
            var applyFiltersButton = webDriver.FindElement(By.Id("CTRL_SQLFilters_FiltroButton"));
            string filterRowId = filterRow.GetAttribute("id");

            applyFiltersButton.Click();

            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.StalenessOf(filterRow));
        }

        private string? ClickOption(IWebElement option)
        {
            var optionText = option.Text;
            Console.WriteLine($"    - With option '{optionText}'...");

            option.Click();

            return optionText;
        }

        private void LogRecordCount()
        {
            try
            {
                var recordCount = CommonHelpers.GetRecordCount(webDriver);
                Console.WriteLine($"       - Returned {recordCount} records");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"       - Warning: Could not retrieve record count");
            }
        }

        private IWebElement RefreshElement(IWebElement filterDropDown, IWebElement filterRow, string optionText)
        {
            try
            {
                if (filterRow != null)
                {
                    filterDropDown = filterRow.FindElement(By.ClassName("ms-parent"));
                }
                else
                {
                    Console.WriteLine($"    - Warning: Filter row '{optionText}' not found after refresh.");
                }

                return filterDropDown;
            }
            catch (Exception)
            {
                Console.WriteLine("COMPATIBILITY ERROR: Select Filter Id does not exist. Cannot continue test.");
                throw;
            }
        }
    }

}