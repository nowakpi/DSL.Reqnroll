@Integration
Feature: Examples of how to use functions

Scenario: I use TODAY function without parameters
	When entered string "Just some text to be able to include When step"
	Then verify string "{{TODAY-6M|MM-dd-yyyy HH:mm}} AH!" equals "10-02-2024 19:49 AH!"

Scenario: I use TODAY function without parameters as a variable
	When entered string "[[TODAY={{TODAY}}]]"
	Then verify string "[[TODAY]]" equals "{{TODAY}}"