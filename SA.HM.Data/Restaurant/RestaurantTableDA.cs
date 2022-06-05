using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantTableDA : BaseService
    {
        public List<RestaurantTableBO> GetRestaurantTableInfo()
        {
            List<RestaurantTableBO> tableList = new List<RestaurantTableBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantTableInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantTableBO tableBO = new RestaurantTableBO();
                                tableBO.TableId = Convert.ToInt32(reader["TableId"]);
                                tableBO.TableNumber = reader["TableNumber"].ToString();
                                tableBO.TableCapacity = reader["TableCapacity"].ToString();
                                tableBO.Remarks = reader["Remarks"].ToString();
                                tableList.Add(tableBO);
                            }
                        }
                    }
                }
            }
            return tableList;
        }
        public bool GetRestaurantTableInfoByName(string TableNumber)
        {
            RestaurantTableBO tableBO = new RestaurantTableBO();
            bool available = true;
            TableNumber = TableNumber.Trim();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantTableInfoByName_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TableNumber", DbType.String, TableNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                available = false;
                            }
                        }
                    }
                }
            }
            return available;
        }
        public RestaurantTableBO GetRestaurantTableInfoById(int costCenterId, string orderType, int sourceId)
        {
            RestaurantTableBO tableBO = new RestaurantTableBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantTableInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCenterId);
                    dbSmartAspects.AddInParameter(cmd, "@OrderType", DbType.String, orderType);
                    dbSmartAspects.AddInParameter(cmd, "@SourceId", DbType.Int32, sourceId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                tableBO.TableId = Convert.ToInt32(reader["TableId"]);
                                tableBO.TableNumber = reader["TableNumber"].ToString();
                                tableBO.TableCapacity = reader["TableCapacity"].ToString();
                                tableBO.StatusId = Convert.ToInt32(reader["StatusId"]);
                                tableBO.Status = reader["StatusName"].ToString();
                                tableBO.Remarks = reader["Remarks"].ToString();
                            }
                        }
                    }
                }
            }
            return tableBO;
        }
        public bool SaveRestaurantTableInfo(RestaurantTableBO tableBO, out int tmpUserInfoId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurantTableInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TableNumber", DbType.String, tableBO.TableNumber);
                    dbSmartAspects.AddInParameter(command, "@TableCapacity", DbType.String, tableBO.TableCapacity);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, tableBO.Remarks);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, tableBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@TableId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpUserInfoId = Convert.ToInt32(command.Parameters["@TableId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateRestaurantTableInfo(RestaurantTableBO tableBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantTableInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TableId", DbType.Int32, tableBO.TableId);
                    dbSmartAspects.AddInParameter(command, "@TableNumber", DbType.String, tableBO.TableNumber);
                    dbSmartAspects.AddInParameter(command, "@TableCapacity", DbType.String, tableBO.TableCapacity);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, tableBO.Remarks);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, tableBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public RestaurantTableBO GetRestaurantTableInfoByTableNumber(string TableNumber)
        {
            RestaurantTableBO tableBO = new RestaurantTableBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantTableInfoByTableNumber_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TableNumber", DbType.String, TableNumber);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                tableBO.TableId = Convert.ToInt32(reader["TableId"]);
                                tableBO.TableNumber = reader["TableNumber"].ToString();
                                tableBO.TableCapacity = reader["TableCapacity"].ToString();
                                tableBO.Remarks = reader["Remarks"].ToString();
                            }
                        }
                    }
                }
            }
            return tableBO;
        }
        public RestaurantTableBO GetRestaurantTableInfoByTableId(int tableId)
        {
            RestaurantTableBO tableBO = new RestaurantTableBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantTableInfoByTableId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TableId", DbType.Int32, tableId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                tableBO.TableId = Convert.ToInt32(reader["TableId"]);
                                tableBO.TableNumber = reader["TableNumber"].ToString();
                                tableBO.TableCapacity = reader["TableCapacity"].ToString();
                                tableBO.Remarks = reader["Remarks"].ToString();
                            }
                        }
                    }
                }
            }
            return tableBO;
        }
        public RestaurantTableBO GetRestaurantTableInfoByTableId(string tableId)
        {
            RestaurantTableBO tableBO = new RestaurantTableBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantTableInfoByTableId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TableId", DbType.String, tableId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                tableBO.TableId = Convert.ToInt32(reader["TableId"]);
                                tableBO.TableNumber = reader["TableNumber"].ToString();
                                tableBO.TableCapacity = reader["TableCapacity"].ToString();
                                tableBO.Remarks = reader["Remarks"].ToString();
                            }
                        }
                    }
                }
            }
            return tableBO;
        }
        public List<RestaurantTableBO> GetRestaurantTableInfoByMultipleTableId(string tableId)
        {
            List<RestaurantTableBO> tableBO = new List<RestaurantTableBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantTableInfoByMultipleTableId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TableId", DbType.String, tableId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantTableBO tables = new RestaurantTableBO();
                                tables.TableId = Convert.ToInt32(reader["TableId"]);
                                tables.TableNumber = reader["TableNumber"].ToString();
                                tables.TableCapacity = reader["TableCapacity"].ToString();
                                tables.Remarks = reader["Remarks"].ToString();

                                tableBO.Add(tables);
                            }
                        }
                    }
                }
            }
            return tableBO;
        }
        public List<RestaurantTableBO> GetRestaurantTableInfoBySearchCriteria(string TableNumber)
        {
            List<RestaurantTableBO> tableList = new List<RestaurantTableBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantTableInfoBySearchCriteria_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(TableNumber))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@TableNumber", DbType.String, TableNumber);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@TableNumber", DbType.String, DBNull.Value);
                    }

                    DataSet restaurantTableDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, restaurantTableDS, "RestaurantTable");
                    DataTable Table = restaurantTableDS.Tables["RestaurantTable"];

                    tableList = Table.AsEnumerable().Select(r => new RestaurantTableBO
                    {
                        TableId = r.Field<Int32>("TableId"),
                        TableNumber = r.Field<string>("TableNumber"),
                        TableCapacity = r.Field<string>("TableCapacity"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }
            return tableList;
        }
        public bool DeleteResturantTabeInfo(int tableId)
        {
            bool retVal = false;
            int status = 0;
            string deleteQuery = string.Empty;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                deleteQuery = "DELETE FROM RestaurantTable WHERE TableId = " + tableId;
                conn.Open();
                using (DbCommand commandMaster = dbSmartAspects.GetSqlStringCommand(deleteQuery))
                {
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        commandMaster.CommandText = deleteQuery;
                        commandMaster.CommandType = CommandType.Text;

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        if (status > 0)
                        {
                            deleteQuery = "DELETE FROM RestaurantTableManagement WHERE TableId = " + tableId;
                            commandMaster.CommandText = deleteQuery;
                            commandMaster.CommandType = CommandType.Text;

                            status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                            if (status > 0)
                            {
                                retVal = true;
                            }
                            else
                            {
                                transction.Rollback();
                                retVal = false;
                            }
                        }
                        else
                        {
                            transction.Rollback();
                            retVal = false;
                        }
                        if (retVal)
                        {
                            transction.Commit();
                        }
                    }
                }
            }

            return retVal;
        }
        public List<RestaurantTableBO> GetAvailableTableNumberInformation(int costCentreId, int arriveHour, int departHour)
        {
            List<RestaurantTableBO> tableNumberList = new List<RestaurantTableBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAvailableTableNumberInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCentreId", DbType.Int32, costCentreId);
                    dbSmartAspects.AddInParameter(cmd, "@ArriveHour", DbType.Int32, arriveHour);
                    dbSmartAspects.AddInParameter(cmd, "@DepartHour", DbType.Int32, departHour);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantTableBO tableNumber = new RestaurantTableBO();

                                tableNumber.TableId = Convert.ToInt32(reader["TableId"]);
                                tableNumber.TableNumber = reader["TableNumber"].ToString();
                                tableNumber.TableCapacity = reader["TableCapacity"].ToString();

                                tableNumberList.Add(tableNumber);
                            }
                        }
                    }
                }
            }
            return tableNumberList;
        }
        public List<RestaurantTableBO> GetTableInfoByCostCentre(int costCentreId)
        {
            List<RestaurantTableBO> tableNumberList = new List<RestaurantTableBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTableInfoByCostCentre_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@CostCentreId", DbType.Int32, costCentreId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantTableBO tableNumber = new RestaurantTableBO();

                                tableNumber.TableId = Convert.ToInt32(reader["TableId"]);
                                tableNumber.TableNumber = reader["TableNumber"].ToString();
                                tableNumber.TableCapacity = reader["TableCapacity"].ToString();

                                tableNumberList.Add(tableNumber);
                            }
                        }
                    }
                }
            }
            return tableNumberList;
        }
        public List<TableReservationForCalendarViewBO> GetAllReservedTableInfoForCalendar(DateTime currentDate, int costCentreId)
        {
            List<TableReservationForCalendarViewBO> entityBOList = new List<TableReservationForCalendarViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetReservedTableInfoForCalender_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@currentDate", DbType.DateTime, currentDate);
                    dbSmartAspects.AddInParameter(cmd, "@CostCenterId", DbType.Int32, costCentreId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                TableReservationForCalendarViewBO entityBO = new TableReservationForCalendarViewBO();
                                entityBO.ReservationId = Convert.ToInt32(reader["ReservationId"]);
                                entityBO.TableId = Convert.ToInt32(reader["TableId"]);
                                entityBO.ContactPerson = reader["ContactPerson"].ToString();
                                entityBO.ArriveHour = Convert.ToInt32(reader["ArriveHour"]);
                                entityBO.DepartHour = Convert.ToInt32(reader["DepartureHour"]);

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public bool SaveEmpKotBillDetailInfo(EmpKotBillDetailBO tableBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                //foreach (EmpKotBillDetailBO row in tableBOList)
                //{
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpKotBillDetailInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, tableBO.EmpId);
                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, tableBO.KotId);
                    dbSmartAspects.AddInParameter(command, "@BillNumber", DbType.String, tableBO.BillNumber);
                    dbSmartAspects.AddInParameter(command, "@KotDetailIdList", DbType.String, tableBO.KotDetailIdList);
                    dbSmartAspects.AddInParameter(command, "@JobStartDate", DbType.DateTime, tableBO.JobStartDate);
                    dbSmartAspects.AddInParameter(command, "@JobEndDate", DbType.DateTime, tableBO.JobEndDate);
                    dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, tableBO.Remarks);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, tableBO.CreatedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
                //}
            }
            return status;
        }
        public bool UpdateEmpKotBillDetailInfo(EmpKotBillDetailBO tableBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpKotBillDetailInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, tableBO.EmpId);
                    dbSmartAspects.AddInParameter(command, "@DetailId", DbType.Int32, tableBO.DetailId);
                    dbSmartAspects.AddInParameter(command, "@KotDetailId", DbType.Int32, tableBO.KotDetailId);
                    dbSmartAspects.AddInParameter(command, "@JobStatus", DbType.String, tableBO.JobStatus);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, tableBO.CreatedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    status = true;
                }
            }
            return status;
        }
        public List<EmpKotBillDetailBO> GetEmpKotBillDetailInformation(int empId, DateTime srcFromDate, DateTime srcToDate, string srcType, string jobStatus)
        {
            List<EmpKotBillDetailBO> entityBOList = new List<EmpKotBillDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpKotBillDetailInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.String, empId);
                    dbSmartAspects.AddInParameter(cmd, "@StartDate", DbType.DateTime, srcFromDate);
                    dbSmartAspects.AddInParameter(cmd, "@EndDate", DbType.DateTime, srcToDate);
                    dbSmartAspects.AddInParameter(cmd, "@SearchType", DbType.String, srcType);
                    dbSmartAspects.AddInParameter(cmd, "@JobStatus", DbType.String, jobStatus);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpKotBillDetailBO entityBO = new EmpKotBillDetailBO();
                                entityBO.DetailId = Convert.ToInt32(reader["DetailId"]);
                                entityBO.KotDetailId = Convert.ToInt32(reader["KotDetailId"]);
                                entityBO.EmpCode = reader["EmpCode"].ToString();
                                entityBO.EmpName = reader["EmpName"].ToString();
                                entityBO.BillNumber = reader["BillNumber"].ToString();
                                entityBO.ItemName = reader["ItemName"].ToString();
                                entityBO.JobStatus = reader["JobStatus"].ToString();
                                entityBO.JobStartDate = Convert.ToDateTime(reader["JobStartDate"]);
                                entityBO.JobStartDateString = reader["JobStartDateString"].ToString();
                                entityBO.JobEndDate = Convert.ToDateTime(reader["JobEndDate"]);
                                entityBO.JobEndDateString = reader["JobEndDateString"].ToString();
                                //entityBO.DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]);
                                entityBO.DeliveryDateString = reader["DeliveryDateString"].ToString();

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
    }
}
