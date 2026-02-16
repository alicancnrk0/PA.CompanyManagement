using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.NotificationService.Application.DTOs.Requests
{
    public record MessageCreateRequest
    {
        public required string Subject { get; init; }
        public required string Body { get; init; }
        public string? Sender { get; set; }
        public string? Reciever { get; set; }
    }
}
