using OpenQA.Selenium;

namespace EnaxisSelenium.FilterHandlers.Contracts
{
    public interface IFilterHandler
    {
        void HandleFilter(IWebDriver webdriver, IWebElement filterRow);
    }
}
