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
        /// <summary>
        /// Was the dialog cancelled (different from setting DialogResult, as Modalness is not changed)
        /// </summary>
        public bool WasCancelled { get; set; }

        /// <summary>
        /// Fired when the cancel button is clicked on the dialog
        /// </summary>
        public event EventHandler OnCancel;

        /// <summary>
        /// Total amount of the progress bar
        /// </summary>
        public double Total { get; set; }

        /// <summary>
        /// Current progress (out of Total)
        /// </summary>
        private double Progress = 0;

        /// <summary>
        /// Update of the progress update events. Used to compare prior status to current.
        /// </summary>
        private Queue<KeyValuePair<int, double>> updateEvents = new Queue<KeyValuePair<int,double>>();

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
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        private string Title { get; set; }

        /// <summary>
        /// Set the filename on the dialog
        /// </summary>
        public string Filename
        {
            set
            {
                this.filenameLabel.Text = value;
            }
        }

        /// <summary>
        /// Handle loading of the dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressForm_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetValue(double value)
        {
            this.Progress = value;
            updateEvents.Clear();
            this.InvokeOnUI(() =>
            {
                this.progressBar.Value = (int)((Progress/Total) * 100.0);
                this.Text = string.Format("{0} {1}%", this.Title, value);
                tb_progress.Text = string.Format("{0:n0} of {1:n0}", Progress, Total);
                tb_time_estimate.Text = "Unknown time remaining";
            });
        }

        /// <summary>
        /// Increment the ProgressBar progress by the given value
        /// </summary>
        /// <param name="value">Value in progress format (must be less than ProgressBar max)</param>
        public void IncrementValue(double value)
        {
            this.Progress += value;
            
            updateEvents.Enqueue(new KeyValuePair<int, double>(Environment.TickCount, value));
            if(updateEvents.Count > 250)
            {
                updateEvents.Dequeue();
            }

            var time = updateEvents.Last().Key - updateEvents.First().Key;
            var updateTotal = updateEvents.Select(t => t.Value).Sum();
            var rate = updateTotal / time;
            var estimatedRemainingTime = 0;
            if(rate != 0)
            {
                estimatedRemainingTime = (int)((Total - Progress) / rate);
                if (estimatedRemainingTime < 0)
                {
                    estimatedRemainingTime = 0;
                }
            }
            this.InvokeOnUI(() =>
            {
                int progress = (int)((Progress / Total) * 100.0);
                this.progressBar.Value = progress;
                this.Text = string.Format("{0} {1}%", this.Title, progress);
                tb_progress.Text = string.Format("{0:n0} of {1:n0}", Progress, Total);
                tb_time_estimate.Text = string.Format("{0:n0} seconds remaining", estimatedRemainingTime / 1000);
            });
        }

        /// <summary>
        /// Handle Cancel clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancel_Click(object sender, EventArgs e)
        {
            WasCancelled = true;

            if (OnCancel != null)
            {
                OnCancel(this, e);
            }
        }
    }
}
