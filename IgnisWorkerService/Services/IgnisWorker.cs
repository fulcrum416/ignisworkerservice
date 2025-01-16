using IgnisWorkerService.Data;
using IgnisWorkerService.Data.DbModel;
using IgnisWorkerService.Utilities;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace IgnisWorkerService.Services
{
    public class IgnisWorker : BackgroundService
    {
        private ILogger<IgnisWorker> _logger;
        private IConfiguration _config;
        private IServiceScopeFactory _scopeFactor;
        private IMqttClient _mqttClient;
        private MqttClientOptions _mqttClientOptions;

        public readonly List<string> _topicsToSubscribe = new()
        {
            "unit/temperature",
            "unit/pressure",
            "unit/level",
            "unit/totalFlow",
            "unit/speed",
            "unit/onoff"
        };

        public IgnisWorker(ILogger<IgnisWorker> logger, IConfiguration config, IServiceScopeFactory scopeFactory) 
        { 
            _logger = logger;
            _config = config;
            _scopeFactor = scopeFactory;


            string basePath = AppDomain.CurrentDomain.BaseDirectory; // Gets the application base directory
            string certPath = Path.Combine(basePath, "Certificates", "mosquitto.crt");
            string clientCertPath = Path.Combine(basePath, "Certificates", "client.crt");
            string clientKeyPath = Path.Combine(basePath, "Certificates", "client.key");


            var mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();

            var mqttDefaults = _config.GetSection("MQTTDefaults").Get<MQTTDefaults>() ?? new MQTTDefaults();

            _mqttClientOptions = new MqttClientOptionsBuilder()
                .WithClientId(mqttDefaults.ClientId)
                .WithTcpServer(mqttDefaults.TCPServer, mqttDefaults.Port)     
                .WithCredentials(mqttDefaults.Username,mqttDefaults.Password)
                //.WithTlsOptions(new MqttClientTlsOptions
                //{
                //    UseTls = true,
                //    CertificateValidationHandler = ctx =>
                //    {
                //        // Optionally validate the broker's certificate
                //        return true; // Accept all certificates (not recommended for production)

                //    },
                //    AllowUntrustedCertificates = true,
                //    IgnoreCertificateChainErrors = true,
                //    IgnoreCertificateRevocationErrors = true,
                //})
                .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311)
                .WithCleanSession()
                .Build();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            _mqttClient.ConnectedAsync += _mqttClient_ConnectedAsync;
            _mqttClient.DisconnectedAsync += _mqttClient_DisconnectedAsync;
            _mqttClient.ApplicationMessageReceivedAsync += _mqttClient_ApplicationMessageReceivedAsync;

            // Connect to the broker
            try
            {
                await _mqttClient.ConnectAsync(_mqttClientOptions, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to connect to MQTT broker: {ex.Message}");
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
            await _mqttClient.DisconnectAsync();

        }

        private async Task _mqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            string topic = arg.ApplicationMessage.Topic;
            string payload = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
            _logger.LogInformation($"Message received: Topic={topic}, Payload={payload}");
            await SaveMessageToDb(topic, payload);
            await Task.CompletedTask;
        }

        private async Task _mqttClient_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            _logger.LogWarning("Disconnected from MQTT broker.");

            await Task.CompletedTask;
        }

        private async Task _mqttClient_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            _logger.LogWarning("Connected to MQTT broker.");
            var topicFilters = new List<MqttTopicFilter>();
            foreach(var topic in _topicsToSubscribe)
            {
                await _mqttClient.SubscribeAsync(topic);
                _logger.LogWarning($"Subscribing to topic: {topic}");
            }                       
        }

        public async Task SaveMessageToDb(string topic, string payload)
        {
            using (var scope = _scopeFactor.CreateScope())
            {
                
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                using(var transaction = await db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var tagDef = db.TagsDefinitions.ToList();
                        DateTime _dt = DateTime.UtcNow;
                        var tags = payload.Split(',');
                        foreach (var tag in tags)
                        {
                            var data = tag.Split("=");


                            if (data[0].ToString() == "DT")
                            {
                                _dt = data[1].ToString().ToDateTimeUtcFromString();
                            }
                            else
                            {
                                var _tag = tagDef.Where(m => m.DataTag == data[0]).FirstOrDefault();

                                if (_tag != null && data[1] != null)
                                {
                                    var tagData = new Tag()
                                    {
                                        Name = _tag.UnitTag,
                                        Type = _tag.UnitType,
                                        Unit = _tag.Unit,
                                        Category = _tag.Category,
                                        Description = _tag.Description,
                                        TagValue = data[1].ToDecimalFromString(),
                                        InDate = DateTime.UtcNow,
                                        LogDate = _dt
                                    };
                                    //tagData.Name = _tag.UnitTag;
                                    //tagData.Type = _tag.UnitType;
                                    //tagData.Unit = _tag.Unit;
                                    //tagData.Category = _tag.Category;
                                    //tagData.Description = _tag.Description;
                                    //tagData.TagValue = data[1].ToDecimalFromString();
                                    //tagData.InDate = DateTime.UtcNow;
                                    //tagData.LogDate = _dt;

                                    await db.Tags.AddAsync(tagData);
                                }
                            }



                        }

                        await db.SaveChangesAsync();
                        // Commit the transaction
                        await transaction.CommitAsync();

                    }
                    catch (Exception ex)
                    {
                        // Roll back the transaction if an error occurs
                        await transaction.RollbackAsync();
                        // Log the exception or rethrow it as needed
                        _logger.LogError($"Error saving data to the database: {ex.Message}");
                        throw;
                    }
                }


  

            }
        }

        //private async Task ProcessPayload(string payload)
        //{
        //    using (var scope = _scopeFactor.CreateScope())
        //    {
        //        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //        var tagDef = db.TagsDefinitions.ToList();
                
        //        var tags = payload.Split(',');
        //        foreach (var tag in tags)
        //        {
        //            var data = tag.Split("=");
        //            var _tag = tagDef.Where(m => m.DataTag == data[0]).FirstOrDefault();

        //            if (_tag != null && data[1] != null) 
        //            {
        //                var tagData = new Tag();
        //                tagData.Name = _tag.UnitTag;
        //                tagData.Type = _tag.UnitType;
        //                tagData.Unit = _tag.Unit;
        //                tagData.Category = _tag.Category;
        //                tagData.Description = _tag.Description;
        //                tagData.TagValue = data[1].ToDecimalFromString();
        //                await db.Tags.AddAsync(tagData);
        //            }
        //        }
        //        await db.SaveChangesAsync();

        //    }
        //}


    }
}
