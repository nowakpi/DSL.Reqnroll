@UT
Feature: Examples of environment variables use

Scenario: Custom env variable
	When environment variable "DSL_REQNROLL" has a value of "Req'n'roll Test String"
	Then verify string "This is ((DSL_REQNROLL)) ! (r)" equals "This is Req'n'roll Test String ! (r)"

Scenario: Windows env variable
	When executed on Windows machine
	Then verify string "{ OS: '((OS))', TMP: 'C:\Users\((USERNAME))\AppData\Local\Temp' }" equals "{ OS: 'Windows_NT', TMP: '((TMP))' }"