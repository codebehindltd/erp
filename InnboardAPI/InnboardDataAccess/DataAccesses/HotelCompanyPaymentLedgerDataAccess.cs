using InnboardAPI.DataAccesses;
using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnboardDataAccess.DataAccesses
{
    public class HotelCompanyPaymentLedgerDataAccess : GenericDataAccess<HotelCompanyPaymentLedger>, IHotelCompanyPaymentLedger
    {
        public HotelCompanyPaymentLedgerDataAccess() 
        {
            
        }
        public new HotelCompanyPaymentLedger Save(HotelCompanyPaymentLedger payment)
        {
            string query = string.Format(@"SELECT dbo.FnCommonBillNumber('HotelCompanyPaymentLedger') as RegNumber");

            string ledgerNumber = InnboardDBContext.Database.SqlQuery<string>(query).FirstOrDefault();

            payment.LedgerNumber = ledgerNumber;

            return base.Save(payment);
        }

    }
}
