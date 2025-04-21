using System;
using System.Text.RegularExpressions;

namespace DSL.ReqnrollPlugin.Matches
{
    public static class RandomFuncMatchInterpreter
    {
        private static readonly char[] _splitChars = { RandomFuncMatchInterpreter.OptionSeparator };
        
        private const int DEFAULT_RANGE_FROM = 1;
        private const int DEFAULT_RANGE_TO = Int32.MaxValue;
        private const int CORRECT_ARRAY_LENGTH = 3;
        private const int RANGE_FROM_INDEX = 1;
        private const int RANGE_TO_INDEX = 2;

        public static int MatchIndex { get => 0; }
        public static int RangeGroupIndex { get => 2; }
        public static char OptionSeparator { get => ':'; }
        public static string FuncName { get => "RANDOM"; }

        public static (int, int) GetRandomRange(MatchCollection randomFunc)
        {
            var userRange = randomFunc[MatchIndex]?.Groups[RangeGroupIndex]?.Value?.Trim();
            if (string.IsNullOrWhiteSpace(userRange)) return (DEFAULT_RANGE_FROM, DEFAULT_RANGE_TO);

            var rangeValues = userRange?.Split(_splitChars);
            if (rangeValues.Length != CORRECT_ARRAY_LENGTH) return (DEFAULT_RANGE_FROM, DEFAULT_RANGE_TO);

            if (int.TryParse(rangeValues[RANGE_FROM_INDEX], out var rangeFrom) && int.TryParse(rangeValues[RANGE_TO_INDEX], out var rangeTo)) return (rangeFrom, rangeTo);
            else return (DEFAULT_RANGE_FROM, DEFAULT_RANGE_TO);
        }
    }
}
