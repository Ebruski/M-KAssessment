using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SmsMs.Api.Controllers
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

        [HttpGet("get-all-pending-sms")]
        public IActionResult GetAllPendingSms()
        {
            return Ok();
        }

        [HttpGet("get-all-failed-sms")]
        public IActionResult GetAllFailedSms()
        {
            return Ok();
        }
    }
}
