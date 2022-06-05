using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.PurchaseManagment;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.Inventory;
using HotelManagement.Data.HMCommon;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Data.PurchaseManagment
{
    public class PMRequisitionDA : BaseService
    {
        public List<PMRequisitionBO> GetPMRequisitionInfo()
        {
            List<PMRequisitionBO> requisitionList = new List<PMRequisitionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMRequisitionInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMRequisitionBO requisitionBO = new PMRequisitionBO();
                                requisitionBO.RequisitionId = Convert.ToInt32(reader["RequisitionId"]);
                                requisitionBO.PRNumber = reader["PRNumber"].ToString();
                                //requisitionBO.ProductId = Convert.ToInt32(reader["ProductId"]);
                                //requisitionBO.ProductName = reader["ProductName"].ToString();
                                //requisitionBO.ProductCode = reader["ProductCode"].ToString();
                                requisitionBO.Remarks = reader["Remarks"].ToString();
                                //requisitionBO.Quantity = Convert.ToDecimal(reader["Quantity"].ToString());
                                requisitionBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                requisitionBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                if (!string.IsNullOrWhiteSpace(reader["ApprovedDate"].ToString()))
                                {
                                    requisitionBO.ApprovedDate = Convert.ToDateTime(reader["ApprovedDate"]);
                                }

                                requisitionBO.RequisitionBy = Convert.ToString(reader["RequisitionBy"]);
                                requisitionList.Add(requisitionBO);
                            }
                        }
                    }
                }
            }
            return requisitionList;
        }
        public List<PMRequisitionBO> GetApprovedPMRequisitionInfo()
        {
            List<PMRequisitionBO> requisitionList = new List<PMRequisitionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApprovedPMRequisitionInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMRequisitionBO requisitionBO = new PMRequisitionBO();
                                requisitionBO.RequisitionId = Convert.ToInt32(reader["RequisitionId"]);
                                requisitionBO.PRNumber = reader["PRNumber"].ToString();
                                requisitionBO.Remarks = reader["Remarks"].ToString();
                                requisitionBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                requisitionBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                if (!string.IsNullOrWhiteSpace(reader["ApprovedDate"].ToString()))
                                {
                                    requisitionBO.ApprovedDate = Convert.ToDateTime(reader["ApprovedDate"]);
                                }

                                requisitionBO.RequisitionBy = Convert.ToString(reader["RequisitionBy"]);
                                requisitionList.Add(requisitionBO);
                            }
                        }
                    }
                }
            }
            return requisitionList;
        }
        public List<PMRequisitionBO> GetApprovedNNotDeliveredRequisitionInfo()
        {
            List<PMRequisitionBO> requisitionList = new List<PMRequisitionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApprovedNNotDeliveredRequisitionInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMRequisitionBO requisitionBO = new PMRequisitionBO();
                                requisitionBO.RequisitionId = Convert.ToInt32(reader["RequisitionId"]);
                                requisitionBO.PRNumber = reader["PRNumber"].ToString();
                                requisitionBO.Remarks = reader["Remarks"].ToString();
                                requisitionBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                requisitionBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                if (!string.IsNullOrWhiteSpace(reader["ApprovedDate"].ToString()))
                                {
                                    requisitionBO.ApprovedDate = Convert.ToDateTime(reader["ApprovedDate"]);
                                }

                                requisitionBO.FromCostCenterId = Convert.ToInt32(reader["FromCostCenterId"]);
                                requisitionBO.ToCostCenterId = Convert.ToInt32(reader["ToCostCenterId"]);
                                requisitionBO.RequisitionBy = Convert.ToString(reader["RequisitionBy"]);
                                requisitionBO.FromCostCenterId = Convert.ToInt32(reader["FromCostCenterId"]);
                                requisitionBO.ToCostCenterId = Convert.ToInt32(reader["ToCostCenterId"]);

                                requisitionList.Add(requisitionBO);
                            }
                        }
                    }
                }
            }
            return requisitionList;
        }
        public List<PMRequisitionBO> GetApprovedNNotDeliveredRequisitionForOut()
        {
            List<PMRequisitionBO> requisitionList = new List<PMRequisitionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetApprovedNNotDeliveredRequisitionForOut_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMRequisitionBO requisitionBO = new PMRequisitionBO();
                                requisitionBO.RequisitionId = Convert.ToInt32(reader["RequisitionId"]);
                                requisitionBO.PRNumber = reader["PRNumber"].ToString();
                                requisitionBO.Remarks = reader["Remarks"].ToString();
                                requisitionBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                requisitionBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                if (!string.IsNullOrWhiteSpace(reader["ApprovedDate"].ToString()))
                                {
                                    requisitionBO.ApprovedDate = Convert.ToDateTime(reader["ApprovedDate"]);
                                }

                                requisitionBO.RequisitionBy = Convert.ToString(reader["RequisitionBy"]);
                                requisitionList.Add(requisitionBO);
                            }
                        }
                    }
                }
            }
            return requisitionList;
        }
        public List<PMRequisitionBO> GetPMRequisitionInfoBySearchCriteriaWithUser(DateTime FromDate, DateTime ToDate, string status, string searchType, int userID)
        {
            List<PMRequisitionBO> requisitionList = new List<PMRequisitionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMRequisitionInfoBySearchCriteriaWithUser_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, FromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, ToDate);
                    dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, status);
                    dbSmartAspects.AddInParameter(cmd, "@SearchType", DbType.String, searchType);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userID);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMRequisitionBO requisitionBO = new PMRequisitionBO();

                                requisitionBO.RequisitionId = Convert.ToInt32(reader["RequisitionId"]);
                                requisitionBO.PRNumber = reader["PRNumber"].ToString();
                                requisitionBO.FromCostCenterId = Convert.ToInt32(reader["FromCostCenterId"]);
                                requisitionBO.FromCostCenter = reader["CostCenter"].ToString();
                                requisitionBO.ToCostCenter = reader["ToCostCenter"].ToString();

                                requisitionBO.Remarks = reader["Remarks"].ToString();
                                requisitionBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                requisitionBO.ApprovedStatus = reader["ApprovedStatus"].ToString();

                                if (!string.IsNullOrWhiteSpace(reader["ApprovedDate"].ToString()))
                                {
                                    requisitionBO.ApprovedDate = Convert.ToDateTime(reader["ApprovedDate"]);
                                }

                                requisitionBO.RequisitionBy = Convert.ToString(reader["RequisitionBy"]);
                                requisitionBO.POStatus = Convert.ToString(reader["POStatus"]);
                                requisitionBO.ReceiveStatus = Convert.ToString(reader["ReceiveStatus"]);
                                requisitionBO.CheckedBy = Convert.ToInt32(reader["CheckedBy"]);
                                requisitionBO.ApprovedBy = Convert.ToInt32(reader["ApprovedBy"]);
                                requisitionBO.IsCanEdit = Convert.ToBoolean(reader["IsCanEdit"]);
                                requisitionBO.IsCanDelete = Convert.ToBoolean(reader["IsCanDelete"]);
                                requisitionBO.IsCanCheckeAble = Convert.ToInt32(reader["IsCanChecked"]);
                                requisitionBO.IsCanApproveAble = Convert.ToInt32(reader["IsCanApproved"]);
                                requisitionList.Add(requisitionBO);
                            }
                        }
                    }
                }
            }
            return requisitionList;
        }
        public List<PMRequisitionBO> GetPMRequisitionInfoBySearchCriteriaWithUser(DateTime FromDate, DateTime ToDate, string status, string searchType, int userID, int companyId, int projectId)
        {
            List<PMRequisitionBO> requisitionList = new List<PMRequisitionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMRequisitionInfoBySearchCriteriaWithCompanyProject_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, FromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, ToDate);
                    dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, status);
                    dbSmartAspects.AddInParameter(cmd, "@SearchType", DbType.String, searchType);
                    dbSmartAspects.AddInParameter(cmd, "@UserInfoId", DbType.Int32, userID);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, DBNull.Value);
                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMRequisitionBO requisitionBO = new PMRequisitionBO();

                                requisitionBO.RequisitionId = Convert.ToInt32(reader["RequisitionId"]);
                                requisitionBO.PRNumber = reader["PRNumber"].ToString();
                                requisitionBO.FromCostCenterId = Convert.ToInt32(reader["FromCostCenterId"]);
                                requisitionBO.FromCostCenter = reader["CostCenter"].ToString();
                                requisitionBO.ToCostCenter = reader["ToCostCenter"].ToString();

                                requisitionBO.Remarks = reader["Remarks"].ToString();
                                requisitionBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                requisitionBO.ApprovedStatus = reader["ApprovedStatus"].ToString();

                                if (!string.IsNullOrWhiteSpace(reader["ApprovedDate"].ToString()))
                                {
                                    requisitionBO.ApprovedDate = Convert.ToDateTime(reader["ApprovedDate"]);
                                }

                                requisitionBO.RequisitionBy = Convert.ToString(reader["RequisitionBy"]);
                                requisitionBO.POStatus = Convert.ToString(reader["POStatus"]);
                                requisitionBO.ReceiveStatus = Convert.ToString(reader["ReceiveStatus"]);

                                requisitionBO.CompanyName = reader["CompanyName"].ToString();
                                requisitionBO.ProjectName = reader["ProjectName"].ToString();

                                requisitionBO.CheckedBy = Convert.ToInt32(reader["CheckedBy"]);
                                requisitionBO.ApprovedBy = Convert.ToInt32(reader["ApprovedBy"]);
                                requisitionBO.IsCanCheckeAble = Convert.ToInt32(reader["IsCanChecked"]);
                                requisitionBO.IsCanApproveAble = Convert.ToInt32(reader["IsCanApproved"]);
                                requisitionList.Add(requisitionBO);
                            }
                        }
                    }
                }
            }
            return requisitionList;
        }
        public List<PMRequisitionBO> GetPMRequisitionInfoBySearchCriteria(DateTime FromDate, DateTime ToDate, string status, string searchType)
        {
            List<PMRequisitionBO> requisitionList = new List<PMRequisitionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMRequisitionInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, FromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, ToDate);
                    dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, status);
                    dbSmartAspects.AddInParameter(cmd, "@SearchType", DbType.String, searchType);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMRequisitionBO requisitionBO = new PMRequisitionBO();

                                requisitionBO.RequisitionId = Convert.ToInt32(reader["RequisitionId"]);
                                requisitionBO.PRNumber = reader["PRNumber"].ToString();
                                requisitionBO.FromCostCenterId = Convert.ToInt32(reader["FromCostCenterId"]);
                                requisitionBO.FromCostCenter = reader["CostCenter"].ToString();
                                requisitionBO.ToCostCenter = reader["ToCostCenter"].ToString();

                                requisitionBO.Remarks = reader["Remarks"].ToString();
                                requisitionBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                requisitionBO.ApprovedStatus = reader["ApprovedStatus"].ToString();

                                if (!string.IsNullOrWhiteSpace(reader["ApprovedDate"].ToString()))
                                {
                                    requisitionBO.ApprovedDate = Convert.ToDateTime(reader["ApprovedDate"]);
                                }

                                requisitionBO.RequisitionBy = Convert.ToString(reader["RequisitionBy"]);
                                requisitionBO.POStatus = Convert.ToString(reader["POStatus"]);
                                requisitionBO.ReceiveStatus = Convert.ToString(reader["ReceiveStatus"]);
                                requisitionBO.CheckedBy = Convert.ToInt32(reader["CheckedBy"]);
                                requisitionBO.ApprovedBy = Convert.ToInt32(reader["ApprovedBy"]);
                                requisitionList.Add(requisitionBO);
                            }
                        }
                    }
                }
            }
            return requisitionList;
        }
        public List<PMRequisitionViewBO> GetPMRequisitionInfoByStatus(DateTime? FromDate, DateTime? ToDate, string requisitionNo, string status, int userInfoId, int companyId, int projectId, int requisitionBy, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<PMRequisitionViewBO> requisitionList = new List<PMRequisitionViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMRequisitionInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, FromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, ToDate);

                    dbSmartAspects.AddInParameter(cmd, "@RequisitionNo", DbType.String, requisitionNo);

                    if (!string.IsNullOrEmpty(status))
                        dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, status);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, DBNull.Value);

                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, DBNull.Value);

                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, DBNull.Value);

                    if (requisitionBy != 0)
                        dbSmartAspects.AddInParameter(cmd, "@RequisitionBy", DbType.Int64, requisitionBy);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@RequisitionBy", DbType.Int64, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, DBNull.Value);
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
                                PMRequisitionViewBO requisitionBO = new PMRequisitionViewBO();

                                requisitionBO.RequisitionId = Convert.ToInt32(reader["RequisitionId"]);
                                requisitionBO.PRNumber = reader["PRNumber"].ToString();
                                requisitionBO.FromCostCenterId = Convert.ToInt32(reader["FromCostCenterId"]);
                                if (!string.IsNullOrWhiteSpace(reader["FormCostCenter"].ToString()))
                                    requisitionBO.FromCostCenter = reader["FormCostCenter"].ToString();
                                requisitionBO.ToCostCenter = reader["ToCostCenter"].ToString();
                                requisitionBO.Remarks = reader["Remarks"].ToString();
                                requisitionBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                requisitionBO.ApprovedStatus = reader["ApprovedStatus"].ToString();

                                requisitionBO.POStatus = reader["POStatus"].ToString();
                                requisitionBO.ReceiveStatus = reader["ReceiveStatus"].ToString();
                                requisitionBO.TransferStatus = reader["TransferStatus"].ToString();

                                requisitionBO.CompanyName = reader["CompanyName"].ToString();
                                requisitionBO.ProjectName = reader["ProjectName"].ToString();

                                if (!string.IsNullOrWhiteSpace(reader["ApprovedDate"].ToString()))
                                {
                                    requisitionBO.ApprovedDate = Convert.ToDateTime(reader["ApprovedDate"]);
                                }
                                if (!string.IsNullOrWhiteSpace(reader["CreatedDate"].ToString()))
                                {
                                    requisitionBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                }
                                requisitionBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                requisitionBO.CreatedByName = Convert.ToString(reader["CreatedByName"]);
                                requisitionBO.RequisitionBy = Convert.ToString(reader["RequisitionBy"]);
                                requisitionBO.CheckedBy = Convert.ToInt32(reader["CheckedBy"]);
                                requisitionBO.ApprovedBy = Convert.ToInt32(reader["ApprovedBy"]);

                                requisitionBO.IsCanEdit = Convert.ToBoolean(reader["IsCanEdit"]);
                                requisitionBO.IsCanDelete = Convert.ToBoolean(reader["IsCanDelete"]);
                                requisitionBO.IsCanChecked = Convert.ToBoolean(reader["IsCanChecked"]);
                                requisitionBO.IsCanApproved = Convert.ToBoolean(reader["IsCanApproved"]);

                                requisitionList.Add(requisitionBO);
                            }
                        }
                    }

                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }
            return requisitionList;
        }
        public PMRequisitionBO GetPMRequisitionInfoByID(int RequisitionId)
        {
            PMRequisitionBO requisitionBO = new PMRequisitionBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMRequisitionInfoByID_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RequisitionId", DbType.Int32, RequisitionId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                requisitionBO.RequisitionId = Convert.ToInt32(reader["RequisitionId"]);
                                requisitionBO.PRNumber = reader["PRNumber"].ToString();
                                requisitionBO.FromCostCenterId = Convert.ToInt32(reader["FromCostCenterId"]);
                                requisitionBO.FromLocationId = Convert.ToInt32(reader["FromLocationId"]);
                                requisitionBO.ToCostCenterId = Convert.ToInt32(reader["ToCostCenterId"]);
                                if (reader["ToLocationId"] != DBNull.Value)
                                    requisitionBO.ToLocationId = Convert.ToInt32(reader["ToLocationId"]);
                                requisitionBO.FromCostCenter = Convert.ToString(reader["FromCostCenter"]);
                                requisitionBO.ToCostCenter = Convert.ToString(reader["ToCostCenter"]);
                                requisitionBO.Remarks = reader["Remarks"].ToString();
                                requisitionBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                requisitionBO.RequisitionBy = Convert.ToString(reader["RequisitionBy"]);
                                requisitionBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                requisitionBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                requisitionBO.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                requisitionBO.CheckedBy = Convert.ToInt32(reader["CheckedBy"]);
                                requisitionBO.ApprovedBy = Convert.ToInt32(reader["ApprovedBy"]);
                                requisitionBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                            }
                        }
                    }
                }
            }
            return requisitionBO;
        }
        public List<PMRequisitionDetailsBO> GetPMRequisitionDetailsByID(int RequisitionId)
        {
            List<PMRequisitionDetailsBO> requisitionBO = new List<PMRequisitionDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMRequisitionDetailsByID_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RequisitionId", DbType.Int32, RequisitionId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RequsitionDetails");
                    DataTable Table = ds.Tables["RequsitionDetails"];

                    requisitionBO = Table.AsEnumerable().Select(r => new PMRequisitionDetailsBO
                    {
                        RequisitionDetailsId = r.Field<long>("RequisitionDetailsId"),
                        RequisitionId = r.Field<Int32>("RequisitionId"),
                        RequisitionRemarks = r.Field<string>("RequisitionRemarks"),
                        ItemRemarks = r.Field<string>("ItemRemarks"),
                        CategoryId = r.Field<Int32>("CategoryId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemName = r.Field<string>("Name"),
                        StockById = r.Field<Int32>("StockById"),
                        HeadName = r.Field<string>("HeadName"),
                        Quantity = r.Field<decimal>("Quantity"),
                        ApprovedQuantity = r.Field<decimal?>("ApprovedQuantity"),
                        DeliveredQuantity = r.Field<decimal?>("DeliveredQuantity"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        InitialApprovedStatus = r.Field<string>("InitialApprovedStatus"),
                        DeliverStatus = r.Field<string>("DeliverStatus"),


                        ColorId = r.Field<Int32>("ColorId"),
                        SizeId = r.Field<Int32>("SizeId"),
                        StyleId = r.Field<Int32>("StyleId"),
                        ColorText = r.Field<string>("ColorText"),
                        SizeText = r.Field<string>("SizeText"),
                        StyleText = r.Field<string>("StyleText"),

                        CurrentStockFromStore = r.Field<decimal?>("CurrentStockFromStore"),
                        RequisitionBy = r.Field<string>("RequisitionBy"),
                        CheckedBy = r.Field<string>("CheckedBy"),
                        LastTransferQuantity = r.Field<decimal?>("LastTransferQuantity"),
                        LastTransferType = r.Field<string>("LastTransferType"),
                        LastRequisitionQuantity = r.Field<decimal?>("LastRequisitionQuantity"),

                    }).ToList();
                }
            }
            return requisitionBO;
        }
        public List<PMRequisitionDetailsBO> GetPMRequisitionDetailsByIDForOut(int RequisitionId, Int64? locationId)
        {
            List<PMRequisitionDetailsBO> requisitionBO = new List<PMRequisitionDetailsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMRequisitionDetailsByIDForOut_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RequisitionId", DbType.Int32, RequisitionId);
                    dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int64, locationId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RequsitionDetails");
                    DataTable Table = ds.Tables["RequsitionDetails"];

                    requisitionBO = Table.AsEnumerable().Select(r => new PMRequisitionDetailsBO
                    {
                        RequisitionDetailsId = r.Field<long>("RequisitionDetailsId"),
                        RequisitionId = r.Field<Int32>("RequisitionId"),
                        CategoryId = r.Field<Int32>("CategoryId"),
                        ItemId = r.Field<Int32>("ItemId"),
                        ItemName = r.Field<string>("Name"),
                        ProductType = r.Field<string>("ProductType"),
                        StockById = r.Field<Int32>("StockById"),
                        HeadName = r.Field<string>("HeadName"),
                        Quantity = r.Field<decimal>("Quantity"),
                        ApprovedQuantity = r.Field<decimal?>("ApprovedQuantity"),
                        DeliveredQuantity = r.Field<decimal?>("DeliveredQuantity"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        DeliverStatus = r.Field<string>("DeliverStatus"),
                        StockQuantity = r.Field<decimal>("StockQuantity"),
                        OutDetailsId = r.Field<Int32>("OutDetailsId"),
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
            return requisitionBO;
        }
        public bool SavePMRequisitionInfo(PMRequisitionBO requsition, List<PMRequisitionDetailsBO> AddedItem, out int tmpRequisitionId, out string requisitionNumber, out string TransactionNo, out string TransactionType, out string ApproveStatus)
        {
            bool retVal = false;
            int status = 0;
            int tmpRequisitionDetailsId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePMRequisitionInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@FromCostCenterId", DbType.String, requsition.FromCostCenterId);
                            dbSmartAspects.AddInParameter(command, "@ToCostCenterId", DbType.String, requsition.ToCostCenterId); 
                            dbSmartAspects.AddInParameter(command, "@FromLocationId", DbType.Int32, requsition.FromLocationId);
                            dbSmartAspects.AddInParameter(command, "@ToLocationId", DbType.Int32, requsition.ToLocationId);
                            dbSmartAspects.AddInParameter(command, "@ReceivedByDate", DbType.DateTime, requsition.ReceivedByDate);
                            dbSmartAspects.AddInParameter(command, "@RequisitionBy", DbType.String, requsition.RequisitionBy);
                            dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, requsition.ApprovedStatus);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, requsition.Remarks);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, requsition.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, requsition.ProjectId);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, requsition.CreatedBy);
                            dbSmartAspects.AddInParameter(command, "@CheckedBy", DbType.Int32, requsition.CheckedBy);
                            dbSmartAspects.AddInParameter(command, "@ApprovedBy", DbType.Int32, requsition.ApprovedBy);
                            dbSmartAspects.AddOutParameter(command, "@RequisitionId", DbType.Int32, sizeof(Int32));
                            dbSmartAspects.AddOutParameter(command, "@RequisitionNumber", DbType.String, 50);
                            dbSmartAspects.AddOutParameter(command, "@TransactionNo", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(command, "@TransactionType", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(command, "@ApproveStatus", DbType.String, 100);

                            

                            status = dbSmartAspects.ExecuteNonQuery(command);
                            tmpRequisitionId = Convert.ToInt32(command.Parameters["@RequisitionId"].Value);
                            requisitionNumber = command.Parameters["@RequisitionNumber"].Value.ToString();

                            TransactionNo = command.Parameters["@TransactionNo"].Value.ToString();
                            TransactionType = command.Parameters["@TransactionType"].Value.ToString();
                            ApproveStatus = command.Parameters["@ApproveStatus"].Value.ToString();
                        }

                        if (status > 0)
                        {
                            using (DbCommand cmdRequsitionDetails = dbSmartAspects.GetStoredProcCommand("SavePMRequisitionDetailsInfo_SP"))
                            {
                                foreach (PMRequisitionDetailsBO rd in AddedItem)
                                {
                                    cmdRequsitionDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@RequisitionId", DbType.Int32, tmpRequisitionId);
                                    dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@CategoryId", DbType.Int32, rd.CategoryId);
                                    dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@StockById", DbType.Int32, rd.StockById);
                                    dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@Quantity", DbType.Decimal, rd.Quantity);
                                    if (rd.ItemRemarks == "")
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@Remarks", DbType.String, DBNull.Value);
                                    else
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@Remarks", DbType.String, rd.ItemRemarks);

                                    if (requsition.ApprovedStatus == "Approved")
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@ApprovedQuantity", DbType.Decimal, rd.Quantity);
                                    else
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@ApprovedQuantity", DbType.Decimal, DBNull.Value);


                                    if (rd.ColorId != 0)
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@ColorId", DbType.Int32, rd.ColorId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@ColorId", DbType.Int32, DBNull.Value);
                                    if (rd.SizeId != 0)
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@SizeId", DbType.Int32, rd.SizeId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@SizeId", DbType.Int32, DBNull.Value);

                                    if (rd.StyleId != 0)
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@StyleId", DbType.Int32, rd.StyleId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@StyleId", DbType.Int32, DBNull.Value);



                                    dbSmartAspects.AddOutParameter(cmdRequsitionDetails, "@RequisitionDetailsId", DbType.Int32, sizeof(Int32));

                                    status = dbSmartAspects.ExecuteNonQuery(cmdRequsitionDetails);
                                    tmpRequisitionDetailsId = Convert.ToInt32(cmdRequsitionDetails.Parameters["@RequisitionDetailsId"].Value);
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
        public bool UpdatePMRequisitionInfo(PMRequisitionBO requsition, List<PMRequisitionDetailsBO> AddedItem, List<PMRequisitionDetailsBO> editedItem, List<PMRequisitionDetailsBO> deletedItem, out string TransactionNo, out string TransactionType, out string ApproveStatus)
        {
            bool retVal = false;
            int status = 0;
            int tmpRequisitionDetailsId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePMRequisitionInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@RequisitionId", DbType.Int32, requsition.RequisitionId);
                            dbSmartAspects.AddInParameter(command, "@FromCostCenterId", DbType.Int32, requsition.FromCostCenterId);
                            dbSmartAspects.AddInParameter(command, "@ToCostCenterId", DbType.Int32, requsition.ToCostCenterId);
                            dbSmartAspects.AddInParameter(command, "@FromLocationId", DbType.Int32, requsition.FromLocationId);
                            dbSmartAspects.AddInParameter(command, "@ToLocationId", DbType.Int32, requsition.ToLocationId);
                            dbSmartAspects.AddInParameter(command, "@ReceivedByDate", DbType.DateTime, requsition.ReceivedByDate);
                            dbSmartAspects.AddInParameter(command, "@RequisitionBy", DbType.String, requsition.RequisitionBy);
                            //dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, requsition.ApprovedStatus);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, requsition.Remarks);
                            dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int64, requsition.CompanyId);
                            dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.Int64, requsition.ProjectId);
                            dbSmartAspects.AddInParameter(command, "@CheckedBy", DbType.Int32, requsition.CheckedBy);
                            dbSmartAspects.AddInParameter(command, "@ApprovedBy", DbType.Int32, requsition.ApprovedBy);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, requsition.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(command, "@TransactionNo", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(command, "@TransactionType", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(command, "@ApproveStatus", DbType.String, 100);

                            status = dbSmartAspects.ExecuteNonQuery(command);

                            TransactionNo = command.Parameters["@TransactionNo"].Value.ToString();
                            TransactionType = command.Parameters["@TransactionType"].Value.ToString();
                            ApproveStatus = command.Parameters["@ApproveStatus"].Value.ToString();
                        }

                        if (status > 0 && AddedItem.Count > 0)
                        {
                            using (DbCommand cmdRequsitionDetails = dbSmartAspects.GetStoredProcCommand("SavePMRequisitionDetailsInfo_SP"))
                            {
                                foreach (PMRequisitionDetailsBO rd in AddedItem)
                                {
                                    cmdRequsitionDetails.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@RequisitionId", DbType.Int32, requsition.RequisitionId);
                                    dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@CategoryId", DbType.Int32, rd.CategoryId);
                                    dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@StockById", DbType.Int32, rd.StockById);
                                    dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@Quantity", DbType.Decimal, rd.Quantity);
                                    if (rd.ItemRemarks == "")
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@Remarks", DbType.String, DBNull.Value);
                                    else
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@Remarks", DbType.String, rd.ItemRemarks);


                                    if (requsition.ApprovedStatus == "Approved")
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@ApprovedQuantity", DbType.Decimal, rd.Quantity);
                                    else
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@ApprovedQuantity", DbType.Decimal, DBNull.Value);


                                    if (rd.ColorId != 0)
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@ColorId", DbType.Int32, rd.ColorId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@ColorId", DbType.Int32, DBNull.Value);
                                    if (rd.SizeId != 0)
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@SizeId", DbType.Int32, rd.SizeId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@SizeId", DbType.Int32, DBNull.Value);

                                    if (rd.StyleId != 0)
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@StyleId", DbType.Int32, rd.StyleId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdRequsitionDetails, "@StyleId", DbType.Int32, DBNull.Value);




                                    dbSmartAspects.AddOutParameter(cmdRequsitionDetails, "@RequisitionDetailsId", DbType.Int32, sizeof(Int32));

                                    status = dbSmartAspects.ExecuteNonQuery(cmdRequsitionDetails);
                                    tmpRequisitionDetailsId = Convert.ToInt32(cmdRequsitionDetails.Parameters["@RequisitionDetailsId"].Value);
                                }
                            }
                        }

                        if (status > 0 && deletedItem.Count > 0)
                        {
                            using (DbCommand cmdDelete = dbSmartAspects.GetStoredProcCommand("DeletePMRequisitionDetailsInfo_SP"))
                            {
                                foreach (PMRequisitionDetailsBO rd in deletedItem)
                                {
                                    cmdDelete.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdDelete, "@RequisitionDetailsId", DbType.Int32, rd.RequisitionDetailsId);
                                    dbSmartAspects.AddInParameter(cmdDelete, "@RequisitionId", DbType.Int32, requsition.RequisitionId);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdDelete);
                                }
                            }
                        }

                        if (status > 0 && editedItem.Count > 0)
                        {
                            using (DbCommand cmdEdit = dbSmartAspects.GetStoredProcCommand("UpdatePMRequisitionDetailsInfo_SP"))
                            {
                                foreach (PMRequisitionDetailsBO rd in editedItem)
                                {
                                    cmdEdit.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdEdit, "@RequisitionDetailsId", DbType.Int32, rd.RequisitionDetailsId);
                                    dbSmartAspects.AddInParameter(cmdEdit, "@RequisitionId", DbType.Int32, requsition.RequisitionId);
                                    dbSmartAspects.AddInParameter(cmdEdit, "@CategoryId", DbType.Int32, rd.CategoryId);
                                    dbSmartAspects.AddInParameter(cmdEdit, "@ItemId", DbType.Int32, rd.ItemId);
                                    dbSmartAspects.AddInParameter(cmdEdit, "@StockById", DbType.Int32, rd.StockById);
                                    dbSmartAspects.AddInParameter(cmdEdit, "@Quantity", DbType.Decimal, rd.Quantity);
                                    dbSmartAspects.AddInParameter(cmdEdit, "@Remarks", DbType.String, rd.ItemRemarks);

                                    if (rd.ColorId != 0)
                                        dbSmartAspects.AddInParameter(cmdEdit, "@ColorId", DbType.Int32, rd.ColorId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdEdit, "@ColorId", DbType.Int32, DBNull.Value);
                                    if (rd.SizeId != 0)
                                        dbSmartAspects.AddInParameter(cmdEdit, "@SizeId", DbType.Int32, rd.SizeId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdEdit, "@SizeId", DbType.Int32, DBNull.Value);

                                    if (rd.StyleId != 0)
                                        dbSmartAspects.AddInParameter(cmdEdit, "@StyleId", DbType.Int32, rd.StyleId);
                                    else
                                        dbSmartAspects.AddInParameter(cmdEdit, "@StyleId", DbType.Int32, DBNull.Value);



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
        public bool DeletePMRequisitionInfo(int requisitionId)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeletePMRequisitionInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@RequisitionId", DbType.Int32, requisitionId);
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
        public bool CancelPMRequisitionInfo(int requisitionId,int LastModifiedBy, out string TransactionNo, out string TransactionType, out string ApproveStatus)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CancelPMRequisitionInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@RequisitionId", DbType.Int32, requisitionId);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, LastModifiedBy);
                            dbSmartAspects.AddOutParameter(command, "@TransactionNo", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(command, "@TransactionType", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(command, "@ApproveStatus", DbType.String, 100);

                            status = dbSmartAspects.ExecuteNonQuery(command);

                            TransactionNo = command.Parameters["@TransactionNo"].Value.ToString();
                            TransactionType = command.Parameters["@TransactionType"].Value.ToString();
                            ApproveStatus = command.Parameters["@ApproveStatus"].Value.ToString();
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
        public bool ApprovePMRequisitionInfo(PMRequisitionBO requsition, List<PMRequisitionDetailsBO> ApprovedItem, List<PMRequisitionDetailsBO> PendingItem, out string TransactionNo, out string TransactionType, out string ApproveStatus)
        {
            bool retVal = false;
            int status = 0;
            int tmpRequisitionDetailsId = 0;
            string OutStatus = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePMRequisitionStatus_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@RequisitionId", DbType.Int32, requsition.RequisitionId);
                            dbSmartAspects.AddInParameter(command, "@ApprovedStatus", DbType.String, requsition.ApprovedStatus);
                            if (requsition.CheckedBy != 0)
                                dbSmartAspects.AddInParameter(command, "@CheckedBy", DbType.Int32, requsition.CheckedBy);
                            else
                                dbSmartAspects.AddInParameter(command, "@CheckedBy", DbType.Int32, DBNull.Value);
                            if (requsition.ApprovedBy != 0)
                                dbSmartAspects.AddInParameter(command, "@ApprovedBy", DbType.Int32, requsition.ApprovedBy);
                            else
                                dbSmartAspects.AddInParameter(command, "@ApprovedBy", DbType.Int32, DBNull.Value);
                            dbSmartAspects.AddOutParameter(command, "@OutStatus", DbType.String, 50);

                            dbSmartAspects.AddOutParameter(command, "@TransactionNo", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(command, "@TransactionType", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(command, "@ApproveStatus", DbType.String, 100);

                            
                            status = dbSmartAspects.ExecuteNonQuery(command, transction);

                            OutStatus = Convert.ToString(command.Parameters["@OutStatus"].Value);

                            TransactionNo = command.Parameters["@TransactionNo"].Value.ToString();
                            TransactionType = command.Parameters["@TransactionType"].Value.ToString();
                            ApproveStatus = command.Parameters["@ApproveStatus"].Value.ToString();
                        }

                        if (status > 0 && ApprovedItem.Count > 0)
                        {
                            using (DbCommand cmdApproved = dbSmartAspects.GetStoredProcCommand("UpdatePMRequisitionDetailsStatus_SP"))
                            {
                                foreach (PMRequisitionDetailsBO rd in ApprovedItem)
                                {
                                    cmdApproved.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdApproved, "@RequisitionDetailsId", DbType.Int32, rd.RequisitionDetailsId);
                                    dbSmartAspects.AddInParameter(cmdApproved, "@RequisitionId", DbType.Int32, requsition.RequisitionId);
                                    dbSmartAspects.AddInParameter(cmdApproved, "@ApprovedQuantity", DbType.Decimal, rd.ApprovedQuantity);
                                    dbSmartAspects.AddInParameter(cmdApproved, "@ApprovedStatus", DbType.String, OutStatus);
                                    if (rd.ItemRemarks == "")
                                        dbSmartAspects.AddInParameter(cmdApproved, "@Remarks", DbType.String, DBNull.Value);
                                    else
                                        dbSmartAspects.AddInParameter(cmdApproved, "@Remarks", DbType.String, rd.ItemRemarks);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdApproved, transction);
                                    tmpRequisitionDetailsId = Convert.ToInt32(cmdApproved.Parameters["@RequisitionDetailsId"].Value);
                                }
                            }
                        }

                        if (status > 0 && PendingItem.Count > 0)
                        {
                            using (DbCommand cmdPending = dbSmartAspects.GetStoredProcCommand("UpdatePMRequisitionDetailsStatus_SP"))
                            {
                                foreach (PMRequisitionDetailsBO rd in PendingItem)
                                {
                                    cmdPending.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(cmdPending, "@RequisitionDetailsId", DbType.Int32, rd.RequisitionDetailsId);
                                    dbSmartAspects.AddInParameter(cmdPending, "@RequisitionId", DbType.Int32, requsition.RequisitionId);
                                    dbSmartAspects.AddInParameter(cmdPending, "@ApprovedQuantity", DbType.Decimal, 0);
                                    dbSmartAspects.AddInParameter(cmdPending, "@ApprovedStatus", DbType.String, OutStatus);
                                    if (rd.ItemRemarks == "")
                                        dbSmartAspects.AddInParameter(cmdPending, "@Remarks", DbType.String, DBNull.Value);
                                    else
                                        dbSmartAspects.AddInParameter(cmdPending, "@Remarks", DbType.String, rd.ItemRemarks);

                                    status = dbSmartAspects.ExecuteNonQuery(cmdPending, transction);
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
        public bool RequisitionOrderApproval(int requisitionId, string approvedStatus, int checkedOrApprovedBy, out string TransactionNo, out string TransactionType, out string ApproveStatus)
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
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("RequisitionOrderApproval_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@RequisitionId", DbType.Int32, requisitionId);
                            dbSmartAspects.AddInParameter(commandMaster, "@ApprovedStatus", DbType.String, approvedStatus);
                            dbSmartAspects.AddInParameter(commandMaster, "@CheckedOrApprovedBy", DbType.Int32, checkedOrApprovedBy);

                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionNo", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@TransactionType", DbType.String, 100);
                            dbSmartAspects.AddOutParameter(commandMaster, "@ApproveStatus", DbType.String, 100);

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            TransactionNo = commandMaster.Parameters["@TransactionNo"].Value.ToString();
                            TransactionType = commandMaster.Parameters["@TransactionType"].Value.ToString();
                            ApproveStatus = commandMaster.Parameters["@ApproveStatus"].Value.ToString();
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
        public bool UpdatePMRequisitionQuantity(int requisitionId, decimal newQuantity)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePMRequisitionQuantity_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RequisitionId", DbType.Int32, requisitionId);
                    dbSmartAspects.AddInParameter(command, "@Quantity", DbType.Decimal, newQuantity);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<PMRequisitionBO> GetPMRequisitionDetailInfoById(int RequisitionId)
        {
            List<PMRequisitionBO> requisitionList = new List<PMRequisitionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMRequisitionDetailInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RequisitionId", DbType.Int32, RequisitionId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMRequisitionBO requisitionBO = new PMRequisitionBO();
                                requisitionBO.RequisitionId = Convert.ToInt32(reader["RequisitionId"]);
                                requisitionBO.PRNumber = reader["PRNumber"].ToString();
                                requisitionBO.FromCostCenterId = Convert.ToInt32(reader["FromCostCenterId"].ToString());
                                requisitionBO.StockById = Convert.ToInt32(reader["StockById"].ToString());
                                requisitionBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                requisitionBO.ItemName = reader["ItemName"].ToString();
                                requisitionBO.ItemCode = reader["ItemCode"].ToString();
                                requisitionBO.Remarks = reader["Remarks"].ToString();
                                requisitionBO.Quantity = Convert.ToDecimal(reader["Quantity"].ToString());
                                requisitionBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"].ToString());
                                requisitionBO.ApprovedStatus = reader["ApprovedStatus"].ToString();
                                if (!string.IsNullOrWhiteSpace(reader["ApprovedDate"].ToString()))
                                {
                                    requisitionBO.ApprovedDate = Convert.ToDateTime(reader["ApprovedDate"]);
                                }

                                requisitionBO.RequisitionBy = Convert.ToString(reader["RequisitionBy"]);
                                requisitionList.Add(requisitionBO);
                            }
                        }
                    }
                }
            }
            return requisitionList;
        }
        public InvItemBO GetInvItemInfoWithRequsitionQuantityById(int itemId, int requisitionId)
        {
            InvItemBO productBO = new InvItemBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInfoWithRequsitionQuantityById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RequisitionId", DbType.Int32, requisitionId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                productBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"].ToString());
                                productBO.StockById = Convert.ToInt32(reader["StockById"].ToString());
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.Name = reader["ItemName"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.Description = reader["Description"].ToString();
                                productBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.ProductType = reader["ProductType"].ToString();
                                productBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                productBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                productBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                productBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                productBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                productBO.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                productBO.PurchaseQuantity = Convert.ToDecimal(reader["PurchaseQuantity"]);
                            }
                        }
                    }
                }
            }
            return productBO;
        }
        public InvItemBO GetInvItemInfoWithAdhocPurchaseQuantityById(int itemId)
        {
            InvItemBO productBO = new InvItemBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInfoWithAdhocPurchaseQuantityById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                productBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"].ToString());
                                productBO.StockById = Convert.ToInt32(reader["StockById"].ToString());
                                productBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                productBO.Name = reader["ItemName"].ToString();
                                productBO.Code = reader["Code"].ToString();
                                productBO.Description = reader["Description"].ToString();
                                productBO.ManufacturerId = Convert.ToInt32(reader["ManufacturerId"]);
                                productBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                productBO.CategoryName = reader["CategoryName"].ToString();
                                productBO.ProductType = reader["ProductType"].ToString();
                                productBO.PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]);
                                productBO.SellingLocalCurrencyId = Int32.Parse(reader["SellingLocalCurrencyId"].ToString());
                                productBO.UnitPriceLocal = Convert.ToDecimal(reader["UnitPriceLocal"]);
                                productBO.SellingUsdCurrencyId = Int32.Parse(reader["SellingUsdCurrencyId"].ToString());
                                productBO.UnitPriceUsd = Convert.ToDecimal(reader["UnitPriceUsd"]);
                                productBO.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                productBO.PurchaseQuantity = Convert.ToDecimal(reader["PurchaseQuantity"]);
                            }
                        }
                    }
                }
            }
            return productBO;
        }
        public List<PMRequisitionReportViewBO> GetDateWiseRequisitionReportInfo(DateTime dateFrom, DateTime dateTo,
                                                                        int? itemId, int costCenter)
        {
            List<PMRequisitionReportViewBO> requisitionList = new List<PMRequisitionReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRequisitionReportInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, dateTo);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int64, itemId);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenter", DbType.Int32, costCenter);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RequisitionInfo");
                    DataTable Table = ds.Tables["RequisitionInfo"];

                    requisitionList = Table.AsEnumerable().Select(r => new PMRequisitionReportViewBO
                    {
                        RequisitionId = r.Field<Int32>("RequisitionId"),
                        PRDate = r.Field<string>("PRDate"),
                        PRNumber = r.Field<string>("PRNumber"),
                        ProductName = r.Field<string>("ProductName"),
                        Quantity = r.Field<decimal?>("Quantity"),
                        Stock = r.Field<string>("Stock"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        AveragePrice = r.Field<decimal>("AveragePrice"),
                        FromCostCenter = r.Field<string>("FromCostCenter")

                    }).ToList();
                }
            }

            return requisitionList;
        }

        public List<PMRequisitionReportViewBO> GetRequisitionReportInfo(DateTime dateFrom, DateTime dateTo, int? itemId, int FromCostCenter,
                                                                        int ToCostCenter, string PMNumber, int CategoryId, int companyId, int projectId)
        {
            List<PMRequisitionReportViewBO> requisitionList = new List<PMRequisitionReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRequisitionReportInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, dateFrom);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, dateTo);
                    if (itemId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, DBNull.Value);

                    if (FromCostCenter > 0)
                        dbSmartAspects.AddInParameter(cmd, "@FromCostCenter", DbType.Int32, FromCostCenter);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromCostCenter", DbType.Int32, DBNull.Value);

                    if (ToCostCenter > 0)
                        dbSmartAspects.AddInParameter(cmd, "@ToCostCenter", DbType.Int32, ToCostCenter);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToCostCenter", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(PMNumber))
                        dbSmartAspects.AddInParameter(cmd, "@PMNumber", DbType.String, PMNumber);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@PMNumber", DbType.String, DBNull.Value);

                    if (CategoryId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@Category", DbType.Int32, CategoryId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Category", DbType.Int32, DBNull.Value);
                    if (companyId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, companyId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CompanyId", DbType.Int64, DBNull.Value);
                    if (projectId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, projectId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int64, DBNull.Value);
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "RequisitionInfo");
                    DataTable Table = ds.Tables["RequisitionInfo"];

                    requisitionList = Table.AsEnumerable().Select(r => new PMRequisitionReportViewBO
                    {
                        RequisitionId = r.Field<Int32>("RequisitionId"),
                        PRDate = r.Field<string>("PRDate"),
                        PRNumber = r.Field<string>("PRNumber"),
                        ProductName = r.Field<string>("ProductName"),
                        Quantity = r.Field<decimal?>("Quantity"),
                        Stock = r.Field<string>("Stock"),
                        ApprovedStatus = r.Field<string>("ApprovedStatus"),
                        FromCostCenter = r.Field<string>("FromCostCenter"),
                        ToCostCenter = r.Field<string>("ToCostCenter"),
                        ProductCategory = r.Field<string>("ProductCategory"),
                        AveragePrice = r.Field<decimal>("AveragePrice"),
                        Remarks = r.Field<string>("Remarks"),
                        CompanyName = r.Field<string>("CompanyName"),
                        ProjectName = r.Field<string>("ProjectName")

                    }).ToList();
                }
            }

            return requisitionList;
        }
        public List<PMRequisitionBO> GetRequisitionInfoForReport(DateTime FromDate, DateTime ToDate,
                                                                     string approvedStatus, string prNumber)
        {
            List<PMRequisitionBO> requisitionList = new List<PMRequisitionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMRequisitionInfoForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, FromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, ToDate);

                    if (!string.IsNullOrEmpty(approvedStatus))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, approvedStatus);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@ApprovedStatus", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(prNumber))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PRNumber", DbType.String, prNumber);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@PRNumber", DbType.String, DBNull.Value);
                    }

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMRequisitionBO requisitionBO = new PMRequisitionBO();

                                requisitionBO.RequisitionId = Convert.ToInt32(reader["RequisitionId"]);
                                requisitionBO.PRNumber = Convert.ToString(reader["PRNumber"]);
                                requisitionBO.FromCostCenterId = Convert.ToInt32(reader["FromCostCenterId"]);
                                requisitionBO.FromCostCenter = Convert.ToString(reader["FromCostCenter"]);
                                requisitionBO.Remarks = Convert.ToString(reader["Remarks"]);
                                requisitionBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"]);
                                if (Convert.ToString(reader["ApprovedStatus"]) == "Submit" || Convert.ToString(reader["ApprovedStatus"]) == "Submitted")
                                {
                                    requisitionBO.ApprovedStatus = "Submitted";
                                }
                                else
                                {
                                    requisitionBO.ApprovedStatus = Convert.ToString(reader["ApprovedStatus"]);
                                }
                                //requisitionBO.ApprovedStatus = Convert.ToString(reader["ApprovedStatus"]);
                                requisitionBO.ApprovedDate = Convert.ToDateTime(reader["ApprovedDate"]);
                                requisitionBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                requisitionBO.RequisitionBy = Convert.ToString(reader["RequisitionBy"]);
                                requisitionBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                requisitionBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                requisitionBO.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                requisitionBO.ApprovedQuantity = Convert.ToDecimal(reader["ApprovedQuantity"]);
                                requisitionBO.ItemName = Convert.ToString(reader["ItemName"]);
                                requisitionBO.CategoryName = Convert.ToString(reader["CategoryName"]);
                                requisitionBO.HeadName = Convert.ToString(reader["HeadName"]);
                                requisitionBO.CheckedByName = reader["CheckedByName"].ToString();
                                requisitionBO.ApprovedByName = reader["ApprovedByName"].ToString();
                                requisitionList.Add(requisitionBO);
                            }
                        }
                    }
                }
            }
            return requisitionList;
        }
        public List<PMRequisitionBO> GetRequisitionOrderInvoice(int requisitionId)
        {
            List<PMRequisitionBO> requisitionList = new List<PMRequisitionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRequisitionOrderInvoice_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RequisitionId", DbType.Int32, requisitionId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMRequisitionBO requisitionBO = new PMRequisitionBO();

                                requisitionBO.RequisitionId = Convert.ToInt32(reader["RequisitionId"]);
                                requisitionBO.PRNumber = Convert.ToString(reader["PRNumber"]);
                                requisitionBO.FromCostCenterId = Convert.ToInt32(reader["FromCostCenterId"]);
                                requisitionBO.FromCostCenter = Convert.ToString(reader["FromCostCenter"]);
                                requisitionBO.CompanyName = Convert.ToString(reader["CompanyName"]);
                                requisitionBO.ProjectName = Convert.ToString(reader["ProjectName"]);
                                requisitionBO.Remarks = Convert.ToString(reader["Remarks"]);
                                requisitionBO.ReceivedByDate = Convert.ToDateTime(reader["ReceivedByDate"]);
                                requisitionBO.ApprovedStatus = Convert.ToString(reader["ApprovedStatus"]);
                                requisitionBO.ApprovedDate = Convert.ToDateTime(reader["ApprovedDate"]);
                                requisitionBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                requisitionBO.RequisitionBy = Convert.ToString(reader["RequisitionBy"]);
                                requisitionBO.ItemId = Convert.ToInt32(reader["ItemId"]);
                                requisitionBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                requisitionBO.Quantity = Convert.ToDecimal(reader["Quantity"]);
                                requisitionBO.ApprovedQuantity = Convert.ToDecimal(reader["ApprovedQuantity"]);
                                requisitionBO.AverageCost = Convert.ToDecimal(reader["AverageCost"]);
                                requisitionBO.ItemName = Convert.ToString(reader["ItemName"]);
                                requisitionBO.ColorText = Convert.ToString(reader["ColorText"]);
                                requisitionBO.SizeText = Convert.ToString(reader["SizeText"]);
                                requisitionBO.StyleText = Convert.ToString(reader["StyleText"]);
                                requisitionBO.CategoryName = Convert.ToString(reader["CategoryName"]);
                                requisitionBO.HeadName = Convert.ToString(reader["HeadName"]);
                                requisitionBO.CheckedByName = Convert.ToString(reader["CheckedByName"]);
                                requisitionBO.ApprovedByName = Convert.ToString(reader["ApprovedByName"]);
                                requisitionBO.ItemRemarks = Convert.ToString(reader["ItemRemarks"]);
                                requisitionList.Add(requisitionBO);
                            }
                        }
                    }
                }
            }
            return requisitionList;
        }

        public bool CopmpleteRequisitionOrder(int requisitionid)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRequisitionWisePurchaseStatus_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@RequisitionId", DbType.Int32, requisitionid);
                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
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

        public bool CompleteRequisitionTransferOrder(int requisitionid)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRequisitionWiseTransferStatus_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@RequisitionId", DbType.Int32, requisitionid);
                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
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

        public List<CostCentreTabBO> GetRequisitionCostCenter()
        {
            List<CostCentreTabBO> costCentreList = new List<CostCentreTabBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRequisitionCostCenter_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CostCentreTabBO costCentreBO = new CostCentreTabBO();

                                costCentreBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                costCentreBO.CostCenter = reader["CostCenter"].ToString();
                                costCentreList.Add(costCentreBO);
                            }
                        }
                    }
                }
            }
            return costCentreList;
        }
    }
}
