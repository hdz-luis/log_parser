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
using ClosedXML.Excel;

namespace OutSystems_Log_Parser
{
    public partial class Form1 : Form
    {
        string fullPath = "";
        char delimiters;
        string extension = "";
        string directory = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
        int totalDatagridviewRowsCount = 0;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            try
            {
                //delete values at the time of browsing for another file
                txtDirectoryName.Text = "";
                txtFileName.Text = "";
                txtExtension.Text = "";
                dataGridView1.DataSource = null;

                //filter the file extensions and get sections from the absolute path
                openFileDialog1.Filter = "XLSX Files (*.xlsx)|*.xlsx|CSV Files (*.csv)|*.csv|TXT Files (*.txt)|*.txt|LOG Files (*.log)|*.log";
                openFileDialog1.Title = "Please select a file to analyze";
                openFileDialog1.FileName = null;
                openFileDialog1.ShowDialog();
                fullPath = openFileDialog1.FileName;
                extension = Path.GetExtension(fullPath);

                if (extension == ".txt" || extension == ".log" || extension == ".csv" || extension == ".xlsx")
                {
                    txtDirectoryName.Text = fullPath.Split('\\')[3];
                    txtFileName.Text = Path.GetFileNameWithoutExtension(fullPath);
                    txtExtension.Text = extension;
                }
                else
                {
                    MessageBox.Show("Please select a .xlsx, .csv, a .txt, or a .log file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (extension == ".txt" || extension == ".log")
                {
                    //make unrelated data invisible
                    label3.Visible = false;
                    txtBoxSearchValue.Visible = false;
                    btnSearchValue.Visible = false;
                    txtBoxSearchValue.Text = "";

                    //for IIS logs, the delimiter is always one space
                    delimiters = ' ';
                    TXTLOGfile(fullPath, delimiters);
                }
                else if (extension == ".csv")
                {
                    //make unrelated data invisible
                    linkLabel1.Visible = false;
                    linkLabel2.Visible = false;
                    linkLabel3.Visible = false;
                    textBox1.Visible = false;
                    textBox2.Visible = false;
                    label1.Visible = false;
                    label2.Visible = false;
                    txtBoxHTTPCode.Visible = false;
                    txtBoxWindowsErrorCodes.Visible = false;
                    txtBoxHTTPCode.Text = "";
                    txtBoxWindowsErrorCodes.Text = "";
                    btnExportTXTFile.Visible = false;

                    //for files exported from Microsoft Excel spreadsheets, always use the pipe as the delimiter
                    delimiters = '|';
                    CSVfile(fullPath, delimiters);
                }
                else if (extension == ".xlsx")
                {
                    //make unrelated data invisible
                    linkLabel1.Visible = false;
                    linkLabel2.Visible = false;
                    linkLabel3.Visible = false;
                    textBox1.Visible = false;
                    textBox2.Visible = false;
                    label1.Visible = false;
                    label2.Visible = false;
                    txtBoxHTTPCode.Visible = false;
                    txtBoxWindowsErrorCodes.Visible = false;
                    txtBoxHTTPCode.Text = "";
                    txtBoxWindowsErrorCodes.Text = "";
                    btnExportTXTFile.Visible = false;

                    XLSXfile(fullPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void XLSXfile(string filePath)
        {
            try
            {
                //Open the Excel file using ClosedXML.
                using (XLWorkbook workBook = new XLWorkbook(filePath))
                {
                    //Read the first Sheet from Excel file.
                    IXLWorksheet workSheet = workBook.Worksheet(1);

                    //Create a new DataTable.
                    DataTable dt = new DataTable();

                    //Loop through the Worksheet rows.
                    bool firstRow = true;
                    foreach (IXLRow row in workSheet.Rows())
                    {
                        //Use the first row to add columns to DataTable.
                        if (firstRow)
                        {
                            foreach (IXLCell cell in row.Cells())
                            {
                                dt.Columns.Add(cell.Value.ToString());
                            }
                            firstRow = false;
                        }
                        else
                        {
                            //Add rows to DataTable.
                            dt.Rows.Add();
                            int i = 0;
                            foreach (IXLCell cell in row.Cells())
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                i++;
                            }
                        }

                        dataGridView1.DataSource = dt;
                    }
                }

                //display the relevant fields to use
                label3.Visible = true;
                txtBoxSearchValue.Visible = true;
                btnSearchValue.Visible = true;

                //so far, only the error logs seem to have the relevant data for troubleshooting purposes
                if (txtFileName.Text.Contains("ErrorLog"))
                {
                    string exportedFilesFolder = directory + "\\exported_files\\";

                    if (!Directory.Exists(exportedFilesFolder))
                    {
                        Directory.CreateDirectory(exportedFilesFolder);
                    }

                    //searching for keywords in the MESSAGE field from the error logs
                    string[] keywords = { "invalid", "corrupt", "roll", "method not found", "cannot read", "mismatch", "task", "terminate", "fatal", "query", "email", "null", "access denied", "connection", "environment health", "refuse", "does not exist", "missing", "unexpected", "escape", "update", "undefined", "cannot insert", "listening" };
                    List<string> foundKeywords = new List<string>();
                    int tempDatagridviewRowsCount = 0;
                    foreach (string keyword in keywords)
                    {
                        string xlsxFile = exportedFilesFolder + txtFileName.Text + "_" + keyword + ".xlsx";
                        (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("MESSAGE LIKE '%{0}%'", keyword);
                        tempDatagridviewRowsCount = dataGridView1.Rows.Count;

                        //if something was found, proceed
                        if (tempDatagridviewRowsCount > 0)
                        {
                            foundKeywords.Add(keyword);

                            //Creating DataTable
                            DataTable dt = new DataTable();

                            //Adding the Columns
                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                dt.Columns.Add(column.HeaderText, column.ValueType);
                            }

                            //Adding the Rows
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                dt.Rows.Add();
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    dt.Rows[dt.Rows.Count - 1][cell.ColumnIndex] = cell.Value.ToString();
                                }
                            }

                            //Exporting to Excel
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                wb.Worksheets.Add(dt, keyword);
                                wb.SaveAs(xlsxFile);
                            }
                        }
                    }

                    //display the keywords found in the right-hand side panel
                    if (foundKeywords.Count > 0)
                    {
                        textBox1.Visible = true;
                        foreach (string k in foundKeywords)
                        {
                            textBox1.Text = String.Join(Environment.NewLine, foundKeywords);
                        }
                        (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
                        MessageBox.Show("Some data has been already exported to the \"exported_files\" folder", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void CSVfile(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            //read the content from the Excel spreadsheet converted to a .csv file
            string[] lines = System.IO.File.ReadAllLines(filePath);
            string[] headerLabels;
            List<string> newLines = new List<string>();

            string firstLine = "";
            int colsExpected = 0;

            try
            {
                if (lines.Length > 0)
                {
                    //first line to create header
                    firstLine = lines[0];
                    headerLabels = firstLine.Split(splitter);
                    foreach (string headerWord in headerLabels)
                    {
                        dt.Columns.Add(new DataColumn(headerWord));
                    }

                    colsExpected = dt.Columns.Count;

                    //for removing line breaks in data
                    for (int i = 1; i < lines.Length; i++)
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

                //display the relevant fields to use
                label3.Visible = true;
                txtBoxSearchValue.Visible = true;
                btnSearchValue.Visible = true;

                //so far, only the error logs seem to have the relevant data for troubleshooting purposes
                if (txtFileName.Text.Contains("ErrorLog"))
                {
                    string exportedFilesFolder = directory + "\\exported_files\\";

                    if (!Directory.Exists(exportedFilesFolder))
                    {
                        Directory.CreateDirectory(exportedFilesFolder);
                    }

                    //searching for keywords in the MESSAGE field from the error logs
                    string[] keywords = { "invalid", "corrupt", "roll", "method not found", "cannot read", "mismatch", "task", "terminate", "fatal", "query", "email", "null", "access denied", "connection", "environment health", "refuse", "does not exist", "missing", "unexpected", "escape", "update", "undefined", "cannot insert", "listening" };
                    List<string> foundKeywords = new List<string>();
                    int tempDatagridviewRowsCount = 0;
                    foreach (string keyword in keywords)
                    {
                        string csvFile = exportedFilesFolder + txtFileName.Text + "_" + keyword + ".csv";
                        (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("MESSAGE LIKE '%{0}%'", keyword);
                        tempDatagridviewRowsCount = dataGridView1.Rows.Count;

                        //if something was found, proceed
                        if (tempDatagridviewRowsCount > 0)
                        {
                            foundKeywords.Add(keyword);

                            //This line of code creates a csv file for the data export.
                            StreamWriter exportFile = new StreamWriter(csvFile);
                            string eLine = "";

                            //This for loop loops through each row in the table
                            for (int r = 0; r <= dataGridView1.Rows.Count - 1; r++)
                            {
                                //This for loop loops through each column, and the row number
                                //is passed from the for loop above.
                                for (int c = 0; c <= dataGridView1.Columns.Count - 1; c++)
                                {
                                    eLine = eLine + dataGridView1.Rows[r].Cells[c].Value;
                                    if (c != dataGridView1.Columns.Count - 1)
                                    {
                                        //Add a text delimiter in order
                                        //to separate each field in the csv file.
                                        eLine = eLine + "|";
                                    }
                                }
                                //The exported text is written to the csv file, one line at a time.
                                exportFile.WriteLine(eLine);
                                eLine = "";
                            }
                            exportFile.Close();
                        }
                    }

                    //display the keywords found in the right-hand side panel
                    if (foundKeywords.Count > 0)
                    {
                        textBox1.Visible = true;
                        foreach (string k in foundKeywords)
                        {
                            textBox1.Text = String.Join(Environment.NewLine, foundKeywords);
                        }
                        (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
                        MessageBox.Show("Some data has been already exported to the \"exported_files\" folder", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void TXTLOGfile(string filePath, char splitter)
        {
            DataTable dt = new DataTable();

            //skipping the first 4 lines from the IIS logs since they contain irrelevant information for our purposes
            string[] lines = System.IO.File.ReadAllLines(filePath).Skip(4).ToArray();

            //since the headers are going to be removed from the IIS logs, insert them manually in the table
            string[] headerLabels = { "Date_date", "Time_time", "ServerIPAddress_sip", "Method_csmethod", "URIStem_csuristem", "URIQuery_csuriquery", "ServerPort_sport", "Username_csusername", "ClientIPAddress_cip", "UserAgent_csUserAgent", "Referrer_csReferrer", "HTTPProtocolStatus_scstatus", "HTTPProtocolSubstatus_scsubstatus", "WindowsSystemErrorCode_scwin32status", "TimeTakenMS_timetaken" };
            List<string> newLines = new List<string>();
            List<string> httpProtocol = new List<string>();
            List<string> winSysErrorCode = new List<string>();

            int colsExpected = 0;

            //index value of the HTTP codes field in the IIS logs
            int indexHttpProtocolCol = 11;

            //index value of the Windows System error codes field in the IIS logs
            int indexWinSysErrorCodeCol = 13;

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
                        //some IIS logs have the irrelevant headers more than once in the same file
                        if (!lines[i].StartsWith("#"))
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

                //saving the total number of rows for comparison later on in the program
                totalDatagridviewRowsCount = dt.Rows.Count;

                //extract the values from the HTTP codes field
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    httpProtocol.Add(row.Cells[indexHttpProtocolCol].Value.ToString());
                }

                //using the long datatype since some values are longer than the usual
                List<long> intHttpProtocol = httpProtocol.ConvertAll<long>(Convert.ToInt64);

                //removing the repeated values from the list
                List<long> httpDistinctProtocol = intHttpProtocol.Distinct().ToList();
                httpDistinctProtocol.Sort();

                //displaying the HTTP codes in the right-hand side panel
                foreach (int h in httpDistinctProtocol)
                {
                    textBox1.Text = String.Join(Environment.NewLine, httpDistinctProtocol);
                }

                //extracting the values from the Windows System error codes field in the IIS logs
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    winSysErrorCode.Add(row.Cells[indexWinSysErrorCodeCol].Value.ToString());
                }

                //using the long datatype to avoid problems with long values
                List<long> intWinSysErrorCode = winSysErrorCode.ConvertAll<long>(Convert.ToInt64);

                //removing repeated values
                List<long> winSysDistinctErrorCode = intWinSysErrorCode.Distinct().ToList();
                winSysDistinctErrorCode.Sort();

                //displaying the Windows System error codes in the right-hand side panel
                foreach (int w in winSysDistinctErrorCode)
                {
                    textBox2.Text = String.Join(Environment.NewLine, winSysDistinctErrorCode);
                }

                //making relevant fields in the interface visible
                linkLabel1.Visible = true;
                linkLabel2.Visible = true;
                linkLabel3.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                txtBoxHTTPCode.Visible = true;
                txtBoxWindowsErrorCodes.Visible = true;

                string exportedFilesFolder = directory + "\\exported_files\\";

                if (!Directory.Exists(exportedFilesFolder))
                {
                    Directory.CreateDirectory(exportedFilesFolder);
                }

                //searching for the HTTP and Windows error codes in the sc-status and sc-win32-status fields from the IIS logs
                string[] httpErrorCodes = { "102", "202", "203", "204", "205", "300", "301", "302", "305", "306", "308", "400", "401", "403", "404", "405", "406", "407", "408", "409", "500", "501", "502", "503", "504", "505", "506", "507", "508" };
                string[] windowsErrorCodes = { "1", "2", "3", "4", "5", "8", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "24", "25", "26", "28", "29", "30", "31", "32", "33", "36", "39", "50", "51", "52", "53",
                                                "54", "55", "56", "57", "58", "59", "60", "61", "62", "63", "64", "65", "66", "67", "69", "70", "71", "72", "82", "84", "85", "86", "87", "88", "89", "107", "108", "109", "110", "111", "112", "118",
                                                "119", "120", "123", "126", "127", "144", "145", "147", "148", "150", "155", "156", "159", "160", "161", "164", "170", "183", "196", "197", "199", "203", "206", "208", "215", "220", "221", "240",
                                                "300", "301", "302", "303", "306", "307", "310", "315", "316", "318", "319", "320", "321", "323", "330", "331", "334", "335", "336", "337", "350", "351", "352", "353", "400", "402" };

                //split the values from textbox1 and textbox2
                string[] txtbx1 = new string[] { "\r\n" };
                string[] myTtxtbx1 = textBox1.Text.Split(txtbx1, StringSplitOptions.RemoveEmptyEntries);
                string[] txtbx2 = new string[] { "\r\n" };
                string[] myTtxtbx2 = textBox2.Text.Split(txtbx1, StringSplitOptions.RemoveEmptyEntries);

                int tempDatagridviewRowsCount = 0;
                int tempDatagridviewRowsCount2 = 0;

                foreach (string httpErr in httpErrorCodes)
                {
                    foreach (string txtbxln1 in myTtxtbx1)
                    {
                        if (httpErr == txtbxln1)
                        {
                            string txtFile = exportedFilesFolder + "IIS_HTTP_Code_" + httpErr + ".txt";

                            //filter the content from the table based on the HTTP code
                            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("HTTPProtocolStatus_scstatus LIKE '%{0}%'", httpErr);
                            tempDatagridviewRowsCount = dataGridView1.Rows.Count;

                            //if something was found, proceed
                            if (tempDatagridviewRowsCount > 0)
                            {
                                //This line of code creates a txt file for the data export.
                                StreamWriter exportFile = new StreamWriter(txtFile);
                                string eLine = "";

                                //This for loop loops through each row in the table
                                for (int r = 0; r <= dataGridView1.Rows.Count - 1; r++)
                                {
                                    //This for loop loops through each column, and the row number
                                    //is passed from the for loop above.
                                    for (int c = 0; c <= dataGridView1.Columns.Count - 1; c++)
                                    {
                                        eLine = eLine + dataGridView1.Rows[r].Cells[c].Value;
                                        if (c != dataGridView1.Columns.Count - 1)
                                        {
                                            //Add a text delimiter in order
                                            //to separate each field in the txt file.
                                            eLine = eLine + " ";
                                        }
                                    }
                                    //The exported text is written to the txt file, one line at a time.
                                    exportFile.WriteLine(eLine);
                                    eLine = "";
                                }
                                exportFile.Close();
                            }
                        }
                    }
                }

                foreach (string windowsErr in windowsErrorCodes)
                {
                    foreach (string txtbxln2 in myTtxtbx2)
                    {
                        if (windowsErr == txtbxln2)
                        {
                            string txtFile2 = exportedFilesFolder + "IIS_Windows_Error_Code_" + windowsErr + ".txt";

                            //filter the content from the table based on the HTTP code
                            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("WindowsSystemErrorCode_scwin32status LIKE '%{0}%'", windowsErr);
                            tempDatagridviewRowsCount2 = dataGridView1.Rows.Count;

                            //if something was found, proceed
                            if (tempDatagridviewRowsCount2 > 0)
                            {
                                //This line of code creates a txt file for the data export.
                                StreamWriter exportFile2 = new StreamWriter(txtFile2);
                                string eLine2 = "";

                                //This for loop loops through each row in the table
                                for (int r2 = 0; r2 <= dataGridView1.Rows.Count - 1; r2++)
                                {
                                    //This for loop loops through each column, and the row number
                                    //is passed from the for loop above.
                                    for (int c2 = 0; c2 <= dataGridView1.Columns.Count - 1; c2++)
                                    {
                                        eLine2 = eLine2 + dataGridView1.Rows[r2].Cells[c2].Value;
                                        if (c2 != dataGridView1.Columns.Count - 1)
                                        {
                                            //Add a text delimiter in order
                                            //to separate each field in the txt file.
                                            eLine2 = eLine2 + " ";
                                        }
                                    }
                                    //The exported text is written to the txt file, one line at a time.
                                    exportFile2.WriteLine(eLine2);
                                    eLine2 = "";
                                }
                                exportFile2.Close();
                            }
                        }
                    }
                }
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
                MessageBox.Show("Some data has been already exported to the \"exported_files\" folder", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void Form1_Load(object sender, EventArgs e)
        {
            //hide everything that is irrelevant at the time of launching the application
            linkLabel1.Visible = false;
            linkLabel2.Visible = false;
            linkLabel3.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            txtBoxHTTPCode.Visible = false;
            txtBoxWindowsErrorCodes.Visible = false;
            btnExportTXTFile.Visible = false;
            label3.Visible = false;
            txtBoxSearchValue.Visible = false;
            btnSearchValue.Visible = false;
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

        private void btnSearchValue_MouseEnter(object sender, EventArgs e)
        {
            btnSearchValue.BackColor = Color.Black;
        }

        private void btnSearchValue_MouseLeave(object sender, EventArgs e)
        {
            btnSearchValue.BackColor = SystemColors.ControlLight;
        }

        private void btnExportTXTFile_MouseEnter(object sender, EventArgs e)
        {
            btnExportTXTFile.BackColor = Color.Black;
        }

        private void btnExportTXTFile_MouseLeave(object sender, EventArgs e)
        {
            btnExportTXTFile.BackColor = SystemColors.ControlLight;
        }

        private void txtBoxHTTPCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int currentDatagridviewRowsCount = 0;

                //make sure the textbox searching for Windows errors is empty
                this.txtBoxWindowsErrorCodes.TextChanged -= txtWindowsErrorCodes_TextChanged;
                this.txtBoxWindowsErrorCodes.Text = "";
                this.txtBoxWindowsErrorCodes.TextChanged += txtWindowsErrorCodes_TextChanged;
                
                //filter the content from the table based on the HTTP code
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("HTTPProtocolStatus_scstatus LIKE '%{0}%'", txtBoxHTTPCode.Text);

                //save the filtered count of rows for comparison
                currentDatagridviewRowsCount = dataGridView1.Rows.Count;

                if(currentDatagridviewRowsCount > 0)
                {
                    btnExportTXTFile.Visible = true;

                    if (txtBoxHTTPCode.Text == "" && txtBoxWindowsErrorCodes.Text == "")
                    {
                        if (currentDatagridviewRowsCount == totalDatagridviewRowsCount)
                        {
                            btnExportTXTFile.Visible = false;
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

        private void txtWindowsErrorCodes_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int currDatagridviewRowsCount = 0;

                //make sure the textbox used to search for the HTTP codes is empty
                this.txtBoxHTTPCode.TextChanged -= txtBoxHTTPCode_TextChanged;
                this.txtBoxHTTPCode.Text = "";
                this.txtBoxHTTPCode.TextChanged += txtBoxHTTPCode_TextChanged;

                //filter the table based on the Windows errors
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("WindowsSystemErrorCode_scwin32status LIKE '%{0}%'", txtBoxWindowsErrorCodes.Text);

                //save the filtered count of rows for comparison
                currDatagridviewRowsCount = dataGridView1.Rows.Count;

                if (currDatagridviewRowsCount > 0)
                {
                    btnExportTXTFile.Visible = true;

                    if (txtBoxHTTPCode.Text == "" && txtBoxWindowsErrorCodes.Text == "")
                    {
                        if (currDatagridviewRowsCount == totalDatagridviewRowsCount)
                        {
                            btnExportTXTFile.Visible = false;
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

        private void btnExportTXTFile_Click(object sender, EventArgs e)
        {
            //export the text files in the current working directory and append either the HTTP code or the Windows error at the end of the file
            string exportedFilesFolder = directory + "\\exported_files\\";

            if (!Directory.Exists(exportedFilesFolder))
            {
                Directory.CreateDirectory(exportedFilesFolder);
            }

            string txtFile = "";

            if (string.IsNullOrEmpty(txtBoxHTTPCode.Text) && string.IsNullOrEmpty(txtBoxWindowsErrorCodes.Text))
            {
                MessageBox.Show("Filter the data by using a specific HTTP Code or a Windows Error Code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(txtBoxHTTPCode.Text) && !string.IsNullOrEmpty(txtBoxWindowsErrorCodes.Text))
            {
                txtFile = exportedFilesFolder + "IIS_Windows_Error_Code_" + txtBoxWindowsErrorCodes.Text + ".txt";
            }
            else if(string.IsNullOrEmpty(txtBoxWindowsErrorCodes.Text) && !string.IsNullOrEmpty(txtBoxHTTPCode.Text))
            {
                txtFile = exportedFilesFolder + "IIS_HTTP_Code_" + txtBoxHTTPCode.Text + ".txt";
            }

            try
            {
                //This line of code creates a text file for the data export.
                StreamWriter exportFile = new StreamWriter(txtFile);
                string eLine = "";

                //This for loop loops through each row in the table
                for (int r = 0; r <= dataGridView1.Rows.Count - 1; r++)
                {
                    //This for loop loops through each column, and the row number
                    //is passed from the for loop above.
                    for (int c = 0; c <= dataGridView1.Columns.Count - 1; c++)
                    {
                        eLine = eLine + dataGridView1.Rows[r].Cells[c].Value;
                        if (c != dataGridView1.Columns.Count - 1)
                        {
                            //Add a text delimiter in order
                            //to separate each field in the text file.
                            eLine = eLine + " ";
                        }
                    }
                    //The exported text is written to the text file, one line at a time.
                    exportFile.WriteLine(eLine);
                    eLine = "";
                }

                exportFile.Close();
                MessageBox.Show("Exported the data to the following file:" + Environment.NewLine + txtFile, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtBoxWindowsErrorCodes.Text = "";
                txtBoxHTTPCode.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void btnSearchValue_Click(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(txtBoxSearchValue.Text))
                {
                    MessageBox.Show("You need to enter a value to search for", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //search for a value anywhere on the table, meaning, any row and any column
                    string cellValue = "";
                    string rowValues = "";
                    string rowValues2 = "";

                    //save the whole row data to be exported later on to the csv file
                    List<string> rowValuesList = new List<string>();
                    List<string> rowValuesList2 = new List<string>();

                    for (int r = 0; r <= dataGridView1.Rows.Count - 1; r++)
                    {
                        for (int c = 0; c <= dataGridView1.Columns.Count - 1; c++)
                        {
                            cellValue = dataGridView1.Rows[r].Cells[c].Value.ToString();
                            if (c != dataGridView1.Columns.Count - 1)
                            {
                                if (cellValue.ToLower().Contains(txtBoxSearchValue.Text.ToLower()))
                                {
                                    DataGridViewRow dataRow = dataGridView1.Rows[r];
                                    for (int col = 0; col <= dataGridView1.Columns.Count - 1; col++)
                                    {
                                        if (col < dataGridView1.Columns.Count)
                                        {
                                            rowValues = rowValues + dataRow.Cells[col].Value.ToString() + "|";
                                            rowValues2 = rowValues2 + dataRow.Cells[col].Value.ToString() + "|";
                                        }
                                        else
                                        {
                                            rowValues = rowValues + dataRow.Cells[col].Value.ToString();
                                            rowValues2 = rowValues2 + dataRow.Cells[col].Value.ToString();
                                        }
                                    }
                                    //make sure the row data is not repeated before exporting it to the csv file
                                    if(rowValuesList.Contains(rowValues) == false)
                                    {
                                        rowValuesList.Add(rowValues);
                                    }

                                    if (rowValuesList2.Contains(rowValues2) == false)
                                    {
                                        rowValuesList2.Add(rowValues2);
                                    }

                                    rowValues = "";
                                    rowValues2 = "";
                                }
                            }
                        }
                    }

                    //use the current working directory to create the file with the data
                    string exportedFilesFolder = directory + "\\exported_files\\";

                    if (!Directory.Exists(exportedFilesFolder))
                    {
                        Directory.CreateDirectory(exportedFilesFolder);
                    }

                    //check the file extension to determine the exporting process
                    if (txtExtension.Text == ".xlsx")
                    {
                        string xlsxFile = exportedFilesFolder + txtFileName.Text + "_" + txtBoxSearchValue.Text + ".xlsx";

                        //columns in the Excel spreadsheet
                        string[] alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                        string[] dataFields = rowValuesList2.ToArray();
                        var wb = new XLWorkbook();
                        var ws = wb.Worksheets.Add(txtBoxSearchValue.Text);
                        
                        for (int r = 0; r < dataFields.Length; r++)
                        {
                            //split the values from the row into columns
                            string[] dataCols = dataFields[r].Split('|');

                            for (int c = 0; c <= dataGridView1.Columns.Count; c++)
                            {
                                //letter of the column in the Excel spreadsheet + row number, insert the value of each column on each cell
                                int val = r + 1;
                                ws.Cell(alphabet[c] + val.ToString()).Value = dataCols[c];
                            }
                        }

                        wb.SaveAs(xlsxFile);
                        txtBoxSearchValue.Text = "";
                        MessageBox.Show("Exported the data to the following file:" + Environment.NewLine + xlsxFile, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if(txtExtension.Text == ".csv")
                    {
                        string csvFile = exportedFilesFolder + txtFileName.Text + "_" + txtBoxSearchValue.Text + ".csv";
                        StreamWriter exportCSVFile = new StreamWriter(csvFile);
                        for (int i = 0; i < rowValuesList.Count; i++)
                        {
                            exportCSVFile.WriteLine(rowValuesList[i]);
                        }
                        exportCSVFile.Close();
                        txtBoxSearchValue.Text = "";
                        MessageBox.Show("Exported the data to the following file:" + Environment.NewLine + csvFile, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
