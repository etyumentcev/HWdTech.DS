using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HWdTech;

namespace HWdTech.Tests
{
    [TestClass]
    public class CreateExceptions
    {
        [TestMethod]
        public void RegisterIOCDependencyExceptionCanBeCreatedUsingMessage()
        {
            Exception ex = new RegisterIOCDependencyException("message");

            Assert.AreEqual("message", ex.Message);
        }

        [TestMethod]
        public void RegisterIOCDependencyExceptionCanBeCreatedUsingMessageAndInnerException()
        {
            Exception inner = new Exception();

            Exception ex = new RegisterIOCDependencyException("message", inner);

            Assert.AreSame(inner, ex.InnerException);
        }

        [TestMethod]
        public void ResolveIOCDependencyExceptionCanBeCreatedUsingMessage()
        {
            Exception ex = new ResolveIOCDependencyException("message");

            Assert.AreEqual("message", ex.Message);
        }

        [TestMethod]
        public void ResolveIOCDependencyExceptionCanBeCreatedUsingMessageAndInnerException()
        {
            Exception inner = new Exception();

            Exception ex = new ResolveIOCDependencyException("message", inner);

            Assert.AreSame(inner, ex.InnerException);
        }
    }
}
