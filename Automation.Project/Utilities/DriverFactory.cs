using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Automation.Project.Utilities
{
    public class DriverFactory
    {

        public IWebDriver GetDriver()
        {
            var path = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = path.Replace("file:\\", "");
            path = path.Replace("file:/", "/");

            var driver = new OpenQA.Selenium.Chrome.ChromeDriver(path);
            driver.Manage().Window.Maximize();

            return driver;
        }
    
        public IWebElement FindElement(IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }
    }

    public static class DriverExtensions
    {
        private static int timeoutInSeconds = 20;

        public static IWebElement FindElementWithWait(this IWebDriver driver, By by)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }

        public static IWebElement WaitUntilElementIsClickable(this IWebDriver driver, By by)
        {
            if (timeoutInSeconds > 0)
            {

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                var elem = wait.Until(drv => drv.FindElement(by));

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));
            }
            return driver.FindElement(by);
        }

        public static void WaitUntilPageIsReady(this IWebDriver driver)
        {
            var OriginalSource = driver.PageSource;
            System.Threading.Thread.Sleep(1500);
            while (driver.PageSource != OriginalSource)
            {
                OriginalSource = driver.PageSource;
                System.Threading.Thread.Sleep(500);
            }

        }

    }
}
