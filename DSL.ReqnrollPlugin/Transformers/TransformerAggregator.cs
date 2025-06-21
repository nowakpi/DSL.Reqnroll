using DSL.ReqnrollPlugin.Helpers;
using Reqnroll;
using System.Collections.Generic;
using System.Text;

namespace DSL.ReqnrollPlugin.Transformers
{
    public class TransformerAggregator : ITransformerAggregator
    {
        public ScenarioContext ScenarioContext { get; set; }

        protected readonly Dictionary<byte, ITransformer> _transformers = new Dictionary<byte, ITransformer>();

        public TransformerAggregator(IUserVariableTransformer userVariableTransformer, IEnvironmentVariableTransformer environmentVariableTransformer, IFunctionTransformer functionTransformer)
        {
            _transformers.Add(environmentVariableTransformer.OrderId, environmentVariableTransformer);
            _transformers.Add(functionTransformer.OrderId, functionTransformer);
            _transformers.Add(userVariableTransformer.OrderId, userVariableTransformer);
        }

        public string Transform(string inputString, ScenarioContext context)
        {
            if (string.IsNullOrEmpty(inputString)) { return null; }

            string statementId = TransformerSequenceGenerator.GetStatementId(inputString);
            StringBuilder transformerSequence = new StringBuilder();

            TransformableText? text;
            while ((text = TransformerSequenceGenerator.GetAnyTransformableText(inputString)) != null) 
            {
                var transformerId = ((TransformableText) text).Transformer;
                var transformer = _transformers[transformerId];

                transformerSequence.Append(transformerId);

                string newLeftSide = inputString.Substring(0, ((TransformableText)text).StartIndex);
                string newRightSide = inputString.Substring(((TransformableText)text).EndIndex + 1);

                inputString = newLeftSide + transformer.Transform(((TransformableText)text).Text, context) + newRightSide;
            }

            if (!string.IsNullOrWhiteSpace(statementId) && context != null) { context[statementId] = transformerSequence; }

            return inputString;
        }
    }
}