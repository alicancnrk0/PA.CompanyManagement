using System;
using System.Collections.Generic;
using System.Text;

namespace PA.CompanyManagement.Core.Exceptions
{
    public class PAContextUncatchedException : Exception
    {
        public PAContextUncatchedException() : base() { }

        public PAContextUncatchedException(string? message) : base(message) { }

        public PAContextUncatchedException(string? message, Exception? innerException) : base(message, innerException) { }

    }
}
