import HotelListCrawler #step 1
import HotelInfoCrawler #step 2
import HotelReviewCrawler #step 3
import ReviewContentCrawler #step 4
import MemberProfileCrawler #step 5
import threading
import time
import datetime
import pypyodbc
from bs4 import BeautifulSoup
import urllib.request
import re

class myThread(threading.Thread):
    def __init__(self, ThreadID, Name, FunctionName,Parameter):
        threading.Thread.__init__(self)
        self.ThreadID = ThreadID
        self.Name = Name
        self.FunctionName = FunctionName
        self.Parameter = Parameter
    def run(self):
        if self.FunctionName == 'HotelInfoCrawler':
            print('Starting : ' + self.name)
            Start_HotelInfoCrawler(self.Parameter)
            print('Exiting : ' + self.name)

        if self.FunctionName == 'HotelReviewCrawler':
            print('Starting : ' + self.name)
            Start_HotelReviewCrawler(self.Parameter)
            print('Exiting : ' + self.name)

        #if self.FunctionName =='ReviewContentCrawler':

        #if self.FunctionName =='MemberProfileCrawler':


#connection_string =''
#connection=pypyodbc.connect(connection_string)

#_______________________________________________________________________step 1____________________________________________________________________________________
#=================================================================================================================================================================
def Start_HotelListCrawler(url):
    print(':::HotelListCrawler Start:::')

    Temp = HotelListCrawler.run_program(CityUrl)
    HotelCrawlerList = Temp

    print(':::HotelListCrawler is finished:::')
    for index, i in enumerate(HotelCrawlerList):
        print(str(index + 1) + '\t' + i.Id + '\t' + i.Name + '\t' + i.Href)

    return HotelCrawlerList
#=================================================================================================================================================================

#_______________________________________________________________________step 2____________________________________________________________________________________
#=================================================================================================================================================================
#HotelInfo=HotelInfoCrawler.run_program('http://www.tripadvisor.com/Hotel_Review-g34439-d547627-Reviews-The_Browns_Hotel-Miami_Beach_Florida.html')
#HotelInfo=HotelInfoCrawler.run_program(url)
#print(HotelInfo.Id + '\t' + HotelInfo.hRank + '\t' + HotelInfo.hRating + '\t'
#+ HotelInfo.hStarClass + '\t' + HotelInfo.hRoomNum + '\t' +
#HotelInfo.hReviewNum + '\t' + HotelInfo.excellent + '\t' + HotelInfo.verygood
#+ '\t' + HotelInfo.average + '\t' + HotelInfo.poor + '\t' + HotelInfo.terrible
#+ '\t' + HotelInfo.hFamilies + '\t' + HotelInfo.hCouples + '\t' +
#HotelInfo.hSolo + '\t' + HotelInfo.hBusiness + '\t' + HotelInfo.hFrineds +
#'\t' + HotelInfo.hMarMay + '\t' + HotelInfo.hJunAug + '\t' + HotelInfo.hSepNov
#+ '\t' + HotelInfo.hDecFeb + '\t' + HotelInfo.hLocation + '\t' +
#HotelInfo.hSleepQuality + '\t' + HotelInfo.hRooms + '\t' + HotelInfo.hService
#+ '\t' + HotelInfo.hValue + '\t' + HotelInfo.hCleanliness)
def Start_HotelInfoCrawler(list):
    print(':::HotelInfoCrawler Start:::')
    HotelInfoList = HotelInfoCrawler.run_program(list)
    for index, i in enumerate(HotelInfoList):
        print(':::::Hotel ' + str(index + 1) + ' :::::')
        print(i.Id + '\t' + i.hRank + '\t' + i.hRating + '\t' + i.hStarClass + '\t' + i.hRoomNum + '\t' + i.hReviewNum + '\t' + i.excellent + '\t' + i.verygood + '\t' + i.average + '\t' + i.poor + '\t' + i.terrible + '\t' + i.hFamilies + '\t' + i.hCouples + '\t' + i.hSolo + '\t' + i.hBusiness + '\t' + i.hFrineds + '\t' + i.hMarMay + '\t' + i.hJunAug + '\t' + i.hSepNov + '\t' + i.hDecFeb)
   
        #print(HotelInfo.Id + '\t' + HotelInfo.hRank + '\t' + HotelInfo.hRating
        #+ '\t' + HotelInfo.hStarClass + '\t' + HotelInfo.hRoomNum + '\t' +
        #HotelInfo.hReviewNum + '\t' + HotelInfo.excellent + '\t' +
        #HotelInfo.verygood + '\t' + HotelInfo.average + '\t' + HotelInfo.poor
        #+ '\t' + HotelInfo.terrible + '\t' + HotelInfo.hFamilies + '\t' +
        #HotelInfo.hCouples + '\t' + HotelInfo.hSolo + '\t' +
        #HotelInfo.hBusiness + '\t' + HotelInfo.hFrineds + '\t' +
        #HotelInfo.hMarMay + '\t' + HotelInfo.hJunAug + '\t' +
        #HotelInfo.hSepNov + '\t' + HotelInfo.hDecFeb)
        #print(HotelInfo.Id + '\t' + HotelInfo.hRank + '\t' + HotelInfo.hRating
        #+ '\t' + HotelInfo.hStarClass + '\t' + HotelInfo.hRoomNum + '\t' +
        #HotelInfo.hReviewNum + '\t' + HotelInfo.excellent + '\t' +
        #HotelInfo.verygood + '\t' + HotelInfo.average + '\t' + HotelInfo.poor
        #+ '\t' + HotelInfo.terrible + '\t' + HotelInfo.hFamilies + '\t' +
        #HotelInfo.hCouples + '\t' + HotelInfo.hSolo + '\t' +
        #HotelInfo.hBusiness + '\t' + HotelInfo.hFrineds + '\t' +
        #HotelInfo.hMarMay + '\t' + HotelInfo.hJunAug + '\t' +
        #HotelInfo.hSepNov + '\t' + HotelInfo.hDecFeb + '\t' +
        #HotelInfo.hLocation + '\t' + HotelInfo.hSleepQuality + '\t' +
        #HotelInfo.hRooms + '\t' + HotelInfo.hService + '\t' + HotelInfo.hValue
        #+ '\t' + HotelInfo.hCleanliness)

    return HotelInfoList
#=================================================================================================================================================================

#_______________________________________________________________________step
#3____________________________________________________________________________________
#=================================================================================================================================================================

#Read Review Link =
#http://www.tripadvisor.com/ShowUserReviews-g34439-d120360-r2148901-Hawaii_Hotel-Miami_Beach_Florida.html#CHECK_RATES_CONT
#print('===HotelReviewCrawler Start====')
#url='http://www.tripadvisor.com/Hotel_Review-g34439-d120360-Reviews-Hawaii_Hotel-Miami_Beach_Florida.html'
#HotelReivewList=HotelReviewCrawler.run_program(url,SystemTime)
#for index,j in enumerate(HotelReivewList):
#    print(str(index + 1) +' '+j.ReviewId+' '+j.UserId+' '+j.AtPage+'
#    '+j.TimeStamp)
def Start_HotelReviewCrawler(list):
    print(':::HotelReviewCrawler Start:::')
    print('System Time = ' + SystemTime)
    for index, i in enumerate(list): 
        print('The hotel is = ' + i.Name + " and the url is " + i.Href)
        TempList = HotelReviewCrawler.run_program(i.Href,SystemTime) 
        if TempList is not None:
            print('Hotel = ' + i.Name) 
            print('Count = ' + str(len(HotelReivewList)))
            for index,j in enumerate(TempList): 
                print(str(index + 1) + '\t' + j.ReviewId + '\t' + j.UserId + '\t' + j.AtPage + '\t' + j.TimeStamp)
            HotelReivewList.append(TempList)
        else:
            break

    return HotelReivewList 
#=================================================================================================================================================================


#As step 3 is finished, step 4 and step 5 can build Thread and crawler.
#_______________________________________________________________________step
#4____________________________________________________________________________________
#=================================================================================================================================================================
#AreaId='34439'
#HotelId='85177'
#UserId='DF1FCD5EE11F625E71E1B6CA95CD8567'
#ReviewId='328090656'

#ReviewContent=ReviewContentCrawler.run_program('http://www.tripadvisor.com/ExpandedUserReviews-g'+AreaId+'-d'+HotelId+'?target='+ReviewId+'&context=1&reviews='+ReviewId,UserId)
#print(ReviewContent.ReviewTitle + '\t' + ReviewContent.ReviewRating + '\t' +
#ReviewContent.RatingDate + '\t' + ReviewContent.ReviewContent + '\t' +
#ReviewContent.ReviewHelpfulvotes)

#AreaId='34439'
#HotelId='85177'
#UserId='66A5F6F16014594C3CB6926570171960'
#ReviewId='331120139'

#ReviewContent=ReviewContentCrawler.run_program('http://www.tripadvisor.com/ExpandedUserReviews-g'+AreaId+'-d'+HotelId+'?target='+ReviewId+'&context=1&reviews='+ReviewId,UserId)
#print(ReviewContent.ReviewTitle + '\t' + ReviewContent.ReviewRating + '\t' +
#ReviewContent.RatingDate + '\t' + ReviewContent.ReviewContent + '\t' +
#ReviewContent.ReviewHelpfulvotes)

#print('Count = '+str(len(HotelReivewList)))
#for index,i in enumerate(HotelReivewList):
#    print(str(index + 1) +' '+i.UserId+' '+i.ReviewId)
#=================================================================================================================================================================

#_______________________________________________________________________step
#5____________________________________________________________________________________
#=================================================================================================================================================================
#UserId='7EC2CD53AB6787D4C94E08F185E05483'
#ReviewId='327918536'

#GetReturn=MemberProfileCrawler.run_program('http://www.tripadvisor.com/MemberOverlay?uid='+UserId+'&c=&src='+ReviewId+'&fus=false&partner=false&LsoId=')
#MemberInfo=GetReturn[0]
#MemberReviewElement=GetReturn[1]
#print(MemberInfo.Name,MemberInfo.TotelHelpfulVotes,MemberInfo.Since,MemberInfo.Location,MemberInfo.Age,MemberInfo.Gender,MemberInfo.Badge)
#for index, i in enumerate(MemberReviewElement):
#    print(str(index + 1) + ' : ' + i.Date + ' ' + str(i.Rating) + ' ' +
#    str(i.Helpful)+ ' ' + i.Hyperlink)
#=================================================================================================================================================================

#*****************Main Program****************
SystemTime = str(datetime.datetime.now())

#Sql connection
IPAddress = '140.123.174.167'
UserName = 'kakaci123'
PassWord = 'zaza5201314'
DataBase = 'TripAdvisor_ChenWei'

connection = pypyodbc.connect('Driver={SQL Server};'
                                'Server=' + IPAddress + ';'
                                'Database=' + DataBase + ';'
                                'uid=' + UserName + ';pwd=' + PassWord + '')
print('===Database is connected===')
cursor = connection.cursor()

#Step 1 (Get Hotel List From Current Area)
NY = 'http://www.tripadvisor.com/Hotels-g60763-New_York_City_New_York-Hotels'
LV = 'http://www.tripadvisor.com/Hotels-g45963-Las_Vegas_Nevada-Hotels'
OF = 'http://www.tripadvisor.com/Hotels-g34515-Orlando_Florida-Hotels'
CI = 'http://www.tripadvisor.com/Hotels-g35805-Chicago_Illinois-Hotels'
MF = 'http://www.tripadvisor.com/Hotels-g34438-Miami_Florida-Hotels'
MFB = 'http://www.tripadvisor.com/Hotels-g34439-Miami_Beach_Florida-Hotels.html'

CityArray = [('New York',NY),('Las Vagas',LV),('Orlando Florida',OF),('Chicago Illinois',CI),('Miami Florida',MF),('Miami Beach Florida',MFB)]

for CityElement in CityArray:
    cursor.execute("SELECT * FROM AreaList WHERE AreaName='" + CityElement[0] + "'")
    row = cursor.fetchone()
    #row=(1, 'New York', '60763')

    AreaId = str(row[0])
    CityUrl = CityElement[1]

    print("Now is ["+row[1]+"] running")

    req = urllib.request.urlopen(CityUrl)
    content = req.read().decode(req.info().get_content_charset())
    soup = BeautifulSoup(content,"html.parser")
    HotelCnt = int(re.findall('#.* (.*?) hotels',soup.find('div',attrs={'class' : 'slim_ranking'}).text)[0])
    cursor.execute("SELECT * FROM HotelList WHERE AreaId='" + AreaId + "'")
    row = cursor.fetchall()

    if HotelCnt > len(row):
        HotelList = Start_HotelListCrawler(CityUrl)

        #Insert to Database
        for index in HotelList:
            cursor.execute("SELECT * FROM HotelList WHERE HotelId='" + index.Id + "'")
            CheckQuery=cursor.fetchone()
            if CheckQuery is None:
                _HotelId = index.Id.replace(',','@[CMA]').replace("'","''")
                _HotelName = index.Name.replace(',','@[CMA]').replace("'","''")
                _HotelHref = index.Href.replace(',','@[CMA]').replace("'","''")
                cursor.execute("INSERT INTO HotelList(AreaId, HotelId,HotelName,HotelHref,inService) values ('" + AreaId + "', '" + _HotelId + "','" + _HotelName + "','" + _HotelHref + "','1')")
                connection.commit()

        HotelList.clear()

    else:
        if HotelCnt<len(row):
            print('Hotel was may out of service')
        HotelList = HotelListCrawler.BuildElement(row)
    
    thread1 = myThread(1, 'Thread-HotelInfoCrawler', 'HotelInfoCrawler',HotelList)
    thread1.start()


## Create new threads
#thread2 = myThread(2,
#'Thread-HotelReviewCrawler','HotelReviewCrawler',HotelList)

## Start new Threads
#thread2.start()
#ReturnHotelInfoCrawler=thread1.join()
#print(len(ReturnHotelInfoCrawler))
connection.close()
print('===Connection is closed===')
