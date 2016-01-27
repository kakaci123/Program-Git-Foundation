import HotelListCrawler #step 1
import HotelInfoCrawler #step 2
import HotelReviewCrawler #step 3
import ReviewContentCrawler #step 4
import MemberProfileCrawler #step 5

import threading
import time

import pypyodbc


class myThread (threading.Thread):
    def __init__(self, ThreadID, Name, FunctionName,Parameter):
        threading.Thread.__init__(self)
        self.ThreadID = ThreadID
        self.Name = Name
        self.FunctionName = FunctionName
        self.Parameter=Parameter
    def run(self):
        if self.FunctionName =='HotelInfoCrawler':
            print('Starting : ' + self.name)
            Start_HotelInfoCrawler(self.Parameter)
            print('Exiting : ' + self.name)

        if self.FunctionName =='HotelReviewCrawler':
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
    print('===HotelListCrawler Start====')
    Temp=HotelListCrawler.run_program(CityUrl)
    HotelCrawlerList=Temp[0]
    AreaId=Temp[1]

    for index, i in enumerate(HotelCrawlerList):
        print(str(index + 1) + '\t' + i.Id + '\t' + i.Name + '\t' + i.Href)
    return HotelCrawlerList
#=================================================================================================================================================================

#_______________________________________________________________________step 2____________________________________________________________________________________
#=================================================================================================================================================================
#HotelInfo=HotelInfoCrawler.run_program('http://www.tripadvisor.com/Hotel_Review-g34439-d547627-Reviews-The_Browns_Hotel-Miami_Beach_Florida.html')
#HotelInfo=HotelInfoCrawler.run_program(url)
#print(HotelInfo.Id + '\t' + HotelInfo.hRank + '\t' + HotelInfo.hRating + '\t' + HotelInfo.hStarClass + '\t' + HotelInfo.hRoomNum + '\t' + HotelInfo.hReviewNum + '\t' + HotelInfo.excellent + '\t' + HotelInfo.verygood + '\t' + HotelInfo.average + '\t' + HotelInfo.poor + '\t' + HotelInfo.terrible + '\t' + HotelInfo.hFamilies + '\t' + HotelInfo.hCouples + '\t' + HotelInfo.hSolo + '\t' + HotelInfo.hBusiness + '\t' + HotelInfo.hFrineds + '\t' + HotelInfo.hMarMay + '\t' + HotelInfo.hJunAug + '\t' + HotelInfo.hSepNov + '\t' + HotelInfo.hDecFeb + '\t' + HotelInfo.hLocation + '\t' + HotelInfo.hSleepQuality + '\t' + HotelInfo.hRooms + '\t' + HotelInfo.hService + '\t' + HotelInfo.hValue + '\t' + HotelInfo.hCleanliness)

def Start_HotelInfoCrawler(list):
    for index, i in enumerate(list):
        HotelInfo=HotelInfoCrawler.run_program(i.Href)
        print(':::::Hotel '+str(index + 1)+' :::::')
        print(HotelInfo.Id + '\t' + HotelInfo.hRank + '\t' + HotelInfo.hRating + '\t' + HotelInfo.hStarClass + '\t' + HotelInfo.hRoomNum + '\t' + HotelInfo.hReviewNum + '\t' + HotelInfo.excellent + '\t' + HotelInfo.verygood + '\t' + HotelInfo.average + '\t' + HotelInfo.poor + '\t' + HotelInfo.terrible + '\t' + HotelInfo.hFamilies + '\t' + HotelInfo.hCouples + '\t' + HotelInfo.hSolo + '\t' + HotelInfo.hBusiness + '\t' + HotelInfo.hFrineds + '\t' + HotelInfo.hMarMay + '\t' + HotelInfo.hJunAug + '\t' + HotelInfo.hSepNov + '\t' + HotelInfo.hDecFeb + '\t' + HotelInfo.hLocation + '\t' + HotelInfo.hSleepQuality + '\t' + HotelInfo.hRooms + '\t' + HotelInfo.hService + '\t' + HotelInfo.hValue + '\t' + HotelInfo.hCleanliness)
    return HotelInfo
#=================================================================================================================================================================

#_______________________________________________________________________step 3____________________________________________________________________________________
#=================================================================================================================================================================

#print('===HotelReviewCrawler Start====')
#HotelReivewList=HotelReviewCrawler.run_program(url)
#for index,j in enumerate(HotelReivewList):
#    print(str(index + 1) +' '+j.ReivewId+' '+j.UserId)

def Start_HotelReviewCrawler(list):
    for index, i in enumerate(list):
        HotelReivewList=HotelReviewCrawler.run_program(i.Href)
        print('Hotel = '+i.Name)
        print('Count = '+str(len(HotelReivewList)))
        for index,j in enumerate(HotelReivewList):
            print(str(index + 1) +' '+j.ReivewId+' '+j.UserId)

    return HotelReivewList
#=================================================================================================================================================================


#As step 3 is finished, step 4 and step 5 can build Thread and crawler.
#_______________________________________________________________________step 4____________________________________________________________________________________
#=================================================================================================================================================================
#AreaId='34439'
#HotelId='85177'
#UserId='DF1FCD5EE11F625E71E1B6CA95CD8567'
#ReviewId='328090656'

#ReviewContent=ReviewContentCrawler.run_program('http://www.tripadvisor.com/ExpandedUserReviews-g'+AreaId+'-d'+HotelId+'?target='+ReviewId+'&context=1&reviews='+ReviewId,UserId)
#print(ReviewContent.ReviewTitle + '\t' + ReviewContent.ReviewRating + '\t' + ReviewContent.RatingDate + '\t' + ReviewContent.ReviewContent + '\t' + ReviewContent.ReviewHelpfulvotes)

#AreaId='34439'
#HotelId='85177'
#UserId='66A5F6F16014594C3CB6926570171960'
#ReviewId='331120139'

#ReviewContent=ReviewContentCrawler.run_program('http://www.tripadvisor.com/ExpandedUserReviews-g'+AreaId+'-d'+HotelId+'?target='+ReviewId+'&context=1&reviews='+ReviewId,UserId)
#print(ReviewContent.ReviewTitle + '\t' + ReviewContent.ReviewRating + '\t' + ReviewContent.RatingDate + '\t' + ReviewContent.ReviewContent + '\t' + ReviewContent.ReviewHelpfulvotes)

#print('Count = '+str(len(HotelReivewList)))
#for index,i in enumerate(HotelReivewList):
#    print(str(index + 1) +' '+i.UserId+' '+i.ReivewId)
#=================================================================================================================================================================

#_______________________________________________________________________step 5____________________________________________________________________________________
#=================================================================================================================================================================
UserId='7EC2CD53AB6787D4C94E08F185E05483'
ReviewId='327918536'

GetReturn=MemberProfileCrawler.run_program('http://www.tripadvisor.com/MemberOverlay?uid='+UserId+'&c=&src='+ReviewId+'&fus=false&partner=false&LsoId=')
MemberInfo=GetReturn[0]
MemberReviewElement=GetReturn[1]
print(MemberInfo.Name,MemberInfo.TotelHelpfulVotes,MemberInfo.Since,MemberInfo.Location,MemberInfo.Age,MemberInfo.Gender,MemberInfo.Badge)
for index, i in enumerate(MemberReviewElement):
    print(str(index + 1) + ' : ' + i.Date + ' ' + str(i.Rating) + ' ' + str(i.Helpful)+ ' ' + i.Hyperlink)
#=================================================================================================================================================================


#Step 1
#cityUrl = 'http://www.tripadvisor.com/Hotels-g60763-New_York_City_New_York-Hotels'
#cityUrl = 'http://www.tripadvisor.com/Hotels-g45963-Las_Vegas_Nevada-Hotels'
#cityUrl = 'http://www.tripadvisor.com/Hotels-g34515-Orlando_Florida-Hotels'
#cityUrl = 'http://www.tripadvisor.com/Hotels-g35805-Chicago_Illinois-Hotels'
#cityUrl = 'http://www.tripadvisor.com/Hotels-g34438-Miami_Florida-Hotels'
#cityUrl = 'http://www.tripadvisor.com/Hotels-g34439-Miami_Beach_Florida-Hotels.html'





##miami_beach_florida
#CityUrl='http://www.tripadvisor.com/Hotels-g34439-Miami_Beach_Florida-Hotels.html'
#HotelList=Start_HotelListCrawler(CityUrl)

## Create new threads
#thread1 = myThread(1, 'Thread-HotelInfoCrawler', 'HotelInfoCrawler',HotelList)
##thread2 = myThread(2, 'Thread-HotelReviewCrawler', 'HotelReviewCrawler',HotelList)

## Start new Threads
#thread1.start()
#ReturnHotelInfoCrawler=thread1.join()
#print(len(ReturnHotelInfoCrawler))
##thread2.start()
