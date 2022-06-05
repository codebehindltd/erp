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
    public partial class DealInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            long dealId = Convert.ToInt64(Request.QueryString["did"]);
            hfDealId.Value = dealId.ToString();
            //FillForm(dealId);
            //FileUpload(dealId);
        }

        private void FileUpload(long dealId)
        {
            Session["DealId"] = dealId;
            string jscript = "function UploadComplete(){ };";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "LoadDetailGridInformation", jscript, true);
            //flashUpload.QueryParameters = "DealId=" + Server.UrlEncode(dealId.ToString());
        }
        [WebMethod]
        public static SMDealView FillForm(int id)
        {
            SMDealView deal = new SMDealView();
            if (id > 0)
            {
                DealDA dealDA = new DealDA();
                deal = dealDA.GetDealInfoForViewById(id);
            }
            return deal;
        }

        [WebMethod]
        public static ReturnInfo DeleteContact(long id)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo returnInfo = new ReturnInfo();
            HMCommonDA commonDA = new HMCommonDA();
            try
            {
                returnInfo.IsSuccess = commonDA.DeleteInfoById("SMDealWiseContactMap", "Id", id);

                if (returnInfo.IsSuccess)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.SMDealWiseContactMap.ToString(), id, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealWiseContactMap));
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Delete, AlertType.Success);
                }
                else
                {
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }

            catch (Exception ex)
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return returnInfo;
        }

        [WebMethod]
        public static ReturnInfo AddDealContact(List<SMDealWiseContactMap> contacts)
        {
            HMUtility hmUtility = new HMUtility();
            ReturnInfo returnInfo = new ReturnInfo();
            DealDA dealDA = new DealDA();
            try
            {
                returnInfo.IsSuccess = dealDA.SaveDealWiseContactMap(contacts);

                if (returnInfo.IsSuccess)
                {
                    Boolean logStatus = hmUtility.CreateActivityLogEntity(ActivityTypeEnum.ActivityType.Delete.ToString(), EntityTypeEnum.EntityType.SMDealWiseContactMap.ToString(), contacts[0].ContactId, ProjectModuleEnum.ProjectModule.SalesMarketingManagement.ToString(), hmUtility.GetEntityTypeEnumDescription(EntityTypeEnum.EntityType.SMDealWiseContactMap));
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Save, AlertType.Success);
                }
                else
                {
                    returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
                }
            }

            catch (Exception ex)
            {
                returnInfo.AlertMessage = CommonHelper.AlertInfo(AlertMessage.Error, AlertType.Error);
            }
            return returnInfo;
        }
        [WebMethod]
        public static List<ContactInformationBO> GetCompanyContacts(int companyId)
        {
            ContactInformationDA contactDA = new ContactInformationDA();
            List<ContactInformationBO> contacts = new List<ContactInformationBO>();

            contacts = contactDA.GetContactInformationByCompanyId(companyId);

            return contacts;
        }

    }
}