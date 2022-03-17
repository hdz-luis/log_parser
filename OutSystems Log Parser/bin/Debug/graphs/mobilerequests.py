import os
import plotly
import plotly.graph_objects as go
import pandas as pd
import plotly.express as px
from datetime import datetime
import warnings

def create_graph(directoryPath):
    foundLogs = False
    with warnings.catch_warnings():
        warnings.warn("", DeprecationWarning)
        warnings.filterwarnings("ignore")
        for root, subFolders, files in os.walk(directoryPath):
            for f in files:
                if "mobilerequestslog" in f.lower():
                    foundLogs = True
                    print("Creating graphs from the Mobile Requests logs")
                    absolutePathOfFile = os.path.join(root, f)
                    start = datetime.now()
                    pd.options.mode.chained_assignment = None  # default="warn"
                    #PROCESSING THE EXCEL FILE
                    df_init = pd.read_excel(absolutePathOfFile,engine="openpyxl")
                    df_init["Screen&Action"]= df_init["SCREEN"].str.cat(df_init["ENDPOINT"], sep=" - ")
                    ##### PLOTTING Scatter chart######
                    print("Plotting the scatter graphs")
                    #using plotly to create a scatter graph
                    fig_scatter= px.scatter(df_init, x="INSTANT", y="DURATION",color="Screen&Action",height=500,title="Mobile Screen Analysis")
                    fig_scatter.update_layout(showlegend=False,title={       
                            "y":0.9,
                            "x":0.5,
                            "xanchor": "center",
                            "yanchor": "top"})

                    #using plotly to create a scatter graph
                    fig_scatter2= px.scatter(df_init, x="INSTANT", y="Screen&Action",color="SOURCE",height=500,title="Source of the Mobile Requests")
                    fig_scatter2.update_layout(showlegend=False,title={       
                            "y":0.9,
                            "x":0.5,
                            "xanchor": "center",
                            "yanchor": "top"})

                    #intializing a new dataframe from the filtered one
                    df_dateonly = df_init

                    #rounding up the instant to an hour timeframe
                    df_dateonly.assign(INSTANT=df_dateonly.INSTANT.dt.round("H"))
                    #removign the time and keeping only the date on the INSTANT column
                    df_dateonly["INSTANT"] = pd.to_datetime(df_dateonly["INSTANT"]).dt.date
                    #gouping by INSTANT and also by Screen&Action and counting the number of occurences
                    new = df_dateonly.groupby(["INSTANT","Screen&Action"]).size()
                    #converting the above series type variable to a dataframe
                    df= new.to_frame().reset_index()
                    #changing the name of the count column to "count"
                    df = df.rename(columns= {0: "count"})

                    ##### PLOTTING BAR CHARTS ######
                    #using plotly to create a bar chart
                    print("Plotting the bar graphs")
                    fig_bar = px.bar(df, x="INSTANT", y="count",
                                 hover_data=["Screen&Action", "count"], color="Screen&Action",
                                 labels={"pop":"Count"}, height=500,title="Mobile Screen Count")

                    fig_bar.update_xaxes(type="category")
                    fig_bar.update_layout(showlegend=False,title={
                            "y":0.9,
                            "x":0.5,
                            "xanchor": "center",
                            "yanchor": "top"}
                    )

                    #gouping by INSTANT and also by SOURCE and counting the number of occurences of each source
                    new = df_dateonly.groupby(["INSTANT","SOURCE"]).size()
                    #converting the above series type variable to a dataframe
                    df= new.to_frame().reset_index()
                    #changing the name of the count column to "count"
                    df = df.rename(columns= {0: "count"})

                    #using plotly to create a bar chart
                    fig_bar2 = px.bar(df, x="INSTANT", y="count",
                                 hover_data=["SOURCE", "count"], color="SOURCE",
                                 labels={"pop":"Count"}, height=500,title="Mobile Requests Source Count")

                    fig_bar2.update_xaxes(type="category")
                    fig_bar2.update_layout(showlegend=False,title={
                            "y":0.9,
                            "x":0.5,
                            "xanchor": "center",
                            "yanchor": "top"}
                    )

                    fig_scatter.show()
                    fig_scatter2.show()
                    fig_bar.show()
                    fig_bar2.show()

                    print("The graphs from the Mobile Requests logs were created. Please check your browser.")

                    end = datetime.now()
                    print("\nElapsed time: {0}".format(end-start))

    if not foundLogs:
        print("The Mobile Requests logs were not found in folder: \"" + directoryPath + "\"")

directoryPath = input("In what folder are the Mobile Requests logs located? ")
create_graph(directoryPath)
input("\nPress \"Enter\" to close the script.")
