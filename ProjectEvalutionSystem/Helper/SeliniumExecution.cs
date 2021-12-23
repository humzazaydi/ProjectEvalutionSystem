using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace ProjectEvalutionSystem.Helper
{
    public static class SeliniumExecution
    {
        public static string _browserDirectory = ConfigurationManager.AppSettings["seleniumBrowserDirectory"];
        public static ChromeDriver driver = new ChromeDriver(_browserDirectory);
        public static WebDriverWait webDriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
        public static void StartProcess(string filePath)
        {
            try
            {
                var AssignmentText = GetTextFromWordDoc(filePath);

                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["PlagiarismCheckerURL"].ToString());

                // ERROR: Caught exception [ERROR: Unsupported command [selectFrame | index=0 | ]]
                webDriverWait.IgnoreExceptionTypes(typeof(NoSuchElementException));

                driver.FindElement(By.Id("textBoxMain")).Clear();
                driver.FindElement(By.Id("textBoxMain")).SendKeys(AssignmentText);
                webDriverWait.IgnoreExceptionTypes(typeof(NoSuchElementException));

                // ERROR: Caught exception [ERROR: Unsupported command [selectFrame | relative=parent | ]]

                Thread.Sleep(5000);
                driver.FindElement(By.XPath("//button[@type='submit']")).Click();


                webDriverWait.IgnoreExceptionTypes(typeof(NoSuchElementException));

                Thread.Sleep(10000);

                string Plagiarism = 
                driver.FindElement(By.XPath("//section[@id='progressBarMain']/div/div/div/div[2]/div[2]/div/span/span[2]")).Text;

                string Unique = 
                driver.FindElement(By.XPath("//section[@id='progressBarMain']/div/div/div/div[2]/div[2]/div/span[2]/span[2]")).Text;
                driver.FindElement(By.XPath("//div[@id='generate_report']/button[3]")).Click();


                webDriverWait.IgnoreExceptionTypes(typeof(NoSuchElementException));


                driver.FindElement(By.Id("in")).Click();
                driver.FindElement(By.Id("in")).Clear();
                webDriverWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
                driver.FindElement(By.Id("in")).SendKeys("Report Name");
                driver.FindElement(By.Id("subBTN")).Click();
                // ERROR: Caught exception [ERROR: Unsupported command [selectWindow | win_ser_1 | ]]
                driver.Close();
                // ERROR: Caught exception [ERROR: Unsupported command [selectWindow | win_ser_local | ]]
            }
            catch (NoSuchElementException exception)
            {
                throw;
            }
        }

        public static String GetTextFromWordDoc(string path)
        {
            //Open word document
            Document document = new Document();
            string docPath = path;

            document.LoadFromFile(docPath);

            //Save doc file.
            document.SaveToFile(docPath, FileFormat.Txt);

            var ExtractedText = File.ReadAllText(docPath);
            //File.Delete(docPath);
            return ExtractedText;
        }

        public static Func<IWebDriver, IWebElement> ElementIsClickable(By locator)
        {
            return driver =>
            {
                var element = driver.FindElement(locator);
                return (element != null && element.Displayed && element.Enabled) ? element : null;
            };
        }

        public static Func<IWebDriver, IWebElement> WaitUntilFrameLoadedAndSwitchToIt(By byToFindFrame)
        {
            return (driver) =>
            {
                try
                {
                    return (IWebElement)driver.SwitchTo().Frame(driver.FindElement(byToFindFrame));
                }
                catch (Exception)
                {
                    return null;
                }
            };
        }
    }
}