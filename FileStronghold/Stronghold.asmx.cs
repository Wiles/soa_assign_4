namespace FileStronghold
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Services;
    using System.Configuration;
    using System.Collections.Specialized;
    using System.Web.Services.Protocols;
    using System.IO;

    /// <summary>
    /// Web Service for managing a remote file store.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Stronghold : WebService
    {
        public Stronghold()
        {
            this.Settings = ConfigurationManager.AppSettings;
            this.FileService = new FileSystemService();
        }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        private NameValueCollection Settings { get; set; }

        /// <summary>
        /// Gets or sets the file service.
        /// </summary>
        /// <value>
        /// The file service.
        /// </value>
        private FileSystemService FileService { get; set; }

        /// <summary>
        /// Gets the storage location.
        /// </summary>
        /// <value>
        /// The storage location.
        /// </value>
        private string StorageLocation
        {
            get
            {
                return this.Settings["StorageRoot"];
            }
        }

        /// <summary>
        /// Creates a new directory.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="path">The path.</param>
        /// <returns>
        ///   <c>true</c> if the directory was created;
        ///   otherwise, <c>false.</c>
        /// </returns>
        [WebMethod]
        public Boolean NewFolder(string username, string path)
        {
            if (!path.Split(new char[]{'\\'}, 2)[0].Equals(username))
            {
                return false;
            }
            return this.FileService.NewDirectory(this.StorageLocation + "\\" + path);
        }

        [WebMethod]
        public Boolean DeleteItem(string username, string path)
        {
            if (!path.Split(new char[] { '\\' }, 2)[0].Equals(username))
            {
                return false;
            }
            return this.FileService.DeleteItem(this.StorageLocation + "\\" + path);
        }

        /// <summary>
        /// Gets the user's directory listing.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>The user's directory listing.</returns>
        [WebMethod]
        public DirectoryListing GetDirectoryListing(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new SoapException(
                    "Invalid user name specified",
                    SoapException.ClientFaultCode);
            }

            try
            {
                var userStore = this.StorageLocation + "\\" + userName;
                return this.FileService.GetDirectory(userStore);
            }
            catch
            {
                throw new SoapException(
                    "Unhandled Server Exception Occured.",
                    SoapException.ServerFaultCode);
            }
        }
    }
}