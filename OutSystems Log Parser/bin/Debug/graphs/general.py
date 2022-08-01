import os
import plotly
import plotly.graph_objects as go
import pandas as pd
import plotly.express as px
from datetime import datetime

def create_graph(directoryPath):
    foundLogs = False
    for root, subFolders, files in os.walk(directoryPath):
        for f in files:
            if "generallog" in f.lower():
                foundLogs = True
                print("Creating graphs from the General logs")
                absolutePathOfFile = os.path.join(root, f)
                start = datetime.now()
                pd.options.mode.chained_assignment = None  # default="warn"
                #PROCESSING THE EXCEL FILE
                df_init = pd.read_excel(absolutePathOfFile,engine="openpyxl",usecols="B,G,I")

                #FILTERING ONLY BY SLOWSQL and SLOWEXTENSION
                df_filtered= df_init.loc[(df_init["Module_Name"] == "SLOWSQL")|(df_init["Module_Name"] =="SLOWEXTENSION")]
                df_filtered= df_filtered[df_filtered["MESSAGE"].str.contains(r"took | ms")]

                #SEPARATING THE DURATION OF THE SQL FROM THE MESSAGE and putting them in a vector in a new column
                df_filtered["Message_2"] = df_filtered["MESSAGE"].str.split("took ")
                #INITIALIZING THE COLUMNS TO STORE THE RESULTS FROM THE SPLIT
                df_filtered["duration_temp"]="null"
                df_filtered["SQL/Extension"]="null"

                #iterating over all rows so that we can assign the duration based on the output from the split above
                for index, row in df_filtered.iterrows():
                    df_filtered["duration_temp"][index]= df_filtered["Message_2"][index][1]
                #removing the "ms" word to store only the time 
                df_filtered["duration"] = df_filtered["duration_temp"].str.split(" ms")
                #storing the time splitted from the "ms" word in a brand new column
                for index, row in df_filtered.iterrows():
                    df_filtered["duration"][index]= df_filtered["duration"][index][0]
                #storing the slow query information in a new single column 
                for index, row in df_filtered.iterrows():
                    df_filtered["SQL/Extension"][index]= df_filtered["Message_2"][index][0]
                    
                df_filtered["duration"] = pd.to_numeric(df_filtered["duration"])

                ##### PLOTTING Scatter Graph ######
                print("Plotting the scatter graph")
                #using plotly to create a scatter graph
                fig_scatter= px.scatter(df_filtered, x="INSTANT", y="duration",color="SQL/Extension",height=500,title="SLOWSQL & SLOWEXTENSION ANALYSIS")
                fig_scatter.update_layout(showlegend=False,title={       
                        "y":0.9,
                        "x":0.5,
                        "xanchor": "center",
                        "yanchor": "top"})

                #PREPARING THE BAR CHART DATAFRAME
                #intializing a new dataframe from the filtered one
                df_filtered_dateonly = df_filtered
                df_filtered_dateonly.assign(INSTANT=df_filtered_dateonly.INSTANT.dt.round("H"))
                #removing the time and keeping only the date on the Instant column
                df_filtered_dateonly["INSTANT"] = pd.to_datetime(df_filtered_dateonly["INSTANT"]).dt.date
                #gouping by instant, Slow SQLS, by the number of occurrences of SQLS
                new = df_filtered_dateonly.groupby(["INSTANT","SQL/Extension"]).size()
                #converting the above series type variable to a dataframe
                df= new.to_frame().reset_index()
                #changing the name of the count column to "count"
                df = df.rename(columns= {0: "count"})

                ##### PLOTTING Bar Graph ######
                #using plotly to create a bar graph
                print("Plotting the bar graph")
                fig_bar = px.bar(df, x="INSTANT", y="count",
                             hover_data=["SQL/Extension", "count"], color="SQL/Extension",
                             labels={"pop":"Count"}, height=500,title="SLOWSQL & SLOWEXTENSION COUNT")

                fig_bar.update_xaxes(type="category")
                fig_bar.update_layout(showlegend=False,title={
                        "y":0.9,
                        "x":0.5,
                        "xanchor": "center",
                        "yanchor": "top"}
                )

                fig_scatter.update_traces(dict(marker_line_width=0))
                fig_bar.update_traces(dict(marker_line_width=0))

                fig_scatter.show()
                fig_bar.show()

                print("The graphs from the General logs were created. Please check your browser.")

                end = datetime.now()
                print("\nElapsed time: {0}".format(end-start))

    if not foundLogs:
        print("The General logs were not found in folder: \"" + directoryPath + "\"")

directoryPath = input("In what folder are the General logs located? ")
create_graph(directoryPath)
input("\nPress \"Enter\" to close the script.")
