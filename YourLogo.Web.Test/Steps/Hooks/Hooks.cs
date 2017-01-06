
using BoDi;
using System;
using TechTalk.SpecFlow;
using YourLogo.Web.Driver;
using YourLogo.Web.Driver.Drivers;

namespace YourLogo.Web.Test.Steps.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _objectContainer;
        public IYourLogoDriver yourlogodriver;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            yourlogodriver = new ChromeYourLogoDriver(AppDomain.CurrentDomain.BaseDirectory);
            _objectContainer.RegisterInstanceAs<IYourLogoDriver>(yourlogodriver);
        }
        [AfterScenario]
        public void AfterScenario()
        {
            // Take Screenshot on failure and Dispose the driver
            try
            {
                //if (ScenarioContext.Current.TestError != null)
                //{
                    yourlogodriver.TakeScreenshot(String.Format("{0}-{1}-{2}.jpeg", "C:\\", ScenarioContext.Current.ScenarioInfo.Title.Replace(" ", ""), DateTime.Now.ToString("ddMMyyyyHHmm")));
                //}
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
