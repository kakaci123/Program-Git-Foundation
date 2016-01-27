from bs4 import BeautifulSoup
import os
import time
from selenium import webdriver

#initial browser
adr = os.getcwd() + '/'
driver = webdriver.Chrome(adr + 'chromedriver')

def CrawlerETC(url):
    driver.get(url)
    soup = BeautifulSoup(driver.page_source, 'html.parser')
    
    FolderList = list()
    AllDateList = list()
    DateList = list()
    FinalList = list()
    
    for i in soup.findAll('a'):
        temp = i.get('href')
        if temp is not None:
            if len(temp) == 4 or len(temp) == 5 and temp not in FolderList:
                FolderList.append(temp) 
    
    for i in FolderList:
        driver.get(url + i)
        soup = BeautifulSoup(driver.page_source, 'html.parser')
        for j in soup.findAll('a'):
            temp = j.get('href')    
            if temp is not None:
                if len(temp) == 9 and temp not in AllDateList:
                    AllDateList.append(temp)
                if 'tar' in temp and (url+i+temp) not in FinalList:
                    FinalList.append(url+i+temp)
        if len(FinalList) !=0 :
            for index in FinalList:
                if 'csv' in index or 'tar' in index:
                    driver.get(index)
                    time.sleep(0.5)
                else :
                    print('error : ' + index)
            print("count = "+str(len(FinalList)))
            FinalList.clear()
          
    for i in FolderList:
        for j in AllDateList:
           driver.get(url + i + j)
           soup = BeautifulSoup(driver.page_source, 'html.parser')
           for k in soup.findAll('a'):
                 temp = k.get('href')
                 if temp is not None:
                    if len(temp) == 3 and temp not in DateList:
                        DateList.append(temp)
                
    for i in FolderList:
        for j in AllDateList:
            for k in DateList:
                driver.get(url + i + j + k)
                soup = BeautifulSoup(driver.page_source, 'html.parser')
                for h in soup.findAll('a'):
                    temp = h.get('href')
                    if temp is not None:
                        if 'csv' in temp and temp not in FinalList:
                            FinalList.append(url + i + j + k + temp) 
            for index in FinalList:
                if 'csv' in index or 'tar' in index:
                    driver.get(index)
                    time.sleep(0.5)
                else :
                    print('error : ' + index)
            print(url + "count = "+str(len(FinalList)))
            FinalList.clear()
        
#main program
CrawlerETC('http://tisvcloud.freeway.gov.tw/demo_data/')
CrawlerETC('http://tisvcloud.freeway.gov.tw/history/TDCS/')  