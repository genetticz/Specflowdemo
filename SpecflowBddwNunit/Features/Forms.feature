Feature: Forms
	
@mytag
Scenario: Attempt to submit an empty form
	Given I am at the "/automatio" endpoint
	And I have not entered anything in the form
	When I click submit on the form
	Then I should see an error
