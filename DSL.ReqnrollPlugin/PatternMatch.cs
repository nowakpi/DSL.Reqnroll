using System.Runtime.CompilerServices;

namespace DSL.ReqnrollPlugin
{
    public class PatternMatch
    {
        public string Prefix { get; set; }
        public string MatchedPattern { get; set; }
        public string Postfix { get; set; }

        public static PatternMatch Parse(in string stringToMatch, in PatternMatchConfig config)
        {
            if (config == null || stringToMatch.IndexOf(config.PrefixLong) < 0 || stringToMatch.IndexOf(config.SuffixLong) < 0) return null;

            // regroup
            var startPattern = stringToMatch.IndexOf(config.PrefixLong) + 2;
            int endPattern = startPattern;
            var i = startPattern;
            int nested = 0;

            while (i++ < stringToMatch.Length)
            {
                if (stringToMatch[i] == config.PrefixShort && stringToMatch[i - 1] == config.PrefixShort)
                {
                    i++;
                    nested++;
                }
                else if (stringToMatch[i] == config.SuffixShort && stringToMatch[i - 1] == config.SuffixShort)
                {
                    if (nested == 0)
                    {
                        endPattern = i - 2;
                        break;
                    }
                    else
                    {
                        i++;
                        nested--;
                    }
                }
            }

            return new PatternMatch()
            {
                Prefix = stringToMatch.Substring(0, startPattern - 2),
                MatchedPattern = stringToMatch.Substring(startPattern, endPattern - startPattern + 1),
                Postfix = stringToMatch.Substring(endPattern + 3),
            };
        }

        public string ReplaceMatched(in string replace)
        {
            return Prefix + replace + Postfix;
        }
    }
}
