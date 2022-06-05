using HotelManagement.Data.HMCommon;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
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
    public partial class CallToActionDetailsEditIFrame : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
        }
        [WebMethod]
        public static CallToActionDetailViewBO GetCallToActionDetailById(int Id)
        {
            CallToActionDetailViewBO CallToActionView = new CallToActionDetailViewBO();
            CallToActionDA DA = new CallToActionDA();
            CallToActionView = DA.GetCallToActionDetailsById(Id);
            if (!(String.IsNullOrEmpty(CallToActionView.PerticipentFromOffice)))
            {
                CallToActionView.PerticipentFromOfficeList = CallToActionView.PerticipentFromOffice.Split(',').Select(t => long.Parse(t)).ToList();
            }
            if (!(String.IsNullOrEmpty(CallToActionView.PerticipentFromClient)))
            {
                CallToActionView.PerticipentFromClientList = CallToActionView.PerticipentFromClient.Split(',').Select(t => long.Parse(t)).ToList();
            }
            if (!(String.IsNullOrEmpty(CallToActionView.ReminderDayList)))
            {
                CallToActionView.ReminderList = CallToActionView.ReminderDayList.Split(',').Select(t => long.Parse(t)).ToList();
            }
            if (!(String.IsNullOrEmpty(CallToActionView.TaskAssignedEmployee)))
            {
                CallToActionView.TaskAssignedEmployeetList = CallToActionView.TaskAssignedEmployee.Split(',').Select(t => long.Parse(t)).ToList();
            }

            return CallToActionView;
        }

        [WebMethod]
        public static List<ContactInformationBO> GetContactInformationByCompanyId(int companyId)
        {
            List<ContactInformationBO> contactInformation = new List<ContactInformationBO>();
            ContactInformationDA DA = new ContactInformationDA();
            contactInformation = DA.GetContactInformationByCompanyId(companyId);

            return contactInformation;
        }

        [WebMethod]
        public static ReturnInfo SaveOrUpdateCallToActionDetails(CallToActionDetailBO CallToActionDetails)
        {
            ReturnInfo info = new ReturnInfo();
            long outId;
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CallToActionDA DA = new CallToActionDA();
            status = DA.UpdateCallToActionDetails( CallToActionDetails, out outId);
            if (status)
            {
                info.IsSuccess = true;
                
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.CallToAction.ToString(), Convert.ToInt32(CallToActionDetails.CallToActionId), ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CallToAction));
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                
            }
            else
            {
                info.IsSuccess = false;
                info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return info;
        }

    }
}