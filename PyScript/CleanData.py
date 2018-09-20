import os
import pandas as pd

data = pd.read_csv('/home/leoginski/Modelos/TCC/DataSet/201301_ItemLicitaç╞o.csv', sep=';', encoding='latin-1', skiprows=1)
data.head()

# csv_data.to_csv('../DataSet/teste.csv', sep=';', encoding='utf-8')