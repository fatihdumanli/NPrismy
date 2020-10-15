namespace NPrismy.Extensions
{
    public static class ColumnNameDecoratorExtension 
    {
        public static void DecorateWithSquareBrackets(this string[] columnNames)
        {
            for(int i = 0; i < columnNames.Length; i++)
            {
                columnNames[i] = string.Format("[{0}]", columnNames[i]);
            }            
        }
    }
}