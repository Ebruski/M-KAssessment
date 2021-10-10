using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SmsMs.Application.Common.Interfaces;
using SmsMs.Application.Sms.Models;
using SmsMs.Common.Model;

namespace SmsMs.Application.Sms.Commands
{
    public class SendSmsCommand : IRequest<Result<SendSmsResponse>>
    {
        public string PhoneNumber { get; set; }
        public string SmsText { get; set; }
    }

    public class SendSmsCommandHandler : IRequestHandler<SendSmsCommand, Result<SendSmsResponse>>
    {
        public readonly ISmsService _smsService;

        public ILogger<SendSmsCommandHandler> _logger;

        public SendSmsCommandHandler(ISmsService smsService, ILogger<SendSmsCommandHandler> logger)
        {
            _smsService = smsService;
            _logger = logger;
        }

        public async Task<Result<SendSmsResponse>> Handle(SendSmsCommand request, CancellationToken cancellationToken)
        {
            //Validate
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return Result.Fail<SendSmsResponse>("Input a valid phone number");
            }

            if (string.IsNullOrEmpty(request.SmsText))
            {
                return Result.Fail<SendSmsResponse>("Sms Text cannot be null or empty");
            }

            //Send Sms
            var req = new SendSmsRequest
            {
                PhoneNumber = request.PhoneNumber,
                SmsText = request.SmsText
            };

            var rsp = await _smsService.SendSms(req);

            //Log Response to Database



            return Result.Ok(rsp);
        }
    }
}
