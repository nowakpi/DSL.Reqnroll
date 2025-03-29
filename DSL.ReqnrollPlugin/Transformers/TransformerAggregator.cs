using Reqnroll;
using System.Collections.Generic;

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

        public string Transform(string inputString, ScenarioContext context)
        {
            foreach (var transformer in _transformers) inputString = transformer.Transform(inputString, context);
            return inputString;
        }
    }
}