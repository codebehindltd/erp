using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.Payroll;
using Microsoft.Reporting.WebForms;
using System.IO;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using Mamun.Presentation.Website.Common;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.GeneralLedger;

namespace HotelManagement.Presentation.Website.Payroll.Reports
{
    public partial class frmReportEmployeeList : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        protected int isMessageBoxEnable = -1;
        protected int _RoomStatusInfoByDate = -1;
        protected int _reportType = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployeeType();
                LoadEmployeeStatus();
                LoadCommonDropDownHiddenField();
                LoadTodaysProbationPeriodEndEmployee();
                LoadDepartment();
                LoadDesignation();
                LoadBloodGroup();
                LoadWorkStation();
            }
            //ListItem listItem = ddlReportType.Items.FindByValue(_reportType.ToString());
            //listItem.Selected = true;
        }
        private void LoadEmployeeType()
        {
            EmpTypeDA entityDA = new EmpTypeDA();
            List<EmpTypeBO> boList = new List<EmpTypeBO>();
            boList = entityDA.GetEmpTypeInfo();

            ddlEmployeeTypeId.DataSource = boList;
            ddlEmployeeTypeId.DataTextField = "Name";
            ddlEmployeeTypeId.DataValueField = "TypeId";
            ddlEmployeeTypeId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEmployeeTypeId.Items.Insert(0, item);
        }
        private void LoadEmployeeStatus()
        {
            EmployeeDA _employeeDA = new EmployeeDA();
            List<EmployeeStatusBO> entityListBO = new List<EmployeeStatusBO>();


            entityListBO = _employeeDA.GetEmployeeStatus();

            string grid = GetHTMLEmployeeStatusGridView(entityListBO);
            ltEmployeeStatus.Text = grid;
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
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlDepartment.Items.Insert(0, item);
        }

        private void LoadDesignation()
        {
            DesignationDA designationDA = new DesignationDA();
            ddlDesignation.DataSource = designationDA.GetDesignationInfo();
            ddlDesignation.DataTextField = "Name";
            ddlDesignation.DataValueField = "DesignationId";
            ddlDesignation.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlDesignation.Items.Insert(0, item);
        }

        private void LoadBloodGroup()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("BloodGroup");

            ddlBloodGroup.DataSource = fields;
            ddlBloodGroup.DataTextField = "FieldValue";
            ddlBloodGroup.DataValueField = "FieldValue";
            ddlBloodGroup.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlBloodGroup.Items.Insert(0, item);
        }

        private void LoadWorkStation()
        {
            EmpWorkStationDA workStationDA = new EmpWorkStationDA();
            ddlWorkStation.DataSource = workStationDA.GetAllWorkStation();
            ddlWorkStation.DataTextField = "WorkStationName";
            ddlWorkStation.DataValueField = "WorkStationId";
            ddlWorkStation.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlWorkStation.Items.Insert(0, item);
        }

        public static string GetHTMLEmployeeStatusGridView(List<EmployeeStatusBO> List)
        {
            string strTable = "";
            strTable += "<table class='table table-bordered table-condensed table-responsive' id='TableWiseItemInformation'><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='text-align:center' scope='col'><input type='checkbox'  id='checkAll' name='checkAll' value='checkAll' ></th><th align='left' scope='col'>Employee Status</th></tr>";
            //strTable += "<tr> <td colspan='2'>";
            //strTable += "<div style=\"height: 100%; overflow-y: scroll; text-align: left;\">";
            //strTable += "<table cellspacing='0' cellpadding='4' width='100%'>";
            int counter = 0;
            foreach (EmployeeStatusBO dr in List)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    // It's even
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }
                else
                {
                    // It's odd
                    strTable += "<tr style='background-color:White;'>";
                }

                strTable += "<td align='center'>";
                strTable += "&nbsp;<input type='checkbox'  id='" + dr.EmployeeStatusId + "' name='" + dr.EmployeeStatus + "' value='" + dr.EmployeeStatusId + "' >";
                strTable += "</td><td align='left'>" + dr.EmployeeStatus + "</td></tr>";
            }

            strTable += "</td> </tr> </table>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available !</td></tr>";
            }

            strTable += "<div style='margin-top:12px;'>";
            strTable += "     <button type='button' onClick='javascript:return GetCheckedStatus()' id='btnAddStatusId' class='btn btn-primary'> OK</button>";
            strTable += "     <button type='button' onclick='javascript:return CloseEmployeeStatusDialog()' id='btnCancelEmployeeStatusId' class='btn btn-primary'> Cancel</button>";
            strTable += "</div>";
            return strTable;
        }
        private void LoadEmployee()
        {
            EmployeeDA employeeDA = new EmployeeDA();
            ddlEmployeedId.DataSource = employeeDA.GetEmployeeInfo();
            ddlEmployeedId.DataTextField = "DisplayName";
            ddlEmployeedId.DataValueField = "EmpId";
            ddlEmployeedId.DataBind();

            ListItem itemEmployee = new ListItem();
            itemEmployee.Value = "0";
            itemEmployee.Text = "---All---";
            ddlEmployeedId.Items.Insert(0, itemEmployee);
        }
        private void LoadTodaysProbationPeriodEndEmployee()
        {
            string todaysdate = DateTime.Now.ToShortDateString();

            List<EmployeeBO> empList = new List<EmployeeBO>();
            EmployeeDA employeeDA = new EmployeeDA();

            empList = employeeDA.GetEmployeeInfo();
            empList = empList.Where(a => a.ShowProvisionPeriod == todaysdate).ToList();

            if (empList != null)
            {
                if (empList.Count > 0)
                {
                    string empName = string.Empty;
                    foreach (EmployeeBO empBO in empList)
                    {
                        if (empName == string.Empty)
                        {
                            empName = empBO.DisplayName;
                        }
                        else
                        {
                            empName += ", " + empBO.DisplayName;
                        }
                    }

                    UserInformationBO userInformationBO = new UserInformationBO();
                    userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                    bool IsMessageSendAllGroupUser = false;

                    CommonMessageDA messageDa = new CommonMessageDA();
                    CommonMessageBO message = new CommonMessageBO();
                    CommonMessageDetailsBO detailBO = new CommonMessageDetailsBO();
                    List<CommonMessageDetailsBO> messageDetails = new List<CommonMessageDetailsBO>();

                    message.Subjects = "Probation Period End";
                    message.MessageBody = empName + ". This employees probation period will over today.";
                    message.MessageFrom = userInformationBO.UserInfoId;
                    message.MessageFromUserId = userInformationBO.UserId;
                    message.MessageDate = DateTime.Now;
                    message.Importance = "Normal";

                    detailBO.MessageTo = userInformationBO.UserInfoId;
                    detailBO.UserId = userInformationBO.UserId;
                    messageDetails.Add(detailBO);

                    bool status = messageDa.SaveMessage(message, messageDetails, IsMessageSendAllGroupUser);
                    if (status)
                    {
                        (Master as HMReport).MessageCount();
                    }
                }
            }
        }
        protected void btnGenarate_Click(object sender, EventArgs e)
        {
            HMCommonDA hmCommonDA = new HMCommonDA();
            EmployeeDA empDA = new EmployeeDA();

            DateTime? fromDate = null, toDate = null;

            if (!string.IsNullOrEmpty(txtFromDate.Text))
            {
                fromDate = hmUtility.GetDateTimeFromString(txtFromDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                fromDate = null;
            }
            if (!string.IsNullOrEmpty(txtToDate.Text))
            {
                toDate = hmUtility.GetDateTimeFromString(txtToDate.Text, hmUtility.GetCurrentApplicationUserInfo().ServerDateFormat);
            }
            else
            {
                toDate = null;
            }
            int EmployeeType = Convert.ToInt32(ddlEmployeeTypeId.SelectedValue);
            string reportType = ddlReportType.SelectedValue;
            string _selectedStatus = hfEmplopoyeeStatusId.Value;
            int companyId = Convert.ToInt32(hfGLCompanyId.Value);
            //string companyName = hfGLCompanyName.Value.Trim();
            if (string.IsNullOrWhiteSpace(hfGLProjectId.Value))
            {
                hfGLProjectId.Value = "0";
            }
            int projectId = Convert.ToInt32(hfGLProjectId.Value);
            string projectName = hfGLProjectName.Value;
            int departmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
            int designationId = Convert.ToInt32(ddlDesignation.SelectedValue);
            string bloodGroup = ddlBloodGroup.SelectedItem.Value;
            int workStationId = Convert.ToInt32(ddlWorkStation.SelectedValue);
            List<EmployeeStatusBO> _selectedEmployee = empDA.GetEmployeeStatusListByIdList(_selectedStatus);
            string _selectedEmployeeStatus = "";
            foreach (var item in _selectedEmployee)
            {
                if (_selectedEmployeeStatus != "")
                {
                    _selectedEmployeeStatus += "," + item.EmployeeStatus.ToString();
                }
                else
                {
                    _selectedEmployeeStatus += item.EmployeeStatus.ToString();
                }
            }
            rvTransaction.LocalReport.DataSources.Clear();
            rvTransaction.ProcessingMode = ProcessingMode.Local;
            var reportPath = "";

            //if ((ddlReportType.SelectedValue == "0" || ddlReportType.SelectedValue == "1") && ddlReportFormat.SelectedValue == "Format2")
            if (reportType == "0")
            {
                if (ddlReportFormat.SelectedValue == "Format2")
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeInfoList.rdlc");
                }
                else
                {
                    reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/RptEmployeeList.rdlc");
                }
            }
            else
            {
                reportPath = Server.MapPath(@"~/Payroll/Reports/Rdlc/EmployeeTypeWiseList.rdlc");
            }


            if (!File.Exists(reportPath))
                return;

            rvTransaction.LocalReport.ReportPath = reportPath;

            CompanyDA companyDA = new CompanyDA();
            List<CompanyBO> files = companyDA.GetCompanyInfo();

            List<ReportParameter> paramReport = new List<ReportParameter>();

            string companyName = string.Empty;
            string companyAddress = string.Empty;
            string webAddress = string.Empty;
            string imageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");

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

            //int glCompanyId = Convert.ToInt32(ddlGLCompany.SelectedValue);
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

            DateTime currentDate = DateTime.Now;
            string printDate = hmUtility.GetDateTimeStringFromDateTime(currentDate);
            string footerPoweredByInfo = string.Empty;
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            footerPoweredByInfo = userInformationBO.FooterPoweredByInfo;

            //_RoomStatusInfoByDate = 1;
            string ImageName = hmCommonDA.GetCustomFieldValueByFieldName("paramHeaderLeftImagePath");
            rvTransaction.LocalReport.EnableExternalImages = true;


            paramReport.Add(new ReportParameter("CompanyProfile", companyName));
            paramReport.Add(new ReportParameter("CompanyAddress", companyAddress));
            paramReport.Add(new ReportParameter("CompanyWeb", webAddress));
            paramReport.Add(new ReportParameter("Path", Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "" + @"/Images/" + imageName)));
            paramReport.Add(new ReportParameter("PrintDateTime", printDate));
            paramReport.Add(new ReportParameter("FooterPoweredByInfo", footerPoweredByInfo));
            paramReport.Add(new ReportParameter("ReportName", _selectedEmployeeStatus));

            if (ddlReportFormat.SelectedValue == "Format1")
            {
                paramReport.Add(new ReportParameter("Company", companyName));
                paramReport.Add(new ReportParameter("Project", "All"));
                paramReport.Add(new ReportParameter("Department", ddlDepartment.SelectedItem.Text));
                paramReport.Add(new ReportParameter("Designation", ddlDesignation.SelectedItem.Text));
                paramReport.Add(new ReportParameter("BloodGroup", ddlBloodGroup.SelectedItem.Text));
                paramReport.Add(new ReportParameter("WorkStation", ddlWorkStation.SelectedItem.Text));
            }
            rvTransaction.LocalReport.SetParameters(paramReport);
            

            List<EmpListForReportViewBO> empList = new List<EmpListForReportViewBO>();
            if (reportType == "0")
            {
                if (ddlReportFormat.SelectedValue == "Format2")
                {
                    empList = empDA.GetSelectedEmployeeList(Convert.ToString(_selectedStatus));
                }
                else
                {
                    empList = empDA.GetSelectedEmployeeInfoListForReport(companyId, projectId, departmentId, designationId, bloodGroup, workStationId, Convert.ToString(_selectedStatus));
                }
            }
            else if (reportType == "2")
            {
                empList = empDA.GetEmployeeTypeWiseListForReport(0, fromDate, toDate, Convert.ToString(_selectedStatus));
                if (ddlDepartment.SelectedValue != "0")
                {
                    empList = empList.Where(p => p.DepartmentName == ddlDepartment.SelectedItem.Text).ToList();
                }
            }
            else
            {
                empList = empDA.GetEmployeeTypeWiseListForReport(EmployeeType, fromDate, toDate, Convert.ToString(_selectedStatus));
            }
            
            var reportDataset = rvTransaction.LocalReport.GetDataSourceNames();
            rvTransaction.LocalReport.DataSources.Add(new ReportDataSource(reportDataset[0], empList));

            rvTransaction.LocalReport.DisplayName = _selectedEmployeeStatus + " Employee List";
            rvTransaction.LocalReport.Refresh();

            frmPrint.Attributes["src"] = "";
            hfReportType.Value = ddlReportType.SelectedValue.ToString();

            //_reportType = Convert.ToInt32(ddlReportType.SelectedValue.ToString());

        }
        protected void btnPrintReportFromClient_Click(object sender, EventArgs e)
        {
            ReportPrinting print = new ReportPrinting();

            LocalReport rpt = rvTransaction.LocalReport;
            var reportSource = print.PrintReport(rpt, HMConstants.PrintPageType.Landscape.ToString());

            frmPrint.Attributes["src"] = reportSource;
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstAllValue();
        }
        [WebMethod(EnableSession = true)]
        public static List<EmployeeBO> LoadEmployee(string activeStat)
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmployeeInfoByStatus(activeStat);

            return empList;
        }
        [WebMethod(EnableSession = true)]
        public static EmpTypeBO GetEmpTypeInfoById(string transactionId)
        {
            EmpTypeBO empTypeBO = new EmpTypeBO();
            if (!string.IsNullOrWhiteSpace(transactionId))
            {
                EmpTypeDA empTypeDA = new EmpTypeDA();
                empTypeBO = empTypeDA.GetEmpTypeInfoById(Convert.ToInt32(transactionId));
            }

            return empTypeBO;
        }
    }
}