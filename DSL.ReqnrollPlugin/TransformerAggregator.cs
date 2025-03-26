using Reqnroll;
using System.Collections.Generic;
using System;

namespace DSL.ReqnrollPlugin
{
    public class TransformerAggregator : ITransformerAggregator
    {
        protected readonly LinkedList<ITransformer> _transformers = new LinkedList<ITransformer>();

        public TransformerAggregator(IParameterTransformer customVariableTransformer, IEnvironmentVariableTransformer environmentVariableTransformer)
        {
            _transformers.AddLast(environmentVariableTransformer);
            _transformers.AddLast(customVariableTransformer);
        }

        public string Transform(in string pattern, in ScenarioContext context)
        {
            string result = pattern;
            
            foreach (var transformer in _transformers) result = transformer.Transform(result, context);
            return result;
        }
    }
}