using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DSL.ReqnrollPlugin.Transformer
{
    public class FunctionParameterTransformer : BaseParameterTransformer, IFunctionTransformer
    {
        private const string DEFAULT_TODAY_FUNC_OUTPUT_FORMAT = @"yyyy-MM-dd";
        private readonly string[] _supportedFunctions = { "TODAY" };

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
            if (isDateFunction[0].Success) buffer = TransformTodayFunction(isDateFunction);

            return buffer;
        }

        private string TransformTodayFunction(MatchCollection dateFunctionMatch)
        {
            DateTimeOffset currDateTimeOffsetUTC = DateTimeOffset.UtcNow;
            char[] charsToRemove = {'#'};

            currDateTimeOffsetUTC = ExtractTimeType(dateFunctionMatch, currDateTimeOffsetUTC, charsToRemove);
            currDateTimeOffsetUTC = ExtractUserOffset(dateFunctionMatch, currDateTimeOffsetUTC);

            return TransformToFinalFormat(dateFunctionMatch, currDateTimeOffsetUTC, charsToRemove);
        }

        private static string TransformToFinalFormat(MatchCollection dateFunctionMatch, DateTimeOffset currDateTimeOffsetUTC, char[] charsToRemove)
        {
            var userFormatting = dateFunctionMatch[0]?.Groups[3]?.Value?.Trim().TrimStart(charsToRemove).Trim();
            return currDateTimeOffsetUTC.ToString(string.IsNullOrWhiteSpace(userFormatting) ? DEFAULT_TODAY_FUNC_OUTPUT_FORMAT : userFormatting);
        }

        private DateTimeOffset ExtractUserOffset(MatchCollection dateFunctionMatch, DateTimeOffset currDateTimeOffsetUTC)
        {
            var userOffset = dateFunctionMatch[0]?.Groups[2]?.Value?.Trim();
            if (!string.IsNullOrWhiteSpace(userOffset)) currDateTimeOffsetUTC = GetCurrentDateWithUserOffset(userOffset, currDateTimeOffsetUTC);
            return currDateTimeOffsetUTC;
        }

        protected static DateTimeOffset ExtractTimeType(MatchCollection dateFunctionMatch, DateTimeOffset currDateTimeOffsetUTC, char[] charsToRemove)
        {
            var timeType = dateFunctionMatch[0]?.Groups[1]?.Value?.Trim().TrimEnd(charsToRemove).Trim();

            if (!string.IsNullOrWhiteSpace(timeType))
                if (timeType == "L") currDateTimeOffsetUTC = DateTimeOffset.UtcNow.ToLocalTime();
                else if (timeType == "U") currDateTimeOffsetUTC = DateTimeOffset.UtcNow;
            
            return currDateTimeOffsetUTC;
        }

        private static DateTimeOffset GetCurrentDateWithUserOffset(in string userOffset, DateTimeOffset currDateTimeOffsetUTC)
        {
            var offsetValue = userOffset.Substring(0, userOffset.Length - 1);
            var offsetType = userOffset.Substring(userOffset.Length - 1, 1);

            if (int.TryParse(offsetValue.Trim(), out var offset))
                switch (offsetType)
                {
                    case "y":
                        currDateTimeOffsetUTC = currDateTimeOffsetUTC.AddYears(offset);
                        break;
                    case "M":
                        currDateTimeOffsetUTC = currDateTimeOffsetUTC.AddMonths(offset);
                        break;
                    case "d":
                        currDateTimeOffsetUTC = currDateTimeOffsetUTC.AddDays(offset);
                        break;
                }

            return currDateTimeOffsetUTC;
        }
    }
}
