namespace NPrismy
{
    internal static class WhereClauseValueFormatter
    {
        internal static string ValueToString(object value, bool quote)
        {
            return value.ToString();
        }
    }
}