using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class GuestPreferenceDA : BaseService
    {
        public bool SaveGuestPreferenceInfo(GuestPreferenceBO prefBO, out int tempPrefId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGuestPreferenceInfo_SP"))
                    {

                        dbSmartAspects.AddInParameter(command, "@PreferenceName", DbType.String, prefBO.PreferenceName);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, prefBO.Description);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, prefBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, prefBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@PreferenceId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tempPrefId = Convert.ToInt32(command.Parameters["@PreferenceId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public bool UpdateGuestPreferenceInfo(GuestPreferenceBO prefBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGuestPreferenceInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@PreferenceId", DbType.Int64, prefBO.PreferenceId);
                        dbSmartAspects.AddInParameter(command, "@PreferenceName", DbType.String, prefBO.PreferenceName);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, prefBO.Description);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, prefBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, prefBO.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public List<GuestPreferenceBO> GetGuestPreferenceInfo(string prefName, bool activeStat)
        {
            List<GuestPreferenceBO> prefList = new List<GuestPreferenceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestPreferenceInfo"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PreferenceName", DbType.String, prefName);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, activeStat);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestPreference");
                    DataTable Table = ds.Tables["GuestPreference"];
                    prefList = Table.AsEnumerable().Select(r =>
                                new GuestPreferenceBO
                                {
                                    PreferenceId = r.Field<long>("PreferenceId"),
                                    PreferenceName = r.Field<string>("PreferenceName"),
                                    Description = r.Field<string>("Description"),
                                    ActiveStat = r.Field<Boolean>("ActiveStat"),
                                    ActiveStatus = r.Field<string>("ActiveStatus")
                                }).ToList();
                }
            }
            return prefList;
        }
        public GuestPreferenceBO GetGuestPreferenceInfoById(int prefId)
        {
            GuestPreferenceBO entityBO = new GuestPreferenceBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestPreferenceInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PreferenceId", DbType.Int64, prefId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestPreference");
                    DataTable Table = ds.Tables["GuestPreference"];
                    entityBO = Table.AsEnumerable().Select(r =>
                                new GuestPreferenceBO
                                {
                                    PreferenceId = r.Field<long>("PreferenceId"),
                                    PreferenceName = r.Field<string>("PreferenceName"),
                                    Description = r.Field<string>("Description"),
                                    ActiveStat = r.Field<Boolean>("ActiveStat"),
                                    ActiveStatus = r.Field<string>("ActiveStatus")
                                }).FirstOrDefault();
                }
            }
            return entityBO;
        }
        public List<GuestPreferenceBO> GetAllGuestPreferenceInfo()
        {
            List<GuestPreferenceBO> prefList = new List<GuestPreferenceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllGuestPreferenceInfo"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestPreference");
                    DataTable Table = ds.Tables["GuestPreference"];
                    prefList = Table.AsEnumerable().Select(r =>
                                new GuestPreferenceBO
                                {
                                    PreferenceId = r.Field<long>("PreferenceId"),
                                    PreferenceName = r.Field<string>("PreferenceName"),
                                    Description = r.Field<string>("Description"),
                                    ActiveStat = r.Field<Boolean>("ActiveStat"),
                                    ActiveStatus = r.Field<string>("ActiveStatus")
                                }).ToList();
                }
            }
            return prefList;
        }
        public List<GuestPreferenceBO> GetActiveGuestPreferenceInfo()
        {
            List<GuestPreferenceBO> prefList = new List<GuestPreferenceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveGuestPreferenceInfo_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestPreference");
                    DataTable Table = ds.Tables["GuestPreference"];
                    prefList = Table.AsEnumerable().Select(r =>
                                new GuestPreferenceBO
                                {
                                    PreferenceId = r.Field<long>("PreferenceId"),
                                    PreferenceName = r.Field<string>("PreferenceName"),
                                    Description = r.Field<string>("Description"),
                                    ActiveStat = r.Field<Boolean>("ActiveStat"),
                                    ActiveStatus = r.Field<string>("ActiveStatus")
                                }).ToList();
                }
            }
            return prefList;
        }
        public List<GuestPreferenceBO> GetGuestPreferenceInfoByGuestId(int guestId)
        {
            List<GuestPreferenceBO> entityList = new List<GuestPreferenceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestPreferenceInfoByGuestId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GuestId", DbType.Int32, guestId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GuestPreference");
                    DataTable Table = ds.Tables["GuestPreference"];
                    entityList = Table.AsEnumerable().Select(r =>
                                new GuestPreferenceBO
                                {
                                    PreferenceId = r.Field<long>("PreferenceId"),
                                    PreferenceName = r.Field<string>("PreferenceName"),
                                    Description = r.Field<string>("Description"),
                                    ActiveStat = r.Field<Boolean>("ActiveStat")
                                }).ToList();
                }
            }
            return entityList;
        }
        public bool SaveGuestPreferenceMappingInfo(string preferenList, int guestId)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandPreference = dbSmartAspects.GetStoredProcCommand("SaveGuestPreferenceMappingInfo_SP"))
                {
                   
                    commandPreference.Parameters.Clear();

                    dbSmartAspects.AddInParameter(commandPreference, "@GuestId", DbType.Int32, guestId);
                    dbSmartAspects.AddInParameter(commandPreference, "@PreferenceIdList", DbType.String, preferenList);

                    status = dbSmartAspects.ExecuteNonQuery(commandPreference) > 0 ? true : false;

                }
            }

            return status;
        }
    }
}
