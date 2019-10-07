using OpenQA.Selenium;
using Automation.Project.Utilities;
using TechTalk.SpecFlow;

namespace Automation.Project
{
    public class BaseFeature
    {
        public DriverFactory DriverFactory = new DriverFactory();
        public static IWebDriver Driver
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentDriver"))
                {
                    DriverFactory factory = new DriverFactory();
                    var driver = factory.GetDriver();

                    ScenarioContext.Current.Add("CurrentDriver", driver);
                }
                return ScenarioContext.Current.Get<IWebDriver>("CurrentDriver");
            }
        }
    }
}
