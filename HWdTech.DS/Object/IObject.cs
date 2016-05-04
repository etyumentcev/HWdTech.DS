namespace HWdTech.Object
{
    public interface IObject
    {
        object this[string key]
        {
            get;
            set;
        }
    }
}
