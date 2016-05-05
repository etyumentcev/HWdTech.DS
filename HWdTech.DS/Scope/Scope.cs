using System;
using System.Collections.Generic;
using System.Threading;

namespace HWdTech.Scopes
{
    public class Scope: IScope
    {
        int timeout;
        IScope parent;

        public Scope(int timeout, IScope parent = null)
        {
            if (timeout <= 0)
            {
                throw new ArgumentException("Arg 'timeout' should be great more than 0.");
            }
            this.timeout = timeout;
            this.parent = parent;
        }

        Dictionary<string, object> scopeData = new Dictionary<string, object>();
        ReaderWriterLock rw = new ReaderWriterLock();

        public void Add(string key, object value)
        {
            try
            {
                rw.AcquireWriterLock(timeout);
                try
                {
                    scopeData.Add(key, value);
                }
                catch (ArgumentNullException ex)
                {
                    throw new ChangeScopeException("Arg 'key' must be not a null.", ex);
                }
                catch (ArgumentException)
                {
                    scopeData.Remove(key);
                    scopeData.Add(key, value);
                }
                finally
                {
                    rw.ReleaseWriterLock();
                }
            }
            catch(ApplicationException ex)
            {
                throw new ChangeScopeException("Timeout is missed till try to add a new record to the scope.", ex);
            }
        }

        public void Remove(string key)
        {
            try
            {
                rw.AcquireWriterLock(timeout);
                try
                {
                    scopeData.Remove(key);
                }
                catch(ArgumentNullException ex)
                {
                    throw new ChangeScopeException("Arg 'Key' must not be a null.", ex);
                }
            }
            catch (ApplicationException ex)
            {
                throw new ChangeScopeException("Timeout is missed till try to remove a record from the scope.", ex);
            }
        }

        public object this[string key]
        {
            get 
            {
                try
                {
                    rw.AcquireReaderLock(timeout);
                    try
                    {
                        object value;
                        if (scopeData.TryGetValue(key, out value))
                        {
                            return value;
                        }
                        else
                        {
                            if (null != parent)
                            {
                                return parent[key];
                            }
                            else
                            {
                                throw new ReadScopeException("There is no record for the key in the scope.");
                            }
                        }
                    }
                    catch(ArgumentNullException ex)
                    {
                        throw new ReadScopeException("Arg 'Key' must not be a null.", ex);
                    }
                }
                catch (ApplicationException ex)
                {
                    throw new ReadScopeException("Timeout is missed till try to read a record from the scope.", ex);
                }
            }
        }
    }
}
