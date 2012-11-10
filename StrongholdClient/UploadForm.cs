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
            if (path.EndsWith("\\"))
            {
                this.Path = path + file;
            }
            else
            {
                this.Path = path + "\\" + file;
            }
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

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the button2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
