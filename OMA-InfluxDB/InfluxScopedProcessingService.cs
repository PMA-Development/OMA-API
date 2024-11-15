using InfluxDB.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OMA_InfluxDB.Converters;

namespace OMA_InfluxDB
{
    internal class InfluxScopedProcessingService : IInfluxDBScopedProcessingService
    {
        internal ILogger<InfluxScopedProcessingService> _logger;
        internal CancellationToken _cancellationToken;
        internal readonly IConfiguration _config;

        private readonly string _token;
        private readonly string _host;
        private readonly string _bucket;
        private readonly string _org;

        private readonly InfluxDBClientOptions _options;
        private readonly SensorEntityConverter _converter;
        private readonly InfluxDBClient _client;

        private readonly string _measurement = "sensor";

        private readonly int _retensionDays;

        public InfluxScopedProcessingService(ILogger<InfluxScopedProcessingService> logger, IConfiguration config, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _config = config;
            _cancellationToken = CancellationToken.None;

            _token = _config.GetValue<string>("InfluxDB:Token")!;
            _host = _config.GetValue<string>("InfluxDB:Host")!;
            _bucket = _config.GetValue<string>("InfluxDB:Bucket")!;
            _org = _config.GetValue<string>("InfluxDB:Org")!;

            _options = new InfluxDBClientOptions(_host)
            {
                Token = _token,
                Org = _org,
                Bucket = _bucket
            };

            _converter = new SensorEntityConverter();
            _client = new InfluxDBClient(_options);

            _retensionDays = config.GetValue<int>("InfluxDB:RetensionDays", -300);

            // Make it to a negative number, because we need it to be negative for the start range.
            if (_retensionDays < 0)
                _retensionDays *= -1;
        }
        public async Task DoWork(CancellationToken stoppingToken)
        {
            
        }
    }
}
