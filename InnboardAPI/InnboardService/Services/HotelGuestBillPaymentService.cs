using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardService.Services
{
    public class HotelGuestBillPaymentService:GenericService<HotelGuestBillPayment>
    {
        public new virtual Response<HotelGuestBillPayment> Save(HotelGuestBillPayment entity)
        {
            var repository = GetInstance<IHotelGuestBillPayment>();
            var result = SafeExecute(() => repository.Save(entity));
            return result;
        }
    }
}
