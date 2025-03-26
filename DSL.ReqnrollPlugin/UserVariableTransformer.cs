using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
        {
            // supports [[key=value]] assignment
            var isAssignment = Regex.Match(pattern, @"(.*)=(.*)");
            return isAssignment.Success
                ? TransformAssignment(scenarioContext, isAssignment)
                : TryGetValueFromScenarioContext(pattern, scenarioContext);
        }

        private string TransformAssignment(Dictionary<string, object> scenarioContext, Match isAssignment)
        {
            // bottom up travese
            var cxtValue = TransformText(isAssignment.Groups[2]?.Value?.Trim(), scenarioContext);
            cxtValue = ApplyBespokeTransformers(cxtValue);
            // apply RegEx
            var regExM = Regex.Match(cxtValue, @"RegEx\((.*)\)", RegexOptions.IgnoreCase);
            if (regExM.Success) cxtValue = new Fare.Xeger(regExM.Groups[1]?.Value).Generate();

            var cxtKey = TransformText(isAssignment.Groups[1]?.Value?.Trim(), scenarioContext);
            scenarioContext[cxtKey] = cxtValue;

            return cxtValue;
        }

        private static string TryGetValueFromScenarioContext(string pattern, Dictionary<string, object> scenarioContext)
        {
            try
            {
                return scenarioContext[pattern] as string;
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException("[DSL.ReqnrollPlugin] Can't find key:" + pattern + " in scenario context");
            }
        }
    }
}
