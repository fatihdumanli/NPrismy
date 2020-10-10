using System;
using System.IO;
using System.Text;

namespace NPrismy.Logging
{
    internal class IOLogger : ILogger
    {        
        public IOLogger()
        {
            
        }
        public void LogError(string errorMessage)
        {
            errorMessage += "\n";
            var finalMessage = string.Format(" [ERROR] [{0}]: {1}", DateTime.UtcNow, errorMessage);
            System.IO.File.AppendAllTextAsync(PackageSettings.LogDirectory, finalMessage);
        }

        public void LogInformation(string message)
        {
            message += "\n";
            var finalMessage = string.Format(" [INFO] [{0}]: {1}", DateTime.UtcNow, message);
            System.IO.File.AppendAllTextAsync(PackageSettings.LogDirectory, finalMessage);
        }
    }
}