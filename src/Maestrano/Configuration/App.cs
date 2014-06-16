using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Configuration
{
    public class App
    {
        // The url of the application
        // e.g: http://localhost:54326
        private string _host;
        public string Host 
        { 
            get 
            { 
                if(string.IsNullOrEmpty(_host)) {
                    return "localhost";
                }
                return _host;
            }

            set { _host = value; }
        }
    }
}
