using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HWdTech.IOCs.Strategies.Tests
{
    [TestClass]
    public class ResolveByNameIOCStrategyTests
    {
        [TestMethod]
        public void ResolveByNameIOCStrategyShouldCallInternalStrategyToResolveDependency()
        {
            int times = 0;
            IIOCStrategy strategy = new ResolveByNameIOCStrategy(1000,
                (args) =>
                {
                    ++times;
                    return null;
                }
            );

            strategy.Resolve("key");

            Assert.AreEqual(1, times);
        }

        [TestMethod]
        public void ResolveByNameIOCStrategyShouldCallInternalStrategyToResolveDependencyOnlyOnce()
        {
            object o = new object();

            int times = 0;
            IIOCStrategy strategy = new ResolveByNameIOCStrategy(1000,
                (args) =>
                {
                    ++times;
                    return o;
                }
            );

            Assert.AreSame(o, strategy.Resolve("key"));
            Assert.AreSame(o, strategy.Resolve("key"));
            Assert.AreSame(o, strategy.Resolve("key"));
            
            Assert.AreEqual(1, times);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveByNameIOCStrategyShouldNeverCreateWithNullHandler()
        {
            new ResolveByNameIOCStrategy(100, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ResolveByNameIOCStrategyShouldNeverCreateWithLess0Timeout()
        {
            new ResolveByNameIOCStrategy(0, (args) => { return null; });
        }
    }
}
