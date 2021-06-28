namespace OutSystems_Log_Parser
{
    partial class Form4
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4));
            this.label5 = new System.Windows.Forms.Label();
            this.txtBoxDescription = new System.Windows.Forms.TextBox();
            this.txtBoxMeaning = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBoxWinSysErrorCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.listViewCodes = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxSearchWinSysErrorCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.RoyalBlue;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(10, 341);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 17);
            this.label5.TabIndex = 42;
            this.label5.Text = "Description:";
            // 
            // txtBoxDescription
            // 
            this.txtBoxDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxDescription.Location = new System.Drawing.Point(135, 338);
            this.txtBoxDescription.Multiline = true;
            this.txtBoxDescription.Name = "txtBoxDescription";
            this.txtBoxDescription.ReadOnly = true;
            this.txtBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxDescription.Size = new System.Drawing.Size(226, 113);
            this.txtBoxDescription.TabIndex = 41;
            this.txtBoxDescription.TabStop = false;
            // 
            // txtBoxMeaning
            // 
            this.txtBoxMeaning.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxMeaning.Location = new System.Drawing.Point(135, 290);
            this.txtBoxMeaning.Multiline = true;
            this.txtBoxMeaning.Name = "txtBoxMeaning";
            this.txtBoxMeaning.ReadOnly = true;
            this.txtBoxMeaning.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBoxMeaning.Size = new System.Drawing.Size(226, 42);
            this.txtBoxMeaning.TabIndex = 40;
            this.txtBoxMeaning.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.RoyalBlue;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(10, 293);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 17);
            this.label4.TabIndex = 39;
            this.label4.Text = "Meaning:";
            // 
            // txtBoxWinSysErrorCode
            // 
            this.txtBoxWinSysErrorCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxWinSysErrorCode.Location = new System.Drawing.Point(135, 251);
            this.txtBoxWinSysErrorCode.Name = "txtBoxWinSysErrorCode";
            this.txtBoxWinSysErrorCode.ReadOnly = true;
            this.txtBoxWinSysErrorCode.Size = new System.Drawing.Size(226, 23);
            this.txtBoxWinSysErrorCode.TabIndex = 38;
            this.txtBoxWinSysErrorCode.TabStop = false;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.RoyalBlue;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(9, 242);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 44);
            this.label3.TabIndex = 37;
            this.label3.Text = "Windows System Error Code:";
            // 
            // listViewCodes
            // 
            this.listViewCodes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewCodes.FullRowSelect = true;
            this.listViewCodes.Location = new System.Drawing.Point(12, 106);
            this.listViewCodes.MultiSelect = false;
            this.listViewCodes.Name = "listViewCodes";
            this.listViewCodes.Size = new System.Drawing.Size(349, 123);
            this.listViewCodes.TabIndex = 3;
            this.listViewCodes.UseCompatibleStateImageBehavior = false;
            this.listViewCodes.Click += new System.EventHandler(this.listViewCodes_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.RoyalBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(10, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(351, 41);
            this.label1.TabIndex = 36;
            this.label1.Text = "Highlight the row to display the information in the bottom section:";
            // 
            // txtBoxSearchWinSysErrorCode
            // 
            this.txtBoxSearchWinSysErrorCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSearchWinSysErrorCode.Location = new System.Drawing.Point(135, 20);
            this.txtBoxSearchWinSysErrorCode.Name = "txtBoxSearchWinSysErrorCode";
            this.txtBoxSearchWinSysErrorCode.Size = new System.Drawing.Size(226, 23);
            this.txtBoxSearchWinSysErrorCode.TabIndex = 1;
            this.txtBoxSearchWinSysErrorCode.TextChanged += new System.EventHandler(this.txtBoxSearchWinSysErrorCode_TextChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.RoyalBlue;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(10, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 43);
            this.label2.TabIndex = 35;
            this.label2.Text = "Windows System Error Code:";
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(371, 463);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBoxDescription);
            this.Controls.Add(this.txtBoxMeaning);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtBoxWinSysErrorCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listViewCodes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxSearchWinSysErrorCode);
            this.Controls.Add(this.label2);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(387, 502);
            this.MinimumSize = new System.Drawing.Size(387, 502);
            this.Name = "Form4";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Windows System Error Codes";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBoxDescription;
        private System.Windows.Forms.TextBox txtBoxMeaning;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBoxWinSysErrorCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView listViewCodes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxSearchWinSysErrorCode;
        private System.Windows.Forms.Label label2;
    }
}