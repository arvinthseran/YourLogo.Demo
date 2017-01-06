Feature: YourLogo
	In Order to deliver the product to the Stack Holders
	As an Test Automation Developer 
	I want to be sure that the Application Under Test is Regressively Tested


Scenario: Login successfully as a registered user
	Given I am in the authentication page
	When I login as a registered user
	Then Login is successful

Scenario: Register successfully as a new user
	Given I am in the authentication page
	When I register as a new user
	| Title | FirstName | LastName | Email                | Password | Date | Month | Year | Company | Address          | Address2            | City   | State   | PostalCode | Country       | HomePhone     | MobilePhone   | Reference       |
	| Mr    | New       | user     | newuser@yourlogo.com | passwd   | 20   | 01    | 1980 | Ten10   | The Hop Exchange | 24 Southwark Street | London | Alabama | 60098      | United States | 020 7100 7794 | 074 6789 7652 | My Comp Address |
	Then Registration is successful

Scenario: Login unsuccessfull
    Given I am in the authentication page
	When I login as unauthorised user
	Then Login is unsuccessful

Scenario: Add item to basket
	Given I am in the authentication page
	When I login as a registered user
	And I add Items to the basket 
	| Product                     | Quantity | Size | Color |
	| Faded Short Sleeve T-shirts | 1        | M    | Blue  |
	Then I can review the items from the basket

Scenario: Purchase Item
    Given I am in the authentication page
	When I login as a registered user
	And I add Items to the basket
	| Product                     | Quantity | Size | Color |
	| Faded Short Sleeve T-shirts | 1        | M    | Blue  |
	| Printed Dress               | 2        | M    | Pink  | 
	And I purchase Item from the basket
	Then Checkout is successful
