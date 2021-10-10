using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmsMs.Application.Sms.Models;

namespace SmsMs.Application.Common.Interfaces
{
    public interface ISmsService
    {
        Task<SendSmsResponse> SendSms(SendSmsRequest requestModel);
    }
}
