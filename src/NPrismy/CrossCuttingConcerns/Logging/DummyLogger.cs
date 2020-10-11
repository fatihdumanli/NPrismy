namespace NPrismy.Logging
{
    internal class DummyLogger : ILogger
    {
        public void LogError(string errorMessage)
        {
        }

        public void LogInformation(string message)
        {
        }

        public void LogWarning(string message)
        {
        }
    }
}
