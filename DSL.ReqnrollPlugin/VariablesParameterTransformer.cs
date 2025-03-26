using Reqnroll;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DSL.ReqnrollPlugin
{
    public abstract class VariablesParameterTransformer : IParameterTransformer
    {
        protected readonly List<Func<string, string>> _bespokeTransformers = new List<Func<string, string>>();
        
        protected abstract string TransformText(in string inputString, in ScenarioContext scenarioContext);

        public void ClearBespokeTransformers() => _bespokeTransformers.Clear();

        public IParameterTransformer AddBespokeTransformer(in Func<string, string> transformer)
        {
            _bespokeTransformers.Add(transformer);
            return this;
        }

        protected string ApplyBespokeTransformers(string pattern)
        {
            // apply user filter
            foreach (var transformer in _bespokeTransformers) pattern = transformer.Invoke(pattern);
            return pattern;
        }

        public virtual string Transform(in string inputString, in Reqnroll.ScenarioContext scenarioContext)
        {
            if (string.IsNullOrEmpty(inputString)) return inputString;

            StringBuilder result = new StringBuilder();
            ProcessMultipleLines(inputString, scenarioContext, result);

            return result.ToString();
        }

        private void ProcessMultipleLines(string inputString, ScenarioContext scenarioContext, StringBuilder stringBuilder)
        {
            using (StringReader reader = new StringReader(inputString))
            {
                string line; while ((line = reader.ReadLine()) != null)
                {
                    string preffix = stringBuilder.Length == 0 ? string.Empty : Environment.NewLine;
                    stringBuilder.Append(preffix + TransformText(line, scenarioContext));
                }
            }
        }
    }
}