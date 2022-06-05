using HotelManagement.Data.Maintenance;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Maintenance;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Maintenance
{
    public partial class GatePassInformation : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadCurrentDate();
            }
        }
        protected void btnNewGatePass_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Maintenance/frmGatePass.aspx");
        }

        private void LoadCurrentDate()
        {
            DateTime dateTime = DateTime.Now;
            this.txtFromDate.Text = hmUtility.GetStringFromDateTime(dateTime);
            this.txtToDate.Text = hmUtility.GetStringFromDateTime(dateTime);
        }
        
        [WebMethod]
        public static GridViewDataNPaging<GatePassViewForCheckOrApproveBO, GridPaging> SearchGatePassList(string fromDate, string toDate, string status, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<GatePassViewForCheckOrApproveBO, GridPaging> myGridData = new GridViewDataNPaging<GatePassViewForCheckOrApproveBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            DateTime FromDate = hmUtility.GetDateTimeFromString(fromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            DateTime ToDate = hmUtility.GetDateTimeFromString(toDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            
            List<GatePassViewForCheckOrApproveBO> gatePassList = new List<GatePassViewForCheckOrApproveBO>();
            GatePassDA gatePassDA = new GatePassDA();

            gatePassList = gatePassDA.GetGatePassBySearchCriteriaForPaging(FromDate, ToDate, status, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(gatePassList, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static string GetGatePassDetailsForApproval(int gatePassId)
        {
            GatePassDA gatePassDA = new GatePassDA();
            List<GatePassItemBO> gatePassItems = new List<GatePassItemBO>();
            gatePassItems = gatePassDA.GetGatePassDetailsByID(gatePassId);

            int row = 0;
            string tr = string.Empty;

            tr = "<table id='tbGatePassDetailsGrid' class='table table-bordered table-condensed table-responsive' style='width: 100%;'> " +
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
            foreach (GatePassItemBO gp in gatePassItems)
            {
                if (row % 2 == 0)
                {
                    tr += "<tr style='background-color:#FFFFFF;'>";
                }
                else
                {
                    tr += "<tr style='background-color:#E3EAEB;'>";
                }

                if (gp.Status == HMConstants.ApprovalStatus.Approved.ToString() || gp.Status == HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    tr += "<td style='width:5%;'> <input type='checkbox' checked= 'checked' disabled='disabled' id='chkb" + gp.GatePassId.ToString() + "' /> </td>";
                }
                else if (gp.Status != HMConstants.ApprovalStatus.Approved.ToString())
                {
                    tr += "<td style='width:5%;'> <input type='checkbox' checked= 'checked' id='chkb" + gp.GatePassId.ToString() + "' /> </td>";
                }

                tr += "<td style='width:30%;'>" + gp.ItemName + "</td>";
                tr += "<td style='width:20%;'>" + gp.StockBy + "</td>";
                tr += "<td style='width:20%;'>" + gp.Quantity + "</td>";

                if (gp.Status == HMConstants.ApprovalStatus.Approved.ToString() || gp.Status == HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    tr += "<td style='width:20%; text-align:center;'> <input type='text' id='txt" + gp.GatePassId.ToString() + "' value = '" + gp.ApprovedQuantity + "' disabled='disabled' onblur='CheckInputValue(this)' class='form-control' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                }
                else if (gp.Status == HMConstants.ApprovalStatus.Checked.ToString())
                {
                    tr += "<td style='width:20%; text-align:center;'> <input type='text' id='txt" + gp.GatePassId.ToString() + "' value = '" + gp.ApprovedQuantity + "' onblur='CheckInputValue(this)' class='form-control' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                }
                else
                {
                    tr += "<td style='width:20%; text-align:center;'> <input type='text' id='txt" + gp.GatePassId.ToString() + "' value = '" + gp.Quantity + "' onblur='CheckInputValue(this)' class='form-control' style='width:65px; padding:0; padding-left:2px; padding-right:2px; margin:0;' /> </td>";
                }
                
                tr += "<td style='display:none;'>" + gp.GatePassItemId + "</td>";
                tr += "<td style='display:none;'>0</td>";
                tr += "</tr>";
                row++;
            }

            tr += "</tbody> " + "</table> ";

            tr += "<div class='divClear'></ div > ";
            return tr;
        }
        [WebMethod]
        public static ReturnInfo CheckOrApprovedGatePass(long gatePassId, List<GatePassItemBO> approvedItem, List<GatePassItemBO> cancelItem)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;
            string requisitionApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();
            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                GatePassBO gatepass = new GatePassBO();
                GatePassDA gatePassDA = new GatePassDA();
                
                if (approvedItem.Count == 0)
                {
                    gatepass.Status = HMConstants.ApprovalStatus.Cancel.ToString();
                    gatepass.LastModifiedBy = userInformationBO.UserInfoId;
                }
                
                else
                {
                    gatepass = gatePassDA.GetGatePassInfoByID(gatePassId);

                    if (gatepass != null)
                    {
                        requisitionApprovedStatus = gatepass.Status;
                        if ((requisitionApprovedStatus == HMConstants.ApprovalStatus.Checked.ToString())|| requisitionApprovedStatus== "Partially Approved")
                        {
                            gatepass.Status = HMConstants.ApprovalStatus.Approved.ToString();
                            gatepass.ApprovedBy = userInformationBO.UserInfoId;
                        }
                        else
                        {
                            gatepass.Status = HMConstants.ApprovalStatus.Checked.ToString();
                            gatepass.CheckedBy = userInformationBO.UserInfoId;
                        }
                    }

                }
                if (approvedItem.Count == 0)
                    status = gatePassDA.CancelGatePass(gatepass.GatePassId, userInformationBO.UserInfoId);
                else
                    status = gatePassDA.CheckOrApproveGatePassInfo(gatepass, approvedItem, cancelItem);

                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.ProductRequisition.ToString(), gatePassId,
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
        [WebMethod]
        public static ReturnInfo CancelGatePass(int gatePassId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            bool status = false;
            GatePassBO gatepass = new GatePassBO();
            GatePassDA gatePassDA = new GatePassDA();

            UserInformationBO userInformationBO = new UserInformationBO();

            string statusType = string.Empty;
            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                gatepass = gatePassDA.GetGatePassInfoByID(gatePassId);

                if (gatepass.CreatedBy == userInformationBO.UserInfoId)
                {
                    status = gatePassDA.DeleteGatePass(gatepass.GatePassId);
                    statusType = "Hard Deleted";
                }

                else
                    status = gatePassDA.CancelGatePass(gatepass.GatePassId, userInformationBO.UserInfoId);
                if (status)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.ProductRequisition.ToString(), gatepass.GatePassId,
                                ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ProductRequisition) + statusType);
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
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
        }
    }
}