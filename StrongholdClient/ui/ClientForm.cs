using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
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
        public static readonly Color DirectoryForeColor = Color.Blue;

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
        /// Help with providing functions on the tree directory
        /// </summary>
        private TreeViewHelper treeViewHelper { get; set; }

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
                this.treeViewHelper = new TreeViewHelper(UserName, treeDirectory);
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
                try
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failure to refresh file browser, because: " + ex.Message, "Error");
                }
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

                if (e.Node.ForeColor == DirectoryForeColor)
                {
                    lblFileType.Text = "directory";
                    btnDownload.Enabled = true;
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
            try
            {
                var remotePath = treeViewHelper.GetSelectedPath();
                if (treeViewHelper.IsSelectedPathDirectory())
                {
                    var dialog = new FolderBrowserDialog();
                    dialog.Description = "Select a download target";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        DownloadFolder(dialog.SelectedPath, remotePath);
                    }
                }
                else
                {
                    var dialog = new SaveFileDialog();
                    dialog.Title = "Select a download target";

                    var fileName = Path.GetFileName(remotePath);
                    dialog.OverwritePrompt = true;
                    dialog.FileName = fileName;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        DownloadFile(dialog.FileName, remotePath);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failure to process download");
            }
        }

        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="localPath">The local path.</param>
        /// <param name="remotePath">The remote path.</param>
        private void DownloadFile(string localPath, string remotePath)
        {
            try
            {
                ProgressForm progress = new ProgressForm("Downloading...");
                var details = this.Client.DownloadDetails(UserName, remotePath);
                progress.Filename = Path.GetFileName(localPath);
                progress.Total = (long)details.NumberOfChunks * (long)details.ChunkSize;

                using (var bw = new BackgroundWorker())
                using (var strm = new FileStream(localPath, FileMode.OpenOrCreate))
                using (var writer = new BinaryWriter(strm))
                {
                    bw.DoWork += (sender, e) =>
                    {
                        try
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
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failure to download file, because: " + ex.Message, "Error");
                            throw;
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
            catch (Exception)
            {
                MessageBox.Show("Failure to download file");
            }
        }

        /// <summary>
        /// Downloads the folder.
        /// </summary>
        /// <param name="localPath">The local path.</param>
        /// <param name="remotePath">The remote path.</param>
        private void DownloadFolder(string localPath, string remotePath)
        {
            try
            {
                if (treeViewHelper.IsSelectedPathDirectory())
                {
                    var selectedFolder = treeViewHelper.GetSelectedPath();
                    var files = treeViewHelper.RecursiveListFiles(treeDirectory.SelectedNode);
                    var folders = treeViewHelper.RecursiveListFolders(treeDirectory.SelectedNode);
                    var localDirectories = (from f in folders
                                            select Path.Combine(localPath, f.Value)).ToList();

                    // Create the selected node on the user's drive
                    var localSelectedFolder = Path.Combine(localPath, Path.GetFileNameWithoutExtension(selectedFolder));
                    if (!Directory.Exists(localSelectedFolder))
                    {
                        Directory.CreateDirectory(localSelectedFolder);
                    }

                    // Create the child directories
                    foreach (var directory in localDirectories)
                    {
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                    }

                    int fileDownloadIndex = 0;
                    foreach (var file in files)
                    {
                        // Remove the parent directories that the user has not selected
                        var fileLocalPath = Path.Combine(localPath, file.Value);
                        var fileRemotePath = file.Key;

                        ProgressForm progress = new ProgressForm(String.Format(
                            "Downloading {0}/{1}...", fileDownloadIndex, files.Count));
                        var details = this.Client.DownloadDetails(UserName, fileRemotePath);
                        progress.Filename = Path.GetFileName(fileLocalPath);
                        progress.Total = (long)details.NumberOfChunks * (long)details.ChunkSize;

                        using (var bw = new BackgroundWorker())
                        using (var strm = new FileStream(fileLocalPath, FileMode.OpenOrCreate))
                        using (var writer = new BinaryWriter(strm))
                        {
                            bw.DoWork += (sender, e) =>
                            {
                                try
                                {
                                    for (int i = 0; i < details.NumberOfChunks; i++)
                                    {
                                        // Exit the loop if we're told to cancel
                                        if (bw.CancellationPending)
                                        {
                                            e.Cancel = true;
                                            break;
                                        }

                                        var data = this.Client.DownloadFile(UserName, fileRemotePath, i);
                                        writer.Write(data);
                                        bw.ReportProgress(details.ChunkSize);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Failure to download folder, because: " + ex.Message, "Error");
                                    throw;
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

                            progress.StartPosition = FormStartPosition.CenterParent;

                            bw.WorkerReportsProgress = true;
                            bw.WorkerSupportsCancellation = true;
                            bw.RunWorkerAsync();

                            progress.ShowDialog();
                            if (progress.WasCancelled)
                            {
                                break;
                            }

                            fileDownloadIndex++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failure to download folder");
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
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var selectedFolder = treeViewHelper.GetSelectedPath();
                String folder = selectedFolder + "\\" + dialog.Folder;
                try
                {
                    this.Client.NewFolder(UserName, folder);
                    this.RefreshFileDirectory();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failure to create new folder, because: " + ex.Message, "Error");
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
                    var form = new UploadForm(treeViewHelper.GetSelectedPath(), file);
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
            try
            {
                ProgressForm progress = new ProgressForm("Uploading...");

                var length = new FileInfo(localPath).Length;

                // Create the file download chunks and get ready to update the
                // progress bar
                progress.Filename = Path.GetFileName(localPath);
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
                using (var strm = new FileStream(localPath, FileMode.Open))
                using (var reader = new BinaryReader(strm))
                {
                    bw.WorkerReportsProgress = true;
                    bw.WorkerSupportsCancellation = true;
                    bw.DoWork += (sender, e) =>
                    {
                        try
                        {
                            for (int i = 0; i < count; i++)
                            {
                                // Check if the worker was cancelled
                                if (bw.CancellationPending)
                                {
                                    e.Cancel = true;
                                    try
                                    {
                                        // Attempt to delete the item on the remote path
                                        this.Client.DeleteItem(this.UserName, remotePath);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Failure to delete remains of upload, because " + ex.Message);
                                    }

                                    break;
                                }

                                var size = chunk;
                                var buffer = reader.ReadBytes((int)size);
                                bool appendToExistingFile = (i > 0);
                                this.Client.UploadFile(
                                    this.UserName, remotePath, buffer, appendToExistingFile);
                                bw.ReportProgress(chunk);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failure during file download, because: " + ex.Message, "Error");
                            throw;
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
            catch (Exception)
            {
                MessageBox.Show("Failure to upload file");
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
            var dialog = new DeleteItemForm(treeViewHelper.GetSelectedPath());
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

    }
}
