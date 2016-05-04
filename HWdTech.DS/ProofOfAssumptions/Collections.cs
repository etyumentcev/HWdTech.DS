using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HWdTech.ProofOfAssumptions
{
    [TestClass]
    public class Collections
    {
        [TestMethod]
        public void TestMethod1()
        {
            int size = 1000000;

            Random r = new Random(size);

            Stopwatch sw;

            sw = new Stopwatch();

            ConcurrentDictionary<string, int> threadsafeCollection = new ConcurrentDictionary<string, int>();

            sw.Start();
            for (int i = 0; i < size; ++i)
            {
                threadsafeCollection.AddOrUpdate(i.ToString(), i, (k, value) => { return i; });
            }

            for (int i = 0; i < size; ++i)
            {
                threadsafeCollection.GetOrAdd(i.ToString(), i);
            }
            sw.Stop();
            Console.WriteLine("threadsafe - {0}", sw.Elapsed);

            {
                sw = new Stopwatch();

                Dictionary<string, int> nonthreadsafeCollection = new Dictionary<string, int>();

                sw.Start();
                for (int i = 0; i < size; ++i)
                {
                    nonthreadsafeCollection.Add(i.ToString(), i);
                }

                for (int i = 0; i < size; ++i)
                {
                    int value;
                    nonthreadsafeCollection.TryGetValue(i.ToString(), out value);
                }
                sw.Stop();
                Console.WriteLine("nonthreadsafe - {0}", sw.Elapsed);
            }

            {
                sw = new Stopwatch();

                ReaderWriterLock rw = new ReaderWriterLock();

                Dictionary<string, int> nonthreadsafeCollection = new Dictionary<string, int>();

                TimeSpan timeout = new TimeSpan(100 * 1000);

                sw.Start();
                for (int i = 0; i < size; ++i)
                {
                    rw.AcquireWriterLock(timeout);
                    try
                    {
                        nonthreadsafeCollection.Add(i.ToString(), i);
                    }
                    finally
                    {
                        rw.ReleaseWriterLock();
                    }

                }

                for (int i = 0; i < size; ++i)
                {
                    rw.AcquireReaderLock(timeout);
                    try
                    {
                        int value;
                        nonthreadsafeCollection.TryGetValue(i.ToString(), out value);
                    }
                    finally
                    {
                        rw.ReleaseReaderLock();
                    }
                }
                sw.Stop();
                Console.WriteLine("reader writer - {0}", sw.Elapsed);
            }


        }
    }
}
