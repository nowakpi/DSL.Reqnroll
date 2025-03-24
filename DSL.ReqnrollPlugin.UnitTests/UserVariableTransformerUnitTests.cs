using Reqnroll;
using Moq;
using System.Text.RegularExpressions;

namespace DSL.ReqnrollPlugin.UnitTests
{
    public class UserVariableTransformerTests
    {
        private readonly UserVariableTransformer _transformer;
        private readonly Mock<ScenarioContext> _mockScenarioContext;

        public UserVariableTransformerTests()
        {
            _transformer = new UserVariableTransformer();
            _mockScenarioContext = new Mock<ScenarioContext>();
        }

        [Fact]
        public void TransformText_NullOrEmptyInput_ReturnsInput()
        {
            Assert.Null(_transformer.TransformText(null, _mockScenarioContext.Object));
            Assert.Equal(string.Empty, _transformer.TransformText(string.Empty, _mockScenarioContext.Object));
        }

        [Fact]
        public void TransformText_NoMatchingPattern_ReturnsInput()
        {
            var input = "Hello, world!";
            Assert.Equal(input, _transformer.TransformText(input, _mockScenarioContext.Object));
        }

        [Fact]
        public void TransformText_MatchingPattern_ReplacesVariable()
        {
            _mockScenarioContext.Setup(c => c["name"]).Returns("John");
            var input = "Hello, [[name]]!";
            var expected = "Hello, John!";
            Assert.Equal(expected, _transformer.TransformText(input, _mockScenarioContext.Object));
        }

        [Fact]
        public void TransformText_AssignmentPattern_SetsContextVariable()
        {
            var input = "[[greeting=Hello]]";
            _transformer.TransformText(input, _mockScenarioContext.Object);
            _mockScenarioContext.VerifySet(c => c["greeting"] = "Hello", Times.Once);
        }

        [Fact]
        public void TransformText_NestedPatterns_ResolvesCorrectly()
        {
            _mockScenarioContext.Setup(c => c["name"]).Returns("John");
            _mockScenarioContext.Setup(c => c["greeting"]).Returns("Hello");
            var input = "[[greeting]], [[name]]!";
            var expected = "Hello, John!";
            Assert.Equal(expected, _transformer.TransformText(input, _mockScenarioContext.Object));
        }

        [Fact]
        public void TransformPattern_UnknownKey_ThrowsKeyNotFoundException()
        {
            _mockScenarioContext.Setup(c => c["unknownKey"]).Throws<KeyNotFoundException>();
            Assert.Throws<KeyNotFoundException>(() => _transformer.TransformText("[[unknownKey]]", _mockScenarioContext.Object));
        }

        [Fact]
        public void TransformPattern_RegExPattern_GeneratesMatchingString()
        {
            var input = "[[value=RegEx([A-Z]{3}\\d{3})]]";
            var result = _transformer.TransformText(input, _mockScenarioContext.Object);
            Assert.Matches(new Regex("[A-Z]{3}\\d{3}"), result);
        }
    }
}
