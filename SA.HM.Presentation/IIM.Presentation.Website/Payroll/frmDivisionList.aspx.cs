using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using System.Collections;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmDivisionList : BasePage
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCountry();
                LoadCommonDropDownHiddenField();
            }
        }
        private void LoadCountry()
        {
            HMCommonDA commonDA = new HMCommonDA();
            var List = commonDA.GetAllCountries();

            ddlCountry.DataSource = List;
            ddlCountry.DataTextField = "CountryName";
            ddlCountry.DataValueField = "CountryId";
            ddlCountry.DataBind();

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();
            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("CompanyCountryId", "CompanyCountryId");
            ddlCountry.SelectedValue = commonSetupBO.SetupValue;

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlCountry.Items.Insert(0, item);
        }
        
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        [WebMethod]
        public static ReturnInfo DeleteData(int Id, string type)
        {
            ReturnInfo rtninf = new ReturnInfo();
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            HMCommonDA DA = new HMCommonDA();
            if (type == "Division")
            {
                status = DA.DeleteInfoById("PayrollEmpDivision", "DivisionId", Id);
            }
            else if(type == "District")
            {
                status = DA.DeleteInfoById("PayrollEmpDistrict", "DistrictId", Id);
            }
            else if (type == "Thana")
            {
                status = DA.DeleteInfoById("PayrollEmpThana", "ThanaId", Id);
            }
            
            if (status)
            {
                rtninf.IsSuccess = true;
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
            }
            else
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return rtninf;
        }
        [WebMethod]
        public static ArrayList FillForm(int Id, string type)
        {
            ArrayList arr = new ArrayList();
            EmployeeDA employeeDA = new EmployeeDA();
            EmpDistrictBO districtBO = new EmpDistrictBO();
            EmpDivisionBO divisionBO = new EmpDivisionBO();
            EmpThanaBO thanaBO = new EmpThanaBO();

            if (type == "Division")
            {
                divisionBO = employeeDA.GetEmpDivisionById(Id);
                
            }
            else if (type == "District")
            {
                districtBO = employeeDA.GetEmpDistrictById(Id);
            }
            else if (type == "Thana")
            {
                thanaBO = employeeDA.GetEmpThanaListById(Id);
            }

            arr.Add(new { Division = divisionBO, District = districtBO, Thana = thanaBO, Type = type });
            return arr;
        }
        [WebMethod]
        public static List<EmpDistrictBO> LoadDistrict()
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmpDistrictBO> disList = new List<EmpDistrictBO>();
            disList = empDA.GetEmpDistrictListForShow();
            return disList;
        }
        [WebMethod]
        public static GridViewDataNPaging<SetupSearchBO, GridPaging>Search(string type, string name, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            EmployeeDA employeeDA = new EmployeeDA();
            List<SetupSearchBO> infoBOs = new List<SetupSearchBO>();
            int totalRecords = 0;

            GridViewDataNPaging<SetupSearchBO, GridPaging> myGridData = new GridViewDataNPaging<SetupSearchBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);
            infoBOs = employeeDA.GetSetupSearchInformation(type, name, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);
            myGridData.GridPagingProcessing(infoBOs, totalRecords);
            return myGridData;
        }
        [WebMethod]
        public static List<EmpDivisionBO> LoadDivision()
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmpDivisionBO> divisionBOs = new List<EmpDivisionBO>();
            divisionBOs = empDA.GetEmpDivisionList();
            return divisionBOs;
        }
        [WebMethod]
        public static ReturnInfo SaveThana(string thana, int districtId, int thanaId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            HMUtility hmUtility = new HMUtility();
            int tempId;
            EmpThanaBO thanaBO = new EmpThanaBO();
            EmployeeDA employeeDA = new EmployeeDA();

            thanaBO.ThanaId = thanaId;
            thanaBO.ThanaName = thana;
            thanaBO.DistrictId = districtId;
            thanaBO.CreatedBy = userInformationBO.UserInfoId;

            rtninf.IsSuccess = employeeDA.SaveThana(thanaBO, out tempId);

            if (rtninf.IsSuccess)
            {
                if (thanaBO.ThanaId == 0)
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }
                
            }
            else
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo SaveDivision(string division, int countryid, int divisionId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;
            HMUtility hmUtility = new HMUtility();
            int tempId;
            EmpDivisionBO divisionBO = new EmpDivisionBO();
            EmployeeDA employeeDA = new EmployeeDA();

            divisionBO.DivisionId = divisionId;
            divisionBO.DivisionName = division;
            divisionBO.CountryId = countryid;
            divisionBO.CreatedBy = userInformationBO.UserInfoId;

            rtninf.IsSuccess = employeeDA.SaveDivision(divisionBO, out tempId);

            if (rtninf.IsSuccess)
            {
                if (divisionBO.DivisionId == 0)
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }
                
            }
            else
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }
        [WebMethod]
        public static ReturnInfo SaveDisctrict(string district, int divisionId, int districtId)
        {
            ReturnInfo rtninf = new ReturnInfo();
            UserInformationBO userInformationBO = new UserInformationBO();
            HMUtility hmUtility = new HMUtility();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            int tempId;
            EmpDistrictBO districtBO = new EmpDistrictBO();
            EmployeeDA employeeDA = new EmployeeDA();

            districtBO.DistrictId = districtId;
            districtBO.DistrictName = district;
            districtBO.DivisionId = divisionId;
            districtBO.CreatedBy = userInformationBO.UserInfoId;

            rtninf.IsSuccess = employeeDA.SaveDisctrict(districtBO, out tempId);

            if (rtninf.IsSuccess)
            {
                if (districtBO.DistrictId == 0)
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }
                
            }
            else
            {
                rtninf.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }

            return rtninf;
        }

        
    }
}