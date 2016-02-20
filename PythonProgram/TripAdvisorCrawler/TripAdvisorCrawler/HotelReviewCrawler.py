import urllib.request
import re
import os
import urllib
from bs4 import BeautifulSoup
from selenium import webdriver


class ReviewElement:
    def __init__(self):         
         self.HotelId='-1'
         self.ReviewId = '-1'
         self.UserId = '-1'
         self.AtPage = '-1'
         self.TimeStamp = '-1'

    def setInfo(self,_HotelId,_ReviewId,_UserId,_AtPage,_TimeStamp):         
         self.HotelId=_HotelId
         self.ReviewId = _ReviewId
         self.UserId = _UserId
         self.AtPage = _AtPage
         self.TimeStamp = _TimeStamp

def run_program(url,hotelid,SystemTime):
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
    Element = main_crawler("1",soup,hotelid,SystemTime)

    if Element is not False:
        ReviewCnt = len(HotelReviewList)
        print('Page 1 is Done And Count= ' + str(ReviewCnt))

        if PageCount > 1:
            for i in range(1,PageCount,1):  
                 req = urllib.request.urlopen(SplitTempLink[0] + 'or' + str(i) + '0' + SplitTempLink[1])
                 content = req.read().decode(req.info().get_content_charset())
                 soup = BeautifulSoup(content,"html.parser")

                 Element = main_crawler(str(i + 1),soup,hotelid,SystemTime)
                 print('Page ' + str(i + 1) + ' is Done And Count= ' + str(len(HotelReviewList) - ReviewCnt))
                 ReviewCnt = len(HotelReviewList)

    driver.close()
    return HotelReviewList

def main_crawler(ThisPage,soup,hotelid,SystemTime):
    SoupTemp = soup.find_all('div',attrs={'class':'reviewSelector'})
   
    if SoupTemp is not None:
        for index in SoupTemp:
            Temp_ReviewElement = ReviewElement()
            ReviewId = index.get('id').replace('review_','')
            UserId = '-1'
            Temp_Info = re.findall('"UID_(.*?)-SRC_(.*?)"',str(index).replace('\n',''))
            if len(Temp_Info) != 0:
                UserId = Temp_Info[0][0]

            if UserId == '-1':
                CheckUrl=SplitTempLink[0].replace('Reviews','r').replace('Hotel_Review','ShowUserReviews').replace('r-','r') + ReviewId +  SplitTempLink[1].replace('REVIEWS','CHECK_RATES_CONT')
                driver.get(CheckUrl)
                soup = BeautifulSoup(driver.page_source,"html.parser") 
                CheckPool = soup.find_all('div',attrs={'class':'memberOverlayLink'})
                for index2 in CheckPool:
                    _UserId= index2.get('id')
                    if ReviewId in _UserId:
                        SetUserId=_UserId.split('-')[0].replace('UID_','')
                        UserId=SetUserId
                        break

            Temp_ReviewElement.setInfo(hotelid,ReviewId,UserId,ThisPage,SystemTime)
            HotelReviewList.append(Temp_ReviewElement)

        return True
    else:
        return False

DomainName = 'http://www.tripadvisor.com'
HotelReviewList = list()
content = ""
SplitTempLink = []

cwd = os.getcwd() + '/'
webdriver.DesiredCapabilities.PHANTOMJS['phantomjs.page.customHeaders.Accept-Language'] = 'en'
driver = webdriver.PhantomJS(cwd + 'phantomjs') 