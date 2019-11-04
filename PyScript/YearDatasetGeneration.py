import pandas as pd
import datetime
import os

extension = '.csv'
directory = '../DataSet'
domain = 'Participantes'

for x in range(2013, 2020):
    li = []
    print('Filtering year: ' + str(x))
    #Varre a pasta capturando os arquivos desejados em uma lista
    for filename in os.listdir(directory):
        if extension in filename and domain in filename and str(x) in filename:
            print(filename)
            df = pd.read_csv('../DataSet/' + filename, low_memory=False, sep=';', encoding='latin-1')
            df.drop(columns=['Nome Órgão', 'Nome UG', 'Descrição Item Compra', 'Nome Participante', 'Código Órgão','Código UG'], inplace=True)
            if 'Número Processo' in df.columns:
                df.drop(columns=['Número Processo'], inplace=True)
            li.append(df)
    #Concatena a lista de arquivos em um dataframe unico
    df = pd.concat(li, axis=0, ignore_index=True)   
    print('Concatenando datasets...')
    li = []
    #Renomeia, Dropa e Agrupa algumas colunas
    df.rename(columns={'Número Licitação':'numero_licitacao', 'Código Item Compra': 'item', 'CNPJ Participante':'cnpj', 'Flag Vencedor':'flag'}, inplace=True)
    df.drop_duplicates(subset=['numero_licitacao', 'item', 'flag'], keep='first', inplace=True)
    print('Agrupando')
    df = df.groupby(['numero_licitacao', 'item' ])
    #Remove eventos de acordo com condições de vitória
    print('Filtrando vitorias')
    df = df.filter(lambda x:  sum(x['flag']=='SIM')==1 and sum(x['flag']=='NÃO') >= 1)
    #Concatena licitação e item como evento
    print('Criando eventos')
    df["event"] = df["numero_licitacao"].map(str) + str(df["item"])

    outputhPath = directory + '/results/'
    #Salva CSV apenas com os campos relevantes para o Apriori
    print('Salvando')
    df.drop(columns=['numero_licitacao', 'item', 'flag']).to_csv(outputhPath+'input'+str(x)+'.csv', index=False, sep=';', header=True, encoding='latin-1')