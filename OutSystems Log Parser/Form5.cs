using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OutSystems_Log_Parser
{
    public partial class Form5 : Form
    {
        string pythonPath = "";
        string pythonPath2 = "";
        string pythonVersion = "";
        string displayName = "";
        string displayVersion = "";
        string dplv = "";
        string regFilePath = "";
        string scriptPath = "";
        string openpyxlPath = "";
        string lxmlPath = "";
        string evtxPath = "";
        string plotlyPath = "";
        string pandasPath = "";
        string py7zrPath = "";
        string patoolPath = "";
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
            var tpl = checkInstalled(@"SOFTWARE\Python\PythonCore\3.6", @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");

            pythonPath = tpl.Item1;
            pythonVersion = tpl.Item2;
            pythonPath2 = pythonPath.Substring(0, pythonPath.LastIndexOf('\\') + 1);

            if (string.IsNullOrEmpty(pythonPath.Trim()) && pythonVersion != "3.6.2")
            {
                txtBoxSearchDirectory.Enabled = false;
                btnBrowseFolder.Enabled = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                btnSubmit.Enabled = false;
                groupBox1.Enabled = false;

                MessageBox.Show("Please install Python version 3.6.2 and restart the application", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
                btnSubmit.Enabled = false;

                openpyxlPath = pythonPath2 + @"Lib\site-packages\openpyxl";
                lxmlPath = pythonPath2 + @"Lib\site-packages\lxml";
                evtxPath = pythonPath2 + @"Lib\site-packages\Evtx";
                plotlyPath = pythonPath2 + @"Lib\site-packages\plotly";
                pandasPath = pythonPath2 + @"Lib\site-packages\pandas";
                py7zrPath = pythonPath2 + @"Lib\site-packages\py7zr";
                patoolPath = pythonPath2 + @"Lib\site-packages\patoolib";

                if (!Directory.Exists(openpyxlPath))
                {
                    chkBoxOpenPyxl.Checked = false;
                    chkBoxOpenPyxl.Enabled = true;
                }
                else
                {
                    chkBoxOpenPyxl.Checked = false;
                    chkBoxOpenPyxl.Enabled = false;
                }

                if (!Directory.Exists(lxmlPath))
                {
                    chkBoxLxml.Checked = false;
                    chkBoxLxml.Enabled = true;
                }
                else
                {
                    chkBoxLxml.Checked = false;
                    chkBoxLxml.Enabled = false;
                }

                if (!Directory.Exists(evtxPath))
                {
                    chkBoxEvtx.Checked = false;
                    chkBoxEvtx.Enabled = true;
                }
                else
                {
                    chkBoxEvtx.Checked = false;
                    chkBoxEvtx.Enabled = false;
                }

                if (!Directory.Exists(plotlyPath))
                {
                    chkBoxPlotly.Checked = false;
                    chkBoxPlotly.Enabled = true;
                }
                else
                {
                    chkBoxPlotly.Checked = false;
                    chkBoxPlotly.Enabled = false;
                }

                if (!Directory.Exists(pandasPath))
                {
                    chkBoxPandas.Checked = false;
                    chkBoxPandas.Enabled = true;
                }
                else
                {
                    chkBoxPandas.Checked = false;
                    chkBoxPandas.Enabled = false;
                }

                if (!Directory.Exists(py7zrPath))
                {
                    chkBoxPy7zr.Checked = false;
                    chkBoxPy7zr.Enabled = true;
                }
                else
                {
                    chkBoxPy7zr.Checked = false;
                    chkBoxPy7zr.Enabled = false;
                }

                if (!Directory.Exists(patoolPath))
                {
                    chkBoxPatool.Checked = false;
                    chkBoxPatool.Enabled = true;
                }
                else
                {
                    chkBoxPatool.Checked = false;
                    chkBoxPatool.Enabled = false;
                }

                if (!Directory.Exists(openpyxlPath) || !Directory.Exists(lxmlPath) || !Directory.Exists(evtxPath) || !Directory.Exists(plotlyPath) || !Directory.Exists(pandasPath) || !Directory.Exists(py7zrPath) || !Directory.Exists(patoolPath))
                {
                    txtBoxSearchDirectory.Enabled = false;
                    btnBrowseFolder.Enabled = false;
                    dateTimePicker1.Enabled = false;
                    dateTimePicker2.Enabled = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    btnSubmit.Enabled = false;
                    MessageBox.Show("Please install the missing libraries and restart the application", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    groupBox1.Enabled = false;
                }
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
                command = @"/K cd " + pythonPath2 + "Scripts & pip3 install openpyxl";
                commandLine(command);
            }
        }

        private void chkBoxLxml_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxLxml.Checked == true)
            {
                command = @"/K cd " + pythonPath2 + "Scripts & pip3 install lxml";
                commandLine(command);
            }
        }

        private void chkBoxEvtx_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxEvtx.Checked == true)
            {
                command = @"/K cd " + pythonPath2 + "Scripts & pip3 install python-evtx";
                commandLine(command);
            }
        }

        private void chkBoxPlotly_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxPlotly.Checked == true)
            {
                command = @"/K cd " + pythonPath2 + "Scripts & pip3 install plotly";
                commandLine(command);
            }
        }

        private void chkBoxPandas_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxPandas.Checked == true)
            {
                command = @"/K cd " + pythonPath2 + "Scripts & pip3 install pandas";
                commandLine(command);
            }
        }

        public Tuple<string, string> checkInstalled(string registryKey, string registryKey2)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser;
                RegistryKey thevalue = key.OpenSubKey(registryKey);
                regFilePath = thevalue.OpenSubKey("InstallPath").GetValue("ExecutablePath").ToString();

                key.Close();

                RegistryKey key64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey key2 = key64.OpenSubKey(registryKey2);

                if (key2 != null)
                {
                    foreach (RegistryKey subkey in key2.GetSubKeyNames().Select(keyName => key2.OpenSubKey(keyName)))
                    {
                        displayName = subkey.GetValue("DisplayName") as string;
                        if (displayName != null)
                        {
                            if (displayName.Contains("Python") && displayName.Contains("Executables (64-bit)"))
                            {
                                displayVersion = subkey.GetValue("DisplayVersion").ToString();
                                dplv = displayVersion.Substring(0, 5);

                                return Tuple.Create(regFilePath, dplv);
                            }
                        }
                    }
                    key2.Close();
                }
                return Tuple.Create(regFilePath, dplv);
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void chkBoxPy7zr_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxPy7zr.Checked == true)
            {
                command = @"/C cd " + pythonPath2 + "Scripts & pip3 install setuptools --upgrade & pip3 install pip --upgrade & pip3 install py7zr";
                commandLine(command);
            }
        }

        private void chkBoxPatool_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxPatool.Checked == true)
            {
                command = @"/C cd " + pythonPath2 + "Scripts & pip3 install patool";
                commandLine(command);
            }
        }
    }
}
