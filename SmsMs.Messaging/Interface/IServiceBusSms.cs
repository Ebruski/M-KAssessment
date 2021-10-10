using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsMs.Messaging.Interface
{
    public interface IServiceBusSms
    {
        Task RegisterOnMessageHandlerAndReceiveMessages();

        Task CloseQueueAsync();
    }
}
