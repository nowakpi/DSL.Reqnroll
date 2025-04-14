using System.Text.RegularExpressions;

namespace DSL.ReqnrollPlugin
{
    public static class RegexMatch
    {
        public const string ASSIGNEMENT_PATTERN = @"(.*)=(.*)";
        public const string TODAY_FUNC_PATTERN = @"(?:([LU]#))?TODAY(?:([+-]\d+[yMd]))?(?:(#[dMyHhmsftz.\s\-\\\/\:]*))?";
        public const string REGEX_PATTERN = @"RegEx\((.*)\)";



        public static Match MatchAssignement(in string inputString) => Regex.Match(inputString, ASSIGNEMENT_PATTERN);
        public static Match MatchRegex(in string inputString) => Regex.Match(inputString, REGEX_PATTERN, RegexOptions.IgnoreCase);
        public static MatchCollection MatchDateFunction(in string inputString) => Regex.Matches(inputString, TODAY_FUNC_PATTERN);
    }
}
