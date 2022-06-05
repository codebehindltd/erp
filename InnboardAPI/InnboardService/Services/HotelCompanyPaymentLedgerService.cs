using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardService.Services
{
    public class HotelCompanyPaymentLedgerService : GenericService<HotelCompanyPaymentLedger>
    {
        public new virtual Response<HotelCompanyPaymentLedger> Save(HotelCompanyPaymentLedger entity)
        {
            var repository = GetInstance<IHotelCompanyPaymentLedger>();
            var result = SafeExecute(() => repository.Save(entity));
            return result;
        }
    }
}
