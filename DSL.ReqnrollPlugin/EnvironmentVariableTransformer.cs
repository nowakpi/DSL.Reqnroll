using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSL.ReqnrollPlugin
{
    public class EnvironmentVariableTransformer : VariablesParameterTransformer, IEnvironmentVariableTransformer
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