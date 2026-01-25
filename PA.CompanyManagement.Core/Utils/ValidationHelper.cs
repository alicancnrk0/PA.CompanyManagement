using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.Core.Utils
{
    public static class ValidationHelper
    {
        public static bool IsUniqueViolation(DbUpdateException ex)
        {
            return ex.InnerException switch
            {
                Microsoft.Data.SqlClient.SqlException sql when sql.Number is 2601 or 2627 => true, // SQL Server unique
                _ => false
            };
        }
    }
}
