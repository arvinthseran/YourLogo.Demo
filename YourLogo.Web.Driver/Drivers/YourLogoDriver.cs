using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using System.Drawing.Imaging;
using OpenQA.Selenium.Support.UI;
using YourLogo.Web.Driver.ExceptionHandling;

namespace YourLogo.Web.Driver.Drivers
{
    public abstract class YourLogoDriver : IYourLogoDriver
    {
        public IWebDriver webdriver { get; set; }

        private readonly int _defaultNavigationTimeOutinSec;

        protected YourLogoDriver(IWebDriver driver, int timeout)
        {
            webdriver = driver;
            _defaultNavigationTimeOutinSec = timeout;
            StarttheDriver();
        }

        private void StarttheDriver()
        {
            GoToURL(@"http://automationpractice.com/index.php");
            webdriver.Manage().Window.Maximize();
            webdriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(_defaultNavigationTimeOutinSec));
        }

        public void GoToURL(string url)
        {
            webdriver.Navigate().GoToUrl(url);
        }

        public abstract void Click(By by);

        public abstract void Click(IWebElement webelement);

        public abstract void Click(Action action);

        public abstract void SendKeys(By by, object text);

        public abstract void SelectByValue(By by, object text);

        public abstract void SelectByText(By by, object text);

        public abstract void SelectFromGroup(By by, object text);

        public void Close()
        {
            webdriver.Close();
        }
        
        public void TakeScreenshot(string filename)
        {
            try
            {
                var shot = webdriver.TakeScreenshot();
                shot.SaveAsFile(filename, ImageFormat.Jpeg);
            }
            catch (Exception screenshotException)
            {
                throw new Exception(string.Format("{0} occured in TakeScreenshot", screenshotException.Message));
            }
        }
        
        public IWebElement FindElement(By @by)
        {
            return WebelementException.HandleStaleElementException(() => webdriver.FindElement(@by));
        }

        public IEnumerable<IWebElement> FindElements(By @by)
        {
            return WebelementException.HandleStaleElementException(() => webdriver.FindElements(@by));
        }

        public IWebElement WaitforElementTobeVisible(By @by)
        {
            return (new WebDriverWait(webdriver, TimeSpan.FromSeconds(_defaultNavigationTimeOutinSec))).Until(ExpectedConditions.ElementIsVisible(@by));
        }

        public void WaitUntilDocIsReady()
        {
            var wait = new WebDriverWait(webdriver, TimeSpan.FromSeconds(_defaultNavigationTimeOutinSec));
            wait.Until((webdriver) =>
               (webdriver as IJavaScriptExecutor).ExecuteScript("return document.readyState").Equals("complete")
            );
            return;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~YourLogoDriver() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
