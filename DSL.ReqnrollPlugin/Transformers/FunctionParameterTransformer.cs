using Reqnroll;
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace DSL.ReqnrollPlugin.Transformer
{
    public class FunctionParameterTransformer : BaseParameterTransformer, IFunctionTransformer
    {
        private readonly string[] _supportedFunctions = { TodayFuncMatchInterpreter.FuncName };

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
            var isDateFunction = RegexMatch.MatchDateFunction(pattern);
            return isDateFunction[TodayFuncMatchInterpreter.MatchIndex].Success ? TransformTodayFunction(isDateFunction) : pattern;
        }

        private string TransformTodayFunction(MatchCollection todayFuncMatches)
        {
            var currDateTimeOffset = TodayFuncMatchInterpreter.ExtractTimeType(todayFuncMatches, DateTimeOffset.UtcNow);
            var dateTimeWithUserOffset = TodayFuncMatchInterpreter.ExtractUserOffset(todayFuncMatches, currDateTimeOffset);

            return TodayFuncMatchInterpreter.TransformToUserFormat(todayFuncMatches, dateTimeWithUserOffset);
        }
    }
}
