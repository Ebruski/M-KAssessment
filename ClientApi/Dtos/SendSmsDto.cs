using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApi.Dtos
{
    public class SendSmsDto
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string SmsText { get; set; }
    }
}
