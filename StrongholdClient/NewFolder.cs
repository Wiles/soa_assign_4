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
    public partial class NewFolder : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewFolder" /> class.
        /// </summary>
        public NewFolder()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        public string Folder
        {
            get { return tb_folder.Text; }
            set { tb_folder.Text = value; }
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
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();

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
            Folder = tb_folder.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Handles the KeyDown event of the tb_folder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="KeyEventArgs" />
        ///   instance containing the event data.
        /// </param>
        private void tb_folder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(this, EventArgs.Empty);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                button2_Click(this, EventArgs.Empty);
            }
        }
    }
}
