using System;

namespace DSL.ReqnrollPlugin.Helpers
{
    internal static class DateTimeOffsetHelper
    {
        private const string YEAR = "y";
        private const string MONTH = "M";
        private const string DAY = "d";
        private const string HOUR = "H";
        private const string MINUTE = "m";
        private const string SECOND = "s";

        internal static DateTimeOffset CalculateNewDateTimeOffset(DateTimeOffset currDateTimeOffsetUTC, string offsetType, int offset)
        {
            switch (offsetType)
            {
                case YEAR:
                    currDateTimeOffsetUTC = currDateTimeOffsetUTC.AddYears(offset);
                    break;
                case MONTH:
                    currDateTimeOffsetUTC = currDateTimeOffsetUTC.AddMonths(offset);
                    break;
                case DAY:
                    currDateTimeOffsetUTC = currDateTimeOffsetUTC.AddDays(offset);
                    break;
                case HOUR:
                    currDateTimeOffsetUTC = currDateTimeOffsetUTC.AddHours(offset);
                    break;
                case MINUTE:
                    currDateTimeOffsetUTC = currDateTimeOffsetUTC.AddMinutes(offset);
                    break;
                case SECOND:
                    currDateTimeOffsetUTC = currDateTimeOffsetUTC.AddSeconds(offset);
                    break;
            }

            return currDateTimeOffsetUTC;
        }
    }
}