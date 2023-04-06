using InnboardDataAccess.DataAccesses;
using InnboardDomain.CriteriaDtoModel;
using InnboardDomain.Models;
using InnboardDomain.ViewModel;
using InnboardService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

        [HttpGet]
        [AllowAnonymous]
        [Route("GetVoucherBySearchCriteria")]
        public async Task<IHttpActionResult> GetVoucherBySearchCriteria([FromUri] GeneralLedgerCriteriaDto criteriaDto)
        {
            GLLedgerMasterDataAccess db = new GLLedgerMasterDataAccess();
            var result = await db.GetVoucherBySearchCriteria(criteriaDto);
            
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetVoucherInformation")]
        public async Task<IHttpActionResult> GetVoucherInformation(int userId, int ledgerMasterId)
        {
            GLLedgerMasterDataAccess db = new GLLedgerMasterDataAccess();
            var result = await db.GetVoucherInformation(userId, ledgerMasterId);

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("VoucherApproval")]
        public async Task<HttpResponseMessage> VoucherApproval([FromBody] GLLedgerMasterVwBO model)
        {
            try { 
                GLLedgerMasterDataAccess db = new GLLedgerMasterDataAccess();
                var isSuccess = db.VoucherApproval(model);

                if (isSuccess)
                {
                    var responseMsg = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                    {
                        Content = new StringContent("Succesfully Done")
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
             catch (Exception ex)
            {
                var responseMsg = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };
                return responseMsg;
            }
        }
    }
}
