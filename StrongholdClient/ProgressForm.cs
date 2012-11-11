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
    public partial class ProgressForm : Form
    {

        public double Total { get; set; }

        private double Progress = 0;


        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ProgressForm" /> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public ProgressForm(string title)
        {
            InitializeComponent();
            this.Title = title;
            this.Text = string.Format("{0} 0%", title);
            this.Height = 80;
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        private string Title { get; set; }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetValue(double value)
        {
            this.Progress = value;
            this.InvokeOnUI(() =>
            {
                this.progressBar1.Value = (int)((Progress/Total) * 100.0);
                this.Text = string.Format("{0} {1}%", this.Title, value);
                tb_progress.Text = string.Format("{0:n0} of {1:n0}", Progress, Total);
            });
        }

        public void IncrementValue(double value)
        {
            this.Progress += value;
            this.InvokeOnUI(() =>
            {
                int progress = (int)((Progress / Total) * 100.0);
                this.progressBar1.Value = progress;
                this.Text = string.Format("{0} {1}%", this.Title, progress);
                tb_progress.Text = string.Format("{0:n0} of {1:n0}", Progress, Total);
            });
        }
    }
}
