using InfluxDB.Client;
using InfluxDB.Client.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OMA_Data.Data;
using OMA_InfluxDB.Models;
using OMA_InfluxDB.Converters;
using OMA_InfluxDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_InfluxDB.Services
{
    public class InfluxDBService : IInfluxDBService
    {
        internal ILogger<InfluxDBService> _logger;
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
        private readonly int _ingestInterval;

        private readonly IDataContext _context;

        public InfluxDBService(ILogger<InfluxDBService> logger, IConfiguration config, IDataContext context)
        {
            _logger = logger;
            _config = config;

            _token = _config.GetValue<string>("InfluxDB:Token")!;
            _host = _config.GetValue<string>("InfluxDB:Host")!;
            _bucket = _config.GetValue<string>("InfluxDB:Bucket")!;
            _org = _config.GetValue<string>("InfluxDB:Org")!;
            _ingestInterval = _config.GetValue<int>("InfluxDB:IngestInterval", 5)!;

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
            
            _context = context;
        }

        public async Task<List<DeviceData>> GetAllDeviceData(int agregateMins=15)
        {
            List<OMA_Data.Entities.Device> devices = _context.DeviceRepository.GetAll().ToList();
            IsConnected();
            _logger.LogDebug("InfluxDB - ReadAll: Before query");
            try
            {
                List<DeviceData> deviceDatas = new List<DeviceData>();
                var queryApi = _client!.GetQueryApi();

                var fluxQuery = $"from(bucket: \"{_bucket}\")"
                   + $" |> range(start: 0)"
                   + $" |> filter(fn: (r) => r._measurement == \"{_measurement}\")"
                   + $" |> filter(fn: (r) => r[\"_field\"] == \"property_Temperature\" or r[\"_field\"] == \"property_Humidity\" or r[\"_field\"] == \"property_Voltage\" or r[\"_field\"] == \"property_AMP\")"
                   + $" |> aggregateWindow(every: {agregateMins}m, fn: median, createEmpty: false)"
                   + $" |> yield(name: \"median\")";

                var tables = await queryApi.QueryAsync(fluxQuery, _org);

                if (tables != null)
                {
                    foreach (var fluxRecord in tables.SelectMany(fluxTable => fluxTable.Records))
                    {
                        var sensorData = _converter.ConvertToEntity<SensorEntity>(fluxRecord);
                        var device = devices.Where(x => x.ClientID == sensorData.Id).FirstOrDefault();
                        if (device != null)
                        {
                            var deviceData = new DeviceData { DeviceID = device.DeviceId, Type = sensorData.Type, Timestamp = sensorData.Timestamp.GetValueOrDefault().DateTime, TurbineID = device.Turbine.TurbineID };
                            foreach (var itemAttribute in sensorData.Attributes)
                            {
                                deviceData.Attributes.Add(new DeviceAttribute { Name = itemAttribute.Name, Value = itemAttribute.Value });
                            }
                            deviceDatas.Add(deviceData);

                        }
                    }
                }
                _logger.LogDebug("InfluxDB - ReadAll: After query");
                return deviceDatas;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "InfluxDB - GetLatestByClientId: ");
                throw;
            }
        }
        
        public async Task<List<DeviceData>> GetLatestDeviceData()
        {
            List<OMA_Data.Entities.Device> devices = _context.DeviceRepository.GetAll().ToList();
            IsConnected();
            _logger.LogDebug("InfluxDB - ReadAll: Before query");
            try
            {
                List<DeviceData> deviceDatas = new List<DeviceData>();
                var queryApi = _client!.GetQueryApi();

                var fluxQuery = $"from(bucket: \"{_bucket}\")"
                   + $" |> range(start: 0)"
                   + $" |> filter(fn: (r) => (r._measurement == \"{_measurement}\" and r.Type != \"\"))"
                   + $" |> filter(fn: (r) => r[\"_field\"] == \"property_Temperature\" or r[\"_field\"] == \"property_Humidity\" or r[\"_field\"] == \"property_Voltage\" or r[\"_field\"] == \"property_AMP\")"
                   + $" |> group(columns: [\"_measurement\", \"Type\", \"Id\"])"
                   + $" |> last()";

                var tables = await queryApi.QueryAsync(fluxQuery, _org);

                if (tables != null)
                {
                    foreach (var fluxRecord in tables.SelectMany(fluxTable => fluxTable.Records))
                    {
                        var sensorData = _converter.ConvertToEntity<SensorEntity>(fluxRecord);
                        var device = devices.Where(x => x.ClientID == sensorData.Id).FirstOrDefault();
                        if (device != null)
                        {
                            var deviceData = new DeviceData { DeviceID = device.DeviceId, Type = sensorData.Type, Timestamp = sensorData.Timestamp.GetValueOrDefault().DateTime, TurbineID = device.Turbine.TurbineID };
                            foreach (var itemAttribute in sensorData.Attributes)
                            {
                                deviceData.Attributes.Add(new DeviceAttribute { Name = itemAttribute.Name, Value = itemAttribute.Value });
                            }
                            deviceDatas.Add(deviceData);

                        }
                    }
                }
                _logger.LogDebug("InfluxDB - ReadAll: After query");
                return deviceDatas;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "InfluxDB - GetLatestByClientId: ");
                throw;
            }
        }  

        public async Task<Dictionary<int, List<DeviceData>>> GetTurbinesLatestDeviceData()
        {
            Dictionary<int, List<DeviceData>> turbineDict = new Dictionary<int, List<DeviceData>>();
            List<OMA_Data.Entities.Turbine> turbineEntities = _context.TurbineRepository.GetAll().ToList();

            foreach (var item in turbineEntities)
            {
                var result = await GetLatestDeviceDataByTurbineId(item.TurbineID, item.ClientID);
                turbineDict.Add(item.TurbineID, result);
            }
            return turbineDict;
        }

        public async Task<List<DeviceData>> GetLatestDeviceDataByTurbineId(int turbineId, string clientId="")
        {
            List<DeviceData> deviceDatas = new List<DeviceData>();
            if (clientId == "")
            {
                OMA_Data.Entities.Turbine turbine = await _context.TurbineRepository.GetByIdAsync(turbineId);
                if (turbine == null)
                {
                    return deviceDatas;
                }
                clientId = turbine.ClientID;
            }

            List<OMA_Data.Entities.Device> devices = _context.DeviceRepository.GetAll().ToList();
            IsConnected();
            _logger.LogDebug("InfluxDB - ReadAll: Before query");
            try
            {
                var queryApi = _client!.GetQueryApi();

                var fluxQuery = $"from(bucket: \"{_bucket}\")"
                   + $" |> range(start: 0)"
                   + $" |> filter(fn: (r) => (r._measurement == \"{_measurement}\" and r.TurbineId == \"{clientId}\" and r.Type != \"\"))"
                   + $" |> filter(fn: (r) => r[\"_field\"] == \"property_AMP\" or r[\"_field\"] == \"property_Humidity\" or r[\"_field\"] == \"property_Temperature\" or r[\"_field\"] == \"property_Voltage\")"
                   + $" |> group(columns: [\"_measurement\", \"TurbineId\", \"Type\", \"Id\"])"
                   + $" |> last()";

                var tables = await queryApi.QueryAsync(fluxQuery, _org);

                if (tables != null)
                {
                    foreach (var fluxRecord in tables.SelectMany(fluxTable => fluxTable.Records))
                    {
                        var sensorData = _converter.ConvertToEntity<SensorEntity>(fluxRecord);
                        var device = devices.Where(x => x.ClientID == sensorData.Id).FirstOrDefault();
                        if (device != null)
                        {
                            var deviceData = new DeviceData { DeviceID = device.DeviceId, Type = sensorData.Type, Timestamp = sensorData.Timestamp.GetValueOrDefault().DateTime, TurbineID = device.Turbine.TurbineID };
                            foreach (var itemAttribute in sensorData.Attributes)
                            {
                                deviceData.Attributes.Add(new DeviceAttribute { Name = itemAttribute.Name, Value = itemAttribute.Value });
                            }
                            deviceDatas.Add(deviceData);

                        }
                    }
                }
                _logger.LogDebug("InfluxDB - ReadAll: After query");
                return deviceDatas;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "InfluxDB - GetLatestByClientId: ");
                throw;
            }
        }        

        public async Task<List<DeviceData>> GetDeviceDataByTurbineId(int turbineId, int agregateMins = 15)
        {
            List<DeviceData> deviceDatas = new List<DeviceData>();
            OMA_Data.Entities.Turbine turbine = await _context.TurbineRepository.GetByIdAsync(turbineId);
            if (turbine == null) 
            { 
                return deviceDatas; 
            }
            List<OMA_Data.Entities.Device> devices = _context.DeviceRepository.GetAll().ToList();
            IsConnected();
            _logger.LogDebug("InfluxDB - ReadAll: Before query");
            try
            {
                var queryApi = _client!.GetQueryApi();

                var fluxQuery = $"from(bucket: \"{_bucket}\")"
                   + $" |> range(start: 0)"
                   + $" |> filter(fn: (r) => (r._measurement == \"{_measurement}\" and r.TurbineId == \"{turbine.ClientID}\"))"
                   + $" |> filter(fn: (r) => r[\"_field\"] == \"property_Voltage\" or r[\"_field\"] == \"property_Temperature\" or r[\"_field\"] == \"property_Humidity\" or r[\"_field\"] == \"property_AMP\")"
                   + $" |> aggregateWindow(every: {agregateMins}m, fn: median, createEmpty: false)"
                   + $" |> yield(name: \"median\")";

                var tables = await queryApi.QueryAsync(fluxQuery, _org);

                if (tables != null)
                {
                    foreach (var fluxRecord in tables.SelectMany(fluxTable => fluxTable.Records))
                    {
                        var sensorData = _converter.ConvertToEntity<SensorEntity>(fluxRecord);
                        var device = devices.Where(x => x.ClientID == sensorData.Id).FirstOrDefault();
                        if (device != null)
                        {
                            var deviceData = new DeviceData { DeviceID = device.DeviceId, Type = sensorData.Type, Timestamp = sensorData.Timestamp.GetValueOrDefault().DateTime, TurbineID = device.Turbine.TurbineID };
                            foreach (var itemAttribute in sensorData.Attributes)
                            {
                                deviceData.Attributes.Add(new DeviceAttribute { Name = itemAttribute.Name, Value = itemAttribute.Value });
                            }
                            deviceDatas.Add(deviceData);

                        }
                    }
                }
                _logger.LogDebug("InfluxDB - ReadAll: After query");
                return deviceDatas;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "InfluxDB - GetLatestByClientId: ");
                throw;
            }
        }

        public async Task<List<DeviceData>> GetDeviceDataByTurbineId(int turbineId, DateTime startDate,DateTime endDate, int agregateMins = 15)
        {
            List<DeviceData> deviceDatas = new List<DeviceData>();
            OMA_Data.Entities.Turbine turbine = await _context.TurbineRepository.GetByIdAsync(turbineId);
            if (turbine == null)
            {
                return deviceDatas;
            }
            List<OMA_Data.Entities.Device> devices = _context.DeviceRepository.GetAll().ToList();
            IsConnected();
            _logger.LogDebug("InfluxDB - ReadAll: Before query");

            string startTimeString;
            string endTimeString;
            List<string> range = new();
           
            try
            {
                var queryApi = _client!.GetQueryApi();

                var fluxQuery = $"from(bucket: \"{_bucket}\")"
                   + $" |> range(start: {((DateTimeOffset)startDate).ToUnixTimeSeconds()}, stop: {((DateTimeOffset)endDate).ToUnixTimeSeconds()})"
                   + $" |> filter(fn: (r) => (r._measurement == \"{_measurement}\" and r.TurbineId == \"{turbine.ClientID}\"))"
                   + $" |> filter(fn: (r) => r[\"_field\"] == \"property_Voltage\" or r[\"_field\"] == \"property_Temperature\" or r[\"_field\"] == \"property_Humidity\" or r[\"_field\"] == \"property_AMP\")"
                   + $" |> aggregateWindow(every: {agregateMins}m, fn: median, createEmpty: false)"
                   + $" |> yield(name: \"median\")";

                var tables = await queryApi.QueryAsync(fluxQuery, _org);

                if (tables != null)
                {
                    foreach (var fluxRecord in tables.SelectMany(fluxTable => fluxTable.Records))
                    {
                        var sensorData = _converter.ConvertToEntity<SensorEntity>(fluxRecord);
                        var device = devices.Where(x => x.ClientID == sensorData.Id).FirstOrDefault();
                        if (device != null)
                        {
                            var deviceData = new DeviceData { DeviceID = device.DeviceId, Type = sensorData.Type, Timestamp = sensorData.Timestamp.GetValueOrDefault().DateTime, TurbineID = device.Turbine.TurbineID };
                            foreach (var itemAttribute in sensorData.Attributes)
                            {
                                deviceData.Attributes.Add(new DeviceAttribute { Name = itemAttribute.Name, Value = itemAttribute.Value });
                            }
                            deviceDatas.Add(deviceData);

                        }
                    }
                }
                _logger.LogDebug("InfluxDB - ReadAll: After query");
                return deviceDatas;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "InfluxDB - GetLatestByClientId: ");
                throw;
            }
        }

        public bool IsConnected()
        {
            var connStatus = _client.PingAsync();
            connStatus.Wait();
            if (!connStatus.Result)
                _logger.LogWarning("InfluxDB - IsConnected: No DB connection...");
            return connStatus.Result;
        }

        ~InfluxDBService()
        {
            _client.Dispose();
        }
    }
}
