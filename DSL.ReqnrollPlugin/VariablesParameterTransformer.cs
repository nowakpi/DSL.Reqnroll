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
        
        public abstract string TransformText(in string inputString, in ScenarioContext scenarioContext);
        
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
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        if (result.Length == 0) result.Append(TransformText(line, scenarioContext));
                        else result.Append(Environment.NewLine + TransformText(line, scenarioContext));
                    }

                } while (line != null);
            }

            return result.ToString();
        }
    }
}