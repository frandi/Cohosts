namespace Cohosts.Lib
{
    public class HostItem
    {
        #region Constructors
        /// <summary>
        /// Instantiate HostItem object
        /// </summary>
        public HostItem()
        {

        }

        /// <summary>
        /// Instantiate HostItem object
        /// </summary>
        /// <param name="hostName">Host Name</param>
        /// <param name="ipAddress">IP Address</param>
        public HostItem(string hostName, string ipAddress)
        {
            HostName = hostName;
            IPAddress = ipAddress;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Host Name
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// IP Address
        /// </summary>
        public string IPAddress { get; set; } 
        #endregion
    }
}
