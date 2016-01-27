import requests
import urllib.request
import re
from bs4 import BeautifulSoup
import os
import time
from selenium import webdriver


class ReviewElement:
    def __init__(self):         
         self.ReivewId = '-1'
         self.UserId = '-1'

    def setInfo(self,_ReivewId,_UserId):         
         self.ReivewId = _ReivewId
         self.UserId = _UserId

def run_program(url):
    print(':::HotelReviewCrawler Start:::')
    driver.get(url)
    soup = BeautifulSoup(driver.page_source,"html.parser")
    HotelReviewList = list()

    LinkSelect = soup.find_all('a',attrs={'class' : 'pageNum'})

    TempLink = DomainName + LinkSelect[0].get('href')
    SplitTempLink = TempLink.split('or10')
    PageCount = re.findall('-or(.*?)0-',LinkSelect[len(LinkSelect) - 1].get('href'))

    print('Total Page=' + str(int(PageCount[0]) + 1))

    main_crawler(HotelReviewList)    
    print('Page 1 : Done And Count ' + str(len(HotelReviewList)))

    for i in range(1,int(PageCount[0]) + 1,1):  
         driver.get(SplitTempLink[0] + 'or' + str(i) + '0' + SplitTempLink[1])
         main_crawler(HotelReviewList)        
         print('Page ' + str(i + 1) + ' : Done And Count ' + str(len(HotelReviewList)))
    return HotelReviewList

def main_crawler(list):

    soup = BeautifulSoup(driver.page_source,"html.parser")
    for index in soup.find_all('div',attrs={'class':'reviewSelector'}):
        ReviewId = index.get('id').replace('review_','')
        UserId = '-1'
        Temp_Info = re.findall('"UID_(.*?)-SRC_(.*?)"',str(index).replace('\n',''))
        if len(Temp_Info) != 0:
            UserId = Temp_Info[0][0]
        Temp_ReviewElement = ReviewElement()
        Temp_ReviewElement.setInfo(ReviewId,UserId)
        list.append(Temp_ReviewElement)

DomainName = 'http://www.tripadvisor.com'

#Browser Setting
cwd = os.getcwd() + '/'
webdriver.DesiredCapabilities.PHANTOMJS['phantomjs.page.customHeaders.Accept-Language'] = 'en'
driver = webdriver.PhantomJS(cwd + 'phantomjs') 
#driver = webdriver.Chrome(cwd + 'chromedriver') # Start chromedriver