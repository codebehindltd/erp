using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Presentation.Website.Common;
using System.Web.Services;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.Payroll;

namespace HotelManagement.Presentation.Website.Recruitment
{
    public partial class frmResumeSearch : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLocation();
                LoadJobCategory();
                LoadOrganizationType();
                LoadEmployeeType();
                LoadDepartment();
                LoadDesignation();
                LoadWorkStation();
                LoadBloodGroup();
                LoadDivision();
                LoadCommonDropDownHiddenField();
            }
        }
        private void LoadLocation()
        {
            LocationDA locationDA = new LocationDA();
            List<LocationBO> entityBOList = new List<LocationBO>();
            entityBOList = locationDA.GetLocationInfo();

            this.ddlPrfJobLocation.DataSource = entityBOList;
            this.ddlPrfJobLocation.DataTextField = "LocationName";
            this.ddlPrfJobLocation.DataValueField = "LocationId";
            this.ddlPrfJobLocation.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlPrfJobLocation.Items.Insert(0, item);
        }
        private void LoadJobCategory()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("EmpJobCategory", hmUtility.GetDropDownFirstValue());

            this.ddlJobCategory.DataSource = fields;
            this.ddlJobCategory.DataTextField = "FieldValue";
            this.ddlJobCategory.DataValueField = "FieldId";
            this.ddlJobCategory.DataBind();
        }
        private void LoadOrganizationType()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("EmpOrganizationType", hmUtility.GetDropDownFirstValue());

            this.ddlOrganizationType.DataSource = fields;
            this.ddlOrganizationType.DataTextField = "FieldValue";
            this.ddlOrganizationType.DataValueField = "FieldId";
            this.ddlOrganizationType.DataBind();
        }
        private void LoadEmployeeType()
        {
            EmpTypeDA entityDA = new EmpTypeDA();
            this.ddlEmpCategoryId.DataSource = entityDA.GetEmpTypeInfo();
            this.ddlEmpCategoryId.DataTextField = "Name";
            this.ddlEmpCategoryId.DataValueField = "TypeId";
            this.ddlEmpCategoryId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEmpCategoryId.Items.Insert(0, item);
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            this.ddlDepartmentId.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartmentId.DataTextField = "Name";
            this.ddlDepartmentId.DataValueField = "DepartmentId";
            this.ddlDepartmentId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlDepartmentId.Items.Insert(0, item);
        }
        private void LoadDesignation()
        {
            DesignationDA entityDA = new DesignationDA();
            this.ddlDesignationId.DataSource = entityDA.GetDesignationInfo();
            this.ddlDesignationId.DataTextField = "Name";
            this.ddlDesignationId.DataValueField = "DesignationId";
            this.ddlDesignationId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlDesignationId.Items.Insert(0, item);
        }
        private void LoadWorkStation()
        {
            EmployeeDA workStationDA = new EmployeeDA();
            List<EmpWorkStationBO> entityBOList = new List<EmpWorkStationBO>();
            entityBOList = workStationDA.GetEmployWorkStation();

            this.ddlWorkStation.DataSource = entityBOList;
            this.ddlWorkStation.DataTextField = "WorkStationName";
            this.ddlWorkStation.DataValueField = "WorkStationId";
            this.ddlWorkStation.DataBind();

            ddlWorkStation.Items.Insert(0, new ListItem { Text = hmUtility.GetDropDownFirstValue(), Value = string.Empty });
        }
        private void LoadBloodGroup()
        {
            HMCommonDA commonDA = new HMCommonDA();
            CustomFieldBO customField = new CustomFieldBO();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("BloodGroup", hmUtility.GetDropDownFirstValue());

            this.ddlBloodGroup.DataSource = fields;
            this.ddlBloodGroup.DataTextField = "FieldValue";
            this.ddlBloodGroup.DataValueField = "FieldValue";
            this.ddlBloodGroup.DataBind();
        }
        private void LoadDivision()
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmpDivisionBO> divList = new List<EmpDivisionBO>();
            divList = empDA.GetEmpDivisionList();

            this.ddlEmpDivision.DataSource = divList;
            this.ddlEmpDivision.DataTextField = "DivisionName";
            this.ddlEmpDivision.DataValueField = "DivisionId";
            this.ddlEmpDivision.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlEmpDivision.Items.Insert(0, item);

            this.ddlAppDivision.DataSource = divList;
            this.ddlAppDivision.DataTextField = "DivisionName";
            this.ddlAppDivision.DataValueField = "DivisionId";
            this.ddlAppDivision.DataBind();

            this.ddlAppDivision.Items.Insert(0, item);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }
        [WebMethod]
        public static GridViewDataNPaging<EmployeeBO, GridPaging> SearchApplicantResume(string appName, string appId, string lookingFor, string availableFor, string preSalFrom, string preSalTo, string expSalFrom, string expSalTo, string currency, string jobCategory, string organizationType, string jobLocation, string expYrFrom, string expYrTo, string divisionId, string districtId, string thanaId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;
            decimal psFrom = 0, psTo = 0, esFrom = 0, esTo = 0;
            int joblocId = 0, jobCat = 0, orgType = 0, exFrom = 0, exTo = 0, divId = 0, distId = 0, thaId = 0;
            if (!string.IsNullOrWhiteSpace(jobLocation))
            {
                joblocId = Convert.ToInt32(jobLocation);
            }
            if (!string.IsNullOrWhiteSpace(jobCategory))
            {
                jobCat = Convert.ToInt32(jobCategory);
            }
            if (!string.IsNullOrWhiteSpace(organizationType))
            {
                orgType = Convert.ToInt32(organizationType);
            }
            if (!string.IsNullOrWhiteSpace(preSalFrom))
            {
                psFrom = Convert.ToDecimal(preSalFrom);
            }
            if (!string.IsNullOrWhiteSpace(preSalTo))
            {
                psTo = Convert.ToDecimal(preSalTo);
            }
            if (!string.IsNullOrWhiteSpace(expSalFrom))
            {
                esFrom = Convert.ToDecimal(expSalFrom);
            }
            if (!string.IsNullOrWhiteSpace(expSalTo))
            {
                esTo = Convert.ToDecimal(expSalTo);
            }
            if (!string.IsNullOrWhiteSpace(expYrFrom))
            {
                exFrom = Convert.ToInt32(expYrFrom);
            }
            if (!string.IsNullOrWhiteSpace(expYrTo))
            {
                exTo = Convert.ToInt32(expYrTo);
            }
            if (!string.IsNullOrEmpty(divisionId))
            {
                divId = Convert.ToInt32(divisionId);
            }
            if (!string.IsNullOrEmpty(districtId))
            {
                distId = Convert.ToInt32(districtId);
            }
            if (!string.IsNullOrEmpty(thanaId))
            {
                thaId = Convert.ToInt32(thanaId);
            }

            GridViewDataNPaging<EmployeeBO, GridPaging> myGridData = new GridViewDataNPaging<EmployeeBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            ApplicantResumeSearchDA resumeSearchDA = new ApplicantResumeSearchDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = resumeSearchDA.GetApplicantResumeInfo(appName, appId, lookingFor, availableFor, psFrom, psTo, esFrom, esTo, Convert.ToInt32(currency), jobCat, orgType, joblocId, exFrom, exTo, divId, distId, thaId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<EmployeeBO> distinctItems = new List<EmployeeBO>();
            distinctItems = empList.GroupBy(test => test.EmpId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }
        [WebMethod]
        public static GridViewDataNPaging<EmployeeBO, GridPaging> SearchEmployeeResume(string empType, string department, string position, string workStation, string ageFrom, string ageTo, string jobLenFrom, string jobLenTo, string expFrom, string expTo, string bloodGroup, string divisionId, string districtId, string thanaId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
        {
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = System.Web.HttpContext.Current.Session["UserInformationBOSession"] as UserInformationBO;

            int totalRecords = 0;
            int empTypeId = 0, depId = 0, posId = 0, workStId = 0, gradeId = 0, fromAge = 0, toAge = 0, divId = 0, distId = 0, thaId = 0; 
            int? fromJobLen = null, toJobLen = null, fromExp = null, toExp = null;
            DateTime? fromDOB = null, toDOB = null;
  
            if (!string.IsNullOrWhiteSpace(empType))
            {
                empTypeId = Convert.ToInt32(empType);
            }
            if (!string.IsNullOrWhiteSpace(department))
            {
                depId = Convert.ToInt32(department);
            }
            if (!string.IsNullOrWhiteSpace(position))
            {
                posId = Convert.ToInt32(position);
            }
            if (!string.IsNullOrWhiteSpace(workStation))
            {
                workStId = Convert.ToInt32(workStation);
            }
            if (!string.IsNullOrWhiteSpace(ageFrom))
            {
                fromAge = Convert.ToInt32(ageFrom);
            }
            if (!string.IsNullOrWhiteSpace(ageTo))
            {
                toAge = Convert.ToInt32(ageTo);
            }
            if (!string.IsNullOrWhiteSpace(jobLenFrom))
            {
                fromJobLen = Convert.ToInt32(jobLenFrom);
            }
            if (!string.IsNullOrWhiteSpace(jobLenTo))
            {
                toJobLen = Convert.ToInt32(jobLenTo);
            }
            if (!string.IsNullOrWhiteSpace(expFrom))
            {
                fromExp = Convert.ToInt32(expFrom);
            }
            if (!string.IsNullOrWhiteSpace(expTo))
            {
                toExp = Convert.ToInt32(expTo);
            }
            if (!string.IsNullOrEmpty(divisionId))
            {
                divId = Convert.ToInt32(divisionId);
            }
            if (!string.IsNullOrEmpty(districtId))
            {
                distId = Convert.ToInt32(districtId);
            }
            if (!string.IsNullOrEmpty(thanaId))
            {
                thaId = Convert.ToInt32(thanaId);
            }

            GridViewDataNPaging<EmployeeBO, GridPaging> myGridData = new GridViewDataNPaging<EmployeeBO, GridPaging>(userInformationBO.GridViewPageSize, userInformationBO.GridViewPageLink, isCurrentOrPreviousPage);
            pageNumber = myGridData.PageNumberCalculation(gridRecordsCount, pageNumber);

            HMCommonDA commonDA = new HMCommonDA();
            ApplicantResumeSearchDA resumeSearchDA = new ApplicantResumeSearchDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();
            empList = resumeSearchDA.GetEmployeeResumeInfo(empTypeId, depId, posId, workStId, fromAge, toAge, fromJobLen, toJobLen, fromExp, toExp, gradeId, bloodGroup, fromDOB, toDOB, divId, distId, thaId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

            List<EmployeeBO> distinctItems = new List<EmployeeBO>();
            distinctItems = empList.GroupBy(test => test.EmpId).Select(group => group.First()).ToList();

            myGridData.GridPagingProcessing(distinctItems, totalRecords);

            return myGridData;
        }

        [WebMethod]
        public static string GetDocumentsByUserTypeAndUserId(string empId)
        {
            string UserType = "";
            int UserId = 0;
            List<DocumentsBO> docList = new List<DocumentsBO>();
            DocumentsDA docDA = new DocumentsDA();
            docList = docDA.GetDocumentsByUserTypeAndUserId("Applicant Other Documents", Int32.Parse(empId));
            //docList = GetDocumentListWithIcon(docList);
            string strTable = "";
            strTable += "<div style='color: White; background-color: #44545E;width:750px;'>";
            int counter = 0;
            foreach (DocumentsBO dr in docList)
            {
                if (dr.Extention == ".jpg")
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:250px; height:250px; float:left;padding:30px'>";
                    strTable += "<a target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img id= style='width: 100px; height: 100px;' src='" + ImgSource + "'  alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
                else
                {
                    string ImgSource = dr.Path + dr.Name;
                    counter++;
                    strTable += "<div style=' width:100px; height:100px; float:left;padding:30px'>";
                    strTable += "<a target='_blank' href='" + ImgSource + "'>";
                    strTable += "<img id= style='width: 100px; height: 100px;' src='" + dr.IconImage + "' alt='Image preview' />";
                    strTable += "<span>'" + dr.Name + "'</span>";
                    strTable += "</a>";
                    strTable += "</div>";
                }
            }
            strTable += "</div>";
            if (strTable == "")
            {
                strTable = "<tr><td colspan='4' align='center'>No Record Available!</td></tr>";
            }
            return strTable;

        }

        [WebMethod]
        public static List<PayrollJobCircularBO> GetJobCircular()
        {
            List<PayrollJobCircularBO> jobCircular = new List<PayrollJobCircularBO>();
            JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();

            jobCircular = jobCircularDa.GetApprovedJobCircular(0, null, null, null);

            return jobCircular;
        }

        [WebMethod]
        public static ReturnInfo AssignApplicantToJob(string applicantType, List<string> emp, List<string> job)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            bool status = false;

            try
            {
                HMUtility hmUtility = new HMUtility();
                UserInformationBO userInformationBO = new UserInformationBO();
                userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

                JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();
                status = jobCircularDa.SavePayrollJobCircularApplicantMapping(applicantType, emp, job);

                if (status)
                {
                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }

                if (!status)
                {
                    rtninfo.IsSuccess = false;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }
            catch (Exception ex)
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
               
            }

            return rtninfo;
        }

        [WebMethod]
        public static List<EmpDistrictBO> LoadDistrict(string divisionId)
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmpDistrictBO> disList = new List<EmpDistrictBO>();
            disList = empDA.GetEmpDistrictList(Convert.ToInt32(divisionId));

            return disList;
        }
        [WebMethod]
        public static List<EmpThanaBO> LoadThana(string districtId)
        {
            EmployeeDA empDA = new EmployeeDA();
            List<EmpThanaBO> thanaList = new List<EmpThanaBO>();
            thanaList = empDA.GetEmpThanaList(Convert.ToInt32(districtId));

            return thanaList;
        }
    }
}