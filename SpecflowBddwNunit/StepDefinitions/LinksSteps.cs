using System;
using NUnit.Framework;
using OpenQA.Selenium;
using SpecflowBddwNunit.Pages;
using TechTalk.SpecFlow;

namespace SpecflowBddwNunit.StepDefinitions
{
    [Binding]
    public class LinksSteps
    {
        private IWebDriver driver;
        private readonly AutomationPage automationpage;

        public LinksSteps(IWebDriver driver)
        {
            this.driver = driver;
            automationpage = new AutomationPage(driver);
        }
        [Given(@"I am at the endpoint /automation")]
        public void GivenIAmAtTheEndpointAutomation()
        {
            automationpage.GoToLink();
        }

        [When(@"I click a link")]
        public void WhenIClickALink()
        {
            automationpage.ClickLink();
        }

        [Then(@"I should be redirected")]
        public void ThenIShouldBeRedirected()
        {
            string pageTitle = automationpage.PageTitle();
            Assert.AreEqual("Automation Practice - Ultimate QA", pageTitle);
        }
    }
}
