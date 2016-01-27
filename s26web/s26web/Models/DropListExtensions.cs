using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{
    public static class DropListExtensions
    {
        public static string DropList(this HtmlHelper helper, string name, List<SelectListItem> data, object htmlAttributes = null)
        {
            if (data == null && helper.ViewData != null)
                data = helper.ViewData.Eval(name) as List<SelectListItem>;
            if (data == null) return string.Empty;

            var select = new TagBuilder("select");

            if (htmlAttributes != null)
                select.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            select.GenerateId(name);
            select.MergeAttribute("name" , name);

            var optHtml = new StringBuilder();

            foreach (var item in data)
            {
                var option = new TagBuilder("option");
                option.Attributes.Add("value", helper.Encode(item.Value));
                if (item.Selected)
                    option.Attributes.Add("selected", "selected");
                option.InnerHtml = helper.Encode(item.Text);
                optHtml.AppendLine(option.ToString(TagRenderMode.Normal));
            }
            select.InnerHtml = optHtml.ToString();
            return select.ToString(TagRenderMode.Normal);
        }
    }
}