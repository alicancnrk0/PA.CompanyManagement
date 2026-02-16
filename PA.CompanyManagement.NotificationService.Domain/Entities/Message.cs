using PA.CompanyManagement.Core.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PA.CompanyManagement.NotificationService.Domain.Entities
{
    [Table("Messages")]
    public class Message : BaseEntity
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public DateTimeOffset? SendDate { get; set; }
    }
}
