using System;

namespace HWdTrech.Objects
{
    public class ChangeObjectException: Exception
    {
        public ChangeObjectException(string message)
            : base(message)
        {
        }

        public ChangeObjectException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
