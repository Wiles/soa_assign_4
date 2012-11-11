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
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetValue(int value)
        {
            this.InvokeOnUI(() =>
            {
                this.progressBar1.Value = value;
                this.Text = string.Format("{0} {1}%", this.Title, value);
            });
        }
    }
}
