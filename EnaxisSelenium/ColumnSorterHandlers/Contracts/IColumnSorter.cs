using OpenQA.Selenium;

namespace EnaxisSelenium.ColumnSorterHandlers.Contracts
{
    public interface IColumnSorter
    {
        void SortColumn(IWebDriver webDriver, ISortableTable table, int columnIndex);
    }
}
