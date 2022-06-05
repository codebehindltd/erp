using HotelManagement.Data.GeneralLedger;
using HotelManagement.Data.HMCommon;
using HotelManagement.Data.TaskManagment;
using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.SalesAndMarketing;
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
    public partial class GanttChartInformation : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                companyProjectUserControl.ddlFirstValueVar = "select";

                //LoadGLProject(false);
                LoadQuotationNo();
                LoadTaskTypeFor();

            }
        }
        private void LoadTaskTypeFor()
        {
            HMCommonDA commonDA = new HMCommonDA();
            List<CustomFieldBO> fields = new List<CustomFieldBO>();
            fields = commonDA.GetCustomField("GanttChart");

            ddlTaskType.DataSource = fields;
            ddlTaskType.DataTextField = "Description";
            ddlTaskType.DataValueField = "FieldValue";
            ddlTaskType.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlTaskType.Items.Insert(0, item);

        }




        [WebMethod]
        public static List<SMTask> LoadTaskByProjectId(int Id,string Type)
        {
            List<SMTask> taskList = new List<SMTask>();
            AssignTaskDA taskDA = new AssignTaskDA();

            if(Type == "Employee")
            {
                taskList = taskDA.GetAllTaskByEmployeeId(Id);
            }
            else
            {
                taskList = taskDA.GetAllTaskByProjectIdAndType(Id, Type);
            }

           
            return taskList;
        }

        private void LoadQuotationNo()
        {
            AssignTaskDA DA = new AssignTaskDA();
            List<SMQuotationBO> SourceBO = new List<SMQuotationBO>();
            SourceBO = DA.GetQuotationInformationForTaskType();

            ddlSaleNo.DataSource = SourceBO;
            ddlSaleNo.DataTextField = "QuotationNo";
            ddlSaleNo.DataValueField = "QuotationId";
            ddlSaleNo.DataBind();

            ListItem item = new ListItem();
            item.Value = "0";
            item.Text = hmUtility.GetDropDownFirstValue();
            ddlSaleNo.Items.Insert(0, item);

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
    }
}