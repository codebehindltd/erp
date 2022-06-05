using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.HMCommon;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using System.Web.Services;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmServiceChargeConfiguration : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadDepartment();
                LoadEmployeeType();
                SaveButton.Visible = false;
                CheckObjectPermission();
            }
        }

        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            ddlDepartmentId.DataSource = entityDA.GetDepartmentInfo();
            ddlDepartmentId.DataTextField = "Name";
            ddlDepartmentId.DataValueField = "DepartmentId";
            ddlDepartmentId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            ddlDepartmentId.Items.Insert(0, item);
        }
        private void LoadEmployeeType()
        {
            EmpTypeDA entityDA = new EmpTypeDA();
            this.ddlEmpCategoryId.DataSource = entityDA.GetEmpTypeInfoByServiceChargeApplicableFlag();
            this.ddlEmpCategoryId.DataTextField = "Name";
            this.ddlEmpCategoryId.DataValueField = "TypeId";
            this.ddlEmpCategoryId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstAllValue();
            this.ddlEmpCategoryId.Items.Insert(0, item);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SaveButton.Visible = true;

            string empGrid = string.Empty, alternateColor = string.Empty;
            short rowCounter = 0;
            int departmentId = 0, empTypeId = 0;
            ServiceChargeDA serviceChargeDA = new ServiceChargeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            List<ServiceChargeConfigurationDetailsBO> serviceChargeDetailsList = new List<ServiceChargeConfigurationDetailsBO>();

            if (ddlDepartmentId.SelectedIndex != 0)
            {
                departmentId = Convert.ToInt32(ddlDepartmentId.SelectedValue);
            }
            if (ddlEmpCategoryId.SelectedIndex != 0)
            {
                empTypeId = Convert.ToInt32(ddlEmpCategoryId.SelectedValue);
            }

            empList = serviceChargeDA.GetEmployeeForServiceChargeConfig(empTypeId, departmentId);
            serviceChargeDetailsList = serviceChargeDA.GetServiceChargeDetails(departmentId, empTypeId);
            if (serviceChargeDetailsList.Count > 0)
            {
                hfServiceId.Value = serviceChargeDetailsList[0].ServiceChargeConfigurationId.ToString();
                txtServiceAmount.Text = serviceChargeDetailsList[0].ServiceAmount.ToString();
            }
            EmployeeBO vv = new EmployeeBO();
            foreach (var v in serviceChargeDetailsList)
            {
                vv = empList.Where(a => a.EmpId == v.EmpId).FirstOrDefault();
                empList.Remove(vv);
            }

            empGrid += "<table id='gvEmployee' class='table table-bordered table-condensed table-responsive' width='100%'>" +

                                                    "<colgroup>" +
                                                    "   <col style='width: 7%;' />" +
                                                    "   <col style='width: 53%;' />" +
                                                    "   <col style='width: 15%;' />" +
                                                    "   <col style='width: 25%;' />" +
                                                    "</colgroup>" +

                                                   " <thead>" +
                                                   "     <tr style='color: White; background-color: #44545E; font-weight: bold;'>" +
                                                   "         <th>" +
                                                   "             <input type='checkbox' id='checkAllEmployee' title='Select All Employee' />" +
                                                   "         </th>" +
                                                   "         <th style='text-align: left;'>" +
                                                   "             Name" +
                                                   "         </th>" +
                                                   "         <th style='text-align: left;'>" +
                                                   "             ID" +
                                                   "         </th>" +
                                                   "         <th style='text-align: left;'>" +
                                                   "             Designation" +
                                                   "         </th>" +
                                                   "     </tr>" +
                                                   " </thead>" +
                                                   " <tbody>";

            foreach (ServiceChargeConfigurationDetailsBO emp in serviceChargeDetailsList)
            {
                rowCounter++;
                if (rowCounter % 2 == 0)
                {
                    alternateColor = "style=\"background-color:#E3EAEB;\"";
                }
                else
                    alternateColor = "style=\"background-color:#FFFFFF;\"";

                empGrid += "<tr " + alternateColor + ">" +
                            "<td style=\"display:none;\">" +
                                emp.EmpId +
                            "</td>" +
                            "<td style=\"display:none;\">" +
                                emp.ServiceChargeConfigurationId +
                            "</td>" +
                            "<td style=\"display:none;\">" +
                                emp.ServiceChargeConfigurationDetailsId +
                            "</td>" +
                            "<td style = 'text-align: center; width:7%;' >" +
                                "<input type='checkbox' checked id='chk" + emp.EmpId + "' />" +
                            "</td>" +
                              "<td style=\"width: 53%;\">" +
                                emp.DisplayName +
                            "</td>" +
                            "<td style=\"width: 15%;\">" +
                                emp.EmpCode +
                            "</td>" +
                            "<td style=\"width: 25%;\">" +
                                emp.Designation +
                            "</td>" +
                        "</tr>";
            }


            foreach (EmployeeBO emp in empList)
            {
                rowCounter++;
                if (rowCounter % 2 == 0)
                {
                    alternateColor = "style=\"background-color:#E3EAEB;\"";
                }
                else
                    alternateColor = "style=\"background-color:#FFFFFF;\"";

                empGrid += "<tr " + alternateColor + ">" +
                            "<td style=\"display:none;\">" +
                                emp.EmpId +
                            "</td>" +
                            "<td style=\"display:none;\">" +
                                0 +
                            "</td>" +
                             "<td style=\"display:none;\">" +
                                0 +
                            "</td>" +
                            "<td style = 'text-align: center; width:7%;' >" +
                                "<input type='checkbox' id='chk" + emp.EmpId + "' />" +
                            "</td>" +
                              "<td style=\"width: 53%;\">" +
                                emp.DisplayName +
                            "</td>" +
                            "<td style=\"width: 15%;\">" +
                                emp.EmpCode +
                            "</td>" +
                            "<td style=\"width: 25%;\">" +
                                emp.Designation +
                            "</td>" +
                        "</tr>";
            }
            empGrid += " </tbody> </table>";
            Employee.InnerHtml = empGrid;
        }

        private void CheckObjectPermission()
        {
            btnSave.Visible = isSavePermission;
        }

        [WebMethod]
        public static ReturnInfo SaveServiceChargeConfiguration(ServiceChargeConfigurationBO ServiceCharge, List<ServiceChargeConfigurationDetailsBO> ServiceChargeDetails, List<ServiceChargeConfigurationDetailsBO> deleteList, Int16 totalEmployee)
        {
            int serviceChargeId = 0;
            HMUtility hmUtility = new HMUtility();
            ReturnInfo rtninf = new ReturnInfo();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            ServiceCharge.CreatedBy = userInformationBO.UserInfoId;
            //ServiceCharge.TotalEmployee = Convert.ToInt16(ServiceChargeDetails.Count());
            ServiceCharge.TotalEmployee = totalEmployee;

            ServiceChargeDA serviceChargeDA = new ServiceChargeDA();

            bool status = false;
            status = serviceChargeDA.SaveServiceChargeConfig(ServiceCharge, ServiceChargeDetails, deleteList, out serviceChargeId);

            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                               EntityTypeEnum.EntityType.ServiceChargeConfiguration.ToString(), serviceChargeId,
                               ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                               hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.ServiceChargeConfiguration));
                //return true;
            }
            else
            {
                rtninf.IsSuccess = false;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
    }
}