using System.Threading.Tasks;

namespace SmsMs.Api.ServiceBusMessaging.Interface
{
    public interface IServiceBusSms
    {
        Task RegisterOnMessageHandlerAndReceiveMessages();

        Task CloseQueueAsync();
    }
}
