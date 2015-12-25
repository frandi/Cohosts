using Cohosts.Lib;
using System;

namespace Cohosts.ConsoleApp
{
    class WriteUtility
    {
        /// <summary>
        /// Add new record to the Hosts
        /// </summary>
        /// <param name="hostsFile">Path of the Hosts. Leave empty to use default.</param>
        /// <param name="hostName">Host name of the new record</param>
        /// <param name="ipAddress">IP Address of the new record</param>
        /// <param name="allowDuplicateHostName">Allow duplication of the host name in the records</param>
        /// <param name="backupHostsFile">Create backup of the Hosts file</param>
        internal static void AddItem(string hostsFile, string hostName, string ipAddress, bool allowDuplicateHostName, bool backupHostsFile)
        {
            HostsDocument doc = new HostsDocument(hostsFile);

            // confirm if backup is needed
            ConfirmBackup(doc, backupHostsFile);

            // add the record now
            bool success = doc.AddItem(hostName, ipAddress, allowDuplicateHostName);

            // display message
            if (success)
                Console.WriteLine($"{hostName} ({ipAddress}) has been added to Hosts file successfully.");
            else
                Console.WriteLine($"Failed to add {hostName} ({ipAddress}) to Hosts file. It might be because the host name is duplicated.");
        }

        /// <summary>
        /// Remove existing records from the Hosts
        /// </summary>
        /// <param name="hostsFile">Path of the Hosts. Leave empty to use default</param>
        /// <param name="hostName">Host name of the records to remove</param>
        /// <param name="backupHostsFile">Create backup of the Hosts file</param>
        internal static void RemoveItem(string hostsFile, string hostName, bool backupHostsFile)
        {
            HostsDocument doc = new HostsDocument(hostsFile);

            // confirm if backup file is needed
            ConfirmBackup(doc, backupHostsFile);

            // remove now
            bool success = doc.RemoveItemByHostName(hostName);

            // display message
            if (success)
                Console.WriteLine($"Host name \"{hostName}\" has been removed from Hosts file successfully");
            else
                Console.WriteLine($"Failed to remove \"{hostName}\" from Hosts file. It might be because the host name was not found in the file.");
        }

        private static void ConfirmBackup(HostsDocument doc, bool backupHostsFile)
        {
            // display confirmation message only if it isn't explicitely defined in the argument
            if (!backupHostsFile)
            {
                ConsoleKeyInfo confirmation;
                do
                {
                    Console.Write("You're about to make modification on Hosts file. Backup the file first? (Y/n):");
                    confirmation = Console.ReadKey();

                    Console.WriteLine();
                } while (confirmation.Key != ConsoleKey.Y && confirmation.Key != ConsoleKey.N);

                backupHostsFile = confirmation.Key == ConsoleKey.Y;
            }

            // backup now
            if (backupHostsFile)
            {
                string backupPath = doc.BackupHostsFile();
                Console.WriteLine($"Backup file has been saved in {backupPath}");
            }
        }
    }
}
