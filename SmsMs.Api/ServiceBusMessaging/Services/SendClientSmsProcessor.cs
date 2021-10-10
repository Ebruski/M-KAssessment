using System;
using System.Threading;
using System.Threading.Tasks;
using M_KShared.Extensions;
using M_KShared.Model;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceBusUtil;
using SmsMs.Api.ServiceBusMessaging.Interface;
using SmsMs.Api.ServiceBusMessaging.Model;
using SmsMs.Application.Sms.Commands;
using SmsMs.Application.Sms.Models;
using SmsMs.Common.Enums;
using SmsMs.Common.Model;

namespace SmsMs.Api.ServiceBusMessaging.Services
{
    public class SendClientSmsProcessor : ISendClientSmsProcessor
    {
        private readonly ILogger<SendClientSmsProcessor> _logger;
        private IRequestHandler<SendSmsCommand, Result<SendSmsResponse>> _sendSmsRequestHandler { get; }

        public SendClientSmsProcessor(ILogger<SendClientSmsProcessor> logger, IRequestHandler<SendSmsCommand, Result<SendSmsResponse>> sendSmsRequestHandler)
        {
            _logger = logger;
            _sendSmsRequestHandler = sendSmsRequestHandler;
        }

        public async Task<SbResponse> ProcessMessage(ServiceBusSendSmsRequest rsp)
        {
            try
            {
                var response = new SbResponse
                {
                    Response = new ServiceBusSendSmsResponse(),
                    ShouldComplete = false
                };

                //Check If ClientId Is Valid

                //Check if ClientReference for ClientId Exist

                //if it Exist Check if its success, Failed or Pending

                //Send Sms

                var req = new SendSmsCommand
                {
                    PhoneNumber = rsp.PhoneNumber,
                    SmsText = rsp.SmsText
                };

                var reqResponse = await _sendSmsRequestHandler.Handle(req, CancellationToken.None);

                if (reqResponse.IsSuccess && reqResponse.Value is not null)
                {
                    response.ShouldComplete = true;
                    response.Response.ClientReference = rsp.ClientReference;

                    if (reqResponse.Value.Status == NotificationStatus.Failed)
                        response.Response.IsSuccess = false;
                    else if (reqResponse.Value.Status == NotificationStatus.Success)
                        response.Response.IsSuccess = true;

                    //Push to Topic
                    var serviceBusManager = new ServiceBusManagement(AppSettingsManager.Fetch("TopicConnectionString"));

                    await serviceBusManager.PushToTopic(response.Response, AppSettingsManager.Fetch("TopicName"), null);
                }
                else
                {
                    response.ShouldComplete = false;
                }


                return response;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }


        public async Task<bool> ProcessMessageGottenFromQueue(string responseStringBody)
        {
            var rsp = JsonConvert.DeserializeObject<ServiceBusSendSmsRequest>(responseStringBody);

            var processRsp = new SbResponse();

            if (rsp is not null)
                processRsp = await ProcessMessage(rsp);

            _logger.LogInformation($"Response Gotten from Process Message::: {JsonConvert.SerializeObject(processRsp)}");

            return processRsp.ShouldComplete;
        }
    }
}
