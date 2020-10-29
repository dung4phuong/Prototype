Feature: create New User

@mytag
Scenario: Register new User to the website
	Given user landing on the home page
	And user start the signin button
	when enter user email for register
	when user start click on register button
	Then get to new user register page


	Scenario: Register Existing User to the website
	Given user landing on the home page
	And user start the signin button
	when enter user email for register
	when user start click on register button
	Then user get an error message
	