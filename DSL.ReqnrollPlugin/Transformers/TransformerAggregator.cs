using Reqnroll;
using System.Collections.Generic;

namespace DSL.ReqnrollPlugin.Transformers
{
    public class TransformerAggregator : ITransformerAggregator
    {
        protected readonly LinkedList<ITransformer> _transformers = new LinkedList<ITransformer>();

        public TransformerAggregator(IUserVariableTransformer customVariableTransformer, IEnvironmentVariableTransformer environmentVariableTransformer, IFunctionTransformer functionTransformer)
        {
            _transformers.AddLast(environmentVariableTransformer);
            _transformers.AddLast(functionTransformer);
            _transformers.AddLast(customVariableTransformer);
        }

        public string Transform(string inputString, ScenarioContext context)
        {
            foreach (var transformer in _transformers) inputString = transformer.Transform(inputString, context);
            return inputString;
        }
    }
}