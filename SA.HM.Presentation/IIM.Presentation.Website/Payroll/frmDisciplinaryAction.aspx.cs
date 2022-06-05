using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmDisciplinaryAction : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                checkObjectPermission();
                this.LoadDisciplinaryActionType();
                this.LoadDisciplinaryActionReason();
                this.LoadEmployee();
                this.LoadProposedDisciplinaryAction();

                string editId = Request.QueryString["editId"];
                if (!string.IsNullOrWhiteSpace(editId))
                {
                    int Id = Convert.ToInt32(editId);
                    if (Id > 0)
                    {
                        FillForm(Id);
                        SetTab("EntryTab");
                    }
                }
            }
            
        }

        protected void checkObjectPermission()
        {
            btnSave.Visible = isSavePermission;
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsFormValid())
                {
                    return;
                }

                DisciplinaryActionBO actionBO = new DisciplinaryActionBO();
                DisciplinaryActionDA disActionDA = new DisciplinaryActionDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                actionBO.ActionTypeId = Convert.ToInt16(ddlActionTypeId.SelectedValue);
                actionBO.DisciplinaryActionReasonId = Convert.ToInt32(ddlActionReasonId.SelectedValue);
                actionBO.EmployeeId = Convert.ToInt32(ddlEmployeeId.SelectedValue);
                if (ddlProposedAction.SelectedIndex != 0)
                {
                    actionBO.ProposedActionId = Convert.ToInt32(ddlProposedAction.SelectedValue);
                }
                actionBO.ActionBody = txtActionBody.Text;
                if (!string.IsNullOrWhiteSpace(txtApplicableDate.Text))
                {
                    //actionBO.ApplicableDate = Convert.ToDateTime(txtApplicableDate.Text);
                    actionBO.ApplicableDate = CommonHelper.DateTimeToMMDDYYYY(txtApplicableDate.Text);
                }

                if (string.IsNullOrWhiteSpace(hfDisciplinaryActionId.Value))
                {
                    actionBO.CreatedBy = userInformationBO.UserInfoId;
                    Boolean status = disActionDA.SaveDisciplinaryActionInfo(actionBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.DisciplinaryAction.ToString(), 0,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DisciplinaryAction));
                        this.Cancel();
                    }
                }
                else
                {
                    actionBO.DisciplinaryActionId = Convert.ToInt64(hfDisciplinaryActionId.Value);
                    actionBO.LastModifiedBy = userInformationBO.UserInfoId;
                    Boolean status = disActionDA.UpdateDisciplinaryActionInfo(actionBO);
                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.DisciplinaryAction.ToString(), actionBO.DisciplinaryActionId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DisciplinaryAction));
                        this.Cancel();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }

        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlActionTypeId.SelectedIndex = 0;
            ddlActionReasonId.SelectedIndex = 0;
            ddlEmployeeId.SelectedIndex = 0;
            txtActionBody.Text = string.Empty;
            txtApplicableDate.Text = string.Empty;
            ddlProposedAction.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            hfDisciplinaryActionId.Value = string.Empty;
        }

        private bool IsFormValid()
        {
            bool status = true;

            if (ddlActionTypeId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Action Type.", AlertType.Warning);
                status = false;
                ddlActionTypeId.Focus();
                SetTab("EntryTab");
            }
            else if (ddlActionReasonId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + "Action Reason.", AlertType.Warning);
                status = false;
                ddlActionTypeId.Focus();
                SetTab("EntryTab");
            }
            else if (ddlEmployeeId.SelectedIndex == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Employee.", AlertType.Warning);
                status = false;
                ddlActionTypeId.Focus();
                SetTab("EntryTab");
            }
            else if (String.IsNullOrEmpty(txtApplicableDate.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.TextTypeValidation + " Applicable Date.", AlertType.Warning);
                status = false;
                txtApplicableDate.Focus();
                SetTab("EntryTab");
            }

            return status;
        }
        public void FillForm(int editId)
        {
            DisciplinaryActionBO actionBO = new DisciplinaryActionBO();
            DisciplinaryActionDA disActionDA = new DisciplinaryActionDA();
            actionBO = disActionDA.GetDisciplinaryActionById(editId);
            ddlActionTypeId.SelectedValue = actionBO.ActionTypeId.ToString();
            ddlActionReasonId.SelectedValue = actionBO.DisciplinaryActionReasonId.ToString();
            ddlEmployeeId.SelectedValue = actionBO.EmployeeId.ToString();
            ddlProposedAction.SelectedValue = actionBO.ProposedActionId.ToString();
            txtActionBody.Text = actionBO.ActionBody;
            if (actionBO.ApplicableDate != null)
            {
                txtApplicableDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(actionBO.ApplicableDate)); 
            }
            else {
                txtApplicableDate.Text = string.Empty;
            }

            hfDisciplinaryActionId.Value = actionBO.DisciplinaryActionId.ToString();
            btnSave.Visible = isUpdatePermission;
            btnSave.Text = "Update";
        }
        private void Cancel()
        {
            ddlActionTypeId.SelectedIndex = 0;
            ddlActionReasonId.SelectedIndex = 0;
            ddlEmployeeId.SelectedIndex = 0;
            txtActionBody.Text = string.Empty;
            txtApplicableDate.Text = string.Empty;
            ddlProposedAction.SelectedIndex = 0;
            this.btnSave.Text = "Save";
            hfDisciplinaryActionId.Value = string.Empty;
        }
        private void LoadDisciplinaryActionType()
        {
            List<DisciplinaryActionTypeBO> typeList = new List<DisciplinaryActionTypeBO>();
            DisciplinaryActionDA disActionDA = new DisciplinaryActionDA();
            typeList = disActionDA.GetDisciplinaryActionTypeList();

            ddlActionTypeId.DataSource = typeList;
            ddlActionTypeId.DataTextField = "ActionName";
            ddlActionTypeId.DataValueField = "DisciplinaryActionTypeId";
            ddlActionTypeId.DataBind();

            ddlSActionTypeId.DataSource = typeList;
            ddlSActionTypeId.DataTextField = "ActionName";
            ddlSActionTypeId.DataValueField = "DisciplinaryActionTypeId";
            ddlSActionTypeId.DataBind();
             
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlActionTypeId.Items.Insert(0, item);
            ddlSActionTypeId.Items.Insert(0, item);            
        }
        private void LoadDisciplinaryActionReason()
        {
            List<DisciplinaryActionReasonBO> reasonList = new List<DisciplinaryActionReasonBO>();
            DisciplinaryActionDA disActionDA = new DisciplinaryActionDA();
            reasonList = disActionDA.GetDisciplinaryActionReasonList();

            ddlActionReasonId.DataSource = reasonList;
            ddlActionReasonId.DataTextField = "ActionReason";
            ddlActionReasonId.DataValueField = "DisciplinaryActionReasonId";
            ddlActionReasonId.DataBind();

            ddlSActionreasonId.DataSource = reasonList;
            ddlSActionreasonId.DataTextField = "ActionReason";
            ddlSActionreasonId.DataValueField = "DisciplinaryActionReasonId";
            ddlSActionreasonId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlActionReasonId.Items.Insert(0, item);
            ddlSActionreasonId.Items.Insert(0, item);
        }
        private void LoadProposedDisciplinaryAction()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("EmpDisciplinaryProposedAction", hmUtility.GetDropDownFirstValue());

            this.ddlProposedAction.DataSource = fields;
            this.ddlProposedAction.DataTextField = "FieldValue";
            this.ddlProposedAction.DataValueField = "FieldId";
            this.ddlProposedAction.DataBind();

            this.ddlSProposedActionId.DataSource = fields;
            this.ddlSProposedActionId.DataTextField = "FieldValue";
            this.ddlSProposedActionId.DataValueField = "FieldId";
            this.ddlSProposedActionId.DataBind();
        }
        private void LoadEmployee()
        {
            EmployeeDA employeeDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = employeeDA.GetEmployeeInfo();

            ddlEmployeeId.DataSource = empList;
            ddlEmployeeId.DataTextField = "EmployeeName";
            ddlEmployeeId.DataValueField = "EmpId";
            ddlEmployeeId.DataBind();

            ddlSEmployeeId.DataSource = empList;
            ddlSEmployeeId.DataTextField = "EmployeeName";
            ddlSEmployeeId.DataValueField = "EmpId";
            ddlSEmployeeId.DataBind();

            ListItem itemEmployee = new ListItem();
            itemEmployee.Value = "0";
            itemEmployee.Text = hmUtility.GetDropDownFirstValue();
            ddlEmployeeId.Items.Insert(0, itemEmployee);
            ddlSEmployeeId.Items.Insert(0, itemEmployee);
        }
        private void SetTab(string TabName)
        {
            if (TabName == "SearchTab")
            {
                B.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                A.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
            else if (TabName == "EntryTab")
            {
                A.Attributes.Add("class", "ui-state-default ui-corner-top ui-tabs-active ui-state-active");
                B.Attributes.Add("class", "ui-state-default ui-corner-top");
            }
        }
        [WebMethod]
        public static GridViewDataNPaging<DisciplinaryActionBO, GridPaging> SearchDisciplinaryAction(string actionType, string actionReason, string emp, string proposeActionId, string fromDate, string toDate, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;
            int actiontypeId = 0, actionReasonId = 0, empId = 0, proposedActionId = 0;           
            if (!string.IsNullOrWhiteSpace(actionType))
            {
                actiontypeId = Convert.ToInt16(actionType);
            }
            if (!string.IsNullOrWhiteSpace(actionReason))
            {
                actionReasonId = Convert.ToInt32(actionReason);
            }
            if (!string.IsNullOrWhiteSpace(emp))
            {
                empId = Convert.ToInt32(emp);
            }
            if (!string.IsNullOrWhiteSpace(proposeActionId))
            {
                proposedActionId = Convert.ToInt32(proposeActionId);
            }
            if (fromDate != "" && toDate == "")
            {
                toDate = DateTime.Now.ToShortDateString();
            }
            GridViewDataNPaging<DisciplinaryActionBO, GridPaging> myGridData = new GridViewDataNPaging<DisciplinaryActionBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            DisciplinaryActionDA disActionDA = new DisciplinaryActionDA();
            List<DisciplinaryActionBO> actionList = new List<DisciplinaryActionBO>();
            actionList = disActionDA.GetDisciplinaryActionBySearchCriteriaForPaging(actiontypeId, actionReasonId, empId, proposedActionId, fromDate, toDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<DisciplinaryActionBO> distinctItems = new List<DisciplinaryActionBO>();
            distinctItems = actionList.GroupBy(test => test.DisciplinaryActionId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo DeleteData(int sActionId)
        {
            HMUtility hmUtility = new HMUtility();
            string result = string.Empty;
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                HMCommonDA hmCommonDA = new HMCommonDA();
                Boolean status = hmCommonDA.DeleteInfoById("PayrollDisciplinaryAction", "DisciplinaryActionId", sActionId);

                if (status)
                {
                    rtninf.IsSuccess = true;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.DisciplinaryAction.ToString(), sActionId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.DisciplinaryAction));
                }
                else
                {
                    rtninf.IsSuccess = false;
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rtninf;
        }
    }
}