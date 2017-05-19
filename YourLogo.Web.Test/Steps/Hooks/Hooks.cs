
using BoDi;
using System;
using System.IO;
using System.Reflection;
using TechTalk.SpecFlow;
using YourLogo.Web.Driver.Drivers;

namespace YourLogo.Web.Test.Steps.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _objectContainer;
        private string _assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public IYourLogoDriver yourlogodriver;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            yourlogodriver = new ChromeYourLogoDriver(_assemblyFolder);
            _objectContainer.RegisterInstanceAs(yourlogodriver);
        }
        [AfterScenario]
        public void AfterScenario()
        {
            // Take Screenshot on failure and Dispose the driver
            try
            {
                    yourlogodriver.TakeScreenshot(String.Format("{0}-{1}-{2}.jpeg", "C:\\", ScenarioContext.Current.ScenarioInfo.Title.Replace(" ", ""), DateTime.Now.ToString("ddMMyyyyHHmm")));
            }
            catch (Exception)
            {
                // Ignore exceptions thrown by screenshot - as we NEED to dispose driver in any circumstance.
            }
            finally
            {
                yourlogodriver.Close();
                yourlogodriver.Dispose();
            }
        }

    }
}
