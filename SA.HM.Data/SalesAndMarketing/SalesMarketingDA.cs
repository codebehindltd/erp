using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.SalesAndMarketing;
using System.Data.Common;
using System.Data;


namespace HotelManagement.Data.SalesAndMarketing
{
    public class SalesMarketingDA : BaseService
    {
        public bool SaveLogKeeping(SMLogKeepingBO salesCallBO, out long LogKeepingId)
        {
            bool status = false;
            LogKeepingId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveLogKeeping_SP"))
                {

                    dbSmartAspects.AddInParameter(command, "@Type", DbType.String, salesCallBO.Type);
                    dbSmartAspects.AddInParameter(command, "@Title", DbType.String, salesCallBO.Title);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, salesCallBO.Description);
                    dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, salesCallBO.CompanyId);
                    dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int64, salesCallBO.ContactId);
                    dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int64, salesCallBO.DealId);
                    dbSmartAspects.AddInParameter(command, "@SalesCallEntryId", DbType.Int64, salesCallBO.SalesCallEntryId);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, salesCallBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    LogKeepingId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                }

            }
            return status;
        }
        public bool SaveLogKeepingContacts(long LogKeepingId, long ContactId)
        {
            bool status = false;
            //LogKeepingId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveLogKeepingContacts_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@LogKeepingId", DbType.Int64, LogKeepingId);
                        dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int64, ContactId);

                        status = dbSmartAspects.ExecuteNonQuery(command, transaction) > 0 ? true : false;
                    }

                    if (status)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
            }
            return status;
        }
        public List<SMCompanySiteBO> GetSite()
        {
            List<SMCompanySiteBO> siteBOs = new List<SMCompanySiteBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSiteInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMCompanySiteBO bO = new SMCompanySiteBO
                                {
                                    SiteId = Convert.ToInt32(reader["SiteId"]),
                                    SiteName = Convert.ToString(reader["SiteName"])
                                };

                                siteBOs.Add(bO);
                            }
                        }
                    }
                }
            }
            return siteBOs;
        }
        public List<SMDeal> GetDealInfo()
        {
            List<SMDeal> siteBOs = new List<SMDeal>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDealInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMDeal bO = new SMDeal
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Name = Convert.ToString(reader["Name"])
                                };

                                siteBOs.Add(bO);
                            }
                        }
                    }
                }
            }
            return siteBOs;
        }
        public List<SMLogKeepingBO> GetSalesMarketingLogActivity(int? companyId, Int64? contactId, Int64? dealId,
                                                                string logType, int? userId, DateTime? dateFrom, DateTime? dateTo)
        {
            List<SMLogKeepingBO> logKeepingList = new List<SMLogKeepingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesMarketingLogActivity_SP"))
                {
                    if (companyId != null)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    if (contactId != null)
                        dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int64, contactId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ContactId", DbType.Int64, DBNull.Value);

                    if (dealId != null)
                        dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.Int64, dealId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.Int64, DBNull.Value);

                    if (!string.IsNullOrEmpty(logType))
                        dbSmartAspects.AddInParameter(cmd, "@LogType", DbType.String, logType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LogType", DbType.String, DBNull.Value);

                    if (userId != null)
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, DBNull.Value);

                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RequsitionDetails");
                    DataTable Table = ds.Tables["RequsitionDetails"];

                    logKeepingList = Table.AsEnumerable().Select(r => new SMLogKeepingBO
                    {
                        Id = r.Field<long>("Id"),
                        Type = r.Field<string>("Type"),
                        Title = r.Field<string>("Title"),
                        Description = r.Field<string>("Description"),
                        LogDateTime = r.Field<DateTime>("LogDateTime"),
                        CompanyId = r.Field<Int32?>("CompanyId"),
                        ContactId = r.Field<Int64?>("ContactId"),
                        DealId = r.Field<Int64?>("DealId"),
                        SalesCallEntryId = r.Field<Int64?>("SalesCallEntryId"),
                        CreatedBy = r.Field<Int32>("CreatedBy")

                    }).OrderByDescending(d => d.Id).ToList();
                }
            }
            return logKeepingList;
        }
        public List<SMLogKeepingBO> GetSalesMarketingLogActivity(long salesCallEntryId)
        {
            List<SMLogKeepingBO> logKeepingList = new List<SMLogKeepingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalesMarketingLogActivityBySalesCallId_SP"))
                {
                    

                    if (salesCallEntryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@SalesCallId", DbType.Int64, salesCallEntryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SalesCallId", DbType.Int64, DBNull.Value);

                    

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RequsitionDetails");
                    DataTable Table = ds.Tables["RequsitionDetails"];

                    logKeepingList = Table.AsEnumerable().Select(r => new SMLogKeepingBO
                    {
                        Id = r.Field<long>("Id"),
                        Type = r.Field<string>("Type"),
                        Title = r.Field<string>("Title"),
                        Description = r.Field<string>("Description"),
                        LogDateTime = r.Field<DateTime>("LogDateTime"),
                        CompanyId = r.Field<Int32?>("CompanyId"),
                        ContactId = r.Field<Int64?>("ContactId"),
                        DealId = r.Field<Int64?>("DealId"),
                        SalesCallEntryId = r.Field<Int64?>("SalesCallEntryId"),
                        CreatedBy = r.Field<Int32>("CreatedBy")

                    }).OrderByDescending(d => d.Id).ToList();
                }
            }
            return logKeepingList;
        }
        public List<SMCompanyTypeInformationBO> GetCompanyTypeForDDL()
        {
            List<SMCompanyTypeInformationBO> typeBOs = new List<SMCompanyTypeInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyTypeForDDL_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMCompanyTypeInformationBO sMLife = new SMCompanyTypeInformationBO()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    TypeName = reader["TypeName"].ToString()
                                };
                                typeBOs.Add(sMLife);
                            }
                        }
                    }
                }
            }

            return typeBOs;
        }
        public List<SMOwnershipInformationBO> GetOwnershipInfoForDDL()
        {
            List<SMOwnershipInformationBO> ownershipInfoBOs = new List<SMOwnershipInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOwnershipInfoForDDL_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMOwnershipInformationBO sMLife = new SMOwnershipInformationBO()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    OwnershipName = reader["OwnershipName"].ToString()
                                };
                                ownershipInfoBOs.Add(sMLife);
                            }
                        }
                    }
                }
            }

            return ownershipInfoBOs;
        }
        //public List<SMCompanyTypeInformationBO> GetCompanyTypeForDDL()
        //{
        //    List<SMCompanyTypeInformationBO> typeBOs = new List<SMCompanyTypeInformationBO>();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyTypeForDDL_SP"))
        //        {
        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        SMCompanyTypeInformationBO sMLife = new SMCompanyTypeInformationBO()
        //                        {
        //                            Id = Convert.ToInt32(reader["Id"]),
        //                            TypeName = reader["TypeName"].ToString()
        //                        };
        //                        typeBOs.Add(sMLife);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return typeBOs;
        //}

    }
}
