using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano
{
    public static class Maestrano
    {
        // VERSION
        public static string Version { get { return "0.1.0"; } }

        public static string Environment { get; set; }
        public static Configuration.Sso Sso { get; private set; }
        public static Configuration.App App { get; private set; }
        public static Configuration.Api Api { get; private set; }

        static Maestrano()
        {
            App = new Configuration.App();
            Api = new Configuration.Api();
            Sso = new Configuration.Sso();
        }
    }
}
