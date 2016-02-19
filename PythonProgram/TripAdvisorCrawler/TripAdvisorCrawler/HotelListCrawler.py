import urllib.request
import re
from bs4 import BeautifulSoup
import time
import threading

class myThread(threading.Thread):
    def __init__(self, ThreadID, Name,list):
        threading.Thread.__init__(self)
        self.ThreadID = ThreadID
        self.Name = Name
        self.list = list
    def run(self):
        print('Starting : ' + self.name)
        for link in self.list:
            req = urllib.request.urlopen(DomainName + link)
            content = req.read().decode(req.info().get_content_charset())  
            soup = BeautifulSoup(content,"html.parser")
            main_crawler(HotelList,ScoreList,soup)
        print('Exiting : ' + self.name)


class HotelListElement:
    def __init__(self):
         self.Id = "-1"
         self.Name = "-1"
         self.Href = "-1"

    def setInfo(self,_Id,_Name,_Href):
         self.Id = _Id
         self.Name = _Name
         self.Href = _Href

def BuildElement(SourceList):
    ResultList=[]
    for index in SourceList:
        NewElement = HotelListElement()
        NewElement.setInfo(index[2],index[3],index[4])
        ResultList.append(NewElement)
    return ResultList   

def run_program(url):
    t1 = time.time()
    req = urllib.request.urlopen(url)
    content = req.read().decode(req.info().get_content_charset())
    city_info = re.findall('Hotels-g(.*?)-(.*?)-Hotels',content)[0]
    print('Crawler City = ' + city_info[0] + '_' + city_info[1])
    soup = BeautifulSoup(content,"html.parser")

    main_crawler(HotelList,ScoreList,soup)
   
    TempList = soup.find_all('a',attrs={'class' : 'pageNum'})
    if TempList is not None:

        #Build the List of Href
        HrefList=[] 
        for i in range(0 , len(TempList)-1,1):
            HrefList.append(TempList[i].get('href'))
        HrefListLast=HrefList[len(HrefList)-1]
        TempListLast=TempList[len(TempList)-1].get('href')
        test=re.findall('-oa(.*?)-',HrefListLast)[0]
        IndexPage=int(test)
        SplitString=HrefListLast.split('-oa'+str(IndexPage)+'-')
        while True:
            IndexPage+=30
            UrlTemp=SplitString[0]+'-oa'+str(IndexPage)+'-'+SplitString[1]
            if UrlTemp==TempListLast:
               break
            else:
               HrefList.append(UrlTemp)        
        HrefList.append(TempListLast)
        #Build is finished

        threads = []
        SubList = [HrefList[x:x + 2] for x in range(0,len(HrefList),2)]
        for i in range(0,len(SubList),1):
            TempThread = myThread(i, 'Thread-' + str(i),SubList[i])
            TempThread.start()
            threads.append(TempThread)
        for t in threads:
            t.join()
    t2 = time.time()
    print(t2 - t1)
    return HotelList, city_info[0]

def main_crawler(list,ScoreList,soup):
    SoupTemp = soup.find_all('a',attrs={'class' : 'property_title'})
    for link in SoupTemp:
        Temp_Id = link.get('id').replace('property_','')
        if Temp_Id not in ScoreList:
            ScoreList.append(Temp_Id)
            Temp_Hotel = HotelListElement()
            Temp_Hotel.setInfo(Temp_Id,link.text,DomainName + link.get('href'))
            list.append(Temp_Hotel)

DomainName = 'http://www.tripadvisor.com'

HotelList = list()
#Avoid Duplicate
ScoreList = list()