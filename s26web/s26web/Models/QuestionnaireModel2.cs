using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using s26web.Models;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using System.Data.Linq.SqlClient;

namespace s26web.Models
{
    public class QuestionnaireModel2
    {

        public string Keyword = "";
        public string InvKeyword = "";
        public string choice = "noassign";
        public DateTime? order_time_start = null;
        public DateTime? order_time_end = null;
        public DateTime? time_start = null;
        public DateTime? time_end = null;

        //=======Declare, but not used=======
        //public int order_states = 0;
        //public int order_from = 3;
        //public int City = 0;
        //public int Area = 0;
        //public int Osid = 0;
        //public string Sort = "OrdersTime";
        //===================================

        /// <summary>
        /// Using [MainMenu] class to join [QuestionnaireMain] and [Volunteers]
        /// </summary>
        public class MainMenu
        {
            public QuestionnaireMain qm = new QuestionnaireMain();
            public Volunteers vl = new Volunteers();
        }

        public class QuestionnaireModelShow
        {
            [Key]
            public int Id { get; set; }
            [DisplayName("會員編號")]
            public int VolunteersId { get; set; }
            [DisplayName("活動問卷編號")]
            public int CategoryId { get; set; }
            [DisplayName("問卷送出時間")]
            public DateTime SubmitTime { get; set; }
            [DisplayName("活動問卷編號")]
            public string Q1 { get; set; }
            public string Q2 { get; set; }
            public string Q2_Note { get; set; }
            public string Q3 { get; set; }
            public string Q3_Note { get; set; }
            public string Q4 { get; set; }
            public string Q5 { get; set; }
            public string Q6 { get; set; }
            public string Q7 { get; set; }
            public string Q7_Note { get; set; }
            public string Q8 { get; set; }
            public string Q8_Note { get; set; }
        }

        #region Insert
        public int Insert(QuestionnaireModelShow item)
        {
            s26webDataContext db = new s26webDataContext();
            try
            {
                QuestionnaireMain new_item = new QuestionnaireMain
                {
                    UserId = item.VolunteersId,
                    CategoryId = item.CategoryId,
                    SubmitTime = DateTime.UtcNow
                };
                db.QuestionnaireMain.InsertOnSubmit(new_item);
                db.SubmitChanges();

                for (int i = 1; i < 9; i++)
                {
                    string Answer = "";
                    switch (i)
                    {
                        case 1:
                            Answer = item.Q1;
                            break;
                        case 2:
                            Answer = item.Q2;
                            break;
                        case 3:
                            Answer = item.Q3;
                            break;
                        case 4:
                            Answer = item.Q4;
                            break;
                        case 5:
                            Answer = item.Q5;
                            break;
                        case 6:
                            Answer = item.Q6;
                            break;
                        case 7:
                            Answer = item.Q7;
                            break;
                        case 8:
                            Answer = item.Q8;
                            break;
                    }
                    QuestionnaireDetail new_item2 = new QuestionnaireDetail
                    {

                        MainId = db.QuestionnaireMain.OrderByDescending(o => o.Id).FirstOrDefault(f => f.Id != null).Id,
                        QuestionId = @i,
                        UserAnswer = Answer
                    };
                    db.QuestionnaireDetail.InsertOnSubmit(new_item2);
                }

                db.SubmitChanges();
                db.Connection.Close();
                return new_item.Id;
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region 完成問卷 - 新增100點
        public int AddPoint(QuestionnaireModelShow item)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();

                //取得會員資料
                var Vol = db.Volunteers.FirstOrDefault(f => f.Id == item.VolunteersId);

                if ( Vol != null)
                {
                    if (Vol.Point == null)
                    {
                        Vol.Point = 0;
                    }

                    Vol.Point += 100;

                    db.SubmitChanges();
                    db.Connection.Close();
                    return Vol.Id;
                }
                db.Connection.Close();
                return -1;
            }
            catch { return -1; }
        }
        #endregion
    }
}