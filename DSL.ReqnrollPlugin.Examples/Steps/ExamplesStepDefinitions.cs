using DSL.ReqnrollPlugin;
using Reqnroll;
using FluentAssertions;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Xunit;
using DSL.ReqnrollPlugin.Transformers;

namespace Examples.Steps
{
    [Binding]
    public sealed class ExamplesStepDefinitions
    {
        ScenarioContext _context;

        public ExamplesStepDefinitions(ScenarioContext context)
        {
            _context = context;
        }

        [When(@"entered int (.*)")]
        public void WhenEnteredInt(int p0)
        {

        }

        [Then(@"verify int (.*) equals (.*)")]
        public void ThenVerifyIntEquals(int p0, int p1)
        {
            p1.Should().Be(p0);
        }

        [When(@"entered string ""(.*)""")]
        public void WhenEnteredString(string p0)
        {
        }

        [When(@"executed on Windows machine")]
        public void WhenExecutedOnWindowsMachine()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) throw new OperationCanceledException("Not a Windows machine.");
        }

        [When(@"environment variable ""(.*)"" has a value of ""(.*)""")]
        public void WhenEnvironmentVariableHasValue(string varName, string varValue)
        {
            Environment.SetEnvironmentVariable(varName, varValue);
        }

        [Given(@"I have a cutomerise pattern mapping ""(.*)"" to ""(.*)""")]
        public void GivenIHaveACutomerisePatternMappingTo(string keyword, string value)
        {
            ((IUserVariableTransformer)
                (_context.GetBindingInstance(typeof(IUserVariableTransformer))))
            .AddBespokeTransformer(s => s.ToLower() == keyword.ToLower() ? value : s);
        }

        [Given(@"I have a cutomerise pattern to support calculation")]
        public void GivenIHaveACutomerisePatternToSupportCalculation()
        {
            ((IUserVariableTransformer)
                (_context.GetBindingInstance(typeof(IUserVariableTransformer))))
            .AddBespokeTransformer(s =>
            {
                var m = Regex.Match(s, "([0-9]+)(\\+|\\-|\\*|\\/)([0-9]+)");
                if (m.Success)
                {
                    switch (m.Groups[2].Value)
                    {
                        case "+":
                            return (int.Parse(m.Groups[1].Value) + int.Parse(m.Groups[3].Value)).ToString();
                        case "-":
                            return (int.Parse(m.Groups[1].Value) - int.Parse(m.Groups[3].Value)).ToString();
                        case "*":
                            return (int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[3].Value)).ToString();
                        case "/":
                            return (int.Parse(m.Groups[1].Value) / int.Parse(m.Groups[3].Value)).ToString();
                        default:
                            return s;
                    }
                }
                return s;
            }
                );
        }



        [Given(@"I have a pattern to transform ""(.*)"" to ""(.*)""")]
        public void GivenIHaveAPatternToTransformTo(string p0, string p1)
        {

        }

        [Then(@"verify string ""(.*)"" equals ""(.*)""")]
        public void ThenVerifyStringEquals(string p0, string p1)
        {
            p1.Should().Be(p0);
        }

        [Then(@"verify string ""(.*)"" is not empty and represents an integer bettwen 1 and 2147483647")]
        public void ThenVerifyStringRepresentsIntegerBetween_1_And_2147483647(string p0)
        {
            if (int.TryParse(p0, out int result)) 
            {  
                if (!(result >= 1 && result <= Int32.MaxValue)) Assert.Fail("Value returned by {{RANDOM}} is not between 1 and 2147483647");
            } else Assert.Fail("Cannot parse value returned by {{RANDOM}} as an integer");
        }

        [Then(@"verify string ""(.*)"" is not empty and represents an integer bettwen 20 and 80")]
        public void ThenVerifyStringRepresentsIntegerBetween_20_And_80(string p0)
        {
            if (int.TryParse(p0, out int result))
            {
                if (!(result >= 20 && result <= 80)) Assert.Fail("Value returned by {{RANDOM}} is not between 20 and 80");
            }
            else Assert.Fail("Cannot parse value returned by {{RANDOM}} as an integer");
        }

        [Then(@"verify string ""(.*)"" equals to the same local time 6 months ago in format of dd-MM-yyyy HH:mm:ss")]
        public void ThenVerifyStringEqualsSpecificDateTimeInLocalTimeMinus6M(string p0)
        {
            DateTimeOffset today = DateTimeOffset.UtcNow.ToLocalTime();
            today = today.AddMonths(-6);
            var formattedDateTime = today.ToString("dd-MM-yyy HH:mm:ss");

            formattedDateTime.Should().Be(p0);
        }

        [Then(@"verify string ""(.*)"" equals to today in local time 3 hours ago in format of dd-MM-yyyy HH:mm:ss")]
        public void ThenVerifyStringEqualsSpecificDateTimeInLocalTimeMinus3H(string p0)
        {
            DateTimeOffset today = DateTimeOffset.UtcNow.ToLocalTime();
            today = today.AddHours(-3);
            var formattedDateTime = today.ToString("dd-MM-yyy HH:mm:ss");

            formattedDateTime.Should().Be(p0);
        }


        [When(@"entered long string")]
        public void GivenEnteredLongString(string multilineText)
        {
            var target = @"
       ok,
       50";
            target.Should().Be(multilineText);
        }

        [Then(@"verify string ""(.*)"" matches ""(.*)""")]
        public void ThenVerifyStringMatches(string str2Match, string regex)
        {
            new Regex(regex).IsMatch(str2Match).Should().BeTrue();
        }

        [Then(@"verify string ""(.*)"" is not defined")]
        public void ThenVerifyStringIsNotDefined(string p0)
        {
            object var;
            _context.TryGetValue(p0, out var).Should().BeFalse();

        }

        [When(@"use table with the following details:")]
        public void WhenUseTableWithTheFollowingDetails(Table table)
        {

        }
    }
}
