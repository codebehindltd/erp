using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Membership;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Data.Membership
{
    public class MemberPaymentDA : BaseService
    {
        public Boolean SaveMemberPaymentLedgerInfo(PMMemberPaymentLedgerBO memPaymentLedgerBO, out long tmpMemberPaymentId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePMMemberPaymentLedger_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, memPaymentLedgerBO.PaymentType);
                    dbSmartAspects.AddInParameter(command, "@BillNumber", DbType.String, memPaymentLedgerBO.BillNumber);
                    dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, memPaymentLedgerBO.PaymentDate);
                    dbSmartAspects.AddInParameter(command, "@MemberId", DbType.Int32, memPaymentLedgerBO.MemberId);
                    dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, memPaymentLedgerBO.CurrencyId);
                    dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, memPaymentLedgerBO.ConvertionRate);
                    dbSmartAspects.AddInParameter(command, "@DRAmount", DbType.Decimal, memPaymentLedgerBO.DRAmount);
                    dbSmartAspects.AddInParameter(command, "@CRAmount", DbType.Decimal, memPaymentLedgerBO.CRAmount);
                    dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, memPaymentLedgerBO.CurrencyAmount);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, memPaymentLedgerBO.Remarks);
                    dbSmartAspects.AddInParameter(command, "@PaymentStatus", DbType.String, memPaymentLedgerBO.PaymentStatus);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, memPaymentLedgerBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@MemberPaymentId", DbType.Int64, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpMemberPaymentId = Convert.ToInt64(command.Parameters["@MemberPaymentId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateMemberPaymentLedgerInfo(PMMemberPaymentLedgerBO memPaymentLedgerBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePMMemberPaymentLedger_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@MemberPaymentId", DbType.Int64, memPaymentLedgerBO.MemberPaymentId);
                    dbSmartAspects.AddInParameter(command, "@PaymentType", DbType.String, memPaymentLedgerBO.PaymentType);
                    dbSmartAspects.AddInParameter(command, "@BillNumber", DbType.String, memPaymentLedgerBO.BillNumber);
                    dbSmartAspects.AddInParameter(command, "@PaymentDate", DbType.DateTime, memPaymentLedgerBO.PaymentDate);
                    dbSmartAspects.AddInParameter(command, "@MemberId", DbType.Int32, memPaymentLedgerBO.MemberId);
                    dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, memPaymentLedgerBO.CurrencyId);
                    dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, memPaymentLedgerBO.ConvertionRate);
                    dbSmartAspects.AddInParameter(command, "@DRAmount", DbType.Decimal, memPaymentLedgerBO.DRAmount);
                    dbSmartAspects.AddInParameter(command, "@CRAmount", DbType.Decimal, memPaymentLedgerBO.CRAmount);
                    dbSmartAspects.AddInParameter(command, "@CurrencyAmount", DbType.Decimal, memPaymentLedgerBO.CurrencyAmount);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, memPaymentLedgerBO.Remarks);
                    dbSmartAspects.AddInParameter(command, "@PaymentStatus", DbType.String, memPaymentLedgerBO.PaymentStatus);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, memPaymentLedgerBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public List<MemberPaymentLedgerVwBo> GetMemberLedger(int memberId, DateTime dateFrom, DateTime dateTo, string paymentStatus, string reportType)
        {
            List<MemberPaymentLedgerVwBo> supplierInfo = new List<MemberPaymentLedgerVwBo>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberPaymentLedgerReport_SP"))
                {
                    if (reportType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, memberId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.Date, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.Date, dateTo);

                    if (paymentStatus != "0")
                        dbSmartAspects.AddInParameter(cmd, "@PaymentStatus", DbType.String, paymentStatus);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PaymentStatus", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemInfo");
                    DataTable Table = ds.Tables["ItemInfo"];

                    supplierInfo = Table.AsEnumerable().Select(r => new MemberPaymentLedgerVwBo
                    {
                        MemberId = r.Field<Int32>("MemberId"),
                        PaymentDate = r.Field<DateTime?>("PaymentDate"),
                        Narration = r.Field<string>("Narration"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        ClosingBalance = r.Field<decimal?>("ClosingBalance"),
                        BalanceCommulative = r.Field<decimal?>("BalanceCommulative"),
                        MemberName = r.Field<string>("MemberName"),
                        ContactName = r.Field<string>("CompanyName"),
                        ContactNumber = r.Field<string>("MobileNumber")                        

                    }).ToList();
                }
            }

            return supplierInfo;
        }
        public List<PMMemberPaymentLedgerBO> GetMemberPaymentLedger(DateTime? dateFrom, DateTime? dateTo, string paymentId, bool isInvoice)
        {
            List<PMMemberPaymentLedgerBO> paymentInfo = new List<PMMemberPaymentLedgerBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberPaymentLedger_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    dbSmartAspects.AddInParameter(cmd, "@PaymentId", DbType.String, paymentId);
                    dbSmartAspects.AddInParameter(cmd, "@IsInvoice", DbType.Boolean, isInvoice);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new PMMemberPaymentLedgerBO
                    {
                        MemberPaymentId = r.Field<Int64>("MemberPaymentId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        BillNumber = r.Field<string>("BillNumber"),
                        PaymentType = r.Field<string>("PaymentType"),
                        CurrencyAmount = r.Field<decimal>("CurrencyAmount"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        MemberName = r.Field<string>("MemberName"),
                        ConvertionRate = r.Field<decimal>("ConvertionRate")

                    }).ToList();
                }
            }

            return paymentInfo;
        }
        public PMMemberPaymentLedgerBO GetMemberPaymentLedgerById(long id)
        {
            PMMemberPaymentLedgerBO paymentInfo = new PMMemberPaymentLedgerBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberPaymentLedgerById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberPaymentId", DbType.Int64, id);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PaymentInfo");
                    DataTable Table = ds.Tables["PaymentInfo"];

                    paymentInfo = Table.AsEnumerable().Select(r => new PMMemberPaymentLedgerBO
                    {
                        MemberPaymentId = r.Field<Int64>("MemberPaymentId"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        BillNumber = r.Field<string>("BillNumber"),
                        PaymentType = r.Field<string>("PaymentType"),
                        CurrencyAmount = r.Field<decimal>("CurrencyAmount"),
                        //CurrencyName = r.Field<string>("CurrencyName"),
                        MemberName = r.Field<string>("MemberName"),
                        ConvertionRate = r.Field<decimal>("ConvertionRate"),
                        Remarks = r.Field<string>("Remarks"),
                        CurrencyId = r.Field<int>("CurrencyId"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        MemberId = r.Field<int>("MemberId")

                    }).SingleOrDefault();
                }
            }

            return paymentInfo;
        }
        public List<MemMemberBasicsBO> GetMemberInfoBySearchCriteria(string searchText)
        {
            List<MemMemberBasicsBO> memberList = new List<MemMemberBasicsBO>();
            string query = "SELECT * FROM MemMemberBasics WHERE FullName LIKE '%" + searchText + "%'";

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet SupplierDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SupplierDS, "Supplier");
                    DataTable table = SupplierDS.Tables["Supplier"];

                    memberList = table.AsEnumerable().Select(r => new MemMemberBasicsBO
                    {
                        MemberId = r.Field<int>("MemberId"),
                        FullName = r.Field<string>("FullName")

                    }).ToList();
                }
            }
            return memberList;
        }


        //Payment Configuration
        public bool SaveMemberPaymentConfigInfo(MemberPaymentConfigurationBO configBO, out long tmpConfigId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveMemberPaymentConfigInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, configBO.TransactionType);
                        dbSmartAspects.AddInParameter(command, "@MemberTypeOrMemberId", DbType.Int64, configBO.MemberTypeOrMemberId);
                        dbSmartAspects.AddInParameter(command, "@BillingPeriod", DbType.String, configBO.BillingPeriod);
                        dbSmartAspects.AddInParameter(command, "@BillingAmount", DbType.Decimal, configBO.BillingAmount);
                        dbSmartAspects.AddInParameter(command, "@BillingStartDate", DbType.DateTime, configBO.BillingStartDate);
                        dbSmartAspects.AddInParameter(command, "@DoorAccessDate", DbType.DateTime, configBO.DoorAccessDate);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, configBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@MemPaymentConfigId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpConfigId = Convert.ToInt64(command.Parameters["@MemPaymentConfigId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public bool UpdateMemberPaymentConfigInfo(MemberPaymentConfigurationBO configBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateMemberPaymentConfigInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, configBO.TransactionType);
                        dbSmartAspects.AddInParameter(command, "@MemPaymentConfigId", DbType.Int64, configBO.MemPaymentConfigId);
                        dbSmartAspects.AddInParameter(command, "@MemberTypeOrMemberId", DbType.Int64, configBO.MemberTypeOrMemberId);
                        dbSmartAspects.AddInParameter(command, "@BillingPeriod", DbType.String, configBO.BillingPeriod);
                        dbSmartAspects.AddInParameter(command, "@BillingAmount", DbType.Decimal, configBO.BillingAmount);
                        dbSmartAspects.AddInParameter(command, "@BillingStartDate", DbType.DateTime, configBO.BillingStartDate);
                        dbSmartAspects.AddInParameter(command, "@DoorAccessDate", DbType.DateTime, configBO.DoorAccessDate);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, configBO.LastModifiedBy);
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
        public MemberPaymentConfigurationBO GetMemberPaymentConfigInfo(string transactionType, int membertype)
        {
            MemberPaymentConfigurationBO bo = new MemberPaymentConfigurationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberPaymentConfigInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transactionType);
                    dbSmartAspects.AddInParameter(cmd, "@MemberTypeOrMemberId", DbType.Int64, membertype);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "PaymentConfig");
                    DataTable Table = incrementDS.Tables["PaymentConfig"];

                    bo = Table.AsEnumerable().Select(r => new MemberPaymentConfigurationBO
                    {
                        MemPaymentConfigId = r.Field<long>("MemPaymentConfigId"),
                        MemberTypeOrMemberId = r.Field<long>("MemberTypeOrMemberId"),
                        BillingPeriod = r.Field<string>("BillingPeriod"),
                        BillingAmount = r.Field<decimal>("BillingAmount"),
                        BillingStartDate = r.Field<DateTime?>("BillingStartDate"),
                        DoorAccessDate = r.Field<DateTime?>("DoorAccessDate"),
                        MemberName = r.Field<string>("MemberName")
                    }).SingleOrDefault();
                }
            }
            return bo;
        }
        public MemberPaymentConfigurationBO GetMemberPaymentConfigInfoByMemberId(int memberId)
        {
            MemberPaymentConfigurationBO configBO = new MemberPaymentConfigurationBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberPaymentConfigInfoByMemberId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int32, memberId);

                    DataSet incrementDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, incrementDS, "PaymentConfig");
                    DataTable Table = incrementDS.Tables["PaymentConfig"];

                    configBO = Table.AsEnumerable().Select(r => new MemberPaymentConfigurationBO
                    {
                        MemPaymentConfigId = r.Field<long>("MemPaymentConfigId"),
                        TransactionType = r.Field<string>("TransactionType"),
                        MemberTypeOrMemberId = r.Field<long>("MemberTypeOrMemberId"),
                        BillingPeriod = r.Field<string>("BillingPeriod"),
                        BillingAmount = r.Field<decimal>("BillingAmount"),
                        MemberName = r.Field<string>("MemberName")
                    }).SingleOrDefault();
                }
            }
            return configBO;
        }
    }
}
