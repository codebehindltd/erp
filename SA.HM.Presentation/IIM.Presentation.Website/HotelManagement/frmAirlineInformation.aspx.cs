using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmAirlineInformation : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();

        HMCommonDA hmCommonDA = new HMCommonDA();

        protected int isNewAddButtonEnable = 1;

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
                CheckPermission();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtAirlineName.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Vehicle Name.", AlertType.Warning);
                this.txtAirlineName.Focus();
                return;
            }
            else
            {
                AirlineBO airlineBO = new AirlineBO();
                AirlineDA airlineDA = new AirlineDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                airlineBO.AirlineName = this.txtAirlineName.Text;
                airlineBO.FlightNumber = this.txtFlightNumber.Text;
                if (!string.IsNullOrWhiteSpace(txtAirlineTime.Text))
                {
                    airlineBO.AirlineTime = Convert.ToDateTime(txtAirlineTime.Text);
                }
                else
                {
                    airlineBO.AirlineTime = null;
                }
                airlineBO.ActiveStat = ddlActiveStat.SelectedIndex == 0 ? true : false;

                if (string.IsNullOrWhiteSpace(txtAirlineId.Value))
                {
                    int tmpAirlineId = 0;
                    airlineBO.CreatedBy = userInformationBO.UserInfoId;
                    //Boolean isAirlineExist = hmCommonDA.DuplicateDataCountDynamicaly("HotelAirlineInformation", "AirlineName", airlineBO.AirlineName) > 0;
                    Boolean isAirlineExist = hmCommonDA.DuplicateCheckDynamicaly("HotelAirlineInformation", "AirlineName", airlineBO.AirlineName, 0, "AirlineId", airlineBO.AirlineId.ToString()) > 0;
                    if (isAirlineExist)
                    {
                        CommonHelper.AlertInfo(innboardMessage, string.Format("Your entered {0} {1} is already exist.", lblAirlineName.Text, airlineBO.AirlineName), AlertType.Warning);
                        this.Cancel();
                        return;
                    }
                    Boolean status = airlineDA.SaveAirlineInfo(airlineBO, out tmpAirlineId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.Airline.ToString(), tmpAirlineId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Airline));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        this.Cancel();
                    }
                }
                else
                {
                    airlineBO.AirlineId = Convert.ToInt32(txtAirlineId.Value);
                    airlineBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean isAirlineExist = hmCommonDA.DuplicateCheckDynamicaly("HotelAirlineInformation", "AirlineName", airlineBO.AirlineName, 1, "AirlineId", airlineBO.AirlineId.ToString()) > 0;
                    if (isAirlineExist)
                    {
                        CommonHelper.AlertInfo(innboardMessage, string.Format("Your entered {0} {1} is already exist.", lblSAirlineName.Text, airlineBO.AirlineName), AlertType.Warning);
                        this.Cancel();
                        return;
                    }
                    Boolean status = airlineDA.UpdateAirlineInfo(airlineBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.Airline.ToString(), airlineBO.AirlineId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Airline));
                        this.Cancel();
                    }
                }

                this.SetTab("EntryTab");
            }
        }

        //************************ User Defined Function ********************//
        private void Cancel()
        {
            this.txtAirlineName.Text = string.Empty;
            this.txtFlightNumber.Text = string.Empty;
            this.txtAirlineTime.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            this.txtAirlineId.Value = string.Empty;
            this.txtAirlineName.Focus();
        }
        private void CheckPermission()
        {
            btnSave.Visible = isSavePermission;

            if (isUpdatePermission)
            {
                IsAirlineUpdatePermission.Value = "1";
            }
            else
            {
                IsAirlineUpdatePermission.Value = "0";
            }

            if (isDeletePermission)
            {
                IsAirlineDeletePermission.Value = "1";
            }
            else
            {
                IsAirlineDeletePermission.Value = "0";
            }

        }
        public void FillForm(int EditId)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            AirlineBO airlineBO = new AirlineBO();
            AirlineDA airlineDA = new AirlineDA();
            airlineBO = airlineDA.GetAirlineInfoById(EditId);
            ddlActiveStat.SelectedValue = (airlineBO.ActiveStat == true ? 0 : 1).ToString();
            txtAirlineId.Value = airlineBO.AirlineId.ToString();
            txtAirlineName.Text = airlineBO.AirlineName.ToString();
            txtFlightNumber.Text = airlineBO.FlightNumber.ToString();
            if (airlineBO.AirlineTime != null)
            {
                this.txtAirlineTime.Text = Convert.ToDateTime(airlineBO.AirlineTime.ToString()).ToString(userInformationBO.TimeFormat);
            }
            else
            {
                this.txtAirlineTime.Text = string.Empty;
            }
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
        public static ReturnInfo DeleteData(int sAirlineId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("HotelAirlineInformation", "AirlineId", sAirlineId);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.Airline.ToString(), sAirlineId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.Airline));
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
            breadCrumbs = "<span class='divider'>/</span><a href='/HotelManagement/frmAirlineInformation.aspx'>Airline</a><span class='divider'></span>";
            return breadCrumbs;
        }

        [WebMethod]
        public static GridViewDataNPaging<AirlineBO, GridPaging> SearchBankAndLoadGridInformation(string airlineName, Boolean activeStat, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<AirlineBO, GridPaging> myGridData = new GridViewDataNPaging<AirlineBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            AirlineDA airlineDA = new AirlineDA();
            List<AirlineBO> airlineInfoList = new List<AirlineBO>();
            airlineInfoList = airlineDA.GetAirlineInformationBySearchCriteriaForPaging(airlineName, activeStat, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<AirlineBO> distinctItems = new List<AirlineBO>();
            distinctItems = airlineInfoList.GroupBy(test => test.AirlineName).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static AirlineBO LoadDetailInformation(int airlineId)
        {
            AirlineDA airlineDA = new AirlineDA();
            return airlineDA.GetAirlineInfoById(airlineId);
        }

    }
}