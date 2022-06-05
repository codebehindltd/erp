using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.Restaurant;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Data.Restaurant
{
    public class TableManagementDA : BaseService
    {
        public List<TableManagementBO> GetAllTableInfoByCostCenterId(int costCenterId)
        {
            List<TableManagementBO> floorList = new List<TableManagementBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllTableInfoByCostCenterId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                TableManagementBO entityBO = new TableManagementBO();
                                entityBO.TableManagementId = Convert.ToInt32(reader["TableManagementId"]);
                                entityBO.TableId = Convert.ToInt32(reader["TableId"]);
                                entityBO.TableNumber = reader["TableNumber"].ToString();
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                floorList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return floorList;
        }
        public List<TableManagementBO> GetTableManagementInfo(int costCenterId)
        {
            List<TableManagementBO> floorList = new List<TableManagementBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTableManagementInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                TableManagementBO entityBO = new TableManagementBO();
                                entityBO.TableManagementId = Convert.ToInt32(reader["TableManagementId"]);
                                entityBO.TableId = Convert.ToInt32(reader["TableId"]);
                                entityBO.TableNumber = reader["TableNumber"].ToString();
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBO.StatusId = Convert.ToInt32(reader["StatusId"]);
                                entityBO.XCoordinate = Convert.ToDecimal(reader["XCoordinate"]);
                                entityBO.YCoordinate = Convert.ToDecimal(reader["YCoordinate"]);
                                entityBO.TableWidth = Convert.ToInt32(reader["TableWidth"]);
                                entityBO.TableHeight = Convert.ToInt32(reader["TableHeight"]);
                                entityBO.DivTransition = Convert.ToString(reader["DivTransition"]);
                                entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.BearerId = Convert.ToInt32(reader["BearerId"]);

                                floorList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return floorList;
        }

        public List<TableManagementBO> GetTableManagementNOrderInfo(int costCenterId, string sourceName)
        {
            List<TableManagementBO> floorList = new List<TableManagementBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTableManagementNOrderInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@SourceName", DbType.String, sourceName);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                TableManagementBO entityBO = new TableManagementBO();
                                entityBO.TableManagementId = Convert.ToInt32(reader["TableManagementId"]);
                                entityBO.TableId = Convert.ToInt32(reader["TableId"]);
                                entityBO.TableNumber = reader["TableNumber"].ToString();
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBO.StatusId = Convert.ToInt32(reader["StatusId"]);
                                entityBO.KotCreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                entityBO.KotCreatedByName = Convert.ToString(reader["KotCreatedByName"]);

                                entityBO.XCoordinate = Convert.ToDecimal(reader["XCoordinate"]);
                                entityBO.YCoordinate = Convert.ToDecimal(reader["YCoordinate"]);
                                entityBO.TableWidth = Convert.ToInt32(reader["TableWidth"]);
                                entityBO.TableHeight = Convert.ToInt32(reader["TableHeight"]);
                                entityBO.DivTransition = Convert.ToString(reader["DivTransition"]);
                                entityBO.CostCenterId = Convert.ToInt32(reader["CostCenterId"]);
                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.BearerId = Convert.ToInt32(reader["BearerId"]);

                                floorList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return floorList;
        }

        public Boolean UpdateDocumentsOwnerIdByTableManagementId(DocumentsBO docBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDocumentsOwnerIdByTableManagementId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, docBO.Id);
                    dbSmartAspects.AddInParameter(command, "@OwnerId", DbType.Int32, docBO.OwnerId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, docBO.CostCenterId);
                    dbSmartAspects.AddInParameter(command, "@DocumentCategory", DbType.String, docBO.DocumentCategory);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public TableManagementBO GetTableInfoByCostCenterNTableId(int costCenterId, long TableId)
        {
            TableManagementBO entityBO = new TableManagementBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTableInfoByCostCenterNTableId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@TableId", DbType.Int32, TableId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                               
                                entityBO.TableManagementId = Convert.ToInt32(reader["TableManagementId"]);
                                entityBO.TableId = Convert.ToInt32(reader["TableId"]);
                                entityBO.TableNumber = reader["TableNumber"].ToString();
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBO.StatusId = Convert.ToInt32(reader["StatusId"]);
                                entityBO.XCoordinate = Convert.ToDecimal(reader["XCoordinate"]);
                                entityBO.YCoordinate = Convert.ToDecimal(reader["YCoordinate"]);
                                entityBO.TableWidth = Convert.ToInt32(reader["TableWidth"]);
                                entityBO.TableHeight = Convert.ToInt32(reader["TableHeight"]);

                               
                            }
                        }
                    }
                }
            }
            return entityBO;
        }

        public Boolean DeleteDocumentsInfoByTableManagementId(int TableManagementId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDocumentsInfoByTableManagementId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TableManagementId", DbType.Int32, TableManagementId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public Boolean SaveHMFloorManagementInfo(TableManagementBO tableManagement, out int tmpTableManagementId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveTableManagementInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, tableManagement.CostCenterId);
                    dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, tableManagement.TableId);
                    dbSmartAspects.AddInParameter(command, "@XCoordinate", DbType.Decimal, tableManagement.XCoordinate);
                    dbSmartAspects.AddInParameter(command, "@YCoordinate", DbType.Decimal, tableManagement.YCoordinate);
                    dbSmartAspects.AddInParameter(command, "@TableWidth", DbType.Int32, tableManagement.TableWidth);
                    dbSmartAspects.AddInParameter(command, "@TableHeight", DbType.Int32, tableManagement.TableHeight);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, tableManagement.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@TableManagementId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpTableManagementId = Convert.ToInt32(command.Parameters["@TableManagementId"].Value);
                }
            }
            return status;
        }

        public Boolean UpdateTableManagementInfo(TableManagementBO floorManagement)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateTableManagementInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TableManagementId", DbType.Int32, floorManagement.TableManagementId);
                    dbSmartAspects.AddInParameter(command, "@CostCenterId", DbType.Int32, floorManagement.CostCenterId);
                    dbSmartAspects.AddInParameter(command, "@XCoordinate", DbType.Decimal, floorManagement.XCoordinate);
                    dbSmartAspects.AddInParameter(command, "@YCoordinate", DbType.Decimal, floorManagement.YCoordinate);
                    dbSmartAspects.AddInParameter(command, "@TableWidth", DbType.Int32, floorManagement.TableWidth);
                    dbSmartAspects.AddInParameter(command, "@TableHeight", DbType.Int32, floorManagement.TableHeight);
                    dbSmartAspects.AddInParameter(command, "@DivTransition", DbType.String, floorManagement.DivTransition);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, floorManagement.LastModifiedBy);


                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public List<TableManagementBO> GetTableInfoByCostCenterNStatus(int costCenterId, int status)
        {
            List<TableManagementBO> floorList = new List<TableManagementBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTableInfoByCostCenterNStatus_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@Status", DbType.Int32, status);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                TableManagementBO entityBO = new TableManagementBO();
                                entityBO.TableManagementId = Convert.ToInt32(reader["TableManagementId"]);
                                entityBO.TableId = Convert.ToInt32(reader["TableId"]);
                                entityBO.TableNumber = reader["TableNumber"].ToString();
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBO.StatusId = Convert.ToInt32(reader["StatusId"]);
                                entityBO.XCoordinate = Convert.ToDecimal(reader["XCoordinate"]);
                                entityBO.YCoordinate = Convert.ToDecimal(reader["YCoordinate"]);
                                entityBO.TableWidth = Convert.ToInt32(reader["TableWidth"]);
                                entityBO.TableHeight = Convert.ToInt32(reader["TableHeight"]);

                                floorList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return floorList;
        }

        public List<TableManagementBO> GetTableInfoByCostCenterNStatusNOrder(int costCenterId, int status)
        {
            List<TableManagementBO> floorList = new List<TableManagementBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTableInfoByCostCenterNStatusNOrder_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@Status", DbType.Int32, status);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                TableManagementBO entityBO = new TableManagementBO();
                                entityBO.TableManagementId = Convert.ToInt32(reader["TableManagementId"]);
                                entityBO.TableId = Convert.ToInt32(reader["TableId"]);
                                entityBO.TableNumber = reader["TableNumber"].ToString();
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBO.StatusId = Convert.ToInt32(reader["StatusId"]);
                                entityBO.XCoordinate = Convert.ToDecimal(reader["XCoordinate"]);
                                entityBO.YCoordinate = Convert.ToDecimal(reader["YCoordinate"]);
                                entityBO.TableWidth = Convert.ToInt32(reader["TableWidth"]);
                                entityBO.TableHeight = Convert.ToInt32(reader["TableHeight"]);

                                entityBO.KotId = Convert.ToInt32(reader["KotId"]);
                                entityBO.BearerId = Convert.ToInt32(reader["BearerId"]);
                                entityBO.DetailId = Convert.ToInt32(reader["DetailId"]);
                                entityBO.BillId = Convert.ToInt32(reader["BillId"]);
                                entityBO.Remarks = Convert.ToString(reader["Remarks"]);

                                floorList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return floorList;
        }


        //------------------Test------------------------
        public List<TableManagementBO> GetRestaurantItemGroupInfo()
        {
            List<TableManagementBO> floorList = new List<TableManagementBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantItemGroupInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                TableManagementBO entityBO = new TableManagementBO();
                                entityBO.TableManagementId = Convert.ToInt32(reader["TableManagementId"]);
                                entityBO.TableId = Convert.ToInt32(reader["TableId"]);
                                entityBO.TableNumber = reader["TableNumber"].ToString();
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                entityBO.XCoordinate = Convert.ToDecimal(reader["XCoordinate"]);
                                entityBO.YCoordinate = Convert.ToDecimal(reader["YCoordinate"]);
                                entityBO.TableWidth = Convert.ToInt32(reader["TableWidth"]);
                                entityBO.TableHeight = Convert.ToInt32(reader["TableHeight"]);

                                floorList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return floorList;
        }
    }
}
