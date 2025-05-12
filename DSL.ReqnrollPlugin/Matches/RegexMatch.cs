using System;
using System.Text.RegularExpressions;

namespace DSL.ReqnrollPlugin.Matches
{
    public static class RegexMatch
    {
        private static readonly TimeSpan DEFAULT_REGEX_PARSE_TIMEOUT = TimeSpan.FromSeconds(1);
        
        public const string ASSIGNEMENT_PATTERN = @"(.*)=(.*)";
        public const string TODAY_FUNC_PATTERN = @"(?:([LU]#))?TODAY(?:([+-]\d+[yMdHms]))?(?:(#[dMyHhmsftz.\s\-\\\/\:]*))?";
        public const string RANDOM_FUNC_PATTERN = @"(RANDOM)(?:(:\d+:\d+))?";
        public const string REGEX_PATTERN = @"RegEx\((.*)\)";

        public static Match MatchAssignement(in string inputString) => Regex.Match(inputString, ASSIGNEMENT_PATTERN, RegexOptions.None, DEFAULT_REGEX_PARSE_TIMEOUT);
        public static Match MatchRegex(in string inputString) => Regex.Match(inputString, REGEX_PATTERN, RegexOptions.IgnoreCase, DEFAULT_REGEX_PARSE_TIMEOUT);
        public static MatchCollection MatchRandomFunction(in string inputString) => Regex.Matches(inputString, RANDOM_FUNC_PATTERN, RegexOptions.None, DEFAULT_REGEX_PARSE_TIMEOUT);
        public static MatchCollection MatchTodateFunction(in string inputString) => Regex.Matches(inputString, TODAY_FUNC_PATTERN, RegexOptions.None, DEFAULT_REGEX_PARSE_TIMEOUT);
    }
}
