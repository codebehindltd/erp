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
using System.Globalization;

namespace HotelManagement.Presentation.Website.Inventory.Reports
{
    public partial class frmReportXYZAnalysis : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int _IsReportPanelEnable = -1;
        protected Boolean isRestaurantReportRestrictionForAllUser = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            rvTransaction.LocalReport.EnableExternalImages = true;
            if (!IsPostBack)
            {
                IsRestaurantReportRestrictionForAllUser();
                LoadCostCenter();
                LoadCategory();
            }
        }
        private void IsRestaurantReportRestrictionForAllUser()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO commonSetupBOIsRestaurantBillByCashier = new HMCommonSetupBO();
            commonSetupBOIsRestaurantBillByCashier = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantBillByCashier", "IsRestaurantBillByCashier");
            if (commonSetupBOIsRestaurantBillByCashier != null)
            {
                if (commonSetupBOIsRestaurantBillByCashier.SetupId > 0)
                {
                    if (commonSetupBOIsRestaurantBillByCashier.SetupValue == "1")
                    {
                        HMCommonSetupBO commonSetupBORescitionForAllUsers = new HMCommonSetupBO();

                        commonSetupBORescitionForAllUsers = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantReportRestrictionForAllUser", "IsRestaurantReportRestrictionForAllUser");

                        if (commonSetupBORescitionForAllUsers != null)
                        {
                            if (commonSetupBORescitionForAllUsers.SetupId > 0)
                            {
                                if (commonSetupBORescitionForAllUsers.SetupValue == "1")
                                {
                                    isRestaurantReportRestrictionForAllUser = true;
                                }
                                else
                                {
                                    isRestaurantReportRestrictionForAllUser = false;
                                }
                            }
                            else
                            {
                                isRestaurantReportRestrictionForAllUser = false;
                            }
                        }
                        else
                        {
                            isRestaurantReportRestrictionForAllUser = false;
                        }
                    }
                }
            }
            else
            {
                isRestaurantReportRestrictionForAllUser = false;
            }
        }
        private void LoadCostCenter()
        {
            Boolean IsAdminUser = false;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                IsAdminUser = true;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 6).Count() > 0)
                    {
                        IsAdminUser = true;
                    }
                }
            }
            #endregion

            CostCentreTabDA costCentreTabDA = new CostCentreTabDA();
            List<CostCentreTabBO> entityListBO = new List<CostCentreTabBO>();

            if (IsAdminUser)
            {
                entityListBO = costCentreTabDA.GetAllRestaurantTypeCostCentreTabInfo().Where(x => x.IsRestaurant == true).ToList();
            }
            else
            {
                if (!isRestaurantReportRestrictionForAllUser)
                {
                    entityListBO = costCentreTabDA.GetAllRestaurantTypeCostCentreTabInfo().Where(x => x.IsRestaurant == true).ToList();
                }
                else
                {
                    entityListBO = costCentreTabDA.GetRestaurantTypeCostCentreTabInfo(userInformationBO.UserId, userInformationBO.UserInfoId, 0).Where(x => x.IsRestaurant == true).ToList();
                }
            }
            string grid = GetHTMLCostcenterGridView(entityListBO);
            ltCostcenter.Text = grid;
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

            return strTable;
        }

        private void LoadCategory()
        {
            List<InvCategoryBO> List = new List<InvCategoryBO>();
            InvCategoryDA da = new InvCategoryDA();
            //List = da.GetAllInvItemCatagoryInfoByServiceType("Product");
            List = da.GetInvCatagoryInfo();
            this.ddlCategory.DataSource = List;
            this.ddlCategory.DataTextField = "MatrixInfo";
            this.ddlCategory.DataValueField = "CategoryId";
            this.ddlCategory.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlCategory.Items.Insert(0, item);
        }

        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            _IsReportPanelEnable = 1;
            HMCommonDA hmCommonDA = new HMCommonDA();

            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            //   31/08/2021

            string strFromDate = this.txtFromDate.Text;
            string strToDate = this.txtToDate.Text;
            string strCostCenterIds = this.hfCostcenterId.Value;
            string strCostCenterValues = this.hfCostcenterListInfo.Value;
            string category = this.ddlCategory.SelectedValue;
            string strCategory = category == "0" ? "" : this.ddlCategory.SelectedItem.Text;

            CompanyDA companyDA = new CompanyDA();
            
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            if (files != null)
            {
                if (files.Count > 0)
                {
                    //dateString = "10-12-2015";
                    // Output: 10/22/2015 12:00:00 AM  
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DateTime dtFromDate = DateTime.MinValue;
                    DateTime dtToDate = DateTime.MinValue;
                    if (!string.IsNullOrEmpty(strFromDate))
                    {
                        dtFromDate = DateTime.ParseExact(strFromDate, new string[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy" }, provider, DateTimeStyles.None);
                    }
                    
                    if (!string.IsNullOrEmpty(strToDate))
                    {
                        dtToDate = DateTime.ParseExact(strToDate, new string[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy" }, provider, DateTimeStyles.None);
                    }
                    
                    int categoryId = 0;
                    if(int.TryParse(category, out categoryId))
                    {

                    }

                    //DateTime dtFromDate = DateTime.Fro
                    List<ReportParameter> reportParam = new List<ReportParameter>();
                    reportParam.Add(new ReportParameter("CompanyName", files[0].CompanyName));
                    reportParam.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));
                    reportParam.Add(new ReportParameter("FromDate", strFromDate));
                    reportParam.Add(new ReportParameter("ToDate", strToDate));
                    reportParam.Add(new ReportParameter("CostCenter", strCostCenterValues));
                    reportParam.Add(new ReportParameter("Category", strCategory));

                    var reportPath = Server.MapPath(@"~/Inventory/Reports/Rdlc/rptXYZAnalysis.rdlc");
                    if (!File.Exists(reportPath))
                        return;

                    rvTransaction.LocalReport.ReportPath = reportPath;
                    AllInventoryReportDA allInvReportda = new AllInventoryReportDA();
                    List<XYZAnalysisBO> xyzList = new List<XYZAnalysisBO>();
                    xyzList = allInvReportda.GetXyzAnalysis(dtFromDate, dtToDate, strCostCenterIds, categoryId);

                    rvTransaction.LocalReport.SetParameters(reportParam);

                    var reportDataSet = rvTransaction.LocalReport.GetDataSourceNames();
                    rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataSet[0], xyzList));

                    rvTransaction.LocalReport.DisplayName = "XYZ Analysis";

                    rvTransaction.LocalReport.Refresh();
                    //frmPrint.Attributes["src"] = "";
                }
            }
        }
        
    }
}