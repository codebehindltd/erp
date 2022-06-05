using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using System.Web.Services;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using System.Text;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpYearlyLeave : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployee();
                LoadLeaveType();
                LoadDummyGridData();
            }
        }

        private void CheckObjectPermission()
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            ObjectPermissionBO objectPermissionBO = new ObjectPermissionBO();
            objectPermissionBO = hmUtility.ObjectPermission(userInformationBO.UserInfoId, HMConstants.ApplicationFormName.frmDepartment.ToString());
            isSavePermission = objectPermissionBO.IsSavePermission;
            isDeletePermission = objectPermissionBO.IsDeletePermission;
            btnSave.Visible = isSavePermission;
        }

        public void LoadEmployee()
        {
            EmployeeDA employeeDA = new EmployeeDA();
            EmployeeBO employeeBO = new EmployeeBO();
            List<EmployeeBO> fields = new List<EmployeeBO>();
            fields = employeeDA.GetEmployeeInfo();
            /*this.ddlEmployee.DataSource = fields;
            this.ddlEmployee.DataTextField = "EmployeeName";
            this.ddlEmployee.DataValueField = "EmpId";
            this.ddlEmployee.DataBind();*/

            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstValue();
            //this.ddlEmployee.Items.Insert(0, item);
        }
        public void LoadLeaveType()
        {
            LeaveTypeDA leaveTypeDA = new LeaveTypeDA();
            LeaveTypeBO leaveTypeBO = new LeaveTypeBO();
            List<LeaveTypeBO> fields = new List<LeaveTypeBO>();
            fields = leaveTypeDA.GetLeaveTypeInfo();
            this.ddlLeaveType.DataSource = fields;
            this.ddlLeaveType.DataTextField = "TypeName";
            this.ddlLeaveType.DataValueField = "LeaveTypeId";
            this.ddlLeaveType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlLeaveType.Items.Insert(0, item);
        }

        [WebMethod]
        public static ReturnInfo SaveYearlyLeave(string yearlyLeaveId, int employeeId, int leaveTypeId, int leaveQuantity)
        {
            ReturnInfo rtninf = new ReturnInfo();
            HMUtility hmUtility = new HMUtility();
            EmployeeYearlyLeaveBO yearlyLeaveBO = new EmployeeYearlyLeaveBO();
            yearlyLeaveBO.EmpId = employeeId;
            yearlyLeaveBO.LeaveTypeId = leaveTypeId;
            yearlyLeaveBO.LeaveQuantity = leaveQuantity;
            EmployeeYearlyLeaveDA yearlyLeaveDA = new EmployeeYearlyLeaveDA();
            Boolean status = false;
            string message = string.Empty;
            int tmpId;

            if (string.IsNullOrWhiteSpace(yearlyLeaveId))
            {
                status = yearlyLeaveDA.SaveLeaveInformation(yearlyLeaveBO, out tmpId);
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmployeeYearlyLeave.ToString(), tmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeeYearlyLeave));
            }
            else
            {
                yearlyLeaveBO.YearlyLeaveId = Int32.Parse(yearlyLeaveId.Trim());
                status = yearlyLeaveDA.UpdateLeaveInformation(yearlyLeaveBO);
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmployeeYearlyLeave.ToString(), yearlyLeaveBO.YearlyLeaveId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeeYearlyLeave));
            }

            if (!status)
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            return rtninf; 
        }

        private void LoadDummyGridData()
        {
            List<EmployeeYearlyLeaveBO> leave = new List<EmployeeYearlyLeaveBO>();

            EmployeeYearlyLeaveBO obj = new EmployeeYearlyLeaveBO();
            obj.YearlyLeaveId = 0;
            obj.LeaveQuantity = 0;
            obj.LeaveType = "";
            leave.Add(obj);

            this.gvEmpLeave.DataSource = leave;
            this.gvEmpLeave.DataBind();
        }

        protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.LoadGridView(Int32.Parse(ddlEmployee.SelectedValue.ToString()));
        }

        [WebMethod]
        public static ReturnInfo DeleteData(int sEmpId)
        {
            string message = string.Empty;
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();

                Boolean status = hmCommonDA.DeleteInfoById("PayrollEmpYearlyLeave", "YearlyLeaveId", sEmpId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.EmployeeYearlyLeave.ToString(), sEmpId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeeYearlyLeave));
                }
            }
            catch (Exception ex)
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }

        [WebMethod]
        public static EmployeeYearlyLeaveBO FillForm(int EditId)
        {
            EmployeeYearlyLeaveBO yearlyLeaveBO = new EmployeeYearlyLeaveBO();
            EmployeeYearlyLeaveDA yearlyLeaveDA = new EmployeeYearlyLeaveDA();
            yearlyLeaveBO = yearlyLeaveDA.GetAllLeaveByLeaveID(EditId);

            yearlyLeaveBO.YearlyLeaveId = EditId;

            return yearlyLeaveBO;
        }

        [WebMethod]
        public static GridViewDataNPaging<EmployeeYearlyLeaveBO, GridPaging> YearlyLeaveLoad(int empId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<EmployeeYearlyLeaveBO, GridPaging> myGridData = new GridViewDataNPaging<EmployeeYearlyLeaveBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            EmployeeYearlyLeaveDA yearlyLeaveDA = new EmployeeYearlyLeaveDA();
            List<EmployeeYearlyLeaveBO> employeeLeave = yearlyLeaveDA.GetAllLeaveByEmployeeIDForGridPaging(empId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            myGridData.GridPagingProcessing(employeeLeave, totalRecords);

            return myGridData;
        }

        /*  .... Server Side Code 
         
         * string DeleteSuccess = Request.QueryString["DeleteConfirmation"];
            if (!string.IsNullOrWhiteSpace(DeleteSuccess))
            {
                this.isMessageBoxEnable = 2;
                this.lblMessage.Text = "Delete Operation Successfull.";
            }
         * 
           protected void btnSaveYarlyLeave_Click(object sender, EventArgs e)
        {
            EmployeeYearlyLeaveBO yearlyLeaveBO = new EmployeeYearlyLeaveBO();
            // yearlyLeaveBO.EmpId = Int32.Parse(ddlEmployee.SelectedValue.ToString());
            yearlyLeaveBO.LeaveTypeId = Int32.Parse(ddlLeaveType.SelectedValue.ToString());
            yearlyLeaveBO.LeaveQuantity = Int32.Parse(txtLeave.Text.ToString());
            EmployeeYearlyLeaveDA yearlyLeaveDA = new EmployeeYearlyLeaveDA();
            Boolean status = yearlyLeaveDA.SaveLeaveInformation(yearlyLeaveBO);
        }
        
         private void LoadGridView(int userID)
        {
            EmployeeYearlyLeaveDA yearlyLeaveDA = new EmployeeYearlyLeaveDA();
            List<EmployeeYearlyLeaveBO> files = yearlyLeaveDA.GetAllLeaveByID(userID);
            this.gvEmpLeave.DataSource = files;
            this.gvEmpLeave.DataBind();
        }

        public bool isValidForm()
        {
            bool status = true;
            //    if (this.ddlEmployee.SelectedValue == "0")
            //    {
            //        status = false;
            //        this.isMessageBoxEnable = 1;
            //        this.isNewAddButtonEnable = -1;
            //        lblMessage.Text = "Please Select Employee";
            //        this.ddlEmployee.Focus();
            //    }
            //    if (this.ddlLeaveType.SelectedValue == "0")
            //    {
            //        status = false;
            //        this.isMessageBoxEnable = 1;
            //        this.isNewAddButtonEnable = -1;
            //        lblMessage.Text = "Please select Leave Type.";
            //        this.ddlLeaveType.Focus();
            //    }
            //    if (string.IsNullOrWhiteSpace(this.txtLeave.Text))
            //    {
            //        status = false;
            //        this.isMessageBoxEnable = 1;
            //        this.isNewAddButtonEnable = -1;
            //        lblMessage.Text = "Please provide Leave Amount.";
            //        this.txtLeave.Focus();
            //    }

            return status;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!isValidForm())
            {
                return;
            }

            EmployeeYearlyLeaveBO yearlyLeaveBO = new EmployeeYearlyLeaveBO();
            //yearlyLeaveBO.EmpId = Int32.Parse(ddlEmployee.SelectedValue.ToString());
            yearlyLeaveBO.LeaveTypeId = Int32.Parse(ddlLeaveType.SelectedValue.ToString());
            yearlyLeaveBO.LeaveQuantity = Int32.Parse(txtLeave.Text.ToString());

            EmployeeYearlyLeaveDA yearlyLeaveDA = new EmployeeYearlyLeaveDA();
            if (string.IsNullOrWhiteSpace(hiddenLeaveID.Value))
            {
                Boolean status = yearlyLeaveDA.SaveLeaveInformation(yearlyLeaveBO);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Saved Operation Successfull.";
                    // this.LoadGridView(yearlyLeaveBO.EmpId);
                    this.Cancel();
                }
            }
            else
            {
                yearlyLeaveBO.YearlyLeaveId = Int32.Parse(hiddenLeaveID.Value.ToString());
                Boolean status = yearlyLeaveDA.UpdateLeaveInformation(yearlyLeaveBO);
                if (status)
                {
                    this.isMessageBoxEnable = 2;
                    lblMessage.Text = "Update Operation Successfull.";
                    //this.LoadGridView(yearlyLeaveBO.EmpId);
                    this.Cancel();
                }
            }
        }
        private void Cancel()
        {
            this.txtLeave.Text = string.Empty;
            //this.ddlEmployee.SelectedIndex = 0;
            this.ddlLeaveType.SelectedIndex = 0;
            this.btnSave.Text = "Save";

        }
        
         public void Clear()
        {
            this.txtLeave.Text = string.Empty;
            // this.ddlEmployee.SelectedValue = "0";
            this.ddlLeaveType.SelectedValue = "0";
            this.btnSave.Text = "Save";
        }
        protected void Close_Click(object sender, EventArgs e)
        {
            Clear();
        }
 
        protected int isMessageBoxEnable = -1;
        protected int isNewAddButtonEnable = 1;
 
         * 
         * //        var xMessage = '<%=isMessageBoxEnable%>';
//        if (xMessage > -1) {
//            MessagePanelShow();

//            if (xMessage == 2) {
//                $('#MessageBox').addClass("alert-success-info").removeClass("alert alert-info");
//            }
//        }
//        else {
//            MessagePanelHide();
//        }

//        var xNewAdd = '<%=isNewAddButtonEnable%>';
//        if (xNewAdd > -1) {
//            NewAddButtonPanelShow();
//        }
//        else {
//            NewAddButtonPanelHide();
//            EntryPanelVisibleTrue();
//        }
         * 
         * 
         * 
         * 
         * 
         */
    }
}