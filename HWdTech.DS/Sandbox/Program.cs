using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HWdTech.IOC;
using HWdTech.IOC.Impl;
using HWdTech.IOC.Strategies;
using HWdTech.Scopes;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            ScopesManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    sc.Add(IOC.IOCKey.ToString(), new IOCImpl(1000));
                }
            );

            IScope scope = ScopesManager.CreateNew();
            ScopesManager.SetCurrent(scope);

            IOC.Register(
                IOC.IDForDependencyID,
                new ResolveByNameIOCStrategy(
                    1000,
                    (a) =>
                    {
                        return new DependencyID((string)a[0]);
                    }
                )
            );

          

            Console.WriteLine(IOC.Resolve<IDependencyID>(IOC.IDForDependencyID, "key1").ToString());
        }
    }
}
