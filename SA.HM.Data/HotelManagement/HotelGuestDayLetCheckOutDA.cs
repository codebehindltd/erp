using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data;
using System.Data.Common;

namespace HotelManagement.Data.HotelManagement
{
    public class HotelGuestDayLetCheckOutDA : BaseService
    {
        public bool SaveOrUpdateDayLetsInformation(List<HotelGuestDayLetCheckOutBO> dayLetBOList, out int DayLetId)
        {
            DayLetId = -1;
            bool status = false;
            foreach (HotelGuestDayLetCheckOutBO dayLetBO in dayLetBOList)
            {
                if (IsExistRegistrationId(dayLetBO.RegistrationId) == true)
                {
                    using (DbConnection conn = dbSmartAspects.CreateConnection())
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDayLetsInformationByRegId_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, dayLetBO.RegistrationId);
                            dbSmartAspects.AddInParameter(command, "@RoomRate", DbType.Decimal, dayLetBO.RoomRate);
                            dbSmartAspects.AddInParameter(command, "@DayLetDiscount", DbType.Decimal, dayLetBO.DayLetDiscount);
                            dbSmartAspects.AddInParameter(command, "@DayLetDiscountAmount", DbType.Decimal, dayLetBO.DayLetDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@DayLetDiscountType", DbType.String, dayLetBO.DayLetDiscountType);
                            dbSmartAspects.AddInParameter(command, "@DayLetStatus", DbType.String, dayLetBO.DayLetStatus);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, dayLetBO.LastModifiedBy);
                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            conn.Close();
                        }
                    }
                }
                else
                {
                    using (DbConnection conn = dbSmartAspects.CreateConnection())
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDayLetsInformationByRegId_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, dayLetBO.RegistrationId);
                            dbSmartAspects.AddInParameter(command, "@RoomRate", DbType.Decimal, dayLetBO.RoomRate);
                            dbSmartAspects.AddInParameter(command, "@DayLetDiscount", DbType.Decimal, dayLetBO.DayLetDiscount);
                            dbSmartAspects.AddInParameter(command, "@DayLetDiscountAmount", DbType.Decimal, dayLetBO.DayLetDiscountAmount);
                            dbSmartAspects.AddInParameter(command, "@DayLetDiscountType", DbType.String, dayLetBO.DayLetDiscountType);
                            dbSmartAspects.AddInParameter(command, "@DayLetStatus", DbType.String, dayLetBO.DayLetStatus);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, dayLetBO.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@DayLetId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                            DayLetId = Convert.ToInt32(command.Parameters["@DayLetId"].Value);
                            conn.Close();
                        }
                    }
                }
            }
            return status;
        }
        public bool IsExistRegistrationId(int registratioId)
        {
            bool isAvailable = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDayLetsInformationByRegId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registratioId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                isAvailable = true;
                            }
                        }
                    }
                }
                conn.Close();
            }
            return isAvailable;

        }
        public HotelGuestDayLetCheckOutBO GetDayLetInformation(int registratioId)
        {
            HotelGuestDayLetCheckOutBO bo = new HotelGuestDayLetCheckOutBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDayLetsInformationByRegId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registratioId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bo.DayLetId = Convert.ToInt32(reader["DayLetId"]);
                                if (reader["RegistrationId"] != DBNull.Value)
                                    bo.RegistrationId = Convert.ToInt32(reader["RegistrationId"]);

                                bo.DayLetDiscountType = reader["DayLetDiscountType"].ToString();
                                bo.DayLetDiscount = Convert.ToDecimal(reader["DayLetDiscount"].ToString());
                                bo.DayLetDiscountAmount = Convert.ToDecimal(reader["DayLetDiscountAmount"].ToString());

                                bo.DayLetStatus = reader["DayLetStatus"].ToString();
                                if (reader["CreatedBy"] != DBNull.Value)
                                    bo.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                if (reader["CreatedDate"] != DBNull.Value)
                                    bo.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                if (reader["LastModifiedBy"] != DBNull.Value)
                                    bo.LastModifiedBy = Convert.ToInt32(reader["LastModifiedBy"]);
                                if (reader["LastModifiedDate"] != DBNull.Value)
                                    bo.LastModifiedDate = Convert.ToDateTime(reader["LastModifiedDate"]);
                            }
                        }
                    }
                }
                conn.Close();
            }
            return bo;
        }
        public List<LateChkOutReportViewBO> GetDayLateCheckOut(string reportType, DateTime startDate, DateTime endDate)
        {
            List<LateChkOutReportViewBO> latrChkOut = new List<LateChkOutReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDayLetsReportByDateRange_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, startDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, endDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "DayLateChkOut");
                    DataTable Table = ds.Tables["DayLateChkOut"];
                    latrChkOut = Table.AsEnumerable().Select(r =>
                                new LateChkOutReportViewBO
                                {
                                    DayLetDate = r.Field<string>("DayLetDate"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    RegistrationNumber = r.Field<string>("RegistrationNumber"),
                                    GuestName = r.Field<string>("GuestName"),
                                    Pax = r.Field<int>("Pax"),
                                    Tariff = r.Field<decimal>("Tariff"),
                                    Discount = r.Field<decimal>("Discount"),
                                    DiscountedTariff = r.Field<decimal>("DiscountedTariff"),
                                    ServiceCharge = r.Field<decimal>("ServiceCharge"),
                                    Vat = r.Field<decimal>("Vat"),
                                    BasicRoomRent = r.Field<decimal>("BasicRoomRent")
                                }).ToList();
                }
            }
            return latrChkOut;
        }
    }
}
