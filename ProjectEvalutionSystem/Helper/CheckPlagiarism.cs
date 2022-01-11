using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebPages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ProjectEvalutionSystem.Helper
{
    public class CheckPlagiarism
    {
        public static string chromeDriverDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            ConfigurationManager.AppSettings["seleniumBrowserDirectory"].ToString());

        public static ChromeDriver _driver = new ChromeDriver(chromeDriverDirectory);
        public static void StartProcess(string text)
        {
            _driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["PlagiarismCheckerURL"].ToString());
            _driver.Manage().Window.Maximize();

            var textArea = _driver.FindElement(By.Id("textBox"));

            if (textArea.Displayed == true)
            {
                textArea.Clear();
                textArea.SendKeys(text);
                _driver.ExecuteScript("onSubmit()");
            }

            Thread.Sleep(2000);

            var uniqueCount = _driver.FindElement(By.Id("uniqueCount"));
            if (uniqueCount.Displayed == true)
            {
                var UniqueCount = uniqueCount.Text;
            }

            var plagCount = _driver.FindElement(By.Id("plagCount"));
            if (plagCount.Displayed == true)
            {
                var PlagCount = plagCount.Text;
                //onSubmit()
            }


            var MatchUrls = _driver.FindElement(By.XPath("//*[@id='mainResultsDisplay']/div[2]/div[2]/ul/li[2]/a"));
            if (MatchUrls.Displayed == true)
            {
                MatchUrls.Click();
            }


            var matchesTable = _driver.FindElement(By.Id("matches"));
            if (matchesTable.Displayed == true)
            {
                var matchesText = matchesTable.;
            }


            _driver.Close();

        }
    }
}