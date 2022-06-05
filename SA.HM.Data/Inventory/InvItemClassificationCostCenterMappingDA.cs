using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Inventory;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Inventory
{
    public class InvItemClassificationCostCenterMappingDA : BaseService
    {
        public List<ItemClassificationBO> GetActiveItemClassificationInfo()
        {
            List<ItemClassificationBO> itemClassificationBOList = new List<ItemClassificationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveItemClassificationInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                ItemClassificationBO itemClassificationBO = new ItemClassificationBO();

                                itemClassificationBO.ClassificationId = Convert.ToInt32(reader["Id"]);
                                itemClassificationBO.ClassificationName = reader["Name"].ToString();
                                itemClassificationBO.ActiveStat = Convert.ToBoolean(reader["IsActive"]);
                                //itemClassificationBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                itemClassificationBOList.Add(itemClassificationBO);
                            }
                        }
                    }
                }
            }
            return itemClassificationBOList;
        }

        public bool SaveInvItemClassificationCostCenterMappingInfo(InvItemClassificationCostCenterMappingBO mappingBO, out Int64 MappingId)
        {

            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveInvItemClassificationCostCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ClassificationId", DbType.Int64, mappingBO.ClassificationId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int64, mappingBO.CostCenterId);
                    dbSmartAspects.AddInParameter(command, "@AccountHeadId", DbType.Int64, mappingBO.AccountHeadId != 0 ? mappingBO.AccountHeadId : 0);
                    dbSmartAspects.AddOutParameter(command, "@MappingId", DbType.Int64, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    MappingId = Convert.ToInt64(command.Parameters["@MappingId"].Value);
                }
            }
            return status;
        }
        public bool UpdateInvItemClassificationCostCenterMappingInfo(InvItemClassificationCostCenterMappingBO mappingBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateInvItemClassificationCostCenterMappingInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@MappingId", DbType.Int64, mappingBO.MappingId);
                    dbSmartAspects.AddInParameter(command, "@ClassificationId", DbType.Int64, mappingBO.ClassificationId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int64, mappingBO.CostCenterId);
                    dbSmartAspects.AddInParameter(command, "@AccountHeadId", DbType.Int64, mappingBO.AccountHeadId != 0 ? mappingBO.AccountHeadId : 0);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public List<InvItemClassificationCostCenterMappingBO> GetInvItemClassificationCostCenterMappingByCostCenterId(Int64 costcenterId)
        {
            List<InvItemClassificationCostCenterMappingBO> mappingList = new List<InvItemClassificationCostCenterMappingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemClassificationCostCenterMappingByCostCenterId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int64, costcenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemClassificationCostCenterMappingBO mappingBO = new InvItemClassificationCostCenterMappingBO();
                                mappingBO.ClassificationId = Convert.ToInt64(reader["ClassificationId"]);
                                mappingBO.MappingId = Convert.ToInt64(reader["MappingId"]);
                                mappingBO.CostCenterId = Convert.ToInt64(reader["CostCenterId"]);
                                if(reader["AccountsPostingHeadId"]!=DBNull.Value)
                                    mappingBO.AccountHeadId = Convert.ToInt64(reader["AccountsPostingHeadId"]);
                                mappingList.Add(mappingBO);
                            }
                        }
                    }
                }
            }
            return mappingList;
        }
        public Boolean DeleteAllInvItemClassificationCostCenterMappingInfoWithoutMappingIdList(Int64 classificationId, string mappingIdList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteAllInvItemClassificationCostCenterMappingInfoWithoutMappingIdList_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ClassificationId", DbType.Int64, classificationId);
                    dbSmartAspects.AddInParameter(command, "@MappingIdList", DbType.String, mappingIdList);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool SaveInvClassificationName(InvItemClassificationBO invItemClassificationBO, List<InvItemClassificationCostCenterMappingBO> mappingBO, out Int64 classifacationId, out Int64 mappingId)
        {
            Boolean status = false;
            classifacationId = 0;
            mappingId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveInvClassificationInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@ClassificationName", DbType.String, invItemClassificationBO.ClassificationName);
                            dbSmartAspects.AddInParameter(command, "@IsActive", DbType.Boolean, invItemClassificationBO.IsActive);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int64, invItemClassificationBO.CreatedBy);

                            dbSmartAspects.AddOutParameter(command, "@ClassificationId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                            classifacationId = Convert.ToInt64(command.Parameters["@ClassificationId"].Value);

                        }

                        if (status)
                        {
                            foreach (var item in mappingBO)
                            {
                                item.ClassificationId = classifacationId;
                                status = this.SaveInvItemClassificationCostCenterMappingInfo(item, out mappingId);
                                if (!status)
                                {
                                    transaction.Rollback();
                                    break;
                                }

                            }
                            if (status)
                                transaction.Commit();
                        }
                        else
                            transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            return status;
        }
        public bool UpdateInvClassificationName(InvItemClassificationBO invItemClassificationBO, List<InvItemClassificationCostCenterMappingBO> mappingBO,Boolean isDeleteOnEdit)
        {
            Int64 mapId = 0;
            //mappingId = null;
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateInvClassificationInfo_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@ClassificationId", DbType.Int64, invItemClassificationBO.ClassificationId);
                            dbSmartAspects.AddInParameter(command, "@ClassificationName", DbType.String, invItemClassificationBO.ClassificationName);
                            dbSmartAspects.AddInParameter(command, "@IsActive", DbType.Boolean, invItemClassificationBO.IsActive);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int64, invItemClassificationBO.LastModifiedBy);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        }
                        if (status)
                        {
                            foreach (var item in mappingBO)
                            {
                                if (item.MappingId == 0)
                                {
                                    status = this.SaveInvItemClassificationCostCenterMappingInfo(item, out mapId);
                                    //mappingId.Add(mapId);
                                }
                                    
                                else
                                    status = this.UpdateInvItemClassificationCostCenterMappingInfo(item);
                                if (!status)
                                {
                                    transaction.Rollback();
                                    break;
                                }
                            }
                            if (status == true && isDeleteOnEdit==true)
                            {
                                String mappingIdList=string.Join(",", mappingBO.Select(x=>x.MappingId).ToList());
                                status = this.DeleteAllInvItemClassificationCostCenterMappingInfoWithoutMappingIdList(invItemClassificationBO.ClassificationId, mappingIdList);
                            }
                            if(status)
                                transaction.Commit();
                            else
                                transaction.Rollback();
                        }
                        else
                            transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                }


            }
            return status;
        }
        public List<InvItemClassificationBO> GetItemClassificationInformationBySearchCriteriaForPaging(string classificationName, Boolean IsActive, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<InvItemClassificationBO> itemClassificationList = new List<InvItemClassificationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetItemClassificationInfoBySearchCriteriaForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ClassificationName", DbType.String, classificationName);
                    dbSmartAspects.AddInParameter(command, "@IsActive", DbType.Boolean, IsActive);
                    dbSmartAspects.AddInParameter(command, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(command, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(command, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(command))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemClassificationBO itemClassification = new InvItemClassificationBO();
                                itemClassification.ClassificationId = Convert.ToInt64(reader["Id"]);
                                itemClassification.ClassificationName = reader["Name"].ToString();
                                itemClassification.IsActive = Convert.ToBoolean(reader["IsActive"]);
                                itemClassification.ActiveStatus = reader["ActiveStatus"].ToString();
                                itemClassificationList.Add(itemClassification);
                            }
                        }
                    }
                    totalRecords = (int)command.Parameters["@RecordCount"].Value;

                }
            }

            return itemClassificationList;
        }
        public InvItemClassificationBO GetItemClassificationInfoById(Int64 classificationId)
        {
            InvItemClassificationBO invItemClassification = new InvItemClassificationBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("GetInvClassificationInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@ClassificationId", DbType.Int64, classificationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(command))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                invItemClassification.ClassificationId = Convert.ToInt64(reader["Id"]);
                                invItemClassification.ClassificationName = reader["Name"].ToString();
                                invItemClassification.IsActive = Convert.ToBoolean(reader["IsActive"]);
                                invItemClassification.ActiveStatus = reader["IsActive"].ToString();
                            }
                        }
                    }
                }
            }
            return invItemClassification;
        }
        public List<InvItemClassificationCostCenterMappingBO> GetInvItemClassificationCostCenterMappingInfoByClassificationId(Int64 classificationId)
        {
            List<InvItemClassificationCostCenterMappingBO> mappingList = new List<InvItemClassificationCostCenterMappingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemClassificationCostCenterMappingByClassificationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ClassificationId", DbType.Int64, classificationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemClassificationCostCenterMappingBO mappingBO = new InvItemClassificationCostCenterMappingBO();
                                mappingBO.ClassificationId = Convert.ToInt64(reader["ClassificationId"]);
                                mappingBO.MappingId = Convert.ToInt64(reader["MappingId"]);
                                mappingBO.CostCenterId = Convert.ToInt64(reader["CostCenterId"]);
                                mappingBO.AccountHeadId = Convert.ToInt64(reader["AccountsPostingHeadId"]);
                                mappingList.Add(mappingBO);
                            }
                        }
                    }
                }
            }
            return mappingList;
        }
        public Boolean DeleteInvClassification(Int64 classificationId)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteItemClassificationInfoByClassificationId_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@ClassificationId", DbType.Int64, classificationId);
                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            if (!status)
                                transaction.Rollback();
                            else
                                transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                }
            }
            return status;
        }

    }
}
