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
                //SmsHelper.SendSmsSingle("SmartLabSMS", memberBasicInfo.MobileNumber);

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
