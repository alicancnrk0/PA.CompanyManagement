using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.Core.Exceptions
{
    public class PAContextQueryException : Exception
    {
        public PAContextQueryException() : base() { }

        public PAContextQueryException(string? message) : base(message) { }

        public PAContextQueryException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
