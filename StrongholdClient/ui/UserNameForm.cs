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
    public partial class UserNameForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="UserNameForm" /> class.
        /// </summary>
        public UserNameForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; private set; }

        /// <summary>
        /// Handles the TextChanged event of the textBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = !string.IsNullOrWhiteSpace(textBox1.Text);
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void button1_Click(object sender, EventArgs e)
        {
            UserName = textBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the button2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Handles the KeyDown event of the textBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///   The <see cref="KeyEventArgs" /> instance
        ///   containing the event data.
        /// </param>
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter &&
                !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                button1_Click(this, EventArgs.Empty);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                button2_Click(this, EventArgs.Empty);
            }
        }
    }
}
