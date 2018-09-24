import os
import pandas as pd
from sqlalchemy import create_engine

engine = create_engine(
    'mysql://root:root@localhost/tcc?unix_socket=/var/run/mysqld/mysqld.sock')


def getTable(filename):
    if('Item' in filename):
        return 'item'
    elif('Participantes' in filename):
        return 'participante'
    return 'licitacao'


def types(x):
    return {
        'item': {'Código Órgão': pd.int64.int64,
                 'Nome Órgão': object,
                 'Código UG': pd.int64.int64,
                 'Nome UG': object,
                 'Número Licitação': pd.int64.int64,
                 'CNPJ Vencedor': object,
                 'Nome Vencedor': object,
                 'Número Item': pd.int64.int64,
                 'Descrição': object,
                 'Quantidade Item': pd.int64.int64,
                 'Valor Item': object},
        'licitacao': {'Número Licitação': pd.int64.int64,
                      'Número Processo': object,
                      'Objeto': object,
                      'Modalidade Compra': object,
                      'Situação Licitação': object,
                      'Código Órgão Superior': pd.int64.int64,
                      'Nome Órgão Superior': object,
                      'Código Órgão': pd.int64.int64,
                      'Nome Órgão': object,
                      'Código UG': pd.int64.int64,
                      'Nome UG': object,
                      'Município': object,
                      'Data PublicaçãoDOU': object,
                      'Data Abertura': object,
                      'Valor Licitação': object},
        'participante': {'Código Órgão': pd.int64.int64,
                         'Nome Órgão': object,
                         'Código UG': pd.int64.int64,
                         'Nome UG': object,
                         'Número Licitação': pd.int64.int64,
                         'Código Item Compra': pd.int64.int64,
                         'Descrição Item Compra': object,
                         'CNPJ Participante': object,
                         'Nome Participante': object,
                         'Flag Vencedor': object}
    }[x]


def writeFile(path, filename):
    f = open(path, 'a')
    f.write(filename + '\r\n')
    f.close()


def getLines(path):
    f = open(path, 'r')
    return [line for line in f.readlines()]
    # return pd.read_csv(path, sep="\r\n", header=None)


def insert(filename):
    try:
        data = pd.read_csv('../DataSet/' + filename,
                           sep=';', encoding='latin-1')

        table = getTable(filename)
        columns = types(table)
        data.to_sql(table, dtype=columns, con=engine, if_exists='append')
        print('Inserting...' + filename)
    except:
        print(filename + ' failed!')
        writeFile('failCSV.txt', filename)


def hasFails():
    fails = [line.replace('\n', '') for line in getLines('failCSV.txt')]
    open('failCSV.txt', 'w').close()

    for filename in fails:
        insert(filename)

    return len(getLines('failCSV.txt')) != 0


def main():
    done = [line.replace('\n', '') for line in getLines('doneCSV.txt')]
    files = [x for x in os.listdir('../DataSet') if x not in done]

    for filename in files:
        insert(filename)
        writeFile('doneCSV.txt', filename)

    while(hasFails()):
        print('Has fails again...')


main()
