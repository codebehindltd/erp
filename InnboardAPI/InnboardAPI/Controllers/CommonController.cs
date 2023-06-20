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
    [RoutePrefix("api/Common")]
    public class CommonController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("GetCustomField")]
        public async Task<IHttpActionResult> GetCustomField([FromUri] string fieldType)
        {
            CommonDataAccess db= new CommonDataAccess();
            var result= await db.GetCustomField(fieldType);
            return Ok(result);
        }

    }
}
