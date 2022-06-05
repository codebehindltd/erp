using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class HMPaidServiceDA : BaseService
    {
        public bool SaveHMPaidServiceInfo(HMPaidServiceBO paidServiceBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveHMPaidServiceInfo_SP"))
                {

                    dbSmartAspects.AddInParameter(command, "@ServiceName", DbType.String, paidServiceBO.ServiceName);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, paidServiceBO.Description);
                    dbSmartAspects.AddInParameter(command, "@ServiceType", DbType.String, paidServiceBO.ServiceType);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceLocal", DbType.Decimal, paidServiceBO.UnitPriceLocal);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceUsd", DbType.Decimal, paidServiceBO.UnitPriceUsd);
                    dbSmartAspects.AddInParameter(command, "@CostCentreId", DbType.Int32, paidServiceBO.CostCenterId);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, paidServiceBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, paidServiceBO.CreatedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool UpdateHMPaidServiceInfo(HMPaidServiceBO paidServiceBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateHMPaidServiceInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@PaidServiceId", DbType.Int32, paidServiceBO.PaidServiceId);
                    dbSmartAspects.AddInParameter(command, "@ServiceName", DbType.String, paidServiceBO.ServiceName);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, paidServiceBO.Description);
                    dbSmartAspects.AddInParameter(command, "@ServiceType", DbType.String, paidServiceBO.ServiceType);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceLocal", DbType.Decimal, paidServiceBO.UnitPriceLocal);
                    dbSmartAspects.AddInParameter(command, "@UnitPriceUsd", DbType.Decimal, paidServiceBO.UnitPriceUsd);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, paidServiceBO.CostCenterId);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, paidServiceBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, paidServiceBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<HMPaidServiceBO> GetPaidServiceInfoBySearchCriteriaForPagination(string serviceName, string serviceType, string activeStat, int recordPerPage, int pageIndex, out int totalRecords)
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
            List<HMPaidServiceBO> paidServiceList = new List<HMPaidServiceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPaidServiceInfoBySearchCriteriaForPagination_SP"))
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
                                HMPaidServiceBO paidServiceBO = new HMPaidServiceBO();
                                paidServiceBO.PaidServiceId = Convert.ToInt32(reader["PaidServiceId"]);
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
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return paidServiceList;
        }
        public HMPaidServiceBO GetPaidServiceInfoById(int paidServiceId)
        {
            HMPaidServiceBO paidServiceBO = new HMPaidServiceBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPaidServiceInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PaidServiceId", DbType.Int32, paidServiceId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                paidServiceBO.PaidServiceId = Convert.ToInt32(reader["PaidServiceId"]);
                                paidServiceBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                paidServiceBO.ServiceName = reader["ServiceName"].ToString();
                                paidServiceBO.ServiceType = reader["ServiceType"].ToString();
                                paidServiceBO.Description = reader["Description"].ToString();
                                paidServiceBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                paidServiceBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                paidServiceBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                            }
                        }
                    }
                }
            }
            return paidServiceBO;
        }
        public List<HMPaidServiceBO> GetPaidServiceInfo()
        {
            List<HMPaidServiceBO> paidService = new List<HMPaidServiceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPaidServiceInfo_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaidService");
                    DataTable Table = ds.Tables["PaidService"];

                    paidService = Table.AsEnumerable().Select(r => new HMPaidServiceBO
                    {
                        PaidServiceId = r.Field<Int32>("PaidServiceId"),
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
        public Boolean DeletePaidServiceById(int paidServiceId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeletePaidServiceInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@PaidServiceId", DbType.Int32, paidServiceId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        //--------------------Registration Paid Service---------------------------------
        public List<HotelRegistrationPaidServiceBO> GetHotelRegistrationPaidServiceByRegistrationId(int registrationId, string currencyType)
        {
            List<HotelRegistrationPaidServiceBO> paidService = new List<HotelRegistrationPaidServiceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelRegistrationPaidServiceByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaidService");
                    DataTable Table = ds.Tables["PaidService"];

                    paidService = Table.AsEnumerable().Select(r => new HotelRegistrationPaidServiceBO
                    {
                        DetailPaidServiceId = r.Field<Int32>("DetailPaidServiceId"),
                        RegistrationId = r.Field<Int32>("RegistrationId"),
                        PaidServiceId = r.Field<Int32>("PaidServiceId"),
                        ServiceName = r.Field<string>("ServiceName"),
                        PaidServicePrice = r.Field<decimal?>("PaidServicePrice"),
                        UnitPrice = r.Field<decimal>("UnitPrice"),
                        CurrencyType = r.Field<int>("CurrencyType"),
                        ConversionRate = r.Field<decimal>("ConversionRate"),
                        IsAchieved = r.Field<bool>("IsAchieved")

                    }).ToList();

                    if (currencyType != "45")
                    {
                        paidService = (from d in paidService
                                       select new HotelRegistrationPaidServiceBO
                                       {
                                           DetailPaidServiceId = d.DetailPaidServiceId,
                                           RegistrationId = d.RegistrationId,
                                           PaidServiceId = d.PaidServiceId,
                                           ServiceName = d.ServiceName,
                                           PaidServicePrice = d.PaidServicePrice,
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
        public List<HotelRegistrationPaidServiceBO> GetHotelReservationPaidServiceByReservationId(int reservationId, string currencyType)
        {
            List<HotelRegistrationPaidServiceBO> paidService = new List<HotelRegistrationPaidServiceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelReservationPaidServiceByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaidService");
                    DataTable Table = ds.Tables["PaidService"];

                    paidService = Table.AsEnumerable().Select(r => new HotelRegistrationPaidServiceBO
                    {
                        DetailPaidServiceId = r.Field<Int32>("DetailPaidServiceId"),
                        ReservationId = r.Field<Int32>("ReservationId"),
                        PaidServiceId = r.Field<Int32>("PaidServiceId"),
                        ServiceName = r.Field<string>("ServiceName"),
                        PaidServicePrice = r.Field<decimal?>("PaidServicePrice"),
                        UnitPrice = r.Field<decimal>("UnitPrice"),
                        CurrencyType = r.Field<int>("CurrencyType"),
                        ConversionRate = r.Field<decimal>("ConversionRate"),
                        IsAchieved = r.Field<bool>("IsAchieved")

                    }).ToList();

                    if (currencyType != "45")
                    {
                        paidService = (from d in paidService
                                       select new HotelRegistrationPaidServiceBO
                                       {
                                           DetailPaidServiceId = d.DetailPaidServiceId,
                                           ReservationId = d.ReservationId,
                                           PaidServiceId = d.PaidServiceId,
                                           ServiceName = d.ServiceName,
                                           PaidServicePrice = d.PaidServicePrice,
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
                    Where += "  ServiceName = '" + serviceName + "' AND ServiceType = '" + serviceType + "' AND ActiveStat = '" + activeStat + "'";
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
        public List<HMPaidServiceBO> GetPaidServiceInfoByCostCenter(int costCenterId)
        {
            List<HMPaidServiceBO> paidServiceList = new List<HMPaidServiceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPaidServiceInfoByCostCenter_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HMPaidServiceBO paidServiceBO = new HMPaidServiceBO();
                                paidServiceBO.PaidServiceId = Convert.ToInt32(reader["PaidServiceId"]);
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
