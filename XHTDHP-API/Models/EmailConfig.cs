using System;
using System.Collections.Generic;

#nullable disable

namespace XHTDHP_API.Models
{
    public partial class EmailConfig
    {
        public string EmailSupport { get; set; }
        public string EmailOrder { get; set; }
        public string EmailSenderSmtp { get; set; }
        public string EmailSenderPort { get; set; }
        public string EmailSender { get; set; }
        public string EmailSenderPassword { get; set; }
        public string Telephone { get; set; }
        public string WebsiteName { get; set; }
        public string Domain { get; set; }
        public bool? EmailSenderSsl { get; set; }
        public string AdminName { get; set; }
    }
}
