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
    public partial class DeleteItemForm : Form
    {

        private String _Item;

        public String Item
        {
            get { return _Item; }
            set { _Item = value; tb_item.Text = value; }
        }

        public DeleteItemForm()
        {
            InitializeComponent();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            this.Item = tb_item.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
