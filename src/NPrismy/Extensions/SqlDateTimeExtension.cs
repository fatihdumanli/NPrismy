using System;

namespace NPrismy
{
    public static class SqlDateTimeExtensions
    {
        public static string ConvertToSqlDateTime(this object o)
        {
            DateTime dateTimeToBeFormatted = (DateTime) o;
            return dateTimeToBeFormatted.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
    }
}