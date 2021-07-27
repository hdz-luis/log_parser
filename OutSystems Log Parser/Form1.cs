using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Windows.Forms.DataVisualization.Charting;

namespace OutSystems_Log_Parser
{
    public partial class Form1 : Form
    {
        string fileSelected = "";
        string fullPath = "";
        string relativePath = "";
        string extension = "";
        char delimiters = '|';
        
        public Form1()
        {
            InitializeComponent();
        }

        //Beginning of the XLSX tab code
        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            try
            {
                //delete values at the time of browsing for another file
                txtDirectoryName.Text = "";
                txtDirectoryName2.Text = "";
                txtDirectoryName3.Text = "";
                txtDirectoryName4.Text = "";
                txtDirectoryName5.Text = "";
                txtFileName.Text = "";
                txtFileName2.Text = "";
                txtFileName3.Text = "";
                txtFileName4.Text = "";
                txtFileName5.Text = "";
                txtExtension.Text = "";
                txtExtension2.Text = "";
                txtExtension3.Text = "";
                txtExtension4.Text = "";
                txtExtension5.Text = "";
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox5.Text = "";
                dataGridView1.DataSource = null;
                dataGridView2.DataSource = null;
                dataGridView3.DataSource = null;
                dataGridView4.DataSource = null;
                dataGridView5.DataSource = null;

                //filter the file extensions and get sections from the absolute path
                openFileDialog1.Filter = "TXT Files (*.txt)|*.txt";
                openFileDialog1.Title = "Please select a file to analyze";
                openFileDialog1.FileName = null;
                
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fullPath = openFileDialog1.FileName;
                    fileSelected = Path.GetFileNameWithoutExtension(fullPath);
                    relativePath = Path.GetDirectoryName(fullPath);
                    extension = Path.GetExtension(fullPath);

                    if (fileSelected == "masterXLSXfile" && extension == ".txt")
                    {
                        TXTfile(fullPath, delimiters);

                        if (File.Exists(relativePath + "\\masterLOGfile_datetime.txt"))
                        {
                            TXTfile2(relativePath + "\\masterLOGfile_datetime.txt", delimiters);
                        }

                        if (File.Exists(relativePath + "\\masterLOGfile_timetaken.txt"))
                        {
                            TXTfile3(relativePath + "\\masterLOGfile_timetaken.txt", delimiters);
                        }

                        if (File.Exists(relativePath + "\\masterEVTXfile.txt"))
                        {
                            TXTfile4(relativePath + "\\masterEVTXfile.txt", delimiters);
                        }

                        if (File.Exists(relativePath + "\\masterTXTfile.txt"))
                        {
                            TXTfile5(relativePath + "\\masterTXTfile.txt", delimiters);
                        }
                    }
                    else if (fileSelected == "masterLOGfile_datetime" && extension == ".txt")
                    {

                        TXTfile2(fullPath, delimiters);

                        if (File.Exists(relativePath + "\\masterXLSXfile.txt"))
                        {
                            TXTfile(relativePath + "\\masterXLSXfile.txt", delimiters);
                        }

                        if (File.Exists(relativePath + "\\masterLOGfile_timetaken.txt"))
                        {
                            TXTfile3(relativePath + "\\masterLOGfile_timetaken.txt", delimiters);
                        }

                        if (File.Exists(relativePath + "\\masterEVTXfile.txt"))
                        {
                            TXTfile4(relativePath + "\\masterEVTXfile.txt", delimiters);
                        }

                        if (File.Exists(relativePath + "\\masterTXTfile.txt"))
                        {
                            TXTfile5(relativePath + "\\masterTXTfile.txt", delimiters);
                        }
                    }
                    else if (fileSelected == "masterLOGfile_timetaken" && extension == ".txt")
                    {

                        TXTfile3(fullPath, delimiters);

                        if (File.Exists(relativePath + "\\masterXLSXfile.txt"))
                        {
                            TXTfile(relativePath + "\\masterXLSXfile.txt", delimiters);
                        }

                        if (File.Exists(relativePath + "\\masterLOGfile_datetime.txt"))
                        {
                            TXTfile2(relativePath + "\\masterLOGfile_datetime.txt", delimiters);
                        }

                        if (File.Exists(relativePath + "\\masterEVTXfile.txt"))
                        {
                            TXTfile4(relativePath + "\\masterEVTXfile.txt", delimiters);
                        }

                        if (File.Exists(relativePath + "\\masterTXTfile.txt"))
                        {
                            TXTfile5(relativePath + "\\masterTXTfile.txt", delimiters);
                        }
                    }
                    else if (fileSelected == "masterEVTXfile" && extension == ".txt")
                    {

                        TXTfile4(fullPath, delimiters);

                        if (File.Exists(relativePath + "\\masterXLSXfile.txt"))
                        {
                            TXTfile(relativePath + "\\masterXLSXfile.txt", delimiters);
                        }

                        if (File.Exists(relativePath + "\\masterLOGfile_datetime.txt"))
                        {
                            TXTfile2(relativePath + "\\masterLOGfile_datetime.txt", delimiters);
                        }

                        if (File.Exists(relativePath + "\\masterLOGfile_timetaken.txt"))
                        {
                            TXTfile3(relativePath + "\\masterLOGfile_timetaken.txt", delimiters);
                        }

                        if (File.Exists(relativePath + "\\masterTXTfile.txt"))
                        {
                            TXTfile5(relativePath + "\\masterTXTfile.txt", delimiters);
                        }
                    }
                    else if (fileSelected == "masterTXTfile" && extension == ".txt")
                    {

                        TXTfile5(fullPath, delimiters);

                        if (File.Exists(relativePath + "\\masterXLSXfile.txt"))
                        {
                            TXTfile(relativePath + "\\masterXLSXfile.txt", delimiters);
                        }

                        if (File.Exists(relativePath + "\\masterLOGfile_datetime.txt"))
                        {
                            TXTfile2(relativePath + "\\masterLOGfile_datetime.txt", delimiters);
                        }

                        if (File.Exists(relativePath + "\\masterLOGfile_timetaken.txt"))
                        {
                            TXTfile3(relativePath + "\\masterLOGfile_timetaken.txt", delimiters);
                        }

                        if (File.Exists(relativePath + "\\masterEVTXfile.txt"))
                        {
                            TXTfile4(relativePath + "\\masterEVTXfile.txt", delimiters);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select any the \"master\" files generated by the Python script", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select any the \"master\" files generated by the Python script", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void TXTfile(string filePath, char splitter)
        {
            txtDirectoryName.Text = filePath.Split('\\')[3];
            txtFileName.Text = Path.GetFileNameWithoutExtension(filePath);
            txtExtension.Text = extension;

            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "LOG", "DATE_TIME", "SENT", "LAST_ERROR", "DURATION", "MESSAGE", "MESSAGE_TYPE",
                "STACK", "FROM", "TO", "SUBJECT", "CC", "BCC", "SCREEN", "SCREEN_TYPE", "MODULE_NAME", "APPLICATION_NAME",
                "APPLICATION_KEY", "NAME", "ACTION_NAME", "ACTION_TYPE", "ACCESS_MODE", "SOURCE", "ENTRYPOINT_NAME",
                "ENDPOINT", "EXECUTED_BY", "SERVER", "CLIENT_IP", "ESPACE_NAME", "ESPACE_ID", "USERNAME", "LOGIN_ID",
                "USER_ID", "SESSION_ID", "SESSION_REQUESTS", "SESSION_BYTES", "VIEW_STATE_BYTES", "EXTENSION_ID",
                "EXTENSION_NAME", "CYCLE", "CYCLIC_JOB_NAME", "CYCLIC_JOB_KEY", "SHOULD_HAVE_RUN_AT", "NEXT_RUN",
                "ENVIRONMENT_INFORMATION", "SIZE", "MS_IS_DN", "ERROR_ID", "MESSAGE_ID", "ACTIVITY", "EMAIL_DEFINITION",
                "STORE_CONTENT", "IS_TEST_EMAIL", "REQUEST_KEY", "ORIGINAL_REQUEST_KEY", "ID", "TENANT_ID"
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
                    dataGridView1.DataSource = dt;
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridView1.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    textBox1.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }
        //End of the XLSX tab code

        //Beginning of the IIS tab code
        private void TXTfile2(string filePath, char splitter)
        {
            txtDirectoryName2.Text = filePath.Split('\\')[3];
            txtFileName2.Text = Path.GetFileNameWithoutExtension(filePath);
            txtExtension2.Text = Path.GetExtension(filePath);

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
                    dataGridView2.DataSource = dt;
                }

                if (File.Exists(relativePath + "\\line_graph.png"))
                {
                    pictureBox1.ImageLocation = relativePath + "\\line_graph.png";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void TXTfile3(string filePath, char splitter)
        {
            txtDirectoryName3.Text = filePath.Split('\\')[3];
            txtFileName3.Text = Path.GetFileNameWithoutExtension(filePath);
            txtExtension3.Text = Path.GetExtension(filePath);

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
                    dataGridView3.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }
        //End of the IIS tab code

        //Beginning of the EVTX tab code
        private void TXTfile4(string filePath, char splitter)
        {
            txtDirectoryName4.Text = filePath.Split('\\')[3];
            txtFileName4.Text = Path.GetFileNameWithoutExtension(filePath);
            txtExtension4.Text = Path.GetExtension(filePath);

            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "LOG", "DATE_TIME", "LEVEL", "PROVIDER_NAME", "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID", "TASK", "KEYWORDS", "MESSAGE", "COMPUTER"
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
                    dataGridView4.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridView4.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridView4.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    textBox2.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }
        //End of the EVTX tab code

        //Beginning of the TXT tab code
        private void TXTfile5(string filePath, char splitter)
        {
            txtDirectoryName5.Text = filePath.Split('\\')[3];
            txtFileName5.Text = Path.GetFileNameWithoutExtension(filePath);
            txtExtension5.Text = Path.GetExtension(filePath);

            DataTable dt = new DataTable();

            string[] lines = System.IO.File.ReadAllLines(filePath).ToArray();

            string[] headerLabels = { "LOG", "DATE_TIME", "MESSAGE_TYPE", "ACTION_NAME", "MESSAGE"
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
                    dataGridView5.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridView5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridView5.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridView5.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    textBox3.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }
        //End of the TXT tab code

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridView2.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    textBox5.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridView3.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridView3.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    textBox5.Text = String.Join(Environment.NewLine, rowInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }
    }
}
