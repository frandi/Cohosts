using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cohosts.Lib;

namespace Cohosts.ConsoleApp
{
    class ReadUtility
    {
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
