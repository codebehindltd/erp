using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.HotelManagement
{
    public class GuestBirthdayNotificationDA : BaseService
    {
        public bool SaveGuestBirthdayNotificationStatus(List<BirthdayNotificationViewBO> guestList)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGuestBirthdayNotificationStatus_SP"))
                {

                    foreach (var item in guestList)
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@GuestId", DbType.Int64, item.GuestId);
                        dbSmartAspects.AddInParameter(command, "@IsEmailSent", DbType.Boolean, item.IsEmailSent);
                        dbSmartAspects.AddInParameter(command, "@IsSmsSent", DbType.Boolean, item.IsSmsSent);
                        dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Boolean, item.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(command);
                    }
                }
                conn.Close();
            }
            if (status > 0)
                return true;
            else
                return false;
        }
        public bool UpdateGuestBirthdayNotificationStatus(List<BirthdayNotificationViewBO> guestList)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGuestBirthdayNotificationStatus_SP"))
                {

                    foreach (var item in guestList)
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, item.Id);
                        dbSmartAspects.AddInParameter(command, "@GuestId", DbType.Int64, item.GuestId);
                        dbSmartAspects.AddInParameter(command, "@IsEmailSent", DbType.Boolean, item.IsEmailSent);
                        dbSmartAspects.AddInParameter(command, "@IsSmsSent", DbType.Boolean, item.IsSmsSent);
                        dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Boolean, item.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(command);
                    }
                }
                conn.Close();
            }
            if (status > 0)
                return true;
            else
                return false;
        }
        public List<BirthdayNotificationViewBO> GetGuestBirthdayNotificationStatus(DateTime? date, string guestType, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<BirthdayNotificationViewBO> guestList = new List<BirthdayNotificationViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestBirthdayNotificationStatus_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@Date", DbType.DateTime, date);
                    dbSmartAspects.AddInParameter(cmd, "@GuestType", DbType.String, guestType);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestsBirthday");
                    DataTable Table = ds.Tables["GuestsBirthday"];
                    guestList = Table.AsEnumerable().Select(r =>
                                new BirthdayNotificationViewBO
                                {
                                    
                                    GuestName = r.Field<string>("GuestName"),
                                    CountryName = r.Field<string>("CountryName"),
                                    GuestPhone = r.Field<string>("GuestPhone"),
                                    GuestEmail = r.Field<string>("GuestEmail"),
                                    Id = r.Field<Int64?>("Id"),
                                    GuestId = r.Field<int>("GuestId"),
                                    Date = r.Field<DateTime?>("Date"),
                                    IsEmailSent = r.Field<bool>("IsEmailSent"),
                                    IsSmsSent = r.Field<bool>("IsSmsSent")

                                }).ToList();
                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
                
            }
            return guestList;
        }
    }
}
