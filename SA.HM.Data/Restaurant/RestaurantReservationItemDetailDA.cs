using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantReservationItemDetailDA: BaseService
    {
        public List<RestaurantReservationItemDetailBO> GetRestaurantReservationItemDetailByReservationId(int reservationId)
        {
            List<RestaurantReservationItemDetailBO> detailList = new List<RestaurantReservationItemDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantReservationItemDetailByResevationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ReservationItemDetail");
                    DataTable Table = ds.Tables["ReservationItemDetail"];

                    detailList = Table.AsEnumerable().Select(r => new RestaurantReservationItemDetailBO
                    {
                        ItemDetailId = r.Field<Int32>("ItemDetailId"),
                        ReservationId = r.Field<Int32>("ReservationId"),
                        ItemTypeId = r.Field<Int32>("ItemTypeId"),
                        ItemType = r.Field<string>("ItemType"),
                        ItemId = r.Field<int>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        ItemUnit = r.Field<decimal>("ItemUnit"),
                        UnitPrice = r.Field<decimal>("UnitPrice")
                    }).ToList();                    
                }
            }
            return detailList;
        }
    }
}
