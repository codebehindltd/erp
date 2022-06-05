using HotelManagement.Data.HMCommon;
using HotelManagement.Data.Inventory;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Inventory;
using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.UserInformation;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.SalesAndMarketing
{
    public partial class SiteSurveyFeedback : System.Web.UI.Page
    {
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEngineer();
            }
        }
        protected void LoadEngineer()
        {
            List<EmployeeBO> employees = new List<EmployeeBO>();

            DepartmentDA DA = new DepartmentDA();
            employees = DA.GetEmployeeOfTechnicalDepartment();
            ddlEngineerName.DataSource = employees;
            ddlEngineerName.DataTextField = "DisplayName";
            ddlEngineerName.DataValueField = "EmpId";
            ddlEngineerName.DataBind();
            ListItem listItem = new ListItem();
            listItem.Value = "0";
            listItem.Text = hmUtility.GetDropDownFirstAllValue();
            ddlEngineerName.Items.Insert(0, listItem);
        }

        [WebMethod]
        public static int CheckFeedback(string siteSurveyId)
        {
            int HasFeedback = 0;
            SiteSurveyNoteDA siteSurveyNoteDA = new SiteSurveyNoteDA();
            HasFeedback = siteSurveyNoteDA.CheckFeedback(Convert.ToInt32(siteSurveyId));
            return HasFeedback;
        }
        [WebMethod]
        public static List<InvItemAutoSearchBO> ItemSearch(string searchTerm)
        {
            List<InvItemAutoSearchBO> itemInfo = new List<InvItemAutoSearchBO>();

            InvItemDA itemDa = new InvItemDA();
            itemInfo = itemDa.GetItemSearchQuatation(searchTerm);
            return itemInfo;
        }
        [WebMethod]
        public static int GetSiteSurveyNoteId(string companyId, string dealId)
        {
            int SiteSurveyNoteId = 0;
            SiteSurveyNoteDA DA = new SiteSurveyNoteDA();
            SiteSurveyNoteId = DA.GetSiteSurveyNoteId(Convert.ToInt32(companyId), Convert.ToInt32(dealId));
            return SiteSurveyNoteId;
        }
        [WebMethod]
        public static ReturnInfo SaveFeedback(int id, int siteSurveyId, string Feedback, string EmpId, List<SMSiteSurveyFeedbackDetailsBO> AddedItem, List<SMSiteSurveyFeedbackDetailsBO> EditedItem, List<SMSiteSurveyFeedbackDetailsBO> DeletedItem)
        {
            ReturnInfo rtninfo = new ReturnInfo();
            SiteSurveyNoteDA siteSurveyNoteDA = new SiteSurveyNoteDA();
            HMUtility hmUtility = new HMUtility();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();
            DepartmentDA DA = new DepartmentDA();
            List<EmployeeBO> employees = new List<EmployeeBO>();
            if (EmpId == "0")
            {
                EmpId = "";
                employees = DA.GetEmployeeOfTechnicalDepartment();
                foreach (var item in employees)
                {
                    if (EmpId == "")
                    {
                        EmpId += Convert.ToString(item.EmpId);
                    }
                    else
                    {
                        EmpId += ",";
                        EmpId += Convert.ToString(item.EmpId);
                    }
                }
            }
            SMSiteSurveyFeedbackBO sMSiteSurvey = new SMSiteSurveyFeedbackBO();
            sMSiteSurvey.Id = id;
            sMSiteSurvey.SiteSurveyNoteId = siteSurveyId;
            sMSiteSurvey.SurveyFeedback = Feedback;
            sMSiteSurvey.CreatedBy = userInformationBO.UserInfoId;
            long OutId;
            var status = siteSurveyNoteDA.SaveUpdateSiteSurveyFeedback(sMSiteSurvey, EmpId, AddedItem, EditedItem, DeletedItem, out OutId);
            if (status)
            {
                if (id == 0)
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);

                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(),
                            EntityTypeEnum.EntityType.WorkingPlan.ToString(), OutId,
                            ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                            hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.WorkingPlan));

                }
                else
                {

                    rtninfo.IsSuccess = true;
                    rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);

                    bool logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(),
                           EntityTypeEnum.EntityType.SalaryHead.ToString(), OutId,
                           ProjectModuleEnum.ProjectModule.PayrollManagement.ToString(),
                           hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SalaryHead));
                }


            }
            else
            {
                rtninfo.IsSuccess = false;
                rtninfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);

            }
            return rtninfo;
        }
        [WebMethod]
        public static SMSiteSurveyFeedbackViewBO LoadFeedbackDetailsById(int feedbackId)
        {
            SMSiteSurveyFeedbackViewBO feedbackViewBOs = new SMSiteSurveyFeedbackViewBO();
            SiteSurveyNoteDA siteSurveyNoteDA = new SiteSurveyNoteDA();
            feedbackViewBOs.SMSiteSurveyFeedbackBO = siteSurveyNoteDA.GetSiteSurveyFeedbackByFeedbackId(feedbackId);
            feedbackViewBOs.SMSiteSurveyFeedbackDetailsBOList = siteSurveyNoteDA.GetSiteSurveyFeedbackDetailsByFeedbackId(feedbackId);
            feedbackViewBOs.SMSiteSurveyEngineerBOList = siteSurveyNoteDA.GetSiteSurveyEngineerByFeedbackId(feedbackId);
            return feedbackViewBOs;
        }
    }
}