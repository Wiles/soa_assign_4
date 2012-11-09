using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StrongholdClient.FileStronghold;
using System.Collections.ObjectModel;

namespace StrongholdClient
{
    public partial class ClientForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientForm" /> class.
        /// </summary>
        public ClientForm()
        {
            InitializeComponent();
            this.Client = new StrongholdSoapClient();
        }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        private StrongholdSoapClient Client { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        private string UserName { get; set; }

        /// <summary>
        /// Handles the Load event of the Form1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void Form1_Load(object sender, EventArgs e)
        {
            var dialog = new UserNameForm();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.UserName = dialog.UserName;
                this.Text = this.Text + " - " + this.UserName;
                RefreshFileDirectory();
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// Refreshes the file directory.
        /// </summary>
        private void RefreshFileDirectory()
        {
            this.InvokeOnUI(() =>
                {
                    btnRefresh.Enabled = false;
                    btnUpload.Enabled = false;
                    btnDownload.Enabled = false;
                    btnDelete.Enabled = false;
                    btnNewFolder.Enabled = false;
                });
            this.InvokeAsync(() =>
            {
                var dir = this.Client.GetDirectoryListing(this.UserName);
                var tree = TreeBuilder.BuildTreeView(dir);
                this.InvokeOnUI(() =>
                {
                    lblFileName.Text = dir.Name;
                    treeDirectory.Nodes.Clear();
                    treeDirectory.Nodes.Add(tree);
                    btnRefresh.Enabled = true;
                    treeDirectory.Nodes[0].Expand();
                    treeDirectory.SelectedNode = treeDirectory.Nodes[0];
                });
            });
        }

        /// <summary>
        /// Handles the Click event of the btnRefresh control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshFileDirectory();
        }

        /// <summary>
        /// Handles the AfterSelect event of the treeDirectory control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="TreeViewEventArgs" />
        ///   instance containing the event data.
        /// </param>
        private void treeDirectory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                lblFileName.Text = e.Node.Text;

                if (e.Node.ForeColor == Color.Blue)
                {
                    lblFileType.Text = "directory";
                    btnDownload.Enabled = false;
                    btnUpload.Enabled = true;
                    btnDelete.Enabled = true;
                    btnNewFolder.Enabled = true;
                }
                else
                {
                    lblFileType.Text = "file";
                    btnDownload.Enabled = true;
                    btnUpload.Enabled = false;
                    btnDelete.Enabled = true;
                    btnNewFolder.Enabled = false;
                }
            }
            else
            {
                lblFileName.Text = "";
                lblFileType.Text = "";
                btnUpload.Enabled = false;
                btnDownload.Enabled = false;
                btnDelete.Enabled = false;
                btnNewFolder.Enabled = false;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnDownload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void btnDownload_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the btnNewFolder control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void btnNewFolder_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the btnUpload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void btnUpload_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
    }
}
