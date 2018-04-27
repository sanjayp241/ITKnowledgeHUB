using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SoftwareLocker
{
    public partial class frmDialog : Form
    {
        private string _Pass;

        public frmDialog(string BaseString,
            string Password,int DaysToEnd, string info)
        {
            InitializeComponent();
            sebBaseString.Text = BaseString;
            _Pass = Password;
            lblD.Text = DaysToEnd.ToString() + " Day(s)";
            //lblTimes.Text = Runed.ToString() + " Time(s)";
            lblText.Text = info;
            if (DaysToEnd <= 0)
            {
                lblD.Text = "false";
                //lblTimes.Text = "Finished";
                btnTrial.Enabled = false;
            }

            sebPassword.Select();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_Pass == sebPassword.Text)
            {
                MessageBox.Show("Password is correct", "Password",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
            }
            else
                MessageBox.Show("Password is incorrect", "Password",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void btnTrial_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
        }

        private void lblCallPhone_Click(object sender, EventArgs e)
        {

        }

        private void lblComment_Click(object sender, EventArgs e)
        {

        }

        private void lblDays_Click(object sender, EventArgs e)
        {

        }

        private void frmDialog_Load(object sender, EventArgs e)
        {

        }
       
    }
}