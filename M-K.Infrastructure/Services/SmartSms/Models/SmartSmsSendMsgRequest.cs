namespace SmsMs.Infrastructure.Services.SmartSms.Models
{
    public class SmartSmsSendMsgRequest
    {
        public string TextMessage { get; set; }
        public string PhoneNumber { get; set; }
        public string Key { get; set; }
    }
}