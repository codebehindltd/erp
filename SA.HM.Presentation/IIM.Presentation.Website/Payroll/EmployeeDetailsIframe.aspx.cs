using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class EmployeeDetailsIframe : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmpSearchEnableWithDetails();
                if (Convert.ToInt32(hfEmpId.Value) > 0)
                {
                    var empId = Convert.ToInt32(hfEmpId.Value);
                    //GenerateReport(empId);
                }
                
            }
        }
        private void LoadEmpSearchEnableWithDetails()
        {
            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupBO commonSetupBO2 = new HMCommonSetupBO();

            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            
            commonSetupBO2 = commonSetupDA.GetCommonConfigurationInfo("IsEmpSearchDetailsFromDashboardEnable", "IsEmpSearchDetailsFromDashboardEnable");
            if (!string.IsNullOrEmpty(commonSetupBO2.SetupValue))
            {
                hfIsEmpSearchDetailsFromDashboardEnable.Value = commonSetupBO2.SetupValue.ToString();
            }
        }
        private void LoadExtraShowHideInformation()
        {
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            HMCommonSetupBO isPayrollWorkStationHideBO = new HMCommonSetupBO();
            isPayrollWorkStationHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollWorkStationHide", "IsPayrollWorkStationHide");
            if (isPayrollWorkStationHideBO != null)
            {
                if (isPayrollWorkStationHideBO.SetupValue == "1")
                {
                    PayrollWorkStationDiv.Visible = false;
                }
            }

            HMCommonSetupBO isPayrollDonorNameAndActivityCodeHideBO = new HMCommonSetupBO();
            isPayrollDonorNameAndActivityCodeHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollDonorNameAndActivityCodeHide", "IsPayrollDonorNameAndActivityCodeHide");
            if (isPayrollDonorNameAndActivityCodeHideBO != null)
            {
                if (isPayrollDonorNameAndActivityCodeHideBO.SetupValue == "1")
                {
                    PayrollDonorNameAndActivityCodeHideDiv.Visible = false;
                }
            }

            HMCommonSetupBO isPayrollDependentHideBO = new HMCommonSetupBO();
            isPayrollDependentHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollDependentHide", "IsPayrollDependentHide");
            if (isPayrollDependentHideBO != null)
            {
                if (isPayrollDependentHideBO.SetupValue == "1")
                {
                    hfIsPayrollDependentHide.Value = "1";
                }
            }

            HMCommonSetupBO isPayrollBeneficiaryHideBO = new HMCommonSetupBO();
            isPayrollBeneficiaryHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollBeneficiaryHide", "IsPayrollBeneficiaryHide");
            if (isPayrollBeneficiaryHideBO != null)
            {
                if (isPayrollBeneficiaryHideBO.SetupValue == "1")
                {
                    hfIsPayrollBeneficiaryHide.Value = "1";
                }
            }
            HMCommonSetupBO isPayrollReferenceHideBO = new HMCommonSetupBO();
            isPayrollReferenceHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollReferenceHide", "IsPayrollReferenceHide");
            if (isPayrollReferenceHideBO != null)
            {
                if (isPayrollReferenceHideBO.SetupValue == "1")
                {
                    hfIsPayrollReferenceHide.Value = "1";
                }
            }

            HMCommonSetupBO isPayrollBenefitsHideBO = new HMCommonSetupBO();
            isPayrollBenefitsHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollBenefitsHide", "IsPayrollBenefitsHide");
            if (isPayrollBenefitsHideBO != null)
            {
                if (isPayrollBenefitsHideBO.SetupValue == "1")
                {
                    hfIsPayrollBenefitsHide.Value = "1";
                }
            }

            HMCommonSetupBO isProvidentFundDeductHideBO = new HMCommonSetupBO();
            isProvidentFundDeductHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollProvidentFundDeductHide", "IsPayrollProvidentFundDeductHide");
            if (isProvidentFundDeductHideBO != null)
            {
                if (isProvidentFundDeductHideBO.SetupValue == "1")
                {
                    //IsProvidentFundDeductHideDiv.Visible = false;
                }
            }

            HMCommonSetupBO isPayrollCostCenterDivHideBO = new HMCommonSetupBO();
            isPayrollCostCenterDivHideBO = commonSetupDA.GetCommonConfigurationInfo("IsPayrollCostCenterDivHide", "IsPayrollCostCenterDivHide");
            if (isPayrollCostCenterDivHideBO != null)
            {
                if (isPayrollCostCenterDivHideBO.SetupValue == "1")
                {
                    CostCenterDiv.Visible = false;
                }
            }
        }
        [WebMethod]
        public static ArrayList FillForm(int Id)
        {
           
            EmployeeDA employeeDA = new EmployeeDA();

            EmployeeBO bo = new EmployeeBO();
            
            bo = employeeDA.GetEmployeeInfoById(Id);

            List<EmpEducationBO> eduBO = new List<EmpEducationBO>();
            EmpEducationDA eduDA = new EmpEducationDA();

            eduBO = eduDA.GetEmpEducationByEmpId(Id);


            List<EmpExperienceBO> expBO = new List<EmpExperienceBO>();
            EmpExperienceDA expDA = new EmpExperienceDA();

            expBO = expDA.GetEmpExperienceByEmpId(Id);


            List<EmpDependentBO> dpnBO = new List<EmpDependentBO>();
            EmpDependentDA dpnDA = new EmpDependentDA();
            dpnBO = dpnDA.GetEmpDependentByEmpId(Id);

            EmpBankInfoBO bankInfo = new EmpBankInfoBO();
            bankInfo = employeeDA.GetEmployeeBankInfo(bo.EmpId);

            List<EmpNomineeBO> nomineeBO = new List<EmpNomineeBO>();
            EmpNomineeDA nomineeDA = new EmpNomineeDA();
            nomineeBO = nomineeDA.GetEmpNomineeByEmpId(Id);


            List<EmpReferenceBO> ReferenceBO = new List<EmpReferenceBO>();
            EmpReferenceDA referenceDA = new EmpReferenceDA();
            ReferenceBO = referenceDA.GetEmpReferenceByEmpId(Id); ;
            

            ArrayList arr = new ArrayList();
            arr.Add(new { EmployeeList = bo, EmpEducation = eduBO, EmpExperience = expBO, EmpDependent = dpnBO, EmpNominee = nomineeBO, EmpReference = ReferenceBO, BankInfo = bankInfo });

            return arr;
        }
        
        [WebMethod]
        public static GridViewDataNPaging<EmployeeBO, GridPaging> SearchNLoadEmpInformation(string text, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;

            GridViewDataNPaging<EmployeeBO, GridPaging> myGridData = new GridViewDataNPaging<EmployeeBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            EmployeeDA empDA = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = empDA.GetEmpInformationBySearchCriteria(text, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<EmployeeBO> distinctItems = new List<EmployeeBO>();
            distinctItems = empList.GroupBy(test => test.EmpId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
    }
}