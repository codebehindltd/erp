using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data.HMCommon;
using Microsoft.Reporting.WebForms;
using HotelManagement.Presentation.Website.Common;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HotelManagement.Data.Security;
using System.Web.Services;
using HotelManagement.Entity.Security;

namespace HotelManagement.Presentation.Website.HMCommon.Reports
{
    public partial class frmReportActivitiLogs : System.Web.UI.Page
    {
        protected int _ActivityLog = -1;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadModuleNameDropDown();
                LoadUserNames();
                LoadGroupName();
                LoadFeatures();
                LoadMenuGroup();
                LoadCommonMenuModule();
            }
        }
        private void LoadFeatures()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> featureList = new List<CustomFieldBO>();
            //featureList = commonDA.GetCustomField("InnboardFeatures", hmUtility.GetDropDownFirstAllValue());
            featureList = commonDA.GetCustomField("InnboardFeatures");

            ddlFeatures.DataSource = featureList;
            ddlFeatures.DataTextField = "Description";
            ddlFeatures.DataValueField = "FieldId";
            ddlFeatures.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlFeatures.Items.Insert(0, item);
        }
        private void LoadMenuGroup()
        {
            MenuDA menuDa = new MenuDA();

            this.ddlMenuGroup.DataSource = menuDa.GetMenuGroup();
            this.ddlMenuGroup.DataTextField = "MenuGroupName";
            this.ddlMenuGroup.DataValueField = "MenuGroupId";
            this.ddlMenuGroup.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlMenuGroup.Items.Insert(0, item);
        }
        private void LoadGroupName()
        {
            UserGroupDA userDa = new UserGroupDA();
            List<UserGroupBO> userGroupBOList = new List<UserGroupBO>();
            userGroupBOList = userDa.GetUserGroupInfo();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.UserInfoId != 1)
            {
                userGroupBOList = userGroupBOList.Where(x => x.UserGroupId != 1).ToList();
            }

            ddlUserGroupName.DataSource = userGroupBOList;
            ddlUserGroupName.DataValueField = "UserGroupId";
            ddlUserGroupName.DataTextField = "GroupName";
            ddlUserGroupName.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlUserGroupName.Items.Insert(0, item);
        }
        private void LoadCommonMenuModule()
        {
            MenuDA menuDa = new MenuDA();
            List<CommonModuleNameBO> menuGroup = new List<CommonModuleNameBO>();
            List<CommonModuleNameBO> permittedMenuGroup = new List<CommonModuleNameBO>();
            menuGroup = menuDa.GetCommonMenuModule();

            string permissionList = Session["SoftwareModulePermissionList"].ToString();
            permissionList = "0," + permissionList;
            permittedMenuGroup = menuGroup.Where(x => permissionList.Contains(x.TypeId.ToString())).ToList();

            this.ddlModuleId.DataSource = permittedMenuGroup;
            this.ddlModuleId.DataTextField = "ModuleName";
            this.ddlModuleId.DataValueField = "ModuleId";
            this.ddlModuleId.DataBind();

            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlModuleId.Items.Insert(0, item);
        }
        private void LoadModuleNameDropDown()
        {
            ProjectModuleEnum moduleEnum = new ProjectModuleEnum();
            ddlModuleName.DataSource = Enum.GetNames(typeof(ProjectModuleEnum.ProjectModule));
            ddlModuleName.DataBind();

            System.Web.UI.WebControls.ListItem itemAll = new System.Web.UI.WebControls.ListItem();
            itemAll.Value = "All";
            itemAll.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlModuleName.Items.Insert(0, itemAll);
        }
        private void LoadUserNames()
        {
            List<UserInformationBO> userList = new List<UserInformationBO>();
            UserInformationDA userDA = new UserInformationDA();
            userList = userDA.GetAllUserInformation();
            ddlUserName.DataSource = userList;
            ddlUserName.DataTextField = "UserName";
            ddlUserName.DataValueField = "UserInfoId";
            ddlUserName.DataBind();

            System.Web.UI.WebControls.ListItem itemAll = new System.Web.UI.WebControls.ListItem();
            itemAll.Value = "0";
            itemAll.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlUserName.Items.Insert(0, itemAll);
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            _ActivityLog = 1;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            string reportType = ddlReportType.SelectedValue;

            var reportPath = "";
            
            if(reportType == "ActivityLogDetails")
            {
                reportPath = Server.MapPath(@"~/HMCommon/Reports/Rdlc/rptActivityLogsReport.rdlc");
            }
            else if (reportType == "ApprovalConfigurationDetails")
            {
                reportPath = Server.MapPath(@"~/HMCommon/Reports/Rdlc/rptApprovalConfiguration.rdlc");
            }
            else if (reportType == "UserInformationDetails")
            {
                reportPath = Server.MapPath(@"~/HMCommon/Reports/Rdlc/rptUserInformation.rdlc");
            }
            else if (reportType == "UserPermissionDetails")
            {
                reportPath = Server.MapPath(@"~/HMCommon/Reports/Rdlc/rptUserPermission.rdlc");
            }            

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            List<ReportParameter> reportParam = new List<ReportParameter>();            

            DateTime currentDate = DateTime.Now;
            HMCommonDA printDateDA = new HMCommonDA();
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));            

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            if (files[0].CompanyId > 0)
            {
                reportParam.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    reportParam.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    reportParam.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }
            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(reportParam);

            string startDate = string.Empty;
            string endDate = string.Empty;
            DateTime dateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = this.txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            ToDate = ToDate.AddDays(1).AddSeconds(-1);

            string userName = ddlUserName.SelectedValue;
            string moduleName = ddlModuleName.SelectedValue;
            int moduleId = Convert.ToInt32( ddlModuleId.SelectedValue);
            string Features = ddlFeatures.SelectedValue;
            string UserGroupName = ddlUserGroupName.SelectedValue;
            string MenuGroup = ddlMenuGroup.SelectedValue;

            hfUserName.Value = userName;
            hfModuleName.Value = moduleName;
            hfModuleId.Value = moduleId.ToString();
            hfFeatures.Value = Features;
            hfUserGroupName.Value = UserGroupName;
            hfMenuGroup.Value = MenuGroup;
            hfreportType.Value = reportType;

            ReportDA reportDA = new ReportDA();
            List<ActivityLogReportViewBO> activitylogBO = new List<ActivityLogReportViewBO>();
            List<GetApprovalConfigurationForReportViewBO> configurationlogBO = new List<GetApprovalConfigurationForReportViewBO>();
            List<GetUserInformationForReportViewBO> informationlogBO = new List<GetUserInformationForReportViewBO>();
            List<GetUserPermissionForReportViewBO> permissionlogBO = new List<GetUserPermissionForReportViewBO>();

            if (reportType == "ActivityLogDetails")
            {
                activitylogBO = reportDA.GetActivityLogInfo(FromDate, ToDate, userName, moduleName);
                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], activitylogBO));
                rvTransaction.LocalReport.DisplayName = "Activity Log Info";
            }
            else if (reportType == "ApprovalConfigurationDetails")
            {
                configurationlogBO = reportDA.GetApprovalConfigurationForReport(Convert.ToInt32( Features), Convert.ToInt32(UserGroupName), Convert.ToInt32(userName));
                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], configurationlogBO));
                rvTransaction.LocalReport.DisplayName = "Approval Configuration Details";
            }
            else if (reportType == "UserInformationDetails")
            {
                informationlogBO = reportDA.GetUserInformationForReport(Convert.ToInt32(UserGroupName));
                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], informationlogBO));
                rvTransaction.LocalReport.DisplayName = "User Information Details";
            }
            else if (reportType == "UserPermissionDetails")
            {
                int menuLinkId = 0;
                if(hfMenuLink.Value != "" && hfMenuLink.Value  != "0")
                {
                    menuLinkId = Convert.ToInt32(hfMenuLink.Value);
                }
                permissionlogBO = reportDA.GetUserPermissionForReport(Convert.ToInt32(UserGroupName), (moduleId), Convert.ToInt32(MenuGroup), menuLinkId);

                var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], permissionlogBO));
                rvTransaction.LocalReport.DisplayName = "User Permission Details";
            }
            
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
        }

        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Portrait.ToString());
            frmPrint.Attributes["src"] = reportSource;             
        }

        [WebMethod]
        public static List<MenuLinksBO> GetLinkForReport(int moduleId, int UserGroupId, int MenuGroupId)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            List<MenuLinksBO> list = new List<MenuLinksBO>();
            ReportDA reportDA = new ReportDA();
            list = reportDA.GetMenuLinksForReportPage(moduleId,  UserGroupId,  MenuGroupId);
            return list;
        }

    }
}