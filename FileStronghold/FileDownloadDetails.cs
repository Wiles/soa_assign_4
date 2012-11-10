using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileStronghold
{
    public class FileDownloadDetails
    {
        /// <summary>
        /// Gets or sets the size of the chunks.
        /// </summary>
        /// <value>
        /// The size of the chunks.
        /// </value>
        public int ChunkSize { get; set; }

        /// <summary>
        /// Gets or sets the number of chunks.
        /// </summary>
        /// <value>
        /// The number of chunks.
        /// </value>
        public int NumberOfChunks { get; set; }
    }
}