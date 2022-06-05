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
    public class HotelGuestBillPaymentDataAccess : GenericDataAccess<HotelGuestBillPayment>, IHotelGuestBillPayment
    {
        public HotelGuestBillPaymentDataAccess() 
        {

        }

        public new HotelGuestBillPayment Save(HotelGuestBillPayment hotelGuestBillPayment)
        {
            string query = string.Format(@"SELECT dbo.FnGuestPaymentBillNumber() as BillNumber");

            string billNumber = InnboardDBContext.Database.SqlQuery<string>(query).FirstOrDefault();
            hotelGuestBillPayment.BillNumber = billNumber;

            return base.Save(hotelGuestBillPayment);
        }


    }
}
