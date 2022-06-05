using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

using HotelManagement.Data.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;
using System.Collections;
using Newtonsoft.Json;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpTraining : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        ArrayList arrayDelete;
        int temEmpId;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            this.AddEditOrDeleteDetail();
            if (!IsPostBack)
            {
                CheckPermission();
                Session["TrainingDetailList"] = null;
                LoadTrainingOrganizer();
                LoadTrainingType();
                LoadDepartment();

                string editId = Request.QueryString["editId"];
                if (!string.IsNullOrWhiteSpace(editId))
                {
                    btnEmpTraining.Visible = isUpdatePermission;
                    int Id = Convert.ToInt32(editId);
                    if (Id > 0)
                    {
                        FillForm(Id);
                    }
                }
            }

        }

        private void AddEditOrDeleteDetail()
        {
            //Delete------------
            if (Session["arrayDelete"] == null)
            {
                arrayDelete = new ArrayList();
                Session.Add("arrayDelete", arrayDelete);
            }
            else
                arrayDelete = Session["arrayDelete"] as ArrayList;
        }
        protected void btnEmpTrainingSave_Click(object sender, EventArgs e)
        {
            int tmpTrainingId = 0;
            bool status = false;
            try
            {
                EmpTrainingBO empTraining = new EmpTrainingBO();
                EmpTrainingDA empTrainingDA = new EmpTrainingDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                empTraining.Trainer = txtTrainer.Text;
                empTraining.TrainingTypeId = Convert.ToInt32(ddlTraining.SelectedValue);
                empTraining.OrganizerId = ddlOrganizer.SelectedIndex;
                if (!string.IsNullOrEmpty(txtFromDate.Text))
                    empTraining.StartDate = hmUtility.GetDateTimeFromString(txtFromDate.Text.Trim(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                else
                    empTraining.StartDate = DateTime.Now;
                if (!string.IsNullOrEmpty(txtToDate.Text))
                    empTraining.EndDate = hmUtility.GetDateTimeFromString(txtToDate.Text.Trim(), hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
                else
                    empTraining.EndDate = DateTime.Now;
                empTraining.Location = txtLocation.Text;
                empTraining.ReminderHour = ddlSendMail.SelectedIndex;                
                empTraining.Reminder = ddlEnableReminder.SelectedValue == "0" ? true : false;

                empTraining.Remarks = txtRemarks.Text;
                empTraining.Discussed = txtDiscussed.Text;
                empTraining.CallToAction = txtCallToAction.Text;
                empTraining.Conclusion = txtConclusion.Text;


                int traininfId = 0;
                if (hfTrainingId.Value != "")
                {
                    traininfId = Convert.ToInt32(hfTrainingId.Value);
                }

                if (traininfId == 0)
                {
                    int trainingId = empTrainingDA.SaveEmpTrainingInfo(empTraining, out tmpTrainingId, Session["TrainingDetailList"] as List<EmpTrainingDetailBO>);

                    if (trainingId > 0)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmpTraining.ToString(), tmpTrainingId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTraining));

                        ClearForm();
                        ClearDetail();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Save Operation Failed.", AlertType.Warning);
                    }
                }
                else
                {
                    empTraining.TrainingId = traininfId;
                    status = empTrainingDA.UpdateEmpTrainingInfo(empTraining, Session["TrainingDetailList"] as List<EmpTrainingDetailBO>, Session["arrayDelete"] as ArrayList);

                    if (status)
                    {
                        CommonHelper.AlertInfo(innboardMessage, AlertMessage.Update, AlertType.Success);

                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmpTraining.ToString(), empTraining.TrainingId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTraining));
                        ClearForm();
                        ClearDetail();
                    }
                    else
                    {
                        CommonHelper.AlertInfo(innboardMessage, "Update Operation Failed.", AlertType.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
            }
        }
        protected void btnEmpTrainingClear_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Payroll/frmEmpTraining.aspx");
        }
        protected void btnAddTrainingDetail_Click(object sender, EventArgs e)
        {
            List<EmpTrainingDetailBO> addList = new List<EmpTrainingDetailBO>();
            addList = JsonConvert.DeserializeObject<List<EmpTrainingDetailBO>>(hfSaveObj.Value);

            ContentPlaceHolder mpContentPlaceHolder;
            UserControl Allocation;
            HiddenField EmployeeId;
            HiddenField EmployeeName;
            TextBox empName;

            mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            Allocation = (UserControl)mpContentPlaceHolder.FindControl("employeeSearch");
            EmployeeId = (HiddenField)Allocation.FindControl("hfEmployeeId");
            EmployeeName = (HiddenField)Allocation.FindControl("hfEmployeeName");
            empName = (TextBox)Allocation.FindControl("txtEmployeeName");
            

            if (addList.Count == 0)
            {
                int dynamicDetailId = 0;
                List<EmpTrainingDetailBO> List = Session["TrainingDetailList"] == null ? new List<EmpTrainingDetailBO>() : Session["TrainingDetailList"] as List<EmpTrainingDetailBO>;

                if (!string.IsNullOrWhiteSpace(lblHiddenId.Text))
                    dynamicDetailId = Convert.ToInt32(lblHiddenId.Text);



                EmpTrainingDetailBO detailBO = dynamicDetailId == 0 ? new EmpTrainingDetailBO() : List.Where(x => x.TrainingDetailId == dynamicDetailId).FirstOrDefault();
                if (List.Contains(detailBO))
                    List.Remove(detailBO);

                for (int i = 0; i < List.Count; i++)
                    temEmpId = List[i].EmpId;
                detailBO.EmpId = Convert.ToInt32(EmployeeId.Value);
                if (detailBO.EmpId == temEmpId)
                {
                    CommonHelper.AlertInfo(innboardMessage, "Cant add one employee twice.", AlertType.Warning);
                }
                else
                {
                    //var temId = detailBO.EmpId;
                    detailBO.TrainingDetailId = dynamicDetailId;
                    detailBO.EmpName = EmployeeName.Value;

                    List.Add(detailBO);

                    Session["TrainingDetailList"] = List;

                    this.gvTrainingDetail.DataSource = Session["TrainingDetailList"] as List<EmpTrainingDetailBO>;
                    this.gvTrainingDetail.DataBind();
                }
            }
            else
            {
                Session["TrainingDetailList"] = addList;

                this.gvTrainingDetail.DataSource = addList as List<EmpTrainingDetailBO>;
                this.gvTrainingDetail.DataBind();

                ddlType.SelectedIndex = 0;
                ddlDepartment.SelectedIndex = 0;
            }
            //mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("ContentPlaceHolder1");
            //Allocation = (UserControl)mpContentPlaceHolder.FindControl("employeeSearch");
            //(HiddenField)Allocation.FindControl("hfEmployeeId") = null;
            //(HiddenField)Allocation.FindControl("hfEmployeeName") = null;
            
            ddlType.SelectedValue = hfAddBy.Value;
            empName.Text = string.Empty;


        }
        protected void gvTrainingDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int _TrainingDetailId;


            if (e.CommandName == "CmdEdit")
            {
                _TrainingDetailId = Convert.ToInt32(e.CommandArgument.ToString());
                lblHiddenId.Text = _TrainingDetailId.ToString();
                var trainingDetailBO = (List<EmpTrainingDetailBO>)Session["TrainingDetailList"];
                var trainingDetail = trainingDetailBO.Where(x => x.TrainingDetailId == _TrainingDetailId).FirstOrDefault();
                if (trainingDetail != null && trainingDetail.TrainingDetailId > 0)
                {
                    btnAddTrainingDetail.Text = "Edit";
                }
                else
                {
                    btnAddTrainingDetail.Text = "Add";
                }
            }
            else if (e.CommandName == "CmdDelete")
            {
                _TrainingDetailId = Convert.ToInt32(e.CommandArgument.ToString());
                var trainingDetailBO = (List<EmpTrainingDetailBO>)Session["TrainingDetailList"];
                var trainingDetail = trainingDetailBO.Where(x => x.TrainingDetailId == _TrainingDetailId).FirstOrDefault();
                trainingDetailBO.Remove(trainingDetail);
                Session["TrainingDetailList"] = trainingDetailBO;
                arrayDelete.Add(_TrainingDetailId);
                this.gvTrainingDetail.DataSource = Session["TrainingDetailList"] as List<EmpTrainingDetailBO>;
                this.gvTrainingDetail.DataBind();
            }

        }

        private void LoadTrainingOrganizer()
        {
            EmpTrainingOrganizerDA organizerDA = new EmpTrainingOrganizerDA();
            ddlOrganizer.DataSource = organizerDA.GetAllOrganizer();
            ddlOrganizer.DataTextField = "OrganizerName";
            ddlOrganizer.DataValueField = "OrganizerId";
            ddlOrganizer.DataBind();

            ddlSearchOrganizer.DataSource = organizerDA.GetAllOrganizer();
            ddlSearchOrganizer.DataTextField = "OrganizerName";
            ddlSearchOrganizer.DataValueField = "OrganizerId";
            ddlSearchOrganizer.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlOrganizer.Items.Insert(0, item);

            ddlSearchOrganizer.Items.Insert(0, item);
        }
        private void LoadTrainingType()
        {
            EmpTrainingDA trainingDa = new EmpTrainingDA();
            List<PayrollEmpTrainingTypeBO> training = new List<PayrollEmpTrainingTypeBO>();
            training = trainingDa.GetTrainingType();

            ddlTraining.DataSource = training;
            ddlTraining.DataTextField = "TrainingName";
            ddlTraining.DataValueField = "TrainingTypeId";
            ddlTraining.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlTraining.Items.Insert(0, item);
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            ddlDepartment.DataSource = entityDA.GetDepartmentInfo();
            ddlDepartment.DataTextField = "Name";
            ddlDepartment.DataValueField = "DepartmentId";
            ddlDepartment.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlDepartment.Items.Insert(0, item);
        }
        private void FillForm(int trainingId)
        {
            EmpTrainingBO training = new EmpTrainingBO();
            EmpTrainingDA trainingdA = new EmpTrainingDA();
            training = trainingdA.GetEmployeeTrainingById(trainingId);

            hfTrainingId.Value = training.TrainingId.ToString();
            txtTrainer.Text = training.Trainer;
            ddlTraining.SelectedValue = training.TrainingTypeId.ToString();
            training.TrainingName = training.TrainingName;
            ddlOrganizer.SelectedValue = training.OrganizerId.ToString();
            txtLocation.Text = training.Location;
            txtFromDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(training.StartDate.Date);
            txtToDate.Text = CommonHelper.DateTimeClientFormatWiseConversionForDisplay(training.EndDate);
            ddlEnableReminder.SelectedValue = training.Reminder.ToString();
            ddlSendMail.SelectedValue = training.ReminderHour.ToString();
            txtRemarks.Text = training.Remarks;
            txtDiscussed.Text = training.Discussed;
            txtCallToAction.Text = training.CallToAction;
            txtConclusion.Text = training.Conclusion;

            btnEmpTraining.Text = "Update";

            List<EmpTrainingDetailBO> detailList = new List<EmpTrainingDetailBO>();
            EmpTrainingDA empTrainingDA = new EmpTrainingDA();
            detailList = empTrainingDA.GetTrainingDetailInfoById(trainingId);

            Session["TrainingDetailList"] = detailList;
            this.gvTrainingDetail.DataSource = Session["TrainingDetailList"] as List<EmpTrainingDetailBO>;
            this.gvTrainingDetail.DataBind();
        }
        private void ClearForm()
        {
            btnEmpTraining.Visible = isSavePermission;
            hfTrainingId.Value = string.Empty;
            txtTrainer.Text = string.Empty;
            ddlTraining.SelectedValue = "0";
            ddlOrganizer.SelectedValue = "0";
            txtLocation.Text = string.Empty;
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            ddlEnableReminder.SelectedIndex = 0;
            ddlSendMail.SelectedValue = string.Empty;
            txtRemarks.Text = string.Empty;
            txtDiscussed.Text = string.Empty;
            txtCallToAction.Text = string.Empty;
            txtConclusion.Text = string.Empty;
            btnEmpTraining.Text = "Save";
        }
        private void ClearDetail()
        {
            Session["TrainingDetailList"] = null;
            this.gvTrainingDetail.DataSource = Session["TrainingDetailList"] as List<EmpTrainingDetailBO>;
            this.gvTrainingDetail.DataBind();
        }
        private void CheckPermission()
        {
            btnEmpTraining.Visible = isSavePermission;
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
        }

        [WebMethod]
        public static GridViewDataNPaging<EmpTrainingBO, GridPaging> SearchTrainingAndLoadGridInformation(string trainer, string courseName, string organizerId, string location, string startDate, string endDate, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;
            DateTime? fromDate = null, toDate = null;


            if (!string.IsNullOrEmpty(startDate))
            {
                //fromDate = Convert.ToDateTime(startDate);
                //string tempFromDate = CommonHelper.DateTimeClientFormatWiseConversionForSaveUpdate(Convert.ToDateTime(startDate));
                fromDate = CommonHelper.DateTimeToMMDDYYYY(startDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                //toDate = Convert.ToDateTime(endDate);
                //string tempToDate = CommonHelper.DateTimeClientFormatWiseConversionForSaveUpdate(Convert.ToDateTime(endDate));
                toDate = CommonHelper.DateTimeToMMDDYYYY(endDate);
            }

            GridViewDataNPaging<EmpTrainingBO, GridPaging> myGridData = new GridViewDataNPaging<EmpTrainingBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            EmpTrainingDA trainingDa = new EmpTrainingDA();
            List<EmpTrainingBO> trainingList = new List<EmpTrainingBO>();
            trainingList = trainingDa.GetOrganizerInfoBySearchCriteriaForPagination(trainer, courseName, organizerId, location, fromDate, toDate, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<EmpTrainingBO> distinctItems = new List<EmpTrainingBO>();
            distinctItems = trainingList.GroupBy(test => test.TrainingId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static ReturnInfo DeleteEmpTrainingInfoById(string trainingId)
        {
            string message = string.Empty;
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            try
            {
                EmpTrainingDA trainingDA = new EmpTrainingDA();
                int trainId = Convert.ToInt32(trainingId);
                bool status = trainingDA.DeleteEmpTrainingInfoById(trainId);
                if (status)
                {
                    rtninf.IsSuccess = true;
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.EmpTraining.ToString(), trainId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpTraining));
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
        [WebMethod]
        public static List<EmployeeBO> LoadEmployeeByDepartment(string departmentId)
        {
            int departId = 0;
            if (!string.IsNullOrEmpty(departmentId))
            {
                departId = Convert.ToInt32(departmentId);
            }
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmployeeByDepartment(departId);

            return empList;
        }
    }
}