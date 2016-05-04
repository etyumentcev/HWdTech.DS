namespace HWdTech.Scopes
{
    public interface IScope
    {
        void Add(string key, object value);
        void Remove(string key);

        object this[string key]
        {
            get;
        }
    }
}
