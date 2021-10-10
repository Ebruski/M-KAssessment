using System.Threading.Tasks;
using M_KShared.Model;
using SmsMs.Messaging.Model;

namespace SmsMs.Messaging.Interface
{
    public interface ISendClientSmsProcessor
    {
        Task<SbResponse> ProcessMessage(ServiceBusSendSmsRequest rsp);
        Task<bool> ProcessMessageGottenFromQueue(string responseStringBody);
    }
}