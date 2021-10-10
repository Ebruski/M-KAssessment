using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;

namespace ServiceBusUtil
{
    public class QueueBusManager
    {
        private ManagementClient _managementClient;

        private IQueueClient _queClient;

        private string _conectionStr;

        public QueueBusManager(string connectionString)
        {
            _managementClient = new ManagementClient(connectionString);

            _conectionStr = connectionString;
        }

        public async Task CreateQueueAsync(string queuePath, bool RequireDuplicateDetection, bool RequireSession, int maxDeliveryCoun, bool useDefault)
        {
            try
            {
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
                var createdDescription = _managementClient.CreateQueueAsync(queueDescript).Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendControlMessage<T>(T QueoObj, string QueueName, string commandVal, int hoursToStartFromCallingDate)
        {
            try
            {
                _queClient = new QueueClient(_conectionStr, QueueName);

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

        public async Task SendObjectListQueue<T>(IList<T> ObjectList, string QueueName)
        {
            try
            {
                _queClient = new QueueClient(_conectionStr, QueueName);

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

        public async Task<bool> SendObjectToQueue<T>(T ObjTosendToQueue, string Quename)
        {
            try
            {
                _queClient = new QueueClient(_conectionStr, Quename);

                var jsonObj = JsonConvert.SerializeObject(ObjTosendToQueue);

                var message = new Message(Encoding.UTF8.GetBytes(jsonObj)) { ContentType = "application/json" };

                await _queClient.SendAsync(message);

                await _queClient.CloseAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Sending to Queue {Quename} :: ObjectTosendToQueue {ObjTosendToQueue}::{ex.Message}::{ex.InnerException}::{ex.StackTrace}");

                return false;
            }
        }
    }
}