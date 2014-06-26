using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Api
{
    public class MnoObject<T>
    {
        public JObject Errors;
        public T Data;
        public string Success;

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
