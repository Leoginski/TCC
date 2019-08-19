import pandas as pd
import sqlalchemy
import pyodbc
import sys
import os

engine = sqlalchemy.create_engine("mssql+pyodbc://localhost/transparencia?driver=ODBC+Driver+17+for+SQL+Server")
engine.connect()

fail_files = []

item = 0
licitacao = 0
participante = 0

try:
    for filename in os.listdir("../DataSet"):
        if ".csv" in filename:
            print("Loading: " + filename)
            df = pd.read_csv('../DataSet/' + filename, low_memory=False, sep=';', encoding='latin-1')		
            print("Inserting...")
            if "Item" in filename:        
                item = item + len(df)        
                df.to_sql("ItemLicitacao", engine, chunksize=10000, if_exists='append', index=False)
            elif "Participantes" in filename:
                participante = participante + len(df)
                df.to_sql("ParticipanteLicitacao", engine, chunksize=10000, if_exists='append', index=False)
            else:
                licitacao = licitacao + len(df)
                df.to_sql("Licitacao", engine, chunksize=10000, if_exists='append', index=False)        
            print("Done!")
except Exception as e:	
    fail_files.append(filename + '=>' + str(e))
    print("Fail!")

f = open("fail_files.txt", "w+")

f.write("Fails:\r\n")
for file in fail_files:
    f.write(file + "\r\n")

f.write("Item => " + str(item) + "\r\n")    
f.write("Licitacao => " + str(licitacao) + "\r\n")    
f.write("Participante => " + str(participante) + "\r\n")

f.close()
engine.dispose()