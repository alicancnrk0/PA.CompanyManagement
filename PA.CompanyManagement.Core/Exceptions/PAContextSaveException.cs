using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.Core.Exceptions
{
    public class PAContextSaveException : Exception
    {
        public PAContextSaveException() : base() { }

        public PAContextSaveException(string? message) : base(message) { }

        public PAContextSaveException(string? message, Exception? innerException) : base(message, innerException) { }

    }
}
