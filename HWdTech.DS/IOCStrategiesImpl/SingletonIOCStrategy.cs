namespace HWdTech.IOC.Strategies
{
    public class SingletonIOCStrategy: IIOCStrategy
    {
        object singleton;

        public SingletonIOCStrategy(object singleton)
        {
            this.singleton = singleton;
        }

        public virtual object Resolve(params object[] args)
        {
            return singleton;
        }
    }
}
