using System.Text.RegularExpressions;

namespace DSL.ReqnrollPlugin.Matches
{
    public static class UserVariableMatchInterpreter
    {
        public static int VariableNameGroupIndex { get => 1; }
        public static int VariableValueGroupIndex { get => 2; }
        public static int CustomRegexGroupIndex { get => 1; }

        public static string ApplyCustomRegex(string custVariableValue)
        {
            var regExM = RegexMatch.MatchRegex(custVariableValue);
            return regExM.Success ? new Fare.Xeger(regExM.Groups[CustomRegexGroupIndex]?.Value).Generate() : custVariableValue;
        }

        public static string GetCustomVariableName(Match isAssignment)
        {
            return isAssignment.Groups[VariableNameGroupIndex]?.Value?.Trim();
        }

        public static string GetCustomVariableValue(Match isAssignment)
        {
            return isAssignment.Groups[VariableValueGroupIndex]?.Value?.Trim();
        }
    }
}