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
    public partial class DeleteItemForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteItemForm" /> class.
        /// </summary>
        public DeleteItemForm(string path)
        {
            InitializeComponent();
            this.Path = path;
        }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public string Path
        {
            get { return lblFile.Text; }
            set { lblFile.Text = value; }
        }

        /// <summary>
        /// Handles the Click event of the btn_delete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void btn_delete_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the btn_cancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
