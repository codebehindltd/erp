using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Membership;
using HotelManagement.Data.Membership;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Data.HotelManagement;
using System.Web.Services;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmployeeBillGeneration : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadEmployee();
                LoadCurrency();
            }
        }

        private void LoadEmployee()
        {
            EmployeeDA employeeDa = new EmployeeDA();
            List<EmployeeBO> employeeList = new List<EmployeeBO>();
            employeeList = employeeDa.GetEmployeeInfo();

            ddlEmployee.DataSource = employeeList;
            ddlEmployee.DataTextField = "DisplayName";
            ddlEmployee.DataValueField = "EmpId";
            ddlEmployee.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownNoneValue();
            ddlEmployee.Items.Insert(0, item);

            ddlEmployeeForSearch.DataSource = employeeList;
            ddlEmployeeForSearch.DataTextField = "DisplayName";
            ddlEmployeeForSearch.DataValueField = "EmpId";
            ddlEmployeeForSearch.DataBind();
            ddlEmployeeForSearch.Items.Insert(0, item);

        }

        private void LoadCurrency()
        {
            CommonCurrencyDA headDA = new CommonCurrencyDA();
            List<CommonCurrencyBO> currencyListBO = new List<CommonCurrencyBO>();
            currencyListBO = headDA.GetConversionHeadInfoByType("LocalNUsd");

            ddlCurrency.DataSource = currencyListBO;
            ddlCurrency.DataTextField = "CurrencyName";
            ddlCurrency.DataValueField = "CurrencyId";
            ddlCurrency.DataBind();
        }

        [WebMethod]
        public static List<EmployeePaymentLedgerBO> EmployeeBillBySearch(int employeeId)
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeePaymentLedgerBO> employeeBill = new List<EmployeePaymentLedgerBO>();

            employeeBill = empDa.EmployeeBillBySearch(employeeId);

            return employeeBill;
        }

        [WebMethod]
        public static ReturnInfo GenerateEmployeeBill(EmployeeBillGenerationBO billGeneration, List<EmployeeBillGenerationDetailsBO> billGenerationDetails,
                      List<EmployeeBillGenerationDetailsBO> billGenerationDetailsEdited, List<EmployeeBillGenerationDetailsBO> billGenerationDetailsDeleted)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            EmployeeDA empDa = new EmployeeDA();
            HMUtility hmUtility = new HMUtility();
            Int64 employeeBillId = 0;

            try
            {
                if (billGeneration.EmployeeBillId == 0)
                {
                    rtninfo.IsSuccess = empDa.SaveEmployeeBillGeneration(billGeneration, billGenerationDetails, out employeeBillId);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmployeeBillGeneration.ToString(), employeeBillId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeeBillGeneration));
                }
                else
                {
                    rtninfo.IsSuccess = empDa.UpdateEmployeeBillGeneration(billGeneration, billGenerationDetails, billGenerationDetailsEdited, billGenerationDetailsDeleted);
                    employeeBillId = billGeneration.EmployeeBillId;
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmployeeBillGeneration.ToString(), employeeBillId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeeBillGeneration));
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            if (rtninfo.IsSuccess)
            {
                rtninfo.IsSuccess = true;
                rtninfo.Pk = employeeBillId;

                if (billGeneration.EmployeeBillId == 0)
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                else
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
            }

            return rtninfo;
        }

        [WebMethod]
        public static List<EmployeeBillGenerationBO> GetEmployeeBillGenerationBySearch(DateTime? dateFrom, DateTime? dateTo, int employeeId)
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBillGenerationBO> paymentInfo = new List<EmployeeBillGenerationBO>();
            paymentInfo = empDa.GetEmployeeBillGenerationBySearch(dateFrom, dateTo, employeeId);

            return paymentInfo;
        }

        [WebMethod]
        public static EmployeeBillGenerationViewBO FillForm(int employeeBillId)
        {
            EmployeeDA empDa = new EmployeeDA();
            EmployeeBillGenerationViewBO paymentBO = new EmployeeBillGenerationViewBO();

            paymentBO.BillGeneration = empDa.GetCompanyBillGeneration(employeeBillId);
            paymentBO.BillGenerationDetails = empDa.GetCompanyBillGenerationDetails(employeeBillId);
            paymentBO.EmployeeBill = empDa.GetCompanyBillForBillGenerationEdit(paymentBO.BillGeneration.EmployeeId, employeeBillId);

            return paymentBO;
        }

        [WebMethod]
        public static ReturnInfo DeleteEmployeeBillGeneration(int employeeBillId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            EmployeeDA empDa = new EmployeeDA();

            try
            {
                rtninfo.IsSuccess = empDa.DeleteEmployeeBillGeneration(employeeBillId);

                if (rtninfo.IsSuccess)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }
    }
}