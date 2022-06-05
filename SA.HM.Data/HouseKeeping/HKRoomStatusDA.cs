using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HouseKeeping;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity;

namespace HotelManagement.Data.HouseKeeping
{
    public class HKRoomStatusDA : BaseService
    {
        public List<HKRoomStatusBO> GetHKRoomStatusType()
        {
            List<HKRoomStatusBO> hkRoomStatusList = new List<HKRoomStatusBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHKRoomStatusInfo_SP"))
                {
                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "HKRoomStatus");
                    DataTable Table = SaleServiceDS.Tables["HKRoomStatus"];

                    hkRoomStatusList = Table.AsEnumerable().Select(r => new HKRoomStatusBO
                    {
                        HKRoomStatusId = r.Field<long>("HKRoomStatusId"),
                        StatusName = r.Field<string>("StatusName"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }
            return hkRoomStatusList;
        }
        public List<HKRoomStatusViewBO> GetHKRoomStatusInfo(string foroomIds, string hkroomIds, int floorId, int floorBlockId)
        {
            List<HKRoomStatusViewBO> roomInfos = new List<HKRoomStatusViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHKRoomStatus_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    if (!string.IsNullOrEmpty(foroomIds))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FORoomStatusIds", DbType.String, foroomIds);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@FORoomStatusIds", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(hkroomIds))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@HKRoomStatusIds", DbType.String, hkroomIds);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@HKRoomStatusIds", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@FloorId", DbType.Int32, floorId);
                    dbSmartAspects.AddInParameter(cmd, "@BlockId", DbType.Int32, floorBlockId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "HKRoomStatus");
                    DataTable Table = ds.Tables["HKRoomStatus"];
                    roomInfos = Table.AsEnumerable().Select(r =>
                                new HKRoomStatusViewBO
                                {
                                    RoomId = r.Field<int>("RoomId"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    RoomType = r.Field<string>("RoomType"),
                                    FORoomStatus = r.Field<string>("FORoomStatus"),
                                    HKRoomStatus = r.Field<string>("HKRoomStatus"),
                                    Floor = r.Field<string>("Floor"),
                                    LastCleanDate = r.Field<DateTime>("LastCleanDate"),
                                    ShowLastCleanDate = r.Field<string>("ShowLastCleanDate"),
                                    Remarks = r.Field<string>("Remarks"),
                                    DateIn = r.Field<string>("DateIn"),
                                    DateOut = r.Field<string>("DateOut"),
                                    FromDateTime = r.Field<DateTime?>("FromDateTime"),
                                    ToDateTime = r.Field<DateTime?>("ToDateTime"),                    
                                    ShowToDate = r.Field<string>("ShowToDate"),
                                    Reason = r.Field<string>("Reason"),
                                }).ToList();
                }
            }
            return roomInfos;
        }
        public List<HKRoomStatusViewBO> GetRoomStatus(int floorId, int foStatusId, int blockId)
        {
            List<HKRoomStatusViewBO> roomInfos = new List<HKRoomStatusViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHKRoomConditions_SP"))
                {
                    if (floorId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FloorId", DbType.Int32, floorId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@FloorId", DbType.Int32, DBNull.Value);

                    if (blockId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@BlockId", DbType.Int32, blockId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@BlockId", DbType.Int32, DBNull.Value);

                    if (foStatusId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FOStatusId", DbType.Int32, foStatusId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@FOStatusId", DbType.Int32, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "HKRoomConditions");
                    DataTable Table = ds.Tables["HKRoomConditions"];
                    roomInfos = Table.AsEnumerable().Select(r =>
                                new HKRoomStatusViewBO
                                {
                                    RoomId = r.Field<int>("RoomId"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    RoomType = r.Field<string>("RoomType"),
                                    FORoomStatus = r.Field<string>("FORoomStatus"),
                                    HKRoomStatus = r.Field<string>("HKRoomStatus"),
                                    Floor = r.Field<string>("Floor"),
                                    FOPersons = r.Field<int?>("FOPersons"),
                                    DateIn = r.Field<string>("DateIn"),
                                    DateOut = r.Field<string>("DateOut")
                                }).ToList();
                }
            }
            return roomInfos;
        }

        public List<HKRoomStatusViewBO> GetHKRoomConditionsForTaskAssignment(int floorId, int foStatusId, int blockId, string roomNumberFrom, string roomNumberTo)
        {
            List<HKRoomStatusViewBO> roomInfos = new List<HKRoomStatusViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHKRoomConditionsForTaskAssignment_SP"))
                {
                    if (floorId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FloorId", DbType.Int32, floorId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@FloorId", DbType.Int32, DBNull.Value);

                    if (blockId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@BlockId", DbType.Int32, blockId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@BlockId", DbType.Int32, DBNull.Value);

                    if (foStatusId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@FOStatusId", DbType.Int32, foStatusId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@FOStatusId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(roomNumberFrom))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumberFrom", DbType.String, roomNumberFrom);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumberFrom", DbType.String, DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(roomNumberTo))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumberTo", DbType.String, roomNumberTo);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RoomNumberTo", DbType.String, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "HKRoomConditions");
                    DataTable Table = ds.Tables["HKRoomConditions"];
                    roomInfos = Table.AsEnumerable().Select(r =>
                                new HKRoomStatusViewBO
                                {
                                    RoomId = r.Field<int>("RoomId"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    RoomType = r.Field<string>("RoomType"),
                                    FORoomStatus = r.Field<string>("FORoomStatus"),
                                    HKRoomStatus = r.Field<string>("HKRoomStatus"),
                                    Floor = r.Field<string>("Floor"),
                                    FOPersons = r.Field<int?>("FOPersons"),
                                    DateIn = r.Field<string>("DateIn"),
                                    DateOut = r.Field<string>("DateOut")
                                }).ToList();
                }
            }
            return roomInfos;
        }

        //Room Wise Task Assignment & Feedback
        public List<HotelRoomConditionBO> GetHotelRoomConditions()
        {
            List<HotelRoomConditionBO> hkRoomStatusList = new List<HotelRoomConditionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHotelRoomConditions_SP"))
                {
                    DataSet SaleServiceDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "HotelRoomConditions");
                    DataTable Table = SaleServiceDS.Tables["HotelRoomConditions"];

                    hkRoomStatusList = Table.AsEnumerable().Select(r => new HotelRoomConditionBO
                    {
                        RoomConditionId = r.Field<long>("RoomConditionId"),
                        ConditionName = r.Field<string>("ConditionName")

                    }).ToList();
                }
            }
            return hkRoomStatusList;
        }
        public Boolean SaveEmpTaskAssignment(HotelEmpTaskAssignmentBO task, List<TaskAssignmentToEmployeeBO> saveEmployeeForTaskList, List<TaskAssignmentRoomWiseBO> saveRoomWiseTaskList, out long taskId)
        {
            int status = 0;
             taskId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpTaskAssignment_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Shift", DbType.String, task.Shift);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.String, task.CreatedBy);
                            dbSmartAspects.AddOutParameter(command, "@TaskId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            taskId = Convert.ToInt64(command.Parameters["@TaskId"].Value);
                        }

                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRoomWiseTaskAssignment_SP"))
                        {
                            foreach (TaskAssignmentRoomWiseBO bo in saveRoomWiseTaskList)
                            {
                                command.Parameters.Clear();

                                dbSmartAspects.AddInParameter(command, "@TaskId", DbType.Int32, taskId);
                                dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, bo.RoomId);
                                dbSmartAspects.AddInParameter(command, "@HKRoomStatusId", DbType.Int64, bo.HKRoomStatusId);
                                dbSmartAspects.AddInParameter(command, "@TaskDetails", DbType.String, bo.TaskDetails);
                                dbSmartAspects.AddInParameter(command, "@TaskStatus", DbType.String, bo.TaskStatus);

                                status = dbSmartAspects.ExecuteNonQuery(command, transction);
                            }
                        }

                        if (status > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveTaskAssignmentToEmployee_SP"))
                            {
                                foreach (TaskAssignmentToEmployeeBO bo in saveEmployeeForTaskList)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@TaskId", DbType.Int32, taskId);
                                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, bo.EmpId);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }

                        if (status > 0)
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
                        status = 0;
                        transction.Rollback();
                        throw ex;
                    }
                }


            }

            return status > 0 ? true : false;
        }
        public Boolean UpdateEmpTaskAssignment(HotelEmpTaskAssignmentBO task, List<TaskAssignmentToEmployeeBO> saveEmployeeForTaskList,
                                               List<TaskAssignmentRoomWiseBO> saveRoomWiseTaskList, List<TaskAssignmentToEmployeeBO> deleteEmployeeForTaskList,
                                               List<TaskAssignmentRoomWiseBO> deleteRoomWiseTaskList)
        {
            int status = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();

                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpTaskAssignment_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@TaskId", DbType.Int64, task.TaskId);
                            dbSmartAspects.AddInParameter(command, "@Shift", DbType.String, task.Shift);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.String, task.CreatedBy);

                            if (!string.IsNullOrEmpty(task.RoomNumber))
                            {
                                dbSmartAspects.AddInParameter(command, "@RoomNumber", DbType.String, task.RoomNumber);
                            }
                            else
                            {
                                dbSmartAspects.AddInParameter(command, "@RoomNumber", DbType.String, DBNull.Value);
                            }

                            if (task.FloorId != 0)
                            {
                                dbSmartAspects.AddInParameter(command, "@FloorId", DbType.String, task.FloorId);
                            }
                            else
                            {
                                dbSmartAspects.AddInParameter(command, "@FloorId", DbType.String, DBNull.Value);
                            }

                            status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        }

                        if (status > 0 && saveRoomWiseTaskList.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRoomWiseTaskAssignment_SP"))
                            {
                                foreach (TaskAssignmentRoomWiseBO bo in saveRoomWiseTaskList)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@TaskId", DbType.Int32, task.TaskId);
                                    dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, bo.RoomId);
                                    dbSmartAspects.AddInParameter(command, "@HKRoomStatusId", DbType.Int64, bo.HKRoomStatusId);
                                    dbSmartAspects.AddInParameter(command, "@TaskDetails", DbType.String, bo.TaskDetails);
                                    dbSmartAspects.AddInParameter(command, "@TaskStatus", DbType.String, bo.TaskStatus);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }

                        if (status > 0 && saveEmployeeForTaskList.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveTaskAssignmentToEmployee_SP"))
                            {
                                foreach (TaskAssignmentToEmployeeBO bo in saveEmployeeForTaskList)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@TaskId", DbType.Int32, task.TaskId);
                                    dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, bo.EmpId);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }

                        if (status > 0 && deleteRoomWiseTaskList.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (TaskAssignmentRoomWiseBO bo in deleteRoomWiseTaskList)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "HotelTaskAssignmentRoomWise");
                                    dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "RoomTaskId");
                                    dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, bo.RoomTaskId);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }

                        if (status > 0 && deleteEmployeeForTaskList.Count > 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (TaskAssignmentToEmployeeBO bo in deleteEmployeeForTaskList)
                                {
                                    command.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(command, "@TableName", DbType.String, "HotelTaskAssignmentToEmployee");
                                    dbSmartAspects.AddInParameter(command, "@TablePKField", DbType.String, "EmpTaskId");
                                    dbSmartAspects.AddInParameter(command, "@TablePKId", DbType.String, bo.EmpTaskId);

                                    status = dbSmartAspects.ExecuteNonQuery(command, transction);
                                }
                            }
                        }

                        if (status > 0)
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
                        status = 0;
                        transction.Rollback();
                        throw ex;
                    }
                }


            }

            return status > 0 ? true : false;

            //using (DbConnection conn = dbSmartAspects.CreateConnection())
            //{
            //    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpTaskAssignment_SP"))
            //    {
            //        foreach (HotelEmpTaskAssignmentBO bo in taskList)
            //        {
            //            command.Parameters.Clear();

            //            dbSmartAspects.AddInParameter(command, "@TaskId", DbType.Int64, bo.TaskId);
            //            //dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, bo.EmpId);
            //            //dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, bo.RoomId);
            //            // dbSmartAspects.AddInParameter(command, "@HKRoomStatusId", DbType.Int64, bo.HKRoomStatusId);
            //            // dbSmartAspects.AddInParameter(command, "@TaskDetails", DbType.String, bo.TaskDetails);
            //            //dbSmartAspects.AddInParameter(command, "@TaskStatus", DbType.String, bo.TaskStatus);
            //            dbSmartAspects.AddInParameter(command, "@Shift", DbType.String, bo.Shift);

            //            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
            //        }
            //    }
            //}
        }
        public Boolean UpdateEmpTaskFromFeedback(List<TaskAssignmentRoomWiseBO> taskList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpTaskFromFeedback_SP"))
                {
                    foreach (TaskAssignmentRoomWiseBO bo in taskList)
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@TaskId", DbType.Int64, bo.TaskId);
                        dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, bo.RoomId);
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int32, bo.EmpId);
                        dbSmartAspects.AddInParameter(command, "@HKRoomStatusId", DbType.Int64, bo.HKRoomStatusId);
                        dbSmartAspects.AddInParameter(command, "@HKRoomStatusName", DbType.String, bo.HKStatusName);
                        dbSmartAspects.AddInParameter(command, "@Feedbacks", DbType.String, bo.Feedbacks);
                        dbSmartAspects.AddInParameter(command, "@InTime", DbType.Time, bo.InTime);
                        dbSmartAspects.AddInParameter(command, "@OutTime", DbType.Time, bo.OutTime);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }
        public Boolean DeleteEmpTaskAssignment(List<HotelEmpTaskAssignmentBO> taskList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteEmpTaskAssignment_SP"))
                {
                    foreach (HotelEmpTaskAssignmentBO bo in taskList)
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@TaskId", DbType.Int64, bo.TaskId);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }

        public List<HotelEmpTaskAssignmentBO> GetTaskAssignment(string shift)
        {
            List<HotelEmpTaskAssignmentBO> taskList = new List<HotelEmpTaskAssignmentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTaskAssignment_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Shift", DbType.String, shift);

                    DataSet SaleServiceDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "AssignedTaskLists");
                    DataTable Table = SaleServiceDS.Tables["AssignedTaskLists"];

                    taskList = Table.AsEnumerable().Select(r => new HotelEmpTaskAssignmentBO
                    {
                        TaskId = r.Field<long>("TaskId"),
                        TaskSequence = r.Field<int>("TaskSequence"),
                        Shift = r.Field<string>("Shift"),
                        AssignDate = r.Field<DateTime>("AssignDate")

                    }).ToList();
                }
            }
            return taskList;
        }

        public List<HotelEmpTaskAssignmentBO> GetTaskAssignment(string shift, DateTime taskAssignDate)
        {
            List<HotelEmpTaskAssignmentBO> taskList = new List<HotelEmpTaskAssignmentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTaskAssignmentByAssignShiftNDate_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Shift", DbType.String, shift);
                    dbSmartAspects.AddInParameter(cmd, "@AssignDate", DbType.DateTime, taskAssignDate);

                    DataSet SaleServiceDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "AssignedTaskLists");
                    DataTable Table = SaleServiceDS.Tables["AssignedTaskLists"];

                    taskList = Table.AsEnumerable().Select(r => new HotelEmpTaskAssignmentBO
                    {
                        TaskId = r.Field<long>("TaskId"),
                        TaskSequence = r.Field<int>("TaskSequence"),
                        Shift = r.Field<string>("Shift"),
                        AssignDate = r.Field<DateTime>("AssignDate")

                    }).ToList();
                }
            }
            return taskList;
        }

        public HotelEmpTaskAssignmentBO GetTaskAssignmentById(Int64 taskId)
        {
            HotelEmpTaskAssignmentBO taskList = new HotelEmpTaskAssignmentBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTaskAssignmentById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TaskId", DbType.Int64, taskId);

                    DataSet SaleServiceDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "AssignedTaskLists");
                    DataTable Table = SaleServiceDS.Tables["AssignedTaskLists"];

                    taskList = Table.AsEnumerable().Select(r => new HotelEmpTaskAssignmentBO
                    {
                        TaskId = r.Field<long>("TaskId"),
                        TaskSequence = r.Field<int>("TaskSequence"),
                        Shift = r.Field<string>("Shift"),
                        AssignDate = r.Field<DateTime>("AssignDate"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        FloorId = r.Field<int?>("FloorId")

                    }).FirstOrDefault();
                }
            }
            return taskList;
        }

        public List<TaskAssignmentRoomWiseBO> GetAssignedTaskLists(Int64 taskId)
        {
            List<TaskAssignmentRoomWiseBO> taskList = new List<TaskAssignmentRoomWiseBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpAssignedTasks_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TaskId", DbType.Int64, taskId);

                    DataSet SaleServiceDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "AssignedTaskLists");
                    DataTable Table = SaleServiceDS.Tables["AssignedTaskLists"];

                    taskList = Table.AsEnumerable().Select(r => new TaskAssignmentRoomWiseBO
                    {
                        TaskId = r.Field<long>("TaskId"),
                        RoomTaskId = r.Field<long>("RoomTaskId"),
                        RoomId = r.Field<int>("RoomId"),
                        TaskDetails = r.Field<string>("TaskDetails"),
                        TaskStatus = r.Field<string>("TaskStatus"),
                        HKRoomStatusId = r.Field<long>("HKRoomStatusId"),
                        Feedbacks = r.Field<string>("Feedbacks"),
                        InTime = r.Field<DateTime?>("InTime"),
                        OutTime = r.Field<DateTime?>("OutTime"),
                        //InTimeString = (r.Field<DateTime?>("InTime") != null ? Convert.ToString(r.Field<DateTime?>("InTime")) : string.Empty),
                        //OutTimeString = (r.Field<DateTime?>("OutTime") != null ? Convert.ToString(r.Field<DateTime?>("OutTime")) : string.Empty),
                        HKStatusName = r.Field<string>("HKStatusName"),
                        RoomNumber = r.Field<string>("RoomNumber")
                    }).ToList();
                }
            }
            return taskList;
        }
        public List<TaskWiseEmployeeVWBO> GetEmployeeByDepartmentForTaskAssignment(long taskId, int departmentId)
        {
            List<TaskWiseEmployeeVWBO> boList = new List<TaskWiseEmployeeVWBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeByDepartmentForTaskAssignment_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);

                    if (taskId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@TaskId", DbType.Int64, taskId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TaskId", DbType.Int64, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                TaskWiseEmployeeVWBO bo = new TaskWiseEmployeeVWBO();

                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmpCode = reader["EmpCode"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.FullName = bo.FirstName + " " + bo.LastName;
                                bo.DisplayName = reader["DisplayName"].ToString();
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                                bo.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bo.Department = reader["Department"].ToString();
                                bo.EmpTypeId = Convert.ToInt32(reader["EmpTypeId"]);
                                bo.EmpType = reader["EmpType"].ToString();
                                bo.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                                bo.Designation = reader["Designation"].ToString();

                                bo.EmpTaskId = Convert.ToInt32(reader["EmpTaskId"]);
                                bo.TaskId = Convert.ToInt32(reader["TaskId"]);

                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }

        public List<TaskWiseEmployeeVWBO> GetEmployeeByTaskId(long taskId)
        {
            List<TaskWiseEmployeeVWBO> boList = new List<TaskWiseEmployeeVWBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGetEmployeeByTaskId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TaskId", DbType.Int64, taskId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                TaskWiseEmployeeVWBO bo = new TaskWiseEmployeeVWBO();

                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmpCode = reader["EmpCode"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.FullName = bo.FirstName + " " + bo.LastName;
                                bo.DisplayName = reader["DisplayName"].ToString();
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                                bo.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bo.Department = reader["Department"].ToString();
                                bo.EmpTypeId = Convert.ToInt32(reader["EmpTypeId"]);
                                bo.EmpType = reader["EmpType"].ToString();
                                bo.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                                bo.Designation = reader["Designation"].ToString();

                                bo.EmpTaskId = Convert.ToInt32(reader["EmpTaskId"]);
                                bo.TaskId = Convert.ToInt32(reader["TaskId"]);

                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }

        public List<TaskAssignmentRoomWiseBO> GetAssignedTaskDetailsById(Int64 taskId)
        {
            List<TaskAssignmentRoomWiseBO> taskList = new List<TaskAssignmentRoomWiseBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAssignedTaskDetailsById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TaskId", DbType.Int64, taskId);

                    DataSet SaleServiceDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "AssignedTaskLists");
                    DataTable Table = SaleServiceDS.Tables["AssignedTaskLists"];

                    taskList = Table.AsEnumerable().Select(r => new TaskAssignmentRoomWiseBO
                    {
                        TaskId = r.Field<long>("TaskId"),
                        RoomTaskId = r.Field<long>("RoomTaskId"),
                        RoomId = r.Field<int>("RoomId"),
                        EmpId = r.Field<int?>("EmpId"),
                        TaskDetails = r.Field<string>("TaskDetails"),
                        TaskStatus = r.Field<string>("TaskStatus"),
                        HKRoomStatusId = r.Field<long>("HKRoomStatusId"),
                        Feedbacks = r.Field<string>("Feedbacks"),
                        InTime = r.Field<DateTime?>("InTime"),
                        OutTime = r.Field<DateTime?>("OutTime"),
                        //InTimeString = (r.Field<DateTime?>("InTime") != null ? Convert.ToString(r.Field<DateTime?>("InTime")) : string.Empty),
                        //OutTimeString = (r.Field<DateTime?>("OutTime") != null ? Convert.ToString(r.Field<DateTime?>("OutTime")) : string.Empty),
                        HKStatusName = r.Field<string>("HKStatusName"),
                        FORoomStatus = r.Field<string>("FORoomStatus"),
                        RoomNumber = r.Field<string>("RoomNumber"),
                        RoomType = r.Field<string>("RoomType")

                    }).ToList();
                }
            }
            return taskList;
        }

        //Room Condition 
        public Boolean SaveRoomCondition(int roomId, int roomConditionId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDailyRoomCondition_SP"))
                {
                    command.Parameters.Clear();

                    dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, roomId);
                    dbSmartAspects.AddInParameter(command, "@RoomConditionId", DbType.Int32, roomConditionId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public Boolean DeleteRoomCondition(int roomId, int roomConditionId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteDailyRoomCondition_SP"))
                {
                    command.Parameters.Clear();

                    dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, roomId);
                    dbSmartAspects.AddInParameter(command, "@RoomConditionId", DbType.Int32, roomConditionId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }  
            }
            return status;
        }
        public Boolean DeleteUncheckedRoom(int roomId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteUnCheckedRoom_SP"))
                {
                    command.Parameters.Clear();

                    dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, roomId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }

            }
            return status;
        }
        public Boolean UpdateRoomCondition(List<HotelDailyRoomConditionBO> conditionList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDailyRoomCondition_SP"))
                {
                    foreach (HotelDailyRoomConditionBO bo in conditionList)
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@DailyRoomConditionId", DbType.Int64, bo.DailyRoomConditionId);
                        dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, bo.RoomId);
                        dbSmartAspects.AddInParameter(command, "@RoomConditionId", DbType.Int64, bo.RoomConditionId);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }
        public List<HotelDailyRoomConditionBO> GetDailyRoomCondition()
        {
            List<HotelDailyRoomConditionBO> conditionList = new List<HotelDailyRoomConditionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDailyRoomCondition_SP"))
                {
                    DataSet SaleServiceDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "RoomCondition");
                    DataTable Table = SaleServiceDS.Tables["RoomCondition"];

                    conditionList = Table.AsEnumerable().Select(r => new HotelDailyRoomConditionBO
                    {
                        DailyRoomConditionId = r.Field<long>("DailyRoomConditionId"),
                        RoomConditionId = r.Field<long>("RoomConditionId"),
                        RoomId = r.Field<int>("RoomId"),
                        Condition = r.Field<string>("Condition")

                    }).ToList();
                }
            }
            return conditionList;
        }

        //Room Discrepancy 
        public Boolean SaveRoomDiscrepancy(List<HotelRoomDiscrepancyBO> dicreList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRoomDiscrepancy_SP"))
                {
                    foreach (HotelRoomDiscrepancyBO bo in dicreList)
                    {
                        command.Parameters.Clear();

                        dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, bo.RoomId);
                        dbSmartAspects.AddInParameter(command, "@TaskId", DbType.Int64, bo.TaskId);
                        dbSmartAspects.AddInParameter(command, "@HKRoomStatusId", DbType.Int64, bo.HKRoomStatusId);
                        dbSmartAspects.AddInParameter(command, "@FOPersons", DbType.Int32, bo.FOPersons);
                        dbSmartAspects.AddInParameter(command, "@HKPersons", DbType.Int32, bo.HKPersons);
                        dbSmartAspects.AddInParameter(command, "@DiscrepanciesDetails", DbType.String, bo.DiscrepanciesDetails);
                        dbSmartAspects.AddInParameter(command, "@Reason", DbType.String, bo.Reason);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }
        public Boolean UpdateRoomDiscrepancy(List<HotelRoomDiscrepancyBO> discreList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRoomDiscrepancy_SP"))
                {
                    foreach (HotelRoomDiscrepancyBO bo in discreList)
                    {
                        if (bo.RoomDiscrepancyId > 0)
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@RoomDiscrepancyId", DbType.Int64, bo.RoomDiscrepancyId);
                            dbSmartAspects.AddInParameter(command, "@RoomId", DbType.Int32, bo.RoomId);
                            dbSmartAspects.AddInParameter(command, "@TaskId", DbType.Int64, bo.TaskId);
                            dbSmartAspects.AddInParameter(command, "@HKRoomStatusId", DbType.Int64, bo.HKRoomStatusId);
                            dbSmartAspects.AddInParameter(command, "@FOPersons", DbType.Int32, bo.FOPersons);
                            dbSmartAspects.AddInParameter(command, "@HKPersons", DbType.Int32, bo.HKPersons);
                            dbSmartAspects.AddInParameter(command, "@DiscrepanciesDetails", DbType.String, bo.DiscrepanciesDetails);
                            dbSmartAspects.AddInParameter(command, "@Reason", DbType.String, bo.Reason);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                    }
                }
            }
            return status;
        }
        public List<HotelRoomDiscrepancyBO> GetRoomDiscrepancy()
        {
            List<HotelRoomDiscrepancyBO> discreList = new List<HotelRoomDiscrepancyBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomDiscrepancies_SP"))
                {
                    DataSet SaleServiceDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, SaleServiceDS, "RoomDiscrepancy");
                    DataTable Table = SaleServiceDS.Tables["RoomDiscrepancy"];

                    discreList = Table.AsEnumerable().Select(r => new HotelRoomDiscrepancyBO
                    {
                        RoomDiscrepancyId = r.Field<long>("RoomDiscrepancyId"),
                        RoomId = r.Field<int>("RoomId"),
                        TaskId = r.Field<long>("TaskId"),
                        HKRoomStatusId = r.Field<long>("HKRoomStatusId"),
                        FOPersons = r.Field<int>("FOPersons"),
                        HKPersons = r.Field<int>("HKPersons"),
                        DiscrepanciesDetails = r.Field<string>("DiscrepanciesDetails"),
                        HKStatusName = r.Field<string>("HKStatusName"),
                        Reason = r.Field<string>("Reason")

                    }).ToList();
                }
            }
            return discreList;
        }


        //Reports
        public List<HKRoomStatusViewBO> GetHKRoomConditionForReport(int roomtypeId, long hkStatusId)
        {
            List<HKRoomStatusViewBO> roomInfos = new List<HKRoomStatusViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHKRoomConditionForReport_SP"))
                {
                    if (roomtypeId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RoomTypeId", DbType.Int32, roomtypeId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@RoomTypeId", DbType.Int32, DBNull.Value);
                    }
                    if (hkStatusId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@HKRoomStatusId", DbType.Int64, hkStatusId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@HKRoomStatusId", DbType.Int64, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "HKRoomCondition");
                    DataTable Table = ds.Tables["HKRoomCondition"];
                    roomInfos = Table.AsEnumerable().Select(r =>
                                new HKRoomStatusViewBO
                                {
                                    RoomId = r.Field<int>("RoomId"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    RoomType = r.Field<string>("RoomType"),
                                    FORoomStatus = r.Field<string>("FORoomStatus"),
                                    HKRoomStatus = r.Field<string>("HKRoomStatus"),
                                    Features = r.Field<string>("Features"),
                                    Conditions = r.Field<string>("Conditions")
                                }).ToList();
                }
            }
            return roomInfos;
        }
        public List<HKRoomStatusViewBO> GetHKRoomDiscrepancyForReport(DateTime? searchDate, string discrepancy)
        {
            List<HKRoomStatusViewBO> roomInfos = new List<HKRoomStatusViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHKRoomDiscrepancyForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@AssignDate", DbType.DateTime, searchDate);
                    if (discrepancy != "0")
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Discrepancies", DbType.String, discrepancy);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@Discrepancies", DbType.String, DBNull.Value);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "HKRoomDiscrepancy");
                    DataTable Table = ds.Tables["HKRoomDiscrepancy"];
                    roomInfos = Table.AsEnumerable().Select(r =>
                                new HKRoomStatusViewBO
                                {
                                    RoomId = r.Field<int>("RoomId"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    RoomType = r.Field<string>("RoomType"),
                                    FORoomStatus = r.Field<string>("FORoomStatus"),
                                    HKRoomStatus = r.Field<string>("HKRoomStatus"),
                                    FOPersons = r.Field<int>("FOPersons"),
                                    HKPersons = r.Field<int>("HKPersons"),
                                    DiscrepanciesDetails = r.Field<string>("DiscrepanciesDetails"),
                                }).ToList();
                }
            }
            return roomInfos;
        }
        public List<HKRoomStatusViewBO> GetHKTaskAssignForReport(Int64 taskId, DateTime searchDate, string shift)
        {
            List<HKRoomStatusViewBO> roomInfos = new List<HKRoomStatusViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHKTaskAssignForReport_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@TaskId", DbType.Int64, taskId);
                    dbSmartAspects.AddInParameter(cmd, "@AssignDate", DbType.DateTime, searchDate);
                    dbSmartAspects.AddInParameter(cmd, "@Shift", DbType.String, shift);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "HKTaskAssign");
                    DataTable Table = ds.Tables["HKTaskAssign"];

                    roomInfos = Table.AsEnumerable().Select(r =>
                                new HKRoomStatusViewBO
                                {
                                    RoomId = r.Field<int>("RoomId"),
                                    RoomNumber = r.Field<string>("RoomNumber"),
                                    ArriveDateString = r.Field<string>("ArriveDateString"),
                                    ExpectedCheckOutDateString = r.Field<string>("ExpectedCheckOutDateString"),
                                    RoomType = r.Field<string>("RoomType"),
                                    TaskDetails = r.Field<string>("TaskDetails"),
                                    Feedbacks = r.Field<string>("Feedbacks"),
                                    OldHKRoomStatus = r.Field<string>("OldHKRoomStatus"),
                                    HKRoomStatus = r.Field<string>("HKRoomStatus"),
                                    FORoomStatus = r.Field<string>("FORoomStatus"),
                                    ShowFeedbackTime = r.Field<string>("ShowFeedbackTime"),
                                    GuestName = r.Field<string>("GuestName"),
                                    DisplayName = r.Field<string>("DisplayName"),
                                    AssignedEmployees = r.Field<string>("AssignedEmployees")

                                }).ToList();
                }
            }
            return roomInfos;
        }
        public List<HKRoomStatusViewBO> GetTotalHKRoomStatistics()
        {
            List<HKRoomStatusViewBO> roomInfos = new List<HKRoomStatusViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTotalHKRoomStatistics_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "HKRoomStatistics");
                    DataTable Table = ds.Tables["HKRoomStatistics"];
                    roomInfos = Table.AsEnumerable().Select(r =>
                                new HKRoomStatusViewBO
                                {
                                    TotalRoomNo = r.Field<int>("TotalRoomNo"),
                                    HKRoomStatus = r.Field<string>("HKRoomStatus")
                                }).ToList();
                }
            }
            return roomInfos;
        }
    }
}
