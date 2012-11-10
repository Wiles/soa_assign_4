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
        /// Initializes a new instance of the
        /// <see cref="FileSystemService" /> class.
        /// </summary>
        public FileSystemService()
        {
            ChunkSize = 4096;
        }

        /// <summary>
        /// Gets the size of the chunk.
        /// </summary>
        /// <value>
        /// The size of the chunk.
        /// </value>
        public int ChunkSize { get; private set; }

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
        public void NewDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                throw new IOException("Path already exists");
            }
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        ///   <c>true</c> if the directory was deleted;
        ///   otherwise, <c>false.</c>
        /// </returns>
        public void DeleteItem(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            else if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                throw new IOException("Path does not exist.");
            }
        }

        /// <summary>
        /// Files the size in chunks.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public int FileSizeInChunks(string path)
        {
            double size = new FileInfo(path).Length;
            double chunk = this.ChunkSize;
            return (int)Math.Ceiling(size / chunk);
        }

        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public byte[] ReadFile(string path, int chunk)
        {
            var size = FileSizeInChunks(path);
            int length;
            if (chunk >= size)
            {
                throw new IndexOutOfRangeException();
            }

            int fileLength = (int)new FileInfo(path).Length;

            using (var stream = new FileStream(path, FileMode.OpenOrCreate))
            using (var reader = new BinaryReader(stream))
            {
                byte[] buffer = null;

                for (int i = 0; i <= chunk; i++)
                {
                    length = (i == size - 1) ?
                                fileLength % this.ChunkSize : this.ChunkSize;
                    buffer = reader.ReadBytes(length);
                }

                return buffer;
            }
        }

        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="data">The data.</param>
        /// <returns>The number of bytes written to file.</returns>
        public int WriteFile(string path, byte[] data)
        {
            using (var stream = new FileStream(
                            path,
                            FileMode.Append,
                            FileAccess.Write))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(data);
            }

            return data.Length;
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