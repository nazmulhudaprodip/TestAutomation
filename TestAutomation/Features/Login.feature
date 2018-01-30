Feature: Login
	In order to login to system
	As a user
	I want to logged into system

@mytag
Scenario: Login to Customer portal
	Given I have valid credentials
	When I press signin
	Then I will be logged in to customer 
