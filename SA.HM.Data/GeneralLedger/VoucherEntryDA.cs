using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.GeneralLedger;
using System.Data.Common;
using System.Data;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Data.GeneralLedger
{
    public class VoucherEntryDA : BaseService
    {
        public Boolean SaveVoucher(GLLedgerMasterBO ledgerMaster, List<GLLedgerDetailsBO> ledgerDetails, string deletedDocument, long RandomId, out long ledgerId, out string voucherNo)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveLedgerMaster_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, ledgerMaster.CompanyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ProjectId", DbType.Int32, ledgerMaster.ProjectId);

                            if (ledgerMaster.DonorId != null)
                                dbSmartAspects.AddInParameter(commandMaster, "@DonorId", DbType.Int32, ledgerMaster.DonorId);
                            else
                                dbSmartAspects.AddInParameter(commandMaster, "@DonorId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandMaster, "@VoucherType", DbType.String, ledgerMaster.VoucherType);

                            dbSmartAspects.AddInParameter(commandMaster, "@IsBankExist", DbType.Boolean, ledgerMaster.IsBankExist);
                            dbSmartAspects.AddInParameter(commandMaster, "@VoucherDate", DbType.DateTime, ledgerMaster.VoucherDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@Narration", DbType.String, ledgerMaster.Narration);

                            dbSmartAspects.AddInParameter(commandMaster, "@CurrencyId", DbType.Int32, ledgerMaster.CurrencyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConvertionRate", DbType.Decimal, ledgerMaster.ConvertionRate);

                            if (!string.IsNullOrEmpty(ledgerMaster.PayerOrPayee))
                                dbSmartAspects.AddInParameter(commandMaster, "@PayerOrPayee", DbType.String, ledgerMaster.PayerOrPayee);
                            else
                                dbSmartAspects.AddInParameter(commandMaster, "@PayerOrPayee", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(ledgerMaster.ReferenceNumber))
                                dbSmartAspects.AddInParameter(commandMaster, "@ReferenceNumber", DbType.String, ledgerMaster.ReferenceNumber);
                            else
                                dbSmartAspects.AddInParameter(commandMaster, "@ReferenceNumber", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandMaster, "@GLStatus", DbType.String, ledgerMaster.GLStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, ledgerMaster.CreatedBy);

                            dbSmartAspects.AddOutParameter(commandMaster, "@LedgerMasterId", DbType.Int64, sizeof(Int64));
                            dbSmartAspects.AddOutParameter(commandMaster, "@VoucherNo", DbType.String, 25);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            ledgerId = Convert.ToInt64(commandMaster.Parameters["@LedgerMasterId"].Value.ToString());
                            voucherNo = commandMaster.Parameters["@VoucherNo"].Value.ToString();
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveLedgerDetails_SP"))
                            {
                                foreach (GLLedgerDetailsBO DetailBO in ledgerDetails)
                                {
                                    commandDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDetails, "@LedgerMasterId", DbType.Int64, ledgerId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@NodeId", DbType.Int32, DetailBO.NodeId);

                                    if (DetailBO.ChequeDate != null)
                                        dbSmartAspects.AddInParameter(commandDetails, "@BankAccountId", DbType.Int32, DetailBO.BankAccountId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@BankAccountId", DbType.Int32, DBNull.Value);

                                    if (!string.IsNullOrEmpty(DetailBO.ChequeNumber))
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeNumber", DbType.String, DetailBO.ChequeNumber);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeNumber", DbType.String, DBNull.Value);

                                    if (DetailBO.ChequeDate != null)
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeDate", DbType.Date, DetailBO.ChequeDate);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeDate", DbType.Date, DBNull.Value);

                                    dbSmartAspects.AddInParameter(commandDetails, "@DRAmount", DbType.Decimal, DetailBO.DRAmount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CRAmount", DbType.Decimal, DetailBO.CRAmount);

                                    dbSmartAspects.AddInParameter(commandDetails, "@NodeNarration", DbType.String, DetailBO.NodeNarration);

                                    dbSmartAspects.AddInParameter(commandDetails, "@CostCenterId", DbType.Int32, DetailBO.CostCenterId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CurrencyAmount", DbType.Decimal, DetailBO.CurrencyAmount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@NodeType", DbType.String, DetailBO.NodeType);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            if (!string.IsNullOrWhiteSpace(ledgerMaster.ChequeNumber) && ledgerMaster.ChequeDate != null)
                            {
                                using (DbCommand commandVoucherCheque = dbSmartAspects.GetStoredProcCommand("UpdateVoucherChequeInfoByLedgerMasterId_SP"))
                                {
                                    commandVoucherCheque.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandVoucherCheque, "@LedgerMasterId", DbType.Int64, ledgerId);
                                    dbSmartAspects.AddInParameter(commandVoucherCheque, "@ChequeNumber", DbType.String, ledgerMaster.ChequeNumber);
                                    dbSmartAspects.AddInParameter(commandVoucherCheque, "@ChequeDate", DbType.Date, ledgerMaster.ChequeDate);

                                    status = dbSmartAspects.ExecuteNonQuery(commandVoucherCheque, transction);
                                }
                            }
                        }

                        if (status > 0 && !string.IsNullOrEmpty(deletedDocument))
                        {
                            bool deleteDocument = new DocumentsDA().DeleteDocumentsByDocumentIdListString(deletedDocument);
                        }
                        if (status > 0)
                        {
                            bool update = new DocumentsDA().UpdateRandomDocumentOwnwerIdWithOwnerId(ledgerId, RandomId);
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
        public Boolean UpdateVoucherChequeInfoByLedgerMasterId(long ledgerMasterId, string chequeNumber, DateTime? chequeDate)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandVoucherCheque = dbSmartAspects.GetStoredProcCommand("UpdateVoucherChequeInfoByLedgerMasterId_SP"))
                {
                    commandVoucherCheque.Parameters.Clear();
                    dbSmartAspects.AddInParameter(commandVoucherCheque, "@LedgerMasterId", DbType.Int64, ledgerMasterId);
                    dbSmartAspects.AddInParameter(commandVoucherCheque, "@ChequeNumber", DbType.String, chequeNumber);
                    dbSmartAspects.AddInParameter(commandVoucherCheque, "@ChequeDate", DbType.Date, chequeDate);

                    status = dbSmartAspects.ExecuteNonQuery(commandVoucherCheque) > 0 ? true : false;
                }
            }

            return status;
        }
        public Boolean UpdateVoucher(GLLedgerMasterBO ledgerMaster, List<GLLedgerDetailsBO> newLedgerDetails, List<GLLedgerDetailsBO> editLedgerDetails, List<GLLedgerDetailsBO> deleteLedgerDetails, string deletedDocument, long RandomId)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateLedgerMaster_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@LedgerMasterId", DbType.Int64, ledgerMaster.LedgerMasterId);
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyId", DbType.Int32, ledgerMaster.CompanyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ProjectId", DbType.Int32, ledgerMaster.ProjectId);

                            if (ledgerMaster.DonorId != null)
                                dbSmartAspects.AddInParameter(commandMaster, "@DonorId", DbType.Int32, ledgerMaster.DonorId);
                            else
                                dbSmartAspects.AddInParameter(commandMaster, "@DonorId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandMaster, "@VoucherType", DbType.String, ledgerMaster.VoucherType);
                            dbSmartAspects.AddInParameter(commandMaster, "@IsBankExist", DbType.Boolean, ledgerMaster.IsBankExist);
                            dbSmartAspects.AddInParameter(commandMaster, "@VoucherDate", DbType.DateTime, ledgerMaster.VoucherDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@Narration", DbType.String, ledgerMaster.Narration);
                            dbSmartAspects.AddInParameter(commandMaster, "@CurrencyId", DbType.Int32, ledgerMaster.CurrencyId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ConvertionRate", DbType.Decimal, ledgerMaster.ConvertionRate);

                            if (!string.IsNullOrEmpty(ledgerMaster.PayerOrPayee))
                                dbSmartAspects.AddInParameter(commandMaster, "@PayerOrPayee", DbType.String, ledgerMaster.PayerOrPayee);
                            else
                                dbSmartAspects.AddInParameter(commandMaster, "@PayerOrPayee", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(ledgerMaster.ReferenceNumber))
                                dbSmartAspects.AddInParameter(commandMaster, "@ReferenceNumber", DbType.String, ledgerMaster.ReferenceNumber);
                            else
                                dbSmartAspects.AddInParameter(commandMaster, "@ReferenceNumber", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandMaster, "@GLStatus", DbType.String, ledgerMaster.GLStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, ledgerMaster.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        }

                        if (status > 0 && newLedgerDetails.Count() > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveLedgerDetails_SP"))
                            {
                                foreach (GLLedgerDetailsBO DetailBO in newLedgerDetails)
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@LedgerMasterId", DbType.Int64, ledgerMaster.LedgerMasterId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@NodeId", DbType.Int32, DetailBO.NodeId);

                                    if (DetailBO.ChequeDate != null)
                                        dbSmartAspects.AddInParameter(commandDetails, "@BankAccountId", DbType.Int32, DetailBO.BankAccountId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@BankAccountId", DbType.Int32, DBNull.Value);

                                    if (!string.IsNullOrEmpty(DetailBO.ChequeNumber))
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeNumber", DbType.String, DetailBO.ChequeNumber);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeNumber", DbType.String, DBNull.Value);

                                    if (DetailBO.ChequeDate != null)
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeDate", DbType.Date, DetailBO.ChequeDate);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeDate", DbType.Date, DBNull.Value);

                                    dbSmartAspects.AddInParameter(commandDetails, "@DRAmount", DbType.Decimal, DetailBO.DRAmount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CRAmount", DbType.Decimal, DetailBO.CRAmount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@NodeNarration", DbType.String, DetailBO.NodeNarration);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CostCenterId", DbType.Int32, DetailBO.CostCenterId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CurrencyAmount", DbType.Decimal, DetailBO.CurrencyAmount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@NodeType", DbType.String, DetailBO.NodeType);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && editLedgerDetails.Count() > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateLedgerDetails_SP"))
                            {
                                foreach (GLLedgerDetailsBO DetailBO in editLedgerDetails)
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@LedgerDetailsId", DbType.Int64, DetailBO.LedgerDetailsId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@LedgerMasterId", DbType.Int64, ledgerMaster.LedgerMasterId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@NodeId", DbType.Int32, DetailBO.NodeId);

                                    if (DetailBO.ChequeDate != null)
                                        dbSmartAspects.AddInParameter(commandDetails, "@BankAccountId", DbType.Int32, DetailBO.BankAccountId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@BankAccountId", DbType.Int32, DBNull.Value);

                                    if (!string.IsNullOrEmpty(DetailBO.ChequeNumber))
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeNumber", DbType.String, DetailBO.ChequeNumber);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeNumber", DbType.String, DBNull.Value);

                                    if (DetailBO.ChequeDate != null)
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeDate", DbType.Date, DetailBO.ChequeDate);
                                    else
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeDate", DbType.Date, DBNull.Value);

                                    dbSmartAspects.AddInParameter(commandDetails, "@DRAmount", DbType.Decimal, DetailBO.DRAmount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CRAmount", DbType.Decimal, DetailBO.CRAmount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@NodeNarration", DbType.String, DetailBO.NodeNarration);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CostCenterId", DbType.Int32, DetailBO.CostCenterId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CurrencyAmount", DbType.Decimal, DetailBO.CurrencyAmount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@NodeType", DbType.String, DetailBO.NodeType);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && deleteLedgerDetails.Count() > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("DeleteLedgerDetails_SP"))
                            {
                                foreach (GLLedgerDetailsBO DetailBO in deleteLedgerDetails)
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@LedgerDetailsId", DbType.Int64, DetailBO.LedgerDetailsId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@LedgerMasterId", DbType.Int64, ledgerMaster.LedgerMasterId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            if (!string.IsNullOrWhiteSpace(ledgerMaster.ChequeNumber) && ledgerMaster.ChequeDate != null)
                            {
                                using (DbCommand commandVoucherCheque = dbSmartAspects.GetStoredProcCommand("UpdateVoucherChequeInfoByLedgerMasterId_SP"))
                                {
                                    commandVoucherCheque.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandVoucherCheque, "@LedgerMasterId", DbType.Int64, ledgerMaster.LedgerMasterId);
                                    dbSmartAspects.AddInParameter(commandVoucherCheque, "@ChequeNumber", DbType.String, ledgerMaster.ChequeNumber);
                                    dbSmartAspects.AddInParameter(commandVoucherCheque, "@ChequeDate", DbType.Date, ledgerMaster.ChequeDate);
                                    status = dbSmartAspects.ExecuteNonQuery(commandVoucherCheque, transction);
                                }
                            }
                        }

                        if (status > 0 && !string.IsNullOrEmpty(deletedDocument))
                        {
                            bool deleteDocument = new DocumentsDA().DeleteDocumentsByDocumentIdListString(deletedDocument);
                        }
                        if (status > 0)
                        {
                            bool update = new DocumentsDA().UpdateRandomDocumentOwnwerIdWithOwnerId(ledgerMaster.LedgerMasterId, RandomId);
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
        public Boolean DeleteVoucher(long ledgerMasterId)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("DeleteLedger_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@LedgerMasterId", DbType.Int64, ledgerMasterId);
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
        public Boolean UpdateVoucherApprovalStatus(List<GLVoucherApprovalVwBO> VocuherApproval)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateVoucherApprovalStatus_SP"))
                        {
                            foreach (GLVoucherApprovalVwBO vp in VocuherApproval)
                            {
                                commandMaster.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandMaster, "@LedgerMasterId", DbType.Int64, vp.LedgerMasterId);
                                dbSmartAspects.AddInParameter(commandMaster, "@GLStatus", DbType.String, vp.GLStatus);
                                dbSmartAspects.AddInParameter(commandMaster, "@ApprovedBy", DbType.Int32, vp.ApprovedRCheckedby);
                                status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
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
        public List<GLLedgerMasterVwBO> GetVoucherBySearchCriteria(int companyId, int projectId, int userInfoId, int userGroupId, string voucherType, string voucherStatus, string voucherNo, DateTime? fromDate, DateTime? toDate, string referenceNo, string referenceVoucherNo, string narration, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<GLLedgerMasterVwBO> voucherSearch = new List<GLLedgerMasterVwBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVoucherBySearchCriteria_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (companyId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    if (projectId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    if (!string.IsNullOrEmpty(voucherNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@VoucherNo", DbType.String, voucherNo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@VoucherNo", DbType.String, DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(referenceNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReferenceNo", DbType.String, referenceNo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReferenceNo", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(referenceVoucherNo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReferenceVoucherNo", DbType.String, referenceVoucherNo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReferenceVoucherNo", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(narration))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Narration", DbType.String, narration);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Narration", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(voucherType))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@VoucherType", DbType.String, voucherType);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@VoucherType", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(voucherStatus))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@VoucherStatus", DbType.String, voucherStatus);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@VoucherStatus", DbType.String, DBNull.Value);
                    }

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "VoucherSearch");
                    DataTable Table = ds.Tables["VoucherSearch"];

                    voucherSearch = Table.AsEnumerable().Select(r => new GLLedgerMasterVwBO
                    {
                        LedgerMasterId = r.Field<Int64>("LedgerMasterId"),
                        VoucherType = r.Field<string>("VoucherType"),
                        VoucherNo = r.Field<string>("VoucherNo"),
                        VoucherDate = r.Field<DateTime>("VoucherDate"),
                        Narration = r.Field<string>("Narration"),
                        ReferenceNumber = r.Field<string>("ReferenceNumber"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ProjectName = r.Field<string>("ProjectName"),
                        VoucherTypeName = r.Field<string>("VoucherTypeName"),
                        GLStatus = r.Field<string>("GLStatus"),
                        CreatedBy = r.Field<int>("CreatedBy"),
                        CanEditDeleteAfterApproved = r.Field<int>("CanEditDeleteAfterApproved"),
                        VoucherDateString = r.Field<string>("VoucherDateString"),
                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanDelete = r.Field<bool>("IsCanDelete"),
                        IsCanCheck = r.Field<bool>("IsCanCheck"),
                        IsCanApprove = r.Field<bool>("IsCanApprove"),
                        VoucherTotalAmount = r.Field<decimal>("VoucherTotalAmount"),
                        IsModulesTransaction = r.Field<bool>("IsModulesTransaction")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return voucherSearch;
        }
        public GLLedgerMasterVwBO GetVoucherById(Int64 ledgerMasterId)
        {
            GLLedgerMasterVwBO voucherSearch = new GLLedgerMasterVwBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVoucherById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LedgerMasterId", DbType.Int64, ledgerMasterId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "VoucherMaster");
                    DataTable Table = ds.Tables["VoucherMaster"];

                    voucherSearch = Table.AsEnumerable().Select(r => new GLLedgerMasterVwBO
                    {
                        LedgerMasterId = r.Field<Int64>("LedgerMasterId"),
                        CompanyId = r.Field<int>("CompanyId"),
                        ProjectId = r.Field<int>("ProjectId"),
                        DonorId = r.Field<int?>("DonorId"),
                        VoucherType = r.Field<string>("VoucherType"),
                        IsBankExist = r.Field<bool>("IsBankExist"),
                        VoucherNo = r.Field<string>("VoucherNo"),
                        VoucherDate = r.Field<DateTime>("VoucherDate"),
                        CurrencyId = r.Field<int>("CurrencyId"),
                        ConvertionRate = r.Field<decimal?>("ConvertionRate"),
                        Narration = r.Field<string>("Narration"),
                        PayerOrPayee = r.Field<string>("PayerOrPayee"),
                        ReferenceNumber = r.Field<string>("ReferenceNumber"),
                        GLStatus = r.Field<string>("GLStatus"),
                        CheckedBy = r.Field<int?>("CheckedBy"),
                        ApprovedBy = r.Field<int?>("ApprovedBy"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ProjectName = r.Field<string>("ProjectName"),
                        DonorName = r.Field<string>("DonorName"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        VoucherTypeName = r.Field<string>("VoucherTypeName")
                    }).FirstOrDefault();
                }
            }
            return voucherSearch;
        }
        public List<GLLedgerDetailsVwBO> GetVoucherDetailsById(Int64 ledgerMasterId)
        {
            List<GLLedgerDetailsVwBO> voucherSearch = new List<GLLedgerDetailsVwBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVoucherDetailsById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LedgerMasterId", DbType.Int64, ledgerMasterId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "VoucherMaster");
                    DataTable Table = ds.Tables["VoucherMaster"];

                    voucherSearch = Table.AsEnumerable().Select(r => new GLLedgerDetailsVwBO
                    {
                        LedgerDetailsId = r.Field<Int64>("LedgerDetailsId"),
                        LedgerMasterId = r.Field<Int64>("LedgerMasterId"),
                        NodeId = r.Field<Int64>("NodeId"),
                        BankAccountId = r.Field<int?>("BankAccountId"),
                        ChequeNumber = r.Field<string>("ChequeNumber"),
                        ChequeDate = r.Field<DateTime?>("ChequeDate"),
                        DRAmount = r.Field<decimal>("DRAmount"),
                        CRAmount = r.Field<decimal>("CRAmount"),
                        NodeNarration = r.Field<string>("NodeNarration"),
                        CostCenterId = r.Field<int>("CostCenterId"),
                        CurrencyAmount = r.Field<decimal?>("CurrencyAmount"),
                        NodeType = r.Field<string>("NodeType"),
                        ParentId = r.Field<Int64?>("ParentId"),
                        ParentLedgerId = r.Field<Int64?>("ParentLedgerId"),
                        Hierarchy = r.Field<string>("Hierarchy"),
                        NodeHead = r.Field<string>("NodeHead"),
                        LedgerMode = r.Field<byte>("LedgerMode"),
                        IsEdited = false

                    }).ToList();
                }
            }
            return voucherSearch;
        }
        public List<GLLedgerMasterVwBO> GetVoucherByCompanyIdNProjectIdNDate(int companyId, int projectId, DateTime? date, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<GLLedgerMasterVwBO> voucherSearch = new List<GLLedgerMasterVwBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVoucherByCompanyIdNProjectIdNDate_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@Date", DbType.DateTime, date);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "VoucherSearch");
                    DataTable Table = ds.Tables["VoucherSearch"];

                    voucherSearch = Table.AsEnumerable().Select(r => new GLLedgerMasterVwBO
                    {
                        LedgerMasterId = r.Field<Int64>("LedgerMasterId"),
                        VoucherType = r.Field<string>("VoucherType"),
                        VoucherNo = r.Field<string>("VoucherNo"),
                        VoucherDate = r.Field<DateTime?>("VoucherDate"),
                        Narration = r.Field<string>("Narration"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ProjectName = r.Field<string>("ProjectName"),
                        VoucherTypeName = r.Field<string>("VoucherTypeName"),
                        GLStatus = r.Field<string>("GLStatus"),
                        CreatedBy = r.Field<int?>("CreatedBy"),
                        CanEditDeleteAfterApproved = r.Field<int>("CanEditDeleteAfterApproved"),
                        VoucherDateString = r.Field<string>("VoucherDateString"),
                        IsSynced = r.Field<bool>("IsSynced")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return voucherSearch;
        }
        public Boolean UpdateVoucherSyncInformation(long ledgerMasterId, string voucherNo, int userInfoId)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateVoucherSyncInformation_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@LedgerMasterId", DbType.Int64, ledgerMasterId);
                            dbSmartAspects.AddInParameter(commandMaster, "@UserInfoId", DbType.Int32, userInfoId);
                            dbSmartAspects.AddInParameter(commandMaster, "@VoucherNo", DbType.String, voucherNo);
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
        public Boolean UpdateVoucherStatusByVoucherNo(int companyIdUC, int projectIdUC, string voucherNo, string voucherStatus, int userInfoId)
        {
            bool retVal = false;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateVoucherStatusByVoucherNo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@VoucherNo", DbType.String, voucherNo);
                            dbSmartAspects.AddInParameter(commandMaster, "@GLStatus", DbType.String, voucherStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CompanyIdUC", DbType.String, companyIdUC);
                            dbSmartAspects.AddInParameter(commandMaster, "@ProjectIdUC", DbType.String, projectIdUC);
                            dbSmartAspects.AddInParameter(commandMaster, "@UserInfoId", DbType.Int32, userInfoId);
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
