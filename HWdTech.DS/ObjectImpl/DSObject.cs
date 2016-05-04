using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using HWdTech.IOC;

namespace HWdTech.Objects.Impl
{
    [JsonObject]
    public class DSObject: IObject
    {
        static IDependencyID bodyForObjectID = IOC.IOC.Resolve<IDependencyID>(IOC.IOC.IDForDependencyID, "bodyForObjectID");

        [JsonProperty]
        IDictionary<IFieldName, object> body;

        public DSObject(IDictionary<IFieldName, object> keyValuePairs)
        {
            if (null == keyValuePairs)
            {
                throw new ArgumentNullException("Arg 'keyValuePairs' must not be null.");
            }

            body = keyValuePairs;
        }

        public DSObject(string body)
        {
            this.body = JsonConvert.DeserializeObject<Dictionary<IFieldName, object>>(body);
        }

        public DSObject()
        {
            this.body = IOC.IOC.Resolve<IDictionary<IFieldName, object>>(bodyForObjectID);
        }

        public object this[IFieldName key]
        {
            get
            {
                object result;
                if (body.TryGetValue(key, out result))
                {
                    return result;
                }
                else
                {
                    throw new ReadObjectException("Field for requested fieldname has not been found.");
                }
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(body);
        }

        public void Add(IFieldName key, object value)
        {
            try
            {
                body.Add(key, value);
            }
            catch (ArgumentException)
            {
                body.Remove(key);
                body.Add(key, value);
            }
        }

        public void Remove(IFieldName key)
        {
            body.Remove(key);
        }
    }
}
