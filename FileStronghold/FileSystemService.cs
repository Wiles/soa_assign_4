using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections.ObjectModel;

namespace FileStronghold
{
    public class FileSystemService
    {
        /// <summary>
        /// Gets the directory structure.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The directory structure.</returns>
        public DirectoryListing GetDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var root = new DirectoryListing();
            root.IsDirectory = true;
            root.Name = Path.GetFileName(path);
            root.SubDirectories = BuildSubdirectories(path);

            return root;
        }

        /// <summary>
        /// Creates a new directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        ///   <c>true</c> if the directory was created;
        ///   otherwise, <c>false.</c>
        /// </returns>
        public Boolean NewDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Builds the subdirectories.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The subdirectories structure.</returns>
        private Collection<DirectoryListing> BuildSubdirectories(string path)
        {
            var dirs = new Collection<DirectoryListing>();

            // add sub directories
            foreach (var dir in Directory.GetDirectories(path))
            {
                var listing = new DirectoryListing();
                listing.Name = Path.GetFileName(dir);
                listing.IsDirectory = true;
                // recursiveness!
                listing.SubDirectories = BuildSubdirectories(dir);
                dirs.Add(listing);
            }

            // add files
            foreach (var file in Directory.GetFiles(path))
            {
                var listing = new DirectoryListing();
                listing.Name = Path.GetFileName(file);
                listing.IsDirectory = false;
                dirs.Add(listing);
            }

            return dirs;
        }
    }
}