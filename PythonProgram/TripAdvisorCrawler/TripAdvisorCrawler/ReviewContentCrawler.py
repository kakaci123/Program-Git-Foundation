import requests
import urllib.request
import re
from bs4 import BeautifulSoup
import os
import time
from selenium import webdriver

class ContentElement:
    def __init__(self):         
        self.ReviewTitle = '-1'
        self.ReviewRating = '-1'
        self.RatingDate = '-1'
        self.ReviewContent = '-1'
        self.ReviewHelpfulvotes = '-1'

    def setInfo(self,_ReviewTitle,_ReviewRating,_RatingDate,_ReviewContent,_ReviewHelpfulvotes):         
        self.ReviewTitle = _ReviewTitle
        self.ReviewRating = _ReviewRating
        self.RatingDate = _RatingDate
        self.ReviewContent = _ReviewContent
        self.ReviewHelpfulvotes = _ReviewHelpfulvotes

def run_program(url,UserId):
    print('===ReviewContentCrawler Start====')
    driver.get(url)
    ReviewContent = main_crawler(UserId)
    return ReviewContent

def main_crawler(UserId):
    soup = BeautifulSoup(driver.page_source,"html.parser")
    check = re.findall('UID_(.*?)-SRC_(.*?)$',soup.findAll('div',attrs={'class':'memberOverlayLink'})[0].get('id'))
    Check_UId = check[0][0]
    Check_RId = check[0][1]

    if UserId == Check_UId and driver.current_url.find(Check_RId) != -1:
        Temp_Info = ContentElement()
        
        Temp_Element = soup.find('span',attrs={'class':'noQuotes'})
        if Temp_Element is not None:
            title = Temp_Element.text
        else:
            title = '-1'

        Temp_Element = soup.findAll('img',attrs={'class':'sprite-rating_s_fill'})
        if Temp_Element is not None:
           rating = re.findall('(.*?) of 5 stars',Temp_Element[0].get('alt'))[0]
        else:            
           rating = '-1'

        Temp_Element = soup.findAll('span',attrs={'class':'ratingDate relativeDate'})
        if Temp_Element is not None:
            date = Temp_Element[0].get('title')
        else:
            date = '-1'
        Temp_Element = soup.find('div',attrs={'class':'entry'})
        if Temp_Element is not None:
            content = Temp_Element.text.replace('\n','').replace('"','')
        else:
            content = '-1'

        Temp_Element = soup.find('div',attrs={'class':'helpfulVotesBadge'})
        if Temp_Element is not None:
            helpfulvote = re.findall('<span class="badgeText">(.*?) helpful vote <\/span>',str(Temp_Element).replace('\n',''))[0]
        else:
            helpfulvote = '0'
        
        Temp_Info.setInfo(title,rating,date,content,helpfulvote)
        return Temp_Info
    else:
        print('Can not find the review')
