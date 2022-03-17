namespace OutSystems_Log_Parser
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.label5 = new System.Windows.Forms.Label();
            this.txtBoxDescription = new System.Windows.Forms.TextBox();
            this.txtBoxMeaning = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBoxHTTPcode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.listViewCodes = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxSearchHTTPcode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.RoyalBlue;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(11, 366);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 20);
            this.label5.TabIndex = 31;
            this.label5.Text = "Description:";
            // 
            // txtBoxDescription
            // 
            this.txtBoxDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDescription.Location = new System.Drawing.Point(126, 362);
            this.txtBoxDescription.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBoxDescription.Multiline = true;
            this.txtBoxDescription.Name = "txtBoxDescription";
            this.txtBoxDescription.ReadOnly = true;
            this.txtBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxDescription.Size = new System.Drawing.Size(322, 140);
            this.txtBoxDescription.TabIndex = 30;
            this.txtBoxDescription.TabStop = false;
            // 
            // txtBoxMeaning
            // 
            this.txtBoxMeaning.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxMeaning.Location = new System.Drawing.Point(126, 322);
            this.txtBoxMeaning.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBoxMeaning.Name = "txtBoxMeaning";
            this.txtBoxMeaning.ReadOnly = true;
            this.txtBoxMeaning.Size = new System.Drawing.Size(322, 26);
            this.txtBoxMeaning.TabIndex = 29;
            this.txtBoxMeaning.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.RoyalBlue;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(11, 326);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 20);
            this.label4.TabIndex = 28;
            this.label4.Text = "Meaning:";
            // 
            // txtBoxHTTPcode
            // 
            this.txtBoxHTTPcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxHTTPcode.Location = new System.Drawing.Point(126, 282);
            this.txtBoxHTTPcode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBoxHTTPcode.Name = "txtBoxHTTPcode";
            this.txtBoxHTTPcode.ReadOnly = true;
            this.txtBoxHTTPcode.Size = new System.Drawing.Size(322, 26);
            this.txtBoxHTTPcode.TabIndex = 27;
            this.txtBoxHTTPcode.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.RoyalBlue;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(11, 286);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 20);
            this.label3.TabIndex = 26;
            this.label3.Text = "HTTP Code:";
            // 
            // listViewCodes
            // 
            this.listViewCodes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewCodes.FullRowSelect = true;
            this.listViewCodes.HideSelection = false;
            this.listViewCodes.Location = new System.Drawing.Point(15, 110);
            this.listViewCodes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listViewCodes.MultiSelect = false;
            this.listViewCodes.Name = "listViewCodes";
            this.listViewCodes.Size = new System.Drawing.Size(438, 153);
            this.listViewCodes.TabIndex = 3;
            this.listViewCodes.UseCompatibleStateImageBehavior = false;
            this.listViewCodes.Click += new System.EventHandler(this.listViewCodes_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.RoyalBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(15, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(439, 60);
            this.label1.TabIndex = 25;
            this.label1.Text = "Click on a row to display the information in the bottom section:";
            // 
            // txtBoxSearchHTTPcode
            // 
            this.txtBoxSearchHTTPcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSearchHTTPcode.Location = new System.Drawing.Point(126, 9);
            this.txtBoxSearchHTTPcode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBoxSearchHTTPcode.Name = "txtBoxSearchHTTPcode";
            this.txtBoxSearchHTTPcode.Size = new System.Drawing.Size(322, 26);
            this.txtBoxSearchHTTPcode.TabIndex = 1;
            this.txtBoxSearchHTTPcode.TextChanged += new System.EventHandler(this.txtBoxSearchHTTPcode_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.RoyalBlue;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(11, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 20);
            this.label2.TabIndex = 24;
            this.label2.Text = "HTTP Code:";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(461, 504);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBoxDescription);
            this.Controls.Add(this.txtBoxMeaning);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtBoxHTTPcode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listViewCodes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxSearchHTTPcode);
            this.Controls.Add(this.label2);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(479, 551);
            this.MinimumSize = new System.Drawing.Size(479, 551);
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HTTP Status Codes";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBoxDescription;
        private System.Windows.Forms.TextBox txtBoxMeaning;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBoxHTTPcode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView listViewCodes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxSearchHTTPcode;
        private System.Windows.Forms.Label label2;
    }
}