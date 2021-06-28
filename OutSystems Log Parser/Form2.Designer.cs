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
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Field Name:";
            // 
            // txtBoxSearchFieldName
            // 
            this.txtBoxSearchFieldName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSearchFieldName.Location = new System.Drawing.Point(101, 6);
            this.txtBoxSearchFieldName.Name = "txtBoxSearchFieldName";
            this.txtBoxSearchFieldName.Size = new System.Drawing.Size(258, 23);
            this.txtBoxSearchFieldName.TabIndex = 1;
            this.txtBoxSearchFieldName.TextChanged += new System.EventHandler(this.txtBoxSearchFieldName_TextChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.RoyalBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(351, 48);
            this.label1.TabIndex = 14;
            this.label1.Text = "Highlight the row to display the information in the bottom section:";
            // 
            // listViewFields
            // 
            this.listViewFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewFields.FullRowSelect = true;
            this.listViewFields.Location = new System.Drawing.Point(12, 87);
            this.listViewFields.MultiSelect = false;
            this.listViewFields.Name = "listViewFields";
            this.listViewFields.Size = new System.Drawing.Size(351, 123);
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
            this.label3.Location = new System.Drawing.Point(12, 228);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 17);
            this.label3.TabIndex = 15;
            this.label3.Text = "Field Name:";
            // 
            // txtBoxFieldName
            // 
            this.txtBoxFieldName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxFieldName.Location = new System.Drawing.Point(120, 225);
            this.txtBoxFieldName.Name = "txtBoxFieldName";
            this.txtBoxFieldName.ReadOnly = true;
            this.txtBoxFieldName.Size = new System.Drawing.Size(239, 23);
            this.txtBoxFieldName.TabIndex = 16;
            this.txtBoxFieldName.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.RoyalBlue;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(12, 257);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 17);
            this.label4.TabIndex = 17;
            this.label4.Text = "IIS Field Name:";
            // 
            // txtBoxIISFieldName
            // 
            this.txtBoxIISFieldName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxIISFieldName.Location = new System.Drawing.Point(120, 254);
            this.txtBoxIISFieldName.Name = "txtBoxIISFieldName";
            this.txtBoxIISFieldName.ReadOnly = true;
            this.txtBoxIISFieldName.Size = new System.Drawing.Size(239, 23);
            this.txtBoxIISFieldName.TabIndex = 18;
            this.txtBoxIISFieldName.TabStop = false;
            // 
            // txtBoxDescription
            // 
            this.txtBoxDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDescription.Location = new System.Drawing.Point(120, 283);
            this.txtBoxDescription.Multiline = true;
            this.txtBoxDescription.Name = "txtBoxDescription";
            this.txtBoxDescription.ReadOnly = true;
            this.txtBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxDescription.Size = new System.Drawing.Size(239, 113);
            this.txtBoxDescription.TabIndex = 19;
            this.txtBoxDescription.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.RoyalBlue;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(12, 286);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 17);
            this.label5.TabIndex = 20;
            this.label5.Text = "Description:";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(371, 407);
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
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(387, 446);
            this.MinimumSize = new System.Drawing.Size(387, 446);
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