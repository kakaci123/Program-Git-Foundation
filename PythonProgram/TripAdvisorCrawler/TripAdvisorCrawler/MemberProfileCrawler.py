import requests
import urllib.request
import re
from bs4 import BeautifulSoup
import os
import time
from selenium import webdriver

class MemeberInfoElement:
    def __init__(self):
         self.Name = '-1'
         self.TotelHelpfulVotes = '-1'
         self.Since = '-1'
         self.Location = '-1'
         self.Age = '-1'
         self.Gender = '-1'
         self.Badge = '-1'

    def setInfo(self,_Name,_TotelHelpfulVotes,_Since,_Location,_Age,_Gender,_Badge):
         self.Name = _Name
         self.TotelHelpfulVotes = _TotelHelpfulVotes
         self.Since = _Since
         self.Location = _Location
         self.Age = _Age
         self.Gender = _Gender
         self.Badge = _Badge

class MemberReviewElement:
    def __init__(self):
         self.Date = "-1"
         self.Rating = '-1'
         self.Helpful = '-1'
         self.Hyperlink= "-1"

    def setInfo(self,_Date,_Rating,_Helpful,_Hyperlink):
         self.Date = _Date
         self.Rating = _Rating
         self.Helpful = _Helpful
         self.Hyperlink= _Hyperlink
         
def run_program(url):
    print('===MemberProfileCrawler Start====')
    Temp = get_crawlerinfo(url)
    CheckCount = Temp[1]
    print('check count=' + CheckCount)
    driver.get(Temp[0])

    MemberInfo = MemeberInfoElement()
    soup = BeautifulSoup(driver.page_source,"html.parser")    
    All_Page = driver.page_source.replace('\n','')

    Temp_Name = Temp[2]

    Temp_Info = soup.find_all("a", attrs={'name':'lists'})
    if Temp_Info is not None:
        Temp_TotelHelpfulVotes = Temp_Info[0].text.replace(' Helpful votes','')
    else:
        Temp_TotelHelpfulVotes = '-1'

    Temp_Since = soup.find('p',attrs={'class':'since'}).text.replace('Since ','')

    Temp_Info = soup.find('div',attrs={'class','hometown'})
    if Temp_Info is not None:
        Temp_Location = Temp_Info.text
    else:
        Temp_Location = '-1'

    Temp_Info = re.findall(',"age":(.*?),',All_Page)
    if Temp_Info is not None:
       Temp_Age = Temp_Info[0]
    else:
       Temp_Age = '-1'

    Temp_Info = re.findall('"gender":"(.*?)"',All_Page)
    if Temp_Info is not None:
       Temp_Gender = Temp_Info[0]
    else:
       Temp_Gender = '-1'

    Temp_Info = soup.find('div',attrs={'class','badgeName'})
    if Temp_Info is not None:
        Temp_Badge = Temp_Info.text
    else:
        Temp_Badge = '-1'

    MemberInfo.setInfo(Temp_Name,Temp_TotelHelpfulVotes,Temp_Since,Temp_Location,Temp_Age,Temp_Gender,Temp_Badge)

    MemberProfileList = list()

    while True:
        main_crawler(MemberProfileList)
        if  len(MemberProfileList) == int(CheckCount):
            break
        else:
            MemberProfileList.clear()
            main_crawler(MemberProfileList)

    return MemberInfo,MemberProfileList
        
def main_crawler(list):
    pat_rating = 'alt="(.*?) of 5 stars'
    revise_rating = 100
    
    while True:
        soup = BeautifulSoup(driver.page_source,"html.parser")
        DateList = soup.select('.cs-review-date')
        if DateList is not None:
            RatingList = soup.select('.cs-review-rating')
            HelpfulList = soup.select('.cs-points')
            HyperlinkList= soup.select('.cs-review-title')
            
            for i in range(0,len(DateList),1):  
                Temp_Rating = re.findall(pat_rating,(str(RatingList[i])))[0]
                Temp_Helpful = int(HelpfulList[i].text) - revise_rating
                Temp_Hyperlink='http://www.tripadvisor.com/'+HyperlinkList[i].get('href')
                Temp_Profile = MemberReviewElement()
                Temp_Profile.setInfo(DateList[i].text,Temp_Rating,Temp_Helpful,Temp_Hyperlink)
                list.append(Temp_Profile)
            if isNext() == False:
                 nextpage()
            else:
                 break
        else:
             return

def nextpage():
    time.sleep(3)
    nextbtn = driver.find_element_by_id('cs-paginate-next')
    nextbtn.click()

def isNext():
    nextbtn = driver.find_element_by_id('cs-paginate-next')
    element_attribute_value = nextbtn.get_attribute('class')
    return element_attribute_value == 'disabled'

def get_crawlerinfo(url):
    driver.get(url)
    soup = BeautifulSoup(driver.page_source,"html.parser")
    return 'http://www.tripadvisor.com/members/' + soup.find('h3',attrs={'class' : 'username'}).text,soup.find('span',attrs={'class' : 'numbersText'}).text.replace('(','').replace(')',''),soup.find('h3',attrs={'class' : 'username'}).text

#Browser Setting
#cwd = os.getcwd() + '/'
#webdriver.DesiredCapabilities.PHANTOMJS['phantomjs.page.customHeaders.Accept-Language'] = 'en'
#driver = webdriver.PhantomJS(cwd + 'phantomjs') 
#driver = webdriver.Chrome(cwd + 'chromedriver') # Start chromedriver