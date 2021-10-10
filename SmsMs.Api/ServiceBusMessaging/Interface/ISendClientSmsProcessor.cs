using System.Threading.Tasks;
using M_KShared.Model;
using SmsMs.Api.ServiceBusMessaging.Model;

namespace SmsMs.Api.ServiceBusMessaging.Interface
{
    public interface ISendClientSmsProcessor
    {
        Task<SbResponse> ProcessMessage(ServiceBusSendSmsRequest rsp);
        Task<bool> ProcessMessageGottenFromQueue(string responseStringBody);
    }
}