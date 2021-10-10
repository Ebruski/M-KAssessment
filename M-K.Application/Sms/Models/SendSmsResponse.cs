using SmsMs.Common.Enums;

namespace SmsMs.Application.Sms.Models
{
    public class SendSmsResponse
    {
        public string VendorName { get; set; }
        public NotificationStatus Status { get; set; }
    }
}
