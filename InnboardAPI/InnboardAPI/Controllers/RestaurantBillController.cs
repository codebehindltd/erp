using InnboardDomain.ViewModel;
using InnboardService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InnboardAPI.Controllers
{
    [RoutePrefix("api/RestaurantBill")]
    public class RestaurantBillController : ApiController
    {
        private RestaurantBillService _billService;
        public RestaurantBillController()
        {
            _billService = new RestaurantBillService();
        }

        [Route("Sync")]
        public IHttpActionResult PostRestaurant(RestaurantDataSync restaurant)
        {
            if (ModelState.IsValid)
            {
                GenericService<RestaurantDataSync> _converisonService = new GenericService<RestaurantDataSync>();

                if (restaurant.Bill != null)
                {
                    var response = _converisonService.ConvertDateTimeUTCtoLocalTime(restaurant);
                    if (response.Success)
                        _billService.Sync(restaurant);
                    return Json(new { response.Success, restaurant.Bill.BillNumber });
                }
                else
                    return Json(new { Success = false });
            }
            else
                return Json(new { Success = false, ErrorMessage = "Model is not in valid format" });
        }
    }
}
