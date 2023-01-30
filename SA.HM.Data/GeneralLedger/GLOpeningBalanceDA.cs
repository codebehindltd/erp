using HotelManagement.Entity.GeneralLedger;
using HotelManagement.Entity.HotelManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.GeneralLedger
{
    public class GLOpeningBalanceDA : BaseService
    {
        public GLOpeningBalanceView GetGLOpeningBalanceDetailByTransactionTypeNGLCompanyIdNProjectIdNFiscalYearId(string transactionType, int glCompanyId, int projectcId, DateTime VoucherDate, int categoryId, int storeId = 0, int locationId = 0, bool isInventory = false)
        {
            GLOpeningBalanceView openingBalance = new GLOpeningBalanceView();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLOpeningBalanceDetailByTransactionTypeNGLCompanyIdNProjectIdNFiscalYearId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transactionType);
                    dbSmartAspects.AddInParameter(cmd, "@GlCompanyId", DbType.Int32, glCompanyId);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectcId", DbType.Int32, projectcId);
                    //dbSmartAspects.AddInParameter(cmd, "@FiscalYearId", DbType.Int64, fiscalYearId);
                    dbSmartAspects.AddInParameter(cmd, "@VoucherDate", DbType.Date, VoucherDate);
                    dbSmartAspects.AddInParameter(cmd, "@StoreId", DbType.Int64, storeId);
                    dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int64, locationId);
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "OpeningBalance");

                    if (isInventory)
                    {
                        if (ds.Tables[0] != null)
                            openingBalance.InvOpeningBalance = ds.Tables[0].AsEnumerable().Select(r => new InvOpeningBalance
                            {
                                Id = r.Field<Int64>("Id"),
                                CompanyId = r.Field<int>("CompanyId"),
                                ProjectId = r.Field<int>("ProjectId"),
                                FiscalYearId = r.Field<int>("FiscalYearId"),
                                StoreId = r.Field<int>("StoreId"),
                                LocationId = r.Field<int>("LocationId")
                            }).FirstOrDefault();

                        if (ds.Tables[1] != null)
                            openingBalance.InvOpeningBalanceDetails = ds.Tables[1].AsEnumerable().Select(r => new InvOpeningBalanceDetail
                            {
                                Id = r.Field<Int64>("Id"),
                                InvOpeningBalanceId = r.Field<Int64>("InvOpeningBalanceId"),
                                TransactionNodeId = r.Field<Int64>("TransactionNodeId"),
                                UnitCost = r.Field<decimal?>("UnitCost"),
                                StockQuantity = r.Field<decimal?>("StockQuantity"),
                                Total = r.Field<decimal?>("Total"),
                                UnitHead = r.Field<string>("UnitHead")

                            }).ToList();
                    }
                    else
                    {
                        if (transactionType == "Accounts")
                        {
                            if (ds.Tables[0] != null)
                                openingBalance.OpeningBalance = ds.Tables[0].AsEnumerable().Select(r => new GLOpeningBalance
                                {
                                    Id = r.Field<Int64>("Id"),
                                    TransactionType = r.Field<string>("TransactionType"),
                                    CompanyId = r.Field<int>("CompanyId"),
                                    ProjectId = r.Field<int>("ProjectId"),
                                    FiscalYearId = r.Field<int>("FiscalYearId")

                                }).FirstOrDefault();

                            if (ds.Tables[1] != null)
                                openingBalance.OpeningBalanceDetails = ds.Tables[1].AsEnumerable().Select(r => new GLOpeningBalanceDetail
                                {
                                    Id = r.Field<Int64>("Id"),
                                    GLOpeningBalanceId = r.Field<Int64>("GLOpeningBalanceId"),
                                    AccountNodeId = r.Field<Int64>("AccountNodeId"),
                                    AccountType = r.Field<string>("AccountType"),
                                    AccountName = r.Field<string>("AccountName"),
                                    OpeningBalance = r.Field<decimal>("OpeningBalance")

                                }).ToList();
                        }
                        else if (transactionType == "Company")
                        {
                            if (ds.Tables[0] != null)
                                openingBalance.OpeningBalance = ds.Tables[0].AsEnumerable().Select(r => new GLOpeningBalance
                                {
                                    Id = r.Field<Int64>("Id"),
                                    CompanyId = r.Field<int>("CompanyId"),
                                    ProjectId = r.Field<int>("ProjectId"),
                                    FiscalYearId = r.Field<int>("FiscalYearId"),
                                    OpeningBalanceDate = r.Field<DateTime>("OpeningDate")

                                }).FirstOrDefault();

                            if (ds.Tables[1] != null)
                                openingBalance.CompanyDebitCreditList = ds.Tables[1].AsEnumerable().Select(r => new GLOpeningBalance
                                {
                                    Id = r.Field<Int64>("Id"),
                                    MasterId = r.Field<Int64>("CompanyMasterId"),
                                    CompanyId = r.Field<Int32>("TransactionNodeId"),
                                    DrAmount = r.Field<decimal>("DrAmount"),
                                    CrAmount = r.Field<decimal>("CrAmount")

                                }).ToList();
                        }
                        else if (transactionType == "Supplier")
                        {
                            if (ds.Tables[0] != null)
                                openingBalance.OpeningBalance = ds.Tables[0].AsEnumerable().Select(r => new GLOpeningBalance
                                {
                                    Id = r.Field<Int64>("Id"),
                                    CompanyId = r.Field<int>("CompanyId"),
                                    ProjectId = r.Field<int>("ProjectId"),
                                    FiscalYearId = r.Field<int>("FiscalYearId"),
                                    OpeningBalanceDate = r.Field<DateTime>("OpeningDate")

                                }).FirstOrDefault();

                            if (ds.Tables[1] != null)
                                openingBalance.SupplierDebitCreditList = ds.Tables[1].AsEnumerable().Select(r => new GLOpeningBalance
                                {
                                    Id = r.Field<Int64>("Id"),
                                    MasterId = r.Field<Int64>("SupplierMasterId"),
                                    SupplierId = r.Field<Int32>("TransactionNodeId"),
                                    DrAmount = r.Field<decimal>("DrAmount"),
                                    CrAmount = r.Field<decimal>("CrAmount")

                                }).ToList();
                        }
                        else if (transactionType == "Employee")
                        {
                            if (ds.Tables[0] != null)
                                openingBalance.OpeningBalance = ds.Tables[0].AsEnumerable().Select(r => new GLOpeningBalance
                                {
                                    Id = r.Field<Int64>("Id"),
                                    CompanyId = r.Field<int>("CompanyId"),
                                    ProjectId = r.Field<int>("ProjectId"),
                                    FiscalYearId = r.Field<int>("FiscalYearId"),
                                    OpeningBalanceDate = r.Field<DateTime>("OpeningDate")

                                }).FirstOrDefault();

                            if (ds.Tables[1] != null)
                                openingBalance.EmployeeDebitCreditList = ds.Tables[1].AsEnumerable().Select(r => new GLOpeningBalance
                                {
                                    Id = r.Field<Int64>("Id"),
                                    MasterId = r.Field<Int64>("EmployeeMasterId"),
                                    EmployeeId = r.Field<Int32>("TransactionNodeId"),
                                    DrAmount = r.Field<decimal>("DrAmount"),
                                    CrAmount = r.Field<decimal>("CrAmount")

                                }).ToList();
                        }
                        else if (transactionType == "Member")
                        {
                            if (ds.Tables[0] != null)
                                openingBalance.OpeningBalance = ds.Tables[0].AsEnumerable().Select(r => new GLOpeningBalance
                                {
                                    Id = r.Field<Int64>("Id"),
                                    CompanyId = r.Field<int>("CompanyId"),
                                    ProjectId = r.Field<int>("ProjectId"),
                                    FiscalYearId = r.Field<int>("FiscalYearId"),
                                    OpeningBalanceDate = r.Field<DateTime>("OpeningDate")

                                }).FirstOrDefault();

                            if (ds.Tables[1] != null)
                                openingBalance.MemberDebitCreditList = ds.Tables[1].AsEnumerable().Select(r => new GLOpeningBalance
                                {
                                    Id = r.Field<Int64>("Id"),
                                    MasterId = r.Field<Int64>("MemberMasterId"),
                                    MemberId = r.Field<Int32>("TransactionNodeId"),
                                    DrAmount = r.Field<decimal>("DrAmount"),
                                    CrAmount = r.Field<decimal>("CrAmount")

                                }).ToList();
                        }
                        else if (transactionType == "CNF")
                        {
                            if (ds.Tables[0] != null)
                                openingBalance.OpeningBalance = ds.Tables[0].AsEnumerable().Select(r => new GLOpeningBalance
                                {
                                    Id = r.Field<Int64>("Id"),
                                    CompanyId = r.Field<int>("CompanyId"),
                                    ProjectId = r.Field<int>("ProjectId"),
                                    FiscalYearId = r.Field<int>("FiscalYearId"),
                                    OpeningBalanceDate = r.Field<DateTime>("OpeningDate")

                                }).FirstOrDefault();

                            if (ds.Tables[1] != null)
                                openingBalance.CNFDebitCreditList = ds.Tables[1].AsEnumerable().Select(r => new GLOpeningBalance
                                {
                                    Id = r.Field<Int64>("Id"),
                                    MasterId = r.Field<Int64>("CNFMasterId"),
                                    CNFId = r.Field<Int32>("TransactionNodeId"),
                                    DrAmount = r.Field<decimal>("DrAmount"),
                                    CrAmount = r.Field<decimal>("CrAmount")

                                }).ToList();
                        }
                    }
                }
            }
            return openingBalance;
        }

        public List<HotelGuestCompanyBO> GetCompanyListForOpeningBalance()
        {
            List<HotelGuestCompanyBO> companyListForOpeningBalance = new List<HotelGuestCompanyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyListForOpeningBalance_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HotelGuestCompanyBO companyForOpeningBalance = new HotelGuestCompanyBO();

                                companyForOpeningBalance.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                companyForOpeningBalance.CompanyName = reader["CompanyName"].ToString();

                                companyListForOpeningBalance.Add(companyForOpeningBalance);
                            }
                        }
                    }
                }
            }
            return companyListForOpeningBalance;
        }

        public bool SaveOrUpdateGLOpeningBalance(GLOpeningBalance OpeningBalance, List<GLOpeningBalanceDetail> OpeningBalanceDetails, out long OpeningBalanceId)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateGLOpeningBalance_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, OpeningBalance.Id);
                            dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, OpeningBalance.TransactionType);
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, OpeningBalance.CompanyId);
                            dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, OpeningBalance.ProjectId);
                            dbSmartAspects.AddInParameter(cmd, "@VoucherDate", DbType.DateTime, OpeningBalance.VoucherDate);
                            dbSmartAspects.AddInParameter(cmd, "@OpeningBalanceEquity", DbType.Decimal, OpeningBalance.OpeningBalanceEquity);
                            dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, OpeningBalance.CreatedBy);
                            dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, OpeningBalance.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(cmd, "@OpeningBalanceId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;

                            OpeningBalanceId = Convert.ToInt64(cmd.Parameters["@OpeningBalanceId"].Value);

                        }

                        if (status)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateGLOpeningBalanceDetail_SP"))
                            {
                                foreach (var detail in OpeningBalanceDetails)
                                {
                                    cmd.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, detail.Id);
                                    dbSmartAspects.AddInParameter(cmd, "@GLOpeningBalanceId", DbType.Int64, OpeningBalanceId);
                                    dbSmartAspects.AddInParameter(cmd, "@TransactionNodeId", DbType.Int64, detail.AccountNodeId);
                                    dbSmartAspects.AddInParameter(cmd, "@AccountType", DbType.String, detail.AccountType);
                                    dbSmartAspects.AddInParameter(cmd, "@AccountName", DbType.String, detail.AccountName);
                                    dbSmartAspects.AddInParameter(cmd, "@OpeningBalance", DbType.Decimal, detail.OpeningBalance);

                                    status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;
                                }

                            }
                        }
                        if (status)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        conn.Close();
                        throw ex;
                    }
                }
            }
            return status;
        }

        public bool SaveOrUpdateInvOpeningBalance(InvOpeningBalance OpeningBalance, List<InvOpeningBalanceDetail> OpeningBalanceDetails, out long OpeningBalanceId)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateInvOpeningBalance_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, OpeningBalance.Id);
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, OpeningBalance.CompanyId);
                            dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, OpeningBalance.ProjectId);
                            dbSmartAspects.AddInParameter(cmd, "@VoucherDate", DbType.Date, OpeningBalance.VoucherDate);
                            //dbSmartAspects.AddInParameter(cmd, "@FiscalYearId", DbType.Int32, OpeningBalance.FiscalYearId);
                            dbSmartAspects.AddInParameter(cmd, "@StoreId", DbType.Int32, OpeningBalance.StoreId);
                            dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, OpeningBalance.LocationId);
                            dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, OpeningBalance.CreatedBy);
                            dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, OpeningBalance.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(cmd, "@OpeningBalanceId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;

                            OpeningBalanceId = Convert.ToInt64(cmd.Parameters["@OpeningBalanceId"].Value);

                        }

                        if (status)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateInvOpeningBalanceDetail_SP"))
                            {
                                foreach (var detail in OpeningBalanceDetails)
                                {
                                    cmd.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, detail.Id);
                                    dbSmartAspects.AddInParameter(cmd, "@InvOpeningBalanceId", DbType.Int64, OpeningBalanceId);
                                    dbSmartAspects.AddInParameter(cmd, "@TransactionNodeId", DbType.Int64, detail.TransactionNodeId);
                                    dbSmartAspects.AddInParameter(cmd, "@UnitCost", DbType.Decimal, detail.UnitCost);
                                    dbSmartAspects.AddInParameter(cmd, "@StockQuantity", DbType.Decimal, detail.StockQuantity);
                                    dbSmartAspects.AddInParameter(cmd, "@UnitHead", DbType.String, detail.UnitHead);
                                    dbSmartAspects.AddInParameter(cmd, "@Total", DbType.Decimal, detail.Total);

                                    status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;
                                }

                            }
                        }                        
                        if (status)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        conn.Close();
                        throw ex;
                    }
                }
            }
            return status;
        }

        public bool SaveOrUpdateCompanyOpeningBalance(InvOpeningBalance OpeningBalance, List<GLOpeningBalance> CompanyDebitCreditList, out long OpeningBalanceId)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCompanyOpeningBalance_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, OpeningBalance.Id);
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, OpeningBalance.CompanyId);
                            dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, OpeningBalance.ProjectId);
                            dbSmartAspects.AddInParameter(cmd, "@VoucherDate", DbType.Date, OpeningBalance.VoucherDate);
                            dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, OpeningBalance.CreatedBy);
                            dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, OpeningBalance.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(cmd, "@OpeningBalanceId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;

                            OpeningBalanceId = Convert.ToInt64(cmd.Parameters["@OpeningBalanceId"].Value);

                        }
                        if (status)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCompanyOpeningBalanceDetail_SP"))
                            {
                                foreach (var company in CompanyDebitCreditList)
                                {
                                    cmd.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, company.Id);
                                    dbSmartAspects.AddInParameter(cmd, "@CompanyOpeningBalanceId", DbType.Int64, OpeningBalanceId);
                                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, company.CompanyId);
                                    dbSmartAspects.AddInParameter(cmd, "@DrAmount", DbType.Decimal, company.DrAmount);
                                    dbSmartAspects.AddInParameter(cmd, "@CrAmount", DbType.Decimal, company.CrAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;
                                }

                            }
                        }
                        if (status)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        conn.Close();
                        throw ex;
                    }
                }
            }
            return status;
        }

        public bool SaveOrUpdateSupplierOpeningBalance(InvOpeningBalance OpeningBalance, List<GLOpeningBalance> SupplierDebitCreditList, out long OpeningBalanceId)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSupplierOpeningBalance_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, OpeningBalance.Id);
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, OpeningBalance.CompanyId);
                            dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, OpeningBalance.ProjectId);
                            dbSmartAspects.AddInParameter(cmd, "@VoucherDate", DbType.Date, OpeningBalance.VoucherDate);
                            dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, OpeningBalance.CreatedBy);
                            dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, OpeningBalance.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(cmd, "@OpeningBalanceId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;

                            OpeningBalanceId = Convert.ToInt64(cmd.Parameters["@OpeningBalanceId"].Value);

                        }
                        if (status)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSupplierOpeningBalanceDetail_SP"))
                            {
                                foreach (var supplier in SupplierDebitCreditList)
                                {
                                    cmd.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, supplier.Id);
                                    dbSmartAspects.AddInParameter(cmd, "@SupplierOpeningBalanceId", DbType.Int64, OpeningBalanceId);
                                    dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int64, supplier.SupplierId);
                                    dbSmartAspects.AddInParameter(cmd, "@DrAmount", DbType.Decimal, supplier.DrAmount);
                                    dbSmartAspects.AddInParameter(cmd, "@CrAmount", DbType.Decimal, supplier.CrAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;
                                }

                            }
                        }
                        if (status)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        conn.Close();
                        throw ex;
                    }
                }
            }
            return status;
        }

        public bool SaveOrUpdateEmployeeOpeningBalance(InvOpeningBalance OpeningBalance, List<GLOpeningBalance> EmployeeDebitCreditList, out long OpeningBalanceId)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateEmployeeOpeningBalance_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, OpeningBalance.Id);
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, OpeningBalance.CompanyId);
                            dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, OpeningBalance.ProjectId);
                            dbSmartAspects.AddInParameter(cmd, "@VoucherDate", DbType.Date, OpeningBalance.VoucherDate);
                            dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, OpeningBalance.CreatedBy);
                            dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, OpeningBalance.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(cmd, "@OpeningBalanceId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;

                            OpeningBalanceId = Convert.ToInt64(cmd.Parameters["@OpeningBalanceId"].Value);

                        }
                        if (status)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateEmployeeOpeningBalanceDetail_SP"))
                            {
                                foreach (var employee in EmployeeDebitCreditList)
                                {
                                    cmd.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, employee.Id);
                                    dbSmartAspects.AddInParameter(cmd, "@EmployeeOpeningBalanceId", DbType.Int64, OpeningBalanceId);
                                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int64, employee.EmployeeId);
                                    dbSmartAspects.AddInParameter(cmd, "@DrAmount", DbType.Decimal, employee.DrAmount);
                                    dbSmartAspects.AddInParameter(cmd, "@CrAmount", DbType.Decimal, employee.CrAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;
                                }

                            }
                        }
                        if (status)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        conn.Close();
                        throw ex;
                    }
                }
            }
            return status;
        }

        public bool SaveOrUpdateMemberOpeningBalance(InvOpeningBalance OpeningBalance, List<GLOpeningBalance> MemberDebitCreditList, out long OpeningBalanceId)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateMemberOpeningBalance_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, OpeningBalance.Id);
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, OpeningBalance.CompanyId);
                            dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, OpeningBalance.ProjectId);
                            dbSmartAspects.AddInParameter(cmd, "@VoucherDate", DbType.Date, OpeningBalance.VoucherDate);
                            dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, OpeningBalance.CreatedBy);
                            dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, OpeningBalance.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(cmd, "@OpeningBalanceId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;

                            OpeningBalanceId = Convert.ToInt64(cmd.Parameters["@OpeningBalanceId"].Value);

                        }
                        if (status)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateMemberOpeningBalanceDetail_SP"))
                            {
                                foreach (var member in MemberDebitCreditList)
                                {
                                    cmd.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, member.Id);
                                    dbSmartAspects.AddInParameter(cmd, "@MemberOpeningBalanceId", DbType.Int64, OpeningBalanceId);
                                    dbSmartAspects.AddInParameter(cmd, "@MemberId", DbType.Int64, member.MemberId);
                                    dbSmartAspects.AddInParameter(cmd, "@DrAmount", DbType.Decimal, member.DrAmount);
                                    dbSmartAspects.AddInParameter(cmd, "@CrAmount", DbType.Decimal, member.CrAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;
                                }

                            }
                        }
                        if (status)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        conn.Close();
                        throw ex;
                    }
                }
            }
            return status;
        }

        public bool SaveOrUpdateCNFOpeningBalance(InvOpeningBalance OpeningBalance, List<GLOpeningBalance> CNFDebitCreditList, out long OpeningBalanceId)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCNFOpeningBalance_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, OpeningBalance.Id);
                            dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, OpeningBalance.CompanyId);
                            dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, OpeningBalance.ProjectId);
                            dbSmartAspects.AddInParameter(cmd, "@VoucherDate", DbType.Date, OpeningBalance.VoucherDate);
                            dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, OpeningBalance.CreatedBy);
                            dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, OpeningBalance.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(cmd, "@OpeningBalanceId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;

                            OpeningBalanceId = Convert.ToInt64(cmd.Parameters["@OpeningBalanceId"].Value);

                        }
                        if (status)
                        {
                            using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateCNFOpeningBalanceDetail_SP"))
                            {
                                foreach (var cnf in CNFDebitCreditList)
                                {
                                    cmd.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, cnf.Id);
                                    dbSmartAspects.AddInParameter(cmd, "@CNFOpeningBalanceId", DbType.Int64, OpeningBalanceId);
                                    dbSmartAspects.AddInParameter(cmd, "@CNFId", DbType.Int64, cnf.CNFId);
                                    dbSmartAspects.AddInParameter(cmd, "@DrAmount", DbType.Decimal, cnf.DrAmount);
                                    dbSmartAspects.AddInParameter(cmd, "@CrAmount", DbType.Decimal, cnf.CrAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;
                                }

                            }
                        }
                        if (status)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        conn.Close();
                        throw ex;
                    }
                }
            }
            return status;
        }

        public bool ApproveOpeningBalance(bool isInventoryType, long id, int UserInfoId)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                try
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("ApproveOpeningBalance_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);
                        dbSmartAspects.AddInParameter(cmd, "@IsInventoryType", DbType.Boolean, isInventoryType);
                        dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, UserInfoId);

                        status = dbSmartAspects.ExecuteNonQuery(cmd) > 0;

                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    throw ex;
                }
            }
            return status;
        }

        public bool ApproveTransactionOpeningBalance(List<GLOpeningBalance> transactionInfo, string transactionType, long userInfoId)
        {
            bool status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("ApproveTransactionOpeningBalance_SP"))
                        {
                            foreach (GLOpeningBalance ti in transactionInfo)
                            {
                                cmd.Parameters.Clear();
                                if (transactionType == "Company")
                                {
                                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int32, ti.CompanyId);
                                }
                                else if (transactionType == "Supplier")
                                {
                                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int32, ti.SupplierId);
                                }
                                else if (transactionType == "Employee")
                                {
                                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int32, ti.EmployeeId);
                                }
                                else if (transactionType == "Member")
                                {
                                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int32, ti.MemberId);
                                }
                                else if (transactionType == "CNF")
                                {
                                    dbSmartAspects.AddInParameter(cmd, "@TransactionId", DbType.Int32, ti.SupplierId);
                                }

                                dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, transactionType);
                                dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int64, userInfoId);

                                status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0;
                            }

                        }
                        if (status)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        conn.Close();
                        throw ex;
                    }
                }
            }
            return status;
        }
    }
}
