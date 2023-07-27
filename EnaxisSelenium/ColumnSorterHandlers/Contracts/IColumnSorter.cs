using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnaxisSelenium.Foo.Contracts
{
    public interface IColumnSorter
    {
        void SortColumn(IWebDriver webDriver, ISortableTable table, int columnIndex);
    }
}
