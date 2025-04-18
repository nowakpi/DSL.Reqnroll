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
        {
            var isAssignment = RegexMatch.MatchAssignement(pattern);
            return isAssignment.Success
                ? TransformAssignment(scenarioContext, isAssignment)
                : TryGetValueFromScenarioContext(pattern, scenarioContext);
        }

        private string TransformAssignment(Dictionary<string, object> scenarioContext, Match isAssignment)
        {
            var custVariableValue = TransformText(UserVariableMatchInterpreter.GetCustomVariableValue(isAssignment), scenarioContext);
            var valueTransformedByBespokeTransformers = ApplyBespokeTransformers(custVariableValue);
            var valueWithAppliedCustomRegex = UserVariableMatchInterpreter.ApplyCustomRegex(valueTransformedByBespokeTransformers);
            var custVariableName = TransformText(UserVariableMatchInterpreter.GetCustomVariableName(isAssignment), scenarioContext);

            scenarioContext[custVariableName] = valueWithAppliedCustomRegex;
            return valueWithAppliedCustomRegex;
        }
    }
}
