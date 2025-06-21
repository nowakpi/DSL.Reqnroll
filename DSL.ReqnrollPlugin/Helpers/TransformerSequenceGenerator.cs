using DSL.ReqnrollPlugin.Matches;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DSL.ReqnrollPlugin.Helpers
{
    public static class TransformerSequenceGenerator
    {
        private static readonly byte[] DEFAULT_TRANSFORMER_SEQUENCE = { PatternMatchConfig.EnvironmentMatchConfig.MatchOrder  , PatternMatchConfig.FunctionsMatchConfig.MatchOrder, PatternMatchConfig.CustomVariablesMatchConfig.MatchOrder };
        private static readonly string KEY_SEPARATOR = "_";

        private static string GetStatementId(in string inputStatement)
        {
            byte[] statementBytes = Encoding.UTF8.GetBytes(inputStatement);

            SHA256 sha256Algorithm = SHA256.Create();
            byte[] hashValue = sha256Algorithm.ComputeHash(statementBytes);

            return BitConverter.ToString(hashValue).Replace("-", "");
        }

        private static bool WasTextAlreadyTransformed(string statementId, TransformableText transformableText, Dictionary<string, object> scenarioContext)
        {
            string key = statementId + KEY_SEPARATOR + transformableText.StartIndex.ToString() + KEY_SEPARATOR + transformableText.EndIndex + KEY_SEPARATOR + transformableText.Text;
            return scenarioContext.ContainsKey(key);
        }

        private static void RegisterTransformedText(string statementId, TransformableText transformableText, Dictionary<string, object> scenarioContext)
        {
            string key = statementId + KEY_SEPARATOR + transformableText.StartIndex.ToString() + KEY_SEPARATOR + transformableText.EndIndex + KEY_SEPARATOR + transformableText.Text;
            scenarioContext.Add(key, true);
        }

        public static TransformableText? GetAnyTransformableText(in string inputStatement, Dictionary<string, object> scenarioContext)
        {
            if (inputStatement == null || string.IsNullOrWhiteSpace(inputStatement)) return null;

            TransformableText? result = null;
            string lastOpenPattern = null;
            int lastOpenPatternIndex = 0;
            string statementId = GetStatementId(inputStatement);

            for (int index = 0; index < inputStatement.Length - 1; index++)
            {
                string buffer = inputStatement.Substring(index, 2);

                if (PatternMatchConfig.IsMatchPatternPrefix(buffer)) 
                { 
                    lastOpenPattern = buffer;
                    lastOpenPatternIndex = index;
                } 
                else if (PatternMatchConfig.IsMatchPatternSuffix(buffer))
                {
                    if (lastOpenPattern != null && PatternMatchConfig.DoesSuffixMatchPrefix(lastOpenPattern, buffer))
                    {
                        TransformableText textCandidate = new TransformableText
                        {
                            StartIndex = lastOpenPatternIndex,
                            EndIndex = index + 1,
                            Text = inputStatement.Substring(lastOpenPatternIndex, 2 + index - lastOpenPatternIndex),
                            TransformerId = PatternMatchConfig.GetTransformerForPrefix(lastOpenPattern)
                        };

                        if (!WasTextAlreadyTransformed(statementId, textCandidate, scenarioContext)) 
                        {
                            result = textCandidate;
                            RegisterTransformedText(statementId, textCandidate, scenarioContext);
                            break;
                        }
                    } 
                }
            }

            return result;
        }
    }
}
