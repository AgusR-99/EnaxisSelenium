using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnaxisSelenium.TestSortingHelpers
{
    public interface IFilterHandler
    {
        void HandleFilter(IWebDriver webdriver, IWebElement filterRow);
    }
}
