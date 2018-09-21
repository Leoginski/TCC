import os
import pandas as pd
from sqlalchemy import create_engine

engine = create_engine(
    "mysql://root:root@localhost/tcc?unix_socket=/var/run/mysqld/mysqld.sock")

for filename in os.listdir('../DataSet'):
    if(filename.str.contains('_Item')):
        table = 'item'
    elif(filename.str.contains('Participantes')):
        table = 'participante'
    else:
        table = 'licitacao'

    data = pd.read_csv(('/home/leoginski/Modelos/TCC/DataSet/%s',
                        filename), sep=';', encoding='latin-1')
    data.to_sql(table, con=engine, if_exists='append')


