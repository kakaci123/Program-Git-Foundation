from bs4 import BeautifulSoup
import os
import time
from selenium import webdriver

#initial browser
adr = os.getcwd() + '/'
driver = webdriver.Chrome(adr + 'chromedriver')

def CrawlerETC(url):

    driver.get(url)

    AllDateList = list()
    DateList = list()
    FinalList = list()
    
    soup = BeautifulSoup(driver.page_source, 'html.parser')
    for j in soup.findAll('a'):
        temp = j.get('href')    
        if temp is not None:
            if len(temp) == 9 and ('20150524' in temp or '20150523' in temp  or '20150519' in temp or '20150518' in temp or '20150517' in temp or '20150516' in temp) and temp not in AllDateList:
                AllDateList.append(temp)
          
    for j in AllDateList:
        driver.get(url + j)
        soup = BeautifulSoup(driver.page_source, 'html.parser')
        for k in soup.findAll('a'):
                temp = k.get('href')
                if temp is not None:
                    if len(temp) == 3 and temp not in DateList:
                        DateList.append(temp)
                
    for j in AllDateList:
        for k in DateList:
            driver.get(url + j + k)
            soup = BeautifulSoup(driver.page_source, 'html.parser')
            for h in soup.findAll('a'):
                temp = h.get('href')
                if temp is not None:
                    if 'csv' in temp and (url + j + k +temp) not in FinalList:
                        FinalList.append(url + j + k + temp) 
        for index in FinalList:
            if 'csv' in index or 'tar' in index:
                driver.get(index)
                time.sleep(0.6)
            else :
                print('error : ' + index)
        print(url + "count = "+str(len(FinalList)))
        FinalList.clear()
        
#main program
#CrawlerETC('http://tisvcloud.freeway.gov.tw/demo_data/')
CrawlerETC('http://tisvcloud.freeway.gov.tw/history/TDCS/M08A/')  