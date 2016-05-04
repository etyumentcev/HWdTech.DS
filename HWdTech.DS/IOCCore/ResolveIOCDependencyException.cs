using System;

namespace HWdTech.IOC
{
    public class ResolveIOCDependencyException: Exception
    {
        public ResolveIOCDependencyException(string message)
            : base(message)
        {
        }

        public ResolveIOCDependencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
