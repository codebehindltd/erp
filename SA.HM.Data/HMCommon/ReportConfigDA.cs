using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HMCommon
{
    public class ReportConfigDA : BaseService
    {
        public List<ReportConfigMasterBO> GetReportCaptionByReportTypeAndIsParentFlag(Int64 reportTypeId, bool isParent = true)
        {
            string query = string.Format("SELECT * FROM CommonReportConfigMaster WHERE ReportTypeId = {0} AND IsParent = {1}", reportTypeId, (isParent ? 1 : 0));
            List<ReportConfigMasterBO> parentCaption = new List<ReportConfigMasterBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ParentCaption");
                    DataTable Table = ds.Tables["ParentCaption"];
                    parentCaption = Table.AsEnumerable().Select(r =>
                                new ReportConfigMasterBO
                                {
                                    Id = r.Field<Int64>("Id"),
                                    ReportTypeId = r.Field<int>("ReportTypeId"),
                                    AncestorId = r.Field<Int64?>("AncestorId"),
                                    Caption = r.Field<string>("Caption"),
                                    SortingOrder = r.Field<Int16>("SortingOrder"),
                                    Lvl = r.Field<Int64?>("Lvl"),
                                    Hierarchy = r.Field<string>("Hierarchy"),
                                    HierarchyIndex = r.Field<Int64?>("HierarchyIndex"),
                                    IsParent = r.Field<bool?>("IsParent"),
                                    NodeType = r.Field<string>("NodeType")

                                }).ToList();
                }
            }
            return parentCaption;
        }

        public List<ReportConfigMasterBO> GetReportCaptionByReportType(Int64 reportTypeId)
        {
            string query = string.Format("SELECT * FROM CommonReportConfigMaster WHERE ReportTypeId = {0}", reportTypeId);
            List<ReportConfigMasterBO> parentCaption = new List<ReportConfigMasterBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ParentCaption");
                    DataTable Table = ds.Tables["ParentCaption"];
                    parentCaption = Table.AsEnumerable().Select(r =>
                                new ReportConfigMasterBO
                                {
                                    Id = r.Field<Int64>("Id"),
                                    ReportTypeId = r.Field<int>("ReportTypeId"),
                                    AncestorId = r.Field<Int64?>("AncestorId"),
                                    Caption = r.Field<string>("Caption"),
                                    SortingOrder = r.Field<Int16>("SortingOrder"),
                                    Lvl = r.Field<Int64?>("Lvl"),
                                    Hierarchy = r.Field<string>("Hierarchy"),
                                    HierarchyIndex = r.Field<Int64?>("HierarchyIndex"),
                                    IsParent = r.Field<bool?>("IsParent"),
                                    NodeType = r.Field<string>("NodeType")

                                }).ToList();
                }
            }
            return parentCaption;
        }

        public bool SaveReportConfigMaster(ReportConfigMasterBO reportConfigMaster, out Int64 reportConfigId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveReportConfigMaster_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ReportTypeId", DbType.String, reportConfigMaster.ReportTypeId);

                    if (reportConfigMaster.AncestorId != null)
                    {
                        dbSmartAspects.AddInParameter(command, "@AncestorId", DbType.String, reportConfigMaster.AncestorId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(command, "@AncestorId", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(command, "@Caption", DbType.String, reportConfigMaster.Caption);
                    dbSmartAspects.AddInParameter(command, "@SortingOrder", DbType.String, reportConfigMaster.SortingOrder);
                    dbSmartAspects.AddInParameter(command, "@IsParent", DbType.String, reportConfigMaster.IsParent);
                    dbSmartAspects.AddInParameter(command, "@NodeType", DbType.String, reportConfigMaster.NodeType);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, reportConfigMaster.CreatedBy);

                    dbSmartAspects.AddOutParameter(command, "@ReportConfigId", DbType.Int64, sizeof(Int64));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    reportConfigId = Convert.ToInt64(command.Parameters["@ReportConfigId"].Value);
                }
            }
            return status;
        }

        public bool UpdateReportConfigMaster(ReportConfigMasterBO reportConfigMaster)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateReportConfigMaster_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, reportConfigMaster.Id);
                    dbSmartAspects.AddInParameter(command, "@ReportTypeId", DbType.String, reportConfigMaster.ReportTypeId);

                    if (reportConfigMaster.AncestorId != null)
                    {
                        dbSmartAspects.AddInParameter(command, "@AncestorId", DbType.String, reportConfigMaster.AncestorId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(command, "@AncestorId", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(command, "@Caption", DbType.String, reportConfigMaster.Caption);
                    dbSmartAspects.AddInParameter(command, "@SortingOrder", DbType.String, reportConfigMaster.SortingOrder);
                    dbSmartAspects.AddInParameter(command, "@IsParent", DbType.String, reportConfigMaster.IsParent);
                    dbSmartAspects.AddInParameter(command, "@NodeType", DbType.String, reportConfigMaster.NodeType);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, reportConfigMaster.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                }
            }
            return status;
        }

        public bool SaveUpdateReportConfigDetails(List<ReportConfigDetailsEditBO> reportConfigDetails, List<ReportConfigDetailsEditBO> deletedConfigDetails)
        {
            int status = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (reportConfigDetails != null)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("SaveReportConfigDetails_SP"))
                            {
                                foreach (ReportConfigDetailsEditBO rcd in reportConfigDetails.Where(d => d.Id == 0))
                                {
                                    if (rcd.Id == 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ReportConfigId", DbType.Int64, rcd.ReportConfigId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@NodeId", DbType.Int64, rcd.NodeId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@NodeName", DbType.String, rcd.NodeName);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@SortingOrder", DbType.Int16, rcd.SortingOrder);

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }
                        }

                        if (reportConfigDetails != null)
                        {
                            using (DbCommand commandPercentageDiscount = dbSmartAspects.GetStoredProcCommand("UpdateReportConfigDetails_SP"))
                            {
                                foreach (ReportConfigDetailsEditBO rcd in reportConfigDetails.Where(d => d.Id > 0))
                                {
                                    if (rcd.Id > 0)
                                    {
                                        commandPercentageDiscount.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@Id", DbType.Int64, rcd.Id);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@ReportConfigId", DbType.Int64, rcd.ReportConfigId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@NodeId", DbType.Int64, rcd.NodeId);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@NodeName", DbType.String, rcd.NodeName);
                                        dbSmartAspects.AddInParameter(commandPercentageDiscount, "@SortingOrder", DbType.Int16, rcd.SortingOrder);

                                        status = dbSmartAspects.ExecuteNonQuery(commandPercentageDiscount, transction);
                                    }
                                }
                            }
                        }

                        if (status > 0 && deletedConfigDetails != null)
                        {
                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (ReportConfigDetailsEditBO rcd in deletedConfigDetails)
                                {
                                    commandGuestBillPayment.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TableName", DbType.String, "CommonReportConfigDetails");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKField", DbType.String, "Id");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@TablePKId", DbType.String, rcd.Id);

                                    status = dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                }
                            }                            
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }

                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                }
            }

            return retVal;
        }

        public List<ReportConfigMasterBO> GetReportConfigBySearch(string searchType, Int64 reportTypeId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<ReportConfigMasterBO> reportConfig = new List<ReportConfigMasterBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReportConfigBySearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchType", DbType.String, searchType);
                    dbSmartAspects.AddInParameter(cmd, "@ReportTypeId", DbType.Int64, reportTypeId);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ReportConfig");
                    DataTable Table = ds.Tables["ReportConfig"];

                    reportConfig = Table.AsEnumerable().Select(r => new ReportConfigMasterBO
                    {
                        Id = r.Field<Int64>("Id"),
                        ReportTypeId = r.Field<int>("ReportTypeId"),
                        AncestorId = r.Field<Int64?>("AncestorId"),
                        Caption = r.Field<string>("Caption"),
                        SortingOrder = r.Field<Int16>("SortingOrder"),
                        Lvl = r.Field<Int64?>("Lvl"),
                        Hierarchy = r.Field<string>("Hierarchy"),
                        HierarchyIndex = r.Field<Int64?>("HierarchyIndex"),
                        IsParent = r.Field<bool?>("IsParent"),
                        NodeType = r.Field<string>("NodeType")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return reportConfig;
        }

        public List<ReportConfigDetailsEditBO> GetReportConfigDetailsByReportTypeAndConfigId(Int64 reportConfigId, Int64 reportTypeId)
        {
            List<ReportConfigDetailsEditBO> parentCaption = new List<ReportConfigDetailsEditBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReportConfigDetailsByReportTypeAndConfigId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReportConfigId", DbType.String, reportConfigId);
                    dbSmartAspects.AddInParameter(cmd, "@ReportTypeId", DbType.Int64, reportTypeId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ParentCaption");
                    DataTable Table = ds.Tables["ParentCaption"];
                    parentCaption = Table.AsEnumerable().Select(r =>
                                new ReportConfigDetailsEditBO
                                {
                                    ReportConfigId = r.Field<Int64>("ReportConfigId"),
                                    ReportTypeId = r.Field<int>("ReportTypeId"),
                                    Id = r.Field<Int64>("Id"),
                                    NodeId = r.Field<Int64>("NodeId"),
                                    NodeName = r.Field<string>("NodeName"),
                                    SortingOrder = r.Field<Int16>("SortingOrder"),
                                    NodeType = r.Field<string>("NodeType")

                                }).ToList();
                }
            }
            return parentCaption;
        }
    }
}
