using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace YourLogo.Web.Driver.Drivers
{
    public interface IYourLogoDriver :  IDisposable
    {
        IWebDriver webdriver { get; set; }

        IWebElement FindElement(By by);

        IEnumerable<IWebElement> FindElements(By by);

        void Click(By by);

        void Click(IWebElement webelement);

        void Click(Action action);

        void SendKeys(By by, object text);

        void GoToURL(string url);

        void SelectByValue(By by, object text);

        void SelectByText(By by, object text);

        void SelectFromGroup(By by, object text);

        void WaitUntilDocIsReady();

        IWebElement WaitforElementTobeVisible(By @by);

        void TakeScreenshot(string filename);

        void Close();

    }
}
