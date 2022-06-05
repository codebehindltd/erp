using HotelManagement.Entity.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace HotelManagement.Data.Inventory
{
    public class InvCategoryDA : BaseService
    {
        public List<InvCategoryBO> GetInvCatagoryInfo()
        {
            List<InvCategoryBO> nodeMatrixList = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvCatagoryInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO catagoryBO = new InvCategoryBO();

                                catagoryBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                catagoryBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                catagoryBO.Code = reader["Code"].ToString();
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                catagoryBO.MatrixInfo = reader["MatrixInfo"].ToString();
                                catagoryBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                catagoryBO.Hierarchy = reader["Hierarchy"].ToString();
                                catagoryBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                catagoryBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                catagoryBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                catagoryBO.ServiceType = reader["ServiceType"].ToString();
                                catagoryBO.Description = reader["Description"].ToString();

                                nodeMatrixList.Add(catagoryBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixList;
        }
        public InvCategoryBO GetInvCategoryInfoById(int nodeId)
        {
            InvCategoryBO catagoryBO = new InvCategoryBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvCategoryInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, nodeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                catagoryBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                catagoryBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                catagoryBO.AncestorHead = reader["AncestorHead"].ToString();
                                catagoryBO.Code = reader["Code"].ToString();
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                catagoryBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                catagoryBO.Hierarchy = reader["Hierarchy"].ToString();
                                catagoryBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                catagoryBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                catagoryBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                catagoryBO.ServiceType = reader["ServiceType"].ToString();
                                catagoryBO.Description = reader["Description"].ToString();
                            }
                        }
                    }
                }
            }
            return catagoryBO;
        }


        public List<InvCategoryBO> GetInvCategoryByAutoSearch(string category)
        {
            List<InvCategoryBO> nodeMatrixBOList = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvCategoryInfoByAutoSearch_SP"))
                {
                    if (!string.IsNullOrEmpty(category))
                        dbSmartAspects.AddInParameter(cmd, "@Category", DbType.String, category);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Category", DbType.String, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO catagoryBO = new InvCategoryBO();

                                catagoryBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                catagoryBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                catagoryBO.AncestorHead = reader["AncestorHead"].ToString();
                                catagoryBO.Code = reader["Code"].ToString();
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                catagoryBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                catagoryBO.Hierarchy = reader["Hierarchy"].ToString();
                                catagoryBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                catagoryBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                catagoryBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                catagoryBO.ServiceType = reader["ServiceType"].ToString();
                                catagoryBO.Description = reader["Description"].ToString();

                                nodeMatrixBOList.Add(catagoryBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixBOList;
        }
        public List<InvCategoryBO> GetCategoryByCostcenterForAutoSearch(string categoryName, int costCenterId)
        {
            List<InvCategoryBO> category = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCategoryByCostcenterForAutoSearch_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(categoryName))
                        dbSmartAspects.AddInParameter(cmd, "@CategoryName", DbType.String, categoryName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@CategoryName", DbType.String, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "Category");
                    DataTable table = ds.Tables["Category"];

                    category = table.AsEnumerable().Select(r => new InvCategoryBO()
                    {
                        CategoryId = r.Field<int>("CategoryId"),
                        Name = r.Field<string>("Name"),
                        HeadWithCode = r.Field<string>("HeadWithCode")
                    }).ToList();
                }
            }
            return category;
        }
        public CogsAccountVsItemCategoryMapppingBO GetCogsAccountVsItemCategoryMappping(int categoryId)
        {
            string query = string.Format("SELECT * FROM InvCogsAccountVsItemCategoryMappping WHERE CategoryId = {0}", categoryId);

            CogsAccountVsItemCategoryMapppingBO catagory = new CogsAccountVsItemCategoryMapppingBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet CategoryDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, CategoryDS, "Category");
                    DataTable Table = CategoryDS.Tables["Category"];

                    catagory = Table.AsEnumerable().Select(r => new CogsAccountVsItemCategoryMapppingBO
                    {
                        CogsAccountMapId = r.Field<int>("CogsAccountMapId"),
                        CategoryId = r.Field<int>("CategoryId"),
                        NodeId = r.Field<int>("NodeId")

                    }).FirstOrDefault();
                }
            }

            return catagory;
        }
        public CogsAccountVsItemCategoryMapppingBO GetInventoryAccountVsItemCategoryMappping(int categoryId)
        {
            string query = string.Format("SELECT * FROM InvInventoryAccountVsItemCategoryMappping WHERE CategoryId = {0}", categoryId);

            CogsAccountVsItemCategoryMapppingBO catagory = new CogsAccountVsItemCategoryMapppingBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                {
                    DataSet CategoryDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, CategoryDS, "Category");
                    DataTable Table = CategoryDS.Tables["Category"];

                    catagory = Table.AsEnumerable().Select(r => new CogsAccountVsItemCategoryMapppingBO
                    {
                        CogsAccountMapId = r.Field<int>("InvAccountMapId"),
                        CategoryId = r.Field<int>("CategoryId"),
                        NodeId = r.Field<int>("NodeId")

                    }).FirstOrDefault();
                }
            }

            return catagory;
        }
        //public List<InvCategoryCostCenterMappingBO> GetInvCategoryInfo(List<InvCategoryCostCenterMappingBO> costCenterList)
        //{
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        conn.Open();
        //        using (DbTransaction transction = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                using (DbCommand commandMapping = dbSmartAspects.GetStoredProcCommand(""))
        //                {

        //                }
        //            }
        //            catch (Exception)
        //            {

        //                throw;
        //            }
        //        }
        //    }
        //}
        public Boolean SaveInvCatagoryInfo(InvCategoryBO catagoryBO, List<InvCategoryCostCenterMappingBO> costCenterList, out int tmpProductCatagoryId)
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
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveInvCatagoryInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@AncestorId", DbType.Int32, catagoryBO.AncestorId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Code", DbType.String, catagoryBO.Code);
                            dbSmartAspects.AddInParameter(commandMaster, "@Name", DbType.String, catagoryBO.Name);
                            dbSmartAspects.AddInParameter(commandMaster, "@ServiceType", DbType.String, catagoryBO.ServiceType);
                            dbSmartAspects.AddInParameter(commandMaster, "@Description", DbType.String, catagoryBO.Description);
                            dbSmartAspects.AddInParameter(commandMaster, "@ActiveStat", DbType.Boolean, catagoryBO.ActiveStat);
                            //dbSmartAspects.AddInParameter(commandMaster, "@CFHeadId", DbType.Int32, catagoryBO.CFHeadId);
                            //dbSmartAspects.AddInParameter(commandMaster, "@PLHeadId", DbType.Int32, catagoryBO.PLHeadId);
                            dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, catagoryBO.CreatedBy);
                            dbSmartAspects.AddOutParameter(commandMaster, "@CategoryId", DbType.Int32, sizeof(Int32));
                            dbSmartAspects.AddOutParameter(commandMaster, "@Err", DbType.Int32, sizeof(Int32));


                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            //int ERR = Convert.ToInt32(commandMaster.Parameters["@Err"].Value);
                            tmpProductCatagoryId = Convert.ToInt32(commandMaster.Parameters["@CategoryId"].Value);
                        }


                        if (status > 0)
                        {
                            using (DbCommand commandMapping = dbSmartAspects.GetStoredProcCommand("SaveInvCategoryCostCenterMappingInfo_SP"))
                            {
                                foreach (InvCategoryCostCenterMappingBO mappingBO in costCenterList)
                                {
                                    commandMapping.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandMapping, "@CategoryId", DbType.Int32, tmpProductCatagoryId);
                                    dbSmartAspects.AddInParameter(commandMapping, "@CostCenterId", DbType.Int32, mappingBO.CostCenterId);
                                    dbSmartAspects.AddOutParameter(commandMapping, "@MappingId", DbType.Int32, sizeof(Int32));

                                    status = dbSmartAspects.ExecuteNonQuery(commandMapping, transction);
                                }
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandMapping = dbSmartAspects.GetStoredProcCommand("SaveInventoryAccountVsItemCategoryMappping_SP"))
                            {
                                commandMapping.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandMapping, "@CategoryId", DbType.Int32, tmpProductCatagoryId);
                                dbSmartAspects.AddInParameter(commandMapping, "@NodeId", DbType.Int32, catagoryBO.InventoryNodeId);
                                dbSmartAspects.AddInParameter(commandMapping, "@Remarks", DbType.String, catagoryBO.Description);
                                dbSmartAspects.AddInParameter(commandMapping, "@CreatedBy", DbType.Int32, catagoryBO.CreatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(commandMapping, transction);
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand commandMapping = dbSmartAspects.GetStoredProcCommand("SaveCogsAccountVsItemCategoryMappping_SP"))
                            {
                                commandMapping.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandMapping, "@CategoryId", DbType.Int32, tmpProductCatagoryId);
                                dbSmartAspects.AddInParameter(commandMapping, "@NodeId", DbType.Int32, catagoryBO.CogsNodeId);
                                dbSmartAspects.AddInParameter(commandMapping, "@Remarks", DbType.String, catagoryBO.Description);
                                dbSmartAspects.AddInParameter(commandMapping, "@CreatedBy", DbType.Int32, catagoryBO.CreatedBy);

                                status = dbSmartAspects.ExecuteNonQuery(commandMapping, transction);
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
                        throw ex;
                    }
                }
            }

            Boolean uploadStatus = SaveInvCategoryImage(tmpProductCatagoryId, catagoryBO.RandomCategoryId);

            return retVal;
        }
        public Boolean SaveInvCategoryImage(int productId, int randomProductId)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandDocument = dbSmartAspects.GetStoredProcCommand("UpdateEmployeeDocumentAndSignature_SP"))
                {
                    commandDocument.Parameters.Clear();
                    dbSmartAspects.AddInParameter(commandDocument, "@EmpId", DbType.Int32, productId);
                    dbSmartAspects.AddInParameter(commandDocument, "@RandomId", DbType.String, randomProductId);
                    status = dbSmartAspects.ExecuteNonQuery(commandDocument) > 0 ? true : false;
                }
            }

            return status;
        }
        public bool UpdateInvCatagoryInfo(InvCategoryBO catagoryBO, List<InvCategoryCostCenterMappingBO> costCenterList, List<InvCategoryCostCenterMappingBO> costCenterDeleteList, CogsAccountVsItemCategoryMapppingBO catagoryInventoryMapping, CogsAccountVsItemCategoryMapppingBO catagoryCogsMapping, out int NodeId)
        {
            bool retVal = false, getStatus = false;
            List<InvCategoryCostCenterMappingBO> getListCostMap = new List<InvCategoryCostCenterMappingBO>();
            NodeId = 0;
            int status = 0;
            Boolean costStatus = false;
            //int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateInvCatagoryInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(commandMaster, "@CategoryId", DbType.Int32, catagoryBO.CategoryId);
                            dbSmartAspects.AddInParameter(commandMaster, "@AncestorId", DbType.Int32, catagoryBO.AncestorId);
                            dbSmartAspects.AddInParameter(commandMaster, "@Code", DbType.String, catagoryBO.Code);
                            dbSmartAspects.AddInParameter(commandMaster, "@Name", DbType.String, catagoryBO.Name);
                            dbSmartAspects.AddInParameter(commandMaster, "@ServiceType", DbType.String, catagoryBO.ServiceType);
                            dbSmartAspects.AddInParameter(commandMaster, "@Description", DbType.String, catagoryBO.Description);
                            dbSmartAspects.AddInParameter(commandMaster, "@ActiveStat", DbType.Boolean, catagoryBO.ActiveStat);

                            dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, catagoryBO.LastModifiedBy);
                            dbSmartAspects.AddOutParameter(commandMaster, "@Err", DbType.Int32, sizeof(Int32));
                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        }

                        if (status > 0 && costCenterList != null)
                        {
                            if (costCenterList.Count > 0)
                            {
                                costStatus = UpdatePMProductCostCenterMapping(costCenterList);
                            }
                        }

                        //if (status > 0 && costCenterDeleteList != null)
                        //{
                        //    if (costCenterDeleteList.Count > 0)
                        //    {
                        //        getStatus = DeletePMProductCostCenterMapping(costCenterDeleteList);
                        //    }
                        //}

                        if (catagoryInventoryMapping != null && status > 0)
                        {
                            using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateInventoryAccountVsItemCategoryMappping_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@InvAccountMapId", DbType.Int32, catagoryInventoryMapping.CogsAccountMapId);
                                dbSmartAspects.AddInParameter(commandMaster, "@NodeId", DbType.Int32, catagoryInventoryMapping.NodeId);
                                dbSmartAspects.AddInParameter(commandMaster, "@CategoryId", DbType.Int32, catagoryBO.CategoryId);
                                dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, catagoryBO.Description);
                                dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, catagoryBO.LastModifiedBy);

                                status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            }
                        }
                        else if (status > 0)
                        {
                            using (DbCommand commandMapping = dbSmartAspects.GetStoredProcCommand("SaveInventoryAccountVsItemCategoryMappping_SP"))
                            {
                                commandMapping.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandMapping, "@CategoryId", DbType.Int32, catagoryBO.CategoryId);
                                dbSmartAspects.AddInParameter(commandMapping, "@NodeId", DbType.Int32, catagoryBO.InventoryNodeId);
                                dbSmartAspects.AddInParameter(commandMapping, "@Remarks", DbType.String, catagoryBO.Description);
                                dbSmartAspects.AddInParameter(commandMapping, "@CreatedBy", DbType.Int32, catagoryBO.LastModifiedBy);

                                status = dbSmartAspects.ExecuteNonQuery(commandMapping, transction);
                            }
                        }

                        if (catagoryCogsMapping != null && status > 0)
                        {
                            using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateCogsAccountVsItemCategoryMappping_SP"))
                            {
                                dbSmartAspects.AddInParameter(commandMaster, "@CogsAccountMapId", DbType.Int32, catagoryCogsMapping.CogsAccountMapId);
                                dbSmartAspects.AddInParameter(commandMaster, "@NodeId", DbType.Int32, catagoryCogsMapping.NodeId);
                                dbSmartAspects.AddInParameter(commandMaster, "@CategoryId", DbType.Int32, catagoryBO.CategoryId);
                                dbSmartAspects.AddInParameter(commandMaster, "@Remarks", DbType.String, catagoryBO.Description);
                                dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, catagoryBO.LastModifiedBy);

                                status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                            }
                        }
                        else if (status > 0)
                        {
                            using (DbCommand commandMapping = dbSmartAspects.GetStoredProcCommand("SaveCogsAccountVsItemCategoryMappping_SP"))
                            {
                                commandMapping.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandMapping, "@CategoryId", DbType.Int32, catagoryBO.CategoryId);
                                dbSmartAspects.AddInParameter(commandMapping, "@NodeId", DbType.Int32, catagoryBO.CogsNodeId);
                                dbSmartAspects.AddInParameter(commandMapping, "@Remarks", DbType.String, catagoryBO.Description);
                                dbSmartAspects.AddInParameter(commandMapping, "@CreatedBy", DbType.Int32, catagoryBO.LastModifiedBy);

                                status = dbSmartAspects.ExecuteNonQuery(commandMapping, transction);
                            }
                        }

                        if (status > 0 && (costCenterList.Count == 0 || costStatus))
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
        //private List<InvCategoryCostCenterMappingBO> GetInvMappingInfo(List<InvCategoryCostCenterMappingBO> unSelectedList)
        //{
        //    List<InvCategoryCostCenterMappingBO> invCategoryCostCenters = new List<InvCategoryCostCenterMappingBO>();
        //    bool status = false;
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvMappingInfo_SP"))
        //        {
        //            foreach (var item in unSelectedList)
        //            {
        //                cmd.Parameters.Clear();
        //                dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, item.CategoryId);

        //                using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //                {
        //                    if (reader != null)
        //                    {
        //                        while (reader.Read())
        //                        {
        //                            InvCategoryCostCenterMappingBO costBO = new InvCategoryCostCenterMappingBO();
        //                            costBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
        //                            costBO.MappingId = Convert.ToInt32(reader["MappingId"]);
        //                            costBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
        //                            costList.Add(costBO);
        //                        }
        //                    }
        //                }
        //                    status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
        //            }
        //        }
        //    }

        //    return invCategoryCostCenters;
        //}
        private bool DeletePMProductCostCenterMapping(List<InvCategoryCostCenterMappingBO> DltCostCenterList)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteCostCenterMapInfo_SP"))
                        {
                            foreach (var item in DltCostCenterList)
                            {
                                command.Parameters.Clear();
                                dbSmartAspects.AddInParameter(command, "@MappingId", DbType.Int32, item.MappingId);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false;
                            }
                        }
                        if (status == true)
                        {
                            transction.Commit();
                        }
                        else
                        {
                            transction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        throw ex;
                    }
                }
            }
            return status;
        }
        private bool UpdatePMProductCostCenterMapping(List<InvCategoryCostCenterMappingBO> costCenterList)
        {
            InvCategoryCostCenterMappingDA costDA = new InvCategoryCostCenterMappingDA();
            List<InvCategoryCostCenterMappingBO> dbCostList = new List<InvCategoryCostCenterMappingBO>();
            dbCostList = costDA.GetInvCategoryCostCenterMappingByCategoryId(costCenterList[0].CategoryId);

            List<int> idList = new List<int>();
            int mappingId;

            for (int i = 0; i < costCenterList.Count; i++)
            {
                int succ = IsAvailableInDb(dbCostList, costCenterList[i]);
                if (succ > 0)
                {
                    //Update
                    costCenterList[i].MappingId = succ;
                    bool status = costDA.UpdateInvCategoryCostCenterMappingInfo(costCenterList[i]);
                    idList.Add(succ);
                }
                else
                {
                    //Insert
                    bool status = costDA.SaveInvCategoryCostCenterMappingInfo(costCenterList[i], out mappingId);
                    idList.Add(mappingId);
                }
            }

            string saveAndUpdatedIdList = string.Empty;
            for (int j = 0; j < idList.Count; j++)
            {
                if (string.IsNullOrWhiteSpace(saveAndUpdatedIdList))
                {
                    saveAndUpdatedIdList = idList[j].ToString();
                }
                else
                {
                    saveAndUpdatedIdList = saveAndUpdatedIdList + "," + idList[j];
                }
            }
            Boolean deleteStatus = costDA.DeleteAllInvItemCostCenterMappingInfoWithoutMappingIdList(costCenterList[0].CategoryId, saveAndUpdatedIdList);
            return true;
        }
        private int IsAvailableInDb(List<InvCategoryCostCenterMappingBO> dbCostList, InvCategoryCostCenterMappingBO costBO)
        {
            int isInDB = 0;
            for (int j = 0; j < dbCostList.Count; j++)
            {
                if (dbCostList[j].CategoryId == costBO.CategoryId && costBO.CostCenterId == dbCostList[j].CostCenterId)
                {
                    isInDB = dbCostList[j].MappingId;
                }
            }
            return isInDB;
        }
        public List<InvCategoryBO> GetInvItemCatagoryInfoByServiceType(string serviceType)
        {
            List<InvCategoryBO> catagoryList = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemCatagoryInfoByServiceType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceType", DbType.String, serviceType);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO catagoryBO = new InvCategoryBO();
                                catagoryBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                catagoryBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                catagoryBO.Code = reader["Code"].ToString();
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                catagoryBO.MatrixInfo = reader["MatrixInfo"].ToString();
                                catagoryBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                catagoryBO.Hierarchy = reader["Hierarchy"].ToString();
                                catagoryBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                catagoryBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                catagoryBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                catagoryBO.ServiceType = reader["ServiceType"].ToString();
                                catagoryBO.Description = reader["Description"].ToString();
                                catagoryList.Add(catagoryBO);
                            }
                        }
                    }
                }
            }
            return catagoryList;
        }
        public List<InvCategoryBO> GetAllInvItemCatagoryInfoByServiceType(string serviceType)
        {
            List<InvCategoryBO> catagoryList = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllInvItemCatagoryInfoByServiceType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceType", DbType.String, serviceType);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO catagoryBO = new InvCategoryBO();
                                catagoryBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                catagoryBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                catagoryBO.Code = reader["Code"].ToString();
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                catagoryBO.MatrixInfo = reader["MatrixInfo"].ToString();
                                catagoryBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                catagoryBO.Hierarchy = reader["Hierarchy"].ToString();
                                catagoryBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                catagoryBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                catagoryBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                catagoryBO.ServiceType = reader["ServiceType"].ToString();
                                catagoryBO.Description = reader["Description"].ToString();
                                catagoryList.Add(catagoryBO);
                            }
                        }
                    }
                }
            }
            return catagoryList;
        }
        public List<InvCategoryBO> GetAllActiveInvItemCatagoryInfoByServiceType(string serviceType)
        {
            List<InvCategoryBO> catagoryList = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllActiveInvItemCatagoryInfoByServiceType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceType", DbType.String, serviceType);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO catagoryBO = new InvCategoryBO();
                                catagoryBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                catagoryBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                catagoryBO.Code = reader["Code"].ToString();
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                catagoryBO.MatrixInfo = reader["MatrixInfo"].ToString();
                                catagoryBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                catagoryBO.Hierarchy = reader["Hierarchy"].ToString();
                                catagoryBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                catagoryBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                catagoryBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                catagoryBO.ServiceType = reader["ServiceType"].ToString();
                                catagoryBO.Description = reader["Description"].ToString();
                                catagoryList.Add(catagoryBO);
                            }
                        }
                    }
                }
            }
            return catagoryList;
        }
        public List<InvCategoryBO> GetCostCenterWiseInvItemCatagoryInfo(string serviceType, int costCenterId)
        {
            List<InvCategoryBO> catagoryList = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCostCenterWiseInvItemCategory_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceType", DbType.String, serviceType);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO catagoryBO = new InvCategoryBO();
                                catagoryBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                catagoryBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                catagoryBO.Code = reader["Code"].ToString();
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                catagoryBO.MatrixInfo = reader["MatrixInfo"].ToString();
                                catagoryBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                catagoryBO.Hierarchy = reader["Hierarchy"].ToString();
                                catagoryBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                catagoryBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                catagoryBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                catagoryBO.ServiceType = reader["ServiceType"].ToString();
                                catagoryBO.Description = reader["Description"].ToString();
                                catagoryList.Add(catagoryBO);
                            }
                        }
                    }
                }
            }
            return catagoryList;
        }
        public List<InvCategoryBO> GetInvCategoryBySearchCriteria(string Name, string Code, string Type)
        {
            string SearchCriteria = string.Empty;
            SearchCriteria = GenerateWhereCondition(Name, Code, Type);

            List<InvCategoryBO> catagoryList = new List<InvCategoryBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvCategoryBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, SearchCriteria);
                    DataSet CategoryDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, CategoryDS, "Category");
                    DataTable Table = CategoryDS.Tables["Category"];

                    catagoryList = Table.AsEnumerable().Select(r => new InvCategoryBO
                    {

                        CategoryId = r.Field<int>("CategoryId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        ServiceType = r.Field<string>("ServiceType"),
                        Description = r.Field<string>("Description")

                    }).ToList();
                }
            }
            return catagoryList;
        }
        public List<InvCategoryBO> GetInvCategoryByService(string SeviceType)
        {
            List<InvCategoryBO> catagoryList = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvCategoryByService_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SeviceType", DbType.String, SeviceType);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO catagoryBO = new InvCategoryBO();
                                catagoryBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryList.Add(catagoryBO);
                            }
                        }
                    }
                }
            }
            return catagoryList;
        }
        public List<InvCategoryBO> GetInvCategoryInfoByLabel(int costCenterId, int categoryId)
        {
            List<InvCategoryBO> entityBOList = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvCategoryInfoByLabel_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@CategoryId", DbType.Int32, categoryId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO entityBO = new InvCategoryBO();

                                entityBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                entityBO.Name = reader["Name"].ToString();
                                entityBO.Code = reader["Code"].ToString();
                                entityBO.ImageName = reader["ImageName"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBO.ChildCount = Convert.ToInt32(reader["ChildCount"]);
                                entityBO.Hierarchy = reader["Hierarchy"].ToString();
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        private string GenerateWhereCondition(string Name, string Code, string Type)
        {
            string Where = string.Empty;
            Type = (Type == "None" ? string.Empty : Type);

            if (!string.IsNullOrWhiteSpace(Name))
            {
                Where = "Name LIKE '%" + Name + "%'";
            }

            if (!string.IsNullOrWhiteSpace(Code))
            {
                if (string.IsNullOrWhiteSpace(Where))
                {
                    Where = "Code LIKE '%" + Code + "%'";
                }
                else
                {
                    Where += " AND Code LIKE '%" + Code + "%'";
                }
            }

            if (!string.IsNullOrWhiteSpace(Type))
            {
                if (string.IsNullOrWhiteSpace(Where))
                {
                    Where = "ServiceType LIKE '%" + Type + "%'";
                }
                else
                {
                    Where += " AND ServiceType LIKE '%" + Type + "%'";
                }
            }

            if (!string.IsNullOrEmpty(Where))
                Where = " WHERE " + Where;

            return Where;
        }
        public List<InvCategoryBO> GetInvCategoryInfoByCustomString(string customString)
        {
            List<InvCategoryBO> nodeMatrixBOList = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvCategoryInfoByCustomString_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CustomString", DbType.String, customString);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO nodeMatrixBO = new InvCategoryBO();

                                nodeMatrixBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.Code = reader["Code"].ToString();
                                nodeMatrixBO.Name = reader["Name"].ToString();
                                nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
                                nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                nodeMatrixBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"].ToString());
                                nodeMatrixBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                nodeMatrixBOList.Add(nodeMatrixBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixBOList;
        }
        public List<InvCategoryBO> GetTPInvCategoryInfoByCustomString(string customString)
        {
            List<InvCategoryBO> nodeMatrixBOList = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTPInvCategoryInfoByCustomString_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CustomString", DbType.String, customString);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO nodeMatrixBO = new InvCategoryBO();

                                nodeMatrixBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                nodeMatrixBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.Code = reader["Code"].ToString();
                                nodeMatrixBO.Name = reader["Name"].ToString();
                                nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
                                nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                nodeMatrixBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                nodeMatrixBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                nodeMatrixBOList.Add(nodeMatrixBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixBOList;
        }
        public List<InvCategoryBO> GetInvCategoryInfoByCategory(string accountHead)
        {
            List<InvCategoryBO> nodeMatrixBOList = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvCategoryInfoByCategory_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Category", DbType.String, accountHead);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO nodeMatrixBO = new InvCategoryBO();

                                nodeMatrixBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.Code = reader["Code"].ToString();
                                nodeMatrixBO.Name = reader["Name"].ToString();
                                nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                nodeMatrixBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                nodeMatrixBO.Hierarchy = reader["Hierarchy"].ToString();
                                nodeMatrixBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                nodeMatrixBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                nodeMatrixBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                nodeMatrixBOList.Add(nodeMatrixBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixBOList;
        }
        public List<string> GetInvCategoryInfoByCategoryNVoucherForm(string accountHead, int isVoucherForm)
        {
            List<string> result = new List<string>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvCategoryInfoByCategory_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Category", DbType.String, accountHead);
                    dbSmartAspects.AddInParameter(cmd, "@IsVoucherForm", DbType.Int32, isVoucherForm);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                //result.Add(reader["NodeHead"].ToString());
                                result.Add(reader["GoogleSearch"].ToString());
                            }
                        }
                    }
                }
            }
            return result;
        }
        public string GetInvCategoryInfoBySpecificCategory(string accountHead)
        {
            string result = "0";
            int isNodeHead = 0;
            if (accountHead.Contains("("))
            {
                isNodeHead = 0;
            }
            else
            {
                isNodeHead = 1;
            }


            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvCategoryInfoBySpecificCategory_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeHead", DbType.String, accountHead);
                    dbSmartAspects.AddInParameter(cmd, "@IsNodeHead", DbType.Int32, isNodeHead);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                result = reader["CategoryId"].ToString();
                            }
                        }
                    }
                }
            }
            return result;
        }
        public List<InvCategoryBO> GetInvItemCatagoryByCostCenter(string serviceType, int costCenterId)
        {
            List<InvCategoryBO> catagoryList = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemCatagoryByCostCenter_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceType", DbType.String, serviceType);
                    dbSmartAspects.AddInParameter(cmd, "@CostcenterId", DbType.String, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO catagoryBO = new InvCategoryBO();
                                catagoryBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                catagoryBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                catagoryBO.Code = reader["Code"].ToString();
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                catagoryBO.MatrixInfo = reader["MatrixInfo"].ToString();
                                catagoryBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                catagoryBO.Hierarchy = reader["Hierarchy"].ToString();
                                catagoryBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                catagoryBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                catagoryBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                catagoryBO.ServiceType = reader["ServiceType"].ToString();
                                catagoryBO.Description = reader["Description"].ToString();
                                catagoryList.Add(catagoryBO);
                            }
                        }
                    }
                }
            }
            return catagoryList;
        }
        public List<InvCategoryBO> GetInvCategoryDetailsForRestaurantBill(int kotId)
        {
            List<InvCategoryBO> catagoryList = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvCategoryDetailsForRestaurantBill_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.String, kotId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO catagoryBO = new InvCategoryBO();
                                catagoryBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                catagoryList.Add(catagoryBO);

                                /*
                                InvCategoryBO catagoryBO = new InvCategoryBO();
                                catagoryBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                catagoryBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                catagoryBO.Code = reader["Code"].ToString();
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.HeadWithCode = reader["HeadWithCode"].ToString();
                                catagoryBO.MatrixInfo = reader["MatrixInfo"].ToString();
                                catagoryBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                catagoryBO.Hierarchy = reader["Hierarchy"].ToString();
                                catagoryBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                catagoryBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                catagoryBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                catagoryBO.ServiceType = reader["ServiceType"].ToString();
                                catagoryBO.Description = reader["Description"].ToString();
                                catagoryList.Add(catagoryBO);
                                */
                            }
                        }
                    }
                }
            }
            return catagoryList;
        }
        public List<InvCategoryBO> GetAllInvItemClassificationInfo()
        {
            List<InvCategoryBO> catagoryList = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllInvItemClassificationInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO catagoryBO = new InvCategoryBO();
                                catagoryBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                catagoryList.Add(catagoryBO);
                            }
                        }
                    }
                }
            }
            return catagoryList;
        }
        public List<ItemClassificationBO> GetInvClassificationDetailsForRestaurantBill(int kotId, string kotIdLst)
        {
            List<ItemClassificationBO> classificationLst = new List<ItemClassificationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvClassificationDetailsForRestaurantBill_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(cmd, "@kotIdLst", DbType.String, kotIdLst);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ItemClassificationBO classificationBO = new ItemClassificationBO();
                                classificationBO.ClassificationId = Convert.ToInt32(reader["ClassificationId"]);
                                classificationBO.ClassificationName = reader["ClassificationName"].ToString();
                                classificationBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                classificationBO.DiscountId = Convert.ToInt32(reader["DiscountId"]);
                                classificationBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);

                                classificationLst.Add(classificationBO);
                            }
                        }
                    }
                }
            }
            return classificationLst;
        }
        public List<ItemClassificationBO> GetRestaurantBillClassificationDetailsByBillId(int billId)
        {
            List<ItemClassificationBO> classificationLst = new List<ItemClassificationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBillClassificationDetailsByBillId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BillId", DbType.Int32, billId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ItemClassificationBO classificationBO = new ItemClassificationBO();

                                classificationBO.DiscountId = Convert.ToInt32(reader["DiscountId"]);
                                classificationBO.BillId = Convert.ToInt32(reader["BillId"]);
                                classificationBO.ClassificationId = Convert.ToInt32(reader["ClassificationId"]);
                                classificationBO.DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"]);

                                classificationLst.Add(classificationBO);
                            }
                        }
                    }
                }
            }
            return classificationLst;
        }
        public List<InvCategoryBO> GetCategoryByLvl(int lvl)
        {
            List<InvCategoryBO> catagoryList = new List<InvCategoryBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCategoryByLvl_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Lvl", DbType.Int32, lvl);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvCategoryBO catagoryBO = new InvCategoryBO();
                                catagoryBO.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                catagoryList.Add(catagoryBO);
                            }
                        }
                    }
                }
            }
            return catagoryList;
        }
    }
}
