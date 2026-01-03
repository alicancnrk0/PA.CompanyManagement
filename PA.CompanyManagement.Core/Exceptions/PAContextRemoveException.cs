using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.Core.Exceptions
{
    public class PAContextRemoveException : Exception
    {
        public PAContextRemoveException() : base() { }

        public PAContextRemoveException(string? message) : base(message) { }

        public PAContextRemoveException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
