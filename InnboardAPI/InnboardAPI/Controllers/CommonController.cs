using InnboardDataAccess.DataAccesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Util;

namespace InnboardAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Common")]
    public class CommonController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("GetCompanyInfo")]
        public async Task<IHttpActionResult> GetCompanyInfo()
        {
            CommonDataAccess db = new CommonDataAccess();
            var result = await db.GetCompanyInfo();
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetCommonConfigurationInfo")]
        public async Task<IHttpActionResult> GetCommonConfigurationInfo(string typeName, string setupName)
        {
            CommonDataAccess db = new CommonDataAccess();
            var result = await db.GetCommonConfigurationInfo(typeName, setupName);
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetCustomField")]
        public async Task<IHttpActionResult> GetCustomField([FromUri] string fieldType)
        {
            CommonDataAccess db= new CommonDataAccess();
            var result= await db.GetCustomField(fieldType);
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetPropertyInformation")]
        public async Task<IHttpActionResult> GetPropertyInformation(string transactionType, int transactionId)
        {
            CommonDataAccess db = new CommonDataAccess();
            var result = await db.GetPropertyInformation(transactionType, transactionId);
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetGuestOrMemberProfileInformation")]
        public async Task<IHttpActionResult> GetGuestOrMemberProfileInformation(string transactionType, int transactionId)
        {
            CommonDataAccess db = new CommonDataAccess();
            var result = await db.GetGuestOrMemberProfileInformation(transactionType, transactionId);
            if (result == null)
            {
                return BadRequest("Data not found");
            }
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetMemberRoomNightInformation")]
        public async Task<IHttpActionResult> GetMemberRoomNightInformation(int transactionId)
        {
            try
            {
                CommonDataAccess db = new CommonDataAccess();
                var result = await db.GetMemberRoomNightInformation(transactionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetMemberRoomNightPromotionalOffer")]
        public async Task<IHttpActionResult> GetMemberRoomNightPromotionalOffer(int transactionId)
        {
            CommonDataAccess db = new CommonDataAccess();
            var result = await db.GetMemberRoomNightPromotionalOffer(transactionId);
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetGuestOrMemberPromotionalOffer")]
        public async Task<IHttpActionResult> GetGuestOrMemberPromotionalOffer(string transactionType, int transactionId)
        {
            CommonDataAccess db = new CommonDataAccess();
            var result = await db.GetGuestOrMemberPromotionalOffer(transactionType, transactionId);
            return Ok(result);
        }
    }
}
