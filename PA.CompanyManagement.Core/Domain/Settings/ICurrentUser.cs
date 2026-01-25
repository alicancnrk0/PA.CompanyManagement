using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.Core.Domain.Settings
{
    public interface ICurrentUser
    {
        Guid Id { get; }
        string Email { get; }
        string FullName { get; }
        bool IsAuthenticated { get; }
    }
}
