import os
import re
import sys
import codecs
import shutil
import openpyxl
from lxml import etree
import Evtx.Evtx as evtx
from collections import Counter
from datetime import datetime, timedelta

import scripts.integrations
import scripts.general
import scripts.timer
import scripts.screen
import scripts.mobilerequests
"""
OutSystems Log Parser

Service Center Logs:
Error logs:
DATE_TIME MESSAGE STACK MODULE_NAME APPLICATION_NAME APPLICATION_KEY ACTION_NAME ENTRYPOINT_NAME SERVER ESPACE_NAME ESPACE_ID USER_ID SESSION_ID ENVIRONMENT_INFORMATION ID TENANT_ID

Device Information:
OPERATING_SYSTEM OPERATING_SYSTEM_VERSION COUNT DEVICE_MODEL CORDOVA_VERSION DEVICE_UUID

General logs:
DATE_TIME MESSAGE MESSAGE_TYPE MODULE_NAME APPLICATION_NAME APPLICATION_KEY ACTION_NAME ENTRYPOINT_NAME CLIENT_IP ESPACE_NAME ESPACE_ID USER_ID SESSION_ID ERROR_ID REQUEST_KEY TENANT_ID

General logs (Slow Extensions/Slow SQLs):
DATE_TIME DURATION_SECONDS MODULE_NAME APPLICATION_NAME ACTION_NAME ESPACE_NAME MESSAGE STACK ENVIRONMENT_INFORMATION ERROR_ID

Integration logs:
DATE_TIME DURATION_SECONDS APPLICATION_NAME APPLICATION_KEY ACTION_NAME ACTION_TYPE SOURCE ENDPOINT EXECUTED_BY ESPACE_NAME ESPACE_ID ERROR_ID REQUEST_KEY TENANT_ID

Integrations logs (Web Services):
DATE_TIME DURATION_SECONDS MODULE_NAME APPLICATION_NAME ACTION_NAME ACTION_TYPE ESPACE_NAME MESSAGE STACK ENVIRONMENT_INFORMATION ERROR_ID

MobileRequestsLog = ScreenRequests
Screen Requests logs:
DATE_TIME DURATION SCREEN APPLICATION_NAME APPLICATION_KEY SOURCE ENDPOINT EXECUTED_BY ESPACE_NAME ESPACE_ID LOGIN_ID USER_ID CYCLE ERROR_ID REQUEST_KEY TENANT_ID

Screen Requests logs (Screens):
DATE_TIME DURATION_SECONDS SCREEN MODULE_NAME APPLICATION_NAME ESPACE_NAME MESSAGE STACK ENVIRONMENT_INFORMATION ERROR_ID

Timer logs:
DATE_TIME DURATION APPLICATION_NAME APPLICATION_KEY EXECUTED_BY ESPACE_NAME ESPACE_ID CYCLIC_JOB_NAME CYCLIC_JOB_KEY SHOULD_HAVE_RUN_AT NEXT_RUN ERROR_ID REQUEST_KEY TENANT_ID

Timer logs (Timers):
DATE_TIME DURATION_SECONDS CYCLIC_JOB_NAME MODULE_NAME APPLICATION_NAME ESPACE_NAME MESSAGE STACK ENVIRONMENT_INFORMATION ERROR_ID

Email logs:
DATE_TIME SENT ERROR_ID FROM TO SUBJECT CC BCC ESPACE_NAME SIZE MESSAGE_ID ACTIVITY EMAIL_DEFINITION STORE_CONTENT IS_TEST_EMAIL ID TENANT_ID

Email logs (Emails):
DATE_TIME DURATION_SECONDS MODULE_NAME APPLICATION_NAME ESPACE_NAME MESSAGE STACK FROM TO SUBJECT CC BCC IS_TEST_EMAIL ENVIRONMENT_INFORMATION ERROR_ID

Extension logs:
DATE_TIME DURATION APPLICATION_NAME APPLICATION_KEY ACTION_NAME EXECUTED_BY ESPACE_NAME ESPACE_ID USERNAME USER_ID SESSION_ID EXTENSION_ID EXTENSION_NAME ERROR_ID REQUEST_KEY TENANT_ID

Extension logs (Extensions):
DATE_TIME DURATION_SECONDS EXTENSION_NAME MODULE_NAME APPLICATION_NAME ACTION_NAME ESPACE_NAME MESSAGE STACK ENVIRONMENT_INFORMATION ERROR_ID

Service Action logs:
DATE_TIME DURATION APPLICATION_NAME APPLICATION_KEY ACTION_NAME SOURCE ENTRYPOINT_NAME ENDPOINT EXECUTED_BY ESPACE_NAME ESPACE_ID USERNAME LOGIN_ID USER_ID SESSION_ID ERROR_ID REQUEST_KEY ORIGINAL_REQUEST_KEY TENANT_ID

Service Action logs (Service Actions):
DATE_TIME DURATION_SECONDS ACTION_NAME MODULE_NAME APPLICATION_NAME ESPACE_NAME MESSAGE STACK ENVIRONMENT_INFORMATION ERROR_ID

TraditionalWebRequests = Screen Logs              
Screen logs:
DATE_TIME DURATION SCREEN SCREEN_TYPE APPLICATION_NAME APPLICATION_KEY ACTION_NAME ACCESS_MODE EXECUTED_BY CLIENT_IP ESPACE_NAME ESPACE_ID USER_ID SESSION_ID SESSION_REQUESTS SESSION_BYTES VIEW_STATE_BYTES MS_IS_DN REQUEST_KEY TENANT_ID

Screen Logs (Screens):
DATE_TIME DURATION_SECONDS SCREEN SCREEN_TYPE APPLICATION_NAME ACTION_NAME ESPACE_NAME

BPTTroubleshootingReports:
DATE_TIME ESPACE_NAME PROCESS_NAME PROCESS_STATUS PROCESS_LAST_MODIFIED PROCESS_SUSPENDED_DATE PROCESS_ID PARENT_PROCESS_ID ACTIVITY_CREATED ACTIVITY_NAME ACTIVITY_KIND ACTIVITY_STATUS ACTIVITY_RUNNING_SINCE ACTIVITY_NEXT_RUN ACTIVITY_CLOSED ACTIVITY_ERROR_COUNT ACTIVITY_ERROR_ID

IIS logs:
DATE_TIME TIME_TAKEN_SECONDS HTTP_CODE HTTP_SUBCODE WINDOWS_ERROR_CODE CLIENT_IP SERVER_IP SERVER_PORT
METHOD URI_STEM URI_QUERY USERNAME BROWSER REFERRER

TIME_TAKEN_SECONDS DATE_TIME HTTP_CODE HTTP_SUBCODE WINDOWS_ERROR_CODE CLIENT_IP SERVER_IP SERVER_PORT
METHOD URI_STEM URI_QUERY USERNAME BROWSER REFERRER

*******   *******   *******
NOTE: The IIS logs with filenames ending in "_x" mean the customer selected the columns to display data from and rearranged
the columns at his own will.

#Fields: date time s-sitename s-computername s-ip cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs-version cs(User-Agent) cs(Referer)
sc-status sc-substatus sc-win32-status sc-bytes cs-bytes time-taken x-forwarded-for

^([\d\-]+)[ ]([\d\:]+)(?:[ ])?([\w]+)?(?:[ ])?([\w]+)?[ ]([\d\.\:]+)[ ](POST|PUT|PROPFIND|(?:n)?GET|OPTIONS|HEAD|ABCD|QUALYS|TRACE|SEARCH|RNDMMTD|TRACK|B(?:A)?DM(?:E)?T(?:H)?(?:O)?(?:D)?|CFYZ|DEBUG|MKCOL|INDEX|DELETE|PATCH|ACUNETIX)[ ]([\w\(\)\[\]\{\}\-\–\:\;\‘\’\'\"\“\”\,\.\>\`\~\<\&\=\\\/\?\+\$\@\%\#\*\!\»\£\€]+)[ ]([\w\(\)\[\]\{\}\-\–\:\;\‘\’\'\"\“\”\,\.\>\`\~\<\&\=\\\/\?\+\$\@\%\#\*\!\»\£\€]+)[ ]([\d]+)[ ]([\w\(\)\[\]\{\}\-\–\:\;\‘\’\'\"\“\”\,\.\>\`\~\<\&\=\\\/\?\+\$\@\%\#\*\!\»\£\€]+)[ ]([\d\.\:]+)(?:[ ])?([\w\/\.]+)?[ ]([\w\(\)\[\]\{\}\-\–\:\;\‘\’\'\"\“\”\,\.\>\`\~\<\&\=\\\/\?\+\$\@\%\#\*\!\»\£\€]+)[ ]([\w\(\)\[\]\{\}\-\–\:\;\‘\’\'\"\“\”\,\.\>\`\~\<\&\=\\\/\?\+\$\@\%\#\*\!\»\£\€]+)[ ]([\d]+)[ ]([\d]+)[ ]([\d]+)(?:[ ])?([\d]+)?(?:[ ])?([\d]+)?[ ]([\d]+)(?:[ ])?([\w\(\)\[\]\{\}\-\–\:\;\‘\’\'\"\“\”\,\.\>\`\~\<\&\=\\\/\?\+\$\@\%\#\*\!\»\£\€]+)?

DATE_TIME TIME_TAKEN HTTP_CODE HTTP_SUBCODE HTTP_VERSION WINDOWS_ERROR_CODE CLIENT_IP ACTUAL_CLIENT_IP SERVER_IP
SERVER_PORT SERVER_NAME SERVICE_NAME METHOD URI_STEM URI_QUERY USERNAME BROWSER REFERRER BYTES_SENT BYTES_RECEIVED
*******   *******   *******

Android and iOS:
DATE_TIME MESSAGE_TYPE ACTION_NAME MESSAGE

Service Studio Reports:
DATE_TIME MESSAGE_TYPE MESSAGE

General Text Logs:
DATE_TIME MESSAGE_TYPE MESSAGE

Windows Event Viewer logs:
DATE_TIME LEVEL MESSAGE TASK COMPUTER SOURCE QUALIFIERS EVENT_ID EVENT_RECORD_ID KEYWORDS

Full Error Dump logs:
PLATFORM_INFORMATION

Environment Capabilities logs:
ID ENVIRONMENT CAPABILITY

Environments logs:
IS_LIFE_TIME IS_REGISTERED ID NAME DESCRIPTION HOST PUBLIC_HOST TYPE PRIMARY_CONTACT ORDER NUMBER_OF_USERS IS_ACTIVE IS_OFFLINE LAST_SET_OFFLINE CREATED_BY CREATED_ON USE_HTTPS VERSION LAST_UPGRADE_VERSION UID CALLBACK_ADDRESS NUMBER_OF_FRONTEND_SERVERS FIRST_SYNC_FINISHED CLOUD_PROVIDER_TYPE ENVIRONMENT_STACK ADDITIONAL_INFO LAST_CACHE_INVALIDATION ENVIRONMENT_DB_PROVIDER ENVIRONMENT_SERVER_KIND TWO_STEP_PUBLISH_MODE LAST_ENV_SYNC_SEQUENTIAL_NUMBER MONITORING_ENABLED DATA_HARVESTED_TO IS_IN_OUTSYSTEMS_CLOUD BEST_HASHING_ALGORITHM ALLOW_NATIVE_BUILDS

Roles logs:
ID NAME DESCRIPTION ORDER CAN_CONFIGURE_INFRASTRUCTURE CAN_CONFIGURE_ROLES CAN_CONFIGURE_USERS CAN_CONFIGURE_APPLICATION_ROLES KEY ID_2 LABEL_OLD SHORT_LABEL_OLD DESCRIPTION_OLD LABEL SHORT_LABEL DESCRIPTION_2 LT_LEVEL SC_LEVEL APPLICATION_LEVEL IS_OLD_LEVEL ID_3 LABEL_2 SHORT_LABEL_2 DESCRIPTION_3 LEVEL IS_COMPUTED ID_4 ROLE ENVIRONMENT DEFAULT_PERMISSION_LEVEL CAN_CREATE_APPLICATIONS CAN_REFERENCE_SYSTEMS IS_INITIALIZED

Roles In Applications logs:
ID USER ROLE APPLICATION

Roles In Teams logs:
ID ROLE KEY TEAM NAME KEY_2

Sync Errors logs:
DATE_TIME SESSION_ID MESSAGE STACK MODULE SERVER USERNAME ENDPOINT ACTION PROCESS_NAME ACTIVITY_NAME

User logs:
ID NAME USERNAME EXTERNAL CREATION_DATE	LAST_LOGIN IS_ACTIVE ID_2 NAME_2 DESCRIPTION ORDER CAN_CONFIGURE_INFRASTRUCTURE CAN_CONFIGURE_ROLES CAN_CONFIGURE_USERS CAN_CONFIGURE_APPLICATION_ROLES KEY USER_ROLE KEY_2 MTSI_IDENTIFIER

User Pools logs:
USER POOL_KEY

Application logs:
ID TEAM	NAME DESCRIPTION PRIMARY_CONTACT URL_PATH KEY LOGO_HASH	IS_ACTIVE DEFAULT_THEME_IS_MOBILE MONITORING_ENABLED APPLICATION_KIND

Application Version logs:
ID APPLICATION VERSION CHANGE_LOG CREATED_ON CREATED_BY	CREATED_ON_ENVIRONMENT FRONT_OFFICE_ESPACE_KEY FRONT_OFFICE_ESPACE_NAME BACK_OFFICE_ESPACE_KEY BACK_OFFICE_ESPACE_NAME WEB_THEME_GLOBAL_KEY MOBILE_THEME_GLOBAL_KEY WAS_AUTO_TAGGED VERSION_DECIMAL TEMPLATE_KEY PRIMARY_COLOR NATIVE_HASH KEY

Application Version Module Version logs:
ID APPLICATION_VERSION MODULE_VERSION ID_2 MODULE HASH GENERAL_HASH CREATED_ON CREATED_BY CREATED_ON_ENVIRONMENT LAST_UPGRADE_VERSION DIRECT_UPGRADE_FROM_VERSION_HASH COMPATIBILITY_SIGNATURE_HASH KEY

ChangeLog logs:
ID LABEL ID_2 DATE_TIME MESSAGE FIRST_OBJECT_TYPE FIRST_OBJECT SECOND_OBJECT_TYPE SECOND_OBJECT IS_WRITE IS_SUCCESSFUL IS_SYSTEM ENTRY_ESPACE USER CLIENT_IP

Consumer Elements logs:
CONSUMER_MODULE CONSUMER_MODULE_NAME CONSUMER_MODULE_VERSION CONSUMER_ELEMENT_VERSION CONSUMER_ELEMENT_VERSION_KEY CONSUMER_ELEMENT_VERSION_TYPE CONSUMER_ELEMENT_VERSION_NAME CONSUMER_ELEMENT_VERSION_COMPATIBILITY_HASH PRODUCER_MODULE PRODUCER_MODULE_KEY PRODUCER_MODULE_NAME PRODUCER_MODULE_TYPE CREATED_ON_PRODUCER_MODULE_VERSION

Entity Configurations logs:
ID ENTITY_KEY MODULE_VERSION_ID CREATED_ON UPDATED_ON ID_2 MODULE_ID STAGING_ID CREATED_ON_2 CREATED_BY UPDATED_ON_2 UPDATED_BY ENTITY_KEY_2 PHYSICAL_TABLE_NAME IS_OVERRIDEN_TABLE_NAME DEFAULT_PHYSICAL_TABLE_NAME ENTITY_NAME SOURCE_PHYSICAL_TABLE_NAME TARGET_PHYSICAL_TABLE_NAME

Environment Application Cache logs:
ID ENV_APPLICATION_CACHE_ID MOBILE_PLATFORM BINARY_AVAILABLE CONFIG_AVAILABLE VERSION_NUMBER VERSION_CODE VERSION_CHANGED CONFIGURATION_CHANGED TAGGED_MABS_VERSION LAST_BUILD_MABS_VERSION LOCKED_MABS_VERSION ID_2 ENVIRONMENT APPLICATION VERSION VERSION_CHANGED_2 NUMBER_OF_USERS CHANGE_STATUS CHANGE_STATUS_MESSAGE SERVICE_CENTER_STATUS SERVICE_CENTER_STATUS_MESSAGE LAST_PUBLISHED LAST_PUBLISHED_BY DELETED CONSISTENCY_STATUS CONSISTENCY_STATUS_MESSAGES FRONT_OFFICE_ESPACE_KEY FRONT_OFFICE_ESPACE_NAME BACK_OFFICE_ESPACE_KEY BACK_OFFICE_ESPACE_NAME WEB_THEME_GLOBAL_KEY MOBILE_THEME_GLOBAL_KEY IS_OUTDATED DEVELOPMENT_EFFORT TEMPLATE_KEY PRIMARY_COLOR NATIVE_HASH ENV_DEPLOYMENT_ZONES IS_IN_MULTIPLE_DEPLOYMENT_ZONES IS_PWA_ENABLED

Environment Application Module logs:
ID APPLICATION	MODULE	ENVIRONMENT LAST_CHANGED_ON

Environment App Version logs:
ID ENVIRONMENT APPLICATION_VERSION

Environment Module Cache logs:
ID ENVIRONMENT MODULE_VERSION INTERNAL_VERSION PUBLISHED_ON PUBLISHED_BY ID_2 ENVIRONMENT_2 MODULE PUBLISH CHANGE_STATUS CHANGE_STATUS_MESSAGE DELETED	CONSISTENCY_STATUS CONSISTENCY_STATUS_MESSAGES IS_OUTDATED

Environment Module Running logs:
ID ENVIRONMENT CONSUMER_MODULE PRODUCER_MODULE_KEY PRODUCER_COMPATIBILITY_HASH IS_WEAK_REFERENCE

Module logs:
ID LABEL TOKEN ID_2 NAME DESCRIPTION KEY TYPE

Module Version References logs:
ID MODULE_VERSION_REFERENCE PRODUCER_MODULE_VERSION IS_COMPATIBLE IS_IN_DIFFERENT_LUV PLATFORM_VERSION ID_2 MODULE_VERSION PRODUCER_MODULE IS_WEAK_REFERENCE ID_3 MODULE_VERSION_REFERENCE_STATUS ELEMENT_NAME ELEMENT_KEY ELEMENT_TYPE ELEMENT_REF_INCONSISTENCY_TYPE_I

Producer Elements logs:
MODULE MODULE_NAME MODULE_VERSION ELEMENT_VERSION ELEMENT_VERSION_KEY ELEMENT_VERSION_TYPE ELEMENT_VERSION_NAME ELEMENT_VERSION_COMPATIBILITY_HASH

Site Properties logs:
ID SS_KEY MODULE_VERSION_ID NAME DATA_TYPE_ID DESCRIPTION DEFAULT_VALUE IS_MULTI_TENANT CREATED_ON UPDATED_ON ID_2 LABEL ORDER IS_ACTIVE ID_3 EFFECTIVE_VALUE_CHANGED_IN_STAGING EFFECTIVE_VALUE_ON_TARGET SITE_PROPERTY_SS_KEY IS_MULTI_TENANT_2 MODULE_ID STAGING_ID IS_NEW CREATED_ON_2 CREATED_BY UPDATED_ON_2 UPDATED_BY

Staging logs:
ID SOURCE_ENVIRONMENT TARGET_ENVIRONMENT LABEL INTERNAL CREATED_BY CREATED_ON STARTED_BY STARTED_ON FINISHED_ON IS_DRAFT SOLUTION_PUBLISHED_FINISHED IS_WAITING_FOR_CONFIRMATION_PROCESS SAVED_BY SAVED_ON LAST_REFRESHED_ON SYNC_FINISHED_ON SOURCE_STAGING STAGING_CONFIRMATION_KIND TWO_STEP_MODE LAST_RESUME_DATE_TIME MARKED_FOR_ABORT_ON ABORTED_BY KEY STATUS

Staging Application Version logs:
ID KEY NAME IS_DEFAULT DEPLOYMENT_TECH ENVIRONMENT IS_ACTIVE ID_2 STAGING_APPLICATION_VERSION MOBILE_PLATFORM SOURCE_BINARY_AVAILABLE SOURCE_CONFIG_AVAILABLE SOURCE_VERSION_NUMBER SOURCE_VERSION_CODE TARGET_BINARY_AVAILABLE TARGET_CONFIG_AVAILABLE TARGET_VERSION_NUMBER TARGET_VERSION_CODE VERSION_CHANGED VERSION_AFTER_FINISH_PUBLISH MABS_VERSION_AFTER_PUBLISH ID_3 STAGING APPLICATION APPLICATION_VERSION VERSION_CHANGED_2 APPLICATION_DELETED LAST_PUBLISHED LAST_PUBLISHED_BY FRONT_OFFICE_ESPACE_KEY FRONT_OFFICE_ESPACE_NAME BACK_OFFICE_ESPACE_KEY BACK_OFFICE_ESPACE_NAME WEB_THEME_GLOBAL_KEY MOBILE_THEME_GLOBAL_KEY	TEMPLATE_KEY PRIMARY_COLOR SOURCE_APPLICATION_VERSION SOURCE_VERSION_CHANGED PREVIOUS_APPLICATION_VERSION PREVIOUS_VERSION_CHANGED PREVIOUS_APPLICATION_DELETED PREVIOUS_LAST_PUBLISHED PREVIOUS_LAST_PUBLISHED_BY PREVIOUS_FRONT_OFFICE_ESPACE_KEY PREVIOUS_FRONT_OFFICE_ESPACE_NAME PREVIOUS_BACK_OFFICE_ESPACE_KEY PREVIOUS_BACK_OFFICE_ESPACE_NAME PREVIOUS_WEB_THEME_GLOBAL_KEY PREVIOUS_MOBILE_THEME_GLOBAL_KEY PREVIOUS_TEMPLATE_KEY PREVIOUS_PRIMARY_COLOR OPERATION OPERATION_LABEL OPERATION_MESSAGE OPERATION_IS_DEPLOY OPERATION_IS_FORCE_DEPLOY VERSION_WHEN_STARTING_DEPLOY VERSION_AFTER_FINISHING_DEPLOY TARGET_ENV_CONSISTENCY_STATUS TARGET_ENV_CONSISTENCY_MSG ORDER_IN_SUMMARY_TABLE NEW_TAG_VERSION NEW_TAG_DESCRIPTION STAGING_OPTION PENDING_VALIDATION SOURCE_NATIVE_HASH TARGET_NATIVE_HASH IS_VISIBLE_IN_STAGING HAS_NEW_SITE_PROP ID_4 ENV_DEPLOYMENT_ZONES ENV_DATABASE_CONFIG IS_PWA_ENABLED_ON_SOURCE IS_AUTO_UPGRADE_DISABLED

Staging Message logs:
ID STAGING MESSAGE DETAIL EXTRA_INFO INTERNAL_ID INTERNAL_TYPE TYPE DATE_TIME

Staging Module Inconsistency logs:
ID STAGING CONSUMER_MODULE PRODUCER_MODULE FIRST_REQUIRED_ELEMENT FIRST_REQUIRED_ELEMENT_TYPE TOTAL_REQUIRED_ELEMENTS PRODUCER_MODULE_NAME CONSUMER_MODULE_NAME INCONSISTENCY_TYPE

Staging Module Version logs:
ID STAGING APPLICATION PREVIOUS_APPLICATION MODULE MODULE_VERSION MODULE_DELETED PREVIOUS_MODULE_VERSION PREVIOUS_MODULE_DELETED OPERATION

Staging Module Version To Publish logs:
ID STAGING PLANNED_MODULE_VERSION_HASH MODULE_VERSION_HASH_TO_PUBLISH

Staging Module Version To Upload logs:
ID STAGING USER ENVIRONMENT_ID_1 ENVIRONMENT_ID_2 TYPE NAME MODULE_KEY VERSION_KEY DIRECT_UPGRADE_FROM_VERSION_KEY APPLICATION_KEY

Staging Option logs:
ID STAGING APPLICATION STAGING_OPTION_TYPE APPLICATION_VERSION APPLICATION_VERSION_LABEL LABEL APPLICATION_DESCRIPTION IS_TOP_OPTION iOS_VERSION_LABEL ANDROID_VERSION_LABEL MOBILE_APPS_DESCRIPTION

Staging Outdated Application logs:
ID STAGING APPLICATION

Staging Outdated Module logs:
ID STAGING MODULE
-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

1) Filter the data based on a date range.
2) Read the input files and rearrange the columns.
3) Filtered outputs should be already sorted by their timestamp (ascending order).
4) Filtered outputs should be TXT files using the pipe ("|") as the delimiter and their filenames should be the logs they represent.
5) All the timestamp fields from the logs should be in the following format: YYYY-MM-DD hh:mm:ss

-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

Convert illegal character to decimal value: https://www.branah.com/unicode-converter
Confirm character based on decimal value: https://www.charset.org/utf-8
"""

myLinesFromDateRange = []
myFinalLinesFromDateRange = []
myNonMatchedValidLinesFromDateRange = []
myTempXLSXlines = []
myLOGlines = []
myTimeTaken = []
iisLineGraphList = []
myFileExt = []
myDateTimes = []
myTimesTaken = []
androidOSVersionList = []
myAndroidOSVersionList = []
androidOSVersionOccurrencesList = []
myandroidOSVersionOccurrencesList = []
iosOSVersionList = []
myiOSVersionList = []
iOSOSVersionOccurrencesList = []
myiOSOSVersionOccurrencesList = []
deviceInformationList = []
myDeviceInformationList = []
errorLogsList = []
generalLogsList = []
integrationsLogsList = []
slowSQLList = []
slowSQLList2 = []
slowSQLErrorsList = []
mySlowSQLErrorsList = []
slowExtensionList = []
slowExtensionList2= []
slowExtensionErrorsList = []
mySlowExtensionErrorsList = []
errorsList = []
errorsList2 = []
webServicesList = []
webServicesList2 = []
webServicesErrorsList = []
myWebServicesErrorsList = []
timerLogsList = []
timersList = []
timersList2 = []
timersErrorsList = []
myTimersErrorsList = []
emailLogsList = []
emailsList = []
emailsList2 = []
emailsErrorsList = []
myEmailErrorsList = []
mobileRequestsLogsList = []
mobileRequestsScreenList = []
mobileRequestsScreenList2 = []
mobileRequestsScreenErrorsList = []
myMobileRequestsScreenErrorsList = []
extensionLogsList = []
extensionList = []
extensionList2 = []
extensionErrorsList = []
myExtensionErrorsList = []
serviceActionLogsList = []
serviceActionList = []
serviceActionList2 = []
serviceActionErrorsList = []
myServiceActionErrorsList = []
screenLogsList = []
screensList = []
myScreensList = []
moduleNamesList = []
myModuleNamesList = []
applicationNamesList = []
myApplicationNamesList = []
actionNamesList = []
myActionNamesList = []
extensionNamesList = []
myExtensionNamesList = []
espaceNamesList = []
myEspaceNamesList = []
cyclicJobNamesList = []
myCyclicJobNamesList = []

#all illegal space characters, control characters, and ASCII characters
replacementDict = {}
replacementDict.update(dict.fromkeys(range(4, 7), ""))
replacementDict.update(dict.fromkeys(range(135, 136), ""))
replacementDict.update(dict.fromkeys(range(166, 169), ""))
replacementDict.update(dict.fromkeys(range(170, 172), ""))
replacementDict.update(dict.fromkeys(range(176, 190), ""))
replacementDict.update(dict.fromkeys(range(196, 198), ""))
replacementDict.update(dict.fromkeys(range(207, 208), ""))
replacementDict.update(dict.fromkeys(range(214, 216), ""))
replacementDict.update(dict.fromkeys(range(221, 223), ""))
replacementDict.update(dict.fromkeys(range(228, 230), ""))
replacementDict.update(dict.fromkeys(range(239, 240), ""))
replacementDict.update(dict.fromkeys(range(246, 248), ""))
replacementDict.update(dict.fromkeys(range(8192, 8207), ""))
replacementDict.update(dict.fromkeys(range(8216, 8218), "'"))
replacementDict.update(dict.fromkeys(range(8220, 8223), "\""))
replacementDict.update(dict.fromkeys(range(9617, 9619), ""))
replacementDict[129] = ""
replacementDict[141] = ""
replacementDict[143] = ""
replacementDict[145] = ""
replacementDict[154] = ""
replacementDict[160] = ""
replacementDict[161] = "!"
replacementDict[164] = ""
replacementDict[173] = ""
replacementDict[175] = "-"
replacementDict[203] = ""
replacementDict[235] = ""
replacementDict[255] = ""
replacementDict[305] = ""
replacementDict[353] = ""
replacementDict[402] = ""
replacementDict[5760] = "-"
replacementDict[6158] = ""
replacementDict[8211] = "-"
replacementDict[8215] = "_"
replacementDict[8218] = ","
replacementDict[8226] = ""
replacementDict[8240] = ""
replacementDict[8801] = "="
replacementDict[9472] = "-"
replacementDict[9474] = ""
replacementDict[9484] = ""
replacementDict[9488] = ""
replacementDict[9492] = ""
replacementDict[9496] = ""
replacementDict[9500] = ""
replacementDict[9516] = ""
replacementDict[9524] = ""
replacementDict[9532] = ""
replacementDict[9552] = "="
replacementDict[9553] = ""
replacementDict[9556] = ""
replacementDict[9562] = ""
replacementDict[9565] = ""
replacementDict[9568] = ""
replacementDict[9571] = ""
replacementDict[9574] = ""
replacementDict[9577] = ""
replacementDict[9580] = ""
replacementDict[9600] = ""
replacementDict[9604] = ""
replacementDict[9608] = ""
replacementDict[9632] = ""
replacementDict[12290] = ""
replacementDict[57425] = ""
replacementDict[61137] = ""
replacementDict[65279] = ""
replacementDict[65533] = ""

numOfLines = 6000
constant = 6000
numOfAndroidLogs = 0
numOfEmailLogs = 0
numOfErrorLogs = 0
numOfExtensionLogs = 0
numOfGeneralLogs = 0
numOfGeneralTXTLogs = 0
numOfIISLogs = 0
numOfIntegrationsLogs = 0
numOfiOSLogs = 0
numOfMobileRequestsLogs = 0
numOfServiceActionLogs = 0
numOfServiceStudioReports = 0
numOfTimerLogs = 0
numOfScreenLogs = 0
numOfWinAppEvViewerLogs = 0
numOfWinSecEvViewerLogs = 0
numOfWinSysEvViewerLogs = 0

errorLogsRegex = r"^([\d]+)\|([\w\-]+)\|([\d\-\:\. ]+)\|([\w\'\/\=\+ ]+)?\|([\d]+)\|([\d]+)\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|([\w\-\(\)\.\* ]+)?\|([\w\.\(\)]+)?\|([\w\-]+)?\|([\w]+)?\|([\w\-\.\,\(\)\[\]\/\& ]+)?\|([\w\-]+)?"
negativeErrorLogsRegex = r"^((?!(?:[\d]+)\|(?:[\w\-]+)\|(?:[\d\-\:\. ]+)\|(?:[\w\'\/\=\+ ]+)?\|(?:[\d]+)\|(?:[\d]+)\|(?:[\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|(?:[\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|(?:[\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|(?:[\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|(?:[\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|(?:[\w\-\(\)\.\* ]+)?\|(?:[\w\.\(\)]+)?\|(?:[\w\-]+)?\|(?:[\w]+)?\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)?\|(?:[\w\-]+)?).*)"
nonMatchedErrorLogsRegex = r"^((?:.*?\|){2})([\d\-]+)(.+)"
japaneseErrorLogsRegex = r"^([\d]+)\|(.*?)\|([\d\-\:\. ]+)\|(.*?)?\|([\d]+)\|([\d]+)\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?"
errorLogsContentRegex = r"^(?:[\d\-\:\. ]+)\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|(?:(?:.*?\|){6})([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|([\w\-]+)\|(?:[\d]+)"
errorLogsContentRegex2 = r"^(?:[\d\-\:\. ]+)\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|(?:(?:.*?\|){5})([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|([\w\-]+)\|(?:[\d]+)"

generalLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\w\+\/\=\' ]+)?\|([\d]+)\|([\d]+)\|([\w\-]+)?\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|([\w]+)?\|([\w\-\.\:\#\[\] ]+)?\|([\w\-]+)?\|([\w\-\(\)\.\* ]+)?\|([\w\(\)\.]+)?\|([\w\-\.\:\;\%\= ]+)?\|([\w]+)?\|([\w\-\.\,\(\)\[\]\/\& ]+)?\|([\w\-]+)?\|([\w\@\.\\]+)?"
negativeGeneralLogsRegex = r"^((?!(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\w\+\/\=\' ]+)?\|(?:[\d]+)\|(?:[\d]+)\|(?:[\w\-]+)?\|(?:[\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|(?:[\w]+)?\|(?:[\w\-\.\:\#\[\] ]+)?\|(?:[\w\-]+)?\|(?:[\w\-\(\)\.\* ]+)?\|(?:[\w\(\)\.]+)?\|(?:[\w\-\.\:\;\%\= ]+)?\|(?:[\w]+)?\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)?\|(?:[\w\-]+)?\|(?:[\w\@\.\\]+)?).*)"
nonMatchedGeneralLogsRegex = r"^((?:.*?\|){1})([\d\-]+)(.+)"
japaneseGeneralLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|(.*?)?\|([\d]+)\|([\d]+)\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?"
generalLogsContentRegex = r"^([\d\-\: ]+)\|.*?([\d]+)\s*?ms.*?\|(SLOWSQL|SLOWEXTENSION)\|([\w\(\)\. ]+)\|.+?\|([\w\(\)\.]+|\s*?)\|(?:(?:.*?\|){2})([\w]+|\s*?)\|(?:(?:.*?\|){3})([\w\-]+|\s*?)?\|.+"

integrationsLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w\.\:\;\-\% ]+)?\|([\w\:\/\\\.\,\*\-\=\%\&\?\~\(\)\{\}]+)?\|([\w\/\.\-\(\) ]+)\|([\w\(\) ]+)\|([\d]+)\|([\w\-]+)?\|([\w\-]+)\|([\w\-\:]+)\|([\w\.]+)?\|([\w\-\.\,\(\)\[\]\/\& ]+)?\|([\w\-]+)?"
negativeIntegrationsLogsRegex = r"^((?!(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\d]+)\|(?:[\w\.\:\;\-\% ]+)?\|(?:[\w\:\/\\\.\,\*\-\=\%\&\?\~\(\)\{\}]+)?\|(?:[\w\/\.\-\(\) ]+)\|(?:[\w\(\) ]+)\|(?:[\d]+)\|(?:[\w\-]+)?\|(?:[\w\-]+)\|(?:[\w\-\:]+)\|(?:[\w\.]+)?\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)?\|(?:[\w\-]+)?).*)"
nonMatchedIntegrationsLogsRegex = r"^((?:.*?\|){1})([\d\-]+)(.+)"
japaneseIntegrationsLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|(.*?)?\|(.*?)?\|(.*?)\|(.*?)\|([\d]+)\|(.*?)?\|(.*?)\|(.*?)\|(.*?)?\|(.*?)?\|(.*?)?"
integrationsLogsContentRegex = r"^([\d\-\:\. ]+)\|([\d]+)\|([\w\-\.\,\(\)\[\]\/\& ]+).+?\|([\w\/\.\-\(\) ]+)\|([\w\(\) ]+)\|(?:(?:.+?\|){3})([\w\.]+|\s*?)\|(?:[\d]+)\|([\w\-]+|\s*?)\|.+"

mobileRequestsLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\.\-\:\;\% ]+)\|([\w\:\/\\\.\-\=\%\&\?]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\d]+)\|([\w\-\:]+)\|([\w\/\+\=\']+)?\|([\d]+)\|([\w\.]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|([\w\-]+)"
negativeMobileRequestsLogsRegex = r"^((?!(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\d]+)\|(?:[\w]+)\|(?:[\w\.\-\:\;\% ]+)\|(?:[\w\:\/\\\.\-\=\%\&\?]+)\|(?:[\d]+)\|(?:[\w\-]+)\|(?:[\w\-]+)?\|(?:[\d]+)\|(?:[\w\-\:]+)\|(?:[\w\/\+\=\']+)?\|(?:[\d]+)\|(?:[\w\.]+)\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)\|(?:[\w\-]+)).*)"
nonMatchedMobileRequestsLogsRegex = r"^((?:.*?\|){1})([\d\-]+)(.+)"
japaneseMobileRequestsLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|(.*?)\|(.*?)\|(.*?)\|([\d]+)\|(.*?)\|(.*?)?\|([\d]+)\|(.*?)\|(.*?)?\|([\d]+)\|(.*?)\|(.*?)\|(.*?)"
mobileRequestsLogsContentRegex = r"^([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|(?:(?:.*?\|){4})([\w\.]+)\|(?:(?:.*?\|){4})([\w\-]+|\s*?)\|.+"

timerLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w\-]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\d\-\:\. ]+)\|([\d\-\:\. ]+)\|([\w\-\:]+)?\|([\w\.]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|([\w\-]+)\|([\w]+)"
negativeTimerLogsRegex = r"^((?!(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\d]+)\|(?:[\w\-]+)\|(?:[\d]+)\|(?:[\w\-]+)\|(?:[\w\-]+)?\|(?:[\d\-\:\. ]+)\|(?:[\d\-\:\. ]+)\|(?:[\w\-\:]+)?\|(?:[\w\.]+)\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)\|(?:[\w\-]+)\|(?:[\w]+)).*)"
nonMatchedTimerLogsRegex = r"^((?:.*?\|){1})([\d\-]+)(.+)"
japaneseTimerLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|(.*?)\|([\d]+)\|(.*?)\|(.*?)?\|([\d\-\:\. ]+)\|([\d\-\:\. ]+)\|(.*?)?\|(.*?)\|(.*?)\|(.*?)\|(.*?)"
timerLogsContentRegex = r"^([\d\-\:\. ]+)\|([\d]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|(?:(?:.+?\|){2})([\w\.]+)\|(?:.+?\|)([\w]+)\|(?:(?:.*?\|){2})(?:[\d\-\:\. ]+)\|([\w\-]+|\s*?)\|.+"

emailLogsRegex = r"^([\d]+)\|([\w]+)\|([\w\-\:\. ]+)\|([\w\-\:\. ]+)?\|([\d]+)\|([\w\@\.\,\- ]+)\|([\w\@\.\,\- ]+)\|([\w\@\.\-]+)?\|([\w\@\.\-]+)?\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)\|([\w]+)\|([\w]+)\|([\d]+)\|([\w\@\.\-]+)"
negativeEmailLogsRegex = r"^((?!(?:[\d]+)\|(?:[\w]+)\|(?:[\w\-\:\. ]+)\|(?:[\w\-\:\. ]+)?\|(?:[\d]+)\|(?:[\w\@\.\,\- ]+)\|(?:[\w\@\.\,\- ]+)\|(?:[\w\@\.\-]+)?\|(?:[\w\@\.\-]+)?\|(?:[\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|(?:[\d\-\:\. ]+)\|(?:[\d]+)\|(?:[\d]+)\|(?:[\w]+)\|(?:[\w]+)\|(?:[\d]+)\|(?:[\w\@\.\-]+)).*)"
nonMatchedEmailLogsRegex = r"^((?:.*?\|){10})([\d\-]+)(.+)"
japaneseEmailLogsRegex = r"^([\d]+)\|(.*?)\|(.*?)\|(.*?)?\|([\d]+)\|(.*?)\|(.*?)\|(.*?)?\|(.*?)?\|(.*?)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)\|(.*?)\|(.*?)\|([\d]+)\|(.*?)"
emailLogsContentRegex = r"^([\d\-\:\. ]+)\|([\w\-\:\. ]+)\|([\w\-\:\. ]+)\|([\w\@\.\,\- ]+)\|([\w\@\.\,\- ]+)\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|([\w\@\.\-]+|\s*?)\|([\w\@\.\-]+|\s*?)\|([\w]+)\|(?:(?:.*?\|){5})([\w]+)\|.+"

extensionLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\/\'\=\+]+)\|([\d]+)\|([\d]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\w\-\:]+)\|([\w\.]+)\|([\w]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|([\w\-]+)\|([\w\@\.\\]+)?"
negativeExtensionLogsRegex = r"^((?!(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\d]+)\|(?:[\w]+)\|(?:[\w\/\'\=\+]+)\|(?:[\d]+)\|(?:[\d]+)\|(?:[\d]+)\|(?:[\w\-]+)\|(?:[\w\-]+)?\|(?:[\w\-\:]+)\|(?:[\w\.]+)\|(?:[\w]+)\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)\|(?:[\w\-]+)\|(?:[\w\@\.\\]+)?).*)"
nonMatchedExtensionLogsRegex = r"^((?:.*?\|){1})([\d\-]+)(.+)"
japaneseExtensionLogsRegex = r"^([\d]+)\|(.*?)\|(.*?)\|(.*?)?\|([\d]+)\|(.*?)\|(.*?)\|(.*?)?\|(.*?)?\|(.*?)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)\|(.*?)\|(.*?)\|([\d]+)\|(.*?)"
extensionLogsContentRegex = r"^([\d\-\:\. ]+)\|([\d]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|.*?\|([\w]+)\|.*?\|([\w\.]+)\|(?:(?:.*?\|){5})([\w]+)\|([\w\-]+|\s*?)\|.+"

serviceActionLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)?\|([\w\/\'\=\+]+)\|([\d]+)\|([\w\-]+)?\|([\w\-]+)\|([\w\-\:]+)\|([\w\-\(\)\.\* ]+)?\|([\w\.\(\)]+)\|([\d]+)\|([\w\.\:\;\-\% ]+)\|([\w\:\/\\\.\-\=\%\&\?]+)\|([\w\.]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|([\w\-]+)\|([\w\@\.\\]+)?\|([\w\-\:]+)"
negativeServiceActionLogsRegex = r"^((?!(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\d]+)\|(?:[\d]+)?\|(?:[\w\/\'\=\+]+)\|(?:[\d]+)\|(?:[\w\-]+)?\|(?:[\w\-]+)\|(?:[\w\-\:]+)\|(?:[\w\-\(\)\.\* ]+)?\|(?:[\w\.\(\)]+)\|(?:[\d]+)\|(?:[\w\.\:\;\-\% ]+)\|(?:[\w\:\/\\\.\-\=\%\&\?]+)\|(?:[\w\.]+)\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)\|(?:[\w\-]+)\|(?:[\w\@\.\\]+)?\|(?:[\w\-\:]+)).*)"
nonMatchedServiceActionLogsRegex = r"^((?:.*?\|){1})([\d\-]+)(.+)"
japaneseServiceActionLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)?\|(.*?)\|([\d]+)\|(.*?)?\|(.*?)\|(.*?)\|(.*?)?\|(.*?)\|([\d]+)\|(.*?)\|(.*?)\|(.*?)\|(.*?)\|(.*?)\|(.*?)?\|(.*?)"
serviceActionLogsContentRegex = r"^([\d\-\:\. ]+)\|([\d]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|.*?\|([\w\.\(\)]+)\|(?:(?:.*?\|)){4}([\w\.]+)\|(?:(?:.*?\|){5})([\w\-]+|\s*?)\|.+"

screenLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\/\'\=\+]+)\|([\d]+)\|([\d]+)\|([\w\-\:\. ]+)?\|([\w]+)\|([\w\-]+)\|([\d]+)\|([\d]+)\|([\d]+)\|([\w]+)\|([\w\-\:]+)\|([\w\(\)\.]+)?\|([\w\-\:\. ]+)\|([\w\.]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|([\w\-]+)"
negativeScreenLogsRegex = r"^((?!(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\d]+)\|(?:[\w]+)\|(?:[\w\/\'\=\+]+)\|(?:[\d]+)\|(?:[\d]+)\|(?:[\w\-\:\. ]+)?\|(?:[\w]+)\|(?:[\w\-]+)\|(?:[\d]+)\|(?:[\d]+)\|(?:[\d]+)\|(?:[\w]+)\|(?:[\w\-\:]+)\|(?:[\w\(\)\.]+)?\|(?:[\w\-\:\. ]+)\|(?:[\w\.]+)\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)\|(?:[\w\-]+)).*)"
nonMatchedScreenLogsRegex = r"^((?:.*?\|){1})([\d\-]+)(.+)"
japaneseScreenLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|(.*?)\|(.*?)\|([\d]+)\|([\d]+)\|(.*?)?\|(.*?)\|(.*?)\|([\d]+)\|([\d]+)\|([\d]+)\|(.*?)\|(.*?)\|(.*?)?\|(.*?)\|(.*?)\|(.*?)\|(.*?)"
screenLogsContentRegex = r"^([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|.*?\|([\w\(\)\.]+|\s*?)\|(?:(?:.*?\|){3})([\w\.]+)\|.+"

bptTroubleshootingReportsRegex = "^([\d]+)\|([\w]+)\|([\w]+)\|([\w]+)\|([\d]+)\|([\d\-\:\. ]+)\|([\d\-\:\. ]+)\|([\w\d\-\:\. ]+)?\|([\w\? ]+)\|([\w]+)\|([\d]+)\|([\w\-]+)?\|([\w ]+)\|([\d\-\:\. ]+)((?:\|)(?:[\w\d\-\:\. ]+))?((?:\|)(?:[\w\d\-\:\. ]+))?((?:\|)(?:[\w\d\-\:\. ]+))?"
negativeBptTroubleshootingReportsRegex = "^((?!(?:[\d]+)\|(?:[\w]+)\|(?:[\w]+)\|(?:[\w]+)\|(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\d\-\:\. ]+)\|(?:[\w\d\-\:\. ]+)?\|(?:[\w\? ]+)\|(?:[\w]+)\|(?:[\d]+)\|(?:[\w\-]+)?\|(?:[\w ]+)\|(?:[\d\-\:\. ]+)(?:(?:\|)(?:[\w\d\-\:\. ]+))?(?:(?:\|)(?:[\w\d\-\:\. ]+))?(?:(?:\|)(?:[\w\d\-\:\. ]+))?).*)"
nonMatchedBptTroubleshootingReportsRegex = "^(.*?\|)([\d\-]+)((?:\s+[\d\:\.]+).+)"

androidiOSBuildLogsRegex = "^\[([\d\-]+)T([\d\:\.]+)Z\][ ]\[(INFO|VERBOSE|ERROR|DEBUG)\][ ](\[(?:[\w\[\] ]+)\])?(?:[ \t]+)?([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?"
negativeAndroidiOSBuildLogsRegex = "^((?!\[(?:[\d\-]+)T(?:[\d\:\.]+)Z\][ ]\[(?:INFO|VERBOSE|ERROR|DEBUG)\][ ](?:\[(?:[\w\[\] ]+)\])?(?:[ \t]+)?(?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?).*)"
nonMatchedAndroidiOSBuildLogsRegex = "^\[([\d\-]+)(?:T.+)"
japaneseAndroidiOSBuildLogsRegex = "^\[([\d\-]+)T([\d\:\.]+)Z\](.+)"

serviceStudioReportsDetailsRegex = "^.*?(Service.*?:\s*?(?:[\d\.])+).*?(Platform.*?:\s*?(?:[\w\d\.\[\] ])+).*?(Service.*?)Channel"
serviceStudioReportsOperationsLogsRegex0 = "(\[(?:[\d\-]+)[ ](?:[\d\:A-Z ])+\])"
serviceStudioReportsOperationsLogsRegex1 = "\[([\d\-]+)[ ]([\d\:A-Z ]+)\][ ](\[(?:[\d\:\?]+)\](?:[\w\-\(\) ]{10,}?)\s+)?([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®\t\r\n ]+)?\|"
serviceStudioReportsOperationsLogsRegex2 = "^([\d\-]+)\|([\d\:A-Z ]+)\|(\[(?:[\d\:\?]+)\](?:[\w\-\(\) ]{10,}?))?\|([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?"
negativeServiceStudioReportsOperationsLogsRegex = "^((?!(?:[\d\-]+)\|(?:[\d\:A-Z ]+)\|(?:\[(?:[\d\:\?]+)\](?:[\w\-\(\) ]{10,}?))?\|(?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?).*)"
nonMatchedServiceStudioReportsOperationsLogsRegex = "^([\d\-]+)\|(.+)"
japaneseServiceStudioReportsOperationsLogsRegex = "^([\d\-]+)\|([\d\:A-Z ]+)\|(.+)"

generalTextLogsRegex = "^([\w]+)\:\s*?\[([\d\:\.]+)\]\s+([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)"
generalTextLogsRegex2 = "^\[([\d\:\.]+)\]\s+([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)"

iisLogsRegex = "^([\d\-]+)[ ]([\d\:]+)[ ]([\w\-\.\:\%]+)[ ](POST|PUT|PROPFIND|(?:n)?GET|OPTIONS|HEAD|ABCD|QUALYS|TRACE|SEARCH|RNDMMTD|TRACK|B(?:A)?DM(?:E)?T(?:H)?(?:O)?(?:D)?|CFYZ|DEBUG|MKCOL|INDEX|DELETE|PATCH|ACUNETIX)[ ]([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ]([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ]([\d]+)[ ]([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ]([\w\-\.\:\%]+)[ ]([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ]([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ]([\d]+)[ ]([\d]+)[ ]([\d]+)[ ]([\d]+)"
negativeIisLogsRegex = "^((?!(?:[\d\-]+)[ ](?:[\d\:]+)[ ](?:[\w\-\.\:\%]+)[ ](?:POST|PUT|PROPFIND|(?:n)?GET|OPTIONS|HEAD|ABCD|QUALYS|TRACE|SEARCH|RNDMMTD|TRACK|B(?:A)?DM(?:E)?T(?:H)?(?:O)?(?:D)?|CFYZ|DEBUG|MKCOL|INDEX|DELETE|PATCH|ACUNETIX)[ ](?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ](?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ](?:[\d]+)[ ](?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ](?:[\w\-\.\:\%]+)[ ](?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ](?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ](?:[\d]+)[ ](?:[\d]+)[ ](?:[\d]+)[ ](?:[\d]+)).*)"
nonMatchedIisLogsRegex = "^([\d\-]+)(.+)"
japaneseIisLogsRegex = "^([\d\-]+)[ ]([\d\:]+)(.+)[ ]([\d]+)"

fullErrorDumpLogsRegex = "^(LifeTime.*?(?:\r\n|\n).*?Platform.*?(?:\r\n|\n))(?:\r\n|\n)(\=.*?(?:[\w ]+)\=.*?(?:\r\n|\n)(?:(?:.+)?(?:\r\n|\n)){1,})"

nonMatchedPath = os.getcwd() + "\\nonmatched_valid_lines\\file.txt"
tempFilePath = os.getcwd() + "\\tempFile.txt"

start = datetime.now()

def prereqs(directoryPath, fromDate, toDate, createGraphs):
    _fromDate = datetime.strptime(fromDate, "%Y-%m-%d").date()
    _toDate = datetime.strptime(toDate, "%Y-%m-%d").date()

    if _fromDate > _toDate:
        print("The \"from date\" cannot be greater than the \"to date\"\nPlease try again.")
    else:
        searchDirectory(directoryPath, _fromDate, _toDate)

        if createGraphs[0].lower() == "y":
            #create the graphs
            scripts.integrations.create_graph(directoryPath)
            scripts.general.create_graph(directoryPath)
            scripts.timer.create_graph(directoryPath)
            scripts.screen.create_graph(directoryPath)
            scripts.mobilerequests.create_graph(directoryPath)

        if os.path.exists(nonMatchedPath):
            print("\nALERT!\nThere were valid lines that did not match the logic.\n" +
                  "A file has been created in your current workig directory: \"" + os.getcwd() + "\" under the \"nonmatched_valid_lines\" folder.\n" +
                  "Please go to the Slack channel #log-parser-feedback and post the generated file for further review.\nALERT!")

def splitDirectory(root, file):
    absolutePathOfFile = os.path.join(root, file)
    filePathWithoutFilename = os.path.split(absolutePathOfFile)[0]
    filenameWithExt = os.path.split(absolutePathOfFile)[1]
    filenameWithoutExt = os.path.splitext(filenameWithExt)[0]
    extension = os.path.splitext(filenameWithExt)[1]

    return absolutePathOfFile, filePathWithoutFilename, filenameWithExt, filenameWithoutExt, extension

def splitDirectory2(root, file):
    absolutePathOfFile = os.path.join(root, file)
    filePathWithoutFilename = os.path.split(absolutePathOfFile)[0]
    filenameWithoutExt = os.path.splitext(file)[0]
    extension = os.path.splitext(file)[1]

    return absolutePathOfFile, filePathWithoutFilename, filenameWithoutExt, extension

def searchDirectory(directoryPath, _fromDate, _toDate):
    readStagingInfrastructureUserPermissionReports = False
    #search for the files with the raw data in the specified directory
    for root, subFolders, files in os.walk(directoryPath):
        if "infrastructurereport" in root.lower():
            readStagingInfrastructureUserPermissionReports = True
            for f in files:
                absolutePathOfFile, filePathWithoutFilename, filenameWithExt, filenameWithoutExt, extension = splitDirectory(root, f)

                if extension == ".xlsx":
                    xlsxFile(absolutePathOfFile, filePathWithoutFilename, filenameWithoutExt, ".txt", _fromDate, _toDate)
                elif extension == ".txt":
                    txtFile(absolutePathOfFile, filenameWithoutExt, filenameWithExt, extension, _fromDate, _toDate)

        elif "stagingreport" in root.lower() or "userpermissionsreport" in root.lower():
            readStagingInfrastructureUserPermissionReports = True
            for f in files:
                absolutePathOfFile, filePathWithoutFilename, filenameWithExt, filenameWithoutExt, extension = splitDirectory(root, f)

                if extension == ".xlsx":
                    xlsxFile(absolutePathOfFile, filePathWithoutFilename, filenameWithoutExt, ".txt", _fromDate, _toDate)

        for s in subFolders:
            if readStagingInfrastructureUserPermissionReports:
                if "infrastructurereport" in s.lower():
                    absolutePathOfFile2 = os.path.join(root, s)
                    for root2, subFolders2, files2 in os.walk(absolutePathOfFile2):
                        for f2 in files2:
                            absolutePathOfFile, filePathWithoutFilename, filenameWithoutExt, extension = splitDirectory2(root2, f2)

                            if extension == ".xlsx":
                                xlsxFile(absolutePathOfFile, filePathWithoutFilename, filenameWithoutExt, ".txt", _fromDate, _toDate)
                            elif extension == ".txt":
                                txtFile(absolutePathOfFile, filenameWithoutExt, f2, extension, _fromDate, _toDate)

                elif "stagingreport" in s.lower() or "userpermissionsreport" in s.lower():
                    absolutePathOfFile3 = os.path.join(root, s)
                    for root3, subFolders3, files3 in os.walk(absolutePathOfFile3):
                        for f3 in files3:
                            absolutePathOfFile, filePathWithoutFilename, filenameWithoutExt, extension = splitDirectory2(root3, f3)

                            if extension == ".xlsx":
                                xlsxFile(absolutePathOfFile, filePathWithoutFilename, filenameWithoutExt, ".txt", _fromDate, _toDate)

        for f in files:
            if f.endswith(".zip"):
                zipFilePath = os.path.join(root, f)
                zipFilePathWithoutFilename = os.path.split(zipFilePath)[0]
                zipFilenameWithExt = os.path.split(zipFilePath)[1]
                zipFilenameWithoutExt = os.path.splitext(zipFilenameWithExt)[0]
                newZipFilePath = zipFilePathWithoutFilename + "\\" + zipFilenameWithoutExt
                shutil.unpack_archive(zipFilePath, newZipFilePath)
                searchDirectory(newZipFilePath, _fromDate, _toDate)
            else:
                if not "infrastructurereport" in root.lower() and not "stagingreport" in root.lower() and not "userpermissionsreport" in root.lower():
                    absolutePathOfFile, filePathWithoutFilename, filenameWithExt, filenameWithoutExt, extension = splitDirectory(root, f)

                    if extension == ".xlsx":
                        xlsxFile(absolutePathOfFile, filePathWithoutFilename, filenameWithoutExt, ".txt", _fromDate, _toDate)
                    elif extension == ".txt":
                        txtFile(absolutePathOfFile, filenameWithoutExt, filenameWithExt, extension, _fromDate, _toDate)
                    elif extension == ".log":
                        logFile(absolutePathOfFile, filenameWithExt, ".txt", _fromDate, _toDate)
                    elif extension == ".evtx":
                        evtxFile(absolutePathOfFile, filenameWithExt, ".txt", _fromDate, _toDate)

def createFolder(fldPath):
    fldName = os.getcwd() + fldPath
    if not os.path.exists(os.path.dirname(fldName)):
        os.makedirs(os.path.dirname(fldName))

def populateList(txtFile, rawList, cleanList):
    with codecs.open(txtFile, "r", "utf-8", "ignore") as linesFromText:
        rawList = linesFromText.readlines()

    rawList.sort()
    #remove duplicate records from the list
    cleanList = list(set(rawList))
    cleanList.sort()

    #remove empty elements from the list
    cleanList = list(filter(lambda x: x != "\n", cleanList))

    with codecs.open(txtFile, "w", "utf-8", "ignore") as linesToText:
        linesToText.writelines(cleanList)

    del rawList[:]
    del cleanList[:]

def populateList2(txtFile, rawList):
    with codecs.open(txtFile, "r", "utf-8", "ignore") as linesFromText:
        rawList = linesFromText.readlines()

    rawList.sort()

    with codecs.open(txtFile, "w", "utf-8", "ignore") as linesToText:
        linesToText.writelines(rawList)

    del rawList[:]

def cleanListFunc(txtFile, rawList, cleanList):
    if len(rawList) > 0:
        rawList.sort()
        #remove duplicate records from the list
        cleanList = list(set(rawList))
        cleanList.sort()

        #remove empty elements from the list
        cleanList = list(filter(lambda x: x != " \n", cleanList))

        with codecs.open(txtFile, "a", "utf-8", "ignore") as linesFromtTxtFile:
            linesFromtTxtFile.writelines(cleanList)

    del rawList[:]
    del cleanList[:]

def writeToFile(absolutePath, txtFile, rawList, cleanList):
    if not "infrastructurereport" in absolutePath.lower() and not "stagingreport" in absolutePath.lower() and not "userpermissionsreport" in absolutePath.lower():
        rawList.sort()
        #remove duplicate records from the list
        cleanList = list(set(rawList))
        cleanList.sort()

        with codecs.open(txtFile, "a+", "utf-8", "ignore") as linesFromDateRange:
            linesFromDateRange.seek(0)
            if len(linesFromDateRange.read(100)) > 0:
                linesFromDateRange.writelines("\n")
            linesFromDateRange.writelines(cleanList)
    else:
        with codecs.open(txtFile, "a+", "utf-8", "ignore") as linesFromDateRange:
            linesFromDateRange.seek(0)
            if len(linesFromDateRange.read(100)) > 0:
                linesFromDateRange.writelines("\n")
            linesFromDateRange.writelines(rawList)

    del rawList[:]
    del cleanList[:]

def writeToFile2(txtFile, rawList):
    rawList.sort()

    with codecs.open(txtFile, "a+", "utf-8", "ignore") as linesFromDateRange:
        linesFromDateRange.seek(0)
        if len(linesFromDateRange.read(100)) > 0:
            linesFromDateRange.writelines("\n")
        linesFromDateRange.writelines(rawList)

    del rawList[:]

def normalizeLines(string):
    string = string.replace("|", " ")
    string = string.replace("\t", " ")

    #remove all the new lines from the string
    newString = ' '.join(string.splitlines())

    #replace white spaces with a single space
    myNewString = ' '.join(newString.split())

    return myNewString

def splitFiles(numOfLines, constant, maxLines, cycleNum, inputTxtFile, ext, outputTxtFile):
    div = round(maxLines/constant)
    partial = constant*div

    myTempXLSXlines = ['']
    del myTempXLSXlines[:]

    with codecs.open(inputTxtFile, "r", "utf-8", "ignore") as inp:
        searchLines = inp.readlines()
        for x, line in enumerate(searchLines):
            if x < numOfLines:
                myTempXLSXlines.append(line)
            #make sure maxLines is greater or equal than numOfLines
            #otherwise, the program will never reach this point
            if x == numOfLines:
                myTempXLSXlines.append(line)
                cycleNum+=1
                with codecs.open(outputTxtFile + str(cycleNum) + ext, "w", "utf-8", "ignore") as out:
                    out.writelines(myTempXLSXlines)
                del myTempXLSXlines[:]
                numOfLines+=constant
                #final cycle
                if numOfLines > partial:
                    numOfLines = maxLines
            x+=1

    return cycleNum

def readSplitFilesXLSX(cycleNum, filename, ext, _fromDate, _toDate):
    count = 1
    try:
        while count <= cycleNum:
            with codecs.open(os.getcwd() + "\\split_data\\file_" + str(count) + ext, "r", "utf-8", "ignore") as linesFromText:
                searchLines = linesFromText.read()
                if "errorlog" in filename.lower():
                    readErrorLogs(searchLines, _fromDate, _toDate)
                elif "generallog" in filename.lower():
                    readGeneralLogs(searchLines, _fromDate, _toDate)
                elif "integrationslog" in filename.lower():
                    readIntegrationsLogs(searchLines, _fromDate, _toDate)
                elif "mobilerequestslog" in filename.lower():
                    readMobileRequestsLogs(searchLines, _fromDate, _toDate)
                elif "timerlog" in filename.lower():
                    readTimerLogs(searchLines, _fromDate, _toDate)
                elif "emaillog" in filename.lower():
                    readEmailLogs(searchLines, _fromDate, _toDate)
                elif "extensionlog" in filename.lower():
                    readExtensionLogs(searchLines, _fromDate, _toDate)
                elif "serviceactionlog" in filename.lower():
                    readServiceActionLogs(searchLines, _fromDate, _toDate)
                elif "screenlog" in filename.lower():
                    readScreenLogs(searchLines, _fromDate, _toDate)
                elif "troubleshootingreport" in filename.lower():
                    readBPTTroubleshootingReportLogs(searchLines, _fromDate, _toDate)
            count+=1
    except ValueError as valError:
        print("\nThe customer altered the original Excel spreadsheet.\n\nPossible reasons:\n1- The customer added a column on the spreadsheet.\n" +
              "2- The customer switched the columns of the spreadsheet.\n3- The customer modified the dates on the spreadsheet.\n")

def readErrorLogs(searchLines, _fromDate, _toDate):
    global numOfErrorLogs
    numOfErrorLogs+=1

    outText = ""
    nonMatchedOutText = ""
    JPText = ""
    nonMatchedLine = ""
    nonMatchedLine2 = ""

    with codecs.open(tempFilePath, "a+", "utf-8", "ignore") as tempFile:
        #split the fields and rearrange them to combine them all later
        regex = re.compile(errorLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in regex.finditer(searchLines):
            tenantID = match.group(1)
            iD = match.group(2)
            timestamp = match.group(3)
            sessionID = match.group(4)#null
            userID = match.group(5)
            eSpaceID = match.group(6)
            message = match.group(7)#null
            stack = match.group(8)#null
            moduleName = match.group(9)#null
            server = match.group(10)
            environmentInformation = match.group(11)#null
            entryPointName = match.group(12)#null
            actionName = match.group(13)#null
            requestKey = match.group(14)#null
            eSpaceName = match.group(15)#null
            applicationName = match.group(16)#null
            applicationKey = match.group(17)#null
                                    
            date = timestamp[0:10]
            time = timestamp[11:19]
            time = time.replace(".", "")

            _date = datetime.strptime(date, "%Y-%m-%d").date()

            if _fromDate <= _date <= _toDate:

                if sessionID == None:
                    sessionID = " "

                if message == None:
                    message = " "

                if stack == None:
                    stack = " "

                if moduleName == None:
                    moduleName = " "

                if environmentInformation == None:
                    environmentInformation = " "
                else:
                    os = " "
                    osVersion = " "
                    deviceUUID = " "
                    cordovaVersion = " "
                    deviceModel = " "
                    
                    operatingSystemRegex = re.search(r"^.*?OperatingSystem:\s*?([\w]+)[ ]([\d\.]+).+", environmentInformation)
                    if operatingSystemRegex:
                        os = operatingSystemRegex.group(1)
                        osVersion = operatingSystemRegex.group(2)

                        if "android" == os.lower():
                            androidOSVersionList.append(osVersion)
                        elif "ios" == os.lower():
                            iosOSVersionList.append(osVersion)

                    deviceUUIDRegex = re.search(r"^.*?DeviceUUID:\s*?([\w\-]+).+", environmentInformation)
                    if deviceUUIDRegex:
                        deviceUUID = deviceUUIDRegex.group(1)

                    cordovaVersionRegex = re.search(r"^.*?Cordova:\s*?([\d\.]+).+", environmentInformation)
                    if cordovaVersionRegex:
                        cordovaVersion = cordovaVersionRegex.group(1)

                    deviceModelRegex = re.search(r"^.*?DeviceModel:\s*?([\w]+).+", environmentInformation)
                    if deviceModelRegex:
                        deviceModel = deviceModelRegex.group(1)

                    if len(os.strip()) > 0 and len(osVersion.strip()) > 0:
                        deviceInformation = os + "|" + osVersion + "|" + deviceModel + "|" + cordovaVersion + "|" + deviceUUID
                        deviceInformationList.append(deviceInformation + "\n")

                if entryPointName == None:
                    entryPointName = " "

                if actionName == None:
                    actionName = " "

                if requestKey == None:
                    requestKey = " "

                if eSpaceName == None:
                    eSpaceName = " "

                if applicationName == None:
                    applicationName = " "

                if applicationKey == None:
                    applicationKey = " "

                moduleNamesList.append(moduleName + "\n")
                applicationNamesList.append(applicationName + "\n")
                actionNamesList.append(actionName + "\n")
                espaceNamesList.append(eSpaceName + "\n")

                outText = date + " " + time + "|" + message + "|" + stack + "|" + moduleName + "|" + applicationName + "|" + applicationKey + "|" + actionName + "|" + entryPointName + "|" + server + "|" + eSpaceName + "|" + eSpaceID + "|" + userID + "|" + sessionID + "|" + environmentInformation + "|" + iD + "|" + tenantID + "\n"

                tempFile.writelines(outText)

        #capture all the lines that didn't match the regex
        negativeRegex = re.compile(negativeErrorLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in negativeRegex.finditer(searchLines):
            nonMatchedLine = match.group(1)

            nonMatchedRegex = re.search(nonMatchedErrorLogsRegex, nonMatchedLine)
            if nonMatchedRegex:
                nonMatchedHead = nonMatchedRegex.group(1)
                nonMatchedDate = nonMatchedRegex.group(2)
                nonMatchedTail = nonMatchedRegex.group(3)

                _nonMatchedDate = datetime.strptime(nonMatchedDate, "%Y-%m-%d").date()

                #check if the non-matched lines fall within the specified range
                if _fromDate <= _nonMatchedDate <= _toDate:

                    nonMatchedLine2 = nonMatchedHead + nonMatchedDate + nonMatchedTail

                    #check if the line has Japanese characters
                    hiragana = re.findall(u'[\u3040-\u309F]', nonMatchedLine2)
                    katakana = re.findall(u'[\u30A0-\u30FF]', nonMatchedLine2)
                    kanji = re.findall(u'[\u4E00-\u9FAF]', nonMatchedLine2)

                    if hiragana or katakana or kanji:
                        JPRegex = re.search(japaneseErrorLogsRegex, nonMatchedLine2)
                        if JPRegex:
                            JPtenantID = JPRegex.group(1)
                            JPiD = JPRegex.group(2)
                            JPtimestamp = JPRegex.group(3)
                            JPsessionID = JPRegex.group(4)#null
                            JPuserID = JPRegex.group(5)
                            JPeSpaceID = JPRegex.group(6)
                            JPmessage = JPRegex.group(7)#null
                            JPstack = JPRegex.group(8)#null
                            JPmoduleName = JPRegex.group(9)#null
                            JPserver = JPRegex.group(10)
                            JPenvironmentInformation = JPRegex.group(11)#null
                            JPentryPointName = JPRegex.group(12)#null
                            JPactionName = JPRegex.group(13)#null
                            JPrequestKey = JPRegex.group(14)#null
                            JPeSpaceName = JPRegex.group(15)#null
                            JPapplicationName = JPRegex.group(16)#null
                            JPapplicationKey = JPRegex.group(17)#null

                            JPdate = JPtimestamp[0:10]
                            JPtime = JPtimestamp[11:19]
                            JPtime = JPtime.replace(".", "")

                            if JPsessionID == None:
                                JPsessionID = " "

                            if JPmessage == None:
                                JPmessage = " "

                            if JPstack == None:
                                JPstack = " "

                            if JPmoduleName == None:
                                JPmoduleName = " "

                            if JPenvironmentInformation == None:
                                JPenvironmentInformation = " "

                            if JPentryPointName == None:
                                JPentryPointName = " "

                            if JPactionName == None:
                                JPactionName = " "

                            if JPrequestKey == None:
                                JPrequestKey = " "

                            if JPeSpaceName == None:
                                JPeSpaceName = " "

                            if JPapplicationName == None:
                                JPapplicationName = " "

                            if JPapplicationKey == None:
                                JPapplicationKey = " "

                            moduleNamesList.append(JPmoduleName + "\n")
                            applicationNamesList.append(JPapplicationName + "\n")
                            actionNamesList.append(JPactionName + "\n")
                            espaceNamesList.append(JPeSpaceName + "\n")

                            JPText = JPdate + " " + JPtime + "|" + JPmessage + "|" + JPstack + "|" + JPmoduleName + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPactionName + "|" + JPentryPointName + "|" + JPserver + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPuserID + "|" + JPsessionID + "|" + JPenvironmentInformation + "|" + JPiD + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "ErrorLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def readGeneralLogs(searchLines, _fromDate, _toDate):
    global numOfGeneralLogs
    numOfGeneralLogs+=1

    outText = ""
    nonMatchedOutText = ""
    JPText = ""
    nonMatchedLine = ""
    nonMatchedLine2 = ""

    with codecs.open(tempFilePath, "a+", "utf-8", "ignore") as tempFile:
        #split the fields and rearrange them to combine them all later
        regex = re.compile(generalLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in regex.finditer(searchLines):
            tenantID = match.group(1)
            timestamp = match.group(2)
            sessionID = match.group(3)#null
            userID = match.group(4)
            eSpaceID = match.group(5)
            errorID = match.group(6)#null
            message = match.group(7)#null
            messageType = match.group(8)#null
            moduleName = match.group(9)#null
            requestKey = match.group(10)#null
            entryPointName = match.group(11)#null
            actionName = match.group(12)#null
            clientIP = match.group(13)#null
            eSpaceName = match.group(14)#null
            applicationName = match.group(15)#null
            applicationKey = match.group(16)#null
            username = match.group(17)#null
                                        
            date = timestamp[0:10]
            time = timestamp[11:19]
            time = time.replace(".", "")

            _date = datetime.strptime(date, "%Y-%m-%d").date()

            if _fromDate <= _date <= _toDate:

                if sessionID == None:
                    sessionID = " "

                if message == None:
                    message = " "

                if errorID == None:
                    errorID = " "

                if messageType == None:
                    messageType = " "

                if moduleName == None:
                    moduleName = " "

                if requestKey == None:
                    requestKey = " "

                if entryPointName == None:
                    entryPointName = " "

                if actionName == None:
                    actionName = " "

                if clientIP == None:
                    clientIP = " "

                if eSpaceName == None:
                    eSpaceName = " "

                if applicationName == None:
                    applicationName = " "

                if applicationKey == None:
                    applicationKey = " "

                if username == None:
                    username = " "

                moduleNamesList.append(moduleName + "\n")
                applicationNamesList.append(applicationName + "\n")
                actionNamesList.append(actionName + "\n")
                espaceNamesList.append(eSpaceName + "\n")

                outText = date + " " + time + "|" + message + "|" + messageType + "|" + moduleName + "|" + applicationName + "|" + applicationKey + "|" + actionName + "|" + entryPointName + "|" + clientIP + "|" + eSpaceName + "|" + eSpaceID + "|" + userID + "|" + sessionID + "|" + errorID + "|" + requestKey + "|" + tenantID + "\n"

                tempFile.writelines(outText)

        #capture all the lines that didn't match the regex
        negativeRegex = re.compile(negativeGeneralLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in negativeRegex.finditer(searchLines):
            nonMatchedLine = match.group(1)

            nonMatchedRegex = re.search(nonMatchedGeneralLogsRegex, nonMatchedLine)
            if nonMatchedRegex:
                nonMatchedHead = nonMatchedRegex.group(1)
                nonMatchedDate = nonMatchedRegex.group(2)
                nonMatchedTail = nonMatchedRegex.group(3)

                _nonMatchedDate = datetime.strptime(nonMatchedDate, "%Y-%m-%d").date()

                #check if the non-matched lines fall within the specified range
                if _fromDate <= _nonMatchedDate <= _toDate:

                    nonMatchedLine2 = nonMatchedHead + nonMatchedDate + nonMatchedTail

                    #check if the line has Japanese characters
                    hiragana = re.findall(u'[\u3040-\u309F]', nonMatchedLine2)
                    katakana = re.findall(u'[\u30A0-\u30FF]', nonMatchedLine2)
                    kanji = re.findall(u'[\u4E00-\u9FAF]', nonMatchedLine2)

                    if hiragana or katakana or kanji:
                        JPRegex = re.search(japaneseGeneralLogsRegex, nonMatchedLine2)
                        if JPRegex:
                            JPtenantID = JPRegex.group(1)
                            JPtimestamp = JPRegex.group(2)
                            JPsessionID = JPRegex.group(3)#null
                            JPuserID = JPRegex.group(4)
                            JPeSpaceID = JPRegex.group(5)
                            JPerrorID = JPRegex.group(6)#null
                            JPmessage = JPRegex.group(7)#null
                            JPmessageType = JPRegex.group(8)#null
                            JPmoduleName = JPRegex.group(9)#null
                            JPrequestKey = JPRegex.group(10)#null
                            JPentryPointName = JPRegex.group(11)#null
                            JPactionName = JPRegex.group(12)#null
                            JPclientIP = JPRegex.group(13)#null
                            JPeSpaceName = JPRegex.group(14)#null
                            JPapplicationName = JPRegex.group(15)#null
                            JPapplicationKey = JPRegex.group(16)#null
                            JPusername = JPRegex.group(17)#null
                                                        
                            JPdate = JPtimestamp[0:10]
                            JPtime = JPtimestamp[11:19]
                            JPtime = JPtime.replace(".", "")

                            if JPsessionID == None:
                                JPsessionID = " "

                            if JPmessage == None:
                                JPmessage = " "

                            if JPerrorID == None:
                                JPerrorID = " "

                            if JPmessageType == None:
                                JPmessageType = " "

                            if JPmoduleName == None:
                                JPmoduleName = " "

                            if JPrequestKey == None:
                                JPrequestKey = " "

                            if JPentryPointName == None:
                                JPentryPointName = " "

                            if JPactionName == None:
                                JPactionName = " "

                            if JPclientIP == None:
                                JPclientIP = " "

                            if JPeSpaceName == None:
                                JPeSpaceName = " "

                            if JPapplicationName == None:
                                JPapplicationName = " "

                            if JPapplicationKey == None:
                                JPapplicationKey = " "

                            if JPusername == None:
                                JPusername = " "

                            moduleNamesList.append(JPmoduleName + "\n")
                            applicationNamesList.append(JPapplicationName + "\n")
                            actionNamesList.append(JPactionName + "\n")
                            espaceNamesList.append(JPeSpaceName + "\n")

                            JPText = JPdate + " " + JPtime + "|" + JPmessage + "|" + JPmessageType + "|" + JPmoduleName + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPactionName + "|" + JPentryPointName + "|" + JPclientIP + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPuserID + "|" + JPsessionID + "|" + JPerrorID + "|" + JPrequestKey + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "GeneralLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def sortGeneralLogsContent(outFile1, outFile2, logsFile1, logsFile2, logsFile1Regex, logsFile2Regex):
    try:
        if not os.path.exists(outFile1) and not os.path.exists(outFile2):
            #populate list with the general_logs file
            with codecs.open(logsFile1, "r", "utf-8", "ignore") as linesFromText:
                generalLogsList = linesFromText.readlines()

            with codecs.open(logsFile2, "r", "utf-8", "ignore") as linesFromText2:
                errorLogsList = linesFromText2.readlines()

            print("Sorting the content from the SlowSQLs and the SlowExtensions")

            for g, gn in enumerate(generalLogsList):
                regex = re.search(logsFile1Regex, generalLogsList[g].strip())
                if regex:
                    timestamp = regex.group(1)
                    duration = regex.group(2)
                    moduleName = regex.group(3)
                    applicationName = regex.group(4)
                    actionName = regex.group(5)
                    eSpaceName = regex.group(6)
                    errorID = regex.group(7)

                    if applicationName == "None":
                        applicationName == " "

                    if actionName == "None":
                        actionName == " "

                    if eSpaceName == "None":
                        eSpaceName == " "

                    if errorID == "None":
                        errorID == " "

                    #duration is in milliseconds and it needs to be converted to seconds
                    seconds = int(duration)/1000

                    if "slowsql" == moduleName.lower():
                        slowSQLList.append(timestamp + "|" + str(seconds) + "|" + moduleName + "|" + applicationName + "|" + actionName + "|" + eSpaceName + "|" + errorID + "\n")
                        slowSQLList2.append(errorID + "\n")

                    elif "slowextension" == moduleName.lower():
                        slowExtensionList.append(timestamp + "|" + str(seconds) + "|" + moduleName + "|" + applicationName + "|" + actionName + "|" + eSpaceName + "|" + errorID + "\n")
                        slowExtensionList2.append(errorID + "\n")
                g+=1

            for e, err in enumerate(errorLogsList):
                regex = re.search(logsFile2Regex, errorLogsList[e].strip())
                if regex:
                    message = regex.group(1)
                    stack = regex.group(2)
                    moduleName = regex.group(3)
                    environmentInformation = regex.group(4)
                    iD = regex.group(5)

                    if message == "None":
                        message == " "

                    if stack == "None":
                        stack == " "

                    if moduleName == "None":
                        moduleName == " "

                    if environmentInformation == "None":
                        environmentInformation == " "

                    errorsList.append(iD + "|" + message + "|" + stack + "|" + moduleName + "|" + environmentInformation + "\n")
                    errorsList2.append(iD + "\n")
                e+=1

            del generalLogsList[:]
            del errorLogsList[:]

            for element in slowSQLList2:
                if len(element.strip()) > 0:
                    if element in errorsList2:
                        ind2 = errorsList2.index(element)
                        item2 = errorsList[ind2].split("|")
                        ind1 = slowSQLList2.index(element)
                        item1 = slowSQLList[ind1].split("|")
                        slowSQLErrorsList.append(item1[0] + "|" + item1[1] + "|" + item1[2] + "|" + item1[3] + "|" + item1[4] + "|" + item1[5] + "|" + item2[1] + "|" + item2[2] + "|" + item2[4].strip() + "|" + item1[6].strip() + "\n")

            for elm in slowSQLList:
                item3 = elm.split("|")
                if not len(item3[6].strip()) > 0:
                    slowSQLErrorsList.append(item3[0] + "|" + item3[1] + "|" + item3[2] + "|" + item3[3] + "|" + item3[4] + "|" + item3[5] + "||||\n")

            for element in slowExtensionList2:
                if len(element.strip()) > 0:
                    if element in errorsList2:
                        ind2 = errorsList2.index(element)
                        item2 = errorsList[ind2].split("|")
                        ind1 = slowExtensionList2.index(element)
                        item1 = slowExtensionList[ind1].split("|")
                        slowExtensionErrorsList.append(item1[0] + "|" + item1[1] + "|" + item1[2] + "|" + item1[3] + "|" + item1[4] + "|" + item1[5] + "|" + item2[1] + "|" + item2[2] + "|" + item2[4].strip() + "|" + item1[6].strip() + "\n")

            for elm in slowExtensionList:
                item3 = elm.split("|")
                if not len(item3[6].strip()) > 0:
                    slowExtensionErrorsList.append(item3[0] + "|" + item3[1] + "|" + item3[2] + "|" + item3[3] + "|" + item3[4] + "|" + item3[5] + "||||\n")

            del slowSQLList[:]
            del slowSQLList2[:]
            del slowExtensionList[:]
            del slowExtensionList2[:]
            del errorsList[:]
            del errorsList2[:]

            mySlowSQLErrorsList = list(set(slowSQLErrorsList))
            mySlowSQLErrorsList.sort()

            mySlowExtensionErrorsList = list(set(slowExtensionErrorsList))
            mySlowExtensionErrorsList.sort()

            if len(mySlowSQLErrorsList) > 0:
                with codecs.open(outFile1, "w", "utf-8", "ignore") as reportFile:
                    reportFile.writelines(mySlowSQLErrorsList)

                del slowSQLErrorsList[:]
                del mySlowSQLErrorsList[:]

            if len(mySlowExtensionErrorsList) > 0:
                with codecs.open(outFile2, "w", "utf-8", "ignore") as reportFile3:
                    reportFile3.writelines(mySlowExtensionErrorsList)

                del slowExtensionErrorsList[:]
                del mySlowExtensionErrorsList[:]

    except FileNotFoundError as fileError:
        pass

def readIntegrationsLogs(searchLines, _fromDate, _toDate):
    global numOfIntegrationsLogs
    numOfIntegrationsLogs+=1

    outText = ""
    nonMatchedOutText = ""
    JPText = ""
    nonMatchedLine = ""
    nonMatchedLine2 = ""

    with codecs.open(tempFilePath, "a+", "utf-8", "ignore") as tempFile:
        #split the fields and rearrange them to combine them all later
        regex = re.compile(integrationsLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in regex.finditer(searchLines):
            tenantID = match.group(1)
            timestamp = match.group(2)
            duration = match.group(3)
            source = match.group(4)#null
            endpoint = match.group(5)#null
            actionName = match.group(6)
            actionType = match.group(7)
            eSpaceID = match.group(8)
            errorID = match.group(9)#null
            executedBy = match.group(10)
            requestKey = match.group(11)
            eSpaceName = match.group(12)#null
            applicationName = match.group(13)#null
            applicationKey = match.group(14)#null
                                    
            date = timestamp[0:10]
            time = timestamp[11:19]
            time = time.replace(".", "")

            _date = datetime.strptime(date, "%Y-%m-%d").date()

            if _fromDate <= _date <= _toDate:

                if source == None:
                    source = " "

                if endpoint == None:
                    endpoint = " "

                if errorID == None:
                    errorID = " "

                if eSpaceName == None:
                    eSpaceName = " "

                if applicationName == None:
                    applicationName = " "

                if applicationKey == None:
                    applicationKey = " "

                applicationNamesList.append(applicationName + "\n")
                actionNamesList.append(actionName + "\n")
                espaceNamesList.append(eSpaceName + "\n")

                outText = date + " " + time + "|" + duration + "|" + applicationName + "|" + applicationKey + "|" + actionName + "|" + actionType + "|" + source + "|" + endpoint + "|" + executedBy + "|" + eSpaceName + "|" + eSpaceID + "|" + errorID + "|" + requestKey + "|" + tenantID + "\n"

                tempFile.writelines(outText)

        #capture all the lines that didn't match the regex
        negativeRegex = re.compile(negativeIntegrationsLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in negativeRegex.finditer(searchLines):
            nonMatchedLine = match.group(1)

            nonMatchedRegex = re.search(nonMatchedIntegrationsLogsRegex, nonMatchedLine)
            if nonMatchedRegex:
                nonMatchedHead = nonMatchedRegex.group(1)
                nonMatchedDate = nonMatchedRegex.group(2)
                nonMatchedTail = nonMatchedRegex.group(3)

                _nonMatchedDate = datetime.strptime(nonMatchedDate, "%Y-%m-%d").date()

                #check if the non-matched lines fall within the specified range
                if _fromDate <= _nonMatchedDate <= _toDate:

                    nonMatchedLine2 = nonMatchedHead + nonMatchedDate + nonMatchedTail

                    #check if the line has Japanese characters
                    hiragana = re.findall(u'[\u3040-\u309F]', nonMatchedLine2)
                    katakana = re.findall(u'[\u30A0-\u30FF]', nonMatchedLine2)
                    kanji = re.findall(u'[\u4E00-\u9FAF]', nonMatchedLine2)

                    if hiragana or katakana or kanji:
                        JPRegex = re.search(japaneseIntegrationsLogsRegex, nonMatchedLine2)
                        if JPRegex:
                            JPtenantID = JPRegex.group(1)
                            JPtimestamp = JPRegex.group(2)
                            JPduration = JPRegex.group(3)
                            JPsource = JPRegex.group(4)#null
                            JPendpoint = JPRegex.group(5)#null
                            JPactionName = JPRegex.group(6)
                            JPactionType = JPRegex.group(7)
                            JPeSpaceID = JPRegex.group(8)
                            JPerrorID = JPRegex.group(9)#null
                            JPexecutedBy = JPRegex.group(10)
                            JPrequestKey = JPRegex.group(11)
                            JPeSpaceName = JPRegex.group(12)#null
                            JPapplicationName = JPRegex.group(13)#null
                            JPapplicationKey = JPRegex.group(14)#null
                                                    
                            JPdate = JPtimestamp[0:10]
                            JPtime = JPtimestamp[11:19]
                            JPtime = JPtime.replace(".", "")

                            if JPsource == None:
                                JPsource = " "

                            if JPendpoint == None:
                                JPendpoint = " "

                            if JPerrorID == None:
                                JPerrorID = " "

                            if JPeSpaceName == None:
                                JPeSpaceName = " "

                            if JPapplicationName == None:
                                JPapplicationName = " "

                            if JPapplicationKey == None:
                                JPapplicationKey = " "

                            applicationNamesList.append(JPapplicationName + "\n")
                            actionNamesList.append(JPactionName + "\n")
                            espaceNamesList.append(JPeSpaceName + "\n")

                            JPText  = JPdate + " " + JPtime + "|" + JPduration + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPactionName + "|" + JPactionType + "|" + JPsource + "|" + JPendpoint + "|" + JPexecutedBy + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPerrorID + "|" + JPrequestKey + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "IntegrationsLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def sortIntegrationsLogsContent(outFile1, logsFile1, logsFile2, logsFile1Regex, logsFile2Regex):
    try:
        if not os.path.exists(outFile1):
            #populate list with the integrations_logs file and the error_logs file
            with codecs.open(logsFile1, "r", "utf-8", "ignore") as linesFromText:
                integrationsLogsList = linesFromText.readlines()

            with codecs.open(logsFile2, "r", "utf-8", "ignore") as linesFromText2:
                errorLogsList = linesFromText2.readlines()

            print("Sorting the content from the Integrations")
                
            for i, inte in enumerate(integrationsLogsList):
                regex = re.search(logsFile1Regex, integrationsLogsList[i].strip())
                if regex:
                    timestamp = regex.group(1)
                    duration = regex.group(2)
                    applicationName = regex.group(3)
                    actionName = regex.group(4)
                    actionType = regex.group(5)
                    eSpaceName = regex.group(6)
                    errorID = regex.group(7)

                    if applicationName == "None":
                        applicationName == " "

                    if eSpaceName == "None":
                        eSpaceName == " "

                    if errorID == "None":
                        errorID == " "

                    #duration is in milliseconds and it needs to be converted to seconds
                    seconds = int(duration)/1000
                    webServicesList.append(timestamp + "|" + str(seconds) + "|" + applicationName + "|" + actionName + "|" + actionType + "|" + eSpaceName + "|" + errorID + "\n")
                    webServicesList2.append(errorID + "\n")
                i+=1

            for e, err in enumerate(errorLogsList):
                regex = re.search(logsFile2Regex, errorLogsList[e].strip())
                if regex:
                    message = regex.group(1)
                    stack = regex.group(2)
                    moduleName = regex.group(3)
                    environmentInformation = regex.group(4)
                    iD = regex.group(5)

                    if message == "None":
                        message == " "

                    if stack == "None":
                        stack == " "

                    if moduleName == "None":
                        moduleName == " "

                    if environmentInformation == "None":
                        environmentInformation == " "

                    errorsList.append(iD + "|" + message + "|" + stack + "|" + moduleName + "|" + environmentInformation + "\n")
                    errorsList2.append(iD + "\n")
                e+=1

            del integrationsLogsList[:]
            del errorLogsList[:]

            for element in webServicesList2:
                if len(element.strip()) > 0:
                    if element in errorsList2:
                        ind2 = errorsList2.index(element)
                        item2 = errorsList[ind2].split("|")
                        ind1 = webServicesList2.index(element)
                        item1 = webServicesList[ind1].split("|")
                        webServicesErrorsList.append(item1[0] + "|" + item1[1] + "|" + item2[3] + "|" + item1[2] + "|" + item1[3] + "|" + item1[4] + "|" + item1[5] + "|" + item2[1] + "|" + item2[2] + "|" + item2[4].strip() + "|" + item1[6].strip() + "\n")

            for elm in webServicesList:
                item3 = elm.split("|")
                if not len(item3[6].strip()) > 0:
                    webServicesErrorsList.append(item3[0] + "|" + item3[1] + "||" + item3[2] + "|" + item3[3] + "|" + item3[4] + "|" + item3[5] + "||||\n")

            del webServicesList[:]
            del webServicesList2[:]
            del errorsList[:]
            del errorsList2[:]

            myWebServicesErrorsList = list(set(webServicesErrorsList))
            myWebServicesErrorsList.sort()

            if len(myWebServicesErrorsList) > 0:
                with codecs.open(outFile1, "w", "utf-8", "ignore") as reportFile:
                    reportFile.writelines(myWebServicesErrorsList)

                del webServicesErrorsList[:]
                del myWebServicesErrorsList[:]

    except FileNotFoundError as fileError:
        pass

def readMobileRequestsLogs(searchLines, _fromDate, _toDate):
    global numOfMobileRequestsLogs
    numOfMobileRequestsLogs+=1

    outText = ""
    nonMatchedOutText = ""
    JPText = ""
    nonMatchedLine = ""
    nonMatchedLine2 = ""

    with codecs.open(tempFilePath, "a+", "utf-8", "ignore") as tempFile:
        #split the fields and rearrange them to combine them all later
        regex = re.compile(mobileRequestsLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in regex.finditer(searchLines):
            tenantID = match.group(1)
            timestamp = match.group(2)
            eSpaceID = match.group(3)
            screen = match.group(4)
            source = match.group(5)
            endpoint = match.group(6)
            duration = match.group(7)
            executedBy = match.group(8)
            errorID = match.group(9)#null
            cycle = match.group(10)
            requestKey = match.group(11)
            loginID = match.group(12)#null
            userID = match.group(13)
            eSpaceName = match.group(14)
            applicationName = match.group(15)
            applicationKey = match.group(16)
                                    
            date = timestamp[0:10]
            time = timestamp[11:19]
            time = time.replace(".", "")

            _date = datetime.strptime(date, "%Y-%m-%d").date()

            if _fromDate <= _date <= _toDate:

                if errorID == None:
                    errorID = " "

                if loginID == None:
                    loginID = " "

                applicationNamesList.append(applicationName + "\n")
                espaceNamesList.append(eSpaceName + "\n")

                outText = date + " " + time + "|" + duration + "|" + screen + "|" + applicationName + "|" + applicationKey + "|" + source + "|" + endpoint + "|" + executedBy + "|" + eSpaceName + "|" + eSpaceID + "|" + loginID + "|" + userID + "|" + cycle + "|" + errorID + "|" + requestKey + "|" + tenantID + "\n"

                tempFile.writelines(outText)

        #capture all the lines that didn't match the regex
        negativeRegex = re.compile(negativeMobileRequestsLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in negativeRegex.finditer(searchLines):
            nonMatchedLine = match.group(1)

            nonMatchedRegex = re.search(nonMatchedMobileRequestsLogsRegex, nonMatchedLine)
            if nonMatchedRegex:
                nonMatchedHead = nonMatchedRegex.group(1)
                nonMatchedDate = nonMatchedRegex.group(2)
                nonMatchedTail = nonMatchedRegex.group(3)

                _nonMatchedDate = datetime.strptime(nonMatchedDate, "%Y-%m-%d").date()

                #check if the non-matched lines fall within the specified range
                if _fromDate <= _nonMatchedDate <= _toDate:

                    nonMatchedLine2 = nonMatchedHead + nonMatchedDate + nonMatchedTail

                    #check if the line has Japanese characters
                    hiragana = re.findall(u'[\u3040-\u309F]', nonMatchedLine2)
                    katakana = re.findall(u'[\u30A0-\u30FF]', nonMatchedLine2)
                    kanji = re.findall(u'[\u4E00-\u9FAF]', nonMatchedLine2)

                    if hiragana or katakana or kanji:
                        JPRegex = re.search(japaneseMobileRequestsLogsRegex, nonMatchedLine2)
                        if JPRegex:
                            JPtenantID = JPRegex.group(1)
                            JPtimestamp = JPRegex.group(2)
                            JPeSpaceID = JPRegex.group(3)
                            JPscreen = JPRegex.group(4)
                            JPsource = JPRegex.group(5)
                            JPendpoint = JPRegex.group(6)
                            JPduration = JPRegex.group(7)
                            JPexecutedBy = JPRegex.group(8)
                            JPerrorID = JPRegex.group(9)#null
                            JPcycle = JPRegex.group(10)
                            JPrequestKey = JPRegex.group(11)
                            JPloginID = JPRegex.group(12)#null
                            JPuserID = JPRegex.group(13)
                            JPeSpaceName = JPRegex.group(14)
                            JPapplicationName = JPRegex.group(15)
                            JPapplicationKey = JPRegex.group(16)
                                                    
                            JPdate = JPtimestamp[0:10]
                            JPtime = JPtimestamp[11:19]
                            JPtime = JPtime.replace(".", "")

                            if JPerrorID == None:
                                JPerrorID = " "

                            if JPloginID == None:
                                JPloginID = " "

                            applicationNamesList.append(JPapplicationName + "\n")
                            espaceNamesList.append(JPeSpaceName + "\n")

                            JPText = JPdate + " " + JPtime + "|" + JPduration + "|" + JPscreen + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPsource + "|" + JPendpoint + "|" + JPexecutedBy + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPloginID + "|" + JPuserID + "|" + JPcycle + "|" + JPerrorID + "|" + JPrequestKey + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:                
                        nonMatchedOutText = "MobileRequestsLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def sortMobileRequestsLogsContent(outFile1, logsFile1, logsFile2, logsFile1Regex, logsFile2Regex):
    try:
        if not os.path.exists(outFile1):
            #populate list with the timer_logs file
            with codecs.open(logsFile1, "r", "utf-8", "ignore") as linesFromText:
                mobileRequestsLogsList = linesFromText.readlines()

            with codecs.open(logsFile2, "r", "utf-8", "ignore") as linesFromText2:
                errorLogsList = linesFromText2.readlines()

            print("Sorting the content from the Mobile Requests Screens")
            
            for m, mo in enumerate(mobileRequestsLogsList):
                regex = re.search(logsFile1Regex, mobileRequestsLogsList[m].strip())
                if regex:
                    timestamp = regex.group(1)
                    duration = regex.group(2)
                    screen = regex.group(3)
                    applicationName = regex.group(4)
                    eSpaceName = regex.group(5)
                    errorID = regex.group(6)

                    if errorID == "None":
                        errorID == " "

                    #duration is in milliseconds and it needs to be converted to seconds
                    seconds = int(duration)/1000
                    mobileRequestsScreenList.append(timestamp + "|" + str(seconds) + "|" + screen + "|" + applicationName + "|" + eSpaceName + "|" + errorID + "\n")
                    mobileRequestsScreenList2.append(errorID + "\n")
                m+=1

            for e, err in enumerate(errorLogsList):
                regex = re.search(logsFile2Regex, errorLogsList[e].strip())
                if regex:
                    message = regex.group(1)
                    stack = regex.group(2)
                    moduleName = regex.group(3)
                    environmentInformation = regex.group(4)
                    iD = regex.group(5)

                    if message == "None":
                        message == " "

                    if stack == "None":
                        stack == " "

                    if moduleName == "None":
                        moduleName == " "

                    if environmentInformation == "None":
                        environmentInformation == " "

                    errorsList.append(iD + "|" + message + "|" + stack + "|" + moduleName + "|" + environmentInformation + "\n")
                    errorsList2.append(iD + "\n")
                e+=1

            del mobileRequestsLogsList[:]
            del errorLogsList[:]

            for element in mobileRequestsScreenList2:
                if len(element.strip()) > 0:
                    if element in errorsList2:
                        ind2 = errorsList2.index(element)
                        item2 = errorsList[ind2].split("|")
                        ind1 = mobileRequestsScreenList2.index(element)
                        item1 = mobileRequestsScreenList[ind1].split("|")
                        mobileRequestsScreenErrorsList.append(item1[0] + "|" + item1[1] + "|" + item1[2] + "|" + item2[3] + "|" + item1[3] + "|" + item1[4] + "|" + item2[1] + "|" + item2[2] + "|" + item2[4].strip() + "|" + item1[5].strip() + "\n")

            for elm in mobileRequestsScreenList:
                item3 = elm.split("|")
                if not len(item3[5].strip()) > 0:
                    mobileRequestsScreenErrorsList.append(item3[0] + "|" + item3[1] + "|" + item3[2] + "||" + item3[3] + "|" + item3[4] + "||||\n")

            del mobileRequestsScreenList[:]
            del mobileRequestsScreenList2[:]
            del errorsList[:]
            del errorsList2[:]

            myMobileRequestsScreenErrorsList = list(set(mobileRequestsScreenErrorsList))
            myMobileRequestsScreenErrorsList.sort()

            if len(myMobileRequestsScreenErrorsList) > 0:
                with codecs.open(outFile1, "w", "utf-8", "ignore") as reportFile:
                    reportFile.writelines(myMobileRequestsScreenErrorsList)

                del mobileRequestsScreenErrorsList[:]
                del myMobileRequestsScreenErrorsList[:]

    except FileNotFoundError as fileError:
        pass

def readTimerLogs(searchLines, _fromDate, _toDate):
    global numOfTimerLogs
    numOfTimerLogs+=1

    outText = ""
    nonMatchedOutText = ""
    JPText = ""
    nonMatchedLine = ""
    nonMatchedLine2 = ""

    with codecs.open(tempFilePath, "a+", "utf-8", "ignore") as tempFile:
        #split the fields and rearrange them to combine them all later
        regex = re.compile(timerLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in regex.finditer(searchLines):
            tenantID = match.group(1)
            timestamp = match.group(2)
            duration = match.group(3)
            cyclicJobKey = match.group(4)
            eSpaceID = match.group(5)
            executedBy = match.group(6)
            errorID = match.group(7)#null
            shouldHaveRunAt = match.group(8)
            nextRun = match.group(9)
            requestKey = match.group(10)#null
            eSpaceName = match.group(11)
            applicationName = match.group(12)
            applicationKey = match.group(13)
            cyclicJobName = match.group(14)
                                    
            date = timestamp[0:10]
            time = timestamp[11:19]
            time = time.replace(".", "")

            _date = datetime.strptime(date, "%Y-%m-%d").date()

            if _fromDate <= _date <= _toDate:

                if errorID == None:
                    errorID = " "

                if requestKey == None:
                    requestKey = " "

                applicationNamesList.append(applicationName + "\n")
                espaceNamesList.append(eSpaceName + "\n")
                cyclicJobNamesList.append(cyclicJobName + "\n")

                outText = date + " " + time + "|" + duration + "|" + applicationName + "|" + applicationKey + "|" + executedBy + "|" + eSpaceName + "|" + eSpaceID + "|" + cyclicJobName + "|" + cyclicJobKey + "|" + shouldHaveRunAt + "|" + nextRun + "|" + errorID + "|" + requestKey + "|" + tenantID + "\n"

                tempFile.writelines(outText)

        #capture all the lines that didn't match the regex
        negativeRegex = re.compile(negativeTimerLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in negativeRegex.finditer(searchLines):
            nonMatchedLine = match.group(1)

            nonMatchedRegex = re.search(nonMatchedTimerLogsRegex, nonMatchedLine)
            if nonMatchedRegex:
                nonMatchedHead = nonMatchedRegex.group(1)
                nonMatchedDate = nonMatchedRegex.group(2)
                nonMatchedTail = nonMatchedRegex.group(3)

                _nonMatchedDate = datetime.strptime(nonMatchedDate, "%Y-%m-%d").date()

                #check if the non-matched lines fall within the specified range
                if _fromDate <= _nonMatchedDate <= _toDate:

                    nonMatchedLine2 = nonMatchedHead + nonMatchedDate + nonMatchedTail

                    #check if the line has Japanese characters
                    hiragana = re.findall(u'[\u3040-\u309F]', nonMatchedLine2)
                    katakana = re.findall(u'[\u30A0-\u30FF]', nonMatchedLine2)
                    kanji = re.findall(u'[\u4E00-\u9FAF]', nonMatchedLine2)

                    if hiragana or katakana or kanji:
                        JPRegex = re.search(japaneseTimerLogsRegex, nonMatchedLine2)
                        if JPRegex:
                            JPtenantID = JPRegex.group(1)
                            JPtimestamp = JPRegex.group(2)
                            JPduration = JPRegex.group(3)
                            JPcyclicJobKey = JPRegex.group(4)
                            JPeSpaceID = JPRegex.group(5)
                            JPexecutedBy = JPRegex.group(6)
                            JPerrorID = JPRegex.group(7)#null
                            JPshouldHaveRunAt = JPRegex.group(8)
                            JPnextRun = JPRegex.group(9)
                            JPrequestKey = JPRegex.group(10)#null
                            JPeSpaceName = JPRegex.group(11)
                            JPapplicationName = JPRegex.group(12)
                            JPapplicationKey = JPRegex.group(13)
                            JPcyclicJobName = JPRegex.group(14)
                                                    
                            JPdate = JPtimestamp[0:10]
                            JPtime = JPtimestamp[11:19]
                            JPtime = JPtime.replace(".", "")

                            if JPerrorID == None:
                                JPerrorID = " "

                            if JPrequestKey == None:
                                JPrequestKey = " "

                            applicationNamesList.append(JPapplicationName + "\n")
                            espaceNamesList.append(JPeSpaceName + "\n")
                            cyclicJobNamesList.append(JPcyclicJobName + "\n")

                            JPText = JPdate + " " + JPtime + "|" + JPduration + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPexecutedBy + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPcyclicJobName + "|" + JPcyclicJobKey + "|" + JPshouldHaveRunAt + "|" + JPnextRun + "|" + JPerrorID + "|" + JPrequestKey + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:                  
                        nonMatchedOutText = "TimerLog -> " + nonMatchedLine2

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def sortTimerLogsContent(outFile1, logsFile1, logsFile2, logsFile1Regex, logsFile2Regex):
    try:
        if not os.path.exists(outFile1):
            #populate list with the timer_logs file
            with codecs.open(logsFile1, "r", "utf-8", "ignore") as linesFromText:
                timerLogsList = linesFromText.readlines()

            with codecs.open(logsFile2, "r", "utf-8", "ignore") as linesFromText2:
                errorLogsList = linesFromText2.readlines()

            print("Sorting the content from the Timers")
            
            for t, ti in enumerate(timerLogsList):
                regex = re.search(logsFile1Regex, timerLogsList[t].strip())
                if regex:
                    timestamp = regex.group(1)
                    duration = regex.group(2)
                    applicationName = regex.group(3)
                    eSpaceName = regex.group(4)
                    cyclicJobName = regex.group(5)
                    errorID = regex.group(6)

                    if errorID == "None":
                        errorID == " "

                    #duration is in milliseconds and it needs to be converted to seconds
                    seconds = int(duration)/1000
                    timersList.append(timestamp + "|" + str(seconds) + "|" + applicationName + "|" + cyclicJobName + "|" + eSpaceName + "|" + errorID + "\n")
                    timersList2.append(errorID + "\n")
                t+=1

            for e, err in enumerate(errorLogsList):
                regex = re.search(logsFile2Regex, errorLogsList[e].strip())
                if regex:
                    message = regex.group(1)
                    stack = regex.group(2)
                    moduleName = regex.group(3)
                    environmentInformation = regex.group(4)
                    iD = regex.group(5)

                    if message == "None":
                        message == " "

                    if stack == "None":
                        stack == " "

                    if moduleName == "None":
                        moduleName == " "

                    if environmentInformation == "None":
                        environmentInformation == " "

                    errorsList.append(iD + "|" + message + "|" + stack + "|" + moduleName + "|" + environmentInformation + "\n")
                    errorsList2.append(iD + "\n")
                e+=1

            del timerLogsList[:]
            del errorLogsList[:]

            for element in timersList2:
                if len(element.strip()) > 0:
                    if element in errorsList2:
                        ind2 = errorsList2.index(element)
                        item2 = errorsList[ind2].split("|")
                        ind1 = timersList2.index(element)
                        item1 = timersList[ind1].split("|")
                        timersErrorsList.append(item1[0] + "|" + item1[1] + "|" + item1[3] + "|" + item2[3] + "|" + item1[2] + "|" + item1[4] + "|" + item2[1] + "|" + item2[2] + "|" + item2[4].strip() + "|" + item1[5].strip() + "\n")

            for elm in timersList:
                item3 = elm.split("|")
                if not len(item3[5].strip()) > 0:
                    timersErrorsList.append(item3[0] + "|" + item3[1] + "|" + item3[3] + "||" + item3[2] + "|" + item3[4] + "||||\n")

            del timersList[:]
            del timersList2[:]
            del errorsList[:]
            del errorsList2[:]

            myTimersErrorsList = list(set(timersErrorsList))
            myTimersErrorsList.sort()

            if len(myTimersErrorsList) > 0:
                with codecs.open(outFile1, "w", "utf-8", "ignore") as reportFile:
                    reportFile.writelines(myTimersErrorsList)

                del timersErrorsList[:]
                del myTimersErrorsList[:]

    except FileNotFoundError as fileError:
        pass

def readEmailLogs(searchLines, _fromDate, _toDate):
    global numOfEmailLogs
    numOfEmailLogs+=1

    outText = ""
    nonMatchedOutText = ""
    JPText = ""
    nonMatchedLine = ""
    nonMatchedLine2 = ""

    with codecs.open(tempFilePath, "a+", "utf-8", "ignore") as tempFile:
        #split the fields and rearrange them to combine them all later
        regex = re.compile(emailLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in regex.finditer(searchLines):
            iD = match.group(1)
            eSpaceName = match.group(2)
            sent = match.group(3)
            errorID = match.group(4)#null
            tenantID = match.group(5)
            from_ = match.group(6)
            to = match.group(7)
            cc = match.group(8)#null
            bcc = match.group(9)#null
            subject = match.group(10)#null
            created = match.group(11)
            activity = match.group(12)
            emailDefinition = match.group(13)
            storeContent = match.group(14)
            isTestEmail = match.group(15)
            size = match.group(16)
            messageID = match.group(17)
                                    
            date = created[0:10]
            time = created[11:19]
            time = time.replace(".", "")

            _date = datetime.strptime(date, "%Y-%m-%d").date()

            if _fromDate <= _date <= _toDate:

                if errorID == None:
                    errorID = " "

                if cc == None:
                    cc = " "

                if bcc == None:
                    bcc = " "

                if subject == None:
                    subject = " "

                espaceNamesList.append(eSpaceName + "\n")

                outText = date + " " + time + "|" + sent + "|" + errorID + "|" + from_ + "|" + to + "|" + subject + "|" + cc + "|" + bcc + "|" + eSpaceName + "|" + size + "|" + messageID + "|" + activity + "|" + emailDefinition + "|" + storeContent + "|" + isTestEmail + "|" + iD + "|" + tenantID + "\n"

                tempFile.writelines(outText)

        #capture all the lines that didn't match the regex
        negativeRegex = re.compile(negativeEmailLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in negativeRegex.finditer(searchLines):
            nonMatchedLine = match.group(1)

            nonMatchedRegex = re.search(nonMatchedEmailLogsRegex, nonMatchedLine)
            if nonMatchedRegex:
                nonMatchedHead = nonMatchedRegex.group(1)
                nonMatchedDate = nonMatchedRegex.group(2)
                nonMatchedTail = nonMatchedRegex.group(3)

                _nonMatchedDate = datetime.strptime(nonMatchedDate, "%Y-%m-%d").date()

                #check if the non-matched lines fall within the specified range
                if _fromDate <= _nonMatchedDate <= _toDate:

                    nonMatchedLine2 = nonMatchedHead + nonMatchedDate + nonMatchedTail

                    #check if the line has Japanese characters
                    hiragana = re.findall(u'[\u3040-\u309F]', nonMatchedLine2)
                    katakana = re.findall(u'[\u30A0-\u30FF]', nonMatchedLine2)
                    kanji = re.findall(u'[\u4E00-\u9FAF]', nonMatchedLine2)

                    if hiragana or katakana or kanji:
                        JPRegex = re.search(japaneseEmailLogsRegex, nonMatchedLine2)
                        if JPRegex:
                            JPiD = JPRegex.group(1)
                            JPeSpaceName = JPRegex.group(2)
                            JPsent = JPRegex.group(3)
                            JPerrorID = JPRegex.group(4)#null
                            JPtenantID = JPRegex.group(5)
                            JPfrom_ = JPRegex.group(6)
                            JPto = JPRegex.group(7)
                            JPcc = JPRegex.group(8)#null
                            JPbcc = JPRegex.group(9)#null
                            JPsubject = JPRegex.group(10)#null
                            JPcreated = JPRegex.group(11)
                            JPactivity = JPRegex.group(12)
                            JPemailDefinition = JPRegex.group(13)
                            JPstoreContent = JPRegex.group(14)
                            JPisTestEmail = JPRegex.group(15)
                            JPsize = JPRegex.group(16)
                            JPmessageID = JPRegex.group(17)
                                                    
                            JPdate = JPcreated[0:10]
                            JPtime = JPcreated[12:19]
                            JPtime = JPtime.replace(".", "")

                            if JPerrorID == None:
                                JPerrorID = " "

                            if JPcc == None:
                                JPcc = " "

                            if JPbcc == None:
                                JPbcc = " "

                            if JPsubject == None:
                                JPsubject = " "

                            espaceNamesList.append(JPeSpaceName + "\n")

                            JPText = JPdate + " " + JPtime + "|" + JPsent + "|" + JPerrorID + "|" + JPfrom_ + "|" + JPto + "|" + JPsubject + "|" + JPcc + "|" + JPbcc + "|" + JPeSpaceName + "|" + JPsize + "|" + JPmessageID + "|" + JPactivity + "|" + JPemailDefinition + "|" + JPstoreContent + "|" + JPisTestEmail + "|" + JPiD + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "EmailLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def sortEmailLogsContent(outFile1, logsFile1, logsFile2, logsFile1Regex, logsFile2Regex):
    try:
        if not os.path.exists(outFile1):
            #populate list with the email_logs file
            with codecs.open(logsFile1, "r", "utf-8", "ignore") as linesFromText:
                emailLogsList = linesFromText.readlines()

            with codecs.open(logsFile2, "r", "utf-8", "ignore") as linesFromText2:
                errorLogsList = linesFromText2.readlines()

            print("Sorting the content from the Emails")
            
            for em, eml in enumerate(emailLogsList):
                regex = re.search(logsFile1Regex, emailLogsList[em].strip())
                if regex:
                    timestamp = regex.group(1)
                    sent = regex.group(2)
                    errorID = regex.group(3)
                    from_ = regex.group(4)
                    to = regex.group(5)
                    subject = regex.group(6)
                    cc = regex.group(7)
                    bcc = regex.group(8)
                    eSpaceName = regex.group(9)
                    isTestEmail = regex.group(10)

                    if sent == "None":
                        sent = " "

                    if errorID == "None":
                        errorID == " "

                    if cc == "None":
                        cc = " "

                    if bcc == "None":
                        bcc = " "

                    if subject == "None":
                        subject = " "

                    #subtract the sentDateTime by the timestamp to get the duration
                    _timestamp = datetime.strptime(timestamp, "%Y-%m-%d %H:%M:%S")

                    if len(sent.strip()) > 0:
                        date = sent[0:10]
                        time = sent[11:19]
                        time = time.replace(".", "")
                        _datetime = date + " " + time

                        sentDateTime = datetime.strptime(_datetime, "%Y-%m-%d %H:%M:%S")

                        diffTime = sentDateTime - _timestamp
                        strDiffTime = str(diffTime)

                        #convert duration time to seconds (whole value)
                        delta = timedelta(hours=int(strDiffTime.split(":")[0]), minutes=int(strDiffTime.split(":")[1]), seconds=int(strDiffTime.split(":")[2]))
                        secs = delta.total_seconds()
                        strSecs = str(secs)
                        _seconds = strSecs.split(".")[0]

                        emailsList.append(timestamp + "|" + _seconds + "|" + eSpaceName + "|" + from_ + "|" + to + "|" + subject + "|" + cc + "|" + bcc + "|" + isTestEmail + "|" + errorID + "\n")
                        emailsList2.append(errorID + "\n")
                    else:
                        emailsList.append(timestamp + "|-1|" + eSpaceName + "|" + from_ + "|" + to + "|" + subject + "|" + cc + "|" + bcc + "|" + isTestEmail + "|" + errorID + "\n")
                        emailsList2.append(errorID + "\n")
                em+=1

            for e, err in enumerate(errorLogsList):
                regex = re.search(logsFile2Regex, errorLogsList[e].strip())
                if regex:
                    message = regex.group(1)
                    stack = regex.group(2)
                    moduleName = regex.group(3)
                    applicationName = regex.group(4)
                    environmentInformation = regex.group(5)
                    iD = regex.group(6)

                    if message == "None":
                        message == " "

                    if stack == "None":
                        stack == " "

                    if moduleName == "None":
                        moduleName == " "

                    if applicationName == "None":
                        applicationName == " "

                    if environmentInformation == "None":
                        environmentInformation == " "

                    errorsList.append(iD + "|" + message + "|" + stack + "|" + moduleName + "|" + applicationName + "|" + environmentInformation + "\n")
                    errorsList2.append(iD + "\n")
                e+=1

            del emailLogsList[:]
            del errorLogsList[:]

            for element in emailsList2:
                if len(element.strip()) > 0:
                    if element in errorsList2:
                        ind2 = errorsList2.index(element)
                        item2 = errorsList[ind2].split("|")
                        ind1 = emailsList2.index(element)
                        item1 = emailsList[ind1].split("|")
                        emailsErrorsList.append(item1[0] + "|" + item1[1] + "|" + item2[3] + "|" + item2[4] + "|" + item1[2] + "|" + item2[1] + "|" + item2[2] + "|" + item1[3] + "|" + item1[4] + "|" + item1[5] + "|" + item1[6] + "|" + item1[7] + "|" + item1[8] + "|" + item2[5].strip() + "|" + item1[9].strip() + "\n")

            for elm in emailsList:
                item3 = elm.split("|")
                if not len(item3[9].strip()) > 0:
                    emailsErrorsList.append(item3[0] + "|" + item3[1] + "|||" + item3[2] + "|||" + item3[3] + "|" + item3[4] + "|" + item3[5] + "|" + item3[6] + "|" + item3[7] + "|" + item3[8] + "||\n")

            del emailsList[:]
            del emailsList2[:]
            del errorsList[:]
            del errorsList2[:]

            myEmailErrorsList = list(set(emailsErrorsList))
            myEmailErrorsList.sort()

            if len(myEmailErrorsList) > 0:
                with codecs.open(outFile1, "w", "utf-8", "ignore") as reportFile:
                    reportFile.writelines(myEmailErrorsList)

                del emailsErrorsList[:]
                del myEmailErrorsList[:]

    except FileNotFoundError as fileError:
        pass

def readExtensionLogs(searchLines, _fromDate, _toDate):
    global numOfExtensionLogs
    numOfExtensionLogs+=1

    outText = ""
    nonMatchedOutText = ""
    JPText = ""
    nonMatchedLine = ""
    nonMatchedLine2 = ""

    with codecs.open(tempFilePath, "a+", "utf-8", "ignore") as tempFile:
        #split the fields and rearrange them to combine them all later
        regex = re.compile(extensionLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in regex.finditer(searchLines):
            tenantID = match.group(1)
            timestamp = match.group(2)
            duration = match.group(3)
            actionName = match.group(4)
            sessionID = match.group(5)
            userID = match.group(6)
            eSpaceID = match.group(7)
            extensionID = match.group(8)
            executedBy = match.group(9)
            errorID = match.group(10)#null
            requestKey = match.group(11)
            eSpaceName = match.group(12)
            extensionName = match.group(13)
            applicationName = match.group(14)
            applicationKey = match.group(15)
            username = match.group(16)#null
                                    
            date = timestamp[0:10]
            time = timestamp[11:19]
            time = time.replace(".", "")

            _date = datetime.strptime(date, "%Y-%m-%d").date()

            if _fromDate <= _date <= _toDate:

                if errorID == None:
                    errorID = " "

                if username == None:
                    username = " "

                applicationNamesList.append(applicationName + "\n")
                actionNamesList.append(actionName + "\n")
                espaceNamesList.append(eSpaceName + "\n")
                extensionNamesList.append(extensionName + "\n")

                outText = date + " " + time + "|" + duration + "|" + applicationName + "|" + applicationKey + "|" + actionName + "|" + executedBy + "|" + eSpaceName + "|" + eSpaceID + "|" + username + "|" + userID + "|" + sessionID + "|" + extensionID + "|" + extensionName + "|" + errorID + "|" + requestKey + "|" + tenantID + "\n"

                tempFile.writelines(outText)

        #capture all the lines that didn't match the regex
        negativeRegex = re.compile(negativeExtensionLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in negativeRegex.finditer(searchLines):
            nonMatchedLine = match.group(1)

            nonMatchedRegex = re.search(nonMatchedExtensionLogsRegex, nonMatchedLine)
            if nonMatchedRegex:
                nonMatchedHead = nonMatchedRegex.group(1)
                nonMatchedDate = nonMatchedRegex.group(2)
                nonMatchedTail = nonMatchedRegex.group(3)

                _nonMatchedDate = datetime.strptime(nonMatchedDate, "%Y-%m-%d").date()

                #check if the non-matched lines fall within the specified range
                if _fromDate <= _nonMatchedDate <= _toDate:

                    nonMatchedLine2 = nonMatchedHead + nonMatchedDate + nonMatchedTail

                    #check if the line has Japanese characters
                    hiragana = re.findall(u'[\u3040-\u309F]', nonMatchedLine2)
                    katakana = re.findall(u'[\u30A0-\u30FF]', nonMatchedLine2)
                    kanji = re.findall(u'[\u4E00-\u9FAF]', nonMatchedLine2)

                    if hiragana or katakana or kanji:
                        JPRegex = re.search(japaneseExtensionLogsRegex, nonMatchedLine2)
                        if JPRegex:
                            JPtenantID = JPRegex.group(1)
                            JPtimestamp = JPRegex.group(2)
                            JPduration = JPRegex.group(3)
                            JPactionName = JPRegex.group(4)
                            JPsessionID = JPRegex.group(5)
                            JPuserID = JPRegex.group(6)
                            JPeSpaceID = JPRegex.group(7)
                            JPextensionID = JPRegex.group(8)
                            JPexecutedBy = JPRegex.group(9)
                            JPerrorID = JPRegex.group(10)#null
                            JPrequestKey = JPRegex.group(11)
                            JPeSpaceName = JPRegex.group(12)
                            JPextensionName = JPRegex.group(13)
                            JPapplicationName = JPRegex.group(14)
                            JPapplicationKey = JPRegex.group(15)
                            JPusername = JPRegex.group(16)#null
                                                    
                            JPdate = JPtimestamp[0:10]
                            JPtime = JPtimestamp[11:19]
                            JPtime = JPtime.replace(".", "")

                            if JPerrorID == None:
                                JPerrorID = " "

                            if JPusername == None:
                                JPusername = " "

                            applicationNamesList.append(JPapplicationName + "\n")
                            actionNamesList.append(JPactionName + "\n")
                            espaceNamesList.append(JPeSpaceName + "\n")
                            extensionNamesList.append(JPextensionName + "\n")

                            JPText = JPdate + " " + JPtime + "|" + JPduration + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPactionName + "|" + JPexecutedBy + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPusername + "|" + JPuserID + "|" + JPsessionID + "|" + JPextensionID + "|" + JPextensionName + "|" + JPerrorID + "|" + JPrequestKey + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "ExtensionLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def sortExtensionLogsContent(outFile1, logsFile1, logsFile2, logsFile1Regex, logsFile2Regex):
    try:
        if not os.path.exists(outFile1):
            #populate list with the timer_logs file
            with codecs.open(logsFile1, "r", "utf-8", "ignore") as linesFromText:
                extensionLogsList = linesFromText.readlines()

            with codecs.open(logsFile2, "r", "utf-8", "ignore") as linesFromText2:
                errorLogsList = linesFromText2.readlines()

            print("Sorting the content from the Extensions")
            
            for x, ex in enumerate(extensionLogsList):
                regex = re.search(logsFile1Regex, extensionLogsList[x].strip())
                if regex:
                    timestamp = regex.group(1)
                    duration = regex.group(2)
                    applicationName = regex.group(3)
                    actionName = regex.group(4)
                    eSpaceName = regex.group(5)
                    extensionName = regex.group(6)
                    errorID = regex.group(7)

                    if errorID == "None":
                        errorID == " "

                    #duration is in milliseconds and it needs to be converted to seconds
                    seconds = int(duration)/1000
                    extensionList.append(timestamp + "|" + str(seconds) + "|" + applicationName + "|" + actionName + "|" + eSpaceName + "|" + extensionName + "|" + errorID + "\n")
                    extensionList2.append(errorID + "\n")
                x+=1

            for e, err in enumerate(errorLogsList):
                regex = re.search(logsFile2Regex, errorLogsList[e].strip())
                if regex:
                    message = regex.group(1)
                    stack = regex.group(2)
                    moduleName = regex.group(3)
                    environmentInformation = regex.group(4)
                    iD = regex.group(5)

                    if message == "None":
                        message == " "

                    if stack == "None":
                        stack == " "

                    if moduleName == "None":
                        moduleName == " "

                    if environmentInformation == "None":
                        environmentInformation == " "

                    errorsList.append(iD + "|" + message + "|" + stack + "|" + moduleName + "|" + environmentInformation + "\n")
                    errorsList2.append(iD + "\n")
                e+=1

            del extensionLogsList[:]
            del errorLogsList[:]

            for element in extensionList2:
                if len(element.strip()) > 0:
                    if element in errorsList2:
                        ind2 = errorsList2.index(element)
                        item2 = errorsList[ind2].split("|")
                        ind1 = extensionList2.index(element)
                        item1 = extensionList[ind1].split("|")
                        extensionErrorsList.append(item1[0] + "|" + item1[1] + "|" + item1[5] + "|" + item2[3] + "|" + item1[2] + "|" + item1[3] + "|" + item1[4] + "|" + item2[1] + "|" + item2[2] + "|" + item2[4].strip() + "|" + item1[6].strip() + "\n")

            for elm in extensionList:
                item3 = elm.split("|")
                if not len(item3[6].strip()) > 0:
                    extensionErrorsList.append(item3[0] + "|" + item3[1] + "|" + item3[5] + "||" + item3[2] + "|" + item3[3] + "|" + item3[4] + "||||\n")

            del extensionList[:]
            del extensionList2[:]
            del errorsList[:]
            del errorsList2[:]

            myExtensionErrorsList = list(set(extensionErrorsList))
            myExtensionErrorsList.sort()

            if len(myExtensionErrorsList) > 0:
                with codecs.open(outFile1, "w", "utf-8", "ignore") as reportFile:
                    reportFile.writelines(myExtensionErrorsList)

                del extensionErrorsList[:]
                del myExtensionErrorsList[:]

    except FileNotFoundError as fileError:
        pass

def readServiceActionLogs(searchLines, _fromDate, _toDate):
    global numOfServiceActionLogs
    numOfServiceActionLogs+=1

    outText = ""
    nonMatchedOutText = ""
    JPText = ""
    nonMatchedLine = ""
    nonMatchedLine2 = ""

    with codecs.open(tempFilePath, "a+", "utf-8", "ignore") as tempFile:
        #split the fields and rearrange them to combine them all later
        regex = re.compile(serviceActionLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in regex.finditer(searchLines):
            tenantID = match.group(1)
            timestamp = match.group(2)
            userID = match.group(3)
            loginID = match.group(4)#null
            sessionID = match.group(5)
            eSpaceID = match.group(6)
            errorID = match.group(7)#null
            executedBy = match.group(8)
            requestKey = match.group(9)
            entrypointName = match.group(10)#null
            actionName = match.group(11)
            duration = match.group(12)
            source = match.group(13)
            endpoint = match.group(14)
            eSpaceName = match.group(15)
            applicationName = match.group(16)
            applicationKey = match.group(17)
            username = match.group(18)#null
            originalRequestKey = match.group(19)
                                    
            date = timestamp[0:10]
            time = timestamp[11:19]
            time = time.replace(".", "")

            _date = datetime.strptime(date, "%Y-%m-%d").date()

            if _fromDate <= _date <= _toDate:

                if loginID == None:
                    loginID = " "

                if errorID == None:
                    errorID = " "

                if entrypointName == None:
                    entrypointName = " "

                if username == None:
                    username = " "

                applicationNamesList.append(applicationName + "\n")
                actionNamesList.append(actionName + "\n")
                espaceNamesList.append(eSpaceName + "\n")

                outText = date + " " + time + "|" + duration + "|" + applicationName + "|" + applicationKey + "|" + actionName + "|" + source + "|" + entrypointName + "|" + endpoint + "|" + executedBy + "|" + eSpaceName + "|" + eSpaceID + "|" + username + "|" + loginID + "|" + userID + "|" + sessionID + "|" + errorID + "|" + requestKey + "|" + originalRequestKey + "|" + tenantID + "\n"

                tempFile.writelines(outText)

        #capture all the lines that didn't match the regex
        negativeRegex = re.compile(negativeServiceActionLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in negativeRegex.finditer(searchLines):
            nonMatchedLine = match.group(1)

            nonMatchedRegex = re.search(nonMatchedServiceActionLogsRegex, nonMatchedLine)
            if nonMatchedRegex:
                nonMatchedHead = nonMatchedRegex.group(1)
                nonMatchedDate = nonMatchedRegex.group(2)
                nonMatchedTail = nonMatchedRegex.group(3)

                _nonMatchedDate = datetime.strptime(nonMatchedDate, "%Y-%m-%d").date()

                #check if the non-matched lines fall within the specified range
                if _fromDate <= _nonMatchedDate <= _toDate:

                    nonMatchedLine2 = nonMatchedHead + nonMatchedDate + nonMatchedTail

                    #check if the line has Japanese characters
                    hiragana = re.findall(u'[\u3040-\u309F]', nonMatchedLine2)
                    katakana = re.findall(u'[\u30A0-\u30FF]', nonMatchedLine2)
                    kanji = re.findall(u'[\u4E00-\u9FAF]', nonMatchedLine2)

                    if hiragana or katakana or kanji:
                        JPRegex = re.search(japaneseServiceActionLogsRegex, nonMatchedLine2)
                        if JPRegex:
                            JPtenantID = JPRegex.group(1)
                            JPtimestamp = JPRegex.group(2)
                            JPuserID = JPRegex.group(3)
                            JPloginID = JPRegex.group(4)#null
                            JPsessionID = JPRegex.group(5)
                            JPeSpaceID = JPRegex.group(6)
                            JPerrorID = JPRegex.group(7)#null
                            JPexecutedBy = JPRegex.group(8)
                            JPrequestKey = JPRegex.group(9)
                            JPentrypointName = JPRegex.group(10)#null
                            JPactionName = JPRegex.group(11)
                            JPduration = JPRegex.group(12)
                            JPsource = JPRegex.group(13)
                            JPendpoint = JPRegex.group(14)
                            JPeSpaceName = JPRegex.group(15)
                            JPapplicationName = JPRegex.group(16)
                            JPapplicationKey = JPRegex.group(17)
                            JPusername = JPRegex.group(18)#null
                            JPoriginalRequestKey = JPRegex.group(19)
                                                    
                            JPdate = JPtimestamp[0:10]
                            JPtime = JPtimestamp[11:19]
                            JPtime = JPtime.replace(".", "")

                            if JPloginID == None:
                                JPloginID = " "

                            if JPerrorID == None:
                                JPerrorID = " "

                            if JPentrypointName == None:
                                JPentrypointName = " "

                            if JPusername == None:
                                JPusername = " "

                            applicationNamesList.append(JPapplicationName + "\n")
                            actionNamesList.append(JPactionName + "\n")
                            espaceNamesList.append(JPeSpaceName + "\n")

                            JPText = JPdate + " " + JPtime + "|" + JPduration + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPactionName + "|" + JPsource + "|" + JPentrypointName + "|" + JPendpoint + "|" + JPexecutedBy + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPusername + "|" + JPloginID + "|" + JPuserID + "|" + JPsessionID + "|" + JPerrorID + "|" + JPrequestKey + "|" + JPoriginalRequestKey + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "ServiceActionLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def sortServiceActionLogsContent(outFile1, logsFile1, logsFile2, logsFile1Regex, logsFile2Regex):
    try:
        if not os.path.exists(os.getcwd() + "\\filtered_data_files\\service_action_logs_service_actions.txt"):
            #populate list with the timer_logs file
            with codecs.open(os.getcwd() + "\\filtered_data_files\\service_action_logs.txt", "r", "utf-8", "ignore") as linesFromText:
                serviceActionLogsList = linesFromText.readlines()

            with codecs.open(os.getcwd() + "\\filtered_data_files\\error_logs.txt", "r", "utf-8", "ignore") as linesFromText2:
                errorLogsList = linesFromText2.readlines()

            print("Sorting the content from the Service Actions")
            
            for s, se in enumerate(serviceActionLogsList):
                regex = re.search(logsFile1Regex, serviceActionLogsList[s].strip())
                if regex:
                    timestamp = regex.group(1)
                    duration = regex.group(2)
                    applicationName = regex.group(3)
                    actionName = regex.group(4)
                    eSpaceName = regex.group(5)
                    errorID = regex.group(6)

                    if errorID == "None":
                        errorID == " "

                    #duration is in milliseconds and it needs to be converted to seconds
                    seconds = int(duration)/1000
                    serviceActionList.append(timestamp + "|" + str(seconds) + "|" + applicationName + "|" + actionName + "|" + eSpaceName + "|" + errorID + "\n")
                    serviceActionList2.append(errorID + "\n")
                s+=1

            for e, err in enumerate(errorLogsList):
                regex = re.search(logsFile2Regex, errorLogsList[e].strip())
                if regex:
                    message = regex.group(1)
                    stack = regex.group(2)
                    moduleName = regex.group(3)
                    environmentInformation = regex.group(4)
                    iD = regex.group(5)

                    if message == "None":
                        message == " "

                    if stack == "None":
                        stack == " "

                    if moduleName == "None":
                        moduleName == " "

                    if environmentInformation == "None":
                        environmentInformation == " "

                    errorsList.append(iD + "|" + message + "|" + stack + "|" + moduleName + "|" + environmentInformation + "\n")
                    errorsList2.append(iD + "\n")
                e+=1

            del serviceActionLogsList[:]
            del errorLogsList[:]

            for element in serviceActionList2:
                if len(element.strip()) > 0:
                    if element in errorsList2:
                        ind2 = errorsList2.index(element)
                        item2 = errorsList[ind2].split("|")
                        ind1 = serviceActionList2.index(element)
                        item1 = serviceActionList[ind1].split("|")
                        serviceActionErrorsList.append(item1[0] + "|" + item1[1] + "|" + item1[3] + "|" + item2[3] + "|" + item1[2] + "|" + item1[4] + "|" + item2[1] + "|" + item2[2] + "|" + item2[4].strip() + "|" + item1[5].strip() + "\n")

            for elm in serviceActionList:
                item3 = elm.split("|")
                if not len(item3[5].strip()) > 0:
                    serviceActionErrorsList.append(item3[0] + "|" + item3[1] + "|" + item3[3] + "||" + item3[2] + "|" + item3[4] + "||||\n")

            del serviceActionList[:]
            del serviceActionList2[:]
            del errorsList[:]
            del errorsList2[:]

            myServiceActionErrorsList = list(set(serviceActionErrorsList))
            myServiceActionErrorsList.sort()

            if len(myServiceActionErrorsList) > 0:
                with codecs.open(outFile1, "w", "utf-8", "ignore") as reportFile:
                    reportFile.writelines(myServiceActionErrorsList)

                del serviceActionErrorsList[:]
                del myServiceActionErrorsList[:]

    except FileNotFoundError as fileError:
        pass

def readScreenLogs(searchLines, _fromDate, _toDate):
    global numOfScreenLogs
    numOfScreenLogs+=1

    outText = ""
    nonMatchedOutText = ""
    JPText = ""
    nonMatchedLine = ""
    nonMatchedLine2 = ""

    with codecs.open(tempFilePath, "a+", "utf-8", "ignore") as tempFile:
        #split the fields and rearrange them to combine them all later
        regex = re.compile(screenLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in regex.finditer(searchLines):
            tenantID = match.group(1)
            timestamp = match.group(2)
            duration = match.group(3)
            screen = match.group(4)
            sessionID = match.group(5)
            userID = match.group(6)
            eSpaceID = match.group(7)
            msisdn = match.group(8)#null
            screenType = match.group(9)
            executedBy = match.group(10)
            sessionBytes = match.group(11)
            viewstateBytes = match.group(12)
            sessionRequests = match.group(13)
            accessMode = match.group(14)
            requestKey = match.group(15)
            actionName = match.group(16)#null
            clientIP = match.group(17)
            eSpaceName = match.group(18)
            applicationName = match.group(19)
            applicationKey = match.group(20)
                            
            date = timestamp[0:10]
            time = timestamp[11:19]
            time = time.replace(".", "")

            _date = datetime.strptime(date, "%Y-%m-%d").date()

            if _fromDate <= _date <= _toDate:

                if msisdn == None:
                    msisdn = " "

                if actionName == None:
                    actionName = " "

                applicationNamesList.append(applicationName + "\n")
                actionNamesList.append(actionName + "\n")
                espaceNamesList.append(eSpaceName + "\n")

                outText = date + " " + time + "|" + duration + "|" + screen + "|" + screenType + "|" + applicationName + "|" + applicationKey + "|" + actionName + "|" + accessMode + "|" + executedBy + "|" + clientIP + "|" + eSpaceName + "|" + eSpaceID + "|" + userID + "|" + sessionID + "|" + sessionRequests + "|" + sessionBytes + "|" + viewstateBytes + "|" + msisdn + "|" + requestKey + "|" + tenantID + "\n"

                tempFile.writelines(outText)

        #capture all the lines that didn't match the regex
        negativeRegex = re.compile(negativeScreenLogsRegex, re.MULTILINE + re.IGNORECASE)
        for match in negativeRegex.finditer(searchLines):
            nonMatchedLine = match.group(1)

            nonMatchedRegex = re.search(nonMatchedScreenLogsRegex, nonMatchedLine)
            if nonMatchedRegex:
                nonMatchedHead = nonMatchedRegex.group(1)
                nonMatchedDate = nonMatchedRegex.group(2)
                nonMatchedTail = nonMatchedRegex.group(3)

                _nonMatchedDate = datetime.strptime(nonMatchedDate, "%Y-%m-%d").date()

                #check if the non-matched lines fall within the specified range
                if _fromDate <= _nonMatchedDate <= _toDate:

                    nonMatchedLine2 = nonMatchedHead + nonMatchedDate + nonMatchedTail

                    #check if the line has Japanese characters
                    hiragana = re.findall(u'[\u3040-\u309F]', nonMatchedLine2)
                    katakana = re.findall(u'[\u30A0-\u30FF]', nonMatchedLine2)
                    kanji = re.findall(u'[\u4E00-\u9FAF]', nonMatchedLine2)

                    if hiragana or katakana or kanji:
                        JPRegex = re.search(japaneseScreenLogsRegex, nonMatchedLine2)
                        if JPRegex:
                            JPtenantID = JPRegex.group(1)
                            JPtimestamp = JPRegex.group(2)
                            JPduration = JPRegex.group(3)
                            JPscreen = JPRegex.group(4)
                            JPsessionID = JPRegex.group(5)
                            JPuserID = JPRegex.group(6)
                            JPeSpaceID = JPRegex.group(7)
                            JPmsisdn = JPRegex.group(8)#null
                            JPscreenType = JPRegex.group(9)
                            JPexecutedBy = JPRegex.group(10)
                            JPsessionBytes = JPRegex.group(11)
                            JPviewstateBytes = JPRegex.group(12)
                            JPsessionRequests = JPRegex.group(13)
                            JPaccessMode = JPRegex.group(14)
                            JPrequestKey = JPRegex.group(15)
                            JPactionName = JPRegex.group(16)#null
                            JPclientIP = JPRegex.group(17)
                            JPeSpaceName = JPRegex.group(18)
                            JPapplicationName = JPRegex.group(19)
                            JPapplicationKey = JPRegex.group(20)
                                            
                            JPdate = JPtimestamp[0:10]
                            JPtime = JPtimestamp[11:19]
                            JPtime = JPtime.replace(".", "")

                            if JPmsisdn == None:
                                JPmsisdn = " "

                            if JPactionName == None:
                                JPactionName = " "

                            applicationNamesList.append(JPapplicationName + "\n")
                            actionNamesList.append(JPactionName + "\n")
                            espaceNamesList.append(JPeSpaceName + "\n")

                            JPText = JPdate + " " + JPtime + "|" + JPduration + "|" + JPscreen + "|" + JPscreenType + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPactionName + "|" + JPaccessMode + "|" + JPexecutedBy + "|" + JPclientIP + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPuserID + "|" + JPsessionID + "|" + JPsessionRequests + "|" + JPsessionBytes + "|" + JPviewstateBytes + "|" + JPmsisdn + "|" + JPrequestKey + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "ScreenLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def sortScreenLogsContent(outFile1, logsFile1, logsFile1Regex):
    try:
        if not os.path.exists(outFile1):
            #populate list with the screen_logs file
            with codecs.open(logsFile1, "r", "utf-8", "ignore") as linesFromText:
                screenLogsList = linesFromText.readlines()

            print("Sorting the content from the Screens")
            
            for s, sc in enumerate(screenLogsList):
                regex = re.search(logsFile1Regex, screenLogsList[s].strip())
                if regex:
                    timestamp = regex.group(1)
                    duration = regex.group(2)
                    screen = regex.group(3)
                    screenType = regex.group(4)
                    applicationName = regex.group(5)
                    actionName = regex.group(6)
                    eSpaceName = regex.group(7)

                    if actionName == "None":
                        actionName == " "

                    #duration is in milliseconds and it needs to be converted to seconds
                    seconds = int(duration)/1000
                    screensList.append(timestamp + "|" + str(seconds) + "|" + screen + "|" + screenType + "|" + applicationName + "|" + actionName + "|" + eSpaceName + "\n")
                s+=1

            del screenLogsList[:]

            myScreensList = list(set(screensList))
            myScreensList.sort()

            if len(myScreensList) > 0:
                with codecs.open(outFile1, "w", "utf-8", "ignore") as reportFile:
                    reportFile.writelines(myScreensList)

                del screensList[:]
                del myScreensList[:]

    except FileNotFoundError as fileError:
        pass

def readBPTTroubleshootingReportLogs(searchLines, _fromDate, _toDate):
    outText = ""
    newOutText = ""
    nonMatchedOutText = ""
    nonMatchedLine = ""
    nonMatchedLine2 = ""

    with codecs.open(tempFilePath, "a+", "utf-8", "ignore") as tempFile:
        #split the fields and rearrange them to combine them all later
        regex = re.compile(bptTroubleshootingReportsRegex, re.MULTILINE + re.IGNORECASE)
        for match in regex.finditer(searchLines):
            processID = match.group(1)
            processStatus = match.group(2)
            eSpaceName = match.group(3)
            processName = match.group(4)
            parentProcessID = match.group(5)
            timestamp = match.group(6)
            processLastModified = match.group(7)
            processSuspendedDate = match.group(8)#null
            activityName = match.group(9)
            activityStatus = match.group(10)
            activityErrorCount = match.group(11)
            activityErrorID = match.group(12)#null
            activityKind = match.group(13)
            activityCreated = match.group(14)
            activityClosed = match.group(15)#null
            activityRunningSince = match.group(16)#null
            activityNextRun = match.group(17)#null
                            
            date = timestamp[0:10]
            time = timestamp[11:19]
            time = time.replace(".", "")

            _date = datetime.strptime(date, "%Y-%m-%d").date()

            if _fromDate <= _date <= _toDate:

                if processSuspendedDate == None or processSuspendedDate == "None":
                    processSuspendedDate = " "

                if activityErrorID == None:
                    activityErrorID = " "

                if activityClosed == None or activityClosed == "None":
                    activityClosed = " "
                else:
                    activityClosed = activityClosed.replace("|", "")

                if activityRunningSince == None or activityRunningSince == "|None":
                    activityRunningSince = " "
                else:
                    activityRunningSince = activityRunningSince.replace("|", "")

                if activityNextRun == None or activityNextRun == "|None":
                    activityNextRun = " "
                else:
                    activityNextRun = activityNextRun.replace("|", "")

                outText = date + " " + time + "|" + eSpaceName + "|" + processName + "|" + processStatus + "|" + processLastModified + "|" + processSuspendedDate + "|" + processID + "|" + parentProcessID + "|" + activityCreated + "|" + activityName + "|" + activityKind + "|" + activityStatus + "|" + activityRunningSince + "|" + activityNextRun + "|" + activityClosed + "|" + activityErrorCount + "|" + activityErrorID

                if outText[-1] == "|":
                    newOutText = outText[:-1]
                    tempFile.writelines(newOutText + "\n")
                else:
                    tempFile.writelines(outText + "\n")

        #capture all the lines that didn't match the regex
        negativeRegex = re.compile(negativeBptTroubleshootingReportsRegex, re.MULTILINE + re.IGNORECASE)
        for match in negativeRegex.finditer(searchLines):
            nonMatchedLine = match.group(1)

            nonMatchedRegex = re.search(nonMatchedBptTroubleshootingReportsRegex, nonMatchedLine)
            if nonMatchedRegex:
                nonMatchedHead = nonMatchedRegex.group(1)
                nonMatchedDate = nonMatchedRegex.group(2)
                nonMatchedTail = nonMatchedRegex.group(3)

                _nonMatchedDate = datetime.strptime(nonMatchedDate, "%Y-%m-%d").date()

                #check if the non-matched lines fall within the specified range
                if _fromDate <= _nonMatchedDate <= _toDate:
                    nonMatchedLine2 = nonMatchedHead + nonMatchedDate + nonMatchedTail

                    nonMatchedOutText = "BPTTroubleshootingReport -> " + nonMatchedLine2 + "\n"

                    myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def readiOSAndroidLogs(filename, searchLines, _fromDate, _toDate):
    if "iosbuildlog" in filename.lower():
        global numOfiOSLogs
        numOfiOSLogs+=1
    elif "androidbuildlog" in filename.lower():
        global numOfAndroidLogs
        numOfAndroidLogs+=1

    #split the fields and rearrange them to combine them all later
    regex = re.compile(androidiOSBuildLogsRegex, re.MULTILINE + re.IGNORECASE)
    for match in regex.finditer(searchLines):
        date = match.group(1)
        time = match.group(2)
        messageType = match.group(3)
        method = match.group(4)#null
        message = match.group(5)#null

        _time = time[:8]

        _date = datetime.strptime(date, "%Y-%m-%d").date()
                    
        if _fromDate <= _date <= _toDate:

            if not method == None:
                method = method.replace("[", "")
                method = method.replace("]", "")
            else:
                method = " "

            if message == None:
                message = " "

            if "iosbuildlog" in filename.lower():
                outText = date + " " + _time + "|" + messageType + "|" + method + "|" + message + "\n"
            elif "androidbuildlog" in filename.lower():
                outText = date + " " + _time + "|" + messageType + "|" + method + "|" + message + "\n"

            myLinesFromDateRange.append(outText)

    #capture all the lines that didn't match the regex
    negativeRegex = re.compile(negativeAndroidiOSBuildLogsRegex, re.MULTILINE + re.IGNORECASE)
    for match in negativeRegex.finditer(searchLines):
        nonMatchedLine = match.group(1)

        nonMatchedRegex = re.search(nonMatchedAndroidiOSBuildLogsRegex, nonMatchedLine)
        if nonMatchedRegex:
            nonMatchedDate = nonMatchedRegex.group(1)

            _nonMatchedDate = datetime.strptime(nonMatchedDate, "%Y-%m-%d").date()

            #check if the non-matched lines fall within the specified range
            if _fromDate <= _nonMatchedDate <= _toDate:

                #check if the line has Japanese characters
                hiragana = re.findall(u'[\u3040-\u309F]', nonMatchedLine)
                katakana = re.findall(u'[\u30A0-\u30FF]', nonMatchedLine)
                kanji = re.findall(u'[\u4E00-\u9FAF]', nonMatchedLine)

                if hiragana or katakana or kanji:
                    JPRegex = re.search(japaneseAndroidiOSBuildLogsRegex, nonMatchedLine)
                    if JPRegex:
                        JPDate = JPRegex.group(1)
                        JPTime = JPRegex.group(2)
                        JPTail = JPRegex.group(3)

                        _JPTime = JPTime[:8]

                        if "iosbuildlog" in filename.lower():
                            JPText = JPDate + " " + _JPTime + "|||" + JPTail + "\n"
                        elif "androidbuildlog" in filename.lower():
                            JPText = JPDate + " " + _JPTime + "|||" + JPTail + "\n"

                        myLinesFromDateRange.append(JPText)

                else:
                    newNonMatchedLine = normalizeLines(nonMatchedLine)
                    newNonMatchedLine = newNonMatchedLine.translate(replacementDict)

                    #check if the line matches the regular expression
                    regex2 = re.search(androidiOSBuildLogsRegex, newNonMatchedLine)
                    if regex2:
                        date = regex2.group(1)
                        time = regex2.group(2)
                        messageType = regex2.group(3)
                        method = regex2.group(4)#null
                        message = regex2.group(5)#null

                        _time = time[:8]

                        if not method == None:
                            method = method.replace("[", "")
                            method = method.replace("]", "")
                        else:
                            method = " "

                        if message == None:
                            message = " "

                        if "iosbuildlog" in filename.lower():
                            outText = date + " " + _time + "|" + messageType + "|" + method + "|" + message + "\n"
                        elif "androidbuildlog" in filename.lower():
                            outText = date + " " + _time + "|" + messageType + "|" + method + "|" + message + "\n"

                        myLinesFromDateRange.append(outText)
                                    
                    if not regex2:
                        if "iosbuildlog" in filename.lower():
                            nonMatchedOutText = "iOSBuildLog -> " + newNonMatchedLine + "\n"
                        elif "androidbuildlog" in filename.lower():
                            nonMatchedOutText = "AndroidBuildLog -> " + newNonMatchedLine + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def readServiceStudioReportLogs(searchLines3, _fromDate, _toDate):
    global numOfServiceStudioReports
    numOfServiceStudioReports+=1

    #split the fields and rearrange them to combine them all later   
    regex1 = re.findall(serviceStudioReportsDetailsRegex, searchLines3)
    if regex1:
        findAllString = ' '.join([str(elm) for elm in regex1])
        findAllString = findAllString.replace("|", "")
        findAllString = findAllString.replace("('", "")
        findAllString = findAllString.replace("')", "")
        findAllString = findAllString.replace("', '", " ")
        outText = fromDate + " 00:00:00||" + findAllString + "\n"
        myLinesFromDateRange.append(outText)

    searchLines3 = re.sub(serviceStudioReportsOperationsLogsRegex0, "|" + r"\1", searchLines3)
            
    regex2 = re.findall(serviceStudioReportsOperationsLogsRegex1, searchLines3)
    if regex2:
        findAllString2 = '\n'.join([str(elm2) for elm2 in regex2])
        findAllString2 = findAllString2.replace("|", "")
        findAllString2 = findAllString2.replace("('", "")
        findAllString2 = findAllString2.replace("')", "")
        findAllString2 = findAllString2.replace("\")", "")
        findAllString2 = findAllString2.replace("', '", "|")
        findAllString2 = findAllString2.replace("', \"", "|")

        with codecs.open(tempFilePath, "w", "utf-8", "ignore") as tempFile:
            tempFile.writelines(findAllString2)

        with codecs.open(tempFilePath, "r", "utf-8", "ignore") as tempFile2:
            _searchLines = tempFile2.read()
            regex3 = re.compile(serviceStudioReportsOperationsLogsRegex2, re.MULTILINE + re.IGNORECASE)
            for match3 in regex3.finditer(_searchLines):
                date = match3.group(1)
                time = match3.group(2)
                messageType = match3.group(3)#null
                message = match3.group(4)#null

                #reformat the dates
                year = date.split("-")[2]
                month = date.split("-")[0]
                day = date.split("-")[1]

                if len(month) != 2:
                    month = "0" + month

                _date = year + "-" + month + "-" + day

                #reformat the times
                if time[-2:] == "AM" and time[:2] == "12":
                    _time = "00" + time[2:-2]
                elif time[-2:] == "AM":
                    hours = time.split(":")[0]
                    minutes = time.split(":")[1]
                    seconds = time.split(":")[2]
                    if len(hours) < 2:
                        hours = "0" + hours
                        _time = hours + ":" + minutes + ":" + seconds[:-2]
                elif time[-2:] == "PM" and time[:2] == "12":
                    _time = time[:-2]
                elif time[-2:] == "PM":
                    hours = time.split(":")[0]
                    minutes = time.split(":")[1]
                    seconds = time.split(":")[2]
                    _time = str(int(hours) + 12) + ":" + minutes + ":" + seconds[:-2]

                _date_ = datetime.strptime(_date, "%Y-%m-%d").date()
                                                
                if _fromDate <= _date_ <= _toDate:

                    if messageType == None:
                        messageType = " "

                    if message == None:
                        message = " "

                    outText2 = _date + " " + _time.strip() + "|" + messageType + "|" + message + "\n"
                    myLinesFromDateRange.append(outText2)

            #capture all the lines that didn't match the regex
            negativeRegex = re.compile(negativeServiceStudioReportsOperationsLogsRegex, re.MULTILINE + re.IGNORECASE)
            for match in negativeRegex.finditer(_searchLines):
                nonMatchedLine = match.group(1)

                nonMatchedRegex = re.search(nonMatchedServiceStudioReportsOperationsLogsRegex, nonMatchedLine)
                if nonMatchedRegex:
                    nonMatchedDate = nonMatchedRegex.group(1)
                    nonMatchedTail = nonMatchedRegex.group(2)

                    #reformat the dates
                    nonMatchedYear = nonMatchedDate.split("-")[2]
                    nonMatchedMonth = nonMatchedDate.split("-")[0]
                    nonMatchedDay = nonMatchedDate.split("-")[1]

                    if len(nonMatchedMonth) != 2:
                        nonMatchedMonth = "0" + nonMatchedMonth

                    _nonMatchedDate = nonMatchedYear + "-" + nonMatchedMonth + "-" + nonMatchedDay

                    _nonMatchedDate_ = datetime.strptime(_nonMatchedDate, "%Y-%m-%d").date()

                    #check if the non-matched lines fall within the specified range
                    if _fromDate <= _nonMatchedDate_ <= _toDate:

                        nonMatchedLine2 = nonMatchedDate + " " + nonMatchedTail

                        #check if the line has Japanese characters
                        hiragana = re.findall(u'[\u3040-\u309F]', nonMatchedLine2)
                        katakana = re.findall(u'[\u30A0-\u30FF]', nonMatchedLine2)
                        kanji = re.findall(u'[\u4E00-\u9FAF]', nonMatchedLine2)

                        if hiragana or katakana or kanji:
                            JPRegex = re.search(japaneseServiceStudioReportsOperationsLogsRegex, nonMatchedLine2)
                            if JPRegex:
                                JPDate = JPRegex.group(1)
                                JPTime = JPRegex.group(2)
                                JPTail = JPRegex.group(3)

                                #reformat the dates
                                year = JPDate.split("-")[2]
                                month = JPDate.split("-")[0]
                                day = JPDate.split("-")[1]

                                if len(month) != 2:
                                    month = "0" + month

                                _JPDate = year + "-" + month + "-" + day

                                #reformat the times
                                if JPTime[-2:] == "AM" and JPTime[:2] == "12":
                                    _JPTime = "00" + JPTime[2:-2]
                                elif JPTime[-2:] == "AM":
                                    hours = JPTime.split(":")[0]
                                    minutes = JPTime.split(":")[1]
                                    seconds = JPTime.split(":")[2]
                                    if len(hours) < 2:
                                        hours = "0" + hours
                                    _JPTime = hours + ":" + minutes + ":" + seconds[:-2]
                                elif JPTime[-2:] == "PM" and JPTime[:2] == "12":
                                    _JPTime = JPTime[:-2]
                                elif JPTime[-2:] == "PM":
                                    hours = JPTime.split(":")[0]
                                    minutes = JPTime.split(":")[1]
                                    seconds = JPTime.split(":")[2]
                                    _JPTime = str(int(hours) + 12) + ":" + minutes + ":" + seconds[:-2]

                                JPText = _JPDate + " " + _JPTime + "||" + JPTail.strip() + "\n"

                                myLinesFromDateRange.append(JPText)

                        else:
                            nonMatchedOutText = "ServiceStudioReport -> " + nonMatchedLine2.strip() + "\n"

                            myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

        #delete the temporary file that was created
        os.remove(tempFilePath)

def readGeneralTextLogs(searchLines, _fromDate, _toDate):
    global numOfGeneralTXTLogs
    numOfGeneralTXTLogs+=1

    #split the fields and rearrange them to combine them all later
    searchLines2 = normalizeLines(searchLines)
    searchLines2 = searchLines2.replace("LoaderExceptions:", "LoaderException:")
    searchLines2 = searchLines2.translate(replacementDict)
    searchLines3 = '\n'.join(searchLines2.split("Exceptions: "))
    
    regex = re.compile(generalTextLogsRegex, re.MULTILINE + re.IGNORECASE)
    for match in regex.finditer(searchLines):
        messageType = match.group(1)
        time = match.group(2)
        message = match.group(3)

        if not "exceptions" == messageType.lower():
            _time = time[:8]

            outText = fromDate + " " + _time + "|" + messageType + "|" + message + "\n"

            myLinesFromDateRange.append(outText)

    exceptionsRegex = re.compile(generalTextLogsRegex2, re.MULTILINE + re.IGNORECASE)
    for match2 in exceptionsRegex.finditer(searchLines3):
        time = match2.group(1)
        message = match2.group(2)

        _time = time[:8]
        
        outText2 = fromDate + " " + _time + "|Exceptions|" + message + "\n"

        myLinesFromDateRange.append(outText2)

def readFullErrorDumpLogs(searchLines):
    #split the fields and rearrange them to combine them all later
    regex = re.compile(fullErrorDumpLogsRegex, re.MULTILINE + re.IGNORECASE)
    for match in regex.finditer(searchLines):
        information = match.group(1)
        details = match.group(2)

        outText = information + "\n" + details

        myLinesFromDateRange.append(outText.strip())

def parseXMLtoString(lineToXML, xmlPath):
    xmlVar = etree.XPath(xmlPath)
    newXMLVar = " ".join(xmlVar(lineToXML))
    return newXMLVar

def xlsxFile(absolutePath, relativePath, filename, ext, _fromDate, _toDate):
    #convert the XLSX file to TXT to manipulate the data in an easier way
    print("Converting " + filename + ".xlsx to .txt format")
    
    xlsx = openpyxl.load_workbook(absolutePath, read_only = True)
    sheet = xlsx.active
    data = sheet.rows
    #convert the "data" generator to a list
    dataList = list(data)
    #remove the column names from the data
    del dataList[0]

    with codecs.open(relativePath + "\\" + filename + ext, "w", "utf-8", "ignore") as linesFromXLSX:
        for row in dataList:
            rowList = list(row)
            for i in range(len(rowList)):
                if i == len(rowList) - 1:
                    line = str(rowList[i].value)
                    newLine = normalizeLines(line)
                    newLine = newLine.translate(replacementDict)
                    linesFromXLSX.write(newLine)
                    linesFromXLSX.write("\n")
                else:
                    line = str(rowList[i].value)
                    newLine = normalizeLines(line)
                    newLine = newLine.translate(replacementDict)
                    linesFromXLSX.write(newLine + "|")

    if "infrastructurereport" in absolutePath.lower() or "stagingreport" in absolutePath.lower() or "userpermissionsreport" in absolutePath.lower():
        createFolder("\\filtered_data_files\\")

        regex = re.match("^([a-zA-Z]+)(?:[\_\d]+)?", filename)
        if regex:
            file_name = regex.group(1)

            if "EnvironmentCapabilities" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\environment_capabilities.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "Environments" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\environments.txt")
                os.remove(relativePath + "\\" + filename + ext)

            elif "Roles" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\roles.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "RolesInApplications" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\roles_in_applications.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "RolesInTeams" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\roles_in_teams.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "SyncErrors" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\sync_errors.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "User" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\user.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "UserPools" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\user_pools.txt")
                os.remove(relativePath + "\\" + filename + ext)

            elif "Application" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\application.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "ApplicationVersion" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\application_version.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "ApplicationVersionModuleVersion" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\application_version_module_version.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "ChangeLog" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\change_log.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "ConsumerElements" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\consumer_elements.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "EntityConfigurations" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\entity_configurations.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "EnvironmentApplicationCache" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\environment_application_cache.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "EnvironmentApplicationModule" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\environment_application_module.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "EnvironmentAppVersion" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\environment_app_version.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "EnvironmentModuleCache" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\environment_module_cache.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "EnvironmentModuleRunning" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\environment_module_running.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "Modules" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\modules.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "ModuleVersionReferences" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\module_version_references.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "ProducerElements" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\producer_elements.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "SiteProperties" in file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\site_properties.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "Staging" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\staging.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "StagingApplicationVersion" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\staging_application_version.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "StagingMessage" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\staging_message.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "StagingModuleInconsistency" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\staging_module_inconsistency.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "StagingModuleVersion" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\staging_module_version.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "StagingModuleVersionToPublish" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\staging_module_version_to_publish.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "StagingModuleVersionToUpload" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\staging_module_version_to_upload.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "StagingOption" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\staging_option.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "StagingOutdatedApplication" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\staging_outdated_application.txt")
                os.remove(relativePath + "\\" + filename + ext)
            elif "StagingOutdatedModule" == file_name:
                shutil.copyfile(relativePath + "\\" + filename + ext, os.getcwd() + "\\filtered_data_files\\staging_outdated_module.txt")
                os.remove(relativePath + "\\" + filename + ext)

            print("Closing: " + filename + ".txt")

    else:
        xlsxtxtFile(relativePath + "\\" + filename + ext, filename, ext, _fromDate, _toDate)

def xlsxtxtFile(absolutePath, filename, ext, _fromDate, _toDate):
    print("Reading: " + filename + ext)

    outText = ""
    nonMatchedOutText = ""
    JPText = ""
    nonMatchedLine = ""
    nonMatchedLine2 = ""
    cycleNum = 0

    with codecs.open(absolutePath, "r", "utf-8", "ignore") as max_lines:
        #total number of lines from the data
        maxLines = sum(1 for line in max_lines)-1
        #for large files, split the data into several files
        if maxLines >= numOfLines:
            createFolder("\\split_data\\")
            cycleNum = splitFiles(numOfLines, constant, maxLines, cycleNum, absolutePath, ext, os.getcwd() + "\\split_data\\file_")

    if maxLines >= numOfLines:
        readSplitFilesXLSX(cycleNum, filename, ext, _fromDate, _toDate)

        if os.path.exists(tempFilePath):
            with codecs.open(tempFilePath, "r", "utf-8", "ignore") as tempFile2:
                myLinesFromDateRange = tempFile2.readlines()
        else:
            print("The file \"" + filename + ext + "\" could not be read.")
            myLinesFromDateRange = ['']
            del myLinesFromDateRange[:]

        #delete the temporary files and directory that were created
        os.remove(absolutePath)
        shutil.rmtree(os.getcwd() + "\\split_data")

        try:
            os.remove(tempFilePath)
        except FileNotFoundError as fileError:
            pass

    else:
        try:
            with codecs.open(absolutePath, "r", "utf-8", "ignore") as linesFromText:
                searchLines = linesFromText.read()
                if "errorlog" in filename.lower():
                    readErrorLogs(searchLines, _fromDate, _toDate)
                elif "generallog" in filename.lower():
                    readGeneralLogs(searchLines, _fromDate, _toDate)
                elif "integrationslog" in filename.lower():
                    readIntegrationsLogs(searchLines, _fromDate, _toDate)
                elif "mobilerequestslog" in filename.lower():
                    readMobileRequestsLogs(searchLines, _fromDate, _toDate)
                elif "timerlog" in filename.lower():
                    readTimerLogs(searchLines, _fromDate, _toDate)
                elif "emaillog" in filename.lower():
                    readEmailLogs(searchLines, _fromDate, _toDate)
                elif "extensionlog" in filename.lower():
                    readExtensionLogs(searchLines, _fromDate, _toDate)
                elif "serviceactionlog" in filename.lower():
                    readServiceActionLogs(searchLines, _fromDate, _toDate)
                elif "screenlog" in filename.lower():
                    readScreenLogs(searchLines, _fromDate, _toDate)
                elif "troubleshootingreport" in filename.lower():
                    readBPTTroubleshootingReportLogs(searchLines, _fromDate, _toDate)
        except ValueError as valError:
            print("\nThe customer altered the original Excel spreadsheet.\n\nPossible reasons:\n1- The customer added a column on the spreadsheet.\n" +
                  "2- The customer switched the columns of the spreadsheet.\n3- The customer modified the dates on the spreadsheet.\n")

        if os.path.exists(tempFilePath):
            with codecs.open(tempFilePath, "r", "utf-8", "ignore") as tempFile2:
                myLinesFromDateRange = tempFile2.readlines()
        else:
            print("The file \"" + filename + ext + "\" could not be read.")
            myLinesFromDateRange = ['']
            del myLinesFromDateRange[:]

        #delete the temporary files that were created
        os.remove(absolutePath)

        try:
            os.remove(tempFilePath)
        except FileNotFoundError as fileError:
            pass

    if len(deviceInformationList) > 0:
        createFolder("\\filtered_data_files\\")
        deviceInformationList.sort()
        #remove duplicate records from the list
        myDeviceInformationList = list(set(deviceInformationList))
        myDeviceInformationList.sort()

        if len(androidOSVersionList) > 0:
            print("Sorting the content from the Android versions")
            
            androidOSVersionList.sort()
            androidOccurrences = Counter(androidOSVersionList)
            #remove duplicate records from the list
            myAndroidOSVersionList = list(set(androidOSVersionList))
            myAndroidOSVersionList.sort()

            for element in myDeviceInformationList:
                item1 = element.split("|")
                if item1[1] in myAndroidOSVersionList:
                    ind2 = myAndroidOSVersionList.index(item1[1])
                    androidOSVersionOccurrencesList.append(item1[0] + "|" + item1[1] + "|" + str(androidOccurrences[myAndroidOSVersionList[ind2]]) + "|" + item1[2] + "|" + item1[3] + "|" + item1[4].strip() + "\n")

            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\device_information.txt", androidOSVersionOccurrencesList, myandroidOSVersionOccurrencesList)
            if numOfErrorLogs > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\device_information.txt", androidOSVersionOccurrencesList, myandroidOSVersionOccurrencesList)

            del deviceInformationList[:]
            del myDeviceInformationList[:]
            del androidOSVersionList[:]
            del myAndroidOSVersionList[:]

        if len(iosOSVersionList) > 0:
            print("Sorting the content from the iOS versions")
            
            iosOSVersionList.sort()
            iOSOccurrences = Counter(iosOSVersionList)
            #remove duplicate records from the list
            myiOSVersionList = list(set(iosOSVersionList))
            myiOSVersionList.sort()

            for element in myDeviceInformationList:
                item1 = element.split("|")
                if item1[1] in myiOSVersionList:
                    ind2 = myiOSVersionList.index(item1[1])
                    iOSOSVersionOccurrencesList.append(item1[0] + "|" + item1[1] + "|" + str(iOSOccurrences[myiOSVersionList[ind2]]) + "|" + item1[2] + "|" + item1[3] + "|" + item1[4].strip() + "\n")

            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\device_information.txt", iOSOSVersionOccurrencesList, myiOSOSVersionOccurrencesList)
            if numOfErrorLogs > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\device_information.txt", iOSOSVersionOccurrencesList, myiOSOSVersionOccurrencesList)

            del deviceInformationList[:]
            del myDeviceInformationList[:]
            del iosOSVersionList[:]
            del myiOSVersionList[:]

    if len(myNonMatchedValidLinesFromDateRange) > 0:
        createFolder("\\nonmatched_valid_lines\\")
        with codecs.open(nonMatchedPath, "a", "utf-8", "ignore") as linesFromDateRange2:
            linesFromDateRange2.writelines(myNonMatchedValidLinesFromDateRange)
        del myNonMatchedValidLinesFromDateRange[:]

    if len(myLinesFromDateRange) > 0:
        createFolder("\\filtered_data_files\\")
        if "ErrorLog" in filename:
            outFilename = "error_logs" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
            if numOfErrorLogs > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
        elif "GeneralLog" in filename:
            outFilename = "general_logs" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
            if numOfGeneralLogs > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
        elif "IntegrationsLog" in filename:
            outFilename = "integrations_logs" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
            if numOfIntegrationsLogs > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
        elif "TimerLog" in filename:
            outFilename = "timer_logs" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
            if numOfTimerLogs > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
        elif "EmailLog" in filename:
            outFilename = "email_logs" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
            if numOfEmailLogs > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
        elif "ExtensionLog" in filename:
            outFilename = "extension_logs" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
            if numOfExtensionLogs > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
        elif "MobileRequestsLog" in filename:
            outFilename = "mobile_requests_logs" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
            if numOfMobileRequestsLogs > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
        elif "ServiceActionLog" in filename:
            outFilename = "service_action_logs" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
            if numOfServiceActionLogs > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
        elif "ScreenLog" in filename:
            outFilename = "screen_logs" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
            if numOfScreenLogs > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
        elif "troubleshootingreport" in filename.lower():
            outFilename = "bpt_troubleshootingreport_logs" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
    else:
        print("No data was found within the specified date range.")

    #read the content from the filtered files to create the detailed tables
    sortGeneralLogsContent(os.getcwd() + "\\filtered_data_files\\general_logs_slowsql.txt", os.getcwd() + "\\filtered_data_files\\general_logs_slowextension.txt", os.getcwd() + "\\filtered_data_files\\general_logs.txt", os.getcwd() + "\\filtered_data_files\\error_logs.txt", generalLogsContentRegex, errorLogsContentRegex)
    sortIntegrationsLogsContent(os.getcwd() + "\\filtered_data_files\\integrations_logs_webservices.txt", os.getcwd() + "\\filtered_data_files\\integrations_logs.txt", os.getcwd() + "\\filtered_data_files\\error_logs.txt", integrationsLogsContentRegex, errorLogsContentRegex)
    sortMobileRequestsLogsContent(os.getcwd() + "\\filtered_data_files\\mobile_requests_logs_screens.txt", os.getcwd() + "\\filtered_data_files\\mobile_requests_logs.txt", os.getcwd() + "\\filtered_data_files\\error_logs.txt", mobileRequestsLogsContentRegex, errorLogsContentRegex)
    sortTimerLogsContent(os.getcwd() + "\\filtered_data_files\\timer_logs_timers.txt", os.getcwd() + "\\filtered_data_files\\timer_logs.txt", os.getcwd() + "\\filtered_data_files\\error_logs.txt", timerLogsContentRegex, errorLogsContentRegex)
    sortEmailLogsContent(os.getcwd() + "\\filtered_data_files\\email_logs_emails.txt", os.getcwd() + "\\filtered_data_files\\email_logs.txt", os.getcwd() + "\\filtered_data_files\\error_logs.txt", emailLogsContentRegex, errorLogsContentRegex2)
    sortExtensionLogsContent(os.getcwd() + "\\filtered_data_files\\extension_logs_extensions.txt", os.getcwd() + "\\filtered_data_files\\extension_logs.txt", os.getcwd() + "\\filtered_data_files\\error_logs.txt", extensionLogsContentRegex, errorLogsContentRegex)
    sortServiceActionLogsContent(os.getcwd() + "\\filtered_data_files\\service_action_logs_service_actions.txt", os.getcwd() + "\\filtered_data_files\\service_action_logs.txt", os.getcwd() + "\\filtered_data_files\\error_logs.txt", serviceActionLogsContentRegex, errorLogsContentRegex)
    sortScreenLogsContent(os.getcwd() + "\\filtered_data_files\\screen_logs_screens.txt", os.getcwd() + "\\filtered_data_files\\screen_logs.txt", screenLogsContentRegex)

    #create files to be used as filters
    cleanListFunc(os.getcwd() + "\\filtered_data_files\\filter_module_names.txt", moduleNamesList, myModuleNamesList)
    cleanListFunc(os.getcwd() + "\\filtered_data_files\\filter_application_names.txt", applicationNamesList, myApplicationNamesList)
    cleanListFunc(os.getcwd() + "\\filtered_data_files\\filter_action_names.txt", actionNamesList, myActionNamesList)
    cleanListFunc(os.getcwd() + "\\filtered_data_files\\filter_extension_names.txt", extensionNamesList, myExtensionNamesList)
    cleanListFunc(os.getcwd() + "\\filtered_data_files\\filter_espace_names.txt", espaceNamesList, myExtensionNamesList)
    cleanListFunc(os.getcwd() + "\\filtered_data_files\\filter_cyclic_job_names.txt", cyclicJobNamesList, myCyclicJobNamesList)

    print("Closing: " + filename + ".txt")

def txtFile(absolutePath, filename, filenameWithExt, ext, _fromDate, _toDate):
    print("Reading: " + filenameWithExt)

    outText = ""
    outText2 = ""
    nonMatchedOutText = ""
    JPText = ""
    nonMatchedLine = ""
    nonMatchedLine2 = ""

    try:
        with codecs.open(absolutePath, "r", "utf-8", "ignore") as linesFromText:
            searchLines = linesFromText.read()
            searchLines = searchLines.replace("|", "")
            searchLines = searchLines.replace("\\", "")
            searchLines = searchLines.replace("/", "-")
            searchLines2 = ' '.join(searchLines.splitlines())
            searchLines3 = ' '.join(searchLines2.split())
            
            if "iosbuildlog" in filename.lower() or "androidbuildlog" in filename.lower():
                readiOSAndroidLogs(filename, searchLines, _fromDate, _toDate)
            elif "studio" in filename.lower() and "report" in filename.lower():
                readServiceStudioReportLogs(searchLines3, _fromDate, _toDate)
            elif "general" in filename.lower():
                readGeneralTextLogs(searchLines, _fromDate, _toDate)
            elif "fullerrordump" == filename.lower():
                readFullErrorDumpLogs(searchLines)
    except ValueError as valError:
        print("\nThe customer altered the original logs.\n\nPossible reasons:\n1- The customer added a column on the logs.\n" +
              "2- The customer switched the columns of the logs.\n3- The customer modified the dates on the logs.\n")

    if len(myNonMatchedValidLinesFromDateRange) > 0:
        createFolder("\\nonmatched_valid_lines\\")
        with codecs.open(nonMatchedPath, "a", "utf-8", "ignore") as linesFromDateRange2:
            linesFromDateRange2.writelines(myNonMatchedValidLinesFromDateRange)
        del myNonMatchedValidLinesFromDateRange[:]

    if len(myLinesFromDateRange) > 0:
        createFolder("\\filtered_data_files\\")
        if "iosbuildlog" in filename.lower():
            outFilename = "iOS_build_logs" + ext
            writeToFile2(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange)
            if numOfiOSLogs > 1:
                populateList2(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange)
        elif "androidbuildlog" in filename.lower():
            outFilename = "android_build_logs" + ext
            writeToFile2(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange)
            if numOfAndroidLogs > 1:
                populateList2(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange)
        elif "studio" in filename.lower() and "report" in filename.lower():
            outFilename = "service_studio_report" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
            if numOfServiceStudioReports > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
        elif "general" in filename.lower():
            outFilename = "general_text_logs" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
            if numOfGeneralTXTLogs > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
        elif "fullerrordump" == filename.lower():
            outFilename = "full_error_dump_logs" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
    else:
        if not "errordump" == filename.lower():
            print("No data was found within the specified date range.")

    print("Closing: " + filenameWithExt)

def logFile(absolutePath, filenameWithExt, ext, _fromDate, _toDate):
    global numOfIISLogs
    numOfIISLogs+=1

    #pattern for filenames: u_exYYMMDD.log
    print("Reading: " + filenameWithExt)

    nonMatchedOutText = ""
    JPText = ""
    nonMatchedLine = ""
    nonMatchedLine2 = ""

    #split the fields and rearrange them to combine them all later
    if filenameWithExt.lower().endswith("_x.log"):
        print("\nThe customer altered the original IIS logs.\n\nFilenames ending in \"_x\" mean the customer created his own customized version of the IIS logs.\n")
    else:
        try:
            with codecs.open(absolutePath, "r", "utf-8", "ignore") as linesFromLog:
                searchLines = linesFromLog.read()
                regex = re.compile(iisLogsRegex, re.MULTILINE + re.IGNORECASE)
                for match in regex.finditer(searchLines):
                    date = match.group(1)
                    time = match.group(2)
                    serverIP = match.group(3)
                    method = match.group(4)
                    uriStem = match.group(5)
                    uriQuery = match.group(6)
                    serverPort = match.group(7)
                    username = match.group(8)
                    clientIP = match.group(9)
                    browser = match.group(10)
                    referrer = match.group(11)
                    httpCode = match.group(12)
                    httpSubCode = match.group(13)
                    windowsCode = match.group(14)
                    timeTaken = match.group(15)

                    _date = datetime.strptime(date, "%Y-%m-%d").date()
                        
                    if _fromDate <= _date <= _toDate:
                        #timeTaken is in milliseconds and it needs to be converted to seconds
                        seconds = int(timeTaken)/1000

                        myLinesFromDateRange.append(date + " " + time + "|" + str(seconds) + "|" + httpCode + "|" + httpSubCode + "|" + windowsCode + "|" + clientIP + "|" + serverIP + "|" + serverPort + "|" + method + "|" + uriStem + "|" + uriQuery + "|" + username + "|" + browser + "|" + referrer + "\n")

                #capture all the lines that didn't match the regex
                negativeRegex = re.compile(negativeIisLogsRegex, re.MULTILINE + re.IGNORECASE)
                for match in negativeRegex.finditer(searchLines):
                    nonMatchedLine = match.group(1)

                    nonMatchedRegex = re.search(nonMatchedIisLogsRegex, nonMatchedLine)
                    if nonMatchedRegex:
                        nonMatchedDate = nonMatchedRegex.group(1)
                        nonMatchedTail = nonMatchedRegex.group(2)

                        _nonMatchedDate = datetime.strptime(nonMatchedDate, "%Y-%m-%d").date()

                        #check if the non-matched lines fall within the specified range
                        if _fromDate <= _nonMatchedDate <= _toDate:

                            nonMatchedLine2 = nonMatchedDate + nonMatchedTail

                            #check if the line has Japanese characters
                            hiragana = re.findall(u'[\u3040-\u309F]', nonMatchedLine2)
                            katakana = re.findall(u'[\u30A0-\u30FF]', nonMatchedLine2)
                            kanji = re.findall(u'[\u4E00-\u9FAF]', nonMatchedLine2)

                            if hiragana or katakana or kanji:
                                JPRegex = re.search(japaneseIisLogsRegex, nonMatchedLine2)
                                if JPRegex:
                                    JPDate = JPRegex.group(1)
                                    JPTime = JPRegex.group(2)
                                    JPTail = JPRegex.group(3)
                                    JPTimeTaken = JPRegex.group(4)

                                    newJPTail = normalizeLines(JPTail)

                                    JPText = JPDate + " " + JPTime + "|" + JPTimeTaken + "|" + newJPTail + "\n"

                                    myLinesFromDateRange.append(JPText)

                            else:
                                newNonMatchedLine2 = normalizeLines(nonMatchedLine2)
                                newNonMatchedLine2 = newNonMatchedLine2.translate(replacementDict)
                                
                                #check if the line matches the regular expression
                                regex2 = re.search(iisLogsRegex, newNonMatchedLine2)
                                if regex2:
                                    date = regex2.group(1)
                                    time = regex2.group(2)
                                    serverIP = regex2.group(3)
                                    method = regex2.group(4)
                                    uriStem = regex2.group(5)
                                    uriQuery = regex2.group(6)
                                    serverPort = regex2.group(7)
                                    username = regex2.group(8)
                                    clientIP = regex2.group(9)
                                    browser = regex2.group(10)
                                    referrer = regex2.group(11)
                                    httpCode = regex2.group(12)
                                    httpSubCode = regex2.group(13)
                                    windowsCode = regex2.group(14)
                                    timeTaken = regex2.group(15)

                                    #timeTaken is in milliseconds and it needs to be converted to seconds
                                    seconds = int(timeTaken)/1000

                                    myLinesFromDateRange.append(date + " " + time + "|" + str(seconds) + "|" + httpCode + "|" + httpSubCode + "|" + windowsCode + "|" + clientIP + "|" + serverIP + "|" + serverPort + "|" + method + "|" + uriStem + "|" + uriQuery + "|" + username + "|" + browser + "|" + referrer + "\n")

                                if not regex2:
                                    nonMatchedOutText = "IISLog -> " + newNonMatchedLine2 + "\n"

                                    myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

        except ValueError as valError:
            print("\nThe customer altered the original IIS logs.\n\nPossible reasons:\n1- The customer added a column on the logs.\n" +
                  "2- The customer switched the columns of the logs.\n")

    if len(myNonMatchedValidLinesFromDateRange) > 0:
        createFolder("\\nonmatched_valid_lines\\")
        with codecs.open(nonMatchedPath, "a", "utf-8", "ignore") as linesFromDateRange2:
            linesFromDateRange2.writelines(myNonMatchedValidLinesFromDateRange)
        del myNonMatchedValidLinesFromDateRange[:]

    if len(myLinesFromDateRange) > 0:
        createFolder("\\filtered_data_files\\")
        outFilename = "iis_logs" + ext
        writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
        if numOfIISLogs > 1:
            populateList(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
    else:
        print("No data was found within the specified date range.")

    print("Closing: " + filenameWithExt)

def evtxFile(absolutePath, filenameWithExt, ext, _fromDate, _toDate):
    print("Reading: " + filenameWithExt)

    outText = ""
    channel = ""

    #read the windows event viewer log and convert its contents to XML
    with codecs.open(tempFilePath, "a+", "utf-8", "ignore") as tempFile:
        with evtx.Evtx(absolutePath) as log:
            for record in log.records():
                xmlLine = record.xml()
                xmlLine = xmlLine.replace(" xmlns=\"http://schemas.microsoft.com/win/2004/08/events/event\"", "")
                xmlParse = etree.XML(xmlLine)

                level = parseXMLtoString(xmlParse, ".//Level/text()")

                if not level == "0" and not level == "4":
                    providerName = parseXMLtoString(xmlParse, ".//Provider/@Name")
                    qualifiers = parseXMLtoString(xmlParse, ".//EventID/@Qualifiers")
                    timestamp = parseXMLtoString(xmlParse, ".//TimeCreated/@SystemTime")
                    eventID = parseXMLtoString(xmlParse, ".//EventID/text()")
                    task = parseXMLtoString(xmlParse, ".//Task/text()")
                    keywords = parseXMLtoString(xmlParse, ".//Keywords/text()")
                    eventRecordID = parseXMLtoString(xmlParse, ".//EventRecordID/text()")
                    channel = parseXMLtoString(xmlParse, ".//Channel/text()")
                    computer = parseXMLtoString(xmlParse, ".//Computer/text()")
                    message1 = parseXMLtoString(xmlParse, ".//Data/@Name")
                    message2 = parseXMLtoString(xmlParse, ".//Data/text()")

                    if level == "1":
                        level = "Critical"
                    elif level == "2":
                        level = "Error"
                    elif level == "3":
                        level = "Warning"
                                
                    date = timestamp[0:10]
                    time = timestamp[11:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()
                                        
                    if _fromDate <= _date <= _toDate:

                        message2 = message2.replace("<string>", "")
                        message2 = message2.replace("</string>", "")
                        newMessage1 = normalizeLines(message1)
                        newMessage2 = normalizeLines(message2)
                        newMessage2 = newMessage2.translate(replacementDict)

                        if len(newMessage1.strip()) > 0:
                            outText = date + " " + time + "|" + level + "|" + "[" + newMessage1.strip() + "] " + newMessage2.strip() + "|" + task + "|" + computer + "|" + providerName + "|" + qualifiers + "|" + eventID + "|" + eventRecordID + "|" + keywords + "\n"
                        else:
                            outText = date + " " + time + "|" + level + "|" + newMessage2.strip() + "|" + task + "|" + computer + "|" + providerName + "|" + qualifiers + "|" + eventID + "|" + eventRecordID + "|" + keywords + "\n"

                        tempFile.writelines(outText)

    with codecs.open(tempFilePath, "r", "utf-8", "ignore") as tempFile2:
        myLinesFromDateRange = tempFile2.readlines()
            
    #delete the temporary file that was created
    os.remove(tempFilePath)          

    if len(myLinesFromDateRange) > 0:
        createFolder("\\filtered_data_files\\")

        if channel.lower() == "application":
            global numOfWinAppEvViewerLogs
            numOfWinAppEvViewerLogs+=1

            outFilename = "windows_" + channel.lower() + "_event_viewer_logs" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
            if numOfWinAppEvViewerLogs > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
        elif channel.lower() == "security":
            global numOfWinSecEvViewerLogs
            numOfWinSecEvViewerLogs+=1

            outFilename = "windows_" + channel.lower() + "_event_viewer_logs" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
            if numOfWinSecEvViewerLogs > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
        elif channel.lower() == "system":
            global numOfWinSysEvViewerLogs
            numOfWinSysEvViewerLogs+=1

            outFilename = "windows_" + channel.lower() + "_event_viewer_logs" + ext
            writeToFile(absolutePath, os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
            if numOfWinSysEvViewerLogs > 1:
                populateList(os.getcwd() + "\\filtered_data_files\\" + outFilename, myLinesFromDateRange, myFinalLinesFromDateRange)
    else:
        print("No data was found within the specified date range.")

    print("Closing: " + filenameWithExt)

#the following line displays the absolute path of the folder where Python was installed on your PC. Uncomment only when necessary.
#print("PYTHON WAS INSTALLED HERE -> " + os.path.dirname(sys.executable))

num_args = len(sys.argv)

if num_args != 5:
    #check if the additional arguments are due to the directory name being separated by spaces
    dirs = "".join(sys.argv[1:num_args - 3])
    os.rename(" ".join(sys.argv[1:num_args - 3]), dirs)
    if os.path.exists(dirs):
        directoryPath = dirs
        fromDate = sys.argv[num_args - 3]
        toDate = sys.argv[num_args - 2]
        createGraphs = sys.argv[num_args -1]
        prereqs(directoryPath, fromDate, toDate, createGraphs)
    else:
        print("Error:\nTotal arguments passed: " + str(num_args))
        print(" ".join(sys.argv[1:]))
        print("\n5 arguments needed: log_parser.py directoryPath fromDate(YYYY-MM-DD) toDate(YYYY-MM-DD) createGraphsOption" +
              "\nPlease try again.")
else:
    directoryPath = sys.argv[1]
    fromDate = sys.argv[2]
    toDate = sys.argv[3]
    createGraphs = sys.argv[4]
    prereqs(directoryPath, fromDate, toDate, createGraphs)

end = datetime.now()
print("\nElapsed time: {0}".format(end-start))
input("\nPress \"Enter\" to close the script.")
