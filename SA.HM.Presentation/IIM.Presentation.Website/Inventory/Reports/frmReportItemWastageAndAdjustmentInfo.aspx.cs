using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using HotelManagement.Data.SalesManagment;
using HotelManagement.Entity.SalesManagment;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HotelManagement;
using System.IO;
using HotelManagement.Data.Restaurant;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.Inventory;
using System.Web.Services;
using System.Collections;

namespace HotelManagement.Presentation.Website.Inventory.Reports
{
    public partial class frmReportItemWastageAndAdjustmentInfo : BasePage
    {
        protected int _IsReportPanelEnable = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                LoadLocation();
                LoadCategory();
                LoadInvTransactionMode();
                LoadCommonDropDownHiddenField();
                LoadItem();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;
            DateTime dateTime = DateTime.Now;
            HMCommonDA hmCommonDA = new HMCommonDA();
            string startDate = string.Empty;
            string endDate = string.Empty;
            string costcenterIdList = string.Empty;
            string serviceIdList = string.Empty, serviceNameList = string.Empty;

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

            DateTime ReportDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            //if (!string.IsNullOrEmpty(hfCostcenterId.Value))
            //{
            //    costcenterIdList = hfCostcenterId.Value;
            //}
            //else
            //{
            //    CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            //    List<CostCentreTabBO> List = costCentreTabDA.GetAllRestaurantTypeCostCentreTabInfo();
            //    foreach (CostCentreTabBO row in List)
            //    {
            //        if (!string.IsNullOrEmpty(costcenterIdList))
            //            costcenterIdList += ", " + row.CostCenterId;
            //        else costcenterIdList += row.CostCenterId;
            //    }
            //}

            int categoryId = Convert.ToInt32(this.ddlCategory.SelectedValue);
            int itemId = !string.IsNullOrWhiteSpace(txtHiddenItemId.Value) ? Convert.ToInt32(txtHiddenItemId.Value) : -1;
            //string referNo = !string.IsNullOrWhiteSpace(this.txtReferenceNo.Text) ? this.txtReferenceNo.Text : "All";
            //string tranType = this.ddlPaymentType.SelectedValue;

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            string reportType = "";

            if (this.ddlReportType.SelectedValue == "Adjustment")
            {
                reportType = "Item Stock Adjustment";
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptStockAdjustmentInformation.rdlc");
            }
            else if (this.ddlReportType.SelectedValue == "Wastage")
            {
                reportType = "Item Wastage";
                reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptItemWastageInformation.rdlc");
            }

            string reportName = "" + reportType + " Information";

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
            paramReport.Add(new ReportParameter("FromDate", startDate));
            paramReport.Add(new ReportParameter("ToDate", endDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("ReportName", reportName));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));

            string reportDateDuration = "Date From: " + this.txtFromDate.Text + " To: " + this.txtToDate.Text;

            paramReport.Add(new ReportParameter("ReportDateDuration", reportDateDuration));

            rvTransaction.LocalReport.SetParameters(paramReport);

            List<ItemStockAdjustmentDetailsBO> stockAdjustmentDetails = new List<ItemStockAdjustmentDetailsBO>();
            InvItemDA itemDA = new InvItemDA();
            string searchCriteria = string.Empty;
            string prmReportType = this.ddlReportType.SelectedValue.ToString();
            string prmLocation = this.ddlLocation.SelectedValue.ToString();
            string prmTransactionType = this.ddlInvTransactionMode.SelectedValue.ToString();
            searchCriteria = GenarateWhereConditionstring(prmReportType, prmTransactionType, FromDate, ToDate, prmLocation, itemId);
            if (this.ddlReportType.SelectedValue == "Adjustment")
            {
                stockAdjustmentDetails = itemDA.GetStockAdjustmentInformation(searchCriteria);
            }
            else
            {
                stockAdjustmentDetails = itemDA.GetItemWastageInformation(searchCriteria);
            }

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], stockAdjustmentDetails));

            rvTransaction.LocalReport.DisplayName = "" + reportType + " Information";
            rvTransaction.LocalReport.Refresh();

        }
        //************************ User Defined Function ********************//
        private void LoadInvTransactionMode()
        {
            InvItemDA entityDA = new InvItemDA();
            var List = entityDA.GetInvTransactionMode();

            this.ddlInvTransactionMode.DataSource = List;
            this.ddlInvTransactionMode.DataTextField = "HeadName";
            this.ddlInvTransactionMode.DataValueField = "TModeId";
            this.ddlInvTransactionMode.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlInvTransactionMode.Items.Insert(0, item);
        }
        public static string GetHTMLCostcenterGridView(List<CostCentreTabBO> List)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th align='center' scope='col' style='width:80px;' >Select</th><th align='left' scope='col'>Cost Center</th></tr>";
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

                strTable += "<td align='center' style='width:30px'>";
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.CostCenterId + "' name='" + dr.CostCenter + "' value='" + dr.CostCenterId + "' >";

                strTable += "</td><td align='left' style='width:150px'>" + dr.CostCenter + "</td></tr>";

            }

            strTable += "</div> </td> </tr> </table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            strTable += "<div style='margin-top:12px;'>";
            strTable += "     <button type='button' onClick='javascript:return GetCheckedCostcenter()' id='btnAddCostcenterId' class='btn btn-primary'> OK</button>";
            strTable += "     <button type='button' onclick='javascript:return CloseCostcenterDialog()' id='btnCancelCostcenterId' class='btn btn-primary'> Cancel</button>";
            strTable += "</div>";
            return strTable;
        }
        public void LoadLocation()
        {
            InvLocationDA locationDa = new InvLocationDA();
            List<InvLocationBO> location = new List<InvLocationBO>();
            location = locationDa.GetInvLocationInfo();

            this.ddlLocation.DataSource = location;
            this.ddlLocation.DataTextField = "Name";
            this.ddlLocation.DataValueField = "LocationId";
            this.ddlLocation.DataBind();

            System.Web.UI.WebControls.ListItem itemNodeId = new System.Web.UI.WebControls.ListItem();
            itemNodeId.Value = "0";
            itemNodeId.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlLocation.Items.Insert(0, itemNodeId);
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

        public string GenarateWhereConditionstring(string reportType, string transactionType, DateTime FromDate, DateTime ToDate, string location, int itemId)
        {
            string Where = string.Empty;
            if (reportType == "Wastage")
            {
                if (transactionType != "0")
                {
                    if (!string.IsNullOrEmpty(Where))
                    {
                        Where += "  iisad.TModeId = '" + transactionType + "'";
                    }
                    else
                    {
                        Where += "  iisad.TModeId = '" + transactionType + "'";
                    }
                }
            }

            if (location != "0")
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += "  AND iisad.LocationId = '" + location + "'";
                }
                else
                {
                    Where += "  iisad.LocationId = '" + location + "'";
                }
            }

            if (itemId > 0)
            {
                Where += "  iisad.ItemId = '" + itemId + "'";
            }

            if (!string.IsNullOrEmpty(FromDate.ToString()) && !string.IsNullOrEmpty(ToDate.ToString()))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    if (reportType == "Adjustment")
                    {
                        Where += " AND ( dbo.FnDate(iisa.AdjustmentDate) >= dbo.FnDate('" + FromDate.ToString("yyyy-MM-dd") + "')  AND dbo.FnDate(iisa.AdjustmentDate) <= dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "') )";
                    }
                    else
                    {
                        Where += " AND ( dbo.FnDate(iisa.StockVarianceDate) >= dbo.FnDate('" + FromDate.ToString("yyyy-MM-dd") + "')  AND dbo.FnDate(iisa.StockVarianceDate) <= dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "') )";
                    }
                }
                else
                {
                    if (reportType == "Adjustment")
                    {
                        Where += " ( dbo.FnDate(iisa.AdjustmentDate) >= dbo.FnDate('" + FromDate.ToString("yyyy-MM-dd") + "')  AND dbo.FnDate(iisa.AdjustmentDate) <= dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "') )";
                    }
                    else
                    {
                        Where += " ( dbo.FnDate(iisa.StockVarianceDate) >= dbo.FnDate('" + FromDate.ToString("yyyy-MM-dd") + "')  AND dbo.FnDate(iisa.StockVarianceDate) <= dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "') )";
                    }
                }
            }


            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = "  WHERE " + Where + " AND iisa.ApprovedStatus = 'Approved'";
            }
            else
            {
                Where = "  WHERE " + Where + " iisa.ApprovedStatus = 'Approved'";
            }
            return Where;
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static List<InvItemBO> GetServiceByCriteria(int CategoryId)
        {
            InvItemDA productDA = new InvItemDA();
            List<InvItemBO> productList = new List<InvItemBO>();
            productList = productDA.GetInvItemInfoByCategory(0, CategoryId);

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