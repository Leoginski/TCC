import pandas as pd
import sqlalchemy
import pyodbc

li = []
df = pd.read_csv('../DataSet/201903_ParticipantesLicitação.csv', index_col=None, header=0, low_memory=False, sep=';', encoding='latin-1')
li.append(df)
df = pd.read_csv('../DataSet/201904_ParticipantesLicitação.csv', index_col=None, header=0, low_memory=False, sep=';', encoding='latin-1')
li.append(df)
df = pd.read_csv('../DataSet/201905_ParticipantesLicitação.csv', index_col=None, header=0, low_memory=False, sep=';', encoding='latin-1')
li.append(df)

df = pd.concat(li, axis=0, ignore_index=True)

df.drop(columns=['Nome Órgão', 'Nome UG', 'Descrição Item Compra', 'Nome Participante'], inplace=True)
df.rename(columns={'Número Licitação':'numero_licitacao', 'Número Processo':'numero_processo', 'Código Órgão':'codigo_orgao', 'Código UG':'codigo_ug', 'Código Item Compra': 'item', 'CNPJ Participante':'cnpj', 'Flag Vencedor':'flag'}, inplace=True)

columns = [
'numero_licitacao',
'numero_processo',
'codigo_orgao',
'codigo_ug',
'item',
'cnpj',
'flag']

df.drop_duplicates(subset=columns, keep='first', inplace=True)

groups = df.groupby(['numero_licitacao', 'item'])


len(df['cnpj'].unique())