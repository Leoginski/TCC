import pandas as pd
import os

fail_files = []

item = 0
licitacao = 0
participante = 0

try:
    for filename in os.listdir("../DataSet"):
        if ".csv" in filename:
            print(filename)
            df = pd.read_csv('../DataSet/' + filename, low_memory=False, sep=';', encoding='latin-1')		                        
            if "Item" in filename:        
                item = item + len(df)        
            elif "Participantes" in filename:
                participante = participante + len(df)
            else:
                licitacao = licitacao + len(df)
except Exception as e:	
    fail_files.append(filename + '=>' + str(e))
    print("Fail!")

f = open("count_Lines.txt", "w+")

f.write("Fails:\r\n")
for file in fail_files:
    f.write(file + "\r\n")

f.write("Item => " + str(item) + "\r\n")    
f.write("Licitacao => " + str(licitacao) + "\r\n")    
f.write("Participante => " + str(participante) + "\r\n")

f.close()