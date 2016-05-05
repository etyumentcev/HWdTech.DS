namespace HWdTech
{
    public interface IIOCStrategy
    {
        object Resolve(params object[] args);
    }
}
