using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebPages;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ProjectEvalutionSystem.Models;

namespace ProjectEvalutionSystem.Helper
{
    public class CheckPlagiarism
    {
        public static ChromeDriver _driver;
        public static CheckPlagiarismResponse StartProcess(string text,int evalutionIndexId, string browserDriverPath)
        {
            var options = new ChromeOptions();
            options.AddArguments("--headless", "--disable-gpu", "--window-size=1920,1200", "--ignore-certificate-errors", "--disable-extensions", "--no-sandbox", "--disable-dev-shm-usage");

            _driver = new ChromeDriver(browserDriverPath, options);

            string PlagCount = string.Empty;
            string UniqueCount = string.Empty;
            string matchesText = string.Empty;
            string regexMatchesUrls = string.Empty;

            
            

            _driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["PlagiarismCheckerURL"].ToString());
            _driver.Manage().Window.Maximize();

            var textArea = _driver.FindElement(By.Id("textBox"));

            if (textArea.Displayed)
            {
                textArea.Clear();
                textArea.SendKeys(text);
                _driver.ExecuteScript("onSubmit()");
            }

            Thread.Sleep(5000);

            var uniqueCount = _driver.FindElement(By.Id("uniqueCount"));
            if (uniqueCount.Displayed == true)
            {
                UniqueCount = uniqueCount.Text;
            }

            var plagCount = _driver.FindElement(By.Id("plagCount"));
            if (plagCount.Displayed == true)
            {
                PlagCount = plagCount.Text;
            }


            var MatchUrls = _driver.FindElement(By.XPath("//*[@id='mainResultsDisplay']/div[2]/div[2]/ul/li[2]/a"));
            if (MatchUrls.Displayed == true)
            {
                MatchUrls.Click();
                Thread.Sleep(1000);
                var matchesTableDiv = _driver.FindElement(By.Id("matches"));
                if (matchesTableDiv.Displayed == true)
                {
                    var matcheTable = _driver.FindElement(By.ClassName("match_table"));
                    if (matchesTableDiv.Displayed == true)
                    {
                        matchesText = matchesTableDiv.Text;
                        var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        foreach (Match m in linkParser.Matches(matchesText))
                        {
                            regexMatchesUrls = regexMatchesUrls + ", " + m;
                        }
                    }
                    else
                    {
                        matchesText = matcheTable.Text;
                    }
                }
            }


            using (ProjectEvalutionSystemEntities _context = new ProjectEvalutionSystemEntities())
            {
                var evalutionIndex = _context.EvalutionIndexes.Find(evalutionIndexId);
                evalutionIndex.Remarks = Convert.ToInt32(PlagCount) == 0 ? "Passed with 0% Plagiarism." : "Failed with " + Convert.ToInt32(PlagCount) + "% Plagiarism";
                evalutionIndex.Comments = matchesText != "" ? "No sources found against your content.." : matchesText;
                evalutionIndex.IsCompleted = true;
                evalutionIndex.PlagCount = PlagCount;
                evalutionIndex.UniqueCount = UniqueCount;
                evalutionIndex.MatchesUrls = regexMatchesUrls;

                _context.Entry(evalutionIndex).State = EntityState.Modified;
                _context.SaveChanges();
            }


            _driver.Close();

            return new CheckPlagiarismResponse
            {
                UniqueCount = UniqueCount + "%",
                PlagCount = PlagCount + "%",
                matchingUrls = regexMatchesUrls.Split(',')
            };
        }
    }

    public class CheckPlagiarismResponse
    {
        public string UniqueCount { get; set; }
        public string PlagCount { get; set; }
        public string[] matchingUrls  { get; set; }
    }
}