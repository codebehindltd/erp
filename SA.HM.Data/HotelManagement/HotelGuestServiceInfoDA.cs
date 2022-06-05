using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class HotelGuestServiceInfoDA : BaseService
    {
        public List<HotelGuestServiceInfoBO> GetAllGuestServiceInfo()
        {
            List<HotelGuestServiceInfoBO> guestHouseServiceList = new List<HotelGuestServiceInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllGuestServiceInfo_SP"))

                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HotelGuestServiceInfoBO guestHouseService = new HotelGuestServiceInfoBO();

                                guestHouseService.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                guestHouseService.ServiceName = reader["ServiceName"].ToString();

                                guestHouseServiceList.Add(guestHouseService);
                            }
                        }
                    }
                }
            }
            return guestHouseServiceList;
        }
        public bool SaveHotelGuestServiceInfo(HotelGuestServiceInfoBO paidServiceBO, out int tempServiceId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveHotelGuestServiceInfo_SP"))
                    {

                        dbSmartAspects.AddInParameter(command, "@ServiceName", DbType.String, paidServiceBO.ServiceName);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, paidServiceBO.Description);
                        dbSmartAspects.AddInParameter(command, "@ServiceType", DbType.String, paidServiceBO.ServiceType);
                        dbSmartAspects.AddInParameter(command, "@UnitPriceLocal", DbType.Decimal, paidServiceBO.UnitPriceLocal);
                        dbSmartAspects.AddInParameter(command, "@UnitPriceUsd", DbType.Decimal, paidServiceBO.UnitPriceUsd);
                        dbSmartAspects.AddInParameter(command, "@CostCentreId", DbType.Int32, paidServiceBO.CostCenterId);
                        dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int32, paidServiceBO.AccountHeadId);
                        dbSmartAspects.AddInParameter(command, "@IsVatEnable", DbType.Boolean, paidServiceBO.IsVatEnable);
                        dbSmartAspects.AddInParameter(command, "@IsServiceChargeEnable", DbType.Boolean, paidServiceBO.IsServiceChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@IsCitySDChargeEnable", DbType.Boolean, paidServiceBO.IsCitySDChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@IsAdditionalChargeEnable", DbType.Boolean, paidServiceBO.IsAdditionalChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@IsGeneralService", DbType.Boolean, paidServiceBO.IsGeneralService);
                        dbSmartAspects.AddInParameter(command, "@IsPaidService", DbType.Boolean, paidServiceBO.IsPaidService);
                        dbSmartAspects.AddInParameter(command, "@IsNextDayAchievement", DbType.Boolean, paidServiceBO.IsNextDayAchievement);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, paidServiceBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, paidServiceBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@ServiceId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tempServiceId = Convert.ToInt32(command.Parameters["@ServiceId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public bool UpdateHotelGuestServiceInfo(HotelGuestServiceInfoBO paidServiceBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateHotelGuestServiceInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ServiceId", DbType.Int32, paidServiceBO.ServiceId);
                        dbSmartAspects.AddInParameter(command, "@ServiceName", DbType.String, paidServiceBO.ServiceName);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, paidServiceBO.Description);
                        dbSmartAspects.AddInParameter(command, "@ServiceType", DbType.String, paidServiceBO.ServiceType);
                        dbSmartAspects.AddInParameter(command, "@UnitPriceLocal", DbType.Decimal, paidServiceBO.UnitPriceLocal);
                        dbSmartAspects.AddInParameter(command, "@UnitPriceUsd", DbType.Decimal, paidServiceBO.UnitPriceUsd);
                        dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, paidServiceBO.CostCenterId);
                        dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int32, paidServiceBO.AccountHeadId);
                        dbSmartAspects.AddInParameter(command, "@IsVatEnable", DbType.Boolean, paidServiceBO.IsVatEnable);
                        dbSmartAspects.AddInParameter(command, "@IsServiceChargeEnable", DbType.Boolean, paidServiceBO.IsServiceChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@IsCitySDChargeEnable", DbType.Boolean, paidServiceBO.IsCitySDChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@IsAdditionalChargeEnable", DbType.Boolean, paidServiceBO.IsAdditionalChargeEnable);
                        dbSmartAspects.AddInParameter(command, "@IsGeneralService", DbType.Boolean, paidServiceBO.IsGeneralService);
                        dbSmartAspects.AddInParameter(command, "@IsPaidService", DbType.Boolean, paidServiceBO.IsPaidService);
                        dbSmartAspects.AddInParameter(command, "@IsNextDayAchievement", DbType.Boolean, paidServiceBO.IsNextDayAchievement);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, paidServiceBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, paidServiceBO.LastModifiedBy);

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
        public List<HotelGuestServiceInfoBO> GetPaidServiceInfoBySearchCriteriaForPagination(string serviceName, string serviceType, string activeStat, int recordPerPage, int pageIndex, out int totalRecords)
        {
            bool actStat;
            if (activeStat == "0")
            {
                actStat = true;
            }
            else
                actStat = false;
            if (serviceType == "PS")
            {
                serviceType = string.Empty;
            }
            string Where = GenarateWhereCondition(serviceName, serviceType, actStat);
            List<HotelGuestServiceInfoBO> paidServiceList = new List<HotelGuestServiceInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelGuestServiceInfoBySearchCriteriaForPagination_SP"))
                {
                    //GetPaidServiceInfoBySearchCriteriaForPagination_SP
                    //dbSmartAspects.AddInParameter(cmd, "@ServiceName", DbType.String, serviceName);
                    //dbSmartAspects.AddInParameter(cmd, "@ServiceType", DbType.String, serviceType);
                    //dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.String, actStat);
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HotelGuestServiceInfoBO paidServiceBO = new HotelGuestServiceInfoBO();
                                paidServiceBO.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                paidServiceBO.ServiceName = reader["ServiceName"].ToString();
                                paidServiceBO.ServiceType = reader["ServiceType"].ToString();
                                paidServiceBO.Description = reader["Description"].ToString();
                                paidServiceBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                paidServiceBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                paidServiceBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                paidServiceBO.AccountHeadId = Convert.ToInt32(reader["NodeId"]);

                                paidServiceBO.IsVatEnable = Convert.ToBoolean(reader["IsVatEnable"]);
                                paidServiceBO.VatEnable = reader["VatEnable"].ToString();
                                paidServiceBO.IsServiceChargeEnable = Convert.ToBoolean(reader["IsServiceChargeEnable"]);
                                paidServiceBO.ServiceChargeEnable = reader["ServiceChargeEnable"].ToString();
                                paidServiceBO.IsGeneralService = Convert.ToBoolean(reader["IsGeneralService"]);
                                paidServiceBO.GeneralService = reader["GeneralService"].ToString();
                                paidServiceBO.IsPaidService = Convert.ToBoolean(reader["IsPaidService"]);
                                paidServiceBO.PaidService = reader["PaidService"].ToString();

                                paidServiceBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                paidServiceBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                paidServiceList.Add(paidServiceBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return paidServiceList;
        }
        public HotelGuestServiceInfoBO GetHotelGuestServiceInfoById(int serviceId)
        {
            HotelGuestServiceInfoBO paidServiceBO = new HotelGuestServiceInfoBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelGuestServiceInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, serviceId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                paidServiceBO.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                paidServiceBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                paidServiceBO.AccountHeadId = Convert.ToInt32(reader["NodeId"]);
                                paidServiceBO.ServiceName = reader["ServiceName"].ToString();
                                paidServiceBO.ServiceType = reader["ServiceType"].ToString();
                                paidServiceBO.Description = reader["Description"].ToString();
                                paidServiceBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                paidServiceBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                paidServiceBO.IsVatEnable = Convert.ToBoolean(reader["IsVatEnable"]);
                                paidServiceBO.IsServiceChargeEnable = Convert.ToBoolean(reader["IsServiceChargeEnable"]);
                                paidServiceBO.IsCitySDChargeEnable = Convert.ToBoolean(reader["IsCitySDChargeEnable"]);
                                paidServiceBO.IsAdditionalChargeEnable = Convert.ToBoolean(reader["IsAdditionalChargeEnable"]);
                                paidServiceBO.IsGeneralService = Convert.ToBoolean(reader["IsGeneralService"]);
                                paidServiceBO.IsPaidService = Convert.ToBoolean(reader["IsPaidService"]);
                                paidServiceBO.IsNextDayAchievement = Convert.ToBoolean(reader["IsNextDayAchievement"]);
                                paidServiceBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                            }
                        }
                    }
                }
            }
            return paidServiceBO;
        }
        public List<HotelGuestServiceInfoBO> GetHotelGuestServiceInfo(int isGeneralService, int isPaidService, int isForRoomRegistration)
        {
            List<HotelGuestServiceInfoBO> paidService = new List<HotelGuestServiceInfoBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelGuestServiceInfo_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@IsGeneralService", DbType.Int32, isGeneralService);
                    dbSmartAspects.AddInParameter(cmd, "@IsPaidService", DbType.Int32, isPaidService);
                    dbSmartAspects.AddInParameter(cmd, "@IsForRoomRegistration", DbType.Int32, isForRoomRegistration);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaidService");
                    DataTable Table = ds.Tables["PaidService"];

                    paidService = Table.AsEnumerable().Select(r => new HotelGuestServiceInfoBO
                    {
                        ServiceId = r.Field<Int32>("ServiceId"),
                        ServiceName = r.Field<string>("ServiceName"),
                        ServiceType = r.Field<string>("ServiceType"),
                        Description = r.Field<string>("Description"),
                        UnitPriceLocal = r.Field<decimal>("UnitPriceLocal"),
                        UnitPriceUsd = r.Field<decimal>("UnitPriceUsd"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        ActiveStat = r.Field<bool>("ActiveStat")

                    }).ToList();
                }
            }
            return paidService;
        }
        public Boolean DeletePaidServiceById(int serviceId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteHotelGuestServiceInfoById_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ServiceId", DbType.Int32, serviceId);

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
        //--------------------Registration Paid Service---------------------------------
        public List<RegistrationServiceInfoBO> GetRegistrationServiceInfoByRegistrationId(int registrationId, string currencyType)
        {
            List<RegistrationServiceInfoBO> paidService = new List<RegistrationServiceInfoBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRegistrationServiceInfoByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaidService");
                    DataTable Table = ds.Tables["PaidService"];

                    paidService = Table.AsEnumerable().Select(r => new RegistrationServiceInfoBO
                    {
                        DetailServiceId = r.Field<Int32>("DetailServiceId"),
                        RegistrationId = r.Field<Int32>("RegistrationId"),
                        ServiceId = r.Field<Int32>("ServiceId"),
                        ServiceName = r.Field<string>("ServiceName"),
                        ServiceRate = r.Field<decimal?>("ServiceRate"),
                        UnitPrice = r.Field<decimal>("UnitPrice"),
                        CurrencyType = r.Field<int>("CurrencyType"),
                        ConversionRate = r.Field<decimal>("ConversionRate"),
                        IsAchieved = r.Field<bool>("IsAchieved")

                    }).ToList();

                    if (currencyType != "Local")
                    {
                        paidService = (from d in paidService
                                       select new RegistrationServiceInfoBO
                                       {
                                           DetailServiceId = d.DetailServiceId,
                                           RegistrationId = d.RegistrationId,
                                           ServiceId = d.ServiceId,
                                           ServiceName = d.ServiceName,
                                           ServiceRate = d.ServiceRate,
                                           UnitPrice = (d.UnitPrice / (d.ConversionRate == 0 ? 1 : d.ConversionRate)),
                                           CurrencyType = d.CurrencyType,
                                           ConversionRate = d.ConversionRate,
                                           IsAchieved = d.IsAchieved

                                       }).ToList();
                    }
                }
            }
            return paidService;
        }

        //--------------------Reservation Paid Service---------------------------------
        public List<RegistrationServiceInfoBO> GetReservationServiceInfoByReservationId(int reservationId, string currencyType)
        {
            List<RegistrationServiceInfoBO> paidService = new List<RegistrationServiceInfoBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservationServiceInfoByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaidService");
                    DataTable Table = ds.Tables["PaidService"];

                    paidService = Table.AsEnumerable().Select(r => new RegistrationServiceInfoBO
                    {
                        DetailServiceId = r.Field<Int32>("DetailServiceId"),
                        ReservationId = r.Field<Int32>("ReservationId"),
                        ServiceId = r.Field<Int32>("ServiceId"),
                        ServiceName = r.Field<string>("ServiceName"),
                        ServiceRate = r.Field<decimal?>("ServiceRate"),
                        UnitPrice = r.Field<decimal>("UnitPrice"),
                        CurrencyType = r.Field<int>("CurrencyType"),
                        ConversionRate = r.Field<decimal>("ConversionRate"),
                        IsAchieved = r.Field<bool>("IsAchieved")

                    }).ToList();

                    if (currencyType != "Local")
                    {
                        paidService = (from d in paidService
                                       select new RegistrationServiceInfoBO
                                       {
                                           DetailServiceId = d.DetailServiceId,
                                           ReservationId = d.ReservationId,
                                           ServiceId = d.ServiceId,
                                           ServiceName = d.ServiceName,
                                           ServiceRate = d.ServiceRate,
                                           UnitPrice = (d.UnitPrice / (d.ConversionRate == 0 ? 1 : d.ConversionRate)),
                                           CurrencyType = d.CurrencyType,
                                           ConversionRate = d.ConversionRate,
                                           IsAchieved = d.IsAchieved

                                       }).ToList();
                    }
                }
            }
            return paidService;
        }

        private string GenarateWhereCondition(string serviceName, string serviceType, bool activeStat)
        {

            string Where = string.Empty;
            if (!string.IsNullOrEmpty(serviceName.ToString()))
            {
                if (!string.IsNullOrEmpty(serviceType.ToString()))
                {
                    if (serviceType == "0")
                    {
                        Where += "  ServiceName = '" + serviceName + "' AND ActiveStat = '" + activeStat + "'";
                    }
                    else
                    {
                        Where += "  ServiceName = '" + serviceName + "' AND ServiceType = '" + serviceType + "' AND ActiveStat = '" + activeStat + "'";
                    }
                }
                else
                    Where += "  ServiceName = '" + serviceName + "' AND ActiveStat = '" + activeStat + "'";
            }
            else
            {
                if (!string.IsNullOrEmpty(serviceType.ToString()))
                {
                    if (serviceType == "0")
                    {
                        Where += " ActiveStat = '" + activeStat + "'";
                    }
                    else
                        Where += " ServiceType = '" + serviceType + "' AND ActiveStat = '" + activeStat + "'";
                }
                else
                    Where += " ActiveStat = '" + activeStat + "'";
            }

            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where;
            }
            return Where;
        }
        public List<HotelGuestServiceInfoBO> GetHotelGuestServiceInfoByCostCenter(int costCenterId)
        {
            List<HotelGuestServiceInfoBO> paidServiceList = new List<HotelGuestServiceInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelGuestServiceInfoByCostCenter_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HotelGuestServiceInfoBO paidServiceBO = new HotelGuestServiceInfoBO();
                                paidServiceBO.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                paidServiceBO.ServiceName = reader["ServiceName"].ToString();
                                paidServiceBO.ServiceType = reader["ServiceType"].ToString();
                                paidServiceBO.Description = reader["Description"].ToString();
                                paidServiceBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                paidServiceBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                paidServiceBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                paidServiceBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                paidServiceList.Add(paidServiceBO);
                            }
                        }
                    }
                }
            }
            return paidServiceList;
        }
    }
}
