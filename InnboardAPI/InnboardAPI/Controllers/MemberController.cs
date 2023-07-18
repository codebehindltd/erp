using InnboardDataAccess.DataAccesses;
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

    }
}
