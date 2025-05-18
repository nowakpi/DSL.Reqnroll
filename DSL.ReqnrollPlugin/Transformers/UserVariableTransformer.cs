using System.Collections.Generic;
using System.Text.RegularExpressions;
using DSL.ReqnrollPlugin.Matches;
using Reqnroll;

namespace DSL.ReqnrollPlugin.Transformers
{
    public class UserVariableTransformer : VariablesParameterTransformer, IUserVariableTransformer
    {
        public override byte TransformerId { get => 3; }

        protected override string TransformText(in string inputString, in ScenarioContext scenarioContext) => TransformTextLocal(inputString, scenarioContext);

        public virtual string TransformTextLocal(in string inputString, in Dictionary<string, object> scenarioContext)
        {
            if (string.IsNullOrEmpty(inputString)) return inputString;

            var match = PatternMatch.Parse(inputString, PatternMatchConfig.CustomVariablesMatchConfig);
            return match == null
                ? inputString
                : match.ReplaceMatched(TransformPattern(match.MatchedPattern, scenarioContext));
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
            var custVariableValue = TransformTextLocal(UserVariableMatchInterpreter.GetCustomVariableValue(isAssignment), scenarioContext);
            var valueTransformedByBespokeTransformers = ApplyBespokeTransformers(custVariableValue);
            var valueWithAppliedCustomRegex = UserVariableMatchInterpreter.ApplyCustomRegex(valueTransformedByBespokeTransformers);
            var custVariableName = TransformTextLocal(UserVariableMatchInterpreter.GetCustomVariableName(isAssignment), scenarioContext);

            scenarioContext[custVariableName] = valueWithAppliedCustomRegex;
            return valueWithAppliedCustomRegex;
        }
    }
}
