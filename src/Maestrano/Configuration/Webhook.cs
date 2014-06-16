using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maestrano.Configuration
{
    public class Webhook
    {
        public Configuration.WebhookAccount Account { get; private set; }

        public Webhook()
        {
            Account = new WebhookAccount();
        }
    }
}
