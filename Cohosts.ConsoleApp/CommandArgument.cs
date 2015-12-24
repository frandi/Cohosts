using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cohosts.ConsoleApp
{
    class CommandArgument
    {
        internal bool AllowDuplicateHostName { get; set; }
        internal bool BackupHostsFile { get; set; }
        internal string Command { get; set; }
        internal string HostsFile { get; set; }
        internal string HostName { get; set; }
        internal string IPAddress { get; set; }
    }
}
