using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using HWdTech.Scopes;

namespace HWdTech.Scopes.Tests
{
    [TestClass]
    public class ScopesManagerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ScopesManager.ClearListOfSubscribers();
        }

        [TestMethod]
        public void ScopesManagerSetCurrentScope()
        {
            Mock<IScope> mock = new Mock<IScope>(MockBehavior.Strict);

            IScope sc = mock.Object;

            ScopesManager.SetCurrent(sc);

            Assert.AreSame(sc, ScopesManager.GetCurrent());
        }

        [TestMethod]
        public void ScopesManagerMaySetOwnScopeForEachThread()
        {
            Mock<IScope> mock1 = new Mock<IScope>(MockBehavior.Strict);
            IScope sc1 = mock1.Object;

            Mock<IScope> mock2 = new Mock<IScope>(MockBehavior.Strict);
            IScope sc2 = mock2.Object;

            Barrier barrier = new Barrier(2);

            Thread thread1 = new Thread(() =>
                {
                    ScopesManager.SetCurrent(sc1);
                    barrier.SignalAndWait();
                    Assert.AreSame(sc1, ScopesManager.GetCurrent());
                }
            );

            Thread thread2 = new Thread(() =>
                {
                    ScopesManager.SetCurrent(sc2);
                    barrier.SignalAndWait();
                    Assert.AreSame(sc2, ScopesManager.GetCurrent());
                }
            );

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
        }

        [TestMethod]
        public void ScopesManagerShouldCreateANewScope()
        {
            IScope sc1 = ScopesManager.CreateNew();
            IScope sc2 = ScopesManager.CreateNew();

            Assert.IsNotNull(sc1);
            Assert.IsNotNull(sc2);
            Assert.AreNotSame(sc1, sc2);
        }

        [TestMethod]
        public void ScopesManagerShouldCreateAScopeWithParentScope()
        {
            IScope sc = ScopesManager.CreateNew();
            sc.Add("key", "value");

            IScope childScope = ScopesManager.CreateNew(sc);

            Assert.AreEqual("value", childScope["key"]);
        }

        [TestMethod]
        public void ScopesManagerAllowsToSubscribeHandlerForCreationEvent()
        {
            bool wasCalled = false;
            ScopesManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    wasCalled = true;
                }
            );

            IScope scope = ScopesManager.CreateNew();

            Assert.IsTrue(wasCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(CreateScopeException))]
        public void ScopesManagerShouldThrowCreateScopeExceptionIfSubscriberThrowsAnyException()
        {
            ScopesManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    throw new Exception();
                }
            );

            ScopesManager.CreateNew();
        }

        [TestMethod]
        public void ScopesManagerShouldThrowCreateScopeExceptionInSubscribeOnCreationOfANewScopeIfSubscriberWorkingSoLong()
        {
            int timeout = 5000;

            ManualResetEvent waitForSignal = new ManualResetEvent(false);

            bool timeoutWasMissed = false;

            ManualResetEvent arriveToSubscriber = new ManualResetEvent(false);

            ScopesManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    arriveToSubscriber.Set();
                    timeoutWasMissed = !waitForSignal.WaitOne(timeout);
                }
            );

            Thread thread = new Thread(
                () =>
                {
                    ScopesManager.CreateNew();
                }
            );

            thread.Start();

            Assert.IsTrue(arriveToSubscriber.WaitOne(timeout));
            
            try
            {
                ScopesManager.SubscribeOnCreationOfANewScope((sc) => { });
                Assert.Fail();
            }
            catch (CreateScopeException)
            {
                waitForSignal.Set();
            }
            thread.Join();

            Assert.IsFalse(timeoutWasMissed);
        }

        [TestMethod]
        public void ScopesManagerShouldThrowCreateScopeExceptionInClearListOfSubsribersIfSubscriberWorkingSoLong()
        {
            int timeout = 5000;

            ManualResetEvent waitForSignal = new ManualResetEvent(false);

            bool timeoutWasMissed = false;

            ManualResetEvent arriveToSubscriber = new ManualResetEvent(false);

            ScopesManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    arriveToSubscriber.Set();
                    timeoutWasMissed = !waitForSignal.WaitOne(timeout);
                }
            );

            Thread thread = new Thread(
                () =>
                {
                    ScopesManager.CreateNew();
                }
            );

            thread.Start();

            Assert.IsTrue(arriveToSubscriber.WaitOne(timeout));

            try
            {
                ScopesManager.ClearListOfSubscribers();
                Assert.Fail();
            }
            catch (CreateScopeException)
            {
                waitForSignal.Set();
            }
            thread.Join();

            Assert.IsFalse(timeoutWasMissed);
        }

    }
}
