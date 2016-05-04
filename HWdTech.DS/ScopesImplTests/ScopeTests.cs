using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HWdTech.Scopes.Impl;

namespace HWdTech.Scopes.Impl.Tests
{
    [TestClass]
    public class ScopeTests
    {
        int timeout = 1000;

        [TestMethod]
        public void ScopeShouldReturnStoredObject()
        {
            Scope s = new Scope(timeout);
            s.Add("key1", "value1");

            Assert.AreEqual("value1", (string) s["key1"]);

        }

        [TestMethod]
        [ExpectedException(typeof(ReadScopeException))]
        public void ScopeShouldThrowReadScopeExceptionOnNullKey()
        {
            Scope s = new Scope(timeout);

            object o = s[null];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ScopeShouldThrowExceptionOnLess0Timeout()
        {
            new Scope(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ChangeScopeException))]
        public void ScopeAddMethodShouldThrowChangeScopeExceptionOnNullKey()
        {
            Scope s = new Scope(timeout);

            s.Add(null, 1);
        }

        [TestMethod]
        public void ScopeAddMethodAllowsToRewriteValueForSameKey()
        {
            Scope s = new Scope(timeout);

            s.Add("key1", 1);
            s.Add("key1", 2);

            Assert.AreEqual(2, s["key1"]);
        }


        [TestMethod]
        [ExpectedException(typeof(ChangeScopeException))]
        public void ScopeRemoveMethodShouldThrowChangeScopeExceptionOnNullKey()
        {
            Scope s = new Scope(timeout);

            s.Remove(null);
        }

        [TestMethod]
        public void ScopeShouldNeverThrowExceptionIfRemoveUnexistingRecord()
        {
            Scope s = new Scope(timeout);

            s.Remove("key");
        }

        [TestMethod]
        [ExpectedException(typeof(ReadScopeException))]
        public void ScopeShouldRemoveRecordByAKey()
        {
            Scope s = new Scope(timeout);

            s.Add("key", 1);

            s.Remove("key");

            object o = s["key"];
        }

        [TestMethod]
        public void ScopeShouldUseParentScopeIfCurrentScopeHasNotARecordForTheKey()
        {
            Scope parent = new Scope(timeout);
            parent.Add("key", 123);

            Scope s = new Scope(timeout, parent);

            Assert.AreEqual(123, s["key"]);

            Console.WriteLine("{0} {1}", typeof(Scope).Assembly.FullName, typeof(Scope).FullName);
        }

        [TestMethod]
        [ExpectedException(typeof(ReadScopeException))]
        public void ScopeShouldThrowReadScopeExceptionIfRequestedRecordIsUnExists()
        {
            Scope s = new Scope(timeout);

            object o = s["unexistedKey"];
        }

        [TestMethod]
        [ExpectedException(typeof(ReadScopeException))]
        public void ScopeShouldThrowReadScopeExceptionIfRequestedKeyIsNull()
        {
            Scope s = new Scope(timeout);

            object o = s[null];
        }
    }
}
