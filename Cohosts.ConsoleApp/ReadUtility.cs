using Cohosts.Lib;
using System;
using System.Collections.Generic;

namespace Cohosts.ConsoleApp
{
    class ReadUtility
    {
        /// <summary>
        /// Show existing records
        /// </summary>
        /// <param name="hostsFile">Path of the Hosts file. Leave empty to use default.</param>
        /// <param name="hostName">Host name to show</param>
        /// <param name="ipAddress">IP Address to show</param>
        internal static void ShowRecords(string hostsFile, string hostName, string ipAddress)
        {
            HostsDocument doc = new HostsDocument(hostsFile);
            if(doc != null)
            {
                if (string.IsNullOrEmpty(hostName) && string.IsNullOrEmpty(ipAddress))
                    DisplayRecords(doc.HostItems);
                else if(!string.IsNullOrEmpty(hostName))
                    DisplayRecords(doc.GetItemsByHostName(hostName));
                else if(!string.IsNullOrEmpty(ipAddress))
                    DisplayRecords(doc.GetItemsByIPAddress(ipAddress));
            }
        }
        
        private static void DisplayRecords(List<HostItem> items)
        {
            Console.WriteLine("Host Name\tIP Address");
            Console.WriteLine("---------\t----------");
            
            foreach (var item in items)
            {
                Console.WriteLine($"{item.HostName}\t{item.IPAddress}");
            }

            Console.WriteLine("-------------------------");
            Console.WriteLine($"Found {items.Count} records in Hosts file.");
        }
    }
}
