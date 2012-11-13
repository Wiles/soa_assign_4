using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using System.Threading;
using StrongholdClient.FileStronghold;
using System.ComponentModel;

namespace StrongholdClient
{
    public partial class ClientForm : Form
    {

        private static int MIN_UPLOAD_CHUNK = 1024 * 1024;
        private static int MAX_UPLOAD_CHUNK = 1024 * 1024 * 8;
        private static int UPLOAD_HEADER_SIZE = 1024 * 1024 * 3;

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
            var dialog = new SaveFileDialog();
            dialog.Title = "Select a download target";
            var remotePath = this.GetPath();
            var fileName = Path.GetFileName(remotePath);
            dialog.OverwritePrompt = true;
            dialog.FileName = fileName;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DownloadFile(dialog.FileName, remotePath);
            }
        }

        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="localPath">The local path.</param>
        /// <param name="remotePath">The remote path.</param>
        private void DownloadFile(string localPath, string remotePath)
        {
            ProgressForm progress = new ProgressForm("Downloading...");
            var details = this.Client.DownloadDetails(UserName, remotePath);
            progress.Total = (long)details.NumberOfChunks * (long)details.ChunkSize;

            using (var bw = new BackgroundWorker())
            {
                using (var strm = new FileStream(localPath, FileMode.OpenOrCreate))
                {
                    using (var writer = new BinaryWriter(strm))
                    {
                        bw.DoWork += (sender, e) =>
                        {
                            for (int i = 0; i < details.NumberOfChunks; i++)
                            {
                                // Exit the loop if we're told to cancel
                                if (bw.CancellationPending)
                                {
                                    e.Cancel = true;
                                    break;
                                }

                                var data = this.Client.DownloadFile(UserName, remotePath, i);
                                writer.Write(data);
                                bw.ReportProgress(details.ChunkSize);
                            }
                        };

                        bw.ProgressChanged += (sender, e) =>
                        {
                            progress.IncrementValue(e.ProgressPercentage);
                        };

                        bw.RunWorkerCompleted += (sender, e) =>
                        {
                            progress.InvokeOnUI(progress.Close);
                        };

                        progress.OnCancel += (sender, e) =>
                        {
                            bw.CancelAsync();
                        };

                        bw.WorkerReportsProgress = true;
                        bw.WorkerSupportsCancellation = true;
                        bw.RunWorkerAsync();
                        progress.ShowDialog();
                    }
                }
            }
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
            var dialog = new NewFolder();
            dialog.Folder = this.GetPath();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String folder = dialog.Folder;
                try{
                    this.Client.NewFolder(UserName, folder);
                    this.RefreshFileDirectory();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
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
            var dialog = new OpenFileDialog();
            dialog.Title = "Select a file to upload...";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var localPath in dialog.FileNames)
	            {
                    var file = Path.GetFileName(localPath);
                    var form = new UploadForm(this.GetPath(), file);
                    if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        UploadFile(localPath, form.Path);
                    }
	            }
            }
        }

        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="localPath">The local path.</param>
        /// <param name="remotePath">The remote path.</param>
        private void UploadFile(string localPath, string remotePath)
        {
            ProgressForm progress = new ProgressForm("Uploading...");

            var length = new FileInfo(localPath).Length;

            // Create the file download chunks and get ready to update the
            // progress bar
            progress.Total = length;
            var chunk = MIN_UPLOAD_CHUNK;
            try
            {
                chunk = this.Client.GetMaxRequestLength() - UPLOAD_HEADER_SIZE;
            }
            finally
            {
                if (chunk < MIN_UPLOAD_CHUNK)
                {
                    chunk = MIN_UPLOAD_CHUNK;
                }
                else if (chunk > MAX_UPLOAD_CHUNK)
                {
                    chunk = MAX_UPLOAD_CHUNK;
                }
            }
            var count = (int)Math.Ceiling((double)length / (double)chunk);

            if (length < chunk)
            {
                chunk = (int)length;
            }

            using (var bw = new BackgroundWorker())
            {
                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
                bw.DoWork += (sender, e) =>
                {
                    using (var strm = new FileStream(localPath, FileMode.Open))
                    using (var reader = new BinaryReader(strm))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            var size = i == (count - 1) ? length % chunk : chunk;
                            var buffer = reader.ReadBytes((int)size);
                            bool appendToExistingFile = (i > 0);
                            this.Client.UploadFile(
                                this.UserName, remotePath, buffer, appendToExistingFile);
                            bw.ReportProgress(chunk);
                        }
                    }
                };

                bw.ProgressChanged += (sender, e) =>
                {
                    progress.IncrementValue(e.ProgressPercentage);
                };

                bw.RunWorkerCompleted += (sender, e) =>
                {
                    progress.InvokeOnUI(progress.Close);
                    if (!e.Cancelled)
                    {
                        this.RefreshFileDirectory();
                    }
                };

                progress.OnCancel += (sender, e) =>
                {
                    bw.CancelAsync();
                };

                bw.RunWorkerAsync();
                progress.ShowDialog();
            }
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
            var dialog = new DeleteItemForm(this.GetPath());
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var folder = dialog.Path;
                try
                {
                    this.Client.DeleteItem(UserName, folder);
                    this.RefreshFileDirectory();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Gets the currently selected path.
        /// </summary>
        /// <returns>The path.</returns>
        private string GetPath()
        {
            List<String> directories = new List<String>();
            TreeNode node = treeDirectory.SelectedNode;
            while (node != null)
            {
                directories.Add(node.Text);
                node = node.Parent;
            }

            if (directories.Count == 0)
            {
                directories.Add(UserName);
            }

            directories.Reverse();
            StringBuilder baseDir = new StringBuilder();
            foreach (var dir in directories)
            {
                baseDir.Append(dir).Append("\\");
            }

            baseDir.Length -= 1;
            return baseDir.ToString();
        }
    }
}
