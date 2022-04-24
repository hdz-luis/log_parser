namespace OutSystems_Log_Parser
{
    partial class Form5
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form5));
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBoxSearchDirectory = new System.Windows.Forms.TextBox();
            this.btnBrowseFolder = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkBoxPandas = new System.Windows.Forms.CheckBox();
            this.chkBoxPlotly = new System.Windows.Forms.CheckBox();
            this.chkBoxEvtx = new System.Windows.Forms.CheckBox();
            this.chkBoxLxml = new System.Windows.Forms.CheckBox();
            this.chkBoxOpenPyxl = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.RoyalBlue;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(87, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 25);
            this.label2.TabIndex = 19;
            this.label2.Text = "To Date:";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker2.Checked = false;
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(181, 82);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(160, 30);
            this.dateTimePicker2.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.RoyalBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(66, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 25);
            this.label1.TabIndex = 18;
            this.label1.Text = "From Date:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Checked = false;
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(181, 46);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(160, 30);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.RoyalBlue;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(163, 25);
            this.label6.TabIndex = 29;
            this.label6.Text = "Search Directory:";
            // 
            // txtBoxSearchDirectory
            // 
            this.txtBoxSearchDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxSearchDirectory.Location = new System.Drawing.Point(181, 10);
            this.txtBoxSearchDirectory.Name = "txtBoxSearchDirectory";
            this.txtBoxSearchDirectory.ReadOnly = true;
            this.txtBoxSearchDirectory.Size = new System.Drawing.Size(471, 30);
            this.txtBoxSearchDirectory.TabIndex = 1;
            // 
            // btnBrowseFolder
            // 
            this.btnBrowseFolder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBrowseFolder.BackgroundImage")));
            this.btnBrowseFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBrowseFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseFolder.Location = new System.Drawing.Point(659, 9);
            this.btnBrowseFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseFolder.Name = "btnBrowseFolder";
            this.btnBrowseFolder.Size = new System.Drawing.Size(64, 32);
            this.btnBrowseFolder.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnBrowseFolder, "Browse for the directory where the logs are stored.");
            this.btnBrowseFolder.UseVisualStyleBackColor = true;
            this.btnBrowseFolder.Click += new System.EventHandler(this.btnBrowseFolder_Click);
            this.btnBrowseFolder.MouseEnter += new System.EventHandler(this.btnBrowseFolder_MouseEnter);
            this.btnBrowseFolder.MouseLeave += new System.EventHandler(this.btnBrowseFolder_MouseLeave);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.BackColor = System.Drawing.Color.RoyalBlue;
            this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.Location = new System.Drawing.Point(181, 120);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(67, 29);
            this.radioButton1.TabIndex = 5;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Yes";
            this.radioButton1.UseVisualStyleBackColor = false;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.BackColor = System.Drawing.Color.RoyalBlue;
            this.radioButton2.Checked = true;
            this.radioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(283, 120);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(58, 29);
            this.radioButton2.TabIndex = 6;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "No";
            this.radioButton2.UseVisualStyleBackColor = false;
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSubmit.BackgroundImage")));
            this.btnSubmit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.Location = new System.Drawing.Point(181, 173);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(64, 32);
            this.btnSubmit.TabIndex = 7;
            this.toolTip1.SetToolTip(this.btnSubmit, "Submit the arguments to the Python script.");
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            this.btnSubmit.MouseEnter += new System.EventHandler(this.btnSubmit_MouseEnter);
            this.btnSubmit.MouseLeave += new System.EventHandler(this.btnSubmit_MouseLeave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.RoyalBlue;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(93, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 25);
            this.label3.TabIndex = 30;
            this.label3.Text = "Graphs:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.RoyalBlue;
            this.groupBox1.Controls.Add(this.chkBoxPandas);
            this.groupBox1.Controls.Add(this.chkBoxPlotly);
            this.groupBox1.Controls.Add(this.chkBoxEvtx);
            this.groupBox1.Controls.Add(this.chkBoxLxml);
            this.groupBox1.Controls.Add(this.chkBoxOpenPyxl);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(755, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(209, 175);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Libraries:";
            // 
            // chkBoxPandas
            // 
            this.chkBoxPandas.AutoSize = true;
            this.chkBoxPandas.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxPandas.Location = new System.Drawing.Point(101, 64);
            this.chkBoxPandas.Name = "chkBoxPandas";
            this.chkBoxPandas.Size = new System.Drawing.Size(99, 29);
            this.chkBoxPandas.TabIndex = 13;
            this.chkBoxPandas.Text = "pandas";
            this.chkBoxPandas.UseVisualStyleBackColor = true;
            this.chkBoxPandas.CheckedChanged += new System.EventHandler(this.chkBoxPandas_CheckedChanged);
            // 
            // chkBoxPlotly
            // 
            this.chkBoxPlotly.AutoSize = true;
            this.chkBoxPlotly.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxPlotly.Location = new System.Drawing.Point(16, 134);
            this.chkBoxPlotly.Name = "chkBoxPlotly";
            this.chkBoxPlotly.Size = new System.Drawing.Size(79, 29);
            this.chkBoxPlotly.TabIndex = 12;
            this.chkBoxPlotly.Text = "plotly";
            this.chkBoxPlotly.UseVisualStyleBackColor = true;
            this.chkBoxPlotly.CheckedChanged += new System.EventHandler(this.chkBoxPlotly_CheckedChanged);
            // 
            // chkBoxEvtx
            // 
            this.chkBoxEvtx.AutoSize = true;
            this.chkBoxEvtx.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxEvtx.Location = new System.Drawing.Point(16, 99);
            this.chkBoxEvtx.Name = "chkBoxEvtx";
            this.chkBoxEvtx.Size = new System.Drawing.Size(70, 29);
            this.chkBoxEvtx.TabIndex = 11;
            this.chkBoxEvtx.Text = "evtx";
            this.chkBoxEvtx.UseVisualStyleBackColor = true;
            this.chkBoxEvtx.CheckedChanged += new System.EventHandler(this.chkBoxEvtx_CheckedChanged);
            // 
            // chkBoxLxml
            // 
            this.chkBoxLxml.AutoSize = true;
            this.chkBoxLxml.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxLxml.Location = new System.Drawing.Point(16, 64);
            this.chkBoxLxml.Name = "chkBoxLxml";
            this.chkBoxLxml.Size = new System.Drawing.Size(68, 29);
            this.chkBoxLxml.TabIndex = 10;
            this.chkBoxLxml.Text = "lxml";
            this.chkBoxLxml.UseVisualStyleBackColor = true;
            this.chkBoxLxml.CheckedChanged += new System.EventHandler(this.chkBoxLxml_CheckedChanged);
            // 
            // chkBoxOpenPyxl
            // 
            this.chkBoxOpenPyxl.AutoSize = true;
            this.chkBoxOpenPyxl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxOpenPyxl.Location = new System.Drawing.Point(16, 29);
            this.chkBoxOpenPyxl.Name = "chkBoxOpenPyxl";
            this.chkBoxOpenPyxl.Size = new System.Drawing.Size(113, 29);
            this.chkBoxOpenPyxl.TabIndex = 9;
            this.chkBoxOpenPyxl.Text = "openpyxl";
            this.chkBoxOpenPyxl.UseVisualStyleBackColor = true;
            this.chkBoxOpenPyxl.CheckedChanged += new System.EventHandler(this.chkBoxOpenPyxl_CheckedChanged);
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(978, 219);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.btnBrowseFolder);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtBoxSearchDirectory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(996, 266);
            this.MinimumSize = new System.Drawing.Size(996, 266);
            this.Name = "Form5";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OutSystems Log Parser V.042322";
            this.Load += new System.EventHandler(this.Form5_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBoxSearchDirectory;
        private System.Windows.Forms.Button btnBrowseFolder;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkBoxOpenPyxl;
        private System.Windows.Forms.CheckBox chkBoxLxml;
        private System.Windows.Forms.CheckBox chkBoxEvtx;
        private System.Windows.Forms.CheckBox chkBoxPlotly;
        private System.Windows.Forms.CheckBox chkBoxPandas;
    }
}