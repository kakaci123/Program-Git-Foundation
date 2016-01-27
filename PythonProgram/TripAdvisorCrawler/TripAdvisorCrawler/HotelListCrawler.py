import requests
import urllib.request
import re
from bs4 import BeautifulSoup
import os
import time
from selenium import webdriver

class HotelListElement:
    def __init__(self):
         self.Id = "-1"
         self.Name = "-1"
         self.Href = "-1"

    def setInfo(self,_Id,_Name,_Href):
         self.Id = _Id
         self.Name = _Name
         self.Href = _Href
         
def run_program(url):
    driver.get(url)
    city_info = re.findall('Hotels-g(.*?)-(.*?)-Hotels',driver.current_url)[0]
    print('Crawler City = ' + city_info[0] + '_' + city_info[1])

    soup = BeautifulSoup(driver.page_source,"html.parser")
    HotelList = list()

    #Avoid Duplicate
    ScoreList = list()

    main_crawler(HotelList,ScoreList,soup)
   
    TempList=soup.find_all('a',attrs={'class' : 'pageNum'})
    if TempList is not None:                           
        for link in TempList:
            driver.get(DomainName + link.get('href'))
            soup = BeautifulSoup(driver.page_source,"html.parser")
            main_crawler(HotelList,ScoreList,soup)

    return HotelList, city_info[0]

def main_crawler(list,ScoreList,soup):
    SoupTemp=soup.find_all('a',attrs={'class' : 'property_title'})
    for link in SoupTemp:
        Temp_Id = link.get('id').replace('property_','')
        if Temp_Id not in ScoreList:
            ScoreList.append(Temp_Id)
            Temp_Hotel = HotelListElement()
            Temp_Hotel.setInfo(Temp_Id,link.text,DomainName + link.get('href'))
            list.append(Temp_Hotel)

DomainName = 'http://www.tripadvisor.com'
 
#Browser Setting
cwd = os.getcwd() + '/'
webdriver.DesiredCapabilities.PHANTOMJS['phantomjs.page.customHeaders.Accept-Language'] = 'en'
driver = webdriver.PhantomJS(cwd + 'phantomjs') 
#driver = webdriver.Chrome(cwd + 'chromedriver') # Start chromedriver