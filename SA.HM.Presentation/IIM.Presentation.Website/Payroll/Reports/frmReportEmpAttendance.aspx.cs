using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using System.Web.Services;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportEmpAttendance : BasePage
    {
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadDepartment();
                LoadWorkStation();
                LoadTimeSlab();
                ControlShowHide();                
            }
        }
        protected void btnSrcEmployees_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(txtSearchEmployee.Text))
            {
                CommonHelper.AlertInfo(innboardMessage, "Please Provide Employee ID.", AlertType.Warning);
                return;

            }

            EmployeeBO empBo = new EmployeeBO();
            EmployeeDA employeeDA = new EmployeeDA();
            empBo = employeeDA.GetEmployeeInfoByCode(txtSearchEmployee.Text.Trim());
            if (empBo.EmpId == 0)
            {
                CommonHelper.AlertInfo(innboardMessage, "No Employee Found", AlertType.Warning);
                return;
            }
            hfEmployeeId.Value = empBo.EmpId.ToString();
            txtEmployeeName.Text = empBo.EmployeeName;
        }
        private void ControlShowHide()
        {
            Boolean IsAdminUser = false;
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

            EmployeeInformationDiv.Visible = IsAdminUser;


            // // ----------IsPayrollWorkStationHide
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            HMCommonSetupBO isPayrollWorkStationHideBO = new HMCommonSetupBO();
            isPayrollWorkStationHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollWorkStationHide", "IsPayrollWorkStationHide");
            if (isPayrollWorkStationHideBO != null)
            {
                if (isPayrollWorkStationHideBO.SetupValue == "1")
                {
                    PayrollWorkStationLabelDiv.Visible = false;
                    PayrollWorkStationControlDiv.Visible = false;
                }
            }
        }
        private void LoadWorkStation()
        {
            EmployeeDA workStationDA = new EmployeeDA();
            List<EmpWorkStationBO> entityBOList = new List<EmpWorkStationBO>();
            entityBOList = workStationDA.GetEmployWorkStation();

            ddlWorkStation.DataSource = entityBOList;
            ddlWorkStation.DataTextField = "WorkStationName";
            ddlWorkStation.DataValueField = "WorkStationId";
            ddlWorkStation.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlWorkStation.Items.Insert(0, item);
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            this.ddlDepartmentId.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartmentId.DataTextField = "Name";
            this.ddlDepartmentId.DataValueField = "DepartmentId";
            this.ddlDepartmentId.DataBind();

            ddlDepartmentId.Items.Insert(0, new ListItem { Text = hmUtility.GetDropDownFirstAllValue(), Value = "0" });
        }
        private void LoadTimeSlab()
        {
            TimeSlabHeadDA entityDA = new TimeSlabHeadDA();
            TimeSlabHeadBO entityBO = new TimeSlabHeadBO();
            List<TimeSlabHeadBO> entityBOList = new List<TimeSlabHeadBO>();
            entityBOList = entityDA.GetAllTimeSlabHeadInfo();

            ddlTimeSlab.DataSource = entityBOList;
            ddlTimeSlab.DataTextField = "TimeSlabHead";
            ddlTimeSlab.DataValueField = "TimeSlabId";
            ddlTimeSlab.DataBind();

            ddlTimeSlab.Items.Insert(0, new ListItem { Text = hmUtility.GetDropDownFirstAllValue(), Value = "0" });
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRosterDateFrom.Text))
            {
                isMessageBoxEnable = 1;
                lblMessage.Text = "Please Give Start Date";
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtRosterDateTo.Text))
            {
                isMessageBoxEnable = 1;
                lblMessage.Text = "Please Give End Date";
                return;
            }

            _RoomStatusInfoByDate = 1;
            bool employeeSearchEnableOrDesable = true;
            DateTime rosterDateFrom = DateTime.Now, rosterDateTo = DateTime.Now;
            int departmentId = 0, workStationId = 0, timeslabId = 0, employeeId = 0;

            rosterDateFrom = hmUtility.GetDateTimeFromString(txtRosterDateFrom.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            rosterDateTo = hmUtility.GetDateTimeFromString(txtRosterDateTo.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            
            Boolean IsAdminUser = false;
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
                employeeSearchEnableOrDesable = Convert.ToInt32(ddlEmployee.SelectedValue) == 1 ? true : false;

                if (employeeSearchEnableOrDesable)
                {
                    if (hfEmployeeId.Value == "0")
                    {
                        isMessageBoxEnable = 1;
                        lblMessage.Text = "Please Give an Employee ID";
                        return;
                    }
                }

                employeeId = Convert.ToInt32(hfEmployeeId.Value);
                departmentId = Convert.ToInt32(ddlDepartmentId.SelectedValue);
                workStationId = Convert.ToInt32(ddlWorkStation.SelectedValue);
                timeslabId = Convert.ToInt32(ddlTimeSlab.SelectedValue);
            }
            else
            {
                hfEmployeeId.Value = userInformationBO.EmpId.ToString();
                employeeId = Convert.ToInt32(hfEmployeeId.Value);
            }

            int companyId = 0;
            string glCompanyName = "All";
            int projectId = 0;
            string glProjectName = "All";

            if (hfCompanyId.Value != "0" && hfCompanyId.Value != "")
            {
                companyId = Convert.ToInt32(hfCompanyId.Value);
                glCompanyName = hfCompanyName.Value;
            }

            if (hfProjectId.Value != "0" && hfProjectId.Value != "")
            {
                projectId = Convert.ToInt32(hfProjectId.Value);
                glProjectName = hfProjectName.Value;
            }

            HMCommonDA hmCommonDA = new HMCommonDA();
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;

            var reportPath = "";
            if (ddlReportType.SelectedValue == "-1")
            {
                if(employeeId == 0)
                {
                    if (rosterDateFrom.Date == rosterDateTo.Date)
                    {
                        reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeAttendanceSingleDay.rdlc");
                    }
                    else
                    {
                        reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeAttendance.rdlc");
                    }
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeIndividualAttendance.rdlc");
                }                
            }
            else if (ddlReportType.SelectedValue == "0")
            {
                if(employeeId == 0)
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeAttendanceInOut.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeAttendanceInOutIndividual.rdlc");
                }                
            }            
            else if (ddlReportType.SelectedValue == "1")
            {
                if (employeeId == 0)
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeAttendanceLog.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeAttendanceLogIndividual.rdlc");
                }                
            }
            else if (ddlReportType.SelectedValue == "2")
            {
                if (employeeId == 0)
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmpWithoutClockOutAttendance.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmpWithoutClockOutAttendanceIndividual.rdlc");
                }                
            }
            else if (ddlReportType.SelectedValue == "3")
            {
                if (employeeId == 0)
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmpLateAttendance.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmpLateAttendanceIndividual.rdlc");
                }                
            }
            else if (ddlReportType.SelectedValue == "4")
            {
                if (employeeId == 0)
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeAttendanceLog.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeAttendanceLogIndividual.rdlc");
                }                
            }

            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            //-- Company Logo -------------------------------
            string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;

            List<ReportParameter> paramLogo = new List<ReportParameter>();
            //paramLogo.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + ImageName)));

            string companyName = string.Empty;
            string companyAddress = string.Empty;
            string webAddress = string.Empty;

            if (files[0].CompanyId > 0)
            {
                companyName = files[0].CompanyName;
                companyAddress = files[0].CompanyAddress;

                if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
                {
                    webAddress = files[0].WebAddress;
                }
                else
                {
                    webAddress = files[0].ContactNumber;
                }
            }

            //if (files[0].CompanyId > 0)
            //{
            //    paramLogo.Add(new ReportParameter("CompanyProfile", files[0].CompanyName));
            //    paramLogo.Add(new ReportParameter("CompanyAddress", files[0].CompanyAddress));

            //    if (!string.IsNullOrWhiteSpace(files[0].WebAddress))
            //    {
            //        paramLogo.Add(new ReportParameter("CompanyWeb", files[0].WebAddress));
            //    }
            //    else
            //    {
            //        paramLogo.Add(new ReportParameter("CompanyWeb", files[0].ContactNumber));
            //    }
            //}

            if (companyId > 0)
            {
                GLCompanyBO glCompanyBO = new GLCompanyBO();
                GLCompanyDA glCompanyDA = new GLCompanyDA();
                glCompanyBO = glCompanyDA.GetGLCompanyInfoById(companyId);
                if (glCompanyBO != null)
                {
                    if (glCompanyBO.CompanyId > 0)
                    {
                        companyName = glCompanyBO.Name;
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.CompanyAddress))
                        {
                            companyAddress = glCompanyBO.CompanyAddress;
                        }
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.WebAddress))
                        {
                            webAddress = glCompanyBO.WebAddress;
                        }
                        if (!string.IsNullOrWhiteSpace(glCompanyBO.ImageName))
                        {
                            imageName = glCompanyBO.ImageName;
                        }
                    }
                }
            }

            paramLogo.Add(new ReportParameter("CompanyProfile", companyName));
            paramLogo.Add(new ReportParameter("CompanyAddress", companyAddress));
            paramLogo.Add(new ReportParameter("CompanyWeb", webAddress));
            paramLogo.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            paramLogo.Add(new ReportParameter("PrintDateTime", printDate));
            paramLogo.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));

            EmpAttendanceDA attendanceDA = new EmpAttendanceDA();
            List<EmpAttendanceReportBO> serviceList = new List<EmpAttendanceReportBO>();

            serviceList = attendanceDA.GetEmpAttendanceReport(Convert.ToInt32(ddlReportType.SelectedValue), companyId, projectId, rosterDateFrom, rosterDateTo, employeeId, departmentId, workStationId, timeslabId);

            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], serviceList));

            int totalLateCount = 0;
            int totalApprovedLateCount = 0;
            int totalWorkingDayCount = 0;
            int totalAbsentCount = 0;
            int totalHolidayCount = 0;
            int totalLeaveCount = 0;
            int totalDayOffCount = 0;
            int totalPenaltyCount = 0;

            paramLogo.Add(new ReportParameter("FromDate", rosterDateFrom.Date.ToString("dd-MMM-yyyy")));
            paramLogo.Add(new ReportParameter("ToDate", rosterDateTo.Date.ToString("dd-MMM-yyyy")));
            if (departmentId > 0)
            {
                DepartmentDA departmentDa = new DepartmentDA();
                DepartmentBO departmentBo = departmentDa.GetDepartmentInfoById(departmentId);
                paramLogo.Add(new ReportParameter("DepartmentName", departmentBo.Name));
            }
            else
            {
                paramLogo.Add(new ReportParameter("DepartmentName", "All"));
            }

            if (employeeId > 0)
            {
                EmployeeDA employeeDa = new EmployeeDA();
                EmployeeBO employeeBo = employeeDa.GetEmployeeInfoById(employeeId);
                paramLogo.Add(new ReportParameter("EmployeeName", employeeBo.Name));
            }
            else
            {
                paramLogo.Add(new ReportParameter("EmployeeName", "All"));
            }

            if (ddlReportType.SelectedValue == "-1")
            {
                totalLateCount = (serviceList.Where(x => x.AttendanceStatus == "P").ToList()).Where(x => x.LateTime != "").ToList().Count();
                totalApprovedLateCount = 0;
                totalWorkingDayCount = serviceList.Where(x => x.AttendanceStatus == "P").ToList().Count();
                totalAbsentCount = serviceList.Where(x => x.AttendanceStatus == "A").ToList().Count();
                totalHolidayCount = serviceList.Where(x => x.AttendanceStatus == "H").ToList().Count();
                totalLeaveCount = serviceList.Where(x => x.AttendanceStatus == "L").ToList().Count();
                totalDayOffCount = serviceList.Where(x => x.AttendanceStatus == "D/O").ToList().Count();
                totalPenaltyCount = 0;

                paramLogo.Add(new ReportParameter("ReportName", "Employee Attendance Information"));
                rvTransaction.LocalReport.DisplayName = "Employee Attendance";
            }
            else if (ddlReportType.SelectedValue == "0")
            {
                paramLogo.Add(new ReportParameter("ReportName", "Employee Attendance (In/Out)"));
                rvTransaction.LocalReport.DisplayName = "Employee Attendance (In/Out)";
            }
            else if (ddlReportType.SelectedValue == "1")
            {
                paramLogo.Add(new ReportParameter("ReportName", "Employee Attendance Log"));
                rvTransaction.LocalReport.DisplayName = "Employee Attendance Log";
            }
            else if (ddlReportType.SelectedValue == "2")
            {
                paramLogo.Add(new ReportParameter("ReportName", "Without Clock Out Attendance"));
                rvTransaction.LocalReport.DisplayName = "Without Clock Out Attendance";
            }
            else if (ddlReportType.SelectedValue == "3")
            {
                paramLogo.Add(new ReportParameter("ReportName", "Late Attendance"));
                rvTransaction.LocalReport.DisplayName = "Late Attendance";
            }
            else if (ddlReportType.SelectedValue == "4")
            {
                paramLogo.Add(new ReportParameter("ReportName", "Employee Overtime"));
                rvTransaction.LocalReport.DisplayName = "Employee Overtime";
            }

            paramLogo.Add(new ReportParameter("EmployeeId", employeeId.ToString()));
            paramLogo.Add(new ReportParameter("TotalLateCount", totalLateCount.ToString()));
            paramLogo.Add(new ReportParameter("TotalApprovedLateCount", totalApprovedLateCount.ToString()));
            paramLogo.Add(new ReportParameter("TotalWorkingDayCount", totalWorkingDayCount.ToString()));
            paramLogo.Add(new ReportParameter("TotalAbsentCount", totalAbsentCount.ToString()));
            paramLogo.Add(new ReportParameter("TotalHolidayCount", totalHolidayCount.ToString()));
            paramLogo.Add(new ReportParameter("TotalLeaveCount", totalLeaveCount.ToString()));
            paramLogo.Add(new ReportParameter("TotalDayOffCount", totalDayOffCount.ToString()));
            paramLogo.Add(new ReportParameter("TotalPenaltyCount", totalPenaltyCount.ToString()));

            rvTransaction.LocalReport.SetParameters(paramLogo);
            rvTransaction.LocalReport.Refresh();
            frmPrint.Attributes["src"] = "";
        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();
            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());
            frmPrint.Attributes["src"] = reportSource;
        }        
    }
}