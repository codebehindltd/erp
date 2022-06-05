using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Inventory;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Entity.Inventory;
using System.Web.Services;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.PurchaseManagment.Reports
{
    public partial class frmReportItemWiseRequisitionInformation : BasePage
    {
        protected int _RoomStatusInfoByDate = -1;
        HMUtility hmUtility = new HMUtility();
        protected bool isSingle = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //this.LoadProduct();
                LoadAllCostCentreTabInfo();
                LoadCategory();
                LoadItem();
                LoadGLCompany(false);
                LoadCommonDropDownHiddenField();
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        private void LoadGLCompany(bool isSingle)
        {
            hfIsSingle.Value = "0";
            GLCompanyDA entityDA = new GLCompanyDA();
            var List = entityDA.GetAllGLCompanyInfo();

            //hfCompanyAll.Value = JsonConvert.SerializeObject(List);

            List<GLCompanyBO> companyList = new List<GLCompanyBO>();
            if (List.Count == 1)
            {
                companyList.Add(List[0]);
                ddlSrcGLCompany.DataSource = companyList;
                ddlSrcGLCompany.DataTextField = "Name";
                ddlSrcGLCompany.DataValueField = "CompanyId";
                ddlSrcGLCompany.DataBind();
                LoadGLProjectByCompany(companyList[0].CompanyId);

            }
            else
            {
                ddlSrcGLCompany.DataSource = List;
                ddlSrcGLCompany.DataTextField = "Name";
                ddlSrcGLCompany.DataValueField = "CompanyId";
                ddlSrcGLCompany.DataBind();
                ListItem itemCompany = new ListItem();
                itemCompany.Value = "0";
                itemCompany.Text = hmUtility.GetDropDownFirstValue();
                ddlSrcGLCompany.Items.Insert(0, itemCompany);
            }


        }
        private void LoadGLProjectByCompany(int companyId)
        {
            GLProjectDA entityDA = new GLProjectDA();
            List<GLProjectBO> projectList = new List<GLProjectBO>();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            var List = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(Convert.ToInt32(companyId), Convert.ToInt32(userInformationBO.UserGroupId)).Where(x => x.IsFinalStage == false).ToList();


            ddlSrcGLProject.DataSource = List;
            ddlSrcGLProject.DataTextField = "Name";
            ddlSrcGLProject.DataValueField = "ProjectId";
            ddlSrcGLProject.DataBind();

            if (List.Count > 1)
            {
                isSingle = false;
                hfIsSingle.Value = "0";
                ListItem itemProject = new ListItem();
                itemProject.Value = "0";
                itemProject.Text = hmUtility.GetDropDownFirstAllValue();
                ddlSrcGLProject.Items.Insert(0, itemProject);
            }
            else
            {
                hfIsSingle.Value = "1";
            }

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
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            HMCommonDA DA = new HMCommonDA();
            _RoomStatusInfoByDate = 1;
            string startDate = string.Empty, endDate = string.Empty;
            string reportType = ddlReportType.SelectedValue;
            
            DateTime dateTime = DateTime.Now;
            int? itemId = null;
            int fromCostCenter = Convert.ToInt32(ddlFromCostCenter.SelectedValue);
            int toCostCenter = Convert.ToInt32(ddlToCostCenter.SelectedValue);
            string pMNumber = txtPMNumber.Text;
            int category = Convert.ToInt32(ddlCategory.SelectedValue);
            //string PMNumber = txtPMNumber.Text;
            itemId = Convert.ToInt32(ddlItemName.SelectedValue);
            if (string.IsNullOrEmpty(txtStartDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtStartDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = txtStartDate.Text;
            }
            if (string.IsNullOrEmpty(txtEndDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                txtEndDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = txtEndDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(1).AddSeconds(-1);


            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            string reportName = "";
            var reportPath = "";
            if (reportType == "Item Wise")
            {
                reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/rptItemWiseRequisition.rdlc");
                reportName = "Item Wise Requisition Information";
            }
            else if (reportType == "Date Wise")
            {
                reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/rptDateWiseRequisition.rdlc");
                reportName = "Date Wise Requisition";
            }
            else if(reportType == "Invoice Format Wise")
            {
                reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/RptRequisitionInfoDetails.rdlc");
                reportName = "Invoice wise Requisition Information";
            }
            else if (reportType == "Costcenter Wise")
            {
                reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/rptCostcenterWiseRequisition.rdlc");
                reportName = "Cost Center Wise Requisition";
            }
            else if (reportType == "Category Wise")
            {
                reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/rptCategoryWiseRequisition.rdlc");
                reportName = "Category Wise Requisition Information";
            }
            else if (reportType == "Requisition Number Wise")
            {
                reportPath = Server.MapPath(@"~/PurchaseManagment/Reports/Rdlc/rptRequisitionNumberWiseRequisition.rdlc");
                reportName = "Requisition Number Wise Information";
            }


            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            int companyId = 0, projectId = 0;

            string companyName = string.Empty;
            string projectName = string.Empty;

            if (ddlSrcGLCompany.SelectedValue != "0") //|| ddlSrcGLCompany.SelectedValue != ""
            {
                companyId = Convert.ToInt32(ddlSrcGLCompany.SelectedValue);
                companyName = ddlSrcGLCompany.SelectedItem.Text;
            }

            if (hfProjectId.Value != "") // ddlSrcGLProject.SelectedValue != "0" ||
            {
                projectId = Convert.ToInt32(hfProjectId.Value);
                projectName = hfProjectId.Value != "0" ? hfProjectName.Value : "All";
            }

            List<ReportParameter> reportParam = new List<ReportParameter>();

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
            byte[] QrImage;

            QrImage = DA.GenerateQrCode(reportName+"; "+files[0].CompanyName+"; "+ files[0].CompanyAddress+";");

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            reportParam.Add(new ReportParameter("PrintDateTime", printDate));
            reportParam.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            HMCommonDA hmCommonDA = new HMCommonDA();
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            reportParam.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            rvTransaction.LocalReport.EnableExternalImages = true;
            reportParam.Add(new ReportParameter("CompanyName", companyName));
            reportParam.Add(new ReportParameter("CompanyProject", projectName));
            reportParam.Add(new ReportParameter("QrImage", Convert.ToBase64String(QrImage)));
            rvTransaction.LocalReport.SetParameters(reportParam);
            var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
            string approvalStatus = string.Empty;
            PMRequisitionDA reqDA = new PMRequisitionDA();
            List<PMRequisitionReportViewBO> purchaseInfo = new List<PMRequisitionReportViewBO>();
            List<PMRequisitionBO> allrequisitionList = new List<PMRequisitionBO>();
            if (ddlPOApprovalStatus.SelectedValue != "0")
            {
                approvalStatus = ddlPOApprovalStatus.SelectedItem.ToString();
                if (reportType == "Invoice Format Wise")
                {
                    allrequisitionList = reqDA.GetRequisitionInfoForReport(FromDate, ToDate, "Submitted", pMNumber);
                }
                else
                {
                    purchaseInfo = reqDA.GetRequisitionReportInfo(FromDate, ToDate, itemId, fromCostCenter, toCostCenter, pMNumber, category, Convert.ToInt32(companyId), Convert.ToInt32(projectId)).Where(x => x.ApprovedStatus == approvalStatus).ToList();

                }
            }
            else
            {
                if (reportType == "Invoice Format Wise")
                {
                    allrequisitionList = reqDA.GetRequisitionInfoForReport(FromDate, ToDate, "", pMNumber);
                }
                else
                {
                    purchaseInfo = reqDA.GetRequisitionReportInfo(FromDate, ToDate, itemId, fromCostCenter, toCostCenter, pMNumber, category, Convert.ToInt32(companyId), Convert.ToInt32(projectId));
                }
            }
            //if (ddlPOApprovalStatus.SelectedValue == "0")
            //{
            //    if (reportType == "Invoice Format Wise")
            //    {
            //        allrequisitionList = reqDA.GetRequisitionInfoForReport(FromDate, ToDate, "", pMNumber);
            //    }
            //    else
            //    {
            //        purchaseInfo = reqDA.GetRequisitionReportInfo(FromDate, ToDate, itemId, fromCostCenter, toCostCenter, pMNumber, category).ToList();
            //    }

            //}
            //else if (ddlPOApprovalStatus.SelectedValue == "1")
            //{
            //    if (reportType == "Invoice Format Wise")
            //    {
            //        allrequisitionList = reqDA.GetRequisitionInfoForReport(FromDate, ToDate, "Submitted", pMNumber);
            //    }
            //    else
            //    {
            //        purchaseInfo = reqDA.GetRequisitionReportInfo(FromDate, ToDate, itemId, fromCostCenter, toCostCenter, pMNumber, category).Where(x => x.ApprovedStatus == "Submitted").ToList();

            //    }
            //}
            //else if (ddlPOApprovalStatus.SelectedValue == "2")
            //{
            //    if (reportType == "Invoice Format Wise")
            //    {
            //        allrequisitionList = reqDA.GetRequisitionInfoForReport(FromDate, ToDate, "Approved", pMNumber);
            //    }
            //    else
            //    {
            //        purchaseInfo = reqDA.GetRequisitionReportInfo(FromDate, ToDate, itemId, fromCostCenter, toCostCenter, pMNumber, category).Where(x => x.ApprovedStatus == "Approved").ToList();
            //    }
            //}
            //else if (ddlPOApprovalStatus.SelectedValue == "3")
            //{
            //    if (reportType == "Invoice Format Wise")
            //    {
            //        allrequisitionList = reqDA.GetRequisitionInfoForReport(FromDate, ToDate, "Checked", pMNumber);
            //    }
            //    else
            //    {
            //        purchaseInfo = reqDA.GetRequisitionReportInfo(FromDate, ToDate, itemId, fromCostCenter, toCostCenter, pMNumber, category).Where(x => x.ApprovedStatus == "Checked").ToList();
            //    }
            //}
            //else if (ddlPOApprovalStatus.SelectedValue == "4")
            //{
            //    if (reportType == "Invoice Format Wise")
            //    {
            //        allrequisitionList = reqDA.GetRequisitionInfoForReport(FromDate, ToDate, "Cancel", pMNumber);
            //    }
            //    else
            //    {
            //        purchaseInfo = reqDA.GetRequisitionReportInfo(FromDate, ToDate, itemId, fromCostCenter, toCostCenter, pMNumber, category).Where(x => x.ApprovedStatus == "Cancel").ToList();
            //    }
            //}

            if (reportType == "Invoice Format Wise")
            {
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], allrequisitionList));
            }
            else
            {
                rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], purchaseInfo));
            }
            //rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[1], QrImage));
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        //************************ User Defined Function ********************//
        //private void LoadProduct()
        //{
        //    int catagory = Convert.ToInt32(ddlCategory.SelectedValue);
        //    int costcenter = Convert.ToInt32(ddlFromCostCenter.SelectedValue);
        //    List <InvItemBO> invItem = new List<InvItemBO>();
        //    InvItemDA productDA = new InvItemDA();
        //    invItem = productDA.GetInvItemInfoByCategoryId(costcenter, catagory);
        //    invItem = invItem.Where(p => p.StockType == "StockItem").ToList();
        //    this.ddlProductId.DataSource = productDA.GetInvItemInfo();
        //    this.ddlProductId.DataTextField = "Name";
        //    this.ddlProductId.DataValueField = "ItemId";
        //    this.ddlProductId.DataBind();

        //    ListItem itemProduct = new ListItem();
        //    itemProduct.Value = "0";
        //    itemProduct.Text = "---All---";
        //    this.ddlProductId.Items.Insert(0, itemProduct);

        //}
        private void LoadAllCostCentreTabInfo()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> costCentreTabBOList = new List<CostCentreTabBO>();
            List<CostCentreTabBO> requisitionToCostCentreList = new List<CostCentreTabBO>();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO invoiceTemplateBO = new HMCommonSetupBO();
            invoiceTemplateBO = commonSetupDA.GetCommonConfigurationInfo("IsUserpermissionAppliedToCostcenterFilteringForPOPR", "IsUserpermissionAppliedToCostcenterFilteringForPOPR");

            costCentreTabBOList = costCentreTabDA.GetCostCentreTabInfoByUserGroupId(userInformationBO.UserGroupId)
                                    .Where(o => o.OutletType == 1 && o.CostCenterType == "Inventory").ToList();

            this.ddlFromCostCenter.DataSource = costCentreTabBOList;
            this.ddlFromCostCenter.DataTextField = "CostCenter";
            this.ddlFromCostCenter.DataValueField = "CostCenterId";
            this.ddlFromCostCenter.DataBind();


            requisitionToCostCentreList = costCentreTabDA.GetCostCentreTabInfoByType("Inventory");

            ddlToCostCenter.DataSource = requisitionToCostCentreList;
            ddlToCostCenter.DataTextField = "CostCenter";
            ddlToCostCenter.DataValueField = "CostCenterId";
            ddlToCostCenter.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlFromCostCenter.Items.Insert(0, item);
            ddlToCostCenter.Items.Insert(0, item);
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllActiveInvItemCatagoryInfoByServiceType("All");
            ddlCategory.DataSource = List;
            ddlCategory.DataTextField = "MatrixInfo";
            ddlCategory.DataValueField = "CategoryId";
            ddlCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlCategory.Items.Insert(0, item);
        }
        private void LoadItem()
        {
            List<InvItemBO> items = new List<InvItemBO>();
            InvItemDA productDA = new InvItemDA();
            items = productDA.GetInvItemInfo();
            items = items.Where(p => p.StockType == "StockItem").ToList();
            this.ddlItemName.DataSource = items;
            this.ddlItemName.DataTextField = "Name";
            this.ddlItemName.DataValueField = "ItemId";
            this.ddlItemName.DataBind();

            ListItem itemProduct = new ListItem();
            itemProduct.Value = "0";
            itemProduct.Text = "---All---";
            this.ddlItemName.Items.Insert(0, itemProduct);
        }


        //[WebMethod]
        //public static List<InvItemAutoSearchBO> ItemSearch(string searchTerm, int costCenterId, int categoryId)
        //{
        //    List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();
        //    InvItemDA itemDa = new InvItemDA();
        //    //itemInfo = itemDa.GetItemDetailsForAutoSearch(searchTerm, costCenterId, ConstantHelper.CustomerSupplierAutoSearch.SupplierItem.ToString(), categoryId, 0);
        //    itemInfo = itemDa.GetInvItemByCostCenterNCategoryIdForAutoSearch("", costCenterId, categoryId);

        //    return itemInfo;
        //}
    }
}