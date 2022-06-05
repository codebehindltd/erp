using InnboardDomain.Models;
using InnboardDomain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDomain.Interfaces
{
    public interface IHotelRoomRegistration:IGenericRepository<HotelRoomRegistration>
    {
        HotelRoomRegistration GetByGuiId(Guid? Id);
        int UpdateRegistration(HotelRoomRegistration room);
        RegistrationDataSync SyncRoomRegistrationData(RegistrationDataSync room);
    }
}
