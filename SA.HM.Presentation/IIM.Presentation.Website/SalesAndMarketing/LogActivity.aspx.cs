using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using System.Web.Services;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using Newtonsoft.Json;
using System.Collections;
using Mamun.Presentation.Website.Common;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.SalesManagment;
using System.Text;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class LogActivity : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");

            if (!IsPostBack)
            {
                LoadAccountManager();
                LoadLogType();
            }
        }

        private void LoadAccountManager()
        {
            int isAdminUser = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            AccountManagerDA accountManagerDA = new AccountManagerDA();
            List<AccountManagerBO> accountManagerBOList = new List<AccountManagerBO>();
            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                isAdminUser = 1;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 22).Count() > 0)
                    {
                        isAdminUser = 1;
                    }
                }
            }
            #endregion

            accountManagerBOList = accountManagerDA.GetAccountManager(isAdminUser, "CRM", userInformationBO.UserInfoId);

            ddlLogOwner.DataSource = accountManagerBOList;
            ddlLogOwner.DataTextField = "DisplayName";
            ddlLogOwner.DataValueField = "UserInfoId";
            ddlLogOwner.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlLogOwner.Items.Insert(0, item);
        }
        private void LoadLogType()
        {
            List<CommonDropDownBO> lst = ((ConstantHelper.SalesandMarketingLogType[])Enum.GetValues(typeof(ConstantHelper.SalesandMarketingLogType))).Select(c => new CommonDropDownBO() { IdString = c.ToString(), Name = CommonHelper.SentenceCaseConvertionToUpperCase(c.ToString()) }).ToList();

            ddlActivity.DataSource = lst;
            ddlActivity.DataTextField = "Name";
            ddlActivity.DataValueField = "IdString";
            ddlActivity.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlActivity.Items.Insert(0, item);
        }

        [WebMethod]
        public static string LoadLoggedDetails(int? companyId, Int64? contactId, Int64? dealId,
                                     string logType, int? userId, DateTime? dateFrom, DateTime? dateTo, long salesCallEntryId)
        {
            if (HttpContext.Current.Request.QueryString["cid"] != null)
            {
                companyId = Convert.ToInt32(HttpContext.Current.Request.QueryString["cid"]);
            }
            if (HttpContext.Current.Request.QueryString["conid"] != null)
            {
                contactId = Convert.ToInt32(HttpContext.Current.Request.QueryString["conid"]);
            }
            if (HttpContext.Current.Request.QueryString["did"] != null)
            {
                dealId = Convert.ToInt32(HttpContext.Current.Request.QueryString["did"]);
            }
            if (HttpContext.Current.Request.QueryString["sceid"] != null)
            {
                salesCallEntryId = Convert.ToInt32(HttpContext.Current.Request.QueryString["sceid"]);
            }

            SalesMarketingDA sm = new SalesMarketingDA();
            List<SMLogKeepingBO> logKeepingList = new List<SMLogKeepingBO>();
            if (salesCallEntryId != 0)
            {
                logKeepingList = sm.GetSalesMarketingLogActivity(salesCallEntryId);
            }
            else
            {
                logKeepingList = sm.GetSalesMarketingLogActivity(companyId, contactId, dealId, logType, userId, dateFrom, dateTo);
            }
            return GenerateLogActivity(logKeepingList, salesCallEntryId);
        }

        public static string GenerateLogActivity(List<SMLogKeepingBO> logKeepingList, long salesCallEntryId)
        {
            StringBuilder logged = new StringBuilder();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            string edit = string.Empty, delete = string.Empty, logtime = string.Empty;

            try
            {
                foreach (SMLogKeepingBO log in logKeepingList)
                {
                    if (log.SalesCallEntryId > 0)
                    {
                        if (salesCallEntryId == 0)
                        {
                            edit = string.Format("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"javascript: void()\" title=\"Edit\" onclick=\"javascript:return EditLog({0})\">Edit</a>&nbsp;&nbsp;&nbsp;&nbsp;", log.SalesCallEntryId);
                            delete = string.Format("&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"javascript: void()\" title=\"Delete\" onclick=\"javascript:return DeleteLog({0})\">Delete</a>", log.SalesCallEntryId);
                        }                        
                    }
                    else { edit = string.Empty; delete = string.Empty; }
                    logtime = string.Format("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span>{0}</span>", log.LogDateTime.ToString("dd MMMM yyyy") + " at " + log.LogDateTime.ToString(userInformationBO.TimeFormat));

                    logged.Append("<div class=\"panel panel-default\">");

                    logged.Append("<div class=\"panel panel-title panel-title-Custom\" style=\"padding: 10px 15px 10px 15px; font-size:14px; \">");

                    if (log.Type == "LoggedCall")
                        logged.Append("<i title = \"Call\" class=\"glyphicon glyphicon-earphone\"></i>&nbsp;&nbsp;&nbsp;");
                    if (log.Type == "LoggedMessage")
                        logged.Append("<i title = \"Call\" class=\"glyphicon glyphicon-envelope\"></i>&nbsp;&nbsp;&nbsp;");
                    else if (log.Type == "LoggedEmail")
                        logged.Append("<i title = \"Email\" class=\"glyphicon glyphicon-envelope\"></i>&nbsp;&nbsp;&nbsp;");
                    else if (log.Type == "LoggedMeeting")
                        logged.Append("<i title = \"Call\" class=\"glyphicon glyphicon-briefcase\"></i>&nbsp;&nbsp;&nbsp;");

                    logged.AppendFormat("{0} {1} {2} {3}", log.Title, logtime, edit, delete);

                    logged.Append("</div>");

                    //if (log.Type == "LoggedEmail")
                    //{
                    //    logged.AppendFormat("<div class=\"panel-body\" style=\"border-bottom: 1px solid #aaa;white-space:pre-wrap;\">{0}", log.Description.Replace("|", "</div><div class=\"panel-body\">"));
                    //    logged.Append("</div>");
                    //    logged.Append("</div>");
                    //}
                    //else if (log.Type == "LoggedCall")
                    //{
                    //    logged.AppendFormat("<div class=\"panel-body\" style=\"border-bottom: 1px solid #aaa;white-space:pre-wrap;\">{0}", log.Description.Replace("|", "</div><div class=\"panel-body\" style=\"border-bottom: 1px solid #aaa;white-space:pre-wrap;\">").Replace("%", "<br />"));
                    //    logged.Append("</div>");
                    //    logged.Append("</div>");
                    //}
                    //else if (log.Type == "LoggedMessage")
                    //{
                    //    logged.AppendFormat("<div class=\"panel-body\" style=\"border-bottom: 1px solid #aaa;white-space:pre-wrap;\">{0}", log.Description.Replace("|", "</div><div class=\"panel-body\" style=\"border-bottom: 1px solid #aaa;white-space:pre-wrap;\">").Replace("%", "<br />"));
                    //    logged.Append("</div>");
                    //    logged.Append("</div>");
                    //}
                    //else
                    //{
                    //    logged.AppendFormat("<div class=\"panel-body\" style=\"white-space:pre-wrap;\">{0}", log.Description.Replace("|", "<br />"));
                    //    logged.Append("</div>");
                    //    logged.Append("</div>");
                    //}

                    if (log.Type == "LoggedMeeting")
                    {
                        logged.AppendFormat("<div class=\"panel-body\" style=\"white-space:pre-wrap;\">{0}", log.Description.Replace("|", "<br />"));
                        logged.Append("</div>");
                        logged.Append("</div>");
                    }
                    else
                    {
                        logged.AppendFormat("<div class=\"panel-body\" style=\"border-bottom: 1px solid #aaa;white-space:pre-wrap;\">{0}", log.Description.Replace("|", "</div><div class=\"panel-body\" style=\"border-bottom: 1px solid #aaa;white-space:pre-wrap;\">").Replace("~", "<br />"));
                        logged.Append("</div>");
                        logged.Append("</div>");
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return logged.ToString();
        }

    }
}