using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HWdTech.IOC;
using HWdTech.IOC.Strategies;

namespace IOCStrategiesTests
{
    [TestClass]
    public class LambdaIOCStrategyTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LambdaIOCStrategyShouldNeverCreateWithNullStrategy()
        {
            new LambdaIOCStrategy(null);
        }

        [TestMethod]
        public void LambdaIOCStrategyShouldCallHandlerToResolveADependency()
        {
            bool wasCalled = false;
            object returningValue = 1;

            IIOCStrategy strategy = new LambdaIOCStrategy(
                (args) => { wasCalled = true;  return returningValue; }
            );

            object result = strategy.Resolve();

            Assert.AreEqual(returningValue, result);
            Assert.AreSame(returningValue, result);
            Assert.IsTrue(wasCalled);
        }

        [TestMethod]
        public void LambdaIOCStrategyShouldPassArgsToTheHandler()
        {
            bool wasCalled = false;

            IIOCStrategy strategy = new LambdaIOCStrategy(
                (args) =>
                {
                    wasCalled = true;

                    Assert.AreEqual(2, args.Length);
                    Assert.AreEqual(1, args[0]);
                    Assert.AreEqual("abc", args[1]);

                    return null;
                }
            );

            strategy.Resolve(1, "abc");

            Assert.IsTrue(wasCalled);
        }
    }
}
