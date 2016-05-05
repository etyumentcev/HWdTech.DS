using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace HWdTech.Tests
{
    [TestClass]
    public class ScopesManagerTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ScopeManager.ClearListOfSubscribers();
        }

        [TestMethod]
        public void ScopesManagerSetCurrentScope()
        {
            Mock<IScope> mock = new Mock<IScope>(MockBehavior.Strict);

            IScope sc = mock.Object;

            ScopeManager.SetCurrent(sc);

            Assert.AreSame(sc, ScopeManager.GetCurrent());
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
                    ScopeManager.SetCurrent(sc1);
                    barrier.SignalAndWait();
                    Assert.AreSame(sc1, ScopeManager.GetCurrent());
                }
            );

            Thread thread2 = new Thread(() =>
                {
                    ScopeManager.SetCurrent(sc2);
                    barrier.SignalAndWait();
                    Assert.AreSame(sc2, ScopeManager.GetCurrent());
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
            IScope sc1 = ScopeManager.CreateNew();
            IScope sc2 = ScopeManager.CreateNew();

            Assert.IsNotNull(sc1);
            Assert.IsNotNull(sc2);
            Assert.AreNotSame(sc1, sc2);
        }

        [TestMethod]
        public void ScopesManagerShouldCreateAScopeWithParentScope()
        {
            IScope sc = ScopeManager.CreateNew();
            sc.Add("key", "value");

            IScope childScope = ScopeManager.CreateNew(sc);

            Assert.AreEqual("value", childScope["key"]);
        }

        [TestMethod]
        public void ScopesManagerAllowsToSubscribeHandlerForCreationEvent()
        {
            bool wasCalled = false;
            ScopeManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    wasCalled = true;
                }
            );

            IScope scope = ScopeManager.CreateNew();

            Assert.IsTrue(wasCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(CreateScopeException))]
        public void ScopesManagerShouldThrowCreateScopeExceptionIfSubscriberThrowsAnyException()
        {
            ScopeManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    throw new Exception();
                }
            );

            ScopeManager.CreateNew();
        }

        [TestMethod]
        public void ScopesManagerShouldThrowCreateScopeExceptionInSubscribeOnCreationOfANewScopeIfSubscriberWorkingSoLong()
        {
            int timeout = 5000;

            ManualResetEvent waitForSignal = new ManualResetEvent(false);

            bool timeoutWasMissed = false;

            ManualResetEvent arriveToSubscriber = new ManualResetEvent(false);

            ScopeManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    arriveToSubscriber.Set();
                    timeoutWasMissed = !waitForSignal.WaitOne(timeout);
                }
            );

            Thread thread = new Thread(
                () =>
                {
                    ScopeManager.CreateNew();
                }
            );

            thread.Start();

            Assert.IsTrue(arriveToSubscriber.WaitOne(timeout));
            
            try
            {
                ScopeManager.SubscribeOnCreationOfANewScope((sc) => { });
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

            ScopeManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    arriveToSubscriber.Set();
                    timeoutWasMissed = !waitForSignal.WaitOne(timeout);
                }
            );

            Thread thread = new Thread(
                () =>
                {
                    ScopeManager.CreateNew();
                }
            );

            thread.Start();

            Assert.IsTrue(arriveToSubscriber.WaitOne(timeout));

            try
            {
                ScopeManager.ClearListOfSubscribers();
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
