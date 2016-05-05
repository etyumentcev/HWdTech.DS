using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HWdTech.Tests
{
    [TestClass]
    public class ExceptionsTests
    {
        [TestMethod]
        public void ReadScopeExceptionCanBeCreatedUsingMessage()
        {
            Exception ex = new ReadScopeException("message");

            Assert.AreEqual("message", ex.Message);
        }

        [TestMethod]
        public void ReadScopeExceptionCanBeCreatedUsingMessageAndInnerException()
        {
            Exception inner = new Exception();

            Exception ex = new ReadScopeException("message", inner);

            Assert.AreSame(inner, ex.InnerException);
        }

        [TestMethod]
        public void ChangeScopeExceptionCanBeCreatedUsingMessage()
        {
            Exception ex = new ChangeScopeException("message");

            Assert.AreEqual("message", ex.Message);
        }

        [TestMethod]
        public void ChangeScopeExceptionCanBeCreatedUsingMessageAndInnerException()
        {
            Exception inner = new Exception();

            Exception ex = new ChangeScopeException("message", inner);

            Assert.AreSame(inner, ex.InnerException);
        }

        [TestMethod]
        public void CreateScopeExceptionCanBeCreatedUsingMessage()
        {
            Exception ex = new CreateScopeException("message");

            Assert.AreEqual("message", ex.Message);
        }

        [TestMethod]
        public void CreateScopeExceptionCanBeCreatedUsingMessageAndInnerException()
        {
            Exception inner = new Exception();

            Exception ex = new CreateScopeException("message", inner);

            Assert.AreSame(inner, ex.InnerException);
        }
    }
}
