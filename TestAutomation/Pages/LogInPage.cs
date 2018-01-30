using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;

namespace TestAutomation.Pages
{
    public class LogInPage : Helper, IPage
    {
        //private IWebDriver driver;
        [FindsBy(How = How.XPath, Using = "//*[@id='input_0']")]
        public IWebElement emailTxtElement;
        [FindsBy(How = How.XPath, Using = "//*[@id='input_1']")]
        public IWebElement passwordTxtElement;
        [FindsBy(How = How.CssSelector, Using = "#signin")]
        public IWebElement signinBtn;
        [FindsBy(How = How.CssSelector, Using = ".sign-out")]
        public IWebElement signoutTxt;

        public string Url
        {
            get
            {
                return "/";
            }
        }





        //public void  LogInPageInit(IWebDriver driver) {

        //    _driver = driver;

        //}

        public void Credentials()
        {
            Helper.EnterText(emailTxtElement, "krogsveenTest@adfenix.com");
            Helper.EnterText(passwordTxtElement, "test99");
        }

        public void SigninBtn()
        {
            Helper.ClickOnButton(this.signinBtn);
        }

        public string SignoutTxt()
        {
            var signoutTxt = this.signoutTxt.Text;
            return signoutTxt;
        }

    }
}