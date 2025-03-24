# DSL.Reqnroll

DSL.Reqnroll is Reqnroll plugin that enables use of dynamic test data in Reqnroll steps by bringing in custom and environment variables, regular expressions and bespoke transformations.

It's re-write of [SpecFlow.DSL](https://github.com/wenyuansong/Specflow.DSL) library, written originally by [Wenyuan(Ryan)](https://github.com/wenyuansong) and [Liam Flanagan](https://github.com/JovialJerboa), to align it with Reqnroll.

### Background
In December 2024, [Tricentis](https://support-hub.tricentis.com/open?number=NEW0001432&id=post) announced the end-of-life of the SpecFlow open source project. According to the announcement, SpecFlow reached its end-of-life on December 31, 2024. As of 1st January 2025, the SpecFlow GitHub projects have been deleted and the support section of the specflow.org website is disabled.

### Syntax
```
  (( ))                //double parentheses in any text will trigger matching with environment variables of the machine executing the test
  ((PATH))             //will get a value of PATH environmental variable

  [[ ]]                //double bracket in any text will trigger pattern matching for custom variables 
  [[varName=value]]    //will create a variable named "varName" with value "value" 
  [[varName]]          //will get value of "varName", throw an error if "varName" is not defined
  [[varName=RegEx(patternText)]]  //RegEx() is a keyword that value is generated from patternText
``` 
**How it works**:
   It actually creates key/value pairs in current ScenarioContext. So be careful not to conflict with your own context variables. 

## Examples: 

  - Read environment variables of the machine executing the test
```
	When executed on Windows machine
	Then verify string "{ OS: '((OS))', TMP: 'C:\Users\((USERNAME))\AppData\Local\Temp' }" equals "{ OS: 'Windows_NT', TMP: '((TMP))' }"
```	
 - Create dynamic test data and refer it in another step
```
	When enter [[var=50]]         //assign 50 to a variable named "var"
	Then [[var]] equals 50    // now get variable "var" value
```	
 - Create dynamic test data using regular expression
```
 	When enter [[var=RegEx([0-9]{3})]]         //assign var with 3 digit random number
	Then [[var]] is a 3 digits number          // now get variable "var" value
```
 - Same applies to Reqnroll tables:
```
     When create new user with the following details:
     |Field   | Value                                 |
     |UserName| [[name=RegEx([a-z]{5,8})]]            |	        
     |Password| [[pwd=RegEx([a-z]{3}[0-9]{3})]]       |
     Then verify can login with username="[[name]]" and password="[[pwd]]"
```   

 - Support bespoke transformation

```
     for example, you want to map Today to YYYY:MM:dd, add the following code in one of your Reqnroll steps 
	 or put it in BeforeScenario step.
      ((IParameterTransform)
                (_scenarioContext.GetBindingInstance(typeof(IParameterTransform))))
            .addTransformer(s => s.ToLower() == today ? DateTime.Now.ToString("yyyy/MM/dd") : s); 
	
     Now in Reqnroll feature files, you can write:	
	 When entered "[[timeVar=Today]]"		  //timeVar will be assigned to yyyy/MM/dd, e.g 2017/12/04
```  
 
 - Calculations are currently NOT supported but it can be done by customerisation as shown in project Examples

## Dependencies
* .NetFramework 4.6.2+ or .NetCore 2.0+
* Reqnroll v2.2.1 +
* Fare 2.1.2 +

License: MIT (https://github.com/nowakpi/DSL.Reqnroll/blob/master/LICENSE)

NuGet: https://www.nuget.org/packages/DSL.Reqnroll

## Installation

- Install plugin from NuGet into your Reqnroll project.

    PM> Install-Package DSL.Reqnroll
 
- In case you use App.config file, ensure the following lines have been added to enable plugin:
```
   <plugins>
      <add name="DSL" type="Runtime"/>
    </plugins>
 ```



