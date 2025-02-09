# Reqnroll.DSL

Reqnroll.DSL is Reqnroll plugin that enables use of dynamic test data in Reqnroll steps by bringing in variables, regular expressions and simple calculations.

It's a re-write of [SpecFlow.DSL](https://github.com/wenyuansong/Specflow.DSL) library written originally by [wenyuansong](https://github.com/wenyuansong) and [JovialJerboa](https://github.com/JovialJerboa).

**Background**:<br>
In December 2024, Tricentis announced the end-of-life of the SpecFlow open source project. According to the announcement, SpecFlow reached its end-of-life on December 31, 2024. As of 1st January 2025, the SpecFlow GitHub projects are deleted and the support section of the specflow.org website is disabled.

**Syntax**:<br>
```
  [[ ]]                //double bracket in any text will trigger pattern matching 
  [[varName=value]]    //will create a variable named "varName" with value "value" 
  [[varName]]         //will get value of "varName", throw an error if "varName" is not defined
  [[varName=RegEx(patternText)]]  //RegEx() is a keyword that value is generated from patternText
```  
**How it works**: <br>
   It actually creates key/value pairs in current ScenarioContext.
   So be careful not to conflict with your own context variables. 

**Examples**: 
 
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

 - Support customerise transformation

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

Dependencies:
* .NetFramework 4.6.2+ or .NetCore 2.0+
* Reqnroll v2.2.1 +
* Fare 2.1.2 +

License: MIT (https://github.com/nowakpi/Reqnroll.DSL/blob/master/LICENSE)

NuGet: https://www.nuget.org/packages/Reqnroll.DSL

## Installation

- Install plugin from NuGet into your Reqnroll project.

    PM> Install-Package Reqnroll.DSL
 
That's it!!!   

Your project's App.config file will be automatically added the following lines to enable plugin:
```
   <plugins>
      <add name="Reqnroll.DSL" type="Runtime"/>
    </plugins>
 ```
