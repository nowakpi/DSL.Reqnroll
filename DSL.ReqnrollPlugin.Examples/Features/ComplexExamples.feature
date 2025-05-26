@integration

Feature: ComplexExamples

#This feature shows how environment, user-defined and function transformers can work together.
#As per current codebase you cannot have multiple user-variable definitions in one input string (you can refer to another one but not define second one):
# - allowed:  [[BOP={{TODAY-[[RADIUS]]y#yyyy}}]], {{L#TODAY-[[RADIUS]]d#dd-MM-yyyy}}
# - not allowed:  
# - multiple functions and environment variables are allowed
# - result of one function can be used in definition of other function

Scenario: Function execution can be embedded in another function
	 When entered string "[[var={{L#TODAY-{{L#TODAY#dd}}d#dd-MM-yyyy}}]]"
	 Then verify string "[[var]]" equals "{{L#TODAY-{{L#TODAY#dd}}d#dd-MM-yyyy}}"

Scenario: Mix of environment user-defined and functions in action 1
	When environment variable "DATE1" has a value of "{{L#TODAY-1d#dd-MM-yyyy}}"
	 And environment variable "DATE2" has a value of "{{L#TODAY#dd-MM-yyyy}}"
	 And environment variable "DATE3" has a value of "{{L#TODAY+1d#dd-MM-yyyy}}"
	 And entered string "[[var=((DATE1)),((DATE2)),((DATE3))]]"
	
	Then verify string "[[var]]" equals "{{L#TODAY-1d#dd-MM-yyyy}},{{L#TODAY#dd-MM-yyyy}},{{L#TODAY+1d#dd-MM-yyyy}}"

Scenario: Mix of environment user-defined and functions in action 2
	When entered string "[[RADIUS={{RANDOM:1:30}}]]"
	 And environment variable "DATE1" has a value of "{{L#TODAY-[[RADIUS]]d#dd-MM-yyyy}}"
	 And environment variable "DATE2" has a value of "{{L#TODAY#dd-MM-yyyy}}"
	 And environment variable "DATE3" has a value of "{{L#TODAY+[[RADIUS]]d#dd-MM-yyyy}}"
	 And entered string "[[var=((DATE1)),((DATE2)),((DATE3))]]"
	
	Then verify string "[[var]]" equals "{{L#TODAY-[[RADIUS]]d#dd-MM-yyyy}},{{L#TODAY#dd-MM-yyyy}},{{L#TODAY+[[RADIUS]]d#dd-MM-yyyy}}"

Scenario: Mix of environment user-defined and functions in action 3
	 When environment variable "DATE1" has a value of "{{L#TODAY-[[RADIUS={{RANDOM:1:30}}]]d#dd-MM-yyyy}}"
	 And environment variable "DATE2" has a value of "{{L#TODAY#dd-MM-yyyy}}"
	 And environment variable "DATE3" has a value of "{{L#TODAY+[[RADIUS]]d#dd-MM-yyyy}}"
	 And entered string "[[var=((DATE1)),((DATE2)),((DATE3))]]"
	
	Then verify string "[[var]]" equals "{{L#TODAY-[[RADIUS]]d#dd-MM-yyyy}},{{L#TODAY#dd-MM-yyyy}},{{L#TODAY+[[RADIUS]]d#dd-MM-yyyy}}"

Scenario: Mix of environment user-defined and functions in action 4
	When environment variable "TEST_ENV" has a value of "LTDEV01{{RANDOM:100:999}}"
	And use table with the following details: 
	| Item                      | Definition                            |
	| Radius of a time period   | [[RADIUS={{RANDOM:1:10}}]]            |
	| Start year of the period  | [[BOP={{TODAY-[[RADIUS]]y#yyyy}}]]    |
	| End year of the period    | [[EOP={{TODAY#yyyy}}]]                |
	| Output file location      | [[OUTPUT=C:\tmp\((TEST_ENV))]]        |

	Then verify table with expected values of variables:
	| Variable | Value                          |
	| BOP      | {{TODAY-[[RADIUS]]y#yyyy}}     |
	| EOP      | {{TODAY#yyyy}}                 |
	| OUTPUT   | C:\tmp\((TEST_ENV))            |