using System;

namespace HWdTech.Scopes
{
    public class ReadScopeException: Exception
    {
        public ReadScopeException(string message)
            : base(message)
        {
        }

        public ReadScopeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
