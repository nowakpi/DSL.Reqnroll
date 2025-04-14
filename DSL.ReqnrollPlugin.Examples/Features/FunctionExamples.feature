@Integration
Feature: Examples of how to use functions

#@Ignored
#Scenario: I use TODAY function without parameters
#	When entered string "Just some text to be able to include When step"
#	Then verify string "{{L#TODAY-6M#dd-MM-yyyy HH}} AH!" equals "14-10-2024 21 AH!"

Scenario: I use TODAY function without parameters as a variable
	When entered string "[[TODAY={{TODAY}}]]"
	Then verify string "[[TODAY]]" equals "{{TODAY}}"