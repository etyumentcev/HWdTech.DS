using System;
using System.Collections.Generic;
using System.Threading;

namespace HWdTech.IOCs
{
    public class IOCImpl: IIOCImpl 
    {
        Dictionary<IDependencyID, IIOCStrategy> iocBody = new Dictionary<IDependencyID, IIOCStrategy>();
        ReaderWriterLock rw = new ReaderWriterLock();

        int timeout;

        public IOCImpl(int timeout)
        {
            if (timeout <= 0)
            {
                throw new ArgumentException("Arg 'timeout' should be great more than 0");
            }
            this.timeout = timeout;
        }

        public object Resolve(IDependencyID dependency, params object[] args)
        {
            try
            {
                rw.AcquireReaderLock(timeout);
                IIOCStrategy resolveStrategy;
                if(iocBody.TryGetValue(dependency, out resolveStrategy))
                {
                    try
                    {
                        return resolveStrategy.Resolve(args);
                    }
                    catch(Exception ex)
                    {
                        throw new ResolveIOCDependencyException("Exception was occured till the strategy was tring to resolve the dependency.", ex);
                    }
                    finally
                    {
                        rw.ReleaseReaderLock();
                    }
                }
                else
                {
                    rw.ReleaseReaderLock();
                    throw new ResolveIOCDependencyException("There is no strategy for requested dependency.");
                }
            }
            catch (ApplicationException ex)
            {
                throw new ResolveIOCDependencyException("Timeout is missed till resolve the IOC dependecy.", ex);
            }
        }

        public void Register(IDependencyID dependency, IIOCStrategy strategy)
        {
            try
            {
                rw.AcquireWriterLock(timeout);
                try
                {
                    iocBody.Add(dependency, strategy);
                }
                catch (Exception ex)
                {
                    throw new RegisterIOCDependencyException("Exception was occured till register new IOC dependency.", ex);
                }
                finally
                {
                    rw.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                throw new RegisterIOCDependencyException("Timeout is missed till register a new dependecy.", ex);
            }
        }
    }
}
