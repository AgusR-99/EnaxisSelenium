using EnaxisSelenium.Summary;
using OpenQA.Selenium;

namespace EnaxisSelenium.ColumnSorterHandlers.Contracts
{
    public interface ISortingTestExecutor
    {
        void Execute(IWebDriver webDriver, TestSummary summary);
    }
}
