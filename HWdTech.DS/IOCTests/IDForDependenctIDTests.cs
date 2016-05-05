using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HWdTech.Tests
{
    [TestClass]
    public class IDForDependencyIDTests
    {
        [TestMethod]
        public void IDForDependencyIDShouldBeCreated()
        {
            IDependencyID id = IOC.IDForDependencyID;

            Assert.AreEqual(id, IOC.IDForDependencyID);
        }

        [TestMethod]
        public void IDForDependencyIDShouldBeCreatedAsSingleton()
        {
            IDependencyID id = IOC.IDForDependencyID;

            Assert.AreSame(id, IOC.IDForDependencyID);
        }
    }
}
