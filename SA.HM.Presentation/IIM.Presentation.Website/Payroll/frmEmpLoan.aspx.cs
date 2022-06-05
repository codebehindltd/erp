using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Newtonsoft.Json;

using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpLoan : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;

        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                Random rd = new Random();
                int seatingId = rd.Next(100000, 999999);
                RandomDocId.Value = seatingId.ToString();
                tempDocId.Value = seatingId.ToString();
                //hfId.Value = "0";
                hfParentDoc.Value = "0";

                CheckObjectPermission();
                LoadUserInformation();
                GetEmpLoanSetupInformation();

                if (Session["EditedLoanId"] != null)
                {
                    hfLoanId.Value = Session["EditedLoanId"].ToString();
                    hfIsEditedFromApprovedForm.Value = "1";
                    Session.Remove("EditedLoanId");
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidityCheck())
                    return;

                int OwnerIdForDocuments = 0;
                HMCommonDA hmCommonDA = new HMCommonDA();

                ContentPlaceHolder mpContentPlaceHolder;
                UserControl loanAllocation;
                HiddenField loanEmployeeId;

                mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
                loanAllocation = (UserControl)mpContentPlaceHolder.FindControl("employeeSearch");
                loanEmployeeId = (HiddenField)loanAllocation.FindControl("hfEmployeeId");

                //(HiddenField)((UserControl)mpContentPlaceHolder.FindControl("employeeSearch")).FindControl("hfEmployeeId")

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                EmpLoanDA loanDa = new EmpLoanDA();
                EmpLoanBO loan = new EmpLoanBO();

                bool status = false;
                int loanId = 0;

                loan.EmpId = Convert.ToInt32(loanEmployeeId.Value);
                loan.LoanNumber = "";
                loan.LoanType = ddlLoanType.SelectedValue;
                loan.LoanAmount = Convert.ToDecimal(txtLoanAmount.Text);
                loan.InterestRate = Convert.ToDecimal(lblInterestRate.InnerText.Substring(2, 1));
                loan.InterestAmount = Convert.ToDecimal(hfInterestAmount.Value);
                loan.DueAmount = loan.LoanAmount;
                loan.DueInterestAmount = loan.InterestAmount;
                //loan.LoanTakenForPeriod = Convert.ToInt32(ddlLoanTakenForPeriod.SelectedValue);
                loan.LoanTakenForPeriod = Convert.ToInt32(txtLoanTakenForPeriod.Text);
                loan.LoanTakenForMonthOrYear = ddlLoanTakenForMonthOrYear.SelectedValue;

                loan.PerInstallLoanAmount = Convert.ToDecimal(hfPerInstallLoanAmount.Value);
                loan.PerInstallInterestAmount = Convert.ToDecimal(hfPerInstallInterestAmount.Value);

                if (ddlCheckedBy.SelectedIndex != 0)
                {
                    loan.CheckedBy = Convert.ToInt32(ddlCheckedBy.SelectedValue);
                }

                if (ddlApprovedBy.SelectedIndex != 0)
                {
                    loan.ApprovedBy = Convert.ToInt32(ddlApprovedBy.SelectedValue);
                }

                loan.LoanStatus = HMConstants.ApprovalStatus.Pending.ToString();
                loan.ApprovedStatus = HMConstants.ApprovalStatus.Pending.ToString();

                // CheckedBy and ApprovedBy Information --------------------
                List<PayrollApprovedInfo> approvedBOList = new List<PayrollApprovedInfo>();

                // CheckedBy -----------------
                PayrollApprovedInfo approvedBOCheckedBy = new PayrollApprovedInfo();
                if (this.ddlCheckedBy.SelectedIndex != 0)
                {
                    approvedBOCheckedBy.ApprovedType = "CheckedBy";
                    approvedBOCheckedBy.UserInfoId = Convert.ToInt32(this.ddlCheckedBy.SelectedValue);
                    approvedBOList.Add(approvedBOCheckedBy);
                    loan.LoanStatus = "Checked";
                }
                //else
                //{
                //    approvedBOCheckedBy.ApprovedType = "CheckedBy";
                //    approvedBOCheckedBy.UserInfoId = Convert.ToInt32(this.ddlCheckedBy.SelectedValue);
                //    approvedBOList.Add(approvedBOCheckedBy);
                //    loan.LoanStatus = "Checked";
                //}

                // ApprovedBy -----------------
                if (this.ddlApprovedBy.SelectedIndex != 0)
                {
                    PayrollApprovedInfo approvedBOApprovedBy = new PayrollApprovedInfo();
                    approvedBOApprovedBy.ApprovedType = "ApprovedBy";
                    approvedBOApprovedBy.UserInfoId = Convert.ToInt32(this.ddlApprovedBy.SelectedValue);
                    approvedBOList.Add(approvedBOApprovedBy);
                    loan.LoanStatus = "Approved";
                }

                //if (ddlLoanTakenForMonthOrYear.SelectedValue == "month")
                //    loan.PerInstallAmount = (loan.LoanAmount / loan.LoanTakenForPeriod) + (loan.InterestAmount / loan.LoanTakenForPeriod);
                //else if (ddlLoanTakenForMonthOrYear.SelectedValue == "year")
                //    loan.PerInstallAmount = (loan.LoanAmount / (loan.LoanTakenForPeriod * 12)) + (loan.InterestAmount / (loan.LoanTakenForPeriod * 12));

                loan.LoanDate = hmUtility.GetDateTimeFromString(txtLoanDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);

                if (string.IsNullOrEmpty(hfLoanId.Value))
                {
                    loan.CreatedBy = userInformationBO.UserInfoId;
                    status = loanDa.SaveEmployeeLoan(loan, approvedBOList, out loanId);

                    if (status)
                        OwnerIdForDocuments = Convert.ToInt32(loanId);

                    DocumentsDA documentsDA = new DocumentsDA();
                    string s = hfDeletedDoc.Value;
                    string[] DeletedDocList = s.Split(',');
                    for (int i = 0; i < DeletedDocList.Length; i++)
                    {
                        DeletedDocList[i] = DeletedDocList[i].Trim();
                        Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                    }
                    Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(RandomDocId.Value));
                    Random rd = new Random();
                    int randomId = rd.Next(100000, 999999);
                    RandomDocId.Value = Convert.ToString(randomId);
                    if (status == true)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpLoan.ToString(), loanId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpLoan));
                        ClearForm();
                    }
                }
                //else
                //{
                //    loan.LoanId = Convert.ToInt32(hfLoanId.Value);
                //    loan.LastModifiedBy = userInformationBO.UserInfoId;
                //    status = loanDa.UpdateEmployeeLoan(loan, approvedBOList);

                //    if (status == true)
                //    {
                //        lblMessage.Text = "Update Operation Successfull.";
                //        isMessageBoxEnable = 2;
                //    }
                //}

                //currentOrPreviousPage = Convert.ToInt32(hfIsCurrentOrPreviousPage.Value);

                SetTab("loanEntry");
            }
            catch (Exception ex)
            {
                innboardMessage.Value = JsonConvert.SerializeObject(CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error));
            }
        }

        //************************ User Defined Function ********************//

        private void GetEmpLoanSetupInformation()
        {
            EmpLoanDA loan = new EmpLoanDA();
            LoanSettingBO loanInfo = new LoanSettingBO();

            loanInfo = loan.GetEmpLoanInformation();

            lblInterestRate.InnerText = " (" + loanInfo.CompanyLoanInterestRate.ToString() + "%)";
        }

        public bool ValidityCheck()
        {
            ContentPlaceHolder mpContentPlaceHolder;
            UserControl loanAllocation;
            HiddenField loanEmployeeId;

            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            loanAllocation = (UserControl)mpContentPlaceHolder.FindControl("employeeSearch");

            loanEmployeeId = (HiddenField)loanAllocation.FindControl("hfEmployeeId");

            if (string.IsNullOrEmpty(loanEmployeeId.Value))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Employee Id.", AlertType.Warning);
                return false;
            }
            else if (string.IsNullOrEmpty(txtLoanTakenForPeriod.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Loan Period.", AlertType.Warning);
                txtLoanTakenForPeriod.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtLoanAmount.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Loan Amount.", AlertType.Warning);
                txtLoanAmount.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(txtLoanDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Loan Date.", AlertType.Warning);
                txtLoanDate.Focus();
                return false;
            }
            else if (this.ddlCheckedBy.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "ApprovedBy.", AlertType.Warning);
                ddlApprovedBy.Focus();
                return false;
            }
            else if (this.ddlApprovedBy.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "ApprovedBy.", AlertType.Warning);
                ddlApprovedBy.Focus();
                return false;
            }

            return true;
        }

        private void LoadUserInformation()
        {
            UserInformationDA entityDA = new UserInformationDA();

            this.ddlCheckedBy.DataSource = entityDA.GetUserInformation();
            this.ddlCheckedBy.DataTextField = "UserName";
            this.ddlCheckedBy.DataValueField = "UserInfoId";
            this.ddlCheckedBy.DataBind();

            ListItem itemEmployee = new ListItem();
            itemEmployee.Value = string.Empty;
            itemEmployee.Text = hmUtility.GetDropDownFirstValue();
            this.ddlCheckedBy.Items.Insert(0, itemEmployee);


            this.ddlApprovedBy.DataSource = entityDA.GetUserInformation();
            this.ddlApprovedBy.DataTextField = "UserName";
            this.ddlApprovedBy.DataValueField = "UserInfoId";
            this.ddlApprovedBy.DataBind();

            this.ddlApprovedBy.Items.Insert(0, itemEmployee);
        }

        private void CheckObjectPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }

        private void SetTab(string TabName)
        {
            if (TabName == "loanEntry")
            {
                loanEntry.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                loanSearch.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "loanSearch")
            {
                loanSearch.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                loanEntry.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }

        private void ClearForm()
        {
            ddlLoanType.SelectedIndex = 0;
            txtLoanAmount.Text = string.Empty;
            //lblInterestRate.InnerText;
            hfInterestAmount.Value = string.Empty;
            //ddlLoanTakenForPeriod.SelectedIndex = 0;
            txtLoanTakenForPeriod.Text = string.Empty;
            ddlLoanTakenForMonthOrYear.SelectedIndex = 0;
            this.btnSave.Text = "Save";

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl loanAllocation, employeeForLoanSearch;

            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            loanAllocation = (UserControl)mpContentPlaceHolder.FindControl("employeeSearch");
            employeeForLoanSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeForLoanSearch");

            ((TextBox)loanAllocation.FindControl("txtSearchEmployee")).Text = string.Empty;
            ((TextBox)loanAllocation.FindControl("txtEmployeeName")).Text = string.Empty;


            ((TextBox)employeeForLoanSearch.FindControl("txtEmployeeName")).Text = string.Empty;
            ((TextBox)employeeForLoanSearch.FindControl("txtSearchEmployee")).Text = string.Empty;
            ((DropDownList)employeeForLoanSearch.FindControl("ddlEmployee")).SelectedValue = "0";

            ((HiddenField)loanAllocation.FindControl("hfEmployeeId")).Value = "0";
            ((HiddenField)employeeForLoanSearch.FindControl("hfEmployeeId")).Value = "0";

            txtLoanDate.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            ddlCheckedBy.SelectedValue = string.Empty;
            ddlApprovedBy.SelectedValue = string.Empty;
        }

        //************************ User Defined WebMethod ********************//

        [WebMethod]
        public static ReturnInfo UpdateEmpLoan(EmpLoanBO loan, string checkedBy, string approvedby, int randomDocId, string deletedDoc)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            try
            {
                frmEmpLoan frm = new frmEmpLoan();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();
                HMCommonDA hmCommonDA = new HMCommonDA();
                EmpLoanDA loanDa = new EmpLoanDA();

                bool status = false;
                string message = string.Empty;
                long OwnerIdForDocuments = 0;
                //loan.LoanStatus = "Regular";

                // CheckedBy and ApprovedBy Information --------------------
                List<PayrollApprovedInfo> approvedBOList = new List<PayrollApprovedInfo>();

                // CheckedBy -----------------
                PayrollApprovedInfo approvedBOCheckedBy = new PayrollApprovedInfo();
                if (!string.IsNullOrEmpty(checkedBy))
                {
                    approvedBOCheckedBy.ApprovedType = "CheckedBy";
                    approvedBOCheckedBy.UserInfoId = Convert.ToInt32(checkedBy);
                    approvedBOList.Add(approvedBOCheckedBy);
                }
                //else
                //{
                //    approvedBOCheckedBy.ApprovedType = "CheckedBy";
                //    approvedBOCheckedBy.UserInfoId = Convert.ToInt32(checkedBy);
                //    approvedBOList.Add(approvedBOCheckedBy);
                //    loan.LoanStatus = "Checked";
                //}

                // ApprovedBy -----------------
                if (!string.IsNullOrEmpty(approvedby))
                {
                    PayrollApprovedInfo approvedBOApprovedBy = new PayrollApprovedInfo();
                    approvedBOApprovedBy.ApprovedType = "ApprovedBy";
                    approvedBOApprovedBy.UserInfoId = Convert.ToInt32(approvedby);
                    approvedBOList.Add(approvedBOApprovedBy);
                }

                loan.LastModifiedBy = userInformationBO.UserInfoId;
                status = loanDa.UpdateEmployeeLoan(loan, approvedBOList);
                if (status)
                    OwnerIdForDocuments = Convert.ToInt32(loan.LoanId);

                DocumentsDA documentsDA = new DocumentsDA();
                string s = deletedDoc;
                string[] DeletedDocList = s.Split(',');
                for (int i = 0; i < DeletedDocList.Length; i++)
                {
                    DeletedDocList[i] = DeletedDocList[i].Trim();
                    Boolean DeleteStatus = documentsDA.DeleteDocumentsByDocumentId(Convert.ToInt32(DeletedDocList[i]));
                }
                Boolean updateStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(OwnerIdForDocuments, Convert.ToInt32(randomDocId));
                if (status == true)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.EmpLoan.ToString(), loan.LoanId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpLoan));
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                Random rd = new Random();
                int randomId = rd.Next(100000, 999999);
                rtninf.Data = randomId;
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }

        [WebMethod]
        public static EmpLoanBO FillForm(int loanId)
        {
            EmpLoanDA loanDa = new EmpLoanDA();
            EmpLoanBO loan = new EmpLoanBO();
            loan = loanDa.GetLoanByLoanId(loanId);

            return loan;
        }

        [WebMethod]
        public static GridViewDataNPaging<EmpLoanBO, GridPaging> LoadEmployeeLoanInfo(string empId, string loanType, string loanStatus, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            int totalRecords = 0;

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            GridViewDataNPaging<EmpLoanBO, GridPaging> myGridData = new GridViewDataNPaging<EmpLoanBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            EmpLoanDA da = new EmpLoanDA();
            List<EmpLoanBO> loan = da.SearchLoan(empId, loanType, loanStatus, userInformationBO.UserInfoId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(loan, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static string GetEmpLoanInformation(string loanType)
        {
            EmpLoanDA loanDa = new EmpLoanDA();

            LoanSettingBO loanSetting = new LoanSettingBO();
            loanSetting = loanDa.GetEmpLoanInformation();

            string interestRate = string.Empty;

            if (loanType == "CompanyLoan")
            { interestRate = loanSetting.CompanyLoanInterestRate.ToString(); }
            else if (loanType == "PFLoan")
            { interestRate = loanSetting.PFLoanInterestRate.ToString(); }

            return interestRate;
        }

        [WebMethod]
        public static ReturnInfo ChangeLoanStatus(int loanId, int empId, string updateType)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            Boolean status = false;

            try
            {
                EmpLoanDA loanDa = new EmpLoanDA();
                frmEmpLoan frm = new frmEmpLoan();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = frm.hmUtility.GetCurrentApplicationUserInfo();

                if (updateType == "Cancel")
                {
                    status = loanDa.UpdateLoanNApprovedStatus(loanId, empId, string.Empty, HMConstants.ApprovalStatus.Cancel.ToString(), userInformationBO.UserInfoId);
                }
                else if (updateType == "Pending")
                {
                    status = loanDa.UpdateLoanNApprovedStatus(loanId, empId, string.Empty, HMConstants.ApprovalStatus.Pending.ToString(), userInformationBO.UserInfoId);
                }

                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Success, AlertType.Success);
                }
            }
            catch
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            if (!status)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }

        [WebMethod]
        public static ReturnInfo LoanApprova(int loanId, string approvedStatus)
        {
            ReturnInfo rtninf = new ReturnInfo();
            EmpLoanDA DA = new EmpLoanDA();

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                rtninf.IsSuccess = DA.LoanApproval(loanId, approvedStatus, userInformationBO.UserInfoId);

                HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
                HMCommonSetupBO isTransferProductReceiveDisable = new HMCommonSetupBO();
                
                if (!rtninf.IsSuccess)
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
                else
                {
                    rtninf.IsSuccess = true;

                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(), EntityTypeEnum.EntityType.PMProductOut.ToString(), loanId,
                               ProjectModuleEnum.ProjectModule.PurchaseManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.PMProductOut));

                }
            }
            catch (Exception ex)
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }

        [WebMethod]
        public static List<DocumentsBO> GetUploadedDocByWebMethod(int randomId, int id, string deletedDoc)
        {
            List<int> delete = new List<int>();
            if (!(String.IsNullOrEmpty(deletedDoc)))
            {
                delete = deletedDoc.Split(',').Select(t => int.Parse(t)).ToList();
            }
            List<DocumentsBO> docList = new List<DocumentsBO>();
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("LoanDocuments", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("LoanDocuments", (int)id));

            docList.RemoveAll(x => delete.Contains(Convert.ToInt32(x.DocumentId)));
            foreach (DocumentsBO dc in docList)
            {

                if (dc.DocumentType == "Image")
                    dc.Path = (dc.Path + dc.Name);

                dc.Name = dc.Name.Remove(dc.Name.LastIndexOf('.'));
            }
            docList = new HMCommonDA().GetDocumentListWithIcon(docList).ToList();
            return docList;
        }

        [WebMethod]
        public static string LoadLoanDocument(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("LoanDocuments", (int)id);
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
    }
}