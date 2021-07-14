import os
import re
import sys
import csv
import codecs
import shutil
import openpyxl
from datetime import datetime
import matplotlib.pyplot as plt
import matplotlib.dates as mdates

"""
OutSystems Log Parser

XSLX:
LOG DATE_TIME SENT LAST_ERROR DURATION MESSAGE MESSAGE_TYPE STACK FROM TO SUBJECT CC BCC SCREEN SCREEN_TYPE
MODULE_NAME APPLICATION_NAME APPLICATION_KEY NAME ACTION_NAME ACTION_TYPE ACCESS_MODE SOURCE ENTRYPOINT_NAME
ENDPOINT EXECUTED_BY SERVER CLIENT_IP ESPACE_NAME ESPACE_ID USERNAME LOGIN_ID USER_ID SESSION_ID SESSION_REQUESTS
SESSION_BYTES VIEW_STATE_BYTES EXTENSION_ID EXTENSION_NAME CYCLE CYCLIC_JOB_NAME CYCLIC_JOB_KEY
SHOULD_HAVE_RUN_AT NEXT_RUN ENVIRONMENT_INFORMATION SIZE MS_IS_DN ERROR_ID MESSAGE_ID ACTIVITY EMAIL_DEFINITION
STORE_CONTENT IS_TEST_EMAIL REQUEST_KEY ORIGINAL_REQUEST_KEY ID TENANT_ID

ScreenLog = TraditionalWebRequests
MobileRequestsLog = ScreenRequests

LOG:
DATE_TIME TIME_TAKEN HTTP_CODE HTTP_SUBCODE WINDOWS_ERROR_CODE CLIENT_IP SERVER_IP SERVER_PORT
METHOD URI_STEM URI_QUERY USERNAME BROWSER REFERRER

TIME_TAKEN DATE_TIME HTTP_CODE HTTP_SUBCODE WINDOWS_ERROR_CODE CLIENT_IP SERVER_IP SERVER_PORT
METHOD URI_STEM URI_QUERY USERNAME BROWSER REFERRER

TXT:
LOG DATE_TIME MESSAGE_TYPE ACTION_NAME MESSAGE

EVTX:
LOG DATE_TIME LEVEL SOURCE EVENT_ID TASK CATEGORY MESSAGE

1) Filter data based on a date range
2) Read the input files and rearrange the columns.
3) Filtered outputs should be TXT files using the pipe ("|") as the delimiter and their filenames should be the logs they represent.
4) All the "datetime" fields from the logs should be in the following format: YYYY-MM-DD hh:mm:ss
5) Combine all the data from all the output files, sort them by time, and then store the data into one single file.
"""

myLinesFromDateRange = []
myTempXLSXlines = []
myXLSXlines = []
myLOGlines = []
myLOGlines2 = []
myTimeTaken = []
myTXTlines = []
myFileExt = []
myDateTimes = []
myTimesTaken = []
myXLSXfiles = [
    "error_logs",
    "general_logs",
    "integrations_logs",
    "timer_logs",
    "email_logs",
    "extension_logs",
    "mobile_requests_logs",
    "service_action_logs",
    "screen_logs"
    ]
numOfLines = 6000
constant = 6000

start = datetime.now()

def searchDirectory(directoryPath):
    #search for the files with the raw data in the specified directory
    for root, subFolders, files in os.walk(directoryPath):
        for f in files:
            absolutePathOfFile = os.path.join(root, f)
            filePathWithoutFilename = os.path.split(absolutePathOfFile)[0]
            filenameWithExt = os.path.split(absolutePathOfFile)[1]
            filenameWithoutExt = os.path.splitext(filenameWithExt)[0]
            extension = os.path.splitext(filenameWithExt)[1]

            if extension == ".xlsx":
                xlsxFile(absolutePathOfFile, filePathWithoutFilename, filenameWithoutExt, ".txt", fromDate, toDate)
            elif extension == ".txt":
                txtFile(absolutePathOfFile, filenameWithoutExt, filenameWithExt, extension, fromDate, toDate)
            elif extension == ".log":
                logFile(absolutePathOfFile, filenameWithExt, ".txt", fromDate, toDate)

def searchDirectory2(directoryPath, myList):
    #search for the files with the filtered data
    for root, subFolders, files in os.walk(directoryPath):
        for f in files:
            fileName, fileExt = os.path.splitext(f)
            if not fileExt + "\n" in myList:
                myList.append(fileExt + "\n")

    for ext in myFileExt:
        if ext.strip() == ".xlsx":
            combineXLSXFile(os.getcwd() + "\\filtered_data_files")
        elif ext.strip() == ".txt":
            combineTXTFile(os.getcwd() + "\\filtered_data_files")
            combineEVTXFile(os.getcwd() + "\\filtered_data_files")
        elif ext.strip() == ".log":
            combineLOGFile(os.getcwd() + "\\filtered_data_files")

def createFolder(fldPath):
    fldName = os.getcwd() + fldPath
    if not os.path.exists(os.path.dirname(fldName)):
        os.makedirs(os.path.dirname(fldName))

def splitFiles(numOfLines, constant, maxLines, cycleNum, inputTxtFile, ext, outputTxtFile, myList):
    div = round(maxLines/constant)
    partial = constant*div

    with codecs.open(inputTxtFile, "r", "utf-8", "ignore") as inp:
        searchLines = inp.readlines()
        searchLines = searchLines[1:]
        for x, line in enumerate(searchLines):
            if x < numOfLines:
                myList.append(line)
            #make sure maxLines is greater or equal than numOfLines
            #otherwise, the program will never reach this point
            if x == numOfLines:
                myList.append(line)
                cycleNum+=1
                with codecs.open(outputTxtFile + str(cycleNum) + ext, "w", "utf-8", "ignore") as out:
                    out.writelines(myList)
                del myList[:]
                numOfLines+=constant
                #final cycle
                if numOfLines > partial:
                    numOfLines = maxLines
            x+=1
    return cycleNum

def readSplitFiles(cycleNum, filename, ext, pattern, _fromDate, _toDate):
    outText = ""
    count = 1
    while count <= cycleNum:
        with codecs.open(os.getcwd() + "\\split_data\\file_" + str(count) + ext, "r", "utf-8", "ignore") as linesFromText:
            searchLines = linesFromText.read()
            #split the fields and rearrange them to combine them all later
            if filename == "ErrorLog":
                regex = re.compile(pattern, re.MULTILINE + re.IGNORECASE)
                for match in regex.finditer(searchLines):
                    tenantID = match.group(1)
                    iD = match.group(2)
                    timestamp = match.group(3)
                    sessionID = match.group(4)#null
                    userID = match.group(5)
                    eSpaceID = match.group(6)
                    message = match.group(7)
                    stack = match.group(8)#null
                    moduleName = match.group(9)#null
                    server = match.group(10)
                    environmentInformation = match.group(11)#null
                    entryPointName = match.group(12)#null
                    actionName = match.group(13)#null
                    requestKey = match.group(14)
                    eSpaceName = match.group(15)
                    applicationName = match.group(16)
                    applicationKey = match.group(17)
                                
                    date = timestamp[0:10]
                    time = timestamp[12:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if sessionID == None:
                            sessionID = " "

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

                        outText = "Error|" + date + " " + time + "||||" + message + "||" + stack + "||||||||" + moduleName + "|" + applicationName + "|" + applicationKey + "||" + actionName + "||||" + entryPointName + "|||" + server + "||" + eSpaceName + "|" + eSpaceID + "|||" + userID + "|" + sessionID + "|||||||||||" + environmentInformation + "|||||||||||" + iD + "|" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif filename == "GeneralLog":
                regex = re.compile(pattern, re.MULTILINE + re.IGNORECASE)
                for match in regex.finditer(searchLines):
                    tenantID = match.group(1)
                    timestamp = match.group(2)
                    sessionID = match.group(3)#null
                    userID = match.group(4)
                    eSpaceID = match.group(5)
                    errorID = match.group(6)#null
                    message = match.group(7)
                    messageType = match.group(8)#null
                    moduleName = match.group(9)
                    requestKey = match.group(10)#null
                    entryPointName = match.group(11)#null
                    actionName = match.group(12)#null
                    clientIP = match.group(13)#null
                    eSpaceName = match.group(14)#null
                    applicationName = match.group(15)#null
                    applicationKey = match.group(16)#null
                    username = match.group(17)#null
                                
                    date = timestamp[0:10]
                    time = timestamp[12:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if sessionID == None:
                            sessionID = " "

                        if errorID == None:
                            errorID = " "

                        if messageType == None:
                            messageType = " "

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

                        outText = "General|" + date + " " + time + "||||" + message + "|" + messageType + "|||||||||" + moduleName + "|" + applicationName + "|" + applicationKey + "||" + actionName + "||||" + entryPointName + "||||" + clientIP + "|" + eSpaceName + "|" + eSpaceID + "|||" + userID + "|" + sessionID + "||||||||||||||" + errorID + "||||||" + requestKey + "|||" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif filename == "IntegrationsLog":
                regex = re.compile(pattern, re.MULTILINE + re.IGNORECASE)
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
                    eSpaceName = match.group(12)
                    applicationName = match.group(13)
                    applicationKey = match.group(14)
                                
                    date = timestamp[0:10]
                    time = timestamp[12:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if source == None:
                            source = " "

                        if endpoint == None:
                            endpoint = " "

                        if errorID == None:
                            errorID = " "

                        outText = "Integrations|" + date + " " + time + "|||" + duration + "||||||||||||" + applicationName + "|" + applicationKey + "||" + actionName + "|" + actionType + "||" + source + "||" + endpoint + "|" + executedBy + "|||" + eSpaceName + "|" + eSpaceID + "||||||||||||||||||" + errorID + "||||||" + requestKey + "|||" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif filename == "MobileRequestsLog":
                regex = re.compile(pattern, re.MULTILINE + re.IGNORECASE)
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
                    loginID = match.group(12)
                    userID = match.group(13)
                    eSpaceName = match.group(14)
                    applicationName = match.group(15)
                    applicationKey = match.group(16)
                                
                    date = timestamp[0:10]
                    time = timestamp[12:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if errorID == None:
                            errorID = " "

                        outText = "ScreenRequests|" + date + " " + time + "|||" + duration + "|||||||||" + screen + "|||" + applicationName + "|" + applicationKey + "|||||" + source + "||" + endpoint + "|" + executedBy + "|||" + eSpaceName + "|" + eSpaceID + "||" + loginID + "|" + userID + "|||||||" + cycle + "||||||||" + errorID + "||||||" + requestKey + "|||" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif filename == "TimerLog":
                regex = re.compile(pattern, re.MULTILINE + re.IGNORECASE)
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
                    requestKey = match.group(10)
                    eSpaceName = match.group(11)
                    applicationName = match.group(12)
                    applicationKey = match.group(13)
                    cyclicJobName = match.group(14)
                                
                    date = timestamp[0:10]
                    time = timestamp[12:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if errorID == None:
                            errorID = " "

                        outText = "Timer|" + date + " " + time + "|||" + duration + "||||||||||||" + applicationName + "|" + applicationKey + "||||||||" + executedBy + "|||" + eSpaceName + "|" + eSpaceID + "|||||||||||" + cyclicJobName + "|" + cyclicJobKey + "|" + shouldHaveRunAt + "|" + nextRun + "||||" + errorID + "||||||" + requestKey + "|||" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif filename == "EmailLog":
                regex = re.compile(pattern, re.MULTILINE + re.IGNORECASE)
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
                    subject = match.group(10)
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

                        outText = "Email|" + date + " " + time + "|" + sent + "|" + lastError + "|||||" + from_ + "|" + to + "|" + subject + "|" + cc + "|" + bcc + "||||||" + name + "|||||||||||||||||||||||||||" + size + "|||" + messageID + "|" + activity + "|" + emailDefinition + "|" + storeContent + "|" + isTestEmail + "|||" + iD + "|" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif filename == "ExtensionLog":
                regex = re.compile(pattern, re.MULTILINE + re.IGNORECASE)
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
                    username = match.group(16)
                                
                    date = timestamp[0:10]
                    time = timestamp[12:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if errorID == None:
                            errorID = " "

                        outText = "Extension|" + date + " " + time + "|||" + duration + "||||||||||||" + applicationName + "|" + applicationKey + "||" + actionName + "||||||" + executedBy + "|||" + eSpaceName + "|" + eSpaceID + "|" + username + "||" + userID + "|" + sessionID + "|" + extensionID + "|" + extensionName + "|||||||||" + errorID + "||||||" + requestKey + "|||" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif filename == "ServiceActionLog":
                regex = re.compile(pattern, re.MULTILINE + re.IGNORECASE)
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
                    time = timestamp[12:19]
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

                        outText = "ServiceAction|" + date + " " + time + "|||" + duration + "||||||||||||" + applicationName + "|" + applicationKey + "||" + actionName + "|||" + source + "|" + entrypointName + "|" + endpoint + "|" + executedBy + "|||" + eSpaceName + "|" + eSpaceID + "|" + username + "|" + loginID + "|" + userID + "|" + sessionID + "|||||||||||" + errorID + "||||||" + requestKey + "|" + originalRequestKey + "||" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif filename == "ScreenLog":
                regex = re.compile(pattern, re.MULTILINE + re.IGNORECASE)
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
                    actionName = match.group(16)
                    clientIP = match.group(17)
                    eSpaceName = match.group(18)
                    applicationName = match.group(19)
                    applicationKey = match.group(20)
                        
                    date = timestamp[0:10]
                    time = timestamp[12:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if msisdn == None:
                            msisdn = " "

                        outText = "TraditionalWebRequests|" + date + " " + time + "|||" + duration + "|||||||||" + screen + "|" + screenType + "||" + applicationName + "|" + applicationKey + "||" + actionName + "||" + accessMode + "||||" + executedBy + "||" + clientIP + "|" + eSpaceName + "|" + eSpaceID + "|||" + userID + "|" + sessionID + "|" + sessionRequests + "|" + sessionBytes + "|" + viewstateBytes + "||||||||||" + msisdn + "||||||" + requestKey + "|||" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)
        count+=1

def xlsxFile(absolutePath, relativePath, filename, ext, fromDate, toDate):
    #convert the XLSX file to TXT to manipulate the data in an easier way
    print("Converting " + filename + ".xlsx to .txt format")
    
    xlsx = openpyxl.load_workbook(absolutePath, read_only = True)
    sheet = xlsx.active
    data = sheet.rows

    with codecs.open(relativePath + "\\" + filename + ext, "w", "utf-8", "ignore") as linesFromXLSX:
        for row in data:
            rowList = list(row)
            for i in range(len(rowList)):
                if i == len(rowList) - 1:
                    line = str(rowList[i].value)
                    line = line.replace("\t", " ")
                    line = line.replace("\r\n", " ")
                    line = line.replace("\n", " ")
                    linesFromXLSX.write(line)
                    linesFromXLSX.write("\n")
                else:
                    line = str(rowList[i].value)
                    line = line.replace("\t", " ")
                    line = line.replace("\r\n", " ")
                    line = line.replace("\n", " ")
                    linesFromXLSX.write(line + "|")

    xlsxtxtFile(relativePath + "\\" + filename + ext, filename, ext, fromDate, toDate)

def xlsxtxtFile(absolutePath, filename, ext, fromDate, toDate):
    print("Reading: " + filename + ".txt")

    del myLinesFromDateRange[:]
    outText = ""
    cycleNum = 0

    _fromDate = datetime.strptime(fromDate, "%Y-%m-%d").date()
    _toDate = datetime.strptime(toDate, "%Y-%m-%d").date()

    with codecs.open(absolutePath, "r", "utf-8", "ignore") as max_lines:
        #total number of lines from the data
        maxLines = sum(1 for line in max_lines)-1
        #for large files, split the data into several files
        if maxLines >= numOfLines:
            createFolder("\\split_data\\")
            cycleNum = splitFiles(numOfLines, constant, maxLines, cycleNum, absolutePath, ext, os.getcwd() + "\\split_data\\file_", myTempXLSXlines)

    if maxLines >= numOfLines:
        if "ErrorLog" in filename:
            readSplitFiles(cycleNum, "ErrorLog", ext, r"^([\d]+)\|([\w\-]+)\|([\d\-\:\. ]+)\|([\w\/\=\+ ]+)?\|([\d]+)\|([\d]+)\|([\w\(\)\[\]\'\.\:\-\>\<\,\=\&\`\\\/\? ]+)\|([\w\(\)\[\]\'\.\,\>\<\:\-\=\&\`\\\/ ]+)?\|([\w\:\-\=\,\.\/\(\)\' ]+)?\|([\w\-\:\=\,\.\/\(\)\' ]+)\|([\w\(\)\[\]\-\:\'\,\.\>\`\<\&\=\\\/ ]+)?\|([\w]+)?\|([\w]+)?\|([\w\-]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)", _fromDate, _toDate)
        elif "GeneralLog" in filename:
            readSplitFiles(cycleNum, "GeneralLog", ext, r"^([\d]+)\|([\d\-\:\. ]+)\|([\w\+\/\=]+)?\|([\d]+)\|([\d]+)\|([\w\-]+)?\|([\w\(\)\.\:\'\- ]+)\|([\w]+)?\|([\w]+)\|([\w\-]+)?\|([\w]+)?\|([\w]+)?\|([\d\.\:]+)?\|([\w]+)?\|([\w ]+)?\|([\w\-]+)?\|([\w\@\.]+)?", _fromDate, _toDate)
        elif "IntegrationsLog" in filename:
            readSplitFiles(cycleNum, "IntegrationsLog", ext, r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d\.]+)?\|([\w\:\/\.\-]+)?\|([\w\.\(\) ]+)\|([\w\(\) ]+)\|([\d]+)\|([\w\-]+)?\|([\w]+)\|([\w\-]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)", _fromDate, _toDate)
        elif "MobileRequestsLog" in filename:
            readSplitFiles(cycleNum, "MobileRequestsLog", ext, r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\.\-\:\; ]+)\|([\w]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\d]+)\|([\w\-]+)\|([\w\/\+\=]+)\|([\d]+)\|([\w]+)\|([\w]+)\|([\w\-]+)", _fromDate, _toDate)
        elif "TimerLog" in filename:
            readSplitFiles(cycleNum, "TimerLog", ext, r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w\-]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\d\-\:\. ]+)\|([\d\-\:\. ]+)\|([\w\-]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)\|([\w]+)", _fromDate, _toDate)
        elif "EmailLog" in filename:
            readSplitFiles(cycleNum, "EmailLog", ext, r"^([\d]+)\|([\w]+)\|([\w\-\:\. ]+)\|([\w\-\:\. ]+)?\|([\d]+)\|([\w\@\.]+)\|([\w\@\.]+)\|([\w\@\.]+)?\|([\w\@\.]+)?\|([\w ]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)\|([\w]+)\|([\w]+)\|([\d]+)\|([\w\@\.]+)", _fromDate, _toDate)
        elif "ExtensionLog" in filename:
            readSplitFiles(cycleNum, "ExtensionLog", ext, r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\/\=\+]+)\|([\d]+)\|([\d]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\w\-]+)\|([\w]+)\|([\w]+)\|([\w]+)\|([\w\-]+)\|([\w\@\.]+)", _fromDate, _toDate)
        elif "ServiceActionLog" in filename:
            readSplitFiles(cycleNum, "ServiceActionLog", ext, r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)?\|([\w\=\/\+]+)\|([\d]+)\|([\w\-]+)?\|([\w]+)\|([\w\-]+)\|([\w\-]+)?\|([\w]+)\|([\d]+)\|([\d\.]+)\|([\w\\]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)\|([\w]+)?\|([\w\-]+)", _fromDate, _toDate)
        elif "ScreenLog" in filename:
            readSplitFiles(cycleNum, "ScreenLog", ext, r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\=\+\/]+)\|([\d]+)\|([\d]+)\|([\w\-\:\. ]+)?\|([\w]+)\|([\w\-]+)\|([\d]+)\|([\d]+)\|([\d]+)\|([\w]+)\|([\w\-]+)\|([\w\(\)\.]+)\|([\w\-\:\. ]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)", _fromDate, _toDate)

        #delete the temporary files and directory that were created
        os.remove(absolutePath)
        shutil.rmtree(os.getcwd() + "\\split_data")

    else:
        with codecs.open(absolutePath, "r", "utf-8", "ignore") as linesFromText:
            searchLines = linesFromText.read()
            #split the fields and rearrange them to combine them all later
            if "ErrorLog" in filename:
                regex = re.compile(r"^([\d]+)\|([\w\-]+)\|([\d\-\:\. ]+)\|([\w\/\=\+ ]+)?\|([\d]+)\|([\d]+)\|([\w\(\)\[\]\'\.\:\-\>\<\,\=\&\`\\\/\? ]+)\|([\w\(\)\[\]\'\.\,\>\<\:\-\=\&\`\\\/ ]+)?\|([\w\:\-\=\,\.\/\(\)\' ]+)?\|([\w\-\:\=\,\.\/\(\)\' ]+)\|([\w\(\)\[\]\-\:\'\,\.\>\`\<\&\=\\\/ ]+)?\|([\w]+)?\|([\w]+)?\|([\w\-]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)", re.MULTILINE + re.IGNORECASE)
                for match in regex.finditer(searchLines):
                    tenantID = match.group(1)
                    iD = match.group(2)
                    timestamp = match.group(3)
                    sessionID = match.group(4)#null
                    userID = match.group(5)
                    eSpaceID = match.group(6)
                    message = match.group(7)
                    stack = match.group(8)#null
                    moduleName = match.group(9)#null
                    server = match.group(10)
                    environmentInformation = match.group(11)#null
                    entryPointName = match.group(12)#null
                    actionName = match.group(13)#null
                    requestKey = match.group(14)
                    eSpaceName = match.group(15)
                    applicationName = match.group(16)
                    applicationKey = match.group(17)
                            
                    date = timestamp[0:10]
                    time = timestamp[12:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if sessionID == None:
                            sessionID = " "

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

                        outText = "Error|" + date + " " + time + "||||" + message + "||" + stack + "||||||||" + moduleName + "|" + applicationName + "|" + applicationKey + "||" + actionName + "||||" + entryPointName + "|||" + server + "||" + eSpaceName + "|" + eSpaceID + "|||" + userID + "|" + sessionID + "|||||||||||" + environmentInformation + "|||||||||||" + iD + "|" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif "GeneralLog" in filename:
                regex = re.compile(r"^([\d]+)\|([\d\-\:\. ]+)\|([\w\+\/\=]+)?\|([\d]+)\|([\d]+)\|([\w\-]+)?\|([\w\(\)\.\:\'\- ]+)\|([\w]+)?\|([\w]+)\|([\w\-]+)?\|([\w]+)?\|([\w]+)?\|([\d\.\:]+)?\|([\w]+)?\|([\w ]+)?\|([\w\-]+)?\|([\w\@\.]+)?", re.MULTILINE + re.IGNORECASE)
                for match in regex.finditer(searchLines):
                    tenantID = match.group(1)
                    timestamp = match.group(2)
                    sessionID = match.group(3)#null
                    userID = match.group(4)
                    eSpaceID = match.group(5)
                    errorID = match.group(6)#null
                    message = match.group(7)
                    messageType = match.group(8)#null
                    moduleName = match.group(9)
                    requestKey = match.group(10)#null
                    entryPointName = match.group(11)#null
                    actionName = match.group(12)#null
                    clientIP = match.group(13)#null
                    eSpaceName = match.group(14)#null
                    applicationName = match.group(15)#null
                    applicationKey = match.group(16)#null
                    username = match.group(17)#null
                        
                    date = timestamp[0:10]
                    time = timestamp[12:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if sessionID == None:
                            sessionID = " "

                        if errorID == None:
                            errorID = " "

                        if messageType == None:
                            messageType = " "

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

                        outText = "General|" + date + " " + time + "||||" + message + "|" + messageType + "|||||||||" + moduleName + "|" + applicationName + "|" + applicationKey + "||" + actionName + "||||" + entryPointName + "||||" + clientIP + "|" + eSpaceName + "|" + eSpaceID + "|||" + userID + "|" + sessionID + "||||||||||||||" + errorID + "||||||" + requestKey + "|||" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif "IntegrationsLog" in filename:
                regex = re.compile(r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d\.]+)?\|([\w\:\/\.\-]+)?\|([\w\.\(\) ]+)\|([\w\(\) ]+)\|([\d]+)\|([\w\-]+)?\|([\w]+)\|([\w\-]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)", re.MULTILINE + re.IGNORECASE)
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
                    eSpaceName = match.group(12)
                    applicationName = match.group(13)
                    applicationKey = match.group(14)
                        
                    date = timestamp[0:10]
                    time = timestamp[12:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if source == None:
                            source = " "

                        if endpoint == None:
                            endpoint = " "

                        if errorID == None:
                            errorID = " "

                        outText = "Integrations|" + date + " " + time + "|||" + duration + "||||||||||||" + applicationName + "|" + applicationKey + "||" + actionName + "|" + actionType + "||" + source + "||" + endpoint + "|" + executedBy + "|||" + eSpaceName + "|" + eSpaceID + "||||||||||||||||||" + errorID + "||||||" + requestKey + "|||" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif "MobileRequestsLog" in filename:
                regex = re.compile(r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\.\-\:\; ]+)\|([\w]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\d]+)\|([\w\-]+)\|([\w\/\+\=]+)\|([\d]+)\|([\w]+)\|([\w]+)\|([\w\-]+)", re.MULTILINE + re.IGNORECASE)
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
                    loginID = match.group(12)
                    userID = match.group(13)
                    eSpaceName = match.group(14)
                    applicationName = match.group(15)
                    applicationKey = match.group(16)
                        
                    date = timestamp[0:10]
                    time = timestamp[12:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if errorID == None:
                            errorID = " "

                        outText = "ScreenRequests|" + date + " " + time + "|||" + duration + "|||||||||" + screen + "|||" + applicationName + "|" + applicationKey + "|||||" + source + "||" + endpoint + "|" + executedBy + "|||" + eSpaceName + "|" + eSpaceID + "||" + loginID + "|" + userID + "|||||||" + cycle + "||||||||" + errorID + "||||||" + requestKey + "|||" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif "TimerLog" in filename:
                regex = re.compile(r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w\-]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\d\-\:\. ]+)\|([\d\-\:\. ]+)\|([\w\-]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)\|([\w]+)", re.MULTILINE + re.IGNORECASE)
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
                    requestKey = match.group(10)
                    eSpaceName = match.group(11)
                    applicationName = match.group(12)
                    applicationKey = match.group(13)
                    cyclicJobName = match.group(14)
                        
                    date = timestamp[0:10]
                    time = timestamp[12:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if errorID == None:
                            errorID = " "

                        outText = "Timer|" + date + " " + time + "|||" + duration + "||||||||||||" + applicationName + "|" + applicationKey + "||||||||" + executedBy + "|||" + eSpaceName + "|" + eSpaceID + "|||||||||||" + cyclicJobName + "|" + cyclicJobKey + "|" + shouldHaveRunAt + "|" + nextRun + "||||" + errorID + "||||||" + requestKey + "|||" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif "EmailLog" in filename:
                regex = re.compile(r"^([\d]+)\|([\w]+)\|([\w\-\:\. ]+)\|([\w\-\:\. ]+)?\|([\d]+)\|([\w\@\.]+)\|([\w\@\.]+)\|([\w\@\.]+)?\|([\w\@\.]+)?\|([\w ]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)\|([\w]+)\|([\w]+)\|([\d]+)\|([\w\@\.]+)", re.MULTILINE + re.IGNORECASE)
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
                    subject = match.group(10)
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

                        outText = "Email|" + date + " " + time + "|" + sent + "|" + lastError + "|||||" + from_ + "|" + to + "|" + subject + "|" + cc + "|" + bcc + "||||||" + name + "|||||||||||||||||||||||||||" + size + "|||" + messageID + "|" + activity + "|" + emailDefinition + "|" + storeContent + "|" + isTestEmail + "|||" + iD + "|" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif "ExtensionLog" in filename:
                regex = re.compile(r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\/\=\+]+)\|([\d]+)\|([\d]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\w\-]+)\|([\w]+)\|([\w]+)\|([\w]+)\|([\w\-]+)\|([\w\@\.]+)", re.MULTILINE + re.IGNORECASE)
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
                    username = match.group(16)
                        
                    date = timestamp[0:10]
                    time = timestamp[12:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if errorID == None:
                            errorID = " "

                        outText = "Extension|" + date + " " + time + "|||" + duration + "||||||||||||" + applicationName + "|" + applicationKey + "||" + actionName + "||||||" + executedBy + "|||" + eSpaceName + "|" + eSpaceID + "|" + username + "||" + userID + "|" + sessionID + "|" + extensionID + "|" + extensionName + "|||||||||" + errorID + "||||||" + requestKey + "|||" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)
                                
            elif "ServiceActionLog" in filename:
                regex = re.compile(r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)?\|([\w\=\/\+]+)\|([\d]+)\|([\w\-]+)?\|([\w]+)\|([\w\-]+)\|([\w\-]+)?\|([\w]+)\|([\d]+)\|([\d\.]+)\|([\w\\]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)\|([\w]+)?\|([\w\-]+)", re.MULTILINE + re.IGNORECASE)
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
                    time = timestamp[12:19]
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

                        outText = "ServiceAction|" + date + " " + time + "|||" + duration + "||||||||||||" + applicationName + "|" + applicationKey + "||" + actionName + "|||" + source + "|" + entrypointName + "|" + endpoint + "|" + executedBy + "|||" + eSpaceName + "|" + eSpaceID + "|" + username + "|" + loginID + "|" + userID + "|" + sessionID + "|||||||||||" + errorID + "||||||" + requestKey + "|" + originalRequestKey + "||" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif "ScreenLog" in filename:
                regex = re.compile(r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\=\+\/]+)\|([\d]+)\|([\d]+)\|([\w\-\:\. ]+)?\|([\w]+)\|([\w\-]+)\|([\d]+)\|([\d]+)\|([\d]+)\|([\w]+)\|([\w\-]+)\|([\w\(\)\.]+)\|([\w\-\:\. ]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)", re.MULTILINE + re.IGNORECASE)
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
                    actionName = match.group(16)
                    clientIP = match.group(17)
                    eSpaceName = match.group(18)
                    applicationName = match.group(19)
                    applicationKey = match.group(20)
                        
                    date = timestamp[0:10]
                    time = timestamp[12:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if msisdn == None:
                            msisdn = " "

                        outText = "TraditionalWebRequests|" + date + " " + time + "|||" + duration + "|||||||||" + screen + "|" + screenType + "||" + applicationName + "|" + applicationKey + "||" + actionName + "||" + accessMode + "||||" + executedBy + "||" + clientIP + "|" + eSpaceName + "|" + eSpaceID + "|||" + userID + "|" + sessionID + "|" + sessionRequests + "|" + sessionBytes + "|" + viewstateBytes + "||||||||||" + msisdn + "||||||" + requestKey + "|||" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

        #delete the temporary file that was created
        os.remove(absolutePath)
        
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

        with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
            linesFromDateRange.seek(0)
            if len(linesFromDateRange.read(100)) > 0:
                linesFromDateRange.writelines("\n")
            linesFromDateRange.writelines(myLinesFromDateRange)
    else:
        print("No data was found within the specified date range.")

    print("Closing: " + filename + ".txt")
    del myLinesFromDateRange[:]

def combineXLSXFile(directoryPath):
    print("Combining content from the .XLSX file(s)")

    outText = ""
    
    for root, subFolders, files in os.walk(directoryPath):
        for f in files:
            filename, ext = os.path.splitext(f)
            #confirm if the filename matches the XLSX filenames
            if filename in myXLSXfiles:
                fullpath = os.path.join(root, f)
                #create a temporary file to store the filtered data from the XLSX files
                with codecs.open(os.getcwd() + "\\master_2.txt", "a", "utf-8", "ignore") as linesToText:
                    with codecs.open(fullpath, "r", "utf-8", "ignore") as linesFromText:
                        contents = linesFromText.readlines()
                        linesToText.writelines(contents)
            else:
                if not filename == "iis_logs" and not filename == "iOS_build_logs" and not filename == "service_studio_report":
                    if not filename == "windows_security_event_viewer" and not filename == "windows_app_sys_event_viewer":
                        print("\"" + filename + "\" was not found in the XLSX files list")

    if os.path.exists(os.getcwd() + "\\master_2.txt"):
        #sort the data by the timestamp
        with codecs.open(os.getcwd() + "\\master_2.txt", "r", "utf-8", "ignore") as linesFromText:
            searchLines = linesFromText.read()
            regex = re.compile("^([\w]+)\|([\d\-\: ]+)\|(.+)", re.MULTILINE + re.IGNORECASE)
            for match in regex.finditer(searchLines):
                source = match.group(1)
                dateTime = match.group(2)
                tail = match.group(3)

                outText = dateTime + "|" + source + "|" + tail + "\n"
                if not outText in myXLSXlines:
                    myXLSXlines.append(outText)

        myXLSXlines.sort()

        #create another temporary file to store the sorted data
        with codecs.open(os.getcwd() + "\\master.txt", "w", "utf-8", "ignore") as linesToText2:
            linesToText2.writelines(myXLSXlines)

        del myXLSXlines[:]
        outText = ""

        #rearrange the fields from the sorted data
        with codecs.open(os.getcwd() + "\\master.txt", "r", "utf-8", "ignore") as linesFromText2:
            searchLines2 = linesFromText2.read()
            regex = re.compile("^([\d\-\: ]+)\|([\w]+)\|(.+)", re.MULTILINE + re.IGNORECASE)
            for match in regex.finditer(searchLines2):
                date_time = match.group(1)
                _source = match.group(2)
                _tail = match.group(3)

                outText = _source + "|" + date_time + "|" + _tail + "\n"
                if not outText in myXLSXlines:
                    myXLSXlines.append(outText)

        createFolder("\\master_files\\")
        with codecs.open(os.getcwd() + "\\master_files\\masterXLSXfile.txt", "w", "utf-8", "ignore") as linesToText3:
            linesToText3.writelines(myXLSXlines)

        print("Master file was created from the .XLSX file(s)")
        del myXLSXlines[:]
        #delete the temporary files
        os.remove(os.getcwd() + "\\master_2.txt")
        os.remove(os.getcwd() + "\\master.txt")

def txtFile(absolutePath, filename, filenameWithExt, ext, fromDate, toDate):
    print("Reading: " + filenameWithExt)
    
    del myLinesFromDateRange[:]
    outText = ""

    with codecs.open(absolutePath, "r", "utf-8", "ignore") as linesFromText:
        searchLines = linesFromText.read()

        #split the fields and rearrange them to combine them all later
        if "iosbuildlog" in filename.lower():
            regex = re.compile("^\[([\d\-]+)T([\d\:\.]+)Z\][ ]\[(INFO|VERBOSE)\][ ]\[([\w\[\] ]+)?\](?:[ \t]+)([\w\:\-\/\*\@\.\#\ \,\"\(\)\'\[\]\?\~\`\>\$\=\\\{\}\^\;]+)?", re.MULTILINE + re.IGNORECASE)
            for match in regex.finditer(searchLines):
                date = match.group(1)
                time = match.group(2)
                messageType = match.group(3)
                method = match.group(4)#null
                message = match.group(5)#null

                _time = time[:8]

                _fromDate = datetime.strptime(fromDate, "%Y-%m-%d").date()
                _toDate = datetime.strptime(toDate, "%Y-%m-%d").date()
                _date = datetime.strptime(date, "%Y-%m-%d").date()
                    
                if _fromDate <= _date <= _toDate:

                    if not method == None:
                        method = method.replace("[", "")
                        method = method.replace("]", "")
                    else:
                        method = " "

                    if message == None:
                        message = " "
                    
                    outText = "iOSBuild|" + date + " " + _time + "|" + messageType + "|" + method + "|" + message + "\n"
                    if not outText in myLinesFromDateRange:
                                myLinesFromDateRange.append(outText)

        elif "androidbuildlog" in filename.lower():
            regex = re.compile("^\[([\d\-]+)T([\d\:\.]+)Z\][ ]\[(INFO|VERBOSE)\][ ]\[([\w\[\] ]+)?\](?:[ \t]+)([\w\:\-\/\*\@\.\#\ \,\"\(\)\'\[\]\?\~\`\>\$\=\\\{\}\^\;]+)?", re.MULTILINE + re.IGNORECASE)
            for match in regex.finditer(searchLines):
                date = match.group(1)
                time = match.group(2)
                messageType = match.group(3)
                method = match.group(4)#null
                message = match.group(5)#null

                _time = time[:8]

                _fromDate = datetime.strptime(fromDate, "%Y-%m-%d").date()
                _toDate = datetime.strptime(toDate, "%Y-%m-%d").date()
                _date = datetime.strptime(date, "%Y-%m-%d").date()
                    
                if _fromDate <= _date <= _toDate:

                    if not method == None:
                        method = method.replace("[", "")
                        method = method.replace("]", "")
                    else:
                        method = " "

                    if message == None:
                        message = " "
                    
                    outText = "AndroidBuild|" + date + " " + _time + "|" + messageType + "|" + method + "|" + message + "\n"
                    if not outText in myLinesFromDateRange:
                                myLinesFromDateRange.append(outText)

        elif "studio" in filename.lower() or "report" in filename.lower():
            #service studio report
            regex = re.compile("^(?:[ \t]+)\[([\d\/]+)[ ]([\d\:A-Z ]+)\][ ]\[([\d\:\?]+)\][ ]([\w\-\(\) ]{10,}?)(\s+(?:(?:[\w\[\]\(\)\:\;\=\-\.\+\>\’\'\`\,\<\&\#\t\r\n ]+){1,}?))?(?:\r\n|\n)", re.MULTILINE + re.IGNORECASE)
            for match in regex.finditer(searchLines):
                date = match.group(1)
                time = match.group(2)
                numberOfOccurrences = match.group(3)
                actionName = match.group(4)
                message = match.group(5)#null

                #reformat the dates
                year = date.split("/")[2]
                month = date.split("/")[0]
                day = date.split("/")[1]

                if len(month) != 2:
                    month = "0" + month

                _date = year + "-" + month + "-" + day

                #reformat the times
                if time[-2:] == "AM" and time[:2] == "12":
                    _time = "00" + time[2:-2]
                elif time[-2:] == "AM":
                    _time = time[:-2]
                elif time[-2:] == "PM" and time[:2] == "12":
                    _time = time[:-2]
                elif time[-2:] == "PM":
                    hours = time.split(":")[0]
                    minutes = time.split(":")[1]
                    seconds = time.split(":")[2]
                    _time = str(int(hours) + 12) + ":" + minutes + ":" + seconds[:-2]

                _fromDate = datetime.strptime(fromDate, "%Y-%m-%d").date()
                _toDate = datetime.strptime(toDate, "%Y-%m-%d").date()
                _date_ = datetime.strptime(_date, "%Y-%m-%d").date()
                            
                if _fromDate <= _date_ <= _toDate:

                    if message == None:
                        message = " "
                        outText = "ServiceStudio|" + _date + " " + _time.strip() + "|" + numberOfOccurrences + "|" + actionName.strip() + "|" + message + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)
                    else:
                        message = message.replace("\t", " ")
                        message = message.replace("\r\n", " ")
                        message = message.replace("\n", " ")
                        outText = "ServiceStudio|" + _date + " " + _time.strip() + "|" + numberOfOccurrences + "|" + actionName.strip() + "|" + message.strip() + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

        elif "security" in filename.lower():
            #windows security event viewer
            regex2 = re.compile("^([\w]+)(?:[\t]+)([\d\/]+)[ ]([\d\:A-Z ]+)(?:[\t]+)([\w\-\. ]+)(?:[\t]+)([\d]+)(?:[\t]+)([\w\(\) ]+)(?:[\t]+)([\w\.\-\:\;\,\"\(\) ]+)(?:(?:\r\n|\n){2})((?:.*(?:\r\n|\n)){1,}?)Token", re.MULTILINE + re.IGNORECASE)
            for match2 in regex2.finditer(searchLines):
                level = match2.group(1)
                date = match2.group(2)
                time = match2.group(3)
                source = match2.group(4)
                eventID = match2.group(5)
                task = match2.group(6)
                category = match2.group(7)
                message = match2.group(8)

                #reformat the dates
                year = date.split("/")[2]
                month = date.split("/")[0]
                day = date.split("/")[1]

                if len(month) != 2:
                    month = "0" + month

                _date = year + "-" + month + "-" + day

                #reformat the times
                if time[-2:] == "AM" and time[:2] == "12":
                    _time = "00" + time[2:-2]
                elif time[-2:] == "AM":
                    _time = time[:-2]
                elif time[-2:] == "PM" and time[:2] == "12":
                    _time = time[:-2]
                elif time[-2:] == "PM":
                    hours = time.split(":")[0]
                    minutes = time.split(":")[1]
                    seconds = time.split(":")[2]
                    _time = str(int(hours) + 12) + ":" + minutes + ":" + seconds[:-2]

                message = message.replace("\t", " ")
                message = message.replace("\r\n", " ")
                message = message.replace("\n", " ")

                _fromDate = datetime.strptime(fromDate, "%Y-%m-%d").date()
                _toDate = datetime.strptime(toDate, "%Y-%m-%d").date()
                _date_ = datetime.strptime(_date, "%Y-%m-%d").date()
                        
                if _fromDate <= _date_ <= _toDate:
                             
                    outText = "WindowsSecurityEventViewer|" + _date + " " + _time.strip() + "|" + level + "|" + source + "|" + eventID + "|" + task + "|" + category + "|" + message.strip() + "\n"
                    if not outText in myLinesFromDateRange:
                        myLinesFromDateRange.append(outText)

        elif "application" in filename.lower() or "system" in filename.lower():
            #windows application/system event viewer
            regex3 = re.compile("^([\w]+)(?:[\t]+)([\d\/]+)[ ]([\d\:A-Z ]+)(?:[\t]+)([\w\-\. ]+)(?:[\t]+)([\d]+)(?:[\t]+)([\w\(\) ]+)(?:[\t]+)([\w\.\-\:\;\,\"\(\) ]+)(?:(?:(?:\r\n|\n){2}).*?originated.*?saved.*?(?:(?:\r\n|\n){2}).*?information.*?event\:\s*?(?:\r\n|\n)(?:\r\n|\n)((?:.*(?:\r\n|\n)){1,}?)\")?", re.MULTILINE + re.IGNORECASE)
            for match3 in regex3.finditer(searchLines):
                level = match3.group(1)
                date = match3.group(2)
                time = match3.group(3)
                source = match3.group(4)
                eventID = match3.group(5)
                task = match3.group(6)
                category = match3.group(7)
                message = match3.group(8)#null

                #reformat the dates
                year = date.split("/")[2]
                month = date.split("/")[0]
                day = date.split("/")[1]

                if len(month) != 2:
                    month = "0" + month

                _date = year + "-" + month + "-" + day

                #reformat the times
                if time[-2:] == "AM" and time[:2] == "12":
                    _time = "00" + time[2:-2]
                elif time[-2:] == "AM":
                    _time = time[:-2]
                elif time[-2:] == "PM" and time[:2] == "12":
                    _time = time[:-2]
                elif time[-2:] == "PM":
                    hours = time.split(":")[0]
                    minutes = time.split(":")[1]
                    seconds = time.split(":")[2]
                    _time = str(int(hours) + 12) + ":" + minutes + ":" + seconds[:-2]

                _fromDate = datetime.strptime(fromDate, "%Y-%m-%d").date()
                _toDate = datetime.strptime(toDate, "%Y-%m-%d").date()
                _date_ = datetime.strptime(_date, "%Y-%m-%d").date()
                            
                if _fromDate <= _date_ <= _toDate:

                    if message == None:
                        message = " "
                        outText = "WindowsAppSysEventViewer|" + _date + " " + _time.strip() + "|" + level + "|" + source + "|" + eventID + "|" + task + "|" + category + "|" + message + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)
                    else:
                        message = message.replace("\t", " ")
                        message = message.replace("\r\n", " ")
                        message = message.replace("\n", " ")
                        outText = "WindowsAppSysEventViewer|" + _date + " " + _time.strip() + "|" + level + "|" + source + "|" + eventID + "|" + task + "|" + category + "|The following information was included with the event: " + message.strip() + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

    if len(myLinesFromDateRange) > 0:
        createFolder("\\filtered_data_files\\")
        if "iosbuildlog" in filename.lower():
            outFilename = "iOS_build_logs" + ext

            with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
                linesFromDateRange.seek(0)
                if len(linesFromDateRange.read(100)) > 0:
                    linesFromDateRange.writelines("\n")
                linesFromDateRange.writelines(myLinesFromDateRange)

        elif "androidbuildlog" in filename.lower():
            outFilename = "android_build_logs" + ext

            with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
                linesFromDateRange.seek(0)
                if len(linesFromDateRange.read(100)) > 0:
                    linesFromDateRange.writelines("\n")
                linesFromDateRange.writelines(myLinesFromDateRange)

        elif "studio" in filename.lower() or "report" in filename.lower():
            outFilename = "service_studio_report" + ext

            with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
                linesFromDateRange.seek(0)
                if len(linesFromDateRange.read(100)) > 0:
                    linesFromDateRange.writelines("\n")
                linesFromDateRange.writelines(myLinesFromDateRange)

            #remove the last line from the filtered file
            with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "r", "utf-8", "ignore") as linesFromText2:
                searchLines2 = linesFromText2.readlines()
                searchLines2 = searchLines2[:-1]
                with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "w", "utf-8", "ignore") as linesFromDateRange2:
                    linesFromDateRange2.writelines(searchLines2)

        elif "security" in filename.lower():
            outFilename = "windows_security_event_viewer" + ext

            with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
                linesFromDateRange.seek(0)
                if len(linesFromDateRange.read(100)) > 0:
                    linesFromDateRange.writelines("\n")
                linesFromDateRange.writelines(myLinesFromDateRange)

        elif "application" in filename.lower() or "system" in filename.lower():
            outFilename = "windows_app_sys_event_viewer" + ext

            with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
                linesFromDateRange.seek(0)
                if len(linesFromDateRange.read(100)) > 0:
                    linesFromDateRange.writelines("\n")
                linesFromDateRange.writelines(myLinesFromDateRange)
                
    else:
        print("No data was found within the specified date range.")

    print("Closing: " + filenameWithExt)
    del myLinesFromDateRange[:]

def combineTXTFile(directoryPath):
    print("Combining content from the .TXT file(s)")

    outText = ""

    for root, subFolders, files in os.walk(directoryPath):
        for f in files:
            filename, ext = os.path.splitext(f)
            #confirm if the filename matches the TXT filenames
            if not filename in myXLSXfiles:
                if not filename == "iis_logs" and not filename == "windows_security_event_viewer" and not filename == "windows_app_sys_event_viewer":
                    if filename == "iOS_build_logs" or filename == "android_build_logs" or filename == "service_studio_report":
                        #create temporary files to store the filtered data from the TXT files
                        fullpath = os.path.join(root, f)
                        with codecs.open(os.getcwd() + "\\master_2.txt", "a", "utf-8", "ignore") as linesToText:
                            with codecs.open(fullpath, "r", "utf-8", "ignore") as linesFromText:
                                contents = linesFromText.readlines()
                                linesToText.writelines(contents)

    if os.path.exists(os.getcwd() + "\\master_2.txt"):
        #sort the data by the timestamp
        with codecs.open(os.getcwd() + "\\master_2.txt", "r", "utf-8", "ignore") as linesFromText:
            searchLines = linesFromText.read()
            regex = re.compile("^([\w]+)\|([\d\-\: ]+)\|(.+)", re.MULTILINE + re.IGNORECASE)
            for match in regex.finditer(searchLines):
                source = match.group(1)
                dateTime = match.group(2)
                tail = match.group(3)

                outText = dateTime + "|" + source + "|" + tail + "\n"
                if not outText in myTXTlines:
                    myTXTlines.append(outText)

        myTXTlines.sort()

        #create another temporary file to store the sorted data
        with codecs.open(os.getcwd() + "\\master.txt", "w", "utf-8", "ignore") as linesToText3:
            linesToText3.writelines(myTXTlines)

        del myTXTlines[:]
        outText = ""

        #rearrange the fields from the sorted data
        with codecs.open(os.getcwd() + "\\master.txt", "r", "utf-8", "ignore") as linesFromText2:
            searchLines2 = linesFromText2.read()
            regex = re.compile("^([\d\-\: ]+)\|([\w]+)\|(.+)", re.MULTILINE + re.IGNORECASE)
            for match in regex.finditer(searchLines2):
                date_time = match.group(1)
                _source = match.group(2)
                _tail = match.group(3)

                outText = _source + "|" + date_time + "|" + _tail + "\n"
                if not outText in myTXTlines:
                    myTXTlines.append(outText)

        createFolder("\\master_files\\")
        with codecs.open(os.getcwd() + "\\master_files\\masterTXTfile.txt", "w", "utf-8", "ignore") as linesToText4:
            linesToText4.writelines(myTXTlines)

        print("Master file was created from the .TXT file(s)")
        del myTXTlines[:]
        #delete the temporary files
        os.remove(os.getcwd() + "\\master_2.txt")
        os.remove(os.getcwd() + "\\master.txt")

def combineEVTXFile(directoryPath):
    outText = ""

    for root, subFolders, files in os.walk(directoryPath):
        for f in files:
            filename, ext = os.path.splitext(f)
            #confirm if the filename matches the TXT filenames
            if not filename in myXLSXfiles:
                if not filename == "iis_logs" and not filename == "iOS_build_logs" and not filename == "android_build_logs" and not filename == "service_studio_report":
                    #create temporary files to store the filtered data from the TXT files
                    fullpath = os.path.join(root, f)
                    with codecs.open(os.getcwd() + "\\master_2.txt", "a", "utf-8", "ignore") as linesToText:
                        with codecs.open(fullpath, "r", "utf-8", "ignore") as linesFromText:
                            contents = linesFromText.readlines()
                            linesToText.writelines(contents)

    if os.path.exists(os.getcwd() + "\\master_2.txt"):
        print("Combining content from the .EVTX file(s)")
        #sort the data by the timestamp
        with codecs.open(os.getcwd() + "\\master_2.txt", "r", "utf-8", "ignore") as linesFromText:
            searchLines = linesFromText.read()
            regex = re.compile("^([\w]+)\|([\d\-\: ]+)\|(.+)", re.MULTILINE + re.IGNORECASE)
            for match in regex.finditer(searchLines):
                source = match.group(1)
                dateTime = match.group(2)
                tail = match.group(3)

                outText = dateTime + "|" + source + "|" + tail + "\n"
                if not outText in myTXTlines:
                    myTXTlines.append(outText)

        myTXTlines.sort()

        #create another temporary file to store the sorted data
        with codecs.open(os.getcwd() + "\\master.txt", "w", "utf-8", "ignore") as linesToText3:
            linesToText3.writelines(myTXTlines)

        del myTXTlines[:]
        outText = ""

        #rearrange the fields from the sorted data
        with codecs.open(os.getcwd() + "\\master.txt", "r", "utf-8", "ignore") as linesFromText2:
            searchLines2 = linesFromText2.read()
            regex = re.compile("^([\d\-\: ]+)\|([\w]+)\|(.+)", re.MULTILINE + re.IGNORECASE)
            for match in regex.finditer(searchLines2):
                date_time = match.group(1)
                _source = match.group(2)
                _tail = match.group(3)

                outText = _source + "|" + date_time + "|" + _tail + "\n"
                if not outText in myTXTlines:
                    myTXTlines.append(outText)

        createFolder("\\master_files\\")
        with codecs.open(os.getcwd() + "\\master_files\\masterEVTXfile.txt", "w", "utf-8", "ignore") as linesToText4:
            linesToText4.writelines(myTXTlines)

        print("Master file was created from the .EVTX file(s)")
        del myTXTlines[:]
        #delete the temporary files
        os.remove(os.getcwd() + "\\master_2.txt")
        os.remove(os.getcwd() + "\\master.txt")

def logFile(absolutePath, filenameWithExt, ext, fromDate, toDate):
    print("Reading: " + filenameWithExt)
    
    del myLinesFromDateRange[:]
    outText = ""

    #split the fields and rearrange them to combine them all later
    with codecs.open(absolutePath, "r", "utf-8", "ignore") as linesFromLog:
        searchLines = linesFromLog.read()
        regex = re.compile("^([\d\-]+)[ ]([\d\:]+)[ ]([\d\.]+)[ ](POST|PUT|PROPFIND|GET)[ ]([\w\/\-\.\(\)\+]+)[ ]([\w\-\=\+\&]+)[ ]([\d]+)[ ]([\w\/\-\.\(\)\+]+)[ ]([\d\.]+)[ ]([\w\/\-\(\)\.\+\;\:\,]+)[ ]([\w\/\-\:\.\?\=\&]+)[ ]([\d]+)[ ]([\d]+)[ ]([\d]+)[ ]([\d]+)", re.MULTILINE + re.IGNORECASE)
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

            _fromDate = datetime.strptime(fromDate, "%Y-%m-%d").date()
            _toDate = datetime.strptime(toDate, "%Y-%m-%d").date()
            _date = datetime.strptime(date, "%Y-%m-%d").date()
                
            if _fromDate <= _date <= _toDate:
                outText = date + " " + time + "|" + timeTaken + "|" + httpCode + "|" + httpSubCode + "|" + windowsCode + "|" + clientIP + "|" + serverIP + "|" + serverPort + "|" + method + "|" + uriStem + "|" + uriQuery + "|" + username + "|" + browser + "|" + referrer + "\n"
                if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

    if len(myLinesFromDateRange) > 0:
        createFolder("\\filtered_data_files\\")
        outFilename = "iis_logs" + ext

        with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
            linesFromDateRange.seek(0)
            if len(linesFromDateRange.read(100)) > 0:
                linesFromDateRange.writelines("\n")
            linesFromDateRange.writelines(myLinesFromDateRange)
    else:
        print("No data was found within the specified date range.")

    print("Closing: " + filenameWithExt)
    del myLinesFromDateRange[:]

def combineLOGFile(directoryPath):
    print("Combining content from the .LOG file(s)")

    outText = ""
    outText2 = ""

    for root, subFolders, files in os.walk(directoryPath):
        for f in files:
            filename, ext = os.path.splitext(f)
            if filename == "iis_logs":
                #create a temporary file to store the filtered data from the LOG files
                with codecs.open(os.getcwd() + "\\master.txt", "a", "utf-8", "ignore") as linesToText:
                    with codecs.open(directoryPath + "\\iis_logs.txt", "r", "utf-8", "ignore") as linesFromText:
                        contents = linesFromText.readlines()
                        linesToText.writelines(contents)

                #sort the data by the timestamp and by the time taken to perform a task
                with codecs.open(os.getcwd() + "\\master.txt", "r", "utf-8", "ignore") as linesFromText2:
                    searchLines = linesFromText2.read()
                    regex = re.compile("^([\d\-\: ]+)\|([\d]+)\|(.+)", re.MULTILINE + re.IGNORECASE)
                    for match in regex.finditer(searchLines):
                        dateTime = match.group(1)
                        timeTaken = match.group(2)
                        tail = match.group(3)

                        outText = dateTime + "|" + timeTaken + "|" + tail + "\n"
                        outText2 = timeTaken + "|" + dateTime + "|" + tail + "\n"
                        _timeTaken = int(timeTaken)

                        if not outText in myLOGlines:
                            myLOGlines.append(outText)

                        if not outText2 in myLOGlines2:
                            myLOGlines2.append(outText2)

                        if not _timeTaken in myTimeTaken:
                            myTimeTaken.append(_timeTaken)

                myLOGlines.sort()
                myTimeTaken.sort()

                minimum = min(myTimeTaken)
                maximum = max(myTimeTaken)

                createFolder("\\master_files\\")
                with codecs.open(os.getcwd() + "\\master_files\\masterLOGfile_datetime.txt", "w", "utf-8", "ignore") as linesToText2:
                    linesToText2.writelines(myLOGlines)

                with codecs.open(os.getcwd() + "\\master_files\\masterLOGfile_timetaken.txt", "w", "utf-8", "ignore") as linesToText3:
                    while maximum >= minimum:
                        if maximum in myTimeTaken:
                            for t, tt in enumerate(myLOGlines2):
                                time = [int(w) for w in myLOGlines2[t].split("|") if w.isdigit()]
                                if maximum == time[0]:
                                    linesToText3.writelines(myLOGlines2[t])
                                t+=1
                        maximum-=1

                print("Master files were created from the .LOG file(s)")

                #create a line graph with the compiled data that was sorted by the timestamp
                #retrieve the timestamp and the time taken to perform a task
                with codecs.open(os.getcwd() + "\\master_files\\masterLOGfile_datetime.txt", "r", "utf-8", "ignore") as linesFromText3:
                    searchLines2 = linesFromText3.read()
                    regex = re.compile("^([\d\-\: ]+)\|([\d]+)\|.+", re.MULTILINE + re.IGNORECASE)
                    for match in regex.finditer(searchLines2):
                        date_time = match.group(1)
                        time_taken = match.group(2)

                        _date_time = datetime.strptime(date_time, "%Y-%m-%d %H:%M:%S")

                        myDateTimes.append(_date_time)
                        myTimesTaken.append(int(time_taken))

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
                plt.savefig(os.getcwd() + "\\master_files\\line_graph.png")
                print("Saved the line graph from the .LOG file(s)")
                plt.show()

                del myDateTimes[:]
                del myTimesTaken[:]

                with codecs.open(os.getcwd() + "\\master_files\\masterLOGfile_timetaken.txt", "r", "utf-8", "ignore") as linesFromText4:
                    searchLines3 = linesFromText4.read()
                    regex2 = re.compile("^([\d]+)\|([\d\-\: ]+)\|.+", re.MULTILINE + re.IGNORECASE)
                    for match2 in regex2.finditer(searchLines3):
                        time_taken_ = match2.group(1)
                        date_time_ = match2.group(2)

                        myDateTimes.append(date_time_)
                        myTimesTaken.append(time_taken_)

                with codecs.open(os.getcwd() + "\\master_files\\line_graph.html", "w", "utf-8", "ignore") as linesToText4:
                    linesToText4.writelines("<!doctype html>\n<body>\n<div id=\"container\" style=\"width: 500px; height: 400px;\"></div>\n<script src=\"https://cdn.anychart.com/releases/v8/js/anychart-base.min.js\" type=\"text/javascript\"></script>\n<script>\n\tanychart.onDocumentReady(function() {\n\t\t//create CSV string\n\t\tvar csvString = 'Timestamp;Time*' +\n")
                    for x, elm in enumerate(myDateTimes):
                        if x == len(myDateTimes) - 1:
                            linesToText4.writelines("\t\t\t'" + myDateTimes[x] + "|" + myTimesTaken[x] + "';\n\n")
                        else:
                            linesToText4.writelines("\t\t\t'" + myDateTimes[x] + "|" + myTimesTaken[x] + "*' +\n")
                        x+=1
                    linesToText4.writelines("\t\t//create an area chart\n\t\tvar chart = anychart.line();\n\n\t\t//create the area series based on CSV data\n\t\tchart.line(csvString, {ignoreFirstRow: true, columnsSeparator: \"|\", rowsSeparator: \"*\"});\n\n\t\t//display a chart\n\t\tchart.container('container').draw();\n\n\t\tchart.xAxis().title(\"Timestamp\");\n\t\tchart.yAxis().title(\"Time Taken\");\n\n\t\t//set ticks interval\n\t\tchart.yScale().ticks().interval(1000);\n\n\t\t//set minor ticks interval\n\t\tchart.yScale().minorTicks().interval(500);\n\n\t\t//settings\n\t\tchart.tooltip().fontColor(\"red\");\n\n\t\t//tooltip padding for all series on a chart\n\t\tchart.tooltip().padding().left(20);\n\n\t\t//background color\n\t\tchart.tooltip().background().fill(\"black\");\n});\n</script>\n</body>\n</html>")
                    
                del myLOGlines[:]
                del myLOGlines2[:]
                del myTimeTaken[:]
                del myDateTimes[:]
                del myTimesTaken[:]
                outText = ""
                outText2 = ""
                #delete the temporary file
                os.remove(os.getcwd() + "\\master.txt")

#the following line displays the absolute path of the folder where
#Python was installed on your PC. Uncomment only when necessary.
#print(os.path.dirname(sys.executable))

num_args = len(sys.argv)

if num_args != 4:
    print("Error:\nTotal arguments passed: " + str(num_args) +
          "\n4 arguments needed: log_parser.py directoryPath fromDate(YYYY-MM-DD) toDate(YYYY-MM-DD)" +
          "\nPlease try again.")
else:
    directoryPath = sys.argv[1]
    fromDate = sys.argv[2]
    toDate = sys.argv[3]

    searchDirectory(directoryPath)
    searchDirectory2(directoryPath, myFileExt)

end = datetime.now()
print("\nElapsed time: {0}".format(end-start))
