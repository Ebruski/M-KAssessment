namespace SmsMs.Application.Sms.Models
{
    public class SendSmsRequest
    {
        public string PhoneNumber { get; set; }
        public string SmsText { get; set; }
    }
}