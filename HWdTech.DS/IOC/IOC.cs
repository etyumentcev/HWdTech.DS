using System;

using HWdTech.IOC.Impl;
using HWdTech.Scopes;

namespace HWdTech.IOC
{
    public class IOC
    {
        static Guid iocKey = Guid.NewGuid();

        public static Guid IOCKey
        {
            get
            {
                return iocKey;
            }
        }

        public static T Resolve<T>(IDependencyID dependency, params object[] args)
        {
            try
            {
                IIOCImpl factory = (IIOCImpl)ScopesManager.GetCurrent()[IOCKey.ToString()];
                return (T)factory.Resolve(dependency, args);
            }
            catch(ResolveIOCDependencyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ResolveIOCDependencyException("", ex);
            }
        }

        public static void Register(IDependencyID dependency, IIOCStrategy strategy)
        {
            try
            {
                IIOCImpl factory = (IIOCImpl)ScopesManager.GetCurrent()[IOCKey.ToString()];
                factory.Register(dependency, strategy);
            }
            catch (RegisterIOCDependencyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RegisterIOCDependencyException("", ex);
            }
        }

        static IDependencyID idForDependencyID = new DependencyID(Guid.NewGuid().ToString());

        public static IDependencyID IDForDependencyID
        {
            get
            {
                return idForDependencyID;
            }
        }

    }
}
