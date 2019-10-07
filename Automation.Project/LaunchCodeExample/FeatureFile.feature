Feature: LaunchCode Web Application Basics

Scenario: 001 LaunchCode: Sign-Up
	Given I am on the LaunchCode website
	When I create a new LaunchCode account
	Then the apprenticeship dashboard is present
	Then I logout of my LaunchCode account

Scenario: 002 LaunchCode: Sign-In and Account Foundation Information Update
	Given I am on the LaunchCode website
	Given I am logged in using the email "oluwatimileyin.ifeanyichukwu@thtt.us" and password "Test1234"
	And I update my basic account information using the following:
		| Zipcode | Race  | Gender | Education   |
		| 12345   | Other | Man    | Associate's |
	And I update my start point to "foundations" with the following information:
		| High School Diploma | Basic Math Skills | Basic Computer Skills | Laptop Ownership |
		| Yes                 | Yes               | No                    | Yes              |
	Then the learning journey fundamentals section has "3" completed stars

Scenario: 003 LaunchCode: Account Password Reset
	Given I am on the LaunchCode website
	When I create a new LaunchCode account
	And I logout of my LaunchCode account
	And I reset my LaunchCode account password
	Then I am unable to login using the old password
	Then I am able to login using the new password