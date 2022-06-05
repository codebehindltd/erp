using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data;
using HotelManagement.Entity;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using System.Web.Services;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HouseKeeping;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmRoomNumber : BasePage
    {
        HiddenField innboardMessage;
        protected int isToDateEnable = -1;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                this.LoadRoomType();
                this.LoadRoomStatus();
                this.LoadHKRoomStatus();
                CheckObjectPermission();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadGridView();
        }
        protected void gvRoomNumber_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvRoomNumber.PageIndex = e.NewPageIndex;
            this.LoadGridView();
        }
        protected void gvRoomNumber_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int floorId = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "CmdEdit")
            {
                FillForm(floorId);

                this.SetTab("EntryTab");
            }
            else if (e.CommandName == "CmdDelete")
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                try
                {
                    Boolean status = hmCommonDA.DeleteInfoById("HotelRoomNumber", "RoomId", floorId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.RoomNumber.ToString(), floorId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomNumber));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                    }
                }
                catch (Exception ex)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
                LoadGridView();
                this.SetTab("SearchTab");
            }
        }
        protected void gvRoomNumber_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                // imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                // imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgUpdate.Visible = isUpdatePermission;
                imgDelete.Visible = isDeletePermission;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckingMaximumRoomPermission())
                {
                    return;
                }

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                if (ddlRoomTypeId.SelectedValue == "0")
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Room Type.", AlertType.Warning);
                    ddlRoomTypeId.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(this.txtRoomNumber.Text))
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Room number.", AlertType.Warning);
                    txtRoomNumber.Focus();
                    return;
                }
                else if (!string.IsNullOrWhiteSpace(this.txtRoomNumber.Text))
                {
                    string str = txtRoomNumber.Text.Trim();
                    double Num;

                    bool isNum = double.TryParse(str, out Num);

                    if (isNum)
                    {

                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "numeric numbers only to Room Number.", AlertType.Warning);
                        return;
                    }
                }

                if (this.ddlActiveStat.SelectedValue != "1")
                {
                    if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
                    {
                        this.isToDateEnable = 1;
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "To Date.", AlertType.Warning);
                        txtToDate.Focus();
                        return;
                    }
                    else if (hmUtility.GetDateTimeFromString(this.txtToDate.Text, userInformationBO.ServerDateFormat).Date < DateTime.Now.Date)
                    {
                        this.isToDateEnable = 1;
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Valid To Date.", AlertType.Warning);
                        txtToDate.Focus();
                        return;
                    }
                    else if (string.IsNullOrWhiteSpace(this.txtRemarks.Text))
                    {
                        isToDateEnable = 1;
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Remarks.", AlertType.Warning);
                        txtRemarks.Focus();
                        return;
                    }
                }
                RoomNumberBO roomNumberBO = new RoomNumberBO();
                RoomNumberDA roomNumberDA = new RoomNumberDA();

                roomNumberBO.RoomTypeId = Convert.ToInt32(this.ddlRoomTypeId.SelectedValue);
                roomNumberBO.RoomNumber = this.txtRoomNumber.Text;
                roomNumberBO.RoomName = this.txtRoomName.Text;
                roomNumberBO.StatusId = Convert.ToInt32(this.ddlActiveStat.SelectedValue);
                roomNumberBO.HKRoomStatusId = Convert.ToInt32(this.ddlHKActiveStat.SelectedValue);
                roomNumberBO.IsSmokingRoom = ddlIsSmokingRoom.SelectedValue == "0" ? false : true;
                DateTime dateTime = DateTime.Now;
                if (this.ddlActiveStat.SelectedValue == "1")
                {
                    roomNumberBO.ToDate = dateTime;
                    roomNumberBO.Remarks = string.Empty;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(txtRoomId.Value))
                    {
                        RoomNumberDA numberDA = new RoomNumberDA();
                        RoomNumberBO numberBO = numberDA.GetRoomNumberInfoById(Int32.Parse(txtRoomId.Value));

                        if (numberBO.StatusId != roomNumberBO.StatusId)
                        {
                            List<RoomNumberBO> list = new List<RoomNumberBO>();
                            //roomNumberBO.ToDate = hmUtility.GetDateTimeFromString(this.txtToDate.Text);
                            roomNumberBO.ToDate = !string.IsNullOrWhiteSpace(this.txtToDate.Text) ? hmUtility.GetDateTimeFromString(this.txtToDate.Text, userInformationBO.ServerDateFormat) : DateTime.Now;
                            list = roomNumberDA.GetAvailableRoomNumberInformation(roomNumberBO.RoomTypeId, 0, dateTime, roomNumberBO.ToDate, 0);

                            List<RoomNumberBO> list2 = list.Where(p => p.RoomNumber == roomNumberBO.RoomNumber).ToList();
                            if (list2 != null)
                            {
                                if (list2.Count > 0)
                                {
                                    roomNumberBO.Remarks = this.txtRemarks.Text;
                                }
                                else
                                {
                                    CommonHelper.AlertInfo(innboardMessage, "This Room Number Already Reserved.", AlertType.Warning);
                                    txtRoomNumber.Focus();
                                    return;
                                }
                            }
                            else
                            {
                                CommonHelper.AlertInfo(innboardMessage, "This Room Number Already Reserved.", AlertType.Warning);
                                txtRoomNumber.Focus();
                                return;
                            }
                        }
                        else
                        {
                            roomNumberBO.ToDate = !string.IsNullOrWhiteSpace(this.txtToDate.Text) ? hmUtility.GetDateTimeFromString(this.txtToDate.Text, userInformationBO.ServerDateFormat) : DateTime.Now;
                        }
                    }
                }


                if (string.IsNullOrWhiteSpace(txtRoomId.Value))
                {

                    if (DuplicateCheckDynamicaly("RoomNumber", this.txtRoomNumber.Text, 0) > 0)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Room Number" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        this.txtRoomNumber.Focus();
                        return;
                    }

                    int tmpRoomNoId = 0;
                    roomNumberBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = roomNumberDA.SaveRoomNumberInfo(roomNumberBO, out tmpRoomNoId);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.RoomNumber.ToString(), tmpRoomNoId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomNumber));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        this.Cancel();
                    }
                }
                else
                {

                    if (DuplicateCheckDynamicaly("RoomNumber", this.txtRoomNumber.Text, 1) == 1)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Room Number" + AlertMessage.DuplicateValidation, AlertType.Warning);
                        this.txtRoomNumber.Focus();
                        return;
                    }

                    roomNumberBO.Remarks = this.txtRemarks.Text;
                    roomNumberBO.RoomId = Convert.ToInt32(txtRoomId.Value);
                    roomNumberBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = roomNumberDA.UpdateRoomNumberInfo(roomNumberBO);
                    if (status)
                    {
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.RoomNumber.ToString(), roomNumberBO.RoomId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.RoomNumber));
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        this.Cancel();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        //************************ User Defined Function ********************//
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;            
        }
        private bool CheckingMaximumRoomPermission()
        {
            bool status = true;
            EncryptionHelper encryptionHelper = new EncryptionHelper();
            string encryptFieldType = encryptionHelper.Encrypt(hmUtility.GetExpireInformation("FieldType"));

            string decryptValue = string.Empty;
            HMCommonDA hmCommonDA = new HMCommonDA();
            CustomFieldBO customFieldForCash = new CustomFieldBO();
            customFieldForCash = hmCommonDA.GetCustomFieldByFieldName(encryptFieldType);
            if (customFieldForCash != null)
            {
                if (customFieldForCash.FieldId > 0)
                {
                    try
                    {
                        int maxRoomCount = 0;
                        decryptValue = encryptionHelper.Decrypt(customFieldForCash.FieldValue);

                        string[] separators = { "Innb0@rd" };
                        string[] decryptWordList = decryptValue.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        if (decryptWordList != null)
                        {
                            if (decryptWordList.Count() == 4)
                            {
                                maxRoomCount = Convert.ToInt32(decryptWordList[3]);
                            }
                        }
                        
                        int alreadydadedUser = 0;

                        RoomNumberDA userEntityDA = new RoomNumberDA();
                        List<RoomNumberBO> userEntityBOList = new List<RoomNumberBO>();
                        userEntityBOList = userEntityDA.GetRoomNumberInfo();

                        if (userEntityBOList != null)
                        {
                            alreadydadedUser = userEntityBOList.Count;

                            if (maxRoomCount < alreadydadedUser)
                            {
                                CommonHelper.AlertInfo(innboardMessage, "The Room information you entered exceeded your room limitation.", AlertType.Warning);
                                txtRoomNumber.Focus();
                                status = false;
                            }
                        }
                        else
                        {
                            CommonHelper.AlertInfo(innboardMessage, "The Room information you entered exceeded your room limitation.", AlertType.Warning);
                            txtRoomNumber.Focus();
                            status = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "The Room information you entered exceeded your room limitation.", AlertType.Warning);
                        txtRoomNumber.Focus();
                        status = false;
                        throw ex;
                    }
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, "The Room information you entered exceeded your room limitation.", AlertType.Warning);
                    txtRoomNumber.Focus();
                    status = false;
                }
            }
            else
            {
                CommonHelper.AlertInfo(innboardMessage, "The user information you entered exceeded your user limitation.", AlertType.Warning);
                txtRoomNumber.Focus();
                status = false;
            }

            return status;
        }
        public int DuplicateCheckDynamicaly(string fieldName, string fieldValue, int isUpdate)
        {
            string tableName = "HotelRoomNumber";
            string pkFieldName = "RoomId";
            string pkFieldValue = this.txtRoomId.Value;
            int IsDuplicate = 0;
            HMCommonDA hmCommonDA = new HMCommonDA();
            IsDuplicate = hmCommonDA.DuplicateCheckDynamicaly(tableName, fieldName, fieldValue, isUpdate, pkFieldName, pkFieldValue);
            return IsDuplicate;
        }        
        private void LoadRoomType()
        {

            RoomTypeDA roomTypeDA = new RoomTypeDA();
            var List = roomTypeDA.GetRoomTypeInfo();
            this.ddlRoomTypeId.DataSource = List;
            this.ddlRoomTypeId.DataTextField = "RoomType";
            this.ddlRoomTypeId.DataValueField = "RoomTypeId";
            this.ddlRoomTypeId.DataBind();

            this.ddlSRoomType.DataSource = List;
            this.ddlSRoomType.DataTextField = "RoomType";
            this.ddlSRoomType.DataValueField = "RoomTypeId";
            this.ddlSRoomType.DataBind();

            ListItem itemRoomType = new ListItem();
            itemRoomType.Value = "0";
            itemRoomType.Text = hmUtility.GetDropDownFirstValue();
            this.ddlRoomTypeId.Items.Insert(0, itemRoomType);
        }
        private void LoadRoomStatus()
        {
            RoomStatusDA roomStatusDA = new RoomStatusDA();
            var List = roomStatusDA.GetRoomStatusInfo().Where(x => x.StatusId != 2).ToList();
            this.ddlActiveStat.DataSource = List;
            this.ddlActiveStat.DataTextField = "StatusName";
            this.ddlActiveStat.DataValueField = "StatusId";
            this.ddlActiveStat.DataBind();

            this.ddlSStatus.DataSource = List;
            this.ddlSStatus.DataTextField = "StatusName";
            this.ddlSStatus.DataValueField = "StatusId";
            this.ddlSStatus.DataBind();
        }
        private void LoadHKRoomStatus()
        {
            //List<long> hkRoomStatusId = new List<long>() { 1, 3, 4 };
            HKRoomStatusDA roomStatusDA = new HKRoomStatusDA();
            //var List = roomStatusDA.GetHKRoomStatusType().Where(x => hkRoomStatusId.Contains(x.HKRoomStatusId)).ToList();
            var List = roomStatusDA.GetHKRoomStatusType().Where(x => x.HKRoomStatusId != 2).ToList();
            this.ddlHKActiveStat.DataSource = List;
            this.ddlHKActiveStat.DataTextField = "StatusName";
            this.ddlHKActiveStat.DataValueField = "HKRoomStatusId";
            this.ddlHKActiveStat.DataBind();
        }
        private void LoadGridView()
        {
            if (this.ddlSRoomType.SelectedIndex != -1)
            {
                string roomNumber = this.txtSRoomNo.Text;
                //string roomName = this.txtRoomName.Text;
                this.CheckObjectPermission();
                RoomNumberDA roomNumberDA = new RoomNumberDA();
                List<RoomNumberBO> files = roomNumberDA.GetRoomNumberInfoBySearchCriteria(roomNumber);
                this.gvRoomNumber.DataSource = files;
                this.gvRoomNumber.DataBind();
                this.SetTab("SearchTab");
            }
        }
        private void Cancel()
        {
            this.txtRoomId.Value = string.Empty;
            this.ddlRoomTypeId.SelectedIndex = 0;
            this.txtRoomNumber.Text = string.Empty;
            this.txtRoomName.Text = string.Empty;
            this.ddlActiveStat.SelectedIndex = 0;
            this.ddlIsSmokingRoom.SelectedValue = "0";
            this.btnSave.Text = "Save";
            btnSave.Visible = isSavePermission;
            this.txtRoomNumber.Focus();
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
        public void FillForm(int EditId)
        {
            RoomNumberBO roomNumberBO = new RoomNumberBO();
            RoomNumberDA roomNumberDA = new RoomNumberDA();
            roomNumberBO = roomNumberDA.GetRoomNumberInfoById(EditId);
            if (roomNumberBO.StatusId != 2)
            {
                ddlRoomTypeId.SelectedValue = roomNumberBO.RoomTypeId.ToString();
                txtRoomId.Value = roomNumberBO.RoomId.ToString();
                txtRoomNumber.Text = roomNumberBO.RoomNumber;
                txtRoomName.Text = roomNumberBO.RoomName;
                ddlActiveStat.SelectedValue = roomNumberBO.StatusId.ToString();
                ddlHKActiveStat.SelectedValue = roomNumberBO.StatusId.ToString();
                ddlIsSmokingRoom.SelectedValue = (roomNumberBO.IsSmokingRoom == true ? 1 : 0).ToString();
                this.btnSave.Text = "Update";
                btnSave.Visible = isUpdatePermission;

                if (this.ddlActiveStat.SelectedValue == "3")
                {
                    this.isToDateEnable = 1;
                    this.txtToDate.Text = hmUtility.GetStringFromDateTime(roomNumberBO.ToDate);
                    this.txtRemarks.Text = roomNumberBO.Remarks;
                }
            }
            else
            {
                this.Cancel();
                CommonHelper.AlertInfo(innboardMessage, "This room is not editable.", AlertType.Warning);
            }
        }
    }
}