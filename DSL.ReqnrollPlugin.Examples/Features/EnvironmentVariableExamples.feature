@Integration
Feature: Examples of environment variables use

Scenario: Custom env variable
	When environment variable "DSL_REQNROLL" has a value of "Req'n'roll Test String"
	Then verify string "This is ((DSL_REQNROLL)) ! (r)" equals "This is Req'n'roll Test String ! (r)"

Scenario: Windows env variable
	When executed on Windows machine
	Then verify string "{ OS: '((OS))', ARCH: '((PROCESSOR_ARCHITECTURE))' }" equals "{ OS: 'Windows_NT', ARCH: '((PROCESSOR_ARCHITECTURE))' }"

Scenario: Env variable does not exist
	When entered string "Just some text to be able to include When step"
	Then verify string "((VARIABLE_THAT_SHOULD_NOT_EXIST))" equals "((VARIABLE_THAT_SHOULD_NOT_EXIST))"

Scenario: mix of custom and env variables
	When environment variable "CRON" has a value of "0 9 * * *"
	 And entered string "[[var=((CRON))]]"
	Then verify string "[[var]]" equals "0 9 * * *"