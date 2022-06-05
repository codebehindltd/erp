using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.TaskManagment;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.TaskManagement;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.TaskManagement
{
    public partial class GanttChartInformationForProject : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGLCompany(false);
                //LoadGLProject(false);


            }
        }

        private void LoadGLCompany(bool isSingle)
        {
            GLCompanyDA entityDA = new GLCompanyDA();
            var List = entityDA.GetAllGLCompanyInfo();
            List<GLCompanyBO> companyList = new List<GLCompanyBO>();

            //hfCompanyAll.Value = JsonConvert.SerializeObject(List);

            if (isSingle == true)
            {
                companyList.Add(List[0]);
                ddlGLCompany.DataSource = companyList;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
            }
            else
            {
                ddlGLCompany.DataSource = List;
                ddlGLCompany.DataTextField = "Name";
                ddlGLCompany.DataValueField = "CompanyId";
                ddlGLCompany.DataBind();
                ListItem itemCompany = new ListItem();
                itemCompany.Value = "0";
                itemCompany.Text = hmUtility.GetDropDownFirstValue();
                ddlGLCompany.Items.Insert(0, itemCompany);
            }
        }

        [WebMethod]
        public static List<SMTask> LoadTaskByProjectId(int projetcId)
        {
            List<SMTask> taskList = new List<SMTask>();
            AssignTaskDA taskDA = new AssignTaskDA();

            taskList = taskDA.GetAllTaskByProjectId(projetcId);
            return taskList;
        }

        [WebMethod]
        public static List<GLProjectBO> LoadProjectByCompanyId(int companyId)
        {
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            GLProjectDA entityDA = new GLProjectDA();
            List<GLProjectBO> list = new List<GLProjectBO>();
            list = entityDA.GetGLProjectInfoByGLCompanyNUserGroup(Convert.ToInt32(companyId), Convert.ToInt32(userInformationBO.UserGroupId));
            return list;
        }
        [WebMethod]
        public static SMTask GetTaskId(long id)
        {
            SMTask stage = new SMTask();
            AssignTaskDA taskDA = new AssignTaskDA();

            stage = taskDA.GetTaskById(id);
            
            return stage;
        }
    }
}