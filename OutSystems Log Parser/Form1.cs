using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
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
        int totalRowsCount;
        string myMessage = "";
        int myMessageCount;
        double percentageMessageCount;
        double roundedPercentageMessageCount;
        bool removeGarbage = false;
        bool highlightError = false;

        public Form1()
        {
            InitializeComponent();
        }

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
                                string[] column_names = { "DATE_TIME", "MESSAGE_TYPE", "ACTION_NAME", "MESSAGE" };
                                populateTables(relativePath + "\\android_build_logs.txt", delimiters, column_names, dataGridViewAndroidlogs);
                            }
                            else if (fileName == "email_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "SENT", "LAST_ERROR", "FROM", "TO", "SUBJECT",
                                    "CC", "BCC", "NAME", "SIZE", "MESSAGE_ID", "ACTIVITY", "EMAIL_DEFINITION", "STORE_CONTENT",
                                    "IS_TEST_EMAIL", "ID", "TENANT_ID" };
                                populateTables(relativePath + "\\email_logs.txt", delimiters, column_names, dataGridViewEmaillogs);
                            }
                            else if (fileName == "error_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "MESSAGE", "STACK", "MODULE_NAME", "APPLICATION_NAME",
                                    "APPLICATION_KEY", "ACTION_NAME", "ENTRYPOINT_NAME", "SERVER", "ESPACE_NAME", "ESPACE_ID",
                                    "USER_ID", "SESSION_ID", "ENVIRONMENT_INFORMATION", "ID", "TENANT_ID" };
                                populateTables(relativePath + "\\error_logs.txt", delimiters, column_names, dataGridViewErrorlogs);
                            }
                            else if (fileName == "extension_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "ACTION_NAME",
                                    "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID", "USERNAME", "USER_ID", "SESSION_ID", "EXTENSION_ID",
                                    "EXTENSION_NAME", "ERROR_ID", "REQUEST_KEY", "TENANT_ID" };
                                populateTables(relativePath + "\\extension_logs.txt", delimiters, column_names, dataGridViewExtensionlogs);
                            }
                            else if (fileName == "general_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "MESSAGE", "MESSAGE_TYPE", "MODULE_NAME", "APPLICATION_NAME",
                                    "APPLICATION_KEY", "ACTION_NAME", "ENTRYPOINT_NAME", "CLIENT_IP", "ESPACE_NAME", "ESPACE_ID",
                                    "USER_ID", "SESSION_ID", "ERROR_ID", "REQUEST_KEY", "TENANT_ID" };
                                populateTables(relativePath + "\\general_logs.txt", delimiters, column_names, dataGridViewGenerallogs);
                            }
                            else if (fileName == "iis_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "TIME_TAKEN", "HTTP_CODE", "HTTP_SUBCODE", "WINDOWS_ERROR_CODE",
                                    "CLIENT_IP", "SERVER_IP", "SERVER_PORT", "METHOD", "URI_STEM", "URI_QUERY", "USERNAME", "BROWSER",
                                    "REFERRER" };
                                populateTables(relativePath + "\\iis_logs.txt", delimiters, column_names, dataGridViewIISDateTime);
                            }
                            else if (fileName == "iis_logs_timetaken.txt")
                            {
                                string[] column_names = { "TIME_TAKEN", "DATE_TIME", "HTTP_CODE", "HTTP_SUBCODE", "WINDOWS_ERROR_CODE",
                                    "CLIENT_IP", "SERVER_IP", "SERVER_PORT", "METHOD", "URI_STEM", "URI_QUERY", "USERNAME", "BROWSER",
                                    "REFERRER" };
                                populateTables(relativePath + "\\iis_logs_timetaken.txt", delimiters, column_names, dataGridViewIISTimeTaken);
                            }
                            else if (fileName == "integrations_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "ACTION_NAME",
                                    "ACTION_TYPE", "SOURCE", "ENDPOINT", "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID", "ERROR_ID",
                                    "REQUEST_KEY", "TENANT_ID" };
                                populateTables(relativePath + "\\integrations_logs.txt", delimiters, column_names, dataGridViewIntegrationslogs);
                            }
                            else if (fileName == "iOS_build_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "MESSAGE_TYPE", "ACTION_NAME", "MESSAGE" };
                                populateTables(relativePath + "\\iOS_build_logs.txt", delimiters, column_names, dataGridViewiOSlogs);
                            }
                            else if (fileName == "mobile_requests_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "DURATION", "SCREEN", "APPLICATION_NAME", "APPLICATION_KEY", "SOURCE",
                                    "ENDPOINT", "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID", "LOGIN_ID", "USER_ID", "CYCLE",
                                    "ERROR_ID", "REQUEST_KEY", "TENANT_ID" };
                                populateTables(relativePath + "\\mobile_requests_logs.txt", delimiters, column_names, dataGridViewScreenRequestslogs);
                            }
                            else if (fileName == "screen_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "DURATION", "SCREEN", "SCREEN_TYPE", "APPLICATION_NAME", "APPLICATION_KEY",
                                    "ACTION_NAME", "ACCESS_MODE", "EXECUTED_BY", "CLIENT_IP", "ESPACE_NAME", "ESPACE_ID",
                                    "USER_ID", "SESSION_ID", "SESSION_REQUESTS", "SESSION_BYTES", "VIEW_STATE_BYTES", "MS_IS_DN", "REQUEST_KEY", "TENANT_ID" };
                                populateTables(relativePath + "\\screen_logs.txt", delimiters, column_names, dataGridViewTradWebRequests);
                            }
                            else if (fileName == "service_action_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "ACTION_NAME", "SOURCE",
                                    "ENTRYPOINT_NAME", "ENDPOINT", "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID", "USERNAME", "LOGIN_ID",
                                    "USER_ID", "SESSION_ID", "ERROR_ID", "REQUEST_KEY", "ORIGINAL_REQUEST_KEY", "TENANT_ID" };
                                populateTables(relativePath + "\\service_action_logs.txt", delimiters, column_names, dataGridViewServiceActionlogs);
                            }
                            else if (fileName == "service_studio_report.txt")
                            {
                                string[] column_names = { "DATE_TIME", "MESSAGE_TYPE", "ACTION_NAME", "MESSAGE" };
                                populateTables(relativePath + "\\service_studio_report.txt", delimiters, column_names, dataGridViewServiceStudiologs);
                            }
                            else if (fileName == "timer_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "EXECUTED_BY",
                                    "ESPACE_NAME", "ESPACE_ID", "CYCLIC_JOB_NAME", "CYCLIC_JOB_KEY", "SHOULD_HAVE_RUN_AT", "NEXT_RUN",
                                    "ERROR_ID", "REQUEST_KEY", "TENANT_ID" };
                                populateTables(relativePath + "\\timer_logs.txt", delimiters, column_names, dataGridViewTimerlogs);
                            }
                            else if (fileName == "windows_application_event_viewer_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "LEVEL", "PROVIDER_NAME", "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID",
                                    "TASK", "KEYWORDS", "MESSAGE", "COMPUTER" };
                                populateTables(relativePath + "\\windows_application_event_viewer_logs.txt", delimiters, column_names, dataGridViewWinAppEventViewer);
                            }
                            else if (fileName == "windows_security_event_viewer_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "LEVEL", "PROVIDER_NAME", "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID",
                                    "TASK", "KEYWORDS", "MESSAGE", "COMPUTER" };
                                populateTables(relativePath + "\\windows_security_event_viewer_logs.txt", delimiters, column_names, dataGridViewWinSecEventViewer);
                            }
                            else if (fileName == "windows_system_event_viewer_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "LEVEL", "PROVIDER_NAME", "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID",
                                    "TASK", "KEYWORDS", "MESSAGE", "COMPUTER" };
                                populateTables(relativePath + "\\windows_system_event_viewer_logs.txt", delimiters, column_names, dataGridViewWinSysEventViewer);
                            }
                        }
                    }

                    btnFilter.Enabled = true;
                    btnFilter.BackColor = SystemColors.ControlLight;
                    dateTimePicker1.Enabled = true;
                    maskedTextBox1.Enabled = true;
                    dateTimePicker2.Enabled = true;
                    maskedTextBox2.Enabled = true;
                    btnRemoveGarbage.Enabled = true;
                    btnRemoveGarbage.BackColor = SystemColors.ControlLight;
                    numericUpDownPercentage.Enabled = true;
                    btnHighlight.Enabled = true;
                    btnHighlight.BackColor = SystemColors.ControlLight;
                    btnClearFilter.Enabled = true;
                    btnClearFilter.BackColor = SystemColors.ControlLight;
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

        private void populateTables(string filePath, char splitter, string[] headerLabels, DataGridView tableName)
        {
            DataTable dt = new DataTable();
            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();
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
                    tableName.DataSource = dt;
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
            var result = MessageBox.Show("Are you sure you want to exit the Log Parser Application?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = (result == DialogResult.No);
        }

        private void btnBrowseFile_MouseEnter(object sender, EventArgs e)
        {
            btnBrowseFile.BackColor = Color.SpringGreen;
        }

        private void btnBrowseFile_MouseLeave(object sender, EventArgs e)
        {
            btnBrowseFile.BackColor = SystemColors.ControlLight;
        }

        private void btnFilter_MouseEnter(object sender, EventArgs e)
        {
            btnFilter.BackColor = Color.SpringGreen;
        }

        private void btnFilter_MouseLeave(object sender, EventArgs e)
        {
            btnFilter.BackColor = SystemColors.ControlLight;
        }

        private void btnClearFilter_MouseEnter(object sender, EventArgs e)
        {
            btnClearFilter.BackColor = Color.SpringGreen;
        }

        private void btnClearFilter_MouseLeave(object sender, EventArgs e)
        {
            btnClearFilter.BackColor = SystemColors.ControlLight;
        }

        private void btnRemoveGarbage_MouseEnter(object sender, EventArgs e)
        {
            btnRemoveGarbage.BackColor = Color.SpringGreen;
        }

        private void btnRemoveGarbage_MouseLeave(object sender, EventArgs e)
        {
            btnRemoveGarbage.BackColor = SystemColors.ControlLight;
        }

        private void btnHighlight_MouseEnter(object sender, EventArgs e)
        {
            btnHighlight.BackColor = Color.SpringGreen;
        }

        private void btnHighlight_MouseLeave(object sender, EventArgs e)
        {
            btnHighlight.BackColor = SystemColors.ControlLight;
        }

        //Beginning of the Service Center Logs tab code
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

        //Beginning of the Windows Event Viewer logs tab code
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

        //Beginning of the IIS tab code
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
        //End of the IIS tab code

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
                    MessageBox.Show("Please enter the time in the following format: HH:mm (24-hour format)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (string.IsNullOrEmpty(timeFilter1) && !string.IsNullOrEmpty(timeFilter2))
                {
                    MessageBox.Show("Please enter the time in the following format: HH:mm (24-hour format)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    maskedTextBox1.BackColor = Color.Orange;
                    maskedTextBox1.Focus();
                }
                else if (!string.IsNullOrEmpty(timeFilter1) && string.IsNullOrEmpty(timeFilter2))
                {
                    MessageBox.Show("Please enter the time in the following format: HH:mm (24-hour format)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                if (removeGarbage)
                {
                    removeGenericErrors(dataGridViewErrorlogs, 1, txtBoxDetailErrorLogs);
                    removeGenericErrors(dataGridViewGenerallogs, 1, txtBoxDetailGenerallogs);
                    removeGenericErrors2(dataGridViewIntegrationslogs, txtBoxDetailIntegrationlogs);
                    removeGenericErrors2(dataGridViewScreenRequestslogs, txtBoxDetailScreenRequestslogs);
                    removeGenericErrors2(dataGridViewTimerlogs, txtBoxDetailTimerlogs);
                    removeGenericErrors2(dataGridViewEmaillogs, txtBoxDetailEmaillogs);
                    removeGenericErrors2(dataGridViewExtensionlogs, txtBoxDetailExtensionlogs);
                    removeGenericErrors2(dataGridViewServiceActionlogs, txtBoxDetailServiceActionlogs);
                    removeGenericErrors2(dataGridViewTradWebRequests, txtBoxDetailTradWebRequests);
                    //removeGenericErrors2(dataGridViewIISDateTime, txtDetailIISlogs);
                    //removeGenericErrors2(dataGridViewIISTimeTaken, txtDetailIISlogs);
                    removeGenericErrors(dataGridViewWinAppEventViewer, 8, txtBoxDetailWinAppEventViewer);
                    removeGenericErrors(dataGridViewWinSysEventViewer, 8, txtBoxDetailWinSysEventViewer);
                    removeGenericErrors(dataGridViewWinSecEventViewer, 8, txtBoxDetailWinSecEventViewer);
                    removeGenericErrors(dataGridViewAndroidlogs, 3, txtBoxDetailAndroidLogs);
                    removeGenericErrors(dataGridViewiOSlogs, 3, txtBoxDetailiOSLogs);
                    removeGenericErrors(dataGridViewServiceStudiologs, 3, txtBoxDetailServiceStudioLogs);
                }

                if (highlightError)
                {
                    string[] knownErrors_Errorlogs = { "url rewrite module error", "an error occurred in task", "server cannot modify cookies", "ping validation failed", "a fatal error has occurred", "communicationexception", "json deserialization" };
                    string[] knownErrors_Generallogs = { "system cannot find" };
                    string[] knownErrors_WinAppEventViewer = { "ora-", "error closing the transaction" };
                    string[] knownErrors_WinSysEventViewer = { "error closing the transaction" };
                    string[] knownErrors_AndroidiOSlogs = { "file is corrupt or invalid", "androidx library", "command finished with error code 0", "plugin is not going to work", "error: spawnsync sudo etimeout", "plugin doesn't support this project's cordova-android version", "failed to fetch plug", "archive failed", "build failed with the following error", "command failed with exit code", "signing certificate is invalid", "the ios deployment target" };
                    string[] knownErrors_ServiceStudiologs = { "oneoftypedefinition", "http forbidden" };

                    highlightKnownErrors(dataGridViewErrorlogs, 1, knownErrors_Errorlogs);
                    highlightKnownErrors(dataGridViewGenerallogs, 1, knownErrors_Generallogs);
                    //highlightKnownErrors(dataGridViewIntegrationslogs);
                    //highlightKnownErrors(dataGridViewScreenRequestslogs);
                    //highlightKnownErrors(dataGridViewTimerlogs);
                    //highlightKnownErrors(dataGridViewEmaillogs);
                    //highlightKnownErrors(dataGridViewExtensionlogs);
                    //highlightKnownErrors(dataGridViewServiceActionlogs);
                    //highlightKnownErrors(dataGridViewTradWebRequests);
                    //highlightKnownErrors(dataGridViewIISDateTime);
                    //highlightKnownErrors(dataGridViewIISTimeTaken);
                    highlightKnownErrors(dataGridViewWinAppEventViewer, 8, knownErrors_WinAppEventViewer);
                    highlightKnownErrors(dataGridViewWinSysEventViewer, 8, knownErrors_WinSysEventViewer);
                    //highlightKnownErrors(dataGridViewWinSecEventViewer, 8);
                    highlightKnownErrors(dataGridViewAndroidlogs, 3, knownErrors_AndroidiOSlogs);
                    highlightKnownErrors(dataGridViewiOSlogs, 3, knownErrors_AndroidiOSlogs);
                    highlightKnownErrors(dataGridViewServiceStudiologs, 3, knownErrors_ServiceStudiologs);
                }

                btnFilter.Enabled = false;
                dateTimePicker1.Enabled = false;
                maskedTextBox1.Enabled = false;
                dateTimePicker2.Enabled = false;
                maskedTextBox2.Enabled = false;
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
            removeGarbage = false;
            highlightError = false;

            dateTimePicker1.Text = "";
            dateTimePicker2.Text = "";
            maskedTextBox1.Text = "";
            maskedTextBox2.Text = "";
            maskedTextBox1.BackColor = SystemColors.Window;
            maskedTextBox2.BackColor = SystemColors.Window;

            numericUpDownPercentage.Value = 20;

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
                        string[] column_names = { "DATE_TIME", "MESSAGE_TYPE", "ACTION_NAME", "MESSAGE" };
                        populateTables(relativePath + "\\android_build_logs.txt", delimiters, column_names, dataGridViewAndroidlogs);
                    }
                    else if (fileName == "email_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "SENT", "LAST_ERROR", "FROM", "TO", "SUBJECT",
                                    "CC", "BCC", "NAME", "SIZE", "MESSAGE_ID", "ACTIVITY", "EMAIL_DEFINITION", "STORE_CONTENT",
                                    "IS_TEST_EMAIL", "ID", "TENANT_ID" };
                        populateTables(relativePath + "\\email_logs.txt", delimiters, column_names, dataGridViewEmaillogs);
                    }
                    else if (fileName == "error_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "MESSAGE", "STACK", "MODULE_NAME", "APPLICATION_NAME",
                                    "APPLICATION_KEY", "ACTION_NAME", "ENTRYPOINT_NAME", "SERVER", "ESPACE_NAME", "ESPACE_ID",
                                    "USER_ID", "SESSION_ID", "ENVIRONMENT_INFORMATION", "ID", "TENANT_ID" };
                        populateTables(relativePath + "\\error_logs.txt", delimiters, column_names, dataGridViewErrorlogs);
                    }
                    else if (fileName == "extension_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "ACTION_NAME",
                                    "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID", "USERNAME", "USER_ID", "SESSION_ID", "EXTENSION_ID",
                                    "EXTENSION_NAME", "ERROR_ID", "REQUEST_KEY", "TENANT_ID" };
                        populateTables(relativePath + "\\extension_logs.txt", delimiters, column_names, dataGridViewExtensionlogs);
                    }
                    else if (fileName == "general_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "MESSAGE", "MESSAGE_TYPE", "MODULE_NAME", "APPLICATION_NAME",
                                    "APPLICATION_KEY", "ACTION_NAME", "ENTRYPOINT_NAME", "CLIENT_IP", "ESPACE_NAME", "ESPACE_ID",
                                    "USER_ID", "SESSION_ID", "ERROR_ID", "REQUEST_KEY", "TENANT_ID" };
                        populateTables(relativePath + "\\general_logs.txt", delimiters, column_names, dataGridViewGenerallogs);
                    }
                    else if (fileName == "iis_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "TIME_TAKEN", "HTTP_CODE", "HTTP_SUBCODE", "WINDOWS_ERROR_CODE",
                                    "CLIENT_IP", "SERVER_IP", "SERVER_PORT", "METHOD", "URI_STEM", "URI_QUERY", "USERNAME", "BROWSER",
                                    "REFERRER" };
                        populateTables(relativePath + "\\iis_logs.txt", delimiters, column_names, dataGridViewIISDateTime);
                    }
                    else if (fileName == "iis_logs_timetaken.txt")
                    {
                        string[] column_names = { "TIME_TAKEN", "DATE_TIME", "HTTP_CODE", "HTTP_SUBCODE", "WINDOWS_ERROR_CODE",
                                    "CLIENT_IP", "SERVER_IP", "SERVER_PORT", "METHOD", "URI_STEM", "URI_QUERY", "USERNAME", "BROWSER",
                                    "REFERRER" };
                        populateTables(relativePath + "\\iis_logs_timetaken.txt", delimiters, column_names, dataGridViewIISTimeTaken);
                    }
                    else if (fileName == "integrations_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "ACTION_NAME",
                                    "ACTION_TYPE", "SOURCE", "ENDPOINT", "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID", "ERROR_ID",
                                    "REQUEST_KEY", "TENANT_ID" };
                        populateTables(relativePath + "\\integrations_logs.txt", delimiters, column_names, dataGridViewIntegrationslogs);
                    }
                    else if (fileName == "iOS_build_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "MESSAGE_TYPE", "ACTION_NAME", "MESSAGE" };
                        populateTables(relativePath + "\\iOS_build_logs.txt", delimiters, column_names, dataGridViewiOSlogs);
                    }
                    else if (fileName == "mobile_requests_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "DURATION", "SCREEN", "APPLICATION_NAME", "APPLICATION_KEY", "SOURCE",
                                    "ENDPOINT", "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID", "LOGIN_ID", "USER_ID", "CYCLE",
                                    "ERROR_ID", "REQUEST_KEY", "TENANT_ID" };
                        populateTables(relativePath + "\\mobile_requests_logs.txt", delimiters, column_names, dataGridViewScreenRequestslogs);
                    }
                    else if (fileName == "screen_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "DURATION", "SCREEN", "SCREEN_TYPE", "APPLICATION_NAME", "APPLICATION_KEY",
                                    "ACTION_NAME", "ACCESS_MODE", "EXECUTED_BY", "CLIENT_IP", "ESPACE_NAME", "ESPACE_ID",
                                    "USER_ID", "SESSION_ID", "SESSION_REQUESTS", "SESSION_BYTES", "VIEW_STATE_BYTES", "MS_IS_DN", "REQUEST_KEY", "TENANT_ID" };
                        populateTables(relativePath + "\\screen_logs.txt", delimiters, column_names, dataGridViewTradWebRequests);
                    }
                    else if (fileName == "service_action_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "ACTION_NAME", "SOURCE",
                                    "ENTRYPOINT_NAME", "ENDPOINT", "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID", "USERNAME", "LOGIN_ID",
                                    "USER_ID", "SESSION_ID", "ERROR_ID", "REQUEST_KEY", "ORIGINAL_REQUEST_KEY", "TENANT_ID" };
                        populateTables(relativePath + "\\service_action_logs.txt", delimiters, column_names, dataGridViewServiceActionlogs);
                    }
                    else if (fileName == "service_studio_report.txt")
                    {
                        string[] column_names = { "DATE_TIME", "MESSAGE_TYPE", "ACTION_NAME", "MESSAGE" };
                        populateTables(relativePath + "\\service_studio_report.txt", delimiters, column_names, dataGridViewServiceStudiologs);
                    }
                    else if (fileName == "timer_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "EXECUTED_BY",
                                    "ESPACE_NAME", "ESPACE_ID", "CYCLIC_JOB_NAME", "CYCLIC_JOB_KEY", "SHOULD_HAVE_RUN_AT", "NEXT_RUN",
                                    "ERROR_ID", "REQUEST_KEY", "TENANT_ID" };
                        populateTables(relativePath + "\\timer_logs.txt", delimiters, column_names, dataGridViewTimerlogs);
                    }
                    else if (fileName == "windows_application_event_viewer_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "LEVEL", "PROVIDER_NAME", "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID",
                                    "TASK", "KEYWORDS", "MESSAGE", "COMPUTER" };
                        populateTables(relativePath + "\\windows_application_event_viewer_logs.txt", delimiters, column_names, dataGridViewWinAppEventViewer);
                    }
                    else if (fileName == "windows_security_event_viewer_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "LEVEL", "PROVIDER_NAME", "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID",
                                    "TASK", "KEYWORDS", "MESSAGE", "COMPUTER" };
                        populateTables(relativePath + "\\windows_security_event_viewer_logs.txt", delimiters, column_names, dataGridViewWinSecEventViewer);
                    }
                    else if (fileName == "windows_system_event_viewer_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "LEVEL", "PROVIDER_NAME", "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID",
                                    "TASK", "KEYWORDS", "MESSAGE", "COMPUTER" };
                        populateTables(relativePath + "\\windows_system_event_viewer_logs.txt", delimiters, column_names, dataGridViewWinSysEventViewer);
                    }

                    btnFilter.Enabled = true;
                    btnFilter.BackColor = SystemColors.ControlLight;
                    dateTimePicker1.Enabled = true;
                    maskedTextBox1.Enabled = true;
                    dateTimePicker2.Enabled = true;
                    maskedTextBox2.Enabled = true;
                    btnRemoveGarbage.Enabled = true;
                    btnRemoveGarbage.BackColor = SystemColors.ControlLight;
                    numericUpDownPercentage.Enabled = true;
                    btnHighlight.Enabled = true;
                    btnHighlight.BackColor = SystemColors.ControlLight;
                }
                else
                {
                    btnFilter.Enabled = false;
                    dateTimePicker1.Enabled = false;
                    maskedTextBox1.Enabled = false;
                    dateTimePicker2.Enabled = false;
                    maskedTextBox2.Enabled = false;
                    btnRemoveGarbage.Enabled = false;
                    numericUpDownPercentage.Enabled = false;
                    btnHighlight.Enabled = false;
                    btnClearFilter.Enabled = false;
                }
            }
        }

        private void clearTextboxes(TextBox txtbox)
        {
            txtbox.Text = "";
        }

        private void clearTables(DataGridView tableName)
        {
            tableName.DataSource = null;
        }

        private void btnRemoveGarbage_Click(object sender, EventArgs e)
        {
            removeGarbage = true;

            removeGenericErrors(dataGridViewErrorlogs, 1, txtBoxDetailErrorLogs);
            removeGenericErrors(dataGridViewGenerallogs, 1, txtBoxDetailGenerallogs);
            removeGenericErrors2(dataGridViewIntegrationslogs, txtBoxDetailIntegrationlogs);
            removeGenericErrors2(dataGridViewScreenRequestslogs, txtBoxDetailScreenRequestslogs);
            removeGenericErrors2(dataGridViewTimerlogs, txtBoxDetailTimerlogs);
            removeGenericErrors2(dataGridViewEmaillogs, txtBoxDetailEmaillogs);
            removeGenericErrors2(dataGridViewExtensionlogs, txtBoxDetailExtensionlogs);
            removeGenericErrors2(dataGridViewServiceActionlogs, txtBoxDetailServiceActionlogs);
            removeGenericErrors2(dataGridViewTradWebRequests, txtBoxDetailTradWebRequests);
            //removeGenericErrors2(dataGridViewIISDateTime, txtDetailIISlogs);
            //removeGenericErrors2(dataGridViewIISTimeTaken, txtDetailIISlogs);
            removeGenericErrors(dataGridViewWinAppEventViewer, 8, txtBoxDetailWinAppEventViewer);
            removeGenericErrors(dataGridViewWinSysEventViewer, 8, txtBoxDetailWinSysEventViewer);
            removeGenericErrors(dataGridViewWinSecEventViewer, 8, txtBoxDetailWinSecEventViewer);
            removeGenericErrors(dataGridViewAndroidlogs, 3, txtBoxDetailAndroidLogs);
            removeGenericErrors(dataGridViewiOSlogs, 3, txtBoxDetailiOSLogs);
            removeGenericErrors(dataGridViewServiceStudiologs, 3, txtBoxDetailServiceStudioLogs);

            if (highlightError)
            {
                string[] knownErrors_Errorlogs = { "url rewrite module error", "an error occurred in task", "server cannot modify cookies", "ping validation failed", "a fatal error has occurred", "communicationexception", "json deserialization" };
                string[] knownErrors_Generallogs = { "system cannot find" };
                string[] knownErrors_WinAppEventViewer = { "ora-", "error closing the transaction" };
                string[] knownErrors_WinSysEventViewer = { "error closing the transaction" };
                string[] knownErrors_AndroidiOSlogs = { "file is corrupt or invalid", "androidx library", "command finished with error code 0", "plugin is not going to work", "error: spawnsync sudo etimeout", "plugin doesn't support this project's cordova-android version", "failed to fetch plug", "archive failed", "build failed with the following error", "command failed with exit code", "signing certificate is invalid", "the ios deployment target" };
                string[] knownErrors_ServiceStudiologs = { "oneoftypedefinition", "http forbidden" };

                highlightKnownErrors(dataGridViewErrorlogs, 1, knownErrors_Errorlogs);
                highlightKnownErrors(dataGridViewGenerallogs, 1, knownErrors_Generallogs);
                //highlightKnownErrors(dataGridViewIntegrationslogs);
                //highlightKnownErrors(dataGridViewScreenRequestslogs);
                //highlightKnownErrors(dataGridViewTimerlogs);
                //highlightKnownErrors(dataGridViewEmaillogs);
                //highlightKnownErrors(dataGridViewExtensionlogs);
                //highlightKnownErrors(dataGridViewServiceActionlogs);
                //highlightKnownErrors(dataGridViewTradWebRequests);
                //highlightKnownErrors(dataGridViewIISDateTime);
                //highlightKnownErrors(dataGridViewIISTimeTaken);
                highlightKnownErrors(dataGridViewWinAppEventViewer, 8, knownErrors_WinAppEventViewer);
                highlightKnownErrors(dataGridViewWinSysEventViewer, 8, knownErrors_WinSysEventViewer);
                //highlightKnownErrors(dataGridViewWinSecEventViewer, 8);
                highlightKnownErrors(dataGridViewAndroidlogs, 3, knownErrors_AndroidiOSlogs);
                highlightKnownErrors(dataGridViewiOSlogs, 3, knownErrors_AndroidiOSlogs);
                highlightKnownErrors(dataGridViewServiceStudiologs, 3, knownErrors_ServiceStudiologs);
            }

            btnRemoveGarbage.Enabled = false;
            numericUpDownPercentage.Enabled = false;
        }

        private void removeGenericErrors(DataGridView tableName, int columnIndex, TextBox txtbox)
        {
            try
            {
                totalRowsCount = tableName.RowCount;

                var messageLineCountQuery = tableName.Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells[columnIndex].Value != null)
                    .Select(r => r.Cells[columnIndex].Value)
                    .GroupBy(msg => msg)
                        .OrderByDescending(msg => msg.Count())
                        .Select(g => new { Message = g.Key, Count = g.Count() });

                foreach (var messageLineCount in messageLineCountQuery)
                {
                    myMessage = messageLineCount.Message.ToString();
                    myMessageCount = messageLineCount.Count;

                    percentageMessageCount = ((double)myMessageCount / (double)totalRowsCount) * 100;

                    roundedPercentageMessageCount = Math.Round(percentageMessageCount, 0, MidpointRounding.AwayFromZero);

                    if (roundedPercentageMessageCount >= (double)numericUpDownPercentage.Value)
                    {
                        foreach (DataGridViewRow row in tableName.Rows)
                        {
                            if (row.Cells[columnIndex].Value.ToString().Equals(myMessage))
                            {
                                tableName.CurrentCell = null;
                                clearTextboxes(txtbox);
                                row.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void removeGenericErrors2(DataGridView tableName, TextBox txtbox)
        {
            try
            {
                totalRowsCount = tableName.RowCount;

                if (tableName.Name.ToString() == "dataGridViewIntegrationslogs")
                {
                    var messageLineCountQuery = tableName.Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null && r.Cells[10].Value != null)
                    .Select(r => new { Val2 = r.Cells[2].Value, Val3 = r.Cells[3].Value, Val4 = r.Cells[4].Value, Val5 = r.Cells[5].Value, Val6 = r.Cells[6].Value, Val7 = r.Cells[7].Value, Val8 = r.Cells[8].Value, Val9 = r.Cells[9].Value, Val10 = r.Cells[10].Value })
                    .GroupBy(msg => msg)
                        .OrderByDescending(msg => msg.Count())
                        .Select(g => new { Message = g.Key, Count = g.Count() });

                    foreach (var messageLineCount in messageLineCountQuery)
                    {
                        myMessage = messageLineCount.Message.ToString();
                        myMessageCount = messageLineCount.Count;

                        percentageMessageCount = ((double)myMessageCount / (double)totalRowsCount) * 100;

                        roundedPercentageMessageCount = Math.Round(percentageMessageCount, 0, MidpointRounding.AwayFromZero);

                        if (roundedPercentageMessageCount >= (double)numericUpDownPercentage.Value)
                        {
                            foreach (DataGridViewRow row in tableName.Rows)
                            {
                                string myString = row.Cells[2].Value.ToString() + row.Cells[3].Value.ToString() + row.Cells[4].Value.ToString() + row.Cells[5].Value.ToString() + row.Cells[6].Value.ToString() + row.Cells[7].Value.ToString() + row.Cells[8].Value.ToString() + row.Cells[9].Value.ToString() + row.Cells[10].Value.ToString();
                                string myNewMessage = "";
                                myNewMessage = Regex.Replace(myMessage, @"([\{\,]?[ ]Val[\d]{1,2}[ ][\=][ ]?)", "");
                                myNewMessage = myNewMessage.Replace(" }", "");
                                if (myString.Equals(myNewMessage))
                                {
                                    tableName.CurrentCell = null;
                                    clearTextboxes(txtbox);
                                    row.Visible = false;
                                }
                            }
                        }
                    }
                }
                else if (tableName.Name.ToString() == "dataGridViewScreenRequestslogs")
                {
                    var messageLineCountQuery = tableName.Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null && r.Cells[10].Value != null && r.Cells[11].Value != null && r.Cells[12].Value != null)
                    .Select(r => new { Val2 = r.Cells[2].Value, Val3 = r.Cells[3].Value, Val4 = r.Cells[4].Value, Val5 = r.Cells[5].Value, Val6 = r.Cells[6].Value, Val7 = r.Cells[7].Value, Val8 = r.Cells[8].Value, Val9 = r.Cells[9].Value, Val10 = r.Cells[10].Value, Val11 = r.Cells[11].Value, Val12 = r.Cells[12].Value })
                    .GroupBy(msg => msg)
                        .OrderByDescending(msg => msg.Count())
                        .Select(g => new { Message = g.Key, Count = g.Count() });

                    foreach (var messageLineCount in messageLineCountQuery)
                    {
                        myMessage = messageLineCount.Message.ToString();
                        myMessageCount = messageLineCount.Count;

                        percentageMessageCount = ((double)myMessageCount / (double)totalRowsCount) * 100;

                        roundedPercentageMessageCount = Math.Round(percentageMessageCount, 0, MidpointRounding.AwayFromZero);

                        if (roundedPercentageMessageCount >= (double)numericUpDownPercentage.Value)
                        {
                            foreach (DataGridViewRow row in tableName.Rows)
                            {
                                string myString = row.Cells[2].Value.ToString() + row.Cells[3].Value.ToString() + row.Cells[4].Value.ToString() + row.Cells[5].Value.ToString() + row.Cells[6].Value.ToString() + row.Cells[7].Value.ToString() + row.Cells[8].Value.ToString() + row.Cells[9].Value.ToString() + row.Cells[10].Value.ToString() + row.Cells[11].Value.ToString() + row.Cells[12].Value.ToString();
                                string myNewMessage = "";
                                myNewMessage = Regex.Replace(myMessage, @"([\{\,]?[ ]Val[\d]{1,2}[ ][\=][ ]?)", "");
                                myNewMessage = myNewMessage.Replace(" }", "");
                                if (myString.Equals(myNewMessage))
                                {
                                    tableName.CurrentCell = null;
                                    clearTextboxes(txtbox);
                                    row.Visible = false;
                                }
                            }
                        }
                    }
                }
                else if (tableName.Name.ToString() == "dataGridViewTimerlogs")
                {
                    var messageLineCountQuery = tableName.Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null)
                    .Select(r => new { Val2 = r.Cells[2].Value, Val3 = r.Cells[3].Value, Val4 = r.Cells[4].Value, Val5 = r.Cells[5].Value, Val6 = r.Cells[6].Value, Val7 = r.Cells[7].Value })
                    .GroupBy(msg => msg)
                        .OrderByDescending(msg => msg.Count())
                        .Select(g => new { Message = g.Key, Count = g.Count() });

                    foreach (var messageLineCount in messageLineCountQuery)
                    {
                        myMessage = messageLineCount.Message.ToString();
                        myMessageCount = messageLineCount.Count;

                        percentageMessageCount = ((double)myMessageCount / (double)totalRowsCount) * 100;

                        roundedPercentageMessageCount = Math.Round(percentageMessageCount, 0, MidpointRounding.AwayFromZero);

                        if (roundedPercentageMessageCount >= (double)numericUpDownPercentage.Value)
                        {
                            foreach (DataGridViewRow row in tableName.Rows)
                            {
                                string myString = row.Cells[2].Value.ToString() + row.Cells[3].Value.ToString() + row.Cells[4].Value.ToString() + row.Cells[5].Value.ToString() + row.Cells[6].Value.ToString() + row.Cells[7].Value.ToString();
                                string myNewMessage = "";
                                myNewMessage = Regex.Replace(myMessage, @"([\{\,]?[ ]Val[\d]{1,2}[ ][\=][ ]?)", "");
                                myNewMessage = myNewMessage.Replace(" }", "");
                                if (myString.Equals(myNewMessage))
                                {
                                    tableName.CurrentCell = null;
                                    clearTextboxes(txtbox);
                                    row.Visible = false;
                                }
                            }
                        }
                    }
                }
                else if (tableName.Name.ToString() == "dataGridViewEmaillogs")
                {
                    var messageLineCountQuery = tableName.Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null)
                    .Select(r => new { Val3 = r.Cells[3].Value, Val4 = r.Cells[4].Value, Val5 = r.Cells[5].Value })
                    .GroupBy(msg => msg)
                        .OrderByDescending(msg => msg.Count())
                        .Select(g => new { Message = g.Key, Count = g.Count() });

                    foreach (var messageLineCount in messageLineCountQuery)
                    {
                        myMessage = messageLineCount.Message.ToString();
                        myMessageCount = messageLineCount.Count;

                        percentageMessageCount = ((double)myMessageCount / (double)totalRowsCount) * 100;

                        roundedPercentageMessageCount = Math.Round(percentageMessageCount, 0, MidpointRounding.AwayFromZero);

                        if (roundedPercentageMessageCount >= (double)numericUpDownPercentage.Value)
                        {
                            foreach (DataGridViewRow row in tableName.Rows)
                            {
                                string myString = row.Cells[3].Value.ToString() + row.Cells[4].Value.ToString() + row.Cells[5].Value.ToString();
                                string myNewMessage = "";
                                myNewMessage = Regex.Replace(myMessage, @"([\{\,]?[ ]Val[\d]{1,2}[ ][\=][ ]?)", "");
                                myNewMessage = myNewMessage.Replace(" }", "");
                                if (myString.Equals(myNewMessage))
                                {
                                    tableName.CurrentCell = null;
                                    clearTextboxes(txtbox);
                                    row.Visible = false;
                                }
                            }
                        }
                    }
                }
                else if (tableName.Name.ToString() == "dataGridViewExtensionlogs")
                {
                    var messageLineCountQuery = tableName.Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null)
                    .Select(r => new { Val2 = r.Cells[2].Value, Val3 = r.Cells[3].Value, Val4 = r.Cells[4].Value, Val5 = r.Cells[5].Value, Val6 = r.Cells[6].Value, Val7 = r.Cells[7].Value })
                    .GroupBy(msg => msg)
                        .OrderByDescending(msg => msg.Count())
                        .Select(g => new { Message = g.Key, Count = g.Count() });

                    foreach (var messageLineCount in messageLineCountQuery)
                    {
                        myMessage = messageLineCount.Message.ToString();
                        myMessageCount = messageLineCount.Count;

                        percentageMessageCount = ((double)myMessageCount / (double)totalRowsCount) * 100;

                        roundedPercentageMessageCount = Math.Round(percentageMessageCount, 0, MidpointRounding.AwayFromZero);

                        if (roundedPercentageMessageCount >= (double)numericUpDownPercentage.Value)
                        {
                            foreach (DataGridViewRow row in tableName.Rows)
                            {
                                string myString = row.Cells[2].Value.ToString() + row.Cells[3].Value.ToString() + row.Cells[4].Value.ToString() + row.Cells[5].Value.ToString() + row.Cells[6].Value.ToString() + row.Cells[7].Value.ToString();
                                string myNewMessage = "";
                                myNewMessage = Regex.Replace(myMessage, @"([\{\,]?[ ]Val[\d]{1,2}[ ][\=][ ]?)", "");
                                myNewMessage = myNewMessage.Replace(" }", "");
                                if (myString.Equals(myNewMessage))
                                {
                                    tableName.CurrentCell = null;
                                    clearTextboxes(txtbox);
                                    row.Visible = false;
                                }
                            }
                        }
                    }
                }
                else if (tableName.Name.ToString() == "dataGridViewServiceActionlogs")
                {
                    var messageLineCountQuery = tableName.Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null)
                    .Select(r => new { Val2 = r.Cells[2].Value, Val3 = r.Cells[3].Value, Val4 = r.Cells[4].Value, Val5 = r.Cells[5].Value })
                    .GroupBy(msg => msg)
                        .OrderByDescending(msg => msg.Count())
                        .Select(g => new { Message = g.Key, Count = g.Count() });

                    foreach (var messageLineCount in messageLineCountQuery)
                    {
                        myMessage = messageLineCount.Message.ToString();
                        myMessageCount = messageLineCount.Count;

                        percentageMessageCount = ((double)myMessageCount / (double)totalRowsCount) * 100;

                        roundedPercentageMessageCount = Math.Round(percentageMessageCount, 0, MidpointRounding.AwayFromZero);

                        if (roundedPercentageMessageCount >= (double)numericUpDownPercentage.Value)
                        {
                            foreach (DataGridViewRow row in tableName.Rows)
                            {
                                string myString = row.Cells[2].Value.ToString() + row.Cells[3].Value.ToString() + row.Cells[4].Value.ToString() + row.Cells[5].Value.ToString();
                                string myNewMessage = "";
                                myNewMessage = Regex.Replace(myMessage, @"([\{\,]?[ ]Val[\d]{1,2}[ ][\=][ ]?)", "");
                                myNewMessage = myNewMessage.Replace(" }", "");
                                if (myString.Equals(myNewMessage))
                                {
                                    tableName.CurrentCell = null;
                                    clearTextboxes(txtbox);
                                    row.Visible = false;
                                }
                            }
                        }
                    }
                }
                else if (tableName.Name.ToString() == "dataGridViewTradWebRequests")
                {
                    var messageLineCountQuery = tableName.Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null && r.Cells[10].Value != null && r.Cells[11].Value != null)
                    .Select(r => new { Val2 = r.Cells[2].Value, Val3 = r.Cells[3].Value, Val4 = r.Cells[4].Value, Val5 = r.Cells[5].Value, Val6 = r.Cells[6].Value, Val7 = r.Cells[7].Value, Val8 = r.Cells[8].Value, Val9 = r.Cells[9].Value, Val10 = r.Cells[10].Value, Val11 = r.Cells[11].Value })
                    .GroupBy(msg => msg)
                        .OrderByDescending(msg => msg.Count())
                        .Select(g => new { Message = g.Key, Count = g.Count() });

                    foreach (var messageLineCount in messageLineCountQuery)
                    {
                        myMessage = messageLineCount.Message.ToString();
                        myMessageCount = messageLineCount.Count;

                        percentageMessageCount = ((double)myMessageCount / (double)totalRowsCount) * 100;

                        roundedPercentageMessageCount = Math.Round(percentageMessageCount, 0, MidpointRounding.AwayFromZero);

                        if (roundedPercentageMessageCount >= (double)numericUpDownPercentage.Value)
                        {
                            foreach (DataGridViewRow row in tableName.Rows)
                            {
                                string myString = row.Cells[2].Value.ToString() + row.Cells[3].Value.ToString() + row.Cells[4].Value.ToString() + row.Cells[5].Value.ToString() + row.Cells[6].Value.ToString() + row.Cells[7].Value.ToString() + row.Cells[8].Value.ToString() + row.Cells[9].Value.ToString() + row.Cells[10].Value.ToString() + row.Cells[11].Value.ToString();
                                string myNewMessage = "";
                                myNewMessage = Regex.Replace(myMessage, @"([\{\,]?[ ]Val[\d]{1,2}[ ][\=][ ]?)", "");
                                myNewMessage = myNewMessage.Replace(" }", "");
                                if (myString.Equals(myNewMessage))
                                {
                                    tableName.CurrentCell = null;
                                    clearTextboxes(txtbox);
                                    row.Visible = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void btnHighlight_Click(object sender, EventArgs e)
        {
            highlightError = true;

            string[] knownErrors_Errorlogs = { "url rewrite module error", "an error occurred in task", "server cannot modify cookies", "ping validation failed", "a fatal error has occurred", "communicationexception", "json deserialization" };
            string[] knownErrors_Generallogs = { "system cannot find" };
            string[] knownErrors_WinAppEventViewer = { "ora-", "error closing the transaction" };
            string[] knownErrors_WinSysEventViewer = { "error closing the transaction" };
            string[] knownErrors_AndroidiOSlogs = { "file is corrupt or invalid", "androidx library", "command finished with error code 0", "plugin is not going to work", "error: spawnsync sudo etimeout", "plugin doesn't support this project's cordova-android version", "failed to fetch plug", "archive failed", "build failed with the following error", "command failed with exit code", "signing certificate is invalid", "the ios deployment target" };
            string[] knownErrors_ServiceStudiologs = { "oneoftypedefinition", "http forbidden" };

            highlightKnownErrors(dataGridViewErrorlogs, 1, knownErrors_Errorlogs);
            highlightKnownErrors(dataGridViewGenerallogs, 1, knownErrors_Generallogs);
            //highlightKnownErrors(dataGridViewIntegrationslogs);
            //highlightKnownErrors(dataGridViewScreenRequestslogs);
            //highlightKnownErrors(dataGridViewTimerlogs);
            //highlightKnownErrors(dataGridViewEmaillogs);
            //highlightKnownErrors(dataGridViewExtensionlogs);
            //highlightKnownErrors(dataGridViewServiceActionlogs);
            //highlightKnownErrors(dataGridViewTradWebRequests);
            //highlightKnownErrors(dataGridViewIISDateTime);
            //highlightKnownErrors(dataGridViewIISTimeTaken);
            highlightKnownErrors(dataGridViewWinAppEventViewer, 8, knownErrors_WinAppEventViewer);
            highlightKnownErrors(dataGridViewWinSysEventViewer, 8, knownErrors_WinSysEventViewer);
            //highlightKnownErrors(dataGridViewWinSecEventViewer, 8);
            highlightKnownErrors(dataGridViewAndroidlogs, 3, knownErrors_AndroidiOSlogs);
            highlightKnownErrors(dataGridViewiOSlogs, 3, knownErrors_AndroidiOSlogs);
            highlightKnownErrors(dataGridViewServiceStudiologs, 3, knownErrors_ServiceStudiologs);

            if (removeGarbage)
            {
                removeGenericErrors(dataGridViewErrorlogs, 1, txtBoxDetailErrorLogs);
                removeGenericErrors(dataGridViewGenerallogs, 1, txtBoxDetailGenerallogs);
                removeGenericErrors2(dataGridViewIntegrationslogs, txtBoxDetailIntegrationlogs);
                removeGenericErrors2(dataGridViewScreenRequestslogs, txtBoxDetailScreenRequestslogs);
                removeGenericErrors2(dataGridViewTimerlogs, txtBoxDetailTimerlogs);
                removeGenericErrors2(dataGridViewEmaillogs, txtBoxDetailEmaillogs);
                removeGenericErrors2(dataGridViewExtensionlogs, txtBoxDetailExtensionlogs);
                removeGenericErrors2(dataGridViewServiceActionlogs, txtBoxDetailServiceActionlogs);
                removeGenericErrors2(dataGridViewTradWebRequests, txtBoxDetailTradWebRequests);
                //removeGenericErrors2(dataGridViewIISDateTime, txtDetailIISlogs);
                //removeGenericErrors2(dataGridViewIISTimeTaken, txtDetailIISlogs);
                removeGenericErrors(dataGridViewWinAppEventViewer, 8, txtBoxDetailWinAppEventViewer);
                removeGenericErrors(dataGridViewWinSysEventViewer, 8, txtBoxDetailWinSysEventViewer);
                removeGenericErrors(dataGridViewWinSecEventViewer, 8, txtBoxDetailWinSecEventViewer);
                removeGenericErrors(dataGridViewAndroidlogs, 3, txtBoxDetailAndroidLogs);
                removeGenericErrors(dataGridViewiOSlogs, 3, txtBoxDetailiOSLogs);
                removeGenericErrors(dataGridViewServiceStudiologs, 3, txtBoxDetailServiceStudioLogs);
            }

            btnHighlight.Enabled = false;
        }

        private void highlightKnownErrors(DataGridView tableName, int columnIndex, string[] errorsList)
        {
            try
            {
                foreach (string error in errorsList)
                {
                    foreach (DataGridViewRow row in tableName.Rows)
                    {
                        if (row.Cells[columnIndex].Value.ToString().ToLower().Contains(error))
                        {
                            row.DefaultCellStyle.BackColor = Color.Yellow;
                        }
                    }
                }   
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        //Beginning of the Service Studio Report tab code
        private void dataGridViewServiceStudiologs_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
        //End of Service Studio Report tab code

        private void Form1_Load(object sender, EventArgs e)
        {
            btnFilter.Enabled = false;
            dateTimePicker1.Enabled = false;
            maskedTextBox1.Enabled = false;
            dateTimePicker2.Enabled = false;
            maskedTextBox2.Enabled = false;
            btnRemoveGarbage.Enabled = false;
            numericUpDownPercentage.Enabled = false;
            btnHighlight.Enabled = false;
            btnClearFilter.Enabled = false;
        }
        
    }
}
