using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Payroll;
using HotelManagement.Entity.Payroll;
using System.Web.Services;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.UserInformation;

namespace HotelManagement.Presentation.Website.Payroll
{
    public partial class frmEmpSearch : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployeeType();
                LoadDepartment();
                LoadDesignation();
                LoadWorkStation();
                LoadGrade();
                LoadBloodGroup();
                LoadDivision();
                LoadCommonDropDownHiddenField();
            }
        }

        private void LoadEmployeeType()
        {
            EmpTypeDA entityDA = new EmpTypeDA();
            ddlEmpCategoryId.DataSource = entityDA.GetEmpTypeInfo();
            ddlEmpCategoryId.DataTextField = "Name";
            ddlEmpCategoryId.DataValueField = "TypeId";
            ddlEmpCategoryId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlEmpCategoryId.Items.Insert(0, item);
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
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlDepartmentId.Items.Insert(0, item);
        }
        private void LoadDesignation()
        {
            DesignationDA entityDA = new DesignationDA();
            ddlDesignationId.DataSource = entityDA.GetDesignationInfo();
            ddlDesignationId.DataTextField = "Name";
            ddlDesignationId.DataValueField = "DesignationId";
            ddlDesignationId.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlDesignationId.Items.Insert(0, item);
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

            ddlWorkStation.Items.Insert(0, new ListItem { Text = hmUtility.GetDropDownFirstValue(), Value = string.Empty });
        }
        private void LoadGrade()
        {
            EmpGradeDA gradeDA = new EmpGradeDA();
            var List = gradeDA.GetGradeInfo(); ;
            this.ddlGradeId.DataSource = List;
            this.ddlGradeId.DataTextField = "Name";
            this.ddlGradeId.DataValueField = "GradeId";
            this.ddlGradeId.DataBind();
            ListItem item = new ListItem();
            item.Value = "0";
            //item.Value = "---None---";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlGradeId.Items.Insert(0, item);
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

            this.ddlDivision.DataSource = divList;
            this.ddlDivision.DataTextField = "DivisionName";
            this.ddlDivision.DataValueField = "DivisionId";
            this.ddlDivision.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlDivision.Items.Insert(0, item);
        }
        private void LoadCommonDropDownHiddenField()
        {
            CommonDropDownHiddenField.Value = hmUtility.GetDropDownFirstValue();
        }

        //************************ User Defined Web Method ********************//
        [WebMethod]
        public static GridViewDataNPaging<EmployeeBO, GridPaging> SearchEmployeeResume(string empType, string department, string position, string workStation, string ageFrom, string ageTo, string jobLenFrom, string jobLenTo, string expFrom, string expTo, string grade, string bloodgroup, string dobFrom, string dobTo, string divisionId, string districtId, string thanaId, int gridRecordsCount, int pageNumber, int isCurrentOrPreviousPage)
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
            if (!string.IsNullOrWhiteSpace(grade))
            {
                gradeId = Convert.ToInt32(grade);
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
            if (!string.IsNullOrWhiteSpace(dobFrom))
            {
                //fromDOB = Convert.ToDateTime(dobFrom);
                fromDOB = CommonHelper.DateTimeToMMDDYYYY(dobFrom);
            }
            if (!string.IsNullOrWhiteSpace(dobTo))
            {
                //toDOB = Convert.ToDateTime(dobTo);
                toDOB = CommonHelper.DateTimeToMMDDYYYY(dobTo);
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
            empList = resumeSearchDA.GetEmployeeResumeInfo(empTypeId, depId, posId, workStId, fromAge, toAge, fromJobLen, toJobLen, fromExp, toExp, gradeId, bloodgroup, fromDOB, toDOB, divId, distId, thaId, userInformationBO.GridViewPageSize, pageNumber, out totalRecords);

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