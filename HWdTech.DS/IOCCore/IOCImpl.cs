namespace HWdTech
{
    public interface IIOCImpl
    {
        object Resolve(IDependencyID dependency, params object[] args);
        void Register(IDependencyID dependency, IIOCStrategy strategy);
    }
}
