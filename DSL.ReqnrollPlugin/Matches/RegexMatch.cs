using System.Text.RegularExpressions;

namespace DSL.ReqnrollPlugin
{
    public static class RegexMatch
    {
        public const string ASSIGNEMENT_PATTERN = @"(.*)=(.*)";
        public const string DATE_FUNC_PATTERN = @"TODAY(?:([+-]\d+[yMd]))?|(?:([dMyHhmsftz.\s\-\\\/\:]*))?";
        private const string REGEX_PATTERN = @"RegEx\((.*)\)";

        public static Match MatchAssignement(in string inputString) => Regex.Match(inputString, ASSIGNEMENT_PATTERN);
        public static MatchCollection MatchDateFunction(in string inputString) => Regex.Matches(inputString, DATE_FUNC_PATTERN);
        public static Match MatchRegex(in string inputString) => Regex.Match(inputString, REGEX_PATTERN, RegexOptions.IgnoreCase);
    }
}
