using OpenQA.Selenium;
using Automation.Project.Utilities;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace Automation.Project.Example
{
    [Binding]
    public class TestFeatureSteps : BaseFeature
    {
        [Given(@"I am on the LaunchCode website")]
        public void GivenIAmOnTheLaunchCodeWebsite()
        {
            // Navigate to the LaunchCode website
            Driver.Navigate().GoToUrl("https://launchcode.org");
            Driver.WaitUntilPageIsReady();
        }

        [When(@"I create a new LaunchCode account")]
        public void WhenICreateANewLaunchCodeAccount()
        {
            // Open a new browser window and switch to the mail service
            ((IJavaScriptExecutor)Driver).ExecuteScript("window.open()");
            var launchCodeWindow = Driver.WindowHandles[0];
            var emailWindow = Driver.WindowHandles[1];
            Driver.SwitchTo().Window(emailWindow);
            Driver.Navigate().GoToUrl("https://www.tempmailaddress.com/");

            // Reset the email and assign the new email name to a variable
            Driver.FindElements(By.CssSelector("a[href=\"/delete\"]"))[1].Click();
            var randomEmailName = Driver.FindElementWithWait(By.CssSelector("span[id=\"email\"]")).Text;

            // Switch windows back to LaunchCode
            Driver.SwitchTo().Window(launchCodeWindow);
            Driver.WaitUntilPageIsReady();
            
            // Go through the sign-up process using the email
            Driver.FindElement(By.CssSelector("a[href=\"/login\"]")).Click();
            Driver.WaitUntilPageIsReady();
            Driver.FindElement(By.CssSelector("a[href=\"/register\"]")).Click();
            Driver.WaitUntilPageIsReady();

            // Enter the new account credentials and sign-up
            Driver.FindElement(By.CssSelector("input[id=\"applicant_first_name\"]")).SendKeys("Test First Name");
            Driver.FindElement(By.CssSelector("input[id=\"applicant_last_name\"]")).SendKeys("Test Last Name");
            Driver.FindElement(By.CssSelector("input[id=\"applicant_email\"]")).SendKeys(randomEmailName);
            Driver.FindElement(By.CssSelector("input[id=\"applicant_email_confirmation\"]")).SendKeys(randomEmailName);
            Driver.FindElement(By.CssSelector("input[id=\"applicant_password\"]")).SendKeys("Test1234");
            Driver.FindElement(By.CssSelector("input[id=\"applicant_password_confirmation\"]")).SendKeys("Test1234");
            Driver.FindElement(By.CssSelector("input[id=\"applicant_legal_terms\"]")).Click();
            Driver.FindElement(By.CssSelector("input[id=\"applicant-signup-btn\"]")).Click();
            Driver.WaitUntilPageIsReady();
        }

        [Then(@"the apprenticeship dashboard is present")]
        public void ThenTheApprenticeshipDashboardIsPresent()
        {
            // Assert the sign-up lands the user onto their dashboard page
            Assert.AreEqual("https://www.launchcode.org/applicants/dashboard", Driver.Url);

            // Dismiss the alert
            Driver.FindElement(By.CssSelector("a[data-dismiss=\"alert\"]")).Click();
            Driver.WaitUntilPageIsReady();

            // Confirm the top navbar contains the LaunchCode icon, dashboard display, applicant name display, and logout display
            Driver.FindElement(By.CssSelector("a.navbar-brand.dabomb"));
            var topNavBar = Driver.FindElement(By.CssSelector("div.collapse.navbar-collapse"));
            Assert.AreEqual("DASHBOARD", topNavBar.FindElement(By.CssSelector("a[href=\"/applicants/dashboard\"]")).Text);
            Assert.AreEqual("TEST FIRST NAME TEST LAST NAME", topNavBar.FindElement(By.CssSelector("a[id=\"applicant-nav-account\"]")).Text);
            topNavBar.FindElement(By.CssSelector("input[value=\"Logout\"]"));

            // Confirm the sidebar navigation widgets are present
            var sideMenuWidget = Driver.FindElement(By.CssSelector("div[class=\"applicant-side-menu\"]"));
            Assert.AreEqual("APPRENTICESHIP", sideMenuWidget.FindElement(By.CssSelector("a[href=\"/applicants/apprenticeships/dashboard\"]")).Text);
            Assert.AreEqual("MY LEARNING JOURNEY", sideMenuWidget.FindElement(By.CssSelector("a[href=\"/pathways/parachute\"]")).Text);
            Assert.AreEqual("CLASSES", sideMenuWidget.FindElement(By.CssSelector("a[href=\"/learners/dashboard\"]")).Text);

            // Confirm the dashboard widgets are present
            // 'My Classes' widget
            var myClassesWidget = Driver.FindElement(By.CssSelector("div[id=\"my-classes-panel\"]"));
            Assert.AreEqual("MY CLASSES", myClassesWidget.FindElement(By.CssSelector("div.header-bar")).Text);
            Assert.AreEqual("You have not registered for any classes.\r\nBROWSE CLASSES", myClassesWidget.FindElement(By.CssSelector("div.dashboard-content")).Text);
            Assert.AreEqual("BROWSE CLASSES", myClassesWidget.FindElement(By.CssSelector("a[href=\"/learn\"]")).Text);

            // 'My Learning Journey' widget
            var myJourneyWidget = Driver.FindElements(By.CssSelector("div[class=\"dashboard-panel margin-bottom-30\"]"))[1];
            Assert.AreEqual("MY LEARNING JOURNEY", myJourneyWidget.FindElement(By.CssSelector("div.header-bar")).Text);
            Assert.AreEqual("START YOUR JOURNEY", myJourneyWidget.FindElement(By.CssSelector("a[href=\"/pathways/parachute\"]")).Text);

            // 'My Apprenticeship' widget
            var myApprenticeshipWidget = Driver.FindElement(By.CssSelector("div[id=\"my-apprenticeship-panel\"]"));
            Assert.AreEqual("MY APPRENTICESHIP", myApprenticeshipWidget.FindElement(By.CssSelector("div.header-bar")).Text);
            Assert.AreEqual("You haven't applied for an apprenticeship yet. Ready to get started?\r\nAPPLY NOW", myApprenticeshipWidget.FindElement(By.CssSelector("div.status-row")).Text);
            Assert.AreEqual("APPLY NOW", myApprenticeshipWidget.FindElement(By.CssSelector("a[href=\"/candidates/applications/new\"]")).Text);
        }

        [When(@"I logout of my LaunchCode account")]
        [Then(@"I logout of my LaunchCode account")]
        public void ThenILogoutOfMyLaunchCodeAccount()
        {
            // Click the 'logout' button
            Driver.FindElement(By.CssSelector("input[value=\"Logout\"]")).Click();
            Driver.WaitUntilPageIsReady();

            // Confirm the URL changes to the default LaunchCode landing page
            Assert.AreEqual("https://www.launchcode.org/", Driver.Url);

            // Confirm the login link is present
            Driver.FindElement(By.CssSelector("a[href=\"/login\"]"));
        }

        [Given(@"I am logged in using the email ""(.*)"" and password ""(.*)""")]
        public void GivenIAmLoggedInUsingTheEmailAndPassword(string loginEmail, string loginPassword)
        {
            // Go through the login process using the email
            Driver.FindElement(By.CssSelector("a[href=\"/login\"]")).Click();
            Driver.WaitUntilPageIsReady();
            Driver.FindElement(By.CssSelector("input[id=\"applicant_email\"]")).SendKeys(loginEmail);
            Driver.FindElement(By.CssSelector("input[id=\"applicant_password\"]")).SendKeys(loginPassword);
            Driver.FindElement(By.CssSelector("button[id=\"applicant-login-btn\"]")).Click();
            Driver.WaitUntilPageIsReady();
        }

        [Given(@"I update my basic account information using the following:")]
        public void GivenIUpdateMyBasicAccountInformationUsingTheFollowing(Table accountInformation)
        {
            // Navigate to the account information page
            Driver.FindElements(By.CssSelector("a[href=\"/pathways/parachute\"]"))[1].Click();
            Driver.WaitUntilPageIsReady();

            // Convert the gherkin account information table into a DT
            var accountInformationDT = TableHandling.ConvertGherkinTableToDataTable(accountInformation);

            // Retrieve all the data from the account information DT into variables in a list format
            var zipcode = TableHandling.getDataTableColumnValues(accountInformationDT, "Zipcode");
            var race = TableHandling.getDataTableColumnValues(accountInformationDT, "Race");
            var gender = TableHandling.getDataTableColumnValues(accountInformationDT, "Gender");
            var education = TableHandling.getDataTableColumnValues(accountInformationDT, "Education");

            // Enter the zipcode information
            Driver.FindElement(By.CssSelector("input[id=\"pathways_parachutes_seeker_info_zipcode\"]")).SendKeys(zipcode[0]);

            // Enter the race information
            var raceSelector = Driver.FindElement(By.CssSelector("select[id=\"pathways_parachutes_seeker_info_race\"]"));
            raceSelector.Click();
            raceSelector.FindElement(By.CssSelector($"option[value=\"{race[0]}\"]")).Click();
            raceSelector.Click();

            // Enter the gender information
            var genderSelector = Driver.FindElement(By.CssSelector("select[id=\"pathways_parachutes_seeker_info_gender\"]"));
            genderSelector.Click();
            genderSelector.FindElement(By.CssSelector($"option[value=\"{gender[0]}\"]")).Click();
            genderSelector.Click();

            // Enter the education information
            var educationSelector = Driver.FindElement(By.CssSelector("select[id=\"pathways_parachutes_seeker_info_education\"]"));
            educationSelector.Click();
            educationSelector.FindElement(By.CssSelector($"option[value=\"{education[0]}\"]")).Click();
            educationSelector.Click();

            // Continue
            Driver.FindElement(By.CssSelector("input[value=\"Continue\"]")).Click();
            Driver.WaitUntilPageIsReady();          
        }

        [Given(@"I update my start point to ""(.*)"" with the following information:")]
        public void GivenIUpdateMyStartPointToWithTheFollowingInformation(string startPoint, Table startPointInformation)
        {
            var startPointInformatonDT = TableHandling.ConvertGherkinTableToDataTable(startPointInformation);

            // Select start point
            switch (startPoint)
            {
                case "foundations":
                    {
                        // Select the foundations level
                        Driver.FindElement(By.CssSelector("button[value=\"foundations\"]")).Click();
                        Driver.WaitUntilPageIsReady();

                        var diploma = TableHandling.getDataTableColumnValues(startPointInformatonDT, "High School Diploma");
                        var math = TableHandling.getDataTableColumnValues(startPointInformatonDT, "Basic Math Skills");
                        var computerSkills = TableHandling.getDataTableColumnValues(startPointInformatonDT, "Basic Computer Skills");
                        var laptopOwnership = TableHandling.getDataTableColumnValues(startPointInformatonDT, "Laptop Ownership");

                        // Select the basic foundations options
                        var checkBoxes = Driver.FindElements(By.CssSelector("input[id=\"completed_milestone_ids_\"]"));
                        var diplomaCheckBox = checkBoxes[0];
                        var mathCheckBox = checkBoxes[1];
                        var computerSkillsCheckBox = checkBoxes[2];
                        var laptopOwnershipCheckBox = checkBoxes[3];

                        if (diploma[0] == "Yes")
                        {
                            diplomaCheckBox.Click();
                        }
                        if (math[0] == "Yes")
                        {
                            mathCheckBox.Click();
                        }
                        if (computerSkills[0] == "Yes")
                        {
                            computerSkillsCheckBox.Click();
                        }
                        if (laptopOwnership[0] == "Yes")
                        {
                            laptopOwnershipCheckBox.Click();
                        }

                        break;
                    }
                case "experience":
                    {
                        // etc.
                        break;
                    }
                case "apply":
                    {
                        // etc.
                        break;
                    }
                case "default":
                    {
                        // etc.
                        break;
                    }

            }

            Driver.FindElement(By.CssSelector("input[value=\"Continue\"]")).Click();
            Driver.WaitUntilPageIsReady();
            Assert.AreEqual("https://www.launchcode.org/pathways/seekers/dashboard", Driver.Url);
        }

        [Then(@"the learning journey fundamentals section has ""(.*)"" completed stars")]
        public void ThenTheLearningJourneyFundamentalsSectionHasCompletedStars(int fundamentalsStarAmount)
        {
            // Target the fundamentals section and confirm the completed star amount
            var fundamentalsSection = Driver.FindElements(By.CssSelector("div.col-md-6"))[0];
            Assert.AreEqual("FUNDAMENTALS", fundamentalsSection.Text);
            var actualStarAmount = fundamentalsSection.FindElements(By.CssSelector("i[class=\"fa fa-star fa-2x stars-completed\"]"));
            Assert.AreEqual(fundamentalsStarAmount, actualStarAmount.Count);
        }

        [When(@"I reset my LaunchCode account password")]
        public void WhenIResetMyLaunchCodeAccountPassword()
        {
            // Assign the browser windows
            var launchCodeWindow = Driver.WindowHandles[0];
            var emailWindow = Driver.WindowHandles[1];

            // Grab the account email
            Driver.SwitchTo().Window(emailWindow);
            var randomEmailName = Driver.FindElementWithWait(By.CssSelector("span[id=\"email\"]")).Text;

            // Start the password reset process
            Driver.SwitchTo().Window(launchCodeWindow);
            Driver.FindElement(By.CssSelector("a[href=\"/login\"]")).Click();
            Driver.WaitUntilPageIsReady();
            Driver.FindElement(By.CssSelector("a[href=\"/applicants/password/new\"]")).Click();
            Driver.WaitUntilPageIsReady();
            Driver.FindElement(By.CssSelector("input[id=\"applicant_email\"]")).SendKeys(randomEmailName);
            Driver.FindElement(By.CssSelector("input[value=\"Reset password\"]")).Click();

            // Confirm the password reset process via email
            Driver.SwitchTo().Window(emailWindow);
            Driver.Navigate().GoToUrl("https://www.tempmailaddress.com/window/id/2");
            Driver.WaitUntilPageIsReady();
            Driver.SwitchTo().Frame(Driver.FindElementWithWait(By.CssSelector("iframe[id=\"iframeMail\"]")));
            Driver.WaitUntilElementIsClickable(By.LinkText("Change my password")).Click();

            // Change the password
            var emailResetWindow = Driver.WindowHandles[2];
            Driver.SwitchTo().Window(emailResetWindow);
            Driver.WaitUntilPageIsReady();
            Driver.FindElement(By.CssSelector("input[id=\"applicant_password\"]")).SendKeys("Test4321");
            Driver.FindElement(By.CssSelector("input[id=\"applicant_password_confirmation\"]")).SendKeys("Test4321");
            Driver.FindElement(By.CssSelector("input[value=\"Change my password\"]")).Click();
            Driver.WaitUntilPageIsReady();

            // Confirm the password changed alert appears
            var alert = Driver.FindElement(By.CssSelector("div[class=\"alert alert-info\"]"));
        }
    }
}
