using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientApi.Dtos;
using M_KShared.Extensions;
using M_KShared.Model;
using ServiceBusUtil;

namespace ClientApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmsController : ControllerBase
    {
        private readonly ILogger<SmsController> _logger;

        public SmsController(ILogger<SmsController> logger)
        {
            _logger = logger;
        }

        [HttpPost("send-sms")]
        public async Task<IActionResult> SendSms(SendSmsDto model)
        {
            if (ModelState.IsValid)
            {
                //Push To Queue
                var requestModel = new ServiceBusSendSmsRequest
                {
                    ClientId = AppSettingsManager.Fetch("ClientId"),
                    ClientReference = DateTime.Now.Ticks.ToString(),
                    PhoneNumber = model.PhoneNumber,
                    SmsText = model.SmsText
                };

                _ = await new ServiceBusManagement(AppSettingsManager.Fetch("SmsServiceQueueConnectionString")).PushToQueue(requestModel, AppSettingsManager.Fetch("SmsServiceQueueName"));

                return Ok();
            }
            else
            {
                return BadRequest("All Fields are Required");
            }
            
        }
    }
}
