using EnaxisSelenium.Foo.Contracts;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnaxisSelenium.Foo
{
    public class DefaultColumnSorter : IColumnSorter
    {
        private readonly IWaitHelper waitHelper;

        public DefaultColumnSorter(IWaitHelper waitHelper)
        {
            this.waitHelper = waitHelper;
        }

        public void SortColumn(IWebDriver webDriver, ISortableTable table, int columnIndex)
        {
            if (table.IsLinkPresentInHeader(columnIndex))
            {
                table.SortByColumn(columnIndex);
                waitHelper.WaitForInvisibilityOfElement(webDriver, By.XPath("/html/body/div[3]"));
            }
        }
    }
}
