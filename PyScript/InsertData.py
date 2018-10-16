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
        'item': {'Código Órgão': 'int64',
                 'Nome Órgão': 'object',
                 'Código UG': 'int64',
                 'Nome UG': 'object',
                 'Número Licitação': 'int64',
                 'CNPJ Vencedor': 'object',
                 'Nome Vencedor': 'object',
                 'Número Item': 'int64',
                 'Descrição': 'object',
                 'Quantidade Item': 'int64',
                 'Valor Item': 'object'},
        'licitacao': {'Número Licitação': 'int64',
                      'Número Processo': 'object',
                      'Objeto': 'object',
                      'Modalidade Compra': 'object',
                      'Situação Licitação': 'object',
                      'Código Órgão Superior': 'int64',
                      'Nome Órgão Superior': 'object',
                      'Código Órgão': 'int64',
                      'Nome Órgão': 'object',
                      'Código UG': 'int64',
                      'Nome UG': 'object',
                      'Município': 'object',
                      'Data PublicaçãoDOU': 'object',
                      'Data Abertura': 'object',
                      'Valor Licitação': 'object'},
        'participante': {'Código Órgão': 'int64',
                         'Nome Órgão': 'object',
                         'Código UG': 'int64',
                         'Nome UG': 'object',
                         'Número Licitação': 'int64',
                         'Código Item Compra': 'object',
                         'Descrição Item Compra': 'object',
                         'CNPJ Participante': 'object',
                         'Nome Participante': 'object',
                         'Flag Vencedor': 'object'}
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
        # filename = '201801_ParticipantesLicitacao.csv'
        table = getTable(filename)
        columns = types(table)

        # https://www.dataquest.io/blog/pandas-big-data/
        
        data = pd.read_csv('../DataSet/'+ filename, dtype=columns,
                           sep=';', encoding='latin-1')
                           
        print('Inserting...' + filename + ' on ' + table)
        data.to_sql(table, con=engine, if_exists='append')
        writeFile('doneCSV.txt', filename)
        print('Done!')
    except Exception as e:
        print(e)
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
    fail = [line.replace('\n', '') for line in getLines('failCSV.txt')]
    files = [x for x in os.listdir('../DataSet') if x not in done]
    files = [x for x in files if x not in fail]

    for filename in files:
        insert(filename)

    # while(hasFails()):
    #     print('Has fails again...')
    print('Finish!')


main()
