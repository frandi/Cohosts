using System;

namespace Cohosts.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // parse arguments
                ArgumentValues cmdArg = ConsoleUtility.ParseArgumentValues(args);

                // do action based on command
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
                    case Commands.VERSION:
                        ConsoleUtility.ShowVersion();
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
