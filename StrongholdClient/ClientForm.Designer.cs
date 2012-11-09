namespace StrongholdClient
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeDirectory = new System.Windows.Forms.TreeView();
            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.lblFileType = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnNewFolder = new System.Windows.Forms.Button();
            this.grpDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeDirectory
            // 
            this.treeDirectory.Location = new System.Drawing.Point(12, 12);
            this.treeDirectory.Name = "treeDirectory";
            this.treeDirectory.Size = new System.Drawing.Size(479, 302);
            this.treeDirectory.TabIndex = 0;
            this.treeDirectory.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeDirectory_AfterSelect);
            // 
            // grpDetails
            // 
            this.grpDetails.Controls.Add(this.btnNewFolder);
            this.grpDetails.Controls.Add(this.btnUpload);
            this.grpDetails.Controls.Add(this.lblFileType);
            this.grpDetails.Controls.Add(this.lblFileName);
            this.grpDetails.Controls.Add(this.btnDownload);
            this.grpDetails.Controls.Add(this.btnDelete);
            this.grpDetails.Location = new System.Drawing.Point(497, 12);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Size = new System.Drawing.Size(247, 271);
            this.grpDetails.TabIndex = 1;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Details";
            // 
            // lblFileType
            // 
            this.lblFileType.Location = new System.Drawing.Point(6, 45);
            this.lblFileType.Name = "lblFileType";
            this.lblFileType.Size = new System.Drawing.Size(229, 23);
            this.lblFileType.TabIndex = 4;
            // 
            // lblFileName
            // 
            this.lblFileName.Location = new System.Drawing.Point(6, 22);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(229, 23);
            this.lblFileName.TabIndex = 3;
            this.lblFileName.Text = "refreshing...";
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(9, 209);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(110, 25);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "Download...";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(125, 240);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(110, 25);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(506, 289);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(110, 25);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(9, 240);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(110, 25);
            this.btnUpload.TabIndex = 3;
            this.btnUpload.Text = "Upload...";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnNewFolder
            // 
            this.btnNewFolder.Location = new System.Drawing.Point(125, 209);
            this.btnNewFolder.Name = "btnNewFolder";
            this.btnNewFolder.Size = new System.Drawing.Size(110, 25);
            this.btnNewFolder.TabIndex = 5;
            this.btnNewFolder.Text = "New Folder...";
            this.btnNewFolder.UseVisualStyleBackColor = true;
            this.btnNewFolder.Click += new System.EventHandler(this.btnNewFolder_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 326);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.grpDetails);
            this.Controls.Add(this.treeDirectory);
            this.Name = "ClientForm";
            this.Text = "Stronghold Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpDetails.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeDirectory;
        private System.Windows.Forms.GroupBox grpDetails;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label lblFileType;
        private System.Windows.Forms.Button btnNewFolder;
    }
}

