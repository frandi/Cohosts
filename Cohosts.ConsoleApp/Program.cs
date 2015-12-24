using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cohosts.Lib;

namespace Cohosts.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandArgument cmdArg = ConsoleUtility.ParseArguments(args);

            try
            {
                switch (cmdArg.Command)
                {
                    case Commands.ADD:
                        WriteUtility.AddItem(cmdArg.HostsFile, cmdArg.HostName, cmdArg.IPAddress, cmdArg.AllowDuplicateHostName, cmdArg.BackupHostsFile);
                        break;
                    case Commands.REMOVE:
                        WriteUtility.RemoveItem(cmdArg.HostsFile, cmdArg.HostName, cmdArg.BackupHostsFile);
                        break;
                    case Commands.SHOW:
                        ReadUtility.ShowRecords(cmdArg.HostsFile, cmdArg.HostName, cmdArg.IPAddress);
                        break;
                    default:
                        ConsoleUtility.ShowHelp();
                        break;
                }
            }
            catch (Exception ex)
            {
                ConsoleUtility.ShowExceptionMessage(ex);
            }
        }
    }
}
