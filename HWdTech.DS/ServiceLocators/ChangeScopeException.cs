using System;

namespace HWdTech.Scopes
{
    public class ChangeScopeException: Exception
    {
        public ChangeScopeException(string message) 
            : base(message)
        {
        }

        public ChangeScopeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
