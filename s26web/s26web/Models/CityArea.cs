using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using s26web.Models;
using System.Web.Mvc;

namespace s26web.Models
{
    public class CityArea
    {
        
        public static List<SelectListItem> Get_City_Select(int city, bool all)
        {
            s26webDataContext db = new s26webDataContext();
            List<SelectListItem> data = new List<SelectListItem>();
            data.Add(new SelectListItem
            {
                Selected = city == 0,
                Text = all ? "全部" : "請選擇",
                Value = "0"
            });
            data.AddRange(db.City.OrderBy(o => o.Id).Select(s =>
                new SelectListItem
                {
                    Selected = city == s.Id,
                    Text = s.Name,
                    Value = s.Id.ToString()
                }));
            db.Connection.Close();
            return data;
        }

        public static List<SelectListItem> Get_Area_Select(int city, int area, bool zip, bool all)
        {
            s26webDataContext db = new s26webDataContext();
            List<SelectListItem> data = new List<SelectListItem>();
            data.Add(new SelectListItem
            {
                Selected = area == 0,
                Text = all ? "全部" : "請選擇",
                Value = "0"
            });
            if (city == 0)
            {
                return data;
            }
            
                data.AddRange(db.Area.Where(w => w.City_Id == city).OrderBy(o => o.Id).ThenBy(t => t.ZipCode).Select(s =>
                    new SelectListItem
                    {
                        Selected = area == s.Id,
                        Text = (zip ? s.ZipCode : "") + s.Name,
                        Value = s.Id.ToString()
                    }));
            

            db.Connection.Close();
            return data;
        }

        public static string Get_Area_Ajax(int city, int area, bool zip, bool all)
        {
            var data = Get_Area_Select(city, area, zip, all);
            string result = "";
            foreach (var i in data)
            {
                result += "<option value=\"" + i.Value + "\" " + (i.Selected ? "selected=\"selected\"" : "") + ">" + i.Text + "</option>";
            }
            return result;
        }
    }
}