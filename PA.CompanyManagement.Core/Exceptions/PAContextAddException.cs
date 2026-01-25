using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.Core.Exceptions
{
    public class PAContextAddException : Exception
    {
        public PAContextAddException() : base() { }

        public PAContextAddException(string? message) : base(message) { }

        public PAContextAddException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
