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

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmBudget : System.Web.UI.Page
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
        public static GLBudgetVwBO FillForm(int glCompanyId, int glProjectId, int fiscalYearId, Int64 nodeId, string hierarchy)
        {
            GLBudgetVwBO budgetView = new GLBudgetVwBO();
            GLBudgetDA budgetDA = new GLBudgetDA();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
            GLFiscalYearDA fiscalYearDA = new GLFiscalYearDA();

            GLBudgetBO budget = new GLBudgetBO();
            List<NodeMatrixBO> chartOfAccountsList = new List<NodeMatrixBO>();
            List<GLBudgetDetailsBO> budgetDetails = new List<GLBudgetDetailsBO>();
            GLFiscalYearBO fiscalYearBO = new GLFiscalYearBO();

            GLCompanyDA companyDA = new GLCompanyDA();
            GLCompanyBO glCompany = new GLCompanyBO();
            glCompany = companyDA.GetGLCompanyInfoById(glCompanyId);

            chartOfAccountsList = nodeMatrixDA.GetGetChartOfAccountsByHierarchy(nodeId, hierarchy);
            fiscalYearBO = fiscalYearDA.GetFiscalYearId(fiscalYearId);
            budget = budgetDA.GetBudgetByCompanyIdNProjectIdNFiscalYearId(glCompanyId, glProjectId, fiscalYearId);
            budgetDetails = budgetDA.GetBudgetDetailsByFiscalYearId(glCompanyId, glProjectId, fiscalYearId);

            List<MonthYearBO> monthyear = new List<MonthYearBO>();
            string format = fiscalYearBO.FromDate.Year == fiscalYearBO.ToDate.Year ? "MMM" : "MMM yy";

            if (glCompany.BudgetType == "Yearly")
            {
                monthyear.Add(new MonthYearBO
                {
                    MonthId = 0,
                    MonthName = fiscalYearBO.FromDate.ToString(format) + " - " + fiscalYearBO.ToDate.ToString(format)
                });
            }
            else if (glCompany.BudgetType == "Half Yearly")
            {
                monthyear.Add(new MonthYearBO
                {
                    MonthId = 1,
                    MonthName = fiscalYearBO.FromDate.ToString(format) + " - " + fiscalYearBO.FromDate.AddMonths(5).ToString(format)
                });

                monthyear.Add(new MonthYearBO
                {
                    MonthId = 7,
                    MonthName = fiscalYearBO.FromDate.AddMonths(6).ToString(format) + " - " + fiscalYearBO.ToDate.ToString(format)
                });
            }
            else if (glCompany.BudgetType == "Quarterly")
            {
                monthyear.Add(new MonthYearBO
                {
                    MonthId = 1,
                    MonthName = fiscalYearBO.FromDate.ToString(format) + " - " + fiscalYearBO.FromDate.AddMonths(3).ToString(format)
                });

                monthyear.Add(new MonthYearBO
                {
                    MonthId = 5,
                    MonthName = fiscalYearBO.FromDate.AddMonths(4).ToString(format) + " - " + fiscalYearBO.FromDate.AddMonths(7).ToString(format)
                });

                monthyear.Add(new MonthYearBO
                {
                    MonthId = 9,
                    MonthName = fiscalYearBO.FromDate.AddMonths(8).ToString(format) + " - " + fiscalYearBO.ToDate.ToString(format)
                });
            }
            else if (glCompany.BudgetType == "Monthly")
            {
                for (var d = fiscalYearBO.FromDate; d <= fiscalYearBO.ToDate; d = d.AddMonths(1))
                {
                    monthyear.Add(new MonthYearBO
                    {
                        MonthId = d.Month,
                        MonthName = d.ToString(format)
                    });
                }
            }

            string tHead = string.Empty, tBody = string.Empty, tFoot = string.Empty;

            tHead = string.Format(@"<table id='budgetTable' class='table table-bordered table-hover' style='width:100%;'>
                                    <thead>");
            tHead += string.Format(@"<tr> <th style=""width:150px;""> Accounts Head </th>");

            foreach (MonthYearBO d in monthyear)
            {
                tHead += string.Format(@"<th style=""width:32px; text-align:center;"">{0}</th>", d.MonthName);
            }
            tHead += string.Format(@"</tr></thead>");
            tBody = string.Format(@"<tbody>");

            foreach (NodeMatrixBO nm in chartOfAccountsList)
            {
                tBody += string.Format(@"<tr> <td mid=""{0}"" style=""width:150px;"">{1}</td>", nm.NodeId, nm.NodeHead);

                foreach (MonthYearBO d in monthyear)
                {
                    var v = (from bd in budgetDetails where bd.MonthId == d.MonthId && bd.NodeId == nm.NodeId select bd).FirstOrDefault();

                    tBody += string.Format(@"<td style=""width:32px;"" mid=""{0}"" did=""{1}"" > <input type=""text"" id=""b{2}"" value='{3}' class=""form-control quantitydecimal"" /> </td>",
                        d.MonthId.ToString(), (v != null ? v.BudgetDetailsId.ToString() : "0"), (nm.NodeId.ToString() + d.MonthId.ToString()),
                        (v != null ? v.Amount.ToString(".00") : ""));
                }
                tBody += string.Format(@"</tr>");
            }
            tFoot = string.Format(@"</tbody></table>");

            budgetView.budget = budget;
            budgetView.budgetTable = (tHead + tBody + tFoot);

            return budgetView;
        }
        [WebMethod]
        public static ReturnInfo SaveBudget(GLBudgetBO Budget, List<GLBudgetDetailsBO> BudgetDetails)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;
            long budgetId = 0;
            string voucherNo = string.Empty;

            GLBudgetDA budgetDA = new GLBudgetDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                Budget.CreatedBy = userInformationBO.UserInfoId;
                Budget.ApprovedStatus = HMConstants.ApprovalStatus.Submit.ToString();

                if (Budget.BudgetId == 0)
                {
                    status = budgetDA.SaveBudget(Budget, BudgetDetails, out budgetId);
                }
                else
                {
                    List<GLBudgetDetailsBO> BudgetDetailsNew = new List<GLBudgetDetailsBO>();
                    List<GLBudgetDetailsBO> BudgetDetailsEdited = new List<GLBudgetDetailsBO>();

                    BudgetDetailsNew = (from bd in BudgetDetails where bd.BudgetDetailsId == 0 select bd).ToList();
                    BudgetDetailsEdited = (from bd in BudgetDetails where bd.BudgetDetailsId > 0 select bd).ToList();
                    status = budgetDA.UpdateBudget(Budget, BudgetDetailsNew, BudgetDetailsEdited);
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

        [WebMethod]
        public static string GetBudgetType(int glCompanyId)
        {
            GLCompanyDA companyDA = new GLCompanyDA();
            GLCompanyBO glCompany = new GLCompanyBO();
            glCompany = companyDA.GetGLCompanyInfoById(glCompanyId);

            return glCompany.BudgetType;
        }


    }
}