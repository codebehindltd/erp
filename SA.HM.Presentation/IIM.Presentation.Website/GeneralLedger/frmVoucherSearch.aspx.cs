using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.GeneralLedger;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmVoucherSearch : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
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
                innboardMessage.Value = JsonConvert.SerializeObject(CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Warning));
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchVoucher();
        }
        protected void gvVoucherInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
            this.gvVoucherInfo.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();

            string fromDate = string.Empty;
            string toDate = string.Empty;
            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                fromDate = hmUtility.GetFromDate();
            }
            else
            {
                fromDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                toDate = hmUtility.GetToDate();
            }
            else
            {
                toDate = this.txtToDate.Text;
            }


            DateTime FromDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

            //this.LoadGridView(hmUtility.GetDateTimeFromString(FromDate, hmUtility.GetDateTimeFromString(txtToDate.Text.Trim()), txtVoucherNumber.Text.Trim());
            this.LoadGridView(FromDate, ToDate, txtVoucherNumber.Text.Trim());
        }
        protected void gvVoucherInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                Label lblValue = (Label)e.Row.FindControl("lblid");
                Label lblGLStatusValue = (Label)e.Row.FindControl("lblgvGLStatus");

                Label lblCheckedByValue = (Label)e.Row.FindControl("lblCheckedBy");
                Label lblApprovedByValue = (Label)e.Row.FindControl("lblApprovedBy");
                Label lblCreatedByValue = (Label)e.Row.FindControl("lblCreatedBy");

                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");
                ImageButton imgSelect = (ImageButton)e.Row.FindControl("ImgSelect");

                imgUpdate.Attributes["onclick"] = "javascript:return PerformFillFormAction('" + lblValue.Text + "');";
                imgDelete.Attributes["onclick"] = "javascript:return PerformDeleteAction('" + lblValue.Text + "');";
                imgSelect.Attributes["onclick"] = "javascript:return AddNewItem('" + lblValue.Text + "');";

                if (lblGLStatusValue.Text != "Approved")
                {
                    imgUpdate.Visible = isSavePermission;
                    imgDelete.Visible = isDeletePermission;
                    imgSelect.Visible = isSavePermission;
                    Boolean isVisibleTrue = false;

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    if (userInformationBO.UserInfoId == 1)
                    {
                        isVisibleTrue = true;
                    }
                    else
                    {
                        if (userInformationBO.UserInfoId.ToString() == lblCreatedByValue.Text)
                        {
                            if (lblCreatedByValue.Text == lblCheckedByValue.Text && lblCheckedByValue.Text == lblApprovedByValue.Text)
                            {
                                isVisibleTrue = true;
                            }
                            else if (lblCreatedByValue.Text == lblCheckedByValue.Text)
                            {
                                if (lblGLStatusValue.Text == "Pending")
                                {
                                    isVisibleTrue = true;
                                }
                                else if (lblGLStatusValue.Text == "Checked")
                                {
                                    isVisibleTrue = false;
                                }
                            }
                            else if (lblCheckedByValue.Text == lblApprovedByValue.Text)
                            {
                                if (lblGLStatusValue.Text == "Checked")
                                {
                                    isVisibleTrue = false;
                                }
                                else if (lblGLStatusValue.Text == "Checked")
                                {
                                    isVisibleTrue = true;
                                }
                            }
                        }
                        else if (userInformationBO.UserInfoId.ToString() == lblCheckedByValue.Text)
                        {
                            if (lblGLStatusValue.Text == "Pending")
                            {
                                isVisibleTrue = true;
                            }
                            else if (lblGLStatusValue.Text == "Checked")
                            {
                                isVisibleTrue = true;
                            }
                        }
                        else if (userInformationBO.UserInfoId.ToString() == lblApprovedByValue.Text)
                        {
                            if (lblGLStatusValue.Text == "Checked")
                            {
                                isVisibleTrue = true;
                            }
                        }
                    }

                    ((CheckBox)e.Row.FindControl("chkbox")).Visible = isVisibleTrue;
                    if (isSavePermission)
                    {
                        imgSelect.Visible = isVisibleTrue;
                    }
                }
                else
                {
                    ((CheckBox)e.Row.FindControl("chkbox")).Visible = false;
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                    imgSelect.Visible = false;
                }
            }
        }
        protected void gvVoucherInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdPreview")
            {
                int DealId = Convert.ToInt32(e.CommandArgument.ToString());
                //Response.Redirect("Reports/frmReportVoucherInfo.aspx?DealId= " + DealId);
                string url = "Reports/frmReportVoucherInfo.aspx?DealId= " + DealId;
                string sPopUp = "window.open('" + url + "', 'popup_window', 'width=715,height=780,left=300,top=50,resizable=yes, scrollbars=1');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", sPopUp, true);
            }
            else if (e.CommandName == "CmdEdit")
            {
                int DealId = Convert.ToInt32(e.CommandArgument.ToString());
                Response.Redirect("frmVoucher.aspx?DealId= " + DealId);

            }
        }
        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {
            int dealId = Int32.Parse(txtReportId.Value);
            string glStatus = ddlChangeStatus.SelectedItem.Text;
            NodeMatrixDA matrixDA = new NodeMatrixDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            Boolean status = matrixDA.UpdateGLDealMasterStatus(dealId, glStatus, userInformationBO.UserInfoId);
            if (status)
            {
                innboardMessage.Value = JsonConvert.SerializeObject(CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Warning));
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.LedgerDealStatus.ToString(), dealId,
                ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LedgerDealStatus));
            }
            SearchVoucher();
        }
        protected void btnApproved_Click(object sender, EventArgs e)
        {
            NodeMatrixDA matrixDA = new NodeMatrixDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            foreach (GridViewRow grRow in gvVoucherInfo.Rows)
            {
                CheckBox chkItem = (CheckBox)grRow.FindControl("chkBox");
                if (chkItem.Checked)
                {
                    string id = ((Label)grRow.FindControl("lblid")).Text.ToString();
                    string gvGLStatus = ((Label)grRow.FindControl("lblgvGLStatus")).Text.ToString();
                    Boolean status;
                    if (userInformationBO.UserGroupId == 1)
                    {
                        status = matrixDA.UpdateGLDealMasterStatus(Int32.Parse(id), "Approved", userInformationBO.UserInfoId);
                    }
                    else
                    {
                        if (gvGLStatus == "Pending")
                        {
                            status = matrixDA.UpdateGLDealMasterStatus(Int32.Parse(id), "Checked", userInformationBO.UserInfoId);
                        }
                        if (gvGLStatus == "Checked")
                        {
                            status = matrixDA.UpdateGLDealMasterStatus(Int32.Parse(id), "Approved", userInformationBO.UserInfoId);
                        }
                    }
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.LedgerDealStatus.ToString(), Int32.Parse(id),
                    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.LedgerDealStatus));
                }
            }
            SearchVoucher();
        }
        //************************ User Defined Function ********************//
        private void SearchVoucher()
        {
            string fromDate = string.Empty;
            string toDate = string.Empty;
            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                fromDate = hmUtility.GetFromDate();
            }
            else
            {
                fromDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                toDate = hmUtility.GetToDate();
            }
            else
            {
                toDate = this.txtToDate.Text;
            }


            DateTime FromDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            string VoucherNo = txtVoucherNumber.Text.Trim();
            LoadGridView(FromDate, ToDate, VoucherNo);
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmVoucherSearch.ToString());
            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        private void LoadGridView(DateTime FromDate, DateTime ToDate, string VoucherNo)
        {
            this.CheckObjectPermission();
            NodeMatrixDA matrixDA = new NodeMatrixDA();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            this.gvVoucherInfo.DataSource = matrixDA.GetVoucherInfoBySearchCriteria(FromDate, ToDate, VoucherNo, userInformationBO.UserInfoId);
            this.gvVoucherInfo.DataBind();
        }
        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static string DeleteData(int DealId)
        {
            string result = string.Empty;
            HMUtility hmUtility = new HMUtility();
            try
            {
                NodeMatrixDA matrixDA = new NodeMatrixDA();
                Boolean status = matrixDA.DeleteVoucherInfoById(DealId);
                Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.GeneralLedgerVoucher.ToString(), DealId,
                    ProjectModuleEnum.ProjectModule.GeneralLedger.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.GeneralLedgerVoucher));
                if (status)
                {
                    result = "success";
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Data Deleted Failed.";
                throw ex;
            }
            return result;
        }
        [WebMethod]
        public static string SetSelected(int DealId)
        {
            string returnString = "Pending";
            NodeMatrixDA matrixDA = new NodeMatrixDA();
            GLDealMasterBO masterBO = new GLDealMasterBO();
            masterBO = matrixDA.GetDealMasterInfoByDealId(DealId);
            if (masterBO.DealId > 0)
            {
                if (masterBO.GLStatus != null)
                {
                    returnString = masterBO.GLStatus;
                }
                else
                {
                    returnString = "Pending";
                }
            }

            return returnString;
        }
    }
}