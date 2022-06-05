using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Data.SalesAndMarketing;
using System.Web.Services;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using Newtonsoft.Json;


namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class frmCity : System.Web.UI.Page
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
        protected void gvGuestHouseService_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.CheckObjectPermission();
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
            int CityId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(CityId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("CommonCity", "CityId", CityId);
                if (status)
                {                    
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                }

                this.SetTab("SearchTab");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtCityName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "City Name.", AlertType.Warning);
                if (!string.IsNullOrWhiteSpace(txtCityId.Value))
                {
                    this.btnSave.Text = "Update";
                }
                this.txtCityName.Focus();
            }
            else
            {
                CityBO bankBO = new CityBO();
                CityDA bankDA = new CityDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                bankBO.CityName = this.txtCityName.Text;
                bankBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;
                bankBO.CountryId = Convert.ToInt64(hfCountryId.Value);
                bankBO.StateId = Convert.ToInt64(hfStateId.Value);
                if (string.IsNullOrWhiteSpace(txtCityId.Value))
                {
                    int tmpCityId = 0;
                    bankBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = bankDA.SaveCityInfo(bankBO, out tmpCityId);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.City.ToString(), tmpCityId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.City));
                        this.Cancel();
                    }
                }
                else
                {
                    bankBO.CityId = Convert.ToInt32(txtCityId.Value);
                    bankBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = bankDA.UpdateCityInfo(bankBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.City.ToString(), bankBO.CityId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.City));
                        this.Cancel();
                    }
                }

                this.SetTab("EntryTab");
            }
        }
        //************************ User Defined Function ********************//
        private void Cancel()
        {
            this.hfCountryId.Value = "0";
            this.hfStateId.Value = "0";
            this.txtCityName.Text = string.Empty;
            this.txtCountry.Text = string.Empty;
            this.txtState.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.txtCityId.Value = string.Empty;
            this.txtCityName.Focus();
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmCity.ToString());
            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        public void FillForm(int EditId)
        {
            CityBO bankBO = new CityBO();
            CityDA bankDA = new CityDA();
            bankBO = bankDA.GetCityInfoById(EditId);
            ddlActiveStat.SelectedValue = (bankBO.ActiveStat == true ? 0 : 1).ToString();
            txtCityId.Value = bankBO.CityId.ToString();
            txtCityName.Text = bankBO.CityName.ToString();
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
        //************************ User Defined WebMethod ********************//
        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            HMUtility hmUtility = new HMUtility(); 
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("CommonCity", "CityId", sEmpId);
                if (status)
                {
                    //result = "success";
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.City.ToString(), sEmpId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.City));
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
                //lblMessage.Text = "Data Deleted Failed.";
                throw ex;
            }

            return rtninf;
        }
        [WebMethod]
        public static string LoadBreadCrumbsInformation()
        {
            string breadCrumbs = string.Empty;
            breadCrumbs = "<span class='divider'>/</span><a href='/SalesAndMarketing/frmCity.aspx'>City</a><span class='divider'></span>";
            return breadCrumbs;
        }
        [WebMethod]
        public static GridViewDataNPaging<CityBO, GridPaging> SearchCityAndLoadGridInformation(string cityName, Boolean activeStat, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<CityBO, GridPaging> myGridData = new GridViewDataNPaging<CityBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            CityDA bankDA = new CityDA();
            List<CityBO> bankInfoList = new List<CityBO>();
            bankInfoList = bankDA.GetCityInformationBySearchCriteriaForPaging(cityName, activeStat, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<CityBO> distinctItems = new List<CityBO>();
            distinctItems = bankInfoList.GroupBy(test => test.CityName).Select(group => group.First()).ToList();
            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static CityBO LoadDetailInformation(int bankId)
        {

            CityDA bankDA = new CityDA();
            return bankDA.GetCityInfoById(bankId);

        }
        [WebMethod]
        public static List<CountriesBO> LoadCountryForAutoSearch(string searchString)
        {

            HMCommonDA commonDA = new HMCommonDA();
            List<CountriesBO> countryList = commonDA.GetCountriesBySearch(searchString);

            return countryList;
        }
        [WebMethod]
        public static List<StateBO> LoadStateForAutoSearchByCountry(string searchString, int CountryId)
        {

            HMCommonDA commonDA = new HMCommonDA();
            List<StateBO> countryList = commonDA.GetStateForAutoSearchByCountry(searchString, CountryId);

            return countryList;
        }
    }
}