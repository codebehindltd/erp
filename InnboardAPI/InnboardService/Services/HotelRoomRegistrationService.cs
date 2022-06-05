using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using InnboardDomain.ViewModel;
using InnboardDomain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardService.Services
{
    public class HotelRoomRegistrationService : GenericService<HotelRoomRegistration>
    {
        public new Response<HotelRoomRegistration> Save(HotelRoomRegistration entity)
        {
            var repository = GetInstance<IHotelRoomRegistration>();
            var result = SafeExecute(() => repository.Save(entity));
            return result;
        }
        public Response<RegistrationDataSync> Sync(RegistrationDataSync entity)
        {
            var repository = GetInstance<IHotelRoomRegistration>();
            var result = SafeExecute(() => repository.SyncRoomRegistrationData(entity));
            return result;
        }
        public Response<HotelRoomRegistration> GetByGuiId(Guid? id)
        {
            var repository = GetInstance<IHotelRoomRegistration>();
            var result = SafeExecute(() => repository.GetByGuiId(id));
            return result;
        }
        public Response<int> UpdateRegistration(HotelRoomRegistration entity)
        {
            var repository = GetInstance<IHotelRoomRegistration>();
            var result = SafeExecute(() => repository.UpdateRegistration(entity));
            return result;
        }        
        
    }
}
