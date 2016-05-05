using System;

namespace HWdTech
{
    public class CreateScopeException: Exception
    {
        public CreateScopeException(string message)
            : base(message)
        {
        }

        public CreateScopeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
