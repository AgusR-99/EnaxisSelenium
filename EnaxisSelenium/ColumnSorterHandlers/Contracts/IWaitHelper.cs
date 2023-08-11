using OpenQA.Selenium;

namespace EnaxisSelenium.ColumnSorterHandlers.Contracts
{
    public interface IWaitHelper
    {
        void WaitForInvisibilityOfElement(IWebDriver webDriver, By elementLocator);
    }
}
