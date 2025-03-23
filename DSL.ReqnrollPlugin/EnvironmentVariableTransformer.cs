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
        public virtual string GetEnvironmentVariable(string key)
        {
            return key == null ? key : Environment.GetEnvironmentVariable(key);
        }

        protected override string TransformText(in string inputString, in ScenarioContext scenarioContext)
        {
            if (string.IsNullOrEmpty(inputString)) return inputString;

            var match = PatternMatch.Parse(inputString, PatternMatchConfig.EnvironmentMatchConfig);
            var envVariableValue = GetEnvironmentVariable(match?.MatchedPattern);

            return (match == null || envVariableValue == null) ? inputString : TransformText(match.ReplaceMatched(envVariableValue), scenarioContext);
        }
    }
}
