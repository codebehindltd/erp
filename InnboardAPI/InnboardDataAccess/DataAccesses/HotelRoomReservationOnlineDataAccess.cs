using InnboardAPI.DataAccesses;
using InnboardDomain.Interfaces;
using InnboardDomain.Models;
using InnboardDomain.ViewModel;
using System.Data.SqlClient;
using System.Linq;

namespace InnboardDataAccess.DataAccesses
{
    public class HotelRoomReservationOnlineDataAccess : GenericDataAccess<HotelRoomReservationOnline>, IHotelRoomReservationOnline
    {
        public new HotelRoomReservationOnline Save(HotelRoomReservationOnline roomReservation)
        {
            string query = string.Format(@"EXEC [dbo].[GetDynamicBillNumber_SP] as RegNumber");

            SqlParameter param1 = new SqlParameter("@mFirstSeries", "OR");
            SqlParameter param2 = new SqlParameter("@mTableName", "HotelRoomReservationOnline");
            SqlParameter param3 = new SqlParameter("@mColumnName", "ReservationNumber");

            string reservationNumber = InnboardDBContext.Database.SqlQuery<string>("EXEC [dbo].[GetDynamicBillNumber_SP] @mFirstSeries,@mTableName,@mColumnName ", param1, param2, param3).FirstOrDefault();

            roomReservation.ReservationNumber = reservationNumber;
            return base.Save(roomReservation);
        }

        public HotelRoomReservationOnline Save(HotelRoomReservationOnlineView hotelRoom)
        {
            using (var tran = InnboardDBContext.Database.BeginTransaction())
            {
                Save(hotelRoom.HotelRoomReservationOnline);
                InnboardDBContext.HotelGuestInformationOnline.Add(hotelRoom.HotelGuestInformationOnline);
                hotelRoom.HotelRoomReservationDetailOnlines.Select(i => { i.ReservationId = (int)hotelRoom.HotelRoomReservationOnline.ReservationId; return i; }).ToList();
                InnboardDBContext.HotelRoomReservationDetailOnline.AddRange(hotelRoom.HotelRoomReservationDetailOnlines);
                SaveChanges();
                tran.Commit();
            }
            return hotelRoom.HotelRoomReservationOnline;
        }
    }
}
