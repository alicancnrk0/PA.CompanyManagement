using PA.CompanyManagement.NotificationService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.NotificationService.Application.DTOs.Responses
{
    public class MessageResponse
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public DateTimeOffset? SendDate { get; set; }
    }

    public class MinimalMessageResponse
    {
        public Guid Id {  get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTimeOffset? SendDate { get; set; }
    }

    public class DetailedMessageResponse : Message
    {

    }
}
