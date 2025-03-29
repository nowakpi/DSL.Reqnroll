using Reqnroll;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DSL.ReqnrollPlugin.Transformer
{
    public class FunctionParameterTransporter : BaseParameterTransformer, IFunctionTransformer
    {
        private string[] _supportedFunctions = { "DATE" };

        protected override string TransformText(in string inputString, in ScenarioContext scenarioContext) => TransformText(inputString);

        string[] IFunctionTransformer.GetSupportedFunctions() => _supportedFunctions;

        public virtual string TransformText(in string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return inputString;

            var match = PatternMatch.Parse(inputString, PatternMatchConfig.FunctionsMatchConfig);
            return match == null
                ? inputString
                : TransformText(match.ReplaceMatched(TransformPattern(match.MatchedPattern)));
        }

        public virtual string TransformPattern(in string pattern)
        {
            string buffer = pattern;
            
            var isDateFunction = RegexMatch.MatchDateFunction(buffer);
            if (isDateFunction.Success) buffer = TransformDateFunction(isDateFunction);

            return buffer;
        }

        private string TransformDateFunction(Match dateFunctionMatch)
        {   // bottom up travese
            var custVariableValue = TransformText(dateFunctionMatch.Groups[2]?.Value?.Trim());

            var custVariableName = TransformText(dateFunctionMatch.Groups[1]?.Value?.Trim());

            return custVariableValue;
        }
    }
}
