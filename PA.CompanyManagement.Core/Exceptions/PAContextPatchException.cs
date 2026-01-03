using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.Core.Exceptions
{
    public class PAContextPatchException : Exception
    {
        public PAContextPatchException() : base() { }

        public PAContextPatchException(string? message) : base(message) { }

        public PAContextPatchException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
