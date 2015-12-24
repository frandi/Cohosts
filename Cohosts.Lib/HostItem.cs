using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cohosts.Lib
{
    public class HostItem
    {
        public HostItem()
        {

        }

        public HostItem(string hostName, string ipAddress)
        {
            HostName = hostName;
            IPAddress = ipAddress;
        }

        public string HostName { get; set; }
        public string IPAddress { get; set; }
    }
}
