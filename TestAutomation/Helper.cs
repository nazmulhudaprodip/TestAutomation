using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Protractor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Support.PageObjects;
using TestAutomation.Pages;
using System.Threading;

namespace TestAutomation
{
    public class Helper
    {
        public static WebDriverWait Wait;
        public static NgWebDriver driver;
        public static void EnterText(IWebElement webElement, string text)
        {

            //webElement = new NgWebElement(driver, webElement);
            webElement.Clear();
            webElement.SendKeys(text);
        }
        public static Func<IWebDriver, IWebElement> ElementIsClickable(IWebElement webElement)
        {
            return dr => (webElement.Displayed && webElement.Enabled) ? webElement : null;
        }

        public static IWebDriver ClickOnLink(IWebElement webElement)
        {
            var i = Int32.Parse(ScenarioContext.Current["stepcounter"].ToString());
            var log = ScenarioContext.Current.Get<TextWriterTraceListener>("report");
            log.WriteLine(i++ + ". And I clicked on '" + webElement.Text + "' link");
            Wait.Until(ElementIsClickable(webElement)).Click();
            log.WriteLine("-----------------------------------------------------------");
            log.Flush();
            log.Close();
            ScenarioContext.Current["stepcounter"] = i;
            return driver;
        }

        public static IWebDriver ClickOnButton(IWebElement webElement)
        {
            //var i = Int32.Parse(ScenarioContext.Current["stepcounter"].ToString());
            //var log = ScenarioContext.Current.Get<TextWriterTraceListener>("report");
            //log.WriteLine(i++ + ". And I clicked on '" + webElement.Text + "' button");
            webElement.Click();
            //log.WriteLine("-----------------------------------------------------------");
            //log.Flush();
            //log.Close();
            //ScenarioContext.Current["stepcounter"] = i;
            return driver;
        }

        public static T PageInit<T>(NgWebDriver driver) where T : class, new()
        {
            var page = new T();
            PageFactory.InitElements(driver, page);

            return page;
        }


    }
}
