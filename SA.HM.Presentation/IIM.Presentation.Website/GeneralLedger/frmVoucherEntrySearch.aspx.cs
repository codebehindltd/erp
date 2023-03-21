using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;


namespace HotelManagement.Presentation.Website.GeneralLedger
{
    public partial class frmVoucherEntrySearch : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                CompanyProjectUserControlVoucherApproval.ddlFirstValueVar = "select";
                IsAdminUser();
                LoadCommonDropDownHiddenField();
            }
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }

        private void IsAdminUser()
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            btnAdminApproval.Visible = false;
            if (userInformationBO.UserInfoId == 1)
            {
                btnAdminApproval.Visible = true;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 2).Count() > 0)
                    {
                        btnAdminApproval.Visible = true;
                    }
                }
            }
            #endregion
        }

        [WebMethod]
        public static GridViewDataNPaging<GLLedgerMasterVwBO, GridPaging> GetVoucherBySearchCriteria(int companyId, int projectId, string voucherType, string voucherStatus, string voucherNo, string fromDate, string toDate, string referenceNo, string referenceVoucherNo, string narration, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<GLLedgerMasterVwBO, GridPaging> myGridData = new GridViewDataNPaging<GLLedgerMasterVwBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            DateTime? startDate = null;
            DateTime? endDate = null;

            if (!string.IsNullOrEmpty(fromDate))
            {
                startDate = hmUtility.GetDateTimeFromString(fromDate, userInformationBO.ServerDateFormat);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                endDate = hmUtility.GetDateTimeFromString(toDate, userInformationBO.ServerDateFormat);
            }

            HMCommonDA commonDA = new HMCommonDA();
            VoucherEntryDA voucherDa = new VoucherEntryDA();
            List<GLLedgerMasterVwBO> voucher = new List<GLLedgerMasterVwBO>();
            voucher = voucherDa.GetVoucherBySearchCriteria(companyId, projectId, userInformationBO.UserInfoId, userInformationBO.UserGroupId, voucherType, voucherStatus, voucherNo, startDate, endDate, referenceNo, referenceVoucherNo, narration, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(voucher, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static GLVoucherVwBO GetVoucherDetailsForDisplay(Int64 ledgerMasterId)
        {
            GLVoucherVwBO vouchervw = new GLVoucherVwBO();
            VoucherEntryDA voucherDa = new VoucherEntryDA();

            vouchervw.LedgerMaster = voucherDa.GetVoucherById(ledgerMasterId);
            vouchervw.LedgerMasterDetails = voucherDa.GetVoucherDetailsById(ledgerMasterId);

            vouchervw.TotalCrAmount = vouchervw.LedgerMasterDetails.Sum(c => c.CRAmount);
            vouchervw.TotalDrAmount = vouchervw.LedgerMasterDetails.Sum(c => c.DRAmount);

            return vouchervw;
        }
        [WebMethod]
        public static ReturnInfo VoucherApproval(List<GLVoucherApprovalVwBO> VocuherApproval)
        {
            Boolean status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            VoucherEntryDA voucherDa = new VoucherEntryDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                foreach (GLVoucherApprovalVwBO va in VocuherApproval)
                {
                    if (va.GLStatus == HMConstants.ApprovalStatus.Pending.ToString())
                    {
                        va.GLStatus = HMConstants.ApprovalStatus.Checked.ToString();
                    }
                    else if (va.GLStatus == HMConstants.ApprovalStatus.Submit.ToString())
                    {
                        va.GLStatus = HMConstants.ApprovalStatus.Checked.ToString();
                    }
                    else if (va.GLStatus == HMConstants.ApprovalStatus.Checked.ToString())
                    {
                        va.GLStatus = HMConstants.ApprovalStatus.Approved.ToString();
                    }
                    else if (va.GLStatus == HMConstants.ApprovalStatus.Approved.ToString())
                    {
                        va.GLStatus = HMConstants.ApprovalStatus.Submit.ToString();
                    }
                    va.ApprovedRCheckedby = userInformationBO.UserInfoId;

                    status = voucherDa.UpdateVoucherApprovalStatus(VocuherApproval);
                    if (status)
                    {
                        rtninfo.IsSuccess = true;
                        if (va.GLStatus == HMConstants.ApprovalStatus.Checked.ToString())
                            rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                        else if (va.GLStatus == HMConstants.ApprovalStatus.Approved.ToString())
                            rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    }
                    else
                    {
                        rtninfo.IsSuccess = false;
                        rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }

        [WebMethod]
        public static string LoadVoucherDocumentById(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("GLVoucherDocuments", id);
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);

            string strTable = "";
            strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                if (dr.Extention == ".jpg")
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:250px; height:250px; float:left;padding:30px'>";
                    strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img style='width: 200px; height: 200px;' src='" + ImgSource + "'  alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
                else
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:100px; height:100px; float:left;padding:30px'>";
                    strTable += "<a style='color:#333333;' target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img style='width: 100px; height: 100px;' src='" + dr.IconImage + "' alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
            }
            strTable += "</div>";
            if (strTable == "")
            {
                strTable = "<tr><td align='center'>No Record Available!</td></tr>";
            }
            return strTable;

        }



        [WebMethod]
        public static ReturnInfo AdminApprovalStatus(int companyIdUC, int projectIdUC, string voucherNo, string voucherStatus)
        {
            Boolean status = false;
            ReturnInfo rtninfo = new ReturnInfo();
            VoucherEntryDA voucherDa = new VoucherEntryDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                status = voucherDa.UpdateVoucherStatusByVoucherNo(companyIdUC, projectIdUC, voucherNo, voucherStatus, userInformationBO.UserInfoId);
                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = "Voucher Unapprove Successfull.";
                }
                else
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }
    }
}