Feature: Links to other pages
	
	
@mytag
Scenario: Pressing a link and being redirected
	Given I am at the endpoint /automation
	When I click a link
	Then I should be redirected