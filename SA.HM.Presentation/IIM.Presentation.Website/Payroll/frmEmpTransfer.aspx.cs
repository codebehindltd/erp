using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Data.UserInformation;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpTransfer : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected int isNewAddButtonEnable = 1;

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
                FileUpload();
                this.LoadEmployee();
                LoadDepartment();
                LoadDesignation();
                CheckObjectPermission();
                //LoadGLCompany();
            }
        }
        private void FileUpload()
        {
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            //flashUpload.QueryParameters = "TaskAssignDocId=" + Server.UrlEncode(RandomDocId.Value);
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            //this.ddlDepartmentFrom.DataSource = entityDA.GetDepartmentInfo();
            //this.ddlDepartmentFrom.DataTextField = "Name";
            //this.ddlDepartmentFrom.DataValueField = "DepartmentId";
            //this.ddlDepartmentFrom.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            //this.ddlDepartmentFrom.Items.Insert(0, item);

            this.ddlDepartmentTo.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartmentTo.DataTextField = "Name";
            this.ddlDepartmentTo.DataValueField = "DepartmentId";
            this.ddlDepartmentTo.DataBind();

            this.ddlDepartmentTo.Items.Insert(0, item);
        }
        private void LoadDesignation()
        {
            DesignationDA entityDA = new DesignationDA();
            this.ddlDesignationTo.DataSource = entityDA.GetDesignationInfo().Where(x => x.ActiveStat == true).ToList();
            this.ddlDesignationTo.DataTextField = "Name";
            this.ddlDesignationTo.DataValueField = "DesignationId";
            this.ddlDesignationTo.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlDesignationTo.Items.Insert(0, item);
        }
        private void LoadEmployee()
        {
            EmployeeDA entityDA = new EmployeeDA();
            List<EmployeeBO> boList = new List<EmployeeBO>();
            boList = entityDA.GetEmployeeInfo();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();

            ddlReportingTo.DataSource = boList;
            ddlReportingTo.DataTextField = "EmployeeName";
            ddlReportingTo.DataValueField = "EmpId";
            ddlReportingTo.DataBind();
            ddlReportingTo.Items.Insert(0, item);

            ddlReportingTo2.DataSource = boList;
            ddlReportingTo2.DataTextField = "EmployeeName";
            ddlReportingTo2.DataValueField = "EmpId";
            ddlReportingTo2.DataBind();
            ddlReportingTo2.Items.Insert(0, item);
        }
        private void CheckObjectPermission()
        {

            btnSave.Visible = isSavePermission;
            if (!isSavePermission)
            {
                isNewAddButtonEnable = -1;
            }
        }
        //private void LoadGLCompany()
        //{
        //    GLCompanyDA entityDA = new GLCompanyDA();
        //    var List = entityDA.GetAllGLCompanyInfo();
        //    //hfCompanyAll.Value = JsonConvert.SerializeObject(List);

        //    List<GLCompanyBO> companyList = new List<GLCompanyBO>();
        //    //companyList.Add(List[0]);

        //    ddlCompanyTo.DataSource = List;
        //    ddlCompanyTo.DataTextField = "Name";
        //    ddlCompanyTo.DataValueField = "CompanyId";
        //    ddlCompanyTo.DataBind();

        //    ListItem itemCompany = new ListItem();
        //    itemCompany.Value = "0";
        //    itemCompany.Text = hmUtility.GetDropDownFirstValue();
        //    ddlCompanyTo.Items.Insert(0, itemCompany);

        //}
        private void SetTab(string TabName)
        {
            if (TabName == "entry")
            {
                entry.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                search.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "search")
            {
                search.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                entry.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }

        private void Clear()
        {
            txtTransferDate.Text = string.Empty;
            //ddlDepartmentFrom.SelectedValue = "0";
            ddlDepartmentTo.SelectedValue = "0";
            ddlReportingTo.SelectedValue = "0";
            ddlDesignationTo.SelectedValue = "0";

            txtReportingDate.Text = string.Empty;
            //txtJoinedDate.Text = string.Empty;
            ddlReportingTo.SelectedValue = "0";
            ddlReportingTo2.SelectedValue = "0";
            //ddlCompanyTo.SelectedValue = "0";
            hfTransferId.Value = "";
            txtDescription.Text = "";

            btnSave.Text = "Save";
            SetTab("entry");

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl empSearch;

            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeeSearch");
            ((HiddenField)empSearch.FindControl("hfEmployeeId")).Value = string.Empty;
            ((HiddenField)empSearch.FindControl("hfEmployeeName")).Value = string.Empty;
            ((TextBox)empSearch.FindControl("txtSearchEmployee")).Text = string.Empty;
            ((TextBox)empSearch.FindControl("txtEmployeeName")).Text = string.Empty;

            ((HiddenField)empSearch.FindControl("hfEmpDepartmentId")).Value = string.Empty;
            ((HiddenField)empSearch.FindControl("hfEmpDepartment")).Value = string.Empty;
            ((TextBox)empSearch.FindControl("txtEmpDepart")).Text = string.Empty;

            ((HiddenField)empSearch.FindControl("hfEmpDesignationId")).Value = string.Empty;
            ((HiddenField)empSearch.FindControl("hfEmpDesignation")).Value = string.Empty;
            ((TextBox)empSearch.FindControl("txtEmpDesig")).Text = string.Empty;
            btnSave.Visible = isSavePermission;
        }

        protected string GetStringFromDateTime(DateTime dateTime)
        {
            return dateTime.ToString(hmUtility.GetFormat(true));
        }

        private void FillForm(Int64 transferId)
        {
            EmployeeDA empDa = new EmployeeDA();
            EmpTransferBO transfer = new EmpTransferBO();

            transfer = empDa.GetEmpTransferById(transferId);

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl empSearch;
            HiddenField employeeId;
            HiddenField departmentId;
            HiddenField designationId;

            UserControl companyProjectInfo;
            HiddenField hfCompanyId;
            HiddenField hfProjectId;

            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeeSearch");
            employeeId = (HiddenField)empSearch.FindControl("hfEmployeeId");
            departmentId = (HiddenField)empSearch.FindControl("hfEmpDepartmentId");
            designationId = (HiddenField)empSearch.FindControl("hfEmpDesignationId");

            companyProjectInfo = (UserControl)mpContentPlaceHolder.FindControl("companyProjectUserControl");
            hfCompanyId = (HiddenField)companyProjectInfo.FindControl("hfGLCompanyId");
            hfProjectId = (HiddenField)companyProjectInfo.FindControl("hfGLProjectId");
                
            hfTransferId.Value = transferId.ToString();

            txtTransferDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(transfer.TransferDate);
            employeeId.Value = Convert.ToString(transfer.EmpId);
            departmentId.Value = Convert.ToString(transfer.PreviousDepartmentId);
            designationId.Value = Convert.ToString(transfer.PreviousDesignationId);

            hfCompanyId.Value = Convert.ToString(transfer.CurrentCompanyId);
            hfProjectId.Value = Convert.ToString(transfer.CurrentProjectId);

            ((TextBox)empSearch.FindControl("txtSearchEmployee")).Text = transfer.EmpCode;
            ((TextBox)empSearch.FindControl("txtEmployeeName")).Text = transfer.EmployeeName;
            ((TextBox)empSearch.FindControl("txtEmpDepart")).Text = transfer.PreviousDepartmentName;
            ((TextBox)empSearch.FindControl("txtEmpDesig")).Text = transfer.PreviousDepartmentName;
            ((TextBox)empSearch.FindControl("txtCompany")).Text = transfer.PreviousCompanyName;
            ((TextBox)empSearch.FindControl("txtReportingTo1")).Text = transfer.PreviousReportingToName;
            ((TextBox)empSearch.FindControl("txtReportingTo2")).Text = transfer.PreviousReportingTo2Name;

            //ddlDepartmentFrom.SelectedValue = transfer.PreviousDepartmentId.ToString();
            ddlDepartmentTo.SelectedValue = transfer.CurrentDepartmentId.ToString();
            ddlDesignationTo.SelectedValue = transfer.CurrentDesignationId.ToString();
            txtReportingDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(transfer.ReportingDate);
            //txtJoinedDate.Text = transfer.JoinedDate.ToString();
            ddlReportingTo.SelectedValue = transfer.ReportingToId.ToString();
            ddlReportingTo2.SelectedValue = transfer.ReportingTo2Id.ToString();
            ((DropDownList)companyProjectInfo.FindControl("ddlGLCompany")).SelectedValue = transfer.CurrentCompanyId.ToString();
            ((DropDownList)companyProjectInfo.FindControl("ddlGLProject")).SelectedValue = transfer.CurrentProjectId.ToString();

            txtDescription.Text = transfer.Description.ToString();

            btnSave.Text = "Update";

            SetTab("entry");
        }

        private void LoadGrid()
        {
            DateTime dateFrom = DateTime.Now, dateTo = DateTime.Now;

            if (!string.IsNullOrEmpty(txtDateFrom.Text))
            {
                //dateFrom = Convert.ToDateTime(txtDateFrom.Text);
                dateFrom = CommonHelper.DateTimeToMMDDYYYY(txtDateFrom.Text);
            }

            if (!string.IsNullOrEmpty(txtDateTo.Text))
            {
                //dateTo = Convert.ToDateTime(txtDateTo.Text);
                dateTo = CommonHelper.DateTimeToMMDDYYYY(txtDateTo.Text);
            }
            ContentPlaceHolder mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            UserControl empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeForLeaveSearch"); ;
            DropDownList srcType = (DropDownList)empSearch.FindControl("ddlEmployee");
            var type = srcType.SelectedValue;
            List<EmpTransferBO> employeeList = new List<EmpTransferBO>();
            EmployeeDA empDa = new EmployeeDA();
            HiddenField employeeId = (HiddenField)empSearch.FindControl("hfEmployeeId");
            var empId = employeeId.Value;
            employeeList = empDa.GetEmpTransfer(dateFrom, dateTo, Convert.ToInt32(type), Convert.ToInt32(empId));

            gvEmployeeTransfer.DataSource = employeeList;
            gvEmployeeTransfer.DataBind();

            SetTab("search");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool status = false;
            int OwnerIdForDocuments = 0;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();

            try
            {
                if (!IsFormValid())
                {
                    return;
                }
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                EmpTransferBO transfer = new EmpTransferBO();
                EmployeeDA empDa = new EmployeeDA();

                ContentPlaceHolder mpContentPlaceHolder;
                UserControl empSearch;
                HiddenField employeeId;
                HiddenField departmentId;
                HiddenField designationId;
                HiddenField PreviousReportingToId;
                HiddenField PreviousReportingTo2Id;
                HiddenField PreviousCompanyId;
                HiddenField PreviousProjectId;

                mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
                empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeeSearch");
                employeeId = (HiddenField)empSearch.FindControl("hfEmployeeId");
                departmentId = (HiddenField)empSearch.FindControl("hfEmpDepartmentId");
                designationId = (HiddenField)empSearch.FindControl("hfEmpDesignationId");
                PreviousReportingToId = (HiddenField)empSearch.FindControl("hfReportingTo1Id");
                PreviousReportingTo2Id = (HiddenField)empSearch.FindControl("hfReportingTo2Id");
                PreviousCompanyId = (HiddenField)empSearch.FindControl("hfCompanyId");
                PreviousProjectId = (HiddenField)empSearch.FindControl("hfProjectId");

                //transfer.TransferDate = Convert.ToDateTime(txtTransferDate.Text);
                transfer.TransferDate = CommonHelper.DateTimeToMMDDYYYY(txtTransferDate.Text);
                transfer.EmpId = Convert.ToInt32(employeeId.Value);
                //transfer.PreviousDepartmentId = Convert.ToInt32(ddlDepartmentFrom.SelectedValue);
                transfer.PreviousDepartmentId = Convert.ToInt32(departmentId.Value);
                transfer.PreviousCompanyId = Convert.ToInt32(PreviousCompanyId.Value);
                transfer.PreviousProjectId = Convert.ToInt32(PreviousProjectId.Value);
                transfer.PreviousReportingToId = Convert.ToInt32(PreviousReportingToId.Value);
                transfer.PreviousReportingTo2Id = Convert.ToInt32(PreviousReportingTo2Id.Value);
                transfer.CurrentDepartmentId = Convert.ToInt32(ddlDepartmentTo.SelectedValue);
                transfer.PreviousDesignationId = Convert.ToInt32(designationId.Value);
                transfer.CurrentDesignationId = Convert.ToInt32(ddlDesignationTo.SelectedValue);
                //transfer.CurrentCompanyId = Convert.ToInt32(ddlCompanyTo.SelectedValue);
                transfer.CurrentCompanyId = Convert.ToInt32(hfGLCompanyId.Value);
                transfer.CurrentProjectId = Convert.ToInt32(hfGLProjectId.Value);
                transfer.Description = Convert.ToString(txtDescription.Text);
                transfer.PreviousLocation = null;
                transfer.CurrentLocation = null;
                //transfer.ReportingDate = Convert.ToDateTime(txtReportingDate.Text);
                transfer.ReportingDate = CommonHelper.DateTimeToMMDDYYYY(txtReportingDate.Text);
                //transfer.JoinedDate = Convert.ToDateTime(txtJoinedDate.Text);
                transfer.ReportingToId = Convert.ToInt32(ddlReportingTo.SelectedValue);
                transfer.ReportingTo2Id = Convert.ToInt64(ddlReportingTo2.SelectedValue);

                transfer.TransferId = hfTransferId.Value == "" ? 0 : Convert.ToInt64(hfTransferId.Value);
                int randomDocId = Convert.ToInt32(RandomDocId.Value);
                string deletedDoc = hfDeletedDoc.Value;
                if (transfer.TransferId == 0)
                {
                    int tmpTransferId = 0;
                    transfer.CreatedBy = Convert.ToInt32(userInformationBO.UserInfoId);
                    status = empDa.SaveEmpTransfer(transfer, out tmpTransferId);
                    if (status)
                        OwnerIdForDocuments = Convert.ToInt32(tmpTransferId);

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
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        Clear();
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.EmpTransfer.ToString(), tmpTransferId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTransfer));
                    }
                }
                else
                {
                    transfer.LastModifiedBy = Convert.ToInt32(userInformationBO.UserInfoId);
                    status = empDa.UpdateEmpTransfer(transfer);
                    if (status)
                        OwnerIdForDocuments = Convert.ToInt32(transfer.TransferId);

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
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        Clear();
                        Boolean logStatus = hmUtility.CreateActivityLogEntity(
                            ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpTransfer.ToString(),
                            transfer.TransferId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTransfer));
                    }
                }

                if (!status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }

        private bool IsFormValid()
        {
            bool status = true;
            ContentPlaceHolder mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            UserControl empSearch = (UserControl)mpContentPlaceHolder.FindControl("employeeeSearch"); ;
            HiddenField employeeId = (HiddenField)empSearch.FindControl("hfEmployeeId");

            if (employeeId.Value == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Employee. ", AlertType.Warning);
                status = false;
                SetTab("entry");
            }
            else if (ddlDepartmentTo.SelectedValue == "0")
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Department To.", AlertType.Warning);
                status = false;
                ddlDepartmentTo.Focus();
                SetTab("entry");
            }
            else if (ddlReportingTo.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Reporting To 1.", AlertType.Warning);
                status = false;
                ddlReportingTo.Focus();
                SetTab("entry");
            }
            else if (ddlReportingTo2.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Reporting To 2.", AlertType.Warning);
                status = false;
                ddlReportingTo2.Focus();
                SetTab("entry");
            }
            else if (ddlReportingTo2.SelectedIndex == ddlReportingTo.SelectedIndex)
            {
                CommonHelper.AlertInfo(innboardMessage, "Reporting To 1 & Reporting To 2 Couldn't be same", AlertType.Warning);
                status = false;
                ddlReportingTo2.Focus();
                ddlReportingTo.Focus();
                SetTab("entry");
            }
            else if (string.IsNullOrEmpty(txtReportingDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Reporting Date.", AlertType.Warning);
                status = false;
                txtReportingDate.Focus();
                SetTab("entry");
            }
            else if (String.IsNullOrEmpty(txtTransferDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Transfer Date.", AlertType.Warning);
                status = false;
                txtTransferDate.Focus();
                SetTab("entry");
            }


            return status;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void gvEmployeeTransfer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            try
            {
                if (e.CommandName == "CmdEdit")
                {
                    int transferId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    EmployeeDA empDa = new EmployeeDA();
                    btnSave.Visible = isUpdatePermission;
                    FillForm(transferId);
                }
                else if (e.CommandName == "CmdApproved")
                {
                    int transferId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    EmployeeDA empDa = new EmployeeDA();

                    EmpTransferBO trn = new EmpTransferBO();
                    trn.TransferId = transferId;
                    trn.LastModifiedBy = userInformationBO.UserInfoId;
                    trn.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();

                    bool status = empDa.UpdateEmpTransferStatus(trn);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Approved, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(),
                            EntityTypeEnum.EntityType.EmpTransfer.ToString(), trn.TransferId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           "Employee Transfer Approved");
                        LoadGrid();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                    this.SetTab("SearchTab");
                }
                else if (e.CommandName == "CmdDelete")
                {
                    int transferId = Convert.ToInt32(e.CommandArgument.ToString());
                    string result = string.Empty;

                    EmployeeDA empDa = new EmployeeDA();
                    EmpTransferBO trn = new EmpTransferBO();
                    trn.TransferId = transferId;
                    trn.LastModifiedBy = userInformationBO.UserInfoId;
                    trn.ApprovedStatus = HMConstants.ApprovalStatus.Approved.ToString();

                    bool status = empDa.DeleteEmpTransfer(trn);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Delete, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(
                           ActivityTypeEnum.ActivityType.Delete.ToString(),
                           EntityTypeEnum.EntityType.EmpTransfer.ToString(),
                           trn.TransferId, ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTransfer));
                        LoadGrid();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                    }
                    this.SetTab("SearchTab");
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }

        protected void gvEmployeeTransfer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                var item = (EmpTransferBO)e.Row.DataItem;

                ImageButton imgApproved = (ImageButton)e.Row.FindControl("ImgApproved");
                ImageButton imgUpdate = (ImageButton)e.Row.FindControl("ImgUpdate");
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("ImgDelete");

                if (item.ApprovedStatus != HMConstants.ApprovalStatus.Approved.ToString() && item.ApprovedStatus != HMConstants.ApprovalStatus.Cancel.ToString())
                {
                    imgUpdate.Visible = isUpdatePermission;
                    imgDelete.Visible = isDeletePermission;
                    imgApproved.Visible = isSavePermission;
                }
                else
                {
                    imgUpdate.Visible = false;
                    imgDelete.Visible = false;
                    imgApproved.Visible = false;
                }
            }
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
            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("EmpTransferDocuments", randomId);
            if (id > 0)
                docList.AddRange(new DocumentsDA().GetDocumentsByUserTypeAndUserId("EmpTransferDocuments", (int)id));

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
        public static string LoadEmpTransferDocument(long id)
        {
            List<DocumentsBO> docList = new List<DocumentsBO>();

            docList = new DocumentsDA().GetDocumentsByUserTypeAndUserId("EmpTransferDocuments", (int)id);
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