// Note: this deliberately uses the MVC namespace 

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace System.Web.Mvc
{
    public static class MvcExtensionMethods
    {
        /// <summary>
        /// Returns a string in the form of "FolderNameViewName"
        /// </summary>
        public static string GetRazorViewIdentifier(this IView view)
        {
            if (view is RazorView)
            {
                var viewUrl = ((RazorView)view).ViewPath;
                var viewSubPath = viewUrl.Replace("~/Views/", "").Replace("/", "");
                var viewSubPathWithoutExtension = Path.GetFileNameWithoutExtension(viewSubPath);
                return (viewSubPathWithoutExtension.ToLower());
            }
            else
            {
                // Handle Glympse in Dev http://getglimpse.com/
                if (view.ToString().EndsWith("GlimpseView"))
                {
                    dynamic dynamicView = view;
                    return GetRazorViewIdentifier(dynamicView.View);
                }
                throw (new InvalidOperationException("This view is not a RazorView"));
            }
        }
    }
}