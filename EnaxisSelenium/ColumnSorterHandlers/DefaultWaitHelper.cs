using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnaxisSelenium.Foo.Contracts
{
    public class DefaultWaitHelper : IWaitHelper
    {
        public void WaitForInvisibilityOfElement(IWebDriver webDriver, By elementLocator)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(elementLocator));
        }
    }
}
