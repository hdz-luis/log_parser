import os
import plotly
import plotly.graph_objects as go
import pandas as pd
import plotly.express as px

def create_graph(directoryPath):
    for root, subFolders, files in os.walk(directoryPath):
        for f in files:
            if "integrationslog" in f.lower():
                print("Creating graphs from the Integrations logs")
                absolutePathOfFile = os.path.join(root, f)
                pd.options.mode.chained_assignment = None  # default="warn"
                #PROCESSING THE EXCEL FILE
                df_init = pd.read_excel(absolutePathOfFile,engine="openpyxl",usecols="B,C,F")
                ##### PLOTTING Scatter Graph ######
                print("Plotting the scatter graph")
                #using plotly to create a scatter graph
                fig_scatter= px.scatter(df_init, x="INSTANT", y="DURATION",color="ACTION",height=500,title="SLOW INTEGRATIONS ANALYSIS")
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
                #gouping by instant, Integrations, by the number of occurrences of integrations
                new = df_dateonly.groupby(["INSTANT","ACTION"]).size()
                #converting the above series type variable to a dataframe
                df= new.to_frame().reset_index()
                #changing the name of the count column to "count"
                df = df.rename(columns= {0: "count"})

                ##### PLOTTING Bar Graph ######
                #using plotly to create a bar graph
                print("Plotting the bar graph")
                fig_bar = px.bar(df, x="INSTANT", y="count",
                             hover_data=["ACTION", "count"], color="ACTION",
                             labels={"pop":"Count"}, height=500,title="SLOW INTEGRATIONS COUNT")

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

                print("The graphs from the Integrations logs were created. Please check your browser.")
