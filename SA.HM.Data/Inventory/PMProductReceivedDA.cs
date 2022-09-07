using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Inventory;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.PurchaseManagment;

namespace HotelManagement.Data.PurchaseManagment
{
    public class PMProductReceivedDA : BaseService
    {
        public bool SaveProductReceiveInfo(PMProductReceivedBO receivedProduct, List<PMProductReceivedDetailsBO> addedReceiveItem,
                                           List<PMProductSerialInfoBO> serialzableProduct, bool isApprovalProcessEnable, List<OverheadExpensesBO> AddedOverheadExpenses, List<OverheadExpensesBO> AddedPaymentMethodInfos, out int receivedId, out string TransactionNo, out string TransactionType, out string ApproveStatus)
        {
            int status = 0;
            bool retVal = false;
            int receivedDetailsId = 0;
            receivedId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePMProductReceived_SP"))
                        {

                            dbSmartAspects.AddInParameter(command,  "@ReceivedDate", DbType.DateTime, receivedProduct.ReceivedDate);
                            dbSmartAspects.AddInParameter(command,  "@ReceiveType", DbType.String, receivedProduct.ReceiveType);
                            dbSmartAspects.AddInParameter(command,  "@POrderId", DbType.Int32, receivedProduct.POrderId);
                            dbSmartAspects.AddInParameter(command,  "@SupplierId", DbType.Int32, receivedProduct.SupplierId);
                            dbSmartAspects.AddInParameter(command,  "@CostCenterId", DbType.Int32, receivedProduct.CostCenterId);
                            dbSmartAspects.AddInParameter(command,  "@LocationId", DbType.Int32, receivedProduct.LocationId);
                            dbSmartAspects.AddInParameter(command,  "@CurrencyId", DbType.Int32, receivedProduct.CurrencyId);
                            dbSmartAspects.AddInParameter(command,  "@ConvertionRate", DbType.Decimal, receivedProduct.ConvertionRate);
                            dbSmartAspects.AddInParameter(command,  "@Status", DbType.String, receivedProduct.Status);
                            dbSmartAspects.AddInParameter(command,  "@Remarks", DbType.String, receivedProduct.Remarks);
                            dbSmartAspects.AddInParameter(command,  "@CreatedBy", DbType.Int32, receivedProduct.CreatedBy);
                            dbSmartAspects.AddInParameter(command, "@ReferenceNumber", DbType.String, receivedProduct.ReferenceNumber);

                            if (receivedProduct.ReferenceBillDate == null)
                            {
                                dbSmartAspects.AddInParameter(command, "@ReferenceBillDate", DbType.DateTime, DBNull.Value);
                            }
                            else
                            {
                                dbSmartAspects.AddInParameter(command, "@ReferenceBillDate", DbType.DateTime, Convert.ToDateTime(receivedProduct.ReferenceBillDate));
                            }
                            if (receivedProduct.CompanyId == null || receivedProduct.CompanyId == 0)
                            {
                                dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, DBNull.Value);
                            }
                            else
                            {
                                dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, (receivedProduct.CompanyId));
                            }
                            if (receivedProduct.ProjectId == null || receivedProduct.ProjectId == 0)
                            {
                                dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int32, DBNull.Value);
                            }
                            else
                            {
                                dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int32, (receivedProduct.ProjectId));
                            }

                            dbSmartAspects.AddOutParameter(command, "@ReceivedId", DbType.Int32, sizeof(Int32));

                            dbSmartAspects.AddOutParameter(command, "@TransactionNo", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(command, "@TransactionType", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(command, "@ApproveStatus", DbType.String, 100);

                            


                            status = dbSmartAspects.ExecuteNonQuery(command, transction);

                            receivedId = Convert.ToInt32(command.Parameters["@ReceivedId"].Value);

                            TransactionNo = command.Parameters["@TransactionNo"].Value.ToString();
                            TransactionType = command.Parameters["@TransactionType"].Value.ToString();
                            ApproveStatus = command.Parameters["@ApproveStatus"].Value.ToString();
                        }

                        if (status > 0 && addedReceiveItem.Count > 0)
                        {
                            using (DbCommand cmdReceiveDetails = dbSmartAspects.GetStoredProcCommand("SavePMProductReceivedDetails_SP"))
                            {
                                foreach (PMProductReceivedDetailsBO rd in addedReceiveItem)
                                {
                                    cmdReceiveDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ReceivedId", DbType.Int32, receivedId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@POrderId", DbType.Int32, receivedProduct.POrderId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ItemId", DbType.Int32, rd.ItemId);

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ColorId", DbType.Int32, rd.ColorId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@SizeId", DbType.Int32, rd.SizeId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@StyleId", DbType.Int32, rd.StyleId);

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@StockById", DbType.Int32, rd.StockById);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@Quantity", DbType.Decimal, rd.Quantity);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@PurchasePrice", DbType.Decimal, rd.PurchasePrice);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ReceiveType", DbType.String, receivedProduct.ReceiveType);

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@BagQuantity", DbType.Decimal, rd.BagQuantity);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@BonusAmount", DbType.Decimal, rd.BonusAmount);

                                    dbSmartAspects.AddOutParameter(cmdReceiveDetails, "@ReceiveDetailsId", DbType.Int32, sizeof(Int32));

                                    status = dbSmartAspects.ExecuteNonQuery(cmdReceiveDetails, transction);

                                    receivedDetailsId = Convert.ToInt32(cmdReceiveDetails.Parameters["@ReceiveDetailsId"].Value);
                                    rd.ReceiveDetailsId = receivedDetailsId;
                                }
                            }
                        }

                        if (status > 0 && serialzableProduct.Count > 0)
                        {
                            using (DbCommand cmdReceiveDetails = dbSmartAspects.GetStoredProcCommand("SavePMSerialInfoByProduct_SP"))
                            {
                                foreach (PMProductSerialInfoBO rd in serialzableProduct)
                                {
                                    cmdReceiveDetails.Parameters.Clear();

                                    receivedDetailsId = (from rds in addedReceiveItem
                                                         where rds.ItemId == rd.ItemId
                                                         select rds.ReceiveDetailsId).FirstOrDefault();

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ReceivedId", DbType.Int32, receivedId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ReceiveDetailsId", DbType.Int32, receivedDetailsId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@POrderId", DbType.Int32, receivedProduct.POrderId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@SerialNumber", DbType.String, rd.SerialNumber);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@CreatedBy", DbType.Int32, receivedProduct.CreatedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdReceiveDetails, transction);
                                }
                            }
                        }

                        if(status > 0 && AddedOverheadExpenses.Count > 0)
                        {
                            using (DbCommand cmdOverheadDetails = dbSmartAspects.GetStoredProcCommand("SaveOverheadExpenseForReceiveId_SP"))
                            {
                                foreach (OverheadExpensesBO oe in AddedOverheadExpenses)
                                {
                                    cmdOverheadDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOverheadDetails, "@ReceivedId", DbType.Int32, receivedId);
                                    dbSmartAspects.AddInParameter(cmdOverheadDetails, "@NodeId", DbType.Int32, oe.NodeId);
                                    dbSmartAspects.AddInParameter(cmdOverheadDetails, "@Amount", DbType.Decimal, oe.Amount);
                                    dbSmartAspects.AddInParameter(cmdOverheadDetails, "@Remarks", DbType.String, oe.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOverheadDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && AddedPaymentMethodInfos.Count > 0)
                        {
                            using (DbCommand cmdPMDetails = dbSmartAspects.GetStoredProcCommand("SavePaymentInformationsForReceiveId_SP"))
                            {
                                foreach (OverheadExpensesBO pm in AddedPaymentMethodInfos)
                                {
                                    cmdPMDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdPMDetails, "@ReceivedId", DbType.Int32, receivedId);
                                    dbSmartAspects.AddInParameter(cmdPMDetails, "@NodeId", DbType.Int32, pm.NodeId);
                                    dbSmartAspects.AddInParameter(cmdPMDetails, "@Amount", DbType.Decimal, pm.Amount);
                                    dbSmartAspects.AddInParameter(cmdPMDetails, "@Remarks", DbType.String, pm.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdPMDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && isApprovalProcessEnable == true && receivedProduct.ReceiveType != "AdHoc")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReceiveStatusForPurchaseWiseReceivee_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@ReceivedId", DbType.Int32, receivedId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ReceiveType", DbType.String, receivedProduct.ReceiveType);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }
                        if (status > 0 && isApprovalProcessEnable == false)
                        {
                            using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("ReceiveOrderApproval_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@ReceivedId", DbType.Int32, receivedId);
                                dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, receivedProduct.Status);
                                dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, receivedProduct.CreatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            }

                            if (status > 0 && receivedProduct.ReceiveType != "AdHoc" && receivedProduct.Status == "Approved")
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReceiveStatusForPurchaseWiseReceivee_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandDetails, "@ReceivedId", DbType.Int32, receivedId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ReceiveType", DbType.String, receivedProduct.ReceiveType);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, receivedProduct.Status);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }

                            if (receivedProduct.POrderId > 0 && status > 0 && receivedProduct.Status == "Approved")
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateProductReceiveStatusNItemStockNAverageCost_SP"))
                                {
                                    dbSmartAspects.AddInParameter(command, "@ReceivedId", DbType.Int32, receivedId);
                                    dbSmartAspects.AddInParameter(command, "@Status", DbType.String, receivedProduct.Status);
                                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, receivedProduct.CreatedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                            else if (status > 0 && receivedProduct.Status == "Approved")
                            {
                                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateProductAdhocReceiveStatusNItemStockNAverageCost_SP"))
                                {
                                    dbSmartAspects.AddInParameter(command, "@ReceivedId", DbType.Int32, receivedId);
                                    dbSmartAspects.AddInParameter(command, "@Status", DbType.String, receivedProduct.Status);
                                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, receivedProduct.CreatedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }
                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public bool UpdateProductReceiveInfo(PMProductReceivedBO receivedProduct, List<PMProductReceivedDetailsBO> AddedReceivedDetails,
                                             List<PMProductReceivedDetailsBO> EditedReceivedDetails,
                                             List<OverheadExpensesBO> AddedOverheadExpenses,
                                             List<OverheadExpensesBO> AddedPaymentMethodInfos,
                                             List<PMProductReceivedDetailsBO> DeletedReceivedDetails,
                                             List<PMProductSerialInfoBO> AddedSerialzableProduct,
                                             List<PMProductSerialInfoBO> DeletedSerialzableProduct, out string TransactionNo, out string TransactionType, out string ApproveStatus
                                            )
        {
            int status = 0;
            bool retVal = false;
            int receivedId = 0, receivedDetailsId = 0;

            receivedId = receivedProduct.ReceivedId;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePMProductReceived_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@ReceivedId", DbType.Int32, receivedProduct.ReceivedId);
                            dbSmartAspects.AddInParameter(command, "@ReceiveType", DbType.String, receivedProduct.ReceiveType);
                            dbSmartAspects.AddInParameter(command, "@POrderId", DbType.Int32, receivedProduct.POrderId);
                            dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, receivedProduct.SupplierId);
                            dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, receivedProduct.CostCenterId);
                            dbSmartAspects.AddInParameter(command, "@LocationId", DbType.Int32, receivedProduct.LocationId);
                            dbSmartAspects.AddInParameter(command, "@CurrencyId", DbType.Int32, receivedProduct.CurrencyId);
                            dbSmartAspects.AddInParameter(command, "@ConvertionRate", DbType.Decimal, receivedProduct.ConvertionRate);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, receivedProduct.Remarks);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, receivedProduct.CreatedBy);
                            dbSmartAspects.AddInParameter(command, "@ReferenceNumber", DbType.String, receivedProduct.ReferenceNumber);
                            if (receivedProduct.ReferenceBillDate == null)
                            {
                                dbSmartAspects.AddInParameter(command, "@ReferenceBillDate", DbType.DateTime, DBNull.Value);
                            }
                            else
                            {
                                dbSmartAspects.AddInParameter(command, "@ReferenceBillDate", DbType.DateTime, Convert.ToDateTime(receivedProduct.ReferenceBillDate));
                            }

                            if (receivedProduct.CompanyId == null || receivedProduct.CompanyId == 0)
                            {
                                dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, DBNull.Value);
                            }
                            else
                            {
                                dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, (receivedProduct.CompanyId));
                            }
                            if (receivedProduct.ProjectId == null || receivedProduct.ProjectId == 0)
                            {
                                dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int32, DBNull.Value);
                            }
                            else
                            {
                                dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int32, (receivedProduct.ProjectId));
                            }
                            dbSmartAspects.AddOutParameter(command, "@TransactionNo", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(command, "@TransactionType", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(command, "@ApproveStatus", DbType.String, 100);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);

                            TransactionNo = command.Parameters["@TransactionNo"].Value.ToString();
                            TransactionType = command.Parameters["@TransactionType"].Value.ToString();
                            ApproveStatus = command.Parameters["@ApproveStatus"].Value.ToString();
                        }

                        if (status > 0 && AddedReceivedDetails.Count > 0)
                        {
                            using (DbCommand cmdReceiveDetails = dbSmartAspects.GetStoredProcCommand("SavePMProductReceivedDetails_SP"))
                            {
                                foreach (PMProductReceivedDetailsBO rd in AddedReceivedDetails)
                                {
                                    cmdReceiveDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ReceivedId", DbType.Int32, receivedId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@POrderId", DbType.Int32, receivedProduct.POrderId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ItemId", DbType.Int32, rd.ItemId);

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ColorId", DbType.Int32, rd.ColorId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@SizeId", DbType.Int32, rd.SizeId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@StyleId", DbType.Int32, rd.StyleId);

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@StockById", DbType.Int32, rd.StockById);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@Quantity", DbType.Decimal, rd.Quantity);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@PurchasePrice", DbType.Decimal, rd.PurchasePrice);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ReceiveType", DbType.String, receivedProduct.ReceiveType);

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@BagQuantity", DbType.Int32, rd.BagQuantity);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@BonusAmount", DbType.Decimal, rd.BonusAmount);

                                    dbSmartAspects.AddOutParameter(cmdReceiveDetails, "@ReceiveDetailsId", DbType.Int32, sizeof(Int32));

                                    status = dbSmartAspects.ExecuteNonQuery(cmdReceiveDetails, transction);

                                    receivedDetailsId = Convert.ToInt32(cmdReceiveDetails.Parameters["@ReceiveDetailsId"].Value);
                                    rd.ReceiveDetailsId = receivedDetailsId;
                                }
                            }
                        }

                        if (status > 0 && EditedReceivedDetails.Count > 0)
                        {
                            using (DbCommand cmdUpdateDetails = dbSmartAspects.GetStoredProcCommand("UpdatePMProductReceivedDetails_SP"))
                            {
                                foreach (PMProductReceivedDetailsBO rd in EditedReceivedDetails)
                                {
                                    cmdUpdateDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdUpdateDetails, "@ReceiveDetailsId", DbType.Int32, rd.ReceiveDetailsId);
                                    dbSmartAspects.AddInParameter(cmdUpdateDetails, "@ReceivedId", DbType.Int32, receivedId);
                                    dbSmartAspects.AddInParameter(cmdUpdateDetails, "@POrderId", DbType.Int32, receivedProduct.POrderId);
                                    dbSmartAspects.AddInParameter(cmdUpdateDetails, "@ItemId", DbType.Int32, rd.ItemId);

                                    dbSmartAspects.AddInParameter(cmdUpdateDetails, "@ColorId", DbType.Int32, rd.ColorId);
                                    dbSmartAspects.AddInParameter(cmdUpdateDetails, "@SizeId", DbType.Int32, rd.SizeId);
                                    dbSmartAspects.AddInParameter(cmdUpdateDetails, "@StyleId", DbType.Int32, rd.StyleId);

                                    dbSmartAspects.AddInParameter(cmdUpdateDetails, "@StockById", DbType.Int32, rd.StockById);
                                    dbSmartAspects.AddInParameter(cmdUpdateDetails, "@Quantity", DbType.Decimal, rd.Quantity);
                                    dbSmartAspects.AddInParameter(cmdUpdateDetails, "@PurchasePrice", DbType.Decimal, rd.PurchasePrice);
                                    dbSmartAspects.AddInParameter(cmdUpdateDetails, "@ReceiveType", DbType.String, receivedProduct.ReceiveType);

                                    dbSmartAspects.AddInParameter(cmdUpdateDetails, "@BagQuantity", DbType.Int32, rd.BagQuantity);
                                    dbSmartAspects.AddInParameter(cmdUpdateDetails, "@BonusAmount", DbType.Decimal, rd.BonusAmount);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdUpdateDetails, transction);
                                }
                            }
                        }

                        if(status > 0 && AddedOverheadExpenses.Count > 0)
                        {
                            using (DbCommand cmdOEDeleteDetails = dbSmartAspects.GetStoredProcCommand("DeleteOverheadExpenseForReceiveId_SP"))
                            {
                                cmdOEDeleteDetails.Parameters.Clear();
                                dbSmartAspects.AddInParameter(cmdOEDeleteDetails, "@ReceivedId", DbType.Int32, receivedId);
                                status = dbSmartAspects.ExecuteNonQuery(cmdOEDeleteDetails, transction);                                
                            }
                            using (DbCommand cmdOEUpdateDetails = dbSmartAspects.GetStoredProcCommand("UpdateOverheadExpenseForReceiveId_SP"))
                            {
                                foreach(OverheadExpensesBO oe in AddedOverheadExpenses)
                                {
                                    cmdOEUpdateDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOEUpdateDetails, "@ReceivedId", DbType.Int32, receivedId);
                                    dbSmartAspects.AddInParameter(cmdOEUpdateDetails, "@NodeId", DbType.Int32, oe.NodeId);
                                    dbSmartAspects.AddInParameter(cmdOEUpdateDetails, "@Amount", DbType.Decimal, oe.Amount);
                                    dbSmartAspects.AddInParameter(cmdOEUpdateDetails, "@Remarks", DbType.String, oe.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOEUpdateDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && AddedPaymentMethodInfos.Count > 0)
                        {
                            using (DbCommand cmdPMDeleteDetails = dbSmartAspects.GetStoredProcCommand("DeletePaymentInformationsForReceiveId_SP"))
                            {
                                cmdPMDeleteDetails.Parameters.Clear();
                                dbSmartAspects.AddInParameter(cmdPMDeleteDetails, "@ReceivedId", DbType.Int32, receivedId);
                                status = dbSmartAspects.ExecuteNonQuery(cmdPMDeleteDetails, transction);
                            }
                            using (DbCommand cmdPMUpdateDetails = dbSmartAspects.GetStoredProcCommand("UpdatePaymentInformationsForReceiveId_SP"))
                            {
                                foreach (OverheadExpensesBO pm in AddedPaymentMethodInfos)
                                {
                                    cmdPMUpdateDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdPMUpdateDetails, "@ReceivedId", DbType.Int32, receivedId);
                                    dbSmartAspects.AddInParameter(cmdPMUpdateDetails, "@NodeId", DbType.Int32, pm.NodeId);
                                    dbSmartAspects.AddInParameter(cmdPMUpdateDetails, "@Amount", DbType.Decimal, pm.Amount);
                                    dbSmartAspects.AddInParameter(cmdPMUpdateDetails, "@Remarks", DbType.String, pm.Remarks);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdPMUpdateDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && AddedSerialzableProduct.Count > 0)
                        {
                            using (DbCommand cmdReceiveDetails = dbSmartAspects.GetStoredProcCommand("SavePMSerialInfoByProduct_SP"))
                            {
                                foreach (PMProductSerialInfoBO rd in AddedSerialzableProduct)
                                {
                                    cmdReceiveDetails.Parameters.Clear();

                                    receivedDetailsId = (from rds in AddedReceivedDetails
                                                         where rds.ItemId == rd.ItemId
                                                         select rds.ReceiveDetailsId).FirstOrDefault();

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ReceivedId", DbType.Int32, receivedId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ReceiveDetailsId", DbType.Int32, receivedDetailsId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@POrderId", DbType.Int32, receivedProduct.POrderId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@SerialNumber", DbType.String, rd.SerialNumber);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@CreatedBy", DbType.Int32, receivedProduct.CreatedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdReceiveDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedSerialzableProduct.Count > 0)
                        {
                            using (DbCommand commandDeletePO = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (PMProductSerialInfoBO rd in DeletedSerialzableProduct)
                                {
                                    commandDeletePO.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TableName", DbType.String, "PMProductSerialInfo");
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TablePKField", DbType.String, "SerialId");
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@TablePKId", DbType.String, rd.SerialId);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePO, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedReceivedDetails.Count() > 0)
                        {
                            using (DbCommand commandDeletePO = dbSmartAspects.GetStoredProcCommand("DeleteReceiveDetailsInfo_SP"))
                            {
                                foreach (PMProductReceivedDetailsBO po in DeletedReceivedDetails)
                                {
                                    commandDeletePO.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ReceiveDetailsId", DbType.Int32, po.ReceiveDetailsId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@POrderId", DbType.Int32, receivedProduct.POrderId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ItemId", DbType.Int32, po.ItemId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ColorId", DbType.Int32, po.ColorId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@SizeId", DbType.Int32, po.SizeId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@StyleId", DbType.Int32, po.StyleId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@Quantity", DbType.Decimal, po.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePO, transction);
                                }
                            }
                        }

                        if (status > 0 && receivedProduct.ReceiveType != "AdHoc")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReceiveStatusForPurchaseWiseReceivee_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@ReceivedId", DbType.Int32, receivedProduct.ReceivedId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ReceiveType", DbType.String, receivedProduct.ReceiveType);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public bool UpdateProductReceiveStatus(PMProductReceivedBO receivedProduct)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePMProductReceivedStatus_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@ReceivedId", DbType.Int32, receivedProduct.ReceivedId);
                            dbSmartAspects.AddInParameter(command, "@Status", DbType.String, receivedProduct.Status);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, receivedProduct.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public bool UpdateProductReceiveStatusNItemStockNAverageCost(PMProductReceivedBO receivedProduct)
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
                        if (receivedProduct.POrderId > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateProductReceiveStatusNItemStockNAverageCost_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@ReceivedId", DbType.Int32, receivedProduct.ReceivedId);
                                dbSmartAspects.AddInParameter(command, "@Status", DbType.String, receivedProduct.Status);
                                dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, receivedProduct.LastModifiedBy);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }
                        else
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateProductAdhocReceiveStatusNItemStockNAverageCost_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@ReceivedId", DbType.Int32, receivedProduct.ReceivedId);
                                dbSmartAspects.AddInParameter(command, "@Status", DbType.String, receivedProduct.Status);
                                dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, receivedProduct.LastModifiedBy);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public bool DeleteProductReceiveInfo(int receivedId, int pOrderId)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeletePMProductReceived_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@ReceivedId", DbType.DateTime, receivedId);
                            dbSmartAspects.AddInParameter(command, "@POrderId", DbType.Int32, pOrderId);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);

                            receivedId = Convert.ToInt32(command.Parameters["@ReceivedId"].Value);
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return retVal;
        }
        public List<PMProductReceivedBO> GetProductreceiveInfo(int companyId, int projectId, string receiveType, DateTime? fromDate, DateTime? toDate,
                                                               string receiveNumber, string status, int? costCenterId, int? supplierId,
                                                               Int32 userInfoId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<PMProductReceivedBO> productReceive = new List<PMProductReceivedBO>();
            totalRecords = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReceive_SP"))
                {
                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.String, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.String, DBNull.Value);

                    if (receiveType != "All")
                        dbSmartAspects.AddInParameter(cmd, "@ReceiveType", DbType.String, receiveType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReceiveType", DbType.String, DBNull.Value);

                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    //dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    //dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    if (!string.IsNullOrEmpty(receiveNumber))
                        dbSmartAspects.AddInParameter(cmd, "@ReceiveNumber", DbType.String, receiveNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReceiveNumber", DbType.String, DBNull.Value);

                    if (costCenterId != null)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.String, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.String, DBNull.Value);

                    if (supplierId != null)
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.String, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.String, DBNull.Value);

                    if (status != "All")
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, status);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReceivedBO
                    {
                        ReceivedId = r.Field<Int32>("ReceivedId"),
                        ReceiveType = r.Field<string>("ReceiveType"),
                        ReceiveNumber = r.Field<string>("ReceiveNumber"),
                        ReceivedDate = r.Field<DateTime>("ReceivedDate"),
                        POrderId = r.Field<Int32>("POrderId"),
                        SupplierId = r.Field<Int32>("SupplierId"),
                        SupplierName = r.Field<string>("SupplierName"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        LocationId = r.Field<Int32>("LocationId"),
                        LocationName = r.Field<string>("LocationName"),
                        Status = r.Field<string>("Status"),
                        Remarks = r.Field<string>("Remarks"),
                        TotalReceivedAmount = r.Field<decimal>("TotalReceivedAmount"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CheckedBy = r.Field<Int32>("CheckedBy"),
                        ApprovedBy = r.Field<Int32>("ApprovedBy"),
                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanDelete = r.Field<bool>("IsCanDelete"),
                        IsCanChecked = r.Field<bool>("IsCanChecked"),
                        IsCanApproved = r.Field<bool>("IsCanApproved")

                    }).ToList();

                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value.ToString());
                }
            }

            return productReceive;
        }

        public List<PMProductReceivedBO> GetProductReceiveByReceiveNoNDateRange(DateTime fromDate, DateTime toDate, string receiveNumber)
        {
            List<PMProductReceivedBO> productReceive = new List<PMProductReceivedBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReceiveByReceiveNoNDateRange_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, Convert.ToDateTime(fromDate));
                    dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, Convert.ToDateTime(toDate));

                    if (!string.IsNullOrEmpty(receiveNumber))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReceiveNumber", DbType.String, receiveNumber);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ReceiveNumber", DbType.String, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReceivedBO
                    {
                        ReceiveNumber = r.Field<string>("ReceiveNumber"),
                        ReceivedDate = r.Field<DateTime>("ReceivedDate"),
                        ReceivedId = r.Field<Int32>("ReceivedId"),
                        POrderId = r.Field<Int32>("POrderId"),
                        PONumber = r.Field<string>("PONumber"),
                        Status = r.Field<string>("Status")

                    }).ToList();
                }
            }

            return productReceive;
        }

        public List<PMProductReceivedBO> GetProductReceiveDetailsByReceiveOrSupplier(DateTime? fromDate, DateTime? toDate, string receiveNumber, int? supplierId)
        {
            List<PMProductReceivedBO> productReceive = new List<PMProductReceivedBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReceiveDetailsByReceiveOrSupplier_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    if (!string.IsNullOrEmpty(receiveNumber))
                        dbSmartAspects.AddInParameter(cmd, "@ReceiveNumber", DbType.String, receiveNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReceiveNumber", DbType.String, DBNull.Value);

                    if (supplierId != null)
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.String, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReceivedBO
                    {
                        ReceivedId = r.Field<Int32>("ReceivedId"),
                        ReceiveType = r.Field<string>("ReceiveType"),
                        ReceiveNumber = r.Field<string>("ReceiveNumber"),
                        ReceivedDate = r.Field<DateTime>("ReceivedDate"),
                        POrderId = r.Field<Int32>("POrderId"),
                        SupplierId = r.Field<Int32>("SupplierId"),
                        SupplierName = r.Field<string>("SupplierName"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        LocationId = r.Field<Int32>("LocationId"),
                        LocationName = r.Field<string>("LocationName"),
                        Status = r.Field<string>("Status"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CheckedBy = r.Field<Int32>("CheckedBy"),
                        ApprovedBy = r.Field<Int32>("ApprovedBy")

                    }).ToList();
                }
            }

            return productReceive;
        }

        public List<PMProductReceivedBO> GetProductReceiveDetailsByIdForReturn(int receivedId)
        {
            List<PMProductReceivedBO> productReceive = new List<PMProductReceivedBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReceiveDetailsByIdForReturn_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReceivedId", DbType.Int32, receivedId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReceivedBO
                    {
                        ReceivedId = r.Field<Int32>("ReceivedId"),
                        ReceiveType = r.Field<string>("ReceiveType"),
                        ReceiveNumber = r.Field<string>("ReceiveNumber"),
                        ReceivedDate = r.Field<DateTime>("ReceivedDate"),
                        POrderId = r.Field<Int32>("POrderId"),
                        SupplierId = r.Field<Int32>("SupplierId"),
                        SupplierName = r.Field<string>("SupplierName"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        CostCenter = r.Field<string>("CostCenter"),
                        LocationId = r.Field<Int32>("LocationId"),
                        LocationName = r.Field<string>("LocationName"),
                        Status = r.Field<string>("Status"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CheckedBy = r.Field<Int32>("CheckedBy"),
                        ApprovedBy = r.Field<Int32>("ApprovedBy")

                    }).ToList();
                }
            }

            return productReceive;
        }


        public PMProductReceivedBO GetProductreceiveInfo(int receivedId)
        {
            PMProductReceivedBO productReceive = new PMProductReceivedBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReceiveById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReceivedId", DbType.Int32, receivedId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReceivedBO
                    {
                        ReceivedId = r.Field<Int32>("ReceivedId"),
                        ReceiveNumber = r.Field<string>("ReceiveNumber"),
                        ReceivedDate = r.Field<DateTime>("ReceivedDate"),
                        POrderId = r.Field<Int32>("POrderId"),
                        SupplierId = r.Field<Int32>("SupplierId"),
                        Status = r.Field<string>("Status"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        ReceiveType = r.Field<string>("ReceiveType"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        ReferenceNumber = r.Field<string> ("ReferenceNumber"),
                        ReferenceBillDate = r.Field<DateTime?>("ReferenceBillDate"),
                        LocationId = r.Field<Int32>("LocationId"),
                        CurrencyId = r.Field<Int32>("CurrencyId"),
                        ConvertionRate = r.Field<decimal>("ConvertionRate"),
                        Remarks = r.Field<string>("Remarks"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        ProjectId = r.Field<Int32>("ProjectId"),
                        PONumber = r.Field<string>("PONumber")

                    }).FirstOrDefault();
                }
            }

            return productReceive;
        }
        public List<PMProductReceivedDetailsBO> GetProductreceiveDetailsInfo(int receivedId)
        {
            List<PMProductReceivedDetailsBO> productReceive = new List<PMProductReceivedDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReceiveDetails_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReceivedId", DbType.Int32, receivedId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReceivedDetailsBO
                    {
                        ReceivedId = r.Field<Int32>("ReceivedId"),
                        POrderId = r.Field<Int32>("POrderId"),
                        ReceiveDetailsId = r.Field<Int32>("ReceiveDetailsId"),
                        ProductId = r.Field<Int32>("ProductId"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        StockById = r.Field<Int32>("StockById"),
                        LocationId = r.Field<Int32>("LocationId"),
                        Quantity = r.Field<decimal>("Quantity"),
                        SerialNumber = r.Field<string>("SerialNumber"),
                        ItemName = r.Field<string>("ItemName"),
                        CostCenter = r.Field<string>("CostCenter"),
                        StockBy = r.Field<string>("StockBy"),
                        LocationName = r.Field<string>("LocationName"),
                        SupplierId = r.Field<Int32>("SupplierId"),
                        SupplierName = r.Field<string>("SupplierName"),
                        OrderedQuantity = r.Field<decimal>("OrderedQuantity"),
                        QuantityReceived = r.Field<decimal>("QuantityReceived"),
                        RemainingQuantity = r.Field<decimal>("RemainingQuantity"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),
                        Status = r.Field<string>("Status"),
                        ReceivedStatus = r.Field<string>("ReceivedStatus"),
                        DefaultStockBy = r.Field<string>("DefaultStockBy")

                    }).ToList();
                }
            }

            return productReceive;
        }
        public List<PMProductReceivedDetailsBO> GetProductreceiveDetailsReportInfo(int receivedId)
        {
            List<PMProductReceivedDetailsBO> productReceive = new List<PMProductReceivedDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReceiveDetailsReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReceivedId", DbType.Int32, receivedId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceivedReport");
                    DataTable Table = ds.Tables["PMProductReceivedReport"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReceivedDetailsBO
                    {
                        ReceivedId = r.Field<Int32>("ReceivedId"),
                        ReceiveNumber = r.Field<string>("ReceiveNumber"),
                        ReceivedDate = r.Field<DateTime>("ReceivedDate"),
                        StringReceivedDate = r.Field<string>("StringReceivedDate"),
                        CompanyId = r.Field<Int32>("CompanyId"),
                        POrderId = r.Field<Int32>("POrderId"),
                        PONumber = r.Field<string>("PONumber"),
                        StringPODate = r.Field<string>("StringPODate"),
                        POByName = r.Field<string>("POByName"),
                        ReceiveDetailsId = r.Field<Int32>("ReceiveDetailsId"),
                        ProductId = r.Field<Int32>("ProductId"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        StockById = r.Field<Int32>("StockById"),
                        LocationId = r.Field<Int32>("LocationId"),
                        Quantity = r.Field<decimal>("Quantity"),
                        SerialNumber = r.Field<string>("SerialNumber"),
                        ItemName = r.Field<string>("ItemName"),
                        CostCenter = r.Field<string>("CostCenter"),
                        StockBy = r.Field<string>("StockBy"),
                        LocationName = r.Field<string>("LocationName"),
                        SupplierId = r.Field<Int32>("SupplierId"),
                        SupplierName = r.Field<string>("SupplierName"),
                        OrderedQuantity = r.Field<decimal>("OrderedQuantity"),
                        QuantityReceived = r.Field<decimal>("QuantityReceived"),
                        RemainingQuantity = r.Field<decimal>("RemainingQuantity"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),
                        Status = r.Field<string>("Status"),
                        ReceivedStatus = r.Field<string>("ReceivedStatus"),
                        CheckedByName = r.Field<string>("CheckedByName"),
                        ApprovedByName = r.Field<string>("ApprovedByName"),
                        ReceivedByName = r.Field<string>("ReceivedByName"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }

            return productReceive;
        }
        public List<PMProductReceivedDetailsBO> GetProductReceiveDetailsById(int receivedId)
        {
            List<PMProductReceivedDetailsBO> productReceive = new List<PMProductReceivedDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReceiveDetailsByReceiveId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReceivedId", DbType.Int32, receivedId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReceivedDetailsBO
                    {
                        ReceivedId = r.Field<Int32>("ReceivedId"),
                        ReceiveDetailsId = r.Field<Int32>("ReceiveDetailsId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        ColorId = r.Field<Int32>("ColorId"),
                        ColorText = r.Field<string>("ColorText"),
                        SizeId = r.Field<Int32>("SizeId"),
                        SizeText = r.Field<string>("SizeText"),
                        StyleId = r.Field<Int32>("StyleId"),
                        StyleText = r.Field<string>("StyleText"),
                        StockById = r.Field<Int32>("StockById"),
                        Quantity = r.Field<decimal>("Quantity"),                        
                        StockBy = r.Field<string>("StockBy"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),
                        ProductType = r.Field<string>("ProductType"),
                        BagQuantity = r.Field<Int32>("BagQuantity"),
                        BonusAmount = r.Field<decimal>("BonusAmount")

                    }).ToList();
                }
            }

            return productReceive;
        }

        public List<PMProductSerialInfoBO> GetProductReceiveSerialById(int receivedId)
        {
            List<PMProductSerialInfoBO> productReceive = new List<PMProductSerialInfoBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReceiveSerialByReceiveId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReceivedId", DbType.Int32, receivedId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductSerialInfoBO
                    {
                        SerialId = r.Field<Int32>("SerialId"),
                        ReceivedId = r.Field<Int32>("ReceivedId"),
                        ReceiveDetailsId = r.Field<Int32>("ReceiveDetailsId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        SerialNumber = r.Field<string>("SerialNumber")

                    }).ToList();
                }
            }

            return productReceive;
        }

        public List<OverheadExpensesBO> GetOverheadExpenseForProductReceiveByReceivedId(int receivedId)
        {
            List<OverheadExpensesBO> productReceive = new List<OverheadExpensesBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOverheadExpenseForProductReceiveByReceivedId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReceivedId", DbType.Int32, receivedId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "OverheadExpenseOfProductReceived");
                    DataTable Table = ds.Tables["OverheadExpenseOfProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new OverheadExpensesBO
                    {
                        ReceivedId = r.Field<Int64>("ReceivedId"),
                        NodeId = r.Field<Int32>("NodeId"),
                        AccountHead = r.Field<string>("AccountHead"),
                        Amount = r.Field<decimal>("OEAmount"),
                        Remarks = r.Field<string>("OERemarks")

                    }).ToList();
                }
            }

            return productReceive;
        }
        public List<OverheadExpensesBO> GetPaymentInformationListByReceiveId(int receivedId)
        {
            List<OverheadExpensesBO> paymentInfoList = new List<OverheadExpensesBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPaymentMethodInfoByReceivedId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReceivedId", DbType.Int32, receivedId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    paymentInfoList = Table.AsEnumerable().Select(r => new OverheadExpensesBO
                    {
                        ReceivedId = r.Field<Int64>("ReceivedId"),
                        NodeId = r.Field<Int32>("NodeId"),
                        AccountHead = r.Field<string>("AccountHead"),
                        Amount = r.Field<decimal>("Amount"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }

            return paymentInfoList;
        }

        public List<PMProductReceivedDetailsBO> GetProductDetailsForReceiveFromPurchaseByReceiveId(int receivedId, int porderId)
        {
            List<PMProductReceivedDetailsBO> productReceive = new List<PMProductReceivedDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductDetailsForReceiveFromPurchaseByReceiveId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReceivedId", DbType.Int32, receivedId);
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, porderId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReceivedDetailsBO
                    {
                        ReceivedId = r.Field<Int32>("ReceivedId"),
                        ReceiveDetailsId = r.Field<Int32>("ReceiveDetailsId"),
                        POrderId = r.Field<Int32>("POrderId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        StockById = r.Field<Int32>("StockById"),
                        Quantity = r.Field<decimal>("Quantity"),
                        ItemName = r.Field<string>("ItemName"),
                        StockBy = r.Field<string>("StockBy"),
                        PurchaseQuantity = r.Field<decimal>("PurchaseQuantity"),
                        QuantityReceived = r.Field<decimal>("QuantityReceived"),
                        RemainingReceiveQuantity = r.Field<decimal>("RemainingReceiveQuantity"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),
                        ProductType = r.Field<string>("ProductType"),

                        ColorId = r.Field<Int32>("ColorId"),
                        SizeId = r.Field<Int32>("SizeId"),
                        StyleId = r.Field<Int32>("StyleId"),
                        ColorText = r.Field<string>("ColorText"),
                        SizeText = r.Field<string>("SizeText"),
                        StyleText = r.Field<string>("StyleText"),

                    }).ToList();
                }
            }

            return productReceive;
        }
        public List<PMProductReceivedDetailsBO> GetProductDetailsForReceiveFromLCByReceiveId(int receivedId, int porderId)
        {
            List<PMProductReceivedDetailsBO> productReceive = new List<PMProductReceivedDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductDetailsForReceiveFromLCByReceiveId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReceivedId", DbType.Int32, receivedId);
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, porderId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductReceivedDetailsBO
                    {
                        ReceivedId = r.Field<Int32>("ReceivedId"),
                        ReceiveDetailsId = r.Field<Int32>("ReceiveDetailsId"),
                        POrderId = r.Field<Int32>("POrderId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        StockById = r.Field<Int32>("StockById"),
                        Quantity = r.Field<decimal>("Quantity"),
                        ItemName = r.Field<string>("ItemName"),
                        StockBy = r.Field<string>("StockBy"),
                        PurchaseQuantity = r.Field<decimal>("PurchaseQuantity"),
                        QuantityReceived = r.Field<decimal>("QuantityReceived"),
                        RemainingReceiveQuantity = r.Field<decimal>("RemainingReceiveQuantity"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),
                        ProductType = r.Field<string>("ProductType")

                    }).ToList();
                }
            }

            return productReceive;
        }
        public List<PMProductReceivedReportViewBO> GetProductreceiveInfo(DateTime fromDate, DateTime toDate, int productId)
        {
            List<PMProductReceivedReportViewBO> productReceiveList = new List<PMProductReceivedReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReceiveForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    dbSmartAspects.AddInParameter(cmd, "@ProductId", DbType.Int32, productId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceiveList = Table.AsEnumerable().Select(r => new PMProductReceivedReportViewBO
                    {
                        Quantity = r.Field<decimal>("Quantity"),
                        ProductName = r.Field<string>("ProductName"),
                        UserName = r.Field<string>("UserName"),
                        PONumber = r.Field<string>("PONumber"),
                        ReceivedDate = r.Field<string>("ReceivedDate"),
                        CreatedBy = r.Field<int>("CreatedBy"),
                        CreatedDate = r.Field<DateTime>("CreatedDate"),
                        UnitHead = r.Field<string>("UnitHead"),
                    }).ToList();
                }
            }

            return productReceiveList;
        }
        public List<PMProductReceivedReportViewBO> GetProductOutInfo(DateTime fromDate, DateTime toDate,int categoryId, int productId, int costCenterIdFrom, int locationIdFrom, int costCenterIdTo, int locationIdTo)
        {
            List<PMProductReceivedReportViewBO> productReceiveList = new List<PMProductReceivedReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductOutForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    if (productId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProductId", DbType.Int32, productId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProductId", DbType.Int32, DBNull.Value);
                    }

                    if (categoryId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    }

                    if (costCenterIdFrom > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterIdFrom", DbType.Int32, costCenterIdFrom);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterIdFrom", DbType.Int32, DBNull.Value);
                    }

                    if (locationIdFrom > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LocationIdFrom", DbType.Int32, locationIdFrom);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LocationIdFrom", DbType.Int32, DBNull.Value);
                    }

                    if (costCenterIdTo > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterIdTo", DbType.Int32, costCenterIdTo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterIdTo", DbType.Int32, DBNull.Value);
                    }

                    if (locationIdTo > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LocationIdTo", DbType.Int32, locationIdTo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LocationIdTo", DbType.Int32, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceiveList = Table.AsEnumerable().Select(r => new PMProductReceivedReportViewBO
                    {
                        Quantity = r.Field<decimal>("Quantity"),
                        SerialNumber = r.Field<string>("SerialNumber"),
                        AverageCost = r.Field<decimal>("AverageCost"),
                        ProductName = r.Field<string>("ProductName"),
                        UserName = r.Field<string>("UserName"),
                        PONumber = r.Field<string>("PONumber"),
                        ReceivedDate = r.Field<string>("ReceivedDate"),
                        CreatedBy = r.Field<int>("CreatedBy"),
                        CreatedDate = r.Field<DateTime>("CreatedDate"),
                        UnitHead = r.Field<string>("UnitHead"),
                        CostCenterFrom = r.Field<string>("CostCenterFrom"),
                        CostCenterIdFrom = r.Field<int?>("CostCenterIdFrom"),
                        CostCenterIdTo = r.Field<int?>("CostCenterIdTo"),
                        CostCenterTo = r.Field<string>("CostCenterTo"),
                        LocationNameFrom = r.Field<string>("LocationNameFrom"),
                        LocationNameTo = r.Field<string>("LocationNameTo"),
                        Category = r.Field<string>("Category"),
                        Remarks = r.Field<string>("Remarks"),
                        IssueType = r.Field<string>("IssueType")

                    }).ToList();
                }
            }

            return productReceiveList;
        }
        public List<PMProductReceivedBillPaymentBO> GetProductreceiveBillPaymentInfo(int receivedId)
        {
            List<PMProductReceivedBillPaymentBO> billList = new List<PMProductReceivedBillPaymentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMProductReceivedBillPaymentInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReceivedId", DbType.Int32, receivedId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceivedBillPayment");
                    DataTable Table = ds.Tables["PMProductReceivedBillPayment"];

                    billList = Table.AsEnumerable().Select(r => new PMProductReceivedBillPaymentBO
                    {
                        ReceivedId = r.Field<Int32>("ReceivedId"),
                        POrderId = r.Field<Int32>("POrderId"),
                        PaymentId = r.Field<Int32>("PaymentId"),
                        PaymentMode = r.Field<string>("PaymentMode"),
                        PaymentAmount = r.Field<decimal>("PaymentAmount")
                    }).ToList();
                }
            }
            return billList;
        }
        public PMProductReceivedDetailsBO GetPOrderWiseProductStockById(int pOrderId, int productId)
        {
            PMProductReceivedDetailsBO productReceive = new PMProductReceivedDetailsBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPOrderWiseProductStockById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@POrderId", DbType.Int32, pOrderId);
                    dbSmartAspects.AddInParameter(cmd, "@ProductId", DbType.Int32, productId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];


                    productReceive = Table.AsEnumerable().Select(r => new PMProductReceivedDetailsBO
                    {
                        ProductId = r.Field<Int32>("ProductId"),
                        StockById = r.Field<Int32>("StockById")

                    }).FirstOrDefault();
                }
            }

            return productReceive;
        }

        public bool UpdateProductReceiveAccountsPostingApproval(List<int> receiveId)
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
                        if (receiveId.Count > 0)
                        {
                            using (DbCommand cmdReceiveDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductReceiveAccountsPostingApproval_SP"))
                            {
                                foreach (int rd in receiveId)
                                {
                                    cmdReceiveDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ReceivedId", DbType.Int32, rd);
                                    status = dbSmartAspects.ExecuteNonQuery(cmdReceiveDetails, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;
                        transction.Rollback();
                        throw ex;
                    }
                }
            }

            return retVal;
        }

        public List<PMProductReceivedReportBO> GetProductreceiveInfoForReport(int companyId, int projectId, DateTime? fromDate, DateTime? toDate, int categoryId, int productId,
                                                                                  int supplierId, string receiveNumber, string referenceNumber, string poNumber, int userInfoId)
        {
            List<PMProductReceivedReportBO> productReceiveList = new List<PMProductReceivedReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductReceiveInfoForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.String, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.String, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.String, DBNull.Value);

                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    if (categoryId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    if (productId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProductId", DbType.Int32, productId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProductId", DbType.Int32, DBNull.Value);

                    if (supplierId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(receiveNumber))
                        dbSmartAspects.AddInParameter(cmd, "@ReceiveNumber", DbType.String, receiveNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReceiveNumber", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(referenceNumber))
                        dbSmartAspects.AddInParameter(cmd, "@ReferenceNumber", DbType.String, referenceNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ReferenceNumber", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(poNumber))
                        dbSmartAspects.AddInParameter(cmd, "@PONumber", DbType.String, poNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PONumber", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceiveList = Table.AsEnumerable().Select(r => new PMProductReceivedReportBO
                    {
                        ReceivedId = r.Field<Int32>("ReceivedId"),
                        ReceivedDate = r.Field<DateTime>("ReceivedDate"),
                        ReceiveNumber = r.Field<string>("ReceiveNumber"),
                        ReferenceNumber = r.Field<string>("ReferenceNumber"),
                        ItemId = r.Field<int>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        CategoryId = r.Field<int>("CategoryId"),
                        CategoryName = r.Field<string>("CategoryName"),
                        StockById = r.Field<int>("StockById"),
                        HeadName = r.Field<string>("HeadName"),
                        PurchasePrice = r.Field<decimal>("PurchasePrice"),
                        Quantity = r.Field<decimal>("Quantity"),
                        SerialNumber = r.Field<string>("SerialNumber"),
                        TotalPrice = r.Field<decimal>("TotalPrice"),
                        SupplierId = r.Field<int>("SupplierId"),
                        SupplierName = r.Field<string>("SupplierName")

                    }).ToList();
                }
            }

            return productReceiveList;
        }


        public bool ReceiveOrderApproval(string receiveType, int receivedId, string approvedStatus, int porderId, int checkedOrApprovedBy, out string TransactionNo, out string TransactionType, out string ApproveStatus)
        {
            bool retVal = false;
            string OutStatus = string.Empty;
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("ReceiveOrderApproval_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@ReceivedId", DbType.Int32, receivedId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, checkedOrApprovedBy);
                            dbSmartAspects.AddOutParameter(commandMaster, "@OutStatus", DbType.String, 50);

                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionNo", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionType", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ApproveStatus", DbType.String, 100);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            TransactionNo = commandMaster.Parameters["@TransactionNo"].Value.ToString();
                            TransactionType = commandMaster.Parameters["@TransactionType"].Value.ToString();
                            ApproveStatus = commandMaster.Parameters["@ApproveStatus"].Value.ToString();

                            
                            OutStatus = Convert.ToString(commandMaster.Parameters["@OutStatus"].Value);
                        }

                        if (status > 0 && receiveType != "AdHoc" && OutStatus == "Approved")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReceiveStatusForPurchaseWiseReceivee_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@ReceivedId", DbType.Int32, receivedId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ReceiveType", DbType.String, receiveType);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, approvedStatus);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }

                        if (porderId > 0 && status > 0 && OutStatus == "Approved")
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateProductReceiveStatusNItemStockNAverageCost_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@ReceivedId", DbType.Int32, receivedId);
                                dbSmartAspects.AddInParameter(command, "@Status", DbType.String, approvedStatus);
                                dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, checkedOrApprovedBy);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }
                        else if (status > 0 && OutStatus == "Approved")
                        {
                            // // // ------Update Product Adhoc Receive Status and Item Stock and Average Cost
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateProductAdhocReceiveStatusNItemStockNAverageCost_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@ReceivedId", DbType.Int32, receivedId);
                                dbSmartAspects.AddInParameter(command, "@Status", DbType.String, approvedStatus);
                                dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, checkedOrApprovedBy);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }                            
                        }

                        if (status > 0 && OutStatus == "Approved")
                        {
                            // // // ------Product Receive Accounts Posting Process
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ProductReceiveAccountsPostingProcess_SP"))
                            {
                                command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                                dbSmartAspects.AddInParameter(command, "@DayClossingDate", DbType.DateTime, DateTime.Now);
                                dbSmartAspects.AddInParameter(command, "@ReceivedIdList", DbType.String, receivedId.ToString());
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, checkedOrApprovedBy);
                                dbSmartAspects.AddOutParameter(command, "@mErr", DbType.Int32, sizeof(Int32));
                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
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

        public bool ReceiveOrderDelete(string receiveType, int receivedId, string approvedStatus, int createdBy, int lastModifiedBy, out string TransactionNo, out string TransactionType, out string ApproveStatus)
        {
            bool retVal = false;
            int status = 0;
            TransactionNo = "";
            TransactionType = "";
            ApproveStatus = "";

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        if (receiveType != "AdHoc")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdatePOStatusForPurchaseWiseReceiveDelete_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@ReceivedId", DbType.Int32, receivedId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ReceiveType", DbType.String, receiveType);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }

                            if (status > 0)
                            {
                                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("ReceiveOrderDelete_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandMaster, "@ReceiveType", DbType.String, receiveType);
                                    dbSmartAspects.AddInParameter(commandMaster, "@ReceivedId", DbType.Int32, receivedId);
                                    dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                                    dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, createdBy);
                                    dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                                    dbSmartAspects.AddOutParameter(commandMaster, "@TransactionNo", DbType.String, 100);
                                    dbSmartAspects.AddOutParameter(commandMaster, "@TransactionType", DbType.String, 100);
                                    dbSmartAspects.AddOutParameter(commandMaster, "@ApproveStatus", DbType.String, 100);

                                    

                                    status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                                    TransactionNo = commandMaster.Parameters["@TransactionNo"].Value.ToString();
                                    TransactionType = commandMaster.Parameters["@TransactionType"].Value.ToString();
                                    ApproveStatus = commandMaster.Parameters["@ApproveStatus"].Value.ToString();
                                }
                            }
                        }
                        else
                        {
                            using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("ReceiveOrderDelete_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@ReceiveType", DbType.String, receiveType);
                                dbSmartAspects.AddInParameter(commandMaster, "@ReceivedId", DbType.Int32, receivedId);
                                dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                                dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, createdBy);
                                dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

                                dbSmartAspects.AddOutParameter(commandMaster, "@TransactionNo", DbType.String, 100);
                                dbSmartAspects.AddOutParameter(commandMaster, "@TransactionType", DbType.String, 100);
                                dbSmartAspects.AddOutParameter(commandMaster, "@ApproveStatus", DbType.String, 100);

                                status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                                TransactionNo = commandMaster.Parameters["@TransactionNo"].Value.ToString();
                                TransactionType = commandMaster.Parameters["@TransactionType"].Value.ToString();
                                ApproveStatus = commandMaster.Parameters["@ApproveStatus"].Value.ToString();
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

        public List<SerialDuplicateBO> DuplicateSerialCheck(string serialNumber)
        {

            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                try
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("DuplicateSerialCheck_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SerialNumber", DbType.String, serialNumber);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                        DataTable Table = ds.Tables["PMProductReceived"];

                        serial = Table.AsEnumerable().Select(r => new SerialDuplicateBO
                        {
                            ItemName = r.Field<string>("ItemName"),
                            SerialNumber = r.Field<string>("SerialNumber")

                        }).ToList();
                    }


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return serial;
        }

    }
}
