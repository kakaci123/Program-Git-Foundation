import HotelListCrawler #step 1
import HotelInfoCrawler #step 2
import HotelReviewCrawler #step 3
import ReviewContentCrawler #step 4
import MemberProfileCrawler #step 5
import threading
import time
import datetime

import pypyodbc


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

#_______________________________________________________________________step
#1____________________________________________________________________________________
#=================================================================================================================================================================
def Start_HotelListCrawler(url):
    print('===HotelListCrawler Start====')
    Temp = HotelListCrawler.run_program(CityUrl)
    HotelCrawlerList = Temp[0]
    AreaId = Temp[1]

    for index, i in enumerate(HotelCrawlerList):
        print(str(index + 1) + '\t' + i.Id + '\t' + i.Name + '\t' + i.Href)
    return HotelCrawlerList
#=================================================================================================================================================================

#_______________________________________________________________________step
#2____________________________________________________________________________________
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
   
        #print(HotelInfo.Id + '\t' + HotelInfo.hRank + '\t' + HotelInfo.hRating + '\t' + HotelInfo.hStarClass + '\t' + HotelInfo.hRoomNum + '\t' + HotelInfo.hReviewNum + '\t' + HotelInfo.excellent + '\t' + HotelInfo.verygood + '\t' + HotelInfo.average + '\t' + HotelInfo.poor + '\t' + HotelInfo.terrible + '\t' + HotelInfo.hFamilies + '\t' + HotelInfo.hCouples + '\t' + HotelInfo.hSolo + '\t' + HotelInfo.hBusiness + '\t' + HotelInfo.hFrineds + '\t' + HotelInfo.hMarMay + '\t' + HotelInfo.hJunAug + '\t' + HotelInfo.hSepNov + '\t' + HotelInfo.hDecFeb)
        #print(HotelInfo.Id + '\t' + HotelInfo.hRank + '\t' + HotelInfo.hRating + '\t' + HotelInfo.hStarClass + '\t' + HotelInfo.hRoomNum + '\t' + HotelInfo.hReviewNum + '\t' + HotelInfo.excellent + '\t' + HotelInfo.verygood + '\t' + HotelInfo.average + '\t' + HotelInfo.poor + '\t' + HotelInfo.terrible + '\t' + HotelInfo.hFamilies + '\t' + HotelInfo.hCouples + '\t' + HotelInfo.hSolo + '\t' + HotelInfo.hBusiness + '\t' + HotelInfo.hFrineds + '\t' + HotelInfo.hMarMay + '\t' + HotelInfo.hJunAug + '\t' + HotelInfo.hSepNov + '\t' + HotelInfo.hDecFeb + '\t' + HotelInfo.hLocation + '\t' + HotelInfo.hSleepQuality + '\t' + HotelInfo.hRooms + '\t' + HotelInfo.hService + '\t' + HotelInfo.hValue + '\t' + HotelInfo.hCleanliness)

    return HotelInfoList
#=================================================================================================================================================================

#_______________________________________________________________________step 3____________________________________________________________________________________
#=================================================================================================================================================================

#print('===HotelReviewCrawler Start====')
url='http://www.tripadvisor.com/Hotel_Review-g34439-d120360-Reviews-Hawaii_Hotel-Miami_Beach_Florida.html'


SystemTime=str(datetime.datetime.now())
HotelReivewList=HotelReviewCrawler.run_program(url,SystemTime)
for index,j in enumerate(HotelReivewList):
    print(str(index + 1) +' '+j.ReivewId+' '+j.UserId+' '+j.AtPage+' '+j.TimeStamp)

def Start_HotelReviewCrawler(list):
    print(':::HotelReviewCrawler Start:::')
    print('System Time = '+SystemTime)
    for index, i in enumerate(list): 
        print('The hotel is = '+i.Name+" and the url is "+i.Href)
        TempList=HotelReviewCrawler.run_program(i.Href,SystemTime) 
        if TempList is not None:
            print('Hotel = ' + i.Name) 
            print('Count = ' + str(len(HotelReivewList)))
            for index,j in enumerate(TempList): 
                print(str(index + 1) +'\t'+j.ReivewId+'\t'+j.UserId+'\t'+j.AtPage+'\t'+j.TimeStamp)
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
#    print(str(index + 1) +' '+i.UserId+' '+i.ReivewId)
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


#SystemTime=str(datetime.datetime.now())
#Step 1
#cityUrl =
#'http://www.tripadvisor.com/Hotels-g60763-New_York_City_New_York-Hotels'
#cityUrl = 'http://www.tripadvisor.com/Hotels-g45963-Las_Vegas_Nevada-Hotels'
#cityUrl = 'http://www.tripadvisor.com/Hotels-g34515-Orlando_Florida-Hotels'
#cityUrl = 'http://www.tripadvisor.com/Hotels-g35805-Chicago_Illinois-Hotels'
#cityUrl = 'http://www.tripadvisor.com/Hotels-g34438-Miami_Florida-Hotels'
#cityUrl =
#'http://www.tripadvisor.com/Hotels-g34439-Miami_Beach_Florida-Hotels.html'

##miami_beach_florida
#CityUrl = 'http://www.tripadvisor.com/Hotels-g34439-Miami_Beach_Florida-Hotels.html'
#HotelList = Start_HotelListCrawler(CityUrl)

## Create new threads
#thread1 = myThread(1, 'Thread-HotelInfoCrawler', 'HotelInfoCrawler',HotelList)
#thread2 = myThread(2, 'Thread-HotelReviewCrawler','HotelReviewCrawler',HotelList)

## Start new Threads
#thread1.start()
#thread2.start()
#ReturnHotelInfoCrawler=thread1.join()
#print(len(ReturnHotelInfoCrawler))