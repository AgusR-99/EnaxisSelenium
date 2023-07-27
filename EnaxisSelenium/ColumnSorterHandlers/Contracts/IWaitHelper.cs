using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnaxisSelenium.Foo.Contracts
{
    public interface IWaitHelper
    {
        void WaitForInvisibilityOfElement(IWebDriver webDriver, By elementLocator);
    }
}
