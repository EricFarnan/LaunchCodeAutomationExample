using System;
using System.Diagnostics;
using TechTalk.SpecFlow;

namespace Automation.Project
{
    [Binding]
    public class Hooks : BaseFeature
    {
        public static string GetCurrentFeatureName()
        {
            return FeatureContext.Current.FeatureInfo.Title;
        }

        public static string GetCurrentScenarioName()
        {
            return ScenarioContext.Current.ScenarioInfo.Title;
        }

        [BeforeFeature]
        public static void WriteCurrentFeature()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.WriteLine("Testing feature: " + GetCurrentFeatureName());
        }

        [BeforeScenario]
        public static void WriteCurrentScenario()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.WriteLine("Testing Scenario: " + GetCurrentScenarioName());
        }

        [AfterScenario]
        public static void CloseDriverAfterTestPasses()
        {
            // Driver tear down after a scenario passes
            if (ScenarioContext.Current.TestError == null)
            {
                Driver.Quit();
                Driver.Dispose();
            }
        }
    }
}
