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
            if "timerlog" in f.lower():
                foundLogs = True
                print("Creating graphs from the Timer logs")
                absolutePathOfFile = os.path.join(root, f)
                start = datetime.now()
                pd.options.mode.chained_assignment = None  # default="warn"
                #PROCESSING THE EXCEL FILE
                df_init = pd.read_excel(absolutePathOfFile,engine="openpyxl")
                ##### PLOTTING Scatter Graph ######
                print("Plotting the scatter graph")
                #using plotly to create a scatter graph
                fig_scatter= px.scatter(df_init, x="INSTANT", y="DURATION",color="Cyclic_Job_Name",height=500,title="TIMERS ANALYSIS")
                fig_scatter.update_layout(showlegend=False,title={       
                        "y":0.9,
                        "x":0.5,
                        "xanchor": "center",
                        "yanchor": "top"})

                #intializing a new dataframe from the filtered one
                df_dateonly = df_init

                df_dateonly.assign(INSTANT=df_dateonly.INSTANT.dt.round("H"))
                #removing the time and keeping only the date on the Instant column
                df_dateonly["INSTANT"] = pd.to_datetime(df_dateonly["INSTANT"]).dt.date
                #gouping by instant, Timer, and by the number of occurrences of timers
                new = df_dateonly.groupby(["INSTANT","Cyclic_Job_Name"]).size()
                #converting the above series type variable to a dataframe
                df= new.to_frame().reset_index()
                #changing the name of the count column to "count"
                df = df.rename(columns= {0: "count"})

                ##### PLOTTING Bar Graph ######
                #using plotly to create a bar graph
                print("Plotting the bar graph")
                fig_bar = px.bar(df, x="INSTANT", y="count",
                             hover_data=["Cyclic_Job_Name", "count"], color="Cyclic_Job_Name",
                             labels={"pop":"Count"}, height=500,title="TIMERS ANALYSIS")

                fig_bar.update_xaxes(type="category")
                fig_bar.update_layout(showlegend=False,title={
                        "y":0.9,
                        "x":0.5,
                        "xanchor": "center",
                        "yanchor": "top"}
                )

                fig_scatter.show()
                fig_bar.show()

                print("The graphs from the Timer logs were created. Please check your browser.")

                end = datetime.now()
                print("\nElapsed time: {0}".format(end-start))

    if not foundLogs:
        print("The Timer logs were not found in folder: \"" + directoryPath + "\"")

directoryPath = input("In what folder are the Timer logs located? ")
create_graph(directoryPath)
input("\nPress \"Enter\" to close the script.")
