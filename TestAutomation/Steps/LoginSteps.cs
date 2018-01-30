using System;
using System.Net;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.PageObjects;
using TestAutomation.Pages;
using NUnit.Framework;

namespace TestAutomation.Steps
{
    [Binding]
    public class LoginSteps : TestBase
    {
        public static LogInPage loginPage;

        [Given(@"I have valid credentials")]
        public void GivenIHaveValidCredentials()
        {
            GoTo<LogInPage>(LoginUrl,false);
            var loginPage = Helper.PageInit<LogInPage>(driver);
            loginPage.Credentials();
            System.Threading.Thread.Sleep(5000);
        }
        
        [When(@"I press signin")]
        public void WhenIPressSignin()
        {
            var loginPage = Helper.PageInit<LogInPage>(driver);
            loginPage.SigninBtn();
        }
        
        [Then(@"I will be logged in to customer")]
        public void ThenIWillBeLoggedInToCustomer()
        {
            var loginPage = Helper.PageInit<LogInPage>(driver);
            System.Threading.Thread.Sleep(5000);
            Assert.AreEqual("LOGG AV", loginPage.SignoutTxt());
        }
    }
}
