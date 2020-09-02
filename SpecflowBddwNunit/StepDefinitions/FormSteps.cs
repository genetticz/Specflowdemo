using System;
using NUnit.Framework;
using OpenQA.Selenium;
using SpecflowBddwNunit.Pages;
using TechTalk.SpecFlow;

namespace SpecflowBddwNunit.StepDefinitions
{
    [Binding]
    public class FormSteps
    {
        private IWebDriver driver;
        private readonly AutomationPage automationpage;
        private readonly FormPage formpage;

        public FormSteps(IWebDriver driver)
        {
            this.driver = driver;
            automationpage = new AutomationPage(driver);
            formpage = new FormPage(driver);
        }
        [Given(@"I am at the ""(.*)"" endpoint")]
        public void GivenIAmAtTheEndpoint(string p0)
        {
            automationpage.GoToLink();
        }

        [Given(@"I have not entered anything in the form")]
        public void GivenIHaveNotEnteredAnythingInTheForm()
        {
            automationpage.FormLink();
        }

        [When(@"I click submit on the form")]
        public void WhenIClickSubmitOnTheForm()
        {
            formpage.FormClick();
        }

        [Then(@"I should see an error")]
        public void ThenIShouldSeeAnError()
        {
            string actual = formpage.FormErr();
            Assert.AreEqual("Please, fill in the following fields:", actual);
        }
    }
}
