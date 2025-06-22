using DSL.ReqnrollPlugin.Matches;
using Reqnroll;
using System;

namespace DSL.ReqnrollPlugin.Transformers
{
    public class EnvironmentVariableTransformer : BaseParameterTransformer, IEnvironmentVariableTransformer
    {
        public override byte OrderId { get => PatternMatchConfig.EnvironmentMatchConfig.MatchOrder; }

        protected override string TransformText(in string inputString, in ScenarioContext scenarioContext) => TransformText(inputString);

        public virtual string GetEnvironmentVariable(string key) => key == null ? key : Environment.GetEnvironmentVariable(key);

        public virtual string TransformText(in string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return inputString;

            var match = PatternMatch.Parse(inputString, PatternMatchConfig.EnvironmentMatchConfig);
            var envVariableValue = GetEnvironmentVariable(match?.MatchedPattern);

            return match == null || string.IsNullOrEmpty(envVariableValue)
                ? inputString
                : match.ReplaceMatched(envVariableValue);
        }
    }
}