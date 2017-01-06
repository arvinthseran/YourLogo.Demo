using BoDi;
using TechTalk.SpecFlow;
using NUnit.Framework;
using YourLogo.Web.Driver.Drivers;
using YourLogo.Web.Driver.PageObject.Model;
using TechTalk.SpecFlow.Assist;
using System.Collections.Generic;
using System.Linq;

namespace YourLogo.Web.Test.Steps.Bindings
{
    [Binding]
    public class StepBindings
    {
        private IYourLogoDriver _driver;

        private readonly IObjectContainer _objectContainer;

        private YourLogoPage yourlogoPage;

        public StepBindings(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
            _driver = _objectContainer.Resolve<IYourLogoDriver>();
            yourlogoPage = new YourLogoPage(_driver);
        }

        [Given(@"I am in the authentication page")]
        public void GivenIAmInTheAuthenticationPage()
        {
            yourlogoPage.NaviagateToAuthPage();
        }

        [When(@"I login as a registered user")]
        public void WhenILoginAsARegisteredUser()
        {
            yourlogoPage.Login("newuser@yourlogo.com", "passwd");
        }

        [Then(@"Login is successful")]
        public void ThenLoginIsSuccessful()
        {
            Assert.True(yourlogoPage.CheckLogin(), "Login is sucessful");
        }

        [When(@"I register as a new user")]
        public void WhenIRegisterAsANewUser(Table table)
        {
            dynamic newUserDetails = table.CreateDynamicInstance();
            yourlogoPage.RegisterAsANewUser(newUserDetails);
        }

        [Then(@"Registration is successful")]
        public void ThenRegistrationIsSuccessful()
        {
            Assert.True(yourlogoPage.CheckRegistration(),"Registration is not sucessful");
        }

        [When(@"I login as unauthorised user")]
        public void WhenILoginAsUnauthorisedUser()
        {
            yourlogoPage.Login("User1@yourlogo.com", "passwd");
        }

        [Then(@"Login is unsuccessful")]
        public void ThenLoginIsUnsuccessful()
        {
            Assert.False(yourlogoPage.CheckLogin(), "Login is sucessful");
        }

        [When(@"I add Items to the basket")]
        public void WhenIAddItemsToTheBasket(Table table)
        {
            List<dynamic> cartItems = table.CreateDynamicSet().ToList();
            cartItems.ForEach(x => yourlogoPage.AddItemsToCart(x));
        }

        [Then(@"I can review the items from the basket")]
        public void ThenICanReviewTheItemsFromTheBasket()
        {
            yourlogoPage.ClickCheckOut();
            Assert.True(yourlogoPage.CheckCartSummary(), "We have not arrived at Shopping - cart summary page");
        }

        [When(@"I purchase Item from the basket")]
        public void WhenIPurchaseItemFromTheBasket()
        {
            yourlogoPage.ClickCheckOut();
            yourlogoPage.PurchaseItemsInTheCart();
        }

        [Then(@"Checkout is successful")]
        public void ThenCheckoutIsSucessful()
        {
            Assert.True(yourlogoPage.CheckOrderConfirmation(), "We are not arrved at Order Confirmation page");
        }
    }
}
