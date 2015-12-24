using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cohosts.ConsoleApp
{
    class ConsoleUtility
    {
        private const string DEFAULT_HOSTS_FILE = @"C:\Windows\System32\drivers\etc\hosts";

        internal static CommandArgument ParseArguments(string[] args)
        {
            CommandArgument cmdArg = new CommandArgument();

            bool expectingValue = false;
            string expectingValueFor = "";
            for (int i = 0; i < args.Length; i++)
            {
                // first argument must be a command
                if (i == 0)
                {
                    cmdArg.Command = args[i].ToLower();
                    continue;
                }

                // argument value
                if (expectingValue)
                {
                    switch (expectingValueFor)
                    {
                        case Arguments.ARG_IP_ADDRESS:
                            cmdArg.IPAddress = args[i].ToLower();
                            break;
                        case Arguments.ARG_HOSTS_FILE:
                            cmdArg.HostsFile = args[i].ToLower();
                            break;
                        case Arguments.ARG_HOST_NAME:
                            cmdArg.HostName = args[i].ToLower();
                            break;
                    }

                    expectingValue = false;
                    expectingValueFor = "";

                    continue;
                }

                // argument key
                if (args[i].StartsWith("-"))
                {
                    switch (args[i].ToLower())
                    {
                        case Arguments.ARG_ALLOW_DUPLICATE:
                            cmdArg.AllowDuplicateHostName = true;
                            break;
                        case Arguments.ARG_BACKUP_FILE:
                            cmdArg.BackupHostsFile = true;
                            break;
                        default:
                            expectingValue = true;
                            expectingValueFor = args[i].ToLower();
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(cmdArg.HostsFile))
                cmdArg.HostsFile = DEFAULT_HOSTS_FILE;

            return cmdArg;
        }

        internal static void ShowHelp()
        {
            string progName = Assembly.GetEntryAssembly().GetName().Name;

            Console.WriteLine("USAGE:");
            Console.WriteLine($"\t{progName} [COMMAND] [-ARGUMENTS] [VALUES]");
            Console.WriteLine();
            Console.WriteLine("COMMAND:");
            Console.WriteLine($"\t{nameof(Commands.ADD)}\t: Add a new record to the Hosts file");
            Console.WriteLine($"\t{nameof(Commands.REMOVE)}\t: Remove a record from the Hosts file");
            Console.WriteLine($"\t{nameof(Commands.SHOW)}\t: Show available records in the Hosts file");
            Console.WriteLine();
            Console.WriteLine("ARGUMENTS:");
            Console.WriteLine(" Key - Value Arguments:");
            Console.WriteLine($"\t{Arguments.ARG_IP_ADDRESS}\t: Define IP Address");
            Console.WriteLine($"\t{Arguments.ARG_HOSTS_FILE}\t: Define the Hosts file path (Optional)");
            Console.WriteLine($"\t{Arguments.ARG_HOST_NAME}\t: Define Host Name");
            Console.WriteLine(" Key Only Arguments (boolean):");
            Console.WriteLine($"\t{Arguments.ARG_ALLOW_DUPLICATE}\t: Allow duplicate host name (for ADD command)");
            Console.WriteLine($"\t{Arguments.ARG_BACKUP_FILE}\t: Create backup before modifying hosts file (for ADD/REMOVE command)");
            Console.WriteLine();
            Console.WriteLine("EXAMPLE:");
            Console.WriteLine($"\t{progName} {nameof(Commands.SHOW)} {Arguments.ARG_IP_ADDRESS} 127.0.0.1");
        }

        internal static void ShowExceptionMessage(Exception ex)
        {
            Console.WriteLine(ex.Message);

            if (ex is UnauthorizedAccessException)
                Console.WriteLine("Please restart the command prompt with \"Run as Administrator\", and run the command again.");
        }
    }
}
