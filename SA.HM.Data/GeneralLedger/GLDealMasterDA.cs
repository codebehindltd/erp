using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.GeneralLedger;
using System.Data.Common;
using System.Data;
using System.Collections;

namespace HotelManagement.Data.GeneralLedger
{
    public class GLDealMasterDA : BaseService
    {
        //public Boolean SaveGLMasterInfo(GLDealMasterBO masterBO, out int tmpDealId, out string currentVoucherNo, List<GLLedgerBO> detailBO)
        //{
        //    bool retVal = false;
        //    int status = 0;
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveGLDealMasterInfo_SP"))
        //        {
        //            conn.Open();
        //            using (DbTransaction transction = conn.BeginTransaction())
        //            {
        //                dbSmartAspects.AddInParameter(commandMaster, "@ProjectId", DbType.Int32, masterBO.ProjectId);
        //                dbSmartAspects.AddInParameter(commandMaster, "@VoucherType", DbType.String, masterBO.VoucherType);
        //                dbSmartAspects.AddInParameter(commandMaster, "@VoucherMode", DbType.Int32, masterBO.VoucherMode);
        //                dbSmartAspects.AddInParameter(commandMaster, "@CashChequeMode", DbType.Int32, masterBO.CashChequeMode);
        //                //dbSmartAspects.AddInParameter(commandMaster, "@VoucherNo", DbType.String, masterBO.VoucherNo);
        //                dbSmartAspects.AddInParameter(commandMaster, "@VoucherDate", DbType.DateTime, masterBO.VoucherDate);
        //                dbSmartAspects.AddInParameter(commandMaster, "@Narration", DbType.String, masterBO.Narration);
        //                dbSmartAspects.AddInParameter(commandMaster, "@PayerOrPayee", DbType.String, masterBO.PayerOrPayee);
        //                dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, masterBO.CreatedBy);
        //                dbSmartAspects.AddOutParameter(commandMaster, "@DealId", DbType.Int32, sizeof(Int32));
        //                dbSmartAspects.AddOutParameter(commandMaster, "@VoucherNo", DbType.String, 20);

        //                status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

        //                tmpDealId = Convert.ToInt32(commandMaster.Parameters["@DealId"].Value);
        //                currentVoucherNo = commandMaster.Parameters["@VoucherNo"].Value.ToString();

        //                if (status > 0)
        //                {
        //                    int count = 0;

        //                    using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveGLLedgerInfo_SP"))
        //                    {
        //                        foreach (GLLedgerBO DetailBO in detailBO)
        //                        {
        //                            commandDetails.Parameters.Clear();

        //                            dbSmartAspects.AddInParameter(commandDetails, "@DealId", DbType.Int32, tmpDealId);
        //                            dbSmartAspects.AddInParameter(commandDetails, "@NodeId", DbType.Int32, DetailBO.NodeId);
        //                            dbSmartAspects.AddInParameter(commandDetails, "@LedgerMode", DbType.Int32, DetailBO.LedgerMode);
        //                            dbSmartAspects.AddInParameter(commandDetails, "@BankAccountId", DbType.Int32, DetailBO.BankAccountId);
        //                            dbSmartAspects.AddInParameter(commandDetails, "@ChequeNumber", DbType.String, DetailBO.ChequeNumber);
        //                            dbSmartAspects.AddInParameter(commandDetails, "@LedgerAmount", DbType.Decimal, DetailBO.LedgerAmount);
        //                            dbSmartAspects.AddInParameter(commandDetails, "@NodeNarration", DbType.String, DetailBO.NodeNarration);
        //                            dbSmartAspects.AddInParameter(commandDetails, "@CostCentreId", DbType.Int32, DetailBO.CostCentreId);

        //                            dbSmartAspects.AddInParameter(commandDetails, "@FieldId", DbType.Int32, DetailBO.FieldId);
        //                            dbSmartAspects.AddInParameter(commandDetails, "@CurrencyAmount", DbType.Decimal, DetailBO.CurrencyAmount);

        //                            count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
        //                        }
        //                    }

        //                    if (count == detailBO.Count)
        //                    {
        //                        transction.Commit();
        //                        retVal = true;
        //                    }
        //                    else
        //                    {
        //                        retVal = false;
        //                    }
        //                }
        //                else
        //                {
        //                    retVal = false;
        //                }
        //            }
        //        }
        //    }
        //    return retVal;
        //}
        //public Boolean SaveGLMasterInfoWithApprovedInfo(GLDealMasterBO masterBO, out int tmpDealId, out string currentVoucherNo, List<GLLedgerBO> detailBO, List<GLVoucherApprovedInfoBO> approvedBOList)
        public Boolean SaveGLMasterInfo(GLDealMasterBO masterBO, out int tmpDealId, out string currentVoucherNo, List<GLLedgerBO> detailBO, List<GLVoucherApprovedInfoBO> approvedBOList)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveGLDealMasterInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@ProjectId", DbType.Int32, masterBO.ProjectId);
                        dbSmartAspects.AddInParameter(commandMaster, "@VoucherType", DbType.String, masterBO.VoucherType);
                        dbSmartAspects.AddInParameter(commandMaster, "@VoucherMode", DbType.Int32, masterBO.VoucherMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@CashChequeMode", DbType.Int32, masterBO.CashChequeMode);
                        //dbSmartAspects.AddInParameter(commandMaster, "@VoucherNo", DbType.String, masterBO.VoucherNo);
                        dbSmartAspects.AddInParameter(commandMaster, "@VoucherDate", DbType.DateTime, masterBO.VoucherDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@Narration", DbType.String, masterBO.Narration);                        
                        dbSmartAspects.AddInParameter(commandMaster, "@PayerOrPayee", DbType.String, masterBO.PayerOrPayee);
                        dbSmartAspects.AddInParameter(commandMaster, "@GLStatus", DbType.String, masterBO.GLStatus);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, masterBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(commandMaster, "@DealId", DbType.Int64, sizeof(Int64));
                        dbSmartAspects.AddOutParameter(commandMaster, "@VoucherNo", DbType.String, 20);

                        dbSmartAspects.AddInParameter(commandMaster, "@CheckedBy", DbType.Int32, masterBO.CheckedBy);
                        dbSmartAspects.AddInParameter(commandMaster, "@ApprovedBy", DbType.Int32, masterBO.ApprovedBy);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        tmpDealId = Convert.ToInt32(commandMaster.Parameters["@DealId"].Value);
                        currentVoucherNo = commandMaster.Parameters["@VoucherNo"].Value.ToString();

                        if (status > 0)
                        {
                            int count = 0;

                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveGLLedgerInfo_SP"))
                            {
                                foreach (GLLedgerBO DetailBO in detailBO)
                                {
                                    commandDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDetails, "@DealId", DbType.Int64, tmpDealId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@NodeId", DbType.Int64, DetailBO.NodeId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@LedgerMode", DbType.Int32, DetailBO.LedgerMode);
                                    dbSmartAspects.AddInParameter(commandDetails, "@BankAccountId", DbType.Int32, DetailBO.BankAccountId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ChequeNumber", DbType.String, DetailBO.ChequeNumber);
                                    dbSmartAspects.AddInParameter(commandDetails, "@LedgerAmount", DbType.Decimal, DetailBO.LedgerAmount);
                                    dbSmartAspects.AddInParameter(commandDetails, "@NodeNarration", DbType.String, DetailBO.NodeNarration);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CostCenterId", DbType.Int32, DetailBO.CostCenterId);

                                    dbSmartAspects.AddInParameter(commandDetails, "@FieldId", DbType.Int32, DetailBO.FieldId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CurrencyAmount", DbType.Decimal, DetailBO.CurrencyAmount);

                                    count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }

                            if (count == detailBO.Count)
                            {
                                // Approved Information -------------------------Start
                                if (approvedBOList != null)
                                {
                                    int approvedCount = 0;
                                    using (DbCommand commandApprovedDetails = dbSmartAspects.GetStoredProcCommand("SaveGLVoucherApprovedInfo_SP"))
                                    {
                                        foreach (GLVoucherApprovedInfoBO approvedBO in approvedBOList)
                                        {
                                            commandApprovedDetails.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandApprovedDetails, "@DealId", DbType.Int64, tmpDealId);
                                            dbSmartAspects.AddInParameter(commandApprovedDetails, "@ApprovedType", DbType.String, approvedBO.ApprovedType);
                                            dbSmartAspects.AddInParameter(commandApprovedDetails, "@UserInfoId", DbType.Int32, approvedBO.UserInfoId);

                                            approvedCount += dbSmartAspects.ExecuteNonQuery(commandApprovedDetails, transction);
                                        }
                                    }

                                    if (approvedCount == approvedBOList.Count)
                                    {
                                        transction.Commit();
                                        retVal = true;
                                    }
                                    else
                                    {
                                        retVal = false;
                                    }
                                }
                                else
                                {
                                    transction.Commit();
                                    retVal = true;
                                }
                                // Approved Information -------------------------End

                                //transction.Commit();
                                //retVal = true;
                            }
                            else
                            {
                                retVal = false;
                            }
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public GLDealMasterBO GetVoucherInfoByDealId(int DealId)
        {
            GLDealMasterBO masterBO = new GLDealMasterBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVoucherInfoByDealId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.Int64, DealId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                masterBO.DealId = Convert.ToInt64(reader["DealId"]);
                               
                                masterBO.ProjectId =Convert.ToInt32( reader["ProjectId"].ToString());
                                masterBO.CompanyId = Convert.ToInt32(reader["CompanyId"].ToString());
                                masterBO.VoucherMode =Convert.ToInt32( reader["VoucherMode"].ToString());
                                masterBO.VoucherNo = reader["VoucherNo"].ToString();
                                masterBO.CashChequeMode = Convert.ToInt32( reader["CashChequeMode"].ToString());
                                masterBO.Narration = reader["Narration"].ToString();
                                masterBO.GLStatus =reader["GLStatus"].ToString();
                                masterBO.VoucherDate = Convert.ToDateTime(reader["VoucherDate"].ToString());

                                masterBO.CheckedBy = Convert.ToInt32(reader["CheckedBy"].ToString());
                                masterBO.ApprovedBy = Convert.ToInt32(reader["ApprovedBy"].ToString());

                                masterBO.CreatedByName = Convert.ToString(reader["CreatedByName"]);
                                masterBO.CheckedByName = Convert.ToString(reader["CheckedByName"]);
                                masterBO.ApprovedByName = Convert.ToString(reader["ApprovedByName"]);
                            }
                        }
                    }
                }
            }
            return masterBO;
        }
        public List<GLLedgerBO> GetVoucherDetailsInfoByDealId(int DealId)
        {

            List<GLLedgerBO> List = new List<GLLedgerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVoucherDetailsInfoByDealId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DealId", DbType.Int64, DealId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLLedgerBO detailsBO = new GLLedgerBO();
                                detailsBO.LedgerId = Convert.ToInt32(reader["LedgerId"]);
                                detailsBO.NodeId = Convert.ToInt64(reader["NodeId"].ToString());
                                detailsBO.NodeHead = reader["NodeHead"].ToString();
                                detailsBO.LedgerMode = Convert.ToInt32(reader["LedgerMode"].ToString());
                                detailsBO.LedgerAmount = Convert.ToDecimal(reader["LedgerAmount"].ToString());
                                detailsBO.LedgerDebitAmount = Convert.ToDecimal(reader["DebitAmount"].ToString());
                                detailsBO.LedgerCreditAmount = Convert.ToDecimal(reader["CreditAmount"].ToString());
                                detailsBO.BankAccountId =Convert.ToInt32( reader["BankAccountId"].ToString());
                                detailsBO.VoucherMode = Convert.ToInt32(reader["VoucherMode"].ToString());
                                detailsBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"].ToString());
                                List.Add(detailsBO);
                            }
                        }
                    }
                }
            }
            return List;
        }
        public bool UpdateGLMasterInfo(GLDealMasterBO glMasterBO, List<GLLedgerBO> detailBO, ArrayList arrayDelete, List<GLVoucherApprovedInfoBO> approvedBOList)
        {
            bool retVal = false;
            int status = 0;
            Int64 tmpDealId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateGLMasterInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@DealId", DbType.Int64, glMasterBO.DealId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ProjectId", DbType.Int32, glMasterBO.ProjectId);
                        dbSmartAspects.AddInParameter(commandMaster, "@VoucherMode", DbType.Int32, glMasterBO.VoucherMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@CashChequeMode", DbType.Int32, glMasterBO.CashChequeMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@VoucherNo", DbType.String, glMasterBO.VoucherNo);
                        dbSmartAspects.AddInParameter(commandMaster, "@VoucherDate", DbType.DateTime, glMasterBO.VoucherDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@Narration", DbType.String, glMasterBO.Narration);
                        dbSmartAspects.AddInParameter(commandMaster, "@PayerOrPayee", DbType.String, glMasterBO.PayerOrPayee);
                        dbSmartAspects.AddInParameter(commandMaster, "@GLStatus", DbType.String, glMasterBO.GLStatus);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, glMasterBO.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        tmpDealId = glMasterBO.DealId;

                        if (status > 0)
                        {
                            int count = 0;
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveGLLedgerInfo_SP"))
                            {
                                foreach (GLLedgerBO DetailBO in detailBO)
                                {
                                    if (DetailBO.LedgerId == 0)
                                    {
                                        commandDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandDetails, "@DealId", DbType.Int64, tmpDealId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@NodeId", DbType.Int64, DetailBO.NodeId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@LedgerMode", DbType.Int32, DetailBO.LedgerMode);
                                        dbSmartAspects.AddInParameter(commandDetails, "@BankAccountId", DbType.Int32, DetailBO.BankAccountId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeNumber", DbType.String, DetailBO.ChequeNumber);
                                        dbSmartAspects.AddInParameter(commandDetails, "@LedgerAmount", DbType.Decimal, DetailBO.LedgerAmount);
                                        dbSmartAspects.AddInParameter(commandDetails, "@NodeNarration", DbType.String, DetailBO.NodeNarration);
                                        dbSmartAspects.AddInParameter(commandDetails, "@CostCenterId", DbType.Int32, DetailBO.CostCenterId);

                                        dbSmartAspects.AddInParameter(commandDetails, "@FieldId", DbType.Int32, DetailBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@CurrencyAmount", DbType.Decimal, DetailBO.CurrencyAmount);

                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateGLLedgerInfo_SP"))
                            {
                                foreach (GLLedgerBO DetailBO in detailBO)
                                {
                                    if (DetailBO.LedgerId != 0)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@LedgerId", DbType.Int64, DetailBO.LedgerId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@DealId", DbType.Int64, tmpDealId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@NodeId", DbType.Int64, DetailBO.NodeId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@LedgerMode", DbType.Int32, DetailBO.LedgerMode);
                                        dbSmartAspects.AddInParameter(commandDetails, "@BankAccountId", DbType.Int32, DetailBO.BankAccountId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeNumber", DbType.String, DetailBO.ChequeNumber);
                                        dbSmartAspects.AddInParameter(commandDetails, "@LedgerAmount", DbType.Decimal, DetailBO.LedgerAmount);
                                        dbSmartAspects.AddInParameter(commandDetails, "@NodeNarration", DbType.String, DetailBO.NodeNarration);
                                        dbSmartAspects.AddInParameter(commandDetails, "@CostCenterId", DbType.Int32, DetailBO.CostCenterId);

                                        dbSmartAspects.AddInParameter(commandDetails, "@FieldId", DbType.Int32, DetailBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@CurrencyAmount", DbType.Decimal, DetailBO.CurrencyAmount);

                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            if (count == detailBO.Count)
                            {
                                if (arrayDelete != null)
                                {
                                    if (arrayDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayDelete)
                                        {
                                            using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandEducation, "@TableName", DbType.String, "GLLedger");
                                                dbSmartAspects.AddInParameter(commandEducation, "@TablePKField", DbType.String, "LedgerId");
                                                dbSmartAspects.AddInParameter(commandEducation, "@TablePKId", DbType.String, delId);
                                                status = dbSmartAspects.ExecuteNonQuery(commandEducation);
                                            }
                                        }
                                    }
                                }
                                //transction.Commit();
                                //retVal = true;
                                // Approved Information -------------------------Start
                                if (approvedBOList != null)
                                {
                                    int approved = 0;
                                    int approvedCount = 0;
                                    using (DbCommand commandApprovedDetails = dbSmartAspects.GetStoredProcCommand("UpdateGLVoucherApprovedInfo_SP"))
                                    {
                                        foreach (GLVoucherApprovedInfoBO approvedBO in approvedBOList)
                                        {
                                            commandApprovedDetails.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(commandApprovedDetails, "@DealId", DbType.Int64, tmpDealId);
                                            dbSmartAspects.AddInParameter(commandApprovedDetails, "@ApprovedType", DbType.String, approvedBO.ApprovedType);
                                            dbSmartAspects.AddInParameter(commandApprovedDetails, "@UserInfoId", DbType.Int32, approvedBO.UserInfoId);

                                            approved += dbSmartAspects.ExecuteNonQuery(commandApprovedDetails, transction);

                                            approvedCount = approvedCount + 1;
                                        }
                                    }

                                    if (approvedCount == approvedBOList.Count)
                                    {
                                        transction.Commit();
                                        retVal = true;
                                    }
                                    else
                                    {
                                        retVal = false;
                                    }
                                }
                                else
                                {
                                    transction.Commit();
                                    retVal = true;
                                }
                            }
                            else
                            {
                                retVal = false;
                            }
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
        public bool UpdateGLMasterInfoWithApprovedInfo(GLDealMasterBO glMasterBO, List<GLLedgerBO> detailBO, ArrayList arrayDelete, List<GLVoucherApprovedInfoBO> approvedBOList)
        {
            bool retVal = false;
            int status = 0;
            Int64 tmpDealId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateGLMasterInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@DealId", DbType.Int64, glMasterBO.DealId);
                        dbSmartAspects.AddInParameter(commandMaster, "@ProjectId", DbType.Int32, glMasterBO.ProjectId);
                        dbSmartAspects.AddInParameter(commandMaster, "@VoucherMode", DbType.Int32, glMasterBO.VoucherMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@CashChequeMode", DbType.Int32, glMasterBO.CashChequeMode);
                        dbSmartAspects.AddInParameter(commandMaster, "@VoucherNo", DbType.String, glMasterBO.VoucherNo);
                        dbSmartAspects.AddInParameter(commandMaster, "@VoucherDate", DbType.DateTime, glMasterBO.VoucherDate);
                        dbSmartAspects.AddInParameter(commandMaster, "@Narration", DbType.String, glMasterBO.Narration);
                        dbSmartAspects.AddInParameter(commandMaster, "@PayerOrPayee", DbType.String, glMasterBO.PayerOrPayee);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, glMasterBO.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        tmpDealId = glMasterBO.DealId;

                        if (status > 0)
                        {
                            int count = 0;
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveGLLedgerInfo_SP"))
                            {
                                foreach (GLLedgerBO DetailBO in detailBO)
                                {
                                    if (DetailBO.LedgerId == 0)
                                    {
                                        commandDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandDetails, "@DealId", DbType.Int64, tmpDealId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@NodeId", DbType.Int64, DetailBO.NodeId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@LedgerMode", DbType.Int32, DetailBO.LedgerMode);
                                        dbSmartAspects.AddInParameter(commandDetails, "@BankAccountId", DbType.Int32, DetailBO.BankAccountId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeNumber", DbType.String, DetailBO.ChequeNumber);
                                        dbSmartAspects.AddInParameter(commandDetails, "@LedgerAmount", DbType.Decimal, DetailBO.LedgerAmount);
                                        dbSmartAspects.AddInParameter(commandDetails, "@NodeNarration", DbType.String, DetailBO.NodeNarration);
                                        dbSmartAspects.AddInParameter(commandDetails, "@CostCenterId", DbType.Int32, DetailBO.CostCenterId);

                                        dbSmartAspects.AddInParameter(commandDetails, "@FieldId", DbType.Int32, DetailBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@CurrencyAmount", DbType.Decimal, DetailBO.CurrencyAmount);

                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateGLLedgerInfo_SP"))
                            {
                                foreach (GLLedgerBO DetailBO in detailBO)
                                {
                                    if (DetailBO.LedgerId != 0)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@LedgerId", DbType.Int64, DetailBO.LedgerId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@DealId", DbType.Int64, tmpDealId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@NodeId", DbType.Int64, DetailBO.NodeId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@LedgerMode", DbType.Int32, DetailBO.LedgerMode);
                                        dbSmartAspects.AddInParameter(commandDetails, "@BankAccountId", DbType.Int32, DetailBO.BankAccountId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@ChequeNumber", DbType.String, DetailBO.ChequeNumber);
                                        dbSmartAspects.AddInParameter(commandDetails, "@LedgerAmount", DbType.Decimal, DetailBO.LedgerAmount);
                                        dbSmartAspects.AddInParameter(commandDetails, "@NodeNarration", DbType.String, DetailBO.NodeNarration);
                                        dbSmartAspects.AddInParameter(commandDetails, "@CostCenterId", DbType.Int32, DetailBO.CostCenterId);

                                        dbSmartAspects.AddInParameter(commandDetails, "@FieldId", DbType.Int32, DetailBO.FieldId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@CurrencyAmount", DbType.Decimal, DetailBO.CurrencyAmount);

                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            if (count == detailBO.Count)
                            {
                                if (arrayDelete != null)
                                {
                                    if (arrayDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayDelete)
                                        {
                                            using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandEducation, "@TableName", DbType.String, "GLLedger");
                                                dbSmartAspects.AddInParameter(commandEducation, "@TablePKField", DbType.String, "LedgerId");
                                                dbSmartAspects.AddInParameter(commandEducation, "@TablePKId", DbType.String, delId);
                                                status = dbSmartAspects.ExecuteNonQuery(commandEducation);
                                            }
                                        }
                                    }
                                }
                                //transction.Commit();
                                //retVal = true;

                                // ---------Voucher Approved Information ------------------------Start
                                if (approvedBOList != null)
                                {
                                    int approvedCount = 0;
                                    using (DbCommand commandSaveDetails = dbSmartAspects.GetStoredProcCommand("SaveGLVoucherApprovedInfo_SP"))
                                    {
                                        foreach (GLVoucherApprovedInfoBO approvedBO in approvedBOList)
                                        {
                                            if (approvedBO.ApprovedId == 0)
                                            {
                                                commandSaveDetails.Parameters.Clear();

                                                dbSmartAspects.AddInParameter(commandSaveDetails, "@DealId", DbType.Int64, tmpDealId);
                                                dbSmartAspects.AddInParameter(commandSaveDetails, "@ApprovedType", DbType.String, approvedBO.ApprovedType);
                                                dbSmartAspects.AddInParameter(commandSaveDetails, "@UserInfoId", DbType.Int32, approvedBO.UserInfoId);

                                                approvedCount += dbSmartAspects.ExecuteNonQuery(commandSaveDetails, transction);
                                            }
                                        }
                                    }
                                    using (DbCommand commandUpdateDetails = dbSmartAspects.GetStoredProcCommand("UpdateGLVoucherApprovedInfo_SP"))
                                    {
                                        foreach (GLVoucherApprovedInfoBO approvedBO in approvedBOList)
                                        {
                                            if (approvedBO.ApprovedId != 0)
                                            {
                                                commandUpdateDetails.Parameters.Clear();
                                                dbSmartAspects.AddInParameter(commandUpdateDetails, "@ApprovedId", DbType.Int32, approvedBO.ApprovedId);
                                                dbSmartAspects.AddInParameter(commandUpdateDetails, "@DealId", DbType.Int64, tmpDealId);
                                                dbSmartAspects.AddInParameter(commandUpdateDetails, "@ApprovedType", DbType.String, approvedBO.ApprovedType);
                                                dbSmartAspects.AddInParameter(commandUpdateDetails, "@UserInfoId", DbType.Int32, approvedBO.UserInfoId);

                                                approvedCount += dbSmartAspects.ExecuteNonQuery(commandUpdateDetails, transction);
                                            }
                                        }
                                    }

                                    transction.Commit();
                                    retVal = true;

                                    //if (count == detailBO.Count)
                                    //{
                                    //    if (arrayDelete != null)
                                    //    {
                                    //        if (arrayDelete.Count > 0)
                                    //        {
                                    //            foreach (int delId in arrayDelete)
                                    //            {
                                    //                using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                    //                {
                                    //                    dbSmartAspects.AddInParameter(commandEducation, "@TableName", DbType.String, "GLLedger");
                                    //                    dbSmartAspects.AddInParameter(commandEducation, "@TablePKField", DbType.String, "LedgerId");
                                    //                    dbSmartAspects.AddInParameter(commandEducation, "@TablePKId", DbType.String, delId);
                                    //                    status = dbSmartAspects.ExecuteNonQuery(commandEducation);
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //    transction.Commit();
                                    //    retVal = true;
                                    //}
                                    //else
                                    //{
                                    //    retVal = false;
                                    //}
                                }
                                else
                                {
                                    transction.Commit();
                                    retVal = true;
                                }

                                // ---------Voucher Approved Information ------------------------End
                            }
                            else
                            {
                                retVal = false;
                            }
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
    }
}
