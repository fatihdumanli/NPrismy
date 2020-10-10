using System.Linq;

namespace NPrismy
{
    internal static class StrinQuoteExtension
    {
        const string quoteChar = "\'";  

        public static bool IsNumeric(this string value)
        {
            return value.All(char.IsNumber);
        }
        public static string DecorateWithQuotes(this string value)
        {   
            return string.Format("{0}{1}{0}", quoteChar, value);
        }
    }
}