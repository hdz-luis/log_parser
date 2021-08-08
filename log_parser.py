import os
import re
import sys
import csv
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
  
IIS logs:
DATE_TIME TIME_TAKEN HTTP_CODE HTTP_SUBCODE WINDOWS_ERROR_CODE CLIENT_IP SERVER_IP SERVER_PORT
METHOD URI_STEM URI_QUERY USERNAME BROWSER REFERRER

TIME_TAKEN DATE_TIME HTTP_CODE HTTP_SUBCODE WINDOWS_ERROR_CODE CLIENT_IP SERVER_IP SERVER_PORT
METHOD URI_STEM URI_QUERY USERNAME BROWSER REFERRER

Android, iOS, and Service Studio Report:
DATE_TIME MESSAGE_TYPE ACTION_NAME MESSAGE

Windows Event Viewer logs:
DATE_TIME LEVEL PROVIDER_NAME QUALIFIERS EVENT_ID EVENT_RECORD_ID TASK KEYWORDS MESSAGE COMPUTER

1) Filter data based on a date range
2) Read the input files and rearrange the columns.
3) Filtered outputs should be already sorted by their timestamp.
4) Filtered outputs should be TXT files using the pipe ("|") as the delimiter and their filenames should be the logs they represent.
5) All the "datetime" fields from the logs should be in the following format: YYYY-MM-DD hh:mm:ss
"""

myLinesFromDateRange = []
myTempXLSXlines = []
myLOGlines = []
myTimeTaken = []
myFileExt = []
myDateTimes = []
myTimesTaken = []
myXMLList = []

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
            elif extension == ".evtx":
                evtxFile(absolutePathOfFile, filenameWithExt, ".txt", fromDate, toDate)

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
                    time = timestamp[11:19]
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

                        outText = date + " " + time + "|" + message + "|" + stack + "|" + moduleName + "|" + applicationName + "|" + applicationKey + "|" + actionName + "|" + entryPointName + "|" + server + "|" + eSpaceName + "|" + eSpaceID + "|" + userID + "|" + sessionID + "|" + environmentInformation + "|" + iD + "|" + tenantID + "\n"
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
                    time = timestamp[11:19]
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

                        outText = date + " " + time + "|" + message + "|" + messageType + "|" + moduleName + "|" + applicationName + "|" + applicationKey + "|" + actionName + "|" + entryPointName + "|" + clientIP + "|" + eSpaceName + "|" + eSpaceID + "|" + userID + "|" + sessionID + "|" + errorID + "|" + requestKey + "|" + tenantID + "\n"
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

                        if applicationKey == None:
                            applicationKey = " "

                        outText = date + " " + time + "|" + duration + "|" + applicationName + "|" + applicationKey + "|" + actionName + "|" + actionType + "|" + source + "|" + endpoint + "|" + executedBy + "|" + eSpaceName + "|" + eSpaceID + "|" + errorID + "|" + requestKey + "|" + tenantID + "\n"
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
                    time = timestamp[11:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if errorID == None:
                            errorID = " "

                        outText = date + " " + time + "|" + duration + "|" + screen + "|" + applicationName + "|" + applicationKey + "|" + source + "|" + endpoint + "|" + executedBy + "|" + eSpaceName + "|" + eSpaceID + "|" + loginID + "|" + userID + "|" + cycle + "|" + errorID + "|" + requestKey + "|" + tenantID + "\n"
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
                    time = timestamp[11:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if errorID == None:
                            errorID = " "

                        outText = date + " " + time + "|" + duration + "|" + applicationName + "|" + applicationKey + "|" + executedBy + "|" + eSpaceName + "|" + eSpaceID + "|" + cyclicJobName + "|" + cyclicJobKey + "|" + shouldHaveRunAt + "|" + nextRun + "|" + errorID + "|" + requestKey + "|" + tenantID + "\n"
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

                        outText = date + " " + time + "|" + sent + "|" + lastError + "|" + from_ + "|" + to + "|" + subject + "|" + cc + "|" + bcc + "|" + name + "|" + size + "|" + messageID + "|" + activity + "|" + emailDefinition + "|" + storeContent + "|" + isTestEmail + "|" + iD + "|" + tenantID + "\n"
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
                    time = timestamp[11:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if msisdn == None:
                            msisdn = " "

                        outText = date + " " + time + "|" + duration + "|" + screen + "|" + screenType + "|" + applicationName + "|" + applicationKey + "|" + actionName + "|" + accessMode + "|" + executedBy + "|" + clientIP + "|" + eSpaceName + "|" + eSpaceID + "|" + userID + "|" + sessionID + "|" + sessionRequests + "|" + sessionBytes + "|" + viewstateBytes + "|" + msisdn + "|" + requestKey + "|" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)
        count+=1

def parseXMLtoString(lineToXML, xmlPath):
    xmlVar = etree.XPath(xmlPath)
    newXMLVar = " ".join(xmlVar(lineToXML))
    return newXMLVar

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
    print("Reading: " + filename + ext)

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
            readSplitFiles(cycleNum, "IntegrationsLog", ext, r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d\.]+)?\|([\w\:\/\.\-]+)?\|([\w\/\.\(\) ]+)\|([\w\(\) ]+)\|([\d]+)\|([\w\-]+)?\|([\w\-]+)\|([\w\-\:]+)\|([\w\.]+)\|([\w\. ]+)\|([\w\-]+)?", _fromDate, _toDate)
        elif "MobileRequestsLog" in filename:
            readSplitFiles(cycleNum, "MobileRequestsLog", ext, r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\.\-\:\; ]+)\|([\w]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\d]+)\|([\w\-]+)\|([\w\/\+\=]+)\|([\d]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)", _fromDate, _toDate)
        elif "TimerLog" in filename:
            readSplitFiles(cycleNum, "TimerLog", ext, r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w\-]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\d\-\:\. ]+)\|([\d\-\:\. ]+)\|([\w\-]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)\|([\w]+)", _fromDate, _toDate)
        elif "EmailLog" in filename:
            readSplitFiles(cycleNum, "EmailLog", ext, r"^([\d]+)\|([\w]+)\|([\w\-\:\. ]+)\|([\w\-\:\. ]+)?\|([\d]+)\|([\w\@\.\-]+)\|([\w\@\.\-]+)\|([\w\@\.]+)?\|([\w\@\.]+)?\|([\w\.\!\?\:\-\(\)\[\]\,\@\#\$\%\&\*\'\"\;\+\= ]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)\|([\w]+)\|([\w]+)\|([\d]+)\|([\w\@\.]+)", _fromDate, _toDate)
        elif "ExtensionLog" in filename:
            readSplitFiles(cycleNum, "ExtensionLog", ext, r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\/\=\+]+)\|([\d]+)\|([\d]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\w\-]+)\|([\w]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)\|([\w\@\.]+)?", _fromDate, _toDate)
        elif "ServiceActionLog" in filename:
            readSplitFiles(cycleNum, "ServiceActionLog", ext, r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)?\|([\w\=\/\+]+)\|([\d]+)\|([\w\-]+)?\|([\w\-]+)\|([\w\-]+)\|([\w\-]+)?\|([\w]+)\|([\d]+)\|([\d\.]+)\|([\w\\]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)\|([\w]+)?\|([\w\-]+)", _fromDate, _toDate)
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
                    time = timestamp[11:19]
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

                        outText = date + " " + time + "|" + message + "|" + stack + "|" + moduleName + "|" + applicationName + "|" + applicationKey + "|" + actionName + "|" + entryPointName + "|" + server + "|" + eSpaceName + "|" + eSpaceID + "|" + userID + "|" + sessionID + "|" + environmentInformation + "|" + iD + "|" + tenantID + "\n"
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
                    time = timestamp[11:19]
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

                        outText = date + " " + time + "|" + message + "|" + messageType + "|" + moduleName + "|" + applicationName + "|" + applicationKey + "|" + actionName + "|" + entryPointName + "|" + clientIP + "|" + eSpaceName + "|" + eSpaceID + "|" + userID + "|" + sessionID + "|" + errorID + "|" + requestKey + "|" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif "IntegrationsLog" in filename:
                regex = re.compile(r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d\.]+)?\|([\w\:\/\.\-]+)?\|([\w\/\.\(\) ]+)\|([\w\(\) ]+)\|([\d]+)\|([\w\-]+)?\|([\w\-]+)\|([\w\-\:]+)\|([\w\.]+)\|([\w\. ]+)\|([\w\-]+)?", re.MULTILINE + re.IGNORECASE)
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

                        if applicationKey == None:
                            applicationKey = " "

                        outText = date + " " + time + "|" + duration + "|" + applicationName + "|" + applicationKey + "|" + actionName + "|" + actionType + "|" + source + "|" + endpoint + "|" + executedBy + "|" + eSpaceName + "|" + eSpaceID + "|" + errorID + "|" + requestKey + "|" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif "MobileRequestsLog" in filename:
                regex = re.compile(r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\.\-\:\; ]+)\|([\w]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\d]+)\|([\w\-]+)\|([\w\/\+\=]+)\|([\d]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)", re.MULTILINE + re.IGNORECASE)
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
                    time = timestamp[11:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if errorID == None:
                            errorID = " "

                        outText = date + " " + time + "|" + duration + "|" + screen + "|" + applicationName + "|" + applicationKey + "|" + source + "|" + endpoint + "|" + executedBy + "|" + eSpaceName + "|" + eSpaceID + "|" + loginID + "|" + userID + "|" + cycle + "|" + errorID + "|" + requestKey + "|" + tenantID + "\n"
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
                    time = timestamp[11:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if errorID == None:
                            errorID = " "

                        outText = date + " " + time + "|" + duration + "|" + applicationName + "|" + applicationKey + "|" + executedBy + "|" + eSpaceName + "|" + eSpaceID + "|" + cyclicJobName + "|" + cyclicJobKey + "|" + shouldHaveRunAt + "|" + nextRun + "|" + errorID + "|" + requestKey + "|" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif "EmailLog" in filename:
                regex = re.compile(r"^([\d]+)\|([\w]+)\|([\w\-\:\. ]+)\|([\w\-\:\. ]+)?\|([\d]+)\|([\w\@\.\-]+)\|([\w\@\.\-]+)\|([\w\@\.]+)?\|([\w\@\.]+)?\|([\w\.\!\?\:\-\(\)\[\]\,\@\#\$\%\&\*\'\"\;\+\= ]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)\|([\w]+)\|([\w]+)\|([\d]+)\|([\w\@\.]+)", re.MULTILINE + re.IGNORECASE)
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

                        outText = date + " " + time + "|" + sent + "|" + lastError + "|" + from_ + "|" + to + "|" + subject + "|" + cc + "|" + bcc + "|" + name + "|" + size + "|" + messageID + "|" + activity + "|" + emailDefinition + "|" + storeContent + "|" + isTestEmail + "|" + iD + "|" + tenantID + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

            elif "ExtensionLog" in filename:
                regex = re.compile(r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\w]+)\|([\w\/\=\+]+)\|([\d]+)\|([\d]+)\|([\d]+)\|([\w\-]+)\|([\w\-]+)?\|([\w\-]+)\|([\w]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)\|([\w\@\.]+)?", re.MULTILINE + re.IGNORECASE)
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
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)
                                
            elif "ServiceActionLog" in filename:
                regex = re.compile(r"^([\d]+)\|([\d\-\:\. ]+)\|([\d]+)\|([\d]+)?\|([\w\=\/\+]+)\|([\d]+)\|([\w\-]+)?\|([\w\-]+)\|([\w\-]+)\|([\w\-]+)?\|([\w]+)\|([\d]+)\|([\d\.]+)\|([\w\\]+)\|([\w]+)\|([\w ]+)\|([\w\-]+)\|([\w]+)?\|([\w\-]+)", re.MULTILINE + re.IGNORECASE)
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
                    time = timestamp[11:19]
                    time = time.replace(".", "")

                    _date = datetime.strptime(date, "%Y-%m-%d").date()

                    if _fromDate <= _date <= _toDate:

                        if msisdn == None:
                            msisdn = " "

                        outText = date + " " + time + "|" + duration + "|" + screen + "|" + screenType + "|" + applicationName + "|" + applicationKey + "|" + actionName + "|" + accessMode + "|" + executedBy + "|" + clientIP + "|" + eSpaceName + "|" + eSpaceID + "|" + userID + "|" + sessionID + "|" + sessionRequests + "|" + sessionBytes + "|" + viewstateBytes + "|" + msisdn + "|" + requestKey + "|" + tenantID + "\n"
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

        myLinesFromDateRange.sort()

        with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
            linesFromDateRange.seek(0)
            if len(linesFromDateRange.read(100)) > 0:
                linesFromDateRange.writelines("\n")
            linesFromDateRange.writelines(myLinesFromDateRange)
    else:
        print("No data was found within the specified date range.")

    print("Closing: " + filename + ".txt")
    del myLinesFromDateRange[:]

def txtFile(absolutePath, filename, filenameWithExt, ext, fromDate, toDate):
    print("Reading: " + filenameWithExt)
    
    del myLinesFromDateRange[:]
    outText = ""

    with codecs.open(absolutePath, "r", "utf-8", "ignore") as linesFromText:
        searchLines = linesFromText.read()

        #split the fields and rearrange them to combine them all later
        if "iosbuildlog" in filename.lower() or "androidbuildlog" in filename.lower():
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

                    if "iosbuildlog" in filename.lower():
                        outText = date + " " + _time + "|" + messageType + "|" + method + "|" + message + "\n"
                    elif "androidbuildlog" in filename.lower():
                        outText = date + " " + _time + "|" + messageType + "|" + method + "|" + message + "\n"

                    if not outText in myLinesFromDateRange:
                                myLinesFromDateRange.append(outText)

        elif "studio" in filename.lower() and "report" in filename.lower():
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
                        outText = _date + " " + _time.strip() + "|" + numberOfOccurrences + "|" + actionName.strip() + "|" + message + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)
                    else:
                        message = message.replace("\t", " ")
                        message = message.replace("\r\n", " ")
                        message = message.replace("\n", " ")
                        outText = _date + " " + _time.strip() + "|" + numberOfOccurrences + "|" + actionName.strip() + "|" + message.strip() + "\n"
                        if not outText in myLinesFromDateRange:
                            myLinesFromDateRange.append(outText)

    if len(myLinesFromDateRange) > 0:
        createFolder("\\filtered_data_files\\")
        if "iosbuildlog" in filename.lower():
            outFilename = "iOS_build_logs" + ext
        elif "androidbuildlog" in filename.lower():
            outFilename = "android_build_logs" + ext
        elif "studio" in filename.lower() and "report" in filename.lower():
            outFilename = "service_studio_report" + ext

        myLinesFromDateRange.sort()

        with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
            linesFromDateRange.seek(0)
            if len(linesFromDateRange.read(100)) > 0:
                linesFromDateRange.writelines("\n")
            linesFromDateRange.writelines(myLinesFromDateRange)

        if "studio" in filename.lower() and "report" in filename.lower():
            #remove the last line from the filtered file
            with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "r", "utf-8", "ignore") as linesFromText2:
                searchLines2 = linesFromText2.readlines()
                searchLines2 = searchLines2[:-1]
                with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "w", "utf-8", "ignore") as linesFromDateRange2:
                    linesFromDateRange2.writelines(searchLines2)
                
    else:
        print("No data was found within the specified date range.")

    print("Closing: " + filenameWithExt)
    del myLinesFromDateRange[:]

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

        myLinesFromDateRange.sort()

        with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
            linesFromDateRange.seek(0)
            if len(linesFromDateRange.read(100)) > 0:
                linesFromDateRange.writelines("\n")
            linesFromDateRange.writelines(myLinesFromDateRange)
    else:
        print("No data was found within the specified date range.")

    print("Closing: " + filenameWithExt)
    del myLinesFromDateRange[:]

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
    plt.show()

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

def evtxFile(absolutePath, filenameWithExt, ext, fromDate, toDate):
    print("Reading: " + filenameWithExt)

    del myLinesFromDateRange[:]
    outText = ""
    channel = ""

    #read the windows event viewer log and convert its contents to XML
    with evtx.Evtx(absolutePath) as log:
        for record in log.records():
            myXMLList.append(record.xml())

    for x, elm in enumerate(myXMLList):
        #remove the namespace from each line of the list
        xmlLine = myXMLList[x].replace(" xmlns=\"http://schemas.microsoft.com/win/2004/08/events/event\"", "")
        #parse each line from the list to XML format
        xmlParse = etree.XML(xmlLine)

        providerName = parseXMLtoString(xmlParse, ".//Provider/@Name")
        qualifiers = parseXMLtoString(xmlParse, ".//EventID/@Qualifiers")
        timestamp = parseXMLtoString(xmlParse, ".//TimeCreated/@SystemTime")
        eventID = parseXMLtoString(xmlParse, ".//EventID/text()")
        level = parseXMLtoString(xmlParse, ".//Level/text()")
        task = parseXMLtoString(xmlParse, ".//Task/text()")
        keywords = parseXMLtoString(xmlParse, ".//Keywords/text()")
        eventRecordID = parseXMLtoString(xmlParse, ".//EventRecordID/text()")
        channel = parseXMLtoString(xmlParse, ".//Channel/text()")
        computer = parseXMLtoString(xmlParse, ".//Computer/text()")
        message = parseXMLtoString(xmlParse, ".//Data/text()")

        date = timestamp[0:10]
        time = timestamp[11:19]
        time = time.replace(".", "")

        _fromDate = datetime.strptime(fromDate, "%Y-%m-%d").date()
        _toDate = datetime.strptime(toDate, "%Y-%m-%d").date()
        _date = datetime.strptime(date, "%Y-%m-%d").date()
                
        if _fromDate <= _date <= _toDate:

            if level == "0" or level == "4":
                level = "Information"
            elif level == "1":
                level = "Critical"
            elif level == "2":
                level = "Error"
            elif level == "3":
                level = "Warning"

            message = message.replace("<string>", "")
            message = message.replace("</string>", "")
            message = message.replace("\t", " ")
            message = message.replace("\r\n", " ")
            message = message.replace("\n", " ")

            outText = date + " " + time + "|" + level + "|" + providerName + "|" + qualifiers + "|" + eventID + "|" + eventRecordID + "|" + task + "|" + keywords + "|" + message.strip() + "|" + computer + "\n"
            if not outText in myLinesFromDateRange:
                        myLinesFromDateRange.append(outText)

    if len(myLinesFromDateRange) > 0:
        createFolder("\\filtered_data_files\\")
        outFilename = "windows_" + channel.lower() + "_event_viewer_logs" + ext

        myLinesFromDateRange.sort()

        with codecs.open(os.getcwd() + "\\filtered_data_files\\" + outFilename, "a+", "utf-8", "ignore") as linesFromDateRange:
            linesFromDateRange.seek(0)
            if len(linesFromDateRange.read(100)) > 0:
                linesFromDateRange.writelines("\n")
            linesFromDateRange.writelines(myLinesFromDateRange)
    else:
        print("No data was found within the specified date range.")

    print("Closing: " + filenameWithExt)
    del myLinesFromDateRange[:]
    
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

    searchDirectory(directoryPath)
    searchDirectory2(directoryPath, myFileExt)

end = datetime.now()
print("\nElapsed time: {0}".format(end-start))
