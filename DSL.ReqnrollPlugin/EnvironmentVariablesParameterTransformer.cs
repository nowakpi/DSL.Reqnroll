using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSL.ReqnrollPlugin
{
    public class EnvironmentVariablesParameterTransformer : VariablesParameterTransformerBase
    {
        protected override string TransformText(in string inputString, in ScenarioContext scenarioContext)
        {
            if (string.IsNullOrEmpty(inputString)) return inputString;
            var match = PatternMatch.Parse(inputString, PatternMatchConfig.EnvironmentMatchConfig);
            var envVariableValue = Environment.GetEnvironmentVariable("ServiceHubLogSessionKey");

            return match == null ? inputString : TransformText(match.ReplaceMatched(envVariableValue), scenarioContext);
        }
    }
}
