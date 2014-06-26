using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Api
{
    public class JsonPropertyNameEqualityComparer : IEqualityComparer<string>
    {
        #region IEqualityComparer<string> Members

        public bool Equals(string x, string y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(string obj)
        {
            return obj.ToLowerInvariant().Replace("_", "").GetHashCode();
        }

        #endregion
    }
}
