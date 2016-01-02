using CohostsLib = Cohosts.Lib;
using System;
using System.Reflection;

namespace Cohosts.ConsoleApp
{
    class ConsoleUtility
    {
        private const string DEFAULT_HOSTS_FILE = @"C:\Windows\System32\drivers\etc\hosts";

        /// <summary>
        /// Parse given arguments
        /// </summary>
        /// <param name="args">Arguments</param>
        /// <returns></returns>
        internal static ArgumentValues ParseArgumentValues(string[] args)
        {
            ArgumentValues cmdArg = new ArgumentValues();

            bool expectingValue = false;
            string expectingValueFor = "";
            for (int i = 0; i < args.Length; i++)
            {
                // first argument must be a command
                if (i == 0)
                {
                    switch (args[i].ToLower())
                    {
                        case Commands.ADD:
                        case Commands.REMOVE:
                        case Commands.SHOW:
                        case Commands.VERSION:
                            cmdArg.Command = args[i].ToLower();
                            break;
                        default:
                            throw new Exception("Command is missing.");
                    }
                    
                    continue;
                }

                // argument value
                if (expectingValue)
                {
                    if (!args[i].StartsWith("-"))
                    {
                        switch (expectingValueFor)
                        {
                            case ArgumentKeys.ARG_IP_ADDRESS:
                                cmdArg.IPAddress = args[i].ToLower();
                                break;
                            case ArgumentKeys.ARG_HOSTS_FILE:
                                cmdArg.HostsFile = args[i].ToLower();
                                break;
                            case ArgumentKeys.ARG_HOST_NAME:
                                cmdArg.HostName = args[i].ToLower();
                                break;
                        }
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
                        case ArgumentKeys.ARG_ALLOW_DUPLICATE:
                            cmdArg.AllowDuplicateHostName = true;
                            break;
                        case ArgumentKeys.ARG_BACKUP_FILE:
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

        /// <summary>
        /// Show exception message
        /// </summary>
        /// <param name="ex">Exception</param>
        internal static void ShowExceptionMessage(Exception ex)
        {
            Console.WriteLine(ex.Message);

            if (ex is UnauthorizedAccessException)
                Console.WriteLine("Please restart the command prompt with \"Run as Administrator\", and run the command again.");
        }

        /// <summary>
        /// Show help
        /// </summary>
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
            Console.WriteLine($"\t{nameof(Commands.VERSION)}\t: Show version of the application");
            Console.WriteLine();
            Console.WriteLine("ARGUMENTS:");
            Console.WriteLine(" Key - Value Arguments:");
            Console.WriteLine($"\t{ArgumentKeys.ARG_IP_ADDRESS}\t: Define IP Address");
            Console.WriteLine($"\t{ArgumentKeys.ARG_HOSTS_FILE}\t: Define the Hosts file path (Optional)");
            Console.WriteLine($"\t{ArgumentKeys.ARG_HOST_NAME}\t: Define Host Name");
            Console.WriteLine(" Key Only Arguments (boolean):");
            Console.WriteLine($"\t{ArgumentKeys.ARG_ALLOW_DUPLICATE}\t: Allow duplicate host name (for ADD command)");
            Console.WriteLine($"\t{ArgumentKeys.ARG_BACKUP_FILE}\t: Create backup before modifying hosts file (for ADD/REMOVE command)");
            Console.WriteLine();
            Console.WriteLine("EXAMPLE:");
            Console.WriteLine($"\t{progName} {nameof(Commands.SHOW)} {ArgumentKeys.ARG_IP_ADDRESS} 127.0.0.1");
        }
        
        /// <summary>
        /// Show version of the application
        /// </summary>
        internal static void ShowVersion()
        {
            Console.WriteLine($"Cohosts v{AppVersion}");
            Console.WriteLine($"CohostsLib v{CohostsLib.LibUtility.Version}");
        }

        /// <summary>
        /// Cohosts app version
        /// </summary>
        private static string AppVersion
        {
            get { return Assembly.GetEntryAssembly().GetName().Version.ToString(); }
        }
    }
}
