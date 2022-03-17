using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OutSystems_Log_Parser
{
    public partial class Form5 : Form
    {
        string currentWorkingDirectory = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

        public Form5()
        {
            InitializeComponent();
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtBoxSearchDirectory.Text = folderBrowserDialog1.SelectedPath;
                btnSubmit.Enabled = true;
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            btnSubmit.Enabled = false;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (DateTime.Compare(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date) > 0)
            {
                MessageBox.Show("The \"start date\" cannot be greater than the \"end date\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string scriptPath = currentWorkingDirectory + "\\log_parser.py";

                try
                {
                    Process runPythonscript = new Process();
                    runPythonscript.StartInfo.FileName = scriptPath;

                    if (radioButton1.Checked == true)
                    {
                        runPythonscript.StartInfo.Arguments = txtBoxSearchDirectory.Text + " " + dateTimePicker1.Text + " " + dateTimePicker2.Text + " y";
                    }
                    else if (radioButton2.Checked == true)
                    {
                        runPythonscript.StartInfo.Arguments = txtBoxSearchDirectory.Text + " " + dateTimePicker1.Text + " " + dateTimePicker2.Text + " n";
                    }

                    runPythonscript.Start();

                    Form1 f1 = new Form1(currentWorkingDirectory);
                    f1.Show();

                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                    throw;
                }
            }
        }

        private void btnBrowseFolder_MouseEnter(object sender, EventArgs e)
        {
            btnBrowseFolder.BackColor = Color.SpringGreen;
        }

        private void btnBrowseFolder_MouseLeave(object sender, EventArgs e)
        {
            btnBrowseFolder.BackColor = SystemColors.ControlLight;
        }

        private void btnSubmit_MouseEnter(object sender, EventArgs e)
        {
            btnSubmit.BackColor = Color.SpringGreen;
        }

        private void btnSubmit_MouseLeave(object sender, EventArgs e)
        {
            btnSubmit.BackColor = SystemColors.ControlLight;
        }
    }
}
