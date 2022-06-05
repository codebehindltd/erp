using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagement.Presentation.Website
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    public class Handler : IHttpHandler
    {

    public void ProcessRequest (HttpContext context) {
        HttpPostedFile fileToUpload = context.Request.Files["Filedata"];
        string pathToSave = HttpContext.Current.Server.MapPath("~/Files/") + fileToUpload.FileName;
        fileToUpload.SaveAs(pathToSave);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    }
}