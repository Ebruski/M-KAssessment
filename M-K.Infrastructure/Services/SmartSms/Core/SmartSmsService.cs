using SmsMs.Application.Common.Interfaces;
using SmsMs.Application.Sms.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmsMs.Common.Enums;
using Newtonsoft.Json.Linq;
using SmsMs.Infrastructure.Services.SmartSms.Models;
using M_KShared.Extensions;

namespace SmsMs.Infrastructure.Services.SmartSms.Core
{
    public class SmartSmsService : ISmsService
    {
        public IHttpClientFactory _httpClientFactory { get; set; }

        public ILogger<SmartSmsService> _logger { get; set; }


        public SmartSmsService(IHttpClientFactory httpClientFactory, ILogger<SmartSmsService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<SendSmsResponse> SendSms(SendSmsRequest requestModel)
        {
            try
            {
                //Do all that is required to send
                var httpClient = _httpClientFactory.CreateClient("SmartSmsApi");

                var req = new SmartSmsSendMsgRequest
                {
                    PhoneNumber = requestModel.PhoneNumber,
                    TextMessage = requestModel.SmsText,
                    Key = AppSettingsManager.SmartSms("Key")
                };

                var content = new StringContent(JObject.FromObject(req).ToString(), Encoding.UTF8, "application/json");

                var request = await httpClient
                    .PatchAsync("sendSms", content)
                    .ConfigureAwait(false);

                var response = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                
                var requestResponse = JsonConvert.DeserializeObject<SmartSmsSendMsgResponse>(response);

                var rsp = new SendSmsResponse
                {
                    VendorName = "SmartSms"
                };

                if (requestResponse.IsSuccess)
                    rsp.Status = NotificationStatus.Success;
                else
                    rsp.Status = NotificationStatus.Pending;

                //Log to SmartSms Database

                return rsp;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }
    }
}
