using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantReservationTableDetailDA : BaseService
    {
        //public List<RestaurantReservationTableDetailBO> GetReservationDetailByReservationId(int reservationId)
        //{
        //    List<RestaurantReservationTableDetailBO> reservationDetailList = new List<RestaurantReservationTableDetailBO>();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservationDetailByReservationId_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        RestaurantReservationTableDetailBO reservationDetail = new RestaurantReservationTableDetailBO();

        //                        reservationDetail.ReservationDetailId = Convert.ToInt32(reader["ReservationDetailId"]);
        //                        reservationDetail.ReservationId = Convert.ToInt32(reader["ReservationId"]);
        //                        reservationDetail.CostCenterId = Convert.ToInt32(reader["CostCentreId"]);
        //                        //reservationDetail.RoomType = reader["RoomType"].ToString();
        //                        reservationDetail.TableId = Convert.ToInt32(reader["TableId"]);
        //                        //reservationDetail.RoomNumber = reader["RoomNumber"].ToString();

        //                        reservationDetailList.Add(reservationDetail);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return reservationDetailList;
        //}
        public List<RestaurantReservationTableDetailBO> GetRestaurantReservationTableDetailByResevationId(long reservationId)
        {
            List<RestaurantReservationTableDetailBO> detailList = new List<RestaurantReservationTableDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantReservationTableDetailByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int64, reservationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ReservationDetail");
                    DataTable Table = ds.Tables["ReservationDetail"];

                    detailList = Table.AsEnumerable().Select(r => new RestaurantReservationTableDetailBO
                    {
                        TableDetailId = r.Field<Int64>("TableDetailId"),
                        ReservationId = r.Field<Int64>("ReservationId"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        TableId = r.Field<Int32>("TableId"),
                        DiscountType = r.Field<string>("DiscountType"),
                        Amount = r.Field<decimal>("DiscountAmount")
                    }).ToList();
                }
            }
            return detailList;
        }
        public List<RestaurantReservationTableDetailBO> GetTableDetailListByRevIdForGrid(int reservationId, int isRegisteredAlso)
        {
            List<RestaurantReservationTableDetailBO> tableDetailList = new List<RestaurantReservationTableDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantReservationDetailByReservId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    //dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@IsRegisteredAlso", DbType.Int32, isRegisteredAlso);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ReservationDetail");
                    DataTable Table = ds.Tables["ReservationDetail"];

                    tableDetailList = Table.AsEnumerable().Select(r => new RestaurantReservationTableDetailBO
                    {                        
                        ReservationId = r.Field<Int64>("ReservationId"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        CostCentre = r.Field<string>("CostCentre"),
                        //TableId = r.Field<Int32>("TableId"),
                        DiscountType = r.Field<string>("DiscountType"),
                        Amount = r.Field<decimal>("DiscountAmount"),
                        TableNumberIdList = r.Field<string>("TableIdList"),
                        TableNumberList = r.Field<string>("TableNumberList"),
                        TableNumberListInfoWithCount = r.Field<string>("TableNumberListInfoWithCount")
                    }).ToList();                    
                }
            }
            return tableDetailList;
        }
    }
}
