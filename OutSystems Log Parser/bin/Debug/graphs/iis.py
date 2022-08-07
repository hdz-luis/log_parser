import os
import re
import codecs
import traceback
import pandas as pd
import plotly.express as px
from datetime import datetime

myTimestampList = []
myServerIPList = []
myMethodList = []
myUriStemList = []
myServerPortList = []
myClientIPList = []
myHTTPcodeList = []
myTimeTakenList = []

iisFilenameRegex = r"^((?!(?:u_ex(?:[\d]{6}))_x).*)"
iisInfoRegex = r"^([\d\-]+)[ ]([\d\:]+)[ ]([\w\-\.\:\%]+)[ ](POST|PUT|PROPFIND|(?:n)?GET|OPTIONS|HEAD|ABCD|QUALYS|TRACE|SEARCH|RNDMMTD|TRACK|B(?:A)?DM(?:E)?T(?:H)?(?:O)?(?:D)?|CFYZ|DEBUG|MKCOL|INDEX|DELETE|PATCH|ACUNETIX)[ ]([\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ].*?[ ]([\d]+)[ ](?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+)[ ]([\w\-\.\:\%]+)[ ](?:[\w\(\)\[\]\{\}\-\–\—\:\;\‘\’\'\"\“\”\,\.\<\>\«\»\`\~\|\á\Á\à\À\â\Â\ã\Ã\é\É\è\È\ê\Ê\í\Í\ì\Ì\î\Î\ó\Ó\ò\Ò\ô\Ô\õ\Õ\ú\Ú\ù\Ù\û\Û\ü\Ü\ñ\Ñ\ç\Ç\&\=\\\/\?\+\$\@\%\^\#\*\!\¿\¡\£\€\¢\¥\©\®]+).*?[ ]([\d]+)(?:(?:[\d ]+){2})[ ]([\d]+)"
errorLogPath = os.getcwd() + "\\error_log.txt"

def error_log(error_message):
    with codecs.open(errorLogPath, "w", "utf-8", "ignore") as errorFile:
        errorFile.writelines("Unexpected Error:\n" + str(error_message).strip())

def create_graph(directoryPath, iisInfoRegex):
    try:
        foundFile = False
        for root, subFolders, files in os.walk(directoryPath):
            for f in files:
                filenameRegex = re.match(iisFilenameRegex, f)
                if filenameRegex:
                    file_name = filenameRegex.group(1)
                    foundFile = True
                    print("Creating the graphs from the IIS logs")
                    absolutePathOfFile = os.path.join(root, file_name)
                    start = datetime.now()
                    with codecs.open(absolutePathOfFile, "r", "utf-8", "ignore") as tempFile:
                        searchLines = tempFile.read()
                        regex = re.compile(iisInfoRegex, re.MULTILINE + re.IGNORECASE)
                        for match in regex.finditer(searchLines):
                            myDate = match.group(1)
                            myTime = match.group(2)
                            myServerIP = match.group(3)
                            myMethod = match.group(4)
                            myUriStem = match.group(5)
                            myServerPort = match.group(6)
                            myClientIP = match.group(7)
                            myHTTPcode = match.group(8)
                            myTimeTaken = match.group(9)

                            myTimestampList.append(myDate + " " + myTime)
                            myServerIPList.append(myServerIP)
                            myMethodList.append(myMethod)
                            myUriStemList.append(myUriStem)
                            myServerPortList.append(myServerPort)
                            myClientIPList.append(myClientIP)
                            myHTTPcodeList.append(myHTTPcode)
                            #timeTaken is in milliseconds and it needs to be converted to seconds
                            seconds = int(myTimeTaken)/1000
                            myTimeTakenList.append(float(seconds))
          
                    plotdata = pd.DataFrame({"TIMESTAMP": myTimestampList,
                                             "TIME_TAKEN_SECONDS": myTimeTakenList,
                                             "HTTP_CODE": myHTTPcodeList,
                                             "CLIENT_IP": myClientIPList,
                                             "SERVER_IP": myServerIPList,
                                             "SERVER_PORT": myServerPortList,
                                             "METHOD": myMethodList,
                                             "PAGE": myUriStemList})
                    fig_line = px.line(plotdata,
                                       x="TIMESTAMP",
                                       y="TIME_TAKEN_SECONDS",
                                       hover_data=["TIMESTAMP", "TIME_TAKEN_SECONDS", "HTTP_CODE", "CLIENT_IP", "SERVER_IP", "SERVER_PORT", "METHOD", "PAGE"],
                                       labels={"pop":"TIME_TAKEN_SECONDS"},
                                       height=500,
                                       color_discrete_sequence=["#109618"],
                                       title="Web Requests And Their Time Taken")
                    fig_line.update_layout(showlegend=False, title={"y":0.9, "x":0.5, "xanchor": "center", "yanchor": "top"})
                    fig_line.update_traces(dict(marker_line_width=0))
                    fig_line.show()

                    newDf = plotdata.groupby(["TIMESTAMP","HTTP_CODE"]).size()
                    df = newDf.to_frame().reset_index()
                    df = df.rename(columns= {0: "COUNT"})
                    fig_bar = px.bar(df,
                                     x="TIMESTAMP",
                                     y="COUNT",
                                     hover_data=["TIMESTAMP", "HTTP_CODE", "COUNT"],
                                     color="HTTP_CODE",
                                     labels={"pop":"HTTP_CODE"},
                                     height=500,
                                     color_discrete_map={
                                         "301": "red",
                                         "400": "red",
                                         "401": "red",
                                         "403": "red",
                                         "404": "red",
                                         "405": "red",
                                         "406": "red",
                                         "407": "red",
                                         "408": "red",
                                         "409": "red",
                                         "410": "red",
                                         "411": "red",
                                         "412": "red",
                                         "413": "red",
                                         "414": "red",
                                         "415": "red",
                                         "416": "red",
                                         "417": "red",
                                         "418": "red",
                                         "422": "red",
                                         "426": "red",
                                         "428": "red",
                                         "429": "red",
                                         "431": "red",
                                         "451": "red",
                                         "500": "red",
                                         "501": "red",
                                         "502": "red",
                                         "503": "red",
                                         "504": "red",
                                         "505": "red",
                                         "506": "red",
                                         "507": "red",
                                         "508": "red",
                                         "510": "red",
                                         "511": "red",
                                         "110": "yellow",
                                         "111": "yellow",
                                         "112": "yellow",
                                         "113": "yellow",
                                         "199": "yellow",
                                         "214": "yellow",
                                         "299": "yellow",
                                         "9009": "yellow",
                                         "100": "blue",
                                         "101": "blue",
                                         "103": "blue",
                                         "200": "blue",
                                         "201": "blue",
                                         "202": "blue",
                                         "203": "blue",
                                         "204": "blue",
                                         "205": "blue",
                                         "206": "blue",
                                         "300": "blue",
                                         "302": "blue",
                                         "303": "blue",
                                         "304": "blue",
                                         "307": "blue",
                                         "308": "blue"
                                         },
                                     title="Web Requests And Their Status")
                    fig_bar.update_layout(showlegend=False, title={"y":0.9, "x":0.5, "xanchor": "center", "yanchor": "top"})
                    fig_bar.update_traces(dict(marker_line_width=0))
                    fig_bar.show()

                    print("The graphs from the IIS logs were created. Please check your browser.")

                    del myTimestampList[:]
                    del myServerIPList[:]
                    del myMethodList[:]
                    del myUriStemList[:]
                    del myServerPortList[:]
                    del myClientIPList[:]
                    del myHTTPcodeList[:]
                    del myTimeTakenList[:]
        if not foundFile:
            print("The IIS logs were not found in folder: \"" + directoryPath + "\"")
    except Exception as e:
        error_stack = traceback.format_exc()
        error_log(error_stack)
        print("A file has been created with the error message.\n\nUnexpected Error:\n", error_stack)

directoryPath = input("In what folder are the IIS logs located? ")
create_graph(directoryPath, iisInfoRegex)
input("\nPress \"Enter\" to close the script.")
