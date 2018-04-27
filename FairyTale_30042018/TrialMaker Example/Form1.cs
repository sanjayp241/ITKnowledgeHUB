using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TrialMaker_Example
{
    public partial class Form1 : Form
    {
        private bool _Trial;

        public Form1(bool IsTrial)
        {
            InitializeComponent();

            if (IsTrial == false)
            {
                label1.Text = "Full Running";
                label1.ForeColor = Color.Blue;
            }

            _Trial = IsTrial;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_Trial == true)
                MessageBox.Show("This button don't run in trial mode");
            else
                MessageBox.Show("This button run in full mode");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This button run in trial or full mode");
        }
    }
}