using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HWdTech.IOC;

namespace HWdTech.IOC.Tests
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
