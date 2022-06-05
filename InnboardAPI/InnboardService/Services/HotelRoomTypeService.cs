using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardService.Services
{
    public class HotelRoomTypeService:GenericService<HotelRoomType>
    {
        public new Response<List<HotelRoomType>> TruncateAllAndInsert(List<HotelRoomType> entity)
        {
            var repository = GetInstance<IHotelRoomType>();
            var result = SafeExecute(() => repository.TruncateAllAndInsert(entity));
            return result;
        }
    }
}
