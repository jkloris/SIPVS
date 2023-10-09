using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        public String XML_file = String.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        public void changeSize(int width, int height)
        {
            this.Size = new Size(width, height);
        }

        private void transformBtn_Click(object sender, EventArgs e)
        {
            
            try
            {
                FormManager.TransformToHtml(XML_file, "../../../xsltest.xsl");
                MessageBox.Show("Formulár bol transformovaný do HTML");
            }
            catch (Exception exception)
            {
                //MessageBox.Show(exception.ToString());
                MessageBox.Show("Najskôr vyplňte a uložte formular prosím.");
            }
        }

        private void checkBtn_Click(object sender, EventArgs e)
        {
            FormManager.ValidateData(this.XML_file);
        }
        
        private Boolean inputValidation()
        {
            Boolean flag = true;
            Boolean missingChildName = false;
            
            //First Name
            if (string.IsNullOrEmpty(textBoxFirstName.Text) )
            {
                labelFirstName.Text = "Meno - Povinné pole";
                labelFirstName.ForeColor = System.Drawing.Color.Red;
                flag = false;
            }
            else
            {
                labelFirstName.Text = "Meno";
                labelFirstName.ForeColor = System.Drawing.Color.Black;
            }
            
            //Last name
            if (string.IsNullOrEmpty(textBoxLastName.Text) )
            {
                labelLastName.Text = "Priezvisko - Povinné pole";
                labelLastName.ForeColor = System.Drawing.Color.Red;
                flag = false;
            }
            else
            {
                labelLastName.Text = "Priezvisko";
                labelLastName.ForeColor = System.Drawing.Color.Black;
            }
            
            //Street
            if (string.IsNullOrEmpty(textBoxStreet.Text) )
            {
                labelStreet.Text = "Ulica - Povinné pole";
                labelStreet.ForeColor = System.Drawing.Color.Red;
                flag = false;
            }
            else
            {
                labelStreet.Text = "Ulica";
                labelStreet.ForeColor = System.Drawing.Color.Black;
            }
            
            //HouseNumber
            if (string.IsNullOrEmpty(textBoxHouseNumber.Text ))
            {
                labelHouseNumber.Text = "Súpisné číslo - Povinné pole";
                labelHouseNumber.ForeColor = System.Drawing.Color.Red;
                flag = false;
            }
            else
            {
                labelHouseNumber.Text = "Súpisné/orientačné číslo";
                labelHouseNumber.ForeColor = System.Drawing.Color.Black;
            }
            
            //PostalCode
            if (string.IsNullOrEmpty(textBoxPostalCode.Text ))
            {
                labelPostalCode.Text = "PSČ - Povinné pole";
                labelPostalCode.ForeColor = System.Drawing.Color.Red;
                flag = false;
            }
            else
            {
                if (!textBoxPostalCode.Text.All(char.IsDigit) || textBoxPostalCode.Text.Length != 5)
                {
                    labelPostalCode.Text = "PSČ - Zadajte spolu 5 číslic";
                    labelPostalCode.ForeColor = System.Drawing.Color.Red;
                    flag = false;
                }
                else
                {
                    labelPostalCode.Text = "PSČ";
                    labelPostalCode.ForeColor = System.Drawing.Color.Black; 
                }
                
            }
            
            //City
            if (string.IsNullOrEmpty(textBoxCity.Text ))
            {
                labelCity.Text = "Obec - Povinné pole";
                labelCity.ForeColor = System.Drawing.Color.Red;
                flag = false;
            }
            else
            {
                labelCity.Text = "Obec";
                labelCity.ForeColor = System.Drawing.Color.Black;
            }
            
            //State
            if (string.IsNullOrEmpty(textBoxState.Text ))
            {
                labelState.Text = "Štát - Povinné pole";
                labelState.ForeColor = System.Drawing.Color.Red;
                flag = false;
            }
            else
            {
                labelState.Text = "Štát";
                labelState.ForeColor = System.Drawing.Color.Black;
            }

            for (int i = 1; i <= numericUpDownNumberOfChildren.Value; i++)
            {
                string name = Controls.Find("textboxChildName_" + i.ToString(), true).FirstOrDefault().Text;
                if (string.IsNullOrEmpty(name))
                {
                    flag = false;
                    missingChildName = true;
                    break;
                }
            }

            if(missingChildName)
            {
                labelChildrenNamesValidation.Visible = true;
            }
            else
            {
                labelChildrenNamesValidation.Visible = false;
            }
            
            return flag;
            
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (this.inputValidation())
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

                this.XML_file = FormManager.SaveData(data);
            }
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
