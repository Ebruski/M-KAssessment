using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;

namespace ServiceBusUtil
{
    public class TopicBusManager
    {
        private ManagementClient _managementClient { get; set; }
        private static ITopicClient _topicClient { get; set; }

        private string _conectionStr { get; set; }

        public TopicBusManager(string connectionString)
        {
            _managementClient = new ManagementClient(connectionString);

            _conectionStr = connectionString;
        }

        public async Task<SubscriptionDescription> CreateSubscription(string topicPath, string subscriptionName)
        {
            try
            {
                return await _managementClient.CreateSubscriptionAsync(topicPath, subscriptionName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> CreateTopic(string topicPath)
        {
            try
            {
                if (!await _managementClient.TopicExistsAsync(topicPath))
                {
                    await _managementClient.CreateTopicAsync(topicPath);
                }
            }
            catch (Exception ex)
            {
                return "Failed";
            }
            return "Success";
        }

        public async Task<IList<SubscriptionDescription>> GetSubscriptionsforTopic(string topicPath)
        {
            return await _managementClient.GetSubscriptionsAsync(topicPath);
        }

        public async Task<bool> SendObjectToSubscription<T>(T ObjTosendToSubscription, string topicName, string purpose)
        {
            try
            {
                _topicClient = new TopicClient(_conectionStr, topicName);

                string requestObjectToString = JsonConvert.SerializeObject(ObjTosendToSubscription);

                var message = new Message(Encoding.UTF8.GetBytes(requestObjectToString));

                if (!string.IsNullOrEmpty(purpose))
                    message.UserProperties.Add("SmsOriginator", purpose.ToUpper());

                // Send the message to the topic
                await _topicClient.SendAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" mesage sending to topic {topicName} failed------- {ex}");
                
                return false;
            }
        }
    }
}