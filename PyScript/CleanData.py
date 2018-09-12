import os
import pandas as pd

csv_data = pd.read_csv('../DataSet/201301_ItemLicitaç╞o.csv', sep=';', encoding='latin-1', skiprows=1)
csv_data.to_csv('../DataSet/teste.csv', sep=';', encoding='utf-8')