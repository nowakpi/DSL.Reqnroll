using DSL.ReqnrollPlugin.Matches;
using Reqnroll;
using System;

namespace DSL.ReqnrollPlugin.Transformers
{
    public class EnvironmentVariableTransformer : BaseParameterTransformer, IEnvironmentVariableTransformer
    {
        public override byte TransformerId { get => 1; }

        protected override string TransformText(in string inputString, in ScenarioContext scenarioContext) => TransformText(inputString);

        public virtual string GetEnvironmentVariable(string key) => key == null ? key : Environment.GetEnvironmentVariable(key);

        public virtual string TransformText(in string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return inputString;

            var match = PatternMatch.Parse(inputString, PatternMatchConfig.EnvironmentMatchConfig);
            var envVariableValue = GetEnvironmentVariable(match?.MatchedPattern);

            return match == null || string.IsNullOrEmpty(envVariableValue)
                ? inputString
                : TransformText(match.ReplaceMatched(envVariableValue));
        }
    }
}