namespace NPrismy.Logging
{
    internal interface ILogger
    {
        void LogInformation(string message);

        void LogError(string errorMessage);
    }
}