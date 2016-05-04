using System;

namespace HWdTech.Objects
{
    public class ReadObjectException: Exception
    {
        public ReadObjectException(string message)
            : base(message)
        {
        }

        public ReadObjectException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
