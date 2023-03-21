using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.Payroll;
using Newtonsoft.Json;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmPFOpeningBalance : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadSalaryProcessMonth();
                LoadYearList();
                LoadDepartment();
                CheckObjectPermission();
            }
        }

        private void LoadSalaryProcessMonth()
        {
            EmpSalaryProcessDA entityDA = new EmpSalaryProcessDA();
            this.ddlProcessMonth.DataSource = entityDA.GetSalaryProcessMonth();
            this.ddlProcessMonth.DataTextField = "MonthHead";
            this.ddlProcessMonth.DataValueField = "MonthValue";
            this.ddlProcessMonth.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlProcessMonth.Items.Insert(0, item);
        }
        private void LoadYearList()
        {
            ddlYear.DataSource = hmUtility.GetReportYearList();
            ddlYear.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlYear.Items.Insert(0, item);
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
        protected void EmpDropDown_Change(object sender, EventArgs e)
        {
            LoadPFInformation();
        }
        private void LoadPFInformation()
        {
            int departId = Convert.ToInt32(ddlDepartment.SelectedValue);
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmpForPFOpeningBalance(departId);

            string strTable = "";
            int counter = 0;

            strTable += "<table  id='PFOpeningBalance' class='table table-bordered table-condensed table-responsive' style='width: 100%;'><thead><tr style='color: White; background-color: #44545E; font-weight: bold;'>";
            strTable += "<th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th><th style='display:none'></th>";
            strTable += "<th align='left' scope='col' style='width: 10%;'>Id</th>";
            strTable += "<th align='left' scope='col' style='width: 45%;'>Name</th>";
            strTable += "<th align='center' scope='col' style='width: 15%;'>Emp Contribution</th>";
            strTable += "<th align='center' scope='col' style='width: 15%;'>Cmp Contribution</th>";
            strTable += "<th align='center' scope='col' style='width: 15%;'>Interest Amount</th>";
            strTable += "</tr></thead><tbody>";

            foreach (EmployeeBO emp in empList)
            {
                counter++;
                if (counter % 2 == 0)
                {
                    strTable += "<tr style='background-color:White;'>";
                }
                else
                {
                    strTable += "<tr style='background-color:#E3EAEB;'>";
                }

                strTable += "<td align='left' style=\"display:none;\">" + emp.EmpId + "</td>";
                strTable += "<td align='left' style=\"'width:10%;\">" + emp.EmpCode + "</td>";
                strTable += "<td align='left' style=\"'width:45%;\">" + emp.DisplayName + "</td>";
                strTable += "<td align='left' style='width:15%;'><input type='text' class='form-control' style='width:100px;' value='" + emp.EmpContribution.ToString() + "'/></td>";
                strTable += "<td align='left' style='width: 15%;'><input type='text' class='form-control' style='width:100px;' value='" + emp.CompanyContribution.ToString() + "'/></td>";
                strTable += "<td align='left' style='width: 15%;'><input type='text' class='form-control' style='width:100px;' value='" + emp.ProvidentFundInterest.ToString() + "'/></td>";

                strTable += "</tr>";
            }

            strTable += "</tbody> </table>";
            ltlEmpList.InnerHtml = strTable;
        }
        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool status = false;
                Int16 year = 0; int createdby = 0; DateTime fromDate = DateTime.Now, toDate = DateTime.Now;

                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                List<EmpPFBO> addList = new List<EmpPFBO>();
                addList = JsonConvert.DeserializeObject<List<EmpPFBO>>(hfSaveObj.Value);

                string selectedMonthRange = ddlProcessMonth.SelectedValue.ToString();
                if (ddlProcessMonth.SelectedIndex == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Month.", AlertType.Warning);
                    return;
                }
                else if (ddlYear.SelectedIndex == 0)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.DropDownValidation + "Year.", AlertType.Warning);
                    return;
                }
                else {
                    //fromDate = hmUtility.GetDateTimeFromString(CommonHelper.SalaryDateFrom(selectedMonthRange, ddlYear.SelectedValue), userInformationBO.ServerDateFormat);
                    //toDate = hmUtility.GetDateTimeFromString(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue), userInformationBO.ServerDateFormat);
                    fromDate = Convert.ToDateTime(CommonHelper.SalaryDateFrom(selectedMonthRange, ddlYear.SelectedValue));
                    toDate = Convert.ToDateTime(CommonHelper.SalaryDateTo(selectedMonthRange, ddlYear.SelectedValue));                    
                    createdby = userInformationBO.UserInfoId;
                    year = Convert.ToInt16(ddlYear.SelectedValue);
                }
                for(int i = 0; i< addList.Count; i++)
                {
                    addList[i].PFDateFrom = fromDate;
                    addList[i].PFDateTo = toDate;
                    addList[i].CreatedBy = createdby;
                }               

                EmpPFDA EmpPFDA = new EmpPFDA();

                status = EmpPFDA.SaveEmpPFOpeningBalance(addList, year);

                if (status)
                {
                    CommonHelper.AlertInfo(innboardMessage, AlertMessage.Save, AlertType.Success);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.PFSetting.ToString(), 0,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            "Employee's Provident Fund is Saved");
                    LoadPFInformation();
                }
                else
                {
                    CommonHelper.AlertInfo(innboardMessage, "Employee's Provident Fund is not Saved. Emp Contribution, Cmp Contribution or Interest amount cannot be blank.", AlertType.Warning);
                }

            }
            catch (Exception ex)
            {
                CommonHelper.AlertInfo(innboardMessage, AlertMessage.Error, AlertType.Error);
                
            }

        }
    }
}