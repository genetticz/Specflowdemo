using System;
using OpenQA.Selenium;

namespace SpecflowBddwNunit.Pages
{
    public class AutomationPage
    {
        private readonly IWebDriver driver;

        public AutomationPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        IWebElement priceLink => driver.FindElement(By.LinkText("Fake Pricing Page"));
        IWebElement formLink => driver.FindElement(By.LinkText("Fill out forms"));
        string title => driver.Title;

        public void GoToLink()
        {
            driver.Navigate().GoToUrl("https://ultimateqa.com/automation/");
        }

        public void ClickLink()
        {
            priceLink.Click();
        }

        public string PageTitle()
        {
            return title;
        }

        public void FormLink()
        {
            formLink.Click();
        }
    }
}
