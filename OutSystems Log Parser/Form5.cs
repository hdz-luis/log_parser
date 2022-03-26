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
        string scriptPath = "";
        string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];
        string path1 = @"C:\Users\";
        string openpyxlPath = "";
        string lxmlPath = "";
        string evtxPath = "";
        string plotlyPath = "";
        string pandasPath = "";
        string pythonPath = @"\AppData\Local\Programs\Python\Python36\Scripts";
        string fullOpenpyxlPath = "";
        string fullLxmlPath = "";
        string fullEvtxPath = "";
        string fullPlotlyPath = "";
        string fullPandasPath = "";
        string fullPythonPath = "";
        string error_message = "";
        string outputTXTfile = "";
        string command = "";
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

            openpyxlPath = @"\AppData\Local\Programs\Python\Python36\Lib\site-packages\openpyxl";
            lxmlPath = @"\AppData\Local\Programs\Python\Python36\Lib\site-packages\lxml";
            evtxPath = @"\AppData\Local\Programs\Python\Python36\Lib\site-packages\Evtx";
            plotlyPath = @"\AppData\Local\Programs\Python\Python36\Lib\site-packages\plotly";
            pandasPath = @"\AppData\Local\Programs\Python\Python36\Lib\site-packages\pandas";
            fullOpenpyxlPath = path1 + userName + openpyxlPath;
            fullLxmlPath = path1 + userName + lxmlPath;
            fullEvtxPath = path1 + userName + evtxPath;
            fullPlotlyPath = path1 + userName + plotlyPath;
            fullPandasPath = path1 + userName + pandasPath;

            if (!Directory.Exists(fullOpenpyxlPath))
            {
                chkBoxOpenPyxl.Checked = false;
                chkBoxOpenPyxl.Enabled = true;
            }
            else
            {
                chkBoxOpenPyxl.Checked = false;
                chkBoxOpenPyxl.Enabled = false;
            }

            if (!Directory.Exists(fullLxmlPath))
            {
                chkBoxLxml.Checked = false;
                chkBoxLxml.Enabled = true;
            }
            else
            {
                chkBoxLxml.Checked = false;
                chkBoxLxml.Enabled = false;
            }

            if (!Directory.Exists(fullEvtxPath))
            {
                chkBoxEvtx.Checked = false;
                chkBoxEvtx.Enabled = true;
            }
            else
            {
                chkBoxEvtx.Checked = false;
                chkBoxEvtx.Enabled = false;
            }

            if (!Directory.Exists(fullPlotlyPath))
            {
                chkBoxPlotly.Checked = false;
                chkBoxPlotly.Enabled = true;
            }
            else
            {
                chkBoxPlotly.Checked = false;
                chkBoxPlotly.Enabled = false;
            }

            if (!Directory.Exists(fullPandasPath))
            {
                chkBoxPandas.Checked = false;
                chkBoxPandas.Enabled = true;
            }
            else
            {
                chkBoxPandas.Checked = false;
                chkBoxPandas.Enabled = false;
            }

            if (!Directory.Exists(fullOpenpyxlPath) || !Directory.Exists(fullLxmlPath) || !Directory.Exists(fullEvtxPath) || !Directory.Exists(fullPlotlyPath) || !Directory.Exists(fullPandasPath))
            {
                btnSubmit.Enabled = false;
                MessageBox.Show("Please install the missing libraries and restart the application", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                groupBox1.Enabled = false;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (DateTime.Compare(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date) > 0)
            {
                MessageBox.Show("The \"start date\" cannot be greater than the \"end date\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                scriptPath = currentWorkingDirectory + "\\log_parser.py";

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
                    error_message = "Error: " + Environment.NewLine + ex.ToString();
                    errorLog("\\error_log.txt", error_message);
                    MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                    throw;
                }
            }
        }
        private void errorLog(string txtFile, string err_msg)
        {
            outputTXTfile = currentWorkingDirectory + txtFile;
            using (StreamWriter logFile = new StreamWriter(outputTXTfile, true))
            {
                logFile.WriteLine(err_msg);
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

        private void commandLine(string myCommand)
        {
            try
            {
                ProcessStartInfo i = new ProcessStartInfo("cmd.exe", myCommand);
                Process p = new Process();
                p.StartInfo = i;
                p.Start();
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void chkBoxOpenPyxl_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxOpenPyxl.Checked == true)
            {
                fullPythonPath = path1 + userName + pythonPath;
                command = @"/C cd " + fullPythonPath + " & pip3 install openpyxl";
                commandLine(command);
            }
        }

        private void chkBoxLxml_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxLxml.Checked == true)
            {
                fullPythonPath = path1 + userName + pythonPath;
                command = @"/C cd " + fullPythonPath + " & pip3 install lxml";
                commandLine(command);
            }
        }

        private void chkBoxEvtx_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxEvtx.Checked == true)
            {
                fullPythonPath = path1 + userName + pythonPath;
                command = @"/C cd " + fullPythonPath + " & pip3 install python-evtx";
                commandLine(command);
            }
        }

        private void chkBoxPlotly_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxPlotly.Checked == true)
            {
                fullPythonPath = path1 + userName + pythonPath;
                command = @"/C cd " + fullPythonPath + " & pip3 install plotly";
                commandLine(command);
            }
        }

        private void chkBoxPandas_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxPandas.Checked == true)
            {
                fullPythonPath = path1 + userName + pythonPath;
                command = @"/C cd " + fullPythonPath + " & pip3 install pandas";
                commandLine(command);
            }
        }
    }
}
