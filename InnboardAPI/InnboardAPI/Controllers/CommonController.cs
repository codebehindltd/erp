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
