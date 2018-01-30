using TestAutomation.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using Protractor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using System.Configuration;
using System.Threading;
using BoDi;
using System.IO;

namespace TestAutomation
{
    [Binding]
    public class TestBase
    {
        public static WebDriverWait Wait;
        public static string LoginUrl = "https://staging-eng.adfenix.com";
        public static string SeleniumDriver = ConfigurationManager.AppSettings["SELENIUM_DRIVER"];
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        public static NgWebDriver driver
        {
            get
            {
                if (!FeatureContext.Current.ContainsKey("browser"))
                {
                    FeatureContext.Current["browser"] = StartBrowser(SeleniumDriver);
                }

                return (NgWebDriver)FeatureContext.Current["browser"];
            }
        }

        public static NgWebDriver StartBrowser(string browser)
        {
            IWebDriver driver;
            if (browser.Equals("Firefox"))
            {
                driver = new FirefoxDriver();

            }
            /* else if (browser.Equals("BrowserStack"))
             {
                 driver = SetupBrowserStack(BrowserStackDriver);
             }*/

            else
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArguments("-incognito");
                options.AddArguments("--disable-default-apps");
                options.AddArguments("--disable-extensions");
                options.AddArguments("disable-infobars");
                //options.AddArguments("start-maximized");
                driver = new ChromeDriver(options);
                //driver.Navigate().GoToUrl("https://staging-eng.adfenix.com/#/signin");

            }

            var ngdriver = new NgWebDriver(driver);
            ngdriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(100);
            ngdriver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(5);
            ngdriver.Manage().Window.Maximize();
            //ngdriver.Navigate().GoToUrl("https://staging-eng.adfenix.com/#/signin");
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            ngdriver.IgnoreSynchronization = true;
            //WaitUntilElementIsPresent(driver);
            return ngdriver;

        }

        public static void GoTo<T>(string host, bool isAngular) where T : IPage, new()
        {
            var i = int.Parse(ScenarioContext.Current["stepcounter"].ToString());
            var log = ScenarioContext.Current.Get<TextWriterTraceListener>("report");
            var page = new T();
            var url = host + page.Url;
            if (isAngular)
            {
                driver.IgnoreSynchronization = false;
                driver.Navigate().GoToUrl(url);
            }
            else
            {
                driver.WrappedDriver.Navigate().GoToUrl(url);
            }

            log.WriteLine(i++ + ". Then I go to " + driver.Title + " page: " + driver.Url);
            log.WriteLine("-----------------------------------------------------------");
            ScenarioContext.Current["stepcounter"] = i;
        }

        /* public static bool WaitUntilElementIsPresent(IWebDriver driver, int timeout = 20)
         {
             // Wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
             for (var i = 0; i < timeout; i++)
             {
                 var elemPresent = Wait.Until(d => d.FindElement(By.LinkText("Login to Adfenix")).Displayed);
                 if (elemPresent == true)
                 {
                     return true;
                 }
                 Thread.Sleep(50000);

             }
             return false;
         } */

        public static string RandomString(int size)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        [BeforeScenario]
        public void RecordLog()
        {

            if (!ScenarioContext.Current.ContainsKey("report"))
            {
                var listenerId = RandomString(20);
                var path = Directory.GetCurrentDirectory();

                var fileName = path + "\\" + FeatureContext.Current.FeatureInfo.Title + "-"
                               + ScenarioContext.Current.ScenarioInfo.Title + "_" + listenerId + "_.txt";
                var log = new TextWriterTraceListener(fileName, listenerId);
                ScenarioContext.Current.Add("report", log);
                ScenarioContext.Current.Add("report_file_name", fileName);
                ScenarioContext.Current.Add("stepcounter", 1);
                ScenarioContext.Current.Add("timeStarted", DateTime.Now);
                Trace.Listeners.Add(log);
                Trace.AutoFlush = true;
                Trace.Indent();
                log.WriteLine("-----------------------------------------------------------");
                log.WriteLine("START OF SCENARIO: " + ScenarioContext.Current.ScenarioInfo.Title);
                log.WriteLine("-----------------------------------------------------------");
            }
        }


        internal class StoredEvent
        {
            public Item[] Items { get; set; }

            public class Item
            {
                public Storage Storage { get; set; }
            }

            public class Storage
            {
                public string Url { get; set; }

                public string Key { get; set; }
            }
        }

        [AfterScenario]
        public static void CloseBrowser()
        {
            var textListener = ScenarioContext.Current.Get<TextWriterTraceListener>("report");

            if (!FeatureContext.Current.ContainsKey("browser"))
            {
                return;
            }

            var dateTime1 = ScenarioContext.Current.Get<DateTime>("timeStarted");
            var dateTime2 = DateTime.Now;
            var diff = dateTime2 - dateTime1;

            textListener.WriteLine("END OF SCENARIO: " + ScenarioContext.Current.ScenarioInfo.Title);
            textListener.WriteLine("-----------------------------------------------------------");
            textListener.WriteLine("STARTED AT: " + ScenarioContext.Current["timeStarted"] + ": COMPLETED AT: " + dateTime2 + ": EXECUTION TIME: " + diff);
            textListener.WriteLine("-----------------------------------------------------------");

            if (ScenarioContext.Current.TestError != null)
            {

                textListener.WriteLine(
                    "ERROR OCCURED: " + ScenarioContext.Current.TestError.Message + " OF TYPE: "
                    + ScenarioContext.Current.TestError.GetType().Name);

                textListener.Flush();
                textListener.Close();
            }
            else
            {
                textListener.Flush();
                textListener.Close();
            }

            if (ConfigurationManager.AppSettings["VERBOSE_MODE"].Equals("OFF"))
            {
                driver.Quit();
                driver.WrappedDriver.Quit();
            }

            FeatureContext.Current.Remove("browser");
            FeatureContext.Current.Remove("report");
        }
    }
}
