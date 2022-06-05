using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantTableReservationDetailDA : BaseService
    {
        public List<RestaurantTableReservationDetailBO> GetReservationDetailByReservationId(int reservationId)
        {
            List<RestaurantTableReservationDetailBO> reservationDetailList = new List<RestaurantTableReservationDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservationDetailByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantTableReservationDetailBO reservationDetail = new RestaurantTableReservationDetailBO();

                                reservationDetail.ReservationDetailId = Convert.ToInt32(reader["ReservationDetailId"]);
                                reservationDetail.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                reservationDetail.CostCenterId = Convert.ToInt32(reader["CostCentreId"]);
                                //reservationDetail.RoomType = reader["RoomType"].ToString();
                                reservationDetail.TableId = Convert.ToInt32(reader["TableId"]);
                                //reservationDetail.RoomNumber = reader["RoomNumber"].ToString();

                                reservationDetailList.Add(reservationDetail);
                            }
                        }
                    }
                }
            }
            return reservationDetailList; 
        }
    }
}
