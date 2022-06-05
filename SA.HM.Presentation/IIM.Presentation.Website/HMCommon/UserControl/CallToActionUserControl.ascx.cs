using HotelManagement.Data.Payroll;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.HMCommon.UserControl
{
    public partial class CallToActionUserControl : System.Web.UI.UserControl
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadEmployeeInfo();
                GetContactInformation();
            }
        }
        public void GetContactInformation()
        {
            List<ContactInformationBO> contactInformationList = new List<ContactInformationBO>();

            ContactInformationDA DA = new ContactInformationDA();
            List<ContactInformationBO> contactInfo = new List<ContactInformationBO>();
            contactInfo = DA.GetContactInformation();

            ddlParticipantFromClient.DataSource = contactInfo;
            ddlParticipantFromClient.DataTextField = "Name";
            ddlParticipantFromClient.DataValueField = "Id";
            ddlParticipantFromClient.DataBind();
        }
        private void LoadEmployeeInfo()
        {
            EmployeeDA empDa = new EmployeeDA();
            List<EmployeeBO> empList = new List<EmployeeBO>();

            empList = empDa.GetEmployeeInfo();
            ddlParticipantFromOffice.DataSource = empList;
            ddlParticipantFromOffice.DataTextField = "DisplayName";
            ddlParticipantFromOffice.DataValueField = "EmpId";
            ddlParticipantFromOffice.DataBind();
            
            ddlAssignToEmployee.DataSource = empList;
            ddlAssignToEmployee.DataTextField = "DisplayName";
            ddlAssignToEmployee.DataValueField = "EmpId";
            ddlAssignToEmployee.DataBind();
                                  
        }
    }
}