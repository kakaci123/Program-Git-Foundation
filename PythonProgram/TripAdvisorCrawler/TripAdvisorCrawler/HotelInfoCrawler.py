﻿import requests
import urllib.request
import re
from bs4 import BeautifulSoup
import os
import time
from selenium import webdriver

class HotelInfoElement:
    #data structure
    def __init__(self):
        self.Id = "-1"
        self.hRank = '-1'
        self.hRating = '-1'
        self.hStarClass = '-1'
        self.hRoomNum = '-1'
        self.hReviewNum = '-1'
        self.excellent = '-1'
        self.verygood = '-1'
        self.average = '-1'
        self.poor = '-1'
        self.terrible = '-1'
        self.hFamilies = '-1'
        self.hCouples = '-1'
        self.hSolo = '-1'
        self.hBusiness = '-1'
        self.hFrineds = '-1'
        self.hMarMay='-1'
        self.hJunAug='-1'
        self.hSepNov='-1'
        self.hDecFeb='-1'
        self.hLocation = '-1'
        self.hSleepQuality = '-1'
        self.hRooms = '-1'
        self.hService = '-1'
        self.hValue = '-1'
        self.hCleanliness = '-1'


    def setInfo(self,_Id,_hRank,_hRating,_hStarClass,_hRoomNum,_hReviewNum,_excellent,_verygood,_average,_poor,_terrible,_hFamilies,_hCouples,_hSolo,_hBusiness,_hFrineds,_hMarMay,_hJunAug,_hSepNov,_hDecFeb,_hLocation,_hSleepQuality,_hRooms,
_hService,_hValue,_hCleanliness):
        self.Id = _Id
        self.hRank = _hRank
        self.hRating = _hRating
        self.hStarClass = _hStarClass
        self.hRoomNum = _hRoomNum
        self.hReviewNum = _hReviewNum

        self.excellent = _excellent
        self.verygood = _verygood
        self.average = _average
        self.poor = _poor
        self.terrible = _terrible

        self.hFamilies = _hFamilies
        self.hCouples = _hCouples
        self.hSolo = _hSolo
        self.hBusiness = _hBusiness
        self.hFrineds = _hFrineds

        self.hMarMay=_hMarMay
        self.hJunAug=_hJunAug
        self.hSepNov=_hSepNov
        self.hDecFeb=_hDecFeb

        self.hLocation = _hLocation
        self.hSleepQuality = _hSleepQuality
        self.hRooms = _hRooms
        self.hService = _hService
        self.hValue = _hValue
        self.hCleanliness = _hCleanliness

def run_program(url):
    print(':::HotelInfoCrawler Start:::')
    driver.get(url)
    HotelInfo = main_crawler()
    return HotelInfo
 
def main_crawler():
    soup = BeautifulSoup(driver.page_source,"html.parser")

    Temp_Info = HotelInfoElement()

    ID = re.findall('-d(.*?)-',driver.current_url)[0]

    Temp_Element = soup.find('b',attrs={ 'class' : 'rank'})
    if Temp_Element is not None:
       hRank = Temp_Element.text.replace('#','')
    else:
       hRank = '-1'

    Temp_Element = soup.find('img',attrs={ 'class' : 'sprite-rating_rr_fill'})
    if Temp_Element is not None:
       hRating = re.findall('content="(.*?)"',str(Temp_Element))[0]
    else:
       hRating = '-1'
    
    Temp_Element = soup.find('div',attrs={'class':'additional_info stars'})
    if Temp_Element is not None:
       hStarClass = re.findall('Hotel Class:(.*?) star',Temp_Element.text)[0]
    else:
       hStarClass = '-1'

    Temp_Element = soup.find('span',attrs={'class':'tabs_num_rooms'})
    if Temp_Element is not None:
        hRoomNum = Temp_Element.text.replace(' ','')
    else:
        hRoomNum = '-1'
       
    Temp_Element = soup.find('a',attrs={'class':'more taLnk'})
    if Temp_Element is not None:
        hReviewNum = Temp_Element.text.replace(' Reviews','')
    else:
        hReviewNum = '-1'


    Temp_Element = soup.find('div',attrs={'id':'ratingFilter'})
    if Temp_Element is not None:
        Temp_Rating = Temp_Element.text.replace('\n','').replace('Traveler rating','').replace(' ','')
        excellent = re.findall('Excellent(.*?)Very',Temp_Rating)[0]
        verygood = re.findall('Verygood(.*?)Average',Temp_Rating)[0]
        average = re.findall('Average(.*?)Poor',Temp_Rating)[0]
        poor = re.findall('Poor(.*?)Terrible',Temp_Rating)[0]
        terrible = re.findall('Terrible(.*?)$',Temp_Rating)[0]
    else:
        excellent='0'
        verygood='0'
        average='0'
        poor='0'
        terrible='0'

    Temp_Element = soup.find('div',attrs={'class':'col segment '})
    if Temp_Element is not None: 
        Temp_Rating = Temp_Element.text.replace('\n','').replace('Traveler type','').replace('(','').replace(')','').replace(' ','')
        hFamilies = re.findall('Families(.*?)Couples',Temp_Rating)[0]
        hCouples = re.findall('Couples(.*?)Solo',Temp_Rating)[0]
        hSolo = re.findall('Solo(.*?)Business',Temp_Rating)[0]
        hBusiness = re.findall('Business(.*?)Friends',Temp_Rating)[0]
        hFrineds = re.findall('Friends(.*?)$',Temp_Rating)[0]
    else:
        hFamilies='0'
        hCouples='0'
        hSolo='0'
        hBusiness='0'
        hFrineds='0'
    
    Temp_Element = soup.find('div',attrs={'class':'col season '})
    if Temp_Element is not None: 
        Temp_Rating = Temp_Element.text.replace('\n','').replace('Time of year','').replace('(','').replace(')','').replace(' ','')
        hMarMay=re.findall('Mar-May(.*?)Jun-Aug',Temp_Rating)[0]
        hJunAug=re.findall('Jun-Aug(.*?)Sep-Nov',Temp_Rating)[0]
        hSepNov=re.findall('Sep-Nov(.*?)Dec-Feb',Temp_Rating)[0]
        hDecFeb=re.findall('Dec-Feb(.*?)$',Temp_Rating)[0]
    else:
        hMarMay='0'
        hJunAug='0'
        hSepNov='0'
        hDecFeb='0'

    #turn into CH page and get the information
    driver.get(driver.current_url.replace('http://www.tripadvisor.com/','http://www.tripadvisor.com.tw/'))
    soup = BeautifulSoup(driver.page_source,"html.parser")

    Temp_Element=soup.find('div',attrs={'id':'SUMMARYBOX'})
    if Temp_Element is not None:
        Temp_Rating = str(Temp_Element).replace('\n','').replace('分','')
        hLocation = re.findall('<img alt="(.*?)"',Temp_Rating)[0]
        hSleepQuality = re.findall('<img alt="(.*?)"',Temp_Rating)[1]
        hRooms = re.findall('<img alt="(.*?)"',Temp_Rating)[2]
        hService = re.findall('<img alt="(.*?)"',Temp_Rating)[3]
        hValue = re.findall('<img alt="(.*?)"',Temp_Rating)[4]
        hCleanliness = re.findall('<img alt="(.*?)"',Temp_Rating)[5]
    else:
        hLocation='0'
        hSleepQuality='0'
        hRooms='0'
        hService='0'
        hValue='0'
        hCleanliness='0'
  
    Temp_Info.setInfo(ID,hRank,hRating,hStarClass,hRoomNum,hReviewNum,excellent,verygood,average,poor,terrible,hFamilies,hCouples,hSolo,hBusiness,hFrineds,hMarMay,hJunAug,hSepNov,hDecFeb,hLocation,hSleepQuality,hRooms,hService,hValue,hCleanliness)

    return Temp_Info

cwd = os.getcwd() + '/'
webdriver.DesiredCapabilities.PHANTOMJS['phantomjs.page.customHeaders.Accept-Language'] = 'en'
driver = webdriver.PhantomJS(cwd + 'phantomjs') 