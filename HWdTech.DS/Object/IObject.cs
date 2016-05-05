namespace HWdTech.Objects
{
    public interface IObject
    {
        void Add(IFieldName key, object value);
        void Remove(IFieldName key);

        object this[IFieldName key]
        {
            get;
        }
    }
}
