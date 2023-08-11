using OpenQA.Selenium;

namespace EnaxisSelenium.TestSortingHelpers
{
    public interface IFilterHandler
    {
        void HandleFilter(IWebDriver webdriver, IWebElement filterRow);
    }
}
