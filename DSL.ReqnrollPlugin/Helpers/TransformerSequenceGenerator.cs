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

        public static string GetStatementId(in string inputStatement)
        {
            byte[] statementBytes = Encoding.UTF8.GetBytes(inputStatement);

            SHA256 sha256Algorithm = SHA256.Create();
            byte[] hashValue = sha256Algorithm.ComputeHash(statementBytes);

            return BitConverter.ToString(hashValue).Replace("-", "");
        }

        public static TransformableText? GetAnyTransformableText(in string inputStatement)
        {
            if (inputStatement == null || string.IsNullOrWhiteSpace(inputStatement)) return null;

            TransformableText? result = null;
            string lastOpenPattern = null;
            int lastOpenPatternIndex = 0;

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
                        result = new TransformableText
                        {
                            StartIndex = lastOpenPatternIndex,
                            EndIndex = index + 2,
                            Text = inputStatement.Substring(lastOpenPatternIndex, 2 + index - lastOpenPatternIndex),
                            Transformer = PatternMatchConfig.GetTransformerForPrefix(lastOpenPattern)
                        };
                        break;
                    } 
                }
            }

            return result;
        }
         
        public static byte[] GetTransformerSequence(in string inputStatement)
        {
            Stack<string> transformersRequested = new Stack<string>();
            int positionInInputStatement = 0;

            while (positionInInputStatement < inputStatement.Length)
            {
                var indicator = inputStatement.Substring(positionInInputStatement, inputStatement.Length - positionInInputStatement > 1 ? 2 : 1);
                if (PatternMatchConfig.IsMatchPatternPrefix(indicator)) 
                {
                    transformersRequested.Push(indicator);
                    positionInInputStatement += 2;
                }
                else positionInInputStatement += 1;
            }

            List<byte> matchOrders = new List<byte>();
            while (transformersRequested.Count > 0)
            {
                var indicator = transformersRequested.Pop();
                var matchOrder = PatternMatchConfig.GetMatchOrder(indicator);
                if (matchOrder > byte.MinValue) matchOrders.Add(matchOrder);
            }

            return matchOrders.Count > 0 ? matchOrders.ToArray() : DEFAULT_TRANSFORMER_SEQUENCE;
        }
    }
}
