namespace M_KShared.Model
{
    public class ServiceBusSendSmsRequest
    {
        public string PhoneNumber { get; set; }
        public string SmsText { get; set; }
        public string ClientId { get; set; }
        public string ClientReference { get; set; }
    }
}
