using System;
using OpenQA.Selenium;

namespace SpecflowBddwNunit.Pages
{
    public class FormPage
    {
        private readonly IWebDriver driver;

        public FormPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        IWebElement formSubmit => driver.FindElement(By.Name("et_builder_submit_button"));
        string formError => driver.FindElement(By.CssSelector(".et-pb-contact-message > p")).Text;

        public void FormClick()
        {
            formSubmit.Click();
        }
        public string FormErr()
        {
            return formError;
        }
    }
}
