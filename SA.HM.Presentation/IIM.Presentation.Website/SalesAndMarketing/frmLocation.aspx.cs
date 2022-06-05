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
    public partial class frmLocation : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                
            }
        }
        protected void gvGuestHouseService_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {            
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
            int LocationId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(LocationId);
                this.btnSave.Text = "Update";
                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("CommonLocation", "LocationId", LocationId);
                if (status)
                {                    
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                }

                this.SetTab("SearchTab");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtLocationName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Location Name.", AlertType.Warning);
                if (!string.IsNullOrWhiteSpace(txtLocationId.Value))
                {
                    this.btnSave.Text = "Update";
                }
                this.txtLocationName.Focus();
            }
            else
            {
                LocationBO bankBO = new LocationBO();
                LocationDA bankDA = new LocationDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                bankBO.LocationName = this.txtLocationName.Text;
                bankBO.CityId = Convert.ToInt32(this.hfCityId.Value);
                bankBO.CountryId = Convert.ToInt32(this.hfCountryId.Value);
                bankBO.StateId = Convert.ToInt32(this.hfStateId.Value);
                bankBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

                if (string.IsNullOrWhiteSpace(txtLocationId.Value))
                {
                    int tmpLocationId = 0;
                    bankBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = bankDA.SaveLocationInfo(bankBO, out tmpLocationId);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.Location.ToString(), tmpLocationId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Location));
                        this.Cancel();
                    }
                }
                else
                {
                    bankBO.LocationId = Convert.ToInt32(txtLocationId.Value);
                    bankBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = bankDA.UpdateLocationInfo(bankBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.Location.ToString(), bankBO.LocationId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Location));
                        this.Cancel();
                    }
                }

                this.SetTab("EntryTab");
            }
        }
        //************************ User Defined Function ********************//
        private void Cancel()
        {
            this.txtLocationName.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.hfCityId.Value = "0";
            this.btnSave.Text = "Save";
            this.txtLocationId.Value = string.Empty;
            this.txtLocationName.Focus();
        }
        
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmLocation.ToString());
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
            LocationBO bankBO = new LocationBO();
            LocationDA bankDA = new LocationDA();
            bankBO = bankDA.GetLocationInfoById(EditId);
            ddlActiveStat.SelectedValue = (bankBO.ActiveStat == true ? 0 : 1).ToString();
            txtLocationId.Value = bankBO.LocationId.ToString();
            txtLocationName.Text = bankBO.LocationName.ToString();
            hfCityId.Value = bankBO.CityId.ToString();
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
                Boolean status = hmCommonDA.DeleteInfoById("CommonLocation", "LocationId", sEmpId);
                if (status)
                {
                    //result = "success";
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.Location.ToString(), sEmpId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Location));
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
            breadCrumbs = "<span class='divider'>/</span><a href='/SalesAndMarketing/frmLocation.aspx'>Location</a><span class='divider'></span>";
            return breadCrumbs;
        }
        [WebMethod]
        public static GridViewDataNPaging<LocationBO, GridPaging> SearchLocationAndLoadGridInformation(string locationName, Boolean activeStat, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<LocationBO, GridPaging> myGridData = new GridViewDataNPaging<LocationBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            LocationDA bankDA = new LocationDA();
            List<LocationBO> bankInfoList = new List<LocationBO>();
            bankInfoList = bankDA.GetLocationInformationBySearchCriteriaForPaging(locationName, activeStat, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<LocationBO> distinctItems = new List<LocationBO>();
            distinctItems = bankInfoList.GroupBy(test => test.LocationName).Select(group => group.First()).ToList();


            //myGridData.GridPagingProcessing(guestInfoList, totalRecords);
            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static LocationBO LoadDetailInformation(int bankId)
        {

            LocationDA bankDA = new LocationDA();
            return bankDA.GetLocationInfoById(bankId);

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
        [WebMethod]
        public static List<CityBO> LoadCityForAutoSearchByState(string searchString, Int64 CountryId, string StateString, Int64 StateId)
        {
            if (string.IsNullOrEmpty(StateString))
            {
                StateId = 0;
            }

            CityDA commonDA = new CityDA();
            List<CityBO> countryList = commonDA.GetCityInfoBySearchAutoSearchByState(searchString, CountryId, StateId);

            return countryList;
        }
    }
}