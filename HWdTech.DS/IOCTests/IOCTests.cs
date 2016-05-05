using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace HWdTech.IOCs.Tests
{
    [TestClass]
    public class IOCTests
    {
        [TestMethod]
        public void IOCShouldRegisterIIOCStrategy()
        {
            Mock<IIOCStrategy> strategyMock = new Mock<IIOCStrategy>();

            IIOCStrategy strategy = strategyMock.Object;

            Mock<IIOCImpl> mock = new Mock<IIOCImpl>(MockBehavior.Strict);
            mock.Setup(
                (ioc) => ioc.Register(
                    It.Is<IDependencyID>((id) => id == IOC.IDForDependencyID),
                    It.Is<IIOCStrategy>((st) => Object.ReferenceEquals(strategy, st))
                )
            ).Verifiable();

            ScopeManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    sc.Add(IOC.IOCKey.ToString(), mock.Object);
                }
            );

            IScope scope = ScopeManager.CreateNew();
            ScopeManager.SetCurrent(scope);

            IOC.Register(IOC.IDForDependencyID, strategyMock.Object);

            mock.VerifyAll();
        }

        [TestMethod]
        public void IOCShouldResolveIIOCStrategy()
        {
            Mock<IIOCStrategy> strategyMock = new Mock<IIOCStrategy>();

            object o = 1;

            Mock<IIOCImpl> mock = new Mock<IIOCImpl>(MockBehavior.Strict);
            mock.Setup<object>(
                (ioc) => ioc.Resolve(
                    It.Is<IDependencyID>((id) => id == IOC.IDForDependencyID),
                    It.IsAny<object[]>()
                )
            ).Returns(o).Verifiable();

            ScopeManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    sc.Add(IOC.IOCKey.ToString(), mock.Object);
                }
            );

            IScope scope = ScopeManager.CreateNew();
            ScopeManager.SetCurrent(scope);

            object result = IOC.Resolve<object>(IOC.IDForDependencyID);

            Assert.AreSame(o, result);

            mock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(RegisterIOCDependencyException))]
        public void IOCShouldThrowRegisterIOCDependencyExceptionIfIOCImplThrowsAnyExceptionExceptRegisterIOCDependencyException()
        {
            Mock<IIOCStrategy> strategyMock = new Mock<IIOCStrategy>();
            IIOCStrategy strategy = strategyMock.Object;

            Mock<IIOCImpl> mock = new Mock<IIOCImpl>(MockBehavior.Strict);
            mock.Setup(
                (ioc) => ioc.Register(
                    It.Is<IDependencyID>((id) => id == IOC.IDForDependencyID),
                    It.Is<IIOCStrategy>((st) => Object.ReferenceEquals(st, strategy))
                )
            ).Throws(new Exception()).Verifiable();

            ScopeManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    sc.Add(IOC.IOCKey.ToString(), mock.Object);
                }
            );

            IScope scope = ScopeManager.CreateNew();
            ScopeManager.SetCurrent(scope);

            try
            {
                IOC.Register(IOC.IDForDependencyID, strategy);
            }
            catch (Exception ex)
            {
                mock.VerifyAll();
                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RegisterIOCDependencyException))]
        public void IOCShouldThrowRegisterIOCDependencyExceptionIfIOCImplThrowsRegisterIOCDependencyException()
        {
            Mock<IIOCStrategy> strategyMock = new Mock<IIOCStrategy>();
            IIOCStrategy strategy = strategyMock.Object;

            Mock<IIOCImpl> mock = new Mock<IIOCImpl>(MockBehavior.Strict);
            mock.Setup(
                (ioc) => ioc.Register(
                    It.Is<IDependencyID>((id) => id == IOC.IDForDependencyID),
                    It.Is<IIOCStrategy>((st) => Object.ReferenceEquals(st, strategy))
                )
            ).Throws(new RegisterIOCDependencyException("aa")).Verifiable();

            ScopeManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    sc.Add(IOC.IOCKey.ToString(), mock.Object);
                }
            );

            IScope scope = ScopeManager.CreateNew();
            ScopeManager.SetCurrent(scope);

            try
            {
                IOC.Register(IOC.IDForDependencyID, strategy);
            }
            catch (Exception ex)
            {
                mock.VerifyAll();
                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ResolveIOCDependencyException))]
        public void IOCShouldThrowResolveIOCDependencyExceptionIfIOCImplThrowsAnyExceptionExceptResolveIOCDependencyException()
        {
            Mock<IIOCImpl> mock = new Mock<IIOCImpl>(MockBehavior.Strict);
            mock.Setup<object>(
                (ioc) => ioc.Resolve(
                    It.Is<IDependencyID>((id) => id == IOC.IDForDependencyID),
                    It.IsAny<object[]>()
                )
            ).Throws(new Exception());

            ScopeManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    sc.Add(IOC.IOCKey.ToString(), mock.Object);
                }
            );

            IScope scope = ScopeManager.CreateNew();
            ScopeManager.SetCurrent(scope);

            try
            {
                IOC.Resolve<object>(IOC.IDForDependencyID);
            }
            catch (Exception ex)
            {
                mock.VerifyAll();
                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ResolveIOCDependencyException))]
        public void IOCShouldThrowResolveIOCDependencyExceptionIfIOCImplThrowsResolveIOCDependencyException()
        {
            Mock<IIOCImpl> mock = new Mock<IIOCImpl>(MockBehavior.Strict);
            mock.Setup<object>(
                (ioc) => ioc.Resolve(
                    It.Is<IDependencyID>((id) => id == IOC.IDForDependencyID),
                    It.IsAny<object[]>()
                )
            ).Throws(new ResolveIOCDependencyException("a"));

            ScopeManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    sc.Add(IOC.IOCKey.ToString(), mock.Object);
                }
            );

            IScope scope = ScopeManager.CreateNew();
            ScopeManager.SetCurrent(scope);

            try
            {
                IOC.Resolve<object>(IOC.IDForDependencyID);
            }
            catch (Exception ex)
            {
                mock.VerifyAll();
                throw ex;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ResolveIOCDependencyException))]
        public void IOCShouldThrowResolveIOCDependencyExceptionIfCurrentScopeIsNotSet()
        {
            ScopeManager.SetCurrent(null);

            IOC.Resolve<object>(IOC.IDForDependencyID);
        }

        [TestMethod]
        [ExpectedException(typeof(RegisterIOCDependencyException))]
        public void IOCShouldThrowRegisterIOCDependencyExceptionIfCurrentScopeIsNotSet()
        {
            ScopeManager.SetCurrent(null);

            IOC.Register(IOC.IDForDependencyID, new Mock<IIOCStrategy>().Object);
        }
    }
}
