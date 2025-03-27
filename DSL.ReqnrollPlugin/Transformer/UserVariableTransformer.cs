using System.Collections.Generic;
using System.Text.RegularExpressions;
using Reqnroll;

namespace DSL.ReqnrollPlugin
{
    public class UserVariableTransformer : VariablesParameterTransformer, IParameterTransformer
    {
        protected override string TransformText(in string inputString, in ScenarioContext scenarioContext) => TransformText(inputString, scenarioContext);

        public virtual string TransformText(in string inputString, in Dictionary<string, object> scenarioContext)
        {
            if (string.IsNullOrEmpty(inputString)) return inputString;

            var match = PatternMatch.Parse(inputString, PatternMatchConfig.CustomVariablesMatchConfig);
            return match == null
                ? inputString
                : TransformText(match.ReplaceMatched(TransformPattern(match.MatchedPattern, scenarioContext)), scenarioContext);
        }

        public virtual string TransformPattern(in string pattern, in Dictionary<string, object> scenarioContext)
        {   // supports [[key=value]] assignment
            var isAssignment = Regex.Match(pattern, @"(.*)=(.*)");
            return isAssignment.Success
                ? TransformAssignment(scenarioContext, isAssignment)
                : TryGetValueFromScenarioContext(pattern, scenarioContext);
        }

        private string TransformAssignment(Dictionary<string, object> scenarioContext, Match isAssignment)
        {   // bottom up travese
            var custVariableValue = TransformText(isAssignment.Groups[2]?.Value?.Trim(), scenarioContext);
            custVariableValue = ApplyBespokeTransformers(custVariableValue);
            // apply RegEx
            var regExM = Regex.Match(custVariableValue, @"RegEx\((.*)\)", RegexOptions.IgnoreCase);
            if (regExM.Success) custVariableValue = new Fare.Xeger(regExM.Groups[1]?.Value).Generate();

            var custVariableName = TransformText(isAssignment.Groups[1]?.Value?.Trim(), scenarioContext);
            scenarioContext[custVariableName] = custVariableValue;

            return custVariableValue;
        }

        private static string TryGetValueFromScenarioContext(string pattern, Dictionary<string, object> scenarioContext)
        {
            return scenarioContext.TryGetValue(pattern, out var value)
                ? value as string
                : throw new KeyNotFoundException("[DSL.ReqnrollPlugin] Can't find key:" + pattern + " in scenario context");
        }
    }
}
