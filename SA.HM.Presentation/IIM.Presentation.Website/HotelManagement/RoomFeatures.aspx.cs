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
    public partial class RoomFeatures : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        HMCommonDA hmCommonDA = new HMCommonDA();
        
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                CheckObjectPermission();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFeatures.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Any Features", AlertType.Warning);
                txtFeatures.Focus();
            }
            else
            {
                RoomFeaturesBO roomFtBO = new RoomFeaturesBO();
                RoomFeaturesDA roomFtDA = new RoomFeaturesDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                roomFtBO.Features = txtFeatures.Text;
                roomFtBO.Description = txtDescription.Text;
                roomFtBO.ActiveStatus = ddlActiveStatus.SelectedValue == "1" ? true : false;

                long tmpFeaturesId = 0;

                if (string.IsNullOrWhiteSpace(hfFeaturesId.Value))
                {
                   
                    bool status;

                    roomFtBO.CreatedBy = userInformationBO.UserInfoId;

                    bool isFeaturesExist = hmCommonDA.DuplicateDataCountDynamicaly("HotelRoomFeatures", "Features", roomFtBO.Features) > 0;

                    if (isFeaturesExist)
                    {
                        CommonHelper.AlertInfo(innboardMessage, string.Format("Your entered {0} {1} is already exist.", lblFeatures.Text, roomFtBO.Features), AlertType.Warning);
                        Cancel();
                        return;
                    }
                    else
                    {
                        status = roomFtDA.SaveRoomFeatures(roomFtBO, out tmpFeaturesId);
                    }
                    
                    if (status)
                    {
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomFeatures.ToString(), tmpFeaturesId, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomFeatures));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Cancel();
                    }
                }
                else
                {
                    bool status;
                    roomFtBO.Id = Convert.ToInt64(hfFeaturesId.Value);
                    roomFtBO.LastModifiedBy = userInformationBO.UserInfoId;

                    bool isFeaturesExist = hmCommonDA.DuplicateCheckDynamicaly("HotelRoomFeatures", "Features", roomFtBO.Features, 1, "Id", roomFtBO.Id.ToString()) > 0;

                    if (isFeaturesExist)
                    {
                        CommonHelper.AlertInfo(innboardMessage, string.Format("Your entered {0} {1} is already exist.", lblFeatures.Text, roomFtBO.Features), AlertType.Warning);
                        Cancel();
                        return;
                    }
                    else
                    {
                        status = roomFtDA.UpdateRoomFeatures(roomFtBO);
                    }
                    if (status)
                    {
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.Bank.ToString(), tmpFeaturesId, ProjectModuleEnum.ProjectModule.InventoryManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomFeatures));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Cancel();
                    }
                }
                this.SetTab("EntryTab");
            }
        }

        //************************ User Defined Function ********************//

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

        private void Cancel()
        {
            txtFeatures.Text = string.Empty;
            txtDescription.Text = string.Empty;
            ddlActiveStatus.SelectedIndex = 0;
            hfFeaturesId.Value = string.Empty;
            txtFeatures.Focus();
        }

        public void FillForm(long EditId)
        {
            RoomFeaturesBO roomFtBO = new RoomFeaturesBO();
            RoomFeaturesDA roomFtDA = new RoomFeaturesDA();

            roomFtBO = roomFtDA.GetRoomFeaturesById(EditId);

            ddlActiveStatus.SelectedValue = (roomFtBO.ActiveStatus == true ? 1 : 0).ToString();
            txtFeatures.Text = roomFtBO.Features.ToString();
            txtDescription.Text = roomFtBO.Description.ToString();
            hfFeaturesId.Value = roomFtBO.Id.ToString();

        }

        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmBank.ToString());

            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;

            btnSave.Visible = isSavePermission;

            if (isSavePermission)
            {
                hfSavePermission.Value = "1";
            }
            else
            {
                hfSavePermission.Value = "0";
            }

            if (isDeletePermission)
            {
                hfDeletePermission.Value = "1";
            }
            else
            {
                hfDeletePermission.Value = "0";
            }

        }

        [WebMethod]
        public static RoomFeaturesBO LoadEdiInfo(long id)
        {
            RoomFeaturesDA roomDA = new RoomFeaturesDA();
            return roomDA.GetRoomFeaturesById(id);
        }
        //public static BankBO LoadDetailInformation(int bankId)
        //{
        //    BankDA bankDA = new BankDA();
        //    return bankDA.GetBankInfoById(bankId);
        //}

        [WebMethod]
        public static GridViewDataNPaging<RoomFeaturesBO, GridPaging> SearchFeaturesAndLoadGridInformation(string features, bool activeStat, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<RoomFeaturesBO, GridPaging> myGridData = new GridViewDataNPaging<RoomFeaturesBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            RoomFeaturesDA roomFtDA = new RoomFeaturesDA();
            List<RoomFeaturesBO> roomFtList = new List<RoomFeaturesBO>();

            roomFtList = roomFtDA.GetFeaturesInfoBySearchCriteriaForPaging(features, activeStat, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            //List<RoomFeaturesBO> distinctItems = new List<RoomFeaturesBO>();
            //distinctItems = roomFtList.GroupBy(test => test.Features).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(roomFtList, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static ReturnInfo DeleteData(long deleteId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();

            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                bool status = hmCommonDA.DeleteInfoById("HotelRoomFeatures", "Id", deleteId);

                if (status)
                {
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.RoomFeatures.ToString(), deleteId, ProjectModuleEnum.ProjectModule.CommonConfiguration.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomFeatures));
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

    }
}