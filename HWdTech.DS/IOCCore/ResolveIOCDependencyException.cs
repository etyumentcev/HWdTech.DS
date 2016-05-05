using System;

namespace HWdTech
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
