using EnaxisSelenium.ColumnSorterHandlers;
using EnaxisSelenium.FilterHandlers;
using EnaxisSelenium.SummaryHandlers;
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
        private static IConfiguration configuration;

        private string loginUrl => GetSetting("LoginUrl");
        private string username => GetSetting("Username");
        private string password => GetSetting("Password");
        private TestSummary summary = new TestSummary();

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--ignore-certificate-errors");

            webDriver = new ChromeDriver(options);

            wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(30));
        }

        private string GetSetting(string key) => configuration.GetSection("TestSettings")[key];

        private static List<string> GetTableUrls()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configuration = configBuilder.Build();
            var foo = configuration.GetSection("TestSettings:TableUrls").GetChildren().Select(x => x.Value).ToList();
            return foo;
        }

        public void TestLogin()
        {
            try
            {
                var loginTimer = Stopwatch.StartNew();

                summary.LogMessage($"Navigating to Login: {loginUrl}");

                webDriver.Navigate().GoToUrl(loginUrl);

                loginTimer.Stop();
                summary.LogTime("Navigation to Login", loginTimer.Elapsed);

                var txtUserName = webDriver.FindElement(By.Name("TextUser"));
                Assert.That(txtUserName.Displayed, Is.True);
                txtUserName.SendKeys(username);

                var txtPassword = webDriver.FindElement(By.Name("TextPassword"));
                Assert.That(txtPassword.Displayed, Is.True);
                txtPassword.SendKeys(password);

                var signInTimer = Stopwatch.StartNew();

                summary.LogMessage("Login in");
                webDriver.FindElement(By.Name("btnLogin")).Submit();

                var imgHomeLogo = webDriver.FindElement(By.Id("Imagen"));
                summary.LogTime("Login", signInTimer.Elapsed);

                Assert.That(imgHomeLogo.Displayed, Is.True);

                // Login successful
                string message = "OK: Login successful!\n";
                Console.WriteLine(message);
            }
            catch (Exception ex)
            {
                // Login failed
                string errorMessage = "FAILED: Login failed. Reason: " + ex.Message;
                Console.WriteLine(errorMessage);
                Assert.Fail(errorMessage);
            }
        }

        [Test]
        [TestCaseSource(nameof(GetTableUrls))]
        public void TestSorting(string tableUrl)
        {
            summary = new TestSummary();

            TestLogin();
            NavigateToTable(tableUrl);

            var sortingTestExecutor = new SortingTestExecutor();
            sortingTestExecutor.Execute(webDriver, summary);

            ExportTestSummary();
        }



        [Test]
        [TestCaseSource(nameof(GetTableUrls))]
        public void FilterTest(string tableUrl)
        {
            summary = new TestSummary();

            TestLogin();
            NavigateToTable(tableUrl);
            var MainTitle = GetMainTitle();
            LogMainTitle(MainTitle);

            var filterRows = webDriver.FindElements(By.CssSelector("tr.XMLFilterRow"));
            LogFilterCount(filterRows.Count);

            var filterManager = new FilterManager(tableUrl);
            ApplyFilters(filterManager, filterRows);

            ExportTestSummary();
        }


        private void NavigateToTable(string tableUrl)
        {
            var navigationTimer = Stopwatch.StartNew();
            summary.LogMessage($"Navigating to table URL: {tableUrl}");
            webDriver.Navigate().GoToUrl(tableUrl);
            navigationTimer.Stop();
            summary.LogTime("Navigation to Table", navigationTimer.Elapsed);
        }

        private string GetMainTitle()
        {
            return webDriver.FindElement(By.XPath("//*[@id=\"TextoTituloPantalla\"]")).Text;
        }

        private void LogMainTitle(string title)
        {
            summary.LogMessage($"-In {title}-");
            Console.WriteLine($"-In {title}-\n");
        }

        private void LogFilterCount(int count)
        {
            summary.LogMessage($"Found {count} filters");
            Console.WriteLine($"Found {count} filters");
        }

        private void ApplyFilters(FilterManager filterManager, IReadOnlyList<IWebElement> filterRows)
        {
            var filteringTimer = Stopwatch.StartNew();
            foreach (var filterRow in filterRows)
            {
                var filterRowText = filterRow.Text;
                filterManager.HandleFilter(webDriver, filterRow);

                // Refresh the filterRows collection to avoid stale elements
                filterRows = webDriver.FindElements(By.CssSelector("tr.XMLFilterRow"));

                // Find the updated filterRow element in the refreshed collection
                var updatedFilterRow = filterRows.FirstOrDefault(fr => fr.Text == filterRowText);
            }
            filteringTimer.Stop();
            summary.LogTime("Filtering Test", filteringTimer.Elapsed);
        }

        private void ExportTestSummary()
        {
            var excelExporter = new ExcelTestSummaryExporter();
            summary.PrintSummary(GetMainTitle(), excelExporter);
        }


        public class UserData
        {
            public string Email { get; set; }
            public string Username { get; set; }
            public string LoginName { get; set; }
            public string Password { get; set; }
            public int GeneralLevel { get; set; }
            public int DocumentsLevel { get; set; }
            public int BSProcessesLevel { get; set; }
        }

        public void CreateUserTest()
        {
            TestLogin();
            webDriver.Navigate().GoToUrl("https://20.206.240.13/enaxis.secco.10334/IKBase.aspx?VIEW=ADMINUSERPROPERTIES&MODE=NEW&USRNAME=New&USRCODE=");
            var userDataList = new List<UserData>
        {
            new UserData
            {
                Email = "administrador@",
                Username = "Administrador",
                LoginName = "administrador",
                Password = "12345678",
                GeneralLevel = 4,
                DocumentsLevel = 0,
                BSProcessesLevel = 0
            },
            new UserData
            {
                Email = "altaas@",
                Username = "Alta AS",
                LoginName = "altaas",
                Password = "12345678",
                GeneralLevel = 1,
                DocumentsLevel = 1,
                BSProcessesLevel = 1
            },
            new UserData
            {
                Email = "coordinador@",
                Username = "Coordinador",
                LoginName = "coordinador",
                Password = "12345678",
                GeneralLevel = 1,
                DocumentsLevel = 1,
                BSProcessesLevel = 1
            },
            new UserData
            {
                Email = "supervisor@",
                Username = "Supervisor",
                LoginName = "supervisor",
                Password = "12345678",
                GeneralLevel = 1,
                DocumentsLevel = 1,
                BSProcessesLevel = 1
            },
            new UserData
            {
                Email = "involucrado@",
                Username = "Involucrado",
                LoginName = "involucrado",
                Password = "12345678",
                GeneralLevel = 1,
                DocumentsLevel = 1,
                BSProcessesLevel = 1
            },
            // If you have more data, add additional entries here
        };

            foreach (var user in userDataList)
            {
                var txtEmail = webDriver.FindElement(By.Id("ctl00_Email"));
                Assert.That(txtEmail.Displayed, Is.True);
                txtEmail.SendKeys(user.Email);

                // Find the username input element by ID and enter the user's username.
                var txtUsername = webDriver.FindElement(By.Id("ctl00_UserName"));
                Assert.That(txtUsername.Displayed, Is.True);
                txtUsername.SendKeys(user.Username);

                // Find the login name input element by ID and enter the user's login name.
                var txtLoginName = webDriver.FindElement(By.Id("ctl00_LoginName"));
                Assert.That(txtLoginName.Displayed, Is.True);
                txtLoginName.SendKeys(user.LoginName);

                // Find the password input element by ID and enter the user's password.
                var txtPassword = webDriver.FindElement(By.Id("ctl00_WebPwd"));
                Assert.That(txtPassword.Displayed, Is.True);
                txtPassword.SendKeys(user.Password);

                // Set the General Level radio button according to the user's desired level.
                var generalLevelRadioButton = webDriver.FindElement(By.CssSelector("#ctl00_UserLevel input[type='radio'][value='" + user.GeneralLevel + "']"));
                generalLevelRadioButton.Click();

                // Set the Documents Level radio button according to the user's desired level.
                var documentsLevelRadioButton = webDriver.FindElement(By.CssSelector("#ctl00_DocumentLevel input[type='radio'][value='" + user.DocumentsLevel + "']"));
                documentsLevelRadioButton.Click();

                // Set the BS/Processes Level radio button according to the user's desired level.
                var bsProcessesLevelRadioButton = webDriver.FindElement(By.CssSelector("#ctl00_BSProcLevel input[type='radio'][value='" + user.BSProcessesLevel + "']"));
                bsProcessesLevelRadioButton.Click();

                var submit = webDriver.FindElement(By.XPath("//*[@id=\"LowerMenu_jmenu1p\"]/a[1]"));
                submit.Click();
            }
        }

        [TearDown]
        public void Teardown()
        {
            webDriver.Quit();
        }
    }
}