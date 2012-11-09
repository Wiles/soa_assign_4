using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

namespace FileStronghold
{
    /// <summary>
    /// Represents 
    /// </summary>
    public class DirectoryListing
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DirectoryListing" /> class.
        /// </summary>
        public DirectoryListing()
        {
            this.SubDirectories = new Collection<DirectoryListing>();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating
        /// whether this instance is a directory.
        /// </summary>
        /// <value>
        /// <c>
        ///   true</c> if this instance is a directory; 
        ///   otherwise, <c>false</c>.
        /// </value>
        public bool IsDirectory { get; set; }

        /// <summary>
        /// Gets or sets the sub directories.
        /// </summary>
        /// <value>
        /// The sub directories.
        /// </value>
        public Collection<DirectoryListing> SubDirectories { get; set;}
    }
}