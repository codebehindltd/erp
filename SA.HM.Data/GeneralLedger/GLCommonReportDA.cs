using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Data.GeneralLedger
{
    public class GLCommonReportDA : BaseService
    {
        public Boolean GenerateGeneralLedgerInfo(DateTime mFromDate, DateTime mToDate, int mNodeId, int mCreatedBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGeneralLedgerInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, mFromDate);
                    dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, mToDate);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int32, mNodeId);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, mCreatedBy);
                    //dbSmartAspects.AddOutParameter(command, "@mError", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    // mError = Convert.ToInt32(command.Parameters["@mError"].Value);
                }
            }
            return status;
        }
        public List<GLJournalRegisterInfoForReportBO> GetJournalRegisterInfo(DateTime fromDate, DateTime toDate, string voucherStatus, string projectId, string donorId)
        {
            List<GLJournalRegisterInfoForReportBO> entityList = new List<GLJournalRegisterInfoForReportBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetJournalRegisterInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@GLStauts", DbType.String, voucherStatus);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.String, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@DonorId", DbType.String, donorId);

                    DataSet VoucherDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, VoucherDS, "VoucherRegister");
                    DataTable Table = VoucherDS.Tables["VoucherRegister"];

                    GLJournalRegisterInfoForReportBO entityBO;
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO = new GLJournalRegisterInfoForReportBO();

                                entityBO.DealId = Convert.ToInt32(reader["DealId"]);
                                entityBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                entityBO.CompanyCode = reader["CompanyCode"].ToString();
                                entityBO.CompanyName = reader["CompanyName"].ToString();
                                entityBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                entityBO.ProjectCode = reader["ProjectCode"].ToString();
                                entityBO.ProjectName = reader["ProjectName"].ToString();
                                entityBO.VoucherMode = Convert.ToByte(reader["VoucherMode"]);
                                entityBO.VoucherNo = reader["VoucherNo"].ToString();
                                entityBO.VoucherDate = Convert.ToString(reader["VoucherDate"]);
                                entityBO.Narration = reader["Narration"].ToString();
                                entityBO.LedgerId = Convert.ToInt32(reader["LedgerId"]);
                                entityBO.LedgerAmount = Convert.ToDecimal(reader["LedgerAmount"]);
                                entityBO.NodeNarration = reader["NodeNarration"].ToString();
                                entityBO.NodeId = Convert.ToInt64(reader["NodeId"]);
                                entityBO.LedgerMode = Convert.ToByte(reader["LedgerMode"]);
                                entityBO.NodeHead = reader["NodeHead"].ToString();
                                entityBO.NodeNumber = reader["NodeNumber"].ToString();
                                entityBO.ChequeNumber = reader["ChequeNumber"].ToString();
                                entityBO.NodeMode = Convert.ToBoolean(reader["NodeMode"]);
                                entityBO.Amount = Convert.ToDecimal(reader["Amount"]);
                                entityBO.DebitAmount = Convert.ToDecimal(reader["DebitAmount"]);
                                entityBO.CreditAmount = Convert.ToDecimal(reader["CreditAmount"]);
                                entityBO.InWordAmount = reader["InWordAmount"].ToString();
                                entityBO.CashChequeMode = Convert.ToByte(reader["CashChequeMode"]);
                                entityBO.VcheqNo = reader["VcheqNo"].ToString();

                                entityList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityList;
        }
        public List<ReceiveNPaymentInfoForReportBO> GetReceiveNPaymentInfo(DateTime fromDate, DateTime toDate, string projectId)
        {
            List<ReceiveNPaymentInfoForReportBO> entityBOList = new List<ReceiveNPaymentInfoForReportBO>();

            DataSet entityBODS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetReceiveNPaymentInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, toDate.Date);
                    dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, projectId);

                    dbSmartAspects.LoadDataSet(command, entityBODS, "ReceiveNPayment");
                    DataTable table = entityBODS.Tables["ReceiveNPayment"];

                    entityBOList = table.AsEnumerable().Select(r => new ReceiveNPaymentInfoForReportBO
                    {
                        CompanyCode = r.Field<string>("CompanyCode"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ProjectCode = r.Field<string>("ProjectCode"),
                        ProjectName = r.Field<string>("ProjectName"),
                        CBCode = r.Field<long>("CBCode"),
                        PartNo = r.Field<byte?>("PartNo"),
                        VoucherMode = r.Field<string>("VoucherMode"),
                        VoucherNumber = r.Field<string>("VoucherNumber"),
                        NodeId = r.Field<int?>("NodeId"),
                        NodeHead = r.Field<string>("NodeHead"),
                        HierarchyIndex = r.Field<string>("HierarchyIndex"),
                        PriorBalance = r.Field<decimal?>("PriorBalance"),
                        ReceivedAmount = r.Field<decimal?>("ReceivedAmount"),
                        PaidAmount = r.Field<decimal?>("PaidAmount"),
                        VoucherDate = r.Field<string>("VoucherDate"),
                        Balance = r.Field<decimal?>("Balance")

                    }).ToList();

                    entityBODS.Dispose();
                }
            }

            return entityBOList;
        }
        public List<CashNBankBookStatementInfoForReportBO> GetCashNBankBookStatementInfo(DateTime fromDate, DateTime toDate, string nodeId, string projectId, string reportType)
        {
            List<CashNBankBookStatementInfoForReportBO> entityBOList = new List<CashNBankBookStatementInfoForReportBO>();

            DataSet entityBODS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetCashNBankBookStatementInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, toDate.Date);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.String, nodeId);
                    dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, projectId);
                    dbSmartAspects.AddInParameter(command, "@ReportType", DbType.String, reportType);

                    dbSmartAspects.LoadDataSet(command, entityBODS, "CashNBankBookStatement");
                    DataTable table = entityBODS.Tables["CashNBankBookStatement"];

                    entityBOList = table.AsEnumerable().Select(r => new CashNBankBookStatementInfoForReportBO
                    {
                        NameOfAccount = r.Field<string>("NameOfAccount"),
                        CompanyCode = r.Field<string>("CompanyCode"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ProjectCode = r.Field<string>("ProjectCode"),
                        ProjectName = r.Field<string>("ProjectName"),
                        CBCode = r.Field<long>("CBCode"),
                        NodeId = r.Field<long?>("NodeId"),
                        AncestorId = r.Field<long?>("AncestorId"),
                        NodeNumber = r.Field<string>("NodeNumber"),
                        NodeHead = r.Field<string>("NodeHead"),
                        Lvl = r.Field<int>("Lvl"),
                        Hierarchy = r.Field<string>("Hierarchy"),
                        HierarchyIndex = r.Field<string>("HierarchyIndex"),
                        NodeMode = r.Field<byte?>("NodeMode"),
                        CostCentreId = r.Field<int?>("CostCentreId"),
                        DealId = r.Field<int?>("DealId"),
                        LedgerId = r.Field<int?>("LedgerId"),
                        VoucherType = r.Field<string>("VoucherType"),
                        VoucherNo = r.Field<string>("VoucherNo"),
                        VoucherDate = r.Field<string>("VoucherDate"),
                        CounterPart = r.Field<string>("CounterPart"),
                        ChequeNumber = r.Field<string>("ChequeNumber"),
                        LedgerMode = r.Field<byte?>("LedgerMode"),
                        PriorBalance = r.Field<decimal?>("PriorBalance"),
                        ReceivedAmount = r.Field<decimal?>("ReceivedAmount"),
                        PaidAmount = r.Field<decimal?>("PaidAmount"),
                        NodeNarration = r.Field<string>("NodeNarration"),
                        Balance = r.Field<decimal?>("Balance")

                    }).ToList();

                    entityBODS.Dispose();
                }
            }

            return entityBOList;
        }
        public List<CashNBankBookStatementInfoForReportBO> GetGeneralLedgerStatementInfo(DateTime fromDate, DateTime toDate, string nodeId, string projectId)
        {
            List<CashNBankBookStatementInfoForReportBO> entityBOList = new List<CashNBankBookStatementInfoForReportBO>();

            DataSet entityBODS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetGeneralLedgerStatementInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.String, nodeId);
                    dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, projectId);

                    dbSmartAspects.LoadDataSet(command, entityBODS, "CashNBankBookStatement");
                    DataTable table = entityBODS.Tables["CashNBankBookStatement"];

                    entityBOList = table.AsEnumerable().Select(r => new CashNBankBookStatementInfoForReportBO
                    {
                        NameOfAccount = r.Field<string>("NameOfAccount"),
                        CompanyCode = r.Field<string>("CompanyCode"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ProjectCode = r.Field<string>("ProjectCode"),
                        ProjectName = r.Field<string>("ProjectName"),
                        CBCode = r.Field<long>("CBCode"),
                        NodeId = r.Field<long?>("NodeId"),
                        AncestorId = r.Field<long?>("AncestorId"),
                        NodeNumber = r.Field<string>("NodeNumber"),
                        NodeHead = r.Field<string>("NodeHead"),
                        Lvl = r.Field<int>("Lvl"),
                        Hierarchy = r.Field<string>("Hierarchy"),
                        HierarchyIndex = r.Field<string>("HierarchyIndex"),
                        NodeMode = r.Field<byte?>("NodeMode"),
                        CostCentreId = r.Field<int?>("CostCentreId"),
                        DealId = r.Field<int?>("DealId"),
                        LedgerId = r.Field<int?>("LedgerId"),
                        VoucherType = r.Field<string>("VoucherType"),
                        VoucherNo = r.Field<string>("VoucherNo"),
                        VoucherDate = r.Field<string>("VoucherDate"),
                        CounterPart = r.Field<string>("CounterPart"),
                        ChequeNumber = r.Field<string>("ChequeNumber"),
                        LedgerMode = r.Field<byte?>("LedgerMode"),
                        PriorBalance = r.Field<decimal?>("PriorBalance"),
                        ReceivedAmount = r.Field<decimal?>("ReceivedAmount"),
                        PaidAmount = r.Field<decimal?>("PaidAmount"),
                        NodeNarration = r.Field<string>("NodeNarration"),
                        Balance = r.Field<decimal?>("Balance")

                    }).ToList();

                    entityBODS.Dispose();
                }
            }

            return entityBOList;
        }
        public List<BalanceSheetStatementInfoForReportBO> GetBalanceSheetStatementInfo(DateTime fromDate, DateTime toDate, Int32 companyId, Int32 projectId, Int32 donorId, string url, string withOrWithoutOpening, string reortType)
        {
            List<BalanceSheetStatementInfoForReportBO> balanceSheet = new List<BalanceSheetStatementInfoForReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBalanceSheetStatementInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ReortType", DbType.String, reortType);
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
                    dbSmartAspects.LoadDataSet(cmd, ds, "BalanceSheetStatement");
                    DataTable Table = ds.Tables["BalanceSheetStatement"];

                    balanceSheet = Table.AsEnumerable().Select(r =>
                                new BalanceSheetStatementInfoForReportBO
                                {
                                    ParentGroupId = r.Field<int?>("ParentGroupId"),
                                    ParentGroup = r.Field<string>("ParentGroup"),
                                    GroupId = r.Field<int?>("GroupId"),
                                    Particulars = r.Field<string>("Particulars"),
                                    NodeId = r.Field<Int64?>("NodeId"),
                                    Notes = r.Field<string>("Notes"),
                                    Amount = r.Field<decimal?>("Amount"),
                                    Lvl = r.Field<int?>("Lvl")

                                }).ToList();
                }
            }

            return balanceSheet;
        }

        public List<BalanceSheetStatementInfoForReportBO> GetBalanceSheetStatementInfoForReportDateRangeWise(DateTime fromDate, DateTime toDate, Int32 companyId, Int32 projectId, Int32 donorId, string url, string withOrWithoutOpening)
        {
            List<BalanceSheetStatementInfoForReportBO> balanceSheet = new List<BalanceSheetStatementInfoForReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetBalanceSheetStatementInfoForReportDateRangeWise_SP"))
                {
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
                    dbSmartAspects.LoadDataSet(cmd, ds, "BalanceSheetStatement");
                    DataTable Table = ds.Tables["BalanceSheetStatement"];

                    balanceSheet = Table.AsEnumerable().Select(r =>
                                new BalanceSheetStatementInfoForReportBO
                                {
                                    ParentGroupId = r.Field<int?>("ParentGroupId"),
                                    ParentGroup = r.Field<string>("ParentGroup"),
                                    GroupId = r.Field<int?>("GroupId"),
                                    Particulars = r.Field<string>("Particulars"),
                                    NodeId = r.Field<Int64?>("NodeId"),
                                    Notes = r.Field<string>("Notes"),
                                    Amount = r.Field<decimal?>("Amount")

                                }).ToList();
                }
            }

            return balanceSheet;
        }


        public List<GLReportDynamicallyForReportBO> GetGLReportDynamically(DateTime fromDate, DateTime toDate, string projectId, string url, string reportType)
        {
            List<GLReportDynamicallyForReportBO> balanceSheet = new List<GLReportDynamicallyForReportBO>();

            DataSet entityBODS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetGLReportDynamicallyForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(command, "@ToDate", DbType.DateTime, toDate);
                    //dbSmartAspects.AddInParameter(command, "@CompanyName", DbType.String, companyName);
                    //dbSmartAspects.AddInParameter(command, "@CompanyAddress", DbType.String, companyAddress);
                    //dbSmartAspects.AddInParameter(command, "@CompanyWeb", DbType.String, companyWeb);
                    dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, projectId);
                    dbSmartAspects.AddInParameter(command, "@Url", DbType.String, url);
                    dbSmartAspects.AddInParameter(command, "@ReportType", DbType.String, reportType);

                    dbSmartAspects.LoadDataSet(command, entityBODS, "GL");
                    DataTable table = entityBODS.Tables["GL"];

                    balanceSheet = table.AsEnumerable().Select(r => new GLReportDynamicallyForReportBO
                    {
                        //HMCompanyProfile = r.Field<string>("HMCompanyProfile"),
                        //HMCompanyAddress = r.Field<string>("HMCompanyAddress"),
                        //HMCompanyWeb = r.Field<string>("HMCompanyWeb"),
                        CompanyCode = r.Field<string>("CompanyCode"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ProjectCode = r.Field<string>("ProjectCode"),
                        ProjectName = r.Field<string>("ProjectName"),
                        RCId = r.Field<long>("RCId"),
                        ReportType = r.Field<string>("ReportType"),
                        AccountType = r.Field<string>("AccountType"),
                        Lvl = r.Field<int?>("Lvl"),
                        NodeHead = r.Field<string>("NodeHead"),
                        NodeNumber = r.Field<string>("NodeNumber"),
                        GroupName = r.Field<string>("GroupName"),
                        CalculatedNodeAmount = r.Field<decimal>("CalculatedNodeAmount"),
                        CalculationType = r.Field<string>("CalculationType"),
                        IsActiveLinkUrl = r.Field<bool?>("IsActiveLinkUrl"),
                        Url = r.Field<string>("Url")

                    }).ToList();

                    entityBODS.Dispose();
                }
            }

            return balanceSheet;
        }
        public List<LedgerBookReportBO> GetLedgerBookReport(DateTime voucherDateFrom, DateTime voucherDateTo, Int64 nodeId, Int32 companyId, Int32 projectId, Int32 donorId, string withOrWithoutOpening)
        {
            List<LedgerBookReportBO> entityBOList = new List<LedgerBookReportBO>();

            DataSet entityBODS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetLedgerBookReport_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@VoucherDateFrom", DbType.DateTime, voucherDateFrom.Date);
                    dbSmartAspects.AddInParameter(command, "@VoucherDateTo", DbType.DateTime, voucherDateTo.Date);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int64, nodeId);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(command, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    dbSmartAspects.LoadDataSet(command, entityBODS, "LedgerBook");
                    DataTable table = entityBODS.Tables["LedgerBook"];

                    entityBOList = table.AsEnumerable().Select(r => new LedgerBookReportBO
                    {
                        NodeId = r.Field<Int64?>("NodeId"),
                        NodeNumber = r.Field<string>("NodeNumber"),
                        NodeHead = r.Field<string>("NodeHead"),
                        DRAmount = r.Field<decimal?>("DRAmount"),
                        CRAmount = r.Field<decimal?>("CRAmount"),
                        BalanceAmount = r.Field<decimal?>("BalanceAmount"),
                        NodeBalanceAmount = r.Field<decimal?>("NodeBalanceAmount"),
                        ParentNodeId = r.Field<Int64?>("ParentNodeId"),
                        ParentNodeHead = r.Field<string>("ParentNodeHead"),
                        ParentNodeNumber = r.Field<string>("ParentNodeNumber")

                    }).ToList();

                    entityBODS.Dispose();
                }
            }

            return entityBOList;
        }

        public List<LedgerBookReportBO> GetLedgerBookReportDaterangeWise(DateTime voucherDateFrom, DateTime voucherDateTo, Int64 nodeId, Int32 companyId, Int32 projectId, Int32 donorId, string withOrWithoutOpening)
        {
            List<LedgerBookReportBO> entityBOList = new List<LedgerBookReportBO>();

            DataSet entityBODS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetLedgerBookReportDaterangeWise_SP"))
                {
                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(command, "@VoucherDateFrom", DbType.DateTime, voucherDateFrom.Date);
                    dbSmartAspects.AddInParameter(command, "@VoucherDateTo", DbType.DateTime, voucherDateTo.Date);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int64, nodeId);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(command, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    dbSmartAspects.LoadDataSet(command, entityBODS, "LedgerBook");
                    DataTable table = entityBODS.Tables["LedgerBook"];

                    entityBOList = table.AsEnumerable().Select(r => new LedgerBookReportBO
                    {
                        NodeId = r.Field<Int64?>("NodeId"),
                        NodeNumber = r.Field<string>("NodeNumber"),
                        NodeHead = r.Field<string>("NodeHead"),
                        DRAmount = r.Field<decimal?>("DRAmount"),
                        CRAmount = r.Field<decimal?>("CRAmount"),
                        BalanceAmount = r.Field<decimal?>("BalanceAmount"),
                        NodeBalanceAmount = r.Field<decimal?>("NodeBalanceAmount"),
                        ParentNodeId = r.Field<Int64?>("ParentNodeId"),
                        ParentNodeHead = r.Field<string>("ParentNodeHead"),
                        ParentNodeNumber = r.Field<string>("ParentNodeNumber")

                    }).ToList();

                    entityBODS.Dispose();
                }
            }

            return entityBOList;
        }

        public List<LedgerBookReportBO> GetGroupLedgerBookReport(DateTime voucherDateFrom, DateTime voucherDateTo, Int64 nodeId, Int32 companyId, Int32 projectId, Int32 donorId, string withOrWithoutOpening)
        {
            List<LedgerBookReportBO> entityBOList = new List<LedgerBookReportBO>();

            DataSet entityBODS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetGroupLedgerBookReport_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@VoucherDateFrom", DbType.DateTime, voucherDateFrom.Date);
                    dbSmartAspects.AddInParameter(command, "@VoucherDateTo", DbType.DateTime, voucherDateTo.Date);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int64, nodeId);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(command, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    dbSmartAspects.LoadDataSet(command, entityBODS, "LedgerBook");
                    DataTable table = entityBODS.Tables["LedgerBook"];

                    entityBOList = table.AsEnumerable().Select(r => new LedgerBookReportBO
                    {
                        NodeId = r.Field<Int64?>("NodeId"),
                        NodeNumber = r.Field<string>("NodeNumber"),
                        NodeHead = r.Field<string>("NodeHead"),
                        Narration = r.Field<string>("Narration"),
                        OpeningBalance = r.Field<decimal?>("OpeningBalance"),
                        DRAmount = r.Field<decimal?>("DRAmount"),
                        CRAmount = r.Field<decimal?>("CRAmount"),
                        ClosingBalance = r.Field<decimal?>("ClosingBalance"),
                        ParentNodeId = r.Field<Int64?>("ParentNodeId"),
                        ParentNodeHead = r.Field<string>("ParentNodeHead"),
                        ParentNodeNumber = r.Field<string>("ParentNodeNumber")

                    }).ToList();

                    entityBODS.Dispose();
                }
            }

            return entityBOList;
        }

        public List<LedgerBookReportBO> GetGroupLedgerBookReportDateRangeWise(DateTime voucherDateFrom, DateTime voucherDateTo, Int64 nodeId, Int32 companyId, Int32 projectId, Int32 donorId, string withOrWithoutOpening)
        {
            List<LedgerBookReportBO> entityBOList = new List<LedgerBookReportBO>();

            DataSet entityBODS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetGroupLedgerBookReportDateRangeWise_SP"))
                {
                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(command, "@VoucherDateFrom", DbType.DateTime, voucherDateFrom.Date);
                    dbSmartAspects.AddInParameter(command, "@VoucherDateTo", DbType.DateTime, voucherDateTo.Date);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int64, nodeId);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(command, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    dbSmartAspects.LoadDataSet(command, entityBODS, "LedgerBook");
                    DataTable table = entityBODS.Tables["LedgerBook"];

                    entityBOList = table.AsEnumerable().Select(r => new LedgerBookReportBO
                    {
                        NodeId = r.Field<Int64?>("NodeId"),
                        NodeNumber = r.Field<string>("NodeNumber"),
                        NodeHead = r.Field<string>("NodeHead"),
                        Narration = r.Field<string>("Narration"),
                        OpeningBalance = r.Field<decimal?>("OpeningBalance"),
                        DRAmount = r.Field<decimal?>("DRAmount"),
                        CRAmount = r.Field<decimal?>("CRAmount"),
                        ClosingBalance = r.Field<decimal?>("ClosingBalance"),
                        ParentNodeId = r.Field<Int64?>("ParentNodeId"),
                        ParentNodeHead = r.Field<string>("ParentNodeHead"),
                        ParentNodeNumber = r.Field<string>("ParentNodeNumber")

                    }).ToList();

                    entityBODS.Dispose();
                }
            }

            return entityBOList;
        }


        public List<GroupLedgerDetailsBO> GetGroupLedgerDetailsReport(DateTime voucherDateFrom, DateTime voucherDateTo, Int64 nodeId, Int32 companyId, Int32 projectId, Int32 donorId, string withOrWithoutOpening)
        {
            List<GroupLedgerDetailsBO> groupLedger = new List<GroupLedgerDetailsBO>();

            DataSet entityBODS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetGroupLedgerDetailsReport_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@VoucherDateFrom", DbType.DateTime, voucherDateFrom.Date);
                    dbSmartAspects.AddInParameter(command, "@VoucherDateTo", DbType.DateTime, voucherDateTo.Date);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int64, nodeId);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(command, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    dbSmartAspects.LoadDataSet(command, entityBODS, "LedgerBook");
                    DataTable table = entityBODS.Tables["LedgerBook"];

                    groupLedger = table.AsEnumerable().Select(r => new GroupLedgerDetailsBO
                    {
                        LedgerMasterId = r.Field<Int64?>("LedgerMasterId"),
                        VoucherDate = r.Field<DateTime?>("VoucherDate"),
                        VoucherType = r.Field<string>("VoucherType"),
                        VoucherNo = r.Field<string>("VoucherNo"),
                        NodeId = r.Field<Int64?>("NodeId"),
                        NodeHead = r.Field<string>("NodeHead"),
                        NodeNumber = r.Field<string>("NodeNumber"),
                        DRAmount = r.Field<decimal?>("DRAmount"),
                        CRAmount = r.Field<decimal?>("CRAmount"),
                        TransactionalBalance = r.Field<decimal?>("TransactionalBalance"),
                        ClosingBalance = r.Field<decimal?>("ClosingBalance"),
                        CommulativeBalance = r.Field<decimal?>("CommulativeBalance"),
                        Narration = r.Field<string>("Narration"),
                        NodeNarration = r.Field<string>("NodeNarration"),
                        NodeType = r.Field<string>("NodeType"),
                        IsTransactionalHead = r.Field<bool?>("IsTransactionalHead"),
                        GroupNodeId = r.Field<Int64?>("GroupNodeId"),
                        GroupNodeNumber = r.Field<string>("GroupNodeNumber"),
                        GroupNodeHead = r.Field<string>("GroupNodeHead"),
                        IsTransactionalHeadGroup = r.Field<bool?>("IsTransactionalHeadGroup"),
                        Rnk = r.Field<int?>("Rnk"),
                        LedgerNodeId = r.Field<Int64?>("LedgerNodeId"),
                        LedgerNodeHead = r.Field<string>("LedgerNodeHead"),
                        LedgerNodeNumber = r.Field<string>("LedgerNodeNumber")

                    }).ToList();

                    entityBODS.Dispose();
                }
            }

            return groupLedger;
        }

        public List<GroupLedgerDetailsBO> GetGroupLedgerDetailsReportDateRangeWise(DateTime voucherDateFrom, DateTime voucherDateTo, Int64 nodeId, Int32 companyId, Int32 projectId, Int32 donorId, string withOrWithoutOpening)
        {
            List<GroupLedgerDetailsBO> groupLedger = new List<GroupLedgerDetailsBO>();

            DataSet entityBODS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetGroupLedgerDetailsReportDateRangeWise_SP"))
                {
                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(command, "@VoucherDateFrom", DbType.DateTime, voucherDateFrom.Date);
                    dbSmartAspects.AddInParameter(command, "@VoucherDateTo", DbType.DateTime, voucherDateTo.Date);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int64, nodeId);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(command, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    dbSmartAspects.LoadDataSet(command, entityBODS, "LedgerBook");
                    DataTable table = entityBODS.Tables["LedgerBook"];

                    groupLedger = table.AsEnumerable().Select(r => new GroupLedgerDetailsBO
                    {
                        LedgerMasterId = r.Field<Int64?>("LedgerMasterId"),
                        VoucherDate = r.Field<DateTime?>("VoucherDate"),
                        VoucherType = r.Field<string>("VoucherType"),
                        VoucherNo = r.Field<string>("VoucherNo"),
                        NodeId = r.Field<Int64?>("NodeId"),
                        NodeHead = r.Field<string>("NodeHead"),
                        NodeNumber = r.Field<string>("NodeNumber"),
                        DRAmount = r.Field<decimal?>("DRAmount"),
                        CRAmount = r.Field<decimal?>("CRAmount"),
                        TransactionalBalance = r.Field<decimal?>("TransactionalBalance"),
                        ClosingBalance = r.Field<decimal?>("ClosingBalance"),
                        CommulativeBalance = r.Field<decimal?>("CommulativeBalance"),
                        Narration = r.Field<string>("Narration"),
                        NodeNarration = r.Field<string>("NodeNarration"),
                        NodeType = r.Field<string>("NodeType"),
                        IsTransactionalHead = r.Field<bool?>("IsTransactionalHead"),
                        GroupNodeId = r.Field<Int64?>("GroupNodeId"),
                        GroupNodeNumber = r.Field<string>("GroupNodeNumber"),
                        GroupNodeHead = r.Field<string>("GroupNodeHead"),
                        IsTransactionalHeadGroup = r.Field<bool?>("IsTransactionalHeadGroup"),
                        Rnk = r.Field<int?>("Rnk"),
                        LedgerNodeId = r.Field<Int64?>("LedgerNodeId"),
                        LedgerNodeHead = r.Field<string>("LedgerNodeHead"),
                        LedgerNodeNumber = r.Field<string>("LedgerNodeNumber")

                    }).ToList();

                    entityBODS.Dispose();
                }
            }

            return groupLedger;
        }

        public List<GroupLedgerDetailsBO> GetIndividualLedgerDetailsReport(DateTime voucherDateFrom, DateTime voucherDateTo, Int64 nodeId, Int32 companyId, Int32 projectId, Int32 donorId, string withOrWithoutOpening)
        {
            List<GroupLedgerDetailsBO> groupLedger = new List<GroupLedgerDetailsBO>();

            DataSet entityBODS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetIndividualLedgerDetailsReport_SP"))
                {
                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(command, "@VoucherDateFrom", DbType.DateTime, voucherDateFrom.Date);
                    dbSmartAspects.AddInParameter(command, "@VoucherDateTo", DbType.DateTime, voucherDateTo.Date);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int64, nodeId);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(command, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    dbSmartAspects.LoadDataSet(command, entityBODS, "LedgerBook");
                    DataTable table = entityBODS.Tables["LedgerBook"];

                    groupLedger = table.AsEnumerable().Select(r => new GroupLedgerDetailsBO
                    {
                        LedgerMasterId = r.Field<Int64?>("LedgerMasterId"),
                        VoucherDate = r.Field<DateTime?>("VoucherDate"),
                        VoucherType = r.Field<string>("VoucherType"),
                        VoucherNo = r.Field<string>("VoucherNo"),
                        NodeId = r.Field<Int64?>("NodeId"),
                        NodeHead = r.Field<string>("NodeHead"),
                        NodeNumber = r.Field<string>("NodeNumber"),
                        DRAmount = r.Field<decimal?>("DRAmount"),
                        CRAmount = r.Field<decimal?>("CRAmount"),
                        TransactionalBalance = r.Field<decimal?>("TransactionalBalance"),
                        ClosingBalance = r.Field<decimal?>("ClosingBalance"),
                        CommulativeBalance = r.Field<decimal?>("CommulativeBalance"),
                        Narration = r.Field<string>("Narration"),
                        NodeNarration = r.Field<string>("NodeNarration"),
                        LedgerNodeId = r.Field<Int64?>("LedgerNodeId"),
                        LedgerNodeHead = r.Field<string>("LedgerNodeHead"),
                        LedgerNodeNumber = r.Field<string>("LedgerNodeNumber"),
                        NodeAcccount = r.Field<string>("NodeAcccount"),
                        GroupNodeHead = r.Field<string>("GroupNodeHead")

                    }).ToList();

                    entityBODS.Dispose();
                }
            }

            return groupLedger;
        }


        public List<GroupLedgerDetailsBO> GetIndividualLedgerDetailsReportDateRangeWise(DateTime voucherDateFrom, DateTime voucherDateTo, Int64 nodeId, Int32 companyId, Int32 projectId, Int32 donorId, string withOrWithoutOpening)
        {
            List<GroupLedgerDetailsBO> groupLedger = new List<GroupLedgerDetailsBO>();

            DataSet entityBODS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetIndividualLedgerDetailsReportDateRangeWise_SP"))
                {
                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(command, "@VoucherDateFrom", DbType.DateTime, voucherDateFrom.Date);
                    dbSmartAspects.AddInParameter(command, "@VoucherDateTo", DbType.DateTime, voucherDateTo.Date);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int64, nodeId);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(command, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    dbSmartAspects.LoadDataSet(command, entityBODS, "LedgerBook");
                    DataTable table = entityBODS.Tables["LedgerBook"];

                    groupLedger = table.AsEnumerable().Select(r => new GroupLedgerDetailsBO
                    {
                        LedgerMasterId = r.Field<Int64?>("LedgerMasterId"),
                        VoucherDate = r.Field<DateTime?>("VoucherDate"),
                        VoucherDateDisplay = r.Field<string>("VoucherDateDisplay"),
                        VoucherType = r.Field<string>("VoucherType"),
                        VoucherNo = r.Field<string>("VoucherNo"),
                        NodeId = r.Field<Int64?>("NodeId"),
                        NodeHead = r.Field<string>("NodeHead"),
                        NodeNumber = r.Field<string>("NodeNumber"),
                        DRAmount = r.Field<decimal?>("DRAmount"),
                        CRAmount = r.Field<decimal?>("CRAmount"),
                        TransactionalBalance = r.Field<decimal?>("TransactionalBalance"),
                        ClosingBalance = r.Field<decimal?>("ClosingBalance"),
                        CommulativeBalance = r.Field<decimal?>("CommulativeBalance"),
                        Narration = r.Field<string>("Narration"),
                        ReferenceNumber = r.Field<string>("ReferenceNumber"),
                        NodeNarration = r.Field<string>("NodeNarration"),
                        LedgerNodeId = r.Field<Int64?>("LedgerNodeId"),
                        LedgerNodeHead = r.Field<string>("LedgerNodeHead"),
                        LedgerNodeNumber = r.Field<string>("LedgerNodeNumber"),
                        NodeAcccount = r.Field<string>("NodeAcccount"),
                        GroupNodeHead = r.Field<string>("GroupNodeHead")

                    }).ToList();

                    entityBODS.Dispose();
                }
            }

            return groupLedger;
        }



        public ReceiptNPaymentStatementViewBO GetReceiptNPaymentStatement(DateTime fromDate, DateTime toDate, Int32 companyId, Int32 projectId, Int32 donorId, string url, string showZeroTransaction, string withOrWithoutOpening)
        {
            ReceiptNPaymentStatementViewBO receiptNPayment = new ReceiptNPaymentStatementViewBO();
            List<ReceiptNPaymentStatementBO> receiptStatement = new List<ReceiptNPaymentStatementBO>();
            List<ReceiptNPaymentStatementBO> paymentStatement = new List<ReceiptNPaymentStatementBO>();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReceiptNPaymentStatementReport_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate.Date);
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate.Date);

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

                        dbSmartAspects.AddInParameter(cmd, "@ShowZeroTransaction", DbType.String, showZeroTransaction);
                        dbSmartAspects.AddInParameter(cmd, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "ReceiptNPayment");
                        //DataTable Table = ds.Tables["ReceiptNPayment"];

                        //Rnk GroupId GroupName NodeGroupId NodeGroupName SubGroupId  SubGroupName NodeId  NodeName 
                        //NotesNumber TransactionalBalance TransactionalOpening    OpeningBalance Amount  GroupAmount GrandAmount

                        receiptStatement = ds.Tables[0].AsEnumerable().Select(r =>
                                    new ReceiptNPaymentStatementBO
                                    {
                                        Rnk = r.Field<Int32?>("GroupId"),
                                        GroupId = r.Field<Int32?>("GroupId"),
                                        GroupName = r.Field<string>("GroupName"),
                                        NodeGroupId = r.Field<Int32?>("NodeGroupId"),
                                        NodeGroupName = r.Field<string>("NodeGroupName"),
                                        SubGroupId = r.Field<Int32?>("SubGroupId"),
                                        SubGroupName = r.Field<string>("SubGroupName"),
                                        NodeId = r.Field<Int32?>("NodeId"),
                                        NodeName = r.Field<string>("NodeName"),

                                        NotesNumber = r.Field<string>("NotesNumber"),
                                        TransactionalBalance = r.Field<decimal?>("TransactionalBalance"),
                                        TransactionalOpening = r.Field<decimal?>("TransactionalOpening"),

                                        OpeningBalance = r.Field<decimal?>("OpeningBalance"),
                                        Amount = r.Field<decimal?>("Amount"),
                                        GroupAmount = r.Field<decimal?>("GroupAmount"),
                                        GrandAmount = r.Field<decimal?>("GrandAmount"),

                                        NetIncreasesDecreasedInCashNCashEquvalent = r.Field<decimal?>("NetIncreasesDecreasedInCashNCashEquvalent"),
                                        CashNCashEquvalentAtEndOfPeriod = r.Field<decimal?>("CashNCashEquvalentAtEndOfPeriod")

                                    }).ToList();

                        paymentStatement = ds.Tables[1].AsEnumerable().Select(r =>
                                   new ReceiptNPaymentStatementBO
                                   {
                                       Rnk = r.Field<Int32?>("GroupId"),
                                       GroupId = r.Field<Int32?>("GroupId"),
                                       GroupName = r.Field<string>("GroupName"),
                                       NodeGroupId = r.Field<Int32?>("NodeGroupId"),
                                       NodeGroupName = r.Field<string>("NodeGroupName"),
                                       SubGroupId = r.Field<Int32?>("SubGroupId"),
                                       SubGroupName = r.Field<string>("SubGroupName"),
                                       NodeId = r.Field<Int32?>("NodeId"),
                                       NodeName = r.Field<string>("NodeName"),

                                       NotesNumber = r.Field<string>("NotesNumber"),
                                       TransactionalBalance = r.Field<decimal?>("TransactionalBalance"),
                                       TransactionalOpening = r.Field<decimal?>("TransactionalOpening"),

                                       OpeningBalance = r.Field<decimal?>("OpeningBalance"),
                                       Amount = r.Field<decimal?>("Amount"),
                                       GroupAmount = r.Field<decimal?>("GroupAmount"),
                                       GrandAmount = r.Field<decimal?>("GrandAmount"),

                                       NetIncreasesDecreasedInCashNCashEquvalent = r.Field<decimal?>("NetIncreasesDecreasedInCashNCashEquvalent"),
                                       CashNCashEquvalentAtEndOfPeriod = r.Field<decimal?>("CashNCashEquvalentAtEndOfPeriod")

                                   }).ToList();
                    }
                }

                receiptNPayment.ReceiptStatement = receiptStatement;
                receiptNPayment.PaymentStatement = paymentStatement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return receiptNPayment;
        }

        public ReceiptNPaymentStatementViewBO GetReceiptNPaymentStatementReportDateRangeWise(DateTime fromDate, DateTime toDate, Int32 companyId, Int32 projectId, Int32 donorId, string url, string showZeroTransaction, string withOrWithoutOpening)
        {
            ReceiptNPaymentStatementViewBO receiptNPayment = new ReceiptNPaymentStatementViewBO();
            List<ReceiptNPaymentStatementBO> receiptStatement = new List<ReceiptNPaymentStatementBO>();
            List<ReceiptNPaymentStatementBO> paymentStatement = new List<ReceiptNPaymentStatementBO>();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReceiptNPaymentStatementReportDateRangeWise_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate.Date);
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate.Date);

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

                        dbSmartAspects.AddInParameter(cmd, "@ShowZeroTransaction", DbType.String, showZeroTransaction);
                        dbSmartAspects.AddInParameter(cmd, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "ReceiptNPayment");
                        //DataTable Table = ds.Tables["ReceiptNPayment"];

                        //Rnk GroupId GroupName NodeGroupId NodeGroupName SubGroupId  SubGroupName NodeId  NodeName 
                        //NotesNumber TransactionalBalance TransactionalOpening    OpeningBalance Amount  GroupAmount GrandAmount

                        receiptStatement = ds.Tables[0].AsEnumerable().Select(r =>
                                    new ReceiptNPaymentStatementBO
                                    {
                                        Rnk = r.Field<Int32?>("GroupId"),
                                        GroupId = r.Field<Int32?>("GroupId"),
                                        GroupName = r.Field<string>("GroupName"),
                                        NodeGroupId = r.Field<Int32?>("NodeGroupId"),
                                        NodeGroupName = r.Field<string>("NodeGroupName"),
                                        SubGroupId = r.Field<Int32?>("SubGroupId"),
                                        SubGroupName = r.Field<string>("SubGroupName"),
                                        NodeId = r.Field<Int32?>("NodeId"),
                                        NodeName = r.Field<string>("NodeName"),

                                        NotesNumber = r.Field<string>("NotesNumber"),
                                        TransactionalBalance = r.Field<decimal?>("TransactionalBalance"),
                                        TransactionalOpening = r.Field<decimal?>("TransactionalOpening"),

                                        OpeningBalance = r.Field<decimal?>("OpeningBalance"),
                                        Amount = r.Field<decimal?>("Amount"),
                                        GroupAmount = r.Field<decimal?>("GroupAmount"),
                                        GrandAmount = r.Field<decimal?>("GrandAmount"),

                                        NetIncreasesDecreasedInCashNCashEquvalent = r.Field<decimal?>("NetIncreasesDecreasedInCashNCashEquvalent"),
                                        CashNCashEquvalentAtEndOfPeriod = r.Field<decimal?>("CashNCashEquvalentAtEndOfPeriod")

                                    }).ToList();

                        paymentStatement = ds.Tables[1].AsEnumerable().Select(r =>
                                   new ReceiptNPaymentStatementBO
                                   {
                                       Rnk = r.Field<Int32?>("GroupId"),
                                       GroupId = r.Field<Int32?>("GroupId"),
                                       GroupName = r.Field<string>("GroupName"),
                                       NodeGroupId = r.Field<Int32?>("NodeGroupId"),
                                       NodeGroupName = r.Field<string>("NodeGroupName"),
                                       SubGroupId = r.Field<Int32?>("SubGroupId"),
                                       SubGroupName = r.Field<string>("SubGroupName"),
                                       NodeId = r.Field<Int32?>("NodeId"),
                                       NodeName = r.Field<string>("NodeName"),

                                       NotesNumber = r.Field<string>("NotesNumber"),
                                       TransactionalBalance = r.Field<decimal?>("TransactionalBalance"),
                                       TransactionalOpening = r.Field<decimal?>("TransactionalOpening"),

                                       OpeningBalance = r.Field<decimal?>("OpeningBalance"),
                                       Amount = r.Field<decimal?>("Amount"),
                                       GroupAmount = r.Field<decimal?>("GroupAmount"),
                                       GrandAmount = r.Field<decimal?>("GrandAmount"),

                                       NetIncreasesDecreasedInCashNCashEquvalent = r.Field<decimal?>("NetIncreasesDecreasedInCashNCashEquvalent"),
                                       CashNCashEquvalentAtEndOfPeriod = r.Field<decimal?>("CashNCashEquvalentAtEndOfPeriod")

                                   }).ToList();
                    }
                }

                receiptNPayment.ReceiptStatement = receiptStatement;
                receiptNPayment.PaymentStatement = paymentStatement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return receiptNPayment;
        }


        public ReceiptNPaymentStatementViewBO GetReceiptNPaymentStatementComparison(DateTime fromDate, DateTime toDate, DateTime? fromDate2, DateTime? toDate2, Int32 companyId, Int32 projectId, Int32 donorId, string url, string withOrWithoutOpening)
        {
            ReceiptNPaymentStatementViewBO receiptNPayment = new ReceiptNPaymentStatementViewBO();
            List<ReceiptNPaymentStatementBO> receiptStatement = new List<ReceiptNPaymentStatementBO>();
            List<ReceiptNPaymentStatementBO> paymentStatement = new List<ReceiptNPaymentStatementBO>();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReceiptNPaymentComparisonReport_SP"))
                    {
                        cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate.Date);
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate.Date);

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
                        dbSmartAspects.AddInParameter(cmd, "@ShowZeroTransaction", DbType.String, "WithZero");
                        dbSmartAspects.AddInParameter(cmd, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "ReceiptNPayment");

                        receiptStatement = ds.Tables[0].AsEnumerable().Select(r =>
                                    new ReceiptNPaymentStatementBO
                                    {
                                        Rnk = r.Field<Int32?>("GroupId"),
                                        GroupId = r.Field<Int32?>("GroupId"),
                                        GroupName = r.Field<string>("GroupName"),
                                        NodeGroupId = r.Field<Int32?>("NodeGroupId"),
                                        NodeGroupName = r.Field<string>("NodeGroupName"),
                                        SubGroupId = r.Field<Int32?>("SubGroupId"),
                                        SubGroupName = r.Field<string>("SubGroupName"),
                                        NodeId = r.Field<Int32?>("NodeId"),
                                        NodeName = r.Field<string>("NodeName"),

                                        NotesNumber = r.Field<string>("NotesNumber"),
                                        TransactionalBalance = r.Field<decimal?>("TransactionalBalance"),
                                        TransactionalOpening = r.Field<decimal?>("TransactionalOpening"),

                                        OpeningBalanceCurrentYear = r.Field<decimal?>("OpeningBalanceCurrentYear"),
                                        AmountCurrentYear = r.Field<decimal?>("AmountCurrentYear"),
                                        GroupAmountCurrentYear = r.Field<decimal?>("GroupAmountCurrentYear"),
                                        GrandAmountCurrentYear = r.Field<decimal?>("GrandAmountCurrentYear"),
                                        NetIncreasesDecreasedInCashNCashEquvalentCurrentYear = r.Field<decimal?>("NetIncreasesDecreasedInCashNCashEquvalentCurrentYear"),
                                        CashNCashEquvalentAtEndOfPeriodCurrentYear = r.Field<decimal?>("CashNCashEquvalentAtEndOfPeriodCurrentYear"),

                                        OpeningBalancePreviousYear = r.Field<decimal?>("OpeningBalancePreviousYear"),
                                        AmountPreviousYear = r.Field<decimal?>("AmountPreviousYear"),
                                        GroupAmountPreviousYear = r.Field<decimal?>("GroupAmountPreviousYear"),
                                        GrandAmountPreviousYear = r.Field<decimal?>("GrandAmountPreviousYear"),
                                        NetIncreasesDecreasedInCashNCashEquvalentPreviousYear = r.Field<decimal?>("NetIncreasesDecreasedInCashNCashEquvalentPreviousYear"),
                                        CashNCashEquvalentAtEndOfPeriodPreviousYear = r.Field<decimal?>("CashNCashEquvalentAtEndOfPeriodPreviousYear"),

                                        CurrentYearDateFrom = r.Field<DateTime?>("CurrentYearDateFrom"),
                                        CurrentYearDateTo = r.Field<DateTime?>("CurrentYearDateTo"),

                                        PreviousYearDateFrom = r.Field<DateTime?>("PreviousYearDateFrom"),
                                        PreviousYearDateTo = r.Field<DateTime?>("PreviousYearDateTo")

                                    }).ToList();

                        paymentStatement = ds.Tables[1].AsEnumerable().Select(r =>
                                   new ReceiptNPaymentStatementBO
                                   {
                                       Rnk = r.Field<Int32?>("GroupId"),
                                       GroupId = r.Field<Int32?>("GroupId"),
                                       GroupName = r.Field<string>("GroupName"),
                                       NodeGroupId = r.Field<Int32?>("NodeGroupId"),
                                       NodeGroupName = r.Field<string>("NodeGroupName"),
                                       SubGroupId = r.Field<Int32?>("SubGroupId"),
                                       SubGroupName = r.Field<string>("SubGroupName"),
                                       NodeId = r.Field<Int32?>("NodeId"),
                                       NodeName = r.Field<string>("NodeName"),

                                       NotesNumber = r.Field<string>("NotesNumber"),
                                       TransactionalBalance = r.Field<decimal?>("TransactionalBalance"),
                                       TransactionalOpening = r.Field<decimal?>("TransactionalOpening"),

                                       OpeningBalanceCurrentYear = r.Field<decimal?>("OpeningBalanceCurrentYear"),
                                       AmountCurrentYear = r.Field<decimal?>("AmountCurrentYear"),
                                       GroupAmountCurrentYear = r.Field<decimal?>("GroupAmountCurrentYear"),
                                       GrandAmountCurrentYear = r.Field<decimal?>("GrandAmountCurrentYear"),
                                       NetIncreasesDecreasedInCashNCashEquvalentCurrentYear = r.Field<decimal?>("NetIncreasesDecreasedInCashNCashEquvalentCurrentYear"),
                                       CashNCashEquvalentAtEndOfPeriodCurrentYear = r.Field<decimal?>("CashNCashEquvalentAtEndOfPeriodCurrentYear"),

                                       OpeningBalancePreviousYear = r.Field<decimal?>("OpeningBalancePreviousYear"),
                                       AmountPreviousYear = r.Field<decimal?>("AmountPreviousYear"),
                                       GroupAmountPreviousYear = r.Field<decimal?>("GroupAmountPreviousYear"),
                                       GrandAmountPreviousYear = r.Field<decimal?>("GrandAmountPreviousYear"),
                                       NetIncreasesDecreasedInCashNCashEquvalentPreviousYear = r.Field<decimal?>("NetIncreasesDecreasedInCashNCashEquvalentPreviousYear"),
                                       CashNCashEquvalentAtEndOfPeriodPreviousYear = r.Field<decimal?>("CashNCashEquvalentAtEndOfPeriodPreviousYear"),

                                       CurrentYearDateFrom = r.Field<DateTime?>("CurrentYearDateFrom"),
                                       CurrentYearDateTo = r.Field<DateTime?>("CurrentYearDateTo"),

                                       PreviousYearDateFrom = r.Field<DateTime?>("PreviousYearDateFrom"),
                                       PreviousYearDateTo = r.Field<DateTime?>("PreviousYearDateTo")

                                   }).ToList();
                    }
                }

                receiptNPayment.ReceiptStatement = receiptStatement;
                receiptNPayment.PaymentStatement = paymentStatement;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return receiptNPayment;
        }

        public List<LedgerBookReportBO> GetNotesBreakdownReport(DateTime voucherDateFrom, DateTime voucherDateTo, Int64 nodeId, Int32 companyId,
                                                                Int32 projectId, Int32 donorId, string nodesId, string withOrWithoutOpening)
        {
            List<LedgerBookReportBO> entityBOList = new List<LedgerBookReportBO>();

            DataSet entityBODS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetNotesBreakdownReport_SP"))
                {
                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(command, "@VoucherDateFrom", DbType.DateTime, voucherDateFrom.Date);
                    dbSmartAspects.AddInParameter(command, "@VoucherDateTo", DbType.DateTime, voucherDateTo.Date);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int64, nodeId);

                    if (!string.IsNullOrEmpty(nodesId))
                        dbSmartAspects.AddInParameter(command, "@NodesId", DbType.String, nodesId);
                    else
                        dbSmartAspects.AddInParameter(command, "@NodesId", DbType.String, DBNull.Value);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (donorId != 0)
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, donorId);
                    else
                        dbSmartAspects.AddInParameter(command, "@DonorId", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(command, "@WithOrWithoutOpening", DbType.String, withOrWithoutOpening);

                    dbSmartAspects.LoadDataSet(command, entityBODS, "LedgerBook");
                    DataTable table = entityBODS.Tables["LedgerBook"];

                    entityBOList = table.AsEnumerable().Select(r => new LedgerBookReportBO
                    {
                        GroupId = r.Field<Int32?>("GroupId"),
                        NotesNumber = r.Field<string>("NotesNumber"),
                        NodeId = r.Field<Int64?>("NodeId"),
                        NodeNumber = r.Field<string>("NodeNumber"),
                        NodeHead = r.Field<string>("NodeHead"),
                        Narration = r.Field<string>("Narration"),
                        DRAmount = r.Field<decimal?>("DRAmount"),
                        CRAmount = r.Field<decimal?>("CRAmount"),
                        ClosingBalance = r.Field<decimal?>("ClosingBalance"),
                        ParentNodeId = r.Field<Int64?>("ParentNodeId"),
                        ParentNodeHead = r.Field<string>("ParentNodeHead"),
                        ParentNodeNumber = r.Field<string>("ParentNodeNumber")

                    }).ToList();

                    entityBODS.Dispose();
                }
            }

            return entityBOList;
        }


        //--- Revenue Related Report

        public List<GLRevenueBO> GetRevenueStatement(Int64 fiscalYearId, DateTime fromDate, DateTime toDate, Int16 monthId,
                                                                              Int32 reportTypeId, Int32 companyId, Int32 projectId, Int32 donorId)
        {
            List<GLRevenueBO> balanceSheet = new List<GLRevenueBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("HotelProfitLossStatement_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FiscalYearId", DbType.Int32, fiscalYearId);

                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate.Date);

                    dbSmartAspects.AddInParameter(cmd, "@MonthId", DbType.Int16, monthId);
                    dbSmartAspects.AddInParameter(cmd, "@ReportTypeId", DbType.Int32, reportTypeId);

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
                    dbSmartAspects.LoadDataSet(cmd, ds, "BalanceSheetStatement");
                    DataTable Table = ds.Tables["BalanceSheetStatement"];

                    balanceSheet = Table.AsEnumerable().Select(r =>
                                new GLRevenueBO
                                {

                                    Id = r.Field<long?>("Id"),
                                    Particulars = r.Field<string>("Particulars"),
                                    SortingOrder = r.Field<Int16?>("SortingOrder"),
                                    GroupId = r.Field<int?>("GroupId"),
                                    GroupName = r.Field<string>("GroupName"),
                                    YearId = r.Field<int?>("YearId"),
                                    MonthId = r.Field<Int16?>("MonthId"),
                                    MonthsName = r.Field<string>("MonthsName"),
                                    SubId = r.Field<int?>("SubId"),
                                    SubName = r.Field<string>("SubName"),
                                    NodeType = r.Field<string>("NodeType"),
                                    Amount = r.Field<decimal?>("Amount"),
                                    ProfitLoss = r.Field<decimal?>("ProfitLoss")

                                }).ToList();
                }
            }

            return balanceSheet;
        }

        public List<GLRevenueDetailsBO> GetRevenueDetailsStatement(Int64 fiscalYearId, DateTime fromDate, DateTime toDate, Int16 monthId,
                                                                             Int32 reportTypeId, Int32 companyId, Int32 projectId, Int32 donorId)
        {
            List<GLRevenueDetailsBO> balanceSheet = new List<GLRevenueDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("HotelProfitLossDetailsStatement_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FiscalYearId", DbType.Int32, fiscalYearId);

                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, fromDate.Date);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, toDate.Date);

                    dbSmartAspects.AddInParameter(cmd, "@MonthId", DbType.Int16, monthId);
                    dbSmartAspects.AddInParameter(cmd, "@ReportTypeId", DbType.Int32, reportTypeId);

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
                    dbSmartAspects.LoadDataSet(cmd, ds, "BalanceSheetStatement");
                    DataTable Table = ds.Tables["BalanceSheetStatement"];

                    balanceSheet = Table.AsEnumerable().Select(r =>
                                new GLRevenueDetailsBO
                                {

                                    GroupId = r.Field<int?>("GroupId"),
                                    GroupName = r.Field<string>("GroupName"),
                                    GroupSortingOrder = r.Field<Int16?>("GroupSortingOrder"),
                                    SubGroupId = r.Field<int?>("SubGroupId"),
                                    SubGroupName = r.Field<string>("SubGroupName"),
                                    SubGroupSortingOrder = r.Field<Int16?>("SubGroupSortingOrder"),
                                    ChildId = r.Field<int?>("ChildId"),
                                    NodeId = r.Field<int?>("NodeId"),
                                    ChildName = r.Field<string>("ChildName"),
                                    NodeType = r.Field<string>("NodeType"),
                                    ChildGroupSortingOrder = r.Field<Int16?>("ChildGroupSortingOrder"),
                                    CurrentMonthActual = r.Field<decimal?>("CurrentMonthActual"),
                                    CurrentMonthBudgetAmount = r.Field<decimal?>("CurrentMonthBudgetAmount"),
                                    LasYearCurrentMonthActual = r.Field<decimal?>("LasYearCurrentMonthActual"),
                                    YearToDateActual = r.Field<decimal?>("YearToDateActual"),
                                    YearToDateBudget = r.Field<decimal?>("YearToDateBudget"),
                                    LastYearToDateActual = r.Field<decimal?>("LastYearToDateActual"),

                                    CurrentMonthCaption = r.Field<string>("CurrentMonthCaption"),
                                    PreviousMonthCaption = r.Field<string>("PreviousMonthCaption"),
                                    CurrentYearToMonthCaption = r.Field<string>("CurrentYearToMonthCaption"),
                                    PreviousYearToMonthCaption = r.Field<string>("PreviousYearToMonthCaption"),

                                }).ToList();
                }
            }

            return balanceSheet;
        }
    }
}
