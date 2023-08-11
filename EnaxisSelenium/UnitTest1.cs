using EnaxisSelenium.ColumnSorterHandlers;
using EnaxisSelenium.Summary;
using EnaxisSelenium.TestSortingHelpers;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;

namespace EnaxisSelenium
{
    public class Tests
    {
        private IWebDriver webDriver;
        private WebDriverWait wait;
        private IConfiguration configuration;

        private string loginUrl => GetSetting("LoginUrl");
        private string tableUrl => GetSetting("TableUrl");
        private string username => GetSetting("Username");
        private string password => GetSetting("Password");

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--ignore-certificate-errors");

            webDriver = new ChromeDriver(options);

            wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30));

            // Load appsettings.json configuration
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configuration = configBuilder.Build();
        }

        private string GetSetting(string key) => configuration.GetSection("TestSettings")[key];


        [Test]
        public void TestLogin()
        {
            try
            {
                webDriver.Navigate().GoToUrl(loginUrl);

                var txtUserName = webDriver.FindElement(By.Name("TextUser"));
                Assert.That(txtUserName.Displayed, Is.True);
                txtUserName.SendKeys(username);

                var txtPassword = webDriver.FindElement(By.Name("TextPassword"));
                Assert.That(txtPassword.Displayed, Is.True);
                txtPassword.SendKeys(password);

                webDriver.FindElement(By.Name("btnLogin")).Submit();

                var imgHomeLogo = webDriver.FindElement(By.Id("Imagen"));
                Assert.That(imgHomeLogo.Displayed, Is.True);

                // Login successful
                Console.WriteLine("OK: Login successful!\n");
            }
            catch (Exception ex)
            {
                // Login failed
                Console.WriteLine("FAILED: Login failed. Reason: " + ex.Message);
                Assert.Fail("FAILED: Login failed. Reason: " + ex.Message);
            }
        }

        [Test]
        public void TestSorting()
        {
            TestLogin();
            webDriver.Navigate().GoToUrl(tableUrl);

            int columnCount = CommonHelpers.GetColumnCount(webDriver);

            Console.WriteLine("Number of sortable columns in the table: " + columnCount);

            Assert.Multiple(() =>
            {
                var defaultWaitHelper = new DefaultWaitHelper();

                var columnSorter = new DefaultColumnSorter(defaultWaitHelper);

                var sortableTable = new SortableTable(webDriver, wait);

                for (int i = 0; i < columnCount; i++)
                {
                    columnSorter.SortColumn(webDriver, sortableTable, i);
                }
            });

            Console.WriteLine("\nAll tests passed!");
        }

        [Test]
        public void FilterTest()
        {
            TestLogin();
            webDriver.Navigate().GoToUrl(tableUrl);

            var filterRows = webDriver.FindElements(By.CssSelector("tr.XMLFilterRow"));
            Console.WriteLine($"Found {filterRows.Count} filters");
            Console.WriteLine($"--------------------------------");

            var filterManager = new FilterManager(tableUrl);

            for (int i = 0; i < filterRows.Count; i++)
            {
                var filterRow = filterRows[i];

                var filterRowText = filterRow.Text;

                filterManager.HandleFilter(webDriver, filterRow);

                // Refresh the filterRows collection to avoid stale elements
                filterRows = webDriver.FindElements(By.CssSelector("tr.XMLFilterRow"));

                // Find the updated filterRow element in the refreshed collection
                filterRow = filterRows.FirstOrDefault(fr => fr.Text == filterRowText);
            }
        }

        [TearDown]
        public void Teardown()
        {
            webDriver.Quit();
        }
    }
}