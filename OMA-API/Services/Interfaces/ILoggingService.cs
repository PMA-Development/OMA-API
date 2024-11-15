namespace OMA_API.Services.Interfaces
{
    public interface ILoggingService
    {
        Task AddLog(LogLevel logLevel, string description);
    }
}
}
