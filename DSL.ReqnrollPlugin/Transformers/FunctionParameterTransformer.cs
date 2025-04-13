using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DSL.ReqnrollPlugin.Transformer
{
    public class FunctionParameterTransformer : BaseParameterTransformer, IFunctionTransformer
    {
        private const string DEFAULT_TODAY_FUNC_OUTPUT_FORMAT = "yyyy-MM-dd";
        private string[] _supportedFunctions = { "TODAY" };

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
            var currentDateUTC = DateTimeOffset.UtcNow;

            var userOffset = string.Empty;

            //todo: carefully intrepret match collection and groups within each item to identify if the function is TODAY and has required elements (offset, formatting)
            //todo2: add U or L prefix before TODAY to handle UTC or local times
            dateFunctionMatch[0]?.Groups[1]?.Value?.Trim();


            if (!string.IsNullOrWhiteSpace(userOffset)) currentDateUTC = GetCurrentDateWithUserOffset(userOffset);

            var userFormatting = dateFunctionMatch[2]?.Groups[0]?.Value?.Trim();

            return currentDateUTC.ToString(string.IsNullOrWhiteSpace(userFormatting) ? DEFAULT_TODAY_FUNC_OUTPUT_FORMAT : userFormatting);
        }

        private DateTimeOffset GetCurrentDateWithUserOffset(in string userOffset)
        {
            var offsetValue = userOffset.Substring(0, userOffset.Length - 1);
            var offsetType = userOffset.Substring(userOffset.Length - 1, 1);
            var currentDateUTC = DateTimeOffset.UtcNow;

            if (int.TryParse(offsetValue.Trim(), out var offset))
            {
                switch (offsetType)
                {
                    case "y":
                        currentDateUTC = currentDateUTC.AddYears(offset);
                        break;
                    case "M":
                        currentDateUTC = currentDateUTC.AddMonths(offset);
                        break;
                    case "d":
                        currentDateUTC = currentDateUTC.AddDays(offset);
                        break;
                }
                
            }

            return currentDateUTC;
        }
    }
}
