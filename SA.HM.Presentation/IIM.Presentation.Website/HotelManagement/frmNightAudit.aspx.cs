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
using HotelManagement.Entity.HotelManagement;
using System.Globalization;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Data.Restaurant;

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmNightAudit : BasePage
    {
        HiddenField innboardMessage;
        int _ApprovedId;
        Boolean isAllApproved = false;
        private Boolean isRoomSaveButtonVisible = false;
        private Boolean isServiceSaveButtonVisible = false;
        private Boolean isRestaurantSaveButtonVisible = false;
        Boolean isDayClosed = false;
        HMUtility hmUtility = new HMUtility();
        List<RoomRegistrationBO> ActiveRoomRegistrationInfoBO = new List<RoomRegistrationBO>();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            this.IsDayClosed();
            if (!IsPostBack)
            {
                this.SoftwareModulePermissionList();
                this.IsRestaurantIntegrateWithFrontOffice();
                this.btnSaveAll.Visible = true;
                this.btnAllServiceBillApprove.Visible = true;
                this.ApprovedIdHiddenField.Value = string.Empty;
                this.ServiceApprovedIdHiddenField.Value = string.Empty;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {
                return;
            }

            string roomRate = txtRoomRate.Text;
            if (!isFormValid())
            {
                return;
            }
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            AvailableGuestListBO availableGuestList = new AvailableGuestListBO();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

            availableGuestList.RegistrationId = Convert.ToInt32(this.txtRegistrationId.Value);
            availableGuestList.RoomType = this.txtRoomType.Text;
            availableGuestList.RoomId = Convert.ToInt32(this.txtRoomIdHiddenField.Value);
            availableGuestList.RoomNumber = !string.IsNullOrWhiteSpace(this.txtRoomNumber.Text) ? Convert.ToInt32(this.txtRoomNumber.Text) : 0;
            availableGuestList.PreviousRoomRate = Convert.ToDecimal(this.txtPreviousRoomRate.Text);
            availableGuestList.ServiceDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);
            availableGuestList.ApprovedDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);
            availableGuestList.RoomRate = !string.IsNullOrWhiteSpace(this.txtRoomRate.Text) ? Convert.ToDecimal(this.txtRoomRate.Text) : 0;
            availableGuestList.BPPercentAmount = !string.IsNullOrWhiteSpace(this.txtBPPercentAmount.Value) ? Convert.ToDecimal(this.txtBPPercentAmount.Value) : 0;
            availableGuestList.BPDiscountAmount = !string.IsNullOrWhiteSpace(this.txtDiscountAmount.Text) ? Convert.ToDecimal(this.txtDiscountAmount.Text) : 0;
            availableGuestList.VatAmount = !string.IsNullOrWhiteSpace(this.txtVatAmount.Text) ? Convert.ToDecimal(this.txtVatAmount.Text) : 0;
            availableGuestList.ServiceCharge = !string.IsNullOrWhiteSpace(this.txtServiceCharge.Text) ? Convert.ToDecimal(this.txtServiceCharge.Text) : 0;
            availableGuestList.ReferenceSalesCommission = !string.IsNullOrWhiteSpace(this.txtReferenceSalesCommission.Text) ? Convert.ToDecimal(this.txtReferenceSalesCommission.Text) : 0;
            availableGuestList.ApprovedStatus = true;

            availableGuestList.VatAmountPercent = !string.IsNullOrWhiteSpace(this.txtVatAmountPercent.Value) ? Convert.ToDecimal(this.txtVatAmountPercent.Value) : 0;
            availableGuestList.ServiceChargePercent = !string.IsNullOrWhiteSpace(this.txtServiceChargePercent.Value) ? Convert.ToDecimal(this.txtServiceChargePercent.Value) : 0;
            availableGuestList.CalculatedPercentAmount = !string.IsNullOrWhiteSpace(this.txtCalculatedPercentAmount.Value) ? Convert.ToDecimal(this.txtCalculatedPercentAmount.Value) : 0;

            if (!string.IsNullOrWhiteSpace(txtRegistrationId.Value))
            {
                int tmpApprovedId = 0;
                availableGuestList.CreatedBy = userInformationBO.UserInfoId;
                Boolean status = roomRegistrationDA.SaveGuestBillApprovedInfo(availableGuestList, out tmpApprovedId);
                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Approved Operation Successfull.", AlertType.Success);
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ApproveAvailableGuest.ToString(), tmpApprovedId,
                     ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ApproveAvailableGuest));
                    this.LoadGridView("0");
                    this.Cancel();
                }
            }
        }
        protected void btnUpdateRoomApprovedData_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {
                return;
            }

            string roomRate = txtRoomRate.Text;
            if (!isFormValid())
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(ApprovedIdHiddenField.Value))
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                AvailableGuestListBO availableGuestList = new AvailableGuestListBO();
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

                availableGuestList.ApprovedId = Convert.ToInt32(ApprovedIdHiddenField.Value);
                availableGuestList.RegistrationId = Convert.ToInt32(this.txtRegistrationId.Value);
                availableGuestList.RoomType = this.txtRoomType.Text;
                availableGuestList.RoomId = Convert.ToInt32(this.txtRoomIdHiddenField.Value);
                availableGuestList.RoomNumber = !string.IsNullOrWhiteSpace(this.txtRoomNumber.Text) ? Convert.ToInt32(this.txtRoomNumber.Text) : 0;
                availableGuestList.PreviousRoomRate = !string.IsNullOrWhiteSpace(this.txtPreviousRoomRate.Text) ? Convert.ToDecimal(this.txtPreviousRoomRate.Text) : 0;
                availableGuestList.ApprovedDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);
                availableGuestList.RoomRate = !string.IsNullOrWhiteSpace(this.txtRoomRate.Text) ? Convert.ToDecimal(this.txtRoomRate.Text) : 0;
                availableGuestList.BPPercentAmount = !string.IsNullOrWhiteSpace(this.txtBPPercentAmount.Value) ? Convert.ToDecimal(this.txtBPPercentAmount.Value) : 0;
                availableGuestList.BPDiscountAmount = !string.IsNullOrWhiteSpace(this.txtDiscountAmount.Text) ? Convert.ToDecimal(this.txtDiscountAmount.Text) : 0;
                availableGuestList.VatAmount = !string.IsNullOrWhiteSpace(this.txtVatAmount.Text) ? Convert.ToDecimal(this.txtVatAmount.Text) : 0;
                availableGuestList.ServiceCharge = !string.IsNullOrWhiteSpace(this.txtServiceCharge.Text) ? Convert.ToDecimal(this.txtServiceCharge.Text) : 0;
                availableGuestList.ReferenceSalesCommission = !string.IsNullOrWhiteSpace(this.txtReferenceSalesCommission.Text) ? Convert.ToDecimal(this.txtReferenceSalesCommission.Text) : 0;
                availableGuestList.ApprovedStatus = true;

                if (!string.IsNullOrWhiteSpace(txtRegistrationId.Value))
                {
                    int tmpApprovedId = 0;
                    availableGuestList.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = roomRegistrationDA.UpdateGuestBillApprovedInfo(availableGuestList);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ApproveAvailableGuest.ToString(), tmpApprovedId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ApproveAvailableGuest));
                        this.LoadGridView("0");
                        this.Cancel();
                    }
                }
            }
        }
        protected void gvAvailableGuestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvAvailableGuestList.PageIndex = e.NewPageIndex;
            this.LoadGridView("0");
        }
        protected void gvAvailableGuestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblIsGuestCheckedOut = (Label)e.Row.FindControl("lblIsGuestCheckedOut");

                Label bpPercentAmount = (Label)e.Row.FindControl("lblgvBPPercentAmount");
                Label bpDiscountAmount = (Label)e.Row.FindControl("lblgvBPDiscountAmount");
                Label bpDisplayDiscountAmount = (Label)e.Row.FindControl("lblgvDisplayBPDiscountAmount");
                bpDisplayDiscountAmount.Text = bpDiscountAmount.Text + "(" + bpPercentAmount.Text + "%)";
                Label bpRoomRateAmount = (Label)e.Row.FindControl("lblgvRoomRate");

                Label lblgvTotalCalculatedUsdAmountValue = (Label)e.Row.FindControl("lblgvTotalCalculatedUsdAmount");
                if (lblgvTotalCalculatedUsdAmountValue.Text == "0")
                {
                    lblgvTotalCalculatedUsdAmountValue.Text = string.Empty;
                }

                //--------------------
                Label lblValue = (Label)e.Row.FindControl("lblid");
                Label lblApprovedIdValue = (Label)e.Row.FindControl("lblApprovedId");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgApprove = (ImageButton)e.Row.FindControl("ImgApprove");
                ImageButton imgApproved = (ImageButton)e.Row.FindControl("ImgApproved");
                ImageButton imgEdit = (ImageButton)e.Row.FindControl("ImgEdit");

                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";

                string serviceType = "GuestRoom";
                imgEdit.Attributes["onclick"] = "javascript:return PerformFillFormForUpdateAction('" + serviceType + "~" + lblValue.Text + "~" + lblApprovedIdValue.Text + "');";

                //imgEdit.Attributes["onclick"] = "javascript:return PerformFillFormForUpdateAction('" + lblValue.Text + "~" + lblApprovedIdValue.Text + "');";

                imgUpdate.Visible = isSavePermission;
                imgApprove.Visible = isSavePermission;

                Label lblActiveStatValue = (Label)e.Row.FindControl("lblApprovedStatus");

                if (lblActiveStatValue.Text == "False")
                {
                    if (isRoomSaveButtonVisible == false)
                    {
                        isRoomSaveButtonVisible = true;
                    }

                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = true;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = true;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = true;

                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;

                }
                else
                {
                    this.isAllApproved = true;
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = true;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = false;

                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = true;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = true;
                }

                ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;

                if (isDayClosed)
                {
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = false;

                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;
                }

                if (lblIsGuestCheckedOut.Text == "1")
                {
                    isAllApproved = true;
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = false;

                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;
                }

                //// //Temporary Block
                //((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;

            }

            if (isAllApproved)
            {
                if (isDayClosed)
                {
                    this.btnSaveAll.Visible = false;
                }
                else
                {
                    if (isSavePermission)
                    {
                        this.btnSaveAll.Visible = isRoomSaveButtonVisible;
                    }
                    else
                    {
                        this.btnSaveAll.Visible = false;
                    }
                }
            }
            else
            {
                if (isSavePermission)
                {
                    this.btnSaveAll.Visible = isRoomSaveButtonVisible;
                }
                else
                {
                    this.btnSaveAll.Visible = false;
                }
            }
        }
        protected void gvAvailableGuestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdApprove")
            {
                try
                {
                    btnSave.Text = "Update";
                    this._ApprovedId = Convert.ToInt32(e.CommandArgument.ToString());
                    ApprovedGuest(this._ApprovedId.ToString());
                    this._ApprovedId = Convert.ToInt32(e.CommandArgument.ToString());
                    Session["_ApprovedId"] = this._ApprovedId;
                    this.Cancel();
                    this.LoadGridView("0");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                DateTime approvedDate = DateTime.Now;

                approvedDate = !string.IsNullOrWhiteSpace(this.txtApprovedDate.Text) ? hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) : DateTime.Now;

                try
                {
                    HMCommonDA hmCommonDA = new HMCommonDA();
                    this._ApprovedId = Convert.ToInt32(e.CommandArgument.ToString());
                    Boolean status = hmCommonDA.DeleteInfoByAnyIdAndDate("HotelGuestBillApproved", "RegistrationId", _ApprovedId.ToString(), "ApprovedDate", approvedDate.ToString());
                    if (status)
                    {
                        hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.ApproveAvailableGuest.ToString(), _ApprovedId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ApproveAvailableGuest));
                        this.ApprovedIdHiddenField.Value = string.Empty;
                        this.ServiceApprovedIdHiddenField.Value = string.Empty;
                        this.CheckObjectPermission();
                        this.LoadGridView("0");
                        this.LoadServiceBillGridView(0);
                        this.LoadRestaurantBillGridView(0);
                    }
                }
                catch (Exception ex)
                {
                    //lblMessage.Text = "Data Deleted Failed.";
                    throw ex;
                }
            }

            this.SetTab("RoomAuditTab");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (isValidNightAuditDateTime())
            {
                this.IsDayClosed();
                this.btnSaveAll.Visible = true;
                this.btnAllServiceBillApprove.Visible = true;
                this.ApprovedIdHiddenField.Value = string.Empty;
                this.ServiceApprovedIdHiddenField.Value = string.Empty;
                this.CheckObjectPermission();
                this.DataCleanUpProcess();

                string registrationId = "0";
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                RoomAlocationBO roomAllocationBO = new RoomAlocationBO();
                roomAllocationBO = roomRegistrationDA.GetActiveRegistrationInfoByRoomNumber(this.txtSrcRoomNumber.Text);
                if (roomAllocationBO.RoomId > 0)
                {
                    registrationId = roomAllocationBO.RegistrationId.ToString();
                }

                this.LoadGridView(registrationId);
                this.LoadServiceBillGridView(0);
                this.LoadRestaurantBillGridView(0);
                this.IsRestaurantIntegrateWithFrontOffice();
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtRegistrationId.Value = string.Empty;
        }
        protected void btnRestaurantCancel_Click(object sender, EventArgs e)
        {
            this.txtRegistrationId.Value = string.Empty;
        }
        protected void gvGHServiceBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvGHServiceBill.PageIndex = e.NewPageIndex;
            this.LoadServiceBillGridView(0);
            this.LoadRestaurantBillGridView(0);
            this.SetTab("ServiceAuditTab");
        }
        protected void gvGHServiceBill_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                Label lblApprovedIdValue = (Label)e.Row.FindControl("lblApprovedId");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgEdit = (ImageButton)e.Row.FindControl("ImgEdit");

                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormServiceAction('" + lblValue.Text + "');";

                string serviceType = "ServiceBill";
                imgEdit.Attributes["onclick"] = "javascript:return PerformFillFormForUpdateAction('" + serviceType + "~" + lblValue.Text + "~" + lblApprovedIdValue.Text + "');";

                //imgEdit.Attributes["onclick"] = "javascript:return PerformFillFormServiceForUpdateAction('" + lblValue.Text + "~" + lblApprovedIdValue.Text + "');";

                Label lblRoomNumberValue = (Label)e.Row.FindControl("lblRoomNumber");

                Label lblActiveStatValue = (Label)e.Row.FindControl("lblServiceApprovedStatus");
                if (lblActiveStatValue.Text == "False")
                {
                    if (isServiceSaveButtonVisible == false)
                    {
                        isServiceSaveButtonVisible = true;
                    }

                    this.isAllApproved = true;
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = true;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = true;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = true;

                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;
                }
                else
                {
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = true;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = false;

                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = true;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = true;
                }

                ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;

                if (lblRoomNumberValue.Text == "Cash")
                {
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = true;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                }
                //Cash
                //--------------- Eddit Button Hide --------------------------------------
                ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                //((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;

                if (isDayClosed)
                {
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = false;

                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;

                    this.btnAllServiceBillApprove.Visible = false;
                }
                else
                {
                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                    Label lblRegiIdValue = (Label)e.Row.FindControl("lblRegiId");
                    Int64 regiId = !string.IsNullOrWhiteSpace(lblRegiIdValue.Text) ? Convert.ToInt64(lblRegiIdValue.Text) : 0;
                    ActiveRoomRegistrationInfoBO = ActiveRoomRegistrationInfoBO.Where(x => x.RegistrationId == regiId).ToList();
                    if (ActiveRoomRegistrationInfoBO != null)
                    {
                        if (ActiveRoomRegistrationInfoBO.Count > 0)
                        {
                            ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = true;
                        }
                    }

                    this.btnAllServiceBillApprove.Visible = isSavePermission;
                }
            }

            //if (isDayClosed)
            //{
            //    this.btnAllServiceBillApprove.Visible = false;
            //}
            //else
            //{
            //    this.btnAllServiceBillApprove.Visible = isSavePermission;
            //}
        }
        protected void gvRestaurantServiceBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvRestaurantServiceBill.PageIndex = e.NewPageIndex;
            this.LoadServiceBillGridView(0);
            this.LoadRestaurantBillGridView(0);
            this.SetTab("ServiceAuditTab");
        }
        protected void gvRestaurantServiceBill_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                Label lblApprovedIdValue = (Label)e.Row.FindControl("lblApprovedId");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgEdit = (ImageButton)e.Row.FindControl("ImgEdit");

                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormRestaurantServiceAction('" + lblValue.Text + "');";

                //string serviceType = "RestaurantBill";
                //imgEdit.Attributes["onclick"] = "javascript:return PerformFillFormForUpdateAction('" + serviceType + "~" + lblValue.Text + "');";

                imgEdit.Attributes["onclick"] = "javascript:return PerformFillFormRestaurantServiceForUpdateAction('" + lblValue.Text + "');";

                Label lblRoomNumberValue = (Label)e.Row.FindControl("lblRoomNumber");

                Label lblActiveStatValue = (Label)e.Row.FindControl("lblServiceApprovedStatus");
                if (lblActiveStatValue.Text == "False")
                {
                    if (isRestaurantSaveButtonVisible == false)
                    {
                        isRestaurantSaveButtonVisible = true;
                    }

                    this.isAllApproved = true;
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = true;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = true;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = true;

                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;
                }
                else
                {
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = true;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = false;

                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = true;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = true;
                }

                ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;

                if (lblRoomNumberValue.Text == "Cash")
                {
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = true;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                }

                //((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;

                if (isDayClosed)
                {
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = false;

                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;

                    this.btnAllRestaurantServiceBillApprove.Visible = false;
                }
                else
                {
                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                    Label lblRegiIdValue = (Label)e.Row.FindControl("lblRegiId");
                    Int64 regiId = !string.IsNullOrWhiteSpace(lblRegiIdValue.Text) ? Convert.ToInt64(lblRegiIdValue.Text) : 0;
                    ActiveRoomRegistrationInfoBO = ActiveRoomRegistrationInfoBO.Where(x => x.RegistrationId == regiId).ToList();
                    if (ActiveRoomRegistrationInfoBO != null)
                    {
                        if (ActiveRoomRegistrationInfoBO.Count > 0)
                        {
                            ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = true;
                        }
                    }

                    this.btnAllRestaurantServiceBillApprove.Visible = isRestaurantSaveButtonVisible;
                }
            }

            ////if (isDayClosed)
            ////{
            ////    this.btnAllRestaurantServiceBillApprove.Visible = false;
            ////}
            ////else
            ////{
            ////    this.btnAllRestaurantServiceBillApprove.Visible = isRestaurantSaveButtonVisible;
            ////}
        }
        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {
                return;
            }
            int tmpApprovedId = 0;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int counter = 0;
            foreach (GridViewRow row in gvAvailableGuestList.Rows)
            {
                counter = counter + 1;
                bool isAccept = ((CheckBox)row.FindControl("CheckBoxAccept")).Checked;
                if (isAccept)
                {
                    Label lblActiveStatValue = (Label)row.FindControl("lblApprovedStatus");
                    if (lblActiveStatValue.Text == "False")
                    {
                        AvailableGuestListBO availableGuestList = new AvailableGuestListBO();
                        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

                        availableGuestList.RegistrationId = Convert.ToInt32(((Label)row.FindControl("lblid")).Text);
                        availableGuestList.RoomId = Convert.ToInt32(((Label)row.FindControl("lblRoomId")).Text);
                        availableGuestList.RoomNumber = Convert.ToInt32(((Label)row.FindControl("lblgvRoomNumber")).Text);
                        availableGuestList.GuestName = ((Label)row.FindControl("lblgvGuestName")).Text;
                        availableGuestList.ServiceName = ((Label)row.FindControl("lblgvServiceName")).Text;
                        availableGuestList.RoomType = ((Label)row.FindControl("lblgvRoomType")).Text;
                        availableGuestList.RoomRate = Convert.ToDecimal(((Label)row.FindControl("lblgvCalculatedRoomRateAmount")).Text);
                        availableGuestList.BPPercentAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvBPPercentAmount")).Text);
                        availableGuestList.BPDiscountAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvBPDiscountAmount")).Text);
                        availableGuestList.VatAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvVatAmount")).Text);
                        availableGuestList.ServiceCharge = Convert.ToDecimal(((Label)row.FindControl("lblgvServiceCharge")).Text);
                        availableGuestList.CitySDCharge = Convert.ToDecimal(((Label)row.FindControl("lblgvCitySDCharge")).Text);
                        availableGuestList.AdditionalCharge = Convert.ToDecimal(((Label)row.FindControl("lblgvAdditionalChargee")).Text);
                        availableGuestList.ReferenceSalesCommission = Convert.ToDecimal(((Label)row.FindControl("lblgvReferenceSalesCommission")).Text);
                        availableGuestList.PreviousRoomRate = Convert.ToDecimal(((Label)row.FindControl("lblgvRoomRate")).Text);
                        availableGuestList.ApprovedStatus = true;
                        availableGuestList.ServiceDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);
                        availableGuestList.ApprovedDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);
                        availableGuestList.CreatedBy = userInformationBO.UserInfoId;
                        availableGuestList.VatAmountPercent = Convert.ToDecimal(((Label)row.FindControl("lblgvVatAmountPercent")).Text);
                        availableGuestList.ServiceChargePercent = Convert.ToDecimal(((Label)row.FindControl("lblgvServiceChargePercent")).Text);
                        availableGuestList.CalculatedPercentAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvCalculatedPercentAmount")).Text);

                        Boolean status = roomRegistrationDA.SaveGuestBillApprovedInfo(availableGuestList, out tmpApprovedId);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.GuestRoomAndServiceApprove.ToString(), tmpApprovedId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GuestRoomAndServiceApprove));

                    }
                }
            }

            this.LoadGridView("0");
            this.SetTab("RoomAuditTab");
            CommonHelper.AlertInfo(innboardMessage, "Approved Operation Successfull", AlertType.Success);
        }
        protected void btnAllServiceBillApproved_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {
                return;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int counter = 0;
            foreach (GridViewRow row in gvGHServiceBill.Rows)
            {
                counter = counter + 1;
                bool isAccept = ((CheckBox)row.FindControl("CheckBoxAccept")).Checked;
                if (isAccept)
                {
                    Label lblActiveStatValue = (Label)row.FindControl("lblServiceApprovedStatus");
                    if (lblActiveStatValue.Text == "False")
                    {
                        int tmpApprovedId = 0;

                        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                        GHServiceBillBO serviceList = new GHServiceBillBO();
                        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

                        serviceList.PaymentMode = ((Label)row.FindControl("lblRoomNumber")).Text;
                        serviceList.RegistrationId = Int32.Parse(((Label)row.FindControl("lblRegiId")).Text);
                        serviceList.ServiceBillId = Int32.Parse(((Label)row.FindControl("lblid")).Text);
                        serviceList.ServiceDate = hmUtility.GetDateTimeFromString(((Label)row.FindControl("lblgvServiceDate")).Text, userInformationBO.ServerDateFormat);
                        serviceList.ServiceType = ((Label)row.FindControl("lblServiceType")).Text;
                        serviceList.ServiceId = Int32.Parse(((Label)row.FindControl("lblServiceId")).Text);
                        serviceList.ServiceName = ((Label)row.FindControl("lblServiceName")).Text;
                        serviceList.ServiceQuantity = Convert.ToDecimal(((Label)row.FindControl("lblServiceQuantity")).Text);
                        serviceList.ServiceRate = Convert.ToDecimal(((Label)row.FindControl("lblServiceRate")).Text);
                        serviceList.DiscountAmount = Convert.ToDecimal(((Label)row.FindControl("lblDiscountAmount")).Text);
                        serviceList.VatAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvVatAmount")).Text);
                        serviceList.ServiceCharge = Convert.ToDecimal(((Label)row.FindControl("lblgvServiceCharge")).Text);
                        serviceList.VatAmountPercent = Convert.ToDecimal(((Label)row.FindControl("lblgvVatAmountPercent")).Text);
                        serviceList.ServiceChargePercent = Convert.ToDecimal(((Label)row.FindControl("lblgvServiceChargePercent")).Text);
                        serviceList.CalculatedPercentAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvCalculatedPercentAmount")).Text);
                        serviceList.ApprovedStatus = true;
                        serviceList.ApprovedDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);
                        serviceList.CreatedBy = userInformationBO.UserInfoId;

                        Boolean status = roomRegistrationDA.SaveGuestServiceBillApprovedInfo(serviceList, out tmpApprovedId);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ApproveGuestService.ToString(), tmpApprovedId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ApproveGuestService));
                    }

                }
            }

            this.LoadServiceBillGridView(0);
            this.SetTab("ServiceAuditTab");
            CommonHelper.AlertInfo(innboardMessage, "Approved Operation Successfull", AlertType.Success);
        }
        protected void btnAllRestaurantServiceBillApprove_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {
                return;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int counter = 0;
            foreach (GridViewRow row in gvRestaurantServiceBill.Rows)
            {
                counter = counter + 1;
                bool isAccept = ((CheckBox)row.FindControl("CheckBoxAccept")).Checked;
                if (isAccept)
                {
                    Label lblActiveStatValue = (Label)row.FindControl("lblServiceApprovedStatus");
                    if (lblActiveStatValue.Text == "False")
                    {
                        int tmpApprovedId = 0;

                        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                        GHServiceBillBO serviceList = new GHServiceBillBO();
                        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

                        serviceList.PaymentMode = ((Label)row.FindControl("lblRoomNumber")).Text;
                        serviceList.RegistrationId = Int32.Parse(((Label)row.FindControl("lblRegiId")).Text);
                        serviceList.ServiceBillId = Int32.Parse(((Label)row.FindControl("lblid")).Text);
                        serviceList.ServiceDate = hmUtility.GetDateTimeFromString(((Label)row.FindControl("lblgvServiceDate")).Text, userInformationBO.ServerDateFormat);
                        serviceList.ServiceType = ((Label)row.FindControl("lblServiceType")).Text;
                        serviceList.ServiceId = Int32.Parse(((Label)row.FindControl("lblServiceId")).Text);
                        serviceList.ServiceName = ((Label)row.FindControl("lblServiceName")).Text;
                        serviceList.ServiceQuantity = Convert.ToDecimal(((Label)row.FindControl("lblServiceQuantity")).Text);
                        serviceList.ServiceRate = Convert.ToDecimal(((Label)row.FindControl("lblServiceRate")).Text);
                        serviceList.DiscountAmount = Convert.ToDecimal(((Label)row.FindControl("lblDiscountAmount")).Text);
                        serviceList.VatAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvVatAmount")).Text);
                        serviceList.ServiceCharge = Convert.ToDecimal(((Label)row.FindControl("lblgvServiceCharge")).Text);
                        serviceList.IsPaidService = Convert.ToBoolean(((Label)row.FindControl("lblgvIsPaidService")).Text);
                        serviceList.VatAmountPercent = Convert.ToDecimal(((Label)row.FindControl("lblgvVatAmountPercent")).Text);
                        serviceList.ServiceChargePercent = Convert.ToDecimal(((Label)row.FindControl("lblgvServiceChargePercent")).Text);
                        serviceList.CalculatedPercentAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvCalculatedPercentAmount")).Text);
                        serviceList.ApprovedStatus = true;
                        serviceList.ApprovedDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);
                        serviceList.CreatedBy = userInformationBO.UserInfoId;

                        Boolean status = roomRegistrationDA.SaveGuestServiceBillApprovedInfo(serviceList, out tmpApprovedId);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ApproveGuestService.ToString(), tmpApprovedId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ApproveGuestService));
                    }

                }
            }

            this.LoadRestaurantBillGridView(0);
            this.SetTab("RestaurantAuditTab");
            CommonHelper.AlertInfo(innboardMessage, "Approved Operation Successfull", AlertType.Success);
        }
        protected void gvGHServiceBill_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdApprove")
            {
                try
                {
                    this._ApprovedId = Convert.ToInt32(e.CommandArgument.ToString());
                    ApprovedGuestService("GuestHouseService", this._ApprovedId);
                    this._ApprovedId = Convert.ToInt32(e.CommandArgument.ToString());
                    Session["_ServiceApprovedId"] = this._ApprovedId;
                    this.LoadServiceBillGridView(0);
                    this.SetTab("ServiceAuditTab");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (e.CommandName == "CmdPreview")
            {
                try
                {
                    int serviceBillId = Convert.ToInt32(e.CommandArgument.ToString());
                    string url = "/HotelManagement/Reports/frmReportServiceBillInfo.aspx?billID=" + serviceBillId.ToString();
                    string sPopUp = "window.open('" + url + "', 'popup_window', 'width=810,height=780,left=300,top=50,resizable=yes, scrollbars=1');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
                    this.SetTab("ServiceAuditTab");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        protected void gvRestaurantServiceBill_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdApprove")
            {
                try
                {
                    this._ApprovedId = Convert.ToInt32(e.CommandArgument.ToString());
                    ApprovedGuestService("RestaurantService", this._ApprovedId);
                    this._ApprovedId = Convert.ToInt32(e.CommandArgument.ToString());
                    Session["_ServiceApprovedId"] = this._ApprovedId;
                    this.LoadRestaurantBillGridView(0);
                    this.SetTab("RestaurantAuditTab");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (e.CommandName == "CmdPreview")
            {
                try
                {
                    int billId = Convert.ToInt32(e.CommandArgument.ToString());
                    string url = "../POS/Reports/frmReportBillInfo.aspx?billID=" + billId.ToString();
                    string sPopUp = "window.open('" + url + "', 'popup_window', 'width=715,height=780,left=300,top=50,resizable=yes, scrollbars=1');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
                    this.SetTab("RestaurantAuditTab");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        protected void btnServiceApproved_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {
                return;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int tmpApprovedId = 0;

            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GHServiceBillBO serviceList = new GHServiceBillBO();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            serviceList.PaymentMode = this.txtPaymentMode.Value;
            serviceList.RegistrationId = Int32.Parse(this.txtServiceRegistrationId.Value);
            serviceList.ServiceBillId = Int32.Parse(this.txtServiceBillId.Value);
            serviceList.ServiceDate = hmUtility.GetDateTimeFromString(this.txtServiceServiceDate.Value, userInformationBO.ServerDateFormat);
            serviceList.ServiceType = this.txtServiceType.Value;
            serviceList.ServiceId = Int32.Parse(this.txtServiceServiceId.Value);
            serviceList.ServiceName = this.txtServiceServiceName.Value;
            serviceList.ServiceQuantity = Convert.ToDecimal(this.txtServiceQuantity.Text);
            serviceList.ServiceRate = Convert.ToDecimal(this.txtServiceRate.Text);
            serviceList.DiscountAmount = Convert.ToDecimal(this.txtDiscountAmount.Text);
            serviceList.VatAmount = Convert.ToDecimal(this.txtGuestServiceVatAmount.Text);
            serviceList.ServiceCharge = Convert.ToDecimal(this.txtGuestServiceServiceCharge.Text);
            serviceList.ApprovedStatus = true;
            serviceList.ApprovedDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);
            serviceList.CreatedBy = userInformationBO.UserInfoId;
            serviceList.VatAmountPercent = Convert.ToDecimal(this.txtGuestServiceVatAmountPercent.Value);
            serviceList.ServiceChargePercent = Convert.ToDecimal(this.txtGuestServiceServiceChargePercent.Value);
            serviceList.CalculatedPercentAmount = Convert.ToDecimal(this.txtGuestServiceCalculatedPercentAmount.Value);

            Boolean status = roomRegistrationDA.SaveGuestServiceBillApprovedInfo(serviceList, out tmpApprovedId);
            if (status)
                hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.ApproveAvailableGuest.ToString(), tmpApprovedId, ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ApproveAvailableGuest));
            this.LoadServiceBillGridView(0);
            this.LoadRestaurantBillGridView(0);
            this.SetTab("ServiceAuditTab");
        }
        protected void btnRestaurantServiceApproved_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {
                return;
            }

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int tmpApprovedId = 0;

            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GHServiceBillBO serviceList = new GHServiceBillBO();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            serviceList.PaymentMode = this.txtRestaurantPaymentMode.Value;
            serviceList.RegistrationId = Int32.Parse(this.txtRestaurantServiceRegistrationId.Value);
            serviceList.ServiceBillId = Int32.Parse(this.txtRestaurantServiceBillId.Value);
            serviceList.ServiceDate = hmUtility.GetDateTimeFromString(this.txtRestaurantServiceServiceDate.Value, userInformationBO.ServerDateFormat);
            serviceList.ServiceType = this.txtRestaurantServiceType.Value;
            serviceList.ServiceId = Int32.Parse(this.txtRestaurantServiceServiceId.Value);
            serviceList.ServiceName = this.txtRestaurantServiceServiceName.Value;
            serviceList.ServiceQuantity = Convert.ToDecimal(this.txtRestaurantServiceQuantity.Text);
            serviceList.ServiceRate = Convert.ToDecimal(this.txtRestaurantServiceRate.Text);
            serviceList.DiscountAmount = Convert.ToDecimal(this.txtRestaurantDiscountAmount.Text);
            serviceList.VatAmount = Convert.ToDecimal(this.txtRestaurantGuestServiceVatAmount.Text);
            serviceList.ServiceCharge = Convert.ToDecimal(this.txtRestaurantGuestServiceServiceCharge.Text);
            serviceList.ApprovedStatus = true;
            serviceList.ApprovedDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);
            serviceList.CreatedBy = userInformationBO.UserInfoId;
            serviceList.VatAmountPercent = Convert.ToDecimal(this.txtRestaurantGuestServiceVatAmountPercent.Value);
            serviceList.ServiceChargePercent = Convert.ToDecimal(this.txtRestaurantGuestServiceServiceChargePercent.Value);
            serviceList.CalculatedPercentAmount = Convert.ToDecimal(this.txtRestaurantGuestServiceCalculatedPercentAmount.Value);

            Boolean status = roomRegistrationDA.SaveGuestServiceBillApprovedInfo(serviceList, out tmpApprovedId);
            this.LoadServiceBillGridView(0);
            this.LoadRestaurantBillGridView(0);
            this.SetTab("RestaurantAuditTab");
        }
        protected void btnUpdateApprovedService_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(ServiceApprovedIdHiddenField.Value))
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                int tmpApprovedId = 0;

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                GHServiceBillBO serviceList = new GHServiceBillBO();
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                serviceList.ApprovedId = Convert.ToInt32(ServiceApprovedIdHiddenField.Value);
                serviceList.PaymentMode = this.txtPaymentMode.Value;
                serviceList.RegistrationId = Int32.Parse(this.txtServiceRegistrationId.Value);
                serviceList.ServiceBillId = Int32.Parse(this.txtServiceBillId.Value);
                serviceList.ServiceDate = hmUtility.GetDateTimeFromString(this.txtServiceServiceDate.Value, userInformationBO.ServerDateFormat);
                serviceList.ServiceType = this.txtServiceType.Value;
                serviceList.ServiceId = Int32.Parse(this.txtServiceServiceId.Value);
                serviceList.ServiceName = this.txtServiceServiceName.Value;
                serviceList.ServiceQuantity = Convert.ToDecimal(this.txtServiceQuantity.Text);
                serviceList.ServiceRate = Convert.ToDecimal(this.txtServiceRate.Text);
                serviceList.DiscountAmount = Convert.ToDecimal(this.txtDiscountAmount.Text);
                serviceList.VatAmount = Convert.ToDecimal(this.txtGuestServiceVatAmount.Text);
                serviceList.ServiceCharge = Convert.ToDecimal(this.txtGuestServiceServiceCharge.Text);
                serviceList.RestaurantBillId = 0;

                serviceList.ApprovedStatus = true;
                serviceList.ApprovedDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);

                if (!string.IsNullOrWhiteSpace(ServiceApprovedIdHiddenField.Value))
                {
                    serviceList.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = roomRegistrationDA.UpdateGuestServiceBillApprovedInfo(serviceList);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ApproveAvailableGuest.ToString(), tmpApprovedId,
                             ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ApproveAvailableGuest));
                        this.LoadServiceBillGridView(0);
                        this.SetTab("ServiceAuditTab");
                    }
                }
            }
        }
        protected void btnUpdateRestaurantApprovedService_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(RestaurantServiceApprovedIdHiddenField.Value))
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                int tmpApprovedId = 0;

                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                GHServiceBillBO serviceList = new GHServiceBillBO();
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                serviceList.ApprovedId = Convert.ToInt32(RestaurantServiceApprovedIdHiddenField.Value);
                serviceList.PaymentMode = this.txtRestaurantPaymentMode.Value;
                serviceList.RegistrationId = Int32.Parse(this.txtRestaurantServiceRegistrationId.Value);
                serviceList.ServiceBillId = Int32.Parse(this.txtRestaurantServiceBillId.Value);
                serviceList.ServiceDate = hmUtility.GetDateTimeFromString(this.txtRestaurantServiceServiceDate.Value, userInformationBO.ServerDateFormat);
                serviceList.ServiceType = this.txtRestaurantServiceType.Value;
                serviceList.ServiceId = Int32.Parse(this.txtRestaurantServiceServiceId.Value);
                serviceList.ServiceName = this.txtRestaurantServiceServiceName.Value;
                serviceList.ServiceQuantity = Convert.ToDecimal(this.txtRestaurantServiceQuantity.Text);
                serviceList.ServiceRate = Convert.ToDecimal(this.txtRestaurantServiceRate.Text);
                serviceList.DiscountAmount = Convert.ToDecimal(this.txtRestaurantDiscountAmount.Text);
                serviceList.VatAmount = Convert.ToDecimal(this.txtRestaurantGuestServiceVatAmount.Text);
                serviceList.ServiceCharge = Convert.ToDecimal(this.txtRestaurantGuestServiceServiceCharge.Text);
                serviceList.RestaurantBillId = 0;

                serviceList.ApprovedStatus = true;
                serviceList.ApprovedDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);

                if (!string.IsNullOrWhiteSpace(RestaurantServiceApprovedIdHiddenField.Value))
                {
                    serviceList.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = roomRegistrationDA.UpdateGuestServiceBillApprovedInfo(serviceList);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ApproveAvailableGuest.ToString(), tmpApprovedId,
                             ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ApproveAvailableGuest));
                        this.LoadRestaurantBillGridView(0);
                        this.SetTab("RestaurantAuditTab");
                    }
                }
            }
        }
        //************************ User Defined Function ********************//
        private void SoftwareModulePermissionList()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            if (userInformationBO.UserInfoId == 1)
            {
                hfNightAuditInformation.Value = "1,2,3";
            }
            else
            {
                if (Session["SoftwareModulePermissionList"] != null)
                {
                    hfNightAuditInformation.Value = Session["SoftwareModulePermissionList"].ToString();
                }
                else
                {
                    Session["UserInformationBOSession"] = null;
                    Response.Redirect("Login.aspx");
                }
            }
        }
        public void DataCleanUpProcess()
        {
            if (!string.IsNullOrWhiteSpace(this.txtApprovedDate.Text))
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                Boolean status = roomRegistrationDA.DeleteCheckOutDataCleanUpProcess(hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat));
            }
        }
        public bool isValidNightAuditDateTime()
        {
            bool status = true;           
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            string approvedDate = txtApprovedDate.Text;
            if (String.IsNullOrWhiteSpace(approvedDate))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Audit Date.", AlertType.Warning);
                txtApprovedDate.Focus();
                status = false;
            }
            else if (!String.IsNullOrWhiteSpace(approvedDate))
            {
                if (hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date >= DateTime.Now.Date)
                {
                    int isValidDate = 1;
                    isValidDate = roomRegistrationDA.GetIsValidNightAuditDateTime(hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat));
                    if (isValidDate == 0)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Night Audit is not aplicable for your provided date.", AlertType.Warning);
                        this.gvAvailableGuestList.DataSource = null;
                        this.gvAvailableGuestList.DataBind();
                        this.gvGHServiceBill.DataSource = null;
                        this.gvGHServiceBill.DataBind();
                        this.gvRestaurantServiceBill.DataSource = null;
                        this.gvRestaurantServiceBill.DataBind();
                        this.btnSaveAll.Visible = false;
                        this.btnAllServiceBillApprove.Visible = false;
                        txtApprovedDate.Focus();
                        status = false;
                    }
                    else if (isValidDate == 2)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Please Firstly Complete Previous Day Night Audit.", AlertType.Warning);
                        this.gvAvailableGuestList.DataSource = null;
                        this.gvAvailableGuestList.DataBind();
                        this.gvGHServiceBill.DataSource = null;
                        this.gvGHServiceBill.DataBind();
                        this.gvRestaurantServiceBill.DataSource = null;
                        this.gvRestaurantServiceBill.DataBind();
                        this.btnSaveAll.Visible = false;
                        this.btnAllServiceBillApprove.Visible = false;
                        txtApprovedDate.Focus();
                        status = false;
                    }
                }

                string failedNightAuditProcessMessage = string.Empty;
                if (!roomRegistrationDA.GetIsValidNightAuditProcess(hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), "ExpectedCheckOutDate", out failedNightAuditProcessMessage))
                {
                    CommonHelper.AlertInfo(innboardMessage, failedNightAuditProcessMessage, AlertType.Warning);
                    this.gvAvailableGuestList.DataSource = null;
                    this.gvAvailableGuestList.DataBind();
                    this.gvGHServiceBill.DataSource = null;
                    this.gvGHServiceBill.DataBind();
                    this.gvRestaurantServiceBill.DataSource = null;
                    this.gvRestaurantServiceBill.DataBind();
                    this.btnSaveAll.Visible = false;
                    this.btnAllServiceBillApprove.Visible = false;
                    txtApprovedDate.Focus();
                    status = false;
                }                
            }
            return status;
        }
        private void IsRestaurantIntegrateWithFrontOffice()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("IsRestaurantIntegrateWithFrontOffice", "IsRestaurantIntegrateWithFrontOffice");
            if (commonSetupBO != null)
            {
                if (!string.IsNullOrEmpty(commonSetupBO.SetupValue))
                {
                    if (commonSetupBO.SetupValue == "0")
                    {
                        CRestaurantAudit.Visible = false;
                        IsRestaurantIntegrateWithFrontOfficeDiv.Visible = false;
                    }
                    else
                    {
                        CRestaurantAudit.Visible = true;
                        IsRestaurantIntegrateWithFrontOfficeDiv.Visible = true;
                    }
                }
            }
        }
        private void IsDayClosed()
        {
            HMCommonDA hmCoomnoDA = new HMCommonDA();
            DayCloseBO dayCloseBO = new DayCloseBO();
            DateTime transactionDate = !string.IsNullOrWhiteSpace(this.txtApprovedDate.Text) ? hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) : DateTime.Now;

            dayCloseBO = hmCoomnoDA.GetHotelDayCloseInformation(transactionDate);
            if (dayCloseBO != null)
            {
                if (dayCloseBO.DayCloseId > 0)
                {
                    isDayClosed = true;
                }
            }
        }
        //private void IsValidNightAuditProcess()
        //{
        //    HMCommonDA hmCoomnoDA = new HMCommonDA();
        //    DayCloseBO dayCloseBO = new DayCloseBO();
        //    DateTime transactionDate = !string.IsNullOrWhiteSpace(this.txtApprovedDate.Text) ? hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat) : DateTime.Now;

        //    dayCloseBO = hmCoomnoDA.GetHotelDayCloseInformation(transactionDate);
        //    if (dayCloseBO != null)
        //    {
        //        if (dayCloseBO.DayCloseId > 0)
        //        {
        //            isDayClosed = true;
        //        }
        //    }
        //}
        private void LoadGridView(string registrationId)
        {
            this.CheckObjectPermission();
            if (!string.IsNullOrWhiteSpace(this.txtApprovedDate.Text))
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                if (registrationId == "0")
                {
                    ActiveRoomRegistrationInfoBO = roomRegistrationDA.GetActiveRoomRegistrationInfo();
                    foreach (RoomRegistrationBO row in ActiveRoomRegistrationInfoBO)
                    {
                        if (registrationId == "0")
                        {
                            registrationId = row.RegistrationId.ToString();

                            List<RoomRegistrationBO> GetActiveRoomRegistrationInfoByTransactionDateListBO = roomRegistrationDA.GetActiveRoomRegistrationInfoByTransactionDate(hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat));
                            foreach (RoomRegistrationBO rowTransactionDateList in GetActiveRoomRegistrationInfoByTransactionDateListBO)
                            {
                                registrationId += "," + rowTransactionDateList.RegistrationId.ToString();
                            }
                        }
                        else
                        {
                            registrationId += "," + row.RegistrationId.ToString();
                        }
                    }
                }

                List<AvailableGuestListBO> files = new List<AvailableGuestListBO>();
                if (hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).Date == DateTime.Now.Date)
                {
                    files = roomRegistrationDA.GetGuestRoomNightAuditInfo(registrationId, DateTime.Now, hmUtility.GetCurrentApplicationUserInfo().UserInfoId);
                }
                else
                {
                    files = roomRegistrationDA.GetGuestRoomNightAuditInfo(registrationId, hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), hmUtility.GetCurrentApplicationUserInfo().UserInfoId);
                }

                this.gvAvailableGuestList.DataSource = files;
                this.gvAvailableGuestList.DataBind();
            }
            else
            {
                this.gvAvailableGuestList.DataSource = null;
                this.gvAvailableGuestList.DataBind();
            }
        }
        private void Cancel()
        {
            //this.ddlRoomTypeId.SelectedIndex = 0;
            //this.txtRoomNumber.Text = string.Empty;
            //this.ddlActiveStat.SelectedIndex = 0;
            //this.btnSave.Text = "Save";
            //this.txtRoomNumber.Focus();
        }
        public bool isFormValid()
        {
            bool status = true;
            string roomRate = txtRoomRate.Text;
            Decimal number;
            if (String.IsNullOrWhiteSpace(roomRate))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Room Rate.", AlertType.Warning);
                txtRoomRate.Focus();
                status = false;
            }
            else if (!Decimal.TryParse(roomRate, out number))
            {
                CommonHelper.AlertInfo(innboardMessage, "Room Rate" + AlertMessage.FormatValidation, AlertType.Warning);
                txtRoomRate.Focus();
                status = false;
            }

            return status;
        }
        public void ApprovedGuest(string rowId)
        {
            if (!isValidForm())
            {
                return;
            }
            int tmpApprovedId = 0;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            AvailableGuestListBO availableGuestList = new AvailableGuestListBO();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

            List<AvailableGuestListBO> files = roomRegistrationDA.GetGuestRoomNightAuditInfo(rowId, hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat), hmUtility.GetCurrentApplicationUserInfo().UserInfoId);
            if (files != null)
            {
                if (files.Count > 0)
                {
                    availableGuestList.RegistrationId = Int32.Parse(files[0].RegistrationId.ToString());
                    availableGuestList.RoomId = Int32.Parse(files[0].RoomId.ToString());
                    availableGuestList.RoomNumber = Convert.ToInt32(files[0].RoomNumber.ToString());
                    availableGuestList.ServiceName = files[0].ServiceName.ToString();
                    availableGuestList.GuestName = files[0].GuestName.ToString();
                    availableGuestList.RoomType = files[0].RoomType.ToString();
                    decimal calculateAmount = Convert.ToDecimal(files[0].CalculatedRoomRate.ToString());
                    availableGuestList.RoomRate = calculateAmount;
                    availableGuestList.BPPercentAmount = Convert.ToDecimal(files[0].BPPercentAmount.ToString());
                    availableGuestList.BPDiscountAmount = Convert.ToDecimal(files[0].BPDiscountAmount.ToString());
                    availableGuestList.PreviousRoomRate = Convert.ToDecimal(files[0].PreviousRoomRate.ToString());
                    availableGuestList.VatAmount = Convert.ToDecimal(files[0].VatAmount.ToString());
                    availableGuestList.ServiceCharge = Convert.ToDecimal(files[0].ServiceCharge.ToString());
                    availableGuestList.CitySDCharge = Convert.ToDecimal(files[0].CitySDCharge.ToString());
                    availableGuestList.AdditionalCharge = Convert.ToDecimal(files[0].AdditionalCharge.ToString());
                    availableGuestList.ReferenceSalesCommission = Convert.ToDecimal(files[0].ReferenceSalesCommission.ToString());
                    availableGuestList.ApprovedStatus = true;
                    availableGuestList.ServiceDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);
                    availableGuestList.ApprovedDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);
                    availableGuestList.CreatedBy = userInformationBO.UserInfoId;
                    availableGuestList.VatAmountPercent = Convert.ToDecimal(files[0].VatAmountPercent.ToString());
                    availableGuestList.ServiceChargePercent = Convert.ToDecimal(files[0].ServiceChargePercent.ToString());
                    availableGuestList.CalculatedPercentAmount = Convert.ToDecimal(files[0].CalculatedPercentAmount.ToString());

                    Boolean status = roomRegistrationDA.SaveGuestBillApprovedInfo(availableGuestList, out tmpApprovedId);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Approved Operation Successfull.", AlertType.Success);
                        this.LoadGridView("0");
                        this.Cancel();
                    }
                }
            }
        }
        public void ApprovedGuestService(string guestServiceType, int rowId)
        {
            if (!isValidForm())
            {
                return;
            }
            int tmpApprovedId = 0;

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GHServiceBillBO serviceList = new GHServiceBillBO();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();

            List<GHServiceBillBO> files = roomRegistrationDA.GetServiceBillNightAudit(guestServiceType, rowId, hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat), this.txtSrcRoomNumber.Text);

            serviceList.PaymentMode = files[0].RoomNumber.ToString();
            serviceList.RegistrationId = Int32.Parse(files[0].RegistrationId.ToString());
            serviceList.ServiceBillId = Int32.Parse(files[0].ServiceBillId.ToString());
            serviceList.ServiceDate = Convert.ToDateTime(files[0].ServiceDate.ToString());
            serviceList.ServiceType = files[0].ServiceType.ToString();
            serviceList.ServiceId = Int32.Parse(files[0].ServiceId.ToString());
            serviceList.ServiceName = files[0].ServiceName.ToString();
            serviceList.ServiceQuantity = Convert.ToDecimal(files[0].ServiceQuantity.ToString());
            serviceList.ServiceRate = Convert.ToDecimal(files[0].ServiceRate.ToString());
            serviceList.DiscountAmount = Convert.ToDecimal(files[0].DiscountAmount.ToString());
            serviceList.VatAmount = Convert.ToDecimal(files[0].VatAmount.ToString());
            serviceList.ServiceCharge = Convert.ToDecimal(files[0].ServiceCharge.ToString());
            serviceList.ApprovedStatus = true;
            serviceList.IsPaidService = Convert.ToBoolean(files[0].IsPaidService.ToString());
            serviceList.ApprovedDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);
            serviceList.CreatedBy = userInformationBO.UserInfoId;
            serviceList.VatAmountPercent = Convert.ToDecimal(files[0].VatAmountPercent.ToString());
            serviceList.ServiceChargePercent = Convert.ToDecimal(files[0].ServiceChargePercent.ToString());
            serviceList.CalculatedPercentAmount = Convert.ToDecimal(files[0].CalculatedPercentAmount.ToString());

            Boolean status = roomRegistrationDA.SaveGuestServiceBillApprovedInfo(serviceList, out tmpApprovedId);
            if (status)
            {
                CommonHelper.AlertInfo(innboardMessage, "Approved Operation Successfull.", AlertType.Success);
                this.Cancel();
            }
        }
        private void CheckObjectPermission()
        {
            if (isDayClosed == false)
            {
                btnSave.Visible = isSavePermission;
                btnSaveAll.Visible = isSavePermission;
            }
            else
            {
                isSavePermission = false;
                isDeletePermission = false;
                btnSave.Visible = isSavePermission;
                btnSaveAll.Visible = isSavePermission;
            }
        }
        public bool isValidForm()
        {
            bool status = true;
            if (string.IsNullOrWhiteSpace(this.txtApprovedDate.Text))
            {
                status = false;
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Approve Date.", AlertType.Warning);
                this.txtApprovedDate.Focus();
            }

            return status;
        }
        private void LoadServiceBillGridView(int serviceBillId)
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<GHServiceBillBO> files = roomRegistrationDA.GetServiceBillNightAudit("GuestHouseService", serviceBillId, hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), this.txtSrcRoomNumber.Text);
            foreach (GHServiceBillBO row in files)
            {
                row.TotalCalculatedAmount = Math.Round(row.TotalCalculatedAmount);
            }

            this.gvGHServiceBill.DataSource = files;
            this.gvGHServiceBill.DataBind();
        }
        private void LoadRestaurantBillGridView(int serviceBillId)
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<GHServiceBillBO> files = roomRegistrationDA.GetServiceBillNightAudit("RestaurantService", serviceBillId, hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), this.txtSrcRoomNumber.Text);
            foreach (GHServiceBillBO row in files)
            {
                row.TotalCalculatedAmount = Math.Round(row.TotalCalculatedAmount);
            }

            this.gvRestaurantServiceBill.DataSource = files;
            this.gvRestaurantServiceBill.DataBind();
        }
        private void SetTab(string TabName)
        {
            if (TabName == "ServiceAuditTab")
            {
                BServiceAudit.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                ARoomAudit.Attributes.Add("class", "ui-state-default ui-corner-top");
                CRestaurantAudit.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "RoomAuditTab")
            {
                ARoomAudit.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                BServiceAudit.Attributes.Add("class", "ui-state-default ui-corner-top");
                CRestaurantAudit.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "RestaurantAuditTab")
            {
                CRestaurantAudit.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                BServiceAudit.Attributes.Add("class", "ui-state-default ui-corner-top");
                ARoomAudit.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        [WebMethod]
        public static GHServiceBillBO FillServiceForm(string GuestServiceType, int EditId)
        {
            List<GHServiceBillBO> availableGuestList = new List<GHServiceBillBO>();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            availableGuestList = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch(GuestServiceType, EditId, DateTime.Now, "");

            if (availableGuestList != null)
            {
                if (availableGuestList.Count > 0)
                {
                    return availableGuestList[0];
                }
            }
            else
            {
                GHServiceBillBO returnGHServiceBillBO = new GHServiceBillBO();
                availableGuestList.Add(returnGHServiceBillBO);
            }
            return availableGuestList[0];
        }
        [WebMethod]
        public static AvailableGuestListBO FillForm(string EditId)
        {
            List<AvailableGuestListBO> availableGuestList = new List<AvailableGuestListBO>();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            availableGuestList = roomRegistrationDA.GetAvailableGuestList(EditId, DateTime.Now, "");
            decimal serviceAmountForCompanyGuest = 0;

            foreach (AvailableGuestListBO row in availableGuestList)
            {
                RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();
                roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Convert.ToInt32(row.RegistrationId));
                if (roomRegistrationBO != null)
                {
                    if (roomRegistrationBO.RegistrationId > 0)
                    {
                        if (roomRegistrationBO.IsCompanyGuest)
                        {
                            row.BPDiscountAmount = row.RoomRate;
                            row.BPPercentAmount = 100;
                            row.CalculatedRoomRate = row.RoomRate;
                            row.VatAmount = serviceAmountForCompanyGuest;
                            row.ServiceCharge = serviceAmountForCompanyGuest;
                            row.TotalCalculatedAmount = serviceAmountForCompanyGuest;
                        }
                    }
                }
            }

            return availableGuestList[0];
        }
        [WebMethod]
        public static AvailableGuestListBO ApprovedNightAuditedDataForUpdateAction(string actionData)
        {
            List<AvailableGuestListBO> availableGuestList = new List<AvailableGuestListBO>();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            availableGuestList = roomRegistrationDA.GetApprovedNightAuditedDataForUpdate(actionData.Split('~')[0].ToString(), Convert.ToInt64(actionData.Split('~')[2].ToString()), DateTime.Now, "");
            return availableGuestList[0];
        }
        //[WebMethod]
        //public static GHServiceBillBO FillServiceFormUpdateAction(string GuestServiceType, string EditId)
        //{
        //    List<GHServiceBillBO> availableGuestList = new List<GHServiceBillBO>();
        //    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
        //    availableGuestList = roomRegistrationDA.GetApprovedServiceBillForUpdate(Convert.ToInt32(EditId.Split('~')[1].ToString()), DateTime.Now, "");

        //    if (availableGuestList != null)
        //    {
        //        if (availableGuestList.Count > 0)
        //        {
        //            return availableGuestList[0];
        //        }
        //    }
        //    else
        //    {
        //        GHServiceBillBO returnGHServiceBillBO = new GHServiceBillBO();
        //        availableGuestList.Add(returnGHServiceBillBO);
        //    }
        //    return availableGuestList[0];
        //}
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        [WebMethod]
        public static string ApprovedNightAuditedData(string serviceType, Int64 registrationId, Int64 approvedId, string txtCalculatedTotalRoomRateData, string txtCalculateRackRateData, string txtCalculateDiscountAmountData, Boolean isCbServiceChargeEnable, Boolean isCbCitySDChargeEnable, Boolean isCbVatEnable, Boolean isCbAdditionalChargeEnable)
        {
            string returnMessage = string.Empty;
            HMUtility hmUtility = new HMUtility();
            decimal calculatedTotalRoomRateData = !string.IsNullOrWhiteSpace(txtCalculatedTotalRoomRateData) ? Convert.ToDecimal(txtCalculatedTotalRoomRateData) : 0;
            
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            Boolean updateStatus = roomRegistrationDA.ApprovedNightAuditedData(serviceType, registrationId, approvedId, calculatedTotalRoomRateData, isCbServiceChargeEnable, isCbCitySDChargeEnable, isCbVatEnable, isCbAdditionalChargeEnable, userInformationBO.UserInfoId);
            if (updateStatus)
            {
                string description = string.Empty;
                if (serviceType == "GuestRoom")
                {
                    description = " For Guest Room";
                }
                else if (serviceType == "ServiceBill")
                {
                    description = " For Service Bill";
                }
                else if (serviceType == "RestaurantBill")
                {
                    description = " For Restaurant Bill";
                }

                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.UpdateApprovedNightAuditedData.ToString(), registrationId,
                                                ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), "Update Night Audited Approved Data" + description);
            }

            returnMessage = "Update Night Audited Approved Data Successfully";
            return returnMessage;
        }
        [WebMethod]
        public static ReturnInfo BillResettlement(int billId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            string sourceType = string.Empty;

            try
            {
                RestaurentBillDA billDa = new RestaurentBillDA();
                KotBillMasterDA kotBillMasterDA = new KotBillMasterDA();
                KotBillMasterBO kotBill = new KotBillMasterBO();
                List<RestaurantBillDetailBO> billDetailList = new List<RestaurantBillDetailBO>();

                //HttpContext.Current.Session.Remove("RestaurantKotBillResumeForResettlement");
                //HttpContext.Current.Session.Remove("KotResettlementBill");

                billDetailList = billDa.GetRestaurantBillDetailsByBillId(billId);

                if (billDetailList.Count > 0)
                {
                    kotBill = kotBillMasterDA.GetKotBillMasterInfoKotId(billDetailList[0].KotId);
                    //HttpContext.Current.Session["KotResettlementBill"] = kotBill;
                }

                if (kotBill.SourceName == ConstantHelper.RestaurantBillSource.RestaurantTable.ToString())
                {
                    sourceType = "tbl";
                }
                else if (kotBill.SourceName == ConstantHelper.RestaurantBillSource.RestaurantToken.ToString())
                {
                    sourceType = "tkn";
                }
                else if (kotBill.SourceName == ConstantHelper.RestaurantBillSource.GuestRoom.ToString())
                {
                    sourceType = "rom";
                }

                rtninf.IsSuccess = true;
                rtninf.RedirectUrl = string.Format("../POS/frmBillReSettlement.aspx?ot={0}&st={1}&sid={2}&cc={3}&kid={4}&dp={5}", "br", sourceType, kotBill.SourceId, kotBill.CostCenterId, kotBill.KotId, "../HotelManagement/frmNightAudit.aspx");

                //rtninf.RedirectUrl = "frmRestaurantBillReSettlement.aspx?IR=TokenAllocation&CostCenterId=" + kotBill.CostCenterId;

                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }

            return rtninf;
        }
    }
}