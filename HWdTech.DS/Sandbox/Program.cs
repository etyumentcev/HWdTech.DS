using System;
using System.IO;
using System.Reflection;

using HWdTech;
using HWdTech.IOCs;
using HWdTech.IOCs.Strategies;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            AssemblyName name = AssemblyName.GetAssemblyName(Directory.GetCurrentDirectory() + "\\IOCDefaultImpl.dll");
            AppDomain.CurrentDomain.Load(name);

            ScopeManager.SubscribeOnCreationOfANewScope(
                (sc) =>
                {
                    sc.Add(
                        IOC.IOCKey.ToString(),
                        AppDomain.CurrentDomain.CreateInstanceAndUnwrap(
                            name.FullName,
                            "HWdTech.IOCs.IOCImpl",
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

            IScope scope = ScopeManager.CreateNew();
            ScopeManager.SetCurrent(scope);

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
