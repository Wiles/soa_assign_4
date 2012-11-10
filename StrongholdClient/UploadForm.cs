using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StrongholdClient
{
    public partial class UploadForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UploadForm" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="file">The file.</param>
        public UploadForm(string path, string file)
        {
            InitializeComponent();
            lblText.Text = string.Format("Upload {0} to...", file);
            this.Path = path;
        }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path
        {
            get
            {
                return txtPath.Text;
            }
            set
            {
                txtPath.Text = value;
            }
        }
    }
}
