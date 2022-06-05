using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.GeneralLedger;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.GeneralLedger
{
    public class GLReportDA : BaseService
    {
        public List<FixedAssetsReportViewBO> GetFixedAssetsStatement(DateTime fromDate, DateTime toDate, string projectId)
        {
            List<FixedAssetsReportViewBO> fixedAssetsStat = new List<FixedAssetsReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFixedAssetsInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.String, projectId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "FixedAssets");
                    DataTable Table = ds.Tables["FixedAssets"];
                    fixedAssetsStat = Table.AsEnumerable().Select(r =>
                                new FixedAssetsReportViewBO
                                {
                                    CompanyCode = r.Field<string>("CompanyCode"),
                                    CompanyName = r.Field<string>("CompanyName"),
                                    ProjectCode = r.Field<string>("ProjectCode"),
                                    ProjectName = r.Field<string>("ProjectName"),
                                    BlockA = r.Field<string>("BlockA"),
                                    BlockB = r.Field<decimal>("BlockB"),
                                    BlockC = r.Field<decimal>("BlockC"),
                                    BlockD = r.Field<decimal>("BlockD"),
                                    BlockE = r.Field<decimal>("BlockE"),
                                    BlockF = r.Field<decimal>("BlockF"),
                                    BlockG = r.Field<decimal>("BlockG"),
                                    BlockH = r.Field<decimal>("BlockH"),
                                    BlockI = r.Field<decimal>("BlockI")
                                }).ToList();
                }
            }
            return fixedAssetsStat;
        }
        public List<NotesBreakDownReportViewBO> GetNotesBreakDownInfo(DateTime fromDate, DateTime toDate, string projectId, string notesNo, int isConfigurable)
        {
            List<NotesBreakDownReportViewBO> notesBreakDown = new List<NotesBreakDownReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBreakdownStatementByNoteForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.String, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@NotesNumber", DbType.String, notesNo);
                    dbSmartAspects.AddInParameter(cmd, "@IsConfigurable", DbType.Int32, isConfigurable);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "NotesBreakDown");
                    DataTable Table = ds.Tables["NotesBreakDown"];
                    notesBreakDown = Table.AsEnumerable().Select(r =>
                                new NotesBreakDownReportViewBO
                                {
                                    CompanyCode = r.Field<string>("CompanyCode"),
                                    CompanyName = r.Field<string>("CompanyName"),
                                    ProjectCode = r.Field<string>("ProjectCode"),
                                    ProjectName = r.Field<string>("ProjectName"),
                                    NotesName = r.Field<string>("NotesName"),
                                    NodeId = r.Field<Int32>("NodeId"),
                                    NodeHead = r.Field<string>("NodeHead"),
                                    NodeNumber = r.Field<string>("NodeNumber"),
                                    ReceivedAmount = r.Field<decimal>("ReceivedAmount"),
                                    PaidAmount = r.Field<decimal>("PaidAmount")
                                }).ToList();
                }
            }
            return notesBreakDown;
        }
        public List<ReceivablePayableReportViewBO> GetPayableOrReceivableInfo(DateTime fromDate, DateTime toDate, string projectId, string reportType)
        {
            List<ReceivablePayableReportViewBO> payOrReceiveBO = new List<ReceivablePayableReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBreakDownStatementInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.String, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayableOrReceivable");
                    DataTable Table = ds.Tables["PayableOrReceivable"];
                    payOrReceiveBO = Table.AsEnumerable().Select(r =>
                                new ReceivablePayableReportViewBO
                                {
                                    CompanyCode = r.Field<string>("CompanyCode"),
                                    CompanyName = r.Field<string>("CompanyName"),
                                    ProjectCode = r.Field<string>("ProjectCode"),
                                    ProjectName = r.Field<string>("ProjectName"),
                                    BSGroupId = r.Field<int>("BSGroupId"),
                                    BSGroupHead = r.Field<string>("BSGroupHead"),
                                    NodeGroupId = r.Field<int>("NodeGroupId"),
                                    NodeGroupHead = r.Field<string>("NodeGroupHead"),
                                    HeadId = r.Field<int>("HeadId"),
                                    NodeHead = r.Field<string>("NodeHead"),
                                    Amount = r.Field<decimal>("Amount"),
                                    NotesNumber = r.Field<string>("NotesNumber")
                                }).ToList();
                }
            }
            return payOrReceiveBO;
        }
        public List<TrialBalanceReportViewBO> GetTrialBalanceInfo(DateTime fromDate, DateTime toDate, Int32 companyId, Int32 projectId, Int32 donorId, string showZeroTransaction, string withOrWithoutOpening)
        {
            List<TrialBalanceReportViewBO> trialBalanceBO = new List<TrialBalanceReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTrialBalanceInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@VoucherDateFrom", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@VoucherDateTo", DbType.DateTime, toDate.Date);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ShowZeroTransaction", DbType.String, showZeroTransaction);

                    dbSmartAspects.AddInParameter(cmd, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "TrialBalance");
                    DataTable Table = ds.Tables["TrialBalance"];

                    trialBalanceBO = Table.AsEnumerable().Select(r =>
                                new TrialBalanceReportViewBO
                                {
                                    NodeId = r.Field<long>("NodeId"),
                                    NodeNumber = r.Field<string>("NodeNumber"),
                                    NodeHead = r.Field<string>("NodeHead"),
                                    HeadType = r.Field<string>("HeadType"),
                                    OpeningBalance = r.Field<decimal?>("OpeningBalance"),
                                    DRAmount = r.Field<decimal?>("DRAmount"),
                                    CRAmount = r.Field<decimal?>("CRAmount"),
                                    ClosingBalance = r.Field<decimal?>("ClosingBalance")

                                }).ToList();
                }
            }
            return trialBalanceBO;
        }
        public List<TrialBalanceReportViewBO> GetTrialBalanceInfoDateRangeWise(DateTime fromDate, DateTime toDate, Int32 companyId, Int32 projectId, Int32 donorId, string showZeroTransaction, string withOrWithoutOpening)
        {
            List<TrialBalanceReportViewBO> trialBalanceBO = new List<TrialBalanceReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTrialBalanceInfoDateRangeWiseForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@VoucherDateFrom", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@VoucherDateTo", DbType.DateTime, toDate.Date);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ShowZeroTransaction", DbType.String, showZeroTransaction);

                    dbSmartAspects.AddInParameter(cmd, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "TrialBalance");
                    DataTable Table = ds.Tables["TrialBalance"];

                    trialBalanceBO = Table.AsEnumerable().Select(r =>
                                new TrialBalanceReportViewBO
                                {
                                    NodeId = r.Field<long>("NodeId"),
                                    NodeNumber = r.Field<string>("NodeNumber"),
                                    NodeHead = r.Field<string>("NodeHead"),
                                    HeadType = r.Field<string>("HeadType"),
                                    OpeningBalance = r.Field<decimal?>("OpeningBalance"),
                                    DRAmount = r.Field<decimal?>("DRAmount"),
                                    CRAmount = r.Field<decimal?>("CRAmount"),
                                    ClosingBalance = r.Field<decimal?>("ClosingBalance")

                                }).ToList();
                }
            }
            return trialBalanceBO;
        }
        public List<TrialBalanceReportViewBO> GetTrialBalanceDetails(DateTime fromDate, DateTime toDate, Int32 companyId, Int32 projectId, Int32 donorId, string showZeroTransaction, string withOrWithoutOpening)
        {
            List<TrialBalanceReportViewBO> trialBalanceBO = new List<TrialBalanceReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTrialBalanceDetailsReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@VoucherDateFrom", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@VoucherDateTo", DbType.DateTime, toDate.Date);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ShowZeroTransaction", DbType.String, showZeroTransaction);
                    dbSmartAspects.AddInParameter(cmd, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "TrialBalance");
                    DataTable Table = ds.Tables["TrialBalance"];

                    trialBalanceBO = Table.AsEnumerable().Select(r =>
                                new TrialBalanceReportViewBO
                                {
                                    NodeId = r.Field<long>("NodeId"),
                                    NodeNumber = r.Field<string>("NodeNumber"),
                                    NodeHead = r.Field<string>("NodeHead"),
                                    HeadType = r.Field<string>("HeadType"),
                                    OpeningBalance = r.Field<decimal?>("OpeningBalance"),
                                    DRAmount = r.Field<decimal?>("DRAmount"),
                                    CRAmount = r.Field<decimal?>("CRAmount"),
                                    ClosingBalance = r.Field<decimal?>("ClosingBalance"),

                                    GroupNodeNumber = r.Field<string>("GroupNodeNumber"),
                                    GroupNodeHead = r.Field<string>("GroupNodeHead"),
                                    NodeOrder = r.Field<int?>("NodeOrder"),
                                    NodeLevel = r.Field<int?>("NodeLevel"),
                                    GroupOrder = r.Field<int?>("GroupOrder"),
                                    GroupLevel = r.Field<int?>("GroupLevel"),
                                    GroupId = r.Field<int?>("GroupId")

                                }).ToList();
                }
            }
            return trialBalanceBO;
        }
        public List<TrialBalanceReportViewBO> GetTrialBalanceDetailsDaterangeWise(DateTime fromDate, DateTime toDate, Int32 companyId, Int32 projectId, Int32 donorId, string showZeroTransaction, string withOrWithoutOpening)
        {
            List<TrialBalanceReportViewBO> trialBalanceBO = new List<TrialBalanceReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTrialBalanceDetailsReportDateRangeWise_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@VoucherDateFrom", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@VoucherDateTo", DbType.DateTime, toDate.Date);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ShowZeroTransaction", DbType.String, showZeroTransaction);
                    dbSmartAspects.AddInParameter(cmd, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "TrialBalance");
                    DataTable Table = ds.Tables["TrialBalance"];

                    trialBalanceBO = Table.AsEnumerable().Select(r =>
                                new TrialBalanceReportViewBO
                                {
                                    NodeId = r.Field<long>("NodeId"),
                                    NodeNumber = r.Field<string>("NodeNumber"),
                                    NodeHead = r.Field<string>("NodeHead"),
                                    HeadType = r.Field<string>("HeadType"),
                                    OpeningBalance = r.Field<decimal?>("OpeningBalance"),
                                    DRAmount = r.Field<decimal?>("DRAmount"),
                                    CRAmount = r.Field<decimal?>("CRAmount"),
                                    ClosingBalance = r.Field<decimal?>("ClosingBalance"),

                                    GroupNodeNumber = r.Field<string>("GroupNodeNumber"),
                                    GroupNodeHead = r.Field<string>("GroupNodeHead"),
                                    NodeOrder = r.Field<int?>("NodeOrder"),
                                    NodeLevel = r.Field<int?>("NodeLevel"),
                                    GroupOrder = r.Field<int?>("GroupOrder"),
                                    GroupLevel = r.Field<int?>("GroupLevel"),
                                    GroupId = r.Field<int?>("GroupId")

                                }).ToList();
                }
            }
            return trialBalanceBO;
        }
        public List<CashFlowStatReportViewBO> GetCashFlowStatInfo(DateTime fromDate, DateTime toDate, Int32 companyId, Int32 projectId, Int32 donorId, string url, string withOrWithoutOpening)
        {
            List<CashFlowStatReportViewBO> cashFlowBO = new List<CashFlowStatReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCashFlowStatementInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate.Date);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@Url", DbType.String, url);
                    dbSmartAspects.AddInParameter(cmd, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "CashFlowStatement");
                    DataTable Table = ds.Tables["CashFlowStatement"];
                    cashFlowBO = Table.AsEnumerable().Select(r =>
                                new CashFlowStatReportViewBO
                                {
                                    GroupId = r.Field<int?>("GroupId"),
                                    GroupName = r.Field<string>("GroupName"),
                                    ParentNodeId = r.Field<int?>("ParentNodeId"),
                                    ParentNodeHead = r.Field<string>("ParentNodeHead"),
                                    NotesNumber = r.Field<string>("NotesNumber"),
                                    DRAmount = r.Field<decimal?>("DRAmount"),
                                    CRAmount = r.Field<decimal?>("CRAmount"),
                                    Balance = r.Field<decimal?>("Balance")

                                }).ToList();
                }
            }
            return cashFlowBO;
        }
        public List<CashFlowStatReportViewBO> GetCashFlowStatementInfoForReportDateRangeWise(DateTime fromDate, DateTime toDate, Int32 companyId, Int32 projectId, Int32 donorId, string url, string withOrWithoutOpening)
        {
            List<CashFlowStatReportViewBO> cashFlowBO = new List<CashFlowStatReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCashFlowStatementInfoForReportDateRangeWise_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate.Date);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@Url", DbType.String, url);
                    dbSmartAspects.AddInParameter(cmd, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "CashFlowStatement");
                    DataTable Table = ds.Tables["CashFlowStatement"];
                    cashFlowBO = Table.AsEnumerable().Select(r =>
                                new CashFlowStatReportViewBO
                                {
                                    GroupId = r.Field<int?>("GroupId"),
                                    GroupName = r.Field<string>("GroupName"),
                                    ParentNodeId = r.Field<int?>("ParentNodeId"),
                                    ParentNodeHead = r.Field<string>("ParentNodeHead"),
                                    NotesNumber = r.Field<string>("NotesNumber"),
                                    DRAmount = r.Field<decimal?>("DRAmount"),
                                    CRAmount = r.Field<decimal?>("CRAmount"),
                                    Balance = r.Field<decimal?>("Balance")
                                }).ToList();
                }
            }
            return cashFlowBO;
        }
        public List<CashFlowStatReportViewBO> GetCashFlowStatInfoForComparson(DateTime fromDate, DateTime toDate, DateTime? fromDate2, DateTime? toDate2, Int32 companyId, Int32 projectId, Int32 donorId, string url, string withOrWithoutOpening)
        {
            List<CashFlowStatReportViewBO> cashFlowBO = new List<CashFlowStatReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCashFlowStatementComparisonReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate.Date);

                    if (fromDate2 != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate2", DbType.DateTime, fromDate2);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate2", DbType.DateTime, DBNull.Value);

                    if (toDate2 != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate2", DbType.DateTime, toDate2);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate2", DbType.DateTime, DBNull.Value);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@Url", DbType.String, url);
                    dbSmartAspects.AddInParameter(cmd, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "CashFlowStatements");
                    DataTable Table = ds.Tables["CashFlowStatements"];

                    cashFlowBO = Table.AsEnumerable().Select(r =>
                                new CashFlowStatReportViewBO
                                {
                                    GroupId = r.Field<int?>("GroupId"),
                                    GroupName = r.Field<string>("GroupName"),
                                    ParentNodeId = r.Field<int?>("ParentNodeId"),
                                    ParentNodeHead = r.Field<string>("ParentNodeHead"),
                                    NotesNumber = r.Field<string>("NotesNumber"),

                                    DRAmountCurrentYear = r.Field<decimal?>("DRAmountCurrentYear"),
                                    CRAmountCurrentYear = r.Field<decimal?>("CRAmountCurrentYear"),
                                    BalanceCurrentYear = r.Field<decimal?>("BalanceCurrentYear"),

                                    DRAmountPreviousYear = r.Field<decimal?>("DRAmountPreviousYear"),
                                    CRAmountPreviousYear = r.Field<decimal?>("CRAmountPreviousYear"),
                                    BalancePreviousYear = r.Field<decimal?>("BalancePreviousYear"),

                                    CurrentYearDateFrom = r.Field<DateTime>("CurrentYearDateFrom"),
                                    CurrentYearDateTo = r.Field<DateTime>("CurrentYearDateTo"),

                                    PreviousYearDateFrom = r.Field<DateTime>("PreviousYearDateFrom"),
                                    PreviousYearDateTo = r.Field<DateTime>("PreviousYearDateTo")

                                }).ToList();
                }
            }
            return cashFlowBO;
        }
        public List<ChequeListStatReportViewBO> GetChequeListStatInfo(DateTime fromDate, DateTime toDate, int nodeId, string projectId)
        {
            List<ChequeListStatReportViewBO> chequeListBO = new List<ChequeListStatReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCheckListInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@NodeId", DbType.String, nodeId);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.String, projectId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ChequeListStatement");
                    DataTable Table = ds.Tables["ChequeListStatement"];
                    chequeListBO = Table.AsEnumerable().Select(r =>
                                new ChequeListStatReportViewBO
                                {
                                    CompanyCode = r.Field<string>("CompanyCode"),
                                    CompanyName = r.Field<string>("CompanyName"),
                                    ProjectCode = r.Field<string>("ProjectCode"),
                                    ProjectName = r.Field<string>("ProjectName"),
                                    VoucherDate = r.Field<string>("VoucherDate"),
                                    NodeHead = r.Field<string>("NodeHead"),
                                    Narration = r.Field<string>("Narration"),
                                    ChequeNumber = r.Field<string>("ChequeNumber"),
                                    VoucherNo = r.Field<string>("VoucherNo"),
                                    LedgerAmount = r.Field<decimal>("LedgerAmount")
                                }).ToList();
                }
            }
            return chequeListBO;
        }
        public List<ProfitNLossReportViewBO> GetProfitNLossStatInfo(DateTime fromDate, DateTime toDate, decimal incomeTaxPercentage, Int32 companyId, Int32 projectId, Int32 donorId, string url, string withOrWithoutOpening)
        {
            List<ProfitNLossReportViewBO> profitNLossBO = new List<ProfitNLossReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProfitNLossStatementInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@IncomeTaxPercentage", DbType.Decimal, incomeTaxPercentage);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@Url", DbType.String, url);
                    dbSmartAspects.AddInParameter(cmd, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ProfitNLossStatement");
                    DataTable Table = ds.Tables["ProfitNLossStatement"];
                    profitNLossBO = Table.AsEnumerable().Select(r =>
                                new ProfitNLossReportViewBO
                                {
                                    GroupId = r.Field<int?>("GroupId"),
                                    HeadDescription = r.Field<string>("HeadDescription"),
                                    NodeId = r.Field<int?>("NodeId"),
                                    Lvl = r.Field<int?>("Lvl"),
                                    Notes = r.Field<string>("Notes"),
                                    Amount = r.Field<decimal?>("Amount"),
                                    AmountToDisplay = r.Field<decimal?>("AmountToDisplay")

                                }).ToList();
                }
            }
            return profitNLossBO;
        }
        public List<ProfitNLossReportViewBO> GetProfitNLossStatementInfoForReportDateRangeWise(DateTime fromDate, DateTime toDate, decimal incomeTaxPercentage, Int32 companyId, Int32 projectId, Int32 donorId, string url, string withOrWithoutOpening)
        {
            List<ProfitNLossReportViewBO> profitNLossBO = new List<ProfitNLossReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProfitNLossStatementInfoForReportDateRangeWise_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@IncomeTaxPercentage", DbType.Decimal, incomeTaxPercentage);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@Url", DbType.String, url);
                    dbSmartAspects.AddInParameter(cmd, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ProfitNLossStatement");
                    DataTable Table = ds.Tables["ProfitNLossStatement"];
                    profitNLossBO = Table.AsEnumerable().Select(r =>
                                new ProfitNLossReportViewBO
                                {
                                    GroupId = r.Field<int?>("GroupId"),
                                    HeadDescription = r.Field<string>("HeadDescription"),
                                    NodeId = r.Field<int?>("NodeId"),
                                    Lvl = r.Field<int?>("Lvl"),
                                    Notes = r.Field<string>("Notes"),
                                    Amount = r.Field<decimal?>("Amount"),
                                    AmountToDisplay = r.Field<decimal?>("AmountToDisplay")

                                }).ToList();
                }
            }
            return profitNLossBO;
        }
        public List<ProfitNLossReportViewBO> GetProfitNLossStatementComparison(DateTime fromDate, DateTime toDate, DateTime? fromDate2, DateTime? toDate2, decimal incomeTaxPercentage, Int32 companyId, Int32 projectId, Int32 donorId, string url, string withOrWithoutOpening)
        {
            List<ProfitNLossReportViewBO> profitNLossBO = new List<ProfitNLossReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProfitNLossStatementComparisonReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate.Date);

                    if (fromDate2 != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate2", DbType.DateTime, fromDate2);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate2", DbType.DateTime, DBNull.Value);

                    if (toDate2 != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate2", DbType.DateTime, toDate2);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate2", DbType.DateTime, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@IncomeTaxPercentage", DbType.Decimal, incomeTaxPercentage);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@Url", DbType.String, url);
                    dbSmartAspects.AddInParameter(cmd, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ProfitNLossStatement");
                    DataTable Table = ds.Tables["ProfitNLossStatement"];

                    profitNLossBO = Table.AsEnumerable().Select(r =>
                                new ProfitNLossReportViewBO
                                {
                                    GroupId = r.Field<int?>("GroupId"),
                                    HeadDescription = r.Field<string>("HeadDescription"),
                                    NodeId = r.Field<int?>("NodeId"),
                                    Lvl = r.Field<int?>("Lvl"),
                                    Notes = r.Field<string>("Notes"),

                                    AmountCurrentYear = r.Field<decimal?>("AmountCurrentYear"),
                                    AmountToDisplayCurrentYear = r.Field<decimal?>("AmountToDisplayCurrentYear"),

                                    AmountPreviousYear = r.Field<decimal?>("AmountPreviousYear"),
                                    AmountToDisplayPreviousYear = r.Field<decimal?>("AmountToDisplayPreviousYear"),

                                    CurrentYearDateFrom = r.Field<DateTime?>("CurrentYearDateFrom"),
                                    CurrentYearDateTo = r.Field<DateTime?>("CurrentYearDateTo"),

                                    PreviousYearDateFrom = r.Field<DateTime?>("PreviousYearDateFrom"),
                                    PreviousYearDateTo = r.Field<DateTime?>("PreviousYearDateTo")

                                }).ToList();
                }
            }
            return profitNLossBO;
        }
        public List<ProfitNLossReportViewBO> GetProfitNLossStatementInfoForNonProfitOrganization(DateTime fromDate, DateTime toDate, decimal incomeTaxPercentage, Int32 companyId, Int32 projectId, Int32 donorId, string url, string withOrWithoutOpening)
        {
            List<ProfitNLossReportViewBO> profitNLossBO = new List<ProfitNLossReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProfitNLossStatementInfoForNonProfitOrganizationReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@IncomeTaxPercentage", DbType.Decimal, incomeTaxPercentage);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@Url", DbType.String, url);
                    dbSmartAspects.AddInParameter(cmd, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ProfitNLossStatement");
                    DataTable Table = ds.Tables["ProfitNLossStatement"];
                    profitNLossBO = Table.AsEnumerable().Select(r =>
                                new ProfitNLossReportViewBO
                                {
                                    GroupId = r.Field<int?>("GroupId"),
                                    HeadDescription = r.Field<string>("HeadDescription"),
                                    NodeId = r.Field<int?>("NodeId"),
                                    Lvl = r.Field<int?>("Lvl"),
                                    Notes = r.Field<string>("Notes"),
                                    Amount = r.Field<decimal?>("Amount"),
                                    AmountToDisplay = r.Field<decimal?>("AmountToDisplay")

                                }).ToList();
                }
            }
            return profitNLossBO;
        }
        public List<BudgetStatementReportDTO> GetBudgetStatement(int fiscalYearId, Int32 companyId, Int32 projectId, Int32 donorId)
        {
            List<BudgetStatementReportDTO> trialBalanceBO = new List<BudgetStatementReportDTO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("BudgetStatementReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);

                    dbSmartAspects.AddInParameter(cmd, "@FiscalYearId", DbType.Int64, fiscalYearId);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.Int64, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "Budget");
                    DataTable Table = ds.Tables["Budget"];

                    trialBalanceBO = Table.AsEnumerable().Select(r =>
                                new BudgetStatementReportDTO
                                {
                                    GroupId = r.Field<int?>("GroupId"),
                                    GroupNodeHead = r.Field<string>("GroupNodeHead"),
                                    GroupOrder = r.Field<int?>("GroupOrder"),
                                    GroupLevel = r.Field<int?>("GroupLevel"),
                                    NodeId = r.Field<long?>("NodeId"),
                                    NodeHead = r.Field<string>("NodeHead"),
                                    NodeNumber = r.Field<string>("NodeNumber"),
                                    NodeOrder = r.Field<int?>("NodeOrder"),
                                    BudgetParentId = r.Field<int?>("BudgetParentId"),
                                    BudgetParentName = r.Field<string>("BudgetParentName"),
                                    BudgetGroupId = r.Field<int?>("BudgetGroupId"),
                                    BudgetGroupName = r.Field<string>("BudgetGroupName"),
                                    BudgetChildGroupId = r.Field<int?>("BudgetChildGroupId"),
                                    BudgetChildGroupName = r.Field<string>("BudgetChildGroupName"),
                                    Amount = r.Field<decimal?>("Amount")

                                }).ToList();
                }
            }
            return trialBalanceBO;
        }
    }
}
