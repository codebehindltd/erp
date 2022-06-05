using HotelManagement.Entity.SupportAndTicket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.SupportAndTicket
{
    public class SupportNCaseSetupDA : BaseService
    {
        public Boolean SaveOrUpdateSetupInfo(STSupportNCaseSetupInfoBO SupportNCase, out int id)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSupportNCaseSetup_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, SupportNCase.Id);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, SupportNCase.Name);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, SupportNCase.Description);
                        dbSmartAspects.AddInParameter(command, "@SetupType", DbType.String, SupportNCase.SetupType);
                        dbSmartAspects.AddInParameter(command, "@Status", DbType.Boolean, SupportNCase.Status);
                        if (SupportNCase.PriorityLabel != null)
                            dbSmartAspects.AddInParameter(command, "@PriorityLabel", DbType.Int32, SupportNCase.PriorityLabel);
                        else
                            dbSmartAspects.AddInParameter(command, "@PriorityLabel", DbType.Int32, DBNull.Value);
                        if (SupportNCase.IsCloseStage != null)
                            dbSmartAspects.AddInParameter(command, "@IsCloseStage", DbType.Boolean, SupportNCase.IsCloseStage);
                        else
                            dbSmartAspects.AddInParameter(command, "@IsCloseStage", DbType.Boolean, DBNull.Value);
                        if (SupportNCase.IsDeclineStage != null)
                            dbSmartAspects.AddInParameter(command, "@IsDeclineStage", DbType.Boolean, SupportNCase.IsDeclineStage);
                        else
                            dbSmartAspects.AddInParameter(command, "@IsDeclineStage", DbType.Boolean, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@UserId", DbType.Int32, SupportNCase.CreatedBy); ;
                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);

                        id = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public bool SaveOrUpdateSupportFeedback(STSupportBO support, out long id)
        {

            Boolean status = false;
            int Recentstatus = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSupportFeedback_SP"))
                        {
                            command.Parameters.Clear();
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, support.Id);
                            dbSmartAspects.AddInParameter(command, "@FeedbackDate", DbType.DateTime, support.FeedbackDate);
                            dbSmartAspects.AddInParameter(command, "@Feedback", DbType.String, support.Feedback);
                            dbSmartAspects.AddInParameter(command, "@SupportStatus", DbType.String, support.SupportStatus);
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, support.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                            Recentstatus = dbSmartAspects.ExecuteNonQuery(command, transction);
                            id = Convert.ToInt64(command.Parameters["@OutId"].Value);

                        }
                        if (Recentstatus > 0)
                        {
                            status = true;
                            transction.Commit();
                        }
                        else
                        {
                            status = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return status;
        }
        public bool GenerateSupportBill(List<long> idList, string sContactId, string sPaymentInstructionId, int userinfoId, string BillStatus)
        {

            Boolean status = false;
            int Recentstatus = 0;
            int contactId = 0;
            int paymentInstructionId = 0;

            if (!string.IsNullOrWhiteSpace(sContactId))
            {
                contactId = Convert.ToInt32(sContactId);
            }

            if (!string.IsNullOrWhiteSpace(sPaymentInstructionId))
            {
                paymentInstructionId = Convert.ToInt32(sPaymentInstructionId);
            }

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (long id in idList)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GenerateSupportBill_SP"))
                            {
                                command.Parameters.Clear();
                                dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, id);
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, contactId);
                                dbSmartAspects.AddInParameter(command, "@PaymentInstructionId", DbType.Int32, paymentInstructionId);
                                dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, BillStatus);
                                dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, userinfoId);

                                Recentstatus = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }
                        if (Recentstatus > 0)
                        {
                            status = true;
                            transction.Commit();
                        }
                        else
                        {
                            status = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return status;
        }
        public void CallCenterCallCenterBillingAccountsVoucherPostingProcess_SP(long billID)
        {
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand commandBillDetails = dbSmartAspects.GetStoredProcCommand("CallCenterCallCenterBillingAccountsVoucherPostingProcess_SP"))
                    {
                        dbSmartAspects.AddInParameter(commandBillDetails, "@BillId", DbType.Int64, billID);
                        dbSmartAspects.AddOutParameter(commandBillDetails, "@mErr", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(commandBillDetails, transction);
                    }

                    if (status > 0)
                    {
                        transction.Commit();
                    }
                    else
                    {
                        transction.Rollback();
                    }
                }
            }
        }
        public bool SaveOrUpdateSupport(STSupportBO support, List<STSupportDetailsBO> supportItems, List<STSupportDetailsBO> supportItemDeleted, List<STSupportDetailsBO> SupportItemForSupportDetails, List<STSupportDetailsBO> SupportItemDeletedForSupportDetails, string supportCallType, out long id)
        {
            Boolean status = false;
            int Recentstatus = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSupport_SP"))
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, support.Id);
                            dbSmartAspects.AddInParameter(command, "@CaseOwnerId", DbType.Int32, support.CaseOwnerId);
                            dbSmartAspects.AddInParameter(command, "@ClientId", DbType.Int32, support.ClientId);
                            dbSmartAspects.AddInParameter(command, "@SupportCategoryId", DbType.Int32, support.SupportCategoryId);
                            dbSmartAspects.AddInParameter(command, "@SupportSource", DbType.String, support.SupportSource);
                            dbSmartAspects.AddInParameter(command, "@SupportSourceOtherDetails", DbType.String, support.SupportSourceOtherDetails);
                            dbSmartAspects.AddInParameter(command, "@CaseId", DbType.Int32, support.CaseId);
                            dbSmartAspects.AddInParameter(command, "@CaseDetails", DbType.String, support.CaseDetails);
                            dbSmartAspects.AddInParameter(command, "@ItemOrServiceDetails", DbType.String, support.ItemOrServiceDetails);
                            dbSmartAspects.AddInParameter(command, "@SupportStageId", DbType.Int32, support.SupportStageId);
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, support.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                            Recentstatus = dbSmartAspects.ExecuteNonQuery(command, transction);
                            id = Convert.ToInt64(command.Parameters["@OutId"].Value);


                        }

                        if (Recentstatus > 0 && supportItems.Count > 0)
                        {
                            foreach (STSupportDetailsBO stsd in supportItems)
                            {
                                using (DbCommand cmdDetails = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSTSupportDetails_SP"))
                                {
                                    cmdDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdDetails, "@STSupportDetailsId", DbType.Int32, stsd.STSupportDetailsId);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@ItemId", DbType.Int32, stsd.ItemId);


                                    dbSmartAspects.AddInParameter(cmdDetails, "@CategoryId", DbType.Int32, stsd.CategoryId);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@StockBy", DbType.Int32, stsd.StockBy);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@Type", DbType.String, stsd.Type);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@STSupportId", DbType.Int32, id);

                                    if (stsd.UnitPrice > 0)
                                        dbSmartAspects.AddInParameter(cmdDetails, "@UnitPrice", DbType.Decimal, stsd.UnitPrice);
                                    else
                                        dbSmartAspects.AddInParameter(cmdDetails, "@UnitPrice", DbType.Decimal, DBNull.Value);

                                    if (stsd.UnitQuantity > 0)
                                        dbSmartAspects.AddInParameter(cmdDetails, "@UnitQuantity", DbType.Decimal, stsd.UnitQuantity);
                                    else
                                        dbSmartAspects.AddInParameter(cmdDetails, "@UnitQuantity", DbType.Decimal, DBNull.Value);

                                    if (stsd.VatRate > 0)
                                        dbSmartAspects.AddInParameter(cmdDetails, "@VatRate", DbType.Decimal, stsd.VatRate);
                                    else
                                        dbSmartAspects.AddInParameter(cmdDetails, "@VatRate", DbType.Decimal, DBNull.Value);

                                    if (stsd.VatAmount > 0)
                                        dbSmartAspects.AddInParameter(cmdDetails, "@VatAmount", DbType.Decimal, stsd.VatAmount);
                                    else
                                        dbSmartAspects.AddInParameter(cmdDetails, "@VatAmount", DbType.Decimal, DBNull.Value);

                                    if (stsd.TotalPrice > 0)
                                        dbSmartAspects.AddInParameter(cmdDetails, "@TotalPrice", DbType.Decimal, stsd.TotalPrice);
                                    else
                                        dbSmartAspects.AddInParameter(cmdDetails, "@TotalPrice", DbType.Decimal, DBNull.Value);

                                    Recentstatus = dbSmartAspects.ExecuteNonQuery(cmdDetails, transction);
                                }
                            }
                        }
                        if (Recentstatus > 0 && supportItemDeleted.Count > 0)
                        {
                            using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteSTSupportDetails_SP"))
                            {
                                foreach (STSupportDetailsBO stsd in supportItemDeleted)
                                {
                                    commandDelete.Parameters.Clear();


                                    dbSmartAspects.AddInParameter(commandDelete, "@STSupportDetailsId", DbType.Int32, stsd.STSupportDetailsId);
                                    dbSmartAspects.AddInParameter(commandDelete, "@ItemId", DbType.Int32, stsd.ItemId);


                                    dbSmartAspects.AddInParameter(commandDelete, "@CategoryId", DbType.Int32, stsd.CategoryId);
                                    dbSmartAspects.AddInParameter(commandDelete, "@StockBy", DbType.Int32, stsd.StockBy);
                                    dbSmartAspects.AddInParameter(commandDelete, "@Type", DbType.String, stsd.Type);
                                    dbSmartAspects.AddInParameter(commandDelete, "@STSupportId", DbType.Int32, id);

                                    Recentstatus = dbSmartAspects.ExecuteNonQuery(commandDelete, transction);
                                }
                            }
                        }

                        if (supportCallType == "Details")
                        {
                            if (support.Id != 0)
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSupportCallDetails_SP"))
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, support.Id);
                                    dbSmartAspects.AddInParameter(command, "@InternalNotesDetails", DbType.String, support.InternalNotesDetails);
                                    dbSmartAspects.AddInParameter(command, "@SupportTypeId", DbType.Int32, support.SupportTypeId);
                                    dbSmartAspects.AddInParameter(command, "@SupportPriorityId", DbType.Int32, support.SupportPriorityId);
                                    dbSmartAspects.AddInParameter(command, "@SupportForwardToId", DbType.Int32, support.SupportForwardToId);
                                    dbSmartAspects.AddInParameter(command, "@SupportDeadline", DbType.DateTime, support.SupportDeadline);
                                    dbSmartAspects.AddInParameter(command, "@BillConfirmation", DbType.String, support.BillConfirmation);

                                    if (support.BillConfirmation == "YES")
                                        dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, "Pending");
                                    else
                                        dbSmartAspects.AddInParameter(command, "@BillStatus", DbType.String, "");

                                    dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, support.CreatedBy);

                                    Recentstatus = dbSmartAspects.ExecuteNonQuery(command, transction);


                                }
                            }

                            if (Recentstatus > 0 && SupportItemForSupportDetails.Count > 0)
                            {
                                foreach (STSupportDetailsBO stsd in SupportItemForSupportDetails)
                                {
                                    using (DbCommand cmdDetails = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSTSupportDetails_SP"))
                                    {
                                        cmdDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(cmdDetails, "@STSupportDetailsId", DbType.Int32, stsd.STSupportDetailsId);
                                        dbSmartAspects.AddInParameter(cmdDetails, "@ItemId", DbType.Int32, stsd.ItemId);

                                        dbSmartAspects.AddInParameter(cmdDetails, "@CategoryId", DbType.Int32, stsd.CategoryId);
                                        dbSmartAspects.AddInParameter(cmdDetails, "@StockBy", DbType.Int32, stsd.StockBy);
                                        dbSmartAspects.AddInParameter(cmdDetails, "@Type", DbType.String, stsd.Type);
                                        dbSmartAspects.AddInParameter(cmdDetails, "@STSupportId", DbType.Int32, id);

                                        if (stsd.UnitPrice > 0)
                                            dbSmartAspects.AddInParameter(cmdDetails, "@UnitPrice", DbType.Decimal, stsd.UnitPrice);
                                        else
                                            dbSmartAspects.AddInParameter(cmdDetails, "@UnitPrice", DbType.Decimal, DBNull.Value);

                                        if (stsd.UnitQuantity > 0)
                                            dbSmartAspects.AddInParameter(cmdDetails, "@UnitQuantity", DbType.Decimal, stsd.UnitQuantity);
                                        else
                                            dbSmartAspects.AddInParameter(cmdDetails, "@UnitQuantity", DbType.Decimal, DBNull.Value);

                                        if (stsd.VatRate > 0)
                                            dbSmartAspects.AddInParameter(cmdDetails, "@VatRate", DbType.Decimal, stsd.VatRate);
                                        else
                                            dbSmartAspects.AddInParameter(cmdDetails, "@VatRate", DbType.Decimal, DBNull.Value);

                                        if (stsd.VatAmount > 0)
                                            dbSmartAspects.AddInParameter(cmdDetails, "@VatAmount", DbType.Decimal, stsd.VatAmount);
                                        else
                                            dbSmartAspects.AddInParameter(cmdDetails, "@VatAmount", DbType.Decimal, DBNull.Value);

                                        if (stsd.TotalPrice > 0)
                                            dbSmartAspects.AddInParameter(cmdDetails, "@TotalPrice", DbType.Decimal, stsd.TotalPrice);
                                        else
                                            dbSmartAspects.AddInParameter(cmdDetails, "@TotalPrice", DbType.Decimal, DBNull.Value);

                                        Recentstatus = dbSmartAspects.ExecuteNonQuery(cmdDetails, transction);
                                    }
                                }
                            }

                            if (Recentstatus > 0 && SupportItemDeletedForSupportDetails.Count > 0)
                            {
                                using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteSTSupportDetails_SP"))
                                {
                                    foreach (STSupportDetailsBO stsd in SupportItemDeletedForSupportDetails)
                                    {
                                        commandDelete.Parameters.Clear();


                                        dbSmartAspects.AddInParameter(commandDelete, "@STSupportDetailsId", DbType.Int32, stsd.STSupportDetailsId);
                                        dbSmartAspects.AddInParameter(commandDelete, "@ItemId", DbType.Int32, stsd.ItemId);


                                        dbSmartAspects.AddInParameter(commandDelete, "@CategoryId", DbType.Int32, stsd.CategoryId);
                                        dbSmartAspects.AddInParameter(commandDelete, "@StockBy", DbType.Int32, stsd.StockBy);
                                        dbSmartAspects.AddInParameter(commandDelete, "@Type", DbType.String, stsd.Type);
                                        dbSmartAspects.AddInParameter(commandDelete, "@STSupportId", DbType.Int32, id);

                                        Recentstatus = dbSmartAspects.ExecuteNonQuery(commandDelete, transction);
                                    }
                                }
                            }
                        }

                        if (Recentstatus > 0)
                        {
                            status = true;
                            transction.Commit();
                        }
                        else
                        {
                            status = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return status;
        }
        public List<STSupportBO> GetSupportCallInformationForGridPaging(string caseStatus, string billStatus, string impStatus, int userInfoId, int empId, int clientId, int caseId, string caseNumber, DateTime? fromDate, DateTime? toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<STSupportBO> STSupportBOList = new List<STSupportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupportCallInformationForGridPaging_SP"))
                {

                    if (!string.IsNullOrEmpty(caseStatus))
                        dbSmartAspects.AddInParameter(cmd, "@SupportStatus", DbType.String, caseStatus);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupportStatus", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(billStatus))
                        dbSmartAspects.AddInParameter(cmd, "@BillStatus", DbType.String, billStatus);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@BillStatus", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(impStatus))
                        dbSmartAspects.AddInParameter(cmd, "@ImpStatus", DbType.String, impStatus);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ImpStatus", DbType.String, DBNull.Value);

                    if (userInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, DBNull.Value);

                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (clientId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ClientId", DbType.Int32, clientId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ClientId", DbType.Int32, DBNull.Value);

                    if (caseId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CaseId", DbType.Int32, caseId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CaseId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(caseNumber))
                        dbSmartAspects.AddInParameter(cmd, "@CaseNumber", DbType.String, caseNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CaseNumber", DbType.String, DBNull.Value);

                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet SupportDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SupportDS, "Support");
                    DataTable Table = SupportDS.Tables["Support"];

                    STSupportBOList = Table.AsEnumerable().Select(r => new STSupportBO
                    {
                        Id = r.Field<Int64>("Id"),
                        CaseNumber = r.Field<string>("CaseNumber"),
                        CaseOwnerId = r.Field<int>("CaseOwnerId"),
                        ClientId = r.Field<int>("ClientId"),
                        CompanyName = r.Field<string>("CompanyName"),
                        SupportCategoryId = r.Field<int>("SupportCategoryId"),
                        SupportSource = r.Field<string>("SupportSource"),
                        SupportSourceOtherDetails = r.Field<string>("SupportSourceOtherDetails"),
                        CaseId = r.Field<int>("CaseId"),
                        CaseName = r.Field<string>("CaseName"),
                        CaseDetails = r.Field<string>("CaseDetails"),
                        ItemOrServiceDetails = r.Field<string>("ItemOrServiceDetails"),
                        CreatedBy = r.Field<int>("CreatedBy"),
                        CreatedByName = r.Field<string>("CreatedByName"),
                        CreatedDate = r.Field<DateTime>("CreatedDate"),
                        SerialNumber = r.Field<Int64>("SerialNumber"),
                        SupportStageId = r.Field<int>("SupportStageId"),
                        TaskStatus = r.Field<string>("TaskStatus"),
                        BillStatus = r.Field<string>("BillStatus"),
                        SupportStatus = r.Field<string>("SupportStatus"),
                        PassDay = r.Field<int>("PassDay"),
                        TaskId = r.Field<long>("TaskId")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return STSupportBOList;
        }
        public List<STSupportBO> GetSupportCallBillingInformationForGridPaging(int userInfoId, int empId, int clientId, int caseId, string caseNumber, DateTime? fromDate, DateTime? toDate, string billingStatus, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<STSupportBO> STSupportBOList = new List<STSupportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupportCallBillingInformationForGridPaging_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (userInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, DBNull.Value);

                    if (empId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, DBNull.Value);

                    if (clientId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ClientId", DbType.Int32, clientId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ClientId", DbType.Int32, DBNull.Value);

                    if (caseId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CaseId", DbType.Int32, caseId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CaseId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(caseNumber))
                        dbSmartAspects.AddInParameter(cmd, "@CaseNumber", DbType.String, caseNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CaseNumber", DbType.String, DBNull.Value);

                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    if (billingStatus != null && billingStatus != "" && billingStatus != "0")
                        dbSmartAspects.AddInParameter(cmd, "@BillingStatus", DbType.String, billingStatus);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@BillingStatus", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet SupportDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SupportDS, "Support");
                    DataTable Table = SupportDS.Tables["Support"];

                    STSupportBOList = Table.AsEnumerable().Select(r => new STSupportBO
                    {
                        Id = r.Field<Int64>("Id"),
                        CaseNumber = r.Field<string>("CaseNumber"),
                        CaseOwnerId = r.Field<int>("CaseOwnerId"),
                        ClientId = r.Field<int>("ClientId"),
                        CompanyName = r.Field<string>("CompanyName"),
                        SupportCategoryId = r.Field<int>("SupportCategoryId"),
                        SupportSource = r.Field<string>("SupportSource"),
                        SupportSourceOtherDetails = r.Field<string>("SupportSourceOtherDetails"),
                        CaseId = r.Field<int>("CaseId"),
                        CaseDetails = r.Field<string>("CaseDetails"),
                        ItemOrServiceDetails = r.Field<string>("ItemOrServiceDetails"),
                        CreatedBy = r.Field<int>("CreatedBy"),
                        CreatedDate = r.Field<DateTime>("CreatedDate"),
                        SerialNumber = r.Field<Int64>("SerialNumber"),
                        SupportStageId = r.Field<int>("SupportStageId"),
                        CaseName = r.Field<string>("CaseName"),
                        TaskStatus = r.Field<string>("TaskStatus"),
                        BillStatus = r.Field<string>("BillStatus"),
                        SupportStatus = r.Field<string>("SupportStatus"),
                        PassDay = r.Field<int>("PassDay")
                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }

            return STSupportBOList;
        }
        public STSupportBO GetSupportCallInformationById(long id)
        {
            STSupportBO BO = new STSupportBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSTSupportById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BO.Id = Convert.ToInt64(reader["Id"]);
                                BO.CaseNumber = (reader["CaseNumber"]).ToString();
                                BO.CaseOwnerId = Convert.ToInt32(reader["CaseOwnerId"]);
                                BO.ClientId = Convert.ToInt32(reader["ClientId"]);
                                BO.SupportCategoryId = Convert.ToInt32(reader["SupportCategoryId"]);
                                BO.SupportSource = (reader["SupportSource"]).ToString();
                                BO.SupportSourceOtherDetails = (reader["SupportSourceOtherDetails"]).ToString();
                                BO.CaseId = Convert.ToInt32(reader["CaseId"]);
                                BO.CaseDetails = (reader["CaseDetails"]).ToString();
                                BO.ItemOrServiceDetails = (reader["ItemOrServiceDetails"]).ToString();
                                BO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                BO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                BO.SupportStageId = Convert.ToInt32(reader["SupportStageId"]);
                                BO.CompanyNameWithCode = (reader["CompanyNameWithCode"]).ToString();
                                BO.InternalNotesDetails = (reader["InternalNotesDetails"]).ToString();
                                BO.BillConfirmation = (reader["BillConfirmation"]).ToString();
                                BO.BillStatus = (reader["BillStatus"]).ToString();

                                if (string.IsNullOrEmpty(BO.BillStatus))
                                    BO.BillStatus = "Pending";

                                if (reader["SupportTypeId"] != DBNull.Value)
                                    BO.SupportTypeId = Convert.ToInt32(reader["SupportTypeId"]);
                                if (reader["SupportPriorityId"] != DBNull.Value)
                                    BO.SupportPriorityId = Convert.ToInt32(reader["SupportPriorityId"]);
                                if (reader["SupportForwardToId"] != DBNull.Value)
                                    BO.SupportForwardToId = Convert.ToInt32(reader["SupportForwardToId"]);
                                if (reader["SupportDeadline"] != DBNull.Value)
                                    BO.SupportDeadline = Convert.ToDateTime(reader["SupportDeadline"]);

                                List<STSupportDetailsBO> itemDetailsList = new List<STSupportDetailsBO>();

                                using (DbCommand cmd2 = dbSmartAspects.GetStoredProcCommand("GetSTSupportItemSpecifications_SP"))
                                {
                                    dbSmartAspects.AddInParameter(cmd2, "@STSupportId", DbType.Int32, BO.Id);

                                    using (IDataReader reader2 = dbSmartAspects.ExecuteReader(cmd2))
                                    {
                                        if (reader2 != null)
                                        {
                                            while (reader2.Read())
                                            {
                                                STSupportDetailsBO itemDetails = new STSupportDetailsBO();
                                                itemDetails.STSupportDetailsId = Convert.ToInt32(reader2["STSupportDetailsId"]);
                                                itemDetails.ItemId = Convert.ToInt32(reader2["ItemId"]);
                                                itemDetails.CategoryId = Convert.ToInt32(reader2["CategoryId"]);
                                                itemDetails.StockBy = Convert.ToInt32(reader2["StockBy"]);
                                                itemDetails.Type = (reader2["Type"]).ToString();
                                                itemDetails.HeadName = (reader2["HeadName"]).ToString();
                                                itemDetails.ItemName = (reader2["ItemName"]).ToString();
                                                itemDetails.STSupportId = Convert.ToInt32(reader2["STSupportId"]);

                                                if (reader2["UnitPrice"] != DBNull.Value)
                                                    itemDetails.UnitPrice = Convert.ToDecimal(reader2["UnitPrice"]);
                                                else
                                                    itemDetails.UnitPrice = 0;

                                                if (reader2["UnitQuantity"] != DBNull.Value)
                                                    itemDetails.UnitQuantity = Convert.ToDecimal(reader2["UnitQuantity"]);
                                                else
                                                    itemDetails.UnitQuantity = 1;

                                                if (reader2["VatRate"] != DBNull.Value)
                                                    itemDetails.VatRate = Convert.ToDecimal(reader2["VatRate"]);
                                                else
                                                    itemDetails.VatRate = 0;

                                                if (reader2["VatAmount"] != DBNull.Value)
                                                    itemDetails.VatAmount = Convert.ToDecimal(reader2["VatAmount"]);
                                                else
                                                    itemDetails.VatAmount = 0;

                                                if (reader2["TotalPrice"] != DBNull.Value)
                                                    itemDetails.TotalPrice = Convert.ToDecimal(reader2["TotalPrice"]);
                                                else
                                                    itemDetails.TotalPrice = 0;

                                                itemDetailsList.Add(itemDetails);
                                            }
                                        }
                                    }
                                }

                                BO.STSupportDetails = itemDetailsList;

                            }
                        }
                    }
                }

            }
            return BO;
        }
        public STSupportBO GetSupportFeedbackById(long id)
        {
            STSupportBO BO = new STSupportBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSTSupportById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BO.Id = Convert.ToInt64(reader["Id"]);

                                if (reader["FeedbackDate"] != DBNull.Value)
                                    BO.FeedbackDate = Convert.ToDateTime(reader["FeedbackDate"]);

                                BO.Feedback = (reader["Feedback"]).ToString();
                                BO.SupportStatus = (reader["SupportStatus"]).ToString();


                            }
                        }
                    }
                }

            }
            return BO;
        }
        public List<STSupportBO> LoadComapnyCallDetails(long companyId, long totalDetails)
        {
            List<STSupportBO> BOList = new List<STSupportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("LoadComapnyCallDetails_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@companyId", DbType.Int64, companyId);
                    dbSmartAspects.AddInParameter(cmd, "@totalDetails", DbType.Int64, totalDetails);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                STSupportBO BO = new STSupportBO();
                                BO.Id = Convert.ToInt64(reader["Id"]);
                                BO.CaseNumber = (reader["CaseNumber"]).ToString();
                                BO.CaseOwnerId = Convert.ToInt32(reader["CaseOwnerId"]);
                                BO.ClientId = Convert.ToInt32(reader["ClientId"]);
                                BO.SupportCategoryId = Convert.ToInt32(reader["SupportCategoryId"]);
                                BO.SupportSource = (reader["SupportSource"]).ToString();
                                BO.SupportSourceOtherDetails = (reader["SupportSourceOtherDetails"]).ToString();
                                BO.CaseId = Convert.ToInt32(reader["CaseId"]);
                                BO.CaseDetails = (reader["CaseDetails"]).ToString();
                                BO.ItemOrServiceDetails = (reader["ItemOrServiceDetails"]).ToString();
                                BO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                BO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                //BO.SerialNumber = Convert.ToInt64(reader["SerialNumber"]);
                                BO.SupportStageId = Convert.ToInt32(reader["SupportStageId"]);
                                BO.CompanyNameWithCode = (reader["CompanyNameWithCode"]).ToString();
                                BO.CaseName = (reader["CaseName"]).ToString();
                                BO.LastModifiedByName = (reader["LastModifiedByName"]).ToString();
                                BO.LastModifiedDateDisplay = (reader["LastModifiedDateDisplay"]).ToString();
                                BO.AssignedTo = (reader["AssignedTo"]).ToString();
                                BO.CaseCloseByName = (reader["CaseCloseByName"]).ToString();
                                BO.CaseCloseDateDisplay = (reader["CaseCloseDateDisplay"]).ToString();
                                BOList.Add(BO);


                            }
                        }
                    }
                }

            }
            return BOList;
        }
        public bool DeleteSupportCallInformation(long id, int lastModifiedBy)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteSTSupportWithDetails_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, id);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        public List<STSupportNCaseSetupInfoBO> GetSupportNCaseSetupBySearchCriteria(string name, string setupType, bool? status, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<STSupportNCaseSetupInfoBO> SupportNCaseList = new List<STSupportNCaseSetupInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupportNCaseSetupInfoForPaging_SP"))
                {
                    if (!string.IsNullOrEmpty(name))
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(setupType))
                        dbSmartAspects.AddInParameter(cmd, "@SetupType", DbType.String, setupType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SetupType", DbType.String, DBNull.Value);

                    if (status != null)
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.Boolean, status);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.Boolean, DBNull.Value);



                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                STSupportNCaseSetupInfoBO SupportNCase = new STSupportNCaseSetupInfoBO();
                                SupportNCase.Id = Convert.ToInt64(reader["Id"]);
                                SupportNCase.Name = reader["Name"].ToString();
                                SupportNCase.Description = reader["Description"].ToString();
                                SupportNCase.SetupType = reader["SetupType"].ToString();
                                SupportNCase.SetupTypeDisplay = reader["SetupTypeDisplay"].ToString();
                                SupportNCase.Status = Convert.ToBoolean(reader["Status"]);
                                SupportNCase.PriorityLabel = Convert.ToInt32(reader["PriorityLabel"]);
                                SupportNCaseList.Add(SupportNCase);
                            }
                        }
                    }

                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }


            return SupportNCaseList;
        }
        public List<STSupportNCaseSetupInfoBO> SupportNCaseSetupInfoAutoSearchBysearchTermAndSetupType(string searchText, string setupType)
        {
            List<STSupportNCaseSetupInfoBO> result = new List<STSupportNCaseSetupInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SupportNCaseSetupInfoAutoSearchBysearchTermAndSetupType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchText", DbType.String, searchText);
                    dbSmartAspects.AddInParameter(cmd, "@SetupType", DbType.String, setupType);

                    DataSet company = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, company, "Company");
                    DataTable Table = company.Tables["Company"];

                    result = Table.AsEnumerable().Select(r => new STSupportNCaseSetupInfoBO
                    {
                        Id = r.Field<Int64>("Id"),
                        Name = r.Field<string>("Name"),
                        Description = r.Field<string>("Description"),
                        SetupType = r.Field<string>("SetupType"),
                        Status = r.Field<bool>("Status"),
                        PriorityLabel = r.Field<int>("PriorityLabel")

                    }).ToList();
                }
            }
            return result;
        }
        public List<STSupportNCaseSetupInfoBO> SupportNCaseSetupInfoBySetupType(string setupType)
        {
            List<STSupportNCaseSetupInfoBO> result = new List<STSupportNCaseSetupInfoBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SupportNCaseSetupInfoBySetupType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SetupType", DbType.String, setupType);

                    DataSet company = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, company, "Company");
                    DataTable Table = company.Tables["Company"];

                    result = Table.AsEnumerable().Select(r => new STSupportNCaseSetupInfoBO
                    {
                        Id = r.Field<Int64>("Id"),
                        Name = r.Field<string>("Name"),
                        Description = r.Field<string>("Description"),
                        SetupType = r.Field<string>("SetupType"),
                        Status = r.Field<bool>("Status"),
                        PriorityLabel = r.Field<int>("PriorityLabel")

                    }).ToList();
                }
            }
            return result;
        }
        public STSupportNCaseSetupInfoBO GetSupportNCaseSetupById(long id)
        {
            STSupportNCaseSetupInfoBO support = new STSupportNCaseSetupInfoBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupportNCaseSetupById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, id);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                support.Id = Convert.ToInt64(reader["Id"]);
                                support.PriorityLabel = Convert.ToInt32(reader["PriorityLabel"]);
                                support.SetupType = (reader["SetupType"].ToString());
                                support.Name = reader["Name"].ToString();
                                support.IsCloseStage = Convert.ToBoolean(reader["IsCloseStage"]);
                                support.IsDeclineStage = Convert.ToBoolean(reader["IsDeclineStage"]);
                                support.Status = Convert.ToBoolean(reader["Status"]);
                                support.Description = reader["Description"].ToString();
                            }
                        }
                    }
                }
            }
            return support;
        }
        public Boolean SaveOrUpdateSupportPriceMatrixSetupInfo(List<STSupportPriceMatrixSetupBO> supportPriceMatrixList)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateSupportPriceMatrixSetup_SP"))
                    {
                        foreach (STSupportPriceMatrixSetupBO supportPriceMatrix in supportPriceMatrixList)
                        {
                            command.Parameters.Clear();
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, supportPriceMatrix.Id);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, supportPriceMatrix.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, supportPriceMatrix.ItemId);
                            dbSmartAspects.AddInParameter(command, "@Price", DbType.Decimal, supportPriceMatrix.Price);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public List<STSupportPriceMatrixSetupBO> GetPriceMatrixSetupBySearchCriteria(string company, string item, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<STSupportPriceMatrixSetupBO> PriceMatrixList = new List<STSupportPriceMatrixSetupBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupportPriceMatrixSetupForPaging_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (!string.IsNullOrEmpty(company))
                        dbSmartAspects.AddInParameter(cmd, "@Company", DbType.String, company);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Company", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(item))
                        dbSmartAspects.AddInParameter(cmd, "@Item", DbType.String, item);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Item", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                STSupportPriceMatrixSetupBO PriceMatrix = new STSupportPriceMatrixSetupBO();
                                PriceMatrix.Id = Convert.ToInt64(reader["Id"]);
                                PriceMatrix.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                PriceMatrix.Company = reader["Company"].ToString();
                                PriceMatrix.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                PriceMatrix.Category = reader["Category"].ToString();
                                PriceMatrix.ItemId = Convert.ToInt32(reader["ItemId"]);
                                PriceMatrix.Item = reader["Item"].ToString();
                                PriceMatrix.UnitHead = reader["UnitHead"].ToString();
                                PriceMatrix.Price = Convert.ToDecimal(reader["Price"]);
                                PriceMatrixList.Add(PriceMatrix);
                            }
                        }
                    }

                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }
            return PriceMatrixList;
        }
        public STSupportPriceMatrixSetupBO GetPriceMatrixSetupById(long id)
        {
            STSupportPriceMatrixSetupBO PriceMatrix = new STSupportPriceMatrixSetupBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPriceMatrixSetupById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, id);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PriceMatrix.Id = Convert.ToInt64(reader["Id"]);
                                PriceMatrix.ItemId = Convert.ToInt32(reader["ItemId"]);
                                PriceMatrix.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                PriceMatrix.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                PriceMatrix.Price = Convert.ToDecimal(reader["Price"]);
                                PriceMatrix.Item = (reader["Item"].ToString());
                                PriceMatrix.Company = reader["Company"].ToString();
                                PriceMatrix.Category = reader["Category"].ToString();
                                PriceMatrix.UnitHead = reader["UnitHead"].ToString();
                                PriceMatrix.Remarks = reader["Remarks"].ToString();
                            }
                        }
                    }
                }
            }
            return PriceMatrix;
        }
        public List<STSupportPriceMatrixSetupBO> GetPriceMatrixSetupForReport(int companyId, int categoryId, int itemId)
        {

            List<STSupportPriceMatrixSetupBO> PriceMatrixList = new List<STSupportPriceMatrixSetupBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupportPriceMatrixSetupForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    if (categoryId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (itemId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);


                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                STSupportPriceMatrixSetupBO PriceMatrix = new STSupportPriceMatrixSetupBO();
                                PriceMatrix.Id = Convert.ToInt64(reader["Id"]);
                                PriceMatrix.Company = reader["Company"].ToString();
                                PriceMatrix.Category = reader["Category"].ToString();
                                PriceMatrix.Item = reader["Item"].ToString();
                                PriceMatrix.UnitHead = reader["UnitHead"].ToString();
                                PriceMatrix.Price = Convert.ToDecimal(reader["Price"]);
                                PriceMatrixList.Add(PriceMatrix);
                            }
                        }
                    }
                }
            }
            return PriceMatrixList;
        }
        public List<STSupportBO> GetSupportTicket(long supportTicketId)
        {
            List<STSupportBO> ticketList = new List<STSupportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupportTicket_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SupportTicketId", DbType.Int64, supportTicketId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SupportTicket");
                    DataTable Table = ds.Tables["SupportTicket"];

                    ticketList = Table.AsEnumerable().Select(r => new STSupportBO
                    {
                        CaseNumber = r.Field<string>("CaseNumber"),
                        CaseName = r.Field<string>("CaseName"),
                        CaseDetails = r.Field<string>("CaseDetails"),
                        SupportCategory = r.Field<string>("SupportCategory"),
                        SupportType = r.Field<string>("SupportType"),
                        SupportStatus = r.Field<string>("SupportStatus"),
                        FeedbackStatus = r.Field<string>("FeedbackStatus"),
                        FeedbackDetails = r.Field<string>("FeedbackDetails"),
                        BillStatus = r.Field<string>("BillStatus"),
                        SupportPriority = r.Field<string>("SupportPriority"),
                        SupportStage = r.Field<string>("SupportStage"),
                        Department = r.Field<string>("Department"),
                        CreatedDateDisplay = r.Field<string>("CreatedDateDisplay"),
                        SupportDeadlineDisplay = r.Field<string>("SupportDeadlineDisplay"),
                        ClientDetails = r.Field<string>("ClientDetails"),
                        SupportSource = r.Field<string>("SupportSource"),
                        CaseOwnerName = r.Field<string>("CaseOwnerName"),
                        CreatedByName = r.Field<string>("CreatedByName"),
                        LastModifiedDateDisplay = r.Field<string>("LastModifiedDateDisplay"),
                        LastModifiedByName = r.Field<string>("LastModifiedByName"),
                        AssignedTo = r.Field<string>("AssignedTo"),
                        CaseCloseByName = r.Field<string>("CaseCloseByName"),
                        CaseCloseDateDisplay = r.Field<string>("CaseCloseDateDisplay"),
                    }).ToList();
                }
            }

            return ticketList;
        }
        public List<STCaseDetailHistoryBO> GetSupportCaseInternalNotesDetailsHistoryById(long supportTicketId)
        {
            List<STCaseDetailHistoryBO> ticketList = new List<STCaseDetailHistoryBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupportCaseInternalNotesDetailsHistoryById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SupportTicketId", DbType.Int64, supportTicketId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "SupportTicket");
                    DataTable Table = ds.Tables["SupportTicket"];

                    ticketList = Table.AsEnumerable().Select(r => new STCaseDetailHistoryBO
                    {
                        Id = r.Field<long>("Id"),
                        ShortInternalNotesDetails = r.Field<string>("ShortInternalNotesDetails"),
                        InternalNotesDetails = r.Field<string>("InternalNotesDetails"),
                        LogNumber = r.Field<int>("LogNumber")
                    }).ToList();
                }
            }

            return ticketList;
        }

        public STCaseDetailHistoryBO GetSupportCaseInternalNotesDetailsInformationById(long id)
        {
            STCaseDetailHistoryBO BO = new STCaseDetailHistoryBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupportCaseInternalNotesDetailsInformationById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                BO.Id = Convert.ToInt64(reader["Id"]);
                                BO.InternalNotesDetails = (reader["InternalNotesDetails"]).ToString();
                            }
                        }
                    }
                }

            }
            return BO;
        }
    }
}
