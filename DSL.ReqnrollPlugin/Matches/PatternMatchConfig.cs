using System;
using System.Collections.Generic;

namespace DSL.ReqnrollPlugin.Matches
{
    public class PatternMatchConfig
    {
        public string PrefixLong { get; set; }
        public char PrefixShort { get; set; }
        public string SuffixLong { get; set; }
        public char SuffixShort { get; set; }
        public byte MatchOrder { get; set; }

        public static PatternMatchConfig CustomVariablesMatchConfig => new PatternMatchConfig() { PrefixLong = "[[", PrefixShort = '[', SuffixLong = "]]", SuffixShort = ']', MatchOrder = 3 };
        public static PatternMatchConfig FunctionsMatchConfig => new PatternMatchConfig() { PrefixLong = "{{", PrefixShort = '{', SuffixLong = "}}", SuffixShort = '}', MatchOrder = 2 };
        public static PatternMatchConfig EnvironmentMatchConfig => new PatternMatchConfig() { PrefixLong = "((", PrefixShort = '(', SuffixLong = "))", SuffixShort = ')', MatchOrder = 1 };

        private static Dictionary<string, string> _prefixToSuffix = new Dictionary<string, string> { { CustomVariablesMatchConfig.PrefixLong, CustomVariablesMatchConfig.SuffixLong }, { FunctionsMatchConfig.PrefixLong, FunctionsMatchConfig.SuffixLong }, { EnvironmentMatchConfig.PrefixLong, EnvironmentMatchConfig.SuffixLong } };
        private static Dictionary<string, byte> _prefixToMatchOrder = new Dictionary<string, byte> { { CustomVariablesMatchConfig.PrefixLong, CustomVariablesMatchConfig.MatchOrder }, { FunctionsMatchConfig.PrefixLong, FunctionsMatchConfig.MatchOrder }, { EnvironmentMatchConfig.PrefixLong, EnvironmentMatchConfig.MatchOrder } };

        public static bool IsMatchPatternPrefix(string indicator) => indicator == CustomVariablesMatchConfig.PrefixLong || indicator == FunctionsMatchConfig.PrefixLong || indicator == EnvironmentMatchConfig.PrefixLong;
        public static bool IsMatchPatternSuffix(string indicator) => indicator == CustomVariablesMatchConfig.SuffixLong || indicator == FunctionsMatchConfig.SuffixLong || indicator == EnvironmentMatchConfig.SuffixLong;

        public static byte GetMatchOrder(string prefixLong)
        {
            return prefixLong.Equals(CustomVariablesMatchConfig.PrefixLong) ? CustomVariablesMatchConfig.MatchOrder
                : prefixLong.Equals(FunctionsMatchConfig.PrefixLong) ? FunctionsMatchConfig.MatchOrder
                : prefixLong.Equals(EnvironmentMatchConfig.PrefixLong) ? EnvironmentMatchConfig.MatchOrder : byte.MinValue;
        }

        public static bool DoesSuffixMatchPrefix(string lastOpenPattern, string buffer)
        {
            return _prefixToSuffix[lastOpenPattern] == buffer;
        }

        internal static byte GetTransformerForPrefix(string lastOpenPattern)
        {
            return _prefixToMatchOrder[lastOpenPattern];
        }
    }
}