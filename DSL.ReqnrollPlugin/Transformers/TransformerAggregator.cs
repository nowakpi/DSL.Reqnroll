using DSL.ReqnrollPlugin.Helpers;
using Reqnroll;
using System.Collections.Generic;

namespace DSL.ReqnrollPlugin.Transformers
{
    public class TransformerAggregator : ITransformerAggregator
    {
        public ScenarioContext ScenarioContext { get; set; }

        protected readonly Dictionary<byte, ITransformer> _transformers = new Dictionary<byte, ITransformer>();

        public TransformerAggregator(IUserVariableTransformer userVariableTransformer, IEnvironmentVariableTransformer environmentVariableTransformer, IFunctionTransformer functionTransformer)
        {
            _transformers.Add(environmentVariableTransformer.TransformerId, environmentVariableTransformer);
            _transformers.Add(functionTransformer.TransformerId, functionTransformer);
            _transformers.Add(userVariableTransformer.TransformerId, userVariableTransformer);
        }

        public string Transform(string inputString, ScenarioContext context)
        {
            if (string.IsNullOrEmpty(inputString)) { return null; }

            string statementId = TransformerSequenceGenerator.GetStatementId(inputString);
            byte[] transformerSequence = TransformerSequenceGenerator.GetTransformerSequence(inputString);

            if (!string.IsNullOrWhiteSpace(statementId) && context != null) { context[statementId] = transformerSequence; }

            foreach (var transformerId in transformerSequence)
            {
                var transformer = _transformers[transformerId];
                inputString = transformer.Transform(inputString, context);
            }

            return inputString;
        }
    }
}