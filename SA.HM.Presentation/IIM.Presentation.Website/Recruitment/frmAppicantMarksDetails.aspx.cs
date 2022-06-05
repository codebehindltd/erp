using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Newtonsoft.Json;

using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.UserInformation;
using HotelManagement.Entity.Payroll;
using HotelManagement.Data.Payroll;
using HotelManagement.Presentation.Website.Common;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Data.HMCommon;
using System.Collections;

namespace HotelManagement.Presentation.Website.Recruitment
{
    public partial class frmAppicantMarksDetails : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        HiddenField innboardMessage;
        private Boolean isSavePermission = false;
        private Boolean isDeletePermission = false;
        //**************************** Handlers ****************************//
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)this.Master.FindControl("InnboardMessageHiddenField");
            if (!IsPostBack)
            {
                LoadJobCircular();
                LoadDepartment();
                SetDefaulTime();
            }
        }
        protected void btnAppointmentLetterShow_Click(object sender, EventArgs e)
        {
            int jobId = 0;
            //string ss = txtArrivalHour.Text;
            string dates = txtReportingDate.Text;
            //string mins = txtArrivalMin.Text;

            string ReportTime = (txtArrivalHour.Text.Replace("AM", "")).Replace("PM", "");
            string[] RTime = ReportTime.Split(':');
            int ss = Convert.ToInt32(RTime[0]);
            int mins = Convert.ToInt32(RTime[1]);

            if (ddlJobCircular.SelectedIndex != 0)
            {
                jobId = Convert.ToInt32(ddlJobCircular.SelectedValue);
            }

            List<int> IdList = new List<int>();
            List<Applicant> appIds = new List<Applicant>();
            appIds = JsonConvert.DeserializeObject<List<Applicant>>(hfApplicantIds.Value);

            //ArrayList list = new ArrayList();
            //list.Add("100");
            //list.Add("101");

            string ids = string.Empty;

            foreach (Applicant sss in appIds)
            {
                IdList.Add(sss.ApplicantId);
                ids = ids + "" + sss.ApplicantId + ",";
            }

            var l = ids.Length;
            ids = ids.TrimEnd(ids[l - 1]);

            EmployeeDA empDA = new EmployeeDA();
            bool status = empDA.UpdateApplicantAsEmployee(IdList);

            string url = "/Recruitment/Reports/frmAppointmentLetter.aspx?appId=" + ids + "&jobId=" + jobId + "&apDt=" + dates + "&rtm=" + ss + ":" + mins;
            string s = "window.open('" + url + "', 'popup_window', 'width=750,height=680,left=300,top=50,resizable=yes');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }
        //**************************** User Defined Method ****************************//
        private void ClearForm()
        {
            hfJobCircularId.Value = string.Empty;
        }
        private void LoadJobCircular()
        {
            JobCircularNRecruitmentDA entityDA = new JobCircularNRecruitmentDA();
            List<PayrollJobCircularBO> jobCircularlst = new List<PayrollJobCircularBO>();

            jobCircularlst = entityDA.GetJobCircularByDate(DateTime.Now);

            this.ddlJobCircular.DataSource = jobCircularlst;
            this.ddlJobCircular.DataTextField = "JobTitle";
            this.ddlJobCircular.DataValueField = "JobCircularId";
            this.ddlJobCircular.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlJobCircular.Items.Insert(0, item);
        }
        private void LoadDepartment()
        {
            DepartmentDA entityDA = new DepartmentDA();
            this.ddlDepartments.DataSource = entityDA.GetDepartmentInfo();
            this.ddlDepartments.DataTextField = "Name";
            this.ddlDepartments.DataValueField = "DepartmentId";
            this.ddlDepartments.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            this.ddlDepartments.Items.Insert(0, item);
        }
        private void SetDefaulTime()
        {
            this.txtArrivalHour.Text = "10:00";
        }
        //************************ User Defined WebMethod ********************//
        [WebMethod]
        public static List<PayrollJobCircularBO> GetApplicantMarks(int jobCircularId, int departmentId)
        {
            List<PayrollJobCircularBO> jobCircularlst = new List<PayrollJobCircularBO>();
            JobCircularNRecruitmentDA jobCircularDa = new JobCircularNRecruitmentDA();
            jobCircularlst = jobCircularDa.GetApplicantNResult(departmentId, jobCircularId);

            return jobCircularlst;
        }
        public class Applicant
        {
            public int ApplicantId { get; set; }
        }
    }
}