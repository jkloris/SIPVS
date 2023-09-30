using System.Drawing;
using System.Windows.Forms;

namespace Z1_forms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.saveBtn = new System.Windows.Forms.Button();
            this.checkBtn = new System.Windows.Forms.Button();
            this.transformBtn = new System.Windows.Forms.Button();
            
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelTitleDescription1 = new System.Windows.Forms.Label();
            this.labelTitleDescription2 = new System.Windows.Forms.Label();
            this.labelEmployeeData = new System.Windows.Forms.Label();
            this.labelLastName = new System.Windows.Forms.Label();
            this.textBoxLastName = new System.Windows.Forms.TextBox();
            this.labelFirstName = new System.Windows.Forms.Label();
            this.textBoxFirstName = new System.Windows.Forms.TextBox();
            this.labelAge = new System.Windows.Forms.Label();
            this.numericUpDownAge = new System.Windows.Forms.NumericUpDown();
            this.labelDegreeBeforeName = new System.Windows.Forms.Label();
            this.textBoxDegreeBeforeName = new System.Windows.Forms.TextBox();
            this.labelDegreeAfterName = new System.Windows.Forms.Label();
            this.textBoxDegreeAfterName = new System.Windows.Forms.TextBox();
            this.labelMaritalStatus = new System.Windows.Forms.Label();
            this.comboBoxMaritalStatus = new System.Windows.Forms.ComboBox();
            this.labelAdress = new System.Windows.Forms.Label();
            this.labelStreet = new System.Windows.Forms.Label();
            this.textBoxStreet = new System.Windows.Forms.TextBox();
            this.labelHouseNumber = new System.Windows.Forms.Label();
            this.textBoxHouseNumber = new System.Windows.Forms.TextBox();
            this.labelPostalCode = new System.Windows.Forms.Label();
            this.textBoxPostalCode = new System.Windows.Forms.TextBox();
            this.labelCity = new System.Windows.Forms.Label();
            this.textBoxCity = new System.Windows.Forms.TextBox();
            this.labelState = new System.Windows.Forms.Label();
            this.textBoxState = new System.Windows.Forms.TextBox();
            this.labelTaxAplicationData = new System.Windows.Forms.Label();
            this.checkBox = new System.Windows.Forms.CheckBox();
            this.labelChildrenData = new System.Windows.Forms.Label();
            this.labelChildName1 = new System.Windows.Forms.Label();
            this.labelChildAge1 = new System.Windows.Forms.Label();
            this.labelChildName2 = new System.Windows.Forms.Label();
            this.labelChildAge2 = new System.Windows.Forms.Label();
            this.labelNumberOfChildren = new System.Windows.Forms.Label();
            this.numericUpDownNumberOfChildren = new System.Windows.Forms.NumericUpDown();
            System.Windows.Forms.TextBox textboxChildName = new System.Windows.Forms.TextBox();
            System.Windows.Forms.NumericUpDown numericUpDownChildAge = new System.Windows.Forms.NumericUpDown();
            this.labelDate = new System.Windows.Forms.Label();
            this.dateTimePicker = new DateTimePicker();
            
            
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAge)).BeginInit();
            this.SuspendLayout();
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(105, 800);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(126, 25);
            this.saveBtn.TabIndex = 0;
            this.saveBtn.Text = "Ulož";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // checkBtn
            // 
            this.checkBtn.Location = new System.Drawing.Point(337, 800);
            this.checkBtn.Name = "checkBtn";
            this.checkBtn.Size = new System.Drawing.Size(126, 25);
            this.checkBtn.TabIndex = 1;
            this.checkBtn.Text = "Over";
            this.checkBtn.UseVisualStyleBackColor = true;
            this.checkBtn.Click += new System.EventHandler(this.checkBtn_Click);
            // 
            // transformBtn
            // 
            this.transformBtn.Location = new System.Drawing.Point(569, 800);
            this.transformBtn.Name = "transformBtn";
            this.transformBtn.Size = new System.Drawing.Size(127, 25);
            this.transformBtn.TabIndex = 2;
            this.transformBtn.Text = "Transformuj";
            this.transformBtn.UseVisualStyleBackColor = true;
            this.transformBtn.Click += new System.EventHandler(this.transformBtn_Click);
            
            // 
            // labelTitle
            // 
            this.labelTitle.Location = new System.Drawing.Point(370, 10);
            this.labelTitle.Text = "VYHLÁSENIE";
            // 
            // labelTitleDescription1
            // 
            this.labelTitleDescription1.Location = new System.Drawing.Point(10, 30);
            this.labelTitleDescription1.Text = "na uplatnenie nezdaniteľnej časti základu dane na daňovníka a daňového bonusu podľa § 36 ods. 6 " +
                                               "neexistujúceho zákona o dani z príjmov v znení neskorších predpisov (ďalej len „vyhlásenie“)";
            this.labelTitleDescription1.Size = new System.Drawing.Size(850, 40);
            this.labelTitleDescription1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelTitleDescription2
            // 
            this.labelTitleDescription2.Location = new System.Drawing.Point(10, 80);
            this.labelTitleDescription2.Text = "Vyhlásenie podľa § 36 ods. 6 neexistujúceho zákona o dani z príjmov v znení neskorších predpisov" +
                                               " (ďalej " + "len „zákon“) doručí zamestnanec zamestnávateľovi, ktorý je platiteľom dane (ďalej len" +
                                               " „zamestnávateľ“), u ktorého si uplatňuje nárok na nezdaniteľnú časť základu dane na daňovníka" +
                                               " a nárok na daňový bonus (§ 33). Ak má zamestnanec súčasne viacerých zamestnávateľov, vyhlásenie" +
                                               " predloží len jednému z nich.";
            this.labelTitleDescription2.Size = new System.Drawing.Size(850, 60);
            this.labelTitleDescription2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelEmployeeData
            // 
            this.labelEmployeeData.Location = new System.Drawing.Point(10, 150);
            this.labelEmployeeData.Text = "I.\tÚDAJE O ZAMESTNANCOVI";
            this.labelEmployeeData.Size = new System.Drawing.Size(850, 20);
            // 
            // labelLastName
            // 
            this.labelLastName.Location = new System.Drawing.Point(10, 180);
            this.labelLastName.Text = "Priezvisko";
            this.labelLastName.Size = new System.Drawing.Size(200, 20);
            // 
            // textBoxLastName
            // 
            this.textBoxLastName.Location = new System.Drawing.Point(10, 200);
            this.textBoxLastName.Name = "textBoxLastName";
            this.textBoxLastName.Size = new System.Drawing.Size(200, 22);
            this.textBoxLastName.TabIndex = 3;
            // 
            // labelFirstName
            // 
            this.labelFirstName.Location = new System.Drawing.Point(220, 180);
            this.labelFirstName.Text = "Meno";
            this.labelFirstName.Size = new System.Drawing.Size(200, 20);
            // 
            // textBoxFirstName
            // 
            this.textBoxFirstName.Location = new System.Drawing.Point(220, 200);
            this.textBoxFirstName.Name = "textBoxFirstName";
            this.textBoxFirstName.Size = new System.Drawing.Size(200, 22);
            this.textBoxFirstName.TabIndex = 4;
            // 
            // labelAge
            // 
            this.labelAge.Location = new System.Drawing.Point(430, 180);
            this.labelAge.Text = "Vek";
            this.labelAge.Size = new System.Drawing.Size(200, 20);
            // 
            // numericUpDownAge
            // 
            this.numericUpDownAge.Location = new System.Drawing.Point(430, 200);
            this.numericUpDownAge.Name = "numericUpDownAge";
            this.numericUpDownAge.Size = new System.Drawing.Size(120, 22);
            this.numericUpDownAge.Controls[0].Visible = false;
            this.numericUpDownAge.Minimum = 0;
            this.numericUpDownAge.Maximum = 150;
            this.numericUpDownAge.DecimalPlaces = 0;
            this.numericUpDownAge.TabIndex = 5;
            // 
            // labelDegreeBeforeName
            // 
            this.labelDegreeBeforeName.Location = new System.Drawing.Point(10, 232);
            this.labelDegreeBeforeName.Text = "Titul (pred menom)";
            this.labelDegreeBeforeName.Size = new System.Drawing.Size(200, 20);
            // 
            // textBoxDegreeBeforeName
            // 
            this.textBoxDegreeBeforeName.Location = new System.Drawing.Point(10, 252);
            this.textBoxDegreeBeforeName.Name = "textBoxDegreeBeforeName";
            this.textBoxDegreeBeforeName.Size = new System.Drawing.Size(200, 22);
            this.textBoxDegreeBeforeName.TabIndex = 6;
            // 
            // labelDegreeAfterName
            // 
            this.labelDegreeAfterName.Location = new System.Drawing.Point(220, 232);
            this.labelDegreeAfterName.Text = "Titul (za priezviskom)";
            this.labelDegreeAfterName.Size = new System.Drawing.Size(200, 20);
            // 
            // textBoxDegreeAfterName
            // 
            this.textBoxDegreeAfterName.Location = new System.Drawing.Point(220, 252);
            this.textBoxDegreeAfterName.Name = "textBoxDegreeAfterName";
            this.textBoxDegreeAfterName.Size = new System.Drawing.Size(200, 22);
            this.textBoxDegreeAfterName.TabIndex = 7;
            // 
            // labelMaritalStatus
            // 
            this.labelMaritalStatus.Location = new System.Drawing.Point(430, 232);
            this.labelMaritalStatus.Text = "Rodinný stav";
            this.labelMaritalStatus.Size = new System.Drawing.Size(200, 20);
            // 
            //comboBoxMaritalStatus
            //
            comboBoxMaritalStatus.Items.AddRange(new string[]{"slobodný/slobodná", "ženatý/vydatá",
                                                                    "rozvedený/rozvedená", "vdovec/vdova"});
            this.comboBoxMaritalStatus.Location = new System.Drawing.Point(430, 252);
            this.comboBoxMaritalStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxMaritalStatus.Name = "comboBoxMaritalStatus";
            this.comboBoxMaritalStatus.Size = new System.Drawing.Size(200, 22);
            this.comboBoxMaritalStatus.TabIndex = 8;
            // 
            // labelAdress
            // 
            this.labelAdress.Location = new System.Drawing.Point(10, 284);
            this.labelAdress.Text = "Adresa trvalého pobytu";
            this.labelAdress.Size = new System.Drawing.Size(200, 20);
            // 
            // labelStreet
            // 
            this.labelStreet.Location = new System.Drawing.Point(10, 314);
            this.labelStreet.Text = "Ulica";
            this.labelStreet.Size = new System.Drawing.Size(200, 20);
            // 
            // textBoxStreet
            // 
            this.textBoxStreet.Location = new System.Drawing.Point(10, 334);
            this.textBoxStreet.Name = "textBoxStreet";
            this.textBoxStreet.Size = new System.Drawing.Size(200, 22);
            this.textBoxStreet.TabIndex = 9;
            // 
            // labelHouseNumber
            // 
            this.labelHouseNumber.Location = new System.Drawing.Point(220, 314);
            this.labelHouseNumber.Text = "Súpisné/orientačné číslo";
            this.labelHouseNumber.Size = new System.Drawing.Size(200, 20);
            // 
            // textBoxHouseNumber
            // 
            this.textBoxHouseNumber.Location = new System.Drawing.Point(220, 334);
            this.textBoxHouseNumber.Name = "textBoxHouseNumber";
            this.textBoxHouseNumber.Size = new System.Drawing.Size(200, 22);
            this.textBoxHouseNumber.TabIndex = 10;
            // 
            // labelPostalCode
            // 
            this.labelPostalCode.Location = new System.Drawing.Point(430, 314);
            this.labelPostalCode.Text = "PSČ";
            this.labelPostalCode.Size = new System.Drawing.Size(200, 20);
            // 
            // textBoxPostalCode
            // 
            this.textBoxPostalCode.Location = new System.Drawing.Point(430, 334);
            this.textBoxPostalCode.Name = "textBoxPostalCode";
            this.textBoxPostalCode.Size = new System.Drawing.Size(200, 22);
            this.textBoxPostalCode.TabIndex = 11;
            // 
            // labelCity
            // 
            this.labelCity.Location = new System.Drawing.Point(10, 366);
            this.labelCity.Text = "Obec";
            this.labelCity.Size = new System.Drawing.Size(200, 20);
            // 
            // textBoxCity
            // 
            this.textBoxCity.Location = new System.Drawing.Point(10, 386);
            this.textBoxCity.Name = "textBoxCity";
            this.textBoxCity.Size = new System.Drawing.Size(200, 22);
            this.textBoxCity.TabIndex = 12;
            // 
            // labelState
            // 
            this.labelState.Location = new System.Drawing.Point(220, 366);
            this.labelState.Text = "Štát";
            this.labelState.Size = new System.Drawing.Size(200, 20);
            // 
            // textBoxState
            // 
            this.textBoxState.Location = new System.Drawing.Point(220, 386);
            this.textBoxState.Name = "textBoxState";
            this.textBoxState.Size = new System.Drawing.Size(200, 22);
            this.textBoxState.TabIndex = 13;
            // 
            // labelTaxAplicationData
            // 
            this.labelTaxAplicationData.Location = new System.Drawing.Point(10, 428);
            this.labelTaxAplicationData.Text = "II.\tÚDAJE NA UPLATNENIE DAŇOVÉHO BONUSU PODĽA § 33 ZÁKONA";
            this.labelTaxAplicationData.Size = new System.Drawing.Size(850, 20);
            //
            // checkBox
            //
            this.checkBox.AutoSize = true;
            this.checkBox.Location = new System.Drawing.Point(10, 468);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(95, 20);
            this.checkBox.TabIndex = 14;
            this.checkBox.Text = "Uplatňujem si daňový bonus na dieťa (deti) žijúce so mnou v domácnosti";
            this.checkBox.UseVisualStyleBackColor = true;
            //this.checkBox.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // labelChildrenData
            // 
            this.labelChildrenData.Location = new System.Drawing.Point(10, 508);
            this.labelChildrenData.Text = "Údaje o vyživovaných deťoch na uplatnenie daňového bonusu podľa § 33 zákona";
            this.labelChildrenData.Size = new System.Drawing.Size(850, 20);
            // 
            // labelNumberOfChildren
            // 
            this.labelNumberOfChildren.Location = new System.Drawing.Point(10, 530);
            this.labelNumberOfChildren.Text = "Počet Detí";
            this.labelNumberOfChildren.Size = new System.Drawing.Size(100, 20);
            // 
            // numericUpDownNumberOfChildren
            // 
            this.numericUpDownNumberOfChildren.Location = new System.Drawing.Point(110, 528);
            this.numericUpDownNumberOfChildren.Name = "numericUpDownNumberOfChildren";
            this.numericUpDownNumberOfChildren.Size = new System.Drawing.Size(120, 22);
            this.numericUpDownNumberOfChildren.ReadOnly = true;
            this.numericUpDownNumberOfChildren.Minimum = 1;
            this.numericUpDownNumberOfChildren.Maximum = 10;
            this.numericUpDownNumberOfChildren.DecimalPlaces = 0;
            this.numericUpDownNumberOfChildren.TabIndex = 15;
            this.numericUpDownNumberOfChildren.ValueChanged += new System.EventHandler(this.numericUpDownNumberOfChildren_ValueChanged);
            // 
            // labelChildName1
            // 
            this.labelChildName1.Location = new System.Drawing.Point(10, 560);
            this.labelChildName1.Text = "Meno a priezvisko";
            this.labelChildName1.Size = new System.Drawing.Size(200, 20);
            // 
            // labelChildAge1
            // 
            this.labelChildAge1.Location = new System.Drawing.Point(220, 560);
            this.labelChildAge1.Text = "Vek";
            this.labelChildAge1.Size = new System.Drawing.Size(120, 20);
            // 
            // labelChildName2
            // 
            this.labelChildName2.Location = new System.Drawing.Point(280, 456);
            this.labelChildName2.Text = "Meno a priezvisko";
            this.labelChildName2.Size = new System.Drawing.Size(150, 18);
            // 
            // labelChildAge2
            // 
            this.labelChildAge2.Location = new System.Drawing.Point(433, 456);
            this.labelChildAge2.Text = "Vek";
            this.labelChildAge2.Size = new System.Drawing.Size(120, 18);
            // 
            // firstChildName
            // 
            textboxChildName.Location = new System.Drawing.Point(10, 582);
            textboxChildName.Size = new System.Drawing.Size(200, 22);
            textboxChildName.Name = "textboxChildName_1";
            textboxChildName.TabIndex = 16;
            // 
            // firstChildAge
            // 
            numericUpDownChildAge.Location = new System.Drawing.Point(220, 582);
            numericUpDownChildAge.Name = "numericUpDownChildAge_1";
            numericUpDownChildAge.Size = new System.Drawing.Size(120, 22);
            numericUpDownChildAge.Controls[0].Visible = false;
            numericUpDownChildAge.Minimum = 0;
            numericUpDownChildAge.Maximum = 26;
            numericUpDownChildAge.DecimalPlaces = 0;
            numericUpDownChildAge.TabIndex = 17;
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(10, 740);
            this.labelDate.Text = "Dňa";
            this.labelDate.Size = new System.Drawing.Size(50, 20);
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Location = new System.Drawing.Point(70, 736); ;
            this.dateTimePicker.Size = new System.Drawing.Size(250, 20);
            this.dateTimePicker.TabIndex = 18;
            
            //
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 900);
     
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.labelTitleDescription1);
            this.Controls.Add(this.labelTitleDescription2);
            this.Controls.Add(this.labelEmployeeData);
            this.Controls.Add(this.labelLastName);
            this.Controls.Add(this.textBoxLastName);
            this.Controls.Add(this.labelFirstName);
            this.Controls.Add(this.textBoxFirstName);
            this.Controls.Add(this.labelAge);
            this.Controls.Add(this.numericUpDownAge);
            this.Controls.Add(this.labelDegreeBeforeName);
            this.Controls.Add(this.textBoxDegreeBeforeName);
            this.Controls.Add(this.labelDegreeAfterName);
            this.Controls.Add(this.textBoxDegreeAfterName);
            this.Controls.Add(this.labelMaritalStatus);
            this.Controls.Add(this.comboBoxMaritalStatus);
            this.Controls.Add(this.labelAdress);
            this.Controls.Add(this.labelStreet);
            this.Controls.Add(this.textBoxStreet);
            this.Controls.Add(this.labelHouseNumber);
            this.Controls.Add(this.textBoxHouseNumber);
            this.Controls.Add(this.labelPostalCode);
            this.Controls.Add(this.textBoxPostalCode);
            this.Controls.Add(this.labelCity);
            this.Controls.Add(this.textBoxCity);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.textBoxState);
            this.Controls.Add(this.labelTaxAplicationData);
            this.Controls.Add(this.checkBox);
            this.Controls.Add(this.labelChildrenData);
            this.Controls.Add(this.labelNumberOfChildren);
            this.Controls.Add(this.numericUpDownNumberOfChildren);
            this.Controls.Add(this.labelChildName1);
            this.Controls.Add(this.labelChildAge1);
            this.Controls.Add(textboxChildName);
            this.Controls.Add(numericUpDownChildAge);
            this.Controls.Add(this.labelDate);
            this.Controls.Add(this.dateTimePicker);
            
            this.Controls.Add(this.transformBtn);
            this.Controls.Add(this.checkBtn);
            this.Controls.Add(this.saveBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAge)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button checkBtn;
        private System.Windows.Forms.Button transformBtn;
       
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelTitleDescription1;
        private System.Windows.Forms.Label labelTitleDescription2;
        private System.Windows.Forms.Label labelEmployeeData;
        private System.Windows.Forms.Label labelLastName;
        private System.Windows.Forms.TextBox textBoxLastName;
        private System.Windows.Forms.Label labelFirstName;
        private System.Windows.Forms.TextBox textBoxFirstName;
        private System.Windows.Forms.Label labelAge;
        private System.Windows.Forms.NumericUpDown numericUpDownAge;
        private System.Windows.Forms.Label labelDegreeBeforeName;
        private System.Windows.Forms.TextBox textBoxDegreeBeforeName;
        private System.Windows.Forms.Label labelDegreeAfterName;
        private System.Windows.Forms.TextBox textBoxDegreeAfterName;
        private System.Windows.Forms.Label labelMaritalStatus;
        private System.Windows.Forms.ComboBox comboBoxMaritalStatus;
        
        private System.Windows.Forms.Label labelAdress;
        private System.Windows.Forms.Label labelStreet;
        private System.Windows.Forms.TextBox textBoxStreet;
        private System.Windows.Forms.Label labelHouseNumber;
        private System.Windows.Forms.TextBox textBoxHouseNumber;
        private System.Windows.Forms.Label labelPostalCode;
        private System.Windows.Forms.TextBox textBoxPostalCode;
        private System.Windows.Forms.Label labelCity;
        private System.Windows.Forms.TextBox textBoxCity;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.TextBox textBoxState;
        
        private System.Windows.Forms.Label labelTaxAplicationData;
        private System.Windows.Forms.CheckBox checkBox;
        
        private System.Windows.Forms.Label labelChildrenData;
        private System.Windows.Forms.Label labelChildName1;
        private System.Windows.Forms.Label labelChildAge1;
        private System.Windows.Forms.Label labelChildName2;
        private System.Windows.Forms.Label labelChildAge2;
        private System.Windows.Forms.Label labelNumberOfChildren;
        private System.Windows.Forms.NumericUpDown numericUpDownNumberOfChildren;
        
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
    
    }
}

