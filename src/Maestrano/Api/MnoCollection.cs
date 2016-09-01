using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Maestrano.Api
{
    public class MnoCollection<T>
    {
        public JObject Errors;
        public List<T> Data;
        public string Success;

        public void AssignPreset(string presetName)
        {
            foreach(T item in Data) {
                System.Reflection.PropertyInfo propertyInfo = item.GetType().GetProperty("PresetName");
                // make sure object has the property
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(item, presetName, null);
                }
            }
        }

        public void ThrowIfErrors()
        {
            if (Errors.Count > 0)
            {
                var error = (JProperty)Errors.First;
                throw new ResourceException(error.Name + " " + error.Value);
            }
        }
    }

}
