import os
import re
import codecs
import pandas as pd
import plotly.express as px
from datetime import datetime

myOsList = []
myOsVersionList = []
myOccurrencesList = []
deviceInfoRegex = r"^([\w]+)\|([\d\.]+)\|([\d]+)\|.+"

def create_graph(directoryPath, deviceInfoRegex):
    foundFile = False
    for root, subFolders, files in os.walk(directoryPath):
        for f in files:
            if "device_information" in f.lower():
                foundFile = True
                print("Creating the graph from the Device Information file")
                absolutePathOfFile = os.path.join(root, f)
                start = datetime.now()
                with codecs.open(absolutePathOfFile, "r", "utf-8", "ignore") as tempFile:
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

                end = datetime.now()
                print("\nElapsed time: {0}".format(end-start))

    if not foundFile:
        print("The Device Information file was not found in folder: \"" + directoryPath + "\"")

directoryPath = input("In what folder is the Device Information file located? ")
create_graph(directoryPath, deviceInfoRegex)
input("\nPress \"Enter\" to close the script.")
