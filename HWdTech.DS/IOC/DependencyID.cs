using System;

namespace HWdTech.IOC.Impl
{
    public class DependencyID: IDependencyID
    {
        string id;

        public DependencyID(string id)
        {
            if (null == id)
            {
                throw new ArgumentNullException();
            }
            if(string.Empty == id)
            {
                throw new ArgumentException();
            }

            this.id = id;
        }

        public override bool Equals(object obj)
        {
            if (obj is IDependencyID)
            {
                return id == obj.ToString();
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public override string ToString()
        {
            return id;
        }
    }
}
