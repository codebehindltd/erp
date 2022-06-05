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

namespace HotelManagement.Presentation.Website.HotelManagement
{
    public partial class frmAvailableGuestList : System.Web.UI.Page
    {
        int _ApprovedId;
        Boolean isAllApproved = false;
        protected int isMessageBoxEnable = -1;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        private Boolean isViewPermission = false;
        Boolean isDayClosed = false;
        HMUtility hmUtility = new HMUtility();
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {

            this.IsDayClosed();
            if (!IsPostBack)
            {
                this.LoadCurrentDate();
                this.LoadGridView("0");
                this.LoadGHServiceBillInfoGridView(0);
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
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Approved Operation Successfull.";
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
                        this.isMessageBoxEnable = 2;
                        lblMessage.Text = "Update Operation Successfull.";
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ApproveAvailableGuest.ToString(), tmpApprovedId,
                            ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ApproveAvailableGuest));
                        this.LoadGridView("0");
                        this.Cancel();
                    }
                }
            }
        }
        protected void gvEarlyCheckInAvailableGuestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvAvailableGuestList.PageIndex = e.NewPageIndex;
            this.LoadGridView("0");
        }
        protected void gvAvailableGuestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvAvailableGuestList.PageIndex = e.NewPageIndex;
            this.LoadGridView("0");
        }
        protected void gvEarlyCheckInAvailableGuestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label bpPercentAmount = (Label)e.Row.FindControl("lblgvBPPercentAmount");
                Label bpDiscountAmount = (Label)e.Row.FindControl("lblgvBPDiscountAmount");
                Label bpDisplayDiscountAmount = (Label)e.Row.FindControl("lblgvDisplayBPDiscountAmount");
                bpDisplayDiscountAmount.Text = bpDiscountAmount.Text + "(" + bpPercentAmount.Text + "%)";
                Label bpRoomRateAmount = (Label)e.Row.FindControl("lblgvRoomRate");
                //Label bpCalculatedAmount = (Label)e.Row.FindControl("lblgvCalculatedRoomRateAmount");
                ////bpCalculatedAmount.Text = (Convert.ToDecimal(bpRoomRateAmount.Text) - Convert.ToDecimal(bpDiscountAmount.Text)).ToString();
                //bpCalculatedAmount.Text = bpDiscountAmount.Text;

                //--------------------
                Label lblValue = (Label)e.Row.FindControl("lblid");
                Label lblApprovedIdValue = (Label)e.Row.FindControl("lblApprovedId");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgApprove = (ImageButton)e.Row.FindControl("ImgApprove");
                ImageButton imgApproved = (ImageButton)e.Row.FindControl("ImgApproved");
                ImageButton imgEdit = (ImageButton)e.Row.FindControl("ImgEdit");

                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //imgApprove.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                imgEdit.Attributes["onclick"] = "javascript:return PerformFillFormForUpdateAction('" + lblValue.Text + "~" + lblApprovedIdValue.Text + "');";

                imgUpdate.Visible = isSavePermission;
                imgApprove.Visible = isSavePermission;

                Label lblActiveStatValue = (Label)e.Row.FindControl("lblApprovedStatus");
                if (lblActiveStatValue.Text == "False")
                {
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

                if (!string.IsNullOrWhiteSpace(lblValue.Text))
                {
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                    RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();

                    roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Convert.ToInt32(lblValue.Text));
                    if (roomRegistrationBO != null)
                    {
                        if (roomRegistrationBO.RegistrationId > 0)
                        {
                            if (roomRegistrationBO.IsCompanyGuest)
                            {
                                int approvedId = !string.IsNullOrWhiteSpace(lblApprovedIdValue.Text) ? Convert.ToInt32(lblApprovedIdValue.Text) : 0;
                                if (approvedId != 0)
                                {
                                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = true;
                                }
                                else
                                {
                                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;
                                }
                            }
                        }
                    }

                }

                if (isDayClosed)
                {
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = false;

                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;
                }
            }

            //if (isAllApproved)
            //{
            //    if (isDayClosed)
            //    {
            //        this.btnSaveAll.Visible = false;
            //    }
            //    else
            //    {
            //        this.btnSaveAll.Visible = isSavePermission;
            //    }
            //}
            //else
            //{
            //    this.btnSaveAll.Visible = false;
            //    //RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            //    //Boolean status = roomRegistrationDA.SaveRoomStatusHistoryInfo();
            //}
        }
        protected void gvAvailableGuestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {

                Label bpPercentAmount = (Label)e.Row.FindControl("lblgvBPPercentAmount");
                Label bpDiscountAmount = (Label)e.Row.FindControl("lblgvBPDiscountAmount");
                Label bpDisplayDiscountAmount = (Label)e.Row.FindControl("lblgvDisplayBPDiscountAmount");
                bpDisplayDiscountAmount.Text = bpDiscountAmount.Text + "(" + bpPercentAmount.Text + "%)";
                Label bpRoomRateAmount = (Label)e.Row.FindControl("lblgvRoomRate");
                //Label bpCalculatedAmount = (Label)e.Row.FindControl("lblgvCalculatedRoomRateAmount");
                ////bpCalculatedAmount.Text = (Convert.ToDecimal(bpRoomRateAmount.Text) - Convert.ToDecimal(bpDiscountAmount.Text)).ToString();
                //bpCalculatedAmount.Text = bpDiscountAmount.Text;

                //--------------------
                Label lblValue = (Label)e.Row.FindControl("lblid");
                Label lblApprovedIdValue = (Label)e.Row.FindControl("lblApprovedId");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgApprove = (ImageButton)e.Row.FindControl("ImgApprove");
                ImageButton imgApproved = (ImageButton)e.Row.FindControl("ImgApproved");
                ImageButton imgEdit = (ImageButton)e.Row.FindControl("ImgEdit");

                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                //imgApprove.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                imgEdit.Attributes["onclick"] = "javascript:return PerformFillFormForUpdateAction('" + lblValue.Text + "~" + lblApprovedIdValue.Text + "');";

                imgUpdate.Visible = isSavePermission;
                imgApprove.Visible = isSavePermission;

                Label lblActiveStatValue = (Label)e.Row.FindControl("lblApprovedStatus");
                if (lblActiveStatValue.Text == "False")
                {
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

                if (!string.IsNullOrWhiteSpace(lblValue.Text))
                {
                    RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                    RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();

                    roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Convert.ToInt32(lblValue.Text));
                    if (roomRegistrationBO != null)
                    {
                        if (roomRegistrationBO.RegistrationId > 0)
                        {
                            if (roomRegistrationBO.IsCompanyGuest)
                            {
                                int approvedId = !string.IsNullOrWhiteSpace(lblApprovedIdValue.Text) ? Convert.ToInt32(lblApprovedIdValue.Text) : 0;
                                if (approvedId != 0)
                                {
                                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = true;
                                }
                                else
                                {
                                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;
                                }
                            }
                        }
                    }

                }

                if (isDayClosed)
                {
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = false;

                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;
                }
            }

            if (isAllApproved)
            {
                if (isDayClosed)
                {
                    this.btnSaveAll.Visible = false;
                }
                else
                {
                    this.btnSaveAll.Visible = isSavePermission;
                }
            }
            else
            {
                this.btnSaveAll.Visible = false;
                //RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                //Boolean status = roomRegistrationDA.SaveRoomStatusHistoryInfo();
            }
        }
        protected void gvEarlyCheckInAvailableGuestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdApprove")
            {
                try
                {
                    btnSave.Text = "Update";
                    this._ApprovedId = Convert.ToInt32(e.CommandArgument.ToString());
                    ApprovedGuest(this._ApprovedId.ToString(), hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat).AddDays(-1));
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
                        this.ApprovedIdHiddenField.Value = string.Empty;
                        this.ServiceApprovedIdHiddenField.Value = string.Empty;
                        this.CheckObjectPermission();
                        this.LoadGridView("0");
                        this.LoadGHServiceBillInfoGridView(0);
                        //result = "success";
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
        protected void gvAvailableGuestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdApprove")
            {
                try
                {
                    btnSave.Text = "Update";
                    this._ApprovedId = Convert.ToInt32(e.CommandArgument.ToString());
                    ApprovedGuest(this._ApprovedId.ToString(), hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat));
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
                        this.ApprovedIdHiddenField.Value = string.Empty;
                        this.ServiceApprovedIdHiddenField.Value = string.Empty;
                        this.CheckObjectPermission();
                        this.LoadGridView("0");
                        this.LoadGHServiceBillInfoGridView(0);
                        //result = "success";
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
            this.IsDayClosed();
            this.ApprovedIdHiddenField.Value = string.Empty;
            this.ServiceApprovedIdHiddenField.Value = string.Empty;
            this.CheckObjectPermission();
            this.LoadGridView("0");
            this.LoadGHServiceBillInfoGridView(0);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtRegistrationId.Value = string.Empty;
        }
        protected void gvGHServiceBill_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.lblMessage.Text = string.Empty;
            this.gvGHServiceBill.PageIndex = e.NewPageIndex;
            this.LoadGHServiceBillInfoGridView(0);
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
                imgEdit.Attributes["onclick"] = "javascript:return PerformFillFormServiceForUpdateAction('" + lblValue.Text + "~" + lblApprovedIdValue.Text + "');";

                //imgUpdate.Visible = isSavePermission;
                Label lblRoomNumberValue = (Label)e.Row.FindControl("lblRoomNumber");

                Label lblActiveStatValue = (Label)e.Row.FindControl("lblServiceApprovedStatus");
                if (lblActiveStatValue.Text == "False")
                {
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

                if (isDayClosed)
                {
                    ((ImageButton)e.Row.FindControl("ImgApproved")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgApprove")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgUpdate")).Visible = false;
                    ((CheckBox)e.Row.FindControl("CheckBoxAccept")).Visible = false;

                    ((ImageButton)e.Row.FindControl("ImgEdit")).Visible = false;
                    ((ImageButton)e.Row.FindControl("ImgDelete")).Visible = false;
                }
            }

            if (isDayClosed)
            {
                this.btnAllServiceBillApprove.Visible = false;
            }
            else
            {
                this.btnAllServiceBillApprove.Visible = isSavePermission;
            }
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
            int counterEarlyCheckIn = 0;
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
                        availableGuestList.RoomType = ((Label)row.FindControl("lblgvRoomType")).Text;
                        availableGuestList.RoomRate = Convert.ToDecimal(((Label)row.FindControl("lblgvCalculatedRoomRateAmount")).Text);
                        availableGuestList.BPPercentAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvBPPercentAmount")).Text);
                        availableGuestList.BPDiscountAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvBPDiscountAmount")).Text);
                        availableGuestList.VatAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvVatAmount")).Text);
                        availableGuestList.ServiceCharge = Convert.ToDecimal(((Label)row.FindControl("lblgvServiceCharge")).Text);
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

            foreach (GridViewRow row in gvEarlyCheckInAvailableGuestList.Rows)
            {
                counterEarlyCheckIn = counterEarlyCheckIn + 1;
                bool isAcceptEarlyCheckIn = ((CheckBox)row.FindControl("CheckBoxAccept")).Checked;
                if (isAcceptEarlyCheckIn)
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
                        availableGuestList.RoomType = ((Label)row.FindControl("lblgvRoomType")).Text;
                        availableGuestList.RoomRate = Convert.ToDecimal(((Label)row.FindControl("lblgvCalculatedRoomRateAmount")).Text);
                        availableGuestList.BPPercentAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvBPPercentAmount")).Text);
                        availableGuestList.BPDiscountAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvBPDiscountAmount")).Text);
                        availableGuestList.VatAmount = Convert.ToDecimal(((Label)row.FindControl("lblgvVatAmount")).Text);
                        availableGuestList.ServiceCharge = Convert.ToDecimal(((Label)row.FindControl("lblgvServiceCharge")).Text);
                        availableGuestList.ReferenceSalesCommission = Convert.ToDecimal(((Label)row.FindControl("lblgvReferenceSalesCommission")).Text);
                        availableGuestList.PreviousRoomRate = Convert.ToDecimal(((Label)row.FindControl("lblgvRoomRate")).Text);
                        availableGuestList.ApprovedStatus = true;
                        availableGuestList.ServiceDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat).AddDays(-1);
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


            if (gvAvailableGuestList.Rows.Count == counter)
            {
                this.isMessageBoxEnable = 2;
                //RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                //Boolean status = roomRegistrationDA.SaveRoomStatusHistoryInfo();
                lblMessage.Text = "Approved Operation Successfull.";
            }
            this.LoadGridView("0");
            this.SetTab("RoomAuditTab");
        }
        protected void btnAllServiceBillApproved_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {
                return;
            }
            //int tmpApprovedId = 0;
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

                        //UserInformationBO userInformationBO = new UserInformationBO();
                        userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                        GHServiceBillBO serviceList = new GHServiceBillBO();
                        RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();


                        //List<GHServiceBillBO> files = roomRegistrationDA.GetGHServiceBillInfoForNightAudit(rowId);
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

            //if (gvGHServiceBill.Rows.Count == counter)
            //{
            //    //this.isMessageBoxEnable = 2;
            //    //RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            //    //Boolean status = roomRegistrationDA.SaveRoomStatusHistoryInfo();
            //    //lblMessage.Text = "Approved Operation Successfull";
            //}
            this.LoadGHServiceBillInfoGridView(0);
            this.SetTab("ServiceAuditTab");
        }
        protected void gvGHServiceBill_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdApprove")
            {
                try
                {
                    this._ApprovedId = Convert.ToInt32(e.CommandArgument.ToString());
                    ApprovedGuestService(this._ApprovedId);
                    this._ApprovedId = Convert.ToInt32(e.CommandArgument.ToString());
                    Session["_ServiceApprovedId"] = this._ApprovedId;
                    //this.Cancel();
                    //this.LoadSerGridView(0);
                    this.SetTab("ServiceAuditTab");
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
            //int tmpApprovedId = 0;
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
            this.LoadGHServiceBillInfoGridView(0);
            this.SetTab("ServiceAuditTab");
        }
        protected void btnUpdateApprovedService_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(ServiceApprovedIdHiddenField.Value))
            {
                //int tmpApprovedId = 0;
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

                serviceList.ApprovedStatus = true;
                serviceList.ApprovedDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);

                if (!string.IsNullOrWhiteSpace(ServiceApprovedIdHiddenField.Value))
                {
                    serviceList.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = roomRegistrationDA.UpdateGuestServiceBillApprovedInfo(serviceList);
                    if (status)
                    {
                        this.isMessageBoxEnable = 2;
                        lblMessage.Text = "Update Operation Successfull.";
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ApproveAvailableGuest.ToString(), tmpApprovedId,
                             ProjectModuleEnum.ProjectModule.FrontOffice.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ApproveAvailableGuest));
                        this.SetTab("ServiceAuditTab");
                    }
                }
            }
        }
        //************************ User Defined Function ********************//
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
        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtApprovedDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
        private void LoadGridView(string registrationId)
        {
            this.CheckObjectPermission();
            if (!string.IsNullOrWhiteSpace(this.txtApprovedDate.Text))
            {
                RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
                List<AvailableGuestListBO> files = roomRegistrationDA.GetAvailableGuestList(registrationId, hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), this.txtSrcRoomNumber.Text);
                decimal serviceAmountForCompanyGuest = 0;

                foreach (AvailableGuestListBO row in files)
                {
                    RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();

                    roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Convert.ToInt32(row.RegistrationId));
                    if (roomRegistrationBO != null)
                    {
                        if (roomRegistrationBO.RegistrationId > 0)
                        {
                            if (roomRegistrationBO.IsCompanyGuest)
                            {
                                //row.RoomRate = serviceAmountForCompanyGuest;
                                //row.VatAmount = serviceAmountForCompanyGuest;
                                //row.ServiceCharge = serviceAmountForCompanyGuest;
                                //row.TotalCalculatedAmount = serviceAmountForCompanyGuest;
                                //row.BPDiscountAmount = serviceAmountForCompanyGuest;
                                //row.BPPercentAmount = serviceAmountForCompanyGuest;
                                //row.CalculatedRoomRate = serviceAmountForCompanyGuest;

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

                //string mamun = "M amun";
                //mamun.Split(' ');

                //List<AvailableGuestListBO> earlyCheckInGuestList = files.Where(x => x.RoomNumber.Split(' ').Count() == 0).ToList(); 
                //List<AvailableGuestListBO> earlyCheckInGuestList = files.Where(x => x.RoomNumber.Contains(' ').Count() == 0).ToList();

                List<AvailableGuestListBO> earlyCheckInGuestList = new List<AvailableGuestListBO>();
                List<AvailableGuestListBO> notEarlyCheckInGuestList = new List<AvailableGuestListBO>();
                foreach (AvailableGuestListBO row in files)
                {
                    if (row.RoomNumber.ToString().Split(' ').Count() > 1)
                    {
                        earlyCheckInGuestList.Add(row);
                    }
                    else
                    {
                        notEarlyCheckInGuestList.Add(row);
                    }

                }

                this.gvEarlyCheckInAvailableGuestList.DataSource = earlyCheckInGuestList;
                this.gvEarlyCheckInAvailableGuestList.DataBind();
                this.gvAvailableGuestList.DataSource = notEarlyCheckInGuestList;
                this.gvAvailableGuestList.DataBind();

                //this.gvAvailableGuestList.DataSource = files;
                //this.gvAvailableGuestList.DataBind();
            }
            else
            {
                this.gvEarlyCheckInAvailableGuestList.DataSource = null;
                this.gvEarlyCheckInAvailableGuestList.DataBind();
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
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Please provide Room Rate.";
                txtRoomRate.Focus();
                status = false;
            }
            else if (!Decimal.TryParse(roomRate, out number))
            {
                this.isMessageBoxEnable = 1;
                this.lblMessage.Text = "Room Rate is not in a correct format.";
                txtRoomRate.Focus();
                status = false;
            }
            //else if (number < 1)
            //{
            //    this.isMessageBoxEnable = 1;
            //    this.lblMessage.Text = "Room Rate must not be zero or less then zero.";
            //    txtRoomRate.Focus();
            //    status = false;
            //}

            return status;

        }
        public void ApprovedGuest(string rowId, DateTime serviceDate)
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

            List<AvailableGuestListBO> files = roomRegistrationDA.GetAvailableGuestList(rowId, hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat), this.txtSrcRoomNumber.Text);

            decimal serviceAmountForCompanyGuest = 0;

            foreach (AvailableGuestListBO row in files)
            {
                RoomRegistrationBO roomRegistrationBO = new RoomRegistrationBO();

                roomRegistrationBO = roomRegistrationDA.GetRoomRegistrationInfoById(Convert.ToInt32(row.RegistrationId));
                if (roomRegistrationBO != null)
                {
                    if (roomRegistrationBO.RegistrationId > 0)
                    {
                        if (roomRegistrationBO.IsCompanyGuest)
                        {
                            //row.RoomRate = serviceAmountForCompanyGuest;
                            //row.VatAmount = serviceAmountForCompanyGuest;
                            //row.ServiceCharge = serviceAmountForCompanyGuest;
                            //row.TotalCalculatedAmount = serviceAmountForCompanyGuest;
                            //row.BPDiscountAmount = serviceAmountForCompanyGuest;
                            //row.BPPercentAmount = serviceAmountForCompanyGuest;
                            //row.CalculatedRoomRate = serviceAmountForCompanyGuest;

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

            availableGuestList.RegistrationId = Int32.Parse(files[0].RegistrationId.ToString());
            availableGuestList.RoomId = Int32.Parse(files[0].RoomId.ToString());
            availableGuestList.RoomNumber = Convert.ToInt32(files[0].RoomNumber.ToString());
            availableGuestList.GuestName = files[0].GuestName.ToString();
            availableGuestList.RoomType = files[0].RoomType.ToString();
            decimal calculateAmount = Convert.ToDecimal(files[0].CalculatedRoomRate.ToString());
            availableGuestList.RoomRate = calculateAmount;
            availableGuestList.BPPercentAmount = Convert.ToDecimal(files[0].BPPercentAmount.ToString());
            availableGuestList.BPDiscountAmount = Convert.ToDecimal(files[0].BPDiscountAmount.ToString());
            availableGuestList.PreviousRoomRate = Convert.ToDecimal(files[0].PreviousRoomRate.ToString());
            availableGuestList.VatAmount = Convert.ToDecimal(files[0].VatAmount.ToString());
            availableGuestList.ServiceCharge = Convert.ToDecimal(files[0].ServiceCharge.ToString());
            availableGuestList.ReferenceSalesCommission = Convert.ToDecimal(files[0].ReferenceSalesCommission.ToString());
            availableGuestList.ApprovedStatus = true;
            availableGuestList.ServiceDate = serviceDate;
            availableGuestList.ApprovedDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);
            availableGuestList.CreatedBy = userInformationBO.UserInfoId;

            availableGuestList.VatAmountPercent = Convert.ToDecimal(files[0].VatAmountPercent.ToString());
            availableGuestList.ServiceChargePercent = Convert.ToDecimal(files[0].ServiceChargePercent.ToString());
            availableGuestList.CalculatedPercentAmount = Convert.ToDecimal(files[0].CalculatedPercentAmount.ToString());


            Boolean status = roomRegistrationDA.SaveGuestBillApprovedInfo(availableGuestList, out tmpApprovedId);
            if (status)
            {
                this.isMessageBoxEnable = 2;
                lblMessage.Text = "Approved Operation Successfull.";
                this.LoadGridView("0");
                this.Cancel();
            }
        }
        public void ApprovedGuestService(int rowId)
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


            //List<GHServiceBillBO> files = roomRegistrationDA.GetGHServiceBillInfoForNightAudit(rowId);
            List<GHServiceBillBO> files = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch("", rowId, hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat), this.txtSrcRoomNumber.Text);
            serviceList.PaymentMode = files[0].RoomNumber.ToString();
            serviceList.RegistrationId = Int32.Parse(files[0].RegistrationId.ToString());
            serviceList.ServiceBillId = Int32.Parse(files[0].ServiceBillId.ToString());
            serviceList.ServiceDate = hmUtility.GetDateTimeFromString(files[0].ServiceDate.ToString(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat); //hmUtility.GetDateTimeFromString(files[0].ServiceDate.ToString());
            serviceList.ServiceType = files[0].ServiceType.ToString();
            serviceList.ServiceId = Int32.Parse(files[0].ServiceId.ToString());
            serviceList.ServiceName = files[0].ServiceName.ToString();
            serviceList.ServiceQuantity = Convert.ToDecimal(files[0].ServiceQuantity.ToString());
            serviceList.ServiceRate = Convert.ToDecimal(files[0].ServiceRate.ToString());
            serviceList.DiscountAmount = Convert.ToDecimal(files[0].DiscountAmount.ToString());

            serviceList.VatAmount = Convert.ToDecimal(files[0].VatAmount.ToString());
            serviceList.ServiceCharge = Convert.ToDecimal(files[0].ServiceCharge.ToString());

            serviceList.ApprovedStatus = true;
            serviceList.ApprovedDate = hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, userInformationBO.ServerDateFormat);
            serviceList.CreatedBy = userInformationBO.UserInfoId;


            serviceList.VatAmountPercent = Convert.ToDecimal(files[0].VatAmountPercent.ToString());
            serviceList.ServiceChargePercent = Convert.ToDecimal(files[0].ServiceChargePercent.ToString());
            serviceList.CalculatedPercentAmount = Convert.ToDecimal(files[0].CalculatedPercentAmount.ToString());

            Boolean status = roomRegistrationDA.SaveGuestServiceBillApprovedInfo(serviceList, out tmpApprovedId);
            if (status)
            {
                this.isMessageBoxEnable = 2;
                lblMessage.Text = "Approved Operation Successfull.";
                this.LoadGHServiceBillInfoGridView(0);
                this.Cancel();
            }
        }
        private void CheckObjectPermission()
        {
            if (isDayClosed == false)
            {
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
                objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmAvailableGuestList.ToString());

                isSavePermission = objectPermissionBO.IsSavePermission;
                isDeletePermission = objectPermissionBO.IsDeletePermission;
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
                this.isMessageBoxEnable = 1;
                lblMessage.Text = "Please Provide Approve Date.";
                this.txtApprovedDate.Focus();
            }

            return status;

        }
        private void LoadGHServiceBillInfoGridView(int serviceBillId)
        {
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            List<GHServiceBillBO> files = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch("", serviceBillId, hmUtility.GetDateTimeFromString(this.txtApprovedDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat), this.txtSrcRoomNumber.Text);

            this.gvGHServiceBill.DataSource = files;
            this.gvGHServiceBill.DataBind();
        }
        private void SetTab(string TabName)
        {
            if (TabName == "ServiceAuditTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "RoomAuditTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        [WebMethod]
        public static GHServiceBillBO FillServiceForm(int EditId)
        {
            List<GHServiceBillBO> availableGuestList = new List<GHServiceBillBO>();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            //availableGuestList = roomRegistrationDA.GetGHServiceBillInfoForNightAudit(EditId);
            availableGuestList = roomRegistrationDA.GetGHServiceBillInfoForNightAuditSearch("", EditId, DateTime.Now, "");

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
                            //row.RoomRate = serviceAmountForCompanyGuest;
                            //row.VatAmount = serviceAmountForCompanyGuest;
                            //row.ServiceCharge = serviceAmountForCompanyGuest;
                            //row.TotalCalculatedAmount = serviceAmountForCompanyGuest;
                            //row.BPDiscountAmount = serviceAmountForCompanyGuest;
                            //row.BPPercentAmount = serviceAmountForCompanyGuest;
                            //row.CalculatedRoomRate = serviceAmountForCompanyGuest;

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
        public static AvailableGuestListBO FillFormUpdateAction(string EditId)
        {
            List<AvailableGuestListBO> availableGuestList = new List<AvailableGuestListBO>();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            availableGuestList = roomRegistrationDA.GetApprovedNightAuditedDataForUpdate("GuestRoom", Convert.ToInt64(EditId.Split('~')[1].ToString()), DateTime.Now, "");

            return availableGuestList[0];
        }
        [WebMethod]
        public static GHServiceBillBO FillServiceFormUpdateAction(string EditId)
        {
            List<GHServiceBillBO> availableGuestList = new List<GHServiceBillBO>();
            RoomRegistrationDA roomRegistrationDA = new RoomRegistrationDA();
            //availableGuestList = roomRegistrationDA.GetGHServiceBillInfoForNightAudit(EditId);
            availableGuestList = roomRegistrationDA.GetApprovedServiceBillForUpdate(Convert.ToInt32(EditId.Split('~')[1].ToString()), DateTime.Now, "");

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
        protected string GetStringFromDateTime(DateTime dateTime)
        {

            return dateTime.ToString(hmUtility.GetFormat(true));
        }
    }
}