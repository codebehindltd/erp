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
    public class HotelGuestHouseCheckOutDataAccess : GenericDataAccess<HotelGuestHouseCheckOut>, IHotelGuestHouseCheckOut
    {
        public HotelGuestHouseCheckOutDataAccess() 
        {
        }

        public new HotelGuestHouseCheckOut Save(HotelGuestHouseCheckOut hotelGuestHouseCheckOut)
        {
            string query = string.Format(@"SELECT dbo.FnCommonBillNumber('HotelGuestHouseCheckOut') as BillNumber");

            string billNumber = InnboardDBContext.Database.SqlQuery<string>(query).FirstOrDefault();

            hotelGuestHouseCheckOut.CheckOutDate = hotelGuestHouseCheckOut.CheckOutDateForSync;
            hotelGuestHouseCheckOut.BillNumber = billNumber;

            return base.Save(hotelGuestHouseCheckOut);
        }
    }
}
