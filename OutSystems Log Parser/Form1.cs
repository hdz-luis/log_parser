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
        bool removeGarbageSuccessful = false;
        bool highlightError = false;
        bool highlightErrorSuccessful = false;
        bool findKeyword = false;
        bool findKeywordSuccessful = false;
        bool screenshotSuccessful = false;
        bool datetimeFilterSuccessful = false;
        string currentWorkingDirectory = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

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
                            else if (fileName == "general_text_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "MESSAGE_TYPE", "MESSAGE" };
                                populateTables(relativePath + "\\general_text_logs.txt", delimiters, column_names, dataGridViewGeneralTXTlogs);
                            }
                            else if (fileName == "timer_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "EXECUTED_BY",
                                    "ESPACE_NAME", "ESPACE_ID", "CYCLIC_JOB_NAME", "CYCLIC_JOB_KEY", "SHOULD_HAVE_RUN_AT", "NEXT_RUN",
                                    "ERROR_ID", "REQUEST_KEY", "TENANT_ID" };
                                populateTables(relativePath + "\\timer_logs.txt", delimiters, column_names, dataGridViewTimerlogs);
                            }
                            else if (fileName == "bpt_troubleshootingreport_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "ESPACE_NAME", "PROCESS_NAME", "PROCESS_STATUS", "PROCESS_LAST_MODIFIED",
                                    "PROCESS_SUSPENDED_DATE", "PROCESS_ID", "PARENT_PROCESS_ID", "ACTIVITY_CREATED", "ACTIVITY_NAME", "ACTIVITY_KIND",
                                    "ACTIVITY_STATUS", "ACTIVITY_RUNNING_SINCE", "ACTIVITY_NEXT_RUN", "ACTIVITY_CLOSED", "ACTIVITY_ERROR_COUNT", "ACTIVITY_ERROR_ID" };
                                populateTables(relativePath + "\\bpt_troubleshootingreport_logs.txt", delimiters, column_names, dataGridViewBPTReportslogs);
                            }
                            else if (fileName == "environment_capabilities.txt")
                            {
                                string[] column_names = { "ID", "ENVIRONMENT", "CAPABILITY" };
                                populateTables(relativePath + "\\environment_capabilities.txt", delimiters, column_names, dataGridViewEnvironmentCapabilitieslogs);
                            }
                            else if (fileName == "environments.txt")
                            {
                                string[] column_names = { "IS_LIFE_TIME", "IS_REGISTERED", "ID", "NAME", "DESCRIPTION", "HOST", "PUBLIC_HOST", "TYPE", "PRIMARY_CONTACT", "ORDER",
                                "NUMBER_OF_USERS", "IS_ACTIVE", "IS_OFFLINE", "LAST_SET_OFFLINE", "CREATED_BY", "CREATED_ON", "USE_HTTPS", "VERSION", "LAST_UPGRADE_VERSION",
                                "UID", "CALLBACK_ADDRESS", "NUMBER_OF_FRONTEND_SERVERS", "FIRST_SYNC_FINISHED", "CLOUD_PROVIDER_TYPE", "ENVIRONMENT_STACK", "ADDITIONAL_INFO",
                                "LAST_CACHE_INVALIDATION", "ENVIRONMENT_DB_PROVIDER", "ENVIRONMENT_SERVER_KIND", "TWO_STEP_PUBLISH_MODE", "LAST_ENV_SYNC_SEQUENTIAL_NUMBER",
                                "DATA_HARVESTED_TO", "IS_IN_OUTSYSTEMS_CLOUD", "BEST_HASHING_ALGORITHM", "ALLOW_NATIVE_BUILDS" };
                                populateTables(relativePath + "\\environments.txt", delimiters, column_names, dataGridViewEnvironmentslogs);
                            }
                            else if (fileName == "full_error_dump_logs.txt")
                            {
                                string[] column_names = { "PLATFORM_INFORMATION" };
                                populateTables(relativePath + "\\full_error_dump_logs.txt", delimiters, column_names, dataGridViewFullErrorDumps);
                            }
                            else if (fileName == "roles.txt")
                            {
                                string[] column_names = { "ID", "NAME", "DESCRIPTION", "ORDER", "CAN_CONFIGURE_INFRASTRUCTURE", "CAN_CONFIGURE_ROLES", "CAN_CONFIGURE_USERS",
                                    "CAN_CONFIGURE_APPLICATION_ROLES", "KEY", "ID_2", "LABEL_OLD", "SHORT_LABEL_OLD", "DESCRIPTION_OLD", "LABEL", "SHORT_LABEL", "DESCRIPTION_2",
                                    "LT_LEVEL", "SC_LEVEL", "APPLICATION_LEVEL", "IS_OLD_LEVEL", "ID_3", "LABEL_2", "SHORT_LABEL_2", "DESCRIPTION_3", "LEVEL", "IS_COMPUTED",
                                    "ID_4", "ROLE", "ENVIRONMENT", "DEFAULT_PERMISSION_LEVEL", "CAN_CREATE_APPLICATIONS", "CAN_REFERENCE_SYSTEMS", "IS_INITIALIZED" };
                                populateTables(relativePath + "\\roles.txt", delimiters, column_names, dataGridViewRoleslogs);
                            }
                            else if (fileName == "roles_in_applications.txt")
                            {
                                string[] column_names = { "ID", "USER", "ROLE", "APPLICATION" };
                                populateTables(relativePath + "\\roles_in_applications.txt", delimiters, column_names, dataGridViewRolesInApplicationslogs);
                            }
                            else if (fileName == "roles_in_teams.txt")
                            {
                                string[] column_names = { "ID", "ROLE", "KEY", "TEAM", "NAME", "KEY_2" };
                                populateTables(relativePath + "\\roles_in_teams.txt", delimiters, column_names, dataGridViewRolesInTeamslogs);
                            }
                            else if (fileName == "sync_errors.txt")
                            {
                                string[] column_names = { "DATE_TIME", "SESSION_ID", "MESSAGE", "STACK", "MODULE", "USERNAME", "ENDPOINT", "ACTION", "PROCESS_NAME",
                                    "ACTIVITY_NAME" };
                                populateTables(relativePath + "\\sync_errors.txt", delimiters, column_names, dataGridViewSyncErrorslogs);
                            }
                            else if (fileName == "user.txt")
                            {
                                string[] column_names = { "ID", "NAME", "USERNAME", "EXTERNAL", "CREATION_DATE", "LAST_LOGIN", "IS_ACTIVE", "ID_2", "NAME_2",
                                    "DESCRIPTION", "ORDER", "CAN_CONFIGURE_INFRASTRUCTURE", "CAN_CONFIGURE_ROLES", "CAN_CONFIGURE_USERS", "CAN_CONFIGURE_APPLICATION_ROLES",
                                    "KEY", "USER_ROLE", "KEY_2", "MTSI_IDENTIFIER" };
                                populateTables(relativePath + "\\user.txt", delimiters, column_names, dataGridViewUserlogs);
                            }
                            else if (fileName == "user_pools.txt")
                            {
                                string[] column_names = { "USER", "POOL_KEY" };
                                populateTables(relativePath + "\\user_pools.txt", delimiters, column_names, dataGridViewUserPoolslogs);
                            }
                            else if (fileName == "application.txt")
                            {
                                string[] column_names = { "ID", "TEAM", "NAME", "DESCRIPTION", "PRIMARY_CONTACT", "URL_PATH", "KEY", "LOGO_HASH", "IS_ACTIVE", "DEFAULT_THEME_IS_MOBILE",
                                    "MONITORING_ENABLED", "APPLICATION_KIND" };
                                populateTables(relativePath + "\\application.txt", delimiters, column_names, dataGridViewStagingApplogs);
                            }
                            else if (fileName == "application_version.txt")
                            {
                                string[] column_names = { "ID", "APPLICATION", "VERSION", "CHANGE_LOG", "CREATED_ON", "CREATED_BY", "CREATED_ON_ENVIRONMENT", "FRONT_OFFICE_ESPACE_KEY",
                                    "FRONT_OFFICE_ESPACE_NAME", "BACK_OFFICE_ESPACE_KEY", "BACK_OFFICE_ESPACE_NAME", "WEB_THEME_GLOBAL_KEY", "MOBILE_THEME_GLOBAL_KEY",
                                    "WAS_AUTO_TAGGED", "VERSION_DECIMAL", "TEMPLATE_KEY", "PRIMARY_COLOR", "NATIVE_HASH", "KEY" };
                                populateTables(relativePath + "\\application_version.txt", delimiters, column_names, dataGridViewStagingAppVerlogs);
                            }
                            else if (fileName == "application_version_module_version.txt")
                            {
                                string[] column_names = { "ID", "APPLICATION_VERSION", "MODULE_VERSION", "ID_2", "MODULE", "HASH", "GENERAL_HASH", "CREATED_ON",
                                    "CREATED_BY", "CREATED_ON_ENVIRONMENT", "LAST_UPGRADE_VERSION", "DIRECT_UPGRADE_FROM_VERSION_HASH", "COMPATIBILITY_SIGNATURE_HASH",
                                    "KEY" };
                                populateTables(relativePath + "\\application_version_module_version.txt", delimiters, column_names, dataGridViewStagingAppVerModuleVerlogs);
                            }
                            else if (fileName == "change_log.txt")
                            {
                                string[] column_names = { "ID", "LABEL", "ID_2", "DATE_TIME", "MESSAGE", "FIRST_OBJECT_TYPE", "FIRST_OBJECT", "SECOND_OBJECT_TYPE",
                                    "SECOND_OBJECT", "IS_WRITE", "IS_SUCCESSFUL", "IS_SYSTEM", "ENTRY_ESPACE", "USER", "CLIENT_IP" };
                                populateTables(relativePath + "\\change_log.txt", delimiters, column_names, dataGridViewStagingChangelog);
                            }
                            else if (fileName == "consumer_elements.txt")
                            {
                                string[] column_names = { "CONSUMER_MODULE", "CONSUMER_MODULE_NAME", "CONSUMER_MODULE_VERSION", "CONSUMER_ELEMENT_VERSION", "CONSUMER_ELEMENT_VERSION_KEY", "CONSUMER_ELEMENT_VERSION_TYPE",
                                    "CONSUMER_ELEMENT_VERSION_NAME", "CONSUMER_ELEMENT_VERSION_COMPATIBILITY_HASH", "PRODUCER_MODULE", "PRODUCER_MODULE_KEY", "PRODUCER_MODULE_NAME", "PRODUCER_MODULE_TYPE", "CREATED_ON_PRODUCER_MODULE_VERSION" };
                                populateTables(relativePath + "\\consumer_elements.txt", delimiters, column_names, dataGridViewStagingConsumerElements);
                            }
                            else if (fileName == "entity_configurations.txt")
                            {
                                string[] column_names = { "ID", "ENTITY_KEY", "MODULE_VERSION_ID", "CREATED_ON", "UPDATED_ON", "ID_2",
                                    "MODULE_ID", "STAGING_ID", "CREATED_ON_2", "CREATED_BY", "UPDATED_ON_2", "UPDATED_BY", "ENTITY_KEY_2",
                                    "PHYSICAL_TABLE_NAME", "IS_OVERRIDEN_TABLE_NAME", "DEFAULT_PHYSICAL_TABLE_NAME", "ENTITY_NAME",
                                    "SOURCE_PHYSICAL_TABLE_NAME", "TARGET_PHYSICAL_TABLE_NAME" };
                                populateTables(relativePath + "\\entity_configurations.txt", delimiters, column_names, dataGridViewStagingEntityConfiguration);
                            }
                            else if (fileName == "environment_app_version.txt")
                            {
                                string[] column_names = { "ID", "ENVIRONMENT", "APPLICATION_VERSION" };
                                populateTables(relativePath + "\\environment_app_version.txt", delimiters, column_names, dataGridViewStagingEnvironmentApplicationVersion);
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
                                populateTables(relativePath + "\\environment_application_cache.txt", delimiters, column_names, dataGridViewStagingEnvironmentAppicationCache);
                            }
                            else if (fileName == "environment_application_module.txt")
                            {
                                string[] column_names = { "ID", "APPLICATION", "MODULE", "ENVIRONMENT", "LAST_CHANGED_ON" };
                                populateTables(relativePath + "\\environment_application_module.txt", delimiters, column_names, dataGridViewStagingEnvironmentApplicationModule);
                            }
                            else if (fileName == "environment_module_cache.txt")
                            {
                                string[] column_names = { "ID", "ENVIRONMENT", "MODULE_VERSION", "INTERNAL_VERSION", "PUBLISHED_ON", "PUBLISHED_BY", "ID_2", "ENVIRONMENT_2", "MODULE",
                                    "PUBLISH", "CHANGE_STATUS", "CHANGE_STATUS_MESSAGE", "DELETED", "CONSISTENCY_STATUS", "CONSISTENCY_STATUS_MESSAGES", "IS_OUTDATED" };
                                populateTables(relativePath + "\\environment_module_cache.txt", delimiters, column_names, dataGridViewStagingEnvironmentModuleCache);
                            }
                            else if (fileName == "environment_module_running.txt")
                            {
                                string[] column_names = { "ID", "ENVIRONMENT", "CONSUMER_MODULE", "PRODUCER_MODULE_KEY", "PRODUCER_COMPATIBILITY_HASH", "IS_WEAK_REFERENCE" };
                                populateTables(relativePath + "\\environment_module_running.txt", delimiters, column_names, dataGridViewStagingEnvironmentModuleRunning);
                            }
                            else if (fileName == "module_version_references.txt")
                            {
                                string[] column_names = { "ID", "MODULE_VERSION_REFERENCE", "PRODUCER_MODULE_VERSION", "IS_COMPATIBLE", "IS_IN_DIFFERENT_LUV", "PLATFORM_VERSION",
                                    "ID_2", "MODULE_VERSION", "PRODUCER_MODULE", "IS_WEAK_REFERENCE", "ID_3", "MODULE_VERSION_REFERENCE_STATUS", "ELEMENT_NAME", "ELEMENT_KEY",
                                    "ELEMENT_TYPE", "ELEMENT_REF_INCONSISTENCY_TYPE_I" };
                                populateTables(relativePath + "\\module_version_references.txt", delimiters, column_names, dataGridViewStagingModuleVersionRefererences);
                            }
                            else if (fileName == "modules.txt")
                            {
                                string[] column_names = { "ID", "LABEL", "TOKEN", "ID_2", "NAME", "DESCRIPTION", "KEY", "TYPE" };
                                populateTables(relativePath + "\\modules.txt", delimiters, column_names, dataGridViewStagingModules);
                            }
                            else if (fileName == "producer_elements.txt")
                            {
                                string[] column_names = { "MODULE", "MODULE_NAME", "MODULE_VERSION", "ELEMENT_VERSION", "ELEMENT_VERSION_KEY", "ELEMENT_VERSION_TYPE", "ELEMENT_VERSION_NAME", "ELEMENT_VERSION_COMPATIBILITY_HASH" };
                                populateTables(relativePath + "\\producer_elements.txt", delimiters, column_names, dataGridViewStagingProducerElements);
                            }
                            else if (fileName == "site_properties.txt")
                            {
                                string[] column_names = { "ID", "SS_KEY", "MODULE_VERSION_ID", "NAME", "DATA_TYPE_ID", "DESCRIPTION", "DEFAULT_VALUE", "IS_MULTI_TENANT", "CREATED_ON", "UPDATED_ON", "ID_2", "LABEL", "ORDER", "IS_ACTIVE",
                                    "ID_3", "EFFECTIVE_VALUE_CHANGED_IN_STAGING", "EFFECTIVE_VALUE_ON_TARGET", "SITE_PROPERTY_SS_KEY", "IS_MULTI_TENANT_2", "MODULE_ID", "STAGING_ID", "IS_NEW", "CREATED_ON_2", "CREATED_BY",
                                    "UPDATED_ON_2", "UPDATED_BY" };
                                populateTables(relativePath + "\\site_properties.txt", delimiters, column_names, dataGridViewStagingSiteProperties);
                            }
                            else if (fileName == "staging.txt")
                            {
                                string[] column_names = { "ID", "SOURCE_ENVIRONMENT", "TARGET_ENVIRONMENT", "LABEL", "INTERNAL", "CREATED_BY", "CREATED_ON", "STARTED_BY", "STARTED_ON", "FINISHED_ON", "IS_DRAFT", "SOLUTION_PUBLISHED_FINISHED",
                                    "IS_WAITING_FOR_CONFIRMATION_PROCESS", "SAVED_BY", "SAVED_ON", "LAST_REFRESHED_ON", "SYNC_FINISHED_ON", "SOURCE_STAGING", "STAGING_CONFIRMATION_KIND", "TWO_STEP_MODE", "LAST_RESUME_DATE_TIME", "MARKED_FOR_ABORT_ON",
                                    "ABORTED_BY", "KEY", "STATUS" };
                                populateTables(relativePath + "\\staging.txt", delimiters, column_names, dataGridViewStaginglogs);
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
                                populateTables(relativePath + "\\staging_application_version.txt", delimiters, column_names, dataGridViewStagingApplicationVersion);
                            }
                            else if (fileName == "staging_message.txt")
                            {
                                string[] column_names = { "ID", "STAGING", "MESSAGE", "DETAIL", "EXTRA_INFO", "INTERNAL_ID", "INTERNAL_TYPE", "TYPE", "DATE_TIME" };
                                populateTables(relativePath + "\\staging_message.txt", delimiters, column_names, dataGridViewStagingMessage);
                            }
                            else if (fileName == "staging_module_inconsistency.txt")
                            {
                                string[] column_names = { "ID", "STAGING", "CONSUMER_MODULE", "PRODUCER_MODULE", "FIRST_REQUIRED_ELEMENT", "FIRST_REQUIRED_ELEMENT_TYPE", "TOTAL_REQUIRED_ELEMENTS", "PRODUCER_MODULE_NAME", "CONSUMER_MODULE_NAME", "INCONSISTENCY_TYPE" };
                                populateTables(relativePath + "\\staging_module_inconsistency.txt", delimiters, column_names, dataGridViewStagingModuleInconsistencies);
                            }
                            else if (fileName == "staging_module_version.txt")
                            {
                                string[] column_names = { "ID", "STAGING", "APPLICATION", "PREVIOUS_APPLICATION", "MODULE", "MODULE_VERSION", "MODULE_DELETED", "PREVIOUS_MODULE_VERSION", "PREVIOUS_MODULE_DELETED", "OPERATION" };
                                populateTables(relativePath + "\\staging_module_version.txt", delimiters, column_names, dataGridViewStagingModuleVersion);
                            }
                            else if (fileName == "staging_module_version_to_publish.txt")
                            {
                                string[] column_names = { "ID", "STAGING", "PLANNED_MODULE_VERSION_HASH", "MODULE_VERSION_HASH_TO_PUBLISH" };
                                populateTables(relativePath + "\\staging_module_version_to_publish.txt", delimiters, column_names, dataGridViewStagingModuleVersionPublished);
                            }
                            else if (fileName == "staging_module_version_to_upload.txt")
                            {
                                string[] column_names = { "ID", "STAGING", "USER", "ENVIRONMENT_ID_1", "ENVIRONMENT_ID_2", "TYPE", "NAME", "MODULE_KEY", "VERSION_KEY", "DIRECT_UPGRADE_FROM_VERSION_KEY", "APPLICATION_KEY" };
                                populateTables(relativePath + "\\staging_module_version_to_upload.txt", delimiters, column_names, dataGridViewStagingModuleVersionUploaded);
                            }
                            else if (fileName == "staging_option.txt")
                            {
                                string[] column_names = { "ID", "STAGING", "APPLICATION", "STAGING_OPTION_TYPE", "APPLICATION_VERSION", "APPLICATION_VERSION_LABEL", "LABEL", "APPLICATION_DESCRIPTION", "IS_TOP_OPTION", "iOS_VERSION_LABEL", "ANDROID_VERSION_LABEL",
                                    "MOBILE_APPS_DESCRIPTION" };
                                populateTables(relativePath + "\\staging_option.txt", delimiters, column_names, dataGridViewStagingOptions);
                            }
                            else if (fileName == "staging_outdated_application.txt")
                            {
                                string[] column_names = { "ID", "STAGING", "APPLICATION" };
                                populateTables(relativePath + "\\staging_outdated_application.txt", delimiters, column_names, dataGridViewStagingOutdatedApplication);
                            }
                            else if (fileName == "staging_outdated_module.txt")
                            {
                                string[] column_names = { "ID", "STAGING", "MODULE" };
                                populateTables(relativePath + "\\staging_outdated_module.txt", delimiters, column_names, dataGridViewStagingOutdatedModule);
                            }
                            else if (fileName == "windows_application_event_viewer_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "LEVEL", "MESSAGE", "TASK", "COMPUTER", "PROVIDER_NAME",
                                    "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID", "KEYWORDS" };
                                populateTables(relativePath + "\\windows_application_event_viewer_logs.txt", delimiters, column_names, dataGridViewWinAppEventViewer);
                            }
                            else if (fileName == "windows_security_event_viewer_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "LEVEL", "MESSAGE", "TASK", "COMPUTER", "PROVIDER_NAME",
                                    "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID", "KEYWORDS" };
                                populateTables(relativePath + "\\windows_security_event_viewer_logs.txt", delimiters, column_names, dataGridViewWinSecEventViewer);
                            }
                            else if (fileName == "windows_system_event_viewer_logs.txt")
                            {
                                string[] column_names = { "DATE_TIME", "LEVEL", "MESSAGE", "TASK", "COMPUTER", "PROVIDER_NAME",
                                    "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID", "KEYWORDS" };
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
                    btnSearchKeyword.Enabled = true;
                    btnSearchKeyword.BackColor = SystemColors.ControlLight;
                    txtBoxKeyword.Enabled = true;
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
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
                        queryDataGridViews(dataGridViewAndroidlogs, isoFrom, isoTo, txtBoxDetailAndroidLogs);
                        queryDataGridViews(dataGridViewEmaillogs, isoFrom, isoTo, txtBoxDetailEmaillogs);
                        queryDataGridViews(dataGridViewErrorlogs, isoFrom, isoTo, txtBoxDetailErrorLogs);
                        queryDataGridViews(dataGridViewExtensionlogs, isoFrom, isoTo, txtBoxDetailExtensionlogs);
                        queryDataGridViews(dataGridViewGenerallogs, isoFrom, isoTo, txtBoxDetailGenerallogs);
                        queryDataGridViews(dataGridViewIISDateTime, isoFrom, isoTo, txtDetailIISlogs);
                        queryDataGridViews(dataGridViewIISTimeTaken, isoFrom, isoTo, txtDetailIISlogs);
                        queryDataGridViews(dataGridViewIntegrationslogs, isoFrom, isoTo, txtBoxDetailIntegrationlogs);
                        queryDataGridViews(dataGridViewiOSlogs, isoFrom, isoTo, txtBoxDetailiOSLogs);
                        queryDataGridViews(dataGridViewScreenRequestslogs, isoFrom, isoTo, txtBoxDetailScreenRequestslogs);
                        queryDataGridViews(dataGridViewServiceActionlogs, isoFrom, isoTo, txtBoxDetailServiceActionlogs);
                        queryDataGridViews(dataGridViewServiceStudiologs, isoFrom, isoTo, txtBoxDetailServiceStudioLogs);
                        queryDataGridViews(dataGridViewGeneralTXTlogs, isoFrom, isoTo, txtBoxDetailGeneralTXTLogs);
                        queryDataGridViews(dataGridViewTimerlogs, isoFrom, isoTo, txtBoxDetailTimerlogs);
                        queryDataGridViews(dataGridViewTradWebRequests, isoFrom, isoTo, txtBoxDetailTradWebRequests);
                        queryDataGridViews(dataGridViewBPTReportslogs, isoFrom, isoTo, txtBoxDetailBPTReportslogs);
                        queryDataGridViews(dataGridViewWinAppEventViewer, isoFrom, isoTo, txtBoxDetailWinAppEventViewer);
                        queryDataGridViews(dataGridViewWinSysEventViewer, isoFrom, isoTo, txtBoxDetailWinSysEventViewer);
                        queryDataGridViews(dataGridViewWinSecEventViewer, isoFrom, isoTo, txtBoxDetailWinSecEventViewer);

                        btnFilter.Enabled = false;
                        dateTimePicker1.Enabled = false;
                        maskedTextBox1.Enabled = false;
                        dateTimePicker2.Enabled = false;
                        maskedTextBox2.Enabled = false;
                    }
                }

                if (removeGarbage)
                {
                    removeGenericErrors(dataGridViewErrorlogs, 1, txtBoxDetailErrorLogs);
                    //removeGenericErrors(dataGridViewGenerallogs, 1, txtBoxDetailGenerallogs);
                    //removeGenericErrors2(dataGridViewIntegrationslogs, txtBoxDetailIntegrationlogs);
                    //removeGenericErrors2(dataGridViewScreenRequestslogs, txtBoxDetailScreenRequestslogs);
                    //removeGenericErrors2(dataGridViewTimerlogs, txtBoxDetailTimerlogs);
                    //removeGenericErrors2(dataGridViewEmaillogs, txtBoxDetailEmaillogs);
                    //removeGenericErrors2(dataGridViewExtensionlogs, txtBoxDetailExtensionlogs);
                    //removeGenericErrors2(dataGridViewServiceActionlogs, txtBoxDetailServiceActionlogs);
                    //removeGenericErrors2(dataGridViewTradWebRequests, txtBoxDetailTradWebRequests);
                    //removeGenericErrors2(dataGridViewIISDateTime, txtDetailIISlogs);
                    //removeGenericErrors2(dataGridViewIISTimeTaken, txtDetailIISlogs);
                    removeGenericErrors(dataGridViewWinAppEventViewer, 2, txtBoxDetailWinAppEventViewer);
                    removeGenericErrors(dataGridViewWinSysEventViewer, 2, txtBoxDetailWinSysEventViewer);
                    removeGenericErrors(dataGridViewWinSecEventViewer, 2, txtBoxDetailWinSecEventViewer);
                    removeGenericErrors(dataGridViewAndroidlogs, 3, txtBoxDetailAndroidLogs);
                    removeGenericErrors(dataGridViewiOSlogs, 3, txtBoxDetailiOSLogs);
                    removeGenericErrors(dataGridViewServiceStudiologs, 3, txtBoxDetailServiceStudioLogs);
                    removeGenericErrors(dataGridViewGeneralTXTlogs, 2, txtBoxDetailGeneralTXTLogs);
                    //removeGenericErrors(dataGridViewBPTReportslogs, txtBoxDetailBPTReportslogs);
                }

                if (highlightError)
                {
                    string[] knownErrors_Errorlogs = { "url rewrite module error", "an error occurred in task", "server cannot modify cookies", "ping validation failed", "a fatal error has occurred", "communicationexception", "json deserialization", "compilation error", "file is corrupt or invalid", "checksum failed for file", "connection failed421", "temporary server error", "can't proceed", "truncated in table", "unknown reference expression type email" };
                    string[] knownErrors_Generallogs = { "system cannot find" };
                    string[] knownErrors_WinAppEventViewer = { "ora-", "error closing", "cannot access", "error opening" };
                    string[] knownErrors_WinSysEventViewer = { "error closing", "timed out" };
                    string[] knownErrors_AndroidiOSlogs = { "file is corrupt or invalid", "androidx library", "command finished with error code 0", "plugin is not going to work", "error: spawnsync sudo etimeout", "plugin doesn't support this project's cordova-android version", "failed to fetch plug", "archive failed", "build failed with the following error", "command failed with exit code", "signing certificate is invalid", "the ios deployment target", "verification failed" };
                    string[] knownErrors_ServiceStudiologs = { "oneoftypedefinition", "http forbidden", "[not connected]" };

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
                    //highlightKnownErrors(dataGridViewGeneralTXTlogs, 2);
                    //highlightKnownErrors(dataGridViewBPTReportslogs);
                }

                if (findKeyword)
                {
                    highlightKeyword(dataGridViewErrorlogs);
                    highlightKeyword(dataGridViewGenerallogs);
                    highlightKeyword(dataGridViewIntegrationslogs);
                    highlightKeyword(dataGridViewScreenRequestslogs);
                    highlightKeyword(dataGridViewTimerlogs);
                    highlightKeyword(dataGridViewEmaillogs);
                    highlightKeyword(dataGridViewExtensionlogs);
                    highlightKeyword(dataGridViewServiceActionlogs);
                    highlightKeyword(dataGridViewTradWebRequests);
                    highlightKeyword(dataGridViewIISDateTime);
                    highlightKeyword(dataGridViewIISTimeTaken);
                    highlightKeyword(dataGridViewWinAppEventViewer);
                    highlightKeyword(dataGridViewWinSysEventViewer);
                    highlightKeyword(dataGridViewWinSecEventViewer);
                    highlightKeyword(dataGridViewAndroidlogs);
                    highlightKeyword(dataGridViewiOSlogs);
                    highlightKeyword(dataGridViewServiceStudiologs);
                    highlightKeyword(dataGridViewGeneralTXTlogs);
                    highlightKeyword(dataGridViewBPTReportslogs);
                    highlightKeyword(dataGridViewEnvironmentCapabilitieslogs);
                    highlightKeyword(dataGridViewEnvironmentslogs);
                    highlightKeyword(dataGridViewFullErrorDumps);
                    highlightKeyword(dataGridViewRoleslogs);
                    highlightKeyword(dataGridViewRolesInApplicationslogs);
                    highlightKeyword(dataGridViewRolesInTeamslogs);
                    highlightKeyword(dataGridViewUserlogs);
                    highlightKeyword(dataGridViewUserPoolslogs);
                    highlightKeyword(dataGridViewSyncErrorslogs);
                    highlightKeyword(dataGridViewStagingApplogs);
                    highlightKeyword(dataGridViewStagingAppVerlogs);
                    highlightKeyword(dataGridViewStagingAppVerModuleVerlogs);
                    highlightKeyword(dataGridViewStagingChangelog);
                    highlightKeyword(dataGridViewStagingConsumerElements);
                    highlightKeyword(dataGridViewStagingEntityConfiguration);
                    highlightKeyword(dataGridViewStagingEnvironmentAppicationCache);
                    highlightKeyword(dataGridViewStagingEnvironmentApplicationModule);
                    highlightKeyword(dataGridViewStagingEnvironmentApplicationVersion);
                    highlightKeyword(dataGridViewStagingEnvironmentModuleCache);
                    highlightKeyword(dataGridViewStagingEnvironmentModuleRunning);
                    highlightKeyword(dataGridViewStagingModules);
                    highlightKeyword(dataGridViewStagingModuleVersionRefererences);
                    highlightKeyword(dataGridViewStagingProducerElements);
                    highlightKeyword(dataGridViewStagingSiteProperties);
                    highlightKeyword(dataGridViewStaginglogs);
                    highlightKeyword(dataGridViewStagingApplicationVersion);
                    highlightKeyword(dataGridViewStagingMessage);
                    highlightKeyword(dataGridViewStagingModuleInconsistencies);
                    highlightKeyword(dataGridViewStagingModuleVersion);
                    highlightKeyword(dataGridViewStagingModuleVersionPublished);
                    highlightKeyword(dataGridViewStagingModuleVersionUploaded);
                    highlightKeyword(dataGridViewStagingOptions);
                    highlightKeyword(dataGridViewStagingOutdatedApplication);
                    highlightKeyword(dataGridViewStagingOutdatedModule);
                }

                if (datetimeFilterSuccessful)
                {
                    MessageBox.Show("The data was filtered by the datetime range provided", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
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

                    datetimeFilterSuccessful = true;
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
            findKeyword = false;
            removeGarbageSuccessful = false;
            highlightErrorSuccessful = false;
            findKeywordSuccessful = false;
            screenshotSuccessful = false;
            datetimeFilterSuccessful = false;

            dateTimePicker1.Text = "";
            dateTimePicker2.Text = "";
            maskedTextBox1.Text = "";
            maskedTextBox2.Text = "";
            txtBoxKeyword.Text = "";
            maskedTextBox1.BackColor = SystemColors.Window;
            maskedTextBox2.BackColor = SystemColors.Window;
            txtBoxKeyword.BackColor = SystemColors.Window;

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
                    else if (fileName == "general_text_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "MESSAGE_TYPE", "MESSAGE" };
                        populateTables(relativePath + "\\general_text_logs.txt", delimiters, column_names, dataGridViewGeneralTXTlogs);
                    }
                    else if (fileName == "timer_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "DURATION", "APPLICATION_NAME", "APPLICATION_KEY", "EXECUTED_BY",
                                    "ESPACE_NAME", "ESPACE_ID", "CYCLIC_JOB_NAME", "CYCLIC_JOB_KEY", "SHOULD_HAVE_RUN_AT", "NEXT_RUN",
                                    "ERROR_ID", "REQUEST_KEY", "TENANT_ID" };
                        populateTables(relativePath + "\\timer_logs.txt", delimiters, column_names, dataGridViewTimerlogs);
                    }
                    else if (fileName == "bpt_troubleshootingreport_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "ESPACE_NAME", "PROCESS_NAME", "PROCESS_STATUS", "PROCESS_LAST_MODIFIED",
                                    "PROCESS_SUSPENDED_DATE", "PROCESS_ID", "PARENT_PROCESS_ID", "ACTIVITY_CREATED", "ACTIVITY_NAME", "ACTIVITY_KIND",
                                    "ACTIVITY_STATUS", "ACTIVITY_RUNNING_SINCE", "ACTIVITY_NEXT_RUN", "ACTIVITY_CLOSED", "ACTIVITY_ERROR_COUNT", "ACTIVITY_ERROR_ID" };
                        populateTables(relativePath + "\\bpt_troubleshootingreport_logs.txt", delimiters, column_names, dataGridViewBPTReportslogs);
                    }
                    else if (fileName == "environment_capabilities.txt")
                    {
                        string[] column_names = { "ID", "ENVIRONMENT", "CAPABILITY" };
                        populateTables(relativePath + "\\environment_capabilities.txt", delimiters, column_names, dataGridViewEnvironmentCapabilitieslogs);
                    }
                    else if (fileName == "environments.txt")
                    {
                        string[] column_names = { "IS_LIFE_TIME", "IS_REGISTERED", "ID", "NAME", "DESCRIPTION", "HOST", "PUBLIC_HOST", "TYPE", "PRIMARY_CONTACT", "ORDER",
                                "NUMBER_OF_USERS", "IS_ACTIVE", "IS_OFFLINE", "LAST_SET_OFFLINE", "CREATED_BY", "CREATED_ON", "USE_HTTPS", "VERSION", "LAST_UPGRADE_VERSION",
                                "UID", "CALLBACK_ADDRESS", "NUMBER_OF_FRONTEND_SERVERS", "FIRST_SYNC_FINISHED", "CLOUD_PROVIDER_TYPE", "ENVIRONMENT_STACK", "ADDITIONAL_INFO",
                                "LAST_CACHE_INVALIDATION", "ENVIRONMENT_DB_PROVIDER", "ENVIRONMENT_SERVER_KIND", "TWO_STEP_PUBLISH_MODE", "LAST_ENV_SYNC_SEQUENTIAL_NUMBER",
                                "DATA_HARVESTED_TO", "IS_IN_OUTSYSTEMS_CLOUD", "BEST_HASHING_ALGORITHM", "ALLOW_NATIVE_BUILDS" };
                        populateTables(relativePath + "\\environments.txt", delimiters, column_names, dataGridViewEnvironmentslogs);
                    }
                    else if (fileName == "full_error_dump_logs.txt")
                    {
                        string[] column_names = { "PLATFORM_INFORMATION" };
                        populateTables(relativePath + "\\full_error_dump_logs.txt", delimiters, column_names, dataGridViewFullErrorDumps);
                    }
                    else if (fileName == "roles.txt")
                    {
                        string[] column_names = { "ID", "NAME", "DESCRIPTION", "ORDER", "CAN_CONFIGURE_INFRASTRUCTURE", "CAN_CONFIGURE_ROLES", "CAN_CONFIGURE_USERS",
                                    "CAN_CONFIGURE_APPLICATION_ROLES", "KEY", "ID_2", "LABEL_OLD", "SHORT_LABEL_OLD", "DESCRIPTION_OLD", "LABEL", "SHORT_LABEL", "DESCRIPTION_2",
                                    "LT_LEVEL", "SC_LEVEL", "APPLICATION_LEVEL", "IS_OLD_LEVEL", "ID_3", "LABEL_2", "SHORT_LABEL_2", "DESCRIPTION_3", "LEVEL", "IS_COMPUTED",
                                    "ID_4", "ROLE", "ENVIRONMENT", "DEFAULT_PERMISSION_LEVEL", "CAN_CREATE_APPLICATIONS", "CAN_REFERENCE_SYSTEMS", "IS_INITIALIZED" };
                        populateTables(relativePath + "\\roles.txt", delimiters, column_names, dataGridViewRoleslogs);
                    }
                    else if (fileName == "roles_in_applications.txt")
                    {
                        string[] column_names = { "ID", "USER", "ROLE", "APPLICATION" };
                        populateTables(relativePath + "\\roles_in_applications.txt", delimiters, column_names, dataGridViewRolesInApplicationslogs);
                    }
                    else if (fileName == "roles_in_teams.txt")
                    {
                        string[] column_names = { "ID", "ROLE", "KEY", "TEAM", "NAME", "KEY_2" };
                        populateTables(relativePath + "\\roles_in_teams.txt", delimiters, column_names, dataGridViewRolesInTeamslogs);
                    }
                    else if (fileName == "sync_errors.txt")
                    {
                        string[] column_names = { "DATE_TIME", "SESSION_ID", "MESSAGE", "STACK", "MODULE", "USERNAME", "ENDPOINT", "ACTION", "PROCESS_NAME",
                                    "ACTIVITY_NAME" };
                        populateTables(relativePath + "\\sync_errors.txt", delimiters, column_names, dataGridViewSyncErrorslogs);
                    }
                    else if (fileName == "user.txt")
                    {
                        string[] column_names = { "ID", "NAME", "USERNAME", "EXTERNAL", "CREATION_DATE", "LAST_LOGIN", "IS_ACTIVE", "ID_2", "NAME_2",
                                    "DESCRIPTION", "ORDER", "CAN_CONFIGURE_INFRASTRUCTURE", "CAN_CONFIGURE_ROLES", "CAN_CONFIGURE_USERS", "CAN_CONFIGURE_APPLICATION_ROLES",
                                    "KEY", "USER_ROLE", "KEY_2", "MTSI_IDENTIFIER" };
                        populateTables(relativePath + "\\user.txt", delimiters, column_names, dataGridViewUserlogs);
                    }
                    else if (fileName == "user_pools.txt")
                    {
                        string[] column_names = { "USER", "POOL_KEY" };
                        populateTables(relativePath + "\\user_pools.txt", delimiters, column_names, dataGridViewUserPoolslogs);
                    }
                    else if (fileName == "application.txt")
                    {
                        string[] column_names = { "ID", "TEAM", "NAME", "DESCRIPTION", "PRIMARY_CONTACT", "URL_PATH", "KEY", "LOGO_HASH", "IS_ACTIVE", "DEFAULT_THEME_IS_MOBILE",
                                    "MONITORING_ENABLED", "APPLICATION_KIND" };
                        populateTables(relativePath + "\\application.txt", delimiters, column_names, dataGridViewStagingApplogs);
                    }
                    else if (fileName == "application_version.txt")
                    {
                        string[] column_names = { "ID", "APPLICATION", "VERSION", "CHANGE_LOG", "CREATED_ON", "CREATED_BY", "CREATED_ON_ENVIRONMENT", "FRONT_OFFICE_ESPACE_KEY",
                                    "FRONT_OFFICE_ESPACE_NAME", "BACK_OFFICE_ESPACE_KEY", "BACK_OFFICE_ESPACE_NAME", "WEB_THEME_GLOBAL_KEY", "MOBILE_THEME_GLOBAL_KEY",
                                    "WAS_AUTO_TAGGED", "VERSION_DECIMAL", "TEMPLATE_KEY", "PRIMARY_COLOR", "NATIVE_HASH", "KEY" };
                        populateTables(relativePath + "\\application_version.txt", delimiters, column_names, dataGridViewStagingAppVerlogs);
                    }
                    else if (fileName == "application_version_module_version.txt")
                    {
                        string[] column_names = { "ID", "APPLICATION_VERSION", "MODULE_VERSION", "ID_2", "MODULE", "HASH", "GENERAL_HASH", "CREATED_ON",
                                    "CREATED_BY", "CREATED_ON_ENVIRONMENT", "LAST_UPGRADE_VERSION", "DIRECT_UPGRADE_FROM_VERSION_HASH", "COMPATIBILITY_SIGNATURE_HASH",
                                    "KEY" };
                        populateTables(relativePath + "\\application_version_module_version.txt", delimiters, column_names, dataGridViewStagingAppVerModuleVerlogs);
                    }
                    else if (fileName == "change_log.txt")
                    {
                        string[] column_names = { "ID", "LABEL", "ID_2", "DATE_TIME", "MESSAGE", "FIRST_OBJECT_TYPE", "FIRST_OBJECT", "SECOND_OBJECT_TYPE",
                                    "SECOND_OBJECT", "IS_WRITE", "IS_SUCCESSFUL", "IS_SYSTEM", "ENTRY_ESPACE", "USER", "CLIENT_IP" };
                        populateTables(relativePath + "\\change_log.txt", delimiters, column_names, dataGridViewStagingChangelog);
                    }
                    else if (fileName == "consumer_elements.txt")
                    {
                        string[] column_names = { "CONSUMER_MODULE", "CONSUMER_MODULE_NAME", "CONSUMER_MODULE_VERSION", "CONSUMER_ELEMENT_VERSION", "CONSUMER_ELEMENT_VERSION_KEY", "CONSUMER_ELEMENT_VERSION_TYPE",
                                    "CONSUMER_ELEMENT_VERSION_NAME", "CONSUMER_ELEMENT_VERSION_COMPATIBILITY_HASH", "PRODUCER_MODULE", "PRODUCER_MODULE_KEY", "PRODUCER_MODULE_NAME", "PRODUCER_MODULE_TYPE", "CREATED_ON_PRODUCER_MODULE_VERSION" };
                        populateTables(relativePath + "\\consumer_elements.txt", delimiters, column_names, dataGridViewStagingConsumerElements);
                    }
                    else if (fileName == "entity_configurations.txt")
                    {
                        string[] column_names = { "ID", "ENTITY_KEY", "MODULE_VERSION_ID", "CREATED_ON", "UPDATED_ON", "ID_2",
                                    "MODULE_ID", "STAGING_ID", "CREATED_ON_2", "CREATED_BY", "UPDATED_ON_2", "UPDATED_BY", "ENTITY_KEY_2",
                                    "PHYSICAL_TABLE_NAME", "IS_OVERRIDEN_TABLE_NAME", "DEFAULT_PHYSICAL_TABLE_NAME", "ENTITY_NAME",
                                    "SOURCE_PHYSICAL_TABLE_NAME", "TARGET_PHYSICAL_TABLE_NAME" };
                        populateTables(relativePath + "\\entity_configurations.txt", delimiters, column_names, dataGridViewStagingEntityConfiguration);
                    }
                    else if (fileName == "environment_app_version.txt")
                    {
                        string[] column_names = { "ID", "ENVIRONMENT", "APPLICATION_VERSION" };
                        populateTables(relativePath + "\\environment_app_version.txt", delimiters, column_names, dataGridViewStagingEnvironmentApplicationVersion);
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
                        populateTables(relativePath + "\\environment_application_cache.txt", delimiters, column_names, dataGridViewStagingEnvironmentAppicationCache);
                    }
                    else if (fileName == "environment_application_module.txt")
                    {
                        string[] column_names = { "ID", "APPLICATION", "MODULE", "ENVIRONMENT", "LAST_CHANGED_ON" };
                        populateTables(relativePath + "\\environment_application_module.txt", delimiters, column_names, dataGridViewStagingEnvironmentApplicationModule);
                    }
                    else if (fileName == "environment_module_cache.txt")
                    {
                        string[] column_names = { "ID", "ENVIRONMENT", "MODULE_VERSION", "INTERNAL_VERSION", "PUBLISHED_ON", "PUBLISHED_BY", "ID_2", "ENVIRONMENT_2", "MODULE",
                                    "PUBLISH", "CHANGE_STATUS", "CHANGE_STATUS_MESSAGE", "DELETED", "CONSISTENCY_STATUS", "CONSISTENCY_STATUS_MESSAGES", "IS_OUTDATED" };
                        populateTables(relativePath + "\\environment_module_cache.txt", delimiters, column_names, dataGridViewStagingEnvironmentModuleCache);
                    }
                    else if (fileName == "environment_module_running.txt")
                    {
                        string[] column_names = { "ID", "ENVIRONMENT", "CONSUMER_MODULE", "PRODUCER_MODULE_KEY", "PRODUCER_COMPATIBILITY_HASH", "IS_WEAK_REFERENCE" };
                        populateTables(relativePath + "\\environment_module_running.txt", delimiters, column_names, dataGridViewStagingEnvironmentModuleRunning);
                    }
                    else if (fileName == "module_version_references.txt")
                    {
                        string[] column_names = { "ID", "MODULE_VERSION_REFERENCE", "PRODUCER_MODULE_VERSION", "IS_COMPATIBLE", "IS_IN_DIFFERENT_LUV", "PLATFORM_VERSION",
                                    "ID_2", "MODULE_VERSION", "PRODUCER_MODULE", "IS_WEAK_REFERENCE", "ID_3", "MODULE_VERSION_REFERENCE_STATUS", "ELEMENT_NAME", "ELEMENT_KEY",
                                    "ELEMENT_TYPE", "ELEMENT_REF_INCONSISTENCY_TYPE_I" };
                        populateTables(relativePath + "\\module_version_references.txt", delimiters, column_names, dataGridViewStagingModuleVersionRefererences);
                    }
                    else if (fileName == "modules.txt")
                    {
                        string[] column_names = { "ID", "LABEL", "TOKEN", "ID_2", "NAME", "DESCRIPTION", "KEY", "TYPE" };
                        populateTables(relativePath + "\\modules.txt", delimiters, column_names, dataGridViewStagingModules);
                    }
                    else if (fileName == "producer_elements.txt")
                    {
                        string[] column_names = { "MODULE", "MODULE_NAME", "MODULE_VERSION", "ELEMENT_VERSION", "ELEMENT_VERSION_KEY", "ELEMENT_VERSION_TYPE", "ELEMENT_VERSION_NAME", "ELEMENT_VERSION_COMPATIBILITY_HASH" };
                        populateTables(relativePath + "\\producer_elements.txt", delimiters, column_names, dataGridViewStagingProducerElements);
                    }
                    else if (fileName == "site_properties.txt")
                    {
                        string[] column_names = { "ID", "SS_KEY", "MODULE_VERSION_ID", "NAME", "DATA_TYPE_ID", "DESCRIPTION", "DEFAULT_VALUE", "IS_MULTI_TENANT", "CREATED_ON", "UPDATED_ON", "ID_2", "LABEL", "ORDER", "IS_ACTIVE",
                                    "ID_3", "EFFECTIVE_VALUE_CHANGED_IN_STAGING", "EFFECTIVE_VALUE_ON_TARGET", "SITE_PROPERTY_SS_KEY", "IS_MULTI_TENANT_2", "MODULE_ID", "STAGING_ID", "IS_NEW", "CREATED_ON_2", "CREATED_BY",
                                    "UPDATED_ON_2", "UPDATED_BY" };
                        populateTables(relativePath + "\\site_properties.txt", delimiters, column_names, dataGridViewStagingSiteProperties);
                    }
                    else if (fileName == "staging.txt")
                    {
                        string[] column_names = { "ID", "SOURCE_ENVIRONMENT", "TARGET_ENVIRONMENT", "LABEL", "INTERNAL", "CREATED_BY", "CREATED_ON", "STARTED_BY", "STARTED_ON", "FINISHED_ON", "IS_DRAFT", "SOLUTION_PUBLISHED_FINISHED",
                                    "IS_WAITING_FOR_CONFIRMATION_PROCESS", "SAVED_BY", "SAVED_ON", "LAST_REFRESHED_ON", "SYNC_FINISHED_ON", "SOURCE_STAGING", "STAGING_CONFIRMATION_KIND", "TWO_STEP_MODE", "LAST_RESUME_DATE_TIME", "MARKED_FOR_ABORT_ON",
                                    "ABORTED_BY", "KEY", "STATUS" };
                        populateTables(relativePath + "\\staging.txt", delimiters, column_names, dataGridViewStaginglogs);
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
                        populateTables(relativePath + "\\staging_application_version.txt", delimiters, column_names, dataGridViewStagingApplicationVersion);
                    }
                    else if (fileName == "staging_message.txt")
                    {
                        string[] column_names = { "ID", "STAGING", "MESSAGE", "DETAIL", "EXTRA_INFO", "INTERNAL_ID", "INTERNAL_TYPE", "TYPE", "DATE_TIME" };
                        populateTables(relativePath + "\\staging_message.txt", delimiters, column_names, dataGridViewStagingMessage);
                    }
                    else if (fileName == "staging_module_inconsistency.txt")
                    {
                        string[] column_names = { "ID", "STAGING", "CONSUMER_MODULE", "PRODUCER_MODULE", "FIRST_REQUIRED_ELEMENT", "FIRST_REQUIRED_ELEMENT_TYPE", "TOTAL_REQUIRED_ELEMENTS", "PRODUCER_MODULE_NAME", "CONSUMER_MODULE_NAME", "INCONSISTENCY_TYPE" };
                        populateTables(relativePath + "\\staging_module_inconsistency.txt", delimiters, column_names, dataGridViewStagingModuleInconsistencies);
                    }
                    else if (fileName == "staging_module_version.txt")
                    {
                        string[] column_names = { "ID", "STAGING", "APPLICATION", "PREVIOUS_APPLICATION", "MODULE", "MODULE_VERSION", "MODULE_DELETED", "PREVIOUS_MODULE_VERSION", "PREVIOUS_MODULE_DELETED", "OPERATION" };
                        populateTables(relativePath + "\\staging_module_version.txt", delimiters, column_names, dataGridViewStagingModuleVersion);
                    }
                    else if (fileName == "staging_module_version_to_publish.txt")
                    {
                        string[] column_names = { "ID", "STAGING", "PLANNED_MODULE_VERSION_HASH", "MODULE_VERSION_HASH_TO_PUBLISH" };
                        populateTables(relativePath + "\\staging_module_version_to_publish.txt", delimiters, column_names, dataGridViewStagingModuleVersionPublished);
                    }
                    else if (fileName == "staging_module_version_to_upload.txt")
                    {
                        string[] column_names = { "ID", "STAGING", "USER", "ENVIRONMENT_ID_1", "ENVIRONMENT_ID_2", "TYPE", "NAME", "MODULE_KEY", "VERSION_KEY", "DIRECT_UPGRADE_FROM_VERSION_KEY", "APPLICATION_KEY" };
                        populateTables(relativePath + "\\staging_module_version_to_upload.txt", delimiters, column_names, dataGridViewStagingModuleVersionUploaded);
                    }
                    else if (fileName == "staging_option.txt")
                    {
                        string[] column_names = { "ID", "STAGING", "APPLICATION", "STAGING_OPTION_TYPE", "APPLICATION_VERSION", "APPLICATION_VERSION_LABEL", "LABEL", "APPLICATION_DESCRIPTION", "IS_TOP_OPTION", "iOS_VERSION_LABEL", "ANDROID_VERSION_LABEL",
                                    "MOBILE_APPS_DESCRIPTION" };
                        populateTables(relativePath + "\\staging_option.txt", delimiters, column_names, dataGridViewStagingOptions);
                    }
                    else if (fileName == "staging_outdated_application.txt")
                    {
                        string[] column_names = { "ID", "STAGING", "APPLICATION" };
                        populateTables(relativePath + "\\staging_outdated_application.txt", delimiters, column_names, dataGridViewStagingOutdatedApplication);
                    }
                    else if (fileName == "staging_outdated_module.txt")
                    {
                        string[] column_names = { "ID", "STAGING", "MODULE" };
                        populateTables(relativePath + "\\staging_outdated_module.txt", delimiters, column_names, dataGridViewStagingOutdatedModule);
                    }
                    else if (fileName == "windows_application_event_viewer_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "LEVEL", "MESSAGE", "TASK", "COMPUTER", "PROVIDER_NAME",
                                    "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID", "KEYWORDS" };
                        populateTables(relativePath + "\\windows_application_event_viewer_logs.txt", delimiters, column_names, dataGridViewWinAppEventViewer);
                    }
                    else if (fileName == "windows_security_event_viewer_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "LEVEL", "MESSAGE", "TASK", "COMPUTER", "PROVIDER_NAME",
                                    "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID", "KEYWORDS" };
                        populateTables(relativePath + "\\windows_security_event_viewer_logs.txt", delimiters, column_names, dataGridViewWinSecEventViewer);
                    }
                    else if (fileName == "windows_system_event_viewer_logs.txt")
                    {
                        string[] column_names = { "DATE_TIME", "LEVEL", "MESSAGE", "TASK", "COMPUTER", "PROVIDER_NAME",
                                    "QUALIFIERS", "EVENT_ID", "EVENT_RECORD_ID", "KEYWORDS" };
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
                    btnScreenshot.Enabled = false;
                    btnSearchKeyword.Enabled = true;
                    btnSearchKeyword.BackColor = SystemColors.ControlLight;
                    txtBoxKeyword.Enabled = true;
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
                    btnScreenshot.Enabled = false;
                    btnSearchKeyword.Enabled = false;
                    txtBoxKeyword.Enabled = false;
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
            //removeGenericErrors(dataGridViewGenerallogs, 1, txtBoxDetailGenerallogs);
            //removeGenericErrors2(dataGridViewIntegrationslogs, txtBoxDetailIntegrationlogs);
            //removeGenericErrors2(dataGridViewScreenRequestslogs, txtBoxDetailScreenRequestslogs);
            //removeGenericErrors2(dataGridViewTimerlogs, txtBoxDetailTimerlogs);
            //removeGenericErrors2(dataGridViewEmaillogs, txtBoxDetailEmaillogs);
            //removeGenericErrors2(dataGridViewExtensionlogs, txtBoxDetailExtensionlogs);
            //removeGenericErrors2(dataGridViewServiceActionlogs, txtBoxDetailServiceActionlogs);
            //removeGenericErrors2(dataGridViewTradWebRequests, txtBoxDetailTradWebRequests);
            //removeGenericErrors2(dataGridViewIISDateTime, txtDetailIISlogs);
            //removeGenericErrors2(dataGridViewIISTimeTaken, txtDetailIISlogs);
            removeGenericErrors(dataGridViewWinAppEventViewer, 2, txtBoxDetailWinAppEventViewer);
            removeGenericErrors(dataGridViewWinSysEventViewer, 2, txtBoxDetailWinSysEventViewer);
            removeGenericErrors(dataGridViewWinSecEventViewer, 2, txtBoxDetailWinSecEventViewer);
            removeGenericErrors(dataGridViewAndroidlogs, 3, txtBoxDetailAndroidLogs);
            removeGenericErrors(dataGridViewiOSlogs, 3, txtBoxDetailiOSLogs);
            removeGenericErrors(dataGridViewServiceStudiologs, 3, txtBoxDetailServiceStudioLogs);
            removeGenericErrors(dataGridViewGeneralTXTlogs, 2, txtBoxDetailGeneralTXTLogs);
            //removeGenericErrors(dataGridViewBPTReportslogs, txtBoxDetailBPTReportslogs);

            if (highlightError)
            {
                string[] knownErrors_Errorlogs = { "url rewrite module error", "an error occurred in task", "server cannot modify cookies", "ping validation failed", "a fatal error has occurred", "communicationexception", "json deserialization", "compilation error", "file is corrupt or invalid", "checksum failed for file", "connection failed421", "temporary server error", "can't proceed", "truncated in table", "unknown reference expression type email" };
                string[] knownErrors_Generallogs = { "system cannot find" };
                string[] knownErrors_WinAppEventViewer = { "ora-", "error closing", "cannot access", "error opening" };
                string[] knownErrors_WinSysEventViewer = { "error closing", "timed out" };
                string[] knownErrors_AndroidiOSlogs = { "file is corrupt or invalid", "androidx library", "command finished with error code 0", "plugin is not going to work", "error: spawnsync sudo etimeout", "plugin doesn't support this project's cordova-android version", "failed to fetch plug", "archive failed", "build failed with the following error", "command failed with exit code", "signing certificate is invalid", "the ios deployment target", "verification failed" };
                string[] knownErrors_ServiceStudiologs = { "oneoftypedefinition", "http forbidden", "[not connected]" };

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
                //highlightKnownErrors(dataGridViewGeneralTXTlogs, 2);
                //highlightKnownErrors(dataGridViewBPTReportslogs);
            }

            if (findKeyword)
            {
                highlightKeyword(dataGridViewErrorlogs);
                highlightKeyword(dataGridViewGenerallogs);
                highlightKeyword(dataGridViewIntegrationslogs);
                highlightKeyword(dataGridViewScreenRequestslogs);
                highlightKeyword(dataGridViewTimerlogs);
                highlightKeyword(dataGridViewEmaillogs);
                highlightKeyword(dataGridViewExtensionlogs);
                highlightKeyword(dataGridViewServiceActionlogs);
                highlightKeyword(dataGridViewTradWebRequests);
                highlightKeyword(dataGridViewIISDateTime);
                highlightKeyword(dataGridViewIISTimeTaken);
                highlightKeyword(dataGridViewWinAppEventViewer);
                highlightKeyword(dataGridViewWinSysEventViewer);
                highlightKeyword(dataGridViewWinSecEventViewer);
                highlightKeyword(dataGridViewAndroidlogs);
                highlightKeyword(dataGridViewiOSlogs);
                highlightKeyword(dataGridViewServiceStudiologs);
                highlightKeyword(dataGridViewGeneralTXTlogs);
                highlightKeyword(dataGridViewBPTReportslogs);
                highlightKeyword(dataGridViewEnvironmentCapabilitieslogs);
                highlightKeyword(dataGridViewEnvironmentslogs);
                highlightKeyword(dataGridViewFullErrorDumps);
                highlightKeyword(dataGridViewRoleslogs);
                highlightKeyword(dataGridViewRolesInApplicationslogs);
                highlightKeyword(dataGridViewRolesInTeamslogs);
                highlightKeyword(dataGridViewUserlogs);
                highlightKeyword(dataGridViewUserPoolslogs);
                highlightKeyword(dataGridViewSyncErrorslogs);
                highlightKeyword(dataGridViewStagingApplogs);
                highlightKeyword(dataGridViewStagingAppVerlogs);
                highlightKeyword(dataGridViewStagingAppVerModuleVerlogs);
                highlightKeyword(dataGridViewStagingChangelog);
                highlightKeyword(dataGridViewStagingConsumerElements);
                highlightKeyword(dataGridViewStagingEntityConfiguration);
                highlightKeyword(dataGridViewStagingEnvironmentAppicationCache);
                highlightKeyword(dataGridViewStagingEnvironmentApplicationModule);
                highlightKeyword(dataGridViewStagingEnvironmentApplicationVersion);
                highlightKeyword(dataGridViewStagingEnvironmentModuleCache);
                highlightKeyword(dataGridViewStagingEnvironmentModuleRunning);
                highlightKeyword(dataGridViewStagingModules);
                highlightKeyword(dataGridViewStagingModuleVersionRefererences);
                highlightKeyword(dataGridViewStagingProducerElements);
                highlightKeyword(dataGridViewStagingSiteProperties);
                highlightKeyword(dataGridViewStaginglogs);
                highlightKeyword(dataGridViewStagingApplicationVersion);
                highlightKeyword(dataGridViewStagingMessage);
                highlightKeyword(dataGridViewStagingModuleInconsistencies);
                highlightKeyword(dataGridViewStagingModuleVersion);
                highlightKeyword(dataGridViewStagingModuleVersionPublished);
                highlightKeyword(dataGridViewStagingModuleVersionUploaded);
                highlightKeyword(dataGridViewStagingOptions);
                highlightKeyword(dataGridViewStagingOutdatedApplication);
                highlightKeyword(dataGridViewStagingOutdatedModule);
            }

            if (removeGarbageSuccessful)
            {
                MessageBox.Show("Duplicate generic lines were hidden", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            btnRemoveGarbage.Enabled = false;
            numericUpDownPercentage.Enabled = false;
        }

        private void removeGenericErrors(DataGridView tableName, int columnIndex, TextBox txtbox)
        {
            try
            {
                if (tableName.Rows.Count > 0)
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

                                    removeGarbageSuccessful = true;
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

            string[] knownErrors_Errorlogs = { "url rewrite module error", "an error occurred in task", "server cannot modify cookies", "ping validation failed", "a fatal error has occurred", "communicationexception", "json deserialization", "compilation error", "file is corrupt or invalid", "checksum failed for file", "connection failed421", "temporary server error", "can't proceed", "truncated in table", "unknown reference expression type email" };
            string[] knownErrors_Generallogs = { "system cannot find" };
            string[] knownErrors_WinAppEventViewer = { "ora-", "error closing", "cannot access", "error opening" };
            string[] knownErrors_WinSysEventViewer = { "error closing", "timed out" };
            string[] knownErrors_AndroidiOSlogs = { "file is corrupt or invalid", "androidx library", "command finished with error code 0", "plugin is not going to work", "error: spawnsync sudo etimeout", "plugin doesn't support this project's cordova-android version", "failed to fetch plug", "archive failed", "build failed with the following error", "command failed with exit code", "signing certificate is invalid", "the ios deployment target", "verification failed" };
            string[] knownErrors_ServiceStudiologs = { "oneoftypedefinition", "http forbidden", "[not connected]" };

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
            //highlightKnownErrors(dataGridViewGeneralTXTlogs, 2);
            //highlightKnownErrors(dataGridViewBPTReportslogs);

            if (removeGarbage)
            {
                removeGenericErrors(dataGridViewErrorlogs, 1, txtBoxDetailErrorLogs);
                //removeGenericErrors(dataGridViewGenerallogs, 1, txtBoxDetailGenerallogs);
                //removeGenericErrors2(dataGridViewIntegrationslogs, txtBoxDetailIntegrationlogs);
                //removeGenericErrors2(dataGridViewScreenRequestslogs, txtBoxDetailScreenRequestslogs);
                //removeGenericErrors2(dataGridViewTimerlogs, txtBoxDetailTimerlogs);
                //removeGenericErrors2(dataGridViewEmaillogs, txtBoxDetailEmaillogs);
                //removeGenericErrors2(dataGridViewExtensionlogs, txtBoxDetailExtensionlogs);
                //removeGenericErrors2(dataGridViewServiceActionlogs, txtBoxDetailServiceActionlogs);
                //removeGenericErrors2(dataGridViewTradWebRequests, txtBoxDetailTradWebRequests);
                //removeGenericErrors2(dataGridViewIISDateTime, txtDetailIISlogs);
                //removeGenericErrors2(dataGridViewIISTimeTaken, txtDetailIISlogs);
                removeGenericErrors(dataGridViewWinAppEventViewer, 2, txtBoxDetailWinAppEventViewer);
                removeGenericErrors(dataGridViewWinSysEventViewer, 2, txtBoxDetailWinSysEventViewer);
                removeGenericErrors(dataGridViewWinSecEventViewer, 2, txtBoxDetailWinSecEventViewer);
                removeGenericErrors(dataGridViewAndroidlogs, 3, txtBoxDetailAndroidLogs);
                removeGenericErrors(dataGridViewiOSlogs, 3, txtBoxDetailiOSLogs);
                removeGenericErrors(dataGridViewServiceStudiologs, 3, txtBoxDetailServiceStudioLogs);
                removeGenericErrors(dataGridViewGeneralTXTlogs, 2, txtBoxDetailGeneralTXTLogs);
                //removeGenericErrors(dataGridViewBPTReportslogs, txtBoxDetailBPTReportslogs);
            }

            if (findKeyword)
            {
                highlightKeyword(dataGridViewErrorlogs);
                highlightKeyword(dataGridViewGenerallogs);
                highlightKeyword(dataGridViewIntegrationslogs);
                highlightKeyword(dataGridViewScreenRequestslogs);
                highlightKeyword(dataGridViewTimerlogs);
                highlightKeyword(dataGridViewEmaillogs);
                highlightKeyword(dataGridViewExtensionlogs);
                highlightKeyword(dataGridViewServiceActionlogs);
                highlightKeyword(dataGridViewTradWebRequests);
                highlightKeyword(dataGridViewIISDateTime);
                highlightKeyword(dataGridViewIISTimeTaken);
                highlightKeyword(dataGridViewWinAppEventViewer);
                highlightKeyword(dataGridViewWinSysEventViewer);
                highlightKeyword(dataGridViewWinSecEventViewer);
                highlightKeyword(dataGridViewAndroidlogs);
                highlightKeyword(dataGridViewiOSlogs);
                highlightKeyword(dataGridViewServiceStudiologs);
                highlightKeyword(dataGridViewGeneralTXTlogs);
                highlightKeyword(dataGridViewBPTReportslogs);
                highlightKeyword(dataGridViewEnvironmentCapabilitieslogs);
                highlightKeyword(dataGridViewEnvironmentslogs);
                highlightKeyword(dataGridViewFullErrorDumps);
                highlightKeyword(dataGridViewRoleslogs);
                highlightKeyword(dataGridViewRolesInApplicationslogs);
                highlightKeyword(dataGridViewRolesInTeamslogs);
                highlightKeyword(dataGridViewUserlogs);
                highlightKeyword(dataGridViewUserPoolslogs);
                highlightKeyword(dataGridViewSyncErrorslogs);
                highlightKeyword(dataGridViewStagingApplogs);
                highlightKeyword(dataGridViewStagingAppVerlogs);
                highlightKeyword(dataGridViewStagingAppVerModuleVerlogs);
                highlightKeyword(dataGridViewStagingChangelog);
                highlightKeyword(dataGridViewStagingConsumerElements);
                highlightKeyword(dataGridViewStagingEntityConfiguration);
                highlightKeyword(dataGridViewStagingEnvironmentAppicationCache);
                highlightKeyword(dataGridViewStagingEnvironmentApplicationModule);
                highlightKeyword(dataGridViewStagingEnvironmentApplicationVersion);
                highlightKeyword(dataGridViewStagingEnvironmentModuleCache);
                highlightKeyword(dataGridViewStagingEnvironmentModuleRunning);
                highlightKeyword(dataGridViewStagingModules);
                highlightKeyword(dataGridViewStagingModuleVersionRefererences);
                highlightKeyword(dataGridViewStagingProducerElements);
                highlightKeyword(dataGridViewStagingSiteProperties);
                highlightKeyword(dataGridViewStaginglogs);
                highlightKeyword(dataGridViewStagingApplicationVersion);
                highlightKeyword(dataGridViewStagingMessage);
                highlightKeyword(dataGridViewStagingModuleInconsistencies);
                highlightKeyword(dataGridViewStagingModuleVersion);
                highlightKeyword(dataGridViewStagingModuleVersionPublished);
                highlightKeyword(dataGridViewStagingModuleVersionUploaded);
                highlightKeyword(dataGridViewStagingOptions);
                highlightKeyword(dataGridViewStagingOutdatedApplication);
                highlightKeyword(dataGridViewStagingOutdatedModule);
            }

            if (highlightErrorSuccessful)
            {
                MessageBox.Show("Known errors were highlighted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            btnHighlight.Enabled = false;
        }

        private void highlightKnownErrors(DataGridView tableName, int columnIndex, string[] errorsList)
        {
            try
            {
                if (tableName.Rows.Count > 0)
                {
                    foreach (string error in errorsList)
                    {
                        foreach (DataGridViewRow row in tableName.Rows)
                        {
                            if (row.Cells[columnIndex].Value.ToString().ToLower().Contains(error))
                            {
                                row.DefaultCellStyle.BackColor = Color.Yellow;

                                highlightErrorSuccessful = true;
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
            btnScreenshot.Enabled = false;
            btnClearFilter.Enabled = false;
            btnSearchKeyword.Enabled = false;
            txtBoxKeyword.Enabled = false;

            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }

        private void btnScreenshot_Click(object sender, EventArgs e)
        {
            string txtDetailErrorLogs = "";

            if (!string.IsNullOrEmpty(txtBoxDetailErrorLogs.Text))
            {
                DataGridViewRow row = this.dataGridViewErrorlogs.Rows[dataGridViewErrorlogs.CurrentCell.RowIndex];
                txtDetailErrorLogs = "DATE_TIME: " + row.Cells["DATE_TIME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "MESSAGE: " + row.Cells["MESSAGE"].Value.ToString() + Environment.NewLine + Environment.NewLine + "STACK: " + row.Cells["STACK"].Value.ToString() + Environment.NewLine + Environment.NewLine + "MODULE_NAME: " + row.Cells["MODULE_NAME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "APPLICATION_NAME: " + row.Cells["APPLICATION_NAME"].Value.ToString() + Environment.NewLine + Environment.NewLine + "SERVER: " + row.Cells["SERVER"].Value.ToString() + Environment.NewLine + Environment.NewLine + "ENVIRONMENT_INFORMATION: " + row.Cells["ENVIRONMENT_INFORMATION"].Value.ToString();
            }

            createScreenshot("detailed_error_logs", txtDetailErrorLogs, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_general_logs", txtBoxDetailGenerallogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_integrations_logs", txtBoxDetailIntegrationlogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_screen_requests_logs", txtBoxDetailScreenRequestslogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_timer_logs", txtBoxDetailTimerlogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_email_logs", txtBoxDetailEmaillogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_extension_logs", txtBoxDetailExtensionlogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_service_action_logs", txtBoxDetailServiceActionlogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_tradional_web_requests_logs", txtBoxDetailTradWebRequests.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_windows_application_viewer_logs", txtBoxDetailWinAppEventViewer.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_windows_system_viewer_logs", txtBoxDetailWinSysEventViewer.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_windows_security_viewer_logs", txtBoxDetailWinSecEventViewer.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_android_logs", txtBoxDetailAndroidLogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_ios_logs", txtBoxDetailiOSLogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_iis_logs", txtDetailIISlogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_service_studio_logs", txtBoxDetailServiceStudioLogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_general_text_logs", txtBoxDetailGeneralTXTLogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_bpt_reports_logs", txtBoxDetailBPTReportslogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_environment_capabilities_logs", txtBoxDetailEnvironmentCapabilitieslogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_environments_logs", txtBoxDetailEnvironmentslogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_full_error_dump_logs", txtBoxDetailFullErrorDumpslogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_roles_logs", txtBoxDetailRoleslogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_roles_in_applications_logs", txtBoxDetailRolesInApplicationslogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_roles_in_teams_logs", txtBoxDetailRolesInTeamslogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_sync_error_logs", txtBoxDetailSyncErrorslogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_user_logs", txtBoxDetailUserlogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_user_pool_logs", txtBoxDetailUserPoolslogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_application_logs", txtBoxDetailStagingApplogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_application_version_logs", txtBoxDetailStagingAppVerlogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_application_version_module_version_logs", txtBoxDetailStagingAppVerModuleVerlogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_change_logs", txtBoxDetailStagingChangelog.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_consumer_elements_logs", txtBoxDetailStagingConsumerElementslogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_entity_configurations_logs", txtBoxDetailStagingEntityConfiguration.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_environment_application_version_logs", txtBoxDetailStagingEnviromentApplicationVersion.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_environment_application_cache_logs", txtBoxDetailStagingEnvironmentApplicationCache.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_environment_application_module_logs", txtBoxDetailStagingEnvironmentApplicationModule.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_environment_module_cache_logs", txtBoxDetailStagingEnvironmentModuleCache.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_environment_module_running_logs", txtBoxDetailStagingEnvironmentModuleRunning.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_module_version_references_logs", txtBoxDetailStagingModuleVersionReferences.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_modules_logs", txtBoxDetailStagingModules.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_producer_elements_logs", txtBoxDetailStagingProducerElements.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_site_properties_logs", txtBoxDetailStagingSiteProperties.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_staging_logs", txtBoxDetailStaginglogs.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_staging_application_version_logs", txtBoxDetailStagingApplicationVersion.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_staging_message_logs", txtBoxDetailStagingMessage.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_staging_module_inconsistency_logs", txtBoxDetailStagingModuleInconsistencies.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_staging_module_version_logs", txtBoxDetailStagingModuleVersion.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_staging_module_version_publish_logs", txtBoxDetailStagingModuleVersionPublished.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_staging_module_version_upload_logs", txtBoxDetailStagingModuleVersionUploaded.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_staging_option_logs", txtBoxDetailStagingOptions.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_staging_oudated_application_logs", txtBoxDetailStagingOutdatedApplication.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);
            createScreenshot("detailed_staging_oudated_module_logs", txtBoxDetailStagingOutdatedModule.Text, new Font("Times New Roman", 10), Color.Black, SystemColors.ControlLight);

            if (screenshotSuccessful)
            {
                MessageBox.Show("A screenshot of the error was taken", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void createScreenshot(String filename, String text, Font font, Color textColor, Color backColor)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    //first, create a dummy bitmap just to get a graphics object
                    Image img = new Bitmap(1, 1);
                    Graphics drawing = Graphics.FromImage(img);

                    //insert a new line after every 10 words
                    StringBuilder sb = new StringBuilder(text);
                    int spaces = 0;
                    int length = sb.Length;
                    for (int i = 0; i < length; i++)
                    {
                        if (sb[i] == ' ')
                        {
                            spaces++;
                        }
                        if (spaces == 10)
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
                    string screenshotsFolder = System.IO.Path.Combine(currentWorkingDirectory, "screenshots");
                    System.IO.Directory.CreateDirectory(screenshotsFolder);

                    string timeStamp = DateTime.Now.ToString();
                    timeStamp = timeStamp.Replace("/", "");
                    timeStamp = timeStamp.Replace(":", "");
                    timeStamp = timeStamp.Replace(" ", "");

                    img.Save(screenshotsFolder + "\\" + filename + "_" + timeStamp + ".jpg");

                    screenshotSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewGeneralTXTlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewGeneralTXTlogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewGeneralTXTlogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailGeneralTXTLogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewBPTReportslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewBPTReportslogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewBPTReportslogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailBPTReportslogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewEnvironmentCapabilitieslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewEnvironmentCapabilitieslogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewEnvironmentCapabilitieslogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailEnvironmentCapabilitieslogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewEnvironmentslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewEnvironmentslogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewEnvironmentslogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailEnvironmentslogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewFullErrorDumps_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewFullErrorDumps.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewFullErrorDumps.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailFullErrorDumpslogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewRoleslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewRoleslogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewRoleslogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailRoleslogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewRolesInApplicationslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewRolesInApplicationslogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewRolesInApplicationslogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailRolesInApplicationslogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewRolesInTeamslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewRolesInTeamslogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewRolesInTeamslogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailRolesInTeamslogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewSyncErrorslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewSyncErrorslogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewSyncErrorslogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailSyncErrorslogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewUserlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewUserlogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewUserlogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailUserlogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewUserPoolslogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewUserPoolslogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewUserPoolslogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailUserPoolslogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingApplogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingApplogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingApplogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingApplogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingAppVerlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingAppVerlogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingAppVerlogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingAppVerlogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingAppVerModuleVerlogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingAppVerModuleVerlogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingAppVerModuleVerlogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingAppVerModuleVerlogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingChangelog_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingChangelog.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingChangelog.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingChangelog.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingConsumerElements_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingConsumerElements.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingConsumerElements.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingConsumerElementslogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingEntityConfiguration_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingEntityConfiguration.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingEntityConfiguration.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingEntityConfiguration.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingEnvironmentAppicationCache_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingEnvironmentAppicationCache.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingEnvironmentAppicationCache.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingEnvironmentApplicationCache.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingEnvironmentApplicationModule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingEnvironmentApplicationModule.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingEnvironmentApplicationModule.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingEnvironmentApplicationModule.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingEnvironmentApplicationVersion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingEnvironmentApplicationVersion.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingEnvironmentApplicationVersion.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingEnviromentApplicationVersion.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingEnvironmentModuleCache_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingEnvironmentModuleCache.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingEnvironmentModuleCache.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingEnvironmentModuleCache.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingEnvironmentModuleRunning_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingEnvironmentModuleRunning.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingEnvironmentModuleRunning.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingEnvironmentModuleRunning.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingModules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingModules.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingModules.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingModules.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingModuleVersionRefererences_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingModuleVersionRefererences.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingModuleVersionRefererences.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingModuleVersionReferences.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingProducerElements_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingProducerElements.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingProducerElements.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingProducerElements.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingSiteProperties_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingSiteProperties.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingSiteProperties.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingSiteProperties.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStaginglogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStaginglogs.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStaginglogs.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStaginglogs.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingApplicationVersion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingApplicationVersion.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingApplicationVersion.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingApplicationVersion.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingMessage_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingMessage.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingMessage.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingMessage.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingModuleInconsistencies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingModuleInconsistencies.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingModuleInconsistencies.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingModuleInconsistencies.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingModuleVersion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingModuleVersion.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingModuleVersion.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingModuleVersion.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingModuleVersionPublished_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingModuleVersionPublished.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingModuleVersionPublished.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingModuleVersionPublished.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingModuleVersionUploaded_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingModuleVersionUploaded.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingModuleVersionUploaded.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingModuleVersionUploaded.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingOptions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingOptions.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingOptions.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingOptions.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingOutdatedApplication_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingOutdatedApplication.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingOutdatedApplication.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingOutdatedApplication.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void dataGridViewStagingOutdatedModule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    List<string> rowInfo = new List<string>();

                    //gets a collection that contains all the rows
                    DataGridViewRow row = this.dataGridViewStagingOutdatedModule.Rows[e.RowIndex];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value.ToString() != null && cell.Value.ToString().Trim() != string.Empty)
                        {
                            rowInfo.Add(dataGridViewStagingOutdatedModule.Columns[cell.ColumnIndex].Name + Environment.NewLine + cell.Value.ToString() + Environment.NewLine);
                        }
                    }

                    txtBoxDetailStagingOutdatedModule.Text = String.Join(Environment.NewLine, rowInfo);

                    if (btnScreenshot.Enabled == false)
                    {
                        btnScreenshot.Enabled = true;
                        btnScreenshot.BackColor = SystemColors.ControlLight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void btnSearchKeyword_Click(object sender, EventArgs e)
        {
            findKeyword = true;

            if (string.IsNullOrEmpty(txtBoxKeyword.Text))
            {
                MessageBox.Show("Please enter a keyword to search for", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtBoxKeyword.BackColor = Color.Orange;
                txtBoxKeyword.Focus();
            }
            else
            {
                highlightKeyword(dataGridViewErrorlogs);
                highlightKeyword(dataGridViewGenerallogs);
                highlightKeyword(dataGridViewIntegrationslogs);
                highlightKeyword(dataGridViewScreenRequestslogs);
                highlightKeyword(dataGridViewTimerlogs);
                highlightKeyword(dataGridViewEmaillogs);
                highlightKeyword(dataGridViewExtensionlogs);
                highlightKeyword(dataGridViewServiceActionlogs);
                highlightKeyword(dataGridViewTradWebRequests);
                highlightKeyword(dataGridViewIISDateTime);
                highlightKeyword(dataGridViewIISTimeTaken);
                highlightKeyword(dataGridViewWinAppEventViewer);
                highlightKeyword(dataGridViewWinSysEventViewer);
                highlightKeyword(dataGridViewWinSecEventViewer);
                highlightKeyword(dataGridViewAndroidlogs);
                highlightKeyword(dataGridViewiOSlogs);
                highlightKeyword(dataGridViewServiceStudiologs);
                highlightKeyword(dataGridViewGeneralTXTlogs);
                highlightKeyword(dataGridViewBPTReportslogs);
                highlightKeyword(dataGridViewEnvironmentCapabilitieslogs);
                highlightKeyword(dataGridViewEnvironmentslogs);
                highlightKeyword(dataGridViewFullErrorDumps);
                highlightKeyword(dataGridViewRoleslogs);
                highlightKeyword(dataGridViewRolesInApplicationslogs);
                highlightKeyword(dataGridViewRolesInTeamslogs);
                highlightKeyword(dataGridViewUserlogs);
                highlightKeyword(dataGridViewUserPoolslogs);
                highlightKeyword(dataGridViewSyncErrorslogs);
                highlightKeyword(dataGridViewStagingApplogs);
                highlightKeyword(dataGridViewStagingAppVerlogs);
                highlightKeyword(dataGridViewStagingAppVerModuleVerlogs);
                highlightKeyword(dataGridViewStagingChangelog);
                highlightKeyword(dataGridViewStagingConsumerElements);
                highlightKeyword(dataGridViewStagingEntityConfiguration);
                highlightKeyword(dataGridViewStagingEnvironmentAppicationCache);
                highlightKeyword(dataGridViewStagingEnvironmentApplicationModule);
                highlightKeyword(dataGridViewStagingEnvironmentApplicationVersion);
                highlightKeyword(dataGridViewStagingEnvironmentModuleCache);
                highlightKeyword(dataGridViewStagingEnvironmentModuleRunning);
                highlightKeyword(dataGridViewStagingModules);
                highlightKeyword(dataGridViewStagingModuleVersionRefererences);
                highlightKeyword(dataGridViewStagingProducerElements);
                highlightKeyword(dataGridViewStagingSiteProperties);
                highlightKeyword(dataGridViewStaginglogs);
                highlightKeyword(dataGridViewStagingApplicationVersion);
                highlightKeyword(dataGridViewStagingMessage);
                highlightKeyword(dataGridViewStagingModuleInconsistencies);
                highlightKeyword(dataGridViewStagingModuleVersion);
                highlightKeyword(dataGridViewStagingModuleVersionPublished);
                highlightKeyword(dataGridViewStagingModuleVersionUploaded);
                highlightKeyword(dataGridViewStagingOptions);
                highlightKeyword(dataGridViewStagingOutdatedApplication);
                highlightKeyword(dataGridViewStagingOutdatedModule);

                if (removeGarbage)
                {
                    removeGenericErrors(dataGridViewErrorlogs, 1, txtBoxDetailErrorLogs);
                    //removeGenericErrors(dataGridViewGenerallogs, 1, txtBoxDetailGenerallogs);
                    //removeGenericErrors2(dataGridViewIntegrationslogs, txtBoxDetailIntegrationlogs);
                    //removeGenericErrors2(dataGridViewScreenRequestslogs, txtBoxDetailScreenRequestslogs);
                    //removeGenericErrors2(dataGridViewTimerlogs, txtBoxDetailTimerlogs);
                    //removeGenericErrors2(dataGridViewEmaillogs, txtBoxDetailEmaillogs);
                    //removeGenericErrors2(dataGridViewExtensionlogs, txtBoxDetailExtensionlogs);
                    //removeGenericErrors2(dataGridViewServiceActionlogs, txtBoxDetailServiceActionlogs);
                    //removeGenericErrors2(dataGridViewTradWebRequests, txtBoxDetailTradWebRequests);
                    //removeGenericErrors2(dataGridViewIISDateTime, txtDetailIISlogs);
                    //removeGenericErrors2(dataGridViewIISTimeTaken, txtDetailIISlogs);
                    removeGenericErrors(dataGridViewWinAppEventViewer, 2, txtBoxDetailWinAppEventViewer);
                    removeGenericErrors(dataGridViewWinSysEventViewer, 2, txtBoxDetailWinSysEventViewer);
                    removeGenericErrors(dataGridViewWinSecEventViewer, 2, txtBoxDetailWinSecEventViewer);
                    removeGenericErrors(dataGridViewAndroidlogs, 3, txtBoxDetailAndroidLogs);
                    removeGenericErrors(dataGridViewiOSlogs, 3, txtBoxDetailiOSLogs);
                    removeGenericErrors(dataGridViewServiceStudiologs, 3, txtBoxDetailServiceStudioLogs);
                    removeGenericErrors(dataGridViewGeneralTXTlogs, 2, txtBoxDetailGeneralTXTLogs);
                    //removeGenericErrors(dataGridViewBPTReportslogs, txtBoxDetailBPTReportslogs);
                }

                if (highlightError)
                {
                    string[] knownErrors_Errorlogs = { "url rewrite module error", "an error occurred in task", "server cannot modify cookies", "ping validation failed", "a fatal error has occurred", "communicationexception", "json deserialization", "compilation error", "file is corrupt or invalid", "checksum failed for file", "connection failed421", "temporary server error", "can't proceed", "truncated in table", "unknown reference expression type email" };
                    string[] knownErrors_Generallogs = { "system cannot find" };
                    string[] knownErrors_WinAppEventViewer = { "ora-", "error closing", "cannot access", "error opening" };
                    string[] knownErrors_WinSysEventViewer = { "error closing", "timed out" };
                    string[] knownErrors_AndroidiOSlogs = { "file is corrupt or invalid", "androidx library", "command finished with error code 0", "plugin is not going to work", "error: spawnsync sudo etimeout", "plugin doesn't support this project's cordova-android version", "failed to fetch plug", "archive failed", "build failed with the following error", "command failed with exit code", "signing certificate is invalid", "the ios deployment target", "verification failed" };
                    string[] knownErrors_ServiceStudiologs = { "oneoftypedefinition", "http forbidden", "[not connected]" };

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
                    //highlightKnownErrors(dataGridViewGeneralTXTlogs, 2);
                    //highlightKnownErrors(dataGridViewBPTReportslogs);
                }

                if (findKeywordSuccessful)
                {
                    MessageBox.Show("The keyword was found", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                btnSearchKeyword.Enabled = false;
                txtBoxKeyword.Enabled = false;
            }
        }

        private void highlightKeyword(DataGridView tableName)
        {
            try
            {
                if (tableName.Rows.Count > 0)
                {
                    //search for the value anywhere on the table, meaning, any row and any column
                    string cellValue = "";

                    for (int r = 0; r <= tableName.Rows.Count - 1; r++)
                    {
                        for (int c = 0; c <= tableName.Columns.Count - 1; c++)
                        {
                            cellValue = tableName.Rows[r].Cells[c].Value.ToString();
                            if (c != tableName.Columns.Count - 1)
                            {
                                if (cellValue.ToLower().Contains(txtBoxKeyword.Text.ToLower()))
                                {
                                    tableName.Rows[r].DefaultCellStyle.ForeColor = Color.Red;

                                    findKeywordSuccessful = true;
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
    }
}

