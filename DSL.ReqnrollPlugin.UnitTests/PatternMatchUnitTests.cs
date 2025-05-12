using DSL.ReqnrollPlugin.Matches;

namespace DSL.ReqnrollPlugin.UnitTests
{
    public class PatternMatchUnitTests
    {
        [Fact]
        public void Parse_ValidInput_ReturnsCorrectPatternMatch()
        {
            // Arrange
            var config = new PatternMatchConfig { PrefixLong = "{{", SuffixLong = "}}", PrefixShort = '{', SuffixShort = '}' };
            var input = "Hello {{world}} test";

            // Act
            var result = PatternMatch.Parse(input, config);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Hello ", result.Prefix);
            Assert.Equal("world", result.MatchedPattern);
            Assert.Equal(" test", result.Postfix);
        }

        [Fact]
        public void Parse_NestedPatterns_ReturnsCorrectPatternMatch()
        {
            // Arrange
            var config = new PatternMatchConfig { PrefixLong = "{{", SuffixLong = "}}", PrefixShort = '{', SuffixShort = '}' };
            var input = "Hello {{nested{{pattern}}here}} test";

            // Act
            var result = PatternMatch.Parse(input, config);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Hello ", result.Prefix);
            Assert.Equal("nested{{pattern}}here", result.MatchedPattern);
            Assert.Equal(" test", result.Postfix);
        }

        [Fact]
        public void Parse_NoMatchingPattern_ReturnsNull()
        {
            // Arrange
            var config = new PatternMatchConfig { PrefixLong = "{{", SuffixLong = "}}", PrefixShort = '{', SuffixShort = '}' };
            var input = "Hello world";

            // Act
            var result = PatternMatch.Parse(input, config);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Parse_NullConfig_ReturnsNull()
        {
            // Arrange
            var input = "Hello {{world}}";

            // Act
            var result = PatternMatch.Parse(input, null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ReplaceMatched_ValidInput_ReturnsCorrectString()
        {
            // Arrange
            var patternMatch = new PatternMatch
            {
                Prefix = "Hello ",
                MatchedPattern = "world",
                Postfix = "!"
            };

            // Act
            var result = patternMatch.ReplaceMatched("universe");

            // Assert
            Assert.Equal("Hello universe!", result);
        }
    }
}