using DSL.ReqnrollPlugin.Helpers;
using Reqnroll.CommonModels;

namespace DSL.ReqnrollPlugin.UnitTests
{
    public class TransformerSequenceGeneratorUnitTests
    {
        private readonly Dictionary<string, object> _scenarioContext = new Dictionary<string, object>();

        [Fact]
        public void GetAnyTransformableText_NullInput_ReturnsNull()
        {
            var result = TransformerSequenceGenerator.GetAnyTransformableText(null, _scenarioContext);
            Assert.Null(result);
        }

        [Fact]
        public void GetAnyTransformableText_EmptyInput_ReturnsNull()
        {
            var result = TransformerSequenceGenerator.GetAnyTransformableText("", _scenarioContext);
            Assert.Null(result);
        }

        [Fact]
        public void GetAnyTransformableText_WhitespaceInput_ReturnsNull()
        {
            var result = TransformerSequenceGenerator.GetAnyTransformableText("   ", _scenarioContext);
            Assert.Null(result);
        }

        [Fact]
        public void GetAnyTransformableText_NoMatchingPatterns_ReturnsNull()
        {
            var result = TransformerSequenceGenerator.GetAnyTransformableText("no patterns here", _scenarioContext);
            Assert.Null(result);
        }

        [Fact]
        public void GetAnyTransformableText_SingleEnvironmentPattern_ReturnsCorrectText()
        {
            var input = "(( text ))";
            var result = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);

            if (result != null)
            {
                var text = (TransformableText) result;
                Assert.Equal("(( text ))", text.Text);
                Assert.Equal(1, text.TransformerId);
                Assert.Equal(0, text.StartIndex);
                Assert.Equal(9, text.EndIndex);
            } else Assert.Fail();
        }

        [Fact]
        public void GetAnyTransformableText_SingleFunctionPattern_ReturnsCorrectText()
        {
            var input = "{{ function }}";
            var result = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);

            if (result != null)
            {
                var text = (TransformableText)result;
                Assert.Equal("{{ function }}", text.Text);
                Assert.Equal(2, text.TransformerId);
                Assert.Equal(0, text.StartIndex);
                Assert.Equal(13, text.EndIndex);
            }
            else Assert.Fail();
        }

        [Fact]
        public void GetAnyTransformableText_SingleCustomVariablePattern_ReturnsCorrectText()
        {
            var input = "[[ custom ]]";
            var result = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);

            if (result != null)
            {
                var text = (TransformableText)result;
                Assert.Equal("[[ custom ]]", text.Text);
                Assert.Equal(3, text.TransformerId);
                Assert.Equal(0, text.StartIndex);
                Assert.Equal(11, text.EndIndex);
            }
            else Assert.Fail();
        }

        [Fact]
        public void GetAnyTransformableText_MultiplePatterns_ReturnsFirstFound()
        {
            var input = "[[ dd ((  ))  dd {{  dd }} ]] [[ eee ]]";
            var result = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);

            if (result != null)
            {
                var text = (TransformableText)result;
                Assert.Equal("((  ))", text.Text);
                Assert.Equal(1, text.TransformerId);
            }
            else Assert.Fail();
        }

        [Fact]
        public void GetAnyTransformableText_SequentialPatterns_ReturnsFirstPattern()
        {
            var input = "(( env )) {{ func }} [[ var ]]";
            var result = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);

            if (result != null)
            {
                var text = (TransformableText)result;
                Assert.Equal("(( env ))", text.Text);
                Assert.Equal(1, text.TransformerId);
            }
            else Assert.Fail();
        }

        [Fact]
        public void GetAnyTransformableText_NestedPatterns_ReturnsFirstCompletePattern()
        {
            var input = "(( sdff {{ ddd {{ ddd }} }}  dd ))";
            var result = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);

            if (result != null)
            {
                var text = (TransformableText)result;
                Assert.Equal("{{ ddd }}", text.Text);
                Assert.Equal(2, text.TransformerId);
            }
            else Assert.Fail();
        }

        [Fact]
        public void GetAnyTransformableText_DeeplyNestedPatterns_ReturnsInnermost()
        {
            var input = "[[ outer {{ middle (( inner )) middle }} outer ]]";
            var result = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);

            if (result != null)
            {
                var text = (TransformableText)result;
                Assert.Equal("(( inner ))", text.Text);
                Assert.Equal(1, text.TransformerId);
            }
            else Assert.Fail();
        }

        [Fact]
        public void GetAnyTransformableText_AlreadyTransformedText_ReturnsNull()
        {
            var input = "(( text ))";

            // First call should return the pattern
            var firstResult = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);
            Assert.NotNull(firstResult);

            // Second call should return null as it's already transformed
            var secondResult = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);
            Assert.Null(secondResult);
        }

        [Fact]
        public void GetAnyTransformableText_DifferentTextsWithSamePattern_BothTransformed()
        {
            var input1 = "(( text1 ))";
            var input2 = "(( text2 ))";

            var result1 = TransformerSequenceGenerator.GetAnyTransformableText(input1, _scenarioContext);
            var result2 = TransformerSequenceGenerator.GetAnyTransformableText(input2, _scenarioContext);

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotEqual(((TransformableText)result1).Text, ((TransformableText)result2).Text);
        }

        [Fact]
        public void GetAnyTransformableText_SameTextDifferentPositions_BothTransformed()
        {
            var input = "(( text )) some other content (( text ))";

            var result1 = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);
            Assert.NotNull(result1);
            Assert.Equal(0, ((TransformableText)result1).StartIndex);

            var result2 = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);
            Assert.NotNull(result2);
            Assert.Equal(30, ((TransformableText)result2).StartIndex);
        }

        [Fact]
        public void GetAnyTransformableText_IncompletePattern_ReturnsNull()
        {
            var input = "(( incomplete";
            var result = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);
            Assert.Null(result);
        }

        [Fact]
        public void GetAnyTransformableText_MismatchedPatterns_ReturnsNull()
        {
            var input = "(( mismatched }}";
            var result = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);
            Assert.Null(result);
        }

        [Fact]
        public void GetAnyTransformableText_OnlyPrefixes_ReturnsNull()
        {
            var input = "(( {{ [[";
            var result = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);
            Assert.Null(result);
        }

        [Fact]
        public void GetAnyTransformableText_OnlySuffixes_ReturnsNull()
        {
            var input = "]] }} ))";
            var result = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);
            Assert.Null(result);
        }

        [Fact]
        public void GetAnyTransformableText_MinimalValidPattern_ReturnsCorrectText()
        {
            var input = "(())";
            var result = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);

            if (result != null)
            {
                var text = (TransformableText)result;
                Assert.Equal("(())", text.Text);
                Assert.Equal(1, text.TransformerId);
            }
            else Assert.Fail();
        }

        [Fact]
        public void GetAnyTransformableText_PatternAtEndOfString_ReturnsCorrectText()
        {
            var input = "some text (( pattern ))";
            var result = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);

            if (result != null)
            {
                var text = (TransformableText)result;
                Assert.Equal("(( pattern ))", text.Text);
                Assert.Equal(1, text.TransformerId);
                Assert.Equal(10, text.StartIndex);
            }
            else Assert.Fail();
        }

        [Fact]
        public void GetAnyTransformableText_OverlappingPatterns_ReturnsFirstComplete()
        {
            var input = "(({{ overlapping }}))";
            var result = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);

            if (result != null)
            {
                var text = (TransformableText)result;
                Assert.Equal("{{ overlapping }}", text.Text);
                Assert.Equal(2, text.TransformerId);
            }
            else Assert.Fail();
        }

        [Fact]
        public void GetAnyTransformableText_MultipleCallsWithMultiplePatterns_ReturnsSequentially()
        {
            var input = "(( first )) {{ second }} [[ third ]]";

            var result1 = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);
            Assert.NotNull(result1);
            Assert.Equal("(( first ))", ((TransformableText)result1).Text);

            var result2 = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);
            Assert.NotNull(result2);
            Assert.Equal("{{ second }}", ((TransformableText)result2).Text);

            var result3 = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);
            Assert.NotNull(result3);
            Assert.Equal("[[ third ]]", ((TransformableText)result3).Text);

            var result4 = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);
            Assert.Null(result4);
        }

        [Fact]
        public void GetAnyTransformableText_LongTextWithMultiplePatterns_HandlesCorrectly()
        {
            var input = "This is a long text with (( environment )) variables and {{ function }} calls and [[ custom ]] variables scattered throughout the text.";

            var result = TransformerSequenceGenerator.GetAnyTransformableText(input, _scenarioContext);
            Assert.NotNull(result);
            Assert.Equal("(( environment ))", ((TransformableText)result).Text);
        }
    }
}
