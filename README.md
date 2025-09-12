# DSL.Reqnroll

DSL.Reqnroll is Reqnroll plugin that enables use of dynamic test data in Reqnroll steps by bringing in custom and environment variables, built-in functions, regular expressions and bespoke transformations.

It's re-write of [SpecFlow.DSL](https://github.com/wenyuansong/Specflow.DSL) library, written originally by [Wenyuan(Ryan)](https://github.com/wenyuansong) and [Liam Flanagan](https://github.com/JovialJerboa), to align it with Reqnroll with multiple enhancements on the top.

### Why to use it?
Big benefit of the plugin is that authors of Reqnroll tests can maintain coherence and relationship between data points (great in finance), across multiple steps. 

They can generate data (numbers, timestamps) without having to hardcode specific values in the feature files. It also helps to avoid duplication of data in multiple steps.

Development team doesn't have to be engaged to make sometimes subtle changes in the step implementation code to accommodate changes in the test data.

### Background
In December 2024, [Tricentis](https://support-hub.tricentis.com/open?number=NEW0001432&id=post) announced the end-of-life of the SpecFlow open source project. According to the announcement, SpecFlow reached its end-of-life on December 31, 2024. As of 1st January 2025, the SpecFlow GitHub projects have been deleted and the support section of the specflow.org website is disabled.

### Syntax
```
  (( ))                //double parentheses in any text will trigger matching with environment variables of the machine executing the test
  ((PATH))             //will get a value of PATH environmental variable

  [[ ]]                //double square bracket in any text will trigger pattern matching for custom variables 
  [[varName=value]]    //will create a variable named "varName" with value "value" 
  [[varName]]          //will get value of "varName", throw an error if "varName" is not defined
  [[varName=RegEx(patternText)]]  //RegEx() is a keyword that value is generated from patternText

  {{ }}                //double curly brackets in any text will trigger pattern matching for built-in function and execute it
  {{TODAY}}            //will execute TODAY function and return current UTC timestamp in a default formatting as dd-MM-yyyy
  {{L#TODAY#dd-MM-yyyy}}  //will execute TODAY function and return curretn timestamp in local time of the machine executing the test formatting the output as dd-MM-yyyy
  {{U#TODAY-7d#dd-MM-yyyy HH:mm:ss}}  //will execute TODAY function and return UTC timestamp from 7 days ago formatting the output as dd-MM-yyyy HH:mm:ss

  {{RANDOM}}           //will execute RANDOM function and return pseudo random number from a default range between 1 and Int32.MaxValue (2147483647)
  {{RANODM:20:80}}     //will execute RANDOM function and return pseudo random number from range between 20 and 80
``` 
**Changes to interfaces and namespaces**:
   In order to employ clean code principles and prepare the code base for further enhancements some interfaces and namespaces have changed in version 1.2.0. 
   
   Users of DSL.Reqnroll plugin used to use _IParameterTransformer_ interface and _AddBespokeTransformer_ method to add custom transformers. _IParameterTransformer_ interface became obsolete in version 1.2.0 - it may still be used but it will be removed in the future. It is adviced for users to use _IUserVariableTransformer_ interface from _DSL.ReqnrollPlugin.Transformers_ namespace, which also has _AddBespokeTransformer_ method to add bespoke transformations.

**How custom variables work**:
   It actually creates key/value pairs in current ScenarioContext. So be careful not to conflict with your own context variables. 

**How TODAY works**:
   You can add or subtract from current timestamp the following periods:
   d - day
   M - month
   y - year
   H - hour
   m - minute
   s - second
   
   You cannot combine the periods.
   
   Allowed: {{TODAY+1d}}  {{TODAY-10m}}
   Not allowed:  {{TODAY+1d-6h}}

   To define output format you can use the following characters:  dMyHhmsftz.-\\/: and space

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

 - More advanced examples: https://github.com/nowakpi/DSL.Reqnroll/blob/master/DSL.ReqnrollPlugin.Examples/Features/ComplexExamples.feature

 ## Other considerations: 
 
 - Plugin supports bespoke transformation

 - _IParameterTransformer_ interface became obsolete in version 1.2.0 - it may still be used but it will be removed in the future. It is adviced for users to use _IUserVariableTransformer_ interface from _DSL.ReqnrollPlugin.Transformers_ namespace, which also has _AddBespokeTransformer_ method to add bespoke transformations.
```
     for example, you want to map Today to YYYY:MM:dd programmatically, add the following code in one of your Reqnroll steps 
	 or put it in BeforeScenario step.
      ((IUserVariableTransformer)
                (_scenarioContext.GetBindingInstance(typeof(IUserVariableTransformer))))
            .AddBespokeTransformer(s => s.ToLower() == today ? DateTime.Now.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture) : s); 
	
     Now in Reqnroll feature files, you can write:	
	 When entered "[[timeVar=Today]]"		  //timeVar will be assigned to yyyy/MM/dd, e.g 2017/12/04
```  
 
 - Calculations are currently NOT supported but it can be done by customerisation as shown in project Examples

## Dependencies
* .NetFramework 4.6.2+ or .NetCore 2.0+
* Reqnroll v2.4.1 +
* Fare 2.2.1 +

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



