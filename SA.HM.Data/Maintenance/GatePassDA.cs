using HotelManagement.Entity.Maintenance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.Maintenance
{
    public class GatePassDA : BaseService
    {
        public bool SaveGatePassInfo(GatePassBO gatepass, List<GatePassItemBO> AddedItem, out long tmpGatePassId, out string gatePassNumber)
        {
            bool retVal = false;
            int status = 0;
            long tmpGatePassDetailsId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        //done
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGatePassInfo_SP"))
                        {

                            dbSmartAspects.AddInParameter(command, "@GatePassDate", DbType.DateTime, gatepass.GatePassDate);
                            dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, gatepass.SupplierId);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, gatepass.Remarks);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, gatepass.CreatedBy);

                            dbSmartAspects.AddOutParameter(command, "@GatePassId", DbType.Int64, sizeof(Int64));
                            dbSmartAspects.AddOutParameter(command, "@GatePassNumber", DbType.String, 20);

                            status = dbSmartAspects.ExecuteNonQuery(command);
                            tmpGatePassId = Convert.ToInt64(command.Parameters["@GatePassId"].Value);
                            gatePassNumber = command.Parameters["@GatePassNumber"].Value.ToString();
                        }

                        if (status > 0)
                        {
                            //done
                            using (DbCommand cmdGatePassDetails = dbSmartAspects.GetStoredProcCommand("SaveGatePassDetailsInfo_SP"))
                            {
                                foreach (GatePassItemBO rd in AddedItem)
                                {
                                    cmdGatePassDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@GatePassId", DbType.Int64, tmpGatePassId);
                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@CostCenterId", DbType.Int32, rd.CostCenterId);
                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@StockById", DbType.Int32, rd.StockById);
                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@Quantity", DbType.Decimal, rd.Quantity);
                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@Description", DbType.String, rd.Description);
                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@ReturnType", DbType.Int32, rd.ReturnType);
                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@ReturnDate", DbType.DateTime, rd.ReturnDate);
                                    dbSmartAspects.AddOutParameter(cmdGatePassDetails, "@GatePassItemId", DbType.Int64, sizeof(Int64));

                                    status = dbSmartAspects.ExecuteNonQuery(cmdGatePassDetails);
                                    tmpGatePassDetailsId = Convert.ToInt64(cmdGatePassDetails.Parameters["@GatePassItemId"].Value);
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
                    catch
                    {
                        transction.Rollback();
                        retVal = false;
                        throw;
                    }
                }

            }

            return retVal;
        }
        public bool UpdateGatePassInfo(GatePassBO gatepass, List<GatePassItemBO> AddedItem, List<GatePassItemBO> editedItem, List<GatePassItemBO> deletedItem)
        {
            bool retVal = false;
            int status = 0;
            long tmpGatePassDetailsId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        //done
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGatePassInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@GatePassId", DbType.Int64, gatepass.GatePassId);
                            dbSmartAspects.AddInParameter(command, "@GatePassDate", DbType.DateTime, gatepass.GatePassDate);
                            dbSmartAspects.AddInParameter(command, "@SupplierId", DbType.Int32, gatepass.SupplierId);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, gatepass.Remarks);
                            dbSmartAspects.AddInParameter(command, "@ResponsiblePersonId", DbType.Int32, gatepass.ResponsiblePersonId);
                            dbSmartAspects.AddInParameter(command, "@ApprovedBy", DbType.Int32, gatepass.ApprovedBy);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, gatepass.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command);
                        }

                        if (status > 0 && AddedItem.Count > 0)
                        {
                            //done
                            using (DbCommand cmdGatePassDetails = dbSmartAspects.GetStoredProcCommand("SaveGatePassDetailsInfo_SP"))
                            {
                                foreach (GatePassItemBO rd in AddedItem)
                                {
                                    cmdGatePassDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@GatePassId", DbType.Int64, gatepass.GatePassId);
                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@CostCenterId", DbType.Int32, rd.CostCenterId);
                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@StockById", DbType.Int32, rd.StockById);
                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@Quantity", DbType.Decimal, rd.Quantity);
                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@Description", DbType.String, rd.Description);
                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@ReturnType", DbType.Int32, rd.ReturnType);
                                    dbSmartAspects.AddInParameter(cmdGatePassDetails, "@ReturnDate", DbType.DateTime, rd.ReturnDate);
                                    dbSmartAspects.AddOutParameter(cmdGatePassDetails, "@GatePassItemId", DbType.Int64, sizeof(Int64));

                                    status = dbSmartAspects.ExecuteNonQuery(cmdGatePassDetails);
                                    tmpGatePassDetailsId = Convert.ToInt64(cmdGatePassDetails.Parameters["@GatePassItemId"].Value);
                                }
                            }
                        }

                        if (status > 0 && deletedItem.Count > 0)
                        {
                            //done
                            using (DbCommand cmdDelete = dbSmartAspects.GetStoredProcCommand("DeleteGatePassDetailsInfo_SP"))
                            {
                                foreach (GatePassItemBO rd in deletedItem)
                                {
                                    cmdDelete.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdDelete, "@GatePassItemId", DbType.Int64, rd.GatePassItemId);
                                    dbSmartAspects.AddInParameter(cmdDelete, "@GatePassId", DbType.Int64, gatepass.GatePassId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdDelete);
                                }
                            }
                        }

                        if (status > 0 && editedItem.Count > 0)
                        {
                            //done
                            using (DbCommand cmdEdit = dbSmartAspects.GetStoredProcCommand("UpdateGatePassDetailsInfo_SP"))
                            {
                                foreach (GatePassItemBO rd in editedItem)
                                {
                                    cmdEdit.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdEdit, "@GatePassItemId", DbType.Int64, rd.GatePassItemId);
                                    dbSmartAspects.AddInParameter(cmdEdit, "@GatePassId", DbType.Int64, gatepass.GatePassId);
                                    dbSmartAspects.AddInParameter(cmdEdit, "@CostCenterId", DbType.Int32, rd.CostCenterId);
                                    dbSmartAspects.AddInParameter(cmdEdit, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdEdit, "@StockById", DbType.Int32, rd.StockById);
                                    dbSmartAspects.AddInParameter(cmdEdit, "@Quantity", DbType.Decimal, rd.Quantity);
                                    dbSmartAspects.AddInParameter(cmdEdit, "@Description", DbType.String, rd.Description);
                                    dbSmartAspects.AddInParameter(cmdEdit, "@ReturnType", DbType.Int32, rd.ReturnType);
                                    dbSmartAspects.AddInParameter(cmdEdit, "@ReturnDate", DbType.DateTime, rd.ReturnDate);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdEdit);
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
                    catch
                    {
                        transction.Rollback();
                        retVal = false;
                        throw;
                    }
                }

            }

            return retVal;
        }
        public GatePassBO GetGatePassInfoByID(long gatePassId)
        {
            GatePassBO gatePass = new GatePassBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGatePassInfoByID_SP"))
                {
                    //done
                    dbSmartAspects.AddInParameter(cmd, "@GatePassId", DbType.Int64, gatePassId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                gatePass.GatePassId = Convert.ToInt64(reader["GatePassId"]);
                                gatePass.GatePassNumber = reader["GatePassNumber"].ToString();
                                gatePass.GatePassDate = Convert.ToDateTime(reader["GatePassDate"]);
                                gatePass.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                gatePass.Supplier = reader["Supplier"].ToString();
                                gatePass.Remarks = reader["Remarks"].ToString();
                                gatePass.ResponsiblePersonId = Convert.ToInt32(reader["ResponsiblePersonId"]);
                                gatePass.ApprovedBy = Convert.ToInt32(reader["ApprovedBy"]);
                                gatePass.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                gatePass.Status = reader["Status"].ToString();
                            }
                        }
                    }
                }
            }
            return gatePass;
        }
        public List<GatePassItemBO> GetGatePassDetailsByID(long gatePassId)
        {
            List<GatePassItemBO> gatePassItems = new List<GatePassItemBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                //done
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGatePassDetailsByID_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GatePassId", DbType.Int64, gatePassId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GatePassItemBO gatePassItem = new GatePassItemBO
                                {
                                    GatePassItemId = Convert.ToInt64(reader["GatePassItemId"]),
                                    GatePassId = Convert.ToInt64(reader["GatePassId"]),
                                    Description = reader["Description"].ToString(),
                                    CostCenterId = Convert.ToInt32(reader["CostCenterId"]),
                                    CostCenter = reader["CostCenter"].ToString(),
                                    ItemId = Convert.ToInt32(reader["ItemId"]),
                                    ItemName = reader["ItemName"].ToString(),
                                    ItemCode = reader["ItemCode"].ToString(),
                                    StockById = Convert.ToInt32(reader["StockById"]),
                                    StockBy = reader["StockBy"].ToString(),
                                    Quantity = Convert.ToDecimal(reader["Quantity"]),
                                    ReturnDate = Convert.ToDateTime(reader["ReturnDate"]),
                                    ReturnType = Convert.ToInt32(reader["ReturnType"]),
                                    Status = reader["Status"].ToString(),
                                    ApprovedQuantity = Convert.ToDecimal(reader["ApprovedQuantity"])
                                };
                                gatePassItem.ReturnTypeName = gatePassItem.ReturnType == 1 ? "Returnable" : "Non Returnable";
                                gatePassItems.Add(gatePassItem);
                            }
                        }
                    }
                }
            }
            return gatePassItems;
        }
        public List<GatePassViewForCheckOrApproveBO> GetGatePassBySearchCriteriaForPaging(DateTime FromDate, DateTime ToDate, string status, int userInfoId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<GatePassViewForCheckOrApproveBO> gatePassList = new List<GatePassViewForCheckOrApproveBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGatePassBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, FromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, ToDate);
                    dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, status);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userInfoId);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GatePassViewForCheckOrApproveBO gatePassViewBO = new GatePassViewForCheckOrApproveBO();

                                gatePassViewBO.GatePassId = Convert.ToInt64(reader["GatePassId"]);
                                gatePassViewBO.GatePassNumber = reader["GatePassNumber"].ToString();
                                gatePassViewBO.GatePassDate = Convert.ToDateTime(reader["GatePassDate"]);
                                gatePassViewBO.SupplierId = Convert.ToInt32(reader["SupplierId"]);
                                gatePassViewBO.Remarks = reader["Remarks"].ToString();
                                gatePassViewBO.ApprovedBy = Convert.ToInt32(reader["ApprovedBy"]);
                                gatePassViewBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                gatePassViewBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                gatePassViewBO.LastModifiedBy = Convert.ToInt32(reader["LastModifiedBy"]);
                                gatePassViewBO.CheckedBy = Convert.ToInt32(reader["CheckedBy"]);
                                gatePassViewBO.CreatedByPerson = reader["CreatedByName"].ToString();
                                gatePassViewBO.IsCanEdit = Convert.ToBoolean(reader["IsCanEdit"]);
                                gatePassViewBO.IsCanDelete = Convert.ToBoolean(reader["IsCanDelete"]);
                                gatePassViewBO.IsCanChecked = Convert.ToBoolean(reader["IsCanChecked"]);
                                gatePassViewBO.IsCanApproved = Convert.ToBoolean(reader["IsCanApproved"]);

                                gatePassList.Add(gatePassViewBO);
                            }

                        }
                    }

                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }

            return gatePassList;
        }
        public bool CheckOrApproveGatePassInfo(GatePassBO gatepass, List<GatePassItemBO> approvedItem, List<GatePassItemBO> cancelItem)
        {
            bool retVal = false;
            int status = 0;
            int tmpRequisitionDetailsId = 0;
            string updateStatus = "";
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CheckOrApproveGatePassInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@GatePassId", DbType.Int64, gatepass.GatePassId);
                            dbSmartAspects.AddInParameter(command, "@Status", DbType.String, gatepass.Status);
                            if (gatepass.CheckedBy != null)
                                dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, gatepass.CheckedBy);
                            else if (gatepass.ApprovedBy != null)
                                dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, gatepass.ApprovedBy);
                            else
                                dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, DBNull.Value);

                            dbSmartAspects.AddOutParameter(command, "@UpdateStatus", DbType.String, 50);
                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            updateStatus = Convert.ToString(command.Parameters["@UpdateStatus"].Value);
                        }

                        if (status > 0 && approvedItem.Count > 0 && (updateStatus == "Checked" || updateStatus == "Approved"))
                        {
                            using (DbCommand cmdApproved = dbSmartAspects.GetStoredProcCommand("CheckOrApproveGatePassDetailsInfo_SP"))
                            {
                                foreach (GatePassItemBO rd in approvedItem)
                                {
                                    cmdApproved.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdApproved, "@GatePassId", DbType.Int64, gatepass.GatePassId);
                                    dbSmartAspects.AddInParameter(cmdApproved, "@GatePassItemId", DbType.Int64, rd.GatePassItemId);
                                    dbSmartAspects.AddInParameter(cmdApproved, "@ApprovedQuantity", DbType.Decimal, rd.ApprovedQuantity);
                                    dbSmartAspects.AddInParameter(cmdApproved, "@Status", DbType.String, gatepass.Status);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdApproved, transction);
                                }
                            }
                        }

                        if (status > 0 && cancelItem.Count > 0)
                        {
                            using (DbCommand cmdCancel = dbSmartAspects.GetStoredProcCommand("CheckOrApproveGatePassDetailsInfo_SP"))
                            {
                                foreach (GatePassItemBO rd in cancelItem)
                                {
                                    cmdCancel.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdCancel, "@GatePassId", DbType.Int64, gatepass.GatePassId);
                                    dbSmartAspects.AddInParameter(cmdCancel, "@GatePassItemId", DbType.Int64, rd.GatePassItemId);
                                    dbSmartAspects.AddInParameter(cmdCancel, "@ApprovedQuantity", DbType.Decimal, 0);
                                    dbSmartAspects.AddInParameter(cmdCancel, "@Status", DbType.String, gatepass.Status);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdCancel, transction);
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
        public bool DeleteGatePass(long gatePassId)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteGatePassInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@GatePassId", DbType.Int64, gatePassId);
                            status = dbSmartAspects.ExecuteNonQuery(command);
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
        public bool CancelGatePass(long gatePassId, int userInfoId)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CancelGatePassInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@GatePassId", DbType.Int64, gatePassId);
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, userInfoId);

                            status = dbSmartAspects.ExecuteNonQuery(command);
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
        public List<SupplierNCompanyInfoForGatePassInvoiceBO> GetGatePassInfoForInvoice(long gatePassId, int supplierId)
        {
            List<SupplierNCompanyInfoForGatePassInvoiceBO> gatePass = new List<SupplierNCompanyInfoForGatePassInvoiceBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGatePassDetailsForInvoice_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GatePassId", DbType.Int64, gatePassId);
                    dbSmartAspects.AddInParameter(cmd, "@SupplierId", DbType.Int32, supplierId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "GatePassInfo");
                    DataTable Table = ds.Tables["GatePassInfo"];

                    gatePass = Table.AsEnumerable().Select(r => new SupplierNCompanyInfoForGatePassInvoiceBO
                    {
                        GatePassId = r.Field<Int64>("GatePassId"),
                        GatePassNumber = r.Field<string>("GatePassNumber"),
                        GatePassDate = r.Field<DateTime>("GatePassDate"),
                        SupplierId = r.Field<int>("SupplierId"),
                        Remarks = r.Field<string>("Remarks"),
                        ItemId = r.Field<int?>("ItemId"),
                        ItemName = r.Field<string>("ItemName"),
                        Quantity = r.Field<decimal?>("Quantity"),
                        HeadName = r.Field<string>("HeadName"),
                        Description = r.Field<string>("Description"),

                        CreatedByName = r.Field<string>("CreatedByName"),
                        CheckedByName = r.Field<string>("CheckedByName"),
                        ApprovedByName = r.Field<string>("ApprovedByName")

                    }).ToList();
                }
            }

            return gatePass;
        }

    }
}
