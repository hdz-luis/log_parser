namespace OutSystems_Log_Parser
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.label2 = new System.Windows.Forms.Label();
            this.txtBoxSearchFieldName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listViewFields = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBoxFieldName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBoxIISFieldName = new System.Windows.Forms.TextBox();
            this.txtBoxDescription = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.RoyalBlue;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(15, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Field Name:";
            // 
            // txtBoxSearchFieldName
            // 
            this.txtBoxSearchFieldName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSearchFieldName.Location = new System.Drawing.Point(126, 8);
            this.txtBoxSearchFieldName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBoxSearchFieldName.Name = "txtBoxSearchFieldName";
            this.txtBoxSearchFieldName.Size = new System.Drawing.Size(322, 26);
            this.txtBoxSearchFieldName.TabIndex = 1;
            this.txtBoxSearchFieldName.TextChanged += new System.EventHandler(this.txtBoxSearchFieldName_TextChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.RoyalBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(15, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(439, 60);
            this.label1.TabIndex = 14;
            this.label1.Text = "Click on a row to display the information in the bottom section:";
            // 
            // listViewFields
            // 
            this.listViewFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewFields.FullRowSelect = true;
            this.listViewFields.HideSelection = false;
            this.listViewFields.Location = new System.Drawing.Point(15, 109);
            this.listViewFields.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listViewFields.MultiSelect = false;
            this.listViewFields.Name = "listViewFields";
            this.listViewFields.Size = new System.Drawing.Size(438, 153);
            this.listViewFields.TabIndex = 2;
            this.listViewFields.UseCompatibleStateImageBehavior = false;
            this.listViewFields.Click += new System.EventHandler(this.listViewFields_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.RoyalBlue;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(15, 285);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 20);
            this.label3.TabIndex = 15;
            this.label3.Text = "Field Name:";
            // 
            // txtBoxFieldName
            // 
            this.txtBoxFieldName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxFieldName.Location = new System.Drawing.Point(150, 281);
            this.txtBoxFieldName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBoxFieldName.Name = "txtBoxFieldName";
            this.txtBoxFieldName.ReadOnly = true;
            this.txtBoxFieldName.Size = new System.Drawing.Size(298, 26);
            this.txtBoxFieldName.TabIndex = 16;
            this.txtBoxFieldName.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.RoyalBlue;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(15, 321);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 20);
            this.label4.TabIndex = 17;
            this.label4.Text = "IIS Field Name:";
            // 
            // txtBoxIISFieldName
            // 
            this.txtBoxIISFieldName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxIISFieldName.Location = new System.Drawing.Point(150, 318);
            this.txtBoxIISFieldName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBoxIISFieldName.Name = "txtBoxIISFieldName";
            this.txtBoxIISFieldName.ReadOnly = true;
            this.txtBoxIISFieldName.Size = new System.Drawing.Size(298, 26);
            this.txtBoxIISFieldName.TabIndex = 18;
            this.txtBoxIISFieldName.TabStop = false;
            // 
            // txtBoxDescription
            // 
            this.txtBoxDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDescription.Location = new System.Drawing.Point(150, 354);
            this.txtBoxDescription.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBoxDescription.Multiline = true;
            this.txtBoxDescription.Name = "txtBoxDescription";
            this.txtBoxDescription.ReadOnly = true;
            this.txtBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxDescription.Size = new System.Drawing.Size(298, 140);
            this.txtBoxDescription.TabIndex = 19;
            this.txtBoxDescription.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.RoyalBlue;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(15, 358);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 20);
            this.label5.TabIndex = 20;
            this.label5.Text = "Description:";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(461, 499);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBoxDescription);
            this.Controls.Add(this.txtBoxIISFieldName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtBoxFieldName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listViewFields);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxSearchFieldName);
            this.Controls.Add(this.label2);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(479, 546);
            this.MinimumSize = new System.Drawing.Size(479, 546);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IIS Log Fields";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBoxSearchFieldName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listViewFields;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBoxFieldName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBoxIISFieldName;
        private System.Windows.Forms.TextBox txtBoxDescription;
        private System.Windows.Forms.Label label5;
    }
}