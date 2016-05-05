using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HWdTech.IOCs.Tests
{
    [TestClass]
    public class DependencyIDTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DependencyIDShouldNeverCreateByNullID()
        {
            new DependencyID(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DependencyIDShouldNeverCreateByEmptyID()
        {
            new DependencyID("");
        }

        [TestMethod]
        public void DependencyIDsShouldBeEquals()
        {
            IDependencyID id1 = new DependencyID("id");
            IDependencyID id2 = new DependencyID("id");

            Assert.AreEqual(id1, id2);
        }

        [TestMethod]
        public void DependencyIDShouldNotBeEqualsAnyOtherObject()
        {
            IDependencyID id1 = new DependencyID("id");

            Assert.AreNotEqual(id1, "id");
        }

        [TestMethod]
        public void DependencyIDShouldBeConvertedToString()
        {
            IDependencyID id = new DependencyID("id");

            IDependencyID id1 = new DependencyID(id.ToString());

            Assert.AreEqual(id, id1);
        }

        [TestMethod]
        public void DependencyIDShouldUseHashCode()
        {
            IDependencyID id1 = new DependencyID("id");
            IDependencyID id2 = new DependencyID("id");
            IDependencyID id3 = new DependencyID("id1");


            Assert.AreEqual(id1.GetHashCode(), id2.GetHashCode());
            Assert.AreNotEqual(id2.GetHashCode(), id3.GetHashCode());
        }
    }
}
