using DSL.ReqnrollPlugin.Transformers;

namespace DSL.ReqnrollPlugin.UnitTests
{
    public class FunctionParameterTransformerUnitTests
    {
        private readonly FunctionParameterTransformer _transformer;

        public FunctionParameterTransformerUnitTests()
        {
            _transformer = new FunctionParameterTransformer();
        }

        [Fact]
        public void TransformText_TodayFunction()
        {
            var input = "Today is {{TODAY}}.";
            var dateFormatted = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd");
            var expected = $"Today is {dateFormatted}.";

            Assert.Equal(expected, _transformer.TransformText(input));
        }

        [Fact]
        public void TransformText_TodayFunction_WithTimeType_Local()
        {
            var input = "Today is {{L#TODAY}}.";
            var dateFormatted = DateTimeOffset.UtcNow.ToLocalTime().ToString("yyyy-MM-dd");
            var expected = $"Today is {dateFormatted}.";

            Assert.Equal(expected, _transformer.TransformText(input));
        }

        [Fact]
        public void TransformText_TodayFunction_WithTimeType_UTC()
        {
            var input = "Today is {{U#TODAY}}.";
            var dateFormatted = DateTimeOffset.UtcNow.ToLocalTime().ToString("yyyy-MM-dd");
            var expected = $"Today is {dateFormatted}.";

            Assert.Equal(expected, _transformer.TransformText(input));
        }

        [Fact]
        public void TransformText_TodayFunction_WithTimeTypeLocal_WithOffset_1()
        {
            var input = "Today is {{L#TODAY+1y}}.";
            var dateFormatted = DateTimeOffset.UtcNow.ToLocalTime().AddYears(1).ToString("yyyy-MM-dd");
            var expected = $"Today is {dateFormatted}.";

            Assert.Equal(expected, _transformer.TransformText(input));
        }

        [Fact]
        public void TransformText_TodayFunction_WithTimeTypeLocal_WithOffset_2()
        {
            var input = "Today is {{L#TODAY-6M}}.";
            var dateFormatted = DateTimeOffset.UtcNow.ToLocalTime().AddMonths(-6).ToString("yyyy-MM-dd");
            var expected = $"Today is {dateFormatted}.";

            Assert.Equal(expected, _transformer.TransformText(input));
        }

        [Fact]
        public void TransformText_TodayFunction_WithOffset_1()
        {
            var input = "Today is {{TODAY-6M}}.";
            var dateFormatted = DateTimeOffset.UtcNow.AddMonths(-6).ToString("yyyy-MM-dd");
            var expected = $"Today is {dateFormatted}.";

            Assert.Equal(expected, _transformer.TransformText(input));
        }

        [Fact]
        public void TransformText_TodayFunction_WithOffset_2()
        {
            var input = "Today is {{TODAY+11y}}.";
            var dateFormatted = DateTimeOffset.UtcNow.AddYears(11).ToString("yyyy-MM-dd");
            var expected = $"Today is {dateFormatted}.";

            Assert.Equal(expected, _transformer.TransformText(input));
        }

        [Fact]
        public void TransformText_TodayFunction_WithOffset_WithFormatting_1()
        {
            var input = "Today is {{TODAY+1M#MM/dd/yyyy ss:mm:hh}}.";
            var dateFormatted = DateTimeOffset.UtcNow.AddMonths(1).ToString("MM/dd/yyyy ss:mm:hh");
            var expected = $"Today is {dateFormatted}.";

            Assert.Equal(expected, _transformer.TransformText(input));
        }

        [Fact]
        public void TransformText_TodayFunction_WithOffset_WithFormatting_2()
        {
            var input = "Today is {{TODAY-50H#dd-MM-yyy hh:mm}}.";
            var dateFormatted = DateTimeOffset.UtcNow.AddHours(-50).ToString("dd-MM-yyy hh:mm");
            var expected = $"Today is {dateFormatted}.";

            Assert.Equal(expected, _transformer.TransformText(input));
        }

        [Fact]
        public void TransformText_TodayFunction_WithTimeTypeLocal_WithOffset_WithFormatting_2()
        {
            var input = "Today is {{L#TODAY-50H#dd-MM-yyy hh:mm}}.";
            var dateFormatted = DateTimeOffset.UtcNow.ToLocalTime().AddHours(-50).ToString("dd-MM-yyy hh:mm");
            var expected = $"Today is {dateFormatted}.";

            Assert.Equal(expected, _transformer.TransformText(input));
        }

        [Fact]
        public void TransformText_TodayFunction_WithTimeType_Local_WithIncorrectOffset()
        {
            ;
            var input = "Today is {{L#TODAY6M}}.";
            var dateFormatted = DateTimeOffset.UtcNow.ToLocalTime().ToString("yyyy-MM-dd");
            var expected = $"Today is {dateFormatted}.";

            Assert.Equal(expected, _transformer.TransformText(input));
        }
    }
}
