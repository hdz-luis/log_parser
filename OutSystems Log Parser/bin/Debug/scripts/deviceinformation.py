import os
import re
import codecs
import pandas as pd
import plotly.express as px

myOsList = []
myOsVersionList = []
myOccurrencesList = []

def create_graph(txtFile, deviceInfoRegex):
    with codecs.open(txtFile, "r", "utf-8", "ignore") as tempFile:
        searchLines = tempFile.read()
        regex = re.compile(deviceInfoRegex, re.MULTILINE + re.IGNORECASE)
        for match in regex.finditer(searchLines):
            myOs = match.group(1)
            myOsVersion = match.group(2)
            myOccurrences = match.group(3)

            myOsList.append(myOs)
            myOsVersionList.append(myOsVersion)
            myOccurrencesList.append(int(myOccurrences))

    plotdata = pd.DataFrame({"OS": myOsList, "OS_VERSION": myOsVersionList, "OCCURRENCES": myOccurrencesList})
    fig_bar = px.bar(plotdata,
                     x="OS_VERSION",
                     y="OCCURRENCES",
                     hover_data=["OS", "OCCURRENCES"],
                     color="OS",
                     labels={"pop":"OCCURRENCES"},
                     height=500,
                     color_discrete_map={"Android": "green", "iOS": "goldenrod"},
                     title="Mobile OS: Their Versions And Their Occurrences")
    fig_bar.update_layout(showlegend=False, title={"y":0.9, "x":0.5, "xanchor": "center", "yanchor": "top"})
    fig_bar.update_traces(dict(marker_line_width=0))
    fig_bar.show()

    print("The graph from the Device Information file was created. Please check your browser.")

    del myOsList[:]
    del myOsVersionList[:]
    del myOccurrencesList[:]
