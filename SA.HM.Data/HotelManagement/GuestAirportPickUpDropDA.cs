using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data;
using System.Data.Common;

namespace HotelManagement.Data.HotelManagement
{
    public class GuestAirportPickUpDropDA: BaseService
    {
        public List<GuestAirportPickUpDropReportViewBO> GetGuestAirportPickupDropInfo(DateTime startDate, DateTime endDate, int companyId)
        {
            List<GuestAirportPickUpDropReportViewBO> guestList = new List<GuestAirportPickUpDropReportViewBO>();
            DataSet customerDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelGuestAirportPickUpDropInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, startDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, endDate);
                    if (companyId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                   // DataTable table = customerDS.Tables["GuestAirportPickupDrop"];
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestAirportPickupDrop");
                    DataTable Table = ds.Tables["GuestAirportPickupDrop"];
                    guestList = Table.AsEnumerable().Select(r =>
                                new GuestAirportPickUpDropReportViewBO
                                {
                                    FromDate = r.Field<DateTime>("FromDate"),
                                    ToDate = r.Field<DateTime>("ToDate"),
                                    TransactionDate = r.Field<DateTime>("TransactionDate"),
                                    TransactionType = r.Field<string>("TransactionType"),
                                    TransactionNumber = r.Field<string>("TransactionNumber"),
                                    GuestName = r.Field<string>("GuestName"),
                                    PickUpDropInfo = r.Field<string>("PickUpDropInfo"),
                                    FlightName = r.Field<string>("FlightName"),
                                    FlightNumber = r.Field<string>("FlightNumber"),
                                    TimeString = r.Field<string>("TimeString"),
                                    RoomNumber = r.Field<string>("RoomNumber")
                                }).ToList();
                }
            }
            return guestList;
        }
        public List<GuestAirportPickUpDropReportViewBO> GetGuestAirportDropInfoByRegistrationId(int registrationId)
        {
            List<GuestAirportPickUpDropReportViewBO> guestList = new List<GuestAirportPickUpDropReportViewBO>();
            DataSet customerDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestAirportDropInfoByRegistrationId_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestAirportPickupDrop");
                    DataTable Table = ds.Tables["GuestAirportPickupDrop"];
                    guestList = Table.AsEnumerable().Select(r =>
                                new GuestAirportPickUpDropReportViewBO
                                {
                                    TransactionDate = r.Field<DateTime>("TransactionDate"),
                                    TransactionType = r.Field<string>("TransactionType"),
                                    TransactionNumber = r.Field<string>("TransactionNumber"),
                                    GuestName = r.Field<string>("GuestName"),
                                    FlightName = r.Field<string>("FlightName"),
                                    FlightNumber = r.Field<string>("FlightNumber"),
                                    TimeString = r.Field<string>("TimeString"),
                                    RoomNumber = r.Field<string>("RoomNumber")
                                }).ToList();
                }
            }
            return guestList;
        }
    }
}
