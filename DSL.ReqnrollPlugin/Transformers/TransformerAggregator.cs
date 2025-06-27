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
            if (string.IsNullOrEmpty(inputString)) { return inputString; }
            
            TransformableText? text;
            while ((text = TransformerSequenceGenerator.GetAnyTransformableText(inputString, context)) != null) 
            {
                TransformableText transformableText = (TransformableText) text;
                var transformer = _transformers[transformableText.TransformerId];

                string newLeftSide = inputString.Substring(0, transformableText.StartIndex);
                string newRightSide = inputString.Substring(transformableText.EndIndex + 1);

                inputString = newLeftSide + transformer.Transform(transformableText.Text, context) + newRightSide;
            }

            return inputString;
        }
    }
}