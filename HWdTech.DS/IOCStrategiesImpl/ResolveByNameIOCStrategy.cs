using System;
using System.Collections.Generic;
using System.Threading;

namespace HWdTech.IOC.Strategies
{
    public class ResolveByNameIOCStrategy: IIOCStrategy
    {
        Dictionary<string, object> objects = new Dictionary<string,object>();
        ReaderWriterLock rw = new ReaderWriterLock();
        int timeout;

        Func<object[], object> strategy;

        public ResolveByNameIOCStrategy(int timeout, Func<object[], object> strategy)
        {
            if (null == strategy)
            {
                throw new ArgumentNullException();
            }
            if (timeout <= 0)
            {
                throw new ArgumentException("Arg 'timeout' should be great more than 0.");
            }
            this.timeout = timeout;
            this.strategy = strategy;
        }

        public object Resolve(params object[] args)
        {
            try
            {
                rw.AcquireReaderLock(timeout);
                object result;
                if (objects.TryGetValue((string)args[0], out result))
                {
                    rw.ReleaseReaderLock();
                }
                else
                {
                    rw.ReleaseReaderLock();
                    try
                    {
                        rw.AcquireWriterLock(timeout);
                        if (!objects.TryGetValue((string)args[0], out result))
                        {
                            result = strategy(args);
                            objects.Add((string)args[0], result);
                        }
                    }
                    catch (ApplicationException ex)
                    {
                        throw new ResolveIOCDependencyException("Timeout was missed till try to add a new object in ResolveByNameIOCStrategy", ex);
                    }
                    finally
                    {
                        rw.ReleaseWriterLock();
                    }
                }
                return result;
            }
            catch (ApplicationException ex)
            {
                throw new ResolveIOCDependencyException("Timeout was missed till try to resolve object in ResolveByNameIOCStrategy", ex);
            }
        }
    }
}
