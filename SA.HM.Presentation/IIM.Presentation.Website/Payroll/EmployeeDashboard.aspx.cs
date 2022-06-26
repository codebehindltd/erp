using HotelManagement.Data.DocumentManagement;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.PurchaseManagment;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Data.TaskManagment;
using HotelManagement.Entity.DocumentManagement;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.TaskManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Presentation.Website.HMCommon;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ListItem = System.Web.UI.WebControls.ListItem;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class EmployeeDashboard : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLeaveType();
                LoadLeaveMode();
                LoadEmployeeInfo();
            }

            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            int empId = userInformationBO.EmpId;
            if (empId > 0)
            {
                LoadTodaysSummary(empId);
                LoadBasicEmployeInfo(empId);
                GetLeaveTakenNBalanceByEmployee(empId);
                LoadTodaysLeaveTakenEmployee();
                LoadTomorrowsLeaveTakenEmployee();
                LoadUpcommingTrainningsByEmployee(empId);
                LoadTask(empId);
                LoadUpcommingBirthdays();
                LoadUpcommingWorkAnniversary();
                LoadEmpProvisionPeriod();
                LoadNotice(empId);
                LoadFixedAsset(empId);
                LoadAssignedDocument(empId);
                LoadLetter(empId);
                LoadCompanyEmployeeCount();
                LoadHolidayInformation();
                LoadReminder(empId);
                hfEmployeeId.Value = Convert.ToString(empId);
            }
        }
        private void LoadLeaveMode()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("LeaveMode", hmUtility.GetDropDownFirstValue());
            fields.RemoveAt(0);

            ddlLeaveMode.DataSource = fields;
            ddlLeaveMode.DataTextField = "Description";
            ddlLeaveMode.DataValueField = "FieldValue";
            ddlLeaveMode.DataBind();

            //System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
            //item.Value = "0";
            //item.Text = hmUtility.GetDropDownFirstValue();
            //ddlLeaveMode.Items.Insert(0, item);
        }
        private void LoadTask(int empId)
        {
            List<SMTask> taskList = new List<SMTask>();
            AssignTaskDA taskDA = new AssignTaskDA();
            string assignToId = empId + "";
            DateTime fromDate = DateTime.Now;
            DateTime toDate = fromDate.AddDays(30);
            int id = 0;

            taskList = taskDA.GetTaskByEmployeeId(empId);

            string subContent = string.Empty;
            HMUtility hmUtility = new HMUtility();
            for (int i = 0; i < taskList.Count; i++)
            {
                //if (taskList[i].ImplementStatus == true)
                //{
                //    subContent += @"<div class='TaskTypeDiv col-md-12'> <s>";
                //    subContent += "<div class='form-group'>";
                //    subContent += "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/task.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Task\">";
                //    subContent += "<h5>&nbsp;<a href='javascript:void();'  onclick= 'PerformProjectDetails(" + taskList[i].Id + ",\"" + taskList[i].TaskName + "\",\"" + taskList[i].TaskFor + "\",\"" + taskList[i].TaskType + "\"," + empId + ")' >" + taskList[i].TaskName + "</a></h5></div>";
                //    if (taskList[i].EstimatedDoneDate != null)
                //    {
                //        subContent += "<div class='form-group'>";
                //        subContent += "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/CalenderIcon.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Calender\">";
                //        subContent += "<b>&nbsp; Estimated Finish Date : " + CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(taskList[i].EstimatedDoneDate)) + "</b>  </div>";
                //    }
                //    subContent += "<div class='form-group'> <b>Type :</b> " + taskList[i].TaskType + "   </div>";
                //    subContent += "<div class='form-group'>";
                //    subContent += "<b>Task Date : " + hmUtility.GetStringFromDateTime(taskList[i].TaskDate) + "</b>  </div>";
                //    subContent += " </s></div>";
                //}
                //else

                if (taskList[i].ImplementStatus != true)
                {
                    subContent += @"<div class='TaskTypeDiv col-md-12'>";
                    subContent += "<div class='form-group'>";
                    subContent += "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/task.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Task\">";
                    subContent += "<h5>&nbsp;<a href='javascript:void();'  onclick= 'PerformProjectDetails(" + taskList[i].Id + ",\"" + taskList[i].TaskName + "\",\"" + taskList[i].TaskFor + "\",\"" + taskList[i].TaskType + "\"," + empId + ")' >" + taskList[i].TaskName + "</a></h5></div>";
                    if (taskList[i].EstimatedDoneDate != null)
                    {
                        subContent += "<div class='form-group'>";
                        subContent += "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/CalenderIcon.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Calender\">";
                        subContent += "<b>&nbsp; Estimated Finish Date : " + CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(taskList[i].EstimatedDoneDate)) + "</b>  </div>";
                    }
                    subContent += "<div class='form-group'> <b>Type :</b> " + taskList[i].TaskType + "   </div>";
                    subContent += "<div class='form-group'>";
                    subContent += "<b>Task Date : " + hmUtility.GetStringFromDateTime(taskList[i].TaskDate) + "</b>  </div>";
                    subContent += "</div>";
                }
            }

            literalTaskTemplete.Text = subContent;
            if (taskList.Count == 0)
            {
                TaskTemplete.Visible = false;
            }
        }
        private void LoadReminder(int empId)
        {
            List<SMTask> taskList = new List<SMTask>();
            AssignTaskDA taskDA = new AssignTaskDA();
            string assignToId = empId + "";
            DateTime fromDate = DateTime.Now;
            DateTime toDate = fromDate.AddDays(30);
            int id = 0;

            taskList = taskDA.GetReminderByEmployeeId(empId);

            string subContent = string.Empty;
            HMUtility hmUtility = new HMUtility();
            for (int i = 0; i < taskList.Count; i++)
            {
                subContent += @"<div class='ReminderTypeDiv col-md-12'>";
                subContent += "<div class='form-group'>";
                subContent += "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/task.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Task\">";
                subContent += "<h5>&nbsp;<a href='javascript:void();'  onclick= 'PerformProjectDetails(" + taskList[i].Id + ",\"" + taskList[i].TaskName + "\"," + empId + ")' >" + taskList[i].TaskName + "</a></h5></div>";
                if (taskList[i].TaskDate != null)
                {
                    subContent += "<div class='form-group'>";
                    subContent += "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/CalenderIcon.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Calender\">";
                    subContent += "<b>&nbsp; Date : " + CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(taskList[i].TaskDate)) + "</b>";
                    subContent += "<b>&nbsp; Time : " + (taskList[i].StartTime).ToString("H:mm") + "</b>  </div>";
                }
                subContent += "<div class='form-group'> <b>Perticipent From Client:</b> " + taskList[i].PerticipentFromClient + "</div>";
                subContent += "<div class='form-group'>";
                subContent += "<b>Task Date : " + hmUtility.GetStringFromDateTime(taskList[i].TaskDate) + "</b>  </div>";
                subContent += "</div>";
            }

            literalReminderTemplete.Text = subContent;
            if (taskList.Count == 0)
            {
                ReminderTemplete.Visible = false;
            }
        }
        private void LoadBasicEmployeInfo(int empId)
        {
            EmployeeBO bo = new EmployeeBO();
            EmployeeDA da = new EmployeeDA();
            bo = da.GetEmployeeInfoById(empId);
            HMUtility hmUtility = new HMUtility();
            if (bo == null)
            {
                lblEmployeeName.Text = string.Empty;
                lblDesignation.Text = string.Empty;
                lblDepartment.Text = string.Empty;
                lblEmployeeType.Text = string.Empty;
                lblJoinDate.Text = string.Empty;
                lblDateOfBirth.Text = string.Empty;
            }
            else
            {
                if (bo.EmpId > 0)
                {
                    if (bo.DisplayName != null)
                    {
                        lblEmployeeName.Text = bo.DisplayName;
                    }
                    else
                    {
                        lblEmployeeName.Text = string.Empty;
                    }

                    if (bo.Designation != null)
                    {
                        lblDesignation.Text = bo.Designation;
                    }
                    else
                    {
                        lblDesignation.Text = string.Empty;
                    }

                    if (bo.Department != null)
                    {
                        lblDepartment.Text = "Department : " + bo.Department;
                    }
                    else
                    {
                        lblDepartment.Text = string.Empty;
                    }

                    if (bo.TypeCategory == "Contractual")
                    {
                        lblEmployeeType.Text = "Type : " + bo.EmpType + ", Contract End Date : " + CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(bo.InitialContractEndDate));
                    }
                    else if (bo.TypeCategory == "Probational")
                    {
                        lblEmployeeType.Text = "Type : " + bo.EmpType + ", Probation Period : " + CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(bo.ProvisionPeriod));
                    }
                    else if (bo.TypeCategory == "Intern")
                    {
                        lblEmployeeType.Text = "Type : " + bo.EmpType + ", Contract End Date : " + CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(bo.InitialContractEndDate));
                    }
                    else
                    {
                        lblEmployeeType.Text = "Type : " + bo.EmpType;
                    }

                    if (bo.JoinDate != null)
                    {
                        string Content = "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/CalenderIcon.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Joining Date\">";
                        Content += "&nbsp;Joining Date : " + CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(bo.JoinDate));
                        lblJoinDate.Text = Content;
                    }
                    else
                    {
                        lblJoinDate.Text = string.Empty;
                    }
                    if (bo.EmpDateOfBirth != null)
                    {
                        string Content = "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/birthdayIcon.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Birth Date\">";
                        Content += "&nbsp;Date Of Birth: " + CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(bo.EmpDateOfBirth));

                        lblDateOfBirth.Text = Content;
                    }
                    else
                    {
                        lblDateOfBirth.Text = string.Empty;
                    }
                }
            }

            DocumentsDA docDA = new DocumentsDA();
            List<DocumentsBO> docBO = new List<DocumentsBO>();
            docBO = docDA.GetLastOneDocumentsInfoByDocCategoryAndOwnerId("Employee Document", empId);
            string subContent = string.Empty;
            subContent = @"<div>";
            if (docBO.Count > 0)
            {
                var Image = docBO[0];
                subContent += "<img class=\"imgCircle\" onClick=\"javascript:return GoToDetaisPage('" + empId + "')\"   src=\"" + Image.Path + Image.Name + "\" title=\"" + bo.DisplayName + "\" style=\" height:180px; width:180px;cursor: pointer; cursor: hand;\"alt=\"Employee\">";
            }
            else
            {
                subContent += "<img class=\"imgCircle\" onClick=\"javascript:return GoToDetaisPage('" + empId + "')\"  src=\"/Payroll/Images/Documents/defaultEmployee.png\" title=\"" + bo.DisplayName + "\" style=\" height:180px; width:180px; cursor: pointer; cursor: hand;\"alt=\"Employee\">";
            }
            subContent += "</div>";
            literalImageTemplete.Text = subContent;

            AppraisalEvaluationDA appDa = new AppraisalEvaluationDA();
            List<BestEmployeeNominationViewBO> employeeLst = new List<BestEmployeeNominationViewBO>();

            employeeLst = appDa.GetEmployeeOfTheYearByEmpId(empId);
            if (employeeLst.Count > 0)
            {
                string Content = string.Empty;
                Content = @"<div ";
                Content += "style =\"text-align:center; vertical-align: middle;\" >";
                Content += "<img   src=\"/Payroll/Images/Documents/employeeOfTheMonth.png\"style=\" width:150px;\"alt=\"Employee\">";
                Content += "</div>";
                literalBestEmployeeImageTemplete.Text = Content;
            }
        }
        private void LoadEmployeeInfo()
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDa.GetEmployeeInfo();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            ddlWorkHandover.DataSource = empList.Where(x => x.EmpId != userInformationBO.EmpId).ToList();
            ddlWorkHandover.DataTextField = "DisplayName";
            ddlWorkHandover.DataValueField = "EmpId";
            ddlWorkHandover.DataBind();
            ListItem FirstItemTaskHandoverTo = new ListItem();
            FirstItemTaskHandoverTo.Value = "0";
            FirstItemTaskHandoverTo.Text = hmUtility.GetDropDownFirstValue();
            ddlWorkHandover.Items.Insert(0, FirstItemTaskHandoverTo);
        }
        private void LoadUpcommingTrainningsByEmployee(int empId)
        {
            List<EmpTrainingBO> lists = new List<EmpTrainingBO>();
            EmpTrainingDA trainingdA = new EmpTrainingDA();
            lists = trainingdA.GetUpcomingProjectsByEmployeeId(empId);
            string subContent = string.Empty;
            HMUtility hmUtility = new HMUtility();
            for (int i = 0; i < lists.Count; i++)
            {
                subContent += @"<div class='TranningTypeDiv col-md-12'>";
                subContent += "<div class='form-group'>";
                subContent += "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/trainning.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Trainning\">";
                subContent += "<h5>&nbsp;" + lists[i].TrainingName + "</h5></div>";
                subContent += "<div class='form-group'>";
                subContent += "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/CalenderIcon.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Calender\">";
                subContent += "<b>&nbsp;" + hmUtility.GetStringFromDateTime(lists[i].StartDate) + " TO " + hmUtility.GetStringFromDateTime(lists[i].EndDate) + "</b>  </div>";
                subContent += "<div class='form-group'> <b>Trainner :</b> " + lists[i].Trainer + "   </div>";
                subContent += "<div class='form-group'> <b>Place :</b> " + lists[i].Location + "  </div>";
                subContent += "</div>";
            }

            literalTranningTemplete.Text = subContent;
            if (lists.Count == 0)
            {
                TranningTemplete.Visible = false;
            }
        }
        private void LoadUpcommingBirthdays()
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetUpcomingEmployeeBirthday();
            string subContent = string.Empty;
            HMUtility hmUtility = new HMUtility();
            for (int i = 0; i < empList.Count; i++)
            {
                subContent += @"<div class='BirthdayTypeDiv col-md-12'>";
                subContent += "<div class='form-group'>";
                subContent += "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/birthdayIcon.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Birthday\">";
                subContent += "<h5>&nbsp;" + empList[i].DisplayName + "</h5></div>";
                subContent += "<div class='form-group'>";
                subContent += "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/CalenderIcon.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Calender\">";
                subContent += "<b>&nbsp; " + (Convert.ToDateTime(empList[i].EmpDateOfBirth)).ToString("MMMM dd") + "</b>  </div>";
                subContent += "<div class='form-group'> <b>Designation :</b> " + empList[i].Designation + "   </div>";
                subContent += "<div class='form-group'> <b>Department :</b> " + empList[i].Department + "  </div>";
                subContent += "</div>";
            }

            literalCelebrationsBirthdayTemplete.Text = subContent;
            if (empList.Count == 0)
            {
                CelebrationsBirthdayTemplete.Visible = false;
            }
        }
        private void LoadUpcommingWorkAnniversary()
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetUpcomingEmployeeWorkAnniversary();
            string subContent = string.Empty;
            HMUtility hmUtility = new HMUtility();
            for (int i = 0; i < empList.Count; i++)
            {
                subContent += @"<div class='BirthdayTypeDiv col-md-12'>";
                subContent += "<div class='form-group'>";
                subContent += "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/birthdayIcon.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Work Anniversary\">";
                subContent += "<h5>&nbsp;" + empList[i].DisplayName + "</h5></div>";
                subContent += "<div class='form-group'>";
                subContent += "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/CalenderIcon.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Calender\">";
                subContent += "<b>&nbsp; " + empList[i].JoinDateDisplay + " (Anniversary: " + empList[i].WorkAnniversary.ToString() + ")" + "</b>  </div>";
                subContent += "<div class='form-group'> <b>Designation :</b> " + empList[i].Designation + "   </div>";
                subContent += "<div class='form-group'> <b>Department :</b> " + empList[i].Department + "  </div>";
                subContent += "</div>";
            }

            literalCelebrationsWorkAnniversaryTemplete.Text = subContent;
            if (empList.Count == 0)
            {
                CelebrationsWorkAnniversaryTemplete.Visible = false;
            }
        }
        private void LoadEmpProvisionPeriod()
        {
            Boolean IsAdminUser = false;
            List<EmployeeBO> empList = new List<EmployeeBO>();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            // // // ------User Admin Authorization BO Session Information --------------------------------
            #region User Admin Authorization
            if (userInformationBO.UserInfoId == 1)
            {
                IsAdminUser = true;
            }
            else
            {
                List<SecurityUserAdminAuthorizationBO> adminAuthorizationList = new List<SecurityUserAdminAuthorizationBO>();
                adminAuthorizationList = System.Web.HttpContext.Current.Session["UserAdminAuthorizationBOSession"] as List<SecurityUserAdminAuthorizationBO>;
                if (adminAuthorizationList != null)
                {
                    if (adminAuthorizationList.Where(x => x.UserInfoId == userInformationBO.UserInfoId && x.ModuleId == 18).Count() > 0)
                    {
                        IsAdminUser = true;
                    }
                }
            }
            #endregion

            if (IsAdminUser)
            {
                EmployeeDA empDA = new EmployeeDA();
                empList = empDA.GetUpcomingEmployeeProvisionPeriod();
                string subContent = string.Empty;
                HMUtility hmUtility = new HMUtility();
                for (int i = 0; i < empList.Count; i++)
                {
                    subContent += @"<div class='BirthdayTypeDiv col-md-12'>";
                    subContent += "<div class='form-group'>";
                    subContent += "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/task.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Birthday\">";
                    subContent += "<h5>&nbsp;" + empList[i].DisplayName + "</h5></div>";
                    subContent += "<div class='form-group'>";
                    subContent += "</div>";

                    subContent += "<div class='form-group'> <b>Join Date :</b> " + (Convert.ToDateTime(empList[i].JoinDate)).ToString("MMMM dd, yyyy") + "   </div>";
                    subContent += "<div class='form-group'> <b>Probation Period :</b> " + (Convert.ToDateTime(empList[i].ProvisionPeriod)).ToString("MMMM dd, yyyy") + "  </div>";

                    subContent += "<div class='form-group'> <b>Designation :</b> " + empList[i].Designation + "   </div>";
                    subContent += "<div class='form-group'> <b>Department :</b> " + empList[i].Department + "  </div>";
                    subContent += "</div>";
                }

                literalPeriodTempleteTemplete.Text = subContent;
            }

            if (empList.Count == 0)
            {
                ProvisionPeriodTemplete.Visible = false;
            }
        }
        private void LoadTodaysSummary(int empId)
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmployeeDashboardAttendanceSummary(DateTime.Now);
            string subContent = string.Empty;
            HMUtility hmUtility = new HMUtility();

            for (int i = 0; i < empList.Count; i++)
            {
                subContent += "<div class='form-group'> <b>" + empList[i].TransactionCount.ToString("000") + " :</b> <a href='javascript:void();'  onclick='ShowSummaryReport(" + empId + ",\"" + empList[i].TransactionName + "\")' >" + empList[i].TransactionName + "</a> </div>";
            }

            literalSummaryTemplete.Text = subContent;
            if (empList.Count == 0)
            {
                literalSummaryTemplete.Visible = false;
            }
        }
        private void LoadCompanyEmployeeCount()
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetCompanyEmployeeCount();
            string subContent = string.Empty;
            HMUtility hmUtility = new HMUtility();
            for (int i = 0; i < empList.Count; i++)
            {
                subContent += "<div class='form-group'> <b>" + empList[i].EmployeeCount.ToString("000") + " :</b> &nbsp;<b><a href='javascript:void();'  onclick='PerformEmployeeCompanyDetails(" + empList[i].CompanyId + ",\"" + empList[i].CompanyName + "\")' >" + empList[i].CompanyName + "</a></b> </div>";
            }

            literalCompanyWiseEmployeeCountTemplete.Text = subContent;
            if (empList.Count == 0)
            {
                CompanyWiseEmployeeCountTemplete.Visible = false;
            }
        }
        private void LoadHolidayInformation()
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetHolidayInformation(DateTime.Now.Date, DateTime.Now.Date.AddDays(30));
            string subContent = string.Empty;
            HMUtility hmUtility = new HMUtility();
            for (int i = 0; i < empList.Count; i++)
            {
                if (empList[i].StartDate.Date == empList[i].EndDate.Date)
                {
                    subContent += "<div style='color: red' class='form-group'> => <b>" + (Convert.ToDateTime(empList[i].StartDate)).ToString("MMMM dd, yyyy") + " :</b> " + empList[i].HolidayName + "   </div>";
                }
                else
                {
                    subContent += "<div style='color: red' class='form-group'> => <b>" + (Convert.ToDateTime(empList[i].StartDate)).ToString("MMMM dd, yyyy") + " to " + (Convert.ToDateTime(empList[i].EndDate)).ToString("MMMM dd, yyyy") + " :</b> " + empList[i].HolidayName + "   </div>";
                }
            }

            literalHolidayInformationTemplete.Text = subContent;
            if (empList.Count == 0)
            {
                HolidayInformationTemplete.Visible = false;
            }
        }
        private void GetLeaveTakenNBalanceByEmployee(int empId)
        {
            List<LeaveTakenNBalanceBO> lists = new List<LeaveTakenNBalanceBO>();

            EmployeeYearlyLeaveDA leaveDa = new EmployeeYearlyLeaveDA();
            lists = leaveDa.GetLeaveTakenNBalanceByEmployee(empId, DateTime.Now);

            string subContent = string.Empty;

            for (int i = 0; i < lists.Count; i++)
            {
                if ((lists[i].TotalTakenLeave + lists[i].RemainingLeave) != 0)
                {
                    subContent += @"<div class='LeaveTypeDiv'>";
                    subContent += "<div class='form-group middle'><h4>" + lists[i].LeaveTypeName + "</h4></div>";
                    subContent += "<div class='form-group middle'> Leave Taken (" + lists[i].TotalTakenLeave + ") </div>";
                    subContent += "<div class='form-group middle'> Remaining Leave (" + lists[i].RemainingLeave + ") </div>";
                    subContent += "</div>";
                }
                else
                {
                    this.ddlLeaveTypeId.Items.Remove(ddlLeaveTypeId.Items.FindByValue(lists[i].LeaveTypeID.ToString()));
                }
            }

            literalLeaveTemplete.Text = subContent;
        }
        private void LoadTodaysLeaveTakenEmployee()
        {
            DocumentsDA docDA = new DocumentsDA();
            DateTime date = DateTime.Now;
            var docList = docDA.GetLeaveEmployeeByDate(date);

            string subContent = string.Empty;

            for (int i = 0; i < docList.Count; i++)
            {
                subContent += @"<div class='LeaveTodayDiv'>";
                if (docList[i].Path == "")
                {
                    subContent += "<img class=\"imgCircle\"  src=\"/Payroll/Images/Documents/defaultEmployee.png\" title=\"" + docList[i].EmployeeName + "\" style=\"height:40px; width:40px;\"alt=\"Employee\">";
                }
                else
                {
                    subContent += "<img class=\"imgCircle\"  src=\"" + docList[i].Path + docList[i].Name + "\" title=\"" + docList[i].EmployeeName + "\" style=\"height:40px; width:40px;\"alt=\"Employee\">";
                }
                subContent += "</div>";
            }

            literalLeaveTodayTemplete.Text = subContent;
            if (docList.Count == 0)
            {
                LeaveTodayTemplete.Visible = false;
            }
        }
        private void LoadTomorrowsLeaveTakenEmployee()
        {
            DocumentsDA docDA = new DocumentsDA();
            var today = DateTime.Now;
            var tomorrow = today.AddDays(1);
            var docList = docDA.GetLeaveEmployeeByDate(tomorrow);

            string subContent = string.Empty;

            for (int i = 0; i < docList.Count; i++)
            {
                subContent += @"<div class='LeaveTodayDiv'>";
                if (docList[i].Path == "")
                {
                    subContent += "<img class=\"imgCircle\"  src=\"/Payroll/Images/Documents/defaultEmployee.png\" title=\"" + docList[i].EmployeeName + "\" style=\"height:40px; width:40px;\"alt=\"Employee\">";
                }
                else
                {
                    subContent += "<img class=\"imgCircle\"  src=\"" + docList[i].Path + docList[i].Name + "\" title=\"" + docList[i].EmployeeName + "\" style=\"height:40px; width:40px;\"alt=\"Employee\">";
                }
                subContent += "</div>";
            }

            literalLeaveTomorrowTemplete.Text = subContent;
            if (docList.Count == 0)
            {
                LeaveTomorrowTemplete.Visible = false;
            }
        }
        private void LoadLeaveType()
        {
            LeaveTypeDA entityDA = new LeaveTypeDA();
            List<LeaveTypeBO> activeLeaveTypeInfoList = new List<LeaveTypeBO>();

            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            if (userInformationBO.EmpId > 0)
            {
                EmployeeDA empDA = new EmployeeDA();
                EmployeeBO empBO = new EmployeeBO();
                empBO = empDA.GetEmployeeInfoById(userInformationBO.EmpId);
                if (empBO != null)
                {
                    if (empBO.EmpId > 0)
                    {
                        if (empBO.Gender == "Male")
                        {
                            activeLeaveTypeInfoList = entityDA.GetActiveLeaveTypeInfo().Where(x => x.LeaveTypeId != 4).ToList();
                        }
                        else if (empBO.Gender == "Female")
                        {
                            activeLeaveTypeInfoList = entityDA.GetActiveLeaveTypeInfo().Where(x => x.LeaveTypeId != 5).ToList();
                        }
                        else
                        {
                            activeLeaveTypeInfoList = entityDA.GetActiveLeaveTypeInfo();
                        }
                    }
                }
            }

            ddlLeaveTypeId.DataSource = activeLeaveTypeInfoList;
            ddlLeaveTypeId.DataTextField = "TypeName";
            ddlLeaveTypeId.DataValueField = "LeaveTypeId";
            ddlLeaveTypeId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlLeaveTypeId.Items.Insert(0, item);
        }
        private void LoadNotice(int empId)
        {
            List<CustomNoticeBO> noticeList = new List<CustomNoticeBO>();
            CustomNoticeDA DA = new CustomNoticeDA();
            noticeList = DA.GetCustomNoticeByEmpId(empId);
            string subContent = string.Empty;
            HMUtility hmUtility = new HMUtility();
            for (int i = 0; i < noticeList.Count; i++)
            {
                List<DocumentsBO> notice = new List<DocumentsBO>();
                notice = new DocumentsDA().GetDocumentsByUserTypeAndUserId("CustomNoticeDocument", Convert.ToInt32(noticeList[i].Id));
                if (notice != null)
                {
                    if (notice.Count > 0)
                    {
                        subContent += @"<div class='TaskTypeDiv col-md-12'>";
                        subContent += "<div class='form-group'>";
                        subContent += "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/task.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Task\">";
                        subContent += "<div class='form-group'> &nbsp;<b><a href='javascript:void();' style='color: red' onclick='LoadCustomNotice(\"" + notice[0].Path + notice[0].Name + "\",\"" + noticeList[i].NoticeName + "\")' >" + noticeList[i].NoticeName + "</a></b> </div>";
                        subContent += "<b>&nbsp; Notice Date : " + CommonHelper.DateTimeClientFormatWiseConversionForDisplay(Convert.ToDateTime(noticeList[i].CreatedDate)) + "</b>  </div>";
                        subContent += "</div>";
                    }
                }
            }
            literalNoticeTemplete.Text = subContent;
            if (noticeList.Count == 0)
            {
                NoticeTemplete.Visible = false;
            }
        }
        private void LoadFixedAsset(int empId)
        {
            List<PMProductOutViewForDashBoardBO> assetList = new List<PMProductOutViewForDashBoardBO>();
            PMProductOutDA DA = new PMProductOutDA();
            assetList = DA.GetProductOutForEmployee(empId);
            string subContent = string.Empty;
            HMUtility hmUtility = new HMUtility();

            subContent += @"<div class='TaskTypeDiv col-md-12'>";
            for (int i = 0; i < assetList.Count; i++)
            {
                subContent += "<div class='form-group'>";
                subContent += "<img class=\"imgCircle\" src =\"/Payroll/Images/Documents/fixedAsset.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Task\">";
                if (assetList[i].ProductType == "Non Serial Product")
                {
                    subContent += "<b>&nbsp;" + assetList[i].Name + "</b>" + "[Quantity: " + assetList[i].Quantity + " " + assetList[i].UnitHead + "]";
                }
                else
                {
                    subContent += "<b>&nbsp;" + assetList[i].Name + "</b>" + " [Quantity: " + assetList[i].Quantity + " " + assetList[i].UnitHead + " (" + assetList[i].SerialNumber + ")]";
                }

                subContent += "</div>";
            }
            subContent += "</div>";
            literalFixedAssetTemplete.Text = subContent;
            if (assetList.Count == 0)
            {
                FixedAssetTemplete.Visible = false;
            }
        }
        private void LoadAssignedDocument(int empId)
        {
            List<DMDocumentBO> documentList = new List<DMDocumentBO>();
            DocumentsForDocManagementDA DA = new DocumentsForDocManagementDA();
            documentList = DA.GetAssignedDocumentByEmpId(empId);
            string subContent = string.Empty;
            HMUtility hmUtility = new HMUtility();
            subContent += @"<div class='TaskTypeDiv col-md-12'>";
            for (int i = 0; i < documentList.Count; i++)
            {
                subContent += "<div class='form-group'>";
                subContent += "<img class=\"imgCircle\" src =\"/Images/document.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Task\">";
                subContent += "<div><h5>&nbsp;<a href='javascript:void();'  onclick= 'ShowDocument(" + documentList[i].Id + ")' >" + documentList[i].DocumentName + "</a></h5></div>";
                subContent += "</div>";
            }
            subContent += "</div>";
            literalAssignedDocTemplete.Text = subContent;
            if (documentList.Count == 0)
            {
                AssignedDocTemplete.Visible = false;
            }
        }
        private void LoadLetter(int empId)
        {
            List<TemplateEmailBO> Template = new List<TemplateEmailBO>();
            TemplateInfoDA DA = new TemplateInfoDA();
            Template = DA.GetTemplateInfoForEmployeeById(empId);
            string subContent = string.Empty;
            HMUtility hmUtility = new HMUtility();
            subContent += @"<div class='TaskTypeDiv col-md-12'>";
            for (int i = 0; i < Template.Count; i++)
            {
                subContent += "<div class='form-group'>";
                subContent += "<img class=\"imgCircle\" src =\"/Images/document.png\"  style=\"height:20px; width:20px; float:left\"alt=\"Task\">";
                subContent += "<div><h5>&nbsp;<a href='javascript:void();'  onclick= 'ShowLetter(" + Template[i].Id + ")' >" + Template[i].Name + "</a></h5></div>";
                subContent += "</div>";
            }
            subContent += "</div>";
            literalLetterTemplete.Text = subContent;
            if (Template.Count == 0)
            {
                LetterTemplete.Visible = false;
            }
        }

        [WebMethod]
        public static ReturnInfo SaveLeaveInformation(LeaveInformationBO leaveInformationBO)
        {
            ReturnInfo rtninf = new ReturnInfo();
            rtninf.IsSuccess = false;
            HMUtility hmUtility = new HMUtility();
            try
            {
                LeaveInformationDA leaveInformationDA = new LeaveInformationDA();

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                if (leaveInformationBO.LeaveId <= 0)//save
                {
                    int tmpUserInfoId = 0;

                    leaveInformationBO.LeaveStatus = HMConstants.ApprovalStatus.Pending.ToString();
                    leaveInformationBO.EmpId = userInformationBO.EmpId;
                    leaveInformationBO.CreatedBy = userInformationBO.UserInfoId;

                    Boolean status = leaveInformationDA.SaveEmpLeaveInformation(leaveInformationBO, out tmpUserInfoId);

                    if (status)
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                        rtninf.IsSuccess = true;
                        bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                           EntityTypeEnum.EntityType.EmpLeaveInfo.ToString(), tmpUserInfoId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmpLeaveInfo));
                    }
                    else
                    {
                        rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                        rtninf.IsSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rtninf;
        }
        [WebMethod]
        public static DMDocumentViewBO LoadDocument(long id)
        {
            DMDocumentViewBO doc = new DMDocumentViewBO();
            DocumentsForDocManagementDA DA = new DocumentsForDocManagementDA();
            doc.DocumentsForDocList = new DocumentsForDocManagementDA().GetDocumentsByUserTypeAndUserId("DocumentsDoc", (int)id);
            doc.DocumentsForDocList = new DocumentsForDocManagementDA().GetDocumentListWithIcon(doc.DocumentsForDocList);
            doc.DocumentBO = DA.GetDocumentById(id);
            return doc;
        }
        [WebMethod]
        public static string ShowLetter(long id, int empId)
        {
            TemplateEmailBO BOs = new TemplateEmailBO();
            TemplateInfoDA DA = new TemplateInfoDA();
            BOs = DA.GetTemplateEmailById(id, "Letter");
            CommonDA commonDA = new CommonDA();
            List<EmployeeBO> employeeBOs = new List<EmployeeBO>();
            EmployeeBO bo = new EmployeeBO();
            EmployeeDA da = new EmployeeDA();
            if (empId > 0)
            {
                bo = da.GetEmployeeInfoById(empId);
            }
            var modifiedBody = string.Empty;
            if (BOs.TemplateId != 0)
            {
                modifiedBody = commonDA.GenerateModifiedBody(BOs.TemplateId, bo);
            }
            else
            {
                modifiedBody = BOs.TemplateBody;
            }

            HMCommonDA hmCommonDA = new HMCommonDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            string fileName = string.Empty, fileNamePrint = string.Empty, path = string.Empty;
            DateTime dateTime = DateTime.Now;
            if (BOs.LastModifiedDate != null)
            {
                fileName = bo.EmpId.ToString() + "_" + BOs.Id.ToString() + String.Format("{0:ddMMMyyyyHHmmssffff}", BOs.LastModifiedDate) + "_" + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";
            }
            else
                fileName = bo.EmpId.ToString() + "_" + BOs.Id.ToString() + String.Format("{0:ddMMMyyyyHHmmssffff}", BOs.CreatedDate) + "_" + (userInformationBO == null ? "0" : userInformationBO.UserInfoId.ToString()) + ".pdf";

            if (File.Exists(HttpContext.Current.Server.MapPath("../HMCommon/CustomTemplate/" + fileName)))
            {
                return "../HMCommon/CustomTemplate/" + fileName;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(modifiedBody);

                //Create our document object
                Document Doc = new Document(PageSize.A4);

                //Create our file stream
                using (FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("../HMCommon/CustomTemplate/" + fileName), FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    //Bind PDF writer to document and stream
                    PdfWriter writer = PdfWriter.GetInstance(Doc, fs);

                    //Open document for writing
                    Doc.Open();

                    //Add a page
                    Doc.NewPage();

                    MemoryStream msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(sb.ToString()));
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, Doc, msHtml, null, Encoding.UTF8, new UnicodeFontFactory());

                    //Close the PDF
                    Doc.Close();
                }
                return "../HMCommon/CustomTemplate/" + fileName;
            }
        }
        public static string GenerateModifiedBody(long templateId, List<EmployeeBO> employeeBOs)
        {
            TemplateInfoDA DA = new TemplateInfoDA();
            TemplateInformationBO templateBo = new TemplateInformationBO();
            templateBo = DA.GetTemplateInformationById(templateId);
            List<TemplateInformationDetailBO> detailsBOs = new List<TemplateInformationDetailBO>();
            detailsBOs = DA.GetTemplateInformationDetail(templateId);
            string modifiedBody = string.Empty;
            List<TemporaryReplaceBO> replaceBOList = new List<TemporaryReplaceBO>();
            var pk = "";
            if (templateBo.TemplateFor == "PayrollEmployee")
            {
                pk = "EmpId";
            }
            else if (templateBo.TemplateFor == "SMContactInformation")
            {
                pk = "Id";
            }
            else if (templateBo.TemplateFor == "HotelGuestCompany")
            {
                pk = "CompanyId";
            }
            else if (templateBo.TemplateFor == "PMSupplier")
            {
                pk = "SupplierId";
            }
            if (detailsBOs.Count > 0)
            {
                foreach (var item in employeeBOs)
                {
                    foreach (var item2 in detailsBOs)
                    {
                        TemporaryReplaceBO replaceBO = new TemporaryReplaceBO();
                        replaceBO.Id = item.EmpId;
                        replaceBO.BodyText = item2.BodyText;
                        replaceBO.ReplacedBy = item2.ReplacedBy;
                        replaceBO.ReplaceByValue = DA.GetReplaceByValue(item.EmpId, item2.ReplacedBy, item2.TableName, pk);
                        replaceBOList.Add(replaceBO);
                    }
                }
                StringBuilder builder = new StringBuilder(templateBo.Body);
                foreach (var item in replaceBOList)
                {
                    if (templateBo.Body.Contains(item.BodyText))
                    {
                        builder.Replace(item.BodyText, item.ReplaceByValue);
                        modifiedBody = builder.ToString();
                    }
                }
            }
            else
            {
                modifiedBody = templateBo.Body;
            }

            return modifiedBody;
        }
        public class UnicodeFontFactory : FontFactoryImp
        {
            private static readonly string FontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
          HttpContext.Current.Server.MapPath("../HMCommon/CustomNoticeFont/ARIALUNI.TTF"));

            private readonly BaseFont _baseFont;

            public UnicodeFontFactory()
            {
                _baseFont = BaseFont.CreateFont(FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            }

            public override iTextSharp.text.Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color, bool cached)
            {
                return new iTextSharp.text.Font(_baseFont, size, style, color);
            }
        }
    }
}