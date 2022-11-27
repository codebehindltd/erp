using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using HotelManagement.Entity.Inventory;
using System.Data;
using HotelManagement.Data.Inventory;
using HotelManagement.Entity.PurchaseManagment;

namespace HotelManagement.Data.PurchaseManagment
{
    public class PMProductOutDA : BaseService
    {
        public bool SaveProductOutInfo(PMProductOutBO ProductOut, List<PMProductOutDetailsBO> AddedOutDetails,
                                       List<PMProductOutSerialInfoBO> ItemSerialDetails, int isProductOutReceiveApproval, out int outId)
        {
            int status = 0;
            outId = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("SaveProductOutInfo_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@OutDate", DbType.DateTime, ProductOut.OutDate);
                            dbSmartAspects.AddInParameter(commandOut, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);
                            dbSmartAspects.AddInParameter(commandOut, "@RequisitionOrSalesId", DbType.Int32, ProductOut.RequisitionOrSalesId);
                            dbSmartAspects.AddInParameter(commandOut, "@Status", DbType.String, ProductOut.Status);

                            if (ProductOut.OutFor != 0)
                                dbSmartAspects.AddInParameter(commandOut, "@OutFor", DbType.Int32, ProductOut.OutFor);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@OutFor", DbType.Int32, DBNull.Value);

                            if (!string.IsNullOrEmpty(ProductOut.IssueType))
                                dbSmartAspects.AddInParameter(commandOut, "@IssueType", DbType.String, ProductOut.IssueType);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@IssueType", DbType.String, DBNull.Value);

                            if (ProductOut.ToCostCenterId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@ToCostCenterId", DbType.Int64, ProductOut.ToCostCenterId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@ToCostCenterId", DbType.Int64, DBNull.Value);

                            if (ProductOut.ToLocationId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@ToLocationId", DbType.Int64, ProductOut.ToLocationId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@ToLocationId", DbType.Int64, DBNull.Value);

                            if (ProductOut.FromCostCenterId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int64, ProductOut.FromCostCenterId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int64, DBNull.Value);

                            if (ProductOut.FromLocationId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int64, ProductOut.FromLocationId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int64, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, ProductOut.Remarks);
                            dbSmartAspects.AddInParameter(commandOut, "@CreatedBy", DbType.Int32, ProductOut.CreatedBy);
                            dbSmartAspects.AddInParameter(commandOut, "@CheckedBy", DbType.Int32, ProductOut.CheckedBy);
                            dbSmartAspects.AddInParameter(commandOut, "@ApprovedBy", DbType.Int32, ProductOut.ApprovedBy);

                            if (ProductOut.AccountPostingHeadId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@AccountPostingHeadId", DbType.Int64, ProductOut.AccountPostingHeadId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@AccountPostingHeadId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddOutParameter(commandOut, "@OutId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);

                            outId = Convert.ToInt32(commandOut.Parameters["@OutId"].Value);
                        }

                        if (status > 0 && AddedOutDetails.Count > 0)
                        {
                            foreach (PMProductOutDetailsBO pout in AddedOutDetails)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveProductOutDetailsInfo_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, outId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@RequisitionOrSalesId", DbType.Int32, ProductOut.RequisitionOrSalesId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@CostCenterIdFrom", DbType.Int32, pout.CostCenterIdFrom);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationIdFrom", DbType.Int32, pout.LocationIdFrom);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@CostCenterIdTo", DbType.Int32, pout.CostCenterIdTo);

                                    if (ProductOut.ProductOutFor != "Sales")
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationIdTo", DbType.Int32, pout.LocationIdTo);
                                    else
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationIdTo", DbType.Int32, DBNull.Value);

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductId", DbType.Int32, pout.ProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SerialNumber", DbType.String, pout.SerialNumber);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && ItemSerialDetails.Count > 0)
                        {
                            using (DbCommand cmdReceiveDetails = dbSmartAspects.GetStoredProcCommand("SaveOutSerialInfo_SP"))
                            {
                                foreach (PMProductOutSerialInfoBO rd in ItemSerialDetails)
                                {
                                    cmdReceiveDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@OutId", DbType.Int32, outId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@SerialNumber", DbType.String, rd.SerialNumber);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@CreatedBy", DbType.Int32, ProductOut.CreatedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdReceiveDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && ProductOut.Status == "Approved" && isProductOutReceiveApproval == 0)
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductOutStatusNItemStock_SP"))
                            {
                                cmdOutDetails.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, outId);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@Status", DbType.String, ProductOut.Status);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@LastModifiedBy", DbType.Int32, ProductOut.CreatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                            }
                        }
                        else if (status > 0 && ProductOut.Status == "Approved" && isProductOutReceiveApproval == 1)
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductOutStatusForReceiveApproval_SP"))
                            {
                                cmdOutDetails.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, outId);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@Status", DbType.String, ProductOut.Status);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@LastModifiedBy", DbType.Int32, ProductOut.CreatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                            }
                        }

                        //if (status > 0 && ProductOut.ProductOutFor == "Requisition")
                        //{
                        //    string query = string.Empty;
                        //    query += "UPDATE PMRequisition " +
                        //             "SET DelivaredStatus = dbo.FnGetInvRequisitionItemDeliverStatus(" + outId + ", NULL, NULL, 'RequisitionStatus') " +
                        //             "WHERE RequisitionId = " + ProductOut.RequisitionOrSalesId;

                        //    using (DbCommand cmdUpdateRequisition = dbSmartAspects.GetSqlStringCommand(query))
                        //    {
                        //        status = dbSmartAspects.ExecuteNonQuery(cmdUpdateRequisition, transction);
                        //    }
                        //}

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
        public bool UpdateProductOutInfo(PMProductOutBO ProductOut, List<PMProductOutDetailsBO> AddedOutDetails, List<PMProductOutDetailsBO> EditedOutDetails, List<PMProductOutDetailsBO> DeletedOutDetails)
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
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("UpdateProductOutInfo_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@OutId", DbType.Int32, ProductOut.OutId);

                            if (!string.IsNullOrEmpty(ProductOut.IssueType))
                                dbSmartAspects.AddInParameter(commandOut, "@IssueType", DbType.String, ProductOut.IssueType);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@IssueType", DbType.String, DBNull.Value);

                            if (ProductOut.ToCostCenterId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@ToCostCenterId", DbType.Int64, ProductOut.ToCostCenterId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@ToCostCenterId", DbType.Int64, DBNull.Value);

                            if (ProductOut.ToLocationId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@ToLocationId", DbType.Int64, ProductOut.ToLocationId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@ToLocationId", DbType.Int64, DBNull.Value);

                            if (ProductOut.FromCostCenterId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int64, ProductOut.FromCostCenterId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int64, DBNull.Value);

                            if (ProductOut.FromLocationId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int64, ProductOut.FromLocationId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int64, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);
                            dbSmartAspects.AddInParameter(commandOut, "@OutDate", DbType.DateTime, DateTime.Now);
                            dbSmartAspects.AddInParameter(commandOut, "@RequisitionOrSalesId", DbType.Int32, ProductOut.RequisitionOrSalesId);

                            if (ProductOut.OutFor != 0)
                                dbSmartAspects.AddInParameter(commandOut, "@OutFor", DbType.Int32, ProductOut.OutFor);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@OutFor", DbType.Int32, DBNull.Value);

                            if (ProductOut.AccountPostingHeadId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@AccountPostingHeadId", DbType.Int64, ProductOut.AccountPostingHeadId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@AccountPostingHeadId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, ProductOut.Remarks);
                            dbSmartAspects.AddInParameter(commandOut, "@LastModifiedBy", DbType.Int32, ProductOut.LastModifiedBy);
                            dbSmartAspects.AddInParameter(commandOut, "@CheckedBy", DbType.Int32, ProductOut.CheckedBy);
                            dbSmartAspects.AddInParameter(commandOut, "@ApprovedBy", DbType.Int32, ProductOut.ApprovedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);

                        }

                        if (status > 0 && AddedOutDetails.Count > 0)
                        {
                            foreach (PMProductOutDetailsBO pout in AddedOutDetails)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveProductOutDetailsInfo_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, ProductOut.OutId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@RequisitionOrSalesId", DbType.Int32, ProductOut.RequisitionOrSalesId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@CostCenterIdFrom", DbType.Int32, pout.CostCenterIdFrom);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationIdFrom", DbType.Int32, pout.LocationIdFrom);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@CostCenterIdTo", DbType.Int32, pout.CostCenterIdTo);

                                    if (ProductOut.ProductOutFor != "Sales")
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationIdTo", DbType.Int32, pout.LocationIdTo);
                                    else
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationIdTo", DbType.Int32, DBNull.Value);

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductId", DbType.Int32, pout.ProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SerialNumber", DbType.String, pout.SerialNumber);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && EditedOutDetails.Count > 0)
                        {
                            foreach (PMProductOutDetailsBO pout in EditedOutDetails)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductOutDetailsInfo_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutDetailsId", DbType.Int32, pout.OutDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, ProductOut.OutId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@RequisitionOrSalesId", DbType.Int32, ProductOut.RequisitionOrSalesId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@CostCenterIdFrom", DbType.Int32, ProductOut.FromCostCenterId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationIdFrom", DbType.Int32, ProductOut.FromLocationId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@CostCenterIdTo", DbType.Int32, ProductOut.ToCostCenterId);

                                    if (ProductOut.ProductOutFor != "Sales")
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationIdTo", DbType.Int32, ProductOut.ToLocationId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationIdTo", DbType.Int32, ProductOut.ToLocationId);

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductId", DbType.Int32, pout.ProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, pout.ColorId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, pout.SizeId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, pout.StyleId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@SerialNumber", DbType.String, pout.SerialNumber);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedOutDetails.Count > 0)
                        {
                            foreach (PMProductOutDetailsBO pout in DeletedOutDetails)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("DeleteProductOutDetailsInfo_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutDetailsId", DbType.Int32, pout.OutDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, ProductOut.OutId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && ProductOut.ProductOutFor == "Requisition")
                        {
                            string query = string.Empty;
                            query += "UPDATE PMRequisition " +
                                     "SET DelivaredStatus = dbo.FnGetInvRequisitionItemDeliverStatus(" + ProductOut.OutId + ", NULL, NULL, 'RequisitionStatus') " +
                                     "WHERE RequisitionId = " + ProductOut.RequisitionOrSalesId;

                            using (DbCommand cmdUpdateRequisition = dbSmartAspects.GetSqlStringCommand(query))
                            {
                                status = dbSmartAspects.ExecuteNonQuery(cmdUpdateRequisition, transction);
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
        //public bool SaveSupplierProductReturnInfo(PMSupplierProductReturnBO ProductReturn, List<PMProductReturnDetailsBO> AddedProductReturnDetails)
        //{
        //    int status = 0, returnId = 0;
        //    bool retVal = false;

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        conn.Open();

        //        using (DbTransaction transction = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("SaveSupplierProductReturnInfo_SP"))
        //                {
        //                    commandOut.Parameters.Clear();
        //                    dbSmartAspects.AddInParameter(commandOut, "@ReceivedId", DbType.Int32, ProductReturn.ReceivedId);
        //                    dbSmartAspects.AddInParameter(commandOut, "@POrderId", DbType.Int32, ProductReturn.POrderId);
        //                    dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, ProductReturn.Remarks);
        //                    dbSmartAspects.AddInParameter(commandOut, "@CreatedBy", DbType.Int32, ProductReturn.CreatedBy);

        //                    dbSmartAspects.AddOutParameter(commandOut, "@ReturnId", DbType.Int32, sizeof(Int32));

        //                    status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);

        //                    returnId = Convert.ToInt32(commandOut.Parameters["@ReturnId"].Value);
        //                }

        //                if (status > 0 && AddedProductReturnDetails.Count > 0)
        //                {
        //                    foreach (PMProductReturnDetailsBO pout in AddedProductReturnDetails)
        //                    {
        //                        using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveSupplierProductReturnDetailsInfo_SP"))
        //                        {
        //                            cmdOutDetails.Parameters.Clear();

        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnId", DbType.Int32, returnId);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@CostCenterIdFrom", DbType.Int32, pout.CostCenterIdFrom);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationIdFrom", DbType.Int32, pout.LocationIdFrom);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductId", DbType.Int32, pout.ProductId);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

        //                            status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
        //                        }
        //                    }
        //                }

        //                if (status > 0 && ProductReturn.Status == "Approved")
        //                {
        //                    using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductOutStatusNItemStock_SP"))
        //                    {
        //                        cmdOutDetails.Parameters.Clear();

        //                        dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, returnId);
        //                        //dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductOutFor", DbType.String, ProductReturn.ProductOutFor);
        //                        dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductOutFor", DbType.String, "");
        //                        dbSmartAspects.AddInParameter(cmdOutDetails, "@Status", DbType.String, ProductReturn.Status);
        //                        dbSmartAspects.AddInParameter(cmdOutDetails, "@LastModifiedBy", DbType.Int32, ProductReturn.CreatedBy);

        //                        status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
        //                    }
        //                }

        //                if (status > 0)
        //                {
        //                    retVal = true;
        //                    transction.Commit();
        //                }
        //                else
        //                {
        //                    retVal = false;
        //                    transction.Rollback();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                retVal = false;
        //                transction.Rollback();
        //                throw ex;
        //            }
        //        }
        //    }

        //    return retVal;
        //}
        //public bool UpdateSupplierProductReturnInfo(PMSupplierProductReturnBO ProductReturn, List<PMProductReturnDetailsBO> AddedProductReturnDetails, List<PMProductReturnDetailsBO> EditedProductReturnDetails, List<PMProductReturnDetailsBO> DeletedProductReturnDetails)
        //{
        //    int status = 0;
        //    bool retVal = false;

        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        conn.Open();

        //        using (DbTransaction transction = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("UpdateSupplierProductReturnInfo_SP"))
        //                {
        //                    dbSmartAspects.AddInParameter(commandOut, "@ReturnId", DbType.Int32, ProductReturn.ReturnId);
        //                    dbSmartAspects.AddInParameter(commandOut, "@ReceivedId", DbType.Int32, ProductReturn.ReceivedId);
        //                    dbSmartAspects.AddInParameter(commandOut, "@POrderId", DbType.Int32, ProductReturn.POrderId);
        //                    dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, ProductReturn.Remarks);
        //                    dbSmartAspects.AddInParameter(commandOut, "@ModifiedBy", DbType.Int32, ProductReturn.LastModifiedBy);

        //                    status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);
        //                }

        //                if (status > 0 && AddedProductReturnDetails.Count > 0)
        //                {
        //                    foreach (PMProductReturnDetailsBO pout in AddedProductReturnDetails)
        //                    {
        //                        using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveSupplierProductReturnDetailsInfo_SP"))
        //                        {
        //                            cmdOutDetails.Parameters.Clear();

        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnId", DbType.Int32, pout.ReturnId);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@CostCenterIdFrom", DbType.Int32, pout.CostCenterIdFrom);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationIdFrom", DbType.Int32, pout.LocationIdFrom);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductId", DbType.Int32, pout.ProductId);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

        //                            status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
        //                        }
        //                    }
        //                }

        //                if (status > 0 && EditedProductReturnDetails.Count > 0)
        //                {
        //                    foreach (PMProductReturnDetailsBO pout in EditedProductReturnDetails)
        //                    {
        //                        using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateSupplierProductReturnDetailsInfo_SP"))
        //                        {
        //                            cmdOutDetails.Parameters.Clear();

        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnDetailsId", DbType.Int32, pout.ReturnDetailsId);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@ReturnId", DbType.Int32, pout.ReturnId);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@CostCenterIdFrom", DbType.Int32, pout.CostCenterIdFrom);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationIdFrom", DbType.Int32, pout.LocationIdFrom);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductId", DbType.Int32, pout.ProductId);
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

        //                            status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
        //                        }
        //                    }
        //                }

        //                if (status > 0 && DeletedProductReturnDetails.Count > 0)
        //                {
        //                    foreach (PMProductReturnDetailsBO pout in DeletedProductReturnDetails)
        //                    {
        //                        using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
        //                        {
        //                            cmdOutDetails.Parameters.Clear();

        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@TableName", DbType.String, "PMSupplierProductReturnDetails");
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@TablePKField", DbType.String, "ReturnDetailsId");
        //                            dbSmartAspects.AddInParameter(cmdOutDetails, "@TablePKId", DbType.String, pout.ReturnDetailsId);

        //                            status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
        //                        }
        //                    }
        //                }

        //                //if (status > 0 && ProductOut.ProductOutFor == "Requisition")
        //                //{
        //                //    string query = string.Empty;
        //                //    query += "UPDATE PMRequisition " +
        //                //             "SET DelivaredStatus = dbo.FnGetInvRequisitionItemDeliverStatus(" + ProductOut.OutId + ", NULL, NULL, 'RequisitionStatus') " +
        //                //             "WHERE RequisitionId = " + ProductOut.RequisitionOrSalesId;

        //                //    using (DbCommand cmdUpdateRequisition = dbSmartAspects.GetSqlStringCommand(query))
        //                //    {
        //                //        status = dbSmartAspects.ExecuteNonQuery(cmdUpdateRequisition, transction);
        //                //    }
        //                //}

        //                if (status > 0)
        //                {
        //                    retVal = true;
        //                    transction.Commit();
        //                }
        //                else
        //                {
        //                    retVal = false;
        //                    transction.Rollback();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                retVal = false;
        //                transction.Rollback();
        //                throw ex;
        //            }
        //        }
        //    }

        //    return retVal;
        //}
        public bool DeleteProductOutInfo(int outId)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteProductOutInfo_SP"))
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@OutId", DbType.Int32, outId);
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
        public bool DeleteProductOutInfo_New(int outId, string approvedStatus, int createdBy, int lastModifiedBy)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteProductOutInfoNew_SP"))
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@OutId", DbType.Int32, outId);
                            dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, createdBy);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, lastModifiedBy);
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
        public bool DeleteProductOutNotCheckedInfo(int outId)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteProductOutInfoIfNotChecked_SP"))
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@OutId", DbType.Int32, outId);
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

        public bool DeleteSupplierProductReturnInfo(int returnId)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteSupplierProductReturnInfo_SP"))
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@ReturnId", DbType.Int32, returnId);
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
        public List<PMProductOutBO> GetProductDirectOutForSearch(string productOutFor, string issueType, string issueNumber,
                            DateTime? dateFrom, DateTime? dateTo, string status, int userInfoId,
                            int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<PMProductOutBO> productReceive = new List<PMProductOutBO>();
            totalRecords = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductConsumptionForSearch_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (!string.IsNullOrEmpty(productOutFor))
                        dbSmartAspects.AddInParameter(cmd, "@ProductOutFor", DbType.String, productOutFor);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProductOutFor", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(issueType))
                        dbSmartAspects.AddInParameter(cmd, "@IssueType", DbType.String, issueType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IssueType", DbType.String, DBNull.Value);

                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.Int32, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(issueNumber))
                        dbSmartAspects.AddInParameter(cmd, "@IssueNumber", DbType.String, issueNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IssueNumber", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(status))
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

                    productReceive = Table.AsEnumerable().Select(r => new PMProductOutBO
                    {
                        OutId = r.Field<Int32>("OutId"),
                        ProductOutFor = r.Field<string>("ProductOutFor"),
                        OutDate = r.Field<DateTime>("OutDate"),
                        RequisitionOrSalesId = r.Field<Int32>("RequisitionOrSalesId"),
                        FromCostCenterId = r.Field<Int32?>("FromCostCenterId"),
                        FromLocationId = r.Field<Int32?>("FromLocationId"),
                        ToCostCenterId = r.Field<Int32?>("ToCostCenterId"),
                        ToLocationId = r.Field<Int32?>("ToLocationId"),
                        FromCostCenter = r.Field<string>("FromCostCenter"),
                        ToCostCenter = r.Field<string>("ToCostCenter"),
                        OutFor = r.Field<Int32?>("OutFor"),
                        IssueType = r.Field<string>("IssueType"),
                        CreatedByName = r.Field<string>("CreatedByName"),

                        Remarks = r.Field<string>("Remarks"),
                        IssueNumber = r.Field<string>("IssueNumber"),
                        Status = r.Field<string>("Status"),

                        IsCanEdit = r.Field<bool>("IsCanEdit"),
                        IsCanDelete = r.Field<bool>("IsCanDelete"),
                        IsCanChecked = r.Field<bool>("IsCanChecked"),
                        IsCanApproved = r.Field<bool>("IsCanApproved")

                    }).ToList();

                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }

            return productReceive;
        }
        public List<PMProductViewBO> GetProductOutByStatus(DateTime? dateFrom, DateTime? dateTo, string status, int userInfoId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<PMProductViewBO> productOut = new List<PMProductViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductOutBySearchCriteria_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, dateTo);
                    dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, status);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productOut = Table.AsEnumerable().Select(r => new PMProductViewBO
                    {
                        OutId = r.Field<Int32>("OutId"),
                        ProductOutFor = r.Field<string>("ProductOutFor"),
                        OutDate = r.Field<DateTime>("OutDate"),
                        RequisitionOrSalesId = r.Field<Int32>("RequisitionOrSalesId"),
                        FromCostCenterId = r.Field<Int32?>("FromCostCenterId"),
                        FromLocationId = r.Field<Int32?>("FromLocationId"),
                        ToCostCenterId = r.Field<Int32?>("ToCostCenterId"),
                        ToLocationId = r.Field<Int32?>("ToLocationId"),
                        //StockById = r.Field<Int32>("StockById"),
                        //SalesId = r.Field<Int32>("SalesId"),
                        //ProductId = r.Field<Int32>("ProductId"),
                        //Quantity = r.Field<decimal>("Quantity"),
                        OutFor = r.Field<Int32?>("OutFor"),
                        IssueType = r.Field<string>("IssueType"),
                        //IssueForId = r.Field<Int64?>("IssueForId"),

                        Remarks = r.Field<string>("Remarks"),
                        RequisitionOrSalesNumber = r.Field<string>("RequisitionOrSalesNumber"),
                        // DelivaredStatus = r.Field<string>("DelivaredStatus"),
                        IssueNumber = r.Field<string>("IssueNumber"),
                        IsCanEdit = r.Field<Boolean>("IsCanEdit"),
                        IsCanDelete = r.Field<Boolean>("IsCanDelete"),
                        IsCanChecked = r.Field<Boolean>("IsCanChecked"),
                        IsCanApproved = r.Field<Boolean>("IsCanApproved"),

                        //ProductName = r.Field<string>("ItemName"),
                        //CostCenterFrom = r.Field<string>("CostCenterFrom"),
                        //CostCenterTo = r.Field<string>("CostCenterTo"),
                        //LocationFrom = r.Field<string>("LocationFrom"),
                        //LocationTo = r.Field<string>("LocationTo"),
                        //StockBy = r.Field<string>("StockBy")
                        Status = r.Field<string>("Status"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CheckedBy = r.Field<Int32?>("CheckedBy"),
                        ApprovedBy = r.Field<Int32?>("ApprovedBy"),

                    }).ToList();

                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }

            return productOut;
        }
        public List<PMProductOutBO> GetSupplierProductReturnForSearch(int costCenterIdFrom, int locationIdFrom, DateTime? dateFrom, DateTime? dateTo)
        {
            List<PMProductOutBO> productReceive = new List<PMProductOutBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierProductReturnForSearch_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (costCenterIdFrom != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterIdFrom", DbType.Int32, costCenterIdFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterIdFrom", DbType.Int32, DBNull.Value);

                    if (locationIdFrom != 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationIdFrom", DbType.Int32, locationIdFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationIdFrom", DbType.Int32, DBNull.Value);

                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.Int32, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductOutBO
                    {
                        ReturnId = r.Field<Int32>("ReturnId"),
                        ReturnDate = r.Field<DateTime>("ReturnDate"),
                        StringReturnDate = r.Field<string>("StringReturnDate"),
                        Remarks = r.Field<string>("Remarks"),
                        Status = r.Field<string>("Status")
                    }).ToList();
                }
            }

            return productReceive;
        }
        public PMProductOutBO GetProductOutById(int outId)
        {
            PMProductOutBO productReceive = new PMProductOutBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductOutById_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@OutId", DbType.Int32, outId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductOutBO
                    {
                        OutId = r.Field<Int32>("OutId"),
                        ProductOutFor = r.Field<string>("ProductOutFor"),
                        OutDate = r.Field<DateTime>("OutDate"),
                        RequisitionOrSalesId = r.Field<Int32>("RequisitionOrSalesId"),
                        FromCostCenterId = r.Field<Int32?>("FromCostCenterId"),
                        FromLocationId = r.Field<Int32?>("FromLocationId"),
                        ToCostCenterId = r.Field<Int32?>("ToCostCenterId"),
                        ToLocationId = r.Field<Int32?>("ToLocationId"),
                        OutFor = r.Field<Int32?>("OutFor"),
                        IssueType = r.Field<string>("IssueType"),
                        IssueNumber = r.Field<string>("IssueNumber"),
                        AccountPostingHeadId = r.Field<Int64?>("AccountPostingHeadId"),
                        Remarks = r.Field<string>("Remarks"),
                        RequisitionOrSalesNumber = r.Field<string>("RequisitionOrSalesNumber"),
                        Status = r.Field<string>("Status"),
                        CreatedBy = r.Field<int>("CreatedBy"),
                        GLCompanyId =  r.Field<int>("GLCompanyId"),
                        GLProjectId =  r.Field<int>("GLProjectId"),
                        ToGLCompanyId = r.Field<int>("ToGLCompanyId"),
                        ToGLProjectId = r.Field<int>("ToGLProjectId"),
                        BillNo = r.Field<string>("BillNo"),
                        TransferFor = r.Field<string>("TransferFor")
                    }).FirstOrDefault();
                }
            }

            return productReceive;
        }
        public PMProductOutBO GetProductOutByIssueNumber(string issueNumber)
        {
            PMProductOutBO PMProductOut = new PMProductOutBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductOutByIssueNumber_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@IssueNumber", DbType.String, issueNumber);
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];
                    PMProductOut = Table.AsEnumerable().Select(r => new PMProductOutBO()
                    {
                        OutId = r.Field<Int32>("OutId"),
                        ProductOutFor = r.Field<string>("ProductOutFor"),
                        OutDate = r.Field<DateTime>("OutDate"),
                        RequisitionOrSalesId = r.Field<Int32>("RequisitionOrSalesId"),
                        OutFor = r.Field<Int32?>("OutFor"),
                        IssueType = r.Field<string>("IssueType"),
                        FromCostCenterId = r.Field<Int32?>("FromCostCenterId"),
                        FromLocationId = r.Field<Int32?>("FromLocationId"),
                        ToCostCenterId = r.Field<Int32?>("ToCostCenterId"),
                        ToLocationId = r.Field<Int32?>("ToLocationId"),
                        //IssueForId = r.Field<Int64?>("IssueForId"),
                        Remarks = r.Field<string>("Remarks"),

                        Status = r.Field<string>("Status"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        //CreatedDate = r.Field<string>("CreatedDate"),
                        //LastModifiedBy = r.Field<Int32>("LastModifiedBy"),
                        //LastModifiedDate = r.Field<string>("LastModifiedDate"),
                        IssueNumber = r.Field<string>("IssueNumber"),
                        AccountPostingHeadId = r.Field<Int64?>("AccountPostingHeadId"),
                    }).SingleOrDefault();

                }
            }
            return PMProductOut;
        }

        public PMSupplierProductReturnBO GetSupplierProductReturnById(int returnId)
        {
            PMSupplierProductReturnBO productReceive = new PMSupplierProductReturnBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSupplierProductReturnById_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@ReturnId", DbType.Int32, returnId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMSupplierProductReturnBO
                    {
                        ReturnId = r.Field<Int32>("ReturnId"),
                        ReturnDate = r.Field<DateTime>("ReturnDate"),
                        POrderId = r.Field<Int32?>("POrderId"),
                        Remarks = r.Field<string>("Remarks")
                    }).FirstOrDefault();
                }
            }

            return productReceive;
        }
        public List<PMProductOutDetailsBO> GetProductOutDetailsById(int outId)
        {
            List<PMProductOutDetailsBO> productOutDetails = new List<PMProductOutDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductOutDetailsByOutId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@OutId", DbType.Int32, outId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMProductOutDetailsBO pMProductOutDetails = new PMProductOutDetailsBO();

                                pMProductOutDetails.OutDetailsId = Convert.ToInt32(reader["OutDetailsId"]);
                                pMProductOutDetails.OutId = Convert.ToInt32(reader["OutId"]);
                                pMProductOutDetails.OutDateString = reader["OutDateString"].ToString();
                                pMProductOutDetails.IssueNumber = Convert.ToString(reader["IssueNumber"]);
                                if (reader["FromCostCenterId"] != DBNull.Value)
                                    pMProductOutDetails.CostCenterIdFrom = Convert.ToInt32(reader["FromCostCenterId"]);
                                if (reader["FromLocationId"] != DBNull.Value)
                                    pMProductOutDetails.LocationIdFrom = Convert.ToInt32(reader["FromLocationId"]);
                                pMProductOutDetails.CostCenterIdTo = Convert.ToInt32(reader["ToCostCenterId"]);
                                pMProductOutDetails.LocationIdTo = Convert.ToInt32(reader["ToLocationId"]);
                                pMProductOutDetails.StockById = Convert.ToInt32(reader["StockById"]);
                                pMProductOutDetails.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                pMProductOutDetails.ProductId = Convert.ToInt32(reader["ItemId"]);
                                pMProductOutDetails.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                pMProductOutDetails.SerialNumber = reader["SerialNumber"].ToString();
                                pMProductOutDetails.AverageCost = Convert.ToDecimal(reader["AverageCost"]);
                                pMProductOutDetails.ItemName = Convert.ToString(reader["ItemName"]);
                                pMProductOutDetails.CostCenterFrom = Convert.ToString(reader["CostCenterFrom"]);
                                pMProductOutDetails.CostCenterTo = Convert.ToString(reader["CostCenterTo"]);
                                pMProductOutDetails.LocationFrom = Convert.ToString(reader["LocationFrom"]);
                                pMProductOutDetails.LocationTo = Convert.ToString(reader["LocationTo"]);
                                pMProductOutDetails.StockBy = Convert.ToString(reader["StockBy"]);
                                pMProductOutDetails.CategoryName = Convert.ToString(reader["CategoryName"]);
                                pMProductOutDetails.Code = Convert.ToString(reader["Code"]);
                                pMProductOutDetails.UserName = Convert.ToString(reader["UserName"]);
                                pMProductOutDetails.ReferenceNumberText = Convert.ToString(reader["ReferenceNumberText"]);
                                pMProductOutDetails.ReferenceNumber = Convert.ToString(reader["ReferenceNumber"]);
                                pMProductOutDetails.ReferenceDate = Convert.ToString(reader["ReferenceDate"]);
                                pMProductOutDetails.ReferenceBy = Convert.ToString(reader["ReferenceBy"]);
                                if (reader["AdjustmentQuantity"] != DBNull.Value)
                                    pMProductOutDetails.AdjustmentQuantity = Convert.ToDecimal(reader["AdjustmentQuantity"]);
                                if (reader["AdjustmentStockById"] != DBNull.Value)
                                    pMProductOutDetails.AdjustmentStockById = Convert.ToInt32(reader["AdjustmentStockById"]);
                                pMProductOutDetails.ApprovalStatus = Convert.ToString(reader["ApprovalStatus"]);
                                pMProductOutDetails.ProductType = Convert.ToString(reader["ProductType"]);
                                pMProductOutDetails.TransferedByName = Convert.ToString(reader["TransferedByName"]);
                                pMProductOutDetails.CheckedByName = Convert.ToString(reader["CheckedByName"]);
                                pMProductOutDetails.ApprovedByName = Convert.ToString(reader["ApprovedByName"]);

                                pMProductOutDetails.FromCompanyId = Convert.ToInt32(reader["FromCompanyId"]);
                                pMProductOutDetails.FromCompany = Convert.ToString(reader["FromCompany"]);
                                pMProductOutDetails.FromProjectId = Convert.ToInt32(reader["FromProjectId"]);
                                pMProductOutDetails.FromProject = Convert.ToString(reader["FromProject"]);
                                pMProductOutDetails.ToCompanyId = Convert.ToInt32(reader["ToCompanyId"]);
                                pMProductOutDetails.ToCompany = Convert.ToString(reader["ToCompany"]);
                                pMProductOutDetails.ToProjectId = Convert.ToInt32(reader["ToProjectId"]);
                                pMProductOutDetails.ToProject = Convert.ToString(reader["ToProject"]);

                                productOutDetails.Add(pMProductOutDetails);
                            }
                        }
                    }
                }
            }

            return productOutDetails;
        }
        public List<PMProductOutDetailsBO> GetProductOutDetailsInfoByOutId(int outId)
        {
            List<PMProductOutDetailsBO> productOutDetails = new List<PMProductOutDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductOutDetailsInfoByOutId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@OutId", DbType.Int32, outId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMProductOutDetailsBO pMProductOutDetails = new PMProductOutDetailsBO();

                                pMProductOutDetails.OutDetailsId = Convert.ToInt32(reader["OutDetailsId"]);
                                pMProductOutDetails.OutId = Convert.ToInt32(reader["OutId"]);
                                pMProductOutDetails.OutDateString = reader["OutDateString"].ToString();
                                pMProductOutDetails.IssueNumber = Convert.ToString(reader["IssueNumber"]);
                                if (reader["FromCostCenterId"] != DBNull.Value)
                                    pMProductOutDetails.CostCenterIdFrom = Convert.ToInt32(reader["FromCostCenterId"]);
                                if (reader["FromLocationId"] != DBNull.Value)
                                    pMProductOutDetails.LocationIdFrom = Convert.ToInt32(reader["FromLocationId"]);
                                pMProductOutDetails.CostCenterIdTo = Convert.ToInt32(reader["ToCostCenterId"]);
                                pMProductOutDetails.LocationIdTo = Convert.ToInt32(reader["ToLocationId"]);
                                pMProductOutDetails.StockById = Convert.ToInt32(reader["StockById"]);
                                pMProductOutDetails.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                pMProductOutDetails.ProductId = Convert.ToInt32(reader["ItemId"]);
                                pMProductOutDetails.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                pMProductOutDetails.SerialNumber = reader["SerialNumber"].ToString();
                                pMProductOutDetails.AverageCost = Convert.ToDecimal(reader["AverageCost"]);
                                pMProductOutDetails.ItemName = Convert.ToString(reader["ItemName"]);
                                pMProductOutDetails.CostCenterFrom = Convert.ToString(reader["CostCenterFrom"]);
                                pMProductOutDetails.CostCenterTo = Convert.ToString(reader["CostCenterTo"]);
                                pMProductOutDetails.LocationFrom = Convert.ToString(reader["LocationFrom"]);
                                pMProductOutDetails.LocationTo = Convert.ToString(reader["LocationTo"]);
                                pMProductOutDetails.StockBy = Convert.ToString(reader["StockBy"]);
                                pMProductOutDetails.CategoryName = Convert.ToString(reader["CategoryName"]);
                                pMProductOutDetails.Code = Convert.ToString(reader["Code"]);
                                pMProductOutDetails.UserName = Convert.ToString(reader["UserName"]);
                                pMProductOutDetails.ReferenceNumberText = Convert.ToString(reader["ReferenceNumberText"]);
                                pMProductOutDetails.ReferenceNumber = Convert.ToString(reader["ReferenceNumber"]);
                                pMProductOutDetails.ReferenceDate = Convert.ToString(reader["ReferenceDate"]);
                                pMProductOutDetails.ReferenceBy = Convert.ToString(reader["ReferenceBy"]);
                                if (reader["AdjustmentQuantity"] != DBNull.Value)
                                    pMProductOutDetails.AdjustmentQuantity = Convert.ToDecimal(reader["AdjustmentQuantity"]);
                                if (reader["AdjustmentStockById"] != DBNull.Value)
                                    pMProductOutDetails.AdjustmentStockById = Convert.ToInt32(reader["AdjustmentStockById"]);
                                pMProductOutDetails.ApprovalStatus = Convert.ToString(reader["ApprovalStatus"]);
                                pMProductOutDetails.ProductType = Convert.ToString(reader["ProductType"]);
                                pMProductOutDetails.TransferedByName = Convert.ToString(reader["TransferedByName"]);
                                pMProductOutDetails.CheckedByName = Convert.ToString(reader["CheckedByName"]);
                                pMProductOutDetails.ApprovedByName = Convert.ToString(reader["ApprovedByName"]);

                                pMProductOutDetails.FromCompanyId = Convert.ToInt32(reader["FromCompanyId"]);
                                pMProductOutDetails.FromCompany = Convert.ToString(reader["FromCompany"]);
                                pMProductOutDetails.FromProjectId = Convert.ToInt32(reader["FromProjectId"]);
                                pMProductOutDetails.FromProject = Convert.ToString(reader["FromProject"]);
                                pMProductOutDetails.ToCompanyId = Convert.ToInt32(reader["ToCompanyId"]);
                                pMProductOutDetails.ToCompany = Convert.ToString(reader["ToCompany"]);
                                pMProductOutDetails.ToProjectId = Convert.ToInt32(reader["ToProjectId"]);
                                pMProductOutDetails.ToProject = Convert.ToString(reader["ToProject"]);

                                productOutDetails.Add(pMProductOutDetails);
                            }
                        }
                    }
                }
            }

            return productOutDetails;
        }
        public List<PMProductOutDetailsBO> GetProductOutDetailsById_(int outId)
        {
            List<PMProductOutDetailsBO> productOutDetails = new List<PMProductOutDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductOutDetailsByOutId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@OutId", DbType.Int32, outId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    productOutDetails = Table.AsEnumerable().Select(r => new PMProductOutDetailsBO
                    {
                        OutDetailsId = r.Field<Int32>("OutDetailsId"),
                        OutId = r.Field<Int32>("OutId"),
                        IssueNumber = r.Field<string>("IssueNumber"),
                        CostCenterIdFrom = r.Field<Int32>("FromCostCenterId"),
                        LocationIdFrom = r.Field<Int32>("FromLocationId"),
                        CostCenterIdTo = r.Field<Int32?>("ToCostCenterId"),
                        LocationIdTo = r.Field<Int32?>("ToLocationId"),
                        StockById = r.Field<Int32>("StockById"),
                        CategoryId = r.Field<Int32>("CategoryId"),
                        ProductId = r.Field<Int32>("ItemId"),
                        Quantity = r.Field<decimal>("Quantity"),
                        ItemName = r.Field<string>("ItemName"),
                        CostCenterFrom = r.Field<string>("CostCenterFrom"),
                        CostCenterTo = r.Field<string>("CostCenterTo"),
                        LocationFrom = r.Field<string>("LocationFrom"),
                        LocationTo = r.Field<string>("LocationTo"),
                        StockBy = r.Field<string>("StockBy"),
                        CategoryName = r.Field<string>("CategoryName"),
                        Code = r.Field<string>("Code"),
                        UserName = r.Field<string>("UserName"),
                        ReferenceNumberText = r.Field<string>("ReferenceNumberText"),
                        ReferenceNumber = r.Field<string>("ReferenceNumber"),
                        ReferenceDate = r.Field<string>("ReferenceDate"),
                        ReferenceBy = r.Field<string>("ReferenceBy"),
                        AdjustmentQuantity = r.Field<decimal?>("AdjustmentQuantity"),
                        AdjustmentStockById = r.Field<Int32?>("AdjustmentStockById"),
                        ApprovalStatus = r.Field<String>("ApprovalStatus"),
                        ApprovedQuantity = r.Field<decimal?>("ApprovedQuantity"),
                        ProductType = r.Field<string>("ProductType"),
                        TransferedByName = r.Field<string>("TransferedByName"),
                        CheckedByName = r.Field<string>("CheckedByName"),
                        ApprovedByName = r.Field<string>("ApprovedByName")
                    }).ToList();
                }
            }

            return productOutDetails;
        }        
        public bool UpdateProductOutStatusNItemStock(PMProductOutBO productOut)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateProductOutStatusNItemStock_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@OutId", DbType.Int32, productOut.OutId);
                            dbSmartAspects.AddInParameter(command, "@ProductOutFor", DbType.String, productOut.ProductOutFor);
                            dbSmartAspects.AddInParameter(command, "@Status", DbType.String, productOut.Status);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, productOut.LastModifiedBy);

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
        public bool UpdateProductOutStatusNItemStockForDirectOut(PMProductOutBO productOut)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateProductOutStatusNItemStockForDirectOut_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@OutId", DbType.Int32, productOut.OutId);
                            dbSmartAspects.AddInParameter(command, "@ProductOutFor", DbType.String, productOut.ProductOutFor);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, productOut.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(command, "@mErr", DbType.Int32, sizeof(Int32));

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
        public bool UpdateSupplierProductReturnStatus(PMProductOutBO productOut)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSupplierProductReturnStatus_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@ReturnId", DbType.Int32, productOut.ReturnId);
                            dbSmartAspects.AddInParameter(command, "@Status", DbType.String, productOut.Status);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, productOut.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(command, "@mErr", DbType.Int32, sizeof(Int32));

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
        //------------------ Product Out For Room 
        public bool SaveProductOutForRoom(HotelRoomInventoryBO ProductOut, List<HotelRoomInventoryDetailsBO> AddedOutDetails, out int outId)
        {
            int status = 0;
            outId = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("SaveProductOutForRoom_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@OutDate", DbType.DateTime, ProductOut.OutDate);
                            dbSmartAspects.AddInParameter(commandOut, "@RoomTypeId", DbType.Int32, ProductOut.RoomTypeId);
                            dbSmartAspects.AddInParameter(commandOut, "@RoomId", DbType.Int32, ProductOut.RoomId);
                            dbSmartAspects.AddInParameter(commandOut, "@Status", DbType.String, ProductOut.Status);
                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, ProductOut.Remarks);
                            dbSmartAspects.AddInParameter(commandOut, "@CreatedBy", DbType.Int32, ProductOut.CreatedBy);

                            dbSmartAspects.AddOutParameter(commandOut, "@InventoryOutId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);

                            outId = Convert.ToInt32(commandOut.Parameters["@InventoryOutId"].Value);
                        }

                        if (status > 0 && AddedOutDetails.Count > 0)
                        {
                            foreach (HotelRoomInventoryDetailsBO pout in AddedOutDetails)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveProductOutDetailsForRoom_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@InventoryOutId", DbType.Int32, outId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@CostCenterId", DbType.Int32, pout.CostCenterId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationId", DbType.Int32, pout.LocationId);

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductId", DbType.Int32, pout.ProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && ProductOut.Status == "Approved")
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductOutStatusNItemStockForRoom_SP"))
                            {
                                cmdOutDetails.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmdOutDetails, "@InventoryOutId", DbType.Int32, outId);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@Status", DbType.String, ProductOut.Status);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@LastModifiedBy", DbType.Int32, ProductOut.CreatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
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
        public bool UpdateProductOutForRoom(HotelRoomInventoryBO ProductOut, List<HotelRoomInventoryDetailsBO> AddedOutDetails, List<HotelRoomInventoryDetailsBO> EditedOutDetails, List<HotelRoomInventoryDetailsBO> DeletedOutDetails)
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
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("UpdateProductOutForRoom_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@InventoryOutId", DbType.Int32, ProductOut.InventoryOutId);
                            dbSmartAspects.AddInParameter(commandOut, "@RoomTypeId", DbType.Int32, ProductOut.RoomTypeId);
                            dbSmartAspects.AddInParameter(commandOut, "@RoomId", DbType.Int32, ProductOut.RoomId);
                            dbSmartAspects.AddInParameter(commandOut, "@Status", DbType.String, ProductOut.Status);
                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, ProductOut.Remarks);
                            dbSmartAspects.AddInParameter(commandOut, "@LastModifiedBy", DbType.Int32, ProductOut.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);
                        }

                        if (status > 0 && AddedOutDetails.Count > 0)
                        {
                            foreach (HotelRoomInventoryDetailsBO pout in AddedOutDetails)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveProductOutDetailsForRoom_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@InventoryOutId", DbType.Int32, ProductOut.InventoryOutId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@CostCenterId", DbType.Int32, pout.CostCenterId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationId", DbType.Int32, pout.LocationId);

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductId", DbType.Int32, pout.ProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && EditedOutDetails.Count > 0)
                        {
                            foreach (HotelRoomInventoryDetailsBO pout in EditedOutDetails)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductOutDetailsForRoom_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutDetailsId", DbType.Int32, pout.OutDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@InventoryOutId", DbType.Int32, ProductOut.InventoryOutId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@CostCenterId", DbType.Int32, pout.CostCenterId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LocationId", DbType.Int32, pout.LocationId);

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductId", DbType.Int32, pout.ProductId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedOutDetails.Count > 0)
                        {
                            foreach (HotelRoomInventoryDetailsBO pout in DeletedOutDetails)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("DeleteProductOutDetailsForRoom_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutDetailsId", DbType.Int32, pout.OutDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@InventoryOutId", DbType.Int32, ProductOut.InventoryOutId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
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
        public bool DeleteProductOutForRoom(int outId)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteProductOutForRoom_SP"))
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@InventoryOutId", DbType.Int32, outId);
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
        public bool UpdateProductOutStatusNItemStockForDirectOutForRoom(HotelRoomInventoryBO productOut)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateProductOutStatusNItemStockForRoom_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@InventoryOutId", DbType.Int32, productOut.InventoryOutId);
                            dbSmartAspects.AddInParameter(command, "@Status", DbType.String, productOut.Status);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, productOut.LastModifiedBy);

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
        public HotelRoomInventoryBO GetProductOutForRoomById(int outId)
        {
            HotelRoomInventoryBO productReceive = new HotelRoomInventoryBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductOutForRoomById_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@InventoryOutId", DbType.Int32, outId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new HotelRoomInventoryBO
                    {
                        InventoryOutId = r.Field<Int32>("InventoryOutId"),
                        RoomTypeId = r.Field<Int32>("RoomTypeId"),
                        RoomId = r.Field<Int32>("RoomId"),
                        OutDate = r.Field<DateTime>("OutDate"),
                        Remarks = r.Field<string>("Remarks")

                    }).FirstOrDefault();
                }
            }

            return productReceive;
        }
        public List<HotelRoomInventoryDetailsBO> GetProductForRoomOutDetailsById(int outId)
        {
            List<HotelRoomInventoryDetailsBO> productOutDetails = new List<HotelRoomInventoryDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductOutForRommDetailsByOutId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@InventoryOutId", DbType.Int32, outId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    productOutDetails = Table.AsEnumerable().Select(r => new HotelRoomInventoryDetailsBO
                    {
                        OutDetailsId = r.Field<Int32>("OutDetailsId"),
                        InventoryOutId = r.Field<Int32>("InventoryOutId"),
                        CostCenterId = r.Field<Int32>("CostCenterId"),
                        LocationId = r.Field<Int32>("LocationId"),
                        StockById = r.Field<Int32>("StockById"),
                        CategoryId = r.Field<Int32>("CategoryId"),
                        ProductId = r.Field<Int32>("ProductId"),
                        Quantity = r.Field<decimal>("Quantity"),
                        ProductName = r.Field<string>("ItemName"),
                        CostCenter = r.Field<string>("CostCenter"),
                        Location = r.Field<string>("Location"),
                        StockBy = r.Field<string>("StockBy")

                    }).ToList();
                }
            }

            return productOutDetails;
        }
        public List<HotelRoomInventoryBO> GetProductOutForRoomSearch(int roomId, int costCenterId, int locationId, DateTime? dateFrom, DateTime? dateTo)
        {
            List<HotelRoomInventoryBO> productReceive = new List<HotelRoomInventoryBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductOutForRoomSearch_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (costCenterId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);

                    if (locationId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);

                    if (roomId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@RoomId", DbType.String, roomId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@RoomId", DbType.String, DBNull.Value);

                    if (dateFrom != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.DateTime, dateFrom);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateFrom", DbType.Int32, DBNull.Value);

                    if (dateTo != null)
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.DateTime, dateTo);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DateTo", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new HotelRoomInventoryBO
                    {
                        InventoryOutId = r.Field<Int32>("InventoryOutId"),
                        OutDate = r.Field<DateTime>("OutDate"),
                        RoomId = r.Field<Int32>("RoomId"),
                        RoomTypeId = r.Field<Int32>("RoomTypeId"),
                        RoomType = r.Field<string>("RoomType"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        Remarks = r.Field<string>("Remarks"),
                        Status = r.Field<string>("Status")

                    }).ToList();
                }
            }

            return productReceive;
        }

        public List<PMProductOutBO> GetProductOutForReceive()
        {
            List<PMProductOutBO> productReceive = new List<PMProductOutBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductOutForReceive_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductOutBO
                    {
                        OutId = r.Field<Int32>("OutId"),
                        ProductOutFor = r.Field<string>("ProductOutFor"),
                        OutDate = r.Field<DateTime>("OutDate"),
                        RequisitionOrSalesId = r.Field<Int32>("RequisitionOrSalesId"),
                        OutFor = r.Field<Int32?>("OutFor"),
                        Remarks = r.Field<string>("Remarks"),
                        PONumber = r.Field<string>("PONumber")
                    }).ToList();
                }
            }

            return productReceive;
        }
        public Boolean UpdateProductOutFromReceive(int outId, int lastmodifiedBy)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductOutFromReceive_SP"))
                {
                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, outId);
                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LastModifiedBy", DbType.Int32, lastmodifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails) > 0 ? true : false;
                }
            }
            return status;
        }

        public Boolean UpdateProductOutConsumptionAdjustment(List<PMProductOutDetailsBO> ProductOutDetails)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                foreach (PMProductOutDetailsBO PMProductOutDetails in ProductOutDetails)
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("UpdateProductOutConsumptionAdjustmentById_SP"))
                    {

                        dbSmartAspects.AddInParameter(cmd, "@OutDetailsId", DbType.Int32, PMProductOutDetails.OutDetailsId);
                        dbSmartAspects.AddInParameter(cmd, "@AdjustmentStockById", DbType.Int32, PMProductOutDetails.AdjustmentStockById);
                        dbSmartAspects.AddInParameter(cmd, "@AdjustmentQuantity", DbType.Decimal, PMProductOutDetails.AdjustmentQuantity);

                        status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                    }
                }

            }
            return status;
        }

        public bool CheckOrApproveProductOutConsumption(PMProductOutBO productOut, List<PMProductOutDetailsBO> approvedItem,
                                                        List<PMProductOutDetailsBO> cancelItem)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CheckOrApproveProductOutConsumption_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@OutId", DbType.Int32, productOut.OutId);
                            dbSmartAspects.AddInParameter(command, "@Status", DbType.String, productOut.Status);
                            dbSmartAspects.AddInParameter(command, "@CheckedBy", DbType.Int32, productOut.CheckedBy);
                            dbSmartAspects.AddInParameter(command, "@ApprovedBy", DbType.Int32, productOut.ApprovedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        }
                        if (status > 0 && approvedItem.Count > 0)
                        {
                            foreach (PMProductOutDetailsBO pout in approvedItem)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("CheckOrApproveProductOutDetailsConsumption_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, productOut.OutId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutDetailsId", DbType.Int32, pout.OutDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Status", DbType.String, productOut.Status);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ApproveQuantity", DbType.Decimal, pout.ApprovedQuantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);

                                }
                            }
                        }
                        if (status > 0 && cancelItem.Count > 0)
                        {
                            foreach (PMProductOutDetailsBO pout in cancelItem)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("CheckOrApproveProductOutDetailsConsumption_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, productOut.OutId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutDetailsId", DbType.Int32, pout.OutDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Status", DbType.String, pout.ApprovalStatus);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ApproveQuantity", DbType.Decimal, pout.ApprovedQuantity);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);

                                }
                            }
                        }
                        if (status > 0 && productOut.Status == "Approved")
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateProductOutStatusNItemStockForDirectOut_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@OutId", DbType.Int32, productOut.OutId);
                                dbSmartAspects.AddInParameter(command, "@ProductOutFor", DbType.String, productOut.ProductOutFor);
                                dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, productOut.LastModifiedBy);
                                dbSmartAspects.AddOutParameter(command, "@mErr", DbType.Int32, sizeof(Int32));

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

        public bool CancelProductOutInfo(int outId)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CancelProductOutInfo_SP"))
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@OutId", DbType.Int32, outId);
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

        //------------ Product Out New

        public bool SaveProductOutInfo(PMProductOutBO ProductOut,
                                       List<PMProductOutDetailsBO> TransferItemAdded,
                                       List<PMProductOutSerialInfoBO> ItemSerialDetails,
                                       bool isApprovalProcessEnable, out int outId)
        {
            int status = 0;
            outId = 0;
            bool retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("SaveProductTransfer_SP"))
                        {
                            commandOut.Parameters.Clear();

                            if (!String.IsNullOrEmpty(ProductOut.ProductOutFor))
                                dbSmartAspects.AddInParameter(commandOut, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@ProductOutFor", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@TransferFor", DbType.String, ProductOut.TransferFor);
                            dbSmartAspects.AddInParameter(commandOut, "@RequisitionOrSalesId", DbType.Int32, ProductOut.RequisitionOrSalesId);
                            dbSmartAspects.AddInParameter(commandOut, "@OutDate", DbType.DateTime, ProductOut.OutDate);

                            if (ProductOut.OutFor != 0)
                                dbSmartAspects.AddInParameter(commandOut, "@OutFor", DbType.Int32, ProductOut.OutFor);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@OutFor", DbType.Int32, DBNull.Value);

                            if (!string.IsNullOrEmpty(ProductOut.IssueType))
                                dbSmartAspects.AddInParameter(commandOut, "@IssueType", DbType.String, ProductOut.IssueType);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@IssueType", DbType.String, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, ProductOut.Remarks);

                            if (ProductOut.FromCostCenterId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int64, ProductOut.FromCostCenterId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int64, DBNull.Value);

                            if (ProductOut.FromLocationId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int64, ProductOut.FromLocationId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int64, DBNull.Value);

                            if (ProductOut.ToCostCenterId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@ToCostCenterId", DbType.Int64, ProductOut.ToCostCenterId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@ToCostCenterId", DbType.Int64, DBNull.Value);

                            if (ProductOut.ToLocationId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@ToLocationId", DbType.Int64, ProductOut.ToLocationId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@ToLocationId", DbType.Int64, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@Status", DbType.String, ProductOut.Status);
                            dbSmartAspects.AddInParameter(commandOut, "@CreatedBy", DbType.Int64, ProductOut.CreatedBy);

                            dbSmartAspects.AddInParameter(commandOut, "@GLCompanyId", DbType.Int32, ProductOut.GLCompanyId);
                            dbSmartAspects.AddInParameter(commandOut, "@GLProjectId", DbType.Int32, ProductOut.GLProjectId);

                            dbSmartAspects.AddInParameter(commandOut, "@ToGLCompanyId", DbType.Int32, ProductOut.ToGLCompanyId);
                            dbSmartAspects.AddInParameter(commandOut, "@ToGLProjectId", DbType.Int32, ProductOut.ToGLProjectId);

                            if (ProductOut.AccountPostingHeadId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@AccountPostingHeadId", DbType.Int64, ProductOut.AccountPostingHeadId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@AccountPostingHeadId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddOutParameter(commandOut, "@OutId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);

                            outId = Convert.ToInt32(commandOut.Parameters["@OutId"].Value);
                        }

                        if (status > 0 && TransferItemAdded.Count > 0)
                        {
                            foreach (PMProductOutDetailsBO pout in TransferItemAdded)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveItemTransferDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, outId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, pout.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@AverageCost", DbType.Decimal, pout.AverageCost);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@RequisitionOrSalesId", DbType.Int32, ProductOut.RequisitionOrSalesId);

                                    if (pout.ColorId != 0)
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, pout.ColorId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, DBNull.Value);

                                    if (pout.SizeId != 0)
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, pout.SizeId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, DBNull.Value);

                                    if (pout.StyleId != 0)
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, pout.StyleId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, DBNull.Value);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && ItemSerialDetails.Count > 0)
                        {
                            if (ProductOut.IssueType == "FixedAssetReturn")
                            {
                                using (DbCommand cmdReceiveDetails = dbSmartAspects.GetStoredProcCommand("SaveOutSerialInfoForFixedAssetReturn_SP"))
                                {
                                    foreach (PMProductOutSerialInfoBO rd in ItemSerialDetails)
                                    {
                                        cmdReceiveDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(cmdReceiveDetails, "@OutId", DbType.Int32, outId);
                                        dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                        dbSmartAspects.AddInParameter(cmdReceiveDetails, "@SerialNumber", DbType.String, rd.SerialNumber);
                                        dbSmartAspects.AddInParameter(cmdReceiveDetails, "@IssueType", DbType.String,  ProductOut.IssueType);
                                        dbSmartAspects.AddInParameter(cmdReceiveDetails, "@CreatedBy", DbType.Int32, ProductOut.CreatedBy);

                                        status = dbSmartAspects.ExecuteNonQuery(cmdReceiveDetails, transction);
                                    }
                                }

                            }
                            else
                            {
                                using (DbCommand cmdReceiveDetails = dbSmartAspects.GetStoredProcCommand("SaveOutSerialInfo_SP"))
                                {
                                    foreach (PMProductOutSerialInfoBO rd in ItemSerialDetails)
                                    {
                                        cmdReceiveDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(cmdReceiveDetails, "@OutId", DbType.Int32, outId);
                                        dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                        dbSmartAspects.AddInParameter(cmdReceiveDetails, "@SerialNumber", DbType.String, rd.SerialNumber);
                                        dbSmartAspects.AddInParameter(cmdReceiveDetails, "@CreatedBy", DbType.Int32, ProductOut.CreatedBy);

                                        status = dbSmartAspects.ExecuteNonQuery(cmdReceiveDetails, transction);
                                    }
                                }

                            }
                        }

                        if (status > 0 && isApprovalProcessEnable && ProductOut.ProductOutFor == "Requisition")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdatePOStatusForRequisitionWiseTransfer_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@OutId", DbType.Int32, outId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }

                        if (status > 0 && !isApprovalProcessEnable)
                        {
                            using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("OutOrderApproval_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@OutId", DbType.Int32, outId);
                                dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, ProductOut.Status);
                                dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, ProductOut.CreatedBy);
                                dbSmartAspects.AddInParameter(commandMaster, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);

                                status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            }

                            if (status > 0 && ProductOut.Status == "Approved")
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductOutStatusNItemStock_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, outId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Status", DbType.String, ProductOut.Status);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@LastModifiedBy", DbType.Int32, ProductOut.CreatedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }

                            if (status > 0 && ProductOut.ProductOutFor == "Requisition" && ProductOut.Status == "Approved")
                            {
                                using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdatePOStatusForRequisitionWiseTransfer_SP"))
                                {
                                    dbSmartAspects.AddInParameter(commandDetails, "@OutId", DbType.Int32, outId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, ProductOut.Status);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && isApprovalProcessEnable && ProductOut.ProductOutFor == "SalesOut")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateStatusForQuotationWiseTransfer_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@QuotationId", DbType.Int32, ProductOut.RequisitionOrSalesId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }
                        //if (status > 0 && ProductOut.Status == "Approved" && isProductOutReceiveApproval == 0)
                        //{
                        //    using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductOutStatusNItemStock_SP"))
                        //    {
                        //        cmdOutDetails.Parameters.Clear();

                        //        dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, outId);
                        //        dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);
                        //        dbSmartAspects.AddInParameter(cmdOutDetails, "@Status", DbType.String, ProductOut.Status);
                        //        dbSmartAspects.AddInParameter(cmdOutDetails, "@LastModifiedBy", DbType.Int32, ProductOut.CreatedBy);

                        //        status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                        //    }
                        //}
                        //else if (status > 0 && ProductOut.Status == "Approved" && isProductOutReceiveApproval == 1)
                        //{
                        //    using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductOutStatusForReceiveApproval_SP"))
                        //    {
                        //        cmdOutDetails.Parameters.Clear();

                        //        dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, outId);
                        //        dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);
                        //        dbSmartAspects.AddInParameter(cmdOutDetails, "@Status", DbType.String, ProductOut.Status);
                        //        dbSmartAspects.AddInParameter(cmdOutDetails, "@LastModifiedBy", DbType.Int32, ProductOut.CreatedBy);

                        //        status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                        //    }
                        //}

                        //if (status > 0 && ProductOut.ProductOutFor == "Requisition")
                        //{
                        //    string query = string.Empty;
                        //    query += "UPDATE PMRequisition " +
                        //             "SET DelivaredStatus = dbo.FnGetInvRequisitionItemDeliverStatus(" + outId + ", NULL, NULL, 'RequisitionStatus') " +
                        //             "WHERE RequisitionId = " + ProductOut.RequisitionOrSalesId;

                        //    using (DbCommand cmdUpdateRequisition = dbSmartAspects.GetSqlStringCommand(query))
                        //    {
                        //        status = dbSmartAspects.ExecuteNonQuery(cmdUpdateRequisition, transction);
                        //    }
                        //}

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
        public bool UpdateProductOutInfo(PMProductOutBO ProductOut,
                                         List<PMProductOutDetailsBO> TransferItemAdded,
                                         List<PMProductOutDetailsBO> TransferItemEdited,
                                         List<PMProductOutDetailsBO> TransferItemDeleted,
                                         List<PMProductOutSerialInfoBO> ItemSerialDetails,
                                         List<PMProductOutSerialInfoBO> DeletedSerialzableProduct)
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
                        using (DbCommand commandOut = dbSmartAspects.GetStoredProcCommand("UpdateProductTransfer_SP"))
                        {
                            commandOut.Parameters.Clear();

                            dbSmartAspects.AddInParameter(commandOut, "@OutId", DbType.Int32, ProductOut.OutId);
                            if (!String.IsNullOrEmpty(ProductOut.ProductOutFor))
                                dbSmartAspects.AddInParameter(commandOut, "@TransferFor", DbType.String, ProductOut.TransferFor);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@TransferFor", DbType.String, DBNull.Value);

                            if (ProductOut.FromCostCenterId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int64, ProductOut.FromCostCenterId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromCostCenterId", DbType.Int64, DBNull.Value);

                            if (ProductOut.FromLocationId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int64, ProductOut.FromLocationId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@FromLocationId", DbType.Int64, DBNull.Value);

                            if (ProductOut.ToCostCenterId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@ToCostCenterId", DbType.Int64, ProductOut.ToCostCenterId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@ToCostCenterId", DbType.Int64, DBNull.Value);

                            if (ProductOut.ToLocationId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@ToLocationId", DbType.Int64, ProductOut.ToLocationId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@ToLocationId", DbType.Int64, DBNull.Value);
                            if (ProductOut.AccountPostingHeadId != null)
                                dbSmartAspects.AddInParameter(commandOut, "@AccountPostingHeadId", DbType.Int64, ProductOut.AccountPostingHeadId);
                            else
                                dbSmartAspects.AddInParameter(commandOut, "@AccountPostingHeadId", DbType.Int64, DBNull.Value);

                            dbSmartAspects.AddInParameter(commandOut, "@Remarks", DbType.String, ProductOut.Remarks);
                            dbSmartAspects.AddInParameter(commandOut, "@LastModifiedBy", DbType.Int32, ProductOut.LastModifiedBy);
                            dbSmartAspects.AddInParameter(commandOut, "@GLCompanyId", DbType.Int32, ProductOut.GLCompanyId);
                            dbSmartAspects.AddInParameter(commandOut, "@GLProjectId", DbType.Int32, ProductOut.GLProjectId);
                            dbSmartAspects.AddInParameter(commandOut, "@ToGLCompanyId", DbType.Int32, ProductOut.ToGLCompanyId);
                            dbSmartAspects.AddInParameter(commandOut, "@ToGLProjectId", DbType.Int32, ProductOut.ToGLProjectId);

                            status = dbSmartAspects.ExecuteNonQuery(commandOut, transction);
                        }

                        if (status > 0 && TransferItemAdded.Count > 0)
                        {
                            foreach (PMProductOutDetailsBO pout in TransferItemAdded)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveItemTransferDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, ProductOut.OutId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, pout.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@AverageCost", DbType.Decimal, pout.AverageCost);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@RequisitionOrSalesId", DbType.Int32, ProductOut.RequisitionOrSalesId);

                                    if (pout.ColorId != 0)
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, pout.ColorId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, DBNull.Value);

                                    if (pout.SizeId != 0)
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, pout.SizeId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, DBNull.Value);

                                    if (pout.StyleId != 0)
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, pout.StyleId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, DBNull.Value);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && TransferItemEdited.Count > 0)
                        {
                            foreach (PMProductOutDetailsBO pout in TransferItemEdited)
                            {
                                using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductTransferDetails_SP"))
                                {
                                    cmdOutDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutDetailsId", DbType.Int32, pout.OutDetailsId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, ProductOut.OutId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@ItemId", DbType.Int32, pout.ItemId);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@Quantity", DbType.Decimal, pout.Quantity);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@AverageCost", DbType.Decimal, pout.AverageCost);
                                    dbSmartAspects.AddInParameter(cmdOutDetails, "@RequisitionOrSalesId", DbType.Int32, ProductOut.RequisitionOrSalesId);

                                    if (pout.ColorId != 0)
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, pout.ColorId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@ColorId", DbType.Int32, DBNull.Value);

                                    if (pout.SizeId != 0)
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, pout.SizeId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@SizeId", DbType.Int32, DBNull.Value);

                                    if (pout.StyleId != 0)
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, pout.StyleId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdOutDetails, "@StyleId", DbType.Int32, DBNull.Value);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && TransferItemDeleted.Count > 0)
                        {
                            using (DbCommand commandDeletePO = dbSmartAspects.GetStoredProcCommand("DeleteProductTransferDetails_SP"))
                            {
                                foreach (PMProductOutDetailsBO pout in TransferItemDeleted)
                                {
                                    commandDeletePO.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeletePO, "@OutDetailsId", DbType.Int32, pout.OutDetailsId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@OutId", DbType.Int32, ProductOut.OutId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@StockById", DbType.Int32, pout.StockById);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ItemId", DbType.Int32, pout.ItemId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@Quantity", DbType.Decimal, pout.Quantity);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@RequisitionOrSalesId", DbType.Int32, ProductOut.RequisitionOrSalesId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ProductOutFor", DbType.String, ProductOut.ProductOutFor);

                                    if (pout.ColorId != 0)
                                        dbSmartAspects.AddInParameter(commandDeletePO, "@ColorId", DbType.Int32, pout.ColorId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDeletePO, "@ColorId", DbType.Int32, DBNull.Value);

                                    if (pout.SizeId != 0)
                                        dbSmartAspects.AddInParameter(commandDeletePO, "@SizeId", DbType.Int32, pout.SizeId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDeletePO, "@SizeId", DbType.Int32, DBNull.Value);

                                    if (pout.StyleId != 0)
                                        dbSmartAspects.AddInParameter(commandDeletePO, "@StyleId", DbType.Int32, pout.StyleId);
                                    else
                                        dbSmartAspects.AddInParameter(commandDeletePO, "@StyleId", DbType.Int32, DBNull.Value);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePO, transction);
                                }
                            }
                        }

                        if (status > 0 && ItemSerialDetails.Count > 0)
                        {
                            using (DbCommand cmdReceiveDetails = dbSmartAspects.GetStoredProcCommand("SaveOutSerialInfo_SP"))
                            {
                                foreach (PMProductOutSerialInfoBO rd in ItemSerialDetails)
                                {
                                    cmdReceiveDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@OutId", DbType.Int32, ProductOut.OutId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@SerialNumber", DbType.String, rd.SerialNumber);
                                    dbSmartAspects.AddInParameter(cmdReceiveDetails, "@CreatedBy", DbType.Int32, ProductOut.CreatedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdReceiveDetails, transction);
                                }
                            }
                        }

                        if (status > 0 && DeletedSerialzableProduct.Count > 0)
                        {
                            using (DbCommand commandDeletePO = dbSmartAspects.GetStoredProcCommand("DeleteOutSerialInfo_SP"))
                            {
                                foreach (PMProductOutSerialInfoBO rd in DeletedSerialzableProduct)
                                {
                                    commandDeletePO.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandDeletePO, "@OutSerialId", DbType.Int32, rd.OutSerialId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@OutId", DbType.Int32, ProductOut.OutId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@SerialNumber", DbType.String, rd.SerialNumber);
                                    dbSmartAspects.AddInParameter(commandDeletePO, "@CreatedBy", DbType.Int32, ProductOut.CreatedBy);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDeletePO, transction);
                                }
                            }
                        }

                        if (status > 0 && ProductOut.ProductOutFor == "Requisition")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdatePOStatusForRequisitionWiseTransfer_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@OutId", DbType.Int32, ProductOut.OutId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }
                        if (status > 0  && ProductOut.ProductOutFor == "SalesOut")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateStatusForQuotationWiseTransfer_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@QuotationId", DbType.Int32, ProductOut.RequisitionOrSalesId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }
                        //if (status > 0 && ProductOut.ProductOutFor == "Requisition")
                        //{
                        //    string query = string.Empty;
                        //    query += "UPDATE PMRequisition " +
                        //             "SET DelivaredStatus = dbo.FnGetInvRequisitionItemDeliverStatus(" + ProductOut.OutId + ", NULL, NULL, 'RequisitionStatus') " +
                        //             "WHERE RequisitionId = " + ProductOut.RequisitionOrSalesId;

                        //    using (DbCommand cmdUpdateRequisition = dbSmartAspects.GetSqlStringCommand(query))
                        //    {
                        //        status = dbSmartAspects.ExecuteNonQuery(cmdUpdateRequisition, transction);
                        //    }
                        //}

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

        public List<PMProductOutBO> GetProductOutForSearch(string outType, DateTime? fromDate, DateTime? toDate,
                                                                string issueNumber, string status,
                                                                Int32 userInfoId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<PMProductOutBO> productReceive = new List<PMProductOutBO>();
            totalRecords = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductOutForSearch_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (outType != "All")
                        dbSmartAspects.AddInParameter(cmd, "@IssueType", DbType.String, outType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IssueType", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    if (!string.IsNullOrEmpty(issueNumber))
                        dbSmartAspects.AddInParameter(cmd, "@IssueNumber", DbType.String, issueNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IssueNumber", DbType.String, DBNull.Value);

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

                    productReceive = Table.AsEnumerable().Select(r => new PMProductOutBO
                    {
                        OutId = r.Field<Int32>("OutId"),
                        ProductOutFor = r.Field<string>("ProductOutFor"),
                        IssueType = r.Field<string>("IssueType"),
                        IssueNumber = r.Field<string>("IssueNumber"),
                        OutDate = r.Field<DateTime>("OutDate"),

                        RequisitionOrSalesId = r.Field<Int32>("RequisitionOrSalesId"),

                        ToCostCenterId = r.Field<Int32?>("ToCostCenterId"),
                        ToLocationId = r.Field<Int32?>("ToLocationId"),
                        FromCostCenterId = r.Field<Int32?>("FromCostCenterId"),
                        FromLocationId = r.Field<Int32?>("FromLocationId"),

                        ToCostCenter = r.Field<string>("ToCostCenter"),
                        FromCostCenter = r.Field<string>("FromCostCenter"),
                        ToLocation = r.Field<string>("ToLocation"),
                        FromLocation = r.Field<string>("FromLocation"),

                        Status = r.Field<string>("Status"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CheckedBy = r.Field<Int32?>("CheckedBy"),
                        ApprovedBy = r.Field<Int32?>("ApprovedBy"),
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


        public List<PMProductOutSerialInfoBO> GetItemOutSerialById(int outId)
        {
            List<PMProductOutSerialInfoBO> productReceive = new List<PMProductOutSerialInfoBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemOutSerialByOutId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@OutId", DbType.Int32, outId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductOutSerialInfoBO
                    {
                        OutSerialId = r.Field<Int64>("OutSerialId"),
                        OutId = r.Field<Int32>("OutId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        SerialNumber = r.Field<string>("SerialNumber")

                    }).ToList();
                }
            }

            return productReceive;
        }

        public List<PMProductOutDetailsBO> GetItemOutDetailsByOutId(int outId)
        {
            List<PMProductOutDetailsBO> productOutDetails = new List<PMProductOutDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemOutDetailsByOutId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@OutId", DbType.Int32, outId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    productOutDetails = Table.AsEnumerable().Select(r => new PMProductOutDetailsBO
                    {
                        OutId = r.Field<Int32>("OutId"),
                        OutDetailsId = r.Field<Int32>("OutDetailsId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        StockById = r.Field<Int32>("StockById"),
                        StockBy = r.Field<string>("StockBy"),
                        ProductType = r.Field<string>("ProductType"),
                        Quantity = r.Field<decimal>("Quantity"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        ColorId = r.Field<int>("ColorId"),
                        SizeId = r.Field<int>("SizeId"),
                        StyleId = r.Field<int>("StyleId"),
                        ColorText = r.Field<string>("ColorText"),
                        StyleText = r.Field<string>("StyleText"),
                        SizeText = r.Field<string>("SizeText")

                    }).ToList();
                }
            }

            return productOutDetails;
        }

        public List<PMProductOutDetailsBO> GetItemOutDetailsFromRequisitionByOutId(int outId, int requisitionOrSalesId)
        {
            List<PMProductOutDetailsBO> productOutDetails = new List<PMProductOutDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemOutDetailsFromRequisitionByOutId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@OutId", DbType.Int32, outId);
                    dbSmartAspects.AddInParameter(cmd, "@RequisitionOrSalesId", DbType.Int32, requisitionOrSalesId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    productOutDetails = Table.AsEnumerable().Select(r => new PMProductOutDetailsBO
                    {
                        OutId = r.Field<Int32>("OutId"),
                        OutDetailsId = r.Field<Int32>("OutDetailsId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        StockById = r.Field<Int32>("StockById"),
                        StockBy = r.Field<string>("StockBy"),
                        Quantity = r.Field<decimal>("Quantity"),
                        ProductType = r.Field<string>("ProductType"),
                        ApprovedQuantity = r.Field<decimal?>("ApprovedQuantity"),
                        DeliveredQuantity = r.Field<decimal?>("DeliveredQuantity"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        RequisitionDetailsId = r.Field<Int64?>("RequisitionDetailsId"),
                        RemainingTransferQuantity = r.Field<decimal>("RemainingTransferQuantity"),
                        ColorId = r.Field<int>("ColorId"),
                        SizeId = r.Field<int>("SizeId"),
                        StyleId = r.Field<int>("StyleId"),
                        ColorText = r.Field<string>("ColorText"),
                        StyleText = r.Field<string>("StyleText"),
                        SizeText = r.Field<string>("SizeText")

                    }).ToList();
                }
            }

            return productOutDetails;
        }

        public List<PMProductOutDetailsBO> GetItemOutDetailsFromQuotationByOutId(int outId, int requisitionOrSalesId)
        {
            List<PMProductOutDetailsBO> productOutDetails = new List<PMProductOutDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemOutDetailsFromQuotationByOutId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@OutId", DbType.Int32, outId);
                    dbSmartAspects.AddInParameter(cmd, "@RequisitionOrSalesId", DbType.Int32, requisitionOrSalesId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    productOutDetails = Table.AsEnumerable().Select(r => new PMProductOutDetailsBO
                    {
                        OutId = r.Field<Int32>("OutId"),
                        OutDetailsId = r.Field<Int32>("OutDetailsId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        StockById = r.Field<Int32>("StockById"),
                        StockBy = r.Field<string>("StockBy"),
                        Quantity = r.Field<decimal>("Quantity"),
                        ProductType = r.Field<string>("ProductType"),
                        ApprovedQuantity = r.Field<decimal?>("ApprovedQuantity"),
                        DeliveredQuantity = r.Field<decimal?>("DeliveredQuantity"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        RequisitionDetailsId = r.Field<Int64?>("RequisitionDetailsId"),
                        RemainingTransferQuantity = r.Field<decimal>("RemainingTransferQuantity")

                    }).ToList();
                }
            }

            return productOutDetails;
        }

        public List<PMProductOutDetailsBO> GetItemOutDetailsFromBillingByOutId(int outId, int requisitionOrSalesId)
        {
            List<PMProductOutDetailsBO> productOutDetails = new List<PMProductOutDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemOutDetailsFromBillingByOutId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@OutId", DbType.Int32, outId);
                    dbSmartAspects.AddInParameter(cmd, "@RequisitionOrSalesId", DbType.Int32, requisitionOrSalesId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    productOutDetails = Table.AsEnumerable().Select(r => new PMProductOutDetailsBO
                    {
                        OutId = r.Field<Int32>("OutId"),
                        OutDetailsId = r.Field<Int32>("OutDetailsId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        StockById = r.Field<Int32>("StockById"),
                        StockBy = r.Field<string>("StockBy"),
                        Quantity = r.Field<decimal>("Quantity"),
                        ProductType = r.Field<string>("ProductType"),
                        ApprovedQuantity = r.Field<decimal?>("ApprovedQuantity"),
                        DeliveredQuantity = r.Field<decimal?>("DeliveredQuantity"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        RequisitionDetailsId = r.Field<Int32?>("RequisitionDetailsId"),
                        RemainingTransferQuantity = r.Field<decimal>("RemainingTransferQuantity")

                    }).ToList();
                }
            }

            return productOutDetails;
        }

        public bool OutOrderApproval(string productOutFor, int outId, string approvedStatus, int requisitionOrSalesId, int checkedOrApprovedBy)
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
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("OutOrderApproval_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@OutId", DbType.Int32, outId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, checkedOrApprovedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@ProductOutFor", DbType.String, productOutFor);
                            dbSmartAspects.AddOutParameter(commandMaster, "@OutStatus", DbType.String, 50);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            OutStatus = Convert.ToString(commandMaster.Parameters["@OutStatus"].Value);
                        }

                        if (status > 0 && OutStatus == "Approved")
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductOutStatusNItemStock_SP"))
                            {
                                cmdOutDetails.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, outId);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductOutFor", DbType.String, productOutFor);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@Status", DbType.String, approvedStatus);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@LastModifiedBy", DbType.Int32, checkedOrApprovedBy);

                                status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                            }
                        }

                        if (status > 0 && productOutFor == "Requisition" && OutStatus == "Approved")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdatePOStatusForRequisitionWiseTransfer_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@OutId", DbType.Int32, outId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, approvedStatus);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
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

        public bool OutOrderDelete(string issueType, int outId, string approvedStatus, int createdBy, int lastModifiedBy)
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
                        if (issueType == "Requisition")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdatePOStatusForRequisitionWiseTransferDelete_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@OutId", DbType.Int32, outId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, DBNull.Value);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }
                        else if (issueType == "SalesOut")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateStatusForQuotationWiseTransferDelete_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@OutId", DbType.Int32, outId);

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            }
                        }

                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("OutOrderDelete_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@ProductOutFor", DbType.String, issueType);
                            dbSmartAspects.AddInParameter(commandMaster, "@OutId", DbType.Int32, outId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, createdBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, lastModifiedBy);

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

        public List<SerialDuplicateBO> SerialAvailabilityCheck(string serialNumber, Int64 locationId)
        {

            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                try
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("AvailableSerialCheck_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@SerialNumber", DbType.String, serialNumber);
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int64, locationId);

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
        public List<SerialDuplicateBO> GetCompanyProjectWiseAvailableSerialForAutoSearch(string serialNumber, Int64 companyId, Int64 projectId, Int64 locationId, Int64 itemId)
        {

            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                try
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCompanyProjectWiseAvailableSerialForAutoSearch_SP"))
                    {
                        cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int64, itemId);
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int64, locationId);
                        dbSmartAspects.AddInParameter(cmd, "@SerialNumber", DbType.String, serialNumber);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "SerialList");
                        DataTable Table = ds.Tables["SerialList"];

                        serial = Table.AsEnumerable().Select(r => new SerialDuplicateBO
                        {
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
        public List<SerialDuplicateBO> GetCostcenterProjectWiseAvailableSerialForAutoSearch(string serialNumber, Int32 costcenterId, Int64 projectId, Int64 itemId)
        {

            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                try
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCostcenterProjectWiseAvailableSerialForAutoSearch_SP"))
                    {
                        cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                        dbSmartAspects.AddInParameter(cmd, "@CostcenterId", DbType.Int32, costcenterId);
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int64, itemId);
                        dbSmartAspects.AddInParameter(cmd, "@SerialNumber", DbType.String, serialNumber);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "SerialList");
                        DataTable Table = ds.Tables["SerialList"];

                        serial = Table.AsEnumerable().Select(r => new SerialDuplicateBO
                        {
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
        public List<SerialDuplicateBO> GetFixedAssetAvailableSerialForAutoSearch(string serialNumber, Int64 companyId, Int64 projectId, Int64 locationId, Int64 itemId)
        {

            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                try
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFixedAssetAvailableSerialForAutoSearch_SP"))
                    {
                        cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int64, itemId);
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int64, locationId);
                        dbSmartAspects.AddInParameter(cmd, "@SerialNumber", DbType.String, serialNumber);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "SerialList");
                        DataTable Table = ds.Tables["SerialList"];

                        serial = Table.AsEnumerable().Select(r => new SerialDuplicateBO
                        {
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
        public List<SerialDuplicateBO> GetAvailableSerialForAutoSearch(string serialNumber, Int64 locationId, Int64 itemId)
        {

            List<SerialDuplicateBO> serial = new List<SerialDuplicateBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                try
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAvailableSerialForAutoSearch_SP"))
                    {
                        cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int64, itemId);
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int64, locationId);
                        dbSmartAspects.AddInParameter(cmd, "@SerialNumber", DbType.String, serialNumber);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "SerialList");
                        DataTable Table = ds.Tables["SerialList"];

                        serial = Table.AsEnumerable().Select(r => new SerialDuplicateBO
                        {
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

        public List<PMProductOutBO> GetTransferOrderForReceive(Int64 costCenterId, Int64 locationId, DateTime? fromDate, DateTime? toDate,
                                                               string status, int recordPerPage, int pageIndex, out int totalRecords
                                                              )
        {
            List<PMProductOutBO> productReceive = new List<PMProductOutBO>();
            totalRecords = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTransferOrderForReceive_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int64, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int64, locationId);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    if (status != "All")
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, status);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductReceived");
                    DataTable Table = ds.Tables["PMProductReceived"];

                    productReceive = Table.AsEnumerable().Select(r => new PMProductOutBO
                    {
                        OutId = r.Field<Int32>("OutId"),
                        ProductOutFor = r.Field<string>("ProductOutFor"),
                        IssueType = r.Field<string>("IssueType"),
                        IssueNumber = r.Field<string>("IssueNumber"),
                        OutDate = r.Field<DateTime>("OutDate"),

                        RequisitionOrSalesId = r.Field<Int32>("RequisitionOrSalesId"),

                        ToCostCenterId = r.Field<Int32?>("ToCostCenterId"),
                        ToLocationId = r.Field<Int32?>("ToLocationId"),
                        FromCostCenterId = r.Field<Int32?>("FromCostCenterId"),
                        FromLocationId = r.Field<Int32?>("FromLocationId"),

                        ToCostCenter = r.Field<string>("ToCostCenter"),
                        FromCostCenter = r.Field<string>("FromCostCenter"),
                        ToLocation = r.Field<string>("ToLocation"),
                        FromLocation = r.Field<string>("FromLocation"),

                        Status = r.Field<string>("Status"),
                        Remarks = r.Field<string>("Remarks"),
                        CreatedBy = r.Field<Int32>("CreatedBy"),
                        CheckedBy = r.Field<Int32>("CheckedBy"),
                        ApprovedBy = r.Field<Int32>("ApprovedBy"),
                        PRNumber = r.Field<string>("PRNumber")

                    }).ToList();

                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value.ToString());
                }
            }

            return productReceive;
        }

        public bool ItemReceiveOutOrder(string productOutFor, int outId, int requisitionOrSalesId, int checkedOrApprovedBy)
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
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("OutOrderReceiveApproval_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@OutId", DbType.Int32, outId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ProductOutFor", DbType.String, productOutFor);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, "Approved");
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, checkedOrApprovedBy);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        }

                        if (status > 0)
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateProductTransferReceiveStatusNItemStock_SP"))
                            {
                                cmdOutDetails.Parameters.Clear();

                                dbSmartAspects.AddInParameter(cmdOutDetails, "@OutId", DbType.Int32, outId);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@ProductOutFor", DbType.String, productOutFor);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@Status", DbType.String, "Approved");
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@LastModifiedBy", DbType.Int32, checkedOrApprovedBy);

                                status = dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction);
                            }
                        }

                        if (status > 0 && productOutFor == "Requisition")
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateReceiveStatusForRequisitionWiseReceive_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandDetails, "@OutId", DbType.Int32, outId);
                                dbSmartAspects.AddInParameter(commandDetails, "@ApprovedStatus", DbType.String, "Approved");

                                status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
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

        public bool DirectOutOrderApproval(string productOutFor, int outId, string approvedStatus, int requisitionOrSalesId, int checkedOrApprovedBy)
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
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("ConsumptionOrderApproval_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@OutId", DbType.Int32, outId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, checkedOrApprovedBy);
                            dbSmartAspects.AddInParameter(commandMaster, "@ProductOutFor", DbType.String, productOutFor);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        }

                        if (status > 0 && approvedStatus == "Approved")
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateProductOutStatusNItemStockForDirectOutConsumption_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@OutId", DbType.Int32, outId);
                                dbSmartAspects.AddInParameter(command, "@ProductOutFor", DbType.String, productOutFor);
                                dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, checkedOrApprovedBy);

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

        public List<ItemConsumptionReportBO> GetItemConsumptionInfoForReport(int companyId, int projectId, DateTime _fromDate, DateTime _toDate, string _costCenterFrom, string _location,
                                                                             string _category, string itemId, string consumptionType, string consumer)
        {
            List<ItemConsumptionReportBO> ItemConsumption = new List<ItemConsumptionReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemConsumtionForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, _fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, _toDate);

                    if (companyId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, companyId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int32, DBNull.Value);
                    }

                    if (projectId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, DBNull.Value);
                    }

                    if (Convert.ToInt32(_costCenterFrom) == 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, Convert.ToInt32(_costCenterFrom));
                    }

                    if (string.IsNullOrEmpty(_location) || Convert.ToInt32(_location) == 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, Convert.ToInt32(_location));
                    }

                    if (Convert.ToInt32(_category) == 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, Convert.ToInt32(_category));
                    }

                    if (string.IsNullOrEmpty(itemId) || Convert.ToInt32(itemId) == 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, Convert.ToInt32(itemId));
                    }

                    if (consumptionType == "0" || string.IsNullOrEmpty(consumptionType))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ConsumptionType", DbType.String, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ConsumptionType", DbType.String, consumptionType);
                    }

                    if (string.IsNullOrEmpty(consumer) || Convert.ToInt32(consumer) == 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Consumer", DbType.Int32, DBNull.Value);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Consumer", DbType.Int32, Convert.ToInt32(consumer));
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemConsumption");
                    DataTable Table = ds.Tables["ItemConsumption"];

                    ItemConsumption = Table.AsEnumerable().Select(r => new ItemConsumptionReportBO
                    {
                        CostCenterFrom = r.Field<string>("CostCenterFrom"),
                        LocationNameFrom = r.Field<string>("LocationNameFrom"),
                        Category = r.Field<string>("Category"),
                        ReceivedDate = r.Field<string>("ReceivedDate"),
                        IssueNumber = r.Field<string>("IssueNumber"),
                        Code = r.Field<string>("Code"),
                        ProductOutFor = r.Field<string>("ProductOutFor"),
                        ProductName = r.Field<string>("ProductName"),
                        UnitHead = r.Field<string>("UnitHead"),
                        AverageCost = r.Field<decimal>("AverageCost"),
                        Quantity = r.Field<decimal>("Quantity"),
                        SerialNumber = r.Field<string>("SerialNumber"),
                        Remarks = r.Field<string>("Remarks"),
                        UserName = r.Field<string>("UserName")
                    }).ToList();
                }
            }

            return ItemConsumption;
        }
        public List<ItemConsumptionInformationReportBO> GetItemConsumptionInformationForReport(DateTime _fromDate, DateTime _toDate, int _consumptionFor, string _consumer, string _category, string _item)
        {
            List<ItemConsumptionInformationReportBO> ItemConsumption = new List<ItemConsumptionInformationReportBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemConsumtionInformationForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, _fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, _toDate);
                    if (_consumptionFor == 1)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ConsumptionFor", DbType.String, "Costcenter");
                    }
                    else if (_consumptionFor == 2)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ConsumptionFor", DbType.String, "Employee");
                    }
                    if (Convert.ToInt32(_consumer) == 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Consumer", DbType.Int32, DBNull.Value);

                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Consumer", DbType.Int32, Convert.ToInt32(_consumer));
                    }
                    if (Convert.ToInt32(_item) == 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);

                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, Convert.ToInt32(_item));
                    }
                    if (Convert.ToInt32(_category) == 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, DBNull.Value);

                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, Convert.ToInt32(_category));
                    }


                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemConsumption");
                    DataTable Table = ds.Tables["ItemConsumption"];

                    ItemConsumption = Table.AsEnumerable().Select(r => new ItemConsumptionInformationReportBO
                    {
                        Consumer = r.Field<string>("Consumer"),
                        CostCenterFrom = r.Field<string>("CostCenterFrom"),
                        Category = r.Field<string>("Category"),
                        Date = r.Field<string>("Date"),
                        ReferenceNumber = r.Field<string>("ReferenceNumber"),
                        Description = r.Field<string>("Description"),
                        Price = r.Field<decimal?>("Price"),
                        Type = r.Field<string>("Type"),
                        UnitHead = r.Field<string>("UnitHead"),
                        Quantity = r.Field<decimal>("Quantity"),
                        SerialNumber = r.Field<string>("SerialNumber")
                    }).ToList();
                }
            }

            return ItemConsumption;
        }

        public List<PMProductOutViewForDashBoardBO> GetProductOutForEmployee(int empId)
        {
            List<PMProductOutViewForDashBoardBO> productOutDetails = new List<PMProductOutViewForDashBoardBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProductOutForEmployee_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "PMProductOut");
                    DataTable Table = ds.Tables["PMProductOut"];

                    productOutDetails = Table.AsEnumerable().Select(r => new PMProductOutViewForDashBoardBO
                    {
                        Name = r.Field<string>("Name"),
                        ProductType = r.Field<string>("ProductType"),
                        Quantity = r.Field<decimal>("Quantity"),
                        SerialNumber = r.Field<string>("SerialNumber"),
                        UnitHead = r.Field<string>("UnitHead")
                    }).ToList();
                }
            }

            return productOutDetails;
        }
    }
}
