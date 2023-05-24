using InnboardDataAccess.DataAccesses;
using InnboardDomain.CriteriaDtoModel;
using InnboardDomain.Models;
using InnboardDomain.Models.Payroll;
using InnboardDomain.ViewModel;
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
    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiController
    {
        // GET api/Employee
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("EmpLocationTrackingSave")]
        public async Task<HttpResponseMessage> EmpLocationTrackingSave(PayrollEmpTracking model)
        {

            try
            {            
                EmployeeDataAccess db = new EmployeeDataAccess();            
                model.GoogleMapUrl = "https://www.google.com/maps/place/" + model.Latitude + "+" + model.Longitude;
                bool isSuccess = await db.EmpLocationTrackingSave(model);
                if (isSuccess)
                {
                    var responseMsg = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                    {
                        Content = new StringContent("Succesfully Recorded")
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

        [HttpGet]
        [AllowAnonymous]
        [Route("GetEmpTrackingList")]
        public async Task<IHttpActionResult> GetEmpTrackingList(int? EmpId, DateTime FromDate, DateTime ToDate)
        {
            EmployeeDataAccess db = new EmployeeDataAccess();            
            var result = await db.GetEmpTrackingList(EmpId, FromDate, ToDate);
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetPayrollEmpListWithLocation")]
        public async Task<IHttpActionResult> GetPayrollEmpListWithLocation([FromUri] PayrollEmpCriteriaDto criteriaDto)
        {
            EmployeeDataAccess db = new EmployeeDataAccess();
            var result = await db.GetPayrollEmpListWithLocation(criteriaDto);
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetLeaveType")]
        public async Task<IHttpActionResult> GetLeaveType(int? EmpId, DateTime CurrentDate)
        {
            EmployeeDataAccess db = new EmployeeDataAccess();
            var result = await db.GetLeaveType(EmpId, CurrentDate);
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetActiveEmployees")]
        public async Task<IHttpActionResult> GetActiveEmployees()
        {
            EmployeeDataAccess db = new EmployeeDataAccess();
            var result = await db.GetActiveEmployees();
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("LeaveApplication")]
        public async Task<HttpResponseMessage> LeaveApplication([FromBody] LeaveInformationBO model)
        {
            try
            {
                EmployeeDataAccess db = new EmployeeDataAccess();
                var isSuccess = db.LeaveApplication(model);

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
