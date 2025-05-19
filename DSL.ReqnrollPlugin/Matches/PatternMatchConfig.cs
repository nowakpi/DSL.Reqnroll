using System;

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

        public static bool IsMatchPattern(string indicator) => indicator == CustomVariablesMatchConfig.PrefixLong || indicator == FunctionsMatchConfig.PrefixLong || indicator == EnvironmentMatchConfig.PrefixLong;
        public static byte GetMatchOrder(string prefixLong)
        {
            return prefixLong.Equals(CustomVariablesMatchConfig.PrefixLong) ? CustomVariablesMatchConfig.MatchOrder
                : prefixLong.Equals(FunctionsMatchConfig.PrefixLong) ? FunctionsMatchConfig.MatchOrder
                : prefixLong.Equals(EnvironmentMatchConfig.PrefixLong) ? EnvironmentMatchConfig.MatchOrder : byte.MinValue;
        }
    }
}
