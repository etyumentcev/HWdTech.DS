using System;
using System.Collections.Generic;
using System.Threading;

using HWdTech.Scopes;

namespace HWdTech
{
    public class ScopeManager
    {
        static ThreadLocal<IScope> threadLocal = new ThreadLocal<IScope>();
        static int timeout = 1000;
        static List<Action<IScope>> createScopeObservers = new List<Action<IScope>>();
        static ReaderWriterLock rw = new ReaderWriterLock();

        static public void SetCurrent(IScope scope)
        {
            threadLocal.Value = scope;
        }

        static public IScope GetCurrent()
        {
            return threadLocal.Value;
        }

        static public void SubscribeOnCreationOfANewScope(Action<IScope> handler)
        {
            try
            {
                rw.AcquireWriterLock(timeout);
                try
                {
                    createScopeObservers.Add(handler);
                }
                finally
                {
                    rw.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                throw new CreateScopeException("Timeout is missed till try to subscribe on a creation of a new scope.", ex);
            }
        }

        static public IScope CreateNew(IScope parent = null)
        {
            IScope scope = new Scope(timeout, parent);
            try
            {
                rw.AcquireReaderLock(timeout);
                try
                {
                    foreach (Action<IScope> action in createScopeObservers)
                    {
                        action(scope);
                    }
                    return scope;
                }
                catch (Exception ex)
                {
                    throw new CreateScopeException("Exception was occur till try to call callback actions on creation a new scope", ex);
                }
                finally
                {
                    rw.ReleaseReaderLock();
                }
            }
            catch (ApplicationException ex)
            {
                throw new CreateScopeException("Timeout is missed till try to create a new scope.", ex);
            }
        }

        static public void ClearListOfSubscribers()
        {
            try
            {
                rw.AcquireWriterLock(timeout);
             
                createScopeObservers.Clear();
                
                rw.ReleaseWriterLock();
            }
            catch (ApplicationException ex)
            {
                throw new CreateScopeException("Timeout is missed till try to create a new scope.", ex);
            }
        }
    }
}
