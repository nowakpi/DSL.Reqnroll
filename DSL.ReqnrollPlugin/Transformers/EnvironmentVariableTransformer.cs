using Reqnroll;
using System;

namespace DSL.ReqnrollPlugin
{
    public class EnvironmentVariableTransformer : BaseParameterTransformer, IEnvironmentVariableTransformer
    {
        protected override string TransformText(in string inputString, in ScenarioContext scenarioContext) => TransformText(inputString);
        
        public virtual string GetEnvironmentVariable(string key) => key == null ? key : Environment.GetEnvironmentVariable(key);

        public virtual string TransformText(in string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return inputString;

            var match = PatternMatch.Parse(inputString, PatternMatchConfig.EnvironmentMatchConfig);
            var envVariableValue = GetEnvironmentVariable(match?.MatchedPattern);

            return (match == null || envVariableValue == null) 
                ? inputString 
                : TransformText(match.ReplaceMatched(envVariableValue));
        }
    }
}