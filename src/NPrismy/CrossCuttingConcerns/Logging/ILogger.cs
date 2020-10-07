namespace NPrismy.Logging
{
    public interface ILogger
    {
        void LogInformation(string message);

        void LogError(string errorMessage);
    }
}