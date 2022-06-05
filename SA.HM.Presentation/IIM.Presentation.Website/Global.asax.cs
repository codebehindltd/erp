using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace HotelManagement.Presentation.Website
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Application["CompanyAccountInfoForSalesBillPayment"] = System.Web.Configuration.WebConfigurationManager.AppSettings["CompanyAccountInfoForSalesBillPayment"];
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (Context.Request.HttpMethod.Equals("OPTIONS"))
            {
                HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "POST, GET,PUT, OPTIONS, DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }
        protected void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs        
            Exception Ex = Server.GetLastError();
            Application["Ex"] = Ex;
            //Common.HMUtility.ReportError(Ex);
        }
        protected void Session_End(object sender, EventArgs e)
        {

        }
        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}