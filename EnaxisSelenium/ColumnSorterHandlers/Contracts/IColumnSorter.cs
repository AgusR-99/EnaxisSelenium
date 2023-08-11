using OpenQA.Selenium;

namespace EnaxisSelenium.Foo.Contracts
{
    public interface IColumnSorter
    {
        void SortColumn(IWebDriver webDriver, ISortableTable table, int columnIndex);
    }
}
