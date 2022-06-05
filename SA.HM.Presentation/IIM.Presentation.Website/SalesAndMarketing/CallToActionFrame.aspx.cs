using HotelManagement.Data.HMCommon;
using HotelManagement.Data.HotelManagement;
using HotelManagement.Data.SalesAndMarketing;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.HotelManagement;
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
    public partial class CallToActionFrame : System.Web.UI.Page
    {
        HiddenField innboardMessage;
        HMUtility hmUtility = new HMUtility();
        protected void Page_Load(object sender, EventArgs e)
        {
            innboardMessage = (HiddenField)Master.FindControl("InnboardMessageHiddenField");
        }
        [WebMethod]
        public static List<GuestCompanyBO> GetCompanyByAutoSearch(string searchTerm)
        {
            List<GuestCompanyBO> companyInfo = new List<GuestCompanyBO>();
            GuestCompanyDA itemDa = new GuestCompanyDA();
            companyInfo = itemDa.GetGuestCompanyInfoByCompanyName(searchTerm);

            return companyInfo;
        }
        [WebMethod]
        public static List<ContactInformationBO> LoadLabelByAutoSearch(string searchTerm)
        {
            ContactInformationDA DA = new ContactInformationDA();
            List<ContactInformationBO> titleList = new List<ContactInformationBO>();
            titleList = DA.GetContactByAutoSearch(searchTerm);
            return titleList;
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
        public static CallToActionViewBO GetCallToActionById(int Id)
        {
            CallToActionViewBO CallToActionView = new CallToActionViewBO();
            CallToActionDA DA = new CallToActionDA();
            CallToActionView.CallToAction = DA.GetCallToActionId(Id);
            CallToActionView.CallToActionDetailList = DA.GetGetCallToActionDetailsByCallToActionId(CallToActionView.CallToAction.Id);

            return CallToActionView;
        }
        [WebMethod]
        public static ReturnInfo SaveOrUpdateCallToAction(CallToActionBO CallToAction, List<CallToActionDetailBO> CallToActionDetails, List<CallToActionDetailBO> CallToActionDetailsDeleted)
        {
            ReturnInfo info = new ReturnInfo();
            long outId;
            bool status = false;
            HMUtility hmUtility = new HMUtility();
            HMCommonDA hmCommonDA = new HMCommonDA();
            UserInformationBO userInformationBO = new UserInformationBO();
            userInformationBO = hmUtility.GetCurrentApplicationUserInfo();

            CallToAction.CreatedBy = userInformationBO.UserInfoId;

            CallToActionDA DA = new CallToActionDA();
            status = DA.SaveSalesCallInfo(CallToAction, CallToActionDetails, CallToActionDetailsDeleted, out outId);
            if (status)
            {
                info.IsSuccess = true;
                if (CallToAction.Id == 0)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Add.ToString(), EntityTypeEnum.EntityType.CallToAction.ToString(), outId, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CallToAction));
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                    info.Data = 0;
                }
                else
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Edit.ToString(), EntityTypeEnum.EntityType.CallToAction.ToString(), CallToAction.Id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.CallToAction));
                    info.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Update, AlertType.Success);
                }
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