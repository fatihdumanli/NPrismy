using System.Linq;

namespace NPrismy.Extensions
{
    internal static class StringQuoteExtension
    {
        const string quoteChar = "\'";  

        private static bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
        public static string DecorateWithQuotes(this string value)
        {   
            if(IsNumeric(value))
            {
                return value;
            }

            else 
            {
                return string.Format("{0}{1}{0}", quoteChar, value);
            }
        
        }
    }
}