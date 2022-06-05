using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class GuestHouseServiceDA : BaseService
    {
        public List<GuestHouseServiceBO> GetGuestHouseServiceInfo()
        {
            List<GuestHouseServiceBO> guestHouseServiceList = new List<GuestHouseServiceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestHouseServiceInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestHouseServiceBO guestHouseService = new GuestHouseServiceBO();

                                guestHouseService.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                guestHouseService.ServiceName = reader["ServiceName"].ToString();
                                guestHouseService.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                guestHouseService.ActiveStatus = reader["ActiveStatus"].ToString();
                                //guestHouseService.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                //guestHouseService.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);

                                guestHouseServiceList.Add(guestHouseService);
                            }
                        }
                    }
                }
            }
            return guestHouseServiceList;
        }
        public List<GuestHouseServiceBO> GetActiveGuestHouseServiceInfo()
        {
            List<GuestHouseServiceBO> guestHouseServiceList = new List<GuestHouseServiceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveGuestHouseServiceInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestHouseServiceBO guestHouseService = new GuestHouseServiceBO();

                                guestHouseService.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                guestHouseService.ServiceName = reader["ServiceName"].ToString();
                                guestHouseService.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                guestHouseService.ActiveStatus = reader["ActiveStatus"].ToString();
                                //guestHouseService.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                //guestHouseService.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);

                                guestHouseServiceList.Add(guestHouseService);
                            }
                        }
                    }
                }
            }
            return guestHouseServiceList;
        }
        public Boolean SaveGuestHouseServiceInfo(GuestHouseServiceBO serviceBO, out int tmpserviceId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGuestHouseServiceInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ServiceName", DbType.String, serviceBO.ServiceName);
                    dbSmartAspects.AddInParameter(command, "@IncomeNodeId", DbType.Int32, serviceBO.IncomeNodeId);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, serviceBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, serviceBO.VatAmount);
                    dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, serviceBO.ServiceCharge);
                    dbSmartAspects.AddInParameter(command, "@IsVatEnable", DbType.Boolean, serviceBO.IsVatEnable);
                    dbSmartAspects.AddInParameter(command, "@IsServiceChargeEnable", DbType.Boolean, serviceBO.IsServiceChargeEnable);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, serviceBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@ServiceId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpserviceId = Convert.ToInt32(command.Parameters["@ServiceId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateGuestHouseServiceInfo(GuestHouseServiceBO serviceBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGuestHouseServiceInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ServiceId", DbType.Int32, serviceBO.ServiceId);
                    dbSmartAspects.AddInParameter(command, "@ServiceName", DbType.String, serviceBO.ServiceName);
                    dbSmartAspects.AddInParameter(command, "@IncomeNodeId", DbType.Int32, serviceBO.IncomeNodeId);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, serviceBO.ActiveStat);
                    //        dbSmartAspects.AddInParameter(command, "@VatAmount", DbType.Decimal, serviceBO.VatAmount);
                    //        dbSmartAspects.AddInParameter(command, "@ServiceCharge", DbType.Decimal, serviceBO.ServiceCharge);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, serviceBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public GuestHouseServiceBO GetGuestHouseServiceInfoById(int serviceId)
        {
            GuestHouseServiceBO guestHouseService = new GuestHouseServiceBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestHouseServiceInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, serviceId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                guestHouseService.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                guestHouseService.ServiceName = reader["ServiceName"].ToString();
                                guestHouseService.IncomeNodeId = Int32.Parse(reader["IncomeNodeId"].ToString());
                                guestHouseService.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                guestHouseService.ActiveStatus = reader["ActiveStatus"].ToString();
                                guestHouseService.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                guestHouseService.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                            }
                        }
                    }
                }
            }
            return guestHouseService;
        }
        public List<GuestHouseServiceBO> GetGuestHouseServiceInfoBySearchCriteria(string ServiceName, bool ActiveStat)
        {
            List<GuestHouseServiceBO> guestHouseServiceList = new List<GuestHouseServiceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestHouseServiceInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceName", DbType.String, ServiceName);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, ActiveStat);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestHouseServiceBO guestHouseService = new GuestHouseServiceBO();

                                guestHouseService.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                guestHouseService.ServiceName = reader["ServiceName"].ToString();
                                guestHouseService.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                guestHouseService.ActiveStatus = reader["ActiveStatus"].ToString();
                                guestHouseService.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                guestHouseService.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);

                                guestHouseServiceList.Add(guestHouseService);
                            }
                        }
                    }
                }
            }
            return guestHouseServiceList;
        }
        public List<GuestHouseServiceBO> GetGuestHouseRestaurantNameInfo()
        {
            List<GuestHouseServiceBO> guestHouseServiceList = new List<GuestHouseServiceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestHouseRestaurantNameInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestHouseServiceBO guestHouseService = new GuestHouseServiceBO();

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
        public List<GuestHouseServiceBO> GetGuestHouseRestaurantNameInfoByUserInfoId(string userId, int userInfoId)
        {
            List<GuestHouseServiceBO> guestHouseServiceList = new List<GuestHouseServiceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestHouseRestaurantNameInfoByUserInfoId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.String, userId);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestHouseServiceBO guestHouseService = new GuestHouseServiceBO();
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
        public GuestHouseServiceBO GetGuestHouseServiceInfoDetailsById(int serviceId)
        {
            GuestHouseServiceBO guestHouseService = new GuestHouseServiceBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestHouseServiceInfoDetailsById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, serviceId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                guestHouseService.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                guestHouseService.ServiceName = reader["ServiceName"].ToString();
                                guestHouseService.IncomeNodeId = Int32.Parse(reader["IncomeNodeId"].ToString());
                                guestHouseService.IncomeNodeHead = reader["IncomeNodeHead"].ToString();
                                guestHouseService.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                guestHouseService.ActiveStatus = reader["ActiveStatus"].ToString();
                                guestHouseService.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                guestHouseService.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);
                            }
                        }
                    }
                }
            }
            return guestHouseService;
        }
        public List<SalesAuditReportBO> GetSalesAuditReportForReport(DateTime fromDate, DateTime toDate)
        {
            List<SalesAuditReportBO> salesAudit = new List<SalesAuditReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelSalesSummaryInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet salarySheetDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, salarySheetDS, "SalarySheet");

                    DataTable Table = salarySheetDS.Tables["SalarySheet"];

                    salesAudit = Table.AsEnumerable().Select(r => new SalesAuditReportBO
                    {
                        TransactionDate = r.Field<string>("TransactionDate"),
                        Cash = r.Field<decimal>("Cash"),
                        Credit = r.Field<decimal>("Credit"),
                        TotalCashCredit = r.Field<decimal>("TotalCashCredit"),
                        TotalRoomSales = r.Field<decimal>("TotalRoomSales"),

                        PaymentAmount = r.Field<decimal>("PaymentAmount"),
                        NodeId = r.Field<int>("NodeId"),
                        NodeHead = r.Field<string>("NodeHead"),
                        ServiceId = r.Field<int>("ServiceId"),
                        ServiceName = r.Field<string>("ServiceName")

                    }).ToList();

                }
            }

            return salesAudit;
        }
    }
}
