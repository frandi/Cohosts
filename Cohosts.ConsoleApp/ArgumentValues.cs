namespace Cohosts.ConsoleApp
{
    class ArgumentValues
    {
        /// <summary>
        /// If <em>true</em>, it will allow duplication of host name in the Hosts
        /// </summary>
        internal bool AllowDuplicateHostName { get; set; }

        /// <summary>
        /// If true, it will create backup of Hosts before modifying the file without confirmation. 
        /// If false, a confirmation will be presented to create backup file.
        /// </summary>
        internal bool BackupHostsFile { get; set; }

        /// <summary>
        /// Command to be executed
        /// </summary>
        internal string Command { get; set; }

        /// <summary>
        /// Path of the Hosts file. If it's left empty, default path will be used.
        /// </summary>
        internal string HostsFile { get; set; }

        /// <summary>
        /// Host name
        /// </summary>
        internal string HostName { get; set; }

        /// <summary>
        /// IP address
        /// </summary>
        internal string IPAddress { get; set; }
    }
}
