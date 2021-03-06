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
        string category = "";
        string categoryName = "";
        string fullPath = "";
        string[] filesPaths;
        string[] lines;
        string[] dataFields;
        string[] dataCols;
        int colsExpected = 0;
        int colIndex = 0;
        int colLengthDiff;
        string[] lineOfContents;
        string[] knownErrors_Errorlogs;
        string[] knownErrors_Generallogs;
        string[] knownErrors_WinAppEventViewer;
        string[] knownErrors_WinSysEventViewer;
        string[] knownErrors_AndroidiOSlogs;
        string[] knownErrors_ServiceStudiologs;
        string[] knownErrors_GeneralTXTlogs;
        string fileName = "";
        char delimiters = '|';
        int totalRowsCount;
        string myMessage = "";
        int myMessageCount;
        string outputCSVfile = "";
        int currentRow;
        string rowValues = "";
        string rowInfo = "";
        string error_message = "";
        string report = "";
        string reportName = "";
        double percentageMessageCount;
        double roundedPercentageMessageCount;
        double sixteenthOfTotal;
        double roundedSixteenthOfTotal;
        double eighthOfTotal;
        double roundedEighthOfTotal;
        double fourthOfTotal;
        double roundedFourthOfTotal;
        double halfOfTotal;
        double roundedHalfOfTotal;
        bool bool_removeGarbage = false;
        bool bool_removeGarbageSuccessful = false;
        bool bool_highlightError = false;
        bool bool_highlightErrorSuccessful_1 = false;
        bool bool_highlightErrorSuccessful_2 = false;
        bool bool_highlightErrorSuccessful_3 = false;
        bool bool_highlightErrorSuccessful_4 = false;
        bool bool_highlightErrorSuccessful_5 = false;
        bool bool_highlightErrorSuccessful_6 = false;
        bool bool_findKeyword = false;
        bool bool_findKeywordSuccessful_1 = false;
        bool bool_findKeywordSuccessful_2 = false;
        bool bool_findKeywordSuccessful_3 = false;
        bool bool_findKeywordSuccessful_4 = false;
        bool bool_findKeywordSuccessful_5 = false;
        bool bool_screenshotSuccessful = false;
        bool bool_datetimeFilterSuccessful = false;
        bool bool_fieldFilterSuccessful = false;
        int countFindKeyword = 0;
        int countFindKnownError = 0;
        List<string> keywordsSaved = new List<string>();
        List<string> listIssueCategory = new List<string>();
        List<string> categorySelected = new List<string>();
        Font myScreenshotFont = new Font("Times New Roman", 10);
        Color myScreenshotForeColor = Color.Gold;
        Color myScreenshotBackColor = Color.Black;

        delegate void TabControlClick(object sender, TabControlEventArgs e);

        TabControlClick tabPage1_MyClick;
        TabControlClick tabPage2_MyClick;
        TabControlClick tabPage3_MyClick;
        TabControlClick tabPage4_MyClick;
        TabControlClick tabPage5_MyClick;
        TabControlClick tabPage6_MyClick;
        TabControlClick tabPage7_MyClick;
        TabControlClick tabPage8_MyClick;
        TabControlClick tabPage9_MyClick;
        TabControlClick tabPage10_MyClick;
        TabControlClick tabPage11_MyClick;
        TabControlClick tabPage12_MyClick;
        TabControlClick tabPage13_MyClick;
        TabControlClick tabPage14_MyClick;
        TabControlClick tabPage15_MyClick;
        TabControlClick tabPage16_MyClick;
        TabControlClick tabPage17_MyClick;
        TabControlClick tabPage18_MyClick;
        TabControlClick tabPage19_MyClick;
        TabControlClick tabPage20_MyClick;
        TabControlClick tabPage21_MyClick;
        TabControlClick tabPage22_MyClick;
        TabControlClick tabPage23_MyClick;
        TabControlClick tabPage24_MyClick;
        TabControlClick tabPage25_MyClick;
        TabControlClick tabPage26_MyClick;
        TabControlClick tabPage27_MyClick;
        TabControlClick tabPage28_MyClick;
        TabControlClick tabPage29_MyClick;
        TabControlClick tabPage30_MyClick;
        TabControlClick tabPage31_MyClick;
        TabControlClick tabPage32_MyClick;
        TabControlClick tabPage33_MyClick;
        TabControlClick tabPage34_MyClick;
        TabControlClick tabPage35_MyClick;
        TabControlClick tabPage36_MyClick;
        TabControlClick tabPage37_MyClick;
        TabControlClick tabPage38_MyClick;
        TabControlClick tabPage39_MyClick;
        TabControlClick tabPage40_MyClick;
        TabControlClick tabPage41_MyClick;
        TabControlClick tabPage42_MyClick;
        TabControlClick tabPage43_MyClick;
        TabControlClick tabPage44_MyClick;
        TabControlClick tabPage45_MyClick;
        TabControlClick tabPage46_MyClick;
        TabControlClick tabPage47_MyClick;
        TabControlClick tabPage48_MyClick;
        TabControlClick tabPage49_MyClick;
        TabControlClick tabPage50_MyClick;
        TabControlClick tabPage51_MyClick;
        TabControlClick tabPage52_MyClick;
        TabControlClick tabPage53_MyClick;
        TabControlClick tabPage54_MyClick;
        TabControlClick tabPage55_MyClick;
        TabControlClick tabPage56_MyClick;
        TabControlClick tabPage57_MyClick;
        TabControlClick tabPage58_MyClick;
        TabControlClick tabPage59_MyClick;
        TabControlClick tabPage61_MyClick;
        TabControlClick tabPage62_MyClick;

        TabControlAction tabControlAction;

        TabControlEventArgs tabControlEventArgsTab1;
        TabControlEventArgs tabControlEventArgsTab2;
        TabControlEventArgs tabControlEventArgsTab3;
        TabControlEventArgs tabControlEventArgsTab4;
        TabControlEventArgs tabControlEventArgsTab5;
        TabControlEventArgs tabControlEventArgsTab6;
        TabControlEventArgs tabControlEventArgsTab7;
        TabControlEventArgs tabControlEventArgsTab8;
        TabControlEventArgs tabControlEventArgsTab9;
        TabControlEventArgs tabControlEventArgsTab10;
        TabControlEventArgs tabControlEventArgsTab11;
        TabControlEventArgs tabControlEventArgsTab12;
        TabControlEventArgs tabControlEventArgsTab13;
        TabControlEventArgs tabControlEventArgsTab14;
        TabControlEventArgs tabControlEventArgsTab15;
        TabControlEventArgs tabControlEventArgsTab16;
        TabControlEventArgs tabControlEventArgsTab17;
        TabControlEventArgs tabControlEventArgsTab18;
        TabControlEventArgs tabControlEventArgsTab19;
        TabControlEventArgs tabControlEventArgsTab20;
        TabControlEventArgs tabControlEventArgsTab21;
        TabControlEventArgs tabControlEventArgsTab22;
        TabControlEventArgs tabControlEventArgsTab23;
        TabControlEventArgs tabControlEventArgsTab24;
        TabControlEventArgs tabControlEventArgsTab25;
        TabControlEventArgs tabControlEventArgsTab26;
        TabControlEventArgs tabControlEventArgsTab27;
        TabControlEventArgs tabControlEventArgsTab28;
        TabControlEventArgs tabControlEventArgsTab29;
        TabControlEventArgs tabControlEventArgsTab30;
        TabControlEventArgs tabControlEventArgsTab31;
        TabControlEventArgs tabControlEventArgsTab32;
        TabControlEventArgs tabControlEventArgsTab33;
        TabControlEventArgs tabControlEventArgsTab34;
        TabControlEventArgs tabControlEventArgsTab35;
        TabControlEventArgs tabControlEventArgsTab36;
        TabControlEventArgs tabControlEventArgsTab37;
        TabControlEventArgs tabControlEventArgsTab38;
        TabControlEventArgs tabControlEventArgsTab39;
        TabControlEventArgs tabControlEventArgsTab40;
        TabControlEventArgs tabControlEventArgsTab41;
        TabControlEventArgs tabControlEventArgsTab42;
        TabControlEventArgs tabControlEventArgsTab43;
        TabControlEventArgs tabControlEventArgsTab44;
        TabControlEventArgs tabControlEventArgsTab45;
        TabControlEventArgs tabControlEventArgsTab46;
        TabControlEventArgs tabControlEventArgsTab47;
        TabControlEventArgs tabControlEventArgsTab48;
        TabControlEventArgs tabControlEventArgsTab49;
        TabControlEventArgs tabControlEventArgsTab50;
        TabControlEventArgs tabControlEventArgsTab51;
        TabControlEventArgs tabControlEventArgsTab52;
        TabControlEventArgs tabControlEventArgsTab53;
        TabControlEventArgs tabControlEventArgsTab54;
        TabControlEventArgs tabControlEventArgsTab55;
        TabControlEventArgs tabControlEventArgsTab56;
        TabControlEventArgs tabControlEventArgsTab57;
        TabControlEventArgs tabControlEventArgsTab58;
        TabControlEventArgs tabControlEventArgsTab59;
        TabControlEventArgs tabControlEventArgsTab61;
        TabControlEventArgs tabControlEventArgsTab62;

        public Form1(string scriptDirectory)
        {
            InitializeComponent();
            label8.Text = scriptDirectory;
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            btnBrowseFile.BackgroundImage = OutSystems_Log_Parser.Properties.Resources.undo_blue;
            toolTip1.SetToolTip(btnBrowseFile, "Undo all the changes.");

            try
            {
                countFindKeyword = 0;
                countFindKnownError = 0;

                bool_removeGarbage = false;
                bool_highlightError = false;
                bool_findKeyword = false;
                bool_removeGarbageSuccessful = false;
                bool_highlightErrorSuccessful_1 = false;
                bool_highlightErrorSuccessful_2 = false;
                bool_highlightErrorSuccessful_3 = false;
                bool_highlightErrorSuccessful_4 = false;
                bool_highlightErrorSuccessful_5 = false;
                bool_highlightErrorSuccessful_6 = false;
                bool_findKeywordSuccessful_1 = false;
                bool_findKeywordSuccessful_2 = false;
                bool_findKeywordSuccessful_3 = false;
                bool_findKeywordSuccessful_4 = false;
                bool_findKeywordSuccessful_5 = false;
                bool_screenshotSuccessful = false;
                bool_datetimeFilterSuccessful = false;
                bool_fieldFilterSuccessful = false;

                dateTimePicker1.Text = "";
                dateTimePicker2.Text = "";
                maskedTextBox1.Text = "";
                maskedTextBox2.Text = "";
                txtBoxKeyword.Text = "";

                maskedTextBox1.BackColor = SystemColors.Window;
                maskedTextBox2.BackColor = SystemColors.Window;
                txtBoxKeyword.BackColor = SystemColors.Window;

                numericUpDownPercentage.Value = 20;

                comBoxIssueCategory.SelectedIndex = -1;
                comBoxField.SelectedIndex = -1;
                comBoxFilterField.SelectedIndex = -1;
                comBoxReport.SelectedIndex = -1;

                comBoxIssueCategory.Items.Clear();
                listIssueCategory.Clear();
                listIssueCategory.Add("Building Mobile App");
                listIssueCategory.Add("Compilation");
                listIssueCategory.Add("Content Security Policy (CSP)");
                listIssueCategory.Add("Database");
                listIssueCategory.Add("Logic");
                listIssueCategory.Add("Network");
                comBoxIssueCategory.Items.AddRange(listIssueCategory.ToArray());
                comBoxIssueCategory.Enabled = true;

                comBoxField.Items.Clear();
                keywordsSaved.Clear();
                categorySelected.Clear();

                clearTextboxes(txtBoxDetailErrorLogs);
                clearTextboxes(txtBoxDetailGenerallogs);
                clearTextboxes(txtBoxDetailsSlowSQLlogs);
                clearTextboxes(txtBoxDetailsSlowExtensionlogs);
                clearTextboxes(txtBoxDetailIntegrationlogs);
                clearTextboxes(txtBoxDetailsIntWebServiceslogs);
                clearTextboxes(txtBoxDetailScreenRequestslogs);
                clearTextboxes(txtBoxDetailScrReqScreenlogs);
                clearTextboxes(txtBoxDetailTimerlogs);
                clearTextboxes(txtBoxDetailsTimerTimerslogs);
                clearTextboxes(txtBoxDetailEmaillogs);
                clearTextboxes(txtBoxDetailsEmailEmailslogs);
                clearTextboxes(txtBoxDetailExtensionlogs);
                clearTextboxes(txtBoxDetailExtExtensionlogs);
                clearTextboxes(txtBoxDetailServiceActionlogs);
                clearTextboxes(txtBoxDetailSrvActServicelogs);
                clearTextboxes(txtBoxDetailTradWebRequests);
                clearTextboxes(txtBoxDetailTradWebRequestsScreenlogs);
                clearTextboxes(txtBoxDetailWinAppEventViewer);
                clearTextboxes(txtBoxDetailWinSysEventViewer);
                clearTextboxes(txtBoxDetailWinSecEventViewer);
                clearTextboxes(txtBoxDetailAndroidLogs);
                clearTextboxes(txtBoxDetailiOSLogs);
                clearTextboxes(txtBoxDetailDeviceInformationlogs);
                clearTextboxes(txtBoxDetailDevInfoCount);
                clearTextboxes(txtDetailIISlogs);
                clearTextboxes(txtBoxDetailsIISLINQ);
                clearTextboxes(txtBoxDetailServiceStudioLogs);
                clearTextboxes(txtBoxDetailGeneralTXTLogs);
                clearTextboxes(txtBoxDetailBPTReportslogs);
                clearTextboxes(txtBoxDetailEnvironmentCapabilitieslogs);
                clearTextboxes(txtBoxDetailEnvironmentslogs);
                clearTextboxes(txtBoxDetailFullErrorDumpslogs);
                clearTextboxes(txtBoxDetailRoleslogs);
                clearTextboxes(txtBoxDetailRolesInApplicationslogs);
                clearTextboxes(txtBoxDetailRolesInTeamslogs);
                clearTextboxes(txtBoxDetailUserlogs);
                clearTextboxes(txtBoxDetailUserPoolslogs);
                clearTextboxes(txtBoxDetailSyncErrorslogs);
                clearTextboxes(txtBoxDetailStagingApplogs);
                clearTextboxes(txtBoxDetailStagingAppVerlogs);
                clearTextboxes(txtBoxDetailStagingAppVerModuleVerlogs);
                clearTextboxes(txtBoxDetailStagingChangelog);
                clearTextboxes(txtBoxDetailStagingConsumerElementslogs);
                clearTextboxes(txtBoxDetailStagingEntityConfiguration);
                clearTextboxes(txtBoxDetailStagingEnvironmentApplicationCache);
                clearTextboxes(txtBoxDetailStagingEnvironmentApplicationModule);
                clearTextboxes(txtBoxDetailStagingEnviromentApplicationVersion);
                clearTextboxes(txtBoxDetailStagingEnvironmentModuleCache);
                clearTextboxes(txtBoxDetailStagingEnvironmentModuleRunning);
                clearTextboxes(txtBoxDetailStagingModules);
                clearTextboxes(txtBoxDetailStagingModuleVersionReferences);
                clearTextboxes(txtBoxDetailStagingProducerElements);
                clearTextboxes(txtBoxDetailStagingSiteProperties);
                clearTextboxes(txtBoxDetailStaginglogs);
                clearTextboxes(txtBoxDetailStagingApplicationVersion);
                clearTextboxes(txtBoxDetailStagingMessage);
                clearTextboxes(txtBoxDetailStagingModuleInconsistencies);
                clearTextboxes(txtBoxDetailStagingModuleVersion);
                clearTextboxes(txtBoxDetailStagingModuleVersionPublished);
                clearTextboxes(txtBoxDetailStagingModuleVersionUploaded);
                clearTextboxes(txtBoxDetailStagingOptions);
                clearTextboxes(txtBoxDetailStagingOutdatedApplication);
                clearTextboxes(txtBoxDetailStagingOutdatedModule);

                clearTables(dataGridViewErrorlogs);
                clearTables(dataGridViewGenerallogs);
                clearTables(dataGridViewSlowSQLlogs);
                clearTables(dataGridViewSlowSQLDurationlogs);
                clearTables(dataGridViewSlowExtensionlogs);
                clearTables(dataGridViewSlowExtensionDurationlogs);
                clearTables(dataGridViewIntegrationslogs);
                clearTables(dataGridViewIntWebServiceslogs);
                clearTables(dataGridViewInWebServicesDurationlogs);
                clearTables(dataGridViewScreenRequestslogs);
                clearTables(dataGridViewScreenRequestsScreenlogs);
                clearTables(dataGridViewScrReqScreenDurationlogs);
                clearTables(dataGridViewTimerlogs);
                clearTables(dataGridViewTimerTimerslogs);
                clearTables(dataGridViewTimerTimersDurationlogs);
                clearTables(dataGridViewEmaillogs);
                clearTables(dataGridViewEmailEmailslogs);
                clearTables(dataGridViewEmailEmailsDurationlogs);
                clearTables(dataGridViewExtensionlogs);
                clearTables(dataGridViewExtensionLogsExtensions);
                clearTables(dataGridViewExtensionsDurationlogs);
                clearTables(dataGridViewServiceActionlogs);
                clearTables(dataGridViewSrvActServicelogs);
                clearTables(dataGridViewSrvActServiceDurationlogs);
                clearTables(dataGridViewTradWebRequests);
                clearTables(dataGridViewTradWebRequestsScreenlogs);
                clearTables(dataGridViewTradWebRequestsScreenDurationlogs);
                clearTables(dataGridViewIISDateTime);
                clearTables(dataGridViewIISTimeTaken);
                clearTables(dataGridViewIISLINQreport);
                clearTables(dataGridViewWinAppEventViewer);
                clearTables(dataGridViewWinSysEventViewer);
                clearTables(dataGridViewWinSecEventViewer);
                clearTables(dataGridViewAndroidlogs);
                clearTables(dataGridViewiOSlogs);
                clearTables(dataGridViewDeviceInformation);
                clearTables(dataGridViewDevInfoCount);
                clearTables(dataGridViewServiceStudiologs);
                clearTables(dataGridViewGeneralTXTlogs);
                clearTables(dataGridViewBPTReportslogs);
                clearTables(dataGridViewEnvironmentCapabilitieslogs);
                clearTables(dataGridViewEnvironmentslogs);
                clearTables(dataGridViewFullErrorDumps);
                clearTables(dataGridViewRoleslogs);
                clearTables(dataGridViewRolesInApplicationslogs);
                clearTables(dataGridViewRolesInTeamslogs);
                clearTables(dataGridViewUserlogs);
                clearTables(dataGridViewUserPoolslogs);
                clearTables(dataGridViewSyncErrorslogs);
                clearTables(dataGridViewStagingApplogs);
                clearTables(dataGridViewStagingAppVerlogs);
                clearTables(dataGridViewStagingAppVerModuleVerlogs);
                clearTables(dataGridViewStagingChangelog);
                clearTables(dataGridViewStagingConsumerElements);
                clearTables(dataGridViewStagingEntityConfiguration);
                clearTables(dataGridViewStagingEnvironmentAppicationCache);
                clearTables(dataGridViewStagingEnvironmentApplicationModule);
                clearTables(dataGridViewStagingEnvironmentApplicationVersion);
                clearTables(dataGridViewStagingEnvironmentModuleCache);
                clearTables(dataGridViewStagingEnvironmentModuleRunning);
                clearTables(dataGridViewStagingModules);
                clearTables(dataGridViewStagingModuleVersionRefererences);
                clearTables(dataGridViewStagingProducerElements);
                clearTables(dataGridViewStagingSiteProperties);
                clearTables(dataGridViewStaginglogs);
                clearTables(dataGridViewStagingApplicationVersion);
                clearTables(dataGridViewStagingMessage);
                clearTables(dataGridViewStagingModuleInconsistencies);
                clearTables(dataGridViewStagingModuleVersion);
                clearTables(dataGridViewStagingModuleVersionPublished);
                clearTables(dataGridViewStagingModuleVersionUploaded);
                clearTables(dataGridViewStagingOptions);
                clearTables(dataGridViewStagingOutdatedApplication);
                clearTables(dataGridViewStagingOutdatedModule);

                fullPath = label8.Text + "\\filtered_data_files";
                filesPaths = Directory.GetFiles(fullPath, "*.txt", SearchOption.TopDirectoryOnly);

                foreach (string filePaths in filesPaths)
                {
                    if (File.Exists(filePaths))
                    {
                        fileName = Path.GetFileName(filePaths);

                        if (fileName == "android_build_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "MESSAGE_TYPE", "ACTION_NAME", "MESSAGE" };
                            populateTables(fullPath + "\\android_build_logs.txt", delimiters, column_names, dataGridViewAndroidlogs);
                        }
                        else if (fileName == "email_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "SENT", "ERROR_ID", "FROM", "TO", "SUBJECT",
                                "CC", "BCC", "ESPACE_NAME", "SIZE", "MESSAGE_ID", "ACTIVITY", "EMAIL_DEFINITION", "STORE_CONTENT",
                                "IS_TEST_EMAIL", "ID", "TENANT_ID" };
                            populateTables(fullPath + "\\email_logs.txt", delimiters, column_names, dataGridViewEmaillogs);
                        }
                        else if (fileName == "email_logs_emails.txt")
                        {
                            string[] column_names = { "DATE_TIME", "DURATION_SECONDS", "MODULE_NAME", "APPLICATION_NAME", "ESPACE_NAME", "MESSAGE",
                                "STACK", "FROM", "TO", "SUBJECT", "CC", "BCC", "IS_TEST_EMAIL", "ENVIRONMENT_INFORMATION", "ERROR_ID" };
                            populateTables(fullPath + "\\email_logs_emails.txt", delimiters, column_names, dataGridViewEmailEmailslogs);
                        }
                        else if (fileName == "error_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "MESSAGE", "STACK", "MODULE_NAME", "APPLICATION_NAME",
                                "APPLICATION_KEY", "ACTION_NAME", "ENTRYPOINT_NAME", "SERVER", "ESPACE_NAME", "ESPACE_ID",
                                "USER_ID", "SESSION_ID", "ENVIRONMENT_INFORMATION", "ID", "TENANT_ID" };
                            populateTables(fullPath + "\\error_logs.txt", delimiters, column_names, dataGridViewErrorlogs);
                        }
                        else if (fileName == "device_information.txt")
                        {
                            string[] column_names = { "OPERATING_SYSTEM", "OPERATING_SYSTEM_VERSION", "COUNT", "DEVICE_MODEL", "CORDOVA_VERSION", "DEVICE_UUID" };
                            populateTables(fullPath + "\\device_information.txt", delimiters, column_names, dataGridViewDeviceInformation);
                        }
                        else if (fileName == "extension_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "ACTION_NAME",
                                "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID", "USERNAME", "USER_ID", "SESSION_ID", "EXTENSION_ID",
                                "EXTENSION_NAME", "ERROR_ID", "REQUEST_KEY", "TENANT_ID" };
                            populateTables(fullPath + "\\extension_logs.txt", delimiters, column_names, dataGridViewExtensionlogs);
                        }
                        else if (fileName == "extension_logs_extensions.txt")
                        {
                            string[] column_names = { "DATE_TIME", "DURATION_SECONDS", "EXTENSION_NAME", "MODULE_NAME", "APPLICATION_NAME", "ACTION_NAME", "ESPACE_NAME", "MESSAGE", "STACK", "ENVIRONMENT_INFORMATION", "ERROR_ID" };
                            populateTables(fullPath + "\\extension_logs_extensions.txt", delimiters, column_names, dataGridViewExtensionLogsExtensions);
                        }
                        else if (fileName == "general_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "MESSAGE", "MESSAGE_TYPE", "MODULE_NAME", "APPLICATION_NAME",
                                "APPLICATION_KEY", "ACTION_NAME", "ENTRYPOINT_NAME", "CLIENT_IP", "ESPACE_NAME", "ESPACE_ID",
                                "USER_ID", "SESSION_ID", "ERROR_ID", "REQUEST_KEY", "TENANT_ID" };
                            populateTables(fullPath + "\\general_logs.txt", delimiters, column_names, dataGridViewGenerallogs);
                        }
                        else if (fileName == "general_logs_slowsql.txt")
                        {
                            string[] column_names = { "DATE_TIME", "DURATION_SECONDS", "MODULE_NAME", "APPLICATION_NAME", "ACTION_NAME", "ESPACE_NAME", "MESSAGE", "STACK", "ENVIRONMENT_INFORMATION", "ERROR_ID" };
                            populateTables(fullPath + "\\general_logs_slowsql.txt", delimiters, column_names, dataGridViewSlowSQLlogs);
                        }
                        else if (fileName == "general_logs_slowextension.txt")
                        {
                            string[] column_names = { "DATE_TIME", "DURATION_SECONDS", "MODULE_NAME", "APPLICATION_NAME", "ACTION_NAME", "ESPACE_NAME", "MESSAGE", "STACK", "ENVIRONMENT_INFORMATION", "ERROR_ID" };
                            populateTables(fullPath + "\\general_logs_slowextension.txt", delimiters, column_names, dataGridViewSlowExtensionlogs);
                        }
                        else if (fileName == "iis_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "TIME_TAKEN_SECONDS", "HTTP_CODE", "HTTP_SUBCODE", "WINDOWS_ERROR_CODE",
                                "CLIENT_IP", "SERVER_IP", "SERVER_PORT", "METHOD", "URI_STEM", "URI_QUERY", "USERNAME", "BROWSER",
                                "REFERRER" };
                            populateTables(fullPath + "\\iis_logs.txt", delimiters, column_names, dataGridViewIISDateTime);
                        }
                        else if (fileName == "integrations_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "ACTION_NAME",
                                "ACTION_TYPE", "SOURCE", "ENDPOINT", "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID", "ERROR_ID",
                                "REQUEST_KEY", "TENANT_ID" };
                            populateTables(fullPath + "\\integrations_logs.txt", delimiters, column_names, dataGridViewIntegrationslogs);
                        }
                        else if (fileName == "integrations_logs_webservices.txt")
                        {
                            string[] column_names = { "DATE_TIME", "DURATION_SECONDS", "MODULE_NAME", "APPLICATION_NAME", "ACTION_NAME", "ACTION_TYPE", "ESPACE_NAME", "MESSAGE", "STACK", "ENVIRONMENT_INFORMATION", "ERROR_ID" };
                            populateTables(fullPath + "\\integrations_logs_webservices.txt", delimiters, column_names, dataGridViewIntWebServiceslogs);
                        }
                        else if (fileName == "iOS_build_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "MESSAGE_TYPE", "ACTION_NAME", "MESSAGE" };
                            populateTables(fullPath + "\\iOS_build_logs.txt", delimiters, column_names, dataGridViewiOSlogs);
                        }
                        else if (fileName == "mobile_requests_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "DURATION", "SCREEN", "APPLICATION_NAME", "APPLICATION_KEY", "SOURCE",
                                "ENDPOINT", "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID", "LOGIN_ID", "USER_ID", "CYCLE",
                                "ERROR_ID", "REQUEST_KEY", "TENANT_ID" };
                            populateTables(fullPath + "\\mobile_requests_logs.txt", delimiters, column_names, dataGridViewScreenRequestslogs);
                        }
                        else if (fileName == "mobile_requests_logs_screens.txt")
                        {
                            string[] column_names = { "DATE_TIME", "DURATION_SECONDS", "SCREEN", "MODULE_NAME", "APPLICATION_NAME", "ESPACE_NAME", "MESSAGE", "STACK", "ENVIRONMENT_INFORMATION", "ERROR_ID" };
                            populateTables(fullPath + "\\mobile_requests_logs_screens.txt", delimiters, column_names, dataGridViewScreenRequestsScreenlogs);
                        }
                        else if (fileName == "screen_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "DURATION", "SCREEN", "SCREEN_TYPE", "APPLICATION_NAME", "APPLICATION_KEY",
                                "ACTION_NAME", "ACCESS_MODE", "EXECUTED_BY", "CLIENT_IP", "ESPACE_NAME", "ESPACE_ID",
                                "USER_ID", "SESSION_ID", "SESSION_REQUESTS", "SESSION_BYTES", "VIEW_STATE_BYTES", "MS_IS_DN", "REQUEST_KEY", "TENANT_ID" };
                            populateTables(fullPath + "\\screen_logs.txt", delimiters, column_names, dataGridViewTradWebRequests);
                        }
                        else if (fileName == "screen_logs_screens.txt")
                        {
                            string[] column_names = { "DATE_TIME", "DURATION_SECONDS", "SCREEN", "SCREEN_TYPE", "APPLICATION_NAME", "ACTION_NAME", "ESPACE_NAME" };
                            populateTables(fullPath + "\\screen_logs_screens.txt", delimiters, column_names, dataGridViewTradWebRequestsScreenlogs);
                        }
                        else if (fileName == "service_action_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "ACTION_NAME", "SOURCE",
                                "ENTRYPOINT_NAME", "ENDPOINT", "EXECUTED_BY", "ESPACE_NAME", "ESPACE_ID", "USERNAME", "LOGIN_ID",
                                "USER_ID", "SESSION_ID", "ERROR_ID", "REQUEST_KEY", "ORIGINAL_REQUEST_KEY", "TENANT_ID" };
                            populateTables(fullPath + "\\service_action_logs.txt", delimiters, column_names, dataGridViewServiceActionlogs);
                        }
                        else if (fileName == "service_action_logs_service_actions.txt")
                        {
                            string[] column_names = { "DATE_TIME", "DURATION_SECONDS", "ACTION_NAME", "MODULE_NAME", "APPLICATION_NAME", "ESPACE_NAME", "MESSAGE", "STACK", "ENVIRONMENT_INFORMATION", "ERROR_ID" };
                            populateTables(fullPath + "\\service_action_logs_service_actions.txt", delimiters, column_names, dataGridViewSrvActServicelogs);
                        }
                        else if (fileName == "service_studio_report.txt")
                        {
                            string[] column_names = { "DATE_TIME", "MESSAGE_TYPE", "MESSAGE" };
                            populateTables(fullPath + "\\service_studio_report.txt", delimiters, column_names, dataGridViewServiceStudiologs);
                        }
                        else if (fileName == "general_text_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "MESSAGE_TYPE", "MESSAGE" };
                            populateTables(fullPath + "\\general_text_logs.txt", delimiters, column_names, dataGridViewGeneralTXTlogs);
                        }
                        else if (fileName == "timer_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "EXECUTED_BY",
                                "ESPACE_NAME", "ESPACE_ID", "CYCLIC_JOB_NAME", "CYCLIC_JOB_KEY", "SHOULD_HAVE_RUN_AT", "NEXT_RUN",
                                "ERROR_ID", "REQUEST_KEY", "TENANT_ID" };
                            populateTables(fullPath + "\\timer_logs.txt", delimiters, column_names, dataGridViewTimerlogs);
                        }
                        else if (fileName == "timer_logs_timers.txt")
                        {
                            string[] column_names = { "DATE_TIME", "DURATION_SECONDS", "CYCLIC_JOB_NAME", "MODULE_NAME", "APPLICATION_NAME", "ESPACE_NAME", "MESSAGE", "STACK", "ENVIRONMENT_INFORMATION", "ERROR_ID" };
                            populateTables(fullPath + "\\timer_logs_timers.txt", delimiters, column_names, dataGridViewTimerTimerslogs);
                        }
                        else if (fileName == "filter_action_names.txt")
                        {
                            comBoxField.Items.Add("Action Name");
                        }
                        else if (fileName == "filter_application_names.txt")
                        {
                            comBoxField.Items.Add("Application Name");
                        }
                        else if (fileName == "filter_cyclic_job_names.txt")
                        {
                            comBoxField.Items.Add("Cyclic Job Name");
                        }
                        else if (fileName == "filter_espace_names.txt")
                        {
                            comBoxField.Items.Add("Espace Name");
                        }
                        else if (fileName == "filter_extension_names.txt")
                        {
                            comBoxField.Items.Add("Extension Name");
                        }
                        else if (fileName == "filter_module_names.txt")
                        {
                            comBoxField.Items.Add("Module Name");
                        }
                        else if (fileName == "bpt_troubleshootingreport_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "ESPACE_NAME", "PROCESS_NAME", "PROCESS_STATUS", "PROCESS_LAST_MODIFIED",
                                "PROCESS_SUSPENDED_DATE", "PROCESS_ID", "PARENT_PROCESS_ID", "ACTIVITY_CREATED", "ACTIVITY_NAME", "ACTIVITY_KIND",
                                "ACTIVITY_STATUS", "ACTIVITY_RUNNING_SINCE", "ACTIVITY_NEXT_RUN", "ACTIVITY_CLOSED", "ACTIVITY_ERROR_COUNT", "ACTIVITY_ERROR_ID" };
                            populateTables(fullPath + "\\bpt_troubleshootingreport_logs.txt", delimiters, column_names, dataGridViewBPTReportslogs);
                        }
                        else if (fileName == "environment_capabilities.txt")
                        {
                            string[] column_names = { "ID", "ENVIRONMENT", "CAPABILITY" };
                            populateTables(fullPath + "\\environment_capabilities.txt", delimiters, column_names, dataGridViewEnvironmentCapabilitieslogs);
                        }
                        else if (fileName == "environments.txt")
                        {
                            string[] column_names = { "IS_LIFE_TIME", "IS_REGISTERED", "ID", "NAME", "DESCRIPTION", "HOST", "PUBLIC_HOST", "TYPE", "PRIMARY_CONTACT", "ORDER",
                            "NUMBER_OF_USERS", "IS_ACTIVE", "IS_OFFLINE", "LAST_SET_OFFLINE", "CREATED_BY", "CREATED_ON", "USE_HTTPS", "VERSION", "LAST_UPGRADE_VERSION",
                            "UID", "CALLBACK_ADDRESS", "NUMBER_OF_FRONTEND_SERVERS", "FIRST_SYNC_FINISHED", "CLOUD_PROVIDER_TYPE", "ENVIRONMENT_STACK", "ADDITIONAL_INFO",
                            "LAST_CACHE_INVALIDATION", "ENVIRONMENT_DB_PROVIDER", "ENVIRONMENT_SERVER_KIND", "TWO_STEP_PUBLISH_MODE", "LAST_ENV_SYNC_SEQUENTIAL_NUMBER",
                            "DATA_HARVESTED_TO", "IS_IN_OUTSYSTEMS_CLOUD", "BEST_HASHING_ALGORITHM", "ALLOW_NATIVE_BUILDS" };
                            populateTables(fullPath + "\\environments.txt", delimiters, column_names, dataGridViewEnvironmentslogs);
                        }
                        else if (fileName == "full_error_dump_logs.txt")
                        {
                            string[] column_names = { "PLATFORM_INFORMATION" };
                            populateTables(fullPath + "\\full_error_dump_logs.txt", delimiters, column_names, dataGridViewFullErrorDumps);
                        }
                        else if (fileName == "roles.txt")
                        {
                            string[] column_names = { "ID", "NAME", "DESCRIPTION", "ORDER", "CAN_CONFIGURE_INFRASTRUCTURE", "CAN_CONFIGURE_ROLES", "CAN_CONFIGURE_USERS",
                                "CAN_CONFIGURE_APPLICATION_ROLES", "KEY", "ID_2", "LABEL_OLD", "SHORT_LABEL_OLD", "DESCRIPTION_OLD", "LABEL", "SHORT_LABEL", "DESCRIPTION_2",
                                "LT_LEVEL", "SC_LEVEL", "APPLICATION_LEVEL", "IS_OLD_LEVEL", "ID_3", "LABEL_2", "SHORT_LABEL_2", "DESCRIPTION_3", "LEVEL", "IS_COMPUTED",
                                "ID_4", "ROLE", "ENVIRONMENT", "DEFAULT_PERMISSION_LEVEL", "CAN_CREATE_APPLICATIONS", "CAN_REFERENCE_SYSTEMS", "IS_INITIALIZED" };
                            populateTables(fullPath + "\\roles.txt", delimiters, column_names, dataGridViewRoleslogs);
                        }
                        else if (fileName == "roles_in_applications.txt")
                        {
                            string[] column_names = { "ID", "USER", "ROLE", "APPLICATION" };
                            populateTables(fullPath + "\\roles_in_applications.txt", delimiters, column_names, dataGridViewRolesInApplicationslogs);
                        }
                        else if (fileName == "roles_in_teams.txt")
                        {
                            string[] column_names = { "ID", "ROLE", "KEY", "TEAM", "NAME", "KEY_2" };
                            populateTables(fullPath + "\\roles_in_teams.txt", delimiters, column_names, dataGridViewRolesInTeamslogs);
                        }
                        else if (fileName == "sync_errors.txt")
                        {
                            string[] column_names = { "DATE_TIME", "SESSION_ID", "MESSAGE", "STACK", "MODULE", "USERNAME", "ENDPOINT", "ACTION", "PROCESS_NAME",
                                "ACTIVITY_NAME" };
                            populateTables(fullPath + "\\sync_errors.txt", delimiters, column_names, dataGridViewSyncErrorslogs);
                        }
                        else if (fileName == "user.txt")
                        {
                            string[] column_names = { "ID", "NAME", "USERNAME", "EXTERNAL", "CREATION_DATE", "LAST_LOGIN", "IS_ACTIVE", "ID_2", "NAME_2",
                                "DESCRIPTION", "ORDER", "CAN_CONFIGURE_INFRASTRUCTURE", "CAN_CONFIGURE_ROLES", "CAN_CONFIGURE_USERS", "CAN_CONFIGURE_APPLICATION_ROLES",
                                "KEY", "USER_ROLE", "KEY_2", "MTSI_IDENTIFIER" };
                            populateTables(fullPath + "\\user.txt", delimiters, column_names, dataGridViewUserlogs);
                        }
                        else if (fileName == "user_pools.txt")
                        {
                            string[] column_names = { "USER", "POOL_KEY" };
                            populateTables(fullPath + "\\user_pools.txt", delimiters, column_names, dataGridViewUserPoolslogs);
                        }
                        else if (fileName == "application.txt")
                        {
                            string[] column_names = { "ID", "TEAM", "NAME", "DESCRIPTION", "PRIMARY_CONTACT", "URL_PATH", "KEY", "LOGO_HASH", "IS_ACTIVE", "DEFAULT_THEME_IS_MOBILE",
                                "MONITORING_ENABLED", "APPLICATION_KIND" };
                            populateTables(fullPath + "\\application.txt", delimiters, column_names, dataGridViewStagingApplogs);
                        }
                        else if (fileName == "application_version.txt")
                        {
                            string[] column_names = { "ID", "APPLICATION", "VERSION", "CHANGE_LOG", "CREATED_ON", "CREATED_BY", "CREATED_ON_ENVIRONMENT", "FRONT_OFFICE_ESPACE_KEY",
                                "FRONT_OFFICE_ESPACE_NAME", "BACK_OFFICE_ESPACE_KEY", "BACK_OFFICE_ESPACE_NAME", "WEB_THEME_GLOBAL_KEY", "MOBILE_THEME_GLOBAL_KEY",
                                "WAS_AUTO_TAGGED", "VERSION_DECIMAL", "TEMPLATE_KEY", "PRIMARY_COLOR", "NATIVE_HASH", "KEY" };
                            populateTables(fullPath + "\\application_version.txt", delimiters, column_names, dataGridViewStagingAppVerlogs);
                        }
                        else if (fileName == "application_version_module_version.txt")
                        {
                            string[] column_names = { "ID", "APPLICATION_VERSION", "MODULE_VERSION", "ID_2", "MODULE", "HASH", "GENERAL_HASH", "CREATED_ON",
                                "CREATED_BY", "CREATED_ON_ENVIRONMENT", "LAST_UPGRADE_VERSION", "DIRECT_UPGRADE_FROM_VERSION_HASH", "COMPATIBILITY_SIGNATURE_HASH",
                                "KEY" };
                            populateTables(fullPath + "\\application_version_module_version.txt", delimiters, column_names, dataGridViewStagingAppVerModuleVerlogs);
                        }
                        else if (fileName == "change_log.txt")
                        {
                            string[] column_names = { "ID", "LABEL", "ID_2", "DATE_TIME", "MESSAGE", "FIRST_OBJECT_TYPE", "FIRST_OBJECT", "SECOND_OBJECT_TYPE",
                                "SECOND_OBJECT", "IS_WRITE", "IS_SUCCESSFUL", "IS_SYSTEM", "ENTRY_ESPACE", "USER", "CLIENT_IP" };
                            populateTables(fullPath + "\\change_log.txt", delimiters, column_names, dataGridViewStagingChangelog);
                        }
                        else if (fileName == "consumer_elements.txt")
                        {
                            string[] column_names = { "CONSUMER_MODULE", "CONSUMER_MODULE_NAME", "CONSUMER_MODULE_VERSION", "CONSUMER_ELEMENT_VERSION", "CONSUMER_ELEMENT_VERSION_KEY", "CONSUMER_ELEMENT_VERSION_TYPE",
                                "CONSUMER_ELEMENT_VERSION_NAME", "CONSUMER_ELEMENT_VERSION_COMPATIBILITY_HASH", "PRODUCER_MODULE", "PRODUCER_MODULE_KEY", "PRODUCER_MODULE_NAME", "PRODUCER_MODULE_TYPE", "CREATED_ON_PRODUCER_MODULE_VERSION" };
                            populateTables(fullPath + "\\consumer_elements.txt", delimiters, column_names, dataGridViewStagingConsumerElements);
                        }
                        else if (fileName == "entity_configurations.txt")
                        {
                            string[] column_names = { "ID", "ENTITY_KEY", "MODULE_VERSION_ID", "CREATED_ON", "UPDATED_ON", "ID_2",
                                "MODULE_ID", "STAGING_ID", "CREATED_ON_2", "CREATED_BY", "UPDATED_ON_2", "UPDATED_BY", "ENTITY_KEY_2",
                                "PHYSICAL_TABLE_NAME", "IS_OVERRIDEN_TABLE_NAME", "DEFAULT_PHYSICAL_TABLE_NAME", "ENTITY_NAME",
                                "SOURCE_PHYSICAL_TABLE_NAME", "TARGET_PHYSICAL_TABLE_NAME" };
                            populateTables(fullPath + "\\entity_configurations.txt", delimiters, column_names, dataGridViewStagingEntityConfiguration);
                        }
                        else if (fileName == "environment_app_version.txt")
                        {
                            string[] column_names = { "ID", "ENVIRONMENT", "APPLICATION_VERSION" };
                            populateTables(fullPath + "\\environment_app_version.txt", delimiters, column_names, dataGridViewStagingEnvironmentApplicationVersion);
                        }
                        else if (fileName == "environment_application_cache.txt")
                        {
                            string[] column_names = { "ID", "ENV_APPLICATION_CACHE_ID", "MOBILE_PLATFORM", "BINARY_AVAILABLE", "CONFIG_AVAILABLE", "VERSION_NUMBER",
                                "VERSION_CODE", "VERSION_CHANGED", "CONFIGURATION_CHANGED", "TAGGED_MABS_VERSION", "LAST_BUILD_MABS_VERSION", "LOCKED_MABS_VERSION",
                                "ID_2", "ENVIRONMENT", "APPLICATION", "VERSION", "VERSION_CHANGED_2", "NUMBER_OF_USERS", "CHANGE_STATUS", "CHANGE_STATUS_MESSAGE",
                                "SERVICE_CENTER_STATUS", "SERVICE_CENTER_STATUS_MESSAGE", "LAST_PUBLISHED", "LAST_PUBLISHED_BY", "DELETED", "CONSISTENCY_STATUS",
                                "CONSISTENCY_STATUS_MESSAGES", "FRONT_OFFICE_ESPACE_KEY", "FRONT_OFFICE_ESPACE_NAME", "BACK_OFFICE_ESPACE_KEY", "BACK_OFFICE_ESPACE_NAME",
                                "WEB_THEME_GLOBAL_KEY", "MOBILE_THEME_GLOBAL_KEY", "IS_OUTDATED", "DEVELOPMENT_EFFORT", "TEMPLATE_KEY", "PRIMARY_COLOR", "NATIVE_HASH",
                                "ENV_DEPLOYMENT_ZONES", "IS_IN_MULTIPLE_DEPLOYMENT_ZONES", "IS_PWA_ENABLED" };
                            populateTables(fullPath + "\\environment_application_cache.txt", delimiters, column_names, dataGridViewStagingEnvironmentAppicationCache);
                        }
                        else if (fileName == "environment_application_module.txt")
                        {
                            string[] column_names = { "ID", "APPLICATION", "MODULE", "ENVIRONMENT", "LAST_CHANGED_ON" };
                            populateTables(fullPath + "\\environment_application_module.txt", delimiters, column_names, dataGridViewStagingEnvironmentApplicationModule);
                        }
                        else if (fileName == "environment_module_cache.txt")
                        {
                            string[] column_names = { "ID", "ENVIRONMENT", "MODULE_VERSION", "INTERNAL_VERSION", "PUBLISHED_ON", "PUBLISHED_BY", "ID_2", "ENVIRONMENT_2", "MODULE",
                                "PUBLISH", "CHANGE_STATUS", "CHANGE_STATUS_MESSAGE", "DELETED", "CONSISTENCY_STATUS", "CONSISTENCY_STATUS_MESSAGES", "IS_OUTDATED" };
                            populateTables(fullPath + "\\environment_module_cache.txt", delimiters, column_names, dataGridViewStagingEnvironmentModuleCache);
                        }
                        else if (fileName == "environment_module_running.txt")
                        {
                            string[] column_names = { "ID", "ENVIRONMENT", "CONSUMER_MODULE", "PRODUCER_MODULE_KEY", "PRODUCER_COMPATIBILITY_HASH", "IS_WEAK_REFERENCE" };
                            populateTables(fullPath + "\\environment_module_running.txt", delimiters, column_names, dataGridViewStagingEnvironmentModuleRunning);
                        }
                        else if (fileName == "module_version_references.txt")
                        {
                            string[] column_names = { "ID", "MODULE_VERSION_REFERENCE", "PRODUCER_MODULE_VERSION", "IS_COMPATIBLE", "IS_IN_DIFFERENT_LUV", "PLATFORM_VERSION",
                                "ID_2", "MODULE_VERSION", "PRODUCER_MODULE", "IS_WEAK_REFERENCE", "ID_3", "MODULE_VERSION_REFERENCE_STATUS", "ELEMENT_NAME", "ELEMENT_KEY",
                                "ELEMENT_TYPE", "ELEMENT_REF_INCONSISTENCY_TYPE_I" };
                            populateTables(fullPath + "\\module_version_references.txt", delimiters, column_names, dataGridViewStagingModuleVersionRefererences);
                        }
                        else if (fileName == "modules.txt")
                        {
                            string[] column_names = { "ID", "LABEL", "TOKEN", "ID_2", "NAME", "DESCRIPTION", "KEY", "TYPE" };
                            populateTables(fullPath + "\\modules.txt", delimiters, column_names, dataGridViewStagingModules);
                        }
                        else if (fileName == "producer_elements.txt")
                        {
                            string[] column_names = { "MODULE", "MODULE_NAME", "MODULE_VERSION", "ELEMENT_VERSION", "ELEMENT_VERSION_KEY", "ELEMENT_VERSION_TYPE", "ELEMENT_VERSION_NAME", "ELEMENT_VERSION_COMPATIBILITY_HASH" };
                            populateTables(fullPath + "\\producer_elements.txt", delimiters, column_names, dataGridViewStagingProducerElements);
                        }
                        else if (fileName == "site_properties.txt")
                        {
                            string[] column_names = { "ID", "SS_KEY", "MODULE_VERSION_ID", "NAME", "DATA_TYPE_ID", "DESCRIPTION", "DEFAULT_VALUE", "IS_MULTI_TENANT", "CREATED_ON", "UPDATED_ON", "ID_2", "LABEL", "ORDER", "IS_ACTIVE",
                                "ID_3", "EFFECTIVE_VALUE_CHANGED_IN_STAGING", "EFFECTIVE_VALUE_ON_TARGET", "SITE_PROPERTY_SS_KEY", "IS_MULTI_TENANT_2", "MODULE_ID", "STAGING_ID", "IS_NEW", "CREATED_ON_2", "CREATED_BY",
                                "UPDATED_ON_2", "UPDATED_BY" };
                            populateTables(fullPath + "\\site_properties.txt", delimiters, column_names, dataGridViewStagingSiteProperties);
                        }
                        else if (fileName == "staging.txt")
                        {
                            string[] column_names = { "ID", "SOURCE_ENVIRONMENT", "TARGET_ENVIRONMENT", "LABEL", "INTERNAL", "CREATED_BY", "CREATED_ON", "STARTED_BY", "STARTED_ON", "FINISHED_ON", "IS_DRAFT", "SOLUTION_PUBLISHED_FINISHED",
                                "IS_WAITING_FOR_CONFIRMATION_PROCESS", "SAVED_BY", "SAVED_ON", "LAST_REFRESHED_ON", "SYNC_FINISHED_ON", "SOURCE_STAGING", "STAGING_CONFIRMATION_KIND", "TWO_STEP_MODE", "LAST_RESUME_DATE_TIME", "MARKED_FOR_ABORT_ON",
                                "ABORTED_BY", "KEY", "STATUS" };
                            populateTables(fullPath + "\\staging.txt", delimiters, column_names, dataGridViewStaginglogs);
                        }
                        else if (fileName == "staging_application_version.txt")
                        {
                            string[] column_names = { "ID", "KEY", "NAME", "IS_DEFAULT", "DEPLOYMENT_TECH", "ENVIRONMENT", "IS_ACTIVE", "ID_2", "STAGING_APPLICATION_VERSION", "MOBILE_PLATFORM", "SOURCE_BINARY_AVAILABLE", "SOURCE_CONFIG_AVAILABLE",
                                "SOURCE_VERSION_NUMBER", "SOURCE_VERSION_CODE", "TARGET_BINARY_AVAILABLE", "TARGET_CONFIG_AVAILABLE", "TARGET_VERSION_NUMBER", "TARGET_VERSION_CODE", "VERSION_CHANGED", "VERSION_AFTER_FINISH_PUBLISH", "MABS_VERSION_AFTER_PUBLISH",
                                "ID_3", "STAGING", "APPLICATION", "APPLICATION_VERSION", "VERSION_CHANGED_2", "APPLICATION_DELETED", "LAST_PUBLISHED", "LAST_PUBLISHED_BY", "FRONT_OFFICE_ESPACE_KEY", "FRONT_OFFICE_ESPACE_NAME", "BACK_OFFICE_ESPACE_KEY",
                                "BACK_OFFICE_ESPACE_NAME", "WEB_THEME_GLOBAL_KEY", "MOBILE_THEME_GLOBAL_KEY", "TEMPLATE_KEY", "PRIMARY_COLOR", "SOURCE_APPLICATION_VERSION", "SOURCE_VERSION_CHANGED", "PREVIOUS_APPLICATION_VERSION", "PREVIOUS_VERSION_CHANGED",
                                "PREVIOUS_APPLICATION_DELETED", "PREVIOUS_LAST_PUBLISHED", "PREVIOUS_LAST_PUBLISHED_BY", "PREVIOUS_FRONT_OFFICE_ESPACE_KEY", "PREVIOUS_FRONT_OFFICE_ESPACE_NAME", "PREVIOUS_BACK_OFFICE_ESPACE_KEY", "PREVIOUS_BACK_OFFICE_ESPACE_NAME",
                                "PREVIOUS_WEB_THEME_GLOBAL_KEY", "PREVIOUS_MOBILE_THEME_GLOBAL_KEY", "PREVIOUS_TEMPLATE_KEY", "PREVIOUS_PRIMARY_COLOR", "OPERATION", "OPERATION_LABEL", "OPERATION_MESSAGE", "OPERATION_IS_DEPLOY", "OPERATION_IS_FORCE_DEPLOY",
                                "VERSION_WHEN_STARTING_DEPLOY", "VERSION_AFTER_FINISHING_DEPLOY", "TARGET_ENV_CONSISTENCY_STATUS", "TARGET_ENV_CONSISTENCY_MSG", "ORDER_IN_SUMMARY_TABLE", "NEW_TAG_VERSION", "NEW_TAG_DESCRIPTION", "STAGING_OPTION",
                                "PENDING_VALIDATION", "SOURCE_NATIVE_HASH", "TARGET_NATIVE_HASH", "IS_VISIBLE_IN_STAGING", "HAS_NEW_SITE_PROP", "ID_4", "ENV_DEPLOYMENT_ZONES", "ENV_DATABASE_CONFIG", "IS_PWA_ENABLED_ON_SOURCE", "IS_AUTO_UPGRADE_DISABLED" };
                            populateTables(fullPath + "\\staging_application_version.txt", delimiters, column_names, dataGridViewStagingApplicationVersion);
                        }
                        else if (fileName == "staging_message.txt")
                        {
                            string[] column_names = { "ID", "STAGING", "MESSAGE", "DETAIL", "EXTRA_INFO", "INTERNAL_ID", "INTERNAL_TYPE", "TYPE", "DATE_TIME" };
                            populateTables(fullPath + "\\staging_message.txt", delimiters, column_names, dataGridViewStagingMessage);
                        }
                        else if (fileName == "staging_module_inconsistency.txt")
                        {
                            string[] column_names = { "ID", "STAGING", "CONSUMER_MODULE", "PRODUCER_MODULE", "FIRST_REQUIRED_ELEMENT", "FIRST_REQUIRED_ELEMENT_TYPE", "TOTAL_REQUIRED_ELEMENTS", "PRODUCER_MODULE_NAME", "CONSUMER_MODULE_NAME", "INCONSISTENCY_TYPE" };
                            populateTables(fullPath + "\\staging_module_inconsistency.txt", delimiters, column_names, dataGridViewStagingModuleInconsistencies);
                        }
                        else if (fileName == "staging_module_version.txt")
                        {
                            string[] column_names = { "ID", "STAGING", "APPLICATION", "PREVIOUS_APPLICATION", "MODULE", "MODULE_VERSION", "MODULE_DELETED", "PREVIOUS_MODULE_VERSION", "PREVIOUS_MODULE_DELETED", "OPERATION" };
                            populateTables(fullPath + "\\staging_module_version.txt", delimiters, column_names, dataGridViewStagingModuleVersion);
                        }
                        else if (fileName == "staging_module_version_to_publish.txt")
                        {
                            string[] column_names = { "ID", "STAGING", "PLANNED_MODULE_VERSION_HASH", "MODULE_VERSION_HASH_TO_PUBLISH" };
                            populateTables(fullPath + "\\staging_module_version_to_publish.txt", delimiters, column_names, dataGridViewStagingModuleVersionPublished);
                        }
                        else if (fileName == "staging_module_version_to_upload.txt")
                        {
                            string[] column_names = { "ID", "STAGING", "USER", "ENVIRONMENT_ID_1", "ENVIRONMENT_ID_2", "TYPE", "NAME", "MODULE_KEY", "VERSION_KEY", "DIRECT_UPGRADE_FROM_VERSION_KEY", "APPLICATION_KEY" };
                            populateTables(fullPath + "\\staging_module_version_to_upload.txt", delimiters, column_names, dataGridViewStagingModuleVersionUploaded);
                        }
                        else if (fileName == "staging_option.txt")
                        {
                            string[] column_names = { "ID", "STAGING", "APPLICATION", "STAGING_OPTION_TYPE", "APPLICATION_VERSION", "APPLICATION_VERSION_LABEL", "LABEL", "APPLICATION_DESCRIPTION", "IS_TOP_OPTION", "iOS_VERSION_LABEL", "ANDROID_VERSION_LABEL",
                                "MOBILE_APPS_DESCRIPTION" };
                            populateTables(fullPath + "\\staging_option.txt", delimiters, column_names, dataGridViewStagingOptions);
                        }
                        else if (fileName == "staging_outdated_application.txt")
                        {
                            string[] column_names = { "ID", "STAGING", "APPLICATION" };
                            populateTables(fullPath + "\\staging_outdated_application.txt", delimiters, column_names, dataGridViewStagingOutdatedApplication);
                        }
                        else if (fileName == "staging_outdated_module.txt")
                        {
                            string[] column_names = { "ID", "STAGING", "MODULE" };
                            populateTables(fullPath + "\\staging_outdated_module.txt", delimiters, column_names, dataGridViewStagingOutdatedModule);
                        }
                        else if (fileName == "windows_application_event_viewer_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "LEVEL", "MESSAGE", "TASK", "COMPUTER", "SOURCE",
                                "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID", "KEYWORDS" };
                            populateTables(fullPath + "\\windows_application_event_viewer_logs.txt", delimiters, column_names, dataGridViewWinAppEventViewer);
                        }
                        else if (fileName == "windows_security_event_viewer_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "LEVEL", "MESSAGE", "TASK", "COMPUTER", "SOURCE",
                                "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID", "KEYWORDS" };
                            populateTables(fullPath + "\\windows_security_event_viewer_logs.txt", delimiters, column_names, dataGridViewWinSecEventViewer);
                        }
                        else if (fileName == "windows_system_event_viewer_logs.txt")
                        {
                            string[] column_names = { "DATE_TIME", "LEVEL", "MESSAGE", "TASK", "COMPUTER", "SOURCE",
                                "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID", "KEYWORDS" };
                            populateTables(fullPath + "\\windows_system_event_viewer_logs.txt", delimiters, column_names, dataGridViewWinSysEventViewer);
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
                        comBoxField.Enabled = true;
                        btnFilterFIeld.Enabled = false;
                        btnHighlight.Enabled = false;
                        btnScreenshot.Enabled = false;
                        btnSearchKeyword.Enabled = true;
                        btnSearchKeyword.BackColor = SystemColors.ControlLight;
                        txtBoxKeyword.Enabled = true;
                        rdBtnSortSlowSQLExtension.Checked = false;
                        rdBtnFilterErrorIDSLowSQLExtension.Checked = false;
                        rdBtnSortFilterErrorIDSlowSQLExtension.Checked = false;
                        rdBtnSortWebServices.Checked = false;
                        rdBtnFilterErrorIDWebServices.Checked = false;
                        rdBtnSortFilterErrorIDWebServices.Checked = false;
                        rdBtnScrReqScreens.Checked = false;
                        rdBtnFilterErrorIDScrReqScreens.Checked = false;
                        rdBtnSortFilterErrorIDScrReqScreens.Checked = false;
                        rdBtnSortTimers.Checked = false;
                        rdBtnFilterErrorIDTimers.Checked = false;
                        rdBtnSortFilterErrorIDTimers.Checked = false;
                        rdBtnSortEmails.Checked = false;
                        rdBtnFilterErrorIDEmails.Checked = false;
                        rdBtnSortExtensions.Checked = false;
                        rdBtnFilterErrorIDExtensions.Checked = false;
                        rdBtnSortFilterErrorIDExtensions.Checked = false;
                        rdBtnSortServiceActions.Checked = false;
                        rdBtnFilterErrorIDServiceActions.Checked = false;
                        rdBtnSortFilterErrorIDServiceActions.Checked = false;
                        chkBoxSortTradWebRequestsScreens.Checked = false;
                        chkBoxSortIIS.Checked = false;
                        chkBoxSortDevinfo.Checked = false;
                    }
                    else
                    {
                        btnFilter.Enabled = false;
                        btnFilterFIeld.Enabled = false;
                        dateTimePicker1.Enabled = false;
                        maskedTextBox1.Enabled = false;
                        dateTimePicker2.Enabled = false;
                        maskedTextBox2.Enabled = false;
                        btnRemoveGarbage.Enabled = false;
                        numericUpDownPercentage.Enabled = false;
                        comBoxIssueCategory.Enabled = false;
                        btnHighlight.Enabled = false;
                        btnScreenshot.Enabled = false;
                        btnSearchKeyword.Enabled = false;
                        txtBoxKeyword.Enabled = false;
                        comBoxReport.Enabled = false;
                        comBoxField.Enabled = false;
                        comBoxFilterField.Enabled = false;
                        rdBtnSortSlowSQLExtension.Checked = false;
                        rdBtnFilterErrorIDSLowSQLExtension.Checked = false;
                        rdBtnSortFilterErrorIDSlowSQLExtension.Checked = false;
                        rdBtnSortWebServices.Checked = false;
                        rdBtnFilterErrorIDWebServices.Checked = false;
                        rdBtnSortFilterErrorIDWebServices.Checked = false;
                        rdBtnScrReqScreens.Checked = false;
                        rdBtnFilterErrorIDScrReqScreens.Checked = false;
                        rdBtnSortFilterErrorIDScrReqScreens.Checked = false;
                        rdBtnSortTimers.Checked = false;
                        rdBtnFilterErrorIDTimers.Checked = false;
                        rdBtnSortFilterErrorIDTimers.Checked = false;
                        rdBtnSortEmails.Checked = false;
                        rdBtnFilterErrorIDEmails.Checked = false;
                        rdBtnSortExtensions.Checked = false;
                        rdBtnFilterErrorIDExtensions.Checked = false;
                        rdBtnSortFilterErrorIDExtensions.Checked = false;
                        rdBtnSortServiceActions.Checked = false;
                        rdBtnFilterErrorIDServiceActions.Checked = false;
                        rdBtnSortFilterErrorIDServiceActions.Checked = false;
                        chkBoxSortTradWebRequestsScreens.Checked = false;
                        chkBoxSortIIS.Checked = false;
                        chkBoxSortDevinfo.Checked = false;
                        rdBtnSortSlowSQLExtension.Enabled = false;
                        rdBtnFilterErrorIDSLowSQLExtension.Enabled = false;
                        rdBtnSortFilterErrorIDSlowSQLExtension.Enabled = false;
                        rdBtnSortWebServices.Enabled = false;
                        rdBtnFilterErrorIDWebServices.Enabled = false;
                        rdBtnSortFilterErrorIDWebServices.Enabled = false;
                        rdBtnScrReqScreens.Enabled = false;
                        rdBtnFilterErrorIDScrReqScreens.Enabled = false;
                        rdBtnSortFilterErrorIDScrReqScreens.Enabled = false;
                        rdBtnSortTimers.Enabled = false;
                        rdBtnFilterErrorIDTimers.Enabled = false;
                        rdBtnSortFilterErrorIDTimers.Enabled = false;
                        rdBtnSortEmails.Enabled = false;
                        rdBtnFilterErrorIDEmails.Enabled = false;
                        rdBtnSortExtensions.Enabled = false;
                        rdBtnFilterErrorIDExtensions.Enabled = false;
                        rdBtnSortFilterErrorIDExtensions.Enabled = false;
                        rdBtnSortServiceActions.Enabled = false;
                        rdBtnFilterErrorIDServiceActions.Enabled = false;
                        rdBtnSortFilterErrorIDServiceActions.Enabled = false;
                        chkBoxSortTradWebRequestsScreens.Enabled = false;
                        chkBoxSortIIS.Enabled = false;
                        chkBoxSortDevinfo.Enabled = false;
                        btnExportSlowSQLExtensionTables.Enabled = false;
                        btnExportWebServiceTable.Enabled = false;
                        btnExportScrReqScreenTable.Enabled = false;
                        btnExportTimerTable.Enabled = false;
                        btnExportEmailsTable.Enabled = false;
                        btnExportExtensionsTable.Enabled = false;
                        btnExportServiceActionsTable.Enabled = false;
                        btnExportScreenTable.Enabled = false;
                        btnExportIISTable.Enabled = false;
                        btnExportIISLINQTable.Enabled = false;
                        btnExportDevInfoTable.Enabled = false;
                    }
                }

                if (dataGridViewIISDateTime.Rows.Count > 0)
                {
                    chkBoxSortIIS.Enabled = true;
                    comBoxReport.Enabled = true;
                }

                if (dataGridViewSlowSQLlogs.Rows.Count > 0 || dataGridViewSlowExtensionlogs.Rows.Count > 0)
                {
                    rdBtnSortSlowSQLExtension.Enabled = true;
                    rdBtnFilterErrorIDSLowSQLExtension.Enabled = true;
                    rdBtnSortFilterErrorIDSlowSQLExtension.Enabled = true;
                }

                if (dataGridViewIntWebServiceslogs.Rows.Count > 0)
                {
                    rdBtnSortWebServices.Enabled = true;
                    rdBtnFilterErrorIDWebServices.Enabled = true;
                    rdBtnSortFilterErrorIDWebServices.Enabled = true;
                }

                if (dataGridViewScreenRequestsScreenlogs.Rows.Count > 0)
                {
                    rdBtnScrReqScreens.Enabled = true;
                    rdBtnFilterErrorIDScrReqScreens.Enabled = true;
                    rdBtnSortFilterErrorIDScrReqScreens.Enabled = true;
                }

                if (dataGridViewTimerTimerslogs.Rows.Count > 0)
                {
                    rdBtnSortTimers.Enabled = true;
                    rdBtnFilterErrorIDTimers.Enabled = true;
                    rdBtnSortFilterErrorIDTimers.Enabled = true;
                }

                if (dataGridViewEmailEmailslogs.Rows.Count > 0)
                {
                    rdBtnSortEmails.Enabled = true;
                    rdBtnFilterErrorIDEmails.Enabled = true;
                }

                if (dataGridViewExtensionLogsExtensions.Rows.Count > 0)
                {
                    rdBtnSortExtensions.Enabled = true;
                    rdBtnFilterErrorIDExtensions.Enabled = true;
                    rdBtnSortFilterErrorIDExtensions.Enabled = true;
                }

                if (dataGridViewSrvActServicelogs.Rows.Count > 0)
                {
                    rdBtnSortServiceActions.Enabled = true;
                    rdBtnFilterErrorIDServiceActions.Enabled = true;
                    rdBtnSortFilterErrorIDServiceActions.Enabled = true;
                }

                if (dataGridViewTradWebRequestsScreenlogs.Rows.Count > 0)
                {
                    chkBoxSortTradWebRequestsScreens.Enabled = true;
                }

                if (dataGridViewDeviceInformation.Rows.Count > 0)
                {
                    chkBoxSortDevinfo.Enabled = true;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void populateTables(string filePath, char splitter, string[] headerLabels, DataGridView tableName)
        {
            DataTable dt = new DataTable();
            lines = File.ReadAllLines(filePath).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            colsExpected = 0;

            if (lines.Length > 0)
            {
                //for the headers
                foreach (string headerWord in headerLabels)
                {
                    dt.Columns.Add(new DataColumn(headerWord));
                }

                colsExpected = dt.Columns.Count;

                //for the data
                dataFields = lines;
                for (int i = 0; i < dataFields.Length; i++)
                {
                    dataCols = dataFields[i].Split(splitter);

                    if (dataCols.Length < colsExpected)
                    {
                        colLengthDiff = colsExpected - dataCols.Length;
                        Array.Resize(ref dataCols, dataCols.Length + colLengthDiff);
                        while(colLengthDiff != 0)
                        {
                            dataCols[dataCols.Length - colLengthDiff] = "N/A";
                            colLengthDiff--;
                        }
                    }

                    DataRow dr = dt.NewRow();
                    colIndex = 0;
                    foreach (string headerWord in headerLabels)
                    {
                        dr[headerWord] = dataCols[colIndex++];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (dt.Rows.Count > 0)
            {
                tableName.DataSource = dt;
            }
            foreach (DataGridViewColumn col in tableName.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
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
            try
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    if (MessageBox.Show("Are you sure you want to exit the Log Parser Application?",
                                   "Question",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question) == DialogResult.Yes)
                        Application.Exit();
                    else
                        e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
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

        private void btnFilterFIeld_MouseEnter(object sender, EventArgs e)
        {
            btnFilterFIeld.BackColor = Color.SpringGreen;
        }

        private void btnFilterFIeld_MouseLeave(object sender, EventArgs e)
        {
            btnFilterFIeld.BackColor = SystemColors.ControlLight;
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

        private void btnScreenshot_MouseEnter(object sender, EventArgs e)
        {
            btnScreenshot.BackColor = Color.SpringGreen;
        }

        private void btnScreenshot_MouseLeave(object sender, EventArgs e)
        {
            btnScreenshot.BackColor = SystemColors.ControlLight;
        }

        private void btnSearchKeyword_MouseEnter(object sender, EventArgs e)
        {
            btnSearchKeyword.BackColor = Color.SpringGreen;
        }

        private void btnSearchKeyword_MouseLeave(object sender, EventArgs e)
        {
            btnSearchKeyword.BackColor = SystemColors.ControlLight;
        }

        private void btnExportSlowSQLExtensionTables_MouseEnter(object sender, EventArgs e)
        {
            btnExportSlowSQLExtensionTables.BackColor = Color.SpringGreen;
        }

        private void btnExportSlowSQLExtensionTables_MouseLeave(object sender, EventArgs e)
        {
            btnExportSlowSQLExtensionTables.BackColor = SystemColors.ControlLight;
        }

        private void btnExportWebServiceTable_MouseEnter(object sender, EventArgs e)
        {
            btnExportWebServiceTable.BackColor = Color.SpringGreen;
        }

        private void btnExportWebServiceTable_MouseLeave(object sender, EventArgs e)
        {
            btnExportWebServiceTable.BackColor = SystemColors.ControlLight;
        }

        private void btnExportTimerTable_MouseEnter(object sender, EventArgs e)
        {
            btnExportTimerTable.BackColor = Color.SpringGreen;
        }

        private void btnExportTimerTable_MouseLeave(object sender, EventArgs e)
        {
            btnExportTimerTable.BackColor = SystemColors.ControlLight;
        }

        private void btnExportEmailsTable_MouseEnter(object sender, EventArgs e)
        {
            btnExportEmailsTable.BackColor = Color.SpringGreen;
        }

        private void btnExportEmailsTable_MouseLeave(object sender, EventArgs e)
        {
            btnExportEmailsTable.BackColor = SystemColors.ControlLight;
        }

        private void btnExportScreenTable_MouseEnter(object sender, EventArgs e)
        {
            btnExportScreenTable.BackColor = Color.SpringGreen;
        }

        private void btnExportScreenTable_MouseLeave(object sender, EventArgs e)
        {
            btnExportScreenTable.BackColor = SystemColors.ControlLight;
        }

        private void btnExportScrReqScreenTable_MouseEnter(object sender, EventArgs e)
        {
            btnExportScrReqScreenTable.BackColor = Color.SpringGreen;
        }

        private void btnExportScrReqScreenTable_MouseLeave(object sender, EventArgs e)
        {
            btnExportScrReqScreenTable.BackColor = SystemColors.ControlLight;
        }

        private void btnExportExtensionsTable_MouseEnter(object sender, EventArgs e)
        {
            btnExportExtensionsTable.BackColor = Color.SpringGreen;
        }

        private void btnExportExtensionsTable_MouseLeave(object sender, EventArgs e)
        {
            btnExportExtensionsTable.BackColor = SystemColors.ControlLight;
        }

        private void btnExportServiceActionsTable_MouseEnter(object sender, EventArgs e)
        {
            btnExportServiceActionsTable.BackColor = Color.SpringGreen;
        }

        private void btnExportServiceActionsTable_MouseLeave(object sender, EventArgs e)
        {
            btnExportServiceActionsTable.BackColor = SystemColors.ControlLight;
        }

        private void btnExportIISTable_MouseEnter(object sender, EventArgs e)
        {
            btnExportIISTable.BackColor = Color.SpringGreen;
        }

        private void btnExportIISTable_MouseLeave(object sender, EventArgs e)
        {
            btnExportIISTable.BackColor = SystemColors.ControlLight;
        }

        private void btnExportDevInfoTable_MouseEnter(object sender, EventArgs e)
        {
            btnExportDevInfoTable.BackColor = Color.SpringGreen;
        }

        private void btnExportDevInfoTable_MouseLeave(object sender, EventArgs e)
        {
            btnExportDevInfoTable.BackColor = SystemColors.ControlLight;
        }

        private void btnExportIISLINQTable_MouseEnter(object sender, EventArgs e)
        {
            btnExportIISLINQTable.BackColor = Color.SpringGreen;
        }

        private void btnExportIISLINQTable_MouseLeave(object sender, EventArgs e)
        {
            btnExportIISLINQTable.BackColor = SystemColors.ControlLight;
        }

        //Beginning of the Service Center Logs tab code
        private void dataGridViewErrorlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewErrorlogs, e, txtBoxDetailErrorLogs);
        }

        private void dataGridViewGenerallogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewGenerallogs, e, txtBoxDetailGenerallogs);
        }

        private void dataGridViewIntegrationslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewIntegrationslogs, e, txtBoxDetailIntegrationlogs);
        }

        private void dataGridViewScreenRequestslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewScreenRequestslogs, e, txtBoxDetailScreenRequestslogs);
        }

        private void dataGridViewTimerlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewTimerlogs, e, txtBoxDetailTimerlogs);
        }

        private void dataGridViewEmaillogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewEmaillogs, e, txtBoxDetailEmaillogs);
        }

        private void dataGridViewExtensionlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewExtensionlogs, e, txtBoxDetailExtensionlogs);
        }

        private void dataGridViewServiceActionlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewServiceActionlogs, e, txtBoxDetailServiceActionlogs);
        }

        private void dataGridViewTradWebRequests_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewTradWebRequests, e, txtBoxDetailTradWebRequests);
        }
        //End of the Service Center Logs tab code

        //Beginning of the Windows Event Viewer logs tab code
        private void dataGridViewWinAppEventViewer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewWinAppEventViewer, e, txtBoxDetailWinAppEventViewer);
        }

        private void dataGridViewWinSysEventViewer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewWinSysEventViewer, e, txtBoxDetailWinSysEventViewer);
        }

        private void dataGridViewWinSecEventViewer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewWinSecEventViewer, e, txtBoxDetailWinSecEventViewer);
        }
        //End of the Windows Event Viewer logs tab code

        //Beginning of the Mobile logs tab code
        private void dataGridViewAndroidlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewAndroidlogs, e, txtBoxDetailAndroidLogs);
        }

        private void dataGridViewiOSlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewiOSlogs, e, txtBoxDetailiOSLogs);
        }
        //End of the Mobile logs tab code

        //Beginning of the IIS tab code
        private void dataGridViewIISDateTime_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewIISDateTime, e, txtDetailIISlogs);
        }

        private void dataGridViewIISTimeTaken_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewIISTimeTaken, e, txtDetailIISlogs);
        }

        private void dataGridViewIISLINQreport_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewIISLINQreport, e, txtBoxDetailsIISLINQ);
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
                        queryDataGridViews(dataGridViewAndroidlogs, isoFrom, isoTo, txtBoxDetailAndroidLogs);
                        queryDataGridViews(dataGridViewEmaillogs, isoFrom, isoTo, txtBoxDetailEmaillogs);
                        queryDataGridViews(dataGridViewEmailEmailslogs, isoFrom, isoTo, txtBoxDetailsEmailEmailslogs);
                        queryDataGridViews(dataGridViewEmailEmailsDurationlogs, isoFrom, isoTo, txtBoxDetailsEmailEmailslogs);
                        queryDataGridViews(dataGridViewErrorlogs, isoFrom, isoTo, txtBoxDetailErrorLogs);
                        queryDataGridViews(dataGridViewExtensionlogs, isoFrom, isoTo, txtBoxDetailExtensionlogs);
                        queryDataGridViews(dataGridViewExtensionLogsExtensions, isoFrom, isoTo, txtBoxDetailExtExtensionlogs);
                        queryDataGridViews(dataGridViewExtensionsDurationlogs, isoFrom, isoTo, txtBoxDetailExtExtensionlogs);
                        queryDataGridViews(dataGridViewGenerallogs, isoFrom, isoTo, txtBoxDetailGenerallogs);
                        queryDataGridViews(dataGridViewSlowSQLlogs, isoFrom, isoTo, txtBoxDetailsSlowSQLlogs);
                        queryDataGridViews(dataGridViewSlowSQLDurationlogs, isoFrom, isoTo, txtBoxDetailsSlowSQLlogs);
                        queryDataGridViews(dataGridViewSlowExtensionlogs, isoFrom, isoTo, txtBoxDetailsSlowExtensionlogs);
                        queryDataGridViews(dataGridViewSlowSQLDurationlogs, isoFrom, isoTo, txtBoxDetailsSlowExtensionlogs);
                        queryDataGridViews(dataGridViewIISDateTime, isoFrom, isoTo, txtDetailIISlogs);
                        queryDataGridViews(dataGridViewIISTimeTaken, isoFrom, isoTo, txtDetailIISlogs);
                        queryDataGridViews(dataGridViewIntegrationslogs, isoFrom, isoTo, txtBoxDetailIntegrationlogs);
                        queryDataGridViews(dataGridViewIntWebServiceslogs, isoFrom, isoTo, txtBoxDetailsIntWebServiceslogs);
                        queryDataGridViews(dataGridViewInWebServicesDurationlogs, isoFrom, isoTo, txtBoxDetailsIntWebServiceslogs);
                        queryDataGridViews(dataGridViewiOSlogs, isoFrom, isoTo, txtBoxDetailiOSLogs);
                        queryDataGridViews(dataGridViewScreenRequestslogs, isoFrom, isoTo, txtBoxDetailScreenRequestslogs);
                        queryDataGridViews(dataGridViewScreenRequestsScreenlogs, isoFrom, isoTo, txtBoxDetailScrReqScreenlogs);
                        queryDataGridViews(dataGridViewScrReqScreenDurationlogs, isoFrom, isoTo, txtBoxDetailScrReqScreenlogs);
                        queryDataGridViews(dataGridViewServiceActionlogs, isoFrom, isoTo, txtBoxDetailServiceActionlogs);
                        queryDataGridViews(dataGridViewSrvActServicelogs, isoFrom, isoTo, txtBoxDetailSrvActServicelogs);
                        queryDataGridViews(dataGridViewSrvActServiceDurationlogs, isoFrom, isoTo, txtBoxDetailSrvActServicelogs);
                        queryDataGridViews(dataGridViewServiceStudiologs, isoFrom, isoTo, txtBoxDetailServiceStudioLogs);
                        queryDataGridViews(dataGridViewGeneralTXTlogs, isoFrom, isoTo, txtBoxDetailGeneralTXTLogs);
                        queryDataGridViews(dataGridViewTimerlogs, isoFrom, isoTo, txtBoxDetailTimerlogs);
                        queryDataGridViews(dataGridViewTimerTimerslogs, isoFrom, isoTo, txtBoxDetailsTimerTimerslogs);
                        queryDataGridViews(dataGridViewTimerTimersDurationlogs, isoFrom, isoTo, txtBoxDetailsTimerTimerslogs);
                        queryDataGridViews(dataGridViewTradWebRequests, isoFrom, isoTo, txtBoxDetailTradWebRequests);
                        queryDataGridViews(dataGridViewTradWebRequestsScreenlogs, isoFrom, isoTo, txtBoxDetailTradWebRequestsScreenlogs);
                        queryDataGridViews(dataGridViewTradWebRequestsScreenDurationlogs, isoFrom, isoTo, txtBoxDetailTradWebRequestsScreenlogs);
                        queryDataGridViews(dataGridViewBPTReportslogs, isoFrom, isoTo, txtBoxDetailBPTReportslogs);
                        queryDataGridViews(dataGridViewWinAppEventViewer, isoFrom, isoTo, txtBoxDetailWinAppEventViewer);
                        queryDataGridViews(dataGridViewWinSysEventViewer, isoFrom, isoTo, txtBoxDetailWinSysEventViewer);
                        queryDataGridViews(dataGridViewWinSecEventViewer, isoFrom, isoTo, txtBoxDetailWinSecEventViewer);

                        btnFilter.Enabled = false;
                        dateTimePicker1.Enabled = false;
                        maskedTextBox1.Enabled = false;
                        dateTimePicker2.Enabled = false;
                        maskedTextBox2.Enabled = false;

                        if (dataGridViewIISDateTime.Rows.Count == 0)
                        {
                            chkBoxSortIIS.Enabled = false; ;
                            comBoxReport.Enabled = false;
                        }

                        if (dataGridViewSlowSQLlogs.Rows.Count == 0 || dataGridViewSlowExtensionlogs.Rows.Count == 0)
                        {
                            rdBtnSortSlowSQLExtension.Enabled = false;
                            rdBtnFilterErrorIDSLowSQLExtension.Enabled = false;
                            rdBtnSortFilterErrorIDSlowSQLExtension.Enabled = false;
                        }

                        if (dataGridViewIntWebServiceslogs.Rows.Count == 0)
                        {
                            rdBtnSortWebServices.Enabled = false;
                            rdBtnFilterErrorIDWebServices.Enabled = false;
                            rdBtnSortFilterErrorIDWebServices.Enabled = false;
                        }

                        if (dataGridViewScreenRequestsScreenlogs.Rows.Count == 0)
                        {
                            rdBtnScrReqScreens.Enabled = false;
                            rdBtnFilterErrorIDScrReqScreens.Enabled = false;
                            rdBtnSortFilterErrorIDScrReqScreens.Enabled = false;
                        }

                        if (dataGridViewTimerTimerslogs.Rows.Count == 0)
                        {
                            rdBtnSortTimers.Enabled = false;
                            rdBtnFilterErrorIDTimers.Enabled = false;
                            rdBtnSortFilterErrorIDTimers.Enabled = false;
                        }

                        if (dataGridViewEmailEmailslogs.Rows.Count == 0)
                        {
                            rdBtnSortEmails.Enabled = false;
                            rdBtnFilterErrorIDEmails.Enabled = false;
                        }

                        if (dataGridViewExtensionLogsExtensions.Rows.Count == 0)
                        {
                            rdBtnSortExtensions.Enabled = false;
                            rdBtnFilterErrorIDExtensions.Enabled = false;
                            rdBtnSortFilterErrorIDExtensions.Enabled = false;
                        }

                        if (dataGridViewSrvActServicelogs.Rows.Count == 0)
                        {
                            rdBtnSortServiceActions.Enabled = false;
                            rdBtnFilterErrorIDServiceActions.Enabled = false;
                            rdBtnSortFilterErrorIDServiceActions.Enabled = false;
                        }

                        if (dataGridViewTradWebRequestsScreenlogs.Rows.Count == 0)
                        {
                            chkBoxSortTradWebRequestsScreens.Enabled = false;
                        }
                    }
                }

                if (bool_removeGarbage)
                {
                    removeGarbage();
                }

                if (bool_highlightError)
                {
                    if (bool_highlightErrorSuccessful_1 || bool_highlightErrorSuccessful_2 || bool_highlightErrorSuccessful_3 || bool_highlightErrorSuccessful_4 || bool_highlightErrorSuccessful_5 || bool_highlightErrorSuccessful_6)
                    {
                        foreach (string c in categorySelected)
                        {
                            highlightError(c);
                        }
                    }
                }

                if (bool_findKeyword)
                {
                    if (bool_findKeywordSuccessful_1 || bool_findKeywordSuccessful_2 || bool_findKeywordSuccessful_3 || bool_findKeywordSuccessful_4 || bool_findKeywordSuccessful_5)
                    {
                        findKeyword2();
                    }
                }

                if (bool_datetimeFilterSuccessful)
                {
                    MessageBox.Show("The data was filtered by the datetime range provided", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void queryDataGridViews(DataGridView tableName, string from, string to, TextBox txtbox)
        {
            try
            {
                if (tableName.Rows.Count > 0)
                {
                    tableName.CurrentCell = null;
                    clearTextboxes(txtbox);

                    string rowFilter = string.Format("DATE_TIME >= '" + from);
                    rowFilter += string.Format("' AND DATE_TIME <= '" + to + "'");
                    (tableName.DataSource as DataTable).DefaultView.RowFilter = rowFilter;

                    bool_datetimeFilterSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
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
            bool_removeGarbage = true;

            removeGarbage();

            if (bool_highlightError)
            {
                if (bool_highlightErrorSuccessful_1 || bool_highlightErrorSuccessful_2 || bool_highlightErrorSuccessful_3 || bool_highlightErrorSuccessful_4 || bool_highlightErrorSuccessful_5 || bool_highlightErrorSuccessful_6)
                {
                    foreach (string c in categorySelected)
                    {
                        highlightError(c);
                    }
                }
            }

            if (bool_findKeyword)
            {
                if (bool_findKeywordSuccessful_1 || bool_findKeywordSuccessful_2 || bool_findKeywordSuccessful_3 || bool_findKeywordSuccessful_4 || bool_findKeywordSuccessful_5)
                {
                    findKeyword2();
                }
            }

            if (bool_removeGarbageSuccessful)
            {
                MessageBox.Show("Duplicate generic lines were hidden", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            btnRemoveGarbage.Enabled = false;
            numericUpDownPercentage.Enabled = false;
        }

        private void removeGenericErrors(string dgvName, DataGridView tableName, int columnIndex, TextBox txtbox)
        {
            try
            {
                if (tableName.Rows.Count > 50)
                {
                    forceTabClick(dgvName);

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

                                    bool_removeGarbageSuccessful = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void btnHighlight_Click(object sender, EventArgs e)
        {
            bool_highlightError = true;

            countFindKnownError++;

            category = comBoxIssueCategory.Text;
            highlightError(category);

            if (bool_removeGarbage)
            {
                removeGarbage();
            }

            if (countFindKnownError == 1)
            {
                if (bool_highlightErrorSuccessful_1)
                {
                    MessageBox.Show("Known errors were highlighted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (countFindKnownError == 2)
            {
                if (bool_highlightErrorSuccessful_2)
                {
                    MessageBox.Show("Known errors were highlighted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (countFindKnownError == 3)
            {
                if (bool_highlightErrorSuccessful_3)
                {
                    MessageBox.Show("Known errors were highlighted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (countFindKnownError == 4)
            {
                if (bool_highlightErrorSuccessful_4)
                {
                    MessageBox.Show("Known errors were highlighted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (countFindKnownError == 5)
            {
                if (bool_highlightErrorSuccessful_5)
                {
                    MessageBox.Show("Known errors were highlighted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (countFindKnownError == 6)
            {
                if (bool_highlightErrorSuccessful_6)
                {
                    MessageBox.Show("Known errors were highlighted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            categorySelected.Add(category);
            listIssueCategory.Remove(category);
            comBoxIssueCategory.Items.Clear();
            comBoxIssueCategory.Items.AddRange(listIssueCategory.ToArray());

            if (listIssueCategory.Count == 0)
            {
                comBoxIssueCategory.Enabled = false;
                btnHighlight.Enabled = false;
            }
        }

        private void highlightKnownErrors(string dgvName, DataGridView tableName, int columnIndex, string[] errorsList)
        {
            try
            {
                if (tableName.Rows.Count > 0)
                {
                    forceTabClick(dgvName);

                    foreach (string error in errorsList)
                    {
                        foreach (DataGridViewRow row in tableName.Rows)
                        {
                            currentRow = row.Index + 1;

                            if (row.Cells[columnIndex].Value.ToString().ToLower().Contains(error))
                            {
                                row.DefaultCellStyle.BackColor = Color.Yellow;

                                string dgvName2 = dgvName.Replace("dataGridView", "");
                                rowInfo += string.Join("|", row.Cells.Cast<DataGridViewCell>().Where(c => c.Value != null).Select(c => c.Value.ToString()).ToArray());
                                exportRowInfo("known_errors", category, dgvName2 + "|" + rowInfo);

                                if (countFindKnownError == 1)
                                {
                                    bool_highlightErrorSuccessful_1 = true;
                                }
                                else if (countFindKnownError == 2)
                                {
                                    bool_highlightErrorSuccessful_2 = true;
                                }
                                else if (countFindKnownError == 3)
                                {
                                    bool_highlightErrorSuccessful_3 = true;
                                }
                                else if (countFindKnownError == 4)
                                {
                                    bool_highlightErrorSuccessful_4 = true;
                                }
                                else if (countFindKnownError == 5)
                                {
                                    bool_highlightErrorSuccessful_5 = true;
                                }
                                else if (countFindKnownError == 6)
                                {
                                    bool_highlightErrorSuccessful_6 = true;
                                }
                            }
                        }
                    }

                    totalRowsCount = tableName.RowCount;

                    if (totalRowsCount > 10000)
                    {
                        sixteenthOfTotal = ((double)totalRowsCount / (double)16);
                        roundedSixteenthOfTotal = Math.Round(sixteenthOfTotal, 0, MidpointRounding.AwayFromZero);

                        eighthOfTotal = ((double)totalRowsCount / (double)8);
                        roundedEighthOfTotal = Math.Round(eighthOfTotal, 0, MidpointRounding.AwayFromZero);

                        fourthOfTotal = ((double)totalRowsCount / (double)4);
                        roundedFourthOfTotal = Math.Round(fourthOfTotal, 0, MidpointRounding.AwayFromZero);

                        halfOfTotal = ((double)totalRowsCount / (double)2);
                        roundedHalfOfTotal = Math.Round(halfOfTotal, 0, MidpointRounding.AwayFromZero);

                        if ((double)currentRow == roundedSixteenthOfTotal || (double)currentRow == roundedEighthOfTotal || (double)currentRow == roundedFourthOfTotal || (double)currentRow == roundedHalfOfTotal)
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                        else if (currentRow == totalRowsCount - 1)
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        //Beginning of the Service Studio Report tab code
        private void dataGridViewServiceStudiologs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewServiceStudiologs, e, txtBoxDetailServiceStudioLogs);
        }
        //End of Service Studio Report tab code

        private void Form1_Load(object sender, EventArgs e)
        {
            tabPage1_MyClick = new TabControlClick(tabPage1_Click);
            tabPage2_MyClick = new TabControlClick(tabPage2_Click);
            tabPage3_MyClick = new TabControlClick(tabPage3_Click);
            tabPage4_MyClick = new TabControlClick(tabPage4_Click);
            tabPage5_MyClick = new TabControlClick(tabPage5_Click);
            tabPage6_MyClick = new TabControlClick(tabPage6_Click);
            tabPage7_MyClick = new TabControlClick(tabPage7_Click);
            tabPage8_MyClick = new TabControlClick(tabPage8_Click);
            tabPage9_MyClick = new TabControlClick(tabPage9_Click);
            tabPage10_MyClick = new TabControlClick(tabPage10_Click);
            tabPage11_MyClick = new TabControlClick(tabPage11_Click);
            tabPage12_MyClick = new TabControlClick(tabPage12_Click);
            tabPage13_MyClick = new TabControlClick(tabPage13_Click);
            tabPage14_MyClick = new TabControlClick(tabPage14_Click);
            tabPage15_MyClick = new TabControlClick(tabPage15_Click);
            tabPage16_MyClick = new TabControlClick(tabPage16_Click);
            tabPage17_MyClick = new TabControlClick(tabPage17_Click);
            tabPage18_MyClick = new TabControlClick(tabPage18_Click);
            tabPage19_MyClick = new TabControlClick(tabPage19_Click);
            tabPage20_MyClick = new TabControlClick(tabPage20_Click);
            tabPage21_MyClick = new TabControlClick(tabPage21_Click);
            tabPage22_MyClick = new TabControlClick(tabPage22_Click);
            tabPage23_MyClick = new TabControlClick(tabPage23_Click);
            tabPage24_MyClick = new TabControlClick(tabPage24_Click);
            tabPage25_MyClick = new TabControlClick(tabPage25_Click);
            tabPage26_MyClick = new TabControlClick(tabPage26_Click);
            tabPage27_MyClick = new TabControlClick(tabPage27_Click);
            tabPage28_MyClick = new TabControlClick(tabPage28_Click);
            tabPage29_MyClick = new TabControlClick(tabPage29_Click);
            tabPage30_MyClick = new TabControlClick(tabPage30_Click);
            tabPage31_MyClick = new TabControlClick(tabPage31_Click);
            tabPage32_MyClick = new TabControlClick(tabPage32_Click);
            tabPage33_MyClick = new TabControlClick(tabPage33_Click);
            tabPage34_MyClick = new TabControlClick(tabPage34_Click);
            tabPage35_MyClick = new TabControlClick(tabPage35_Click);
            tabPage36_MyClick = new TabControlClick(tabPage36_Click);
            tabPage37_MyClick = new TabControlClick(tabPage37_Click);
            tabPage38_MyClick = new TabControlClick(tabPage38_Click);
            tabPage39_MyClick = new TabControlClick(tabPage39_Click);
            tabPage40_MyClick = new TabControlClick(tabPage40_Click);
            tabPage41_MyClick = new TabControlClick(tabPage41_Click);
            tabPage42_MyClick = new TabControlClick(tabPage42_Click);
            tabPage43_MyClick = new TabControlClick(tabPage43_Click);
            tabPage44_MyClick = new TabControlClick(tabPage44_Click);
            tabPage45_MyClick = new TabControlClick(tabPage45_Click);
            tabPage46_MyClick = new TabControlClick(tabPage46_Click);
            tabPage47_MyClick = new TabControlClick(tabPage47_Click);
            tabPage48_MyClick = new TabControlClick(tabPage48_Click);
            tabPage49_MyClick = new TabControlClick(tabPage49_Click);
            tabPage50_MyClick = new TabControlClick(tabPage50_Click);
            tabPage51_MyClick = new TabControlClick(tabPage51_Click);
            tabPage52_MyClick = new TabControlClick(tabPage52_Click);
            tabPage53_MyClick = new TabControlClick(tabPage53_Click);
            tabPage54_MyClick = new TabControlClick(tabPage54_Click);
            tabPage55_MyClick = new TabControlClick(tabPage55_Click);
            tabPage56_MyClick = new TabControlClick(tabPage56_Click);
            tabPage57_MyClick = new TabControlClick(tabPage57_Click);
            tabPage58_MyClick = new TabControlClick(tabPage58_Click);
            tabPage59_MyClick = new TabControlClick(tabPage59_Click);
            tabPage61_MyClick = new TabControlClick(tabPage61_Click);
            tabPage62_MyClick = new TabControlClick(tabPage62_Click);

            btnFilter.Enabled = false;
            btnFilterFIeld.Enabled = false;
            dateTimePicker1.Enabled = false;
            maskedTextBox1.Enabled = false;
            dateTimePicker2.Enabled = false;
            maskedTextBox2.Enabled = false;
            btnRemoveGarbage.Enabled = false;
            numericUpDownPercentage.Enabled = false;
            comBoxIssueCategory.Enabled = false;
            comBoxField.Enabled = false;
            comBoxFilterField.Enabled = false;
            btnHighlight.Enabled = false;
            btnScreenshot.Enabled = false;
            btnSearchKeyword.Enabled = false;
            txtBoxKeyword.Enabled = false;
            comBoxReport.Enabled = false;
            rdBtnSortSlowSQLExtension.Checked = false;
            rdBtnFilterErrorIDSLowSQLExtension.Checked = false;
            rdBtnSortFilterErrorIDSlowSQLExtension.Checked = false;
            rdBtnSortWebServices.Checked = false;
            rdBtnFilterErrorIDWebServices.Checked = false;
            rdBtnSortFilterErrorIDWebServices.Checked = false;
            rdBtnScrReqScreens.Checked = false;
            rdBtnFilterErrorIDScrReqScreens.Checked = false;
            rdBtnSortFilterErrorIDScrReqScreens.Checked = false;
            rdBtnSortTimers.Checked = false;
            rdBtnFilterErrorIDTimers.Checked = false;
            rdBtnSortFilterErrorIDTimers.Checked = false;
            rdBtnSortEmails.Checked = false;
            rdBtnFilterErrorIDEmails.Checked = false;
            rdBtnSortExtensions.Checked = false;
            rdBtnFilterErrorIDExtensions.Checked = false;
            rdBtnSortFilterErrorIDExtensions.Checked = false;
            rdBtnSortServiceActions.Checked = false;
            rdBtnFilterErrorIDServiceActions.Checked = false;
            rdBtnSortFilterErrorIDServiceActions.Checked = false;
            chkBoxSortTradWebRequestsScreens.Checked = false;
            chkBoxSortIIS.Checked = false;
            chkBoxSortDevinfo.Checked = false;
            rdBtnSortSlowSQLExtension.Enabled = false;
            rdBtnFilterErrorIDSLowSQLExtension.Enabled = false;
            rdBtnSortFilterErrorIDSlowSQLExtension.Enabled = false;
            rdBtnSortWebServices.Enabled = false;
            rdBtnFilterErrorIDWebServices.Enabled = false;
            rdBtnSortFilterErrorIDWebServices.Enabled = false;
            rdBtnScrReqScreens.Enabled = false;
            rdBtnFilterErrorIDScrReqScreens.Enabled = false;
            rdBtnSortFilterErrorIDScrReqScreens.Enabled = false;
            rdBtnSortTimers.Enabled = false;
            rdBtnFilterErrorIDTimers.Enabled = false;
            rdBtnSortFilterErrorIDTimers.Enabled = false;
            rdBtnSortEmails.Enabled = false;
            rdBtnFilterErrorIDEmails.Enabled = false;
            rdBtnSortExtensions.Enabled = false;
            rdBtnFilterErrorIDExtensions.Enabled = false;
            rdBtnSortFilterErrorIDExtensions.Enabled = false;
            rdBtnSortServiceActions.Enabled = false;
            rdBtnFilterErrorIDServiceActions.Enabled = false;
            rdBtnSortFilterErrorIDServiceActions.Enabled = false;
            chkBoxSortTradWebRequestsScreens.Enabled = false;
            chkBoxSortIIS.Enabled = false;
            chkBoxSortDevinfo.Enabled = false;
            btnExportSlowSQLExtensionTables.Enabled = false;
            btnExportWebServiceTable.Enabled = false;
            btnExportScrReqScreenTable.Enabled = false;
            btnExportTimerTable.Enabled = false;
            btnExportEmailsTable.Enabled = false;
            btnExportExtensionsTable.Enabled = false;
            btnExportServiceActionsTable.Enabled = false;
            btnExportScreenTable.Enabled = false;
            btnExportIISTable.Enabled = false;
            btnExportIISLINQTable.Enabled = false;
            btnExportDevInfoTable.Enabled = false;

            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }

        private void btnScreenshot_Click(object sender, EventArgs e)
        {
            string txtDetailErrorLogs = "";
            string txtWinAppEventVieLogs = "";
            string txtWinSecEventVieLogs = "";
            string txtWinSysEventVieLogs = "";
            string txtDetailAndromob = "";
            string txtDetailIOSmob = "";
            string txtDetailSerStudioRpt = "";

            if (!string.IsNullOrEmpty(txtBoxDetailErrorLogs.Text))
            {
                DataGridViewRow row = this.dataGridViewErrorlogs.Rows[dataGridViewErrorlogs.CurrentCell.RowIndex];
                txtDetailErrorLogs = "DATE_TIME: " + row.Cells["DATE_TIME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "MESSAGE: " + row.Cells["MESSAGE"].Value.ToString() + Environment.NewLine + Environment.NewLine + "STACK: " + row.Cells["STACK"].Value.ToString() + Environment.NewLine + Environment.NewLine + "MODULE_NAME: " + row.Cells["MODULE_NAME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "APPLICATION_NAME: " + row.Cells["APPLICATION_NAME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "ESPACE_NAME: " + row.Cells["ESPACE_NAME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "SERVER: " + row.Cells["SERVER"].Value.ToString() + Environment.NewLine + Environment.NewLine + "ENVIRONMENT_INFORMATION: " + row.Cells["ENVIRONMENT_INFORMATION"].Value.ToString();
            }
            else if (!string.IsNullOrEmpty(txtBoxDetailAndroidLogs.Text))
            {
                DataGridViewRow row = this.dataGridViewAndroidlogs.Rows[dataGridViewAndroidlogs.CurrentCell.RowIndex];
                txtDetailAndromob = "DATE_TIME: " + row.Cells["DATE_TIME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "ACTION_NAME: " + row.Cells["ACTION_NAME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "MESSAGE: " + row.Cells["MESSAGE"].Value.ToString();
            }
            else if (!string.IsNullOrEmpty(txtBoxDetailiOSLogs.Text))
            {
                DataGridViewRow row = this.dataGridViewiOSlogs.Rows[dataGridViewiOSlogs.CurrentCell.RowIndex];
                txtDetailIOSmob = "DATE_TIME: " + row.Cells["DATE_TIME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "ACTION_NAME: " + row.Cells["ACTION_NAME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "MESSAGE: " + row.Cells["MESSAGE"].Value.ToString();
            }
            else if (!string.IsNullOrEmpty(txtBoxDetailServiceStudioLogs.Text))
            {
                DataGridViewRow row = this.dataGridViewServiceStudiologs.Rows[dataGridViewServiceStudiologs.CurrentCell.RowIndex];
                txtDetailSerStudioRpt = "DATE_TIME: " + row.Cells["DATE_TIME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "ACTION_NAME: " + row.Cells["ACTION_NAME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "MESSAGE: " + row.Cells["MESSAGE"].Value.ToString();
            }
            else if (!string.IsNullOrEmpty(txtBoxDetailWinAppEventViewer.Text))
            {
                DataGridViewRow row = this.dataGridViewWinAppEventViewer.Rows[dataGridViewWinAppEventViewer.CurrentCell.RowIndex];
                txtWinAppEventVieLogs = "DATE_TIME: " + row.Cells["DATE_TIME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "LEVEL: " + row.Cells["LEVEL"].Value.ToString() + Environment.NewLine + Environment.NewLine + "MESSAGE: " + row.Cells["MESSAGE"].Value.ToString() + Environment.NewLine + Environment.NewLine + "SOURCE: " + row.Cells["SOURCE"].Value.ToString() + Environment.NewLine + Environment.NewLine + "COMPUTER: " + row.Cells["COMPUTER"].Value.ToString();
            }
            else if (!string.IsNullOrEmpty(txtBoxDetailWinSecEventViewer.Text))
            {
                DataGridViewRow row = this.dataGridViewWinSecEventViewer.Rows[dataGridViewWinSecEventViewer.CurrentCell.RowIndex];
                txtWinSecEventVieLogs = "DATE_TIME: " + row.Cells["DATE_TIME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "LEVEL: " + row.Cells["LEVEL"].Value.ToString() + Environment.NewLine + Environment.NewLine + "MESSAGE: " + row.Cells["MESSAGE"].Value.ToString() + Environment.NewLine + Environment.NewLine + "SOURCE: " + row.Cells["SOURCE"].Value.ToString() + Environment.NewLine + Environment.NewLine + "COMPUTER: " + row.Cells["COMPUTER"].Value.ToString();
            }
            else if (!string.IsNullOrEmpty(txtBoxDetailWinSysEventViewer.Text))
            {
                DataGridViewRow row = this.dataGridViewWinSysEventViewer.Rows[dataGridViewWinSysEventViewer.CurrentCell.RowIndex];
                txtWinSysEventVieLogs = "DATE_TIME: " + row.Cells["DATE_TIME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "LEVEL: " + row.Cells["LEVEL"].Value.ToString() + Environment.NewLine + Environment.NewLine + "MESSAGE: " + row.Cells["MESSAGE"].Value.ToString() + Environment.NewLine + Environment.NewLine + "SOURCE: " + row.Cells["SOURCE"].Value.ToString() + Environment.NewLine + Environment.NewLine + "COMPUTER: " + row.Cells["COMPUTER"].Value.ToString();
            }

            createScreenshot("detailed_error_logs", txtBoxDetailErrorLogs, txtDetailErrorLogs, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_general_logs", txtBoxDetailGenerallogs, txtBoxDetailGenerallogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_slowsql_logs", txtBoxDetailsSlowSQLlogs, txtBoxDetailsSlowSQLlogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_slowextension_logs", txtBoxDetailsSlowExtensionlogs, txtBoxDetailsSlowExtensionlogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_integrations_logs", txtBoxDetailIntegrationlogs, txtBoxDetailIntegrationlogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_integrations_webservices_logs", txtBoxDetailsIntWebServiceslogs, txtBoxDetailsIntWebServiceslogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_screen_requests_logs", txtBoxDetailScreenRequestslogs, txtBoxDetailScreenRequestslogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_screen_requests_screen_logs", txtBoxDetailScrReqScreenlogs, txtBoxDetailScrReqScreenlogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_timer_logs", txtBoxDetailTimerlogs, txtBoxDetailTimerlogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_timer_timers_logs", txtBoxDetailsTimerTimerslogs, txtBoxDetailsTimerTimerslogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_email_logs", txtBoxDetailEmaillogs, txtBoxDetailEmaillogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_email_emails_logs", txtBoxDetailsEmailEmailslogs, txtBoxDetailsEmailEmailslogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_extension_logs", txtBoxDetailExtensionlogs, txtBoxDetailExtensionlogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_extension_extensions_logs", txtBoxDetailExtExtensionlogs, txtBoxDetailExtExtensionlogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_service_action_logs", txtBoxDetailServiceActionlogs, txtBoxDetailServiceActionlogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_service_action_serviceactions_logs", txtBoxDetailSrvActServicelogs, txtBoxDetailSrvActServicelogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_tradional_web_requests_logs", txtBoxDetailTradWebRequests, txtBoxDetailTradWebRequests.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_tradional_web_requests_screens_logs", txtBoxDetailTradWebRequestsScreenlogs, txtBoxDetailTradWebRequestsScreenlogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_windows_application_viewer_logs", txtBoxDetailWinAppEventViewer, txtWinAppEventVieLogs, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_windows_system_viewer_logs", txtBoxDetailWinSysEventViewer, txtWinSysEventVieLogs, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_windows_security_viewer_logs", txtBoxDetailWinSecEventViewer, txtWinSecEventVieLogs, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_android_logs", txtBoxDetailAndroidLogs, txtDetailAndromob, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_ios_logs", txtBoxDetailiOSLogs, txtDetailIOSmob, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_iis_logs", txtDetailIISlogs, txtDetailIISlogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_device_information_logs", txtBoxDetailDeviceInformationlogs, txtBoxDetailDeviceInformationlogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_dev_info_count_logs", txtBoxDetailDevInfoCount, txtBoxDetailDevInfoCount.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_service_studio_logs", txtBoxDetailServiceStudioLogs, txtDetailSerStudioRpt, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_general_text_logs", txtBoxDetailGeneralTXTLogs, txtBoxDetailGeneralTXTLogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_bpt_reports_logs", txtBoxDetailBPTReportslogs, txtBoxDetailBPTReportslogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_environment_capabilities_logs", txtBoxDetailEnvironmentCapabilitieslogs, txtBoxDetailEnvironmentCapabilitieslogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_environments_logs", txtBoxDetailEnvironmentslogs, txtBoxDetailEnvironmentslogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_full_error_dump_logs", txtBoxDetailFullErrorDumpslogs, txtBoxDetailFullErrorDumpslogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_roles_logs", txtBoxDetailRoleslogs, txtBoxDetailRoleslogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_roles_in_applications_logs", txtBoxDetailRolesInApplicationslogs, txtBoxDetailRolesInApplicationslogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_roles_in_teams_logs", txtBoxDetailRolesInTeamslogs, txtBoxDetailRolesInTeamslogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_sync_error_logs", txtBoxDetailSyncErrorslogs, txtBoxDetailSyncErrorslogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_user_logs", txtBoxDetailUserlogs, txtBoxDetailUserlogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_user_pool_logs", txtBoxDetailUserPoolslogs, txtBoxDetailUserPoolslogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_application_logs", txtBoxDetailStagingApplogs, txtBoxDetailStagingApplogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_application_version_logs", txtBoxDetailStagingAppVerlogs, txtBoxDetailStagingAppVerlogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_application_version_module_version_logs", txtBoxDetailStagingAppVerModuleVerlogs, txtBoxDetailStagingAppVerModuleVerlogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_change_logs", txtBoxDetailStagingChangelog, txtBoxDetailStagingChangelog.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_consumer_elements_logs", txtBoxDetailStagingConsumerElementslogs, txtBoxDetailStagingConsumerElementslogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_entity_configurations_logs", txtBoxDetailStagingEntityConfiguration, txtBoxDetailStagingEntityConfiguration.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_environment_application_version_logs", txtBoxDetailStagingEnviromentApplicationVersion, txtBoxDetailStagingEnviromentApplicationVersion.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_environment_application_cache_logs", txtBoxDetailStagingEnvironmentApplicationCache, txtBoxDetailStagingEnvironmentApplicationCache.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_environment_application_module_logs", txtBoxDetailStagingEnvironmentApplicationModule, txtBoxDetailStagingEnvironmentApplicationModule.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_environment_module_cache_logs", txtBoxDetailStagingEnvironmentModuleCache, txtBoxDetailStagingEnvironmentModuleCache.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_environment_module_running_logs", txtBoxDetailStagingEnvironmentModuleRunning, txtBoxDetailStagingEnvironmentModuleRunning.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_module_version_references_logs", txtBoxDetailStagingModuleVersionReferences, txtBoxDetailStagingModuleVersionReferences.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_modules_logs", txtBoxDetailStagingModules, txtBoxDetailStagingModules.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_producer_elements_logs", txtBoxDetailStagingProducerElements, txtBoxDetailStagingProducerElements.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_site_properties_logs", txtBoxDetailStagingSiteProperties, txtBoxDetailStagingSiteProperties.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_staging_logs", txtBoxDetailStaginglogs, txtBoxDetailStaginglogs.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_staging_application_version_logs", txtBoxDetailStagingApplicationVersion, txtBoxDetailStagingApplicationVersion.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_staging_message_logs", txtBoxDetailStagingMessage, txtBoxDetailStagingMessage.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_staging_module_inconsistency_logs", txtBoxDetailStagingModuleInconsistencies, txtBoxDetailStagingModuleInconsistencies.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_staging_module_version_logs", txtBoxDetailStagingModuleVersion, txtBoxDetailStagingModuleVersion.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_staging_module_version_publish_logs", txtBoxDetailStagingModuleVersionPublished, txtBoxDetailStagingModuleVersionPublished.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_staging_module_version_upload_logs", txtBoxDetailStagingModuleVersionUploaded, txtBoxDetailStagingModuleVersionUploaded.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_staging_option_logs", txtBoxDetailStagingOptions, txtBoxDetailStagingOptions.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_staging_oudated_application_logs", txtBoxDetailStagingOutdatedApplication, txtBoxDetailStagingOutdatedApplication.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);
            createScreenshot("detailed_staging_oudated_module_logs", txtBoxDetailStagingOutdatedModule, txtBoxDetailStagingOutdatedModule.Text, myScreenshotFont, myScreenshotForeColor, myScreenshotBackColor);

            if (bool_screenshotSuccessful)
            {
                MessageBox.Show("A screenshot of the error was taken", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void createScreenshot(String filename, TextBox txtbox, String text, Font font, Color textColor, Color backColor)
        {
            try
            {
                if (!string.IsNullOrEmpty(text.Trim()))
                {
                    //first, create a dummy bitmap just to get a graphics object
                    Image img = new Bitmap(1, 1);
                    Graphics drawing = Graphics.FromImage(img);

                    //insert a new line after every 7 words
                    StringBuilder sb = new StringBuilder(text);
                    int spaces = 0;
                    int length = sb.Length;
                    for (int i = 0; i < length; i++)
                    {
                        if (sb[i] == ' ')
                        {
                            spaces++;
                        }
                        if (spaces == 7)
                        {
                            sb.Insert(i, Environment.NewLine);
                            spaces = 0;
                        }

                    }

                    string newText = sb.ToString();

                    //measure the string to see how big the image needs to be
                    SizeF textSize = drawing.MeasureString(newText, font);

                    //free up the dummy image and old graphics object
                    img.Dispose();
                    drawing.Dispose();

                    //create a new image of the right size
                    img = new Bitmap((int)textSize.Width, (int)textSize.Height);

                    drawing = Graphics.FromImage(img);

                    //paint the background
                    drawing.Clear(backColor);

                    //create a brush for the text
                    Brush textBrush = new SolidBrush(textColor);

                    drawing.DrawString(newText, font, textBrush, 0, 0);

                    drawing.Save();

                    textBrush.Dispose();
                    drawing.Dispose();

                    //create a folder
                    string screenshotsFolder = Path.Combine(label8.Text, "screenshots");
                    Directory.CreateDirectory(screenshotsFolder);

                    string timeStamp = DateTime.Now.ToString();
                    timeStamp = timeStamp.Replace("/", "");
                    timeStamp = timeStamp.Replace(":", "");
                    timeStamp = timeStamp.Replace(" ", "");

                    img.Save(screenshotsFolder + "\\" + filename + "_" + timeStamp + ".jpg");

                    bool_screenshotSuccessful = true;

                    clearTextboxes(txtbox);
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void dataGridViewGeneralTXTlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewGeneralTXTlogs, e, txtBoxDetailGeneralTXTLogs);
        }

        private void dataGridViewBPTReportslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewBPTReportslogs, e, txtBoxDetailBPTReportslogs);
        }

        private void dataGridViewEnvironmentCapabilitieslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewEnvironmentCapabilitieslogs, e, txtBoxDetailEnvironmentCapabilitieslogs);
        }

        private void dataGridViewEnvironmentslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewEnvironmentslogs, e, txtBoxDetailEnvironmentslogs);
        }

        private void dataGridViewFullErrorDumps_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewFullErrorDumps, e, txtBoxDetailFullErrorDumpslogs);
        }

        private void dataGridViewRoleslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewRoleslogs, e, txtBoxDetailRoleslogs);
        }

        private void dataGridViewRolesInApplicationslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewRolesInApplicationslogs, e, txtBoxDetailRolesInApplicationslogs);
        }

        private void dataGridViewRolesInTeamslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewRolesInTeamslogs, e, txtBoxDetailRolesInTeamslogs);
        }

        private void dataGridViewSyncErrorslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewSyncErrorslogs, e, txtBoxDetailSyncErrorslogs);
        }

        private void dataGridViewUserlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewUserlogs, e, txtBoxDetailUserlogs);
        }

        private void dataGridViewUserPoolslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewUserPoolslogs, e, txtBoxDetailUserPoolslogs);
        }

        private void dataGridViewStagingApplogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingApplogs, e, txtBoxDetailStagingApplogs);
        }

        private void dataGridViewStagingAppVerlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingAppVerlogs, e, txtBoxDetailStagingAppVerlogs);
        }

        private void dataGridViewStagingAppVerModuleVerlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingAppVerModuleVerlogs, e, txtBoxDetailStagingAppVerModuleVerlogs);
        }

        private void dataGridViewStagingChangelog_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingChangelog, e, txtBoxDetailStagingChangelog);
        }

        private void dataGridViewStagingConsumerElements_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingConsumerElements, e, txtBoxDetailStagingConsumerElementslogs);
        }

        private void dataGridViewStagingEntityConfiguration_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingEntityConfiguration, e, txtBoxDetailStagingEntityConfiguration);
        }

        private void dataGridViewStagingEnvironmentAppicationCache_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingEnvironmentAppicationCache, e, txtBoxDetailStagingEnvironmentApplicationCache);
        }

        private void dataGridViewStagingEnvironmentApplicationModule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingEnvironmentApplicationModule, e, txtBoxDetailStagingEnvironmentApplicationModule);
        }

        private void dataGridViewStagingEnvironmentApplicationVersion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingEnvironmentApplicationVersion, e, txtBoxDetailStagingEnviromentApplicationVersion);
        }

        private void dataGridViewStagingEnvironmentModuleCache_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingEnvironmentModuleCache, e, txtBoxDetailStagingEnvironmentModuleCache);
        }

        private void dataGridViewStagingEnvironmentModuleRunning_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingEnvironmentModuleRunning, e, txtBoxDetailStagingEnvironmentModuleRunning);
        }

        private void dataGridViewStagingModules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingModules, e, txtBoxDetailStagingModules);
        }

        private void dataGridViewStagingModuleVersionRefererences_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingModuleVersionRefererences, e, txtBoxDetailStagingModuleVersionReferences);
        }

        private void dataGridViewStagingProducerElements_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingProducerElements, e, txtBoxDetailStagingProducerElements);
        }

        private void dataGridViewStagingSiteProperties_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingSiteProperties, e, txtBoxDetailStagingSiteProperties);
        }

        private void dataGridViewStaginglogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStaginglogs, e, txtBoxDetailStaginglogs);
        }

        private void dataGridViewStagingApplicationVersion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingApplicationVersion, e, txtBoxDetailStagingApplicationVersion);
        }

        private void dataGridViewStagingMessage_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingMessage, e, txtBoxDetailStagingMessage);
        }

        private void dataGridViewStagingModuleInconsistencies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingModuleInconsistencies, e, txtBoxDetailStagingModuleInconsistencies);
        }

        private void dataGridViewStagingModuleVersion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingModuleVersion, e, txtBoxDetailStagingModuleVersion);
        }

        private void dataGridViewStagingModuleVersionPublished_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingModuleVersionPublished, e, txtBoxDetailStagingModuleVersionPublished);
        }

        private void dataGridViewStagingModuleVersionUploaded_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingModuleVersionUploaded, e, txtBoxDetailStagingModuleVersionUploaded);
        }

        private void dataGridViewStagingOptions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingOptions, e, txtBoxDetailStagingOptions);
        }

        private void dataGridViewStagingOutdatedApplication_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingOutdatedApplication, e, txtBoxDetailStagingOutdatedApplication);
        }

        private void dataGridViewStagingOutdatedModule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewStagingOutdatedModule, e, txtBoxDetailStagingOutdatedModule);
        }

        private void btnSearchKeyword_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBoxKeyword.Text))
            {
                MessageBox.Show("Please enter a keyword to search for", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtBoxKeyword.BackColor = Color.Orange;
                txtBoxKeyword.Focus();
            }
            else
            {
                bool_findKeyword = true;

                keywordsSaved.Add(txtBoxKeyword.Text);

                countFindKeyword++;

                findKeyword();

                if (bool_removeGarbage)
                {
                    removeGarbage();
                }

                if (countFindKeyword != 5)
                {
                    txtBoxKeyword.Text = "";
                }

                if (countFindKeyword == 5)
                {
                    btnSearchKeyword.Enabled = false;
                    txtBoxKeyword.Enabled = false;
                }

                if (countFindKeyword == 1)
                {
                    if (bool_findKeywordSuccessful_1)
                    {
                        MessageBox.Show("The keyword was found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (countFindKeyword == 2)
                {
                    if (bool_findKeywordSuccessful_2)
                    {
                        MessageBox.Show("The keyword was found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (countFindKeyword == 3)
                {
                    if (bool_findKeywordSuccessful_3)
                    {
                        MessageBox.Show("The keyword was found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (countFindKeyword == 4)
                {
                    if (bool_findKeywordSuccessful_4)
                    {
                        MessageBox.Show("The keyword was found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (countFindKeyword == 5)
                {
                    if (bool_findKeywordSuccessful_5)
                    {
                        MessageBox.Show("The keyword was found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void highlightKeyword(DataGridView tableName, string dgvName, string myKeyword, int myCount)
        {
            try
            {
                if (tableName.Rows.Count > 0)
                {
                    forceTabClick(dgvName);

                    //search for the value in all of the table's rows
                    foreach (DataGridViewRow row in tableName.Rows)
                    {
                        currentRow = row.Index + 1;

                        rowValues += string.Join(" ", row.Cells.Cast<DataGridViewCell>().Where(c => c.Value != null).Select(c => c.Value.ToString()).ToArray());
                        
                        if (rowValues.ToLower().Contains(myKeyword.ToLower()))
                        {
                            rowInfo += string.Join("|", row.Cells.Cast<DataGridViewCell>().Where(c => c.Value != null).Select(c => c.Value.ToString()).ToArray());
                            string dgvName2 = dgvName.Replace("dataGridView", "");
                            exportRowInfo("keywords", myKeyword, dgvName2 + "|" + rowInfo);

                            if (myCount == 1)
                            {
                                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 240);
                                row.DefaultCellStyle.ForeColor = Color.Red;
                                bool_findKeywordSuccessful_1 = true;
                            }
                            else if (myCount == 2)
                            {
                                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 247, 183);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(255, 108, 0);
                                bool_findKeywordSuccessful_2 = true;
                            }
                            else if (myCount == 3)
                            {
                                row.DefaultCellStyle.BackColor = Color.FromArgb(245, 213, 255);
                                row.DefaultCellStyle.ForeColor = Color.Purple;
                                bool_findKeywordSuccessful_3 = true;
                            }
                            else if (myCount == 4)
                            {
                                row.DefaultCellStyle.BackColor = Color.FromArgb(213, 255, 219);
                                row.DefaultCellStyle.ForeColor = Color.DarkGreen;
                                bool_findKeywordSuccessful_4 = true;
                            }
                            else if (myCount == 5)
                            {
                                row.DefaultCellStyle.BackColor = Color.FromArgb(225, 255, 253);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(2, 181, 208);
                                bool_findKeywordSuccessful_5 = true;
                            }
                        }

                        rowValues = null;
                        rowInfo = null;
                    }

                    totalRowsCount = tableName.RowCount;

                    if (totalRowsCount > 10000)
                    {
                        sixteenthOfTotal = ((double)totalRowsCount / (double)16);
                        roundedSixteenthOfTotal = Math.Round(sixteenthOfTotal, 0, MidpointRounding.AwayFromZero);

                        eighthOfTotal = ((double)totalRowsCount / (double)8);
                        roundedEighthOfTotal = Math.Round(eighthOfTotal, 0, MidpointRounding.AwayFromZero);

                        fourthOfTotal = ((double)totalRowsCount / (double)4);
                        roundedFourthOfTotal = Math.Round(fourthOfTotal, 0, MidpointRounding.AwayFromZero);

                        halfOfTotal = ((double)totalRowsCount / (double)2);
                        roundedHalfOfTotal = Math.Round(halfOfTotal, 0, MidpointRounding.AwayFromZero);

                        if ((double)currentRow == roundedSixteenthOfTotal || (double)currentRow == roundedEighthOfTotal || (double)currentRow == roundedFourthOfTotal || (double)currentRow == roundedHalfOfTotal)
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                        else if (currentRow == totalRowsCount - 1)
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void comBoxReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewIISDateTime.Rows.Count > 0)
                {
                    if (comBoxReport.SelectedIndex > -1)
                    {
                        report = comBoxReport.Text;

                        if (report == "All pages hits")
                        {
                            var pageCountQuery = (dataGridViewIISDateTime.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[9].Value != null)
                                .Select(r => r.Cells[9].Value)
                                .GroupBy(pg => pg)
                                    .OrderByDescending(g => g.Count())
                                    .Select(g => new { PAGE = g.Key, HITS = g.Count() })).ToList();

                            dataGridViewIISLINQreport.DataSource = pageCountQuery;
                        }
                        else if (report == "IPs generating traffic")
                        {
                            var ipCountQuery = (dataGridViewIISDateTime.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[5].Value != null)
                                .Select(r => r.Cells[5].Value)
                                .GroupBy(ip => ip)
                                    .OrderByDescending(g => g.Count())
                                    .Select(g => new { IP_ADDRESS = g.Key, VISITS = g.Count() })).ToList();

                            dataGridViewIISLINQreport.DataSource = ipCountQuery;
                        }
                        else if (report == "All pages hits and the IPs hitting them")
                        {
                            var pagesIPCountQuery = (dataGridViewIISDateTime.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[9].Value != null && r.Cells[5].Value != null)
                                .Select(r => new { Page = r.Cells[9].Value, IP = r.Cells[5].Value })
                                .GroupBy(pageip => pageip)
                                .OrderByDescending(g => g.Count())
                                .Select(g => new { PAGE = g.Key.Page, IP_ADDRESS = g.Key.IP, HITS = g.Count() })).ToList();

                            dataGridViewIISLINQreport.DataSource = pagesIPCountQuery;
                        }
                        else if (report == "All browsers")
                        {
                            var browsersCountQuery = (dataGridViewIISDateTime.Rows.Cast<DataGridViewRow>()
                                    .Where(r => r.Cells[12].Value != null)
                                    .Select(r => r.Cells[12].Value)
                                    .GroupBy(br => br)
                                        .OrderByDescending(g => g.Count())
                                        .Select(g => new { BROWSER = g.Key, COUNT = g.Count() })).ToList();

                            dataGridViewIISLINQreport.DataSource = browsersCountQuery;
                        }
                        else if (report == "All pages hits and their load time")
                        {
                            var pagesTimeTakenCountQuery = (dataGridViewIISDateTime.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[9].Value != null && r.Cells[1].Value != null)
                                .Select(r => new { Page = r.Cells[9].Value, TimeTaken = r.Cells[1].Value })
                                .GroupBy(pagetime => pagetime)
                                .OrderByDescending(g => g.Count())
                                .Select(g => new { PAGE = g.Key.Page, TIME_TAKEN_SECONDS = g.Key.TimeTaken, HITS = g.Count() })).ToList();

                            dataGridViewIISLINQreport.DataSource = pagesTimeTakenCountQuery;
                        }
                        else if (report == "Slowest pages to load")
                        {
                            var pageTimeQuery = (dataGridViewIISDateTime.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[9].Value != null && r.Cells[1].Value != null)
                                .Select(r => new { Page = r.Cells[9].Value, TimeTaken = r.Cells[1].Value })
                                .GroupBy(pagetime => pagetime)
                                .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                                .Select(g => new { PAGE = g.Key.Page, TIME_TAKEN_SECONDS = g.Key.TimeTaken })).ToList();

                            dataGridViewIISLINQreport.DataSource = pageTimeQuery;
                        }
                        else if (report == "Page load time per user")
                        {
                            var usernameTimeQuery = (dataGridViewIISDateTime.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[11].Value != null && r.Cells[1].Value != null)
                                .Select(r => new { Username = r.Cells[11].Value, TimeTaken = r.Cells[1].Value })
                                .GroupBy(usernametime => usernametime)
                                .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                                .Select(g => new { USERNAME = g.Key.Username, TIME_TAKEN_SECONDS = g.Key.TimeTaken })).ToList();

                            dataGridViewIISLINQreport.DataSource = usernameTimeQuery;
                        }
                        else if (report == "Domains referring traffic to pages")
                        {
                            var refCountQuery = (dataGridViewIISDateTime.Rows.Cast<DataGridViewRow>()
                                    .Where(r => r.Cells[13].Value != null)
                                    .Select(r => r.Cells[13].Value)
                                    .GroupBy(refs => refs)
                                    .OrderByDescending(g => g.Count())
                                    .Select(g => new { REFERRER = g.Key, COUNT = g.Count() })).ToList();

                            dataGridViewIISLINQreport.DataSource = refCountQuery;
                        }
                        else if (report == "Referrer broken links")
                        {
                            var brokenLinksQuery = (dataGridViewIISDateTime.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[13].Value != null && r.Cells[9].Value != null && r.Cells[2].Value.Equals("404") && r.Cells[3].Value.Equals("0"))
                                .Select(r => new { Referrer = r.Cells[13].Value, Page = r.Cells[9].Value, Http = r.Cells[2].Value, HttpSub = r.Cells[3].Value })
                                .GroupBy(referrerpagehttp => referrerpagehttp)
                                .OrderByDescending(g => g.Count())
                                .Select(g => new { REFERRER = g.Key.Referrer, PAGE = g.Key.Page, HTTP_CODE = g.Key.Http, HTTP_SUBCODE = g.Key.HttpSub })).ToList();

                            dataGridViewIISLINQreport.DataSource = brokenLinksQuery;
                        }
                        else if (report == "HTTP statuses")
                        {
                            var httpCountQuery = (dataGridViewIISDateTime.Rows.Cast<DataGridViewRow>()
                                    .Where(r => r.Cells[2].Value != null)
                                    .Select(r => r.Cells[2].Value)
                                    .GroupBy(http => http)
                                    .OrderByDescending(g => g.Count())
                                    .Select(g => new { HTTP_CODE = g.Key, COUNT = g.Count() })).ToList();

                            dataGridViewIISLINQreport.DataSource = httpCountQuery;
                        }
                        else if (report == "Windows errors")
                        {
                            var winErrCountQuery = (dataGridViewIISDateTime.Rows.Cast<DataGridViewRow>()
                                    .Where(r => r.Cells[4].Value != null)
                                    .Select(r => r.Cells[4].Value)
                                    .GroupBy(winErr => winErr)
                                    .OrderByDescending(g => g.Count())
                                    .Select(g => new { WINDOWS_ERROR_CODE = g.Key, COUNT = g.Count() })).ToList();

                            dataGridViewIISLINQreport.DataSource = winErrCountQuery;
                        }
                        else if (report == "500 errors per page and user")
                        {
                            var error500Query = (dataGridViewIISDateTime.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[9].Value != null && r.Cells[2].Value.Equals("500") && r.Cells[11].Value != null)
                                .Select(r => new { Page = r.Cells[9].Value, HTTP = r.Cells[2].Value, Username = r.Cells[11].Value })
                                .GroupBy(usernamepagehttp => usernamepagehttp)
                                .OrderByDescending(g => g.Count())
                                .Select(g => new { USERNAME = g.Key.Username, PAGE = g.Key.Page, HTTP_CODE = g.Key.HTTP, HITS = g.Count() })).ToList();

                            dataGridViewIISLINQreport.DataSource = error500Query;
                        }

                        foreach (DataGridViewColumn col in dataGridViewIISLINQreport.Columns)
                        {
                            col.SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                    }
                    btnExportIISLINQTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void dataGridViewSlowSQLlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewSlowSQLlogs, e, txtBoxDetailsSlowSQLlogs);
        }

        private void dataGridViewSlowSQLDurationlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewSlowSQLDurationlogs, e, txtBoxDetailsSlowSQLlogs);
        }

        private void dataGridViewIntWebServiceslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewIntWebServiceslogs, e, txtBoxDetailsIntWebServiceslogs);
        }

        private void dataGridViewInWebServicesDurationlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewInWebServicesDurationlogs, e, txtBoxDetailsIntWebServiceslogs);
        }

        private void dataGridViewTimerTimerslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewTimerTimerslogs, e, txtBoxDetailsTimerTimerslogs);
        }

        private void dataGridViewTimerTimersDurationlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewTimerTimersDurationlogs, e, txtBoxDetailsTimerTimerslogs);
        }

        private void dataGridViewEmailEmailslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewEmailEmailslogs, e, txtBoxDetailsEmailEmailslogs);
        }

        private void dataGridViewEmailEmailsDurationlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewEmailEmailsDurationlogs, e, txtBoxDetailsEmailEmailslogs);
        }

        private void dataGridViewTradWebRequestsScreenlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewTradWebRequestsScreenlogs, e, txtBoxDetailTradWebRequestsScreenlogs);
        }

        private void dataGridViewTradWebRequestsScreenDurationlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewTradWebRequestsScreenDurationlogs, e, txtBoxDetailTradWebRequestsScreenlogs);
        }

        private void dataGridViewSlowExtensionlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewSlowExtensionlogs, e, txtBoxDetailsSlowExtensionlogs);
        }

        private void dataGridViewSlowExtensionDurationlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewSlowExtensionDurationlogs, e, txtBoxDetailsSlowExtensionlogs);
        }

        private void dataGridViewDeviceInformation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewDeviceInformation, e, txtBoxDetailDeviceInformationlogs);
        }

        private void dataGridViewDevInfoCount_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewDevInfoCount, e, txtBoxDetailDevInfoCount);
        }

        private void dataGridViewScreenRequestsScreenlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewScreenRequestsScreenlogs, e, txtBoxDetailScrReqScreenlogs);
        }

        private void dataGridViewScrReqScreenDurationlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewScrReqScreenDurationlogs, e, txtBoxDetailScrReqScreenlogs);
        }

        private void dataGridViewExtensionLogsExtensions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewExtensionLogsExtensions, e, txtBoxDetailExtExtensionlogs);
        }

        private void dataGridViewExtensionsDurationlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewExtensionsDurationlogs, e, txtBoxDetailExtExtensionlogs);
        }

        private void dataGridViewSrvActServicelogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewSrvActServicelogs, e, txtBoxDetailSrvActServicelogs);
        }

        private void dataGridViewSrvActServiceDurationlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cellClick(dataGridViewSrvActServiceDurationlogs, e, txtBoxDetailSrvActServicelogs);
        }

        private void cellClick(DataGridView tableName, DataGridViewCellEventArgs e, TextBox txtbox)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = tableName.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(tableName.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtbox.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void removeGarbage()
        {
            removeGenericErrors("dataGridViewErrorlogs", dataGridViewErrorlogs, 1, txtBoxDetailErrorLogs);
            removeGenericErrors("dataGridViewWinAppEventViewer", dataGridViewWinAppEventViewer, 2, txtBoxDetailWinAppEventViewer);
            removeGenericErrors("dataGridViewWinSysEventViewer", dataGridViewWinSysEventViewer, 2, txtBoxDetailWinSysEventViewer);
            removeGenericErrors("dataGridViewWinSecEventViewer", dataGridViewWinSecEventViewer, 2, txtBoxDetailWinSecEventViewer);
            removeGenericErrors("dataGridViewAndroidlogs", dataGridViewAndroidlogs, 3, txtBoxDetailAndroidLogs);
            removeGenericErrors("dataGridViewiOSlogs", dataGridViewiOSlogs, 3, txtBoxDetailiOSLogs);
            removeGenericErrors("dataGridViewServiceStudiologs", dataGridViewServiceStudiologs, 2, txtBoxDetailServiceStudioLogs);
            removeGenericErrors("dataGridViewGeneralTXTlogs", dataGridViewGeneralTXTlogs, 2, txtBoxDetailGeneralTXTLogs);
        }

        private void highlightError(string ctg)
        {
            if (ctg == "Building Mobile App")
            {
                knownErrors_AndroidiOSlogs = new string[] { "command finished with error code 0", "plugin is not going to work", "plugin doesn't support this project's cordova-android version", "failed to fetch plug", "build failed with the following error", "command failed with exit code", "the ios deployment target", "kotlin", "cordovaerror", "file is corrupt or invalid", "error: spawnsync sudo etimeout", "signing certificate is invalid", "verification failed", "incompatibility", "android:exported", "could not find any", "archive failed", "missing the aps-environment entitlement", "google-services.json is missing", "appshield fargate task definition result payload", "invalid cordova plugin change-manifest" };

                highlightKnownErrors("dataGridViewAndroidlogs", dataGridViewAndroidlogs, 3, knownErrors_AndroidiOSlogs);
                highlightKnownErrors("dataGridViewiOSlogs", dataGridViewiOSlogs, 3, knownErrors_AndroidiOSlogs);
            }
            else if (ctg == "Compilation")
            {
                knownErrors_Errorlogs = new string[] { "compilation error", "can't proceed", "error loading espace", "failed to parse response", "error obtaining version", "check if a third-party program is using", "the paging file is too small for this operation to complete", "could not allocate space for object", "environment fault", "old producer", "locked module", "an error occurred in task" };

                highlightKnownErrors("dataGridViewErrorlogs", dataGridViewErrorlogs, 1, knownErrors_Errorlogs);

            }
            else if (ctg == "Content Security Policy (CSP)")
            {
                knownErrors_Errorlogs = new string[] { "violated-directive" };

                highlightKnownErrors("dataGridViewErrorlogs", dataGridViewErrorlogs, 1, knownErrors_Errorlogs);
            }
            else if (ctg == "Database")
            {
                knownErrors_Errorlogs = new string[] { "truncated in table", "dequeuing", "connection is broken", "locked module" };
                knownErrors_WinAppEventViewer = new string[] { "ora-", "error closing", "error opening" };
                knownErrors_WinSysEventViewer = new string[] { "error closing", "timed out" };

                highlightKnownErrors("dataGridViewErrorlogs", dataGridViewErrorlogs, 1, knownErrors_Errorlogs);
                highlightKnownErrors("dataGridViewWinAppEventViewer", dataGridViewWinAppEventViewer, 2, knownErrors_WinAppEventViewer);
                highlightKnownErrors("dataGridViewWinSysEventViewer", dataGridViewWinSysEventViewer, 2, knownErrors_WinSysEventViewer);
            }
            else if (ctg == "Logic")
            {
                knownErrors_Errorlogs = new string[] { "url rewrite module error", "an error occurred in task", "a fatal error has occurred", "json deserialization", "unknown reference expression type email", "bad json escape sequence", "dequeuing", "error loading espace", "waited too long for debug command" };
                knownErrors_Generallogs = new string[] { "system cannot find" };
                knownErrors_WinAppEventViewer = new string[] { "error closing", "error opening" };
                knownErrors_WinSysEventViewer = new string[] { "error closing", "timed out" };
                knownErrors_AndroidiOSlogs = new string[] { "androidx library", "error: spawnsync sudo etimeout", "verification failed", "could not find any", "archive failed" };
                knownErrors_ServiceStudiologs = new string[] { "oneoftypedefinition", "unable to consume soap web service" };

                highlightKnownErrors("dataGridViewErrorlogs", dataGridViewErrorlogs, 1, knownErrors_Errorlogs);
                highlightKnownErrors("dataGridViewGenerallogs", dataGridViewGenerallogs, 1, knownErrors_Generallogs);
                highlightKnownErrors("dataGridViewWinAppEventViewer", dataGridViewWinAppEventViewer, 2, knownErrors_WinAppEventViewer);
                highlightKnownErrors("dataGridViewWinSysEventViewer", dataGridViewWinSysEventViewer, 2, knownErrors_WinSysEventViewer);
                highlightKnownErrors("dataGridViewAndroidlogs", dataGridViewAndroidlogs, 3, knownErrors_AndroidiOSlogs);
                highlightKnownErrors("dataGridViewiOSlogs", dataGridViewiOSlogs, 3, knownErrors_AndroidiOSlogs);
                highlightKnownErrors("dataGridViewServiceStudiologs", dataGridViewServiceStudiologs, 2, knownErrors_ServiceStudiologs);
            }
            else if (ctg == "Network")
            {
                knownErrors_Errorlogs = new string[] { "url rewrite module error", "an error occurred in task", "server cannot modify cookies", "ping validation failed", "communicationexception", "file is corrupt or invalid", "checksum failed for file", "connection failed421", "temporary server error", "unable to open service studio", "invalid authentication token", "connection is broken", "cannot decrypt the content", "could not establish trust relationship for the ssl/tls secure channel", "the remote certificate is invalid according to the validation procedure", "unexpected content found in ping", "win32error", "internal network use only", "could not ping", "error invalidating cache" };
                knownErrors_WinAppEventViewer = new string[] { "error closing", "cannot access", "error opening", "certificate" };
                knownErrors_WinSysEventViewer = new string[] { "error closing", "timed out", "certificate" };
                knownErrors_AndroidiOSlogs = new string[] { "file is corrupt or invalid", "error: spawnsync sudo etimeout", "signing certificate is invalid", "verification failed" };
                knownErrors_ServiceStudiologs = new string[] { "http forbidden", "[not connected]" };
                knownErrors_GeneralTXTlogs = new string[] { "unable to load one or more of the requested types" };

                highlightKnownErrors("dataGridViewErrorlogs", dataGridViewErrorlogs, 1, knownErrors_Errorlogs);
                highlightKnownErrors("dataGridViewWinAppEventViewer", dataGridViewWinAppEventViewer, 2, knownErrors_WinAppEventViewer);
                highlightKnownErrors("dataGridViewWinSysEventViewer", dataGridViewWinSysEventViewer, 2, knownErrors_WinSysEventViewer);
                highlightKnownErrors("dataGridViewAndroidlogs", dataGridViewAndroidlogs, 3, knownErrors_AndroidiOSlogs);
                highlightKnownErrors("dataGridViewiOSlogs", dataGridViewiOSlogs, 3, knownErrors_AndroidiOSlogs);
                highlightKnownErrors("dataGridViewServiceStudiologs", dataGridViewServiceStudiologs, 2, knownErrors_ServiceStudiologs);
                highlightKnownErrors("dataGridViewGeneralTXTlogs", dataGridViewGeneralTXTlogs, 2, knownErrors_GeneralTXTlogs);
            }
        }

        private void findKeyword()
        {
            foreach (string k in keywordsSaved)
            {
                int idx = keywordsSaved.IndexOf(k);

                if (idx+1 == countFindKeyword)
                {
                    highlightKeyword(dataGridViewErrorlogs, "dataGridViewErrorlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewGenerallogs, "dataGridViewGenerallogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewSlowSQLlogs, "dataGridViewSlowSQLlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewSlowSQLDurationlogs, "dataGridViewSlowSQLDurationlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewSlowExtensionlogs, "dataGridViewSlowExtensionlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewSlowExtensionDurationlogs, "dataGridViewSlowExtensionDurationlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewIntegrationslogs, "dataGridViewIntegrationslogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewIntWebServiceslogs, "dataGridViewIntWebServiceslogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewInWebServicesDurationlogs, "dataGridViewInWebServicesDurationlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewScreenRequestslogs, "dataGridViewScreenRequestslogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewScreenRequestsScreenlogs, "dataGridViewScreenRequestsScreenlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewScrReqScreenDurationlogs, "dataGridViewScrReqScreenDurationlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewTimerlogs, "dataGridViewTimerlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewTimerTimerslogs, "dataGridViewTimerTimerslogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewTimerTimersDurationlogs, "dataGridViewTimerTimersDurationlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewEmaillogs, "dataGridViewEmaillogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewEmailEmailslogs, "dataGridViewEmailEmailslogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewEmailEmailsDurationlogs, "dataGridViewEmailEmailsDurationlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewExtensionlogs, "dataGridViewExtensionlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewExtensionLogsExtensions, "dataGridViewExtensionLogsExtensions", k, countFindKeyword);
                    highlightKeyword(dataGridViewExtensionsDurationlogs, "dataGridViewExtensionsDurationlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewServiceActionlogs, "dataGridViewServiceActionlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewSrvActServicelogs, "dataGridViewSrvActServicelogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewSrvActServiceDurationlogs, "dataGridViewSrvActServiceDurationlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewTradWebRequests, "dataGridViewTradWebRequests", k, countFindKeyword);
                    highlightKeyword(dataGridViewTradWebRequestsScreenlogs, "dataGridViewTradWebRequestsScreenlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewTradWebRequestsScreenDurationlogs, "dataGridViewTradWebRequestsScreenDurationlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewIISDateTime, "dataGridViewIISDateTime", k, countFindKeyword);
                    highlightKeyword(dataGridViewIISTimeTaken, "dataGridViewIISTimeTaken", k, countFindKeyword);
                    highlightKeyword(dataGridViewWinAppEventViewer, "dataGridViewWinAppEventViewer", k, countFindKeyword);
                    highlightKeyword(dataGridViewWinSysEventViewer, "dataGridViewWinSysEventViewer", k, countFindKeyword);
                    highlightKeyword(dataGridViewWinSecEventViewer, "dataGridViewWinSecEventViewer", k, countFindKeyword);
                    highlightKeyword(dataGridViewAndroidlogs, "dataGridViewAndroidlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewiOSlogs, "dataGridViewiOSlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewServiceStudiologs, "dataGridViewServiceStudiologs", k, countFindKeyword);
                    highlightKeyword(dataGridViewGeneralTXTlogs, "dataGridViewGeneralTXTlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewBPTReportslogs, "dataGridViewBPTReportslogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewEnvironmentCapabilitieslogs, "dataGridViewEnvironmentCapabilitieslogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewEnvironmentslogs, "dataGridViewEnvironmentslogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewFullErrorDumps, "dataGridViewFullErrorDumps", k, countFindKeyword);
                    highlightKeyword(dataGridViewRoleslogs, "dataGridViewRoleslogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewRolesInApplicationslogs, "dataGridViewRolesInApplicationslogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewRolesInTeamslogs, "dataGridViewRolesInTeamslogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewUserlogs, "dataGridViewUserlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewUserPoolslogs, "dataGridViewUserPoolslogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewSyncErrorslogs, "dataGridViewSyncErrorslogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingApplogs, "dataGridViewStagingApplogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingAppVerlogs, "dataGridViewStagingAppVerlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingAppVerModuleVerlogs, "dataGridViewStagingAppVerModuleVerlogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingChangelog, "dataGridViewStagingChangelog", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingConsumerElements, "dataGridViewStagingConsumerElements", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingEntityConfiguration, "dataGridViewStagingEntityConfiguration", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingEnvironmentAppicationCache, "dataGridViewStagingEnvironmentAppicationCache", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingEnvironmentApplicationModule, "dataGridViewStagingEnvironmentApplicationModule", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingEnvironmentApplicationVersion, "dataGridViewStagingEnvironmentApplicationVersion", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingEnvironmentModuleCache, "dataGridViewStagingEnvironmentModuleCache", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingEnvironmentModuleRunning, "dataGridViewStagingEnvironmentModuleRunning", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingModules, "dataGridViewStagingModules", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingModuleVersionRefererences, "dataGridViewStagingModuleVersionRefererences", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingProducerElements, "dataGridViewStagingProducerElements", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingSiteProperties, "dataGridViewStagingSiteProperties", k, countFindKeyword);
                    highlightKeyword(dataGridViewStaginglogs, "dataGridViewStaginglogs", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingApplicationVersion, "dataGridViewStagingApplicationVersion", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingMessage, "dataGridViewStagingMessage", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingModuleInconsistencies, "dataGridViewStagingModuleInconsistencies", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingModuleVersion, "dataGridViewStagingModuleVersion", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingModuleVersionPublished, "dataGridViewStagingModuleVersionPublished", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingModuleVersionUploaded, "dataGridViewStagingModuleVersionUploaded", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingOptions, "dataGridViewStagingOptions", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingOutdatedApplication, "dataGridViewStagingOutdatedApplication", k, countFindKeyword);
                    highlightKeyword(dataGridViewStagingOutdatedModule, "dataGridViewStagingOutdatedModule", k, countFindKeyword);
                }
            }
        }

        private void findKeyword2()
        {
            foreach (string k in keywordsSaved)
            {
                int idx = keywordsSaved.IndexOf(k);

                highlightKeyword(dataGridViewErrorlogs, "dataGridViewErrorlogs", k, idx + 1);
                highlightKeyword(dataGridViewGenerallogs, "dataGridViewGenerallogs", k, idx + 1);
                highlightKeyword(dataGridViewSlowSQLlogs, "dataGridViewSlowSQLlogs", k, idx + 1);
                highlightKeyword(dataGridViewSlowSQLDurationlogs, "dataGridViewSlowSQLDurationlogs", k, idx + 1);
                highlightKeyword(dataGridViewSlowExtensionlogs, "dataGridViewSlowExtensionlogs", k, idx + 1);
                highlightKeyword(dataGridViewSlowExtensionDurationlogs, "dataGridViewSlowExtensionDurationlogs", k, idx + 1);
                highlightKeyword(dataGridViewIntegrationslogs, "dataGridViewIntegrationslogs", k, idx + 1);
                highlightKeyword(dataGridViewIntWebServiceslogs, "dataGridViewIntWebServiceslogs", k, idx + 1);
                highlightKeyword(dataGridViewInWebServicesDurationlogs, "dataGridViewInWebServicesDurationlogs", k, idx + 1);
                highlightKeyword(dataGridViewScreenRequestslogs, "dataGridViewScreenRequestslogs", k, idx + 1);
                highlightKeyword(dataGridViewScreenRequestsScreenlogs, "dataGridViewScreenRequestsScreenlogs", k, idx + 1);
                highlightKeyword(dataGridViewScrReqScreenDurationlogs, "dataGridViewScrReqScreenDurationlogs", k, idx + 1);
                highlightKeyword(dataGridViewTimerlogs, "dataGridViewTimerlogs", k, idx + 1);
                highlightKeyword(dataGridViewTimerTimerslogs, "dataGridViewTimerTimerslogs", k, idx + 1);
                highlightKeyword(dataGridViewTimerTimersDurationlogs, "dataGridViewTimerTimersDurationlogs", k, idx + 1);
                highlightKeyword(dataGridViewEmaillogs, "dataGridViewEmaillogs", k, idx + 1);
                highlightKeyword(dataGridViewEmailEmailslogs, "dataGridViewEmailEmailslogs", k, idx + 1);
                highlightKeyword(dataGridViewEmailEmailsDurationlogs, "dataGridViewEmailEmailsDurationlogs", k, idx + 1);
                highlightKeyword(dataGridViewExtensionlogs, "dataGridViewExtensionlogs", k, idx + 1);
                highlightKeyword(dataGridViewExtensionLogsExtensions, "dataGridViewExtensionLogsExtensions", k, idx + 1);
                highlightKeyword(dataGridViewExtensionsDurationlogs, "dataGridViewExtensionsDurationlogs", k, idx + 1);
                highlightKeyword(dataGridViewServiceActionlogs, "dataGridViewServiceActionlogs", k, idx + 1);
                highlightKeyword(dataGridViewSrvActServicelogs, "dataGridViewSrvActServicelogs", k, idx + 1);
                highlightKeyword(dataGridViewSrvActServiceDurationlogs, "dataGridViewSrvActServiceDurationlogs", k, idx + 1);
                highlightKeyword(dataGridViewTradWebRequests, "dataGridViewTradWebRequests", k, idx + 1);
                highlightKeyword(dataGridViewTradWebRequestsScreenlogs, "dataGridViewTradWebRequestsScreenlogs", k, idx + 1);
                highlightKeyword(dataGridViewTradWebRequestsScreenDurationlogs, "dataGridViewTradWebRequestsScreenDurationlogs", k, idx + 1);
                highlightKeyword(dataGridViewIISDateTime, "dataGridViewIISDateTime", k, idx + 1);
                highlightKeyword(dataGridViewIISTimeTaken, "dataGridViewIISTimeTaken", k, idx + 1);
                highlightKeyword(dataGridViewWinAppEventViewer, "dataGridViewWinAppEventViewer", k, idx + 1);
                highlightKeyword(dataGridViewWinSysEventViewer, "dataGridViewWinSysEventViewer", k, idx + 1);
                highlightKeyword(dataGridViewWinSecEventViewer, "dataGridViewWinSecEventViewer", k, idx + 1);
                highlightKeyword(dataGridViewAndroidlogs, "dataGridViewAndroidlogs", k, idx + 1);
                highlightKeyword(dataGridViewiOSlogs, "dataGridViewiOSlogs", k, idx + 1);
                highlightKeyword(dataGridViewServiceStudiologs, "dataGridViewServiceStudiologs", k, idx + 1);
                highlightKeyword(dataGridViewGeneralTXTlogs, "dataGridViewGeneralTXTlogs", k, idx + 1);
                highlightKeyword(dataGridViewBPTReportslogs, "dataGridViewBPTReportslogs", k, idx + 1);
                highlightKeyword(dataGridViewEnvironmentCapabilitieslogs, "dataGridViewEnvironmentCapabilitieslogs", k, idx + 1);
                highlightKeyword(dataGridViewEnvironmentslogs, "dataGridViewEnvironmentslogs", k, idx + 1);
                highlightKeyword(dataGridViewFullErrorDumps, "dataGridViewFullErrorDumps", k, idx + 1);
                highlightKeyword(dataGridViewRoleslogs, "dataGridViewRoleslogs", k, idx + 1);
                highlightKeyword(dataGridViewRolesInApplicationslogs, "dataGridViewRolesInApplicationslogs", k, idx + 1);
                highlightKeyword(dataGridViewRolesInTeamslogs, "dataGridViewRolesInTeamslogs", k, idx + 1);
                highlightKeyword(dataGridViewUserlogs, "dataGridViewUserlogs", k, idx + 1);
                highlightKeyword(dataGridViewUserPoolslogs, "dataGridViewUserPoolslogs", k, idx + 1);
                highlightKeyword(dataGridViewSyncErrorslogs, "dataGridViewSyncErrorslogs", k, idx + 1);
                highlightKeyword(dataGridViewStagingApplogs, "dataGridViewStagingApplogs", k, idx + 1);
                highlightKeyword(dataGridViewStagingAppVerlogs, "dataGridViewStagingAppVerlogs", k, idx + 1);
                highlightKeyword(dataGridViewStagingAppVerModuleVerlogs, "dataGridViewStagingAppVerModuleVerlogs", k, idx + 1);
                highlightKeyword(dataGridViewStagingChangelog, "dataGridViewStagingChangelog", k, idx + 1);
                highlightKeyword(dataGridViewStagingConsumerElements, "dataGridViewStagingConsumerElements", k, idx + 1);
                highlightKeyword(dataGridViewStagingEntityConfiguration, "dataGridViewStagingEntityConfiguration", k, idx + 1);
                highlightKeyword(dataGridViewStagingEnvironmentAppicationCache, "dataGridViewStagingEnvironmentAppicationCache", k, idx + 1);
                highlightKeyword(dataGridViewStagingEnvironmentApplicationModule, "dataGridViewStagingEnvironmentApplicationModule", k, idx + 1);
                highlightKeyword(dataGridViewStagingEnvironmentApplicationVersion, "dataGridViewStagingEnvironmentApplicationVersion", k, idx + 1);
                highlightKeyword(dataGridViewStagingEnvironmentModuleCache, "dataGridViewStagingEnvironmentModuleCache", k, idx + 1);
                highlightKeyword(dataGridViewStagingEnvironmentModuleRunning, "dataGridViewStagingEnvironmentModuleRunning", k, idx + 1);
                highlightKeyword(dataGridViewStagingModules, "dataGridViewStagingModules", k, idx + 1);
                highlightKeyword(dataGridViewStagingModuleVersionRefererences, "dataGridViewStagingModuleVersionRefererences", k, idx + 1);
                highlightKeyword(dataGridViewStagingProducerElements, "dataGridViewStagingProducerElements", k, idx + 1);
                highlightKeyword(dataGridViewStagingSiteProperties, "dataGridViewStagingSiteProperties", k, idx + 1);
                highlightKeyword(dataGridViewStaginglogs, "dataGridViewStaginglogs", k, idx + 1);
                highlightKeyword(dataGridViewStagingApplicationVersion, "dataGridViewStagingApplicationVersion", k, idx + 1);
                highlightKeyword(dataGridViewStagingMessage, "dataGridViewStagingMessage", k, idx + 1);
                highlightKeyword(dataGridViewStagingModuleInconsistencies, "dataGridViewStagingModuleInconsistencies", k, idx + 1);
                highlightKeyword(dataGridViewStagingModuleVersion, "dataGridViewStagingModuleVersion", k, idx + 1);
                highlightKeyword(dataGridViewStagingModuleVersionPublished, "dataGridViewStagingModuleVersionPublished", k, idx + 1);
                highlightKeyword(dataGridViewStagingModuleVersionUploaded, "dataGridViewStagingModuleVersionUploaded", k, idx + 1);
                highlightKeyword(dataGridViewStagingOptions, "dataGridViewStagingOptions", k, idx + 1);
                highlightKeyword(dataGridViewStagingOutdatedApplication, "dataGridViewStagingOutdatedApplication", k, idx + 1);
                highlightKeyword(dataGridViewStagingOutdatedModule, "dataGridViewStagingOutdatedModule", k, idx + 1);
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPage1;
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPage2;
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPage3;
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPage4;
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage5;
        }

        private void tabPage6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage6;
        }

        private void tabPage7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage7;
        }

        private void tabPage8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage8;
        }

        private void tabPage9_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage9;
        }

        private void tabPage10_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage10;
        }

        private void tabPage11_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage11;
        }

        private void tabPage12_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage12;
        }

        private void tabPage13_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage13;
        }

        private void tabPage14_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedTab = tabPage14;
        }

        private void tabPage15_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedTab = tabPage15;
        }

        private void tabPage16_Click(object sender, EventArgs e)
        {
            tabControl2.SelectedTab = tabPage16;
        }

        private void tabPage17_Click(object sender, EventArgs e)
        {
            tabControl3.SelectedTab = tabPage17;
        }

        private void tabPage18_Click(object sender, EventArgs e)
        {
            tabControl3.SelectedTab = tabPage18;
        }

        private void tabPage19_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPage19;
        }

        private void tabPage20_Click(object sender, EventArgs e)
        {
            tabControl4.SelectedTab = tabPage20;
        }

        private void tabPage21_Click(object sender, EventArgs e)
        {
            tabControl4.SelectedTab = tabPage21;
        }

        private void tabPage22_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPage22;
        }

        private void tabPage23_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPage23;
        }

        private void tabPage24_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPage24;
        }

        private void tabPage25_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabPage25;
        }

        private void tabPage26_Click(object sender, EventArgs e)
        {
            tabControl5.SelectedTab = tabPage26;
        }

        private void tabPage27_Click(object sender, EventArgs e)
        {
            tabControl5.SelectedTab = tabPage27;
        }

        private void tabPage28_Click(object sender, EventArgs e)
        {
            tabControl5.SelectedTab = tabPage28;
        }

        private void tabPage29_Click(object sender, EventArgs e)
        {
            tabControl6.SelectedTab = tabPage29;
        }

        private void tabPage30_Click(object sender, EventArgs e)
        {
            tabControl6.SelectedTab = tabPage30;
        }

        private void tabPage31_Click(object sender, EventArgs e)
        {
            tabControl6.SelectedTab = tabPage31;
        }

        private void tabPage32_Click(object sender, EventArgs e)
        {
            tabControl6.SelectedTab = tabPage32;
        }

        private void tabPage33_Click(object sender, EventArgs e)
        {
            tabControl6.SelectedTab = tabPage33;
        }

        private void tabPage34_Click(object sender, EventArgs e)
        {
            tabControl6.SelectedTab = tabPage34;
        }

        private void tabPage35_Click(object sender, EventArgs e)
        {
            tabControl7.SelectedTab = tabPage35;
        }

        private void tabPage36_Click(object sender, EventArgs e)
        {
            tabControl7.SelectedTab = tabPage36;
        }

        private void tabPage37_Click(object sender, EventArgs e)
        {
            tabControl7.SelectedTab = tabPage37;
        }

        private void tabPage38_Click(object sender, EventArgs e)
        {
            tabControl7.SelectedTab = tabPage38;
        }

        private void tabPage39_Click(object sender, EventArgs e)
        {
            tabControl7.SelectedTab = tabPage39;
        }

        private void tabPage40_Click(object sender, EventArgs e)
        {
            tabControl7.SelectedTab = tabPage40;
        }

        private void tabPage41_Click(object sender, EventArgs e)
        {
            tabControl7.SelectedTab = tabPage41;
        }

        private void tabPage42_Click(object sender, EventArgs e)
        {
            tabControl7.SelectedTab = tabPage42;
        }

        private void tabPage43_Click(object sender, EventArgs e)
        {
            tabControl7.SelectedTab = tabPage43;
        }

        private void tabPage44_Click(object sender, EventArgs e)
        {
            tabControl7.SelectedTab = tabPage44;
        }

        private void tabPage45_Click(object sender, EventArgs e)
        {
            tabControl8.SelectedTab = tabPage45;
        }

        private void tabPage46_Click(object sender, EventArgs e)
        {
            tabControl8.SelectedTab = tabPage46;
        }

        private void tabPage47_Click(object sender, EventArgs e)
        {
            tabControl8.SelectedTab = tabPage47;
        }

        private void tabPage48_Click(object sender, EventArgs e)
        {
            tabControl8.SelectedTab = tabPage48;
        }

        private void tabPage49_Click(object sender, EventArgs e)
        {
            tabControl8.SelectedTab = tabPage49;
        }

        private void tabPage50_Click(object sender, EventArgs e)
        {
            tabControl8.SelectedTab = tabPage50;
        }

        private void tabPage51_Click(object sender, EventArgs e)
        {
            tabControl8.SelectedTab = tabPage51;
        }

        private void tabPage52_Click(object sender, EventArgs e)
        {
            tabControl8.SelectedTab = tabPage52;
        }

        private void tabPage53_Click(object sender, EventArgs e)
        {
            tabControl8.SelectedTab = tabPage53;
        }

        private void tabPage54_Click(object sender, EventArgs e)
        {
            tabControl8.SelectedTab = tabPage54;
        }

        private void tabPage55_Click(object sender, EventArgs e)
        {
            tabControl9.SelectedTab = tabPage55;
        }

        private void tabPage56_Click(object sender, EventArgs e)
        {
            tabControl9.SelectedTab = tabPage56;
        }

        private void tabPage57_Click(object sender, EventArgs e)
        {
            tabControl9.SelectedTab = tabPage57;
        }

        private void tabPage58_Click(object sender, EventArgs e)
        {
            tabControl9.SelectedTab = tabPage58;
        }

        private void tabPage59_Click(object sender, EventArgs e)
        {
            tabControl9.SelectedTab = tabPage59;
        }

        private void tabPage61_Click(object sender, EventArgs e)
        {
            tabControl10.SelectedTab = tabPage61;
        }

        private void tabPage62_Click(object sender, EventArgs e)
        {
            tabControl10.SelectedTab = tabPage62;
        }

        private void forceTabClick(string dgvName)
        {
            object sender = new object();
            tabControlAction = TabControlAction.Selected;

            if (dgvName == "dataGridViewErrorlogs")
            {
                tabControlEventArgsTab1 = new TabControlEventArgs(tabPage1, 0, tabControlAction);
                tabPage1_MyClick(sender, tabControlEventArgsTab1);
                tabPage1_Click(sender, tabControlEventArgsTab1);

                tabControlEventArgsTab5 = new TabControlEventArgs(tabPage5, 1, tabControlAction);
                tabPage5_MyClick(sender, tabControlEventArgsTab5);
                tabPage5_Click(sender, tabControlEventArgsTab5);
            }
            else if (dgvName == "dataGridViewGenerallogs")
            {
                tabControlEventArgsTab1 = new TabControlEventArgs(tabPage1, 0, tabControlAction);
                tabPage1_MyClick(sender, tabControlEventArgsTab1);
                tabPage1_Click(sender, tabControlEventArgsTab1);

                tabControlEventArgsTab6 = new TabControlEventArgs(tabPage6, 1, tabControlAction);
                tabPage6_MyClick(sender, tabControlEventArgsTab6);
                tabPage6_Click(sender, tabControlEventArgsTab6);
            }
            else if (dgvName == "dataGridViewSlowSQLlogs" || dgvName == "dataGridViewSlowSQLDurationlogs")
            {
                tabControlEventArgsTab1 = new TabControlEventArgs(tabPage1, 0, tabControlAction);
                tabPage1_MyClick(sender, tabControlEventArgsTab1);
                tabPage1_Click(sender, tabControlEventArgsTab1);

                tabControlEventArgsTab6 = new TabControlEventArgs(tabPage6, 1, tabControlAction);
                tabPage6_MyClick(sender, tabControlEventArgsTab6);
                tabPage6_Click(sender, tabControlEventArgsTab6);

                tabControlEventArgsTab61 = new TabControlEventArgs(tabPage61, 2, tabControlAction);
                tabPage61_MyClick(sender, tabControlEventArgsTab61);
                tabPage61_Click(sender, tabControlEventArgsTab61);
            }
            else if (dgvName == "dataGridViewSlowExtensionlogs" || dgvName == "dataGridViewSlowExtensionDurationlogs")
            {
                tabControlEventArgsTab1 = new TabControlEventArgs(tabPage1, 0, tabControlAction);
                tabPage1_MyClick(sender, tabControlEventArgsTab1);
                tabPage1_Click(sender, tabControlEventArgsTab1);

                tabControlEventArgsTab6 = new TabControlEventArgs(tabPage6, 1, tabControlAction);
                tabPage6_MyClick(sender, tabControlEventArgsTab6);
                tabPage6_Click(sender, tabControlEventArgsTab6);

                tabControlEventArgsTab62 = new TabControlEventArgs(tabPage62, 2, tabControlAction);
                tabPage62_MyClick(sender, tabControlEventArgsTab62);
                tabPage62_Click(sender, tabControlEventArgsTab62);
            }
            else if (dgvName == "dataGridViewIntegrationslogs" || dgvName == "dataGridViewIntWebServiceslogs" || dgvName == "dataGridViewInWebServicesDurationlogs")
            {
                tabControlEventArgsTab1 = new TabControlEventArgs(tabPage1, 0, tabControlAction);
                tabPage1_MyClick(sender, tabControlEventArgsTab1);
                tabPage1_Click(sender, tabControlEventArgsTab1);

                tabControlEventArgsTab7 = new TabControlEventArgs(tabPage7, 1, tabControlAction);
                tabPage7_MyClick(sender, tabControlEventArgsTab7);
                tabPage7_Click(sender, tabControlEventArgsTab7);
            }
            else if (dgvName == "dataGridViewScreenRequestslogs" || dgvName == "dataGridViewScreenRequestsScreenlogs" || dgvName == "dataGridViewScrReqScreenDurationlogs")
            {
                tabControlEventArgsTab1 = new TabControlEventArgs(tabPage1, 0, tabControlAction);
                tabPage1_MyClick(sender, tabControlEventArgsTab1);
                tabPage1_Click(sender, tabControlEventArgsTab1);

                tabControlEventArgsTab8 = new TabControlEventArgs(tabPage8, 1, tabControlAction);
                tabPage8_MyClick(sender, tabControlEventArgsTab8);
                tabPage8_Click(sender, tabControlEventArgsTab8);
            }
            else if (dgvName == "dataGridViewTimerlogs" || dgvName == "dataGridViewTimerTimerslogs" || dgvName == "dataGridViewTimerTimersDurationlogs")
            {
                tabControlEventArgsTab1 = new TabControlEventArgs(tabPage1, 0, tabControlAction);
                tabPage1_MyClick(sender, tabControlEventArgsTab1);
                tabPage1_Click(sender, tabControlEventArgsTab1);

                tabControlEventArgsTab9 = new TabControlEventArgs(tabPage9, 1, tabControlAction);
                tabPage9_MyClick(sender, tabControlEventArgsTab9);
                tabPage9_Click(sender, tabControlEventArgsTab9);
            }
            else if (dgvName == "dataGridViewEmaillogs" || dgvName == "dataGridViewEmailEmailslogs" || dgvName == "dataGridViewEmailEmailsDurationlogs")
            {
                tabControlEventArgsTab1 = new TabControlEventArgs(tabPage1, 0, tabControlAction);
                tabPage1_MyClick(sender, tabControlEventArgsTab1);
                tabPage1_Click(sender, tabControlEventArgsTab1);

                tabControlEventArgsTab10 = new TabControlEventArgs(tabPage10, 1, tabControlAction);
                tabPage10_MyClick(sender, tabControlEventArgsTab10);
                tabPage10_Click(sender, tabControlEventArgsTab10);
            }
            else if (dgvName == "dataGridViewExtensionlogs" || dgvName == "dataGridViewExtensionLogsExtensions" || dgvName == "dataGridViewExtensionsDurationlogs")
            {
                tabControlEventArgsTab1 = new TabControlEventArgs(tabPage1, 0, tabControlAction);
                tabPage1_MyClick(sender, tabControlEventArgsTab1);
                tabPage1_Click(sender, tabControlEventArgsTab1);

                tabControlEventArgsTab11 = new TabControlEventArgs(tabPage11, 1, tabControlAction);
                tabPage11_MyClick(sender, tabControlEventArgsTab11);
                tabPage11_Click(sender, tabControlEventArgsTab11);
            }
            else if (dgvName == "dataGridViewServiceActionlogs" || dgvName == "dataGridViewSrvActServicelogs" || dgvName == "dataGridViewSrvActServiceDurationlogs")
            {
                tabControlEventArgsTab1 = new TabControlEventArgs(tabPage1, 0, tabControlAction);
                tabPage1_MyClick(sender, tabControlEventArgsTab1);
                tabPage1_Click(sender, tabControlEventArgsTab1);

                tabControlEventArgsTab12 = new TabControlEventArgs(tabPage12, 1, tabControlAction);
                tabPage12_MyClick(sender, tabControlEventArgsTab12);
                tabPage12_Click(sender, tabControlEventArgsTab12);
            }
            else if (dgvName == "dataGridViewTradWebRequests" || dgvName == "dataGridViewTradWebRequestsScreenlogs" || dgvName == "dataGridViewTradWebRequestsScreenDurationlogs")
            {
                tabControlEventArgsTab1 = new TabControlEventArgs(tabPage1, 0, tabControlAction);
                tabPage1_MyClick(sender, tabControlEventArgsTab1);
                tabPage1_Click(sender, tabControlEventArgsTab1);

                tabControlEventArgsTab13 = new TabControlEventArgs(tabPage13, 1, tabControlAction);
                tabPage13_MyClick(sender, tabControlEventArgsTab13);
                tabPage13_Click(sender, tabControlEventArgsTab13);
            }
            else if (dgvName == "dataGridViewIISDateTime" || dgvName == "dataGridViewIISTimeTaken")
            {
                tabControlEventArgsTab2 = new TabControlEventArgs(tabPage2, 0, tabControlAction);
                tabPage2_MyClick(sender, tabControlEventArgsTab2);
                tabPage2_Click(sender, tabControlEventArgsTab2);
            }
            else if (dgvName == "dataGridViewWinAppEventViewer")
            {
                tabControlEventArgsTab3 = new TabControlEventArgs(tabPage3, 0, tabControlAction);
                tabPage3_MyClick(sender, tabControlEventArgsTab3);
                tabPage3_Click(sender, tabControlEventArgsTab3);

                tabControlEventArgsTab14 = new TabControlEventArgs(tabPage14, 1, tabControlAction);
                tabPage14_MyClick(sender, tabControlEventArgsTab14);
                tabPage14_Click(sender, tabControlEventArgsTab14);
            }
            else if (dgvName == "dataGridViewWinSysEventViewer")
            {
                tabControlEventArgsTab3 = new TabControlEventArgs(tabPage3, 0, tabControlAction);
                tabPage3_MyClick(sender, tabControlEventArgsTab3);
                tabPage3_Click(sender, tabControlEventArgsTab3);

                tabControlEventArgsTab15 = new TabControlEventArgs(tabPage15, 1, tabControlAction);
                tabPage15_MyClick(sender, tabControlEventArgsTab15);
                tabPage15_Click(sender, tabControlEventArgsTab15);
            }
            else if (dgvName == "dataGridViewWinSecEventViewer")
            {
                tabControlEventArgsTab3 = new TabControlEventArgs(tabPage3, 0, tabControlAction);
                tabPage3_MyClick(sender, tabControlEventArgsTab3);
                tabPage3_Click(sender, tabControlEventArgsTab3);

                tabControlEventArgsTab16 = new TabControlEventArgs(tabPage16, 1, tabControlAction);
                tabPage16_MyClick(sender, tabControlEventArgsTab16);
                tabPage16_Click(sender, tabControlEventArgsTab16);
            }
            else if (dgvName == "dataGridViewAndroidlogs")
            {
                tabControlEventArgsTab4 = new TabControlEventArgs(tabPage4, 0, tabControlAction);
                tabPage4_MyClick(sender, tabControlEventArgsTab4);
                tabPage4_Click(sender, tabControlEventArgsTab4);

                tabControlEventArgsTab17 = new TabControlEventArgs(tabPage17, 1, tabControlAction);
                tabPage17_MyClick(sender, tabControlEventArgsTab17);
                tabPage17_Click(sender, tabControlEventArgsTab17);
            }
            else if (dgvName == "dataGridViewiOSlogs")
            {
                tabControlEventArgsTab4 = new TabControlEventArgs(tabPage4, 0, tabControlAction);
                tabPage4_MyClick(sender, tabControlEventArgsTab4);
                tabPage4_Click(sender, tabControlEventArgsTab4);

                tabControlEventArgsTab18 = new TabControlEventArgs(tabPage18, 1, tabControlAction);
                tabPage18_MyClick(sender, tabControlEventArgsTab18);
                tabPage18_Click(sender, tabControlEventArgsTab18);
            }
            else if (dgvName == "dataGridViewServiceStudiologs")
            {
                tabControlEventArgsTab19 = new TabControlEventArgs(tabPage19, 0, tabControlAction);
                tabPage19_MyClick(sender, tabControlEventArgsTab19);
                tabPage19_Click(sender, tabControlEventArgsTab19);

                tabControlEventArgsTab20 = new TabControlEventArgs(tabPage20, 1, tabControlAction);
                tabPage20_MyClick(sender, tabControlEventArgsTab20);
                tabPage20_Click(sender, tabControlEventArgsTab20);
            }
            else if (dgvName == "dataGridViewGeneralTXTlogs")
            {
                tabControlEventArgsTab19 = new TabControlEventArgs(tabPage19, 0, tabControlAction);
                tabPage19_MyClick(sender, tabControlEventArgsTab19);
                tabPage19_Click(sender, tabControlEventArgsTab19);

                tabControlEventArgsTab21 = new TabControlEventArgs(tabPage21, 1, tabControlAction);
                tabPage21_MyClick(sender, tabControlEventArgsTab21);
                tabPage21_Click(sender, tabControlEventArgsTab21);
            }
            else if (dgvName == "dataGridViewBPTReportslogs")
            {
                tabControlEventArgsTab22 = new TabControlEventArgs(tabPage22, 0, tabControlAction);
                tabPage22_MyClick(sender, tabControlEventArgsTab22);
                tabPage22_Click(sender, tabControlEventArgsTab22);
            }
            else if (dgvName == "dataGridViewEnvironmentCapabilitieslogs")
            {
                tabControlEventArgsTab24 = new TabControlEventArgs(tabPage24, 0, tabControlAction);
                tabPage24_MyClick(sender, tabControlEventArgsTab24);
                tabPage24_Click(sender, tabControlEventArgsTab24);

                tabControlEventArgsTab26 = new TabControlEventArgs(tabPage26, 1, tabControlAction);
                tabPage26_MyClick(sender, tabControlEventArgsTab26);
                tabPage26_Click(sender, tabControlEventArgsTab26);
            }
            else if (dgvName == "dataGridViewEnvironmentslogs")
            {
                tabControlEventArgsTab24 = new TabControlEventArgs(tabPage24, 0, tabControlAction);
                tabPage24_MyClick(sender, tabControlEventArgsTab24);
                tabPage24_Click(sender, tabControlEventArgsTab24);

                tabControlEventArgsTab27 = new TabControlEventArgs(tabPage27, 1, tabControlAction);
                tabPage27_MyClick(sender, tabControlEventArgsTab27);
                tabPage27_Click(sender, tabControlEventArgsTab27);
            }
            else if (dgvName == "dataGridViewFullErrorDumps")
            {
                tabControlEventArgsTab24 = new TabControlEventArgs(tabPage24, 0, tabControlAction);
                tabPage24_MyClick(sender, tabControlEventArgsTab24);
                tabPage24_Click(sender, tabControlEventArgsTab24);

                tabControlEventArgsTab28 = new TabControlEventArgs(tabPage28, 1, tabControlAction);
                tabPage28_MyClick(sender, tabControlEventArgsTab28);
                tabPage28_Click(sender, tabControlEventArgsTab28);
            }
            else if (dgvName == "dataGridViewRoleslogs")
            {
                tabControlEventArgsTab25 = new TabControlEventArgs(tabPage25, 0, tabControlAction);
                tabPage25_MyClick(sender, tabControlEventArgsTab25);
                tabPage25_Click(sender, tabControlEventArgsTab25);

                tabControlEventArgsTab29 = new TabControlEventArgs(tabPage29, 1, tabControlAction);
                tabPage29_MyClick(sender, tabControlEventArgsTab29);
                tabPage29_Click(sender, tabControlEventArgsTab29);
            }
            else if (dgvName == "dataGridViewRolesInApplicationslogs")
            {
                tabControlEventArgsTab25 = new TabControlEventArgs(tabPage25, 0, tabControlAction);
                tabPage25_MyClick(sender, tabControlEventArgsTab25);
                tabPage25_Click(sender, tabControlEventArgsTab25);

                tabControlEventArgsTab30 = new TabControlEventArgs(tabPage30, 1, tabControlAction);
                tabPage30_MyClick(sender, tabControlEventArgsTab30);
                tabPage30_Click(sender, tabControlEventArgsTab30);
            }
            else if (dgvName == "dataGridViewRolesInTeamslogs")
            {
                tabControlEventArgsTab25 = new TabControlEventArgs(tabPage25, 0, tabControlAction);
                tabPage25_MyClick(sender, tabControlEventArgsTab25);
                tabPage25_Click(sender, tabControlEventArgsTab25);

                tabControlEventArgsTab31 = new TabControlEventArgs(tabPage31, 1, tabControlAction);
                tabPage31_MyClick(sender, tabControlEventArgsTab31);
                tabPage31_Click(sender, tabControlEventArgsTab31);
            }
            else if (dgvName == "dataGridViewUserlogs")
            {
                tabControlEventArgsTab25 = new TabControlEventArgs(tabPage25, 0, tabControlAction);
                tabPage25_MyClick(sender, tabControlEventArgsTab25);
                tabPage25_Click(sender, tabControlEventArgsTab25);

                tabControlEventArgsTab33 = new TabControlEventArgs(tabPage33, 1, tabControlAction);
                tabPage33_MyClick(sender, tabControlEventArgsTab33);
                tabPage33_Click(sender, tabControlEventArgsTab33);
            }
            else if (dgvName == "dataGridViewUserPoolslogs")
            {
                tabControlEventArgsTab25 = new TabControlEventArgs(tabPage25, 0, tabControlAction);
                tabPage25_MyClick(sender, tabControlEventArgsTab25);
                tabPage25_Click(sender, tabControlEventArgsTab25);

                tabControlEventArgsTab34 = new TabControlEventArgs(tabPage34, 1, tabControlAction);
                tabPage34_MyClick(sender, tabControlEventArgsTab34);
                tabPage34_Click(sender, tabControlEventArgsTab34);
            }
            else if (dgvName == "dataGridViewSyncErrorslogs")
            {
                tabControlEventArgsTab25 = new TabControlEventArgs(tabPage25, 0, tabControlAction);
                tabPage25_MyClick(sender, tabControlEventArgsTab25);
                tabPage25_Click(sender, tabControlEventArgsTab25);

                tabControlEventArgsTab32 = new TabControlEventArgs(tabPage32, 1, tabControlAction);
                tabPage32_MyClick(sender, tabControlEventArgsTab32);
                tabPage32_Click(sender, tabControlEventArgsTab32);
            }
            else if (dgvName == "dataGridViewStagingApplogs")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab35 = new TabControlEventArgs(tabPage35, 1, tabControlAction);
                tabPage35_MyClick(sender, tabControlEventArgsTab35);
                tabPage35_Click(sender, tabControlEventArgsTab35);
            }
            else if (dgvName == "dataGridViewStagingAppVerlogs")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab36 = new TabControlEventArgs(tabPage36, 1, tabControlAction);
                tabPage36_MyClick(sender, tabControlEventArgsTab36);
                tabPage36_Click(sender, tabControlEventArgsTab36);
            }
            else if (dgvName == "dataGridViewStagingAppVerModuleVerlogs")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab37 = new TabControlEventArgs(tabPage37, 1, tabControlAction);
                tabPage37_MyClick(sender, tabControlEventArgsTab37);
                tabPage37_Click(sender, tabControlEventArgsTab37);
            }
            else if (dgvName == "dataGridViewStagingChangelog")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab38 = new TabControlEventArgs(tabPage38, 1, tabControlAction);
                tabPage38_MyClick(sender, tabControlEventArgsTab38);
                tabPage38_Click(sender, tabControlEventArgsTab38);
            }
            else if (dgvName == "dataGridViewStagingConsumerElements")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab39 = new TabControlEventArgs(tabPage39, 1, tabControlAction);
                tabPage39_MyClick(sender, tabControlEventArgsTab39);
                tabPage39_Click(sender, tabControlEventArgsTab39);
            }
            else if (dgvName == "dataGridViewStagingEntityConfiguration")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab40 = new TabControlEventArgs(tabPage40, 1, tabControlAction);
                tabPage40_MyClick(sender, tabControlEventArgsTab40);
                tabPage40_Click(sender, tabControlEventArgsTab40);
            }
            else if (dgvName == "dataGridViewStagingEnvironmentAppicationCache")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab41 = new TabControlEventArgs(tabPage41, 1, tabControlAction);
                tabPage41_MyClick(sender, tabControlEventArgsTab41);
                tabPage41_Click(sender, tabControlEventArgsTab41);
            }
            else if (dgvName == "dataGridViewStagingEnvironmentApplicationModule")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab42 = new TabControlEventArgs(tabPage42, 1, tabControlAction);
                tabPage42_MyClick(sender, tabControlEventArgsTab42);
                tabPage42_Click(sender, tabControlEventArgsTab42);
            }
            else if (dgvName == "dataGridViewStagingEnvironmentApplicationVersion")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab43 = new TabControlEventArgs(tabPage43, 1, tabControlAction);
                tabPage43_MyClick(sender, tabControlEventArgsTab43);
                tabPage43_Click(sender, tabControlEventArgsTab43);
            }
            else if (dgvName == "dataGridViewStagingEnvironmentModuleCache")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab44 = new TabControlEventArgs(tabPage44, 1, tabControlAction);
                tabPage44_MyClick(sender, tabControlEventArgsTab44);
                tabPage44_Click(sender, tabControlEventArgsTab44);
            }
            else if (dgvName == "dataGridViewStagingEnvironmentModuleRunning")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab45 = new TabControlEventArgs(tabPage45, 1, tabControlAction);
                tabPage45_MyClick(sender, tabControlEventArgsTab45);
                tabPage45_Click(sender, tabControlEventArgsTab45);
            }
            else if (dgvName == "dataGridViewStagingModules")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab46 = new TabControlEventArgs(tabPage46, 1, tabControlAction);
                tabPage46_MyClick(sender, tabControlEventArgsTab46);
                tabPage46_Click(sender, tabControlEventArgsTab46);
            }
            else if (dgvName == "dataGridViewStagingModuleVersionRefererences")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab47 = new TabControlEventArgs(tabPage47, 1, tabControlAction);
                tabPage47_MyClick(sender, tabControlEventArgsTab47);
                tabPage47_Click(sender, tabControlEventArgsTab47);
            }
            else if (dgvName == "dataGridViewStagingProducerElements")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab48 = new TabControlEventArgs(tabPage48, 1, tabControlAction);
                tabPage48_MyClick(sender, tabControlEventArgsTab48);
                tabPage48_Click(sender, tabControlEventArgsTab48);
            }
            else if (dgvName == "dataGridViewStagingSiteProperties")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab49 = new TabControlEventArgs(tabPage49, 1, tabControlAction);
                tabPage49_MyClick(sender, tabControlEventArgsTab49);
                tabPage49_Click(sender, tabControlEventArgsTab49);
            }
            else if (dgvName == "dataGridViewStaginglogs")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab50 = new TabControlEventArgs(tabPage50, 1, tabControlAction);
                tabPage50_MyClick(sender, tabControlEventArgsTab50);
                tabPage50_Click(sender, tabControlEventArgsTab50);
            }
            else if (dgvName == "dataGridViewStagingApplicationVersion")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab51 = new TabControlEventArgs(tabPage51, 1, tabControlAction);
                tabPage51_MyClick(sender, tabControlEventArgsTab51);
                tabPage51_Click(sender, tabControlEventArgsTab51);
            }
            else if (dgvName == "dataGridViewStagingMessage")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab52 = new TabControlEventArgs(tabPage52, 1, tabControlAction);
                tabPage52_MyClick(sender, tabControlEventArgsTab52);
                tabPage52_Click(sender, tabControlEventArgsTab52);
            }
            else if (dgvName == "dataGridViewStagingModuleInconsistencies")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab53 = new TabControlEventArgs(tabPage53, 1, tabControlAction);
                tabPage53_MyClick(sender, tabControlEventArgsTab53);
                tabPage53_Click(sender, tabControlEventArgsTab53);
            }
            else if (dgvName == "dataGridViewStagingModuleVersion")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab54 = new TabControlEventArgs(tabPage54, 1, tabControlAction);
                tabPage54_MyClick(sender, tabControlEventArgsTab54);
                tabPage54_Click(sender, tabControlEventArgsTab54);
            }
            else if (dgvName == "dataGridViewStagingModuleVersionPublished")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab55 = new TabControlEventArgs(tabPage55, 1, tabControlAction);
                tabPage55_MyClick(sender, tabControlEventArgsTab55);
                tabPage55_Click(sender, tabControlEventArgsTab55);
            }
            else if (dgvName == "dataGridViewStagingModuleVersionUploaded")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab56 = new TabControlEventArgs(tabPage56, 1, tabControlAction);
                tabPage56_MyClick(sender, tabControlEventArgsTab56);
                tabPage56_Click(sender, tabControlEventArgsTab56);
            }
            else if (dgvName == "dataGridViewStagingOptions")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab57 = new TabControlEventArgs(tabPage57, 1, tabControlAction);
                tabPage57_MyClick(sender, tabControlEventArgsTab57);
                tabPage57_Click(sender, tabControlEventArgsTab57);
            }
            else if (dgvName == "dataGridViewStagingOutdatedApplication")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab58 = new TabControlEventArgs(tabPage58, 1, tabControlAction);
                tabPage58_MyClick(sender, tabControlEventArgsTab58);
                tabPage58_Click(sender, tabControlEventArgsTab58);
            }
            else if (dgvName == "dataGridViewStagingOutdatedModule")
            {
                tabControlEventArgsTab23 = new TabControlEventArgs(tabPage23, 0, tabControlAction);
                tabPage23_MyClick(sender, tabControlEventArgsTab23);
                tabPage23_Click(sender, tabControlEventArgsTab23);

                tabControlEventArgsTab59 = new TabControlEventArgs(tabPage59, 1, tabControlAction);
                tabPage59_MyClick(sender, tabControlEventArgsTab59);
                tabPage59_Click(sender, tabControlEventArgsTab59);
            }
        }

        private void btnExportSlowSQLExtensionTables_Click(object sender, EventArgs e)
        {
            if (rdBtnSortSlowSQLExtension.Checked == true)
            {
                exportTableContent(dataGridViewSlowSQLDurationlogs, "\\slow_sql_table_sort_duration.csv", "|");
                exportTableContent(dataGridViewSlowExtensionDurationlogs, "\\slow_extension_table_sort_duration.csv", "|");
            }
            else if (rdBtnFilterErrorIDSLowSQLExtension.Checked == true)
            {
                exportTableContent(dataGridViewSlowSQLDurationlogs, "\\slow_sql_table_filter_with_error_id.csv", "|");
                exportTableContent(dataGridViewSlowExtensionDurationlogs, "\\slow_extension_table_filter_with_error_id.csv", "|");
            }
            else if (rdBtnSortFilterErrorIDSlowSQLExtension.Checked == true)
            {
                exportTableContent(dataGridViewSlowSQLDurationlogs, "\\slow_sql_table_sort_duration_filter_with_error_id.csv", "|");
                exportTableContent(dataGridViewSlowExtensionDurationlogs, "\\slow_extension_table_sort_duration_filter_with_error_id.csv", "|");
            }
        }

        private void btnExportWebServiceTable_Click(object sender, EventArgs e)
        {
            if (rdBtnSortWebServices.Checked == true)
            {
                exportTableContent(dataGridViewInWebServicesDurationlogs, "\\webservices_table_sort_duration.csv", "|");
            }
            else if (rdBtnFilterErrorIDWebServices.Checked == true)
            {
                exportTableContent(dataGridViewInWebServicesDurationlogs, "\\webservices_table_filter_with_error_id.csv", "|");
            }
            else if (rdBtnSortFilterErrorIDWebServices.Checked == true)
            {
                exportTableContent(dataGridViewInWebServicesDurationlogs, "\\webservices_table_sort_duration_filter_with_error_id.csv", "|");
            }
        }

        private void btnExportTimerTable_Click(object sender, EventArgs e)
        {
            if (rdBtnSortTimers.Checked == true)
            {
                exportTableContent(dataGridViewTimerTimersDurationlogs, "\\timers_table_sort_duration.csv", "|");
            }
            else if (rdBtnFilterErrorIDTimers.Checked == true)
            {
                exportTableContent(dataGridViewTimerTimersDurationlogs, "\\timers_table_filter_with_error_id.csv", "|");
            }
            else if (rdBtnSortFilterErrorIDTimers.Checked == true)
            {
                exportTableContent(dataGridViewTimerTimersDurationlogs, "\\timers_table_sort_duration_filter_with_error_id.csv", "|");
            }
        }

        private void btnExportScreenTable_Click(object sender, EventArgs e)
        {
            exportTableContent(dataGridViewTradWebRequestsScreenDurationlogs, "\\screens_table.csv", "|");
        }

        private void exportTableContent(DataGridView tableName, string txtFile, string delimiter)
        {
            if (tableName.Rows.Count > 0)
            {
                outputCSVfile = label8.Text + txtFile;

                try
                {
                    //This line of code creates a csv file for the data export.
                    StreamWriter exportFile = new StreamWriter(outputCSVfile);
                    string eLine = "";

                    //This for loop loops through the headers
                    for (int h = 0; h <= tableName.Columns.Count - 1; h++)
                    {
                        eLine = eLine + tableName.Columns[h].HeaderText;
                        if (h != tableName.Columns.Count - 1)
                        {
                            //Add a text delimiter in order
                            //to separate each field in the csv file.
                            eLine = eLine + delimiter;
                        }
                        else
                        {
                            //The exported text is written to the csv file, one line at a time.
                            exportFile.WriteLine(eLine);
                            eLine = "";
                        }
                    }

                    //This for loop loops through each row in the table
                    for (int r = 0; r <= tableName.Rows.Count - 1; r++)
                    {
                        //This for loop loops through each column, and the row number
                        //is passed from the for loop above.
                        for (int c = 0; c <= tableName.Columns.Count - 1; c++)
                        {
                            eLine = eLine + tableName.Rows[r].Cells[c].Value;
                            if (c != tableName.Columns.Count - 1)
                            {
                                //Add a csv delimiter in order
                                //to separate each field in the csv file.
                                eLine = eLine + delimiter;
                            }
                        }
                        //The exported text is written to the csv file, one line at a time.
                        exportFile.WriteLine(eLine);
                        eLine = "";
                    }

                    exportFile.Close();
                    MessageBox.Show("Exported the data to the following file:" + Environment.NewLine + outputCSVfile, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void exportRowInfo(string fldName, string kWord, string rowVal)
        {
            try
            {
                //create a folder
                string outputFolder = Path.Combine(label8.Text, fldName);
                Directory.CreateDirectory(outputFolder);

                //replace illegal characters in filename
                kWord = Regex.Replace(kWord, "[\\/:*?<>|\"]", "");

                string kWord2 = kWord.Replace(" ", "_");

                outputCSVfile = outputFolder + "\\" + kWord2.ToLower() + ".csv";

                File.AppendAllText(outputCSVfile, rowVal + Environment.NewLine);
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void comBoxIssueCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comBoxIssueCategory.SelectedIndex > -1)
            {
                if (btnHighlight.Enabled == false)
                {
                    btnHighlight.Enabled = true;
                    btnHighlight.BackColor = SystemColors.ControlLight;
                }
            }
        }

        private void btnExportExtensionsTable_Click(object sender, EventArgs e)
        {
            if (rdBtnSortExtensions.Checked == true)
            {
                exportTableContent(dataGridViewExtensionsDurationlogs, "\\extensions_table_sort_duration.csv", "|");
            }
            else if (rdBtnFilterErrorIDExtensions.Checked == true)
            {
                exportTableContent(dataGridViewExtensionsDurationlogs, "\\extensions_table_filter_with_error_id.csv", "|");
            }
            else if (rdBtnSortFilterErrorIDExtensions.Checked == true)
            {
                exportTableContent(dataGridViewExtensionsDurationlogs, "\\extensions_table_sort_duration_filter_with_error_id.csv", "|");
            }
        }

        private void btnExportScrReqScreenTable_Click(object sender, EventArgs e)
        {
            if (rdBtnScrReqScreens.Checked == true)
            {
                exportTableContent(dataGridViewScrReqScreenDurationlogs, "\\mobile_requests_screen_table_sort_duration.csv", "|");
            }
            else if (rdBtnFilterErrorIDScrReqScreens.Checked == true)
            {
                exportTableContent(dataGridViewScrReqScreenDurationlogs, "\\mobile_requests_screen_table_filter_with_error_id.csv", "|");
            }
            else if (rdBtnSortFilterErrorIDScrReqScreens.Checked == true)
            {
                exportTableContent(dataGridViewScrReqScreenDurationlogs, "\\mobile_requests_screen_table_sort_duration_filter_with_error_id.csv", "|");
            }
        }

        private void btnExportServiceActionsTable_Click(object sender, EventArgs e)
        {
            if (rdBtnSortServiceActions.Checked == true)
            {
                exportTableContent(dataGridViewSrvActServiceDurationlogs, "\\service_actions_table_sort_duration.csv", "|");
            }
            else if (rdBtnFilterErrorIDServiceActions.Checked == true)
            {
                exportTableContent(dataGridViewSrvActServiceDurationlogs, "\\service_actions_table_filter_with_error_id.csv", "|");
            }
            else if (rdBtnSortFilterErrorIDServiceActions.Checked == true)
            {
                exportTableContent(dataGridViewSrvActServiceDurationlogs, "\\service_actions_table_sort_duration_filter_with_error_id.csv", "|");
            }
        }

        private void chkBoxSortIIS_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkBoxSortIIS.Checked == true)
                {
                    var sortIISDuration = (dataGridViewIISDateTime.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null && r.Cells[10].Value != null && r.Cells[11].Value != null && r.Cells[12].Value != null && r.Cells[13].Value != null)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, HttpCode = r.Cells[2].Value, HttpSubcode = r.Cells[3].Value, WindowsErrorCode = r.Cells[4].Value, Clientip = r.Cells[5].Value, Serverip = r.Cells[6].Value, ServerPort = r.Cells[7].Value, Method = r.Cells[8].Value, UriStem = r.Cells[9].Value, UriQuery = r.Cells[10].Value, Username = r.Cells[11].Value, Browser = r.Cells[12].Value, Referrer = r.Cells[13].Value })
                                .GroupBy(srtiistime => srtiistime)
                                .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                                .Select(g => new { TIME_TAKEN_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, HTTP_CODE = g.Key.HttpCode, HTTP_SUBCODE = g.Key.HttpSubcode, WINDOWS_ERROR_CODE = g.Key.WindowsErrorCode, CLIENT_IP = g.Key.Clientip, SERVER_IP = g.Key.Serverip, SERVER_PORT = g.Key.ServerPort, METHOD = g.Key.Method, URI_STEM = g.Key.UriStem, URI_QUERY = g.Key.UriQuery, USERNAME = g.Key.Username, BROWSER = g.Key.Browser, REFERRER = g.Key.Referrer })).ToList();

                    dataGridViewIISTimeTaken.DataSource = sortIISDuration;

                    btnExportIISTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void chkBoxSortDevinfo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkBoxSortDevinfo.Checked == true)
                {
                    var sortDeviceInformationCount = (dataGridViewDeviceInformation.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null)
                                .Select(r => new { OperatingSystem = r.Cells[0].Value, OperatingSystemVersion = r.Cells[1].Value, Count = r.Cells[2].Value })
                                .GroupBy(srtdevinfo => srtdevinfo)
                                .OrderByDescending(g => Convert.ToInt32(g.Key.Count))
                                .Select(g => new { COUNT = g.Key.Count, OPERATING_SYSTEM = g.Key.OperatingSystem, OPERATING_SYTEM_VERSION = g.Key.OperatingSystemVersion })).ToList();

                    dataGridViewDevInfoCount.DataSource = sortDeviceInformationCount;

                    btnExportDevInfoTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void btnExportIISLINQTable_Click(object sender, EventArgs e)
        {
            reportName = report.Replace(" ", "_").ToLower();
            exportTableContent(dataGridViewIISLINQreport, "\\iis_logs_" + reportName + "_table.csv", "|");
        }

        private void btnExportIISTable_Click(object sender, EventArgs e)
        {
            exportTableContent(dataGridViewIISTimeTaken, "\\iis_logs_timetaken_table.csv", "|");
        }

        private void btnExportDevInfoTable_Click(object sender, EventArgs e)
        {
            exportTableContent(dataGridViewDevInfoCount, "\\device_information_count_table.csv", "|");
        }

        private void btnExportEmailsTable_Click(object sender, EventArgs e)
        {
            if (rdBtnSortEmails.Checked == true)
            {
                exportTableContent(dataGridViewEmailEmailsDurationlogs, "\\emails_table_sort_duration.csv", "|");
            }
            else if (rdBtnFilterErrorIDEmails.Checked == true)
            {
                exportTableContent(dataGridViewEmailEmailsDurationlogs, "\\emails_table_filter_with_error_id.csv", "|");
            }
        }

        private void comBoxField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comBoxField.SelectedIndex > -1)
            {
                string field = comBoxField.Text;

                if (field == "Action Name")
                {
                    loadTXT("filter_action_names.txt");
                }
                else if (field == "Application Name")
                {
                    loadTXT("filter_application_names.txt");
                }
                else if (field == "Cyclic Job Name")
                {
                    loadTXT("filter_cyclic_job_names.txt");
                }
                else if (field == "Espace Name")
                {
                    loadTXT("filter_espace_names.txt");
                }
                else if (field == "Extension Name")
                {
                    loadTXT("filter_extension_names.txt");
                }
                else if (field == "Module Name")
                {
                    loadTXT("filter_module_names.txt");
                }
            }
        }

        private void loadTXT(string txtFile)
        {
            try
            {
                if (comBoxFilterField.Enabled == false)
                {
                    comBoxFilterField.Enabled = true;
                }

                comBoxFilterField.Items.Clear();

                if (lineOfContents != null)
                {
                    Array.Clear(lineOfContents, 0, lineOfContents.Length);
                }

                lineOfContents = File.ReadAllLines(fullPath + "\\" + txtFile);
                foreach (var line in lineOfContents)
                {
                    if (!string.IsNullOrEmpty(line.Trim()))
                    {
                        comBoxFilterField.Items.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void btnFilterFIeld_Click(object sender, EventArgs e)
        {
            //filter the content from the table based on the field selected
            queryDataGridViews2(dataGridViewEmaillogs, txtBoxDetailEmaillogs);
            queryDataGridViews2(dataGridViewEmailEmailslogs, txtBoxDetailsEmailEmailslogs);
            queryDataGridViews2(dataGridViewEmailEmailsDurationlogs, txtBoxDetailsEmailEmailslogs);
            queryDataGridViews2(dataGridViewErrorlogs, txtBoxDetailErrorLogs);
            queryDataGridViews2(dataGridViewExtensionlogs, txtBoxDetailExtensionlogs);
            queryDataGridViews2(dataGridViewExtensionLogsExtensions, txtBoxDetailExtExtensionlogs);
            queryDataGridViews2(dataGridViewExtensionsDurationlogs, txtBoxDetailExtExtensionlogs);
            queryDataGridViews2(dataGridViewGenerallogs, txtBoxDetailGenerallogs);
            queryDataGridViews2(dataGridViewSlowSQLlogs, txtBoxDetailsSlowSQLlogs);
            queryDataGridViews2(dataGridViewSlowSQLDurationlogs, txtBoxDetailsSlowSQLlogs);
            queryDataGridViews2(dataGridViewSlowExtensionlogs, txtBoxDetailsSlowExtensionlogs);
            queryDataGridViews2(dataGridViewSlowSQLDurationlogs, txtBoxDetailsSlowExtensionlogs);
            queryDataGridViews2(dataGridViewIntegrationslogs, txtBoxDetailIntegrationlogs);
            queryDataGridViews2(dataGridViewIntWebServiceslogs, txtBoxDetailsIntWebServiceslogs);
            queryDataGridViews2(dataGridViewInWebServicesDurationlogs, txtBoxDetailsIntWebServiceslogs);
            queryDataGridViews2(dataGridViewScreenRequestslogs, txtBoxDetailScreenRequestslogs);
            queryDataGridViews2(dataGridViewScreenRequestsScreenlogs, txtBoxDetailScrReqScreenlogs);
            queryDataGridViews2(dataGridViewScrReqScreenDurationlogs, txtBoxDetailScrReqScreenlogs);
            queryDataGridViews2(dataGridViewServiceActionlogs, txtBoxDetailServiceActionlogs);
            queryDataGridViews2(dataGridViewSrvActServicelogs, txtBoxDetailSrvActServicelogs);
            queryDataGridViews2(dataGridViewSrvActServiceDurationlogs, txtBoxDetailSrvActServicelogs);
            queryDataGridViews2(dataGridViewTimerlogs, txtBoxDetailTimerlogs);
            queryDataGridViews2(dataGridViewTimerTimerslogs, txtBoxDetailsTimerTimerslogs);
            queryDataGridViews2(dataGridViewTimerTimersDurationlogs, txtBoxDetailsTimerTimerslogs);
            queryDataGridViews2(dataGridViewTradWebRequests, txtBoxDetailTradWebRequests);
            queryDataGridViews2(dataGridViewTradWebRequestsScreenlogs, txtBoxDetailTradWebRequestsScreenlogs);
            queryDataGridViews2(dataGridViewTradWebRequestsScreenDurationlogs, txtBoxDetailTradWebRequestsScreenlogs);

            btnFilterFIeld.Enabled = false;
            comBoxField.Enabled = false;
            comBoxFilterField.Enabled = false;

            if (dataGridViewSlowSQLlogs.Rows.Count == 0 || dataGridViewSlowExtensionlogs.Rows.Count == 0)
            {
                rdBtnSortSlowSQLExtension.Enabled = false;
                rdBtnFilterErrorIDSLowSQLExtension.Enabled = false;
                rdBtnSortFilterErrorIDSlowSQLExtension.Enabled = false;
            }

            if (dataGridViewIntWebServiceslogs.Rows.Count == 0)
            {
                rdBtnSortWebServices.Enabled = false;
                rdBtnFilterErrorIDWebServices.Enabled = false;
                rdBtnSortFilterErrorIDWebServices.Enabled = false;
            }

            if (dataGridViewScreenRequestsScreenlogs.Rows.Count == 0)
            {
                rdBtnScrReqScreens.Enabled = false;
                rdBtnFilterErrorIDScrReqScreens.Enabled = false;
                rdBtnSortFilterErrorIDScrReqScreens.Enabled = false;
            }

            if (dataGridViewTimerTimerslogs.Rows.Count == 0)
            {
                rdBtnSortTimers.Enabled = false;
                rdBtnFilterErrorIDTimers.Enabled = false;
                rdBtnSortFilterErrorIDTimers.Enabled = false;
            }

            if (dataGridViewEmailEmailslogs.Rows.Count == 0)
            {
                rdBtnSortEmails.Enabled = false;
                rdBtnFilterErrorIDEmails.Enabled = false;
            }

            if (dataGridViewExtensionLogsExtensions.Rows.Count == 0)
            {
                rdBtnSortExtensions.Enabled = false;
                rdBtnFilterErrorIDExtensions.Enabled = false;
                rdBtnSortFilterErrorIDExtensions.Enabled = false;
            }

            if (dataGridViewSrvActServicelogs.Rows.Count == 0)
            {
                rdBtnSortServiceActions.Enabled = false;
                rdBtnFilterErrorIDServiceActions.Enabled = false;
                rdBtnSortFilterErrorIDServiceActions.Enabled = false;
            }

            if (dataGridViewTradWebRequestsScreenlogs.Rows.Count == 0)
            {
                chkBoxSortTradWebRequestsScreens.Enabled = false;
            }

            if (bool_removeGarbage)
            {
                removeGarbage();
            }

            if (bool_highlightError)
            {
                if (bool_highlightErrorSuccessful_1 || bool_highlightErrorSuccessful_2 || bool_highlightErrorSuccessful_3 || bool_highlightErrorSuccessful_4 || bool_highlightErrorSuccessful_5 || bool_highlightErrorSuccessful_6)
                {
                    foreach (string c in categorySelected)
                    {
                        highlightError(c);
                    }
                }
            }

            if (bool_findKeyword)
            {
                if (bool_findKeywordSuccessful_1 || bool_findKeywordSuccessful_2 || bool_findKeywordSuccessful_3 || bool_findKeywordSuccessful_4 || bool_findKeywordSuccessful_5)
                {
                    findKeyword2();
                }
            }

            if (bool_fieldFilterSuccessful)
            {
                MessageBox.Show("The data was filtered by the field selected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void queryDataGridViews2(DataGridView tableName, TextBox txtbox)
        {
            try
            {
                string fieldName = comBoxField.Text;
                string fieldSelected = comBoxFilterField.Text;

                fieldName = fieldName.Replace(" ", "_");

                if (tableName.Rows.Count > 0)
                {
                    if (tableName.Columns.Contains(fieldName.ToUpper()))
                    {
                        tableName.CurrentCell = null;
                        clearTextboxes(txtbox);

                        string rowFilter = string.Format(tableName.Columns[fieldName.ToUpper()].HeaderText.ToString() + " LIKE '%" + fieldSelected + "%'");
                        (tableName.DataSource as DataTable).DefaultView.RowFilter = rowFilter;

                        bool_fieldFilterSuccessful = true;
                    }
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void comBoxFilterField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comBoxFilterField.SelectedIndex > -1)
            {
                if (btnFilterFIeld.Enabled == false)
                {
                    btnFilterFIeld.Enabled = true;
                }
            }
        }

        private void rdBtnSortSlowSQLExtension_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnSortSlowSQLExtension.Checked == true)
                {
                    if (dataGridViewSlowSQLlogs.Rows.Count > 0)
                    {
                        var sortSlowSQLDuration = (dataGridViewSlowSQLlogs.Rows.Cast<DataGridViewRow>()
                            .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null)
                            .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ModuleName = r.Cells[2].Value, ApplicationName = r.Cells[3].Value, ActionName = r.Cells[4].Value, EspaceName = r.Cells[5].Value, Message = r.Cells[6].Value, Stack = r.Cells[7].Value, EnvironmentInformation = r.Cells[8].Value, ErrorID = r.Cells[9].Value })
                            .GroupBy(slowsqltime => slowsqltime)
                            .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                            .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ACTION_NAME = g.Key.ActionName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                        dataGridViewSlowSQLDurationlogs.DataSource = sortSlowSQLDuration;
                    }

                    if (dataGridViewSlowExtensionlogs.Rows.Count > 0)
                    {
                        var sortSlowExtensionDuration = (dataGridViewSlowExtensionlogs.Rows.Cast<DataGridViewRow>()
                            .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null)
                            .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ModuleName = r.Cells[2].Value, ApplicationName = r.Cells[3].Value, ActionName = r.Cells[4].Value, EspaceName = r.Cells[5].Value, Message = r.Cells[6].Value, Stack = r.Cells[7].Value, EnvironmentInformation = r.Cells[8].Value, ErrorID = r.Cells[9].Value })
                            .GroupBy(slowexttime => slowexttime)
                            .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                            .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ACTION_NAME = g.Key.ActionName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                        dataGridViewSlowExtensionDurationlogs.DataSource = sortSlowExtensionDuration;
                    }

                    btnExportSlowSQLExtensionTables.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnFilterErrorIDSLowSQLExtension_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnFilterErrorIDSLowSQLExtension.Checked == true)
                {
                    if (dataGridViewSlowSQLlogs.Rows.Count > 0)
                    {
                        var filterSlowSQLErrorID = (dataGridViewSlowSQLlogs.Rows.Cast<DataGridViewRow>()
                            .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value.ToString().Trim().Length > 0)
                            .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ModuleName = r.Cells[2].Value, ApplicationName = r.Cells[3].Value, ActionName = r.Cells[4].Value, EspaceName = r.Cells[5].Value, Message = r.Cells[6].Value, Stack = r.Cells[7].Value, EnvironmentInformation = r.Cells[8].Value, ErrorID = r.Cells[9].Value })
                            .GroupBy(slowsqlerrorid => slowsqlerrorid)
                            .OrderBy(g => Convert.ToDateTime(g.Key.DateTime))
                            .Select(g => new { DATE_TIME = g.Key.DateTime, DURATION_SECONDS = g.Key.TimeTaken, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ACTION_NAME = g.Key.ActionName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                        dataGridViewSlowSQLDurationlogs.DataSource = filterSlowSQLErrorID;
                    }

                    if (dataGridViewSlowExtensionlogs.Rows.Count > 0)
                    {
                        var filterSlowExtensionErrorID = (dataGridViewSlowExtensionlogs.Rows.Cast<DataGridViewRow>()
                            .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value.ToString().Trim().Length > 0)
                            .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ModuleName = r.Cells[2].Value, ApplicationName = r.Cells[3].Value, ActionName = r.Cells[4].Value, EspaceName = r.Cells[5].Value, Message = r.Cells[6].Value, Stack = r.Cells[7].Value, EnvironmentInformation = r.Cells[8].Value, ErrorID = r.Cells[9].Value })
                            .GroupBy(slowexterrorid => slowexterrorid)
                            .OrderByDescending(g => g.Key.DateTime)
                            .Select(g => new { DATE_TIME = g.Key.DateTime, DURATION_SECONDS = g.Key.TimeTaken, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ACTION_NAME = g.Key.ActionName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                        dataGridViewSlowExtensionDurationlogs.DataSource = filterSlowExtensionErrorID;
                    }

                    btnExportSlowSQLExtensionTables.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnSortFilterErrorIDSlowSQLExtension_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnSortFilterErrorIDSlowSQLExtension.Checked == true)
                {
                    if (dataGridViewSlowSQLlogs.Rows.Count > 0)
                    {
                        var sortFilterSlowSQLDurationErrorID = (dataGridViewSlowSQLlogs.Rows.Cast<DataGridViewRow>()
                            .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value.ToString().Trim().Length > 0)
                            .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ModuleName = r.Cells[2].Value, ApplicationName = r.Cells[3].Value, ActionName = r.Cells[4].Value, EspaceName = r.Cells[5].Value, Message = r.Cells[6].Value, Stack = r.Cells[7].Value, EnvironmentInformation = r.Cells[8].Value, ErrorID = r.Cells[9].Value })
                            .GroupBy(slowsqltimeError => slowsqltimeError)
                            .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                            .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ACTION_NAME = g.Key.ActionName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                        dataGridViewSlowSQLDurationlogs.DataSource = sortFilterSlowSQLDurationErrorID;
                    }

                    if (dataGridViewSlowExtensionlogs.Rows.Count > 0)
                    {
                        var sortFilterSlowExtensionDurationErrorID = (dataGridViewSlowExtensionlogs.Rows.Cast<DataGridViewRow>()
                            .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value.ToString().Trim().Length > 0)
                            .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ModuleName = r.Cells[2].Value, ApplicationName = r.Cells[3].Value, ActionName = r.Cells[4].Value, EspaceName = r.Cells[5].Value, Message = r.Cells[6].Value, Stack = r.Cells[7].Value, EnvironmentInformation = r.Cells[8].Value, ErrorID = r.Cells[9].Value })
                            .GroupBy(slowextTimeError => slowextTimeError)
                            .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                            .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ACTION_NAME = g.Key.ActionName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                        dataGridViewSlowExtensionDurationlogs.DataSource = sortFilterSlowExtensionDurationErrorID;
                    }

                    btnExportSlowSQLExtensionTables.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnSortWebServices_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnSortWebServices.Checked == true)
                {
                    var sortWebServicesDuration = (dataGridViewIntWebServiceslogs.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null && r.Cells[10].Value != null)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ModuleName = r.Cells[2].Value, ApplicationName = r.Cells[3].Value, ActionName = r.Cells[4].Value, ActionType = r.Cells[5].Value, EspaceName = r.Cells[6].Value, Message = r.Cells[7].Value, Stack = r.Cells[8].Value, EnvironmentInformation = r.Cells[9].Value, ErrorID = r.Cells[10].Value })
                                .GroupBy(srtwebsrvs => srtwebsrvs)
                                .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                                .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ACTION_NAME = g.Key.ActionName, ACTION_TYPE = g.Key.ActionType, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                    dataGridViewInWebServicesDurationlogs.DataSource = sortWebServicesDuration;

                    btnExportWebServiceTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnSortFilterErrorIDWebServices_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnSortFilterErrorIDWebServices.Checked == true)
                {
                    var sortFilterWebServicesDurationErrorID = (dataGridViewIntWebServiceslogs.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null && r.Cells[10].Value.ToString().Trim().Length > 0)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ModuleName = r.Cells[2].Value, ApplicationName = r.Cells[3].Value, ActionName = r.Cells[4].Value, ActionType = r.Cells[5].Value, EspaceName = r.Cells[6].Value, Message = r.Cells[7].Value, Stack = r.Cells[8].Value, EnvironmentInformation = r.Cells[9].Value, ErrorID = r.Cells[10].Value })
                                .GroupBy(srtfilterwebsrvs => srtfilterwebsrvs)
                                .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                                .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ACTION_NAME = g.Key.ActionName, ACTION_TYPE = g.Key.ActionType, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                    dataGridViewInWebServicesDurationlogs.DataSource = sortFilterWebServicesDurationErrorID;

                    btnExportWebServiceTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnFilterErrorIDWebServices_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnFilterErrorIDWebServices.Checked == true)
                {
                    var filterWebServicesErrorID = (dataGridViewIntWebServiceslogs.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null && r.Cells[10].Value.ToString().Trim().Length > 0)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ModuleName = r.Cells[2].Value, ApplicationName = r.Cells[3].Value, ActionName = r.Cells[4].Value, ActionType = r.Cells[5].Value, EspaceName = r.Cells[6].Value, Message = r.Cells[7].Value, Stack = r.Cells[8].Value, EnvironmentInformation = r.Cells[9].Value, ErrorID = r.Cells[10].Value })
                                .GroupBy(filterwebsrvs => filterwebsrvs)
                                .OrderBy(g => Convert.ToDateTime(g.Key.DateTime))
                                .Select(g => new { DATE_TIME = g.Key.DateTime, DURATION_SECONDS = g.Key.TimeTaken, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ACTION_NAME = g.Key.ActionName, ACTION_TYPE = g.Key.ActionType, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                    dataGridViewInWebServicesDurationlogs.DataSource = filterWebServicesErrorID;

                    btnExportWebServiceTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnScrReqScreens_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnScrReqScreens.Checked == true)
                {
                    var sortScrReqScreenDuration = (dataGridViewScreenRequestsScreenlogs.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ScreenName = r.Cells[2].Value, ModuleName = r.Cells[3].Value, ApplicationName = r.Cells[4].Value, EspaceName = r.Cells[5].Value, Message = r.Cells[6].Value, Stack = r.Cells[7].Value, EnvironmentInformation = r.Cells[8].Value, ErrorID = r.Cells[9].Value })
                                .GroupBy(srtscrreqs => srtscrreqs)
                                .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                                .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, SCREEN = g.Key.ScreenName, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                    dataGridViewScrReqScreenDurationlogs.DataSource = sortScrReqScreenDuration;

                    btnExportScrReqScreenTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnSortFilterErrorIDScrReqScreens_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnSortFilterErrorIDScrReqScreens.Checked == true)
                {
                    var sortFilterScrReqScreenDurationErrorID = (dataGridViewScreenRequestsScreenlogs.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value.ToString().Trim().Length > 0)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ScreenName = r.Cells[2].Value, ModuleName = r.Cells[3].Value, ApplicationName = r.Cells[4].Value, EspaceName = r.Cells[5].Value, Message = r.Cells[6].Value, Stack = r.Cells[7].Value, EnvironmentInformation = r.Cells[8].Value, ErrorID = r.Cells[9].Value })
                                .GroupBy(srtfilterscrreqs => srtfilterscrreqs)
                                .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                                .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, SCREEN = g.Key.ScreenName, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                    dataGridViewScrReqScreenDurationlogs.DataSource = sortFilterScrReqScreenDurationErrorID;

                    btnExportScrReqScreenTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnFilterErrorIDScrReqScreens_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnFilterErrorIDScrReqScreens.Checked == true)
                {
                    var filterScrReqScreenErrorID = (dataGridViewScreenRequestsScreenlogs.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value.ToString().Trim().Length > 0)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ScreenName = r.Cells[2].Value, ModuleName = r.Cells[3].Value, ApplicationName = r.Cells[4].Value, EspaceName = r.Cells[5].Value, Message = r.Cells[6].Value, Stack = r.Cells[7].Value, EnvironmentInformation = r.Cells[8].Value, ErrorID = r.Cells[9].Value })
                                .GroupBy(filterscrreqs => filterscrreqs)
                                .OrderBy(g => Convert.ToDateTime(g.Key.DateTime))
                                .Select(g => new { DATE_TIME = g.Key.DateTime, DURATION_SECONDS = g.Key.TimeTaken, SCREEN = g.Key.ScreenName, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                    dataGridViewScrReqScreenDurationlogs.DataSource = filterScrReqScreenErrorID;

                    btnExportScrReqScreenTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnSortTimers_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnSortTimers.Checked == true)
                {
                    var sortTimersDuration = (dataGridViewTimerTimerslogs.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, CyclicJobNameName = r.Cells[2].Value, ModuleName = r.Cells[3].Value, ApplicationName = r.Cells[4].Value, EspaceName = r.Cells[5].Value, Message = r.Cells[6].Value, Stack = r.Cells[7].Value, EnvironmentInformation = r.Cells[8].Value, ErrorID = r.Cells[9].Value })
                                .GroupBy(srttimers => srttimers)
                                .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                                .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, CYCLIC_JOB_NAME = g.Key.CyclicJobNameName, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                    dataGridViewTimerTimersDurationlogs.DataSource = sortTimersDuration;

                    btnExportTimerTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnSortFilterErrorIDTimers_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnSortFilterErrorIDTimers.Checked == true)
                {
                    var sortFilterTimersDurationErrorID = (dataGridViewTimerTimerslogs.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value.ToString().Trim().Length > 0)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, CyclicJobNameName = r.Cells[2].Value, ModuleName = r.Cells[3].Value, ApplicationName = r.Cells[4].Value, EspaceName = r.Cells[5].Value, Message = r.Cells[6].Value, Stack = r.Cells[7].Value, EnvironmentInformation = r.Cells[8].Value, ErrorID = r.Cells[9].Value })
                                .GroupBy(srtfiltertimers => srtfiltertimers)
                                .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                                .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, CYCLIC_JOB_NAME = g.Key.CyclicJobNameName, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                    dataGridViewTimerTimersDurationlogs.DataSource = sortFilterTimersDurationErrorID;

                    btnExportTimerTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnFilterErrorIDTimers_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnFilterErrorIDTimers.Checked == true)
                {
                    var filterTimersErrorID = (dataGridViewTimerTimerslogs.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value.ToString().Trim().Length > 0)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, CyclicJobNameName = r.Cells[2].Value, ModuleName = r.Cells[3].Value, ApplicationName = r.Cells[4].Value, EspaceName = r.Cells[5].Value, Message = r.Cells[6].Value, Stack = r.Cells[7].Value, EnvironmentInformation = r.Cells[8].Value, ErrorID = r.Cells[9].Value })
                                .GroupBy(filtertimers => filtertimers)
                                .OrderBy(g => Convert.ToDateTime(g.Key.DateTime))
                                .Select(g => new { DATE_TIME = g.Key.DateTime, DURATION_SECONDS = g.Key.TimeTaken, CYCLIC_JOB_NAME = g.Key.CyclicJobNameName, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                    dataGridViewTimerTimersDurationlogs.DataSource = filterTimersErrorID;

                    btnExportTimerTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnSortEmails_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnSortEmails.Checked == true)
                {
                    var sortEmailsDuration = (dataGridViewEmailEmailslogs.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null && r.Cells[10].Value != null && r.Cells[11].Value != null && r.Cells[12].Value != null && r.Cells[13].Value != null && r.Cells[14].Value != null)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ModuleName = r.Cells[2].Value, ApplicationName = r.Cells[3].Value, EspaceName = r.Cells[4].Value, Message = r.Cells[5].Value, Stack = r.Cells[6].Value, From = r.Cells[7].Value, To = r.Cells[8].Value, Subject = r.Cells[9].Value, cc = r.Cells[10].Value, bcc = r.Cells[11].Value, IsTestEmail = r.Cells[12].Value, EnvironmentInformation = r.Cells[13].Value, ErrorId = r.Cells[14].Value })
                                .GroupBy(srtemails => srtemails)
                                .OrderByDescending(g => Convert.ToInt32(g.Key.TimeTaken))
                                .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, FROM = g.Key.From, TO = g.Key.To, SUBJECT = g.Key.Subject, CC = g.Key.cc, BCC = g.Key.bcc, IS_TEST_EMAIL = g.Key.IsTestEmail, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorId })).ToList();

                    dataGridViewEmailEmailsDurationlogs.DataSource = sortEmailsDuration;

                    btnExportEmailsTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnFilterErrorIDEmails_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnFilterErrorIDEmails.Checked == true)
                {
                    var filterEmailsErrorID = (dataGridViewEmailEmailslogs.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null && r.Cells[10].Value != null && r.Cells[11].Value != null && r.Cells[12].Value != null && r.Cells[13].Value != null && r.Cells[14].Value.ToString().Trim().Length > 0)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ModuleName = r.Cells[2].Value, ApplicationName = r.Cells[3].Value, EspaceName = r.Cells[4].Value, Message = r.Cells[5].Value, Stack = r.Cells[6].Value, From = r.Cells[7].Value, To = r.Cells[8].Value, Subject = r.Cells[9].Value, cc = r.Cells[10].Value, bcc = r.Cells[11].Value, IsTestEmail = r.Cells[12].Value, EnvironmentInformation = r.Cells[13].Value, ErrorId = r.Cells[14].Value })
                                .GroupBy(filteremails => filteremails)
                                .OrderBy(g => Convert.ToDateTime(g.Key.DateTime))
                                .Select(g => new { DATE_TIME = g.Key.DateTime, DURATION_SECONDS = g.Key.TimeTaken, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, FROM = g.Key.From, TO = g.Key.To, SUBJECT = g.Key.Subject, CC = g.Key.cc, BCC = g.Key.bcc, IS_TEST_EMAIL = g.Key.IsTestEmail, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorId })).ToList();

                    dataGridViewEmailEmailsDurationlogs.DataSource = filterEmailsErrorID;

                    btnExportEmailsTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnSortExtensions_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnSortExtensions.Checked == true)
                {
                    var sortExtensionsDuration = (dataGridViewExtensionLogsExtensions.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null && r.Cells[10].Value != null)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ExtensionName = r.Cells[2].Value, ModuleName = r.Cells[3].Value, ApplicationName = r.Cells[4].Value, ActionName = r.Cells[5].Value, EspaceName = r.Cells[6].Value, Message = r.Cells[7].Value, Stack = r.Cells[8].Value, EnvironmentInformation = r.Cells[9].Value, ErrorID = r.Cells[10].Value })
                                .GroupBy(srtexts => srtexts)
                                .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                                .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, EXTENSION_NAME = g.Key.ExtensionName, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ACTION_NAME = g.Key.ActionName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                    dataGridViewExtensionsDurationlogs.DataSource = sortExtensionsDuration;

                    btnExportExtensionsTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnSortFilterErrorIDExtensions_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnSortFilterErrorIDExtensions.Checked == true)
                {
                    var sortFilterExtensionsDurationErrorID = (dataGridViewExtensionLogsExtensions.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null && r.Cells[10].Value.ToString().Trim().Length > 0)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ExtensionName = r.Cells[2].Value, ModuleName = r.Cells[3].Value, ApplicationName = r.Cells[4].Value, ActionName = r.Cells[5].Value, EspaceName = r.Cells[6].Value, Message = r.Cells[7].Value, Stack = r.Cells[8].Value, EnvironmentInformation = r.Cells[9].Value, ErrorID = r.Cells[10].Value })
                                .GroupBy(srtfilterexts => srtfilterexts)
                                .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                                .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, EXTENSION_NAME = g.Key.ExtensionName, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ACTION_NAME = g.Key.ActionName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                    dataGridViewExtensionsDurationlogs.DataSource = sortFilterExtensionsDurationErrorID;

                    btnExportExtensionsTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnFilterErrorIDExtensions_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnFilterErrorIDExtensions.Checked == true)
                {
                    var filterExtensionsErrorID = (dataGridViewExtensionLogsExtensions.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null && r.Cells[10].Value.ToString().Trim().Length > 0)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ExtensionName = r.Cells[2].Value, ModuleName = r.Cells[3].Value, ApplicationName = r.Cells[4].Value, ActionName = r.Cells[5].Value, EspaceName = r.Cells[6].Value, Message = r.Cells[7].Value, Stack = r.Cells[8].Value, EnvironmentInformation = r.Cells[9].Value, ErrorID = r.Cells[10].Value })
                                .GroupBy(filterexts => filterexts)
                                .OrderBy(g => Convert.ToDateTime(g.Key.DateTime))
                                .Select(g => new { DATE_TIME = g.Key.DateTime, DURATION_SECONDS = g.Key.TimeTaken, EXTENSION_NAME = g.Key.ExtensionName, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ACTION_NAME = g.Key.ActionName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                    dataGridViewExtensionsDurationlogs.DataSource = filterExtensionsErrorID;

                    btnExportExtensionsTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnSortServiceActions_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnSortServiceActions.Checked == true)
                {
                    var sortServiceActionsDuration = (dataGridViewSrvActServicelogs.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value != null)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ActionName = r.Cells[2].Value, ModuleName = r.Cells[3].Value, ApplicationName = r.Cells[4].Value, EspaceName = r.Cells[5].Value, Message = r.Cells[6].Value, Stack = r.Cells[7].Value, EnvironmentInformation = r.Cells[8].Value, ErrorID = r.Cells[9].Value })
                                .GroupBy(srtsrvacts => srtsrvacts)
                                .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                                .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, ACTION_NAME = g.Key.ActionName, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                    dataGridViewSrvActServiceDurationlogs.DataSource = sortServiceActionsDuration;

                    btnExportServiceActionsTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnSortFilterErrorIDServiceActions_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnSortFilterErrorIDServiceActions.Checked == true)
                {
                    var sortFilterServiceActionsDurationErrorID = (dataGridViewSrvActServicelogs.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value.ToString().Trim().Length > 0)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ActionName = r.Cells[2].Value, ModuleName = r.Cells[3].Value, ApplicationName = r.Cells[4].Value, EspaceName = r.Cells[5].Value, Message = r.Cells[6].Value, Stack = r.Cells[7].Value, EnvironmentInformation = r.Cells[8].Value, ErrorID = r.Cells[9].Value })
                                .GroupBy(srtfiltersrvacts => srtfiltersrvacts)
                                .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                                .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, ACTION_NAME = g.Key.ActionName, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                    dataGridViewSrvActServiceDurationlogs.DataSource = sortFilterServiceActionsDurationErrorID;

                    btnExportServiceActionsTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void rdBtnFilterErrorIDServiceActions_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdBtnFilterErrorIDServiceActions.Checked == true)
                {
                    var filterServiceActionsErrorID = (dataGridViewSrvActServicelogs.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null && r.Cells[7].Value != null && r.Cells[8].Value != null && r.Cells[9].Value.ToString().Trim().Length > 0)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, ActionName = r.Cells[2].Value, ModuleName = r.Cells[3].Value, ApplicationName = r.Cells[4].Value, EspaceName = r.Cells[5].Value, Message = r.Cells[6].Value, Stack = r.Cells[7].Value, EnvironmentInformation = r.Cells[8].Value, ErrorID = r.Cells[9].Value })
                                .GroupBy(filtersrvacts => filtersrvacts)
                                .OrderBy(g => Convert.ToDateTime(g.Key.DateTime))
                                .Select(g => new { DATE_TIME = g.Key.DateTime, DURATION_SECONDS = g.Key.TimeTaken, ACTION_NAME = g.Key.ActionName, MODULE_NAME = g.Key.ModuleName, APPLICATION_NAME = g.Key.ApplicationName, ESPACE_NAME = g.Key.EspaceName, MESSAGE = g.Key.Message, STACK = g.Key.Stack, ENVIRONMENT_INFORMATION = g.Key.EnvironmentInformation, ERROR_ID = g.Key.ErrorID })).ToList();

                    dataGridViewSrvActServiceDurationlogs.DataSource = filterServiceActionsErrorID;

                    btnExportServiceActionsTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void chkBoxSortTradWebRequestsScreens_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkBoxSortTradWebRequestsScreens.Checked == true)
                {
                    var sortScreenssDuration = (dataGridViewTradWebRequestsScreenlogs.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells[0].Value != null && r.Cells[1].Value != null && r.Cells[2].Value != null && r.Cells[3].Value != null && r.Cells[4].Value != null && r.Cells[5].Value != null && r.Cells[6].Value != null)
                                .Select(r => new { DateTime = r.Cells[0].Value, TimeTaken = r.Cells[1].Value, Screen = r.Cells[2].Value, ScreenType = r.Cells[3].Value, ApplicationName = r.Cells[4].Value, ActionName = r.Cells[5].Value, EspaceName = r.Cells[6].Value })
                                .GroupBy(srtscreens => srtscreens)
                                .OrderByDescending(g => Convert.ToDecimal(g.Key.TimeTaken))
                                .Select(g => new { DURATION_SECONDS = g.Key.TimeTaken, DATE_TIME = g.Key.DateTime, SCREEN = g.Key.Screen, SCREEN_TYPE = g.Key.ScreenType, APPLICATION_NAME = g.Key.ApplicationName, ACTION_NAME = g.Key.ActionName, ESPACE_NAME = g.Key.EspaceName })).ToList();

                    dataGridViewTradWebRequestsScreenDurationlogs.DataSource = sortScreenssDuration;

                    btnExportScreenTable.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                error_message = "Error: " + Environment.NewLine + ex.ToString();
                errorLog("\\error_log.txt", error_message);
                MessageBox.Show("A file has been created with the error message." + Environment.NewLine + Environment.NewLine + error_message);
                throw;
            }
        }

        private void errorLog(string txtFile, string err_msg)
        {
            outputCSVfile = label8.Text + txtFile;
            using (StreamWriter logFile = new StreamWriter(outputCSVfile, true))
            {
                logFile.WriteLine(err_msg);
            }
        }
    }
}

