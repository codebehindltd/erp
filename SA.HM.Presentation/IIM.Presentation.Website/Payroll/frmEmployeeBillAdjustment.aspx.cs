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
using HotelManagement.Data.GeneralLedger;
using HotelManagement.Entity.GeneralLedger;
using System.Web.Services;
using System.Collections;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmployeeBillAdjustment : BasePage
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadEmployee();
                LoadCommonDropDownHiddenField();
            }
            CheckObjectPermission();
        }

        protected void CheckObjectPermission()
        {
            hfSavePermission.Value = isSavePermission ? "1" : "0";
            hfDeletePermission.Value = isDeletePermission ? "1" : "0";
            hfEditPermission.Value = isUpdatePermission ? "1" : "0";
            hfViewPermission.Value = isViewPermission ? "1" : "0";
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
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }

        [WebMethod]
        public static ArrayList EmployeeGeneratedBillBySearch(int employeeId)
        {
            EmployeeDA empDa = new EmployeeDA();
            EmployeeBO employee = new EmployeeBO();
            List<EmployeeBillGenerationBO> paymentInfo = new List<EmployeeBillGenerationBO>();

            employee = empDa.GetEmployeeInfoById(employeeId);
            paymentInfo = empDa.GetEmployeeGeneratedBillByBillStatus(employeeId);

            ArrayList arr = new ArrayList();
            arr.Add(employee);
            arr.Add(paymentInfo);

            return arr;
        }

        [WebMethod]
        public static List<EmployeeBillGenerateViewBO> EmployeeBillBySearch(int employeeBillId, int employeeId)
        {
            EmployeeDA employeeDa = new EmployeeDA();
            List<EmployeeBillGenerateViewBO> employeeBill = new List<EmployeeBillGenerateViewBO>();
            employeeBill = employeeDa.GetEmployeeBillForBillReceive(employeeId, employeeBillId);

            return employeeBill;
        }

        [WebMethod]
        public static ReturnInfo AdjustedEmployeeBillPayment(EmployeePaymentBO employeePayment, List<EmployeePaymentDetailsBO> employeePaymentDetails,
                                                      List<EmployeePaymentDetailsBO> employeePaymentDetailsEdited, List<EmployeePaymentDetailsBO> employeePaymentDetailsDeleted)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            EmployeeDA empDa = new EmployeeDA();
            long employeePaymentId;
            HMUtility hmUtility = new HMUtility();

            try
            {
                if (employeePayment.PaymentId == 0)
                {
                    rtninfo.IsSuccess = empDa.SaveEmployeeBillPayment(employeePayment, employeePaymentDetails, out employeePaymentId);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.EmployeePayment.ToString(), employeePaymentId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeePayment));
                }
                else
                {
                    rtninfo.IsSuccess = empDa.UpdateEmployeeBillPayment(employeePayment, employeePaymentDetails, employeePaymentDetailsEdited, employeePaymentDetailsDeleted);
                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                            EntityTypeEnum.EntityType.EmployeePayment.ToString(), employeePayment.PaymentId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeePayment));
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

                if (employeePayment.PaymentId == 0)
                {
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    

                } 
                else if (employeePayment.PaymentId > 0)
                {
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }
                   
            }
            else if (!rtninfo.IsSuccess)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninfo;
        }

        //[WebMethod]
        //public static List<CompanyBillGenerationBO> GetCompanyGeneratedBillByBillStatus(int companyId)
        //{
        //    GuestCompanyDA companyDa = new GuestCompanyDA();
        //    List<CompanyBillGenerationBO> paymentInfo = new List<CompanyBillGenerationBO>();

        //    paymentInfo = companyDa.GetCompanyGeneratedBillByBillStatus(companyId);

        //    return paymentInfo;
        //}

        [WebMethod]
        public static List<EmployeePaymentLedgerBO> EmployeeNonGeneratedBillBySearch(int employeeId)
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeePaymentLedgerBO> employeeBill = new List<EmployeePaymentLedgerBO>();

            employeeBill = empDa.EmployeeBillBySearch(employeeId);

            return employeeBill;
        }

        [WebMethod]
        public static List<EmployeePaymentBO> GetEmployeePaymentBySearch(int employeeId, DateTime? dateFrom, DateTime? dateTo)
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeePaymentBO> paymentInfo = new List<EmployeePaymentBO>();
            paymentInfo = empDa.GetEmployeePaymentBySearch(employeeId, dateFrom, dateTo, "Adjustment");

            return paymentInfo;
        }

        [WebMethod]
        public static List<EmployeePaymentLedgerVwBO> EmployeeBillAdvanceBySearch(int employeeId)
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeePaymentLedgerVwBO> paymentInfo = new List<EmployeePaymentLedgerVwBO>();

            paymentInfo = empDa.EmployeeBillAdvanceBySearch(employeeId);

            return paymentInfo;
        }

        [WebMethod]
        public static EmployeePaymentViewBO FillForm(Int64 paymentId)
        {
            EmployeeDA employeeDa = new EmployeeDA();
            EmployeePaymentViewBO paymentBO = new EmployeePaymentViewBO();

            paymentBO.EmployeePayment = employeeDa.GetEmployeePayment(paymentId);
            paymentBO.EmployeePaymentDetails = employeeDa.GetEmployeePaymentDetails(paymentId);
            paymentBO.Employee = employeeDa.GetEmployeeInfoById(paymentBO.EmployeePayment.EmployeeId);

            if (paymentBO.EmployeePayment.EmployeeBillId == 0)
            {
                paymentBO.EmployeeBill = employeeDa.GetCompanyBillForBillGenerationEdit(paymentBO.EmployeePayment.EmployeeId, 0);
            }
            else
            {
                paymentBO.EmployeeGeneratedBill = employeeDa.EmployeeBillBySearch(paymentBO.EmployeePayment.EmployeeId, paymentBO.EmployeePayment.EmployeeBillId);
            }

            return paymentBO;
        }

        [WebMethod]
        public static ReturnInfo DeleteEmployeePayment(Int64 paymentId)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            EmployeeDA employeeDa = new EmployeeDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
                rtninfo.IsSuccess = employeeDa.DeleteEmployeePayment(paymentId, userInformationBO.EmpId);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(),
                            EntityTypeEnum.EntityType.EmployeePayment.ToString(), paymentId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeePayment));
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            if (rtninfo.IsSuccess)
            {
                rtninfo.IsSuccess = true;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
            }

            return rtninfo;
        }

        [WebMethod]
        public static ReturnInfo ApprovedPaymentAdjustment(Int64 paymentId, string adjustmentType)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            EmployeeDA empDa = new EmployeeDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();

            try
            {
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                if (adjustmentType == "Adjustment")
                {
                    rtninfo.IsSuccess = empDa.ApprovedPayment(paymentId, userInformationBO.EmpId);
                }
                else
                {
                    rtninfo.IsSuccess = empDa.ApprovedRefund(paymentId, userInformationBO.EmpId);
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
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Approved, AlertType.Success);
                bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Approve.ToString(),
                            EntityTypeEnum.EntityType.EmployeePayment.ToString(), paymentId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.EmployeePayment));
            }

            return rtninfo;
        }
    }
}