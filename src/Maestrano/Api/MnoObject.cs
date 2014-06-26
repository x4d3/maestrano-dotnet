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
    }
}
