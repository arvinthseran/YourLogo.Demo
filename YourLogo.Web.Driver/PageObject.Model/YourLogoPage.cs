using OpenQA.Selenium;
using System.Linq;
using System.Collections.Generic;
using YourLogo.Web.Driver.Drivers;
using System;
using OpenQA.Selenium.Interactions;
using System.Collections;

namespace YourLogo.Web.Driver.PageObject.Model
{
    public class YourLogoPage
    {
        private IYourLogoDriver _driver;

        public YourLogoPage(IYourLogoDriver driver)
        {
            this._driver = driver;
        }

        public void NaviagateToAuthPage()
        {
            _driver.Click(By.CssSelector("a.login"));
            VerifyNavigation(By.ClassName("page-heading"), "AUTHENTICATION");
            Console.WriteLine("Navigated to AUTHENTICATION page");
        }

        public void Login(string username, string password)
        {
             _driver.SendKeys(By.Id("email"), username);
            _driver.SendKeys(By.Id("passwd"), password);
            _driver.Click(By.Id("SubmitLogin"));
        }

        public bool CheckLogin()
        {
            return !(VerifyNavigation(By.TagName("li"), "Authentication failed."));
        }

        public bool CheckRegistration()
        {
            return (VerifyNavigation(By.ClassName("page-heading"), "MY ACCOUNT"));
        }

        public bool CheckCartSummary()
        {
            return VerifyNavigation(By.ClassName("page-heading"), "SHOPPING-CART SUMMARY");
        }
        public bool CheckOrderConfirmation()
        {
            return VerifyNavigation(By.ClassName("page-heading"), "ORDER CONFIRMATION");
        }

        private bool VerifyNavigation(By @by, string text)
        {
            _driver.WaitUntilDocIsReady();
            _driver.WaitforElementTobeVisible(@by);
            var page = _driver.FindElements(@by);
            return page.Count() == 0 ? false : page.Any(x => x.Text.ToLower().Contains(text.ToLower()));
        }

        public bool NavigateToCreateAnAccount(string username)
        {
            _driver.SendKeys(By.Id("email_create"), username);
            _driver.Click(By.Id("SubmitCreate"));
            return VerifyNavigation(By.ClassName("page-heading"), "CREATE AN ACCOUNT");
        }

        public void RegisterAsANewUser(dynamic userDetails)
        {
            var emailid = (string)userDetails.Email;
            string[] emailSplit = emailid.Split('@');
            var uniqueEmail = string.Format("{0}{1}@{2}", emailSplit[0], DateTime.Now.ToString("ddMMyyyyHHmm"), emailSplit[1]); 

            NavigateToCreateAnAccount(uniqueEmail);

            //Your Personal Information
            _driver.Click(By.Id("id_gender1"));
            _driver.SendKeys(By.Id("customer_firstname"), (string)userDetails.FirstName);
            _driver.SendKeys(By.Id("customer_lastname"), (string)userDetails.LastName);
            _driver.SendKeys(By.Id("passwd"), (string)userDetails.Password);
            _driver.SelectByValue(By.Id("days"), (int)userDetails.Date);
            _driver.SelectByValue(By.Id("months"), (int)userDetails.Month);
            _driver.SelectByValue(By.Id("years"), (int)userDetails.Year);

            //Your Address Details
            _driver.SendKeys(By.Id("firstname"), (string)userDetails.FirstName);
            _driver.SendKeys(By.Id("lastname"), (string)userDetails.LastName);
            _driver.SendKeys(By.Id("company"), (string)userDetails.Company);
            _driver.SendKeys(By.Id("address1"), (string)userDetails.Address);
            _driver.SendKeys(By.Id("address2"), (string)userDetails.Address2);
            _driver.SendKeys(By.Id("city"), (string)userDetails.City);
            _driver.SelectByText(By.Id("id_state"), (string)userDetails.State);
            _driver.SendKeys(By.Id("postcode"), (int)userDetails.PostalCode);
            _driver.SelectByText(By.Id("id_country"), (string)userDetails.Country);
            _driver.SendKeys(By.Id("phone"), (string)userDetails.HomePhone);
            _driver.SendKeys(By.Id("phone_mobile"), (string)userDetails.MobilePhone);
            _driver.SendKeys(By.Id("alias"), (string)userDetails.Reference);

            //Submit the details
            Console.WriteLine("Registering User '{0}'", uniqueEmail);
            _driver.Click(By.Id("submitAccount"));
            
        }

        public void AddItemsToCart(dynamic itemDetails)
        {
            _driver.SendKeys(By.Id("search_query_top"), (string)itemDetails.Product);
            _driver.Click(By.Name("submit_search"));
            VerifyNavigation(By.ClassName("heading-counter"), "results have been found");
            _driver.SelectFromGroup(By.CssSelector("a.product-name"), (string)itemDetails.Product);
            _driver.SendKeys(By.Id("quantity_wanted"), (int)itemDetails.Quantity);
            _driver.SelectByText(By.Id("group_1"), (string)itemDetails.Size);
            _driver.SelectFromGroup(By.CssSelector("a.color_pick"), (string)itemDetails.Color);
            _driver.Click(By.Name("Submit"));
            
            //Check for dialog box message
            VerifyNavigation(By.ClassName("icon-ok"), "Product successfully added to your shopping cart");
            _driver.Click(By.CssSelector("span.cross"));
            VerifyNavigation(By.TagName("h1"), (string)itemDetails.Product);
        }

        public void ClickCheckOut()
        {
            Func<IEnumerable<IWebElement>> carts = () => _driver.FindElements(By.CssSelector("div.shopping_cart a"));

            ((IJavaScriptExecutor)_driver.webdriver).ExecuteScript("window.scrollBy(0,-250);");
            _driver.Click(By.CssSelector("img.logo.img-responsive"));
            var cart = carts().FirstOrDefault(x => x.Text.Contains("Cart"));
            _driver.Click(() =>
                {
                    Actions action = new Actions(_driver.webdriver);
                    action.MoveToElement(carts().FirstOrDefault(x => x.Text.Contains("Cart")))
                           .Click()
                           .Build()
                           .Perform();
                    action.MoveToElement(carts().FirstOrDefault(x => x.GetAttribute("title") == "Check out"))
                          .Click()
                          .Build()
                          .Perform();
                }
            );       
        }

        public void PurchaseItemsInTheCart()
        {
            _driver.Click(By.CssSelector("a.button.standard-checkout"));
            VerifyNavigation(By.ClassName("page-heading"), "ADDRESSES");
            
            _driver.Click(By.CssSelector("button.btn.btn-default.button-medium"));
            VerifyNavigation(By.ClassName("page-heading"), "SHIPPING");

            _driver.Click(By.Id("cgv"));
            _driver.Click(By.CssSelector("button.standard-checkout"));
            
            _driver.Click(By.CssSelector("a.cheque"));
            VerifyNavigation(By.ClassName("page-heading"), "ORDER SUMMARY");

            _driver.Click(By.CssSelector("button.btn.btn-default.button-medium"));
        }
    }
}
