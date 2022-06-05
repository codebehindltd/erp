using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Inventory;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Inventory
{
    public class InvUnitHeadDA: BaseService
    {
        public List<InvUnitHeadBO> GetInvUnitHeadInfo()
        {
            List<InvUnitHeadBO> headListBO = new List<InvUnitHeadBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvUnitHeadInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvUnitHeadBO headBO = new InvUnitHeadBO();

                                headBO.UnitHeadId = Convert.ToInt32(reader["UnitHeadId"]);
                                headBO.HeadName = reader["HeadName"].ToString();
                                headBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                headBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                headListBO.Add(headBO);
                            }
                        }
                    }
                }
            }
            return headListBO;
        }
        public List<InvUnitHeadBO> GetRelatedStockBy(int stockById)
        {
            List<InvUnitHeadBO> headList = new List<InvUnitHeadBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRelatedInvUnitHeadInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@StockById", DbType.Int32, stockById);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "UnitHead");
                    DataTable Table = ds.Tables["UnitHead"];

                    headList = Table.AsEnumerable().Select(r => new InvUnitHeadBO
                    {
                        UnitHeadId = r.Field<int>("UnitHeadId"),
                        HeadName = r.Field<string>("HeadName")                        
                    }).ToList();
                }
            }
            return headList;
        }
        public Boolean SaveHeadInfo(InvUnitHeadBO headBO, out int tmpId)
        {
            Boolean status = false;
            tmpId = 0;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveUnitHeadInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@HeadName", DbType.String, headBO.HeadName);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, headBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, headBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@UnitHeadId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        
                        tmpId = Convert.ToInt32(command.Parameters["@UnitHeadId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateHeadInfo(InvUnitHeadBO headBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateUnitHeadInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@UnitHeadId", DbType.Int32, headBO.UnitHeadId);
                        dbSmartAspects.AddInParameter(command, "@HeadName", DbType.String, headBO.HeadName);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, headBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, headBO.LastModifiedBy);

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
        public InvUnitHeadBO GetHeadInfoById(int headId)
        {
            InvUnitHeadBO headBO = new InvUnitHeadBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUnitHeadInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@UnitHeadId", DbType.Int32, headId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                headBO.UnitHeadId = Convert.ToInt32(reader["UnitHeadId"]);
                                headBO.HeadName = reader["HeadName"].ToString();
                                headBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                headBO.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return headBO;
        }
        //public List<InvUnitHeadBO> GetAllConversionHeadInfo()
        //{
        //    List<InvUnitHeadBO> headBOList = new List<InvUnitHeadBO>();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllUnitHeadInfo_SP"))
        //        {
        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        InvUnitHeadBO headBO = new InvUnitHeadBO();

        //                        headBO.UnitHeadId = Convert.ToInt32(reader["UnitHeadId"]);
        //                        headBO.HeadName = reader["HeadName"].ToString();

        //                        headBOList.Add(headBO);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return headBOList;
        //}
        public List<InvUnitHeadBO> GetAllActiveConversionHeadInfo()
        {
            List<InvUnitHeadBO> headBOList = new List<InvUnitHeadBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllActiveConversionHeadInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvUnitHeadBO headBO = new InvUnitHeadBO();

                                headBO.UnitHeadId = Convert.ToInt32(reader["UnitHeadId"]);
                                headBO.HeadName = reader["HeadName"].ToString();

                                headBOList.Add(headBO);
                            }
                        }
                    }
                }
            }
            return headBOList;
        }
        public List<InvUnitHeadBO> GetHeadInformationBySearchCriteriaForPaging(string headName, Boolean activeStat, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<InvUnitHeadBO> headList = new List<InvUnitHeadBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUnitInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@HeadName", DbType.String, headName);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, activeStat);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvUnitHeadBO headBO = new InvUnitHeadBO();
                                headBO.UnitHeadId = Convert.ToInt32(reader["UnitHeadId"]);
                                headBO.HeadName = reader["HeadName"].ToString();
                                headBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                headBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                headList.Add(headBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return headList;
        }

        public List<InvUnitHeadBO> GetItemRelatedStockByItemId(int itemId)
        {
            List<InvUnitHeadBO> headList = new List<InvUnitHeadBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemStockIdByItemId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "UnitHead");
                    DataTable Table = ds.Tables["UnitHead"];

                    headList = Table.AsEnumerable().Select(r => new InvUnitHeadBO
                    {
                        UnitHeadId = r.Field<int>("UnitHeadId"),
                        HeadName = r.Field<string>("HeadName")
                    }).ToList();
                }
            }
            return headList;
        }
        public InvItemBO GetItemPrice(int itemId, int stockId)
        {
            InvItemBO bo = new InvItemBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetItemPriceByStockId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);
                    dbSmartAspects.AddInParameter(cmd, "@StockId", DbType.Int32, stockId);                    

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ItemPrice");
                    DataTable Table = ds.Tables["ItemPrice"];

                    bo = Table.AsEnumerable().Select(r => new InvItemBO
                    {
                        PurchasePrice = r.Field<decimal>("PurchasePrice")                        
                    }).SingleOrDefault();
                }
            }
            return bo;
        }
    }    
}
