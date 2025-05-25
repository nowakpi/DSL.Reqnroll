using DSL.ReqnrollPlugin.Transformers;

namespace DSL.ReqnrollPlugin.UnitTests
{
    public class EnvironmentVariableTransformerUnitTests
    {
        private readonly EnvironmentVariableTransformer _transformer;

        public EnvironmentVariableTransformerUnitTests()
        {
            _transformer = new EnvironmentVariableTransformer();
        }

        [Fact]
        public void GetEnvironmentVariable_NullKey_ReturnsNull()
        {
            Assert.Null(_transformer.GetEnvironmentVariable(null));
        }

        [Fact]
        public void GetEnvironmentVariable_ValidKey_ReturnsValue()
        {
            Environment.SetEnvironmentVariable("TEST_VAR", "TestValue");
            Assert.Equal("TestValue", _transformer.GetEnvironmentVariable("TEST_VAR"));
            Environment.SetEnvironmentVariable("TEST_VAR", null); // Clean up
        }

        [Fact]
        public void TransformText_NullOrEmptyInput_ReturnsInput()
        {
            Assert.Null(_transformer.TransformText(null));
            Assert.Equal(string.Empty, _transformer.TransformText(string.Empty));
        }

        [Fact]
        public void TransformText_NoMatchingPattern_ReturnsInput()
        {
            var input = "Hello, world!";
            Assert.Equal(input, _transformer.TransformText(input));
        }

        [Fact]
        public void TransformText_MatchingPattern_ReplacesEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("USER_NAME", "John");
            var input = "Hello, ((USER_NAME))!";
            var expected = "Hello, John!";
            Assert.Equal(expected, _transformer.TransformText(input));
            Environment.SetEnvironmentVariable("USER_NAME", null); // Clean up
        }

        [Fact]
        public void TransformText_NonExistentEnvironmentVariable_ReturnsInput()
        {
            var input = "Hello, ((ON_EXISTENT_VAR))!";
            Assert.Equal(input, _transformer.TransformText(input));
        }

        [Fact]
        public void TransformText_NestedPatterns_ResolvesCorrectly()
        {
            Environment.SetEnvironmentVariable("GREETING", "((USER_NAME))");
            Environment.SetEnvironmentVariable("USER_NAME", "John");
            var input = "Greetings ((GREETING))!";
            var expected = "Greetings John!";
            Assert.Equal(expected, _transformer.TransformText(_transformer.TransformText(input)));
            Environment.SetEnvironmentVariable("GREETING", null); // Clean up
            Environment.SetEnvironmentVariable("USER_NAME", null); // Clean up
        }

        [Fact]
        public void TransformText_MixedContent_ReplacesOnlyEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("VAR1", "Hello");
            Environment.SetEnvironmentVariable("VAR2", "world");
            var input = "((VAR1)), [[user]]! Welcome to ((VAR2))!";
            var expected = "Hello, [[user]]! Welcome to world!";
            Assert.Equal(expected, _transformer.TransformText(_transformer.TransformText(input)));
            Environment.SetEnvironmentVariable("VAR1", null); // Clean up
            Environment.SetEnvironmentVariable("VAR2", null); // Clean up
        }
    }
}
