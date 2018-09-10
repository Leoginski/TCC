import datetime
import requests
import zipfile
import io

fail_urls = []
save_path = '../DataSet/'
link = 'http://www.portaltransparencia.gov.br/download-de-dados/licitacoes/'


def main():
    for url in getUrls():
        response = doRequest(url)
        saveResponse(response)

    if fail_urls:
        saveFail()


def getUrls():
    urls = []
    for i in range(2013, 2019):
        for j in range(1, 13):
            urls.append(link + str(i) + str(format(j, '02')))
    return urls


def doRequest(url):
    print('Requesting: ' + url)
    r = requests.get(url)

    if r.status_code != 200:
        print('Failed!')
        fail_urls.append("Status: " + r.status_code + " URL: " + url)

    r.encoding = 'utf-8'
    return r.content


def saveResponse(response):
    if response:
        print("Success!")
        z = zipfile.ZipFile(io.BytesIO(response))
        z.extractall(save_path)


def saveFail():
    f = open("fail_urls.txt", "w+")
    f.write("Data da Execucao: " + datetime.datetime.now().strftime('%d/%m/%Y %H:%M') + "\r\n")

    for url in fail_urls:
        f.write(url + "\r\n")

    f.close()


main()