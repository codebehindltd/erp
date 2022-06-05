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
    public class HotelRoomReservationOnlineService:GenericService<HotelRoomReservationOnline>
    {
        public Response<HotelRoomReservationOnline> Save(HotelRoomReservationOnlineView entity)
        {
            var repository = GetInstance<IHotelRoomReservationOnline>();
            var result = SafeExecute(() => repository.Save(entity));
            return result;
        }
    }
}
