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

       /* private void checkBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(textBox.Text);
        }*/

        private void saveBtn_Click(object sender, EventArgs e)
        {
            FormData data = new FormData();
            data.name = textBoxFirstName.Text;
            data.surname = textBoxLastName.Text;
            data.age = numericUpDownAge.Value;

            data.degreeAfter = textBoxDegreeAfterName.Text;
            data.degreeBefore = textBoxDegreeBeforeName.Text;
            data.maritalStatus = comboBoxMaritalStatus.Text;

            data.streetName = textBoxStreet.Text;
            data.houseNum = textBoxHouseNumber.Text;
            data.postcode = textBoxPostalCode.Text;

            data.city = textBoxCity.Text;
            data.country = textBoxState.Text;

            data.tax = checkBox.Checked;
            data.date = dateTimePicker.Value;

            data.kids = getChildren();

            FormManager.SaveData(data);
        }

        private List<Child> getChildren()
        {
            List<Child> children = new List<Child>();
            string name;
            NumericUpDown age;
            for (int i = 1; i <= numericUpDownNumberOfChildren.Value; i++) {
                name = Controls.Find("textboxChildName_" + i.ToString(), true).FirstOrDefault().Text;
                age =  Controls.Find("numericUpDownChildAge_" + i.ToString(), true).FirstOrDefault() as NumericUpDown;
                children.Add(new Child(name, age.Value));


            }
            return children;
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {

        }
        
        private void numericUpDownNumberOfChildren_ValueChanged(Object sender, EventArgs e) {

            int countAfterClick =Convert.ToInt32(Math.Round(numericUpDownNumberOfChildren.Value, 0));
            int countBeforeClick = int.Parse(numericUpDownNumberOfChildren.Text);
            if (countAfterClick == 6 && countBeforeClick == 5)
            {
                Controls.Add(labelChildName2);
                Controls.Add(labelChildAge2);
            }
            if (countAfterClick == 5 && countBeforeClick == 6)
            {
                Controls.Remove(labelChildName2);
                Controls.Remove(labelChildAge2);
            }
            if (countBeforeClick < countAfterClick)
            {
                //Create the dynamic TextBox.
                TextBox textboxChildName = new TextBox();
                textboxChildName.Size = new Size(153, 22);
                textboxChildName.Name = "textboxChildName_" + (countAfterClick);
                textboxChildName.TabIndex = 18+2*countAfterClick-3;
                
                NumericUpDown numericUpDownChildAge = new NumericUpDown();
                numericUpDownChildAge.Name = "numericUpDownChildAge_" + (countAfterClick);
                numericUpDownChildAge.Size = new Size(90, 22);
                numericUpDownChildAge.Controls[0].Visible = false;
                numericUpDownChildAge.Minimum = 0;
                numericUpDownChildAge.Maximum = 26;
                numericUpDownChildAge.DecimalPlaces = 0;
                numericUpDownChildAge.TabIndex = 18+2*countAfterClick-2;
                if (countAfterClick <= 5)
                {
                    textboxChildName.Location = new Point(8, 472 + (22 * (countAfterClick - 1)));
                    numericUpDownChildAge.Location = new Point(165, 472 + (22 * (countAfterClick - 1)));
                }
                else
                {
                    textboxChildName.Location = new Point(280, 472 + (22 * (countAfterClick - 6)));
                    numericUpDownChildAge.Location = new Point(435, 472 + (22 * (countAfterClick - 6)));
                }
                Controls.Add(textboxChildName);
                Controls.Add(numericUpDownChildAge);
            }
            else if(countBeforeClick > countAfterClick)
            {
                Controls.Remove(Controls.Find("textboxChildName_" + countBeforeClick, true)[0]);
                Controls.Remove(Controls.Find("numericUpDownChildAge_" + countBeforeClick, true)[0]);
            }
            
            
        }
    }
}
