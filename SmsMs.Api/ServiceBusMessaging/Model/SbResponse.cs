using M_KShared.Model;

namespace SmsMs.Api.ServiceBusMessaging.Model
{
    public class SbResponse
    {
        public ServiceBusSendSmsResponse Response { get; set; }
        public bool ShouldComplete { get; set; }
    }
}
