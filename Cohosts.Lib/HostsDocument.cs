using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cohosts.Lib
{
    public class HostsDocument
    {
        #region Private Variables
        private string _hostFile;
        private List<HostItem> _hostItems; 
        #endregion

        #region Constructors
        /// <summary>
        /// Instantiate HostsDocument object
        /// </summary>
        /// <param name="hostFile">Path of the Hosts file</param>
        public HostsDocument(string hostFile)
        {
            _hostFile = hostFile;
        }
        #endregion

        #region Public Fields
        /// <summary>
        /// List of all records in the Hosts
        /// </summary>
        public List<HostItem> HostItems
        {
            get
            {
                if (_hostItems == null)
                    RefreshHostItems();

                return _hostItems;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Add a new record to the Hosts
        /// </summary>
        /// <param name="hostName">Host Name</param>
        /// <param name="ipAddress">IP Address</param>
        /// <param name="allowDuplicateHostName">Allow duplicate host name in the Hosts records</param>
        /// <returns></returns>
        public bool AddItem(string hostName, string ipAddress, bool allowDuplicateHostName)
        {
            if(string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException(nameof(hostName));
            }

            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new ArgumentNullException(nameof(ipAddress));
            }

            if (!allowDuplicateHostName && ItemExists(hostName))
            {
                return false;
            }

            using (StreamWriter writer = File.AppendText(_hostFile))
            {
                writer.Write(Environment.NewLine + $"{ipAddress}\t{hostName}");
            }

            return true;
        }

        /// <summary>
        /// Create backup for the Hosts file in the same location
        /// </summary>
        /// <returns></returns>
        public string BackupHostsFile()
        {
            return BackupHostsFile(null, null);
        }

        /// <summary>
        /// Create backup for the Hosts file
        /// </summary>
        /// <param name="backupFileName">Name of the backup file</param>
        /// <param name="backupLocation">Location of the backup file to be saved</param>
        /// <returns></returns>
        public string BackupHostsFile(string backupFileName, string backupLocation)
        {
            if (string.IsNullOrEmpty(backupFileName))
                backupFileName = $"hosts.{DateTime.Now.ToString("yyyyMMddHHmmss")}.bak";

            if (string.IsNullOrEmpty(backupLocation))
                backupLocation = Path.GetDirectoryName(_hostFile);

            string backupFullPath = Path.Combine(backupLocation, backupFileName);

            File.Copy(_hostFile, backupFullPath, true);

            return backupFullPath;
        }
        
        /// <summary>
        /// Get items by the host name
        /// </summary>
        /// <param name="hostName">Host Name filter</param>
        /// <returns></returns>
        public List<HostItem> GetItemsByHostName(string hostName)
        {
            return GetItems(hostName, null);
        }

        /// <summary>
        /// Get items by the IP address
        /// </summary>
        /// <param name="ipAddress">IP Address filter</param>
        /// <returns></returns>
        public List<HostItem> GetItemsByIPAddress(string ipAddress)
        {
            return GetItems(null, ipAddress);
        }

        /// <summary>
        /// Remove existing items by the host name
        /// </summary>
        /// <param name="hostName">Host Name filter</param>
        /// <returns></returns>
        public bool RemoveItemByHostName(string hostName)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                throw new ArgumentNullException(nameof(hostName));
            }

            // it is the container of content which will be written back to the hosts file
            // the lines with matched hostname should not be included here
            string contents = "";
            bool itemFound = false;

            using(StreamReader reader = File.OpenText(_hostFile))
            {
                string line = "";
                string[] lineParts;
                string lineHostName = "";
                while (reader.Peek() >= 0)
                {
                    line = reader.ReadLine();
                    if (line.Trim().StartsWith("#")) // ignore comments
                    {
                        if (!string.IsNullOrEmpty(contents))
                            contents += Environment.NewLine;
                        contents += line;
                    }
                    else
                    {
                        lineParts = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                        if (lineParts.Length >= 2)
                            lineHostName = lineParts[1].ToLower();

                        if (lineHostName.Equals(hostName.ToLower()))
                            itemFound = true;
                        else // ignore if hostname doesn't match
                        {
                            if (!string.IsNullOrEmpty(contents))
                                contents += Environment.NewLine;
                            contents += line;
                        }
                    }
                }
            }

            // write back ignored content
            if (itemFound)
                File.WriteAllText(_hostFile, contents);

            return itemFound;
        }
        #endregion

        #region Private Methods
        private void RefreshHostItems()
        {
            _hostItems = new List<HostItem>();

            using (var reader = File.OpenText(_hostFile))
            {
                string line = "";
                string[] lineParts;
                string hostName = "";
                string ipAddress = "";

                while (reader.Peek() >= 0)
                {
                    line = reader.ReadLine().Trim();
                    if (!string.IsNullOrEmpty(line) && !line.StartsWith("#"))
                    {
                        lineParts = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                        ipAddress = line.Length >= 1 ? lineParts[0] : "";
                        hostName = line.Length >= 2 ? lineParts[1] : "";

                        _hostItems.Add(new HostItem(hostName, ipAddress));
                    }
                }
            }
        }

        private List<HostItem> GetItems(string hostName, string ipAddress)
        {
            return HostItems.Where(h =>
                        (string.IsNullOrEmpty(hostName) || h.HostName.Equals(hostName, StringComparison.CurrentCultureIgnoreCase))
                        && (string.IsNullOrEmpty(ipAddress) || h.IPAddress.Equals(ipAddress, StringComparison.CurrentCultureIgnoreCase))
                    ).ToList();
        }

        private bool ItemExists(string hostName)
        {
            return HostItems.Any(h => h.HostName.Equals(hostName, StringComparison.CurrentCultureIgnoreCase));
        } 
        #endregion
    }
}
