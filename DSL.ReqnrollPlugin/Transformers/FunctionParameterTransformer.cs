using DSL.ReqnrollPlugin.Helpers;
using DSL.ReqnrollPlugin.Matches;
using Reqnroll;
using System;
using System.Text.RegularExpressions;

namespace DSL.ReqnrollPlugin.Transformers
{
    public class FunctionParameterTransformer : BaseParameterTransformer, IFunctionTransformer
    {
        private readonly string[] _supportedFunctions = { TodayFuncMatchInterpreter.FuncName, RandomFuncMatchInterpreter.FuncName };

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

        public virtual string TransformPattern(in string inputString)
        {
            var isDateFunction = RegexMatch.MatchTodateFunction(inputString);
            if (isDateFunction.Count > 0 && isDateFunction[TodayFuncMatchInterpreter.MatchIndex].Success) return TransformTodayFunction(isDateFunction);

            var isRandomFunction = RegexMatch.MatchRandomFunction(inputString);
            return isRandomFunction.Count > 0 && isRandomFunction[RandomFuncMatchInterpreter.MatchIndex].Success ? TransformRandomFunction(isRandomFunction) : inputString;
        }

        private static string TransformTodayFunction(MatchCollection todayFuncMatches)
        {
            var currDateTimeOffset = TodayFuncMatchInterpreter.ExtractTimeType(todayFuncMatches, DateTimeOffset.UtcNow);
            var dateTimeWithUserOffset = TodayFuncMatchInterpreter.ExtractUserOffset(todayFuncMatches, currDateTimeOffset);

            return TodayFuncMatchInterpreter.TransformToUserFormat(todayFuncMatches, dateTimeWithUserOffset);
        }

        private static string TransformRandomFunction(MatchCollection randomFuncMatches)
        {
            var (rangeFrom, rangeTo) = RandomFuncMatchInterpreter.GetRandomRange(randomFuncMatches);
            return SecureRandomHelper.GetSecureRandomInt(rangeFrom, rangeTo).ToString();
        }
    }
}
