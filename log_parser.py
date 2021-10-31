import os
import re
import sys
import codecs
import shutil
import openpyxl
from lxml import etree
import Evtx.Evtx as evtx
from datetime import datetime
import matplotlib.pyplot as plt
import matplotlib.dates as mdates

"""
OutSystems Log Parser

Service Center Logs:
Error logs:
DATE_TIME MESSAGE STACK MODULE_NAME APPLICATION_NAME APPLICATION_KEY ACTION_NAME ENTRYPOINT_NAME SERVER ESPACE_NAME ESPACE_ID USER_ID SESSION_ID ENVIRONMENT_INFORMATION ID TENANT_ID

General logs:
DATE_TIME MESSAGE MESSAGE_TYPE MODULE_NAME APPLICATION_NAME APPLICATION_KEY ACTION_NAME ENTRYPOINT_NAME CLIENT_IP ESPACE_NAME ESPACE_ID USER_ID SESSION_ID ERROR_ID REQUEST_KEY TENANT_ID

Integration logs:
DATE_TIME DURATION APPLICATION_NAME APPLICATION_KEY ACTION_NAME ACTION_TYPE SOURCE ENDPOINT EXECUTED_BY ESPACE_NAME ESPACE_ID ERROR_ID REQUEST_KEY TENANT_ID

MobileRequestsLog = ScreenRequests
Screen Requests logs:
DATE_TIME DURATION SCREEN APPLICATION_NAME APPLICATION_KEY SOURCE ENDPOINT EXECUTED_BY ESPACE_NAME ESPACE_ID LOGIN_ID USER_ID CYCLE ERROR_ID REQUEST_KEY TENANT_ID

Timer logs:
DATE_TIME DURATION APPLICATION_NAME APPLICATION_KEY EXECUTED_BY ESPACE_NAME ESPACE_ID CYCLIC_JOB_NAME CYCLIC_JOB_KEY SHOULD_HAVE_RUN_AT NEXT_RUN ERROR_ID REQUEST_KEY TENANT_ID

Email logs:
DATE_TIME SENT LAST_ERROR FROM TO SUBJECT CC BCC NAME SIZE MESSAGE_ID ACTIVITY EMAIL_DEFINITION STORE_CONTENT IS_TEST_EMAIL ID TENANT_ID

Extension logs:
DATE_TIME DURATION APPLICATION_NAME APPLICATION_KEY ACTION_NAME EXECUTED_BY ESPACE_NAME ESPACE_ID USERNAME USER_ID SESSION_ID EXTENSION_ID EXTENSION_NAME ERROR_ID REQUEST_KEY TENANT_ID

Service Action logs:
DATE_TIME DURATION APPLICATION_NAME APPLICATION_KEY ACTION_NAME SOURCE ENTRYPOINT_NAME ENDPOINT EXECUTED_BY ESPACE_NAME ESPACE_ID USERNAME LOGIN_ID USER_ID SESSION_ID ERROR_ID REQUEST_KEY ORIGINAL_REQUEST_KEY TENANT_ID

ScreenLog = TraditionalWebRequests             
TraditionalWebRequests logs:
DATE_TIME DURATION SCREEN SCREEN_TYPE APPLICATION_NAME APPLICATION_KEY ACTION_NAME ACCESS_MODE EXECUTED_BY CLIENT_IP ESPACE_NAME ESPACE_ID USER_ID SESSION_ID SESSION_REQUESTS SESSION_BYTES VIEW_STATE_BYTES MS_IS_DN REQUEST_KEY TENANT_ID

BPTTroubleshootingReports:
DATE_TIME ESPACE_NAME PROCESS_NAME PROCESS_STATUS PROCESS_LAST_MODIFIED PROCESS_SUSPENDED_DATE PROCESS_ID PARENT_PROCESS_ID ACTIVITY_CREATED ACTIVITY_NAME ACTIVITY_KIND ACTIVITY_STATUS ACTIVITY_RUNNING_SINCE ACTIVITY_NEXT_RUN ACTIVITY_CLOSED ACTIVITY_ERROR_COUNT ACTIVITY_ERROR_ID

IIS logs:
DATE_TIME TIME_TAKEN HTTP_CODE HTTP_SUBCODE WINDOWS_ERROR_CODE CLIENT_IP SERVER_IP SERVER_PORT
METHOD URI_STEM URI_QUERY USERNAME BROWSER REFERRER

TIME_TAKEN DATE_TIME HTTP_CODE HTTP_SUBCODE WINDOWS_ERROR_CODE CLIENT_IP SERVER_IP SERVER_PORT
METHOD URI_STEM URI_QUERY USERNAME BROWSER REFERRER

*******   *******   *******
NOTE: The IIS logs with filenames ending in "_x" mean the customer selected the columns to display data from and rearranged
the columns at his own will. Until we have a defined set of columns and their order, no logic can anticipate the customer's will.

#Fields: date time s-sitename s-computername s-ip cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs-version cs(User-Agent) cs(Referer)
sc-status sc-substatus sc-win32-status sc-bytes cs-bytes time-taken x-forwarded-for

^([\d\-]+)[ ]([\d\:]+)(?:[ ])?([\w]+)?(?:[ ])?([\w]+)?[ ]([\d\.\:]+)[ ](POST|PUT|PROPFIND|(?:n)?GET|OPTIONS|HEAD|ABCD|QUALYS|TRACE|SEARCH|RNDMMTD|TRACK|B(?:A)?DM(?:E)?T(?:H)?(?:O)?(?:D)?|CFYZ|DEBUG|MKCOL|INDEX|DELETE|PATCH|ACUNETIX)[ ]([\w\(\)\[\]\{\}\-\–\:\;\‘\’\'\"\“\”\,\.\>\`\~\<\&\=\\\/\?\+\$\@\%\#\*\!\»\£\€]+)[ ]([\w\(\)\[\]\{\}\-\–\:\;\‘\’\'\"\“\”\,\.\>\`\~\<\&\=\\\/\?\+\$\@\%\#\*\!\»\£\€]+)[ ]([\d]+)[ ]([\w\(\)\[\]\{\}\-\–\:\;\‘\’\'\"\“\”\,\.\>\`\~\<\&\=\\\/\?\+\$\@\%\#\*\!\»\£\€]+)[ ]([\d\.\:]+)(?:[ ])?([\w\/\.]+)?[ ]([\w\(\)\[\]\{\}\-\–\:\;\‘\’\'\"\“\”\,\.\>\`\~\<\&\=\\\/\?\+\$\@\%\#\*\!\»\£\€]+)[ ]([\w\(\)\[\]\{\}\-\–\:\;\‘\’\'\"\“\”\,\.\>\`\~\<\&\=\\\/\?\+\$\@\%\#\*\!\»\£\€]+)[ ]([\d]+)[ ]([\d]+)[ ]([\d]+)(?:[ ])?([\d]+)?(?:[ ])?([\d]+)?[ ]([\d]+)(?:[ ])?([\w\(\)\[\]\{\}\-\–\:\;\‘\’\'\"\“\”\,\.\>\`\~\<\&\=\\\/\?\+\$\@\%\#\*\!\»\£\€]+)?

DATE_TIME TIME_TAKEN HTTP_CODE HTTP_SUBCODE HTTP_VERSION WINDOWS_ERROR_CODE CLIENT_IP ACTUAL_CLIENT_IP SERVER_IP
SERVER_PORT SERVER_NAME SERVICE_NAME METHOD URI_STEM URI_QUERY USERNAME BROWSER REFERRER BYTES_SENT BYTES_RECEIVED
*******   *******   *******

Android, iOS, and Service Studio Report:
DATE_TIME MESSAGE_TYPE ACTION_NAME MESSAGE

General Text Logs:
DATE_TIME MESSAGE_TYPE MESSAGE

Windows Event Viewer logs:
DATE_TIME LEVEL MESSAGE TASK COMPUTER PROVIDER_NAME QUALIFIERS EVENT_ID EVENT_RECORD_ID KEYWORDS

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

1) Filter data based on a date range
2) Read the input files and rearrange the columns.
3) Filtered outputs should be already sorted by their timestamp.
4) Filtered outputs should be TXT files using the pipe ("|") as the delimiter and their filenames should be the logs they represent.
5) All the "datetime" fields from the logs should be in the following format: YYYY-MM-DD hh:mm:ss
"""

myLinesFromDateRange = []
myFinalLinesFromDateRange = []
myNonMatchedValidLinesFromDateRange = []
myTempXLSXlines = []
myLOGlines = []
myTimeTaken = []
myFileExt = []
myDateTimes = []
myTimesTaken = []

#all illegal space characters, control characters, ASCII characters, and emojis
replacementDict = {}
replacementDict.update(dict.fromkeys(range(8220, 8221), ""))
replacementDict.update(dict.fromkeys(range(166, 168), ""))
replacementDict.update(dict.fromkeys(range(170, 172), ""))
replacementDict.update(dict.fromkeys(range(176, 190), ""))
replacementDict.update(dict.fromkeys(range(196, 198), ""))
replacementDict.update(dict.fromkeys(range(207, 208), ""))
replacementDict.update(dict.fromkeys(range(214, 216), ""))
replacementDict.update(dict.fromkeys(range(221, 223), ""))
replacementDict.update(dict.fromkeys(range(228, 230), ""))
replacementDict.update(dict.fromkeys(range(239, 240), ""))
replacementDict.update(dict.fromkeys(range(246, 248), ""))
replacementDict.update(dict.fromkeys(range(9617, 9619), ""))
replacementDict.update(dict.fromkeys(range(8192, 8207), ""))
replacementDict.update(dict.fromkeys(range(4, 7), ""))
replacementDict[8240] = ""
replacementDict[8218] = ""
replacementDict[8211] = "-"
replacementDict[353] = ""
replacementDict[164] = ""
replacementDict[175] = "-"
replacementDict[203] = ""
replacementDict[235] = ""
replacementDict[255] = ""
replacementDict[305] = ""
replacementDict[402] = ""
replacementDict[8215] = "_"
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
replacementDict[160] = ""
replacementDict[173] = ""
replacementDict[129] = ""
replacementDict[141] = ""
replacementDict[5760] = "-"
replacementDict[6158] = ""
replacementDict[65279] = ""

numOfLines = 6000
constant = 6000

errorLogsRegex = r"^([\d]+)\|([\w\-]+)\|([\d\-\:\. ]+)\|([\w\'\/\=\+ ]+)?\|([\d]+)\|([\d]+)\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|([\w\-\(\)\.\* ]+)?\|([\w\.\(\)]+)?\|([\w\-]+)?\|([\w]+)?\|([\w\-\.\,\(\)\[\]\/\& ]+)?\|([\w\-]+)?"
negativeErrorLogsRegex = r"^((?!(?:[\d]+)\|(?:[\w\-]+)\|(?:[\d\-\:\. ]+)\|(?:[\w\'\/\=\+ ]+)?\|(?:[\d]+)\|(?:[\d]+)\|(?:[\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|(?:[\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|(?:[\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|(?:[\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|(?:[\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|(?:[\w\-\(\)\.\* ]+)?\|(?:[\w\.\(\)]+)?\|(?:[\w\-]+)?\|(?:[\w]+)?\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)?\|(?:[\w\-]+)?).*)"
nonMatchedErrorLogsRegex = r"^((?:.*?\|){2})([\d\-]+)(.+)"
japaneseErrorLogsRegex = r"^([\d]+)\|(.*?)\|([\d\-\:\. ]+)\|(.*?)?\|([\d]+)\|([\d]+)\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?"

generalLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\w\+\/\=\' ]+)?\|([\d]+)\|([\d]+)\|([\w\-]+)?\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|([\w]+)?\|([\w\-\.\:\# ]+)?\|([\w\-]+)?\|([\w\-\(\)\.\* ]+)?\|([\w\(\)\.]+)?\|([\w\-\.\:\;\% ]+)?\|([\w]+)?\|([\w\-\.\,\(\)\[\]\/\& ]+)?\|([\w\-]+)?\|([\w\@\.\\]+)?"
negativeGeneralLogsRegex = r"^((?!(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\w\+\/\=\' ]+)?\|(?:[\d]+)\|(?:[\d]+)\|(?:[\w\-]+)?\|(?:[\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?\|(?:[\w]+)?\|(?:[\w\-\.\:\# ]+)?\|(?:[\w\-]+)?\|(?:[\w\-\(\)\.\* ]+)?\|(?:[\w\(\)\.]+)?\|(?:[\w\-\.\:\;\% ]+)?\|(?:[\w]+)?\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)?\|(?:[\w\-]+)?\|(?:[\w\@\.\\]+)?).*)"
nonMatchedGeneralLogsRegex = r"^((?:.*?\|){1})([\d\-]+)(.+)"
japaneseGeneralLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|(.*?)?\|([\d]+)\|([\d]+)\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?\|(.*?)?"

integrationsLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w\.\:\;\-\% ]+)?\|([\w\:\/\\\.\-\=\%\&\?\~]+)?\|([\w\/\.\-\(\) ]+)\|([\w\(\) ]+)\|([\d]+)\|([\w\-]+)?\|([\w\-]+)\|([\w\-\:]+)\|([\w\.]+)?\|([\w\-\.\,\(\)\[\]\/\& ]+)?\|([\w\-]+)?"
negativeIntegrationsLogsRegex = r"^((?!(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\d]+)\|(?:[\w\.\:\;\-\% ]+)?\|(?:[\w\:\/\\\.\-\=\%\&\?\~]+)?\|(?:[\w\/\.\-\(\) ]+)\|(?:[\w\(\) ]+)\|(?:[\d]+)\|(?:[\w\-]+)?\|(?:[\w\-]+)\|(?:[\w\-\:]+)\|(?:[\w\.]+)?\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)?\|(?:[\w\-]+)?).*)"
nonMatchedIntegrationsLogsRegex = r"^((?:.*?\|){1})([\d\-]+)(.+)"
japaneseIntegrationsLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|(.*?)?\|(.*?)?\|(.*?)\|(.*?)\|([\d]+)\|(.*?)?\|(.*?)\|(.*?)\|(.*?)?\|(.*?)?\|(.*?)?"

mobileRequestsLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\.\-\:\;\% ]+)\|([\w\:\/\\\.\-\=\%\&\?]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\d]+)\|([\w\-\:]+)\|([\w\/\+\=]+)?\|([\d]+)\|([\w\.]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|([\w\-]+)"
negativeMobileRequestsLogsRegex = r"^((?!(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\d]+)\|(?:[\w]+)\|(?:[\w\.\-\:\;\% ]+)\|(?:[\w\:\/\\\.\-\=\%\&\?]+)\|(?:[\d]+)\|(?:[\w\-]+)\|(?:[\w\-]+)?\|(?:[\d]+)\|(?:[\w\-\:]+)\|(?:[\w\/\+\=]+)?\|(?:[\d]+)\|(?:[\w\.]+)\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)\|(?:[\w\-]+)).*)"
nonMatchedMobileRequestsLogsRegex = r"^((?:.*?\|){1})([\d\-]+)(.+)"
japaneseMobileRequestsLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|(.*?)\|(.*?)\|(.*?)\|([\d]+)\|(.*?)\|(.*?)?\|([\d]+)\|(.*?)\|(.*?)?\|([\d]+)\|(.*?)\|(.*?)\|(.*?)"

timerLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w\-]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\d\-\:\. ]+)\|([\d\-\:\. ]+)\|([\w\-\:]+)?\|([\w\.]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|([\w\-]+)\|([\w]+)"
negativeTimerLogsRegex = r"^((?!(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\d]+)\|(?:[\w\-]+)\|(?:[\d]+)\|(?:[\w\-]+)\|(?:[\w\-]+)?\|(?:[\d\-\:\. ]+)\|(?:[\d\-\:\. ]+)\|(?:[\w\-\:]+)?\|(?:[\w\.]+)\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)\|(?:[\w\-]+)\|(?:[\w]+)).*)"
nonMatchedTimerLogsRegex = r"^((?:.*?\|){1})([\d\-]+)(.+)"
japaneseTimerLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|(.*?)\|([\d]+)\|(.*?)\|(.*?)?\|([\d\-\:\. ]+)\|([\d\-\:\. ]+)\|(.*?)?\|(.*?)\|(.*?)\|(.*?)\|(.*?)"

emailLogsRegex = r"^([\d]+)\|([\w]+)\|([\w\-\:\. ]+)\|([\w\-\:\. ]+)?\|([\d]+)\|([\w\@\.\,\- ]+)\|([\w\@\.\,\- ]+)\|([\w\@\.\-]+)?\|([\w\@\.\-]+)?\|([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)\|([\w]+)\|([\w]+)\|([\d]+)\|([\w\@\.\-]+)"
negativeEmailLogsRegex = r"^((?!(?:[\d]+)\|(?:[\w]+)\|(?:[\w\-\:\. ]+)\|(?:[\w\-\:\. ]+)?\|(?:[\d]+)\|(?:[\w\@\.\,\- ]+)\|(?:[\w\@\.\,\- ]+)\|(?:[\w\@\.\-]+)?\|(?:[\w\@\.\-]+)?\|(?:[\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)\|(?:[\d\-\:\. ]+)\|(?:[\d]+)\|(?:[\d]+)\|(?:[\w]+)\|(?:[\w]+)\|(?:[\d]+)\|(?:[\w\@\.\-]+)).*)"
nonMatchedEmailLogsRegex = r"^((?:.*?\|){10})([\d\-]+)(.+)"
japaneseEmailLogsRegex = r"^([\d]+)\|(.*?)\|(.*?)\|(.*?)?\|([\d]+)\|(.*?)\|(.*?)\|(.*?)?\|(.*?)?\|(.*?)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)\|(.*?)\|(.*?)\|([\d]+)\|(.*?)"

extensionLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\/\'\=\+]+)\|([\d]+)\|([\d]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\w\-\:]+)\|([\w\.]+)\|([\w]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|([\w\-]+)\|([\w\@\.\\]+)?"
negativeExtensionLogsRegex = r"^((?!(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\d]+)\|(?:[\w]+)\|(?:[\w\/\'\=\+]+)\|(?:[\d]+)\|(?:[\d]+)\|(?:[\d]+)\|(?:[\w\-]+)\|(?:[\w\-]+)?\|(?:[\w\-\:]+)\|(?:[\w\.]+)\|(?:[\w]+)\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)\|(?:[\w\-]+)\|(?:[\w\@\.\\]+)?).*)"
nonMatchedExtensionLogsRegex = r"^((?:.*?\|){1})([\d\-]+)(.+)"
japaneseExtensionLogsRegex = r"^([\d]+)\|(.*?)\|(.*?)\|(.*?)?\|([\d]+)\|(.*?)\|(.*?)\|(.*?)?\|(.*?)?\|(.*?)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)\|(.*?)\|(.*?)\|([\d]+)\|(.*?)"

serviceActionLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)?\|([\w\/\'\=\+]+)\|([\d]+)\|([\w\-]+)?\|([\w\-]+)\|([\w\-\:]+)\|([\w\-\(\)\.\* ]+)?\|([\w\.\(\)]+)\|([\d]+)\|([\w\.\:\;\-\% ]+)\|([\w\:\/\\\.\-\=\%\&\?]+)\|([\w\.]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|([\w\-]+)\|([\w\@\.\\]+)?\|([\w\-\:]+)"
negativeServiceActionLogsRegex = r"^((?!(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\d]+)\|(?:[\d]+)?\|(?:[\w\/\'\=\+]+)\|(?:[\d]+)\|(?:[\w\-]+)?\|(?:[\w\-]+)\|(?:[\w\-\:]+)\|(?:[\w\-\(\)\.\* ]+)?\|(?:[\w\.\(\)]+)\|(?:[\d]+)\|(?:[\w\.\:\;\-\% ]+)\|(?:[\w\:\/\\\.\-\=\%\&\?]+)\|(?:[\w\.]+)\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)\|(?:[\w\-]+)\|(?:[\w\@\.\\]+)?\|(?:[\w\-\:]+)).*)"
nonMatchedServiceActionLogsRegex = r"^((?:.*?\|){1})([\d\-]+)(.+)"
japaneseServiceActionLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)?\|(.*?)\|([\d]+)\|(.*?)?\|(.*?)\|(.*?)\|(.*?)?\|(.*?)\|([\d]+)\|(.*?)\|(.*?)\|(.*?)\|(.*?)\|(.*?)\|(.*?)?\|(.*?)"

screenLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\/\'\=\+]+)\|([\d]+)\|([\d]+)\|([\w\-\:\. ]+)?\|([\w]+)\|([\w\-]+)\|([\d]+)\|([\d]+)\|([\d]+)\|([\w]+)\|([\w\-\:]+)\|([\w\(\)\.]+)?\|([\w\-\:\. ]+)\|([\w\.]+)\|([\w\-\.\,\(\)\[\]\/\& ]+)\|([\w\-]+)"
negativeScreenLogsRegex = r"^((?!(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\d]+)\|(?:[\w]+)\|(?:[\w\/\'\=\+]+)\|(?:[\d]+)\|(?:[\d]+)\|(?:[\w\-\:\. ]+)?\|(?:[\w]+)\|(?:[\w\-]+)\|(?:[\d]+)\|(?:[\d]+)\|(?:[\d]+)\|(?:[\w]+)\|(?:[\w\-\:]+)\|(?:[\w\(\)\.]+)?\|(?:[\w\-\:\. ]+)\|(?:[\w\.]+)\|(?:[\w\-\.\,\(\)\[\]\/\& ]+)\|(?:[\w\-]+)).*)"
nonMatchedScreenLogsRegex = r"^((?:.*?\|){1})([\d\-]+)(.+)"
japaneseScreenLogsRegex = r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|(.*?)\|(.*?)\|([\d]+)\|([\d]+)\|(.*?)?\|(.*?)\|(.*?)\|([\d]+)\|([\d]+)\|([\d]+)\|(.*?)\|(.*?)\|(.*?)?\|(.*?)\|(.*?)\|(.*?)\|(.*?)"

bptTroubleshootingReportsRegex = "^([\d]+)\|([\w]+)\|([\w]+)\|([\w]+)\|([\d]+)\|([\d\-\:\. ]+)\|([\d\-\:\. ]+)\|([\w\d\-\:\. ]+)?\|([\w\? ]+)\|([\w]+)\|([\d]+)\|([\w\-]+)?\|([\w ]+)\|([\d\-\:\. ]+)((?:\|)(?:[\w\d\-\:\. ]+))?((?:\|)(?:[\w\d\-\:\. ]+))?((?:\|)(?:[\w\d\-\:\. ]+))?"
negativeBptTroubleshootingReportsRegex = "^((?!(?:[\d]+)\|(?:[\w]+)\|(?:[\w]+)\|(?:[\w]+)\|(?:[\d]+)\|(?:[\d\-\:\. ]+)\|(?:[\d\-\:\. ]+)\|(?:[\w\d\-\:\. ]+)?\|(?:[\w\? ]+)\|(?:[\w]+)\|(?:[\d]+)\|(?:[\w\-]+)?\|(?:[\w ]+)\|(?:[\d\-\:\. ]+)(?:(?:\|)(?:[\w\d\-\:\. ]+))?(?:(?:\|)(?:[\w\d\-\:\. ]+))?(?:(?:\|)(?:[\w\d\-\:\. ]+))?).*)"
nonMatchedBptTroubleshootingReportsRegex = "^(.*?\|)([\d\-]+)((?:\s+[\d\:\.]+).+)"

androidiOSBuildLogsRegex = "^\[([\d\-]+)T([\d\:\.]+)Z\][ ]\[(INFO|VERBOSE|ERROR|DEBUG)\][ ](\[(?:[\w\[\] ]+)\])?(?:[ \t]+)?([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?"
negativeAndroidiOSBuildLogsRegex = "^((?!\[(?:[\d\-]+)T(?:[\d\:\.]+)Z\][ ]\[(?:INFO|VERBOSE|ERROR|DEBUG)\][ ](?:\[(?:[\w\[\] ]+)\])?(?:[ \t]+)?(?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?).*)"
nonMatchedAndroidiOSBuildLogsRegex = "^\[([\d\-]+)(?:T.+)"
japaneseAndroidiOSBuildLogsRegex = "^\[([\d\-]+)T([\d\:\.]+)Z\](.+)"

serviceStudioReportsDetailsRegex = "^.*?(Service.*?:\s*?(?:[\d\.])+).*?(Platform.*?:\s*?(?:[\d\.])+).*?(Service.*?)Channel"
serviceStudioReportsOperationsLogsRegex = "\[([\d\-]+)[ ]([\d\:A-Z ]+)\][ ]\[([\d\:\?]+)\][ ]([\w\-\(\) ]{10,}?)\s+([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®\t\r\n ]+)?\|"
serviceStudioReportsOperationsLogsRegex2 = "^([\d\-]+)\|([\d\:A-Z ]+)\|([\d\:\?]+)\|([\w\-\(\) ]{10,}?)\|([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?"
negativeServiceStudioReportsOperationsLogsRegex = "^((?!(?:[\d\-]+)\|(?:[\d\:A-Z ]+)\|(?:[\d\:\?]+)\|(?:[\w\-\(\) ]{10,}?)\|(?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)?).*)"
nonMatchedServiceStudioReportsOperationsLogsRegex = "^([\d\-]+)\|(.+)"
japaneseServiceStudioReportsOperationsLogsRegex = "^([\d\-]+)\|([\d\:A-Z ]+)\|(.+)"

generalTextLogsRegex = "^([\w]+)\:\s*?\[([\d\:\.]+)\]\s+([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)"
generalTextLogsRegex2 = "^\[([\d\:\.]+)\]\s+([\w\(\)\[\]\{\}\-\:\;\'\"\,\.\<\>\«\»\`\~\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\® ]+)"

iisLogsRegex = "^([\d\-]+)[ ]([\d\:]+)[ ]([\d\.\:]+)[ ](POST|PUT|PROPFIND|(?:n)?GET|OPTIONS|HEAD|ABCD|QUALYS|TRACE|SEARCH|RNDMMTD|TRACK|B(?:A)?DM(?:E)?T(?:H)?(?:O)?(?:D)?|CFYZ|DEBUG|MKCOL|INDEX|DELETE|PATCH|ACUNETIX)[ ]([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ]([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ]([\d]+)[ ]([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ]([\w\-\.\:]+)[ ]([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ]([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ]([\d]+)[ ]([\d]+)[ ]([\d]+)[ ]([\d]+)"
negativeIisLogsRegex = "^((?!(?:[\d\-]+)[ ](?:[\d\:]+)[ ](?:[\d\.\:]+)[ ](?:POST|PUT|PROPFIND|(?:n)?GET|OPTIONS|HEAD|ABCD|QUALYS|TRACE|SEARCH|RNDMMTD|TRACK|B(?:A)?DM(?:E)?T(?:H)?(?:O)?(?:D)?|CFYZ|DEBUG|MKCOL|INDEX|DELETE|PATCH|ACUNETIX)[ ](?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ](?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ](?:[\d]+)[ ](?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ](?:[\w\-\.\:]+)[ ](?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ](?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ](?:[\d]+)[ ](?:[\d]+)[ ](?:[\d]+)[ ](?:[\d]+)).*)"
nonMatchedIisLogsRegex = "^([\d\-]+)(.+)"
japaneseIisLogsRegex = "^([\d\-]+)[ ]([\d\:]+)(.+)[ ]([\d]+)"

fullErrorDumpLogsRegex = "^(LifeTime.*?(?:\r\n|\n).*?Platform.*?(?:\r\n|\n))(?:\r\n|\n)(\=.*?(?:[\w ]+)\=.*?(?:\r\n|\n)(?:(?:.+)?(?:\r\n|\n)){1,})"

nonMatchedPath = os.getcwd() + "\\nonmatched_valid_lines\\file.txt"
tempFilePath = os.getcwd() + "\\tempFile.txt"

start = datetime.now()

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
    #search for the files with the raw data in the specified directory
    for root, subFolders, files in os.walk(directoryPath):
        for s in subFolders:
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

def searchDirectory2(directoryPath, myList):
    #search for the files with the filtered data
    for root, subFolders, files in os.walk(directoryPath):
        for f in files:
            fileName, fileExt = os.path.splitext(f)
            if not fileExt + "\n" in myList:
                myList.append(fileExt + "\n")

    for ext in myList:
        if ext.strip() == ".log":
            sortLOGFile(os.getcwd() + "\\filtered_data_files")

def createFolder(fldPath):
    fldName = os.getcwd() + fldPath
    if not os.path.exists(os.path.dirname(fldName)):
        os.makedirs(os.path.dirname(fldName))

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

                            JPText = JPdate + " " + JPtime + "|" + JPmessage + "|" + JPstack + "|" + JPmoduleName + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPactionName + "|" + JPentryPointName + "|" + JPserver + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPuserID + "|" + JPsessionID + "|" + JPenvironmentInformation + "|" + JPiD + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "ErrorLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def readGeneralLogs(searchLines, _fromDate, _toDate):
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

                            JPText = JPdate + " " + JPtime + "|" + JPmessage + "|" + JPmessageType + "|" + JPmoduleName + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPactionName + "|" + JPentryPointName + "|" + JPclientIP + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPuserID + "|" + JPsessionID + "|" + JPerrorID + "|" + JPrequestKey + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "GeneralLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def readIntegrationsLogs(searchLines, _fromDate, _toDate):
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

                            JPText  = JPdate + " " + JPtime + "|" + JPduration + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPactionName + "|" + JPactionType + "|" + JPsource + "|" + JPendpoint + "|" + JPexecutedBy + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPerrorID + "|" + JPrequestKey + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "IntegrationsLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def readMobileRequestsLogs(searchLines, _fromDate, _toDate):
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

                            JPText = JPdate + " " + JPtime + "|" + JPduration + "|" + JPscreen + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPsource + "|" + JPendpoint + "|" + JPexecutedBy + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPloginID + "|" + JPuserID + "|" + JPcycle + "|" + JPerrorID + "|" + JPrequestKey + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "MobileRequestsLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def readTimerLogs(searchLines, _fromDate, _toDate):
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

                            JPText = JPdate + " " + JPtime + "|" + JPduration + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPexecutedBy + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPcyclicJobName + "|" + JPcyclicJobKey + "|" + JPshouldHaveRunAt + "|" + JPnextRun + "|" + JPerrorID + "|" + JPrequestKey + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "TimerLog -> " + nonMatchedLine2

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def readEmailLogs(searchLines, _fromDate, _toDate):
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
            name = match.group(2)
            sent = match.group(3)
            lastError = match.group(4)#null
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
            time = created[12:19]
            time = time.replace(".", "")

            _date = datetime.strptime(date, "%Y-%m-%d").date()

            if _fromDate <= _date <= _toDate:

                if lastError == None:
                    lastError = " "

                if cc == None:
                    cc = " "

                if bcc == None:
                    bcc = " "

                if subject == None:
                    subject = " "

                outText = date + " " + time + "|" + sent + "|" + lastError + "|" + from_ + "|" + to + "|" + subject + "|" + cc + "|" + bcc + "|" + name + "|" + size + "|" + messageID + "|" + activity + "|" + emailDefinition + "|" + storeContent + "|" + isTestEmail + "|" + iD + "|" + tenantID + "\n"

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
                            JPname = JPRegex.group(2)
                            JPsent = JPRegex.group(3)
                            JPlastError = JPRegex.group(4)#null
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

                            if JPlastError == None:
                                JPlastError = " "

                            if JPcc == None:
                                JPcc = " "

                            if JPbcc == None:
                                JPbcc = " "

                            if JPsubject == None:
                                JPsubject = " "

                            JPText = JPdate + " " + JPtime + "|" + JPsent + "|" + JPlastError + "|" + JPfrom_ + "|" + JPto + "|" + JPsubject + "|" + JPcc + "|" + JPbcc + "|" + JPname + "|" + JPsize + "|" + JPmessageID + "|" + JPactivity + "|" + JPemailDefinition + "|" + JPstoreContent + "|" + JPisTestEmail + "|" + JPiD + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "EmailLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def readExtensionLogs(searchLines, _fromDate, _toDate):
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

                            JPText = JPdate + " " + JPtime + "|" + JPduration + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPactionName + "|" + JPexecutedBy + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPusername + "|" + JPuserID + "|" + JPsessionID + "|" + JPextensionID + "|" + JPextensionName + "|" + JPerrorID + "|" + JPrequestKey + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "ExtensionLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def readServiceActionLogs(searchLines, _fromDate, _toDate):
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

                            JPText = JPdate + " " + JPtime + "|" + JPduration + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPactionName + "|" + JPsource + "|" + JPentrypointName + "|" + JPendpoint + "|" + JPexecutedBy + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPusername + "|" + JPloginID + "|" + JPuserID + "|" + JPsessionID + "|" + JPerrorID + "|" + JPrequestKey + "|" + JPoriginalRequestKey + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "ServiceActionLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

def readScreenLogs(searchLines, _fromDate, _toDate):
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

                            JPText = JPdate + " " + JPtime + "|" + JPduration + "|" + JPscreen + "|" + JPscreenType + "|" + JPapplicationName + "|" + JPapplicationKey + "|" + JPactionName + "|" + JPaccessMode + "|" + JPexecutedBy + "|" + JPclientIP + "|" + JPeSpaceName + "|" + JPeSpaceID + "|" + JPuserID + "|" + JPsessionID + "|" + JPsessionRequests + "|" + JPsessionBytes + "|" + JPviewstateBytes + "|" + JPmsisdn + "|" + JPrequestKey + "|" + JPtenantID + "\n"

                            tempFile.writelines(JPText)

                    else:
                        nonMatchedOutText = "ScreenLog -> " + nonMatchedLine2 + "\n"

                        myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

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
    #split the fields and rearrange them to combine them all later
    regex1 = re.findall(serviceStudioReportsDetailsRegex, searchLines3)
    if regex1:
        findAllString = ' '.join([str(elm) for elm in regex1])
        findAllString = findAllString.replace("|", "")
        findAllString = findAllString.replace("('", "")
        findAllString = findAllString.replace("')", "")
        findAllString = findAllString.replace("', '", " ")
        outText = fromDate + " 00:00:00|||" + findAllString + "\n"
        myLinesFromDateRange.append(outText)
            
    regex2 = re.findall(serviceStudioReportsOperationsLogsRegex, searchLines3)
    if regex2:
        findAllString2 = '\n'.join([str(elm2) for elm2 in regex2])
        findAllString2 = findAllString2.replace("|", "")
        findAllString2 = findAllString2.replace("('", "")
        findAllString2 = findAllString2.replace("')", "")
        findAllString2 = findAllString2.replace("(\"", "")
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
                numberOfOccurrences = match3.group(3)
                actionName = match3.group(4)
                message = match3.group(5)#null

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

                    if message == None:
                        message = " "
                        outText2 = _date + " " + _time.strip() + "|" + numberOfOccurrences + "|" + actionName.strip() + "|" + message + "\n"
                        myLinesFromDateRange.append(outText2)
                    else:            
                        outText2 = _date + " " + _time.strip() + "|" + numberOfOccurrences + "|" + actionName.strip() + "|" + message.strip() + "\n"
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

                                JPText = _JPDate + " " + _JPTime + "|||" + JPTail.strip() + "\n"

                                myLinesFromDateRange.append(JPText)

                        else:
                            nonMatchedOutText = "ServiceStudioReport -> " + nonMatchedLine2.strip() + "\n"

                            myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

        #delete the temporary file that was created
        os.remove(tempFilePath)

def readGeneralTextLogs(searchLines, _fromDate, _toDate):
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

    if len(myNonMatchedValidLinesFromDateRange) > 0:
        createFolder("\\nonmatched_valid_lines\\")
        with codecs.open(nonMatchedPath, "a+", "utf-8", "ignore") as linesFromDateRange2:
            linesFromDateRange2.seek(0)
            if len(linesFromDateRange2.read(100)) > 0:
                linesFromDateRange2.writelines("\n")
            linesFromDateRange2.writelines(myNonMatchedValidLinesFromDateRange)
        del myNonMatchedValidLinesFromDateRange[:]

    if len(myLinesFromDateRange) > 0:
        createFolder("\\filtered_data_files\\")
        if "ErrorLog" in filename:
            outFilename = "error_logs" + ext
        elif "GeneralLog" in filename:
            outFilename = "general_logs" + ext
        elif "IntegrationsLog" in filename:
            outFilename = "integrations_logs" + ext
        elif "TimerLog" in filename:
            outFilename = "timer_logs" + ext
        elif "EmailLog" in filename:
            outFilename = "email_logs" + ext
        elif "ExtensionLog" in filename:
            outFilename = "extension_logs" + ext
        elif "MobileRequestsLog" in filename:
            outFilename = "mobile_requests_logs" + ext
        elif "ServiceActionLog" in filename:
            outFilename = "service_action_logs" + ext
        elif "ScreenLog" in filename:
            outFilename = "screen_logs" + ext
        elif "troubleshootingreport" in filename.lower():
            outFilename = "bpt_troubleshootingreport_logs" + ext

        myLinesFromDateRange.sort()
        #remove duplicate records from the list
        myFinalLinesFromDateRange = list(set(myLinesFromDateRange))
        myFinalLinesFromDateRange.sort()

        with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
            linesFromDateRange.seek(0)
            if len(linesFromDateRange.read(100)) > 0:
                linesFromDateRange.writelines("\n")
            linesFromDateRange.writelines(myFinalLinesFromDateRange)
        del myLinesFromDateRange[:]
        del myFinalLinesFromDateRange[:]
    else:
        print("No data was found within the specified date range.")

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
            searchLines2 = '|'.join(searchLines.splitlines())
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
        with codecs.open(nonMatchedPath, "a+", "utf-8", "ignore") as linesFromDateRange2:
            linesFromDateRange2.seek(0)
            if len(linesFromDateRange2.read(100)) > 0:
                linesFromDateRange2.writelines("\n")
            linesFromDateRange2.writelines(myNonMatchedValidLinesFromDateRange)
        del myNonMatchedValidLinesFromDateRange[:]

    if len(myLinesFromDateRange) > 0:
        createFolder("\\filtered_data_files\\")
        if "iosbuildlog" in filename.lower():
            outFilename = "iOS_build_logs" + ext
        elif "androidbuildlog" in filename.lower():
            outFilename = "android_build_logs" + ext
        elif "studio" in filename.lower() and "report" in filename.lower():
            outFilename = "service_studio_report" + ext
        elif "general" in filename.lower():
            outFilename = "general_text_logs" + ext
        elif "fullerrordump" == filename.lower():
            outFilename = "full_error_dump_logs" + ext

        if not "infrastructurereport" in absolutePath.lower() and not "stagingreport" in absolutePath.lower() and not "userpermissionsreport" in absolutePath.lower():
            myLinesFromDateRange.sort()
            #remove duplicate records from the list
            myFinalLinesFromDateRange = list(set(myLinesFromDateRange))
            myFinalLinesFromDateRange.sort()

            with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
                linesFromDateRange.seek(0)
                if len(linesFromDateRange.read(100)) > 0:
                    linesFromDateRange.writelines("\n")
                linesFromDateRange.writelines(myFinalLinesFromDateRange)
            del myLinesFromDateRange[:]
            del myFinalLinesFromDateRange[:]

        else:
            with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
                linesFromDateRange.seek(0)
                if len(linesFromDateRange.read(100)) > 0:
                    linesFromDateRange.writelines("\n")
                linesFromDateRange.writelines(myLinesFromDateRange)
            del myLinesFromDateRange[:]
                
    else:
        if not "errordump" == filename.lower():
            print("No data was found within the specified date range.")

    print("Closing: " + filenameWithExt)

def logFile(absolutePath, filenameWithExt, ext, _fromDate, _toDate):
    #pattern for filenames: u_exYYMMDD.log
    print("Reading: " + filenameWithExt)
    
    outText = ""
    outText2 = ""
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
                        outText = date + " " + time + "|" + timeTaken + "|" + httpCode + "|" + httpSubCode + "|" + windowsCode + "|" + clientIP + "|" + serverIP + "|" + serverPort + "|" + method + "|" + uriStem + "|" + uriQuery + "|" + username + "|" + browser + "|" + referrer + "\n"
                        myLinesFromDateRange.append(outText)

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

                                    outText2 = date + " " + time + "|" + timeTaken + "|" + httpCode + "|" + httpSubCode + "|" + windowsCode + "|" + clientIP + "|" + serverIP + "|" + serverPort + "|" + method + "|" + uriStem + "|" + uriQuery + "|" + username + "|" + browser + "|" + referrer + "\n"
                                    myLinesFromDateRange.append(outText2)

                                if not regex2:
                                    nonMatchedOutText = "IISLog -> " + newNonMatchedLine2 + "\n"

                                    myNonMatchedValidLinesFromDateRange.append(nonMatchedOutText)

        except ValueError as valError:
            print("\nThe customer altered the original IIS logs.\n\nPossible reasons:\n1- The customer added a column on the logs.\n" +
                  "2- The customer switched the columns of the logs.\n")

    if len(myNonMatchedValidLinesFromDateRange) > 0:
        createFolder("\\nonmatched_valid_lines\\")
        with codecs.open(nonMatchedPath, "a+", "utf-8", "ignore") as linesFromDateRange2:
            linesFromDateRange2.seek(0)
            if len(linesFromDateRange2.read(100)) > 0:
                linesFromDateRange2.writelines("\n")
            linesFromDateRange2.writelines(myNonMatchedValidLinesFromDateRange)
        del myNonMatchedValidLinesFromDateRange[:]

    if len(myLinesFromDateRange) > 0:
        createFolder("\\filtered_data_files\\")
        outFilename = "iis_logs" + ext

        myLinesFromDateRange.sort()
        #remove duplicate records from the list
        myFinalLinesFromDateRange = list(set(myLinesFromDateRange))
        myFinalLinesFromDateRange.sort()

        with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
            linesFromDateRange.seek(0)
            if len(linesFromDateRange.read(100)) > 0:
                linesFromDateRange.writelines("\n")
            linesFromDateRange.writelines(myFinalLinesFromDateRange)
        del myLinesFromDateRange[:]
        del myFinalLinesFromDateRange[:]
    else:
        print("No data was found within the specified date range.")

    print("Closing: " + filenameWithExt)

def sortLOGFile(directoryPath):
    print("Sorting the content from the IIS log(s)")

    outText = ""

    #sort the data by time taken to perform a task
    with codecs.open(os.getcwd() + "\\filtered_data_files\\iis_logs.txt", "r", "utf-8", "ignore") as linesFromText2:
        searchLines = linesFromText2.read()
        regex = re.compile("^([\d\-\: ]+)\|([\d]+)\|(.+)", re.MULTILINE + re.IGNORECASE)
        for match in regex.finditer(searchLines):
            dateTime = match.group(1)
            timeTaken = match.group(2)
            tail = match.group(3)

            outText = timeTaken + "|" + dateTime + "|" + tail + "\n"
            _timeTaken = int(timeTaken)
            _date_time = datetime.strptime(dateTime, "%Y-%m-%d %H:%M:%S")

            if not outText in myLOGlines:
                myLOGlines.append(outText)

            if not _timeTaken in myTimeTaken:
                myTimeTaken.append(_timeTaken)

            myDateTimes.append(dateTime)
            myTimesTaken.append(_timeTaken)

    myTimeTaken.sort()

    minimum = min(myTimeTaken)
    maximum = max(myTimeTaken)

    with codecs.open(os.getcwd() + "\\filtered_data_files\\iis_logs_timetaken.txt", "w", "utf-8", "ignore") as linesToText3:
        while maximum >= minimum:
            if maximum in myTimeTaken:
                for t, tt in enumerate(myLOGlines):
                    time = [int(w) for w in myLOGlines[t].split("|") if w.isdigit()]
                    if maximum == time[0]:
                        linesToText3.writelines(myLOGlines[t])
                    t+=1
            maximum-=1

    #create a line graph with the compiled data
    createFolder("\\graphs\\")

    fig, ax = plt.subplots()
    ax.plot(myDateTimes, myTimesTaken)
    #reformat the timestamps to display the month-day hour:minutes
    myFmt = mdates.DateFormatter("%m-%d %H:%M")
    ax.xaxis.set_major_formatter(myFmt)
    #only display ten labels for the X and the Y axis
    ax.xaxis.set_major_locator(plt.MaxNLocator(10))
    ax.yaxis.set_major_locator(plt.MaxNLocator(10))
    #change the label's font size and rotate the labels from the X axis
    ax.xaxis.set_tick_params(labelsize = 6)
    ax.yaxis.set_tick_params(labelsize = 6)
    plt.xticks(rotation = 45)
    #save a copy of the line graph and display a dialog window with the interactive line graph
    plt.savefig(os.getcwd() + "\\graphs\\line_graph.png")
    #plt.show()

    del myDateTimes[:]
    del myTimesTaken[:]

    with codecs.open(os.getcwd() + "\\filtered_data_files\\iis_logs_timetaken.txt", "r", "utf-8", "ignore") as linesFromText4:
        searchLines3 = linesFromText4.read()
        regex2 = re.compile("^([\d]+)\|([\d\-\: ]+)\|.+", re.MULTILINE + re.IGNORECASE)
        for match2 in regex2.finditer(searchLines3):
            time_taken_ = match2.group(1)
            date_time_ = match2.group(2)

            myDateTimes.append(date_time_)
            myTimesTaken.append(time_taken_)

    with codecs.open(os.getcwd() + "\\graphs\\line_graph.html", "w", "utf-8", "ignore") as linesToText4:
        linesToText4.writelines("<!doctype html>\n<body>\n<div id=\"container\" style=\"width: 500px; height: 400px;\"></div>\n<script src=\"https://cdn.anychart.com/releases/v8/js/anychart-base.min.js\" type=\"text/javascript\"></script>\n<script>\n\tanychart.onDocumentReady(function() {\n\t\t//create CSV string\n\t\tvar csvString = 'Timestamp;Time*' +\n")
        for x, elm in enumerate(myDateTimes):
            if x == len(myDateTimes) - 1:
                linesToText4.writelines("\t\t\t'" + myDateTimes[x] + "|" + myTimesTaken[x] + "';\n\n")
            else:
                linesToText4.writelines("\t\t\t'" + myDateTimes[x] + "|" + myTimesTaken[x] + "*' +\n")
            x+=1
        linesToText4.writelines("\t\t//create an area chart\n\t\tvar chart = anychart.line();\n\n\t\t//create the area series based on CSV data\n\t\tchart.line(csvString, {ignoreFirstRow: true, columnsSeparator: \"|\", rowsSeparator: \"*\"});\n\n\t\t//display a chart\n\t\tchart.container('container').draw();\n\n\t\tchart.xAxis().title(\"Timestamp\");\n\t\tchart.yAxis().title(\"Time Taken\");\n\n\t\t//set ticks interval\n\t\tchart.yScale().ticks().interval(1000);\n\n\t\t//set minor ticks interval\n\t\tchart.yScale().minorTicks().interval(500);\n\n\t\t//settings\n\t\tchart.tooltip().fontColor(\"red\");\n\n\t\t//tooltip padding for all series on a chart\n\t\tchart.tooltip().padding().left(20);\n\n\t\t//background color\n\t\tchart.tooltip().background().fill(\"black\");\n});\n</script>\n</body>\n</html>")

    print("Saved the line graphs from the IIS log(s)")     
    del myLOGlines[:]
    del myTimeTaken[:]
    del myDateTimes[:]
    del myTimesTaken[:]
    outText = ""

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
                    message = parseXMLtoString(xmlParse, ".//Data/text()")

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

                        message = message.replace("<string>", "")
                        message = message.replace("</string>", "")
                        newMessage = normalizeLines(message)
                        newMessage = newMessage.translate(replacementDict)
                                    
                        outText = date + " " + time + "|" + level + "|" + newMessage.strip() + "|" + task + "|" + computer + "|" + providerName + "|" + qualifiers + "|" + eventID + "|" + eventRecordID + "|" + keywords + "\n"

                        tempFile.writelines(outText)

    with codecs.open(tempFilePath, "r", "utf-8", "ignore") as tempFile2:
        myLinesFromDateRange = tempFile2.readlines()
            
    #delete the temporary file that was created
    os.remove(tempFilePath)          

    if len(myLinesFromDateRange) > 0:
        createFolder("\\filtered_data_files\\")
        outFilename = "windows_" + channel.lower() + "_event_viewer_logs" + ext

        myLinesFromDateRange.sort()
        #remove duplicate records from the list
        myFinalLinesFromDateRange = list(set(myLinesFromDateRange))
        myFinalLinesFromDateRange.sort()

        with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
            linesFromDateRange.seek(0)
            if len(linesFromDateRange.read(100)) > 0:
                linesFromDateRange.writelines("\n")
            linesFromDateRange.writelines(myFinalLinesFromDateRange)
        del myLinesFromDateRange[:]
        del myFinalLinesFromDateRange[:]
    else:
        print("No data was found within the specified date range.")

    print("Closing: " + filenameWithExt)
    
#the following line displays the absolute path of the folder where Python was installed on your PC. Uncomment only when necessary.
#print("PYTHON WAS INSTALLED HERE -> " + os.path.dirname(sys.executable))

num_args = len(sys.argv)

if num_args != 4:
    print("Error:\nTotal arguments passed: " + str(num_args) +
          "\n4 arguments needed: log_parser.py directoryPath fromDate(YYYY-MM-DD) toDate(YYYY-MM-DD)" +
          "\nPlease try again.")
else:
    directoryPath = sys.argv[1]
    fromDate = sys.argv[2]
    toDate = sys.argv[3]

    _fromDate = datetime.strptime(fromDate, "%Y-%m-%d").date()
    _toDate = datetime.strptime(toDate, "%Y-%m-%d").date()

    if _fromDate > _toDate:
        print("The \"from date\" cannot be greater than the \"to date\"\nPlease try again.")
    else:
        searchDirectory(directoryPath, _fromDate, _toDate)
        searchDirectory2(directoryPath, myFileExt)

        if os.path.exists(nonMatchedPath):
            print("\nALERT!\nThere were valid lines that did not match the logic.\n" +
                  "A file has been created in your current workig directory: \"" + os.getcwd() + "\" under the \"nonmatched_valid_lines\" folder.\n" +
                  "Please go to the Slack channel #log-parser-feedback and post the generated file for further review.\nALERT!")

end = datetime.now()
print("\nElapsed time: {0}".format(end-start))
