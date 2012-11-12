namespace FileStronghold
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Web.Configuration;
    using System.Web.Services;
    using System.Web.Services.Protocols;

    /// <summary>
    /// Web Service for managing a remote file store.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script,
    // using ASP.NET AJAX, uncomment the following line. 
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
        /// Gets the file download details.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="path">The path.</param>
        /// <returns>The file download details.</returns>
        [WebMethod]
        public FileDownloadDetails DownloadDetails(
                    string username,
                    string path)
        {
            if (!path.Split(new char[] { '\\' }, 2)[0].Equals(username))
            {
                throw new SoapException(
                    "Invalid username.",
                    SoapException.ClientFaultCode);
            }

            var details = new FileDownloadDetails();
            details.ChunkSize = this.FileService.ChunkSize;
            details.NumberOfChunks =
                    this.FileService.FileSizeInChunks(
                            this.StorageLocation + "\\" + path);

            return details;
        }

        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="path">The path.</param>
        /// <returns>The file data.</returns>
        [WebMethod]
        public byte[] DownloadFile(string username, string path, int chunk)
        {
            if (!path.Split(new char[] { '\\' }, 2)[0].Equals(username))
            {
                throw new SoapException(
                    "Invalid username.",
                    SoapException.ClientFaultCode);
            }

            return this.FileService.ReadFile(
                        this.StorageLocation + "\\" + path,
                        chunk);
        }

        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="path">The path.</param>
        /// <param name="data">The data.</param>
        /// <returns>The number of bytes written to storage.</returns>
        [WebMethod]
        public int UploadFile(string username, string path, byte[] data, bool append)
        {
            if (!path.Split(new char[] { '\\' }, 2)[0].Equals(username))
            {
                throw new SoapException(
                    "Invalid username.",
                    SoapException.ClientFaultCode);
            }

            return this.FileService.WriteFile(
                        this.StorageLocation + "\\" + path,
                        data,
                        append);
        }

        /// <summary>
        /// Creates a new directory.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="path">The path.</param>
        /// <param name="append">if set to <c>true</c> [append].</param>
        [WebMethod]
        public void NewFolder(string username, string path)
        {
            if (!path.Split(new char[]{'\\'}, 2)[0].Equals(username))
            {
                throw new SoapException(
                    "Invalid username.",
                    SoapException.ClientFaultCode);
            }
            try
            {
                this.FileService.NewDirectory(
                        this.StorageLocation + "\\" + path);
            }
            catch (Exception e)
            {
                throw new SoapException(e.Message, SoapException.ClientFaultCode);
            }
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="path">The path.</param>
        /// <returns>
        ///   <c>true</c> if the directory was deleted;
        ///   otherwise, <c>false.</c></returns>
        [WebMethod]
        public void DeleteItem(string username, string path)
        {
            if (!path.Split(new char[] { '\\' }, 2)[0].Equals(username))
            {
                throw new SoapException(
                    "Invalid username.",
                    SoapException.ClientFaultCode);
            }
            try
            {
                this.FileService.DeleteItem(
                                        this.StorageLocation + "\\" + path);
            }
            catch (Exception e)
            {
                throw new SoapException(e.Message, SoapException.ClientFaultCode);
            }            
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

        /// <summary>
        /// Gets the length of the max request.
        /// </summary>
        /// <returns>Max request length</returns>
        [WebMethod]
        public int GetMaxRequestLength()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            HttpRuntimeSection section = config.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            
            return section.MaxRequestLength;
        }
    }
}