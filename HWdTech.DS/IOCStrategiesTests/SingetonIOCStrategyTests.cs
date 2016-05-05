using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HWdTech.IOCs.Strategies.Tests
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
