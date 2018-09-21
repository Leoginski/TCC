import pandas as pd
from sqlalchemy import create_engine

data = pd.read_csv('/home/leoginski/Modelos/TCC/201301_Licitacoes.csv', sep=';', encoding='latin-1', skiprows=1)
data.replace("\"", "\'")

engine = create_engine("mysql://root:root@localhost/tcc?unix_socket=/var/run/mysqld/mysqld.sock")
con = engine.connect()

data.to_sql('licitacoes', con=con, if_exists='append', index=False)
#print(engine.execute("SELECT * FROM participantes").fetchall())

con.close()