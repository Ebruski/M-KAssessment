using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M_KShared.Model;

namespace SmsMs.Messaging.Model
{
    public class SbResponse
    {
        public ServiceBusSendSmsResponse Response { get; set; }
        public bool ShouldComplete { get; set; }
    }
}
