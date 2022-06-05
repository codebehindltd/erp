using HotelManagement.Data.Payroll;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class CashRequisitionOrBillVoucherInformation : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                if (!isValidEmployee())
                {
                    return;
                }
                IsAdminUser();
                LoadEmployee();
                CheckPermission();
                LoadStatusDropDown();
                LoadConfigurationAndMethoShowHide();
            }
        }
        private void LoadConfigurationAndMethoShowHide()
        {
            HMCommonSetupBO setUpBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            btnCreateNewCashRequisition.Visible = true;
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsCashRequisitionEnable", "IsCashRequisitionEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                if (setUpBO.SetupValue == "0")
                    btnCreateNewCashRequisition.Visible = false;
            }

            btnCreateNewBillVoucher.Visible = true;
            setUpBO = commonSetupDA.GetCommonConfigurationInfo("IsBillVoucherEnable", "IsBillVoucherEnable");
            if (!string.IsNullOrWhiteSpace(setUpBO.SetupValue))
            {
                if (setUpBO.SetupValue == "0")
                    btnCreateNewBillVoucher.Visible = false;
            }
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
        private void LoadStatusDropDown()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO homePageSetupBO = new HMCommonSetupBO();
            homePageSetupBO = commonSetupDA.GetCommonConfigurationInfo("ApprovalPolicyConfiguration", "ApprovalPolicyConfiguration");
            if (homePageSetupBO != null)
            {
                if (homePageSetupBO.SetupId > 0)
                {
                    if (homePageSetupBO.SetupValue == "0")
                    {
                        this.ddlSearchStatus.Items.Remove(ddlSearchStatus.Items.FindByValue("Partially Checked"));
                        this.ddlSearchStatus.Items.Remove(ddlSearchStatus.Items.FindByValue("Partially Approved"));
                    }
                }
            }
        }
        public bool isValidEmployee()
        {
            bool status = true;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO.EmpId == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "Your Employee is not mapped with software user, Please contact with admin.", AlertType.Warning);
                status = false;
            }
            return status;
        }
        private void LoadEmployee()
        {
            EmployeeDA EmpDA = new EmployeeDA();
            List<EmployeeBO> EmpBO = new List<EmployeeBO>();
            EmpBO = EmpDA.GetEmployeeInfo();

            ddlAssignEmployee.DataSource = EmpBO;
            ddlAssignEmployee.DataTextField = "DisplayName";
            ddlAssignEmployee.DataValueField = "EmpId";
            ddlAssignEmployee.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlAssignEmployee.Items.Insert(0, item);
        }
        private void CheckPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
        }

        [WebMethod]
        public static ReturnInfo ApprovalStatusUpdate(long id, string ApprovalStatusUpdate)
        {
            ReturnInfo info = new ReturnInfo();
            HMCommonDA hmCommonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            string TransactionNo;
            string TransactionTypeCash;
            string ApproveStatusCash;

            try
            {
                int isAdminUser = 0;
                //// // // ------User Admin Authorization BO Session Information --------------------------------
                //#region User Admin Authorization

                //if (userInformationBO.UserInfoId == 1)
                //{
                //    isAdminUser = 1;
                //}
                //else
                //{
                //    List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                //    adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                //    if (adminAuthorizationList != null)
                //    {
                //        if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 2).Count() > 0)
                //        {
                //            isAdminUser = 1;
                //        }
                //    }
                //}
                //#endregion

                CashRequisitionDA cashRequisitionDA = new CashRequisitionDA();
                var status = cashRequisitionDA.ApprovalStatusUpdate(isAdminUser, id, ApprovalStatusUpdate, userInformationBO.UserInfoId, out TransactionNo, out TransactionTypeCash, out ApproveStatusCash);
                if (status && ApprovalStatusUpdate == "Checked")
                {
                    info.IsSuccess = true;
                    info.PrimaryKeyValue = id.ToString();
                    info.TransactionNo = TransactionNo;
                    info.TransactionType = TransactionTypeCash;
                    info.TransactionStatus = ApproveStatusCash;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                }
                else if (status && ApprovalStatusUpdate == "Approved")
                {
                    info.IsSuccess = true;
                    info.PrimaryKeyValue = id.ToString();
                    info.TransactionNo = TransactionNo;
                    info.TransactionType = TransactionTypeCash;
                    info.TransactionStatus = ApproveStatusCash;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                }
                else
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }

        [WebMethod]
        public static ReturnInfo DeleteCashRequisition(long id)
        {
            ReturnInfo info = new ReturnInfo();
            HMCommonDA hmCommonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();

            try
            {
                CashRequisitionDA cashRequisitionDA = new CashRequisitionDA();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                var status = cashRequisitionDA.DeleteCashRequisition(id, userInformationBO.UserInfoId);
                if (status)
                {
                    info.IsSuccess = true;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Cancel, AlertType.Success);
                }
                else
                {
                    info.IsSuccess = false;
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }

        [WebMethod]
        public static GridViewDataNPaging<CashRequisitionBO, GridPaging> GetCashRequisition(int companyId, int projectId, int employeeId, string transactionType, string pFromDate, string pToDate,
                                                                                            string fromAmount, string toAmount, string transactionNo, string adjustmentNo, string status, string Remarks, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            DateTime? fromDate = null;
            DateTime? toDate = null;

            if (!string.IsNullOrWhiteSpace(pFromDate))
            {
                fromDate = hmUtility.GetDateTimeFromString(pFromDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            if (!string.IsNullOrWhiteSpace(pToDate))
            {
                toDate = hmUtility.GetDateTimeFromString(pToDate, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }

            List<CashRequisitionBO> cashRequisitionList = new List<CashRequisitionBO>();
            GridViewDataNPaging<CashRequisitionBO, GridPaging> myGridData = new GridViewDataNPaging<CashRequisitionBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            if (userInformationBO.EmpId != 0)
            {
                CashRequisitionDA cashRequisitionDA = new CashRequisitionDA();

                int empId = userInformationBO.EmpId;
                // // // ------User Admin Authorization BO Session Information --------------------------------
                #region User Admin Authorization
                if (userInformationBO.UserInfoId == 1)
                {
                    empId = 0;
                }
                else
                {
                    List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                    adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                    if (adminAuthorizationList != null)
                    {
                        if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 2).Count() > 0)
                        {
                            empId = 0;
                        }
                    }
                }
                #endregion

                cashRequisitionList = cashRequisitionDA.GetCashRequisitionForGridPaging(userInformationBO.UserInfoId, empId, companyId, projectId, employeeId, transactionType, fromDate, toDate, fromAmount, toAmount, transactionNo, adjustmentNo, status, Remarks, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);
            }

            myGridData.GridPagingProcessing(cashRequisitionList, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static string LoadDocument(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("BillVoucherDoc", (int)id);
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);

            string strTable = "";
            strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                if (dr.Extention == ".jpg" || dr.Extention == ".png" || dr.Extention == ".PNG" || dr.Extention == ".JPG")
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
            if (docList.Count == 0)
            {
                strTable = "<tr><td align='center'>No Record Available!</td></tr>";
            }
            strTable += "</div>";

            return strTable;
        }

        [WebMethod]
        public static string LoadCashRequisitionDocument(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("CashRequisitionDoc", (int)id);
            docList = new HMCommonDA().GetDocumentListWithIcon(docList);

            string strTable = "";
            strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                if (dr.Extention == ".jpg" || dr.Extention == ".png" || dr.Extention == ".PNG" || dr.Extention == ".JPG")
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
            if (docList.Count == 0)
            {
                strTable = "<tr><td align='center'>No Record Available!</td></tr>";
            }
            strTable += "</div>";

            return strTable;
        }
        [WebMethod]
        public static ReturnInfo AdminApprovalStatus(string TransactionNo, string ApprovalStatusUpdate)
        {
            ReturnInfo info = new ReturnInfo();
            HMCommonDA hmCommonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            try
            {
                CashRequisitionDA cashRequisitionDA = new CashRequisitionDA();
                CashRequisitionBO cashRequisition = new CashRequisitionBO();

                cashRequisition = cashRequisitionDA.GetCashRequisitionByTransactionNo(TransactionNo);
                if(cashRequisition.Id > 0)
                {
                    int isAdminUser = 0;
                    string TransactionTypeCash;
                    string ApproveStatusCash;

                    // // // ------User Admin Authorization BO Session Information --------------------------------
                    #region User Admin Authorization                    
                    if (userInformationBO.UserInfoId == 1)
                    {
                        isAdminUser = 1;
                    }
                    else
                    {
                        List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                        adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                        if (adminAuthorizationList != null)
                        {
                            if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 2).Count() > 0)
                            {
                                isAdminUser = 1;
                            }
                        }
                    }
                    #endregion

                    var status = cashRequisitionDA.ApprovalStatusUpdate(isAdminUser, cashRequisition.Id, ApprovalStatusUpdate, userInformationBO.UserInfoId, out TransactionNo, out TransactionTypeCash, out ApproveStatusCash);
                    if (status && ApprovalStatusUpdate == "Checked")
                    {
                        info.IsSuccess = true;
                        info.TransactionNo = TransactionNo;
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Checked, AlertType.Success);
                    }
                    else if (status && ApprovalStatusUpdate == "Approved")
                    {
                        info.IsSuccess = true;
                        info.TransactionNo = TransactionNo;
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                    }
                    else
                    {
                        info.IsSuccess = false;
                        info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                    }
                }                
            }
            catch (Exception ex)
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }
    }
}