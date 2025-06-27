using DSL.ReqnrollPlugin.Transformers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSL.ReqnrollPlugin.UnitTests
{
    public class TransformerAggregatorUnitTests
    {
        private readonly TransformerAggregator _transformerAggregator;

        public TransformerAggregatorUnitTests()
        {
            _transformerAggregator = new TransformerAggregator(new UserVariableTransformer(), new EnvironmentVariableTransformer(), new FunctionParameterTransformer());
        }

        [Fact]
        public void TransformText_Null()
        {
            string? input = null;

            Assert.Null(_transformerAggregator.Transform(input, null));
        }

        [Fact]
        public void TransformText_EmptyString()
        {
            string input = string.Empty;

            Assert.Equal(string.Empty, _transformerAggregator.Transform(input, null));
        }

        [Fact]
        public void TransformText_NonTransformableString()
        {
            string input = @"This text should not be transformed in any way
                              and this line should not be transformed as well.";

            Assert.Equal(input, _transformerAggregator.Transform(input, null));
        }
    }
}
