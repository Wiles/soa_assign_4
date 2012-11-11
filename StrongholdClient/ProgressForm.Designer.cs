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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tb_progress = new System.Windows.Forms.Label();
            this.tb_time_estimate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(11, 24);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(224, 19);
            this.progressBar1.TabIndex = 0;
            // 
            // tb_progress
            // 
            this.tb_progress.Location = new System.Drawing.Point(12, 9);
            this.tb_progress.Name = "tb_progress";
            this.tb_progress.Size = new System.Drawing.Size(223, 13);
            this.tb_progress.TabIndex = 1;
            this.tb_progress.Text = "### of #####";
            this.tb_progress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_time_estimate
            // 
            this.tb_time_estimate.Location = new System.Drawing.Point(8, 45);
            this.tb_time_estimate.Name = "tb_time_estimate";
            this.tb_time_estimate.Size = new System.Drawing.Size(227, 15);
            this.tb_time_estimate.TabIndex = 2;
            this.tb_time_estimate.Text = "Estimated time remaining";
            this.tb_time_estimate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 96);
            this.ControlBox = false;
            this.Controls.Add(this.tb_time_estimate);
            this.Controls.Add(this.tb_progress);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Progress";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label tb_progress;
        private System.Windows.Forms.Label tb_time_estimate;
    }
}