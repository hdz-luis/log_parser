namespace OutSystems_Log_Parser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtDirectoryName = new System.Windows.Forms.TextBox();
            this.txtExtension = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.txtBoxHTTPCode = new System.Windows.Forms.TextBox();
            this.txtBoxWindowsErrorCodes = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExportTXTFile = new System.Windows.Forms.Button();
            this.txtBoxSearchValue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearchValue = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFileName
            // 
            this.txtFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileName.Location = new System.Drawing.Point(124, 12);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(288, 26);
            this.txtFileName.TabIndex = 0;
            this.txtFileName.TabStop = false;
            // 
            // btnBrowseFile
            // 
            this.btnBrowseFile.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBrowseFile.BackgroundImage")));
            this.btnBrowseFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBrowseFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseFile.Location = new System.Drawing.Point(473, 12);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(38, 26);
            this.btnBrowseFile.TabIndex = 1;
            this.btnBrowseFile.UseVisualStyleBackColor = true;
            this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
            this.btnBrowseFile.MouseEnter += new System.EventHandler(this.btnBrowseFile_MouseEnter);
            this.btnBrowseFile.MouseLeave += new System.EventHandler(this.btnBrowseFile_MouseLeave);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 44);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1171, 612);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtDirectoryName
            // 
            this.txtDirectoryName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDirectoryName.Location = new System.Drawing.Point(12, 12);
            this.txtDirectoryName.Name = "txtDirectoryName";
            this.txtDirectoryName.ReadOnly = true;
            this.txtDirectoryName.Size = new System.Drawing.Size(106, 26);
            this.txtDirectoryName.TabIndex = 3;
            this.txtDirectoryName.TabStop = false;
            // 
            // txtExtension
            // 
            this.txtExtension.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExtension.Location = new System.Drawing.Point(418, 12);
            this.txtExtension.Name = "txtExtension";
            this.txtExtension.ReadOnly = true;
            this.txtExtension.Size = new System.Drawing.Size(49, 26);
            this.txtExtension.TabIndex = 4;
            this.txtExtension.TabStop = false;
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.Silver;
            this.linkLabel1.DisabledLinkColor = System.Drawing.Color.Silver;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.LinkColor = System.Drawing.Color.Gold;
            this.linkLabel1.Location = new System.Drawing.Point(3, 10);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(98, 45);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "IIS Logs - Fields";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Gold;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.linkLabel3);
            this.panel1.Controls.Add(this.linkLabel2);
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Location = new System.Drawing.Point(1189, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(106, 609);
            this.panel1.TabIndex = 6;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.Black;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.Color.Gold;
            this.textBox2.Location = new System.Drawing.Point(8, 391);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(94, 213);
            this.textBox2.TabIndex = 9;
            this.textBox2.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Black;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Gold;
            this.textBox1.Location = new System.Drawing.Point(7, 105);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(94, 213);
            this.textBox1.TabIndex = 7;
            this.textBox1.TabStop = false;
            // 
            // linkLabel3
            // 
            this.linkLabel3.ActiveLinkColor = System.Drawing.Color.Silver;
            this.linkLabel3.DisabledLinkColor = System.Drawing.Color.Silver;
            this.linkLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel3.LinkColor = System.Drawing.Color.Gold;
            this.linkLabel3.Location = new System.Drawing.Point(4, 321);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(102, 67);
            this.linkLabel3.TabIndex = 8;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Windows System Error Codes";
            this.linkLabel3.VisitedLinkColor = System.Drawing.Color.Gold;
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.ActiveLinkColor = System.Drawing.Color.Silver;
            this.linkLabel2.DisabledLinkColor = System.Drawing.Color.Silver;
            this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel2.LinkColor = System.Drawing.Color.Gold;
            this.linkLabel2.Location = new System.Drawing.Point(3, 55);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(102, 47);
            this.linkLabel2.TabIndex = 7;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "HTTP Status Codes";
            this.linkLabel2.VisitedLinkColor = System.Drawing.Color.Gold;
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // txtBoxHTTPCode
            // 
            this.txtBoxHTTPCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxHTTPCode.Location = new System.Drawing.Point(576, 12);
            this.txtBoxHTTPCode.Name = "txtBoxHTTPCode";
            this.txtBoxHTTPCode.Size = new System.Drawing.Size(49, 26);
            this.txtBoxHTTPCode.TabIndex = 2;
            this.txtBoxHTTPCode.TextChanged += new System.EventHandler(this.txtBoxHTTPCode_TextChanged);
            // 
            // txtBoxWindowsErrorCodes
            // 
            this.txtBoxWindowsErrorCodes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxWindowsErrorCodes.Location = new System.Drawing.Point(722, 11);
            this.txtBoxWindowsErrorCodes.Name = "txtBoxWindowsErrorCodes";
            this.txtBoxWindowsErrorCodes.Size = new System.Drawing.Size(49, 26);
            this.txtBoxWindowsErrorCodes.TabIndex = 3;
            this.txtBoxWindowsErrorCodes.TextChanged += new System.EventHandler(this.txtWindowsErrorCodes_TextChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.RoyalBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(528, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 30);
            this.label1.TabIndex = 9;
            this.label1.Text = "HTTP Code:";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.RoyalBlue;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(643, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 30);
            this.label2.TabIndex = 10;
            this.label2.Text = "Windows Error Code:";
            // 
            // btnExportTXTFile
            // 
            this.btnExportTXTFile.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExportTXTFile.BackgroundImage")));
            this.btnExportTXTFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExportTXTFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportTXTFile.Location = new System.Drawing.Point(1252, 14);
            this.btnExportTXTFile.Name = "btnExportTXTFile";
            this.btnExportTXTFile.Size = new System.Drawing.Size(38, 26);
            this.btnExportTXTFile.TabIndex = 7;
            this.btnExportTXTFile.UseVisualStyleBackColor = true;
            this.btnExportTXTFile.Click += new System.EventHandler(this.btnExportTXTFile_Click);
            this.btnExportTXTFile.MouseEnter += new System.EventHandler(this.btnExportTXTFile_MouseEnter);
            this.btnExportTXTFile.MouseLeave += new System.EventHandler(this.btnExportTXTFile_MouseLeave);
            // 
            // txtBoxSearchValue
            // 
            this.txtBoxSearchValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSearchValue.Location = new System.Drawing.Point(867, 12);
            this.txtBoxSearchValue.Name = "txtBoxSearchValue";
            this.txtBoxSearchValue.Size = new System.Drawing.Size(272, 26);
            this.txtBoxSearchValue.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.RoyalBlue;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(788, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 30);
            this.label3.TabIndex = 13;
            this.label3.Text = "Search For:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSearchValue
            // 
            this.btnSearchValue.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSearchValue.BackgroundImage")));
            this.btnSearchValue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSearchValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchValue.Location = new System.Drawing.Point(1145, 14);
            this.btnSearchValue.Name = "btnSearchValue";
            this.btnSearchValue.Size = new System.Drawing.Size(38, 26);
            this.btnSearchValue.TabIndex = 6;
            this.btnSearchValue.UseVisualStyleBackColor = true;
            this.btnSearchValue.Click += new System.EventHandler(this.btnSearchValue_Click);
            this.btnSearchValue.MouseEnter += new System.EventHandler(this.btnSearchValue_MouseEnter);
            this.btnSearchValue.MouseLeave += new System.EventHandler(this.btnSearchValue_MouseLeave);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1307, 668);
            this.Controls.Add(this.btnSearchValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBoxSearchValue);
            this.Controls.Add(this.btnExportTXTFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxWindowsErrorCodes);
            this.Controls.Add(this.txtBoxHTTPCode);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtExtension);
            this.Controls.Add(this.txtDirectoryName);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnBrowseFile);
            this.Controls.Add(this.txtFileName);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1323, 707);
            this.MinimumSize = new System.Drawing.Size(1323, 707);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OutSystems Log Parser V.062521";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button btnBrowseFile;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtDirectoryName;
        private System.Windows.Forms.TextBox txtExtension;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtBoxHTTPCode;
        private System.Windows.Forms.TextBox txtBoxWindowsErrorCodes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExportTXTFile;
        private System.Windows.Forms.TextBox txtBoxSearchValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearchValue;
    }
}

