using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;

namespace ServiceBusUtil
{
    public interface IQueueBusManagerDI
    {
        Task<bool> SendObjectToQueue<T>(T ObjTosendToQueue, string Quename, string connectionString);
    }

    public class QueueBusManagerDI : IQueueBusManagerDI
    {
        private IQueueClient _queClient;
        //private readonly ILogger<QueueBusManagerDI> _logger;

        public QueueBusManagerDI(/*ILogger<QueueBusManagerDI> logger*/)
        {
            //_logger = logger;
        }

        public async Task CreateQueueAsync(string queuePath, bool RequireDuplicateDetection, bool RequireSession, int maxDeliveryCoun, bool useDefault, string connectionString)
        {
            try
            {
                var managementClient = new ManagementClient(connectionString);

                var queueDescript = new QueueDescription(queuePath);
                if (!useDefault)
                {
                    queueDescript = new QueueDescription(queuePath)
                    {
                        RequiresDuplicateDetection = RequireDuplicateDetection,
                        RequiresSession = RequireSession,
                        MaxDeliveryCount = 20
                    };
                }
                var createdDescription = managementClient.CreateQueueAsync(queueDescript).Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendControlMessage<T>(T QueoObj, string QueueName, string commandVal, int hoursToStartFromCallingDate, string connectionString)
        {
            try
            {
                _queClient = new QueueClient(connectionString, QueueName);

                var jsonObj = JsonConvert.SerializeObject(QueoObj);

                var message = new Message(Encoding.UTF8.GetBytes(jsonObj)) { ContentType = "application/json" };

                message.UserProperties.Add("Command", commandVal);
                message.UserProperties.Add("ActionTime", DateTime.Now.AddHours(hoursToStartFromCallingDate));

                await _queClient.SendAsync(message);
            }
            catch (Exception ex)
            {
                await _queClient.CloseAsync();
                throw ex;
            }

            await _queClient.CloseAsync();
        }

        public async Task SendObjectListQueue<T>(IList<T> ObjectList, string QueueName, string connectionString)
        {
            try
            {
                _queClient = new QueueClient(connectionString, QueueName);

                var messageList = new List<Message>();

                foreach (var queueObj in ObjectList)
                {
                    var jSonObj = JsonConvert.SerializeObject(queueObj);

                    var message = new Message(Encoding.UTF8.GetBytes(jSonObj))

                    { ContentType = "application/json" };

                    await _queClient.SendAsync(message);
                }
            }
            catch (Exception ex)
            {
                await _queClient.CloseAsync();
                throw ex;
            }

            await _queClient.CloseAsync();
        }

        public async Task<bool> SendObjectToQueue<T>(T ObjTosendToQueue, string Quename, string connectionString)
        {
            try
            {
                _queClient = new QueueClient(connectionString, Quename);

                var jsonObj = JsonConvert.SerializeObject(ObjTosendToQueue);

                var message = new Message(Encoding.UTF8.GetBytes(jsonObj)) { ContentType = "application/json" };
                //_logger.LogInformation($"{AppsettingsManager.EnvironmentOfDeployment}::Sending to queue to Queue {Quename} :: ObjectTosendToQueue {ObjTosendToQueue}");

                await _queClient.SendAsync(message);

                await _queClient.CloseAsync();
                return true;
            }
            catch (Exception ex)
            {
               // _logger.LogError($"{AppsettingsManager.EnvironmentOfDeployment}::Exception Sending to Queue {Quename} :: ObjectTosendToQueue {ObjTosendToQueue}::{ex.Message}::{ex.InnerException}::{ex.StackTrace}");
                Console.WriteLine($"Exception Sending to Queue {Quename} :: ObjectTosendToQueue {ObjTosendToQueue}::{ex.Message}::{ex.InnerException}::{ex.StackTrace}");

                return false;
            }
        }
    }
}