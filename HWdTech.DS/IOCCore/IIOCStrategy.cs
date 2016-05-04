namespace HWdTech.IOC
{
    public interface IIOCStrategy
    {
        object Resolve(params object[] args);
    }
}
