using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSL.ReqnrollPlugin
{
    internal class PatternMatchConfig
    {
        public string PrefixLong { get; set; }
        public char PrefixShort { get; set; }
        public string SuffixLong {  get; set; }
        public char SuffixShort { get; set; }

        public static PatternMatchConfig CustomVariablesMatchConfig => new PatternMatchConfig() { PrefixLong = "[[", PrefixShort = '[', SuffixLong = "]]", SuffixShort = ']' };
        public static PatternMatchConfig FunctionsMatchConfig => new PatternMatchConfig() { PrefixLong = "{{", PrefixShort = '{', SuffixLong = "}}", SuffixShort = '}' };
        public static PatternMatchConfig EnvironmentMatchConfig => new PatternMatchConfig() { PrefixLong = "((", PrefixShort = '(', SuffixLong = "))", SuffixShort = ')' };
    }
}
