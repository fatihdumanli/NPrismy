using System;

namespace NPrismy
{
    internal static class PackageSettings
    {
        public static string LogDirectory { get; set; } = string.Format(@"c:\logs\NPrismyLogs.txt", DateTime.UtcNow.Date);
    }
}