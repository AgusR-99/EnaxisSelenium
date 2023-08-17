using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnaxisSelenium.FilterHandlers.Contracts
{
    public interface IDropdownFilterOptionHandler
    {
        void HandleFullFilterOption(IWebElement filterRow);

        void HandleFastFilterOption(IWebElement filterRow);
    }
}
