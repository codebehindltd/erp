using InnboardDomain.Models;
using InnboardDomain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Interfaces
{
    public interface IRestaurantBill : IGenericRepository<RestaurantBill>
    { 
        RestaurantBill GetByGuiId(Guid? Id);
        RestaurantDataSync Sync(RestaurantDataSync restaurant);
    }
}
