//using System;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.Firefox;
//using OpenQA.Selenium.Support.UI;

//namespace ProjectEvalutionSystem
//{
//    public class UntitledTestCase
//    {
//        public void ProcessStart()
//        {
//            driver.Navigate().GoToUrl("https://www.plagiarismchecker.co/");
//            driver.FindElement(By.Id("textBoxMain")).Click();
//            // ERROR: Caught exception [ERROR: Unsupported command [selectFrame | index=0 | ]]
//            driver.FindElement(By.XPath("//span[@id='recaptcha-anchor']/div")).Click();
//            // ERROR: Caught exception [ERROR: Unsupported command [selectFrame | relative=parent | ]]
//            driver.FindElement(By.Id("textBoxMain")).Clear();
//            driver.FindElement(By.Id("textBoxMain")).SendKeys("");
//            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
//            driver.FindElement(By.XPath("//section[@id='progressBarMain']/div/div/div/div[2]/div[2]/div/span/span[2]")).Click();
//            driver.FindElement(By.XPath("//section[@id='progressBarMain']/div/div/div/div[2]/div[2]/div/span[2]/span[2]")).Click();
//            driver.FindElement(By.XPath("//div[@id='generate_report']/button[3]")).Click();
//            driver.FindElement(By.Id("in")).Click();
//            driver.FindElement(By.Id("in")).Clear();
//            driver.FindElement(By.Id("in")).SendKeys("Report Name");
//            driver.FindElement(By.Id("subBTN")).Click();
//            // ERROR: Caught exception [ERROR: Unsupported command [selectWindow | win_ser_1 | ]]
//            driver.Close();
//            // ERROR: Caught exception [ERROR: Unsupported command [selectWindow | win_ser_local | ]]
//        }
//        private bool IsElementPresent(By by)
//        {
//            try
//            {
//                driver.FindElement(by);
//                return true;
//            }
//            catch (NoSuchElementException)
//            {
//                return false;
//            }
//        }

//        private bool IsAlertPresent()
//        {
//            try
//            {
//                driver.SwitchTo().Alert();
//                return true;
//            }
//            catch (NoAlertPresentException)
//            {
//                return false;
//            }
//        }

//        private string CloseAlertAndGetItsText()
//        {
//            try
//            {
//                IAlert alert = driver.SwitchTo().Alert();
//                string alertText = alert.Text;
//                if (acceptNextAlert)
//                {
//                    alert.Accept();
//                }
//                else
//                {
//                    alert.Dismiss();
//                }
//                return alertText;
//            }
//            finally
//            {
//                acceptNextAlert = true;
//            }
//        }
//    }
//}
