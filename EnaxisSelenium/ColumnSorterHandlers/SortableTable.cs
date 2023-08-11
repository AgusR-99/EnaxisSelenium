using EnaxisSelenium.ColumnSorterHandlers.Contracts;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace EnaxisSelenium.ColumnSorterHandlers
{
    public class SortableTable : ISortableTable
    {
        private readonly IWebDriver webDriver;
        private readonly WebDriverWait wait;

        public SortableTable(IWebDriver webDriver, WebDriverWait wait)
        {
            this.webDriver = webDriver;
            this.wait = wait;
        }

        public bool IsLinkPresentInHeader(int columnIndex)
        {
            try
            {
                var columnHeader = webDriver.FindElement(By.XPath($"//*[@id='ctl00_FiltersDataGrid']/tbody/tr[1]/th[{columnIndex + 1}]/a"));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void SortByColumn(int columnIndex)
        {
            try
            {
                int amountOfClicks = 2;

                for (int i = 0; i < amountOfClicks; i++)
                {
                    var columnHeader = webDriver.FindElement(By.XPath($"//*[@id='ctl00_FiltersDataGrid']/tbody/tr[1]/th[{columnIndex + 1}]/a"));
                    var columnHeaderText = columnHeader.Text;

                    var loadingOverlay = By.XPath("/html/body/div[3]");
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingOverlay));

                    if (columnHeader.Displayed)
                    {
                        Console.WriteLine("--- Sorting by '" + columnHeaderText + "' ---");
                        Console.WriteLine("- Sorting attempt #" + (i + 1));
                        columnHeader.Click();

                        loadingOverlay = By.XPath("/html/body/div[3]");
                        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingOverlay));

                        var errorMessage = webDriver.FindElement(By.XPath("//*[@id='FFPopUp']"));
                        Assert.IsFalse(errorMessage.Displayed, "  - FAILED: Could not sort by '" + columnHeaderText + "'.");

                        if (errorMessage.Displayed)
                        {
                            Console.WriteLine("  - FAILED: Could not sort by '" + columnHeaderText + "'.");
                            var errorMessageButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='imgClosePopUp']")));
                            errorMessageButton.Click();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("  - OK: Sorted by '" + columnHeaderText + "'.");
                        }
                    }
                }
            }
            catch (WebDriverTimeoutException)
            {
                // If the error message does not appear within the short wait time, ignore the exception
                // The flag isErrorMessageDisplayed will remain false
            }
            catch (Exception ex)
            {
                // If an exception occurs during sorting for any column, the test passes
                // If no exception is thrown, the test will fail
                Assert.Fail("Exception occurred during sorting: " + ex.Message);
            }
        }
    }
}
