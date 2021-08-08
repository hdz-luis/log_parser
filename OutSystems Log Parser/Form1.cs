using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OutSystems_Log_Parser
{
    public partial class Form1 : Form
    {
        string fullPath = "";
        string relativePath = "";
        string[] filesPaths;
        string fileName = "";
        char delimiters = '|';
        
        public Form1()
        {
            InitializeComponent();
        }

        //Beginning of the Service Center Logs tab code
        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            try
            {
                //delete values at the time of browsing for another file
                clearTextboxes(txtBoxDetailErrorLogs);
                clearTextboxes(txtBoxDetailGenerallogs);
                clearTextboxes(txtBoxDetailIntegrationlogs);
                clearTextboxes(txtBoxDetailScreenRequestslogs);
                clearTextboxes(txtBoxDetailTimerlogs);
                clearTextboxes(txtBoxDetailEmaillogs);
                clearTextboxes(txtBoxDetailExtensionlogs);
                clearTextboxes(txtBoxDetailServiceActionlogs);
                clearTextboxes(txtBoxDetailTradWebRequests);
                clearTextboxes(txtBoxDetailWinAppEventViewer);
                clearTextboxes(txtBoxDetailWinSysEventViewer);
                clearTextboxes(txtBoxDetailWinSecEventViewer);
                clearTextboxes(txtBoxDetailAndroidLogs);
                clearTextboxes(txtBoxDetailiOSLogs);
                clearTextboxes(txtDetailIISlogs);
                clearTextboxes(txtBoxDetailServiceStudioLogs);

                clearTables(dataGridViewErrorlogs);
                clearTables(dataGridViewGenerallogs);
                clearTables(dataGridViewIntegrationslogs);
                clearTables(dataGridViewScreenRequestslogs);
                clearTables(dataGridViewTimerlogs);
                clearTables(dataGridViewEmaillogs);
                clearTables(dataGridViewExtensionlogs);
                clearTables(dataGridViewServiceActionlogs);
                clearTables(dataGridViewTradWebRequests);
                clearTables(dataGridViewIISDateTime);
                clearTables(dataGridViewIISTimeTaken);
                clearTables(dataGridViewWinAppEventViewer);
                clearTables(dataGridViewWinSysEventViewer);
                clearTables(dataGridViewWinSecEventViewer);
                clearTables(dataGridViewAndroidlogs);
                clearTables(dataGridViewiOSlogs);
                clearTables(dataGridViewServiceStudiologs);

                //filter the file extensions and get sections from the absolute path
                openFileDialog1.Filter = "TXT Files (*.txt)|*.txt";
                openFileDialog1.Title = "Please select a file to analyze";
                openFileDialog1.FileName = null;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fullPath = openFileDialog1.FileName;
                    relativePath = Path.GetDirectoryName(fullPath);

                    filesPaths = Directory.GetFiles(relativePath, "*.txt", SearchOption.TopDirectoryOnly);

                    foreach (string filePaths in filesPaths)
                    {
                        if (File.Exists(filePaths))
                        {
                            fileName = Path.GetFileName(filePaths);

                            if (fileName == "android_build_logs.txt")
                            {
                                androidLogs(relativePath + "\\android_build_logs.txt", delimiters);
                            }
                            else if (fileName == "email_logs.txt")
                            {
                                emailLogs(relativePath + "\\email_logs.txt", delimiters);
                            }
                            else if (fileName == "error_logs.txt")
                            {
                                errorLogs(relativePath + "\\error_logs.txt", delimiters);
                            }
                            else if (fileName == "extension_logs.txt")
                            {
                                extensionLogs(relativePath + "\\extension_logs.txt", delimiters);
                            }
                            else if (fileName == "general_logs.txt")
                            {
                                generalLogs(relativePath + "\\general_logs.txt", delimiters);
                            }
                            else if (fileName == "iis_logs.txt")
                            {
                                iisLogs(relativePath + "\\iis_logs.txt", delimiters);
                            }
                            else if (fileName == "iis_logs_timetaken.txt")
                            {
                                iisLogsTime(relativePath + "\\iis_logs_timetaken.txt", delimiters);
                            }
                            else if (fileName == "integrations_logs.txt")
                            {
                                integrationsLogs(relativePath + "\\integrations_logs.txt", delimiters);
                            }
                            else if (fileName == "iOS_build_logs.txt")
                            {
                                iOSLogs(relativePath + "\\iOS_build_logs.txt", delimiters);
                            }
                            else if (fileName == "mobile_requests_logs.txt")
                            {
                                mobileRequestsLogs(relativePath + "\\mobile_requests_logs.txt", delimiters);
                            }
                            else if (fileName == "screen_logs.txt")
                            {
                                screenLogs(relativePath + "\\screen_logs.txt", delimiters);
                            }
                            else if (fileName == "service_action_logs.txt")
                            {
                                serviceActionLogs(relativePath + "\\service_action_logs.txt", delimiters);
                            }
                            else if (fileName == "service_studio_report.txt")
                            {
                                serviceStudioLogs(relativePath + "\\service_studio_report.txt", delimiters);
                            }
                            else if (fileName == "timer_logs.txt")
                            {
                                timerLogs(relativePath + "\\timer_logs.txt", delimiters);
                            }
                            else if (fileName == "windows_application_event_viewer_logs.txt")
                            {
                                winAppEventViewer(relativePath + "\\windows_application_event_viewer_logs.txt", delimiters);
                            }
                            else if (fileName == "windows_security_event_viewer_logs.txt")
                            {
                                winSecEventViewer(relativePath + "\\windows_security_event_viewer_logs.txt", delimiters);
                            }
                            else if (fileName == "windows_system_event_viewer_logs.txt")
                            {
                                winSysEventViewer(relativePath + "\\windows_system_event_viewer_logs.txt", delimiters);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select any of the \"filtered\" files generated by the Python script", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }  
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void errorLogs(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "DATE_TIME", "MESSAGE", "STACK", "MODULE_NAME", "APPLICATION_NAME",
                "APPLICATION_KEY", "ACTION_NAME", "ENTRYPOINT_NAME", "SERVER", "ESPACE_NAME", "ESPACE_ID", 
                "USER_ID", "SESSION_ID", "ENVIRONMENT_INFORMATION", "ID", "TENANT_ID"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }
                    
                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewErrorlogs.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void generalLogs(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "DATE_TIME", "MESSAGE", "MESSAGE_TYPE", "MODULE_NAME", "APPLICATION_NAME",
                "APPLICATION_KEY", "ACTION_NAME", "ENTRYPOINT_NAME", "CLIENT_IP", "ESPACE_NAME", "ESPACE_ID",
                "USER_ID", "SESSION_ID", "ERROR_ID", "REQUEST_KEY", "TENANT_ID"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }

                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewGenerallogs.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void integrationsLogs(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "ACTION_NAME",
                "ACTION_TYPE", "SOURCE", "ENDPOINT", "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID",
                "ERROR_ID", "REQUEST_KEY", "TENANT_ID"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }

                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewIntegrationslogs.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void mobileRequestsLogs(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "DATE_TIME", "DURATION", "SCREEN", "APPLICATION_NAME", "APPLICATION_KEY", "SOURCE",
                "ENDPOINT", "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID", "LOGIN_ID", "USER_ID", "CYCLE",
                "ERROR_ID", "REQUEST_KEY", "TENANT_ID"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }

                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewScreenRequestslogs.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void timerLogs(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "EXECUTED_BY",
                "ESPACE_NAME", "ESPACE_ID", "CYCLIC_JOB_NAME", "CYCLIC_JOB_KEY", "SHOULD_HAVE_RUN_AT", "NEXT_RUN",
                "ERROR_ID", "REQUEST_KEY", "TENANT_ID"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }

                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewTimerlogs.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void emailLogs(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "DATE_TIME", "SENT", "LAST_ERROR", "FROM", "TO",
                "SUBJECT", "CC", "BCC", "NAME", "SIZE", "MESSAGE_ID", "ACTIVITY",
                "EMAIL_DEFINITION", "STORE_CONTENT", "IS_TEST_EMAIL", "ID", "TENANT_ID"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }

                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewEmaillogs.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void extensionLogs(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "ACTION_NAME",
                "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID", "USERNAME", "USER_ID", "SESSION_ID", "EXTENSION_ID",
                "EXTENSION_NAME", "ERROR_ID", "REQUEST_KEY", "TENANT_ID"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }

                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewExtensionlogs.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void serviceActionLogs(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "ACTION_NAME", "SOURCE",
                "ENTRYPOINT_NAME", "ENDPOINT", "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID", "USERNAME", "LOGIN_ID",
                "USER_ID", "SESSION_ID", "ERROR_ID", "REQUEST_KEY", "ORIGINAL_REQUEST_KEY", "TENANT_ID"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }

                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewServiceActionlogs.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void screenLogs(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "DATE_TIME", "DURATION", "SCREEN", "SCREEN_TYPE", "APPLICATION_NAME", "APPLICATION_KEY",
                "ACTION_NAME", "ACCESS_MODE", "EXECUTED_BY", "CLIENT_IP", "ESPACE_NAME", "ESPACE_ID",
                "USER_ID", "SESSION_ID", "SESSION_REQUESTS", "SESSION_BYTES", "VIEW_STATE_BYTES", "MS_IS_DN", "REQUEST_KEY", "TENANT_ID"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }

                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewTradWebRequests.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            var result = MessageBox.Show("ARE YOU SURE YOU WANT TO EXIT THE LOG PARSER APPLICATION?", "QUESTION", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = (result == DialogResult.No);
        }

        private void btnBrowseFile_MouseEnter(object sender, EventArgs e)
        {
            btnBrowseFile.BackColor = Color.Black;
        }

        private void btnBrowseFile_MouseLeave(object sender, EventArgs e)
        {
            btnBrowseFile.BackColor = SystemColors.ControlLight;
        }

        private void btnFilter_MouseEnter(object sender, EventArgs e)
        {
            btnFilter.BackColor = Color.Black;
        }

        private void btnFilter_MouseLeave(object sender, EventArgs e)
        {
            btnFilter.BackColor = SystemColors.ControlLight;
        }

        private void btnClearFilter_MouseEnter(object sender, EventArgs e)
        {
            btnClearFilter.BackColor = Color.Black;
        }

        private void btnClearFilter_MouseLeave(object sender, EventArgs e)
        {
            btnClearFilter.BackColor = SystemColors.ControlLight;
        }

        private void dataGridViewErrorlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewErrorlogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewErrorlogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailErrorLogs.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewGenerallogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewGenerallogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewGenerallogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailGenerallogs.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewIntegrationslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewIntegrationslogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewIntegrationslogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailIntegrationlogs.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewScreenRequestslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewScreenRequestslogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewScreenRequestslogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailScreenRequestslogs.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewTimerlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewTimerlogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewTimerlogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailTimerlogs.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewEmaillogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewEmaillogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewEmaillogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailEmaillogs.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewExtensionlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewExtensionlogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewExtensionlogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailExtensionlogs.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewServiceActionlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewServiceActionlogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewServiceActionlogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailServiceActionlogs.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewTradWebRequests_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewTradWebRequests.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewTradWebRequests.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailTradWebRequests.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }
        //End of the Service Center Logs tab code

        //Beginning of the IIS log tab code
        private void iisLogs(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "DATE_TIME", "TIME_TAKEN", "HTTP_CODE", "HTTP_SUBCODE", "WINDOWS_ERROR_CODE",
                "CLIENT_IP", "SERVER_IP", "SERVER_PORT", "METHOD", "URI_STEM", "URI_QUERY", "USERNAME", "BROWSER",
                "REFERRER"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }

                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewIISDateTime.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void iisLogsTime(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "TIME_TAKEN", "DATE_TIME", "HTTP_CODE", "HTTP_SUBCODE", "WINDOWS_ERROR_CODE",
                "CLIENT_IP", "SERVER_IP", "SERVER_PORT", "METHOD", "URI_STEM", "URI_QUERY", "USERNAME", "BROWSER",
                "REFERRER"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }

                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewIISTimeTaken.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }
        //End of the IIS logs tab code

        //Beginning of the Windows Event Viewer logs tab code
        private void winAppEventViewer(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "DATE_TIME", "LEVEL", "PROVIDER_NAME", "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID", "TASK", "KEYWORDS", "MESSAGE", "COMPUTER"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }

                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewWinAppEventViewer.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void winSecEventViewer(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "DATE_TIME", "LEVEL", "PROVIDER_NAME", "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID", "TASK", "KEYWORDS", "MESSAGE", "COMPUTER"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }

                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewWinSecEventViewer.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void winSysEventViewer(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "DATE_TIME", "LEVEL", "PROVIDER_NAME", "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID", "TASK", "KEYWORDS", "MESSAGE", "COMPUTER"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }

                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewWinSysEventViewer.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewWinAppEventViewer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewWinAppEventViewer.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewWinAppEventViewer.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailWinAppEventViewer.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewWinSysEventViewer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewWinSysEventViewer.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewWinSysEventViewer.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailWinSysEventViewer.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewWinSecEventViewer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewWinSecEventViewer.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewWinSecEventViewer.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailWinSecEventViewer.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }
        //End of the Windows Event Viewer logs tab code

        //Beginning of the Mobile logs tab code
        private void androidLogs(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "DATE_TIME", "MESSAGE_TYPE", "ACTION_NAME", "MESSAGE"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }

                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewAndroidlogs.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void iOSLogs(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "DATE_TIME", "MESSAGE_TYPE", "ACTION_NAME", "MESSAGE"
                };
            List<string> newLines = new List<string>();

            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //for the headers
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string temp = lines[i];
                        string[] fields = temp.Split(splitter);
                        while (fields.Length < colsExpected && i < (lines.Length - 1))
                        {
                            i++;
                            temp += lines[i];
                            fields = temp.Split(splitter);
                        }
                        newLines.Add(temp);
                    }

                    //for the data
                    string[] dataFields = newLines.ToArray();
                    for (int i = 0; i < dataFields.Length; i++)
                    {
                        string[] dataCols = dataFields[i].Split(splitter);
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dr[headerWord] = dataCols[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridViewiOSlogs.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewAndroidlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewAndroidlogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewAndroidlogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailAndroidLogs.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewiOSlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewiOSlogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewiOSlogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailiOSLogs.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }
        //End of the Mobile logs tab code

        private void dataGridViewIISDateTime_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewIISDateTime.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewIISDateTime.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtDetailIISlogs.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewIISTimeTaken_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewIISTimeTaken.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewIISTimeTaken.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtDetailIISlogs.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

            private void serviceStudioLogs(string filePath, char splitter)
            {
                DataTable dt = new DataTable();

                string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

                string[] headerLabels = { "DATE_TIME", "MESSAGE_TYPE", "ACTION_NAME", "MESSAGE"
                };
                List<string> newLines = new List<string>();

                int colsExpected = 0;

                try
                {
                    if (lines.Length > 0)
                    {
                        //for the headers
                        foreach (string headerWord in headerLabels)
                        {
                            dt.Columns.Add(new DataColumn(headerWord));
                        }

                        colsExpected = dt.Columns.Count;

                        //for removing line breaks in data
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string temp = lines[i];
                            string[] fields = temp.Split(splitter);
                            while (fields.Length < colsExpected && i < (lines.Length - 1))
                            {
                                i++;
                                temp += lines[i];
                                fields = temp.Split(splitter);
                            }
                            newLines.Add(temp);
                        }

                        //for the data
                        string[] dataFields = newLines.ToArray();
                        for (int i = 0; i < dataFields.Length; i++)
                        {
                            string[] dataCols = dataFields[i].Split(splitter);
                            DataRow dr = dt.NewRow();
                            int columnIndex = 0;
                            foreach (string headerWord in headerLabels)
                            {
                                dr[headerWord] = dataCols[columnIndex++];
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dataGridViewServiceStudiologs.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                    throw;
                }
            }

        private void dataGridViewServiceStudiologs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewServiceStudiologs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewServiceStudiologs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailServiceStudioLogs.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                maskedTextBox1.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                maskedTextBox2.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;

                string timeFilter1 = maskedTextBox1.Text.ToString().Trim();
                string timeFilter2 = maskedTextBox2.Text.ToString().Trim();

                if (string.IsNullOrEmpty(timeFilter1) && string.IsNullOrEmpty(timeFilter2))
                {
                    MessageBox.Show("Please enter the time in the following format: HH:mm (24 hour format)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (string.IsNullOrEmpty(timeFilter1) && !string.IsNullOrEmpty(timeFilter2))
                {
                    MessageBox.Show("Please enter the time in the following format: HH:mm (24 hour format)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    maskedTextBox1.BackColor = Color.Orange;
                    maskedTextBox1.Focus();
                }
                else if (!string.IsNullOrEmpty(timeFilter1) && string.IsNullOrEmpty(timeFilter2))
                {
                    MessageBox.Show("Please enter the time in the following format: HH:mm (24 hour format)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    maskedTextBox2.BackColor = Color.Orange;
                    maskedTextBox2.Focus();
                }
                else if (timeFilter1.Length < 4 && timeFilter2.Length < 4)
                {
                    MessageBox.Show("Please make sure to enter 2 digits for the hour and 2 digits for the minutes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (timeFilter1.Length < 4 && timeFilter2.Length == 4)
                {
                    MessageBox.Show("Please make sure to enter 2 digits for the hour and 2 digits for the minutes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    maskedTextBox1.BackColor = Color.Orange;
                    maskedTextBox1.Focus();
                }
                else if (timeFilter1.Length == 4 && timeFilter2.Length < 4)
                {
                    MessageBox.Show("Please make sure to enter 2 digits for the hour and 2 digits for the minutes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    maskedTextBox2.BackColor = Color.Orange;
                    maskedTextBox2.Focus();
                }
                else
                {
                    maskedTextBox1.BackColor = SystemColors.Window;
                    maskedTextBox2.BackColor = SystemColors.Window;

                    maskedTextBox1.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;
                    maskedTextBox2.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;

                    //parse the values from the masked textboxes to datetime and then back to string to be used in the query
                    DateTime msktxtBoxFromDateTime = DateTime.ParseExact(maskedTextBox1.Text, "HH:mm", null);
                    DateTime msktxtBoxToDateTime = DateTime.ParseExact(maskedTextBox2.Text, "HH:mm", null);

                    //parse the values from the dateTimePickers to force them to use the correct format
                    string dtpISOfrom = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    string dtpISOto = dateTimePicker2.Value.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

                    string msktxtBoxISOfrom = msktxtBoxFromDateTime.ToString("HH:mm", CultureInfo.InvariantCulture);
                    string msktxtBoxISOto = msktxtBoxToDateTime.ToString("HH:mm", CultureInfo.InvariantCulture);

                    string isoFrom = dtpISOfrom + " " + msktxtBoxISOfrom;
                    string isoTo = dtpISOto + " " + msktxtBoxISOto;

                    DateTime fromDateTime = DateTime.ParseExact(isoFrom, "yyyy-MM-dd HH:mm", null);
                    DateTime toDateTime = DateTime.ParseExact(isoTo, "yyyy-MM-dd HH:mm", null);

                    if (DateTime.Compare(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date) > 0)
                    {
                        MessageBox.Show("The \"start date\" cannot be greater than the \"end date\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (DateTime.Compare(fromDateTime, toDateTime) > 0)
                    {
                        MessageBox.Show("The \"start time\" cannot be greater than the \"end time\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        //filter the content from the table based on the datetime range
                        queryDataGridViews(dataGridViewAndroidlogs, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewEmaillogs, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewErrorlogs, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewExtensionlogs, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewGenerallogs, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewIISDateTime, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewIISTimeTaken, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewIntegrationslogs, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewiOSlogs, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewScreenRequestslogs, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewServiceActionlogs, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewServiceStudiologs, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewTimerlogs, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewTradWebRequests, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewWinAppEventViewer, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewWinSysEventViewer, isoFrom, isoTo);
                        queryDataGridViews(dataGridViewWinSecEventViewer, isoFrom, isoTo);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void queryDataGridViews(DataGridView tableName, string from, string to)
        {
            try
            {
                if (tableName.Rows.Count > 0)
                {
                    string rowFilter = string.Format("DATE_TIME >= '" + from);
                    rowFilter += string.Format("' AND DATE_TIME <= '" + to + "'");
                    (tableName.DataSource as DataTable).DefaultView.RowFilter = rowFilter;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Text = "";
            dateTimePicker2.Text = "";
            maskedTextBox1.Text = "";
            maskedTextBox2.Text = "";
            maskedTextBox1.BackColor = SystemColors.Window;
            maskedTextBox2.BackColor = SystemColors.Window;

            clearTextboxes(txtBoxDetailErrorLogs);
            clearTextboxes(txtBoxDetailGenerallogs);
            clearTextboxes(txtBoxDetailIntegrationlogs);
            clearTextboxes(txtBoxDetailScreenRequestslogs);
            clearTextboxes(txtBoxDetailTimerlogs);
            clearTextboxes(txtBoxDetailEmaillogs);
            clearTextboxes(txtBoxDetailExtensionlogs);
            clearTextboxes(txtBoxDetailServiceActionlogs);
            clearTextboxes(txtBoxDetailTradWebRequests);
            clearTextboxes(txtBoxDetailWinAppEventViewer);
            clearTextboxes(txtBoxDetailWinSysEventViewer);
            clearTextboxes(txtBoxDetailWinSecEventViewer);
            clearTextboxes(txtBoxDetailAndroidLogs);
            clearTextboxes(txtBoxDetailiOSLogs);
            clearTextboxes(txtDetailIISlogs);
            clearTextboxes(txtBoxDetailServiceStudioLogs);

            clearTables(dataGridViewErrorlogs);
            clearTables(dataGridViewGenerallogs);
            clearTables(dataGridViewIntegrationslogs);
            clearTables(dataGridViewScreenRequestslogs);
            clearTables(dataGridViewTimerlogs);
            clearTables(dataGridViewEmaillogs);
            clearTables(dataGridViewExtensionlogs);
            clearTables(dataGridViewServiceActionlogs);
            clearTables(dataGridViewTradWebRequests);
            clearTables(dataGridViewIISDateTime);
            clearTables(dataGridViewIISTimeTaken);
            clearTables(dataGridViewWinAppEventViewer);
            clearTables(dataGridViewWinSysEventViewer);
            clearTables(dataGridViewWinSecEventViewer);
            clearTables(dataGridViewAndroidlogs);
            clearTables(dataGridViewiOSlogs);
            clearTables(dataGridViewServiceStudiologs);

            foreach (string filePaths in filesPaths)
            {
                if (File.Exists(filePaths))
                {
                    fileName = Path.GetFileName(filePaths);

                    if (fileName == "android_build_logs.txt")
                    {
                        androidLogs(relativePath + "\\android_build_logs.txt", delimiters);
                    }
                    else if (fileName == "email_logs.txt")
                    {
                        emailLogs(relativePath + "\\email_logs.txt", delimiters);
                    }
                    else if (fileName == "error_logs.txt")
                    {
                        errorLogs(relativePath + "\\error_logs.txt", delimiters);
                    }
                    else if (fileName == "extension_logs.txt")
                    {
                        extensionLogs(relativePath + "\\extension_logs.txt", delimiters);
                    }
                    else if (fileName == "general_logs.txt")
                    {
                        generalLogs(relativePath + "\\general_logs.txt", delimiters);
                    }
                    else if (fileName == "iis_logs.txt")
                    {
                        iisLogs(relativePath + "\\iis_logs.txt", delimiters);
                    }
                    else if (fileName == "iis_logs_timetaken.txt")
                    {
                        iisLogsTime(relativePath + "\\iis_logs_timetaken.txt", delimiters);
                    }
                    else if (fileName == "integrations_logs.txt")
                    {
                        integrationsLogs(relativePath + "\\integrations_logs.txt", delimiters);
                    }
                    else if (fileName == "iOS_build_logs.txt")
                    {
                        iOSLogs(relativePath + "\\iOS_build_logs.txt", delimiters);
                    }
                    else if (fileName == "mobile_requests_logs.txt")
                    {
                        mobileRequestsLogs(relativePath + "\\mobile_requests_logs.txt", delimiters);
                    }
                    else if (fileName == "screen_logs.txt")
                    {
                        screenLogs(relativePath + "\\screen_logs.txt", delimiters);
                    }
                    else if (fileName == "service_action_logs.txt")
                    {
                        serviceActionLogs(relativePath + "\\service_action_logs.txt", delimiters);
                    }
                    else if (fileName == "service_studio_report.txt")
                    {
                        serviceStudioLogs(relativePath + "\\service_studio_report.txt", delimiters);
                    }
                    else if (fileName == "timer_logs.txt")
                    {
                        timerLogs(relativePath + "\\timer_logs.txt", delimiters);
                    }
                    else if (fileName == "windows_application_event_viewer_logs.txt")
                    {
                        winAppEventViewer(relativePath + "\\windows_application_event_viewer_logs.txt", delimiters);
                    }
                    else if (fileName == "windows_security_event_viewer_logs.txt")
                    {
                        winSecEventViewer(relativePath + "\\windows_security_event_viewer_logs.txt", delimiters);
                    }
                    else if (fileName == "windows_system_event_viewer_logs.txt")
                    {
                        winSysEventViewer(relativePath + "\\windows_system_event_viewer_logs.txt", delimiters);
                    }
                }
            }
        }

        private void clearTextboxes(TextBox txt_box)
        {
            txt_box.Text = "";
        }

        private void clearTables(DataGridView table_name)
        {
            table_name.DataSource = null;
        }
    }
}
