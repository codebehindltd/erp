using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using System.Web.Services;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Newtonsoft.Json;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;

namespace HotelManagement.Presentation.Website.POS
{
    public partial class frmDailySalesStatementConfiguration : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
            }
        }

        protected void gvGuestHouseService_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                imgUpdate.Visible = isSavePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void gvGuestHouseService_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int BankId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                //FillForm(BankId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("CommonBank", "BankId", BankId);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                }

                this.SetTab("SearchTab");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtPercentageAmount.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Percentage Amount.", AlertType.Warning);
                this.txtPercentageAmount.Focus();
            }
            else
            {
                RestaurantDailySalesStatementConfigurationBO salesStatementConfiguration = new RestaurantDailySalesStatementConfigurationBO();
                RestaurantConfigurationDA configurationDA = new RestaurantConfigurationDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                string startDate = string.Empty, endDate = string.Empty;
                int dailySalesStatementId = 0;

                if (string.IsNullOrWhiteSpace(txtStartDate.Text))
                {
                    startDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                }
                else
                {
                    startDate = txtStartDate.Text;
                }
                if (string.IsNullOrWhiteSpace(txtEndDate.Text))
                {
                    endDate = hmUtility.GetStringFromDateTime(DateTime.Now);
                }
                else
                {
                    endDate = txtEndDate.Text;
                }

                salesStatementConfiguration.DateFrom = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                salesStatementConfiguration.DateTo = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                salesStatementConfiguration.PercentageAmount = Convert.ToDecimal(txtPercentageAmount.Text);
                salesStatementConfiguration.AmountInPercentage = (salesStatementConfiguration.PercentageAmount / Convert.ToDecimal(100.00));

                if (string.IsNullOrWhiteSpace(hfDailySalesStatementId.Value))
                {
                    salesStatementConfiguration.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = configurationDA.SaveDailySalesStatementConfiguration(salesStatementConfiguration, out dailySalesStatementId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.Bank.ToString(), dailySalesStatementId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantDailySalesStatementConfiguration));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        this.Cancel();
                    }
                }
                else
                {
                    salesStatementConfiguration.DailySalesStatementId = Convert.ToInt32(hfDailySalesStatementId.Value);
                    salesStatementConfiguration.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = configurationDA.UpdateDailySalesStatementConfiguration(salesStatementConfiguration);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.Bank.ToString(), salesStatementConfiguration.DailySalesStatementId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RestaurantDailySalesStatementConfiguration));
                        this.Cancel();
                    }
                }

                this.SetTab("EntryTab");
            }
        }
        //************************ User Defined Function ********************//
        private void Cancel()
        {
            txtEndDate.Text = "";
            txtStartDate.Text = "";
            txtPercentageAmount.Text = "";
        }
        
        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("CommonBank", "BankId", sEmpId);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.Bank.ToString(), sEmpId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Bank));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rtninf;
        }
        [WebMethod]
        public static string LoadBreadCrumbsInformation()
        {
            string breadCrumbs = string.Empty;
            breadCrumbs = "<span class='divider'>/</span><a href='/HMCommon/frmBank.aspx'>Bank</a><span class='divider'></span>";
            return breadCrumbs;
        }

        [WebMethod]
        public static GridViewDataNPaging<RestaurantDailySalesStatementConfigurationBO, GridPaging> SearchtDailySalesStatementConfiguration(string dateFrom, string dateTo, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            List<RestaurantDailySalesStatementConfigurationBO> dailySalesStatementConfiguration = new List<RestaurantDailySalesStatementConfigurationBO>();
            RestaurantConfigurationDA configurationDA = new RestaurantConfigurationDA();

            DateTime? FromDate = null, ToDate = null;
            int totalRecords = 0;

            if (!string.IsNullOrEmpty(dateFrom))
                FromDate = Convert.ToDateTime(dateFrom);

            if (!string.IsNullOrEmpty(dateTo))
                ToDate = Convert.ToDateTime(dateTo);

            GridViewDataNPaging<RestaurantDailySalesStatementConfigurationBO, GridPaging> myGridData = new GridViewDataNPaging<RestaurantDailySalesStatementConfigurationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            dailySalesStatementConfiguration = configurationDA.GetDailySalesStatementConfigurationBySearchCriteria(FromDate, ToDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(dailySalesStatementConfiguration, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static RestaurantDailySalesStatementConfigurationBO FillForm(int dailySalesStatementId)
        {
            RestaurantDailySalesStatementConfigurationBO dailySalesStatementConfiguration = new RestaurantDailySalesStatementConfigurationBO();
            RestaurantConfigurationDA configurationDA = new RestaurantConfigurationDA();
            dailySalesStatementConfiguration = configurationDA.GetDailySalesStatementConfigurationById(dailySalesStatementId);

            return dailySalesStatementConfiguration;
        }
    }
}