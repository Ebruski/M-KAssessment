using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;

namespace ServiceBusUtil
{
    public interface ITopicBusManagerDI
    {
        Task<bool> SendObjectToSubscription<T>(T ObjTosendToSubscription, string topicName, string purpose, string connectionString);
    }

    public class TopicBusManagerDI : ITopicBusManagerDI
    {
        private static ITopicClient topicClient { get; set; }

        public TopicBusManagerDI()
        {
        }

        public async Task<SubscriptionDescription> CreateSubscription(string topicPath, string subscriptionName, string connectionString)
        {
            try
            {
                var managementClient = new ManagementClient(connectionString);

                return await managementClient.CreateSubscriptionAsync(topicPath, subscriptionName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> CreateTopic(string topicPath, string connectionString)
        {
            try
            {
                var managementClient = new ManagementClient(connectionString);

                if (!await managementClient.TopicExistsAsync(topicPath))
                {
                    await managementClient.CreateTopicAsync(topicPath);
                }
            }
            catch (Exception ex)
            {
                return "Failed";
            }
            return "Success";
        }

        public async Task<IList<SubscriptionDescription>> GetSubscriptionsforTopic(string topicPath, string connectionString)
        {
            var managementClient = new ManagementClient(connectionString);

            return await managementClient.GetSubscriptionsAsync(topicPath);
        }

        public async Task<bool> SendObjectToSubscription<T>(T ObjTosendToSubscription, string topicName, string purpose, string connectionString)
        {
            try
            {
                topicClient = new TopicClient(connectionString, topicName);

                string requestObjectToString = JsonConvert.SerializeObject(ObjTosendToSubscription);

                var message = new Message(Encoding.UTF8.GetBytes(requestObjectToString));

                if (!string.IsNullOrEmpty(purpose))
                    message.UserProperties.Add("TransferOriginator", purpose.ToUpper());

                // Send the message to the topic
                await topicClient.SendAsync(message);
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