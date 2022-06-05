using HotelManagement.Entity.HouseKeeping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.HouseKeeping
{
    public class HotelRepairNMaintenanceDA : BaseService
    {
        public Boolean SaveOrUpdateRepairNMaintenance(HotelRepairNMaintenanceBO RepairNMaintenanceBO, out int tmpserviceId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateRepairNMaintenance_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, RepairNMaintenanceBO.Id);

                    dbSmartAspects.AddInParameter(command, "@MaintenanceType", DbType.String, RepairNMaintenanceBO.MaintenanceType);

                    if (!string.IsNullOrEmpty(RepairNMaintenanceBO.ItemName))
                        dbSmartAspects.AddInParameter(command, "@ItemName", DbType.String, RepairNMaintenanceBO.ItemName);
                    else
                        dbSmartAspects.AddInParameter(command, "@ItemName", DbType.String, DBNull.Value);

                    if (RepairNMaintenanceBO.ItemId != 0)
                        dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, RepairNMaintenanceBO.ItemId);
                    else
                        dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(command, "@Details", DbType.String, RepairNMaintenanceBO.Details);

                    dbSmartAspects.AddInParameter(command, "@MaintenanceArea", DbType.String, RepairNMaintenanceBO.MaintenanceArea);

                    if (RepairNMaintenanceBO.TransectionId != 0 && RepairNMaintenanceBO.TransectionId != null)
                        dbSmartAspects.AddInParameter(command, "@TransectionId", DbType.Int32, RepairNMaintenanceBO.TransectionId);
                    else
                        dbSmartAspects.AddInParameter(command, "@TransectionId", DbType.Int32, DBNull.Value);

                    dbSmartAspects.AddInParameter(command, "@IsEmergency", DbType.Boolean, RepairNMaintenanceBO.IsEmergency);

                    dbSmartAspects.AddInParameter(command, "@ExpectedDate", DbType.DateTime, RepairNMaintenanceBO.ExpectedDate);

                    dbSmartAspects.AddInParameter(command, "@ExpectedTime", DbType.Time, RepairNMaintenanceBO.ExpectedTime);

                    if (RepairNMaintenanceBO.RequestedById != 0 && RepairNMaintenanceBO.RequestedById != null)
                        dbSmartAspects.AddInParameter(command, "@RequestedById", DbType.Int32, RepairNMaintenanceBO.RequestedById);
                    else
                        dbSmartAspects.AddInParameter(command, "@RequestedById", DbType.Int32, DBNull.Value);

                    if (string.IsNullOrEmpty(RepairNMaintenanceBO.RequestedByName))
                        dbSmartAspects.AddInParameter(command, "@RequestedByName", DbType.String, RepairNMaintenanceBO.RequestedByName);
                    else
                        dbSmartAspects.AddInParameter(command, "@RequestedByName", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(command, "@UserId", DbType.Int32, RepairNMaintenanceBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpserviceId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                }
            }
            return status;
        }
        public List<HotelRepairNMaintenanceBO> GetRepairNMaintenanceForSearchCriteria(DateTime fromDate, DateTime toDate, string maintenanceType, int itemId, string itemName,
                                                                  string maintenanceArea, string isEmergency, int transectionId, int requestedById,
                                                                  string requestedByName, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<HotelRepairNMaintenanceBO> RepairNMaintenanceList = new List<HotelRepairNMaintenanceBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRepairNMaintenanceForPaging_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate ", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    if (maintenanceType != "0")
                        dbSmartAspects.AddInParameter(cmd, "@MaintenanceType", DbType.String, maintenanceType);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@MaintenanceType", DbType.String, DBNull.Value);

                    if (itemId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@ItemId ", DbType.Int32, itemId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemId ", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(itemName))
                        dbSmartAspects.AddInParameter(cmd, "@ItemName ", DbType.String, itemName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ItemName ", DbType.String, DBNull.Value);

                    if (maintenanceArea != "0")
                        dbSmartAspects.AddInParameter(cmd, "@MaintenanceArea ", DbType.String, maintenanceArea);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@MaintenanceArea ", DbType.String, DBNull.Value);

                    if (isEmergency == "All")
                        dbSmartAspects.AddInParameter(cmd, "@IsEmergency ", DbType.Boolean, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsEmergency ", DbType.Boolean, isEmergency == "0" ? false : true);

                    if (transectionId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@TransectionId ", DbType.Int32, transectionId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TransectionId ", DbType.Int32, DBNull.Value);

                    if (requestedById != 0)
                        dbSmartAspects.AddInParameter(cmd, "@RequestedById ", DbType.Int32, requestedById);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@RequestedById ", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(requestedByName))
                        dbSmartAspects.AddInParameter(cmd, "@RequestedByName ", DbType.String, requestedByName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@RequestedByName ", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HotelRepairNMaintenanceBO RepairNMaintenance = new HotelRepairNMaintenanceBO();
                                RepairNMaintenance.Id = Convert.ToInt64(reader["Id"]);
                                RepairNMaintenance.MaintenanceType = reader["MaintenanceType"].ToString();
                                RepairNMaintenance.ItemName = reader["ItemName"].ToString();
                                RepairNMaintenance.FixedItemName = reader["FixedItemName"].ToString();
                                RepairNMaintenance.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);// + " " + reader["DueTime"]);
                                RepairNMaintenance.ExpectedDate = Convert.ToDateTime(reader["ExpectedDate"]);// + " " + reader["DueTime"]);
                                RepairNMaintenance.ExpectedTime = Convert.ToDateTime(currentDate + " " + reader["ExpectedTime"]);
                                RepairNMaintenance.MaintenanceArea = reader["MaintenanceArea"].ToString();
                                RepairNMaintenance.IsEmergency = Convert.ToBoolean(reader["IsEmergency"]);
                                RepairNMaintenanceList.Add(RepairNMaintenance);
                            }
                        }
                    }
                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }
            return RepairNMaintenanceList;
        }

        public HotelRepairNMaintenanceBO GetHotelRepairNMaintenanceById(int id)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            HotelRepairNMaintenanceBO RepairNMaintenance = new HotelRepairNMaintenanceBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRepairNMaintenanceById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, id);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RepairNMaintenance.Id = Int64.Parse(reader["Id"].ToString());
                                RepairNMaintenance.MaintenanceType =(reader["MaintenanceType"].ToString());
                                RepairNMaintenance.ItemId = Convert.ToInt32(reader["ItemId"]);
                                RepairNMaintenance.ItemName = reader["ItemName"].ToString();
                                RepairNMaintenance.FixedItemName = reader["FixedItemName"].ToString();
                                RepairNMaintenance.MaintenanceArea = reader["MaintenanceArea"].ToString();
                                RepairNMaintenance.TransectionId = Convert.ToInt32(reader["TransectionId"]);
                                RepairNMaintenance.IsEmergency = Convert.ToBoolean(reader["IsEmergency"]);
                                RepairNMaintenance.ExpectedDate = Convert.ToDateTime(reader["ExpectedDate"]);
                                RepairNMaintenance.ExpectedTime = Convert.ToDateTime(currentDate+" "+reader["ExpectedTime"]);
                                RepairNMaintenance.RequestedById = Convert.ToInt32(reader["RequestedById"]);
                                RepairNMaintenance.RequestedByName = reader["RequestedByName"].ToString();
                                RepairNMaintenance.Details = reader["Details"].ToString();
                            }
                        }
                    }
                }
            }
            return RepairNMaintenance;
        }
    }
}
