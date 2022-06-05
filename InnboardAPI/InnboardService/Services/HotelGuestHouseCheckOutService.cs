using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardService.Services
{
    public class HotelGuestHouseCheckOutService:GenericService<HotelGuestHouseCheckOut>
    {
        public new virtual Response<HotelGuestHouseCheckOut> Save(HotelGuestHouseCheckOut entity)
        {
            var repository = GetInstance<IHotelGuestHouseCheckOut>();
            var result = SafeExecute(() => repository.Save(entity));
            return result;
        }
    }
}
