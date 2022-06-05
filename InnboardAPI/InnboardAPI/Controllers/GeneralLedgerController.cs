using InnboardDomain.Models;
using InnboardService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InnboardAPI.Controllers
{
    [RoutePrefix("api/GeneralLedger")]   
    public class GeneralLedgerController : ApiController
    {
        GLLedgerMasterService _ledgerService;
        public GeneralLedgerController()
        {
            _ledgerService = new GLLedgerMasterService();
        }
        [Route("Sync")]
        public IHttpActionResult Sync(GLLedgerMaster ledger)
        {
            if (ModelState.IsValid)
            {
                GenericService<GLLedgerMaster> converisonService = new GenericService<GLLedgerMaster>();

                var response = converisonService.ConvertDateTimeUTCtoLocalTime(ledger);
                if (response.Success)
                    response = _ledgerService.Save(ledger);

                return Json(new { response.Success, response.Data.VoucherNo });
            }
            else
                return Json(new { Success = false, ErrorMessage = "Model is not in valid format" });
        }
    }
}
