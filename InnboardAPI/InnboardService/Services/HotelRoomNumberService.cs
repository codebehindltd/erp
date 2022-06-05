using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardService.Services
{
    public class HotelRoomNumberService:GenericService<HotelRoomNumber>
    {
        public Response<int> UpdateRoomStatus(HotelRoomNumber entity)
        {
            var repository = GetInstance<IHotelRoomNumber>();
            var result = SafeExecute(() => repository.UpdateRoomStatus(entity));
            return result;
        }

        public virtual Response<List<HotelRoomNumber>> TruncateAllAndInsert(List<HotelRoomNumber> entity)
        {
            var repository = GetInstance<IHotelRoomNumber>();
            var result = SafeExecute(() => repository.TruncateAllAndInsert(entity));
            return result;
        }
    }
}
