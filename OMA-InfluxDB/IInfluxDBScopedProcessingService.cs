namespace OMA_InfluxDB
{
    public interface IInfluxDBScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}
