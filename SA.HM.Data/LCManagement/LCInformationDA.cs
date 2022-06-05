using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.LCManagement;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.HotelManagement;
using HotelManagement.Data.HMCommon;

namespace HotelManagement.Data.LCManagement
{
    public class LCInformationDA : BaseService
    {
        public bool SaveLCInformation(LCInformationBO lcBO, List<LCInformationDetailBO> detailBO, List<LCPaymentBO> paymentBO, out int tmpOrderId)
        {
            bool retVal = false;
            tmpOrderId = -1;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveLCInformationInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@LCNumber", DbType.String, lcBO.LCNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@LCOpenDate", DbType.String, lcBO.LCOpenDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@LCMatureDate  ", DbType.DateTime, lcBO.LCMatureDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@SupplierId", DbType.Int32, lcBO.SupplierId);
                            //dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, orderBO.Remarks);                            
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedBy", DbType.Int32, lcBO.CheckedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedBy", DbType.Int32, lcBO.ApprovedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, lcBO.CreatedBy);

                            dbSmartAspects.AddOutParameter(commandMaster, "@LCId", DbType.Int64, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            tmpOrderId = Convert.ToInt32(commandMaster.Parameters["@LCId"].Value);

                            HMCommonDA hmCommonDA = new HMCommonDA();
                            Boolean uploadStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(tmpOrderId, lcBO.RandomProductId);
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveLCInformationDetailInfo_SP"))
                            {
                                foreach (LCInformationDetailBO orderDetailBO in detailBO)
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@LCId", DbType.Int64, tmpOrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, orderDetailBO.POrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CostCenterId", DbType.Int32, orderDetailBO.CostCenterId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@StockById", DbType.Int32, orderDetailBO.StockById);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ProductId", DbType.Int32, orderDetailBO.ProductId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Quantity", DbType.Decimal, orderDetailBO.Quantity);
                                    dbSmartAspects.AddInParameter(commandDetails, "@PurchasePrice", DbType.Decimal, orderDetailBO.PurchasePrice);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveLCPaymentInfo_SP"))
                            {
                                foreach (LCPaymentBO payBO in paymentBO)
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@LCId", DbType.Int64, tmpOrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@AccHeadId", DbType.Int32, payBO.AccountHeadId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CurrencyId", DbType.Int32, payBO.CurrencyId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Amount", DbType.Decimal, payBO.Amount);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
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
        public bool ApprovalStatusUpdate(long id, string ApprovalStatusUpdate, int checkedOrApprovedBy)
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
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("ApprovalStatusUpdateForLC_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@Id", DbType.Int32, id);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApproveStatus", DbType.String, ApprovalStatusUpdate);
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
        public bool DeleteLCInformationById(long id)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteLCInformationMasterWithDetails_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, id);

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
        public List<LCPaymentBO> GetLCTotalPaymentById(long lcId)
        {
            List<LCPaymentBO> paymentList = new List<LCPaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCTotalPaymentById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LCId", DbType.Int64, lcId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "LCPaymentInfo");
                    DataTable Table = ds.Tables["LCPaymentInfo"];

                    paymentList = Table.AsEnumerable().Select(r => new LCPaymentBO
                    {
                        PaymentId = r.Field<long>("PaymentId"),
                        LCId = r.Field<long>("LCId"),
                        AccountHeadId = r.Field<int>("AccountHeadId"),
                        CurrencyId = r.Field<int>("CurrencyId"),
                        Amount = r.Field<decimal>("Amount"),
                        ConvertionRate = r.Field<decimal>("ConvertionRate"),
                        LocalCurrencyAmount = r.Field<decimal>("LocalCurrencyAmount"),
                        CurrencyName = r.Field<string>("CurrencyName"),
                        AccountHeadName = r.Field<string>("AccountHeadName"),

                    }).ToList();
                }
            }
            return paymentList;
        }
        public List<LCPaymentBO> GetLCBankClearenceById(long lcId)
        {
            List<LCPaymentBO> paymentList = new List<LCPaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCBankClearenceById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LCId", DbType.Int64, lcId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "LCPaymentInfo");
                    DataTable Table = ds.Tables["LCPaymentInfo"];

                    paymentList = Table.AsEnumerable().Select(r => new LCPaymentBO
                    {
                        PaymentId = r.Field<long>("PaymentId"),
                        LCId = r.Field<long>("LCId"),
                        AccountHeadId = r.Field<int>("AccountHeadId"),
                        CurrencyId = r.Field<int>("CurrencyId"),
                        Amount = r.Field<decimal>("Amount"),
                        AccountHeadName = r.Field<string>("AccountHeadName"),

                    }).ToList();
                }
            }
            return paymentList;
        }
        public List<LCReportViewBO> GetLCDetailsReportInfo(string reportType, string lCNumber)
        {
            List<LCReportViewBO> requisitionList = new List<LCReportViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCDetailsReportInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReportType", DbType.String, reportType);
                    dbSmartAspects.AddInParameter(cmd, "@LCNumber", DbType.String, lCNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                LCReportViewBO requisitionBO = new LCReportViewBO();
                                if (reportType == "LCInformation")
                                {
                                    requisitionBO.LCId = Convert.ToInt32(reader["LCId"]);
                                    requisitionBO.LCNumber = reader["LCNumber"].ToString();
                                    requisitionBO.PINumber = reader["PINumber"].ToString();
                                    requisitionBO.LCType = reader["LCType"].ToString();
                                    requisitionBO.BankAccount = reader["BankAccount"].ToString();
                                    requisitionBO.LCValue = reader["LCValue"].ToString();
                                    requisitionBO.LCOpenDateString = reader["LCOpenDateString"].ToString();
                                    requisitionBO.LCMatureDateString = reader["LCMatureDateString"].ToString();
                                    requisitionBO.SupplierInfo = reader["SupplierInfo"].ToString();
                                    requisitionBO.Remarks = reader["Remarks"].ToString();
                                    requisitionBO.UserName = reader["UserName"].ToString();
                                    requisitionBO.CreatedDateString = reader["CreatedDateString"].ToString();
                                    requisitionBO.SettlementByName = reader["SettlementByName"].ToString();
                                    requisitionBO.SettlementDateString = reader["SettlementDateString"].ToString();
                                    requisitionBO.SettlementDescription = reader["SettlementDescription"].ToString();
                                }
                                else if (reportType == "LCInformationDetail")
                                {
                                    requisitionBO.Code = reader["Code"].ToString();
                                    requisitionBO.Name = reader["Name"].ToString();
                                    requisitionBO.StockUnit = reader["StockUnit"].ToString();

                                    requisitionBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                    requisitionBO.Quantity = Convert.ToInt32(reader["Quantity"]);
                                    requisitionBO.Total = Convert.ToInt32(reader["Total"]);

                                }
                                else if (reportType == "LCPayment")
                                {
                                    requisitionBO.Code = reader["Code"].ToString();
                                    requisitionBO.Name = reader["Name"].ToString();
                                    requisitionBO.Amount = Convert.ToDecimal(reader["Amount"]);
                                }
                                else if (reportType == "LCOverHeadExpense")
                                {
                                    requisitionBO.ExpenseDate = Convert.ToDateTime(reader["ExpenseDate"]);
                                    requisitionBO.ExpenseDateString = reader["ExpenseDateString"].ToString();
                                    requisitionBO.TransactionType = reader["TransactionType"].ToString();
                                    requisitionBO.Name = reader["Name"].ToString();
                                    requisitionBO.ReceiveAmount = Convert.ToDecimal(reader["ReceiveAmount"]);
                                    requisitionBO.PaymentAmount = Convert.ToDecimal(reader["PaymentAmount"]);
                                    requisitionBO.PaymentBy = reader["PaymentBy"].ToString();
                                    requisitionBO.Description = reader["Description"].ToString();
                                }
                                else if (reportType == "PMProductReceived")
                                {
                                    requisitionBO.ReceivedDate = Convert.ToDateTime(reader["ReceivedDate"]);
                                    requisitionBO.ReceivedDateString = reader["ReceivedDateString"].ToString();
                                    requisitionBO.Code = reader["Code"].ToString();
                                    requisitionBO.Name = reader["Name"].ToString();
                                    requisitionBO.StockUnit = reader["StockUnit"].ToString();

                                    requisitionBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                                    requisitionBO.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                    requisitionBO.Total = Convert.ToDecimal(reader["Total"]);
                                }
                                else if (reportType == "LCPaymentLedger")
                                {
                                    requisitionBO.LCId = Convert.ToInt32(reader["LCId"]);
                                }
                                requisitionList.Add(requisitionBO);
                            }
                        }
                    }
                }
            }
            return requisitionList;
        }
        public bool SaveOrUpdateLCInformation(LCInformationViewBO LCInformationViewBOForAdded, LCInformationViewBO LCInformationViewBOForDeleted, out long id)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateLCInformation_SP"))
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, LCInformationViewBOForAdded.LCInformation.LCId);
                            dbSmartAspects.AddInParameter(command, "@LCNumber", DbType.String, LCInformationViewBOForAdded.LCInformation.LCNumber);
                            dbSmartAspects.AddInParameter(command, "@PINumber", DbType.String, LCInformationViewBOForAdded.LCInformation.PINumber);
                            dbSmartAspects.AddInParameter(command, "@LCValue", DbType.Decimal, LCInformationViewBOForAdded.LCInformation.LCValue);
                            dbSmartAspects.AddInParameter(command, "@LCOpenDate", DbType.DateTime, LCInformationViewBOForAdded.LCInformation.LCOpenDate);

                            if (LCInformationViewBOForAdded.LCInformation.LCMatureDate != null)
                            {
                                dbSmartAspects.AddInParameter(command, "@LCMatureDate  ", DbType.DateTime, LCInformationViewBOForAdded.LCInformation.LCMatureDate);
                            }
                            else
                            {
                                dbSmartAspects.AddInParameter(command, "@LCMatureDate", DbType.DateTime, DBNull.Value);
                            }
                            dbSmartAspects.AddInParameter(command, "@LCTypes", DbType.String, LCInformationViewBOForAdded.LCInformation.LCTypes);
                            dbSmartAspects.AddInParameter(command, "@Incoterms", DbType.String, LCInformationViewBOForAdded.LCInformation.Incoterms);
                            dbSmartAspects.AddInParameter(command, "@LCManageAccountId", DbType.Int32, LCInformationViewBOForAdded.LCInformation.LCManageAccountId);
                            dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, LCInformationViewBOForAdded.LCInformation.SupplierId);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, LCInformationViewBOForAdded.LCInformation.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int32, LCInformationViewBOForAdded.LCInformation.ProjectId);
                            dbSmartAspects.AddInParameter(command, "@POorderId", DbType.Int32, LCInformationViewBOForAdded.LCInformation.POorderId);
                            dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, LCInformationViewBOForAdded.LCInformation.ApprovedStatus);
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, LCInformationViewBOForAdded.LCInformation.CreatedBy);


                            dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));
                            Recentstatus = dbSmartAspects.ExecuteNonQuery(command, transction);
                            id = Convert.ToInt64(command.Parameters["@OutId"].Value);
                        }

                        if (Recentstatus > 0 && LCInformationViewBOForAdded.LCInformationDetail.Count > 0)
                        {
                            foreach (LCInformationDetailBO lcd in LCInformationViewBOForAdded.LCInformationDetail)
                            {
                                using (DbCommand cmdDetails = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateLCInformationDetail_SP"))
                                {
                                    cmdDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdDetails, "@LCDetailId", DbType.Int32, lcd.LCDetailId);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@LCId", DbType.Int32, id);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@POrderId", DbType.Int32, lcd.POrderId);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@CostCenterId", DbType.Int32, lcd.CostCenterId);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@StockById", DbType.Int32, lcd.StockById);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@StockBy", DbType.String, lcd.StockBy);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@ProductId", DbType.Int32, lcd.ProductId);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@PurchasePrice", DbType.Decimal, lcd.PurchasePrice);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@Quantity", DbType.Decimal, lcd.Quantity);

                                    Recentstatus = dbSmartAspects.ExecuteNonQuery(cmdDetails, transction);
                                }
                            }
                        }

                        if (Recentstatus > 0 && LCInformationViewBOForAdded.LCPayment.Count > 0)
                        {
                            foreach (LCPaymentBO lcd in LCInformationViewBOForAdded.LCPayment)
                            {
                                using (DbCommand cmdDetails = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateLCPayment_SP"))
                                {
                                    cmdDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdDetails, "@PaymentId", DbType.Int32, lcd.PaymentId);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@LCId", DbType.Int32, id);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@AccountHeadId", DbType.Int32, lcd.AccountHeadId);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@AccountHeadName", DbType.String, lcd.AccountHeadName);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@CurrencyId", DbType.Int32, lcd.CurrencyId);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@ConvertionRate", DbType.Decimal, lcd.ConvertionRate);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@Amount", DbType.Decimal, lcd.Amount);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@Remarks", DbType.String, lcd.Remarks);
                                    dbSmartAspects.AddInParameter(cmdDetails, "@PaymentDate", DbType.DateTime, lcd.PaymentDate);

                                    Recentstatus = dbSmartAspects.ExecuteNonQuery(cmdDetails, transction);
                                }
                            }
                        }
                        if (Recentstatus > 0 && LCInformationViewBOForDeleted.LCPayment.Count > 0)
                        {
                            using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteLCPaymentById_SP"))
                            {
                                foreach (LCPaymentBO payment in LCInformationViewBOForDeleted.LCPayment)
                                {
                                    commandDelete.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDelete, "@PaymentId", DbType.Int32, payment.PaymentId);

                                    Recentstatus = dbSmartAspects.ExecuteNonQuery(commandDelete, transction);
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

        public LCInformationViewBO GetLCInformationALlDetailsByLCNumber(int lcId)
        {
            LCInformationViewBO viewBO = new LCInformationViewBO();
            LCInformationBO lcBO = new LCInformationBO();
            List<LCInformationDetailBO> LCInformationDetailBOList = new List<LCInformationDetailBO>();
            List<LCPaymentBO> LCPaymentBOList = new List<LCPaymentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCInformationByLCId_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@lcId", DbType.Int32, lcId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "LCInfo");
                    DataTable Table = ds.Tables["LCInfo"];

                    lcBO = Table.AsEnumerable().Select(r => new LCInformationBO
                    {
                        LCId = r.Field<long>("LCId"),
                        LCNumber = r.Field<string>("LCNumber"),
                        PINumber = r.Field<string>("PINumber"),
                        LCTypes = r.Field<string>("LCType"),
                        LCManageAccountId = r.Field<int>("BankAccountId"),
                        LCOpenDate = r.Field<DateTime>("LCOpenDate"),
                        LCMatureDate = r.Field<DateTime?>("LCMatureDate"),
                        SupplierId = r.Field<int>("SupplierId"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        Incoterms = r.Field<string>("Incoterms"),
                        CompanyId = r.Field<int>("CompanyId"),
                        ProjectId = r.Field<int>("ProjectId"),
                        POorderId = r.Field<int>("PODId"),
                        LCValue = r.Field<decimal?>("LCValue")

                    }).FirstOrDefault();
                }
                viewBO.LCInformation = lcBO;

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCInformationDetailByLCId_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@lcId", DbType.Int32, lcId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "LCInfoDetails");
                    DataTable Table = ds.Tables["LCInfoDetails"];

                    LCInformationDetailBOList = Table.AsEnumerable().Select(r => new LCInformationDetailBO
                    {
                        LCDetailId = r.Field<Int64>("LCDetailId"),
                        LCId = r.Field<Int64>("LCId"),
                        POrderId = r.Field<int>("POrderId"),
                        CostCenterId = r.Field<int>("CostCenterId"),
                        StockById = r.Field<int>("StockById"),
                        ProductId = r.Field<int>("ProductId"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),
                        Quantity = r.Field<decimal>("Quantity"),
                        StockBy = r.Field<string>("StockBy"),
                        ItemName = r.Field<string>("ItemName"),

                    }).ToList();
                }
                viewBO.LCInformationDetail = LCInformationDetailBOList;

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCPaymentDetailByLCId_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@lcId", DbType.Int32, lcId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "LCPaymentDetails");
                    DataTable Table = ds.Tables["LCPaymentDetails"];

                    LCPaymentBOList = Table.AsEnumerable().Select(r => new LCPaymentBO
                    {
                        PaymentId = r.Field<Int64>("PaymentId"),
                        LCId = r.Field<Int64>("LCId"),
                        AccountHeadId = r.Field<int>("AccountHeadId"),
                        CurrencyId = r.Field<int>("CurrencyId"),
                        Amount = r.Field<decimal>("Amount"),
                        AccountHeadName = r.Field<string>("AccountHeadName"),
                        ConvertionRate = r.Field<decimal>("ConvertionRate"),
                        Remarks = r.Field<string>("Remarks"),
                        PaymentDate = r.Field<DateTime>("PaymentDate"),
                        CurrencyName = r.Field<string>("CurrencyName")

                    }).ToList();
                }
                viewBO.LCPayment = LCPaymentBOList;
            }
            return viewBO;
        }

        public List<LCInformationBO> GetLCInformationForGridPaging(int userInfoId, int CompanyId, int ProjectId, int supplierId, String LCNumber, String PINumber, DateTime? fromDate, DateTime? toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<LCInformationBO> LCInformationBOList = new List<LCInformationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCInformationForGridPaging_SP"))
                {
                    if (userInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, DBNull.Value);
                    
                    if (CompanyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, CompanyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);

                    if (ProjectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, ProjectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, DBNull.Value);

                    if (supplierId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(LCNumber))
                        dbSmartAspects.AddInParameter(cmd, "@LCNumber", DbType.String, LCNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LCNumber", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(PINumber))
                        dbSmartAspects.AddInParameter(cmd, "@PINumber", DbType.String, PINumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PINumber", DbType.String, DBNull.Value);

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

                    DataSet CashDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, CashDS, "Cash");
                    DataTable Table = CashDS.Tables["Cash"];

                    LCInformationBOList = Table.AsEnumerable().Select(r => new LCInformationBO
                    {
                        LCId = r.Field<Int64>("LCId"),
                        LCNumber = r.Field<string>("LCNumber"),
                        PINumber = r.Field<string>("PINumber"),
                        LCTypes = r.Field<string>("LCType"),
                        LCValue = r.Field<decimal>("LCValue"),
                        Incoterms = r.Field<string>("Incoterms"),
                        LCOpenDate = r.Field<DateTime>("LCOpenDate"),
                        LCMatureDate = r.Field<DateTime?>("LCMatureDate"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanDelete = r.Field<bool>("IsCanDelete"),
                        IsCanCheck = r.Field<bool>("IsCanCheck"),
                        IsCanApprove = r.Field<bool>("IsCanApprove")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return LCInformationBOList;
        }
        public bool UpdateLCInformation(LCInformationBO lcBO, List<LCInformationDetailBO> AddedLCDetails, List<LCInformationDetailBO> EditedLCDetails, List<LCInformationDetailBO> DeletedLCDetails, List<LCPaymentBO> AddedPayment, List<LCPaymentBO> EditedPayment, List<LCPaymentBO> DeletedPayment)
        {
            bool retVal = false;
            int status = 0;
            long tmplcId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateLCInformation_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@LCId", DbType.Int64, lcBO.LCId);
                            dbSmartAspects.AddInParameter(commandMaster, "@LCNumber", DbType.String, lcBO.LCNumber);
                            dbSmartAspects.AddInParameter(commandMaster, "@LCOpenDate", DbType.String, lcBO.LCOpenDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@LCMatureDate  ", DbType.DateTime, lcBO.LCMatureDate);
                            dbSmartAspects.AddInParameter(commandMaster, "@SupplierId", DbType.Int32, lcBO.SupplierId);
                            //dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, orderBO.Remarks);                            
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedBy", DbType.Int32, lcBO.CheckedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedBy", DbType.Int32, lcBO.ApprovedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, lcBO.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            tmplcId = lcBO.LCId;

                            HMCommonDA hmCommonDA = new HMCommonDA();
                            Boolean uploadStatus = hmCommonDA.UpdateUploadedDocumentsInformationByOwnerId(lcBO.LCId, lcBO.RandomProductId);
                        }

                        if (status > 0 && AddedLCDetails.Count() > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveLCInformationDetailInfo_SP"))
                            {
                                foreach (LCInformationDetailBO po in AddedLCDetails)
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@LCId", DbType.Int64, tmplcId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, po.POrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CostCenterId", DbType.Int32, po.CostCenterId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@StockById", DbType.Int32, po.StockById);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ProductId", DbType.Int32, po.ProductId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Quantity", DbType.Decimal, po.Quantity);
                                    dbSmartAspects.AddInParameter(commandDetails, "@PurchasePrice", DbType.Decimal, po.PurchasePrice);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && EditedLCDetails.Count() > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateLCInformationDetail_SP"))
                            {
                                foreach (LCInformationDetailBO po in EditedLCDetails)
                                {
                                    commandDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDetails, "@LCDetailId", DbType.Int32, po.LCDetailId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@LCId", DbType.Int64, tmplcId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@POrderId", DbType.Int32, po.POrderId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CostCenterId", DbType.Int32, po.CostCenterId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@StockById", DbType.Int32, po.StockById);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ProductId", DbType.Int32, po.ProductId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Quantity", DbType.Decimal, po.Quantity);
                                    dbSmartAspects.AddInParameter(commandDetails, "@PurchasePrice", DbType.Decimal, po.PurchasePrice);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedLCDetails.Count() > 0)
                        {
                            using (DbCommand commandDeletePO = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (LCInformationDetailBO po in DeletedLCDetails)
                                {
                                    commandDeletePO.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TableName", DbType.String, "LCInformationDetail");
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TablePKField", DbType.String, "LCDetailId");
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TablePKId", DbType.String, po.LCDetailId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePO);
                                }
                            }
                        }

                        if (status > 0 && AddedPayment.Count() > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveLCPaymentInfo_SP"))
                            {
                                foreach (LCPaymentBO bo in AddedPayment)
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@LCId", DbType.Int64, tmplcId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@AccHeadId", DbType.Int32, bo.AccountHeadId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CurrencyId", DbType.Int32, bo.CurrencyId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Amount", DbType.Decimal, bo.Amount);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && EditedPayment.Count() > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateLCPaymentInfo_SP"))
                            {
                                foreach (LCPaymentBO bo in EditedPayment)
                                {
                                    commandDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDetails, "@PaymentId", DbType.Int64, bo.PaymentId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@LCId", DbType.Int64, tmplcId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@AccHeadId", DbType.Int32, bo.AccountHeadId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@CurrencyId", DbType.Int32, bo.CurrencyId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Amount", DbType.Decimal, bo.Amount);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedPayment.Count() > 0)
                        {
                            using (DbCommand commandDeletePO = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (LCPaymentBO bo in DeletedPayment)
                                {
                                    commandDeletePO.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TableName", DbType.String, "LCPayment");
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TablePKField", DbType.String, "PaymentId");
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TablePKId", DbType.String, bo.PaymentId);
                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePO);
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
        public List<LCInformationBO> GetLCInformationBySearchCriteria(DateTime fromDate, DateTime toDate, string lcNumber, string status)
        {
            List<LCInformationBO> orderList = new List<LCInformationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCInformationBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    if (!string.IsNullOrEmpty(lcNumber))
                        dbSmartAspects.AddInParameter(cmd, "@LCNumber", DbType.String, lcNumber);
                    else dbSmartAspects.AddInParameter(cmd, "@LCNumber", DbType.String, DBNull.Value);
                    if (status != "All")
                        dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, status);
                    else dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, DBNull.Value);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                LCInformationBO orderBO = new LCInformationBO();
                                orderBO.LCId = Int32.Parse(reader["LCId"].ToString());
                                orderBO.LCNumber = reader["LCNumber"].ToString();
                                orderBO.LCOpenDate = Convert.ToDateTime(reader["LCOpenDate"].ToString());
                                orderBO.LCMatureDate = Convert.ToDateTime(reader["LCMatureDate"].ToString());
                                orderBO.Remarks = reader["Remarks"].ToString();
                                orderBO.SupplierId = Int32.Parse(reader["SupplierId"].ToString());
                                orderBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                                orderBO.CheckedBy = Int32.Parse(reader["CheckedBy"].ToString());
                                orderBO.ApprovedBy = Int32.Parse(reader["ApprovedBy"].ToString());
                                orderBO.ApprovedStatus = reader["ApprovedStatus"].ToString();

                                orderList.Add(orderBO);
                            }
                        }
                    }
                }
            }
            return orderList;
        }
        public LCInformationBO GetLCInformationByLCNumber(string lcNumber)
        {
            LCInformationBO lcBO = new LCInformationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCInformationByLCNumber_SP"))
                {
                    if (!string.IsNullOrEmpty(lcNumber))
                        dbSmartAspects.AddInParameter(cmd, "@LCNumber", DbType.String, lcNumber);
                    else dbSmartAspects.AddInParameter(cmd, "@LCNumber", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "LCInfo");
                    DataTable Table = ds.Tables["LCInfo"];

                    lcBO = Table.AsEnumerable().Select(r => new LCInformationBO
                    {
                        LCId = r.Field<long>("LCId"),
                        LCNumber = r.Field<string>("LCNumber"),
                        LCOpenDate = r.Field<DateTime>("LCOpenDate"),
                        LCMatureDate = r.Field<DateTime>("LCMatureDate"),
                        SupplierId = r.Field<int>("SupplierId"),
                        Supplier = r.Field<string>("Supplier"),
                        CheckedBy = r.Field<int>("CheckedBy"),
                        ApprovedBy = r.Field<int>("ApprovedBy")
                    }).FirstOrDefault();
                }
            }
            return lcBO;
        }


        public List<LCInformationBO> GetApprovedLCInformation()
        {
            List<LCInformationBO> lcList = new List<LCInformationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApprovedLCInformation_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "LCInfo");
                    DataTable Table = ds.Tables["LCInfo"];

                    lcList = Table.AsEnumerable().Select(r => new LCInformationBO
                    {
                        LCId = r.Field<long>("LCId"),
                        LCNumber = r.Field<string>("LCNumber"),
                        LCOpenDate = r.Field<DateTime>("LCOpenDate"),
                        LCMatureDate = r.Field<DateTime?>("LCMatureDate"),
                        SupplierId = r.Field<int>("SupplierId"),
                        CheckedBy = r.Field<int>("CheckedBy"),
                        ApprovedBy = r.Field<int>("ApprovedBy")

                    }).ToList();
                }
            }
            return lcList;
        }
        public List<LCInformationBO> GetApprovedLCInformationInfoForReceive(int userGroupId)
        {
            List<LCInformationBO> lcList = new List<LCInformationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApprovedLCInformationInfoForReceive_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UserGroupId", DbType.Int32, userGroupId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "LCInfo");
                    DataTable Table = ds.Tables["LCInfo"];

                    lcList = Table.AsEnumerable().Select(r => new LCInformationBO
                    {
                        LCId = r.Field<long>("LCId"),
                        LCNumber = r.Field<string>("LCNumber"),
                        LCOpenDate = r.Field<DateTime>("LCOpenDate"),
                        LCMatureDate = r.Field<DateTime?>("LCMatureDate"),
                        SupplierId = r.Field<int>("SupplierId"),
                        CostCenterId = r.Field<int>("CostCenterId")                        
                    }).ToList();
                }
            }
            return lcList;
        }
        public LCInformationBO GetLCInformationById(long lcId)
        {
            LCInformationBO lcBO = new LCInformationBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCInformationById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LCId", DbType.Int64, lcId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "LCInfo");
                    DataTable Table = ds.Tables["LCInfo"];

                    lcBO = Table.AsEnumerable().Select(r => new LCInformationBO
                    {
                        LCId = r.Field<long>("LCId"),
                        LCNumber = r.Field<string>("LCNumber"),
                        LCOpenDate = r.Field<DateTime>("LCOpenDate"),
                        LCMatureDate = r.Field<DateTime>("LCMatureDate"),
                        SupplierId = r.Field<int>("SupplierId"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        CheckedBy = r.Field<int>("CheckedBy"),
                        ApprovedBy = r.Field<int>("ApprovedBy")

                    }).SingleOrDefault();
                }
            }
            return lcBO;
        }
        public List<LCInformationDetailBO> GetLCDetailInformationById(long lcId)
        {
            List<LCInformationDetailBO> lcList = new List<LCInformationDetailBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCDetailInformationByLCId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LCId", DbType.Int64, lcId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "LCDetailInfo");
                    DataTable Table = ds.Tables["LCDetailInfo"];

                    lcList = Table.AsEnumerable().Select(r => new LCInformationDetailBO
                    {
                        LCDetailId = r.Field<long>("LCDetailId"),
                        LCId = r.Field<long>("LCId"),
                        POrderId = r.Field<int>("POrderId"),
                        CostCenterId = r.Field<int>("CostCenterId"),
                        StockById = r.Field<int>("StockById"),
                        StockBy = r.Field<string>("StockBy"),
                        ProductId = r.Field<int>("ProductId"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),
                        Quantity = r.Field<decimal>("Quantity"),
                        ItemName = r.Field<string>("ItemName"),
                        ItemTotal = r.Field<decimal>("ItemTotal")
                    }).ToList();
                }
            }
            return lcList;
        }
        public List<LCPaymentBO> GetLCPaymentById(long lcId)
        {
            List<LCPaymentBO> paymentList = new List<LCPaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCPaymentByLCId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LCId", DbType.Int64, lcId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "LCPaymentInfo");
                    DataTable Table = ds.Tables["LCPaymentInfo"];

                    paymentList = Table.AsEnumerable().Select(r => new LCPaymentBO
                    {
                        PaymentId = r.Field<long>("PaymentId"),
                        LCId = r.Field<long>("LCId"),
                        AccountHeadId = r.Field<int>("AccountHeadId"),
                        CurrencyId = r.Field<int>("CurrencyId"),
                        Amount = r.Field<decimal>("Amount"),
                        AccountHeadName = r.Field<string>("AccountHeadName"),

                    }).ToList();
                }
            }
            return paymentList;
        }
        public bool DeleteLCInformation(long lcId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteLCInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@LCId", DbType.Int64, lcId);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool LCSettlementInformation(LCInformationBO lcBO)
        {
            bool retVal = false;
            int status = 0;
            long tmplcId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("LCSettlementInformation_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@LCId", DbType.Int64, lcBO.LCId);
                            dbSmartAspects.AddInParameter(commandMaster, "@SettlementDescription", DbType.String, lcBO.SettlementDescription);
                            dbSmartAspects.AddInParameter(commandMaster, "@SettlementBy", DbType.Int32, lcBO.SettlementBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            tmplcId = lcBO.LCId;
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
        public bool LCBankSettlementInformation(LCInformationBO lcBO, List<GuestBillPaymentBO> guestPaymentDetailList)
        {
            bool retVal = false;
            int status = 0;
            long tmplcId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("LCBankSettlementInformation_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@LCId", DbType.Int64, lcBO.LCId);
                            dbSmartAspects.AddInParameter(commandMaster, "@BankSettlementBy", DbType.Int32, lcBO.BankSettlementBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            tmplcId = lcBO.LCId;
                        }

                        if (status > 0)
                        {
                            //transction.Commit();
                            //retVal = true;
                            int countGuestBillPaymentList = 0;

                            using (DbCommand commandGuestBillPayment = dbSmartAspects.GetStoredProcCommand("SaveLCPaymentLedgerInfo_SP"))
                            {
                                foreach (GuestBillPaymentBO guestBillPaymentBO in guestPaymentDetailList)
                                {
                                    commandGuestBillPayment.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentType", DbType.String, guestBillPaymentBO.PaymentType);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@BillNumber", DbType.String, guestBillPaymentBO.BillNumber);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentDate", DbType.DateTime, guestBillPaymentBO.PaymentDate);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@LCId", DbType.Int32, lcBO.LCId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@LCBankAccountHeadId", DbType.Int32, guestBillPaymentBO.LCBankAccountHeadId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@AccountHeadId", DbType.Int32, guestBillPaymentBO.AccountsPostingHeadId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyId", DbType.Int32, guestBillPaymentBO.FieldId);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@ConvertionRate", DbType.Decimal, guestBillPaymentBO.ConvertionRate);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@DRAmount", DbType.Decimal, guestBillPaymentBO.DRAmount);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CRAmount", DbType.Decimal, guestBillPaymentBO.CRAmount);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CurrencyAmount", DbType.Decimal, guestBillPaymentBO.CurrencyAmount);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@Remarks", DbType.String, guestBillPaymentBO.Remarks);
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@PaymentStatus", DbType.String, "Approved");
                                    dbSmartAspects.AddInParameter(commandGuestBillPayment, "@CreatedBy", DbType.Int32, guestBillPaymentBO.CreatedBy);
                                    dbSmartAspects.AddOutParameter(commandGuestBillPayment, "@LCPaymentId", DbType.Int64, sizeof(Int32));
                                    dbSmartAspects.ExecuteNonQuery(commandGuestBillPayment, transction);
                                    countGuestBillPaymentList = countGuestBillPaymentList + 1;
                                }
                            }

                            if (countGuestBillPaymentList == guestPaymentDetailList.Count)
                            {
                                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("LCBankSettlementVoucherPosting_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandMaster, "@LCId", DbType.Int64, lcBO.LCId);
                                    dbSmartAspects.AddInParameter(commandMaster, "@BankSettlementBy", DbType.Int32, lcBO.BankSettlementBy);

                                    status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                                }
                                if (status > 0)
                                {
                                    guestPaymentDetailList = null;
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
                                retVal = false;
                            }
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
        public List<LCReportViewBO> GetLCReportInfo(DateTime dateFrom, DateTime dateTo, int? lCId, string lCType, int? supplierId)
        {
            string Where = GenerateLCReportWhereCondition(dateFrom, dateTo, lCId, lCType, supplierId);
            List<LCReportViewBO> requisitionList = new List<LCReportViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCReportInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                LCReportViewBO requisitionBO = new LCReportViewBO();

                                requisitionBO.LCId = Convert.ToInt32(reader["LCId"]);
                                requisitionBO.Supplier = reader["Supplier"].ToString();
                                requisitionBO.LCOpenDate = reader["LCOpenDate"].ToString();
                                requisitionBO.LCMatureDate = reader["LCMatureDate"].ToString();
                                requisitionBO.LCSettlementDate = reader["LCSettlementDate"].ToString();
                                requisitionBO.LCNumber = reader["LCNumber"].ToString();
                                requisitionBO.ProductName = reader["ProductName"].ToString();
                                if (!string.IsNullOrWhiteSpace(reader["Quantity"].ToString()))
                                {
                                    requisitionBO.Quantity = Convert.ToInt32(reader["Quantity"]);
                                }
                                else
                                {
                                    requisitionBO.Quantity = null;
                                }
                                requisitionBO.Stock = reader["Stock"].ToString();
                                requisitionBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                requisitionList.Add(requisitionBO);
                            }
                        }
                    }
                }
            }
            return requisitionList;
        }
        public string GenerateLCReportWhereCondition(DateTime fromDate, DateTime toDate, int? LCId, string lCType, int? supplierId)
        {
            string Where = string.Empty;
            //guestName = guestName.Trim();
            //reserveNo = reserveNo.Trim();
            //contactPerson = contactPerson.Trim();

            if (lCType == "Open")
            {
                //Date
                if (fromDate != null)
                {
                    Where = "(dbo.FnDate(pr.LCOpenDate) BETWEEN dbo.FnDate('" + fromDate + "') AND dbo.FnDate('" + fromDate + "') )";
                }
                if (toDate != null)
                {
                    Where = "(dbo.FnDate(pr.LCOpenDate) BETWEEN dbo.FnDate('" + toDate + "') AND dbo.FnDate('" + toDate + "') )";
                }
                if (fromDate != null && toDate != null)
                {
                    Where = "(dbo.FnDate(pr.LCOpenDate) BETWEEN dbo.FnDate('" + fromDate + "') AND dbo.FnDate('" + toDate + "') )";
                }
            }
            else if (lCType == "Mature")
            {
                //Date
                if (fromDate != null)
                {
                    Where = "(dbo.FnDate(pr.LCMatureDate) BETWEEN dbo.FnDate('" + fromDate + "') AND dbo.FnDate('" + fromDate + "') )";
                }
                if (toDate != null)
                {
                    Where = "(dbo.FnDate(pr.LCMatureDate) BETWEEN dbo.FnDate('" + toDate + "') AND dbo.FnDate('" + toDate + "') )";
                }
                if (fromDate != null && toDate != null)
                {
                    Where = "(dbo.FnDate(pr.LCMatureDate) BETWEEN dbo.FnDate('" + fromDate + "') AND dbo.FnDate('" + toDate + "') )";
                }
            }
            else if (lCType == "Settle")
            {
                //Date
                if (fromDate != null)
                {
                    Where = "(dbo.FnDate(pr.SettlementDate) BETWEEN dbo.FnDate('" + fromDate + "') AND dbo.FnDate('" + fromDate + "') )";
                }
                if (toDate != null)
                {
                    Where = "(dbo.FnDate(pr.SettlementDate) BETWEEN dbo.FnDate('" + toDate + "') AND dbo.FnDate('" + toDate + "') )";
                }
                if (fromDate != null && toDate != null)
                {
                    Where = "(dbo.FnDate(pr.SettlementDate) BETWEEN dbo.FnDate('" + fromDate + "') AND dbo.FnDate('" + toDate + "') )";
                }
            }
            //else if (lCType == "Cancel")
            //{
            //    //Date
            //    if (fromDate != null)
            //    {
            //        Where = "(dbo.FnDate(pr.LCOpenDate) BETWEEN dbo.FnDate('" + fromDate + "') AND dbo.FnDate('" + fromDate + "') )";
            //    }
            //    if (toDate != null)
            //    {
            //        Where = "(dbo.FnDate(pr.LCOpenDate) BETWEEN dbo.FnDate('" + toDate + "') AND dbo.FnDate('" + toDate + "') )";
            //    }
            //    if (fromDate != null && toDate != null)
            //    {
            //        Where = "(dbo.FnDate(pr.LCOpenDate) BETWEEN dbo.FnDate('" + fromDate + "') AND dbo.FnDate('" + toDate + "') )";
            //    }
            //}
            else if (lCType == "Supplier")
            {
                //Date
                if (fromDate != null)
                {
                    Where = "(dbo.FnDate(pr.LCOpenDate) BETWEEN dbo.FnDate('" + fromDate + "') AND dbo.FnDate('" + fromDate + "') )";
                }
                if (toDate != null)
                {
                    Where = "(dbo.FnDate(pr.LCOpenDate) BETWEEN dbo.FnDate('" + toDate + "') AND dbo.FnDate('" + toDate + "') )";
                }
                if (fromDate != null && toDate != null)
                {
                    Where = "(dbo.FnDate(pr.LCOpenDate) BETWEEN dbo.FnDate('" + fromDate + "') AND dbo.FnDate('" + toDate + "') )";
                }

                if (!string.IsNullOrWhiteSpace(supplierId.ToString()))
                {
                    if (supplierId != 0)
                    {
                        if (!string.IsNullOrWhiteSpace(Where))
                        {
                            Where += " AND pr.SupplierId =" + supplierId.ToString() + "";
                        }
                        else
                        {
                            Where += "pr.SupplierId =" + supplierId.ToString() + "";
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where;
            }

            return Where;
        }
        public bool UpdateLCOrderStatus(int orderId, string approvedStatus, int approvedBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateLCOrderStatus_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@LCId", DbType.Int32, orderId);
                    dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, approvedStatus);
                    dbSmartAspects.AddInParameter(command, "@ApprovedBy", DbType.Int32, approvedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;

        }
    }
}
