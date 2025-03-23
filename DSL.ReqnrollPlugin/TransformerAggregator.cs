using Reqnroll;
using System.Collections.Generic;
using System;

namespace DSL.ReqnrollPlugin
{
    public class TransformerAggregator : ITransformerAggregator
    {
        protected readonly List<ITransformer> _transformers = new List<ITransformer>();

        public TransformerAggregator(IParameterTransformer customVariableTransformer, IEnvironmentVariableTransformer environmentVariableTransformer)
        {
            _transformers.Add(customVariableTransformer);
            _transformers.Add(environmentVariableTransformer);
        }

        public string Transform(in string pattern, in ScenarioContext context)
        {
            string result = pattern;
            
            foreach (var transformer in _transformers) result = transformer.Transform(result, context);
            return result;
        }
    }
}