using System;

namespace HWdTech.IOC.Strategies
{
    public class LambdaIOCStrategy: IIOCStrategy
    {
        Func<object[], object> handler;

        public LambdaIOCStrategy(Func<object[], object> handler)
        {
            if (null == handler)
            {
                throw new ArgumentNullException();
            }
            this.handler = handler;
        }

        public object Resolve(params object[] args)
        {
            return handler(args);
        }
    }
}
