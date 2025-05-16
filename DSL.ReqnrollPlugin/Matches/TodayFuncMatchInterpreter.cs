using System;
using System.Text.RegularExpressions;

namespace DSL.ReqnrollPlugin.Matches
{
    public static class TodayFuncMatchInterpreter
    {
        private static readonly char[] _charsToRemove = { TodayFuncMatchInterpreter.OptionSeparator };

        private const string DEFAULT_TODAY_FUNC_OUTPUT_FORMAT = @"yyyy-MM-dd";
        private const string LOCAL_TIME_TYPE = "L";
        private const string UTC_TIME_TYPE = "U";

        public static int MatchIndex           { get => 0; }
        public static int TimeTypeGroupIndex   { get => 1; }
        public static int UserOffsetGroupIndex { get => 2; }
        public static int UserFormatGroupIndex { get => 3; }
        public static char OptionSeparator     { get => '#'; }
        public static string FuncName          { get => "TODAY"; }

        public static DateTimeOffset ExtractTimeType(MatchCollection todayFuncMatch, DateTimeOffset currDateTimeOffsetUTC)
        {
            var timeType = todayFuncMatch[MatchIndex]?.Groups[TimeTypeGroupIndex]?.Value?.Trim().TrimEnd(_charsToRemove).Trim();

            if (string.IsNullOrWhiteSpace(timeType)) return currDateTimeOffsetUTC;

            if (timeType == LOCAL_TIME_TYPE) currDateTimeOffsetUTC = DateTimeOffset.UtcNow.ToLocalTime();
            else if (timeType == UTC_TIME_TYPE) currDateTimeOffsetUTC = DateTimeOffset.UtcNow;

            return currDateTimeOffsetUTC;
        }

        public static DateTimeOffset ExtractUserOffset(MatchCollection todayFuncMatch, DateTimeOffset currDateTimeOffsetUTC)
        {
            var userOffset = todayFuncMatch[MatchIndex]?.Groups[UserOffsetGroupIndex]?.Value?.Trim();
            if (!string.IsNullOrWhiteSpace(userOffset)) currDateTimeOffsetUTC = GetCurrentDateWithUserOffset(userOffset, currDateTimeOffsetUTC);
            return currDateTimeOffsetUTC;
        }

        public static DateTimeOffset GetCurrentDateWithUserOffset(in string userOffset, DateTimeOffset currDateTimeOffsetUTC)
        {
            var offsetValue = userOffset.Substring(0, userOffset.Length - 1);
            var offsetType = userOffset.Substring(userOffset.Length - 1, 1);

            if (int.TryParse(offsetValue.Trim(), out var offset)) currDateTimeOffsetUTC = Helpers.DateTimeOffsetHelper.CalculateNewDateTimeOffset(currDateTimeOffsetUTC, offsetType, offset);

            return currDateTimeOffsetUTC;
        }

        public static string TransformToUserFormat(MatchCollection todayFuncMatch, DateTimeOffset currDateTimeOffsetUTC)
        {
            var userFormatting = todayFuncMatch[MatchIndex]?.Groups[UserFormatGroupIndex]?.Value?.Trim().TrimStart(_charsToRemove).Trim();
            return currDateTimeOffsetUTC.ToString(string.IsNullOrWhiteSpace(userFormatting) ? DEFAULT_TODAY_FUNC_OUTPUT_FORMAT : userFormatting);
        }
    }
}