namespace NPrismy
{
    internal static class WhereClauseValueFormatter
    {
        const string quoteChar = "\'";  

        internal static string ValueToString(object value, bool quote)
        {
            if(quote)
            {
                return string.Format(quoteChar + value.ToString() + quoteChar);
            }

            else 
            {
                return string.Format(value.ToString());
            }
        }
    }
}