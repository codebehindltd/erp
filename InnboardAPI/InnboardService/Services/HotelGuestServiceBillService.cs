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
    public class HotelGuestServiceBillService:GenericService<HotelGuestServiceBill>
    {
        public new virtual Response<HotelGuestServiceBill> Save(HotelGuestServiceBill entity)
        {
            var repository = GetInstance<IHotelGuestServiceBill>();
            var result = SafeExecute(() => repository.Save(entity));
            return result;
        }
        public virtual Response<ServiceBillDataSync> Sync(ServiceBillDataSync entity)
        {
            var repository = GetInstance<IHotelGuestServiceBill>();
            var result = SafeExecute(() => repository.Sync(entity));
            return result;
        }

        public Response<HotelGuestServiceBill> GetByGuiId(Guid? id)
        {
            var repository = GetInstance<IHotelGuestServiceBill>();
            var result = SafeExecute(() => repository.GetByGuiId(id));
            return result;
        }
    }
}
