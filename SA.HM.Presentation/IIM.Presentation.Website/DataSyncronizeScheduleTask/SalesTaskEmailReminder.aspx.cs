using HotelManagement.Data.HMCommon;
using HotelManagement.Data.TaskManagment;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Presentation.Website.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HotelManagement.Presentation.Website.DataSyncronizeScheduleTask
{
    public partial class SalesTaskEmailReminder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SendMail();
        }

        private static bool SendMail()
        {
            Email email;
            bool status = false;

            CompanyDA companyInfoDA = new CompanyDA();
            List<CompanyBO> list = companyInfoDA.GetCompanyInfo();
            string comapnyName = "", companyEmailAddress = "", companyContactNo = "";
            if (list != null)
            {
                if (list.Count > 0)
                {
                    comapnyName = list[0].CompanyName;
                    companyEmailAddress = list[0].EmailAddress;
                    companyContactNo = list[0].ContactNumber;
                }
            }

            HMCommonSetupBO commonSetupBO = new HMCommonSetupBO();
            HMCommonSetupDA commonSetupDA = new HMCommonSetupDA();

            commonSetupBO = commonSetupDA.GetCommonConfigurationInfo("SendEmailAddress", "SendEmailConfiguration");
            string mainString = commonSetupBO.SetupValue;

            if (!string.IsNullOrEmpty(mainString))
            {
                string[] dataArray = mainString.Split('~');
               

                AssignTaskDA taskDA = new AssignTaskDA();
                List<SMTaskAssignmentView> taskList = new List<SMTaskAssignmentView>();
                taskList = taskDA.GetTaskForEmailReminder();

                foreach (var task in taskList)
                {
                    email = new Email()
                    {
                        From = dataArray[0],
                        Password = dataArray[1],
                        To = task.AssignToEmailAddress,
                        Subject = "Task Reminder",
                        Host = dataArray[2],
                        Port = dataArray[3],
                        TempleteName = HMConstants.EmailTemplates.SalesTaskReminderTemplate
                    };
                    var tokens = new Dictionary<string, string>
                         {
                            {"NAME", task.AssignToName},
                            {"ASSIGNEENAME",task.AssigneeName },
                            {"DESCRIPTION",task.Description },
                            {"COMPANYNAME",comapnyName},
                            {"COMPANYCONTACTNO",companyContactNo },
                            {"COMPANYEMAILADDRESS",companyEmailAddress }
                };
                    try
                    {
                        status = EmailHelper.SendEmail(email, tokens);
                        if(status)
                        {
                            taskDA.UpdateEmailSentInformation(task.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return status;
        }
    }
}