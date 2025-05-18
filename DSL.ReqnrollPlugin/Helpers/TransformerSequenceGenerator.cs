using DSL.ReqnrollPlugin.Matches;
using System.Collections.Specialized;
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
            return Encoding.UTF8.GetString(hashValue);
        }
         
        public static byte[] GetTransformerSequence(in string inputStatement)
        {
            return DEFAULT_TRANSFORMER_SEQUENCE;
        }
    }
}
