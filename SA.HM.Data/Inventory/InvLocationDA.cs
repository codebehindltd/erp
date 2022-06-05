using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Inventory;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Inventory
{
    public class InvLocationDA : BaseService
    {
        public List<InvLocationBO> GetInvLocationInfo()
        {
            List<InvLocationBO> nodeMatrixList = new List<InvLocationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvLocationInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvLocationBO catagoryBO = new InvLocationBO();

                                catagoryBO.LocationId = Convert.ToInt32(reader["LocationId"]);
                                catagoryBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.Code = reader["Code"].ToString();
                                catagoryBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                catagoryBO.Hierarchy = reader["Hierarchy"].ToString();
                                catagoryBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                catagoryBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                catagoryBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                catagoryBO.Description = reader["Description"].ToString();

                                nodeMatrixList.Add(catagoryBO);
                            }
                        }
                    }
                }
            }
            return nodeMatrixList;
        }
        public InvLocationBO GetInvLocationInfoById(int nodeId)
        {
            InvLocationBO catagoryBO = new InvLocationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvLocationInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.Int32, nodeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                catagoryBO.LocationId = Convert.ToInt32(reader["LocationId"]);
                                catagoryBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                catagoryBO.AncestorHead = reader["AncestorHead"].ToString();
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.Code = reader["Code"].ToString();
                                catagoryBO.Lvl = Convert.ToInt32(reader["Lvl"]);
                                catagoryBO.Hierarchy = reader["Hierarchy"].ToString();
                                catagoryBO.HierarchyIndex = reader["HierarchyIndex"].ToString();
                                catagoryBO.IsStoreLocation = Convert.ToBoolean(reader["ActiveStat"]);
                                catagoryBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                catagoryBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                catagoryBO.Description = reader["Description"].ToString();
                            }
                        }
                    }
                }
            }
            return catagoryBO;
        }
        public Boolean SaveInvLocationInfo(InvLocationBO catagoryBO, List<InvLocationCostCenterMappingBO> costCenterList, out int tmpProductCatagoryId)
        {
            bool retVal = false;
            int status = 0;
            Boolean costStatus = false;
            //int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveInvLocationInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@AncestorId", DbType.Int32, catagoryBO.AncestorId);
                        dbSmartAspects.AddInParameter(commandMaster, "@Name", DbType.String, catagoryBO.Name);
                        dbSmartAspects.AddInParameter(commandMaster, "@Code", DbType.String, catagoryBO.Code);
                        dbSmartAspects.AddInParameter(commandMaster, "@Description", DbType.String, catagoryBO.Description);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsStoreLocation", DbType.Boolean, catagoryBO.IsStoreLocation);
                        dbSmartAspects.AddInParameter(commandMaster, "@ActiveStat", DbType.Boolean, catagoryBO.ActiveStat);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, catagoryBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(commandMaster, "@LocationId", DbType.Int32, sizeof(Int32));
                        dbSmartAspects.AddOutParameter(commandMaster, "@Err", DbType.Int32, sizeof(Int32));


                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);
                        //int ERR = Convert.ToInt32(commandMaster.Parameters["@Err"].Value);
                        tmpProductCatagoryId = Convert.ToInt32(commandMaster.Parameters["@LocationId"].Value);


                        using (DbCommand commandMapping = dbSmartAspects.GetStoredProcCommand("SaveInvLocationCostCenterMappingInfo_SP"))
                        {
                            foreach (InvLocationCostCenterMappingBO mappingBO in costCenterList)
                            {
                                commandMapping.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandMapping, "@LocationId", DbType.Int32, tmpProductCatagoryId);
                                dbSmartAspects.AddInParameter(commandMapping, "@CostCenterId", DbType.Int32, mappingBO.CostCenterId);
                                dbSmartAspects.AddOutParameter(commandMapping, "@MappingId", DbType.Int32, sizeof(Int32));
                                status = dbSmartAspects.ExecuteNonQuery(commandMapping, transction);
                                //MappingId = Convert.ToInt32(command.Parameters["@MappingId"].Value);
                            }
                        }

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
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
        public bool UpdateInvLocationInfo(InvLocationBO catagoryBO, List<InvLocationCostCenterMappingBO> costCenterList, out int NodeId)
        {
            bool retVal = false;
            NodeId = 0;
            int status = 0;
            Boolean costStatus = false;
            //int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateInvLocationInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@LocationId", DbType.Int32, catagoryBO.LocationId);
                        dbSmartAspects.AddInParameter(commandMaster, "@AncestorId", DbType.Int32, catagoryBO.AncestorId);
                        dbSmartAspects.AddInParameter(commandMaster, "@Name", DbType.String, catagoryBO.Name);
                        dbSmartAspects.AddInParameter(commandMaster, "@Code", DbType.String, catagoryBO.Code);
                        dbSmartAspects.AddInParameter(commandMaster, "@Description", DbType.String, catagoryBO.Description);
                        dbSmartAspects.AddInParameter(commandMaster, "@IsStoreLocation", DbType.Boolean, catagoryBO.IsStoreLocation);
                        dbSmartAspects.AddInParameter(commandMaster, "@ActiveStat", DbType.Boolean, catagoryBO.ActiveStat);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, catagoryBO.LastModifiedBy);
                        dbSmartAspects.AddOutParameter(commandMaster, "@Err", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        if (costCenterList != null)
                        {
                            if (costCenterList.Count > 0)
                            {
                                costStatus = UpdatePMProductCostCenterMapping(costCenterList);
                                if (status > 0)
                                    if (costStatus)
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
                            transction.Commit();
                            retVal = true;
                        }
                    }
                }
            }
            return retVal;
        }
        private bool UpdatePMProductCostCenterMapping(List<InvLocationCostCenterMappingBO> costCenterList)
        {
            InvCategoryCostCenterMappingDA costDA = new InvCategoryCostCenterMappingDA();
            List<InvLocationCostCenterMappingBO> dbCostList = new List<InvLocationCostCenterMappingBO>();
            dbCostList = costDA.GetInvLocationCostCenterMappingByCategoryId(costCenterList[0].LocationId);

            List<int> idList = new List<int>();
            int mappingId;

            for (int i = 0; i < costCenterList.Count; i++)
            {
                int succ = IsAvailableInDb(dbCostList, costCenterList[i]);
                if (succ > 0)
                {
                    //Update
                    costCenterList[i].MappingId = succ;
                    bool status = costDA.UpdateInvLocationCostCenterMappingInfo(costCenterList[i]);
                    idList.Add(succ);
                }
                else
                {
                    //Insert
                    bool status = costDA.SaveInvLocationCostCenterMappingInfo(costCenterList[i], out mappingId);
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
            Boolean deleteStatus = costDA.DeleteAllInvLocationCostCenterMappingInfoWithoutMappingIdList(costCenterList[0].LocationId, saveAndUpdatedIdList);
            return true;
        }
        private int IsAvailableInDb(List<InvLocationCostCenterMappingBO> dbCostList, InvLocationCostCenterMappingBO costBO)
        {
            int isInDB = 0;
            for (int j = 0; j < dbCostList.Count; j++)
            {
                if (dbCostList[j].LocationId == costBO.LocationId && costBO.CostCenterId == dbCostList[j].CostCenterId)
                {
                    isInDB = dbCostList[j].MappingId;
                }
            }
            return isInDB;
        }
        public List<InvLocationBO> GetInvLocationBySearchCriteria(string name, string code, int costCenterId)
        {
            string SearchCriteria = string.Empty;
            SearchCriteria = GenerateWhereCondition(name, code, costCenterId);

            List<InvLocationBO> catagoryList = new List<InvLocationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvLocationBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, SearchCriteria);
                    DataSet CategoryDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, CategoryDS, "Location");
                    DataTable Table = CategoryDS.Tables["Location"];

                    catagoryList = Table.AsEnumerable().Select(r => new InvLocationBO
                    {

                        LocationId = r.Field<int>("LocationId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        //CostCenterId = r.Field<int>("CostCenterId"),
                        Description = r.Field<string>("Description")

                    }).ToList();
                }
            }
            return catagoryList;
        }
        private string GenerateWhereCondition(string name, string code, int costCenterId)
        {
            string Where = string.Empty;
            if (!string.IsNullOrWhiteSpace(name))
            {
                Where = "Name LIKE '%" + name + "%'";
            }

            if (!string.IsNullOrWhiteSpace(code))
            {
                if (string.IsNullOrWhiteSpace(Where))
                {
                    Where = "Code LIKE '%" + code + "%'";
                }
                else
                {
                    Where += " AND Code LIKE '%" + code + "%'";
                }
            }

            if (costCenterId > 0)
            {
                if (string.IsNullOrWhiteSpace(Where))
                {
                    Where = "CostCenterId = " + costCenterId + "";
                }
                else
                {
                    Where += " AND CostCenterId = " + costCenterId + "";
                }
            }

            if (!string.IsNullOrEmpty(Where))
                Where = " WHERE " + Where;

            return Where;
        }
        public List<InvLocationBO> GetInvLocationInfoByCustomString(string customString)
        {
            List<InvLocationBO> nodeMatrixBOList = new List<InvLocationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvLocationInfoByCustomString_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CustomString", DbType.String, customString);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvLocationBO nodeMatrixBO = new InvLocationBO();

                                nodeMatrixBO.LocationId = Convert.ToInt32(reader["LocationId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.Name = reader["Name"].ToString();
                                nodeMatrixBO.Code = reader["Code"].ToString();
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
        public List<InvLocationBO> GetInvLocationInfoByLocation(string accountHead)
        {
            List<InvLocationBO> nodeMatrixBOList = new List<InvLocationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvLocationInfoByLocation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Location", DbType.String, accountHead);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvLocationBO nodeMatrixBO = new InvLocationBO();

                                nodeMatrixBO.LocationId = Convert.ToInt32(reader["LocationId"]);
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
        public List<string> GetInvLocationInfoByLocationNVoucherForm(string accountHead, int isVoucherForm)
        {
            List<string> result = new List<string>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvLocationInfoByLocation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Location", DbType.String, accountHead);
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
        public string GetInvLocationInfoBySpecificLocation(string accountHead)
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
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvLocationInfoBySpecificLocation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeHead", DbType.String, accountHead);
                    dbSmartAspects.AddInParameter(cmd, "@IsNodeHead", DbType.Int32, isNodeHead);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                result = reader["LocationId"].ToString();
                            }
                        }
                    }
                }
            }
            return result;
        }
        public List<InvLocationBO> GetInvItemLocationByCostCenter(int costCenterId)
        {
            List<InvLocationBO> catagoryList = new List<InvLocationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemLocationByCostCenter_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.String, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvLocationBO catagoryBO = new InvLocationBO();
                                catagoryBO.LocationId = Convert.ToInt32(reader["LocationId"]);
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.Code = reader["Code"].ToString();
                                catagoryBO.CompanyType = reader["CompanyType"].ToString();
                                catagoryBO.Description = reader["Description"].ToString();
                                catagoryBO.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                catagoryList.Add(catagoryBO);
                            }
                        }
                    }
                }
            }
            return catagoryList;
        }
        public List<InvLocationBO> GetInvItemLocationByProjectId(int projectId)
        {
            List<InvLocationBO> catagoryList = new List<InvLocationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemLocationByProjectId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvLocationBO catagoryBO = new InvLocationBO();
                                catagoryBO.LocationId = Convert.ToInt32(reader["LocationId"]);
                                catagoryBO.Name = reader["Name"].ToString();
                                catagoryBO.Code = reader["Code"].ToString();
                                catagoryBO.Description = reader["Description"].ToString();
                                catagoryList.Add(catagoryBO);
                            }
                        }
                    }
                }
            }
            return catagoryList;
        }
        public List<InvLocationBO> GetInvLocationInfoByCostCenterNameCodeAutoSearch(int costCenterId, string searchTerm)
        {
            List<InvLocationBO> location = new List<InvLocationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvLocationInfoByCostCenterNameCodeAutoSearch_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@SearchTerm", DbType.String, searchTerm);

                    DataSet CategoryDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, CategoryDS, "Location");
                    DataTable Table = CategoryDS.Tables["Location"];

                    location = Table.AsEnumerable().Select(r => new InvLocationBO
                    {

                        LocationId = r.Field<int>("LocationId"),
                        Name = r.Field<string>("Name"),
                        Code = r.Field<string>("Code"),
                        Description = r.Field<string>("Description")

                    }).ToList();
                }
            }

            return location;
        }
        public List<InvLocationBO> GetLocationInfoByAutoSearch(string location)
        {
            List<InvLocationBO> nodeMatrixBOList = new List<InvLocationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLocationInfoByAutoSearch_SP"))
                {
                    if (!string.IsNullOrEmpty(location))
                        dbSmartAspects.AddInParameter(cmd, "@Location", DbType.String, location);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Location", DbType.String, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvLocationBO nodeMatrixBO = new InvLocationBO();

                                nodeMatrixBO.LocationId = Convert.ToInt32(reader["LocationId"]);
                                nodeMatrixBO.AncestorId = Convert.ToInt32(reader["AncestorId"]);
                                nodeMatrixBO.Code = reader["Code"].ToString();
                                nodeMatrixBO.Name = reader["Name"].ToString();
                                //nodeMatrixBO.HeadWithCode = reader["HeadWithCode"].ToString();
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
        //public List<InvLocationBO> GetInvItemInformationByCostCenterIdNLocationId(int costCenterId, int locationId)
        //{
        //    List<InvLocationBO> catagoryList = new List<InvLocationBO>();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemInformationByCostCenterIdNLocationId_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.String, costCenterId);
        //            dbSmartAspects.AddInParameter(cmd, "@LocationId", DbType.String, locationId);

        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        InvLocationBO catagoryBO = new InvLocationBO();
        //                        catagoryBO.LocationId = Convert.ToInt32(reader["LocationId"]);
        //                        catagoryBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
        //                        catagoryBO.Name = reader["Name"].ToString();
        //                        catagoryBO.Code = reader["Code"].ToString();
        //                        catagoryBO.Description = reader["Description"].ToString();
        //                        catagoryList.Add(catagoryBO);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return catagoryList;
        //}
    }
}
