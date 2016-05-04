using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HWdTech.IOC;
using HWdTech.IOC.Strategies;

namespace IOCStrategiesTests
{
    [TestClass]
    public class SingletonIOCStrategyTests
    {
        [TestMethod]
        public void SigletonIOCStrategyShouldReturnSameObject()
        {
            object etalon = 1;
            object other = 1;

            IIOCStrategy strategy = new SingletonIOCStrategy(etalon);

            object obj = (object)strategy.Resolve();

            Assert.AreEqual(etalon, obj);
            Assert.AreSame(etalon, obj);
            Assert.AreEqual(other, obj);
            Assert.AreNotSame(other, obj);
        }
    }
}
