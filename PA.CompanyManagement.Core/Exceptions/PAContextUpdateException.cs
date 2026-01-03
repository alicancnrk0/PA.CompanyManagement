using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.Core.Exceptions
{
    public class PAContextUpdateException : Exception
    {
        public PAContextUpdateException() : base() { }

        public PAContextUpdateException(string? message) : base(message) { }

        public PAContextUpdateException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
