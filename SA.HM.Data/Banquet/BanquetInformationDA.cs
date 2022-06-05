using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Banquet;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Banquet
{
    public class BanquetInformationDA : BaseService
    {
        public List<BanquetInformationBO> GetBanquetInfoBySearchCriteria(string Name, int Status)
        {
            List<BanquetInformationBO> entityBOList = new List<BanquetInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetInfoBySearchCriteria_SP"))
                {

                    if (!string.IsNullOrWhiteSpace(Name))
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, Name);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                    if (Status != 0)
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.Int32, Status);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.Int32, DBNull.Value);


                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetInformationBO entityBO = new BanquetInformationBO();
                                entityBO.Id = Convert.ToInt64(reader["Id"]);
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.Capacity = Convert.ToDecimal(reader["Capacity"].ToString());
                                entityBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.Status = Int32.Parse(reader["Status"].ToString());
                                entityBO.AccountsPostingHeadId = Convert.ToInt64(reader["AccountsPostingHeadId"].ToString());
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<BanquetInformationBO> GetAllBanquetInformation()
        {
            List<BanquetInformationBO> entityBOList = new List<BanquetInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllBanquetInformation_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetInformationBO entityBO = new BanquetInformationBO();
                                entityBO.Id = Convert.ToInt64(reader["Id"]);
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.Capacity = Convert.ToDecimal(reader["Capacity"].ToString());
                                entityBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.Status = Int32.Parse(reader["Status"].ToString());

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public bool SaveBanquetInformation(BanquetInformationBO entityBO, out Int64 tmpBanquetId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveBanquetInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, entityBO.CostCenterId);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, entityBO.Name);
                        dbSmartAspects.AddInParameter(command, "@UnitPrice", DbType.Decimal, entityBO.UnitPrice);
                        dbSmartAspects.AddInParameter(command, "@Capacity", DbType.Decimal, entityBO.Capacity);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, entityBO.Description);
                        dbSmartAspects.AddInParameter(command, "@Status", DbType.Boolean, entityBO.Status);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int64, entityBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@Id", DbType.Int64, sizeof(Int64));
                        if (entityBO.AccountsPostingHeadId != 0)
                            dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int64, entityBO.AccountsPostingHeadId);
                        else
                            dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int64, DBNull.Value);
                        if (entityBO.ExpenseAccountsPostingHeadId != 0)
                            dbSmartAspects.AddInParameter(command, "@ExpenseAccountsPostingHeadId", DbType.Int64, entityBO.ExpenseAccountsPostingHeadId);
                        else
                            dbSmartAspects.AddInParameter(command, "@ExpenseAccountsPostingHeadId", DbType.Int64, DBNull.Value);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpBanquetId = Convert.ToInt64(command.Parameters["@Id"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public bool UpdateBanquetInfo(BanquetInformationBO entityBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateBanquetInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, entityBO.Id);
                        dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, entityBO.CostCenterId);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, entityBO.Name);
                        dbSmartAspects.AddInParameter(command, "@UnitPrice", DbType.String, entityBO.UnitPrice);
                        dbSmartAspects.AddInParameter(command, "@Capacity", DbType.String, entityBO.Capacity);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, entityBO.Description);

                        if (entityBO.AccountsPostingHeadId != 0)
                            dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int64, entityBO.AccountsPostingHeadId);
                        else
                            dbSmartAspects.AddInParameter(command, "@AccountsPostingHeadId", DbType.Int64, DBNull.Value);
                        if (entityBO.ExpenseAccountsPostingHeadId != 0)
                            dbSmartAspects.AddInParameter(command, "@ExpenseAccountsPostingHeadId", DbType.Int64, entityBO.ExpenseAccountsPostingHeadId);
                        else
                            dbSmartAspects.AddInParameter(command, "@ExpenseAccountsPostingHeadId", DbType.Int64, DBNull.Value);
                        dbSmartAspects.AddInParameter(command, "@Status", DbType.Int32, entityBO.Status);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int64, entityBO.LastModifiedBy);

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
        public BanquetInformationBO GetBanquetInformationById(Int64 EditId)
        {
            BanquetInformationBO entityBO = new BanquetInformationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, EditId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.Id = Convert.ToInt64(reader["Id"]);
                                entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());
                                entityBO.Capacity = Convert.ToDecimal(reader["Capacity"].ToString());
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.AccountsPostingHeadId = Convert.ToInt64(reader["AccountsPostingHeadId"].ToString());
                                entityBO.ExpenseAccountsPostingHeadId = Convert.ToInt64(reader["ExpenseAccountsPostingHeadId"].ToString());
                                entityBO.Status = Int32.Parse(reader["Status"].ToString());
                            }
                        }
                    }
                }
            }
            return entityBO;
        }
        public List<BanquetClientRevenueReportBO> GetBanquetClientRevenueReport(string reportFor, string reportDurationName, string reportYear)
        {
            List<BanquetClientRevenueReportBO> banquetClientRevenue = new List<BanquetClientRevenueReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInnboardBanquetRevenueInfoBySearchCriteria_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@ReportFor", DbType.String, reportFor);
                    dbSmartAspects.AddInParameter(cmd, "@ReportDurationName", DbType.String, reportDurationName);
                    dbSmartAspects.AddInParameter(cmd, "@ReportYear", DbType.String, reportYear);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BanquetClientRevenue");
                    DataTable Table = ds.Tables["BanquetClientRevenue"];

                    banquetClientRevenue = Table.AsEnumerable().Select(r => new BanquetClientRevenueReportBO
                    {
                        ReportTitle = r.Field<string>("ReportTitle"),
                        HeadName = r.Field<string>("HeadName"),
                        TotalAmount = r.Field<decimal?>("TotalAmount")

                    }).ToList();
                }
            }

            return banquetClientRevenue;
        }
        public List<BanquetClientRevenueReportBO> GetBanquetRevenueInfoForBarNLineChartReport(string reportFor, string reportDurationName, string reportYear)
        {
            List<BanquetClientRevenueReportBO> banquetClientRevenue = new List<BanquetClientRevenueReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInnboardBanquetRevenueInfoBySearchCriteria_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@ReportFor", DbType.String, reportFor);
                    dbSmartAspects.AddInParameter(cmd, "@ReportDurationName", DbType.String, reportDurationName);
                    dbSmartAspects.AddInParameter(cmd, "@ReportYear", DbType.String, reportYear);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BanquetClientRevenue");
                    DataTable Table = ds.Tables["BanquetClientRevenue"];

                    banquetClientRevenue = Table.AsEnumerable().Select(r => new BanquetClientRevenueReportBO
                    {
                        ReportTitle = r.Field<string>("ReportTitle"),
                        HeadName = r.Field<string>("HeadName"),
                        TotalAmount = r.Field<decimal?>("TotalAmount"),
                        ServiceName = r.Field<string>("ServiceName"),
                        MonthName = r.Field<string>("MonthName"),
                        MonthValue = r.Field<int?>("MonthValue")

                    }).ToList();
                }
            }

            return banquetClientRevenue;
        }
        public List<BanquetInformationBO> GetBanquetInfoForCalender()
        {
            List<BanquetInformationBO> banquetList = new List<BanquetInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBanquetInfoForCalender_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BanquetInformationBO banquet = new BanquetInformationBO();

                                banquet.Id = Convert.ToInt64(reader["Id"]);
                                banquet.Name = reader["Name"].ToString();
                                banquet.Capacity = Convert.ToDecimal(reader["Capacity"]);

                                banquetList.Add(banquet);
                            }
                        }
                    }
                }
            }
            return banquetList;
        }

    }
}
