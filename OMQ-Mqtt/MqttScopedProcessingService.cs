using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using OMA_Mqtt.Models;
using System.Text;
using Newtonsoft.Json;
using MQTTnet.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OMA_Data.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace OMA_Mqtt
{
    public class MqttScopedProcessingService : IMqttScopedProcessingService
    {
        internal ILogger<MqttScopedProcessingService> _logger;
        internal CancellationToken _cancellationToken;
        internal readonly IConfiguration _config;

        internal readonly SemaphoreSlim _concurrentProcesses;
        internal readonly MqttFactory _mqttFactory;
        internal readonly IMqttClient _mqttClient;
        internal readonly string _mqttHost;
        internal readonly int _mqttPort;
        internal readonly string _mqttClientId;
        internal readonly string _mqttUsername;
        internal readonly string _mqttPassword;
        internal readonly bool _mqttUseTLS;
        internal readonly string _mqttTopicOutbound = "device/outbound";
        internal readonly string _mqttTopicBeacon = "device/inbound/beacon";
        internal readonly string _mqttTopicPing = "device/outbound/ping";


        private static Queue<DeviceBeacon> _deviceBeaconQueue = new Queue<DeviceBeacon>();

        public static readonly List<string> AllowedActions = new List<string> { "ChangeState", "Settings" };

        // 1="On", 2="Off", 3="ServiceMode"
        public static readonly List<int> AllowedStates = new List<int> { 1, 2, 3 };
        public static Queue<ActionRequest> ActionQueue = new Queue<ActionRequest>();

        private readonly IServiceScopeFactory scopeFactory;

        public MqttScopedProcessingService(ILogger<MqttScopedProcessingService> logger, IConfiguration config, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _config = config;
            _cancellationToken = CancellationToken.None;

            _concurrentProcesses = new SemaphoreSlim(_config.GetValue("MQTT:ConcurrentProcesses", 1));

            // MQTT
            _mqttFactory = new MqttFactory();
            _mqttClient = _mqttFactory.CreateMqttClient();
            _mqttHost = _config.GetValue<string>("MQTT:Host")!;
            _mqttPort = _config.GetValue("MQTT:Port", 1883);
            _mqttClientId = _config.GetValue<string>("MQTT:ClientId")!;
            _mqttUsername = _config.GetValue<string>("MQTT:Username")!;
            _mqttPassword = _config.GetValue<string>("MQTT:Password")!;
            _mqttUseTLS = _config.GetValue<bool>("MQTT:UseTLS");

            this.scopeFactory = scopeFactory;
        }

        internal async Task OnTopic(MqttApplicationMessageReceivedEventArgs ea, string payload)
        {
            if (ea.ApplicationMessage.Topic == _mqttTopicBeacon)
                OnTopicBeacon(ea, payload);

            else
                _logger.LogDebug($"Unknown topic {ea.ApplicationMessage.Topic}!");
        }

        internal void OnTopicBeacon(MqttApplicationMessageReceivedEventArgs ea, string payload)
        {
            DeviceBeacon? deviceBeaconModel = JsonConvert.DeserializeObject<DeviceBeacon>(payload);
            if (deviceBeaconModel == null)
            {
                _logger.LogDebug($"Inbound Beacon reveived but unable to intercept");
                return;
            }

            _logger.LogDebug($"Beacon reveived and enqueued.");
            _deviceBeaconQueue.Enqueue(deviceBeaconModel);
        }

        internal async Task OnConnect()
        {
            // This method is ran after every succesfull connection to the MQTT Broker.
            // This can be used for a beacon or ping.
            await PublishPing();
        }

        public async Task PublishPing()
        {
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(_mqttTopicPing)
                .WithPayload("{}")
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce) // QoS: 0
                .Build();

            await PublishMessage(applicationMessage);
        }

        internal async Task StartWorker()
        {
            try
            {
                _mqttClient.ApplicationMessageReceivedAsync += async ea =>
                {
                    // Wait for an available process, before creating a new Task.
                    await _concurrentProcesses.WaitAsync(_cancellationToken).ConfigureAwait(false);
                    try
                    {
                        _ = Task.Run(async () => await ProcessApplicationMessageReceivedAsync(ea), _cancellationToken);
                    }
                    finally
                    {
                        _concurrentProcesses.Release();
                    }
                };

                await HandleMqttConnection();

                _logger.LogInformation("Cancellation requested. Exiting...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Connection failed.");
            }
        }

        private async Task ProcessApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs ea)
        {
            string? payload = Encoding.UTF8.GetString(ea.ApplicationMessage.PayloadSegment);
            _logger.LogDebug($"Received message \"{payload}\", topic \"{ea.ApplicationMessage.Topic}\", ResponseTopic: \"{ea.ApplicationMessage.ResponseTopic}\"");

            await OnTopic(ea, payload);
        }

        private async Task HandleMqttConnection()
        {
            MqttClientOptions mqttClientOptions = GetMqttClientOptionsBuilder().Build();
            List<MqttClientSubscribeOptions> subscribeOptions = GetSubScriptionOptions();

            // Handle reconnection logic and cancellation token properly
            while (!_cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // Periodically check if the connection is alive, otherwise reconnect
                    if (!await _mqttClient.TryPingAsync())
                    {
                        _logger.LogInformation("Attempting to connect to MQTT Broker...");
                        await _mqttClient.ConnectAsync(mqttClientOptions, _cancellationToken);

                        foreach (var subscribeOption in subscribeOptions)
                        {
                            await _mqttClient.SubscribeAsync(subscribeOption, _cancellationToken);
                            _logger.LogInformation($"MQTT client subscribed to topic: {subscribeOption}.");
                        }

                        // Method to override.
                        await OnConnect();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during MQTT operation.");
                }

                // Check the connection status every 5 seconds
                await Task.Delay(TimeSpan.FromSeconds(5), _cancellationToken);
            }
        }

        internal async Task PublishMessage(MqttApplicationMessage? msg)
        {
            if (await _mqttClient.TryPingAsync())
            {
                try
                {
                    await _mqttClient.PublishAsync(msg, _cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "PulbishMessage: Connection failed.");
                }
            }
        }

        private MqttClientOptionsBuilder GetMqttClientOptionsBuilder()
        {
            MqttClientOptionsBuilder mqttClientOptionsBuilder = new MqttClientOptionsBuilder()
                    .WithTcpServer(_mqttHost, _mqttPort) // MQTT broker address and port
                    .WithCredentials(_mqttUsername, _mqttPassword) // Set username and password
                    .WithClientId(_mqttClientId)
                    .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500);

            if (_mqttUseTLS)
            {
                mqttClientOptionsBuilder.WithTlsOptions(
                o => o.WithCertificateValidationHandler(
                    // The used public broker sometimes has invalid certificates. This sample accepts all
                    // certificates. This should not be used in live environments.
                    _ => true));
            }

            return mqttClientOptionsBuilder;
        }

        internal List<MqttClientSubscribeOptions> GetSubScriptionOptions()
        {
            List<MqttClientSubscribeOptions> subscribeOptions = new List<MqttClientSubscribeOptions>();

            var mqttSensorTopicFilter = new MqttTopicFilterBuilder()
                .WithTopic(_mqttTopicBeacon)
                .WithAtLeastOnceQoS()
                .Build();

            var sensorSubscribeOption = _mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(mqttSensorTopicFilter)
                .Build();

            subscribeOptions.Add(sensorSubscribeOption);

            return subscribeOptions;
        }

        private async Task ProcessBeaconQueue()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                if (_deviceBeaconQueue.Count != 0)
                {
                    try
                    {
                        _logger.LogInformation("Processing device beacon from queue...");
                        DeviceBeacon deviceBeacon = _deviceBeaconQueue.Dequeue();
                        if (deviceBeacon != null)
                        {
                            if (deviceBeacon.Type == "Sensor")
                            {
                                await OnDeviceBeaconSensor(deviceBeacon);
                            }
                            else if (deviceBeacon.Type == "Turbine")
                            {
                                await OnDeviceBeaconTurbine(deviceBeacon);
                            }
                            else if (deviceBeacon.Type == "Island")
                            {
                                await OnDeviceBeaconIsland(deviceBeacon);
                            }
                            else
                                _logger.LogInformation($"DeviceBeacon: Unknown device type: {deviceBeacon.Type}");
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogInformation(e, "Error in Inbound: ");
                        throw;
                    }
                }

            }

        }

        private async Task OnDeviceBeaconTurbine(DeviceBeacon deviceBeacon)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IDataContext>();
                OMA_Data.Entities.Turbine? turbine = dbContext.TurbineRepository.GetByClintId(deviceBeacon.Id);
                if (turbine != null)
                    return;
                OMA_Data.Entities.Island? island = dbContext.IslandRepository.GetByClintId(deviceBeacon.IslandId);
                if (island == null)
                    return;

                OMA_Data.Entities.Turbine turbineEntity = new OMA_Data.Entities.Turbine { ClientID = deviceBeacon.Id, Island = island };
                try
                {
                    await dbContext.TurbineRepository.Add(turbineEntity);
                    await dbContext.CommitAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "OnDeviceBeaconTurbine - Error: ");
                }
            }
        }

        private async Task OnDeviceBeaconIsland(DeviceBeacon deviceBeacon)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IDataContext>();
                OMA_Data.Entities.Island? island = dbContext.IslandRepository.GetByClintId(deviceBeacon.Id);
                if (island != null)
                    return;

                OMA_Data.Entities.Island islandEntity = new OMA_Data.Entities.Island { ClientID = deviceBeacon.Id, Title = deviceBeacon.Id, Abbreviation = deviceBeacon.Id };
                try
                {
                    await dbContext.IslandRepository.Add(islandEntity);
                    await dbContext.CommitAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "OnDeviceBeaconTurbine - Error: ");
                }
            }
        }

        private async Task OnDeviceBeaconSensor(DeviceBeacon deviceBeacon)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IDataContext>();
                OMA_Data.Entities.Device? device = dbContext.DeviceRepository.GetByClintId(deviceBeacon.Id);
                if (device != null)
                    return;
                OMA_Data.Entities.Turbine? turbine = dbContext.TurbineRepository.GetByClintId(deviceBeacon.TurbineId);
                if (turbine == null)
                    return;

                OMA_Data.Entities.Device deviceEntity = new OMA_Data.Entities.Device { ClientID = deviceBeacon.Id, Turbine = turbine };
                try
                {
                    await dbContext.DeviceRepository.Add(deviceEntity);
                    await dbContext.CommitAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "OnDeviceBeaconTurbine - Error: ");
                }
            }
        }

        private async Task ProcessActionQueue()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                if (ActionQueue.Count != 0)
                {
                    try
                    {
                        _logger.LogInformation("Processing action from queue...");
                        ActionRequest actionRequest = ActionQueue.Dequeue();
                        if (actionRequest.Action == "ChangeState") 
                        {
                            await ProcessActionChangeState(actionRequest);
                        }
                        else if (actionRequest.Action == "Settings")
                        {
                            await ProcessActionSettings(actionRequest);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogInformation(e, "Error in Inbound: ");
                        throw;
                    }
                }

            }

        }

        private async Task ProcessActionChangeState(ActionRequest action)
        {
            if (AllowedStates.Contains(action.Value))
            {
                string value = string.Empty;
                if (action.Value == 1)
                {
                    value = "On";
                }
                else if (action.Value == 2)
                {
                    value = "Off";
                }
                else if (action.Value == 3)
                {
                    value = "ServiceMode";
                }
                ChangeState changeState = new ChangeState { Value = value };
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic($"{_mqttTopicOutbound}/{action.ClientId}/changestate")
                    .WithPayload(JsonConvert.SerializeObject(changeState))
                    .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce) // QoS: 0 | QoS: 2
                    .Build();

                await PublishMessage(applicationMessage);
            }
            else
            {
                _logger.LogInformation("ProcessActionChangeState: State not allowed: " + action.Value);
            }
        }

        private async Task ProcessActionSettings(ActionRequest action)
        {
            Settings settings = new Settings { CollectionInterval = action.Value };
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic($"{_mqttTopicOutbound}/{action.ClientId}/settings")
                .WithPayload(JsonConvert.SerializeObject(settings))
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce) // QoS: 0 | QoS: 2
                .Build();

            await PublishMessage(applicationMessage);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _ = Task.Run(() => StartWorker());
            _ = Task.Run(() => ProcessBeaconQueue());
            _ = Task.Run(() => ProcessActionQueue());
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // Dispose of the MQTT client manually at the end
            if (_mqttClient.IsConnected)
            {
                _mqttClient.DisconnectAsync().Wait();
            }

            _mqttClient.Dispose();
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            await StartAsync(stoppingToken);
        }

        ~MqttScopedProcessingService()
        {
            Dispose();
        }
    }
}
