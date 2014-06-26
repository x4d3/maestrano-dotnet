using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Api
{
    public class MnoObject : JsonObject
    {
        public bool IsError { get { return HasProperty("error"); } }
    }
}
