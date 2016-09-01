using Newtonsoft.Json.Linq;

namespace Maestrano.Api
{
    public class MnoObject<T>
    {
        public JObject Errors;
        public T Data;
        public string Success;

        public void AssignPreset(string presetName)
        {
            System.Reflection.PropertyInfo propertyInfo = Data.GetType().GetProperty("PresetName");
            // make sure object has the property
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(Data, presetName, null);
            }
        }

        public void ThrowIfErrors()
        {
            if (Errors.Count > 0)
            {
                var error = (JProperty) Errors.First;
                throw new ResourceException(error.Name + " " + error.Value);
            }
        }
    }
}
