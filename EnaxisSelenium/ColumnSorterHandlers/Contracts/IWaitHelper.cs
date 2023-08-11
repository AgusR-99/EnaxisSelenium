using OpenQA.Selenium;

namespace EnaxisSelenium.Foo.Contracts
{
    public interface IWaitHelper
    {
        void WaitForInvisibilityOfElement(IWebDriver webDriver, By elementLocator);
    }
}
