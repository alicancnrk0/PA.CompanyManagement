using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.EmployeeService.Application.DTOs.Requests
{
    public record EmployeeUpdateRequest
    {
        public required Guid Id { get; init; }
        public required Guid UpdatedBy { get; init; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? Address { get; set; }
    }
}
