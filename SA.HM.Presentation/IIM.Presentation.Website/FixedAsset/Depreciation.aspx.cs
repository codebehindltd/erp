using HotelManagement.Data.FixedAsset;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.FixedAsset;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.FixedAsset
{
    public partial class Depreciation : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
                LoadCompany();
            }

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
        public static List<GLOpeningBalanceTransactionNodeAutoComplete> AutoCompleteTransactionNode(string searchText)
        {
            List<GLOpeningBalanceTransactionNodeAutoComplete> nodeList = new List<GLOpeningBalanceTransactionNodeAutoComplete>();


            List<NodeMatrixBO> nodeMatrixBOList = new List<NodeMatrixBO>();
            NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();

            nodeMatrixBOList = nodeMatrixDA.GetNodeMatrixInfoByNameAndTransactionFlag(searchText, false);
            nodeList = (from n in nodeMatrixBOList
                        select new GLOpeningBalanceTransactionNodeAutoComplete()
                        {
                            TransactionNodeId = n.NodeId,
                            NodeName = n.NodeHead,
                            Lvl = n.Lvl,
                            Hierarchy = n.Hierarchy
                        }).ToList();

            return nodeList;
        }
        [WebMethod]
        public static FADepreciationViewBO FillForm(int companyId, int projectcId,int fiscalYearId, long transactionNodeId, string hierarchy)
        {
            FADepreciationViewBO DepreciationView = new FADepreciationViewBO();
            List<GLOpeningBalanceTransactionNodeAutoComplete> nodeList = new List<GLOpeningBalanceTransactionNodeAutoComplete>();

            DepreciationDA depreciationDA = new DepreciationDA();
            string TableHeader = string.Empty;
            DepreciationView = depreciationDA.GetFADepreciationDetailsByNCompanyIdNProjectIdNFiscalYearId(companyId, projectcId, fiscalYearId);

            
                List<NodeMatrixBO> chartOfAccountsList = new List<NodeMatrixBO>();
                NodeMatrixDA nodeMatrixDA = new NodeMatrixDA();
                chartOfAccountsList = nodeMatrixDA.GetGetChartOfAccountsByHierarchy(transactionNodeId, hierarchy);

                TableHeader = "Accounts Head";
                nodeList = (from n in chartOfAccountsList
                            select new GLOpeningBalanceTransactionNodeAutoComplete()
                            {
                                TransactionNodeId = n.NodeId,
                                NodeName = n.NodeHeadDisplay,
                                Code = n.NodeNumber
                            }).ToList();

            DepreciationView.DepreciationTableString = GenerateTable(nodeList, DepreciationView.DepreciationDetailsBOList, TableHeader);

            return DepreciationView;
        }
        [WebMethod]
        public static ReturnInfo SaveDepreciation(FADepreciationBO DepreciationBO, List<FADepreciationDetailsBO> DepreciationDetailsBO)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            DepreciationDA DepreciationDA = new DepreciationDA();
            long DepreciationId = 0;
            try
            {
                UserInformationBO userInformation = hmUtility.GetCurrentApplicationUserInfo();
                DepreciationBO.CreatedBy = userInformation.UserInfoId;
                DepreciationBO.LastModifiedBy = userInformation.UserInfoId;
                returnInfo.IsSuccess = DepreciationDA.SaveOrUpdateDepreciation(DepreciationBO, DepreciationDetailsBO, out DepreciationId);

                if (returnInfo.IsSuccess)
                {
                    if (DepreciationBO.Id == 0)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                        EntityTypeEnum.EntityType.FixedAssets.ToString(), DepreciationId,
                        ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.FixedAssets));
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    }
                    else
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                        EntityTypeEnum.EntityType.FixedAssets.ToString(), DepreciationBO.Id,
                        ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.FixedAssets));
                        returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    }
                }
                else
                {
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                throw ex;
            }
            return returnInfo;
        }

        private static string GenerateTable(List<GLOpeningBalanceTransactionNodeAutoComplete> nodeList, List<FADepreciationDetailsBO> DepreciationDetails, string TableHeader)
        {
            string tHead = string.Empty, tBody = string.Empty, tFoot = string.Empty;

            tHead = string.Format(@"<table id='depressionTable' class='table table-bordered table-hover' style='width:100%;'>
                                    <thead>");
            tHead += string.Format(@"<tr> <th style=""width:40%;"">" + TableHeader + "</th>");
            tHead += string.Format(@"<th style=""width:20%; text-align:left;"">{0}</th>", "Code");
            tHead += string.Format(@"<th style=""width:20%; text-align:left;"">{0}</th>", "Depreciation Percentage");

            tHead += string.Format(@"</tr></thead>");
            tBody = string.Format(@"<tbody>");

            foreach (GLOpeningBalanceTransactionNodeAutoComplete opd in nodeList)
            {
                var detail = DepreciationDetails.Where(i => i.TransactionNodeId == opd.TransactionNodeId).FirstOrDefault();
                tBody += string.Format(@"<tr> <td did =""{0}""  tnid=""{1}"" style=""width:40%;"">{2}</td>", detail == null ? 0 : detail.Id, opd.TransactionNodeId, opd.NodeName);
                tBody += "<td style=\"width:20%;\" >" + opd.Code + "</td>";
                tBody += "<td style=\"width:20%;\" > <input type=\"text\" onblur=\"return CheckInputValue(this)\" class=\"form-control quantitynegativedecimal\" value=" + (detail == null ? "" : detail.DepreciationPercentage.ToString()) + "> </td>";                
                tBody += string.Format(@"</tr>");
            }
            tFoot = string.Format(@"</tbody></table>");

            return (tHead + tBody + tFoot);
        }

    }
}