import HotelListCrawler #step 1
import HotelInfoCrawler #step 2
import HotelReviewCrawler #step 3
import ReviewContentCrawler #step 4
import MemberProfileCrawler #step 5
import datetime
import pypyodbc
from bs4 import BeautifulSoup
import urllib.request
import re


#Step 1
#=================================================================================================================================================================
def Start_HotelListCrawler(url):

    #######[output]########
    #     1.Id # Stored Database[HotelList]
    #     2.Name #
    #     3.Href #
    #     4.TimeStamp #
    #  [System Variable] #
    #     1.AreaId # from Database[AreaList]
    #     2.inService #
    #######################

    print(':::HotelListCrawler Start:::')
    HotelCrawlerList = HotelListCrawler.run_program(CityUrl)
    print(':::HotelListCrawler is finished:::')

    #for index, i in enumerate(HotelCrawlerList):
    #    print(str(index + 1) + '\t' + i.Id + '\t' + i.Name + '\t' + i.Href)

    return HotelCrawlerList
#=================================================================================================================================================================

#Step 2
#=================================================================================================================================================================
def Start_HotelInfoCrawler(list):

    #######[output]########
    #   [HotelInfoMain] # Stored Database[HotelMain]
    #     1.Id #
    #     2.hRank #
    #     3.hRating #
    #     4.hStarClass #
    #     5.hRoomNum #
    #     6.hReviewNum #
    #  [HotelInfoDetail] # Stored Database[HotelDetail]
    #     1.Id #
    #     2.excellent #
    #     3.verygood #
    #     4.average #
    #     5.poor #
    #     6.terrible #
    #     7.hFamilies #
    #     8.hCouples #
    #     9.hSolo #
    #    10.hBusiness #
    #    11.hFrineds #
    #    12.hMarMay #
    #    13.hJunAug #
    #    14.hSepNov #
    #    15.hDecFeb #
    #######################

    print(':::HotelInfoCrawler Start:::')
    HotelInfoList = HotelInfoCrawler.run_program(list)
    print(':::Start Writing to Database:::')

    #Insert to Database
    for index in HotelInfoList:
    
        _HotelId = index.Id.replace(',','')
        _hRank = index.hRank.replace(',','')
        _hRating = index.hRating.replace(',','')
        _hStarClass = index.hStarClass.replace(',','')
        _hRoomNum = index.hRoomNum.replace(',','')
        _hReviewNum = index.hReviewNum.replace(',','')

        cursor.execute("SELECT * FROM HotelMain WHERE HotelId='" + index.Id + "'")
        CheckQuery = cursor.fetchall()

        if len(CheckQuery) != 0 :
            CheckTemp = CheckQuery[len(CheckQuery) - 1]
            if str(CheckTemp[1]) != _HotelId and str(CheckTemp[2]) != _hRank and str(CheckTemp[3]) != _hRating and str(CheckTemp[4]) != _hStarClass and str(CheckTemp[5]) != _hRoomNum and str(CheckTemp[6]) != _hReviewNum:
                break

        cursor.execute("INSERT INTO HotelMain(HotelId, hRank,hRating,hStarClass,hRoomNum,hReviewNum,TimeStamp) values ('" + _HotelId + "', '" + _hRank + "','" + _hRating + "','" + _hStarClass + "','" + _hRoomNum + "','" + _hReviewNum + "','" + SystemTime + "')")
        connection.commit()

        _excellent = index.excellent.replace(',','')
        _verygood = index.verygood.replace(',','')
        _average = index.average.replace(',','')
        _poor = index.poor.replace(',','')
        _terrible = index.terrible.replace(',','')
        _hFamilies = index.hFamilies.replace(',','')
        _hCouples = index.hCouples.replace(',','')
        _hSolo = index.hSolo.replace(',','')
        _hBusiness = index.hBusiness.replace(',','')
        _hFrineds = index.hFrineds.replace(',','')
        _hMarMay = index.hMarMay.replace(',','')
        _hJunAug = index.hJunAug.replace(',','')
        _hSepNov = index.hSepNov.replace(',','')
        _hDecFeb = index.hDecFeb.replace(',','')

        cursor.execute("SELECT * FROM HotelDetail WHERE HotelId='" + index.Id + "'")
        CheckQuery = cursor.fetchall()
    
        if len(CheckQuery) != 0:
            CheckTemp = CheckQuery[len(CheckQuery) - 1]
            if str(CheckTemp[1]) != _HotelId and str(CheckTemp[2]) != _excellent and str(CheckTemp[3]) != _verygood and str(CheckTemp[4]) != _average and str(CheckTemp[5]) != _poor and str(CheckTemp[6]) != _terrible and str(CheckTemp[7]) != _hFamilies and str(CheckTemp[8]) != _hCouples and str(CheckTemp[9]) != _hSolo and str(CheckTemp[10]) != _hBusiness and str(CheckTemp[11]) != _hFrineds and str(CheckTemp[12]) != _hMarMay and str(CheckTemp[13]) != _hJunAug and str(CheckTemp[14]) != _hSepNov and str(CheckTemp[15]) != _hDecFeb:
                break

        cursor.execute("INSERT INTO HotelDetail(HotelId,excellent,verygood,average,poor,terrible,hFamilies,hCouples,hSolo,hBusiness,hFrineds,hMarMay,hJunAug,hSepNov,hDecFeb) values ('" + _HotelId + "', '" + _excellent + "','" + _verygood + "','" + _average  + "','" + _poor  + "','" + _terrible  + "','" + _hFamilies    + "','" + _hCouples   + "','" + _hSolo   + "','" + _hBusiness   + "','" + _hFrineds   + "','" + _hMarMay   + "','" + _hJunAug   + "','" + _hSepNov   + "','" + _hDecFeb  + "')")
        connection.commit()

    print(':::HotelInfoCrawler is finished:::')

    #for index, i in enumerate(HotelInfoList):
    #    print(':::::Hotel ' + str(index + 1) + ' :::::')
    #    print(i.Id + '\t' + i.hRank + '\t' + i.hRating + '\t' + i.hStarClass +
    #    '\t' + i.hRoomNum + '\t' + i.hReviewNum + '\t' + i.excellent + '\t' +
    #    i.verygood + '\t' + i.average + '\t' + i.poor + '\t' + i.terrible +
    #    '\t' + i.hFamilies + '\t' + i.hCouples + '\t' + i.hSolo + '\t' +
    #    i.hBusiness + '\t' + i.hFrineds + '\t' + i.hMarMay + '\t' + i.hJunAug
    #    + '\t' + i.hSepNov + '\t' + i.hDecFeb)
   
    return HotelInfoList

#=================================================================================================================================================================

#Step 3
#=================================================================================================================================================================
def Start_HotelReviewCrawler(list,SystemTime):

    #######[output]########
    #     1.HotelId # Stored Database[ReviewOverview]
    #     2.ReviewId #
    #     3.UserId #
    #     4.AtPage #
    #     5.TimeStamp #
    #######################

    print(':::HotelReviewCrawler Start:::')
    HotelReivewList =[]
    for index, i in enumerate(list): 
        print('The hotel = ' + i.Name)
        print('The url = ' + i.Href)
        TempList = HotelReviewCrawler.run_program(i.Href,i.Id,SystemTime) 
        HotelReivewList.append(TempList)
       
        print('Inserting to Database')

        #Insert to Database
        for index in HotelReviewList:
            cursor.execute("SELECT HotelId FROM HotelList WHERE HotelHref='" + index.Href + "'")
            CheckQuery = cursor.fetchone()

            _HotelId = CheckQuery[0]
            _ReviewId = index.ReviewId
            _UserId = index.UserId
            _AtPage = index.AtPage
            _TimeStamp = index.TimeStamp

            cursor.execute("INSERT INTO ReviewOverview(HotelId, ReviewId,UserId,AtPage,TimeStamp) values ('" + _HotelId + "', '" + _ReviewId + "','" + _UserId + "','" + _AtPage + "','" + _TimeStamp + "')")
            connection.commit()

        print('Insert Process is finished')

    print(':::HotelInfoCrawler is finished:::')

    return HotelReivewList 
#=================================================================================================================================================================

#Step 4(Start_ReviewContentCrawler())
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

#step 5
#=================================================================================================================================================================
def Start_MemberProfileCrawler(QueryArray):

#UserId='7EC2CD53AB6787D4C94E08F185E05483'
#ReviewId='327918536'

    for index in QueryArray:
        UserId=index[0]
        ReviewId=index[1]

#[MemberInfo]Name,TotelHelpfulVotes,Since,Location,Age,Gender,Badge
#(List)[MemberReviewElement]Rating,Helpful,Date,Hyperlink


    GetReturn=MemberProfileCrawler.run_program('http://www.tripadvisor.com/MemberOverlay?uid='+UserId+'&c=&src='+ReviewId+'&fus=false&partner=false&LsoId=')
    MemberInfo=GetReturn[0]
    MemberReviewElement=GetReturn[1]
    print(MemberInfo.Name,MemberInfo.TotelHelpfulVotes,MemberInfo.Since,MemberInfo.Location,MemberInfo.Age,MemberInfo.Gender,MemberInfo.Badge)
    for index, i in enumerate(MemberReviewElement):
        print(str(index + 1) + ' : ' + i.Date + ' ' + str(i.Rating) + ' ' +  str(i.Helpful)+ ' ' + i.Hyperlink)
#=================================================================================================================================================================

#*****************Main Program****************
SystemTime = str(datetime.datetime.now()).split(' ')[0]

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

#CityArray = [('New York',NY),('Las Vagas',LV),('Orlando Florida',OF),('Chicago Illinois',CI),('Miami Florida',MF),('Miami Beach Florida',MFB)]
CityArray=[('Miami Florida',MF)]

for CityElement in CityArray:
    cursor.execute("SELECT * FROM AreaList WHERE AreaName='" + CityElement[0] + "'")
    row = cursor.fetchone()
    #row=(1, 'New York', '60763')

    AreaId = str(row[0])
    CityUrl = CityElement[1]

    print("Now is [" + row[1] + "] running")

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
            CheckQuery = cursor.fetchone()
            if CheckQuery is None:
                _HotelId = index.Id.replace(',','@[CMA]').replace("'","''")
                _HotelName = index.Name.replace(',','@[CMA]').replace("'","''")
                _HotelHref = index.Href.replace(',','@[CMA]').replace("'","''")
                cursor.execute("INSERT INTO HotelList(AreaId, HotelId,HotelName,HotelHref,inService) values ('" + AreaId + "', '" + _HotelId + "','" + _HotelName + "','" + _HotelHref + "','1')")
                connection.commit()

        HotelList.clear()

    else:
        if HotelCnt < len(row):
            print('Hotel was may out of service, please check the database!')
        HotelList = HotelListCrawler.BuildElement(row)
    
    #Start_HotelInfoCrawler(HotelList)
    Start_HotelReviewCrawler(HotelList,SystemTime)

    #BARRIER()  
    #cursor.execute("SELECT * FROM HotelList WHERE HotelId='" + index.Id + "'")
    #CheckQuery = cursor.fetchall()

    #Start_ReviewContentCrawler()
    #Start_MemberProfileCrawler()

BARRIER()      
connection.close()
print('===Connection is closed===')   