using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace YourLogo.Web.Driver.Drivers
{
    public class ChromeYourLogoDriver : YourLogoDriver
    {
        private static int _pageNavigationTimeout = 10;

        public ChromeYourLogoDriver()
            : this(Environment.CurrentDirectory, _pageNavigationTimeout)
        {

        }
        public ChromeYourLogoDriver(int pageNavigationTimeout)
            : this(Environment.CurrentDirectory, pageNavigationTimeout)
        {
        }

        public ChromeYourLogoDriver(string filepath)
            : this(filepath, _pageNavigationTimeout)
        {
        }

        public ChromeYourLogoDriver(string filepath, int pageNavigationTimeout)
            : base(new ChromeDriver(filepath, Options()), pageNavigationTimeout)
        {
            _pageNavigationTimeout = pageNavigationTimeout;
        }

        private static ChromeOptions Options()
        {
            var options = new ChromeOptions();
            options.AddUserProfilePreference("download.prompt_for_download", true);
            options.AddArguments("no-sandbox");
            return options;
        }

        Func<IWebElement, SelectElement> selectElement = (element) =>
        {
            return new SelectElement(element);
        };


        public override void Click(By @by)
        {
            FindElement(@by).Click();
        }

        public override void Click(IWebElement webelement)
        {
            webelement.Click();
        }

        public override void Click(Action action)
        {
            action();
        }

        public override void SendKeys(By @by, object text)
        {
            var webelement = FindElement(@by);
            webelement.Clear();
            webelement.SendKeys(text.ToString());
        }

        public override void SelectByValue(By @by, object text)
        {
            var webelement = selectElement(webdriver.FindElement(@by));
            webelement.SelectByValue(text.ToString());
        }

        public override void SelectByText(By @by, object text)
        {
            var webelement = selectElement(webdriver.FindElement(@by));
            if (!(webelement.Options.Any(x => x.Text == text.ToString())))
            {
                text = webelement.Options.FirstOrDefault(x => !(string.IsNullOrEmpty(x.Text))).Text;
            }
            webelement.SelectByText(text.ToString());
        }

        public override void SelectFromGroup(By by, object text)
        {
            var webelement = FindElements(@by);
            IWebElement webelementtoSelectFromGroup;
            if (webelement.Any(x => x.GetAttribute("title") == text.ToString()))
            {
                webelementtoSelectFromGroup = webelement.FirstOrDefault(x => x.GetAttribute("title") == text.ToString());
            }
            else
            {
                webelementtoSelectFromGroup = webelement.Where(x => !(string.IsNullOrEmpty(x.GetAttribute("title")))).FirstOrDefault();
            }
            Click(webelementtoSelectFromGroup);
        }
    }
}
