using System;

namespace HWdTech.IOC
{
    public class RegisterIOCDependencyException: Exception
    {
        public RegisterIOCDependencyException(string message)
            : base(message)
        {
        }

        public RegisterIOCDependencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
