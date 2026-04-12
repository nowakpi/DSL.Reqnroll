@Integration
Feature: Examples of how to use functions

Scenario: I use TODAY to define formatted date/time with month offset in local time
	When entered string "Just some text to be able to include When step"
	Then verify string "{{L#TODAY-6M#dd-MM-yyyy HH:mm:ss}}" equals to the same local time 6 months ago in format of dd-MM-yyyy HH:mm:ss

Scenario: I use TODAY to define formatted date/time with hour offset in local time
	When entered string "Just some text to be able to include When step"
	Then verify string "{{L#TODAY-3H#dd-MM-yyyy HH:mm:ss}}" equals to today in local time 3 hours ago in format of dd-MM-yyyy HH:mm:ss

Scenario: I use TODAY function without parameters as a variable
	When entered string "[[DZIS={{TODAY#dd-MM-yyyy}}]]"
	Then verify string "[[DZIS]]" equals "{{TODAY#dd-MM-yyyy}}"
	And verify string "[[DZIS]]" equals "{{TODAY#dd-MM-yyyy}}"
	And verify string "[[DZIS]]" equals "{{TODAY#dd-MM-yyyy}}"

Scenario: I use TODAY function and TODAY as variable in the table
	When use table with the following details: 
	 | Field     | Value                            |
	 | Field 1   | [[DZIS={{TODAY#dd-MM-yyyy}}]]    |
	Then verify column values are equal for each row:
	 | Column A  | Column B                |
	 | [[DZIS]]  | {{TODAY#dd-MM-yyyy}}    |
	 | [[DZIS]]  | {{TODAY#dd-MM-yyyy}}    |
	 | [[DZIS]]  | {{TODAY#dd-MM-yyyy}}    |

Scenario: I use RANDOM function without parameters as a variable
	When entered string "[[RVAL={{RANDOM}}]]"
	Then verify string "[[RVAL]]" is not empty and represents an integer bettwen 1 and 2147483647

Scenario: I use RANDOM function with parameters as a variavble
	When entered string "[[RVAL={{RANDOM:20:80}}]]"
	Then verify string "[[RVAL]]" is not empty and represents an integer bettwen 20 and 80