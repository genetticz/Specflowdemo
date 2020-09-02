using System;
using System.IO;
using System.Reflection;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace SpecflowBddwNunit.StepDefinitions
{
    [Binding]
    public class Hooks
    {
        //Global Variable for Extent report
        private static ExtentTest featureName;
        private static ExtentTest scenario;
        private static ExtentReports extent;

        private IWebDriver driver = null;
        private readonly IObjectContainer objectContainer;
        private readonly ScenarioContext scenariocontext;
        //FeatureContext featurecontext;



        public Hooks(IObjectContainer objectContainer, ScenarioContext scenariocontext)
        {
            this.objectContainer = objectContainer;
            this.scenariocontext = scenariocontext;
            //this.featurecontext = featurecontext;
        }

        [BeforeTestRun]
        public static void InitReporting()
        {
            var htmlReporter = new ExtentHtmlReporter(@"/Users/michaelwitter/Projects/SpecflowBddwNunit/SpecflowBddwNunit/Report/ExtentReport.html");
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            //Attach report to reporter
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        [AfterTestRun]
        public static void TearDownReporting()
        {
            //Flush report once test completes
            extent.Flush();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featurecontext)
        {
            //Create dynamic feature name
            featureName = extent.CreateTest<Feature>(featurecontext.FeatureInfo.Title);
        }

        [AfterStep]
        public void InsertReportingSteps()
        {

            var stepType = scenariocontext.StepContext.StepInfo.StepDefinitionType.ToString();

            PropertyInfo pInfo = typeof(ScenarioContext).GetProperty("ScenarioExecutionStatus", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo getter = pInfo.GetGetMethod(nonPublic: true);
            object TestResult = getter.Invoke(scenariocontext, null);

            if (scenariocontext.TestError == null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(scenariocontext.StepContext.StepInfo.Text);
                else if (stepType == "When")
                    scenario.CreateNode<When>(scenariocontext.StepContext.StepInfo.Text);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(scenariocontext.StepContext.StepInfo.Text);
                else if (stepType == "And")
                    scenario.CreateNode<And>(scenariocontext.StepContext.StepInfo.Text);
            }
            else if (scenariocontext.TestError != null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(scenariocontext.StepContext.StepInfo.Text).Fail(scenariocontext.TestError.InnerException);
                else if (stepType == "When")
                    scenario.CreateNode<When>(scenariocontext.StepContext.StepInfo.Text).Fail(scenariocontext.TestError.InnerException);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(scenariocontext.StepContext.StepInfo.Text).Fail(scenariocontext.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(Capture(driver)).Build());
            }

            //Pending Status
            if (TestResult.ToString() == "StepDefinitionPending")
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(scenariocontext.StepContext.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "When")
                    scenario.CreateNode<When>(scenariocontext.StepContext.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(scenariocontext.StepContext.StepInfo.Text).Skip("Step Definition Pending");

            }

        }

        [BeforeScenario]
        public void Setup()
        {
            Console.WriteLine("this " + AppDomain.CurrentDomain.BaseDirectory);
            driver = new ChromeDriver(@"/Users/michaelwitter/Downloads");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(15);
            objectContainer.RegisterInstanceAs<IWebDriver>(driver);
            scenario = featureName.CreateNode<Scenario>(scenariocontext.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }

        public string Capture(IWebDriver driver)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            Screenshot screenshot = ts.GetScreenshot();
            string screenShotName = "screenShotName_" + DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss_fff");
            string workingDirectory = Environment.CurrentDirectory;
            string assemblyFolder = Directory.GetParent(workingDirectory).Parent.Parent.FullName;//Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string finalpth = assemblyFolder + @"/screenshots/";
            string screenshotDir = finalpth + screenShotName + ".png";
            string localpath = new Uri(screenshotDir).LocalPath;
            System.IO.Directory.CreateDirectory(finalpth);
            screenshot.SaveAsFile(localpath, ScreenshotImageFormat.Png);
            return localpath;
        }
    }
}
