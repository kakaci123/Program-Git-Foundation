import requests
import urllib.request
import re
from bs4 import BeautifulSoup
import os
import time
import urllib
import datetime

from selenium import webdriver


class ReviewElement:
    def __init__(self):         
         self.ReivewId = '-1'
         self.UserId = '-1'
         self.AtPage = '-1'
         self.TimeStamp = '-1'

    def setInfo(self,_ReivewId,_UserId,_AtPage,_TimeStamp):         
         self.ReivewId = _ReivewId
         self.UserId = _UserId
         self.AtPage = _AtPage
         self.TimeStamp = _TimeStamp

def run_program(url,SystemTime):
    req = urllib.request.urlopen(url)
    global content
    content = req.read().decode(req.info().get_content_charset())
    soup = BeautifulSoup(content,"html.parser")
    LinkSelect = soup.find_all('a',attrs={'class' : 'pageNum'})
    PageCount = 1

    if LinkSelect is not None:

        TempLink = DomainName + LinkSelect[0].get('href')
        global SplitTempLink
        SplitTempLink = TempLink.split('or10')
        PageCount = int(re.findall('-or(.*?)0-',LinkSelect[len(LinkSelect) - 1].get('href'))[0]) + 1
         
    print('Total Page=' + str(PageCount))
    Element = main_crawler("1",soup,SystemTime)

    if Element is not False:
        ReviewCnt = len(HotelReviewList)
        print('Page 1 : Done And Count= ' + str(ReviewCnt))

        if PageCount > 1:
            for i in range(1,PageCount,1):  
                 if Element is False:
                    print('occur error!')
                    break
                 req = urllib.request.urlopen(SplitTempLink[0] + 'or' + str(i) + '0' + SplitTempLink[1])
                 content = req.read().decode(req.info().get_content_charset())
                 soup = BeautifulSoup(content,"html.parser")

                 Element = main_crawler(str(i + 1),soup,SystemTime)
                 print('Page ' + str(i + 1) + ' : Done And Count= ' + str(len(HotelReviewList) - ReviewCnt))
                 ReviewCnt = len(HotelReviewList)

    for index in HotelReviewList:
        if index.UserId == '-1':
            CheckUrl=SplitTempLink[0].replace('Reviews','r').replace('Hotel_Review','ShowUserReviews').replace('r-','r') + index.ReivewId +  SplitTempLink[1].replace('REVIEWS','CHECK_RATES_CONT')
            req = urllib.request.urlopen(CheckUrl)
            content = req.read().decode(req.info().get_content_charset())
            soup = BeautifulSoup(content,"html.parser")
            UserTemp = str(soup.find_all('div',attrs={'class':'member_info'})[0]).replace('\n','')
            index.UserId=re.findall('"UID_(.*?)-SRC_(.*?)"',UserTemp)[0][0]
            #正規式下這
            print(index.ReivewId+"\t"+index.UserId+'\t'+index.AtPage+'\t'+index.TimeStamp)
   
    return HotelReviewList

def main_crawler(ThisPage,soup,SystemTime):
    SoupTemp = soup.find_all('div',attrs={'class':'reviewSelector'})
   
    if SoupTemp is not None:
        for index in SoupTemp:
            Temp_ReviewElement = ReviewElement()
            ReviewId = index.get('id').replace('review_','')
            UserId = '-1'
            Temp_Info = re.findall('"UID_(.*?)-SRC_(.*?)"',str(index).replace('\n',''))
            if len(Temp_Info) != 0:
                UserId = Temp_Info[0][0]
            Temp_ReviewElement.setInfo(ReviewId,UserId,ThisPage,SystemTime)
            HotelReviewList.append(Temp_ReviewElement)
        return True
    else:
        return False

DomainName = 'http://www.tripadvisor.com'
HotelReviewList = list()
content = ""
SplitTempLink = []