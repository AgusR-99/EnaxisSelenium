using EnaxisSelenium.ColumnSorterHandlers.Contracts;
using OpenQA.Selenium;

namespace EnaxisSelenium.ColumnSorterHandlers
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
