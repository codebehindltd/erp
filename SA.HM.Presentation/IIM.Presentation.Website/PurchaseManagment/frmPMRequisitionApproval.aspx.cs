using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.PurchaseManagment;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.PurchaseManagment
{
    public partial class frmRequisitionApproval : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                this.IsRequisitionCheckedByEnable();
                //this.btnApprove.Visible = false;
            }
        }
        protected void gvRequisitionInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CmdEdit")
            {
                int RequisitionId = Convert.ToInt32(e.CommandArgument.ToString());
                PMRequisitionDA requisitionDA = new PMRequisitionDA();
                PMRequisitionBO requisitionBO = new PMRequisitionBO();
                requisitionBO = requisitionDA.GetPMRequisitionInfoByID(RequisitionId);
            }
            else if (e.CommandName == "CmdDelete")
            {
                int RequisitionId = Convert.ToInt32(e.CommandArgument.ToString());
                string result = string.Empty;

                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("PMRequisition", "RequisitionId", RequisitionId);
                if (status)
                {
                }
            }
            else if (e.CommandName == "CmdReportRI")
            {
                int RequisitionId = Convert.ToInt32(e.CommandArgument.ToString());

                string url = "/PurchaseManagment/Reports/frmReportRequisitionOrderInvoice.aspx?RequisitionId=" + RequisitionId;
                string s = "window.open('" + url + "', 'popup_window', 'width=800,height=680,left=300,top=50,resizable=yes');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
        }
        protected void gvRequisitionInfo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvRequisitionInfo.PageIndex = e.NewPageIndex;
            this.CheckObjectPermission();
        }
        protected void gvRequisitionInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                //Label lblValue = (Label)e.Row.FindControl("lblid");
                //ImageButton imgRequisitionDetails = (ImageButton)e.Row.FindControl("ImgRequisitionDetails");
                //Label lblApprovedStatusValue = (Label)e.Row.FindControl("lblApprovedStatus");

                //if (lblApprovedStatusValue.Text != "Approved")
                //{
                //    imgRequisitionDetails.Visible = true;
                //}
                //else
                //{
                //    imgRequisitionDetails.Visible = false;
                //}
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGridview();
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            //UserInformationBO userInformationBO = new UserInformationBO();
            //userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            //int currentUserIs = userInformationBO.UserInfoId;
            //int count = 0;
            //foreach (GridViewRow row in gvRequisitionInfo.Rows)
            //{
            //    bool isAccept = ((CheckBox)row.FindControl("CheckBoxAccept")).Checked;
            //    if (isAccept)
            //    {
            //        PMRequisitionDA requisitionDA = new PMRequisitionDA();
            //        int requisitionId = Convert.ToInt32(((Label)row.FindControl("lblid")).Text);
            //        Boolean status = requisitionDA.ApprovePMRequisitionInfo(requisitionId, "Approved", currentUserIs);
            //        if (status)
            //        {
            //            count = count + 1;
            //        }
            //    }
            //}

            //if (count > 0)
            //{
            //    CommonHelper.AlertInfo(innboardMessage, "Requisition Approval Successfull", AlertType.Success);
            //    LoadGridview();
            //}

        }
        //************************ User Defined Function ********************//
        private void IsRequisitionCheckedByEnable()
        {
            pnlStatus.Visible = false;
            hfIsRequisitionCheckedByEnable.Value = "1";
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO checkNApproveBO = new HMCommonSetupBO();
            checkNApproveBO = commonSetupDA.GetCommonConfigurationInfo("IsRequisitionCheckedByEnable", "IsRequisitionCheckedByEnable");
            if (Convert.ToInt32(checkNApproveBO.SetupValue) != 1)
            {
                hfIsRequisitionCheckedByEnable.Value = "0";
                this.ddlStatus.Items.Remove(ddlStatus.Items.FindByValue(HMConstants.ApprovalStatus.Checked.ToString()));
                pnlStatus.Visible = true;
            }

        }
        private void LoadGridview()
        {
            this.CheckObjectPermission();
            PMRequisitionDA requisitionDA = new PMRequisitionDA();
            List<PMRequisitionBO> allrequisitionList = new List<PMRequisitionBO>();

            string startDate = string.Empty;
            string endDate = string.Empty;
            DateTime dateTime = DateTime.Now;

            if (string.IsNullOrWhiteSpace(this.txtFromDate.Text))
            {
                startDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                startDate = this.txtFromDate.Text;
            }
            if (string.IsNullOrWhiteSpace(this.txtToDate.Text))
            {
                endDate = hmUtility.GetStringFromDateTime(dateTime);
                this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            }
            else
            {
                endDate = this.txtToDate.Text;
            }
            DateTime FromDate = hmUtility.GetDateTimeFromString(startDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(endDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            if (this.ddlDateType.SelectedIndex == 0)
            {
                allrequisitionList = requisitionDA.GetPMRequisitionInfoBySearchCriteria(FromDate, ToDate, this.ddlStatus.SelectedItem.Text, "Pending");
            }
            else if (this.ddlDateType.SelectedIndex == 1)
            {
                allrequisitionList = requisitionDA.GetPMRequisitionInfoBySearchCriteria(FromDate, ToDate, this.ddlStatus.SelectedItem.Text, "CreatedDate");
            }
            else if (this.ddlDateType.SelectedIndex == 2)
            {
                allrequisitionList = requisitionDA.GetPMRequisitionInfoBySearchCriteria(FromDate, ToDate, this.ddlStatus.SelectedItem.Text, "ReceivedDate");
            }

            if (hfIsRequisitionCheckedByEnable.Value == "1")
            {
                foreach (PMRequisitionBO row in allrequisitionList)
                {
                    if (row.ApprovedStatus == "Partial Checked")
                    {
                        row.ApprovedStatus = "Pending";
                    }

                    if (row.ApprovedStatus == "Partial Approved")
                    {
                        row.ApprovedStatus = "Checked";
                    }

                    if (row.ApprovedStatus == HMConstants.ApprovalStatus.Pending.ToString())
                    {
                        row.ApprovedStatus = HMConstants.ApprovalStatus.Submit.ToString();
                    }
                }

                List<PMRequisitionBO> userWiseCreatedByrequisitionList = new List<PMRequisitionBO>();
                List<PMRequisitionBO> userWiseCheckedByrequisitionList = new List<PMRequisitionBO>();
                List<PMRequisitionBO> userWiseApprovedByrequisitionList = new List<PMRequisitionBO>();

                if (hmUtility.GetCurrentApplicationUserInfo().UserInfoId == 1)
                {
                    userWiseCreatedByrequisitionList = allrequisitionList.Where(x => (x.ApprovedStatus == HMConstants.ApprovalStatus.Submit.ToString())).ToList();
                    userWiseCheckedByrequisitionList = allrequisitionList.Where(x => (x.ApprovedStatus == HMConstants.ApprovalStatus.Checked.ToString())).ToList();
                    userWiseApprovedByrequisitionList = allrequisitionList.Where(x => (x.ApprovedStatus == HMConstants.ApprovalStatus.Approved.ToString())).ToList();
                }
                else if (hmUtility.GetCurrentApplicationUserInfo().IsAdminUser == true)
                {
                    userWiseCreatedByrequisitionList = allrequisitionList.Where(x => (x.ApprovedStatus == HMConstants.ApprovalStatus.Submit.ToString())).ToList();
                    userWiseCheckedByrequisitionList = allrequisitionList.Where(x => (x.ApprovedStatus == HMConstants.ApprovalStatus.Checked.ToString())).ToList();
                    userWiseApprovedByrequisitionList = allrequisitionList.Where(x => (x.ApprovedStatus == HMConstants.ApprovalStatus.Approved.ToString())).ToList();
                }
                else
                {
                    userWiseCreatedByrequisitionList = allrequisitionList.Where(x => (x.ApprovedStatus == HMConstants.ApprovalStatus.Submit.ToString()) && (x.CheckedBy == hmUtility.GetCurrentApplicationUserInfo().UserInfoId)).ToList();
                    userWiseCheckedByrequisitionList = allrequisitionList.Where(x => (x.ApprovedStatus == HMConstants.ApprovalStatus.Checked.ToString()) && (x.ApprovedBy == hmUtility.GetCurrentApplicationUserInfo().UserInfoId)).ToList();
                    userWiseApprovedByrequisitionList = allrequisitionList.Where(x => (x.ApprovedStatus == HMConstants.ApprovalStatus.Approved.ToString()) && (x.ApprovedBy == hmUtility.GetCurrentApplicationUserInfo().UserInfoId)).ToList();
                }

                List<PMRequisitionBO> userWiserequisitionList = new List<PMRequisitionBO>();
                userWiserequisitionList.AddRange(userWiseCreatedByrequisitionList);
                userWiserequisitionList.AddRange(userWiseCheckedByrequisitionList);
                userWiserequisitionList.AddRange(userWiseApprovedByrequisitionList);

                gvRequisitionInfo.DataSource = userWiserequisitionList;
                gvRequisitionInfo.DataBind();
            }
            else
            {
                gvRequisitionInfo.DataSource = allrequisitionList;
                gvRequisitionInfo.DataBind();
            }
        }
        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmPMRequisitionApproval.ToString());
            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
        }
        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static decimal SetSelected(int RequisitionId)
        {

            PMRequisitionDA requisitionDA = new PMRequisitionDA();
            PMRequisitionBO requisitionBO = requisitionDA.GetPMRequisitionInfoByID(RequisitionId);
            if (requisitionBO != null)
            {
                return 0; //requisitionBO.Quantity;
            }
            else
            {
                return 0;
            }

        }
        [WebMethod]
        public static string GetRequisitionDetails(int requisitionid)
        {
            PMRequisitionDA requisitionDA = new PMRequisitionDA();
            List<PMRequisitionDetailsBO> requisitionDetails = new List<PMRequisitionDetailsBO>();
            requisitionDetails = requisitionDA.GetPMRequisitionDetailsByID(requisitionid);

            int row = 0;
            string tr = string.Empty;

            tr = "<table id='DetailsRequisitionGrid' class='table table-bordered table-condensed table-responsive' style='width: 100%;'> " +
                 "<thead> " +
                 "    <tr style='color: White; background-color: #44545E; text-align: left; font-weight: bold;'> " +
                 "       <th style='width: 5%;'> " +
                 "       </th> " +
                 "       <th style='width: 30%;'> " +
                 "            Item " +
                 "        </th> " +
                 "        <th style='width: 15%;'> " +
                 "            Stock By " +
                 "        </th> " +
                 "        <th style='width: 15%;'> " +
                 "            Quantity " +
                 "        </th> " +
                 "        <th style='width: 20%;'> " +
                 "            Approved Quantity " +
                 "        </th> " +
                 "        <th style='width: 10%;'> " +
                 "            Action " +
                 "        </th> " +
                 "        <th style='display: none;'> " +
                 "            Details Id " +
                 "        </th> " +
                 "        <th style='display: none;'> " +
                 "            Is Edit" +
                 "        </th> " +
                 "    </tr> " +
                 "</thead> " +
                 "<tbody> ";

            //tr += "&nbsp;&nbsp;<a href='#' onclick= 'DeleteItemRequsition(this)' ><img alt='Delete' src='../Images/delete.png' /></a>";
            string requisitionRemarks = string.Empty;
            foreach (PMRequisitionDetailsBO rd in requisitionDetails)
            {
                if (row % 2 == 0)
                {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else
                {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                if (rd.ApprovedStatus == HMConstants.ApprovalStatus.Approved.ToString())
                {
                    tr += "<td style='width:5%;'> <input type='checkbox' checked= 'checked' disabled='disabled' id='chkb" + rd.RequisitionDetailsId.ToString() + "' /> </td>";
                }
                else if (rd.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString())
                {
                    tr += "<td style='width:5%;'> <input type='checkbox' checked= 'checked' id='chkb" + rd.RequisitionDetailsId.ToString() + "' /> </td>";
                }

                tr += "<td style='width:30%;'>" + rd.ItemName + "</td>";
                tr += "<td style='width:20%;'>" + rd.HeadName + "</td>";
                tr += "<td style='width:20%;'>" + rd.Quantity + "</td>";

                if (rd.ApprovedQuantity == 0)
                {
                    rd.ApprovedQuantity = null;
                }

                if (rd.ApprovedQuantity != null)
                {
                    tr += "<td style='width:20%; text-align:center;'> <input type='text' id='txt" + rd.RequisitionDetailsId.ToString() + "' value = '" + rd.ApprovedQuantity + "' disabled='disabled' onblur='CheckInputValue(this)' class='form-control' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                }
                else
                {
                    tr += "<td style='width:20%; text-align:center;'> <input type='text' id='txt" + rd.RequisitionDetailsId.ToString() + "' value = '" + rd.Quantity + "' onblur='CheckInputValue(this)' class='form-control' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                }

                if (rd.ApprovedStatus == HMConstants.ApprovalStatus.Approved.ToString())
                {
                    tr += "<td style='width:20%; text-align:center;'></td>";
                    //tr += "<td style='width:20%; text-align:center;'><a href='#' onclick= 'EditApprovedRequisition(this)' ><img alt='Delete' src='../Images/edit.png' /></a></td>";
                }
                else
                {
                    tr += "<td style='width:20%; text-align:center;'></td>";
                }

                tr += "<td style='display:none;'>" + rd.RequisitionDetailsId + "</td>";
                tr += "<td style='display:none;'>0</td>";
                tr += "</tr>";
                requisitionRemarks = rd.RequisitionRemarks;
                row++;
            }

            tr += "</tbody> " + "</table> ";

            tr += "<div class='divClear'></ div > ";
            tr += "<div>Requisition Remarks: " + requisitionRemarks + "</ div >";
            return tr;
        }
        [WebMethod]
        public static ReturnInfo ApprovedRequsition(int hfIsRequisitionCheckedByEnable, int requsitionId, List<PMRequisitionDetailsBO> approvedItem, List<PMRequisitionDetailsBO> pendingItem)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;

            string TransactionNo;
            string TransactionType;
            string ApproveStatus;

            string requisitionApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                PMRequisitionBO requsition = new PMRequisitionBO();
                PMRequisitionDA requisitionDA = new PMRequisitionDA();

                requsition.RequisitionId = requsitionId;

                if (hfIsRequisitionCheckedByEnable == 1)
                {
                    PMRequisitionBO viewBo = new PMRequisitionBO();

                    viewBo = requisitionDA.GetPMRequisitionInfoByID(requsition.RequisitionId);
                    if (viewBo != null)
                    {
                        requisitionApprovedStatus = viewBo.ApprovedStatus;

                        if (pendingItem.Count == 0)
                        {
                            if (requisitionApprovedStatus == HMConstants.ApprovalStatus.Checked.ToString())
                            {
                                requsition.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
                            }
                            else
                            {
                                requsition.ApprovedStatus = HMConstants.ApprovalStatus.Checked.ToString();
                            }
                        }
                        else if (pendingItem.Count > 0)
                        {
                            if (requisitionApprovedStatus == "Partial Checked")
                            {
                                requsition.ApprovedStatus = "Partial Approved";
                            }
                            else if (requisitionApprovedStatus == "Partial Approved")
                            {
                                requsition.ApprovedStatus = "Approved";
                            }
                            else
                            {
                                requsition.ApprovedStatus = "Partial Checked";
                            }
                        }
                    }

                }
                else
                {
                    if (pendingItem.Count == 0)
                    {
                        requsition.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
                    }
                    else if (pendingItem.Count > 0)
                    {
                        requsition.ApprovedStatus = "Partial Approved";
                    }
                }

                requsition.ApprovedBy = userInformationBO.UserInfoId;

                status = requisitionDA.ApprovePMRequisitionInfo(requsition, approvedItem, pendingItem, out TransactionNo, out TransactionType, out ApproveStatus);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ProductRequisition.ToString(), requsitionId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductRequisition));
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }

            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                
            }

            return rtninf;
        }
    }
}