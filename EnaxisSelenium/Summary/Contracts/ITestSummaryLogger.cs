namespace EnaxisSelenium.Helpers.Contracts
{
    public interface ITestSummaryLogger
    {
        void LogMessage(string message);
        void LogTime(string actionName, TimeSpan timeTaken);
    }
}
