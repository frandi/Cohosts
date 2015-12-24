using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cohosts.Lib;

namespace Cohosts.ConsoleApp
{
    class WriteUtility
    {
        internal static void AddItem(string hostsFile, string hostName, string ipAddress, bool allowDuplicateHostName, bool backupHostsFile)
        {
            HostsDocument doc = new HostsDocument(hostsFile);

            ConfirmBackup(doc, backupHostsFile);

            bool success = doc.AddItem(hostName, ipAddress, allowDuplicateHostName);
            if (success)
                Console.WriteLine($"{hostName} ({ipAddress}) has been added to Hosts file successfully.");
            else
                Console.WriteLine($"Failed to add {hostName} ({ipAddress}) to Hosts file. It might be because the host name is duplicated.");
        }

        internal static void RemoveItem(string hostsFile, string hostName, bool backupHostsFile)
        {
            HostsDocument doc = new HostsDocument(hostsFile);

            ConfirmBackup(doc, backupHostsFile);

            bool success = doc.RemoveItemByHostName(hostName);
            if (success)
                Console.WriteLine($"Host name \"{hostName}\" has been removed from Hosts file successfully");
            else
                Console.WriteLine($"Failed to remove \"{hostName}\" from Hosts file. It might be because the host name was not found in the file.");
        }

        private static void ConfirmBackup(HostsDocument doc, bool backupHostsFile)
        {
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

            if (backupHostsFile)
            {
                string backupPath = doc.BackupHostsFile();
                Console.WriteLine($"Backup file has been saved in {backupPath}");
            }
        }
    }
}
