using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.EmployeeService.Application.DTOs.Requests
{
    public record EmployeeCreateRequest
    {
        public required Guid CreatedBy {  get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public DateOnly? BirthDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? Address { get; set; }
    }
}
