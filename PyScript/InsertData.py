import os
import pandas as pd
import pymysql.cursors
from sqlalchemy import create_engine


engine = create_engine("mysql://root:root@localhost/tcc")
con = engine.connect()

# connection = pymysql.connect(host='localhost',
#                              user='root',
#                              password='root',
#                              db='tcc',
#                              local_infile=True,
#                              cursorclass=pymysql.cursors.DictCursor)

# with connection.cursor() as cursor:
#     sql = "LOAD DATA LOCAL INFILE '/home/leoginski/Modelos/TCC/DataSet/201301_LicitaЗ╞o.csv' INTO TABLE licitacoes FIELDS TERMINATED BY ';'"
#     cursor.execute(sql)
#     connection.commit()

csv_data = pd.read_csv('/home/leoginski/Modelos/TCC/DataSet/201301_LicitaЗ╞o.csv',
                       sep=';', encoding='latin-1', skiprows=1)

# csv_data.to_csv('../DataSet/teste.csv', sep=';', encoding='utf-8')

csv_data.to_sql(name='licitacoes',con=con,if_exists='append')

# datatables = ['licitacoes', 'participantes']
# for table in datatables:

# for filename in os.listdir('../DataSet'):
#     try:
#         with connection.cursor() as cursor:
#             sql = "LOAD DATA LOCAL INFILE `/home/leoginski/Modelos/TCC/DataSet/%s` INTO TABLE %s FIELDS TERMINATED BY ';'"
#             cursor.execute(sql, (filename, 'licitacoes'))
#     finally:
#         connection.commit()

con.close()
