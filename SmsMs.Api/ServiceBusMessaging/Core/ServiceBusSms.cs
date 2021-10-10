using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using M_KShared.Extensions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmsMs.Api.ServiceBusMessaging.Interface;

namespace SmsMs.Api.ServiceBusMessaging.Core
{
    public class ServiceBusSms : IServiceBusSms
    {
        private readonly string _smsQueueName = AppSettingsManager.Fetch("SmsServiceQueueName");
        private readonly string _smsQueueConnectionString = AppSettingsManager.Fetch("SmsServiceQueueConnectionString");
        private IQueueClient _smsServiceQueue;

        private readonly ILogger<ServiceBusSms> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ServiceBusSms(IServiceScopeFactory serviceScopeFactory, ILogger<ServiceBusSms> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        

        public async Task RegisterOnMessageHandlerAndReceiveMessages()
        {
            try
            {
                _smsServiceQueue = new QueueClient(_smsQueueConnectionString, _smsQueueName);

                // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
                var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
                {
                    // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                    // Set it according to how many messages the application wants to process in parallel.
                    MaxConcurrentCalls = 2,//20,

                    // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                    // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                    AutoComplete = false,
                    MaxAutoRenewDuration = TimeSpan.FromMinutes(1)
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task ProcessClientMessagesAsync(Message message, CancellationToken token)
        {
            bool shouldComplete;
            string messageBodyText = Encoding.UTF8.GetString(message.Body);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var processor = scope.ServiceProvider.GetService<ISendClientSmsProcessor>();
                shouldComplete = await processor.ProcessMessageGottenFromQueue(messageBodyText);
            }

            if (shouldComplete)
                await _smsServiceQueue.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError(exceptionReceivedEventArgs.Exception, "Message handler encountered an exception");

            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            _logger.LogInformation($"Exception for serviceBus {JsonConvert.SerializeObject(exceptionReceivedEventArgs)}");

            return Task.CompletedTask;
        }

        public async Task CloseQueueAsync()
        {
            await _smsServiceQueue.CloseAsync();
        }
    }
}
