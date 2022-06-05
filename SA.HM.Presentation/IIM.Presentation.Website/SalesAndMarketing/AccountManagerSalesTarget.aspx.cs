using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using System.Collections;
using HotelManagement.Entity.GeneralLedger;
using Microsoft.Reporting.WebForms;
using System.IO;
using Newtonsoft.Json;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Data.SalesAndMarketing;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class AccountManagerSalesTarget : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCommonDropDownHiddenField();
                LoadCompany();
            }
        }
        //************************ User Defined Function ********************//
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadCompany()
        {
            GLCompanyDA companyDA = new GLCompanyDA();
            List<GLCompanyBO> List = new List<GLCompanyBO>();
            List = companyDA.GetAllGLCompanyInfo();

            ddlCompany.DataSource = List;
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyId";
            ddlCompany.DataBind();

            ListItem itemNodeId = new ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstValue();
            ddlCompany.Items.Insert(0, itemNodeId);
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<NodeMatrixBO> GetAutoCompleteData(string searchText)
        {
            List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            nodeMatrixBOList = nodeMatrixDA.GetNodeMatrixInfoByNameAndTransactionFlag(searchText, false);
            return nodeMatrixBOList;
        }
        [WebMethod]
        public static AccountManagerSalesTargetVwBO FillForm(int glCompanyId, int glProjectId, int fiscalYearId)
        {
            AccountManagerSalesTargetVwBO salesTargetView = new AccountManagerSalesTargetVwBO();
            AccountManagerSalesTargetDA salesTargetDA = new AccountManagerSalesTargetDA();
            AccountManagerDA accountManagerDA = new AccountManagerDA();
            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();

            AccountManagerSalesTargetBO salesTarget = new AccountManagerSalesTargetBO();
            List<AccountManagerBO> accountManagerBOList = new List<AccountManagerBO>();
            List<AccountManagerSalesTargetDetailsBO> salesTargetDetails = new List<AccountManagerSalesTargetDetailsBO>();
            GLFiscalYearBO fiscalYearBO = new GLFiscalYearBO();

            accountManagerBOList = accountManagerDA.GetAccountManagerInfoByCustomString("WHERE  lvl = 0 AND Type = 'CRM'");
            fiscalYearBO = fiscalYearDA.GetFiscalYearId(fiscalYearId);
            salesTarget = salesTargetDA.GetSalesTargetByCompanyIdNProjectIdNFiscalYearId(glCompanyId, glProjectId, fiscalYearId);
            salesTargetDetails = salesTargetDA.GetSalesTargetDetailsByFiscalYearId(glCompanyId, glProjectId, fiscalYearId);

            List<MonthYearBO> monthyear = new List<MonthYearBO>();
            string format = fiscalYearBO.FromDate.Year == fiscalYearBO.ToDate.Year ? "MMM" : "MMM yy";

            string tHead = string.Empty, tBody = string.Empty, tFoot = string.Empty;

            for (var d = fiscalYearBO.FromDate; d <= fiscalYearBO.ToDate; d = d.AddMonths(1))
            {
                monthyear.Add(new MonthYearBO
                {
                    MonthId = d.Month,
                    MonthName = d.ToString(format)
                });
            }

            tHead = string.Format(@"<table id='salesTargetTable' class='table table-bordered table-hover' style='width:100%;'>
                                    <thead>");
            tHead += string.Format(@"<tr> <th style=""width:150px;""> Account Manager </th>");

            foreach (MonthYearBO d in monthyear)
            {
                tHead += string.Format(@"<th style=""width:32px; text-align:center;"">{0}</th>", d.MonthName);
            }
            tHead += string.Format(@"</tr></thead>");
            tBody = string.Format(@"<tbody>");

            foreach (AccountManagerBO nm in accountManagerBOList)
            {
                tBody += string.Format(@"<tr> <td mid=""{0}"" style=""width:150px;"">{1}</td>", nm.AccountManagerId, nm.AccountManager);

                foreach (MonthYearBO d in monthyear)
                {
                    var v = (from bd in salesTargetDetails where bd.MonthId == d.MonthId && bd.AccountManagerId == nm.AccountManagerId select bd).FirstOrDefault();
                    tBody += string.Format(@"<td style=""width:32px;"" mid=""{0}"" did=""{1}"" > <input type=""text"" id=""b{2}"" value='{3}' class=""form-control quantitydecimal"" /> </td>",
                        d.MonthId.ToString(), (v != null ? v.TargetDetailsId.ToString() : "0"), (nm.AccountManagerId.ToString() + d.MonthId.ToString()),
                        (v != null ? v.Amount.ToString(".00") : ""));
                }
                tBody += string.Format(@"</tr>");
            }
            tFoot = string.Format(@"</tbody></table>");

            salesTargetView.salesTarget = salesTarget;
            salesTargetView.SalesTargetTable = (tHead + tBody + tFoot);

            return salesTargetView;
        }
        [WebMethod]
        public static ReturnInfo SaveAccountManagerSalesTarget(AccountManagerSalesTargetBO SalesTarget, List<AccountManagerSalesTargetDetailsBO> SalesTargetDetails)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            long targetId = 0;
            string voucherNo = string.Empty;

            AccountManagerSalesTargetDA budgetDA = new AccountManagerSalesTargetDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                SalesTarget.CreatedBy = userInformationBO.UserInfoId;
                SalesTarget.ApprovedStatus = HMConstants.ApprovalStatus.Submit.ToString();

                if (SalesTarget.TargetId == 0)
                {
                    status = budgetDA.SaveAccountManagerSalesTarget(SalesTarget, SalesTargetDetails, out targetId);
                }
                else
                {
                    List<AccountManagerSalesTargetDetailsBO> SalesTargetDetailsNew = new List<AccountManagerSalesTargetDetailsBO>();
                    List<AccountManagerSalesTargetDetailsBO> SalesTargetDetailsEdited = new List<AccountManagerSalesTargetDetailsBO>();

                    SalesTargetDetailsNew = (from bd in SalesTargetDetails where bd.TargetDetailsId == 0 select bd).ToList();
                    SalesTargetDetailsEdited = (from bd in SalesTargetDetails where bd.TargetDetailsId > 0 select bd).ToList();
                    status = budgetDA.UpdateAccountManagerSalesTarget(SalesTarget, SalesTargetDetailsNew, SalesTargetDetailsEdited);
                }

                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }

                if (!status)
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }
        [WebMethod]
        public static List<GLProjectBO> GetGLProjectByGLCompanyId(int companyId)
        {
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            GLProjectDA entityDA = new GLProjectDA();

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            projectList = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(companyId, userInformationBO.UserGroupId);

            return projectList;
        }
        [WebMethod]
        public static List<GLFiscalYearBO> PopulateFiscalYear(int projectId)
        {
            ArrayList list = new ArrayList();
            List<GLFiscalYearBO> fiscalYearList = new List<GLFiscalYearBO>();
            GLFiscalYearDA entityDA = new GLFiscalYearDA();
            fiscalYearList = entityDA.GetFiscalYearListByProjectId(Convert.ToInt32(projectId));

            return fiscalYearList;
        }


    }
}