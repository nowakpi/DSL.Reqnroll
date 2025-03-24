using Xunit;

namespace DSL.ReqnrollPlugin.UnitTests
{
    public class UserVariableTransformerTests
    {
        private readonly UserVariableTransformer _transformer;
        private readonly Dictionary<string, object> _scenarioContext;

        public UserVariableTransformerTests()
        {
            _transformer = new UserVariableTransformer();
            _scenarioContext = new Dictionary<string, object>();
        }

        [Fact]
        public void TransformText_NullOrEmptyInput_ReturnsInput()
        {
            Assert.Null(_transformer.TransformText(null, _scenarioContext));
            Assert.Equal(string.Empty, _transformer.TransformText(string.Empty, _scenarioContext));
        }

        [Fact]
        public void TransformText_NoMatchingPattern_ReturnsInput()
        {
            var input = "Hello, World!";
            Assert.Equal(input, _transformer.TransformText(input, _scenarioContext));
        }

        [Fact]
        public void TransformText_SimpleAssignment_AddsToContext()
        {
            var input = "[[key=value]]";
            var result = _transformer.TransformText(input, _scenarioContext);

            Assert.Equal("value", result);
            Assert.Equal("value", _scenarioContext["key"]);
        }

        [Fact]
        public void TransformText_NestedAssignment_ResolvesCorrectly()
        {
            _scenarioContext["innerKey"] = "innerValue";
            var input = "[[outerKey=[[innerKey]]]]";
            var result = _transformer.TransformText(input, _scenarioContext);

            Assert.Equal("innerValue", result);
            Assert.Equal("innerValue", _scenarioContext["outerKey"]);
        }

        [Fact]
        public void TransformText_RegExPattern_GeneratesMatchingString()
        {
            var input = "[[key=RegEx([a-z]{5})]]";
            var result = _transformer.TransformText(input, _scenarioContext);

            Assert.Matches(@"^[a-z]{5}$", result);
            Assert.Matches(@"^[a-z]{5}$", _scenarioContext["key"] as string);
        }

        [Fact]
        public void TransformPattern_ExistingKey_ReturnsValue()
        {
            _scenarioContext["[[existingKey]]"] = "existingValue";
            var result = _transformer.TransformPattern("[[existingKey]]", _scenarioContext);

            Assert.Equal("existingValue", result);
        }

        [Fact]
        public void TransformPattern_NonExistingKey_ThrowsKeyNotFoundException()
        {
            Assert.Throws<KeyNotFoundException>(() => _transformer.TransformPattern("nonExistingKey", _scenarioContext));
        }

        [Fact]
        public void TransformText_MultiplePatterns_ResolvesAll()
        {
            _scenarioContext["key1"] = "value1";
            _scenarioContext["key2"] = "value2";
            var input = "Hello [[key1]], how are you [[key2]]?";
            var result = _transformer.TransformText(input, _scenarioContext);

            Assert.Equal("Hello value1, how are you value2?", result);
        }
    }
}
