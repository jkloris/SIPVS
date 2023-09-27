using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Z1_forms.model;

namespace Z1_forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void transformBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Transform Clicked!");
        }

        private void checkBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(textBox1.Text);
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            POC_model data = new POC_model();
            data.textInput = textBox1.Text;
            data.boolInput = checkBox1.Checked;
            data.numInput = numericUpDown1.Value;
            //data.boolInput = textBox3.Text;


            FormManager.SaveData(data);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
