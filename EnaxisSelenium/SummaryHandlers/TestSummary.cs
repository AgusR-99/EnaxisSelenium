using EnaxisSelenium.SummaryHandlers.Contracts;
using System.Diagnostics;

namespace EnaxisSelenium.SummaryHandlers
{
    public class TestSummary : ITestSummaryLogger
    {
        private readonly List<(TimeSpan ElapsedTime, string Action, TimeSpan? TimeTaken)> logMessages;
        private readonly Stopwatch stopwatch;

        public TestSummary()
        {
            logMessages = new List<(TimeSpan ElapsedTime, string Action, TimeSpan? TimeTaken)>();
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public void LogMessage(string message)
        {
            logMessages.Add((stopwatch.Elapsed, message, null));
        }

        public void LogTime(string actionName, TimeSpan timeTaken)
        {
            logMessages.Add((stopwatch.Elapsed, actionName, timeTaken));
        }

        public void PrintSummary(string title, ITestSummaryExporter exporter, string outputDirectory = "C:\\Users\\agus-\\OneDrive\\Documentos\\Logs")
        {
            // Generate a timestamp-based filename
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string outputPath = Path.Combine(outputDirectory, $"{title}_{timestamp}.xlsx");

            exporter.Export(logMessages, outputPath);
        }
    }
}