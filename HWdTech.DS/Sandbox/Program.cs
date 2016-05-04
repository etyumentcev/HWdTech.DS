using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            AssemblyName name = AssemblyName.GetAssemblyName(Directory.GetCurrentDirectory() + "\\IOCImpls.dll");
            AppDomain.CurrentDomain.Load(name);

            ScopesManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    sc.Add(
                        IOC.IOCKey.ToString(),
                        AppDomain.CurrentDomain.CreateInstanceAndUnwrap(
                            name.FullName,
                            "HWdTech.IOC.Impl.IOCImpl",
                            true,
                            BindingFlags.Default, 
                            null,
                            new object[]{1000},
                            System.Globalization.CultureInfo.CurrentCulture, 
                            null
                        )
                    );
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
