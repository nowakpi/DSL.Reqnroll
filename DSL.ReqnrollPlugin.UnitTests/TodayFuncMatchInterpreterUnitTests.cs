using System;
using System.Text.RegularExpressions;
using Xunit;

namespace DSL.ReqnrollPlugin.UnitTests
{
    public class TodayFuncMatchInterpreterTests
    {
        public const string EMPTY_STRING = "";

        [Fact]
        public void ExtractTimeType_ShouldReturnLocalTime_WhenTimeTypeIsLocal()
        {
            // Arrange
            var matchCollection = CreateMatchCollection("L");
            var expectedDateTime = DateTimeOffset.UtcNow.ToLocalTime();

            // Act
            var result = TodayFuncMatchInterpreter.ExtractTimeType(matchCollection, DateTimeOffset.UtcNow);

            // Assert
            Assert.Equal(expectedDateTime.Date, result.Date);
        }

        [Fact]
        public void ExtractTimeType_ShouldReturnUtcTime_WhenTimeTypeIsUtc()
        {
            // Arrange
            var matchCollection = CreateMatchCollection("U");
            var expectedDateTime = DateTimeOffset.UtcNow;

            // Act
            var result = TodayFuncMatchInterpreter.ExtractTimeType(matchCollection, DateTimeOffset.UtcNow);

            // Assert
            Assert.Equal(expectedDateTime.Date, result.Date);
        }

        [Fact]
        public void ExtractUserOffset_ShouldApplyUserOffset_WhenOffsetIsValid()
        {
            // Arrange
            var matchCollection = CreateMatchCollection("U", "+1d");
            var initialDateTime = DateTimeOffset.UtcNow;
            var expectedDateTime = initialDateTime.AddDays(1);

            // Act
            var result = TodayFuncMatchInterpreter.ExtractUserOffset(matchCollection, initialDateTime);

            // Assert
            Assert.Equal(expectedDateTime.Date, result.Date);
        }

        [Fact]
        public void TransformToUserFormat_ShouldReturnFormattedDate_WhenUserFormatIsProvided()
        {
            // Arrange
            var matchCollection = CreateMatchCollection("U", "", "yyyy/MM/dd");
            var dateTime = DateTimeOffset.UtcNow;
            var expectedFormat = dateTime.ToString("yyyy/MM/dd");

            // Act
            var result = TodayFuncMatchInterpreter.TransformToUserFormat(matchCollection, dateTime);

            // Assert
            Assert.Equal(expectedFormat, result);
        }

        [Fact]
        public void TransformToUserFormat_ShouldReturnDefaultFormat_WhenUserFormatIsNotProvided()
        {
            // Arrange
            var matchCollection = CreateMatchCollection("L", "", "yyyy-MM-dd");
            var dateTime = DateTimeOffset.UtcNow.ToLocalTime();
            var expectedFormat = dateTime.ToString("yyyy-MM-dd");

            // Act
            var result = TodayFuncMatchInterpreter.TransformToUserFormat(matchCollection, dateTime);

            // Assert
            Assert.Equal(expectedFormat, result);
        }

        private MatchCollection CreateMatchCollection(string timeType = EMPTY_STRING, string offset = EMPTY_STRING, string format = EMPTY_STRING)
        {
            var stringToBeMatched = !string.IsNullOrWhiteSpace(timeType) ? timeType + TodayFuncMatchInterpreter.OptionSeparator + "TODAY" : "TODAY";

            stringToBeMatched = !string.IsNullOrWhiteSpace(offset) ? stringToBeMatched + offset : stringToBeMatched;
            stringToBeMatched = !string.IsNullOrWhiteSpace(format) ? stringToBeMatched + TodayFuncMatchInterpreter.OptionSeparator + format : stringToBeMatched;
            
            return RegexMatch.MatchDateFunction(stringToBeMatched);
        }
    }
}
