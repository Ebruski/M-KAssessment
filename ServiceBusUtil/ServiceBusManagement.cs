using System.Threading.Tasks;

namespace ServiceBusUtil
{
    public class ServiceBusManagement
    {
        private string serviceBusconnectionString { get; set; }
        private TopicBusManager topicBusManager { get; set; }
        private QueueBusManager queueBusManager { get; set; }

        public ServiceBusManagement(string ServiceBusconnectionString)
        {
            serviceBusconnectionString = ServiceBusconnectionString;
            topicBusManager = new TopicBusManager(ServiceBusconnectionString);
            queueBusManager = new QueueBusManager(ServiceBusconnectionString);
        }

        public async Task<bool> PushToQueue<T>(T serviceBusTransferRequest, string queueName)
        {
            // send data to bus

            return await queueBusManager.SendObjectToQueue(serviceBusTransferRequest, queueName);
        }

        public async Task<bool> PushToTopic<T>(T serviceBusTransferRequest, string topicName, string purpose)
        {
            // send data to bus

            return await topicBusManager.SendObjectToSubscription(serviceBusTransferRequest, topicName, purpose);
        }
    }
}