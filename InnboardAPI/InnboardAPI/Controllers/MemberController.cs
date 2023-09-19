using InnboardDataAccess.DataAccesses;
using InnboardDataAccess.SMSGetway;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using InnboardAPI.Models;
using InnboardDomain.Common;

namespace InnboardAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Member")]
    public class MemberController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("GetMembershipSetupData")]
        public async Task<IHttpActionResult> GetMembershipSetupData()
        {
            MemberDataAccess db = new MemberDataAccess();
            var result = await db.GetMembershipSetupData();
            return Ok(result);
        }

        // Member By Mobile Apps Registration
        [HttpPost]
        [AllowAnonymous]
        [Route("SaveMemMemberBasicInfoForMobileAppsRegistration")]
        public async Task<HttpResponseMessage> SaveMemMemberBasicInfoForMobileAppsRegistration([FromBody] MemMemberBasics memberBasicInfo)
        {
            int tmpMemberId = 0;
            MemberDataAccess dbLogin = new MemberDataAccess();

            bool isSuccess = dbLogin.SaveMemMemberBasicInfoForMobileAppsRegistration(memberBasicInfo, out tmpMemberId);
            if (isSuccess)
            {
                CommonDataAccess db = new CommonDataAccess();
                var commonSetupSMSAutoPostingBO = await db.GetCommonConfigurationInfo("SMSAutoPosting", "IsMemberRegistrationSMSAutoPostingEnable");
                if (commonSetupSMSAutoPostingBO.SetupValue == "1")
                {

                    CommonDataAccess dbCommonDataAccess = new CommonDataAccess();
                    var companyInfo = await dbCommonDataAccess.GetCompanyInfo();

                    var commonSetupBO = await db.GetCommonConfigurationInfo("SendSMS", "SendSMSConfiguration");
                    string commonSetupBODescription = commonSetupBO.Description;
                    string[] dataArray = commonSetupBODescription.Split('~');
                    var smsGetway = dataArray[0];

                    //send msg 
                    SMSView sms = new SMSView
                    {
                        TempleteName = HMConstants.SMSTemplates.MemberRegistrationSms
                    };
                    var singletoken = new Dictionary<string, string>
                        {
                        {"COMPANY", companyInfo[0].CompanyName},
                        {"COMPANYADDRESS", companyInfo[0].CompanyAddress},
                        {"CONTACTNUMBER", companyInfo[0].ContactNumber},
                        {"Name", memberBasicInfo.FullName}
                        };
                    SmsHelper.SendSmsSingle(sms, singletoken, smsGetway, memberBasicInfo.MobileNumber, commonSetupBODescription);
                    // send msg end
                }

                var responseMsg = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(tmpMemberId.ToString())
                };
                return responseMsg;
            }
            else
            {
                var responseMsg = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Bad Request")
                };
                return responseMsg;
            }
        }

        // Member Payment By Mobile Apps Registration
        [HttpPost]
        [AllowAnonymous]
        [Route("SaveMemberPaymentInfoForMobileAppsRegistration")]
        public async Task<HttpResponseMessage> SaveMemberPaymentInfoForMobileAppsRegistration([FromBody] MemMemberBasics memberBasicInfo)
        {
            MemberDataAccess dbLogin = new MemberDataAccess();

            bool isSuccess = dbLogin.SaveMemberPaymentInfoForMobileAppsRegistration(memberBasicInfo);
            if (isSuccess)
            {
                var responseMsg = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent("Succesfully Payment Posted.")
                };
                return responseMsg;
            }
            else
            {
                var responseMsg = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Bad Request")
                };
                return responseMsg;
            }
        }

        // Member Profile Update By Mobile Apps Registration
        [HttpPost]
        [AllowAnonymous]
        [Route("UpdateMemMemberBasicInfoForMobileApps")]
        public async Task<HttpResponseMessage> UpdateMemMemberBasicInfoForMobileApps([FromBody] MemMemberBasics memberBasicInfo)
        {
            int tmpMemberId = 0;
            MemberDataAccess dbLogin = new MemberDataAccess();

            if (memberBasicInfo.MemberAppsProfilePictureByte != null)
            {
                memberBasicInfo.MemberAppsProfilePicture = UtilityMethods.UploadByteFile(memberBasicInfo.MemberAppsProfilePicture, "GuestOrMemberProfilePicture", memberBasicInfo.MemberAppsProfilePictureByte);
            }
            
            bool isSuccess = dbLogin.UpdateMemMemberBasicInfoForMobileApps(memberBasicInfo);
            if (isSuccess)
            {
                var responseMsg = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(tmpMemberId.ToString())
                };
                return responseMsg;
            }
            else
            {
                var responseMsg = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Bad Request")
                };
                return responseMsg;
            }
        }
    }
}
