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

namespace OutSystems_Log_Parser
{
    public partial class Form2 : Form
    {
        string fieldName = "";
        string iisFieldName = "";
        string description = "";

        public Form2()
        {
            InitializeComponent();
        }

        // Instantiate a new 2D string array.
        //the field name to search for, the field name as it is found in the IIS logs, and its description
        string[,] array = new string[22, 3]
        {
            {"Date", "date", "The date on which the activity occurred."},
            {"Time", "time", "The time, in coordinated universal time (UTC), at which the activity occurred."},
            {"Client IP Address", "c-ip", "The IP address of the client that made the request."},
            {"User Name", "cs-username", "The name of the authenticated user who accessed your server. Anonymous users are indicated by a hyphen."},
            {"Service Name and Instance Number", "s-sitename", "The Internet service name and instance number that was running on the client."},
            {"Server Name", "s-computername", "The name of the server on which the log file entry was generated."},
            {"Server IP Address", "s-ip", "The IP address of the server on which the log file entry was generated."},
            {"Server Port", "s-port", "The server port number that is configured for the service."},
            {"Method", "cs-method", "The requested action, for example, a GET method."},
            {"URI Stem", "cs-uri-stem", "The target of the action, for example, Default.htm."},
            {"URI Query", "cs-uri-query", "The query, if any, that the client was trying to perform. A Universal Resource Identifier (URI) query is necessary only for dynamic pages."},
            {"HTTP Protocol Status", "sc-status", "The HTTP status code."},
            {"Windows System Error Code", "sc-win32-status", "The Windows status code."},
            {"Bytes Sent", "sc-bytes", "The number of bytes that the server sent."},
            {"Bytes Received", "cs-bytes", "The number of bytes that the server received."},
            {"Time Taken", "time-taken", "The length of time that the action took, in milliseconds."},
            {"Protocol Version", "cs-version", "The protocol version —HTTP or FTP —that the client used."},
            {"Host", "cs-host", "The host header name, if any."},
            {"User Agent", "cs(User-Agent)", "The browser type that the client used."},
            {"Cookie", "cs(Cookie)", "The content of the cookie sent or received, if any."},
            {"Referrer", "cs(Referrer)", "The site that the user last visited. This site provided a link to the current site."},
            {"HTTP Protocol Substatus", "sc-substatus", "The substatus error code."}
        };

        private void listViewFields_Click(object sender, EventArgs e)
        {
            //when clicking on the table, make sure to pass those values from the selected row to the textboxes
            fieldName = listViewFields.SelectedItems[0].SubItems[0].Text;
            iisFieldName = listViewFields.SelectedItems[0].SubItems[1].Text;
            description = listViewFields.SelectedItems[0].SubItems[2].Text;

            txtBoxFieldName.Text = fieldName;
            txtBoxIISFieldName.Text = iisFieldName;
            txtBoxDescription.Text = description;
        }

        private void txtBoxSearchFieldName_TextChanged(object sender, EventArgs e)
        {
            //make sure there are at least two letters before starting to search for similar field names
            if (txtBoxSearchFieldName.Text.Length <= 1)
            {
                listViewFields.Clear();
                txtBoxDescription.Clear();
                txtBoxFieldName.Clear();
                txtBoxIISFieldName.Clear();
            }
            else if (txtBoxSearchFieldName.Text.Length > 1)
            {
                listViewFields.Clear();
                listViewFields.View = View.Details;
                listViewFields.Columns.Add("Field Name");
                listViewFields.Columns.Add("IIS Field Name");
                listViewFields.Columns.Add("Description");
                listViewFields.GridLines = true;

                for (int i = 0; i < array.GetLength(0); i++)
                {
                    if (array[i, 0].ToLower().Contains(txtBoxSearchFieldName.Text.ToLower()))
                    {
                        listViewFields.Items.Add(new ListViewItem(new string[] { array[i, 0], array[i, 1], array[i, 2] }));
                    }
                }
            }
        }
    }
}
