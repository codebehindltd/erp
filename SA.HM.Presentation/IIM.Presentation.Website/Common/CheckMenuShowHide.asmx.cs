using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;

namespace HotelManagement.Presentation.Website.Common
{
    /// <summary>
    /// Summary description for CheckMenuShowHide
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    [ScriptService]
    public class CheckMenuShowHide : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ReportMenuShowHide(string showOrHide)
        {
            HttpContext.Current.Session["ReportMenuShowHide"] = showOrHide;            
            return showOrHide;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string HomeMenuShowHide(string showOrHide)
        {
            HttpContext.Current.Session["HomeMenuShowHide"] = showOrHide;
            return showOrHide;
        }
    }
}
