using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using System.Web.Services;
using System.Collections;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HotelManagement;
using System.Linq;

namespace HotelManagement.Presentation.Website.Inventory.Reports
{
    public partial class frmReportInvItemInfo : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int _IsReportPanelEnable = -1;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                LoadCategory();
                LoadItemClassificationInfo();
                LoadCommonDropDownHiddenField();
                LoadItem();
                LoadCostCenter();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            if (ddlCostCenterConfig.SelectedValue == "2")
            {
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptInvItemInfo.rdlc");
            }
            else
            {
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptItemInformation.rdlc");
            }
                

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();
            List<ReportParameter> paramReport = new List<ReportParameter>();

            if (files[0].CompanyId > 0)
            {
                paramReport.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
                paramReport.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
                }
                else
                {
                    paramReport.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
                }
            }

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            rvTransaction.LocalReport.SetParameters(paramReport);

            InvItemDA itemDa = new InvItemDA();
            List<InvItemInformationReportBO> itemInfo = new List<InvItemInformationReportBO>();

            int categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
            int itemId = !string.IsNullOrWhiteSpace(txtHiddenItemId.Value) ? Convert.ToInt32(txtHiddenItemId.Value) : 0;
            string adjustmentFrequency = this.ddlAdjustmentFrequency.SelectedValue;
            int classificationId = Convert.ToInt32(this.ddlClassification.SelectedValue);
            if (ddlCostCenterConfig.SelectedValue == "2")
                itemInfo = itemDa.GetInventoryItemInformationWitoutCostCenter(categoryId, itemId, adjustmentFrequency, classificationId);
            else
                itemInfo = itemDa.GetInventoryItemInformation(categoryId, itemId, adjustmentFrequency, classificationId);

            if (hfCostcenterId.Value != "")
            {
                List<int> costCenterIdList = hfCostcenterId.Value.Split(',').Select(Int32.Parse).ToList();
                if (costCenterIdList.Count > 0)
                    itemInfo = itemInfo.Where(i => costCenterIdList.Contains(i.CostCenterId)).ToList();
            }
            
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], itemInfo));

            rvTransaction.LocalReport.DisplayName = "Inventory Item Information";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
        }
        private void LoadCommonDropDownHiddenField()
        {
            this.CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        }
        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            List = da.GetAllInvItemCatagoryInfoByServiceType("Product");
            this.ddlCategory.DataSource = List;
            this.ddlCategory.DataTextField = "MatrixInfo";
            this.ddlCategory.DataValueField = "CategoryId";
            this.ddlCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCategory.Items.Insert(0, item);
        }

        private void LoadItem()
        {
            List<InvItemBO> productList = new List<InvItemBO>();
            InvItemDA da = new InvItemDA();
            productList = da.GetInvItemInfo();
            ddlItem.DataSource = productList;
            ddlItem.DataTextField = "Name";
            ddlItem.DataValueField = "ItemId";
            ddlItem.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlItem.Items.Insert(0, item);
        }

        private void LoadItemClassificationInfo()
        {
            InvItemClassificationCostCenterMappingDA DA = new InvItemClassificationCostCenterMappingDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<ItemClassificationBO> fields = new List<ItemClassificationBO>();
            fields = DA.GetActiveItemClassificationInfo();
            ddlClassification.DataSource = fields;
            ddlClassification.DataTextField = "ClassificationName";
            ddlClassification.DataValueField = "ClassificationId";
            ddlClassification.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlClassification.Items.Insert(0, item);
        }

        private void LoadCostCenter()
        {
            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> list = new List<CostCentreTabBO>();
            list = costCentreTabDA.GetAllCostCentreTabInfo();

            string grid = GetHTMLCostCenterGridView(list);
            ltCostCenter.Text = grid;
        }
        public static string GetHTMLCostCenterGridView(List<CostCentreTabBO> List)
        {
            string strTable = "";
            
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation' ><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='text-align:center' scope='col' style='width:80px;' >Select</th><th align='left' scope='col'>Service</th></tr>";

            int counter = 0;
            foreach (CostCentreTabBO dr in List)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:White;'>";
                }

                strTable += "<td align='center'>";
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.CostCenterId + "' name='" + dr.CostCenter.Replace("'", "&#39;") + "' value='" + dr.CostCenterId + "' >";
                strTable += "</td><td align='left'>" + dr.CostCenter + "</td></tr>";
            }

            strTable += "</td> </tr> </table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }
            
            return strTable;
        }
        [WebMethod]
        public static List<InvItemBO> GetServiceByCriteria(int CategoryId)
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetInvItemInfoByCategoryId(0, CategoryId);

            return productList;
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int CategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            ArrayList list = new ArrayList();
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetInvItemInfoByCategoryId(0, CategoryId);

            ddlItem.DataSource = productList;
            ddlItem.DataTextField = "Name";
            ddlItem.DataValueField = "ItemId";
            ddlItem.DataBind();
            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlItem.Items.Insert(0, item);
        }
    }
}