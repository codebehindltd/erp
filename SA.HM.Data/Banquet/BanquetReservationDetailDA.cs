using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Banquet;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Banquet
{
    public class BanquetReservationDetailDA : BaseService
    { 
        public List<BanquetReservationDetailBO> GetBanquetReservationDetailInfoByReservationId(long ReservationId, int costCenterId)
        {
            List<BanquetReservationDetailBO> detailList = new List<BanquetReservationDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetReservationDetailInfoByResevationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int64, ReservationId);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetReservationDetailBO detail = new BanquetReservationDetailBO();

                                detail.ItemTypeId = Convert.ToInt64(reader["ItemTypeId"].ToString());
                                detail.ItemType = reader["ItemType"].ToString();
                                detail.ItemId = Int64.Parse(reader["ItemId"].ToString());
                                detail.ItemName = reader["ItemName"].ToString();
                                detail.ReservationId = Int64.Parse(reader["ReservationId"].ToString());
                                detail.ItemUnit = Convert.ToDecimal(reader["ItemUnit"].ToString());
                                detail.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());
                                detail.TotalPrice = Convert.ToDecimal(reader["TotalPrice"].ToString());
                                detail.Id = Int64.Parse(reader["Id"].ToString());

                                detail.ItemWiseDiscountType = Convert.ToString(reader["ItemWiseDiscountType"]);
                                detail.ItemWiseIndividualDiscount = Convert.ToDecimal(reader["ItemWiseIndividualDiscount"]);

                                detail.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                                detail.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                detail.CitySDCharge = Convert.ToDecimal(reader["CitySDCharge"]);
                                detail.AdditionalChargeType = reader["AdditionalChargeType"].ToString();
                                detail.AdditionalCharge = Convert.ToDecimal(reader["AdditionalCharge"]);
                                detail.IsItemEditable = Convert.ToBoolean(reader["IsItemEditable"]);
                                detail.IsComplementary = Convert.ToBoolean(reader["IsComplementary"]);
                                if ((reader["ItemArrivalTime"]) != DBNull.Value)
                                {
                                    detail.ItemArrivalTime = Convert.ToDateTime(reader["ItemArrivalTime"]);
                                }
                                
                                detail.ItemDescription = Convert.ToString(reader["ItemDescription"]);

                                detailList.Add(detail);
                            }
                        }
                    }
                }
            }
            return detailList;
        }
        public BanquetReservationDetailBO GetBanquetReservationDetailInfoById(int detailId, int reservationId)
        {
            BanquetReservationDetailBO detailBO = new BanquetReservationDetailBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetReservationDetailInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DetailId", DbType.Int32, detailId);
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                detailBO.ItemTypeId = Convert.ToInt32(reader["ItemTypeId"].ToString());
                                detailBO.ItemType = reader["ItemType"].ToString();
                                detailBO.ItemId = Int32.Parse(reader["ItemId"].ToString());
                                detailBO.ItemName = reader["ItemName"].ToString();
                                detailBO.ReservationId = Int32.Parse(reader["ReservationId"].ToString());
                                detailBO.ItemUnit = Convert.ToDecimal(reader["ItemUnit"].ToString());
                                detailBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());
                                detailBO.TotalPrice = Convert.ToDecimal(reader["TotalPrice"].ToString());

                                detailBO.Id = Int32.Parse(reader["DetailId"].ToString());
                            }
                        }
                    }
                }
            }
            return detailBO;
        }
    }
}
