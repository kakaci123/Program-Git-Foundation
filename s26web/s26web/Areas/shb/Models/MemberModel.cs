using s26web.Areas.shb.Models;
using s26web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Security;

namespace s26web.Areas.shb.Models
{
    public class MemberModel
    {
        public class MemberShow
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [DisplayName("帳號")]
            public string Account_Phone { get; set; }
            [DisplayName("登入密碼")]
            public string Password { get; set; }
            [DisplayName("使用人員名稱")]
            public string Name { get; set; }
            [DisplayName("群組權限")]
            public int Level { get; set; }
            [DisplayName("群組權限")]
            public string Level_Name { get; set; }
            [DisplayName("權限開放")]
            public bool Enable { get; set; }
            [DisplayName("資料建立時間")]
            public DateTime CreateTime { get; set; }
        }

        public class LoginModel
        {
            [Required(ErrorMessage = "必填欄位")]
            [Display(Name = "使用者名稱")]
            public string UserName { get; set; }
            [Required(ErrorMessage = "必填欄位")]
            [DataType(DataType.Password)]
            [Display(Name = "密碼")]
            public string Password { get; set; }
            [Display(Name = "記住我")]
            public bool RememberMe { get; set; }
        }

        public class UserLevelShow
        {
            [Key]
            [DisplayName("編號")]
            public int Id { get; set; }
            [DisplayName("群組類別")]
            public string Name { get; set; }
            [DisplayName("資料建立時間")]
            public DateTime CreateTime { get; set; }
            [DisplayName("權限項目")]
            public bool[] Competence { get; set; }
            [DisplayName("權限項目")]
            public string[] CompetenceName { get; set; }
        }

        private const string hash = "soohoobook";

        public string Keyword = "";
        public List<int> Level = new List<int>();
        public bool Search_Enable = false;
        public bool Enable = false;

        public MemberModel()
        {
        }

        /// <summary>
        /// Take user's input of passsword to get the hash number before comparing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Get_HashPassword(string input)
        {
            return Method.GetMD5(hash + input, true);
        }

        #region query

        /// <summary>
        /// Query [UserLevel],[CompetenceTable] of database to build the model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string get_Competence2(int id)
        {
            s26webDataContext db = new s26webDataContext();
            int level = db.UserProfile.FirstOrDefault(w => w.Id == id).Level;
            var data = db.CompetenceTable.Where(w => w.UserLevelId == level & w.Enable).ToList();
            var com_str = "";
            foreach (var i in data)
            {
                if (i.FunctionId < 10)
                    com_str += "0" + i.FunctionId + ",";
                else
                    com_str += i.FunctionId + ",";
            }
            db.Connection.Close();
            return com_str;
        }
        public UserLevelShow Get_UserLevel_One(int id)
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.UserLevel.FirstOrDefault(f => f.Id == id);
            db.Connection.Close();
            if (data != null)
            {
                return new UserLevelShow
                {
                    Id = data.Id,
                    Name = data.Name,
                    CreateTime = data.CreateTime.AddHours(8),//Format type [2005-11-5 14:23:23]
                    //Another Query
                    Competence = Get_Competence(data.Id),
                    CompetenceName = Get_CompetenceName()
                };
            }
            return null;
        }

        /// <summary>
        /// Using keyword to query [UserLevel],and count it,it's not using in the project
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public int Get_UserLevel_Count(string keyword = "")
        {
            s26webDataContext db = new s26webDataContext();
            IQueryable<UserLevel> data = db.UserLevel;
            db.Connection.Close();
            if (keyword != "")
            {
                data = from i in data
                       where SqlMethods.Like(i.Name, "%" + keyword + "%")
                       select i;
            }
            return data.Count();
        }

        /// <summary>
        /// Using keyword to query [UserLevel],and show on the page(num=10),it's not using in the project
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="p"></param>
        /// <param name="show_number"></param>
        /// <returns></returns>
        public IQueryable<UserLevel> Get_UserLevel(string keyword = "", int p = 1, int show_number = 10)
        {
            s26webDataContext db = new s26webDataContext();
            IQueryable<UserLevel> data = db.UserLevel;
            db.Connection.Close();
            if (keyword != "")
            {
                data = from i in data
                       where SqlMethods.Like(i.Name, "%" + keyword + "%")
                       select i;
            }
            return data.OrderByDescending(o => o.Id).Skip((p - 1) * show_number).Take(show_number);
        }

        /// <summary>
        /// Use id to query [UserProfile],[UserLevel] and build the MemberShow model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MemberShow Get_One(int id)
        {
            s26webDataContext db = new s26webDataContext();
            var data = (from i in db.UserProfile
                        join j in db.UserLevel on i.Level equals j.Id
                        where i.Id == id
                        select new MemberShow
                        {
                            Id = i.Id,
                            Account_Phone = i.Account_Phone,
                            Level = i.Level,
                            Level_Name = j.Name,
                            Name = i.Name,
                            Password = i.Password,
                            CreateTime = i.CreateTime,
                            Enable = i.Enable,
                        }).FirstOrDefault();
            db.Connection.Close();
            return data;
        }

        /// <summary>
        /// Use account to query [UserProfile],[UserLevel] and build the MemberShow model
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static MemberShow Get_One(string account)
        {
            s26webDataContext db = new s26webDataContext();
            var data = (from i in db.UserProfile
                        join j in db.UserLevel on i.Level equals j.Id
                        where i.Account_Phone == account &&
                        i.Enable == true
                        select new MemberShow
                        {
                            Id = i.Id,
                            Account_Phone = i.Account_Phone,
                            Level = i.Level,
                            Level_Name = j.Name,
                            Name = i.Name,
                            Password = i.Password,
                            CreateTime = i.CreateTime,
                            Enable = i.Enable
                        }).FirstOrDefault();
            db.Connection.Close();

            return data;
        }

        /// <summary>
        /// Use account and password to query [UserProfile],[UserLevel] and build the MemberShow model
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public MemberShow Get_One(string account, string password)
        {
            s26webDataContext db = new s26webDataContext();
            var data = (from i in db.UserProfile
                        join j in db.UserLevel on i.Level equals j.Id
                        where i.Account_Phone.Equals(account) &&
                            i.Password.Equals(Get_HashPassword(password)) &&
                        i.Enable == true
                        select new MemberShow
                        {
                            Id = i.Id,
                            Account_Phone = i.Account_Phone,
                            Level = i.Level,
                            Level_Name = j.Name,
                            Name = i.Name,
                            Password = i.Password,
                            CreateTime = i.CreateTime,
                            Enable = i.Enable
                        }).FirstOrDefault();
            db.Connection.Close();
            return data;
        }

        /// <summary>
        /// Initail the competence array to insert the new record
        /// </summary>
        /// <returns></returns>
        public bool[] Initial_Competence()
        {
            s26webDataContext db = new s26webDataContext();
            bool[] UnusingArray = new bool[db.Function.Count()];
            for (int i = 0; i < UnusingArray.Count(); i++) { UnusingArray[i] = false; }
            return UnusingArray;
        }

        /// <summary>
        /// Get competence of the user, the type is bool array
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool[] Get_Competence(int UserId)
        {
            s26webDataContext db = new s26webDataContext();
            bool[] FunctionEnable = new bool[db.Function.Count()];
            var query = db.CompetenceTable.Where(w => w.UserLevelId == UserId);
            foreach (var customer in query)
            {
                FunctionEnable[customer.FunctionId - 1] = customer.Enable;
            }
            return FunctionEnable;
        }

        /// <summary>
        /// Get competence of the user, the type is bool array
        /// </summary>
        /// <returns></returns>
        public string[] Get_CompetenceName()
        {
            s26webDataContext db = new s26webDataContext();
            string[] rlt = new string[db.Function.Count()];
            int temp = 0;
            var query = db.Function;
            foreach (var i in query)
            {
                rlt[temp++] = i.Name;
            }
            return rlt;
        }

        /// <summary>
        /// Query the data to show on the table
        /// </summary>
        /// <param name="p"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public List<MemberShow> Get_Data(int p = 1, int take = 10)
        {
            s26webDataContext db = new s26webDataContext();
            var data = Get().OrderByDescending(o => o.Id).Skip((p - 1) * take).Take(take);
            List<MemberShow> mm = new List<MemberShow>();
            foreach (var i in data)
            {
                var temp = db.UserLevel.FirstOrDefault(w => w.Id == i.Level);
                mm.Add(new MemberShow
                {
                    Id = i.Id,
                    Account_Phone = i.Account_Phone,
                    Level = i.Level,
                    Level_Name = temp == null ? "未設定" : temp.Name,
                    Name = i.Name,
                    Password = i.Password,
                    CreateTime = i.CreateTime.AddHours(8),
                    Enable = i.Enable
                });
            }
            db.Connection.Close();
            return mm;
        }

        public Method.Paging Get_Page(int p = 1, int take = 10)
        {
            return Method.Get_Page(Get_Count(), p, take);
        }

        public int Get_Count()
        {
            return Get().Count();
        }

        /// <summary>
        /// Query UserProfile,it's not using in the project
        /// </summary>
        /// <returns></returns>
        private IQueryable<UserProfile> Get()
        {
            s26webDataContext db = new s26webDataContext();
            IQueryable<UserProfile> data = db.UserProfile;
            if (Keyword != "")
            {
                data = data.Where(Query(Keyword));
            }
            if (Level.Any())
            {
                data = data.Where(w => Level.Contains(w.Level));
            }
            if (Search_Enable)
            {
                data = data.Where(w => w.Enable == Enable);
            }
            db.Connection.Close();
            return data.Where(w => w.Id != 0);
        }

        /// <summary>
        /// Sql query, but it's not using in the project
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private string Query(string query = "")
        {
            string sql = "";

            if (query != "")
            {
                query = HttpUtility.HtmlEncode(query);
                sql = Method.SQL_Combin(sql, "Account_Phone", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "Name", "OR", "( \"" + query + "\")", ".Contains", false);
                sql = Method.SQL_Combin(sql, "Email", "OR", "( \"" + query + "\")", ".Contains", false);
            }

            return sql;
        }

        #endregion

        #region insert

        /// <summary>
        /// Insert a new record to [UserProfile] database.If success return [Id],otherwise, return -1
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Insert(MemberShow item)
        {
            s26webDataContext db = new s26webDataContext();
            if (db.UserProfile.FirstOrDefault(f => f.Account_Phone.ToLower() == item.Account_Phone.ToLower()) == null)
            {
                UserProfile new_item = new UserProfile
                {
                    Account_Phone = item.Account_Phone,
                    Password = Get_HashPassword(item.Password),
                    Name = item.Name,
                    Level = item.Level,
                    Enable = item.Enable,
                    CreateTime = DateTime.UtcNow
                };
                db.UserProfile.InsertOnSubmit(new_item);
                db.SubmitChanges();
                db.Connection.Close();
                return new_item.Id;
            }
            else
            {
                db.Connection.Close();
                return -1;
            }
        }
        /// <summary>
        /// Insert a new record to [UserLevel] database.Finally, return the record [Id]
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Insert_UserLevel(UserLevelShow item)
        {
            s26webDataContext db = new s26webDataContext();
            var result = new UserLevel
            {
                Name = item.Name,
                CreateTime = DateTime.UtcNow
            };
            db.UserLevel.InsertOnSubmit(result);
            db.SubmitChanges();
            Insert_CompetenceTable(item, result.Id);
            return result.Id;
        }

        /// <summary>
        /// Insert an array of new record to [CompetenceTable] database.Finally
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Id"></param>
        public void Insert_CompetenceTable(UserLevelShow item, int Id)
        {
            s26webDataContext db = new s26webDataContext();
            List<CompetenceTable> ct = new List<CompetenceTable>();
            for (int i = 0; i < item.Competence.Count(); i++)
            {
                ct.Add(new CompetenceTable
                {
                    UserLevelId = Id,
                    FunctionId = (i + 1),
                    Enable = item.Competence[i]
                });
            } db.CompetenceTable.InsertAllOnSubmit(ct);
            db.SubmitChanges();
            db.Connection.Close();
        }
        #endregion

        #region update
        /// <summary>
        /// Entering function to update the record
        /// </summary>
        /// <param name="scs"></param>
        /// <returns></returns>
        public int Update(MemberShow scs)
        {
            List<MemberShow> sc = new List<MemberShow>();
            sc.Add(scs);
            return Update(sc);
        }

        /// <summary>
        /// Using sql-insert the record.If success,return [Id], otherwise, return [-1] 
        /// </summary>
        /// <param name="scs"></param>
        /// <returns></returns>
        private int Update(List<MemberShow> scs)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                int id = 0;
                foreach (var i in scs)
                {
                    var data = db.UserProfile.FirstOrDefault(f => f.Id == i.Id && f.Id != 0);
                    if (data != null)
                    {
                        data.Name = i.Name;
                        data.Level = i.Level;
                        if (i.Password != "" && i.Password != null)
                        {
                            data.Password = Get_HashPassword(i.Password);
                        }
                        data.Enable = i.Enable;
                        db.SubmitChanges();
                        id = data.Id;
                    }
                }
                db.Connection.Close();
                return id;
            }
            catch { return -1; }
        }

        /// <summary>
        /// The entering of update,it's not using in the project
        /// </summary>
        /// <param name="id"></param>
        /// <param name="en"></param>
        /// <returns></returns>
        public int Update_Enable(int id, bool en)
        {
            int[] id_arr = new int[1];
            id_arr[0] = id;
            return Update_Enable(id_arr, en);
        }

        /// <summary>
        /// Update [Enable] of UserProfile,it's not using in the project
        /// </summary>
        /// <param name="id"></param>
        /// <param name="en"></param>
        /// <returns></returns>
        private int Update_Enable(int[] id, bool en)
        {
            try
            {
                if (id != null)
                {
                    if (id.Any())
                    {
                        s26webDataContext db = new s26webDataContext();
                        var data = db.UserProfile.Where(w => id.Contains(w.Id) && w.Id != 0);
                        if (data.Any())
                        {
                            foreach (var i in data)
                            {
                                i.Enable = en;
                            }
                            db.SubmitChanges();
                        }
                        db.Connection.Close();
                        return id.LastOrDefault();
                    }
                }
                return -1;
            }
            catch { return -1; }
        }

        /// <summary>
        /// The entering of update,it's not using in the project
        /// </summary>
        /// <param name="id"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public int Update_Level(int id, int level)
        {
            int[] id_arr = new int[1];
            id_arr[0] = id;
            return Update_Level(id_arr, level);
        }

        /// <summary>
        /// Update [Level] of UserProfile,it's not using in the project
        /// </summary>
        /// <param name="id"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private int Update_Level(int[] id, int level)
        {
            try
            {
                if (id != null)
                {
                    if (id.Any())
                    {
                        s26webDataContext db = new s26webDataContext();
                        int result = -1;
                        var data = db.UserProfile.Where(w => id.Contains(w.Id) && w.Id != 0);
                        if (data.Any())
                        {
                            foreach (var i in data)
                            {
                                i.Level = level;
                                result = i.Id;
                            }
                            db.SubmitChanges();
                        }
                        db.Connection.Close();
                        return result;
                    }
                }
                return -1;
            }
            catch { return -1; }
        }

        /// <summary>
        /// Update [Name] of UserLevel and the update function of competence.If success finally, return [1], otherwise, return [-1] 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Update_UserLevel(UserLevelShow item)
        {
            s26webDataContext db = new s26webDataContext();
            var data = db.UserLevel.FirstOrDefault(f => f.Id == item.Id);
            if (data != null)
            {
                data.Name = item.Name;
                db.SubmitChanges();
                db.Connection.Close();
                //Another function
                if (Update_FunctionCompetence(item.Competence, item.Id))
                {
                    return 1;
                }
            }
            return -1;
        }

        /// <summary>
        /// Update [all-col] of CompetenceTable. If success finally, return [true], otherwise, return [false] 
        /// </summary>
        /// <param name="FunctionChange"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        private bool Update_FunctionCompetence(bool[] FunctionChange, int Id)
        {
            try
            {
                s26webDataContext db = new s26webDataContext();
                var query = db.CompetenceTable.Where(w => w.UserLevelId == Id);
                List<CompetenceTable> ct = new List<CompetenceTable>();
                for (int i = 0; i < FunctionChange.Count(); i++)
                {
                    var temp = db.CompetenceTable.FirstOrDefault(f => f.UserLevelId == Id && f.FunctionId == (i + 1));
                    if (temp == null)
                    {
                        ct.Add(new CompetenceTable
                        {
                            UserLevelId = Id,
                            FunctionId = (i + 1),
                            Enable = FunctionChange[i]
                        });
                    }
                    else
                    {
                        temp.Enable = FunctionChange[i];
                    }
                }
                db.CompetenceTable.InsertAllOnSubmit(ct);
                db.SubmitChanges();
                db.Connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region delete
        /// <summary>
        /// Entering function to delete the record
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id = 0)
        {
            if (id > 0)
            {
                Delete(new int[1] { id });
            }
        }

        /// <summary>
        /// Delete index[id] record of UserProfile
        /// </summary>
        /// <param name="id"></param>
        private void Delete(int[] id)
        {
            if (id.Any())
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.UserProfile.Where(w => id.Contains(w.Id) && w.Id != 0);
                if (data.Any())
                {
                    db.UserProfile.DeleteAllOnSubmit(data);
                    db.SubmitChanges();
                }
                db.Connection.Close();
            }
        }

        /// <summary>
        /// Entering function to delete the record
        /// </summary>
        /// <param name="id"></param>
        public void Delete_UserLevel(int id = 0)
        {
            if (id > 0)
            {
                Delete_UserLevel(new int[1] { id });
            }
        }

        /// <summary>
        /// Delete index[id] record of UserLevel
        /// </summary>
        /// <param name="id"></param>
        public void Delete_UserLevel(int[] id)
        {
            if (id.Any())
            {
                s26webDataContext db = new s26webDataContext();
                var data = db.UserLevel.Where(w => id.Contains(w.Id) && w.Id != 1);
                if (data.Any())
                {
                    db.UserLevel.DeleteAllOnSubmit(data);
                    db.SubmitChanges();
                }
                db.Connection.Close();
            }
        }
        #endregion
    }
}