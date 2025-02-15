using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Reqnroll;

namespace DSL
{
    public interface IParameterTransformer
    {
        string Transform(in string pattern, in ScenarioContext context);
        IParameterTransformer AddBespokeTransformer(in Func<string, string> transformer);
        void ClearBespokeTransformers();
    }

    public class DSLParameterTransformer : IParameterTransformer
    {
        private readonly List<Func<string, string>> _bespokeTransformers = new List<Func<string, string>>();
        public IParameterTransformer AddBespokeTransformer(in Func<string, string> transformer)
        {
            _bespokeTransformers.Add(transformer);
            return this;
        }
        public void ClearBespokeTransformers()
        {
            _bespokeTransformers.Clear();
        }
        public virtual string Transform(in string inputString, in Reqnroll.ScenarioContext scenarioContext)
        {
            if (string.IsNullOrEmpty(inputString)) return inputString;
            // deal with multiple line text
            StringBuilder result = new StringBuilder();
            using (StringReader reader = new StringReader(inputString))
            {
                string line = string.Empty;
                do {
                    line = reader.ReadLine();
                    if (line != null) {
                        if (result.Length == 0) result.Append(TransformText(line, scenarioContext));
                        else result.Append(Environment.NewLine + TransformText(line, scenarioContext));
                    }

                } while (line != null);
            }

            return result.ToString();
        }
        protected virtual string TransformPattern(in string pattern, in ScenarioContext scenarioContext)
        {
            // supports [[key=value]] assignment
            var isAssignment = Regex.Match(pattern, @"(.*)=(.*)");
            if (isAssignment.Success)
            {
                // bottom up travese
                var cxtValue = TransformText(isAssignment.Groups[2]?.Value?.Trim(), scenarioContext);

                // apply user filter
                foreach (var transformer in _bespokeTransformers) cxtValue = transformer.Invoke(cxtValue);

                // apply RegEx
                var regExM = Regex.Match(cxtValue, @"RegEx\((.*)\)", RegexOptions.IgnoreCase);
                if (regExM.Success)
                {
                    cxtValue = new Fare.Xeger(regExM.Groups[1]?.Value).Generate();
                }

                var cxtKey = TransformText(isAssignment.Groups[1]?.Value?.Trim(), scenarioContext);
                scenarioContext[cxtKey] = cxtValue;

                return cxtValue;
            }
            else
            {
                // read value from scenario context
                try
                {
                    return scenarioContext[pattern] as string;
                }
                catch (KeyNotFoundException)
                {
                    throw new KeyNotFoundException("[DSL.ReqnrollPlugin] Can't find key:" + pattern + " in scenario context");
                }
            }
        }
        protected virtual string TransformText(in string inputString, in ScenarioContext scenarioContext)
        {
            if (string.IsNullOrEmpty(inputString)) return inputString;

            var match = PatternMatch.Parse(inputString);

            return match == null ? inputString
                : TransformText(match.ReplaceMatched(TransformPattern(match.MatchedPattern, scenarioContext)), scenarioContext);
        }

        internal class PatternMatch
        {
            string Prefix { get; set; }
            public string MatchedPattern { get; set; }
            string Postfix { get; set; }

            public static PatternMatch Parse(in string stringToMatch)
            {
                if (stringToMatch.IndexOf("[[") < 0 || stringToMatch.IndexOf("]]") < 0) return null;

                // regroup
                var startPattern = stringToMatch.IndexOf("[[") + 2;
                int endPattern = startPattern;
                var i = startPattern;
                int nested = 0;

                while (i++ < stringToMatch.Length)
                {
                    if (stringToMatch[i] == '[' && stringToMatch[i - 1] == '[')
                    {
                        i++;
                        nested++;
                    }
                    else if (stringToMatch[i] == ']' && stringToMatch[i - 1] == ']')
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
}
