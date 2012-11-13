namespace StrongholdClient
{
    partial class ProgressForm
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.tb_progress = new System.Windows.Forms.Label();
            this.tb_time_estimate = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(15, 30);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(299, 23);
            this.progressBar.TabIndex = 0;
            // 
            // tb_progress
            // 
            this.tb_progress.Location = new System.Drawing.Point(16, 11);
            this.tb_progress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tb_progress.Name = "tb_progress";
            this.tb_progress.Size = new System.Drawing.Size(297, 16);
            this.tb_progress.TabIndex = 1;
            this.tb_progress.Text = "### of #####";
            this.tb_progress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_time_estimate
            // 
            this.tb_time_estimate.Location = new System.Drawing.Point(11, 55);
            this.tb_time_estimate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tb_time_estimate.Name = "tb_time_estimate";
            this.tb_time_estimate.Size = new System.Drawing.Size(303, 18);
            this.tb_time_estimate.TabIndex = 2;
            this.tb_time_estimate.Text = "Estimated time remaining";
            this.tb_time_estimate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(239, 97);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 4;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 132);
            this.ControlBox = false;
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.tb_time_estimate);
            this.Controls.Add(this.tb_progress);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Progress";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label tb_progress;
        private System.Windows.Forms.Label tb_time_estimate;
        private System.Windows.Forms.Button cancel;
    }
}