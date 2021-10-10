using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServiceBusUtil
{
    public interface IServiceBusManagementDI
    {
        Task<bool> PushToQueue<T>(T serviceBusTransferRequest, string queueName, string connectionString);
        Task<bool> PushToTopic<T>(T serviceBusTransferRequest, string topicName, string purpose, string connectionString);
    }

    public class ServiceBusManagementDI : IServiceBusManagementDI
    {
        private IQueueBusManagerDI _queueBusManager;
        private ITopicBusManagerDI _topicBusManager;
        //private readonly ILogger<ServiceBusManagementDI> _logger;

        public ServiceBusManagementDI(IQueueBusManagerDI queueBusManager, ITopicBusManagerDI topicBusManager/*, ILogger<ServiceBusManagementDI> logger*/)
        {
            //_logger = logger;
            _queueBusManager = queueBusManager;
            _topicBusManager = topicBusManager;
        }

        public async Task<bool> PushToQueue<T>(T serviceBusTransferRequest, string queueName, string connectionString)
        {
            // send data to bus
            //_logger.LogInformation($"{AppsettingsManager.EnvironmentOfDeployment}::Sending to queue to Queue {JsonConvert.SerializeObject(serviceBusTransferRequest)} :: ObjectTosendToQueue {queueName}");

           

            var sendResponse = await _queueBusManager.SendObjectToQueue(serviceBusTransferRequest, queueName, connectionString);
            //_logger.LogInformation($"{AppsettingsManager.EnvironmentOfDeployment}::Response from sending {sendResponse} :: {JsonConvert.SerializeObject(serviceBusTransferRequest)} :: ObjectTosendToQueue {queueName} :: {connectionString}");
            return sendResponse;
        }

        public async Task<bool> PushToTopic<T>(T serviceBusTransferRequest, string topicName, string purpose, string connectionString)
        {
            // send data to bus
            
            return await _topicBusManager.SendObjectToSubscription(serviceBusTransferRequest, topicName, purpose, connectionString);
        }
    }
}