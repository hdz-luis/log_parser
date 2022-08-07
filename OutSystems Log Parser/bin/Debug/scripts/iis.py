import os
import re
import codecs
import pandas as pd
import plotly.express as px

myTimestampList = []
myTimeTakenList = []
myHTTPcodeList = []
myClientIPList = []
myServerIPList = []
myServerPortList = []
myMethodList = []
myUriStemList = []

def create_graph(txtFile, iisInfoRegex):
    try:
        with codecs.open(txtFile, "r", "utf-8", "ignore") as tempFile:
            searchLines = tempFile.read()
            regex = re.compile(iisInfoRegex, re.MULTILINE + re.IGNORECASE)
            for match in regex.finditer(searchLines):
                myTimestamp = match.group(1)
                myTimeTaken = match.group(2)
                myHTTPcode = match.group(3)
                myClientIP = match.group(4)
                myServerIP = match.group(5)
                myServerPort = match.group(6)
                myMethod = match.group(7)
                myUriStem = match.group(8)

                myTimestampList.append(myTimestamp)
                myTimeTakenList.append(float(myTimeTaken))
                myHTTPcodeList.append(myHTTPcode)
                myClientIPList.append(myClientIP)
                myServerIPList.append(myServerIP)
                myServerPortList.append(myServerPort)
                myMethodList.append(myMethod)
                myUriStemList.append(myUriStem)
                                
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
        del myTimeTakenList[:]
        del myHTTPcodeList[:]
        del myClientIPList[:]
        del myServerIPList[:]
        del myServerPortList[:]
        del myMethodList[:]
        del myUriStemList[:]
    except FileNotFoundError as fileError:
        pass
