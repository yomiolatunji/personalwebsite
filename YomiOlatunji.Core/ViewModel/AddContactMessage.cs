using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace YomiOlatunji.Core.ViewModel
{
    public class AddContactMessage
    {
        [MaxLength(200)]
        [Display(Name = "Name", Order = -9, Prompt = "Enter Your Full Name", Description = "Full Name")]
        public string? Name { get; set; }
        [MaxLength(200)]
        [Required]
        [Display(Name = "Email Address", Order = -9, Prompt = "Enter Your Email Address", Description = "Email Address")]
        public string EmailAddress { get; set; }
        [MaxLength(100)]
        [Display(Name = "Phone Number", Order = -9, Prompt = "Enter Your Phone Number", Description = "Phone Number")]
        public string? PhoneNumber { get; set; }
        [MaxLength(400)]
        [Display(Name = "Subject", Order = -9, Prompt = "Type your message subject", Description = "Message Subject")]
        public string? Title { get; set; }
        [Required]
        [Display(Name = "Message", Order = -9, Prompt = "Type your message here", Description = "Message")]
        public string Message { get; set; }
    }
}
