using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using InnboardDomain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardService.Services
{
    public class RestaurantBillService: GenericService<RestaurantBill>
    {
        public new Response<RestaurantBill> Save(RestaurantBill entity)
        {
            var repository = GetInstance<IRestaurantBill>();
            var result = SafeExecute(() => repository.Save(entity));
            return result;
        }

        public Response<RestaurantDataSync> Sync(RestaurantDataSync entity)
        {
            var repository = GetInstance<IRestaurantBill>();
            var result = SafeExecute(() => repository.Sync(entity));
            return result;
        }

        public Response<RestaurantBill> GetByGuiId(Guid? id)
        {
            var repository = GetInstance<IRestaurantBill>();
            var result = SafeExecute(() => repository.GetByGuiId(id));
            return result;
        }
    }
}
