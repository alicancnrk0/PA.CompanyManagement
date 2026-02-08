using PA.CompanyManagement.EmployeeService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.EmployeeService.Application.DTOs.Responses
{
    public class EmployeeResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? Address { get; set; }
    }

    public class DetailedEmployeeResponse : Employee
    {

    }
}
