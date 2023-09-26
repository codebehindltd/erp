using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;
using System.Collections;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Data.Payroll
{
    public class EmpLoanDA : BaseService
    {
        public bool SaveEmployeeLoan(EmpLoanBO empLoan, List<PayrollApprovedInfo> approvedInfo, out int loanId)
        {
            int status = 0;
            bool retValues = false;
            loanId = 0;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveEmpLoan_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empLoan.EmpId);
                            dbSmartAspects.AddInParameter(cmd, "@LoanNumber", DbType.String, empLoan.LoanNumber);
                            dbSmartAspects.AddInParameter(cmd, "@LoanType", DbType.String, empLoan.LoanType);
                            dbSmartAspects.AddInParameter(cmd, "@LoanAmount", DbType.Decimal, empLoan.LoanAmount);
                            dbSmartAspects.AddInParameter(cmd, "@InterestRate", DbType.Decimal, empLoan.InterestRate);
                            dbSmartAspects.AddInParameter(cmd, "@InterestAmount", DbType.Decimal, empLoan.InterestAmount);
                            dbSmartAspects.AddInParameter(cmd, "@DueAmount", DbType.Decimal, empLoan.DueAmount);
                            dbSmartAspects.AddInParameter(cmd, "@DueInterestAmount", DbType.Decimal, empLoan.DueInterestAmount);
                            dbSmartAspects.AddInParameter(cmd, "@LoanTakenForPeriod", DbType.Int32, empLoan.LoanTakenForPeriod);
                            dbSmartAspects.AddInParameter(cmd, "@LoanTakenForMonthOrYear", DbType.String, empLoan.LoanTakenForMonthOrYear);
                            dbSmartAspects.AddInParameter(cmd, "@PerInstallLoanAmount", DbType.Decimal, empLoan.PerInstallLoanAmount);
                            dbSmartAspects.AddInParameter(cmd, "@PerInstallInterestAmount", DbType.Decimal, empLoan.PerInstallInterestAmount);
                            dbSmartAspects.AddInParameter(cmd, "@LoanPaymentFromAccountHeadId", DbType.Int32, empLoan.LoanPaymentFromAccountHeadId);
                            dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, empLoan.Remarks);
                            dbSmartAspects.AddInParameter(cmd, "@CheckedBy", DbType.Int32, empLoan.CheckedBy);
                            dbSmartAspects.AddInParameter(cmd, "@ApprovedBy", DbType.Int32, empLoan.ApprovedBy);
                            dbSmartAspects.AddInParameter(cmd, "@LoanDate", DbType.DateTime, empLoan.LoanDate);
                            dbSmartAspects.AddInParameter(cmd, "@LoanStatus", DbType.String, empLoan.LoanStatus);
                            dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, empLoan.ApprovedStatus);
                            dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, empLoan.CreatedBy);
                            dbSmartAspects.AddOutParameter(cmd, "@LoanId", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(cmd, transction);
                            loanId = Convert.ToInt32(cmd.Parameters["@LoanId"].Value);
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retValues = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retValues = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retValues;
        }

        public bool UpdateEmployeeLoan(EmpLoanBO empLoan, List<PayrollApprovedInfo> approvedInfo)
        {
            int status = 0;
            bool retValues = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateEmpLoan_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@LoanId", DbType.Int32, empLoan.LoanId);
                            dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empLoan.EmpId);
                            dbSmartAspects.AddInParameter(cmd, "@LoanNumber", DbType.String, empLoan.LoanNumber);
                            dbSmartAspects.AddInParameter(cmd, "@LoanType", DbType.String, empLoan.LoanType);
                            dbSmartAspects.AddInParameter(cmd, "@LoanAmount", DbType.Decimal, empLoan.LoanAmount);
                            dbSmartAspects.AddInParameter(cmd, "@InterestRate", DbType.Decimal, empLoan.InterestRate);
                            dbSmartAspects.AddInParameter(cmd, "@InterestAmount", DbType.Decimal, empLoan.InterestAmount);
                            dbSmartAspects.AddInParameter(cmd, "@DueAmount", DbType.Decimal, empLoan.DueAmount);
                            dbSmartAspects.AddInParameter(cmd, "@DueInterestAmount", DbType.Decimal, empLoan.DueInterestAmount);
                            dbSmartAspects.AddInParameter(cmd, "@LoanTakenForPeriod", DbType.Int32, empLoan.LoanTakenForPeriod);
                            dbSmartAspects.AddInParameter(cmd, "@LoanTakenForMonthOrYear", DbType.String, empLoan.LoanTakenForMonthOrYear);
                            dbSmartAspects.AddInParameter(cmd, "@PerInstallLoanAmount", DbType.Decimal, empLoan.PerInstallLoanAmount);
                            dbSmartAspects.AddInParameter(cmd, "@PerInstallInterestAmount", DbType.Decimal, empLoan.PerInstallInterestAmount);
                            dbSmartAspects.AddInParameter(cmd, "@LoanPaymentFromAccountHeadId", DbType.Int32, empLoan.LoanPaymentFromAccountHeadId);
                            dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, empLoan.Remarks);
                            dbSmartAspects.AddInParameter(cmd, "@CheckedBy", DbType.Int32, empLoan.CheckedBy);
                            dbSmartAspects.AddInParameter(cmd, "@ApprovedBy", DbType.Int32, empLoan.ApprovedBy);
                            dbSmartAspects.AddInParameter(cmd, "@LoanDate", DbType.DateTime, empLoan.LoanDate);
                            dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, empLoan.LastModifiedBy);
                            status = dbSmartAspects.ExecuteNonQuery(cmd);
                        }
                        
                        if (status > 0)
                        {
                            transction.Commit();
                            retValues = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retValues = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retValues;
        }

        public bool UpdateLoanNApprovedStatus(int loanId, int empId, string loanStatus, string approvedStatus, int modifiedBy)
        {
            int status = 0;
            bool retValues = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateLoanNApprovedStatus_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@LoanId", DbType.Int32, loanId);
                            dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                            if (!string.IsNullOrEmpty(loanStatus))
                                dbSmartAspects.AddInParameter(cmd, "@LoanStatus", DbType.String, loanStatus);
                            else
                                dbSmartAspects.AddInParameter(cmd, "@LoanStatus", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(approvedStatus))
                                dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, approvedStatus);
                            else
                                dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.String, modifiedBy);
                            status = dbSmartAspects.ExecuteNonQuery(cmd);
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retValues = true;
                        }
                        else
                        {
                            transction.Rollback();
                            retValues = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retValues;
        }

        public EmpLoanBO GetLoanByLoanId(int loanId)
        {
            EmpLoanBO loan = new EmpLoanBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpLoanById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LoanId", DbType.Int32, loanId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmployeeLoan");
                    DataTable Table = ds.Tables["EmployeeLoan"];

                    loan = Table.AsEnumerable().Select(r => new EmpLoanBO
                    {
                        LoanId = r.Field<Int32>("LoanId"),
                        EmpId = r.Field<Int32>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        LoanNumber = r.Field<string>("LoanNumber"),
                        LoanType = r.Field<string>("LoanType"),
                        LoanAmount = r.Field<decimal>("LoanAmount"),
                        InterestRate = r.Field<decimal>("InterestRate"),
                        InterestAmount = r.Field<decimal>("InterestAmount"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        DueInterestAmount = r.Field<decimal>("DueInterestAmount"),
                        OverDueAmount = r.Field<decimal>("OverDueAmount"),
                        LoanTakenForPeriod = r.Field<Int32>("LoanTakenForPeriod"),
                        LoanTakenForMonthOrYear = r.Field<string>("LoanTakenForMonthOrYear"),
                        PerInstallLoanAmount = r.Field<decimal>("PerInstallLoanAmount"),
                        PerInstallInterestAmount = r.Field<decimal>("PerInstallInterestAmount"),
                        LoanPaymentFromAccountHeadId = r.Field<Int32>("LoanPaymentFromAccountHeadId"),
                        Remarks = r.Field<string>("Remarks"),
                        LoanDate = r.Field<DateTime>("LoanDate"),
                        LoanStatus = r.Field<string>("LoanStatus"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        CheckedBy = r.Field<Int32?>("CheckedBy"),
                        ApprovedBy = r.Field<Int32?>("ApprovedBy")
                    }).FirstOrDefault();
                }
            }

            return loan;
        }

        public List<EmpLoanBO> SearchLoan(string empId, string loanType, string loanStatus, int? userId, int recordPerPage, int pageNumber, out int totalRecords)
        {
            List<EmpLoanBO> loan = new List<EmpLoanBO>();
            totalRecords = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpLoanDetails_SP"))
                {
                    if (!string.IsNullOrEmpty(empId))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(loanType))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LoanType", DbType.String, loanType);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LoanType", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userId);
                    dbSmartAspects.AddInParameter(cmd, "@LoanStatus", DbType.String, loanStatus);

                    dbSmartAspects.AddInParameter(cmd, "@recordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageNumber);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmployeeLoan");
                    DataTable Table = ds.Tables["EmployeeLoan"];

                    loan = Table.AsEnumerable().Select(r => new EmpLoanBO
                    {
                        LoanId = r.Field<Int32>("LoanId"),
                        EmpId = r.Field<Int32>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        LoanNumber = r.Field<string>("LoanNumber"),
                        LoanType = r.Field<string>("LoanType"),
                        LoanAmount = r.Field<decimal>("LoanAmount"),
                        InterestRate = r.Field<decimal>("InterestRate"),
                        InterestAmount = r.Field<decimal>("InterestAmount"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        DueInterestAmount = r.Field<decimal>("DueInterestAmount"),
                        LoanTakenForPeriod = r.Field<Int32>("LoanTakenForPeriod"),
                        LoanTakenForMonthOrYear = r.Field<string>("LoanTakenForMonthOrYear"),
                        PerInstallLoanAmount = r.Field<decimal>("PerInstallLoanAmount"),
                        PerInstallInterestAmount = r.Field<decimal>("PerInstallInterestAmount"),
                        LoanDate = r.Field<DateTime>("LoanDate"),
                        LoanStatus = r.Field<string>("LoanStatus"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        IsAutoLoanCollectionProcessEnable = r.Field<int>("IsAutoLoanCollectionProcessEnable"),
                        CheckedBy = r.Field<int?>("CheckedBy"),
                        ApprovedBy = r.Field<int?>("ApprovedBy"),
                        CreatedBy = r.Field<int>("CreatedBy"),
                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanDelete = r.Field<bool>("IsCanDelete"),
                        IsCanChecked = r.Field<bool>("IsCanChecked"),
                        IsCanApproved = r.Field<bool>("IsCanApproved")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return loan;
        }

        public Boolean SaveEmpLoanInformation(LoanSettingBO loanInfo, out int tmpId)
        {
            Boolean status = false;
            tmpId = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpLoanInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@CompanyLoanInterestRate", DbType.Decimal, loanInfo.CompanyLoanInterestRate);
                        dbSmartAspects.AddInParameter(command, "@PFLoanInterestRate", DbType.Decimal, loanInfo.PFLoanInterestRate);
                        dbSmartAspects.AddInParameter(command, "@MaxAmountCanWithdrawFromPFInPercentage", DbType.Decimal, loanInfo.MaxAmountCanWithdrawFromPFInPercentage);
                        dbSmartAspects.AddInParameter(command, "@MinPFMustAvailableToAllowLoan", DbType.Decimal, loanInfo.MinPFMustAvailableToAllowLoan);
                        dbSmartAspects.AddInParameter(command, "@MinJobLengthToAllowCompanyLoan", DbType.Decimal, loanInfo.MinJobLengthToAllowCompanyLoan);
                        dbSmartAspects.AddInParameter(command, "@DurationToAllowNextLoanAfterCompletetionTakenLoan", DbType.Int32, loanInfo.DurationToAllowNextLoanAfterCompletetionTakenLoan);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, loanInfo.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@LoanSettingId", DbType.Int32, loanInfo.LoanSettingId);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpId = Convert.ToInt32(command.Parameters["@LoanSettingId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public LoanSettingBO GetEmpLoanInformation()
        {
            LoanSettingBO loanInfo = new LoanSettingBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpLoanInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                loanInfo.LoanSettingId = Convert.ToInt32(reader["LoanSettingId"]);
                                loanInfo.CompanyLoanInterestRate = Convert.ToDecimal(reader["CompanyLoanInterestRate"]);
                                loanInfo.PFLoanInterestRate = Convert.ToDecimal(reader["PFLoanInterestRate"]);
                                loanInfo.MaxAmountCanWithdrawFromPFInPercentage = Convert.ToDecimal(reader["MaxAmountCanWithdrawFromPFInPercentage"]);
                                loanInfo.MinPFMustAvailableToAllowLoan = Convert.ToDecimal(reader["MinPFMustAvailableToAllowLoan"]);
                                loanInfo.MinJobLengthToAllowCompanyLoan = Convert.ToDecimal(reader["MinJobLengthToAllowCompanyLoan"]);
                                loanInfo.DurationToAllowNextLoanAfterCompletetionTakenLoan = Convert.ToInt32(reader["DurationToAllowNextLoanAfterCompletetionTakenLoan"]);
                            }
                        }
                    }
                }
            }
            return loanInfo;
        }

        public Boolean UpdateEmpLoanInfo(LoanSettingBO loanInfo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpLoanInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@LoanSettingId", DbType.Int32, loanInfo.LoanSettingId);
                        dbSmartAspects.AddInParameter(command, "@CompanyLoanInterestRate", DbType.Decimal, loanInfo.CompanyLoanInterestRate);
                        dbSmartAspects.AddInParameter(command, "@PFLoanInterestRate", DbType.Decimal, loanInfo.PFLoanInterestRate);
                        dbSmartAspects.AddInParameter(command, "@MaxAmountCanWithdrawFromPFInPercentage", DbType.Decimal, loanInfo.MaxAmountCanWithdrawFromPFInPercentage);
                        dbSmartAspects.AddInParameter(command, "@MinPFMustAvailableToAllowLoan", DbType.Decimal, loanInfo.MinPFMustAvailableToAllowLoan);
                        dbSmartAspects.AddInParameter(command, "@MinJobLengthToAllowCompanyLoan", DbType.Decimal, loanInfo.MinJobLengthToAllowCompanyLoan);
                        dbSmartAspects.AddInParameter(command, "@DurationToAllowNextLoanAfterCompletetionTakenLoan", DbType.Int32, loanInfo.DurationToAllowNextLoanAfterCompletetionTakenLoan);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, loanInfo.LastModifiedBy);

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

        //-------------------------------- Loan Collection ----------------------------------

        public bool SaveLoanCollection(LoanCollectionBO loanCollection, out int loanCollectionId)
        {
            int status = 0;
            loanCollectionId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveLoanCollection_SP"))
                {
                    conn.Open();

                    dbSmartAspects.AddInParameter(cmd, "@LoanId", DbType.Int32, loanCollection.LoanId);
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.String, loanCollection.EmpId);
                    dbSmartAspects.AddInParameter(cmd, "@InstallmentNumber", DbType.Int32, loanCollection.InstallmentNumber);
                    dbSmartAspects.AddInParameter(cmd, "@CollectionDate", DbType.DateTime, loanCollection.CollectionDate);
                    dbSmartAspects.AddInParameter(cmd, "@CollectedLoanAmount", DbType.Decimal, loanCollection.CollectedLoanAmount);
                    dbSmartAspects.AddInParameter(cmd, "@CollectedInterestAmount", DbType.Decimal, loanCollection.CollectedInterestAmount);
                    dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, loanCollection.CreatedBy);
                    dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, loanCollection.ApprovedStatus);

                    dbSmartAspects.AddOutParameter(cmd, "@LoanCollectionId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(cmd);
                    loanCollectionId = Convert.ToInt32(cmd.Parameters["@LoanCollectionId"].Value);
                }
            }

            if (status > 0)
                return true;
            else
                return false;
        }

        public bool UpdateLoanCollection(LoanCollectionBO loanCollection)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateLoanCollection_SP"))
                {
                    conn.Open();

                    dbSmartAspects.AddInParameter(cmd, "@CollectionId", DbType.Int32, loanCollection.CollectionId);
                    dbSmartAspects.AddInParameter(cmd, "@LoanId", DbType.Int32, loanCollection.LoanId);
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, loanCollection.EmpId);
                    dbSmartAspects.AddInParameter(cmd, "@InstallmentNumber", DbType.Int32, loanCollection.InstallmentNumber);
                    dbSmartAspects.AddInParameter(cmd, "@CollectionDate", DbType.DateTime, loanCollection.CollectionDate);
                    dbSmartAspects.AddInParameter(cmd, "@CollectedLoanAmount", DbType.Decimal, loanCollection.CollectedLoanAmount);
                    dbSmartAspects.AddInParameter(cmd, "@CollectedInterestAmount", DbType.Decimal, loanCollection.CollectedInterestAmount);
                    dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, loanCollection.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(cmd);
                }
            }

            if (status > 0)
                return true;
            else
                return false;
        }

        public bool ApprovedLoanCollection(int collectionId, int loanId, string approvedStatus)
        {
            int status = 0;
            string query = string.Format("UPDATE PayrollLoanCollection SET ApprovedStatus = '{0}' WHERE CollectionId = {1}", approvedStatus, collectionId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    conn.Open();
                    status = dbSmartAspects.ExecuteNonQuery(cmd);
                }
            }

            if (status > 0)
                return true;
            else
                return false;
        }

        public bool DeleteLoanCollection(int collectionId, int loanId)
        {
            int status = 0;
            string query = string.Format("DELETE FROM PayrollLoanCollection WHERE CollectionId = {0}", collectionId);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    conn.Open();
                    status = dbSmartAspects.ExecuteNonQuery(cmd);
                }
            }

            if (status > 0)
                return true;
            else
                return false;
        }
        public List<LoanCollectionBO> GetLoanCollectionByLoanId(int loanId)
        {
            List<LoanCollectionBO> loanCollection = new List<LoanCollectionBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLoanCollectionByLoanId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LoanId", DbType.Int32, loanId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollLoanCollection");
                    DataTable Table = ds.Tables["PayrollLoanCollection"];

                    loanCollection = Table.AsEnumerable().Select(r => new LoanCollectionBO
                    {
                        CollectionId = r.Field<Int32>("CollectionId"),
                        LoanId = r.Field<Int32>("LoanId"),
                        EmpId = r.Field<Int32>("EmpId"),
                        CollectionDate = r.Field<DateTime>("CollectionDate"),
                        CollectedLoanAmount = r.Field<decimal>("CollectedLoanAmount"),
                        CollectedInterestAmount = r.Field<decimal>("CollectedInterestAmount"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus")

                    }).ToList();
                }
            }

            return loanCollection;
        }

        public LoanCollectionBO GetLastPaidLoanCollectionByLoanId(int loanId)
        {
            LoanCollectionBO loanCollection = new LoanCollectionBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLastPaidLoanCollectionByLoanId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LoanId", DbType.Int32, loanId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PayrollLoanCollection");
                    DataTable Table = ds.Tables["PayrollLoanCollection"];

                    loanCollection = Table.AsEnumerable().Select(r => new LoanCollectionBO
                    {
                        CollectionId = r.Field<Int32>("CollectionId"),
                        LoanId = r.Field<Int32>("LoanId"),
                        EmpId = r.Field<Int32>("EmpId"),
                        CollectionDate = r.Field<DateTime>("CollectionDate"),
                        CollectedLoanAmount = r.Field<decimal>("CollectionAmount"),
                        CollectedInterestAmount = r.Field<decimal>("InterestCollectionAmount"),
                        InstallmentNumber = r.Field<Int32>("InstallmentNumber")

                    }).FirstOrDefault();
                }
            }

            return loanCollection;
        }

        public List<EmpLoanCollectionDetails> GetLoanCollectionDetailsForSearch(string empId, string loanType, int recordPerPage, int pageNumber, out int totalRecords)
        {
            List<EmpLoanCollectionDetails> loanCollection = new List<EmpLoanCollectionDetails>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpLoanCollectionDetails_SP"))
                {
                    if (!string.IsNullOrEmpty(empId))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(loanType))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LoanType", DbType.String, loanType);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LoanType", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageNumber);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "LoanCollection");
                    DataTable Table = ds.Tables["LoanCollection"];

                    loanCollection = Table.AsEnumerable().Select(r => new EmpLoanCollectionDetails
                    {
                        EmpId = r.Field<Int32>("EmpId"),
                        LoanId = r.Field<Int32>("LoanId"),
                        CollectionId = r.Field<Int32>("CollectionId"),
                        LoanType = r.Field<string>("LoanType"),
                        LoanTakenForPeriod = r.Field<Int32>("LoanTakenForPeriod"),
                        LoanTakenForMonthOrYear = r.Field<string>("LoanTakenForMonthOrYear"),
                        LoanAmount = r.Field<decimal>("LoanAmount"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        CollectionDate = r.Field<DateTime?>("CollectionDate"),
                        InstallmentNumber = r.Field<Int32?>("InstallmentNumber"),
                        CollectionAmount = r.Field<decimal?>("CollectionAmount"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return loanCollection;
        }

        //------------------------Loan Hold Up-----------------------------------------------

        public bool SaveLoanHoldUp(LoanHoldupBO loanHoldup, out int loanHoldupId)
        {
            int status = 0;
            loanHoldupId = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveEmpLoanHoldup_SP"))
                    {
                        conn.Open();

                        dbSmartAspects.AddInParameter(cmd, "@LoanId", DbType.Int32, loanHoldup.LoanId);
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, loanHoldup.EmpId);
                        dbSmartAspects.AddInParameter(cmd, "@LoanHoldupDateFrom", DbType.DateTime, loanHoldup.LoanHoldupDateFrom);
                        dbSmartAspects.AddInParameter(cmd, "@LoanHoldupDateTo", DbType.DateTime, loanHoldup.LoanHoldupDateTo);
                        dbSmartAspects.AddInParameter(cmd, "@InstallmentNumberWhenLoanHoldup", DbType.Int32, loanHoldup.InstallmentNumberWhenLoanHoldup);
                        dbSmartAspects.AddInParameter(cmd, "@DueAmount", DbType.Decimal, loanHoldup.DueAmount);
                        dbSmartAspects.AddInParameter(cmd, "@OverDueAmount", DbType.Decimal, loanHoldup.OverDueAmount);
                        dbSmartAspects.AddInParameter(cmd, "@ApprovedBy", DbType.Int32, loanHoldup.ApprovedBy);
                        dbSmartAspects.AddInParameter(cmd, "@ApprovedDate", DbType.DateTime, loanHoldup.ApprovedDate);
                        dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, loanHoldup.Remarks);
                        dbSmartAspects.AddInParameter(cmd, "@HoldupStatus", DbType.String, loanHoldup.HoldupStatus);
                        dbSmartAspects.AddInParameter(cmd, "@CreatedBy", DbType.Int32, loanHoldup.CreatedBy);

                        dbSmartAspects.AddOutParameter(cmd, "@LoanHoldupId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(cmd);
                        loanHoldupId = Convert.ToInt32(cmd.Parameters["@LoanHoldupId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (status > 0)
                return true;
            else
                return false;
        }

        public bool UpdateLoanHoldUp(LoanHoldupBO loanHoldup)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateEmpLoanHoldup_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LoanHoldupId", DbType.Int32, loanHoldup.LoanHoldupId);
                    dbSmartAspects.AddInParameter(cmd, "@LoanId", DbType.Int32, loanHoldup.LoanId);
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, loanHoldup.EmpId);
                    dbSmartAspects.AddInParameter(cmd, "@LoanHoldupDateFrom", DbType.DateTime, loanHoldup.LoanHoldupDateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@LoanHoldupDateTo", DbType.DateTime, loanHoldup.LoanHoldupDateTo);
                    dbSmartAspects.AddInParameter(cmd, "@InstallmentNumberWhenLoanHoldup", DbType.Int32, loanHoldup.InstallmentNumberWhenLoanHoldup);
                    dbSmartAspects.AddInParameter(cmd, "@DueAmount", DbType.Decimal, loanHoldup.DueAmount);
                    dbSmartAspects.AddInParameter(cmd, "@OverDueAmount", DbType.Decimal, loanHoldup.OverDueAmount);
                    dbSmartAspects.AddInParameter(cmd, "@ApprovedBy", DbType.Int32, loanHoldup.ApprovedBy);
                    dbSmartAspects.AddInParameter(cmd, "@ApprovedDate", DbType.DateTime, loanHoldup.ApprovedDate);
                    dbSmartAspects.AddInParameter(cmd, "@Remarks", DbType.String, loanHoldup.Remarks);
                    dbSmartAspects.AddInParameter(cmd, "@HoldupStatus", DbType.String, loanHoldup.HoldupStatus);
                    dbSmartAspects.AddInParameter(cmd, "@LastModifiedBy", DbType.Int32, loanHoldup.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(cmd);
                }
            }

            if (status > 0)
                return true;
            else
                return false;
        }

        public LoanHoldupBO GetGetPayrollLoanHoldupByLoanId(int loanId)
        {
            LoanHoldupBO loan = new LoanHoldupBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollLoanHoldupByLoanId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LoanId", DbType.Int32, loanId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmployeeLoan");
                    DataTable Table = ds.Tables["EmployeeLoan"];

                    loan = Table.AsEnumerable().Select(r => new LoanHoldupBO
                    {
                        LoanHoldupId = r.Field<Int32>("LoanHoldupId"),
                        LoanId = r.Field<Int32>("LoanId"),
                        EmpId = r.Field<Int32>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        LoanHoldupDateFrom = r.Field<DateTime>("LoanHoldupDateFrom"),
                        LoanHoldupDateTo = r.Field<DateTime>("LoanHoldupDateTo"),
                        InstallmentNumberWhenLoanHoldup = r.Field<Int32>("InstallmentNumberWhenLoanHoldup"),
                        DueAmount = r.Field<decimal>("DueAmount"),
                        OverDueAmount = r.Field<decimal>("OverDueAmount"),
                        HoldupStatus = r.Field<string>("HoldupStatus"),
                        Remarks = r.Field<string>("Remarks")

                    }).FirstOrDefault();
                }
            }

            return loan;
        }

        public List<PFLoanCollectionViewBO> GetPFLoanCollection(int empId)
        {
            List<PFLoanCollectionViewBO> viewList = new List<PFLoanCollectionViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPFLoanCollectionForReport_SP"))
                {
                    if (empId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PFLoanCollection");
                    DataTable Table = ds.Tables["PFLoanCollection"];

                    viewList = Table.AsEnumerable().Select(r => new PFLoanCollectionViewBO
                    {
                        CollectionDate = r.Field<DateTime>("CollectionDate"),
                        Advance = r.Field<decimal?>("Advance"),
                        Refund = r.Field<decimal?>("Refund"),
                        Interest = r.Field<decimal?>("Interest"),
                        Balance = r.Field<decimal?>("Balance")
                    }).ToList();
                }
            }

            return viewList;
        }

        public List<LoanApplicationViewBO> GetEmpInfoForLoanApplication(int empId)
        {
            List<LoanApplicationViewBO> loanApp = new List<LoanApplicationViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeLoanTakenStatus_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "LoanApplication");
                    DataTable Table = ds.Tables["LoanApplication"];

                    loanApp = Table.AsEnumerable().Select(r => new LoanApplicationViewBO
                    {
                        EmpId = r.Field<Int32>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        JoinDate = r.Field<DateTime?>("JoinDate"),
                        BasicAmount = r.Field<decimal>("BasicAmount"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        Designation = r.Field<string>("Designation"),
                        ServiceYear = r.Field<int>("ServiceYear"),
                        PFEligibilityDate = r.Field<DateTime?>("PFEligibilityDate"),
                        IsLoanTakenBefore = r.Field<int>("IsLoanTakenBefore"),
                        IsThisLoanClear = r.Field<int>("IsThisLoanClear"),
                        BeforeLoanTakenStatus = r.Field<string>("BeforeLoanTakenStatus"),
                        LoanStatus = r.Field<string>("LoanStatus")
                    }).ToList();
                }
            }

            return loanApp;
        }

        public bool LoanApproval(int loanId, string approvedStatus,  int checkedOrApprovedBy)
        {
            bool retVal = false;
            int status = 0;
            string OutStatus = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("LoanApproval_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@LoanId", DbType.Int32, loanId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, checkedOrApprovedBy);
                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
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
    }
}

