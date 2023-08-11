using EnaxisSelenium.SummaryHandlers;
using OpenQA.Selenium;

namespace EnaxisSelenium.ColumnSorterHandlers.Contracts
{
    public interface ISortingTestExecutor
    {
        void Execute(IWebDriver webDriver, TestSummary summary);
    }
}
