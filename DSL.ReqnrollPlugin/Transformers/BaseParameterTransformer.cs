using Reqnroll;
using System;
using System.IO;
using System.Text;

namespace DSL.ReqnrollPlugin
{
    public abstract class BaseParameterTransformer : ITransformer
    {
        protected abstract string TransformText(in string inputString, in ScenarioContext scenarioContext);

        public virtual string Transform(in string inputString, in ScenarioContext scenarioContext)
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