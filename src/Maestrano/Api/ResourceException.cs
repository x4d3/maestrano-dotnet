using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Api
{
    class ResourceException : Exception
    {
        public ResourceException()
    {
    }

    public ResourceException(string message)
        : base(message)
    {
    }

    public ResourceException(string message, Exception inner)
        : base(message, inner)
    {
    }

    }
}
