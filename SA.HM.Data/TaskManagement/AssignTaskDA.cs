using HotelManagement.Entity.Payroll;
using HotelManagement.Entity.SalesAndMarketing;
using HotelManagement.Entity.TaskManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.TaskManagment
{
    public class AssignTaskDA : BaseService
    {
        public bool SaveOrUpdateTask(SMTask task, string empId, out long id)
        {
            Boolean status = false;
            bool retVal = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateTask_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, task.Id);
                            dbSmartAspects.AddInParameter(command, "@TaskName", DbType.String, task.TaskName);
                            dbSmartAspects.AddInParameter(command, "@TaskStage", DbType.Int32, task.TaskStage);
                            dbSmartAspects.AddInParameter(command, "@SourceNameId", DbType.Int32, task.SourceNameId);
                            if (task.TaskDate != null)
                                dbSmartAspects.AddInParameter(command, "@TaskDate", DbType.Date, task.TaskDate);
                            else
                                dbSmartAspects.AddInParameter(command, "@TaskDate", DbType.Date, DBNull.Value);
                            dbSmartAspects.AddInParameter(command, "@EstimatedDoneDate", DbType.Date, task.EstimatedDoneDate);
                            //if (task.EstimatedDoneHour != 0)
                            //    dbSmartAspects.AddInParameter(command, "@EstimatedDoneHour", DbType.Decimal, task.EstimatedDoneHour);
                            //else
                            //    dbSmartAspects.AddInParameter(command, "@EstimatedDoneHour", DbType.Decimal, DBNull.Value);
                            //dbSmartAspects.AddInParameter(command, "@StartTime", DbType.Time, task.StartTime);
                            //dbSmartAspects.AddInParameter(command, "@EndTime", DbType.Time, task.EndTime);
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, task.Description);
                            dbSmartAspects.AddInParameter(command, "@TaskType", DbType.String, task.TaskType);
                            //if (task.AssignToId != 0)
                            //    dbSmartAspects.AddInParameter(command, "@AssignToId", DbType.Int32, task.AssignToId);
                            //else
                            //    dbSmartAspects.AddInParameter(command, "@AssignToId", DbType.Int32, DBNull.Value);
                            dbSmartAspects.AddInParameter(command, "@EmailReminderType", DbType.String, task.EmailReminderType);
                            if (task.EmailReminderDate != null)
                                dbSmartAspects.AddInParameter(command, "@EmailReminderDate", DbType.DateTime, task.EmailReminderDate);
                            else
                                dbSmartAspects.AddInParameter(command, "@EmailReminderDate", DbType.DateTime, DBNull.Value);
                            if (task.EmailReminderTime != null)
                                dbSmartAspects.AddInParameter(command, "@EmailReminderTime", DbType.DateTime, task.EmailReminderTime);
                            else
                                dbSmartAspects.AddInParameter(command, "@EmailReminderTime", DbType.DateTime, DBNull.Value);
                            dbSmartAspects.AddInParameter(command, "@CallToAction", DbType.String, task.CallToAction);
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, task.CreatedBy);
                            dbSmartAspects.AddInParameter(command, "@ParentTaskId", DbType.Int32, task.ParentTaskId == null ? 0 : task.ParentTaskId);
                            dbSmartAspects.AddInParameter(command, "@DependentTaskId", DbType.Int32, task.DependentTaskId == null ? 0 : task.DependentTaskId);

                            dbSmartAspects.AddInParameter(command, "@AssignType", DbType.String, task.AssignType);
                            if (task.EmpDepartment != 0)
                                dbSmartAspects.AddInParameter(command, "@EmpDepartment", DbType.Int32, task.EmpDepartment);
                            else
                                dbSmartAspects.AddInParameter(command, "@EmpDepartment", DbType.Int32, DBNull.Value);

                            if (task.AccountManagerId != 0)
                                dbSmartAspects.AddInParameter(command, "@AccountManagerId", DbType.Int32, task.AccountManagerId);
                            else
                                dbSmartAspects.AddInParameter(command, "@AccountManagerId", DbType.Int32, DBNull.Value);

                            if (task.TaskPriority != 0)
                                dbSmartAspects.AddInParameter(command, "@TaskPriority", DbType.Int32, task.TaskPriority);
                            else
                                dbSmartAspects.AddInParameter(command, "@TaskPriority", DbType.Int32, DBNull.Value);

                            if (task.CompanyId != 0)
                                dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, task.CompanyId);
                            else
                                dbSmartAspects.AddInParameter(command, "@CompanyId", DbType.Int32, DBNull.Value);

                            if (task.ContactId != 0)
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, task.ContactId);
                            else
                                dbSmartAspects.AddInParameter(command, "@ContactId", DbType.Int32, DBNull.Value);

                            if (task.DealId != 0)
                                dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int32, task.DealId);
                            else
                                dbSmartAspects.AddInParameter(command, "@DealId", DbType.Int32, DBNull.Value);

                            if (task.ReminderDateFrom != null)
                                dbSmartAspects.AddInParameter(command, "@ReminderDateFrom", DbType.DateTime, task.ReminderDateFrom);
                            else
                                dbSmartAspects.AddInParameter(command, "@ReminderDateFrom", DbType.DateTime, DBNull.Value);

                            if (task.ReminderDateTo != null)
                                dbSmartAspects.AddInParameter(command, "@ReminderDateTo", DbType.DateTime, task.ReminderDateTo);
                            else
                                dbSmartAspects.AddInParameter(command, "@ReminderDateTo", DbType.DateTime, DBNull.Value);

                            if (!String.IsNullOrEmpty(task.TaskFor))
                                dbSmartAspects.AddInParameter(command, "@TaskFor", DbType.String, task.TaskFor);
                            else
                                dbSmartAspects.AddInParameter(command, "@TaskFor", DbType.String, DBNull.Value);

                            dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                            status = (dbSmartAspects.ExecuteNonQuery(command, transction) > 0);

                            id = Convert.ToInt64(command.Parameters["@OutId"].Value);
                        }
                        if (status && empId != "")
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveUpdateTaskAssignedEmployee_SP"))
                            {
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@TaskId", DbType.Int64, id);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@EmployeeList", DbType.String, empId);

                                status = (dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction) > 0);
                            }
                        }
                        if (status)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                            status = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public bool SaveOrUpdateTaskParticipantClient(long taskId, string ClientLIst, string ParticipantType)
        {
            Boolean status = false;
            bool retVal = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveUpdateTaskParticipantClient_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@TaskId", DbType.Int64, taskId);
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@ParticipantType", DbType.String, ParticipantType);
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@ClientList", DbType.String, ClientLIst);

                            status = (dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction) > 0);
                        }
                        if (status)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                            status = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public List<SMTask> GetAllTaskByProjectIdAndType(int Id, string Type)
        {
            List<SMTask> taskList = new List<SMTask>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllTaskByProjectIdAndType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, Id);
                    dbSmartAspects.AddInParameter(cmd, "@Type", DbType.String, Type);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMTask task = new SMTask();
                                task.Id = Convert.ToInt64(reader["Id"]);
                                task.TaskName = reader["TaskName"].ToString();
                                task.TaskType = reader["TaskType"].ToString();
                                task.TaskStage = Convert.ToInt32(reader["TaskStageId"]);
                                task.TaskDate = Convert.ToDateTime(reader["TaskDate"]);// + " " + reader["DueTime"]);
                                task.EstimatedDoneDate = Convert.ToDateTime(reader["DoneDate"]);// + " " + reader["DueTime"]);
                                task.SourceName = reader["SourceName"].ToString();
                                task.ProjectComplete = Convert.ToDecimal(reader["projectComplete"]);
                                if (reader["Complete"] == DBNull.Value)
                                {
                                    task.Complete = Convert.ToDecimal(0.0);
                                }
                                else
                                {
                                    task.Complete = Convert.ToDecimal(reader["Complete"]);
                                }
                                //task.Complete = Convert.ToDecimal(reader["Complete"]);
                                task.EmployeeName = reader["EmployeeName"].ToString();
                                task.ParentTaskId = Convert.ToInt32(reader["ParentTaskId"]);
                                task.DependentTaskId = Convert.ToInt32(reader["DependentTaskId"]);
                                task.HasChild = Convert.ToInt32(reader["HasChild"]);

                                taskList.Add(task);
                            }
                        }
                    }
                }
            }
            return taskList;
        }
        public List<SMTask> GetAllTaskByEmployeeId(int EmployeeId)
        {
            List<SMTask> taskList = new List<SMTask>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllTaskByEmployeeId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, EmployeeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMTask task = new SMTask();
                                task.Id = Convert.ToInt64(reader["Id"]);
                                task.TaskName = reader["TaskName"].ToString();
                                task.TaskType = reader["TaskType"].ToString();
                                task.TaskStage = Convert.ToInt32(reader["TaskStage"]);
                                task.TaskDate = Convert.ToDateTime(reader["TaskDate"]);// + " " + reader["DueTime"]);
                                task.EstimatedDoneDate = Convert.ToDateTime(reader["EstimatedDoneDate"]);// + " " + reader["DueTime"]);
                                task.SourceName = reader["SourceName"].ToString();
                                task.ProjectComplete = Convert.ToDecimal(reader["projectComplete"]);
                                task.Complete = Convert.ToDecimal(reader["Complete"]);
                                task.ParentTaskId = Convert.ToInt32(reader["ParentTaskId"]);
                                task.DependentTaskId = Convert.ToInt32(reader["DependentTaskId"]);
                                task.HasChild = Convert.ToInt32(reader["HasChild"]);

                                taskList.Add(task);
                            }
                        }
                    }
                }
            }
            return taskList;
        }
        public SMTask GetTaskById(long id)
        {
            SMTask task = new SMTask();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            //string query = string.Format("SELECT * FROM SMTask WHERE Id = {0}", id);

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllTaskById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int32, id);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                task.Id = Convert.ToInt64(reader["Id"]);
                                if (reader["TaskStage"] != DBNull.Value)
                                    task.TaskStage = Convert.ToInt32(reader["TaskStage"]);
                                if (reader["SourceNameId"] != DBNull.Value)
                                {
                                    task.SourceNameId = Convert.ToInt64(reader["SourceNameId"]);
                                }
                                if (reader["TaskType"] != DBNull.Value)
                                    task.TaskType = (reader["TaskType"].ToString());
                                if (reader["TaskName"] != DBNull.Value)
                                    task.TaskName = reader["TaskName"].ToString();
                                if (reader["TaskDate"] != DBNull.Value)
                                    task.TaskDate = Convert.ToDateTime(reader["TaskDate"]);
                                if (reader["EstimatedDoneDate"] != DBNull.Value)
                                    task.EstimatedDoneDate = Convert.ToDateTime(reader["EstimatedDoneDate"]);
                                if (reader["ReminderDateFrom"] != DBNull.Value)
                                    task.ReminderDateFrom = Convert.ToDateTime(reader["ReminderDateFrom"]);
                                if (reader["ReminderDateTo"] != DBNull.Value)
                                    task.ReminderDateTo = Convert.ToDateTime(reader["ReminderDateTo"]);
                                //if (reader["EstimatedDoneHour"] != DBNull.Value)
                                //{
                                //    task.EstimatedDoneHour = Convert.ToDecimal(reader["EstimatedDoneHour"]);
                                //}
                                //task.StartTime = Convert.ToDateTime(currentDate + " " + reader["StartTime"]);
                                //task.EndTime = Convert.ToDateTime(currentDate + " " + reader["EndTime"]);
                                //task.ho = Convert.ToInt32(reader["TaskType"]);
                                if (reader["Description"] != DBNull.Value)
                                {
                                    task.Description = reader["Description"].ToString();
                                }
                                if (reader["EmailReminderType"] != DBNull.Value)
                                    task.EmailReminderType = reader["EmailReminderType"].ToString();
                                if (reader["EmailReminderDate"] != DBNull.Value)
                                    task.EmailReminderDate = Convert.ToDateTime(reader["EmailReminderDate"]);
                                if (reader["EmailReminderTime"] != DBNull.Value)
                                    task.EmailReminderTime = Convert.ToDateTime(currentDate + " " + reader["EmailReminderTime"]);
                                if (reader["CallToAction"] != DBNull.Value)
                                {
                                    task.CallToAction = reader["CallToAction"].ToString();
                                }
                                if (reader["ParentTaskId"] != DBNull.Value)
                                    task.ParentTaskId = Convert.ToInt32(reader["ParentTaskId"]);
                                if (reader["DependentTaskId"] != DBNull.Value)
                                    task.DependentTaskId = Convert.ToInt32(reader["DependentTaskId"]);
                                if (reader["IsCompleted"] != DBNull.Value)
                                    task.IsCompleted = Convert.ToBoolean(reader["IsCompleted"]);
                                if (reader["CreatedBy"] != DBNull.Value)
                                    task.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);

                                if (reader["AssignType"] != DBNull.Value)
                                {
                                    task.AssignType = reader["AssignType"].ToString();
                                }
                                if (reader["EmpDepartment"] != DBNull.Value)
                                {
                                    task.EmpDepartment = Convert.ToInt32(reader["EmpDepartment"]);
                                }
                                if (reader["SourceName"] != DBNull.Value)
                                    task.SourceName = reader["SourceName"].ToString();

                                if (reader["CompanyId"] != DBNull.Value)
                                {
                                    task.CompanyId = Convert.ToInt32(reader["CompanyId"]);
                                    task.CompanyName = reader["CompanyName"].ToString();
                                }

                                if (reader["ContactId"] != DBNull.Value)
                                {
                                    task.ContactId = Convert.ToInt32(reader["ContactId"]);
                                    task.ContactName = reader["ContactName"].ToString();
                                }

                                if (reader["DealId"] != DBNull.Value)
                                {
                                    task.DealId = Convert.ToInt32(reader["DealId"]);
                                    task.DealName = reader["DealName"].ToString();
                                }

                                if (reader["SourceName"] != DBNull.Value)
                                    task.SourceName = reader["SourceName"].ToString();
                                if (reader["TaskFor"] != DBNull.Value)
                                    task.TaskFor = reader["TaskFor"].ToString();
                                if (reader["AssignType"] != DBNull.Value)
                                    task.AssignType = reader["AssignType"].ToString();
                                if (reader["TaskPriority"] != DBNull.Value)
                                    task.TaskPriority = Convert.ToInt32(reader["TaskPriority"]);
                                if (reader["AccountManagerId"] != DBNull.Value)
                                    task.AccountManagerId = Convert.ToInt32(reader["AccountManagerId"]);
                                task.TaskStatus = reader["TaskStatus"].ToString();
                            }
                        }
                    }
                    //DataSet dataSet = new DataSet();

                    //dbSmartAspects.LoadDataSet(cmd, dataSet, "Stage");
                    //DataTable Table = dataSet.Tables["Stage"];

                    //task = Table.AsEnumerable().Select(r => new SMTask
                    //{
                    //    Id = r.Field<long>("Id"),
                    //    TaskName = r.Field<string>("TaskName"),
                    //    DueDate = r.Field<DateTime>("DueDate"),
                    //    DueTime = r.Field<DateTime>("DueTime"),
                    //    TaskType = r.Field<string>("TaskType"),
                    //    Description = r.Field<string>("Description"),
                    //    AssignTo = r.Field<int>("AssignTo"),
                    //    EmailReminderDate = r.Field<DateTime?>("EmailReminderDate"),
                    //    EmailReminderTime = r.Field<DateTime?>("EmailReminderTime"),
                    //    CreatedBy = r.Field<int>("CreatedBy"),
                    //    IsCompleted = r.Field<bool>("IsCompleted")

                    //}).FirstOrDefault();
                }
            }
            return task;
        }
        public List<SMTask> GetTaskBySearchCriteria(string taskName, string assignedEmp, string fromDate, string toDate,
                                                        int empId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<SMTask> taskList = new List<SMTask>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAssignTaskForPaging_SP"))
                {
                    if (!string.IsNullOrEmpty(taskName))
                        dbSmartAspects.AddInParameter(cmd, "@TaskName", DbType.String, taskName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TaskName", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(fromDate))
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (!string.IsNullOrEmpty(toDate))
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    if (!string.IsNullOrEmpty(assignedEmp))
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeList", DbType.String, assignedEmp);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeList", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMTask task = new SMTask();
                                task.Id = Convert.ToInt64(reader["Id"]);
                                task.TaskName = reader["TaskName"].ToString();
                                task.TaskDate = Convert.ToDateTime(Convert.ToDateTime(reader["TaskDate"]).ToString("yyyy-MM-dd"));// + " " + reader["DueTime"]);
                                task.EstimatedDoneDate = Convert.ToDateTime(Convert.ToDateTime(reader["EstimatedDoneDate"]).ToString("yyyy-MM-dd"));// + " " + reader["DueTime"]);
                                task.TaskType = reader["TaskType"].ToString();
                                task.CompanyName = reader["CompanyName"].ToString();
                                task.ContactName = reader["ContactName"].ToString();
                                task.DealName = reader["DealName"].ToString();
                                task.EmployeeNameList = reader["EmployeeNameList"].ToString();
                                task.PerticipentFromClient = reader["PerticipentFromClient"].ToString();
                                task.AccountManagerName = reader["AccountManagerName"].ToString();
                                task.DueDateTime = reader["DueDateTime"].ToString();
                                task.TaskPriority = Convert.ToInt32(reader["TaskPriority"]);
                                if (reader["TaskFor"] != DBNull.Value)
                                    task.TaskFor = reader["TaskFor"].ToString();

                                task.TaskStatus = reader["TaskStatus"].ToString();
                                task.EmpId = Convert.ToInt32(reader["EmpId"]);
                                taskList.Add(task);
                            }
                        }
                    }
                    //            DataSet dataSet = new DataSet();

                    //dbSmartAspects.LoadDataSet(cmd, dataSet, "Task");
                    //DataTable Table = dataSet.Tables["Task"];

                    //taskList = Table.AsEnumerable().Select(r => new SMTask
                    //{
                    //    Id = r.Field<long>("Id"),
                    //    TaskName = r.Field<string>("TaskName"),
                    //    DueDate = r.Field<DateTime>("DueDate"),
                    //    DueTime = r.Field<DateTime>("DueTime"),
                    //    TaskType = r.Field<string>("TaskType"),
                    //    Description = r.Field<string>("Description"),
                    //    AssignTo = r.Field<int>("AssignTo"),
                    //    EmailReminderDate = r.Field<DateTime?>("EmailReminderDate"),
                    //    EmailReminderTime = r.Field<DateTime?>("EmailReminderTime"),
                    //    CreatedBy = r.Field<int>("CreatedBy"),
                    //    IsCompleted = r.Field<bool>("IsCompleted")

                    //}).ToList();
                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }


            return taskList;
        }
        public bool UpdateTaskIsCompleted(long id, int userId, bool isCompleted)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateTaskIsCompleted_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.String, id);
                        dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, userId);
                        dbSmartAspects.AddInParameter(command, "@IsCompleted", DbType.Boolean, isCompleted);

                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        public List<SMTaskAssignmentView> GetTaskForEmailReminder()
        {

            List<SMTaskAssignmentView> taskList = new List<SMTaskAssignmentView>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTaskForEmailReminder_SP"))
                {

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        DataSet dataSet = new DataSet();

                        dbSmartAspects.LoadDataSet(cmd, dataSet, "Task");
                        DataTable Table = dataSet.Tables["Task"];

                        taskList = Table.AsEnumerable().Select(r => new SMTaskAssignmentView
                        {
                            Id = r.Field<long>("Id"),
                            Description = r.Field<string>("Description"),
                            AssignToName = r.Field<string>("AssignToName"),
                            AssignToEmailAddress = r.Field<string>("AssignToEmailAddress"),
                            AssigneeName = r.Field<string>("AssigneeName")

                        }).ToList();

                    }
                }
            }
            return taskList;
        }
        public bool UpdateEmailSentInformation(long id)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmailSentInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.String, id);

                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }
        //public List<long> GetTaskAssignedEmployeeById(long id)
        //{
        //    List<long> EmpList = new List<long>();
        //    string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

        //    string query = string.Format("SELECT * FROM TMTaskAssignedEmployee WHERE TaskId = {0}", id);

        //    try
        //    {
        //        using (DbConnection conn = dbSmartAspects.CreateConnection())
        //        {
        //            using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
        //            {
        //                using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //                {
        //                    if (reader != null)
        //                    {
        //                        while (reader.Read())
        //                        {
        //                            TMTaskAssignedEmployee tMTaskAssignedEmployee = new TMTaskAssignedEmployee();

        //                            tMTaskAssignedEmployee.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);

        //                            EmpList.Add(tMTaskAssignedEmployee.EmployeeId);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return EmpList;
        //}
        public List<SMTask> GetTaskByProjectId(int projectId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<SMTask> taskList = new List<SMTask>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTaskByProjectId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMTask task = new SMTask();
                                task.Id = Convert.ToInt64(reader["Id"]);
                                task.TaskName = reader["TaskName"].ToString();
                                task.TaskDate = Convert.ToDateTime(reader["TaskDate"]);// + " " + reader["DueTime"]);
                                task.EstimatedDoneDate = Convert.ToDateTime(reader["EstimatedDoneDate"]);// + " " + reader["DueTime"]);
                                task.TaskType = reader["TaskType"].ToString();
                                taskList.Add(task);
                            }
                        }
                    }
                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }
            return taskList;
        }

        public List<SMTask> GetAllTaskByProjectId(int projectId)
        {
            List<SMTask> taskList = new List<SMTask>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllTaskByProjectId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.Int32, projectId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMTask task = new SMTask();
                                task.Id = Convert.ToInt64(reader["Id"]);
                                task.TaskName = reader["TaskName"].ToString();
                                task.TaskType = reader["TaskType"].ToString();
                                task.TaskStage = Convert.ToInt32(reader["TaskStageId"]);
                                task.TaskDate = Convert.ToDateTime(reader["TaskDate"]);// + " " + reader["DueTime"]);
                                task.EstimatedDoneDate = Convert.ToDateTime(reader["DoneDate"]);// + " " + reader["DueTime"]);
                                task.ProjectName = reader["ProjectName"].ToString();
                                task.ProjectComplete = Convert.ToDecimal(reader["projectComplete"]);
                                task.Complete = Convert.ToDecimal(reader["Complete"]);
                                task.EmployeeName = reader["EmployeeName"].ToString();
                                task.ParentTaskId = Convert.ToInt32(reader["ParentTaskId"]);
                                task.DependentTaskId = Convert.ToInt32(reader["DependentTaskId"]);
                                task.HasChild = Convert.ToInt32(reader["HasChild"]);

                                taskList.Add(task);
                            }
                        }
                    }
                }
            }
            return taskList;
        }

        public List<SMTask> GetAllTaskBySourceNameId(int sourceNameId)
        {
            List<SMTask> taskList = new List<SMTask>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllTaskBySourceNameId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SourceNameId", DbType.Int32, sourceNameId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMTask task = new SMTask();
                                task.Id = Convert.ToInt64(reader["Id"]);
                                task.TaskName = reader["TaskName"].ToString();
                                task.TaskType = reader["TaskType"].ToString();
                                //task.TaskStage = Convert.ToInt32(reader["TaskStageId"]);
                                //task.TaskDate = Convert.ToDateTime(reader["TaskDate"]);// + " " + reader["DueTime"]);
                                //task.EstimatedDoneDate = Convert.ToDateTime(reader["DoneDate"]);// + " " + reader["DueTime"]);
                                //task.ProjectName = reader["ProjectName"].ToString();
                                //task.ProjectComplete = Convert.ToDecimal(reader["projectComplete"]);
                                //task.Complete = Convert.ToDecimal(reader["Complete"]);
                                //task.EmployeeName = reader["EmployeeName"].ToString();
                                //task.ParentTaskId = Convert.ToInt32(reader["ParentTaskId"]);
                                //task.DependentTaskId = Convert.ToInt32(reader["DependentTaskId"]);
                                //task.HasChild = Convert.ToInt32(reader["HasChild"]);

                                taskList.Add(task);
                            }
                        }
                    }
                }
            }
            return taskList;
        }

        public Boolean DeleteTask(long pkId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteTask_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, pkId);

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

        public List<SMTask> GetAllTaskInfo()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<SMTask> taskList = new List<SMTask>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllTaskInfo"))
                {

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMTask task = new SMTask();
                                task.Id = Convert.ToInt64(reader["Id"]);
                                task.TaskName = reader["TaskName"].ToString();
                                task.TaskDate = Convert.ToDateTime(Convert.ToDateTime(reader["TaskDate"]).ToString("yyyy-MM-dd"));// + " " + reader["DueTime"]);
                                task.EstimatedDoneDate = Convert.ToDateTime(Convert.ToDateTime(reader["EstimatedDoneDate"]).ToString("yyyy-MM-dd"));// + " " + reader["DueTime"]);
                                task.TaskType = reader["TaskType"].ToString();
                                //task.StartTime = Convert.ToDateTime(currentDate + " " + reader["StartTime"]);
                                //task.EndTime = Convert.ToDateTime(currentDate + " " + reader["EndTime"]);
                                task.IsCompleted = Convert.ToBoolean(reader["IsCompleted"]);
                                task.EmployeeName = reader["EmployeeName"].ToString();
                                task.TaskStage = Convert.ToInt32(reader["TaskStage"]);
                                taskList.Add(task);
                            }
                        }
                    }

                }
            }
            return taskList;
        }
        public List<SMQuotationBO> GetQuotationInformationForTaskType()
        {
            List<SMQuotationBO> QuotationList = new List<SMQuotationBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetQuotationInformationForTaskType_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMQuotationBO quotation = new SMQuotationBO();
                                quotation.QuotationId = Convert.ToInt64(reader["QuotationId"]);
                                quotation.QuotationNo = reader["QuotationNo"].ToString();

                                QuotationList.Add(quotation);
                            }
                        }
                    }
                }
            }
            return QuotationList;
        }
        public bool UpdateStage(int stageId, int Id)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateTaskStage_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, Id);
                        dbSmartAspects.AddInParameter(command, "@StageId", DbType.Int32, stageId);

                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return status;
        }

        public List<SMTask> GetTaskInformationForReport(string taskName, string assignedEmp, DateTime fromDate, DateTime toDate)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<SMTask> taskList = new List<SMTask>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTaskInformationForReport_SP"))
                {
                    if (!string.IsNullOrEmpty(taskName))
                        dbSmartAspects.AddInParameter(cmd, "@TaskName", DbType.String, taskName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TaskName", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);

                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    if (!string.IsNullOrEmpty(assignedEmp))
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeList", DbType.String, assignedEmp);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeList", DbType.String, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMTask task = new SMTask();
                                task.Id = Convert.ToInt64(reader["Id"]);
                                task.TaskName = reader["TaskName"].ToString();
                                task.TaskDate = Convert.ToDateTime(Convert.ToDateTime(reader["TaskDate"]).ToString("yyyy-MM-dd"));// + " " + reader["DueTime"]);
                                task.EstimatedDoneDate = Convert.ToDateTime(Convert.ToDateTime(reader["EstimatedDoneDate"]).ToString("yyyy-MM-dd"));// + " " + reader["DueTime"]);
                                task.TaskType = reader["TaskType"].ToString();
                                //task.EstimatedDoneHour = Convert.ToDecimal(reader["EstimatedDoneHour"]);
                                //task.StartTime = Convert.ToDateTime(currentDate + " " + reader["StartTime"]);
                                //task.EndTime = Convert.ToDateTime(currentDate + " " + reader["EndTime"]);
                                task.IsCompleted = Convert.ToBoolean(reader["IsCompleted"]);
                                task.EmployeeName = reader["EmployeeName"].ToString();
                                taskList.Add(task);
                            }
                        }
                    }

                }
            }
            return taskList;
        }

        public bool SaveTaskFeedback(int stage, string feedback, int taskId, int empId, bool ImpStatus, DateTime date, DateTime time, out long id)
        {
            Boolean status = false;
            bool retVal = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();


                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateTaskStageForFeedback_SP"))
                    {

                        dbSmartAspects.AddInParameter(command, "@TaskStage", DbType.Int32, stage);
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, taskId);
                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));
                        status = (dbSmartAspects.ExecuteNonQuery(command) > 0);

                        id = Convert.ToInt64(command.Parameters["@OutId"].Value);
                    }
                    if (status && id != 0)
                    {
                        using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("UpdateTaskFeedbackForFeedback_SP"))
                        {
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@TaskId", DbType.Int64, id);
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@EmployeeId", DbType.Int64, empId);
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@TaskFeedback", DbType.String, feedback);
                            dbSmartAspects.AddInParameter(cmdOutDetails, "@ImpStatus", DbType.Boolean, ImpStatus);
                            if (date != null)
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@ImpDate", DbType.DateTime, date);
                            else
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@ImpDate", DbType.DateTime, DBNull.Value);
                            if (time != null)
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@ImpTime", DbType.DateTime, time);
                            else
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@ImpTime", DbType.DateTime, DBNull.Value);

                            status = (dbSmartAspects.ExecuteNonQuery(cmdOutDetails) > 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public List<EmployeeBO> GetTaskAssignedEmployeeById(long id)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<EmployeeBO> documentList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAssignedEmployeeByTaskId"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO Emp = new EmployeeBO();
                                Emp.EmpId = Convert.ToInt32(reader["EmpId"]);
                                Emp.DisplayName = reader["DisplayName"].ToString();
                                Emp.EmpCode = reader["EmpCode"].ToString();
                                Emp.Department = reader["Department"].ToString();
                                documentList.Add(Emp);
                            }
                        }
                    }
                }
            }

            return documentList;
        }

        public List<SMTask> GetTaskBySearchCriteria(string taskName, string assignedEmp, DateTime fromDate, DateTime toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<SMTask> taskList = new List<SMTask>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAssignTaskForPaging_SP"))
                {
                    if (!string.IsNullOrEmpty(taskName))
                        dbSmartAspects.AddInParameter(cmd, "@TaskName", DbType.String, taskName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TaskName", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);

                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);


                    if (!string.IsNullOrEmpty(assignedEmp))
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeList", DbType.String, assignedEmp);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@EmployeeList", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMTask task = new SMTask();
                                task.Id = Convert.ToInt64(reader["Id"]);
                                task.TaskName = reader["TaskName"].ToString();
                                task.TaskDate = Convert.ToDateTime(Convert.ToDateTime(reader["TaskDate"]).ToString("yyyy-MM-dd"));// + " " + reader["DueTime"]);
                                task.EstimatedDoneDate = Convert.ToDateTime(Convert.ToDateTime(reader["EstimatedDoneDate"]).ToString("yyyy-MM-dd"));// + " " + reader["DueTime"]);
                                task.TaskType = reader["TaskType"].ToString();
                                // task.AssignToName = reader["AssignToName"].ToString();
                                task.StartTime = Convert.ToDateTime(currentDate + " " + reader["StartTime"]);
                                task.EndTime = Convert.ToDateTime(currentDate + " " + reader["EndTime"]);
                                task.IsCompleted = Convert.ToBoolean(reader["IsCompleted"]);
                                taskList.Add(task);
                            }
                        }
                    }
                    //            DataSet dataSet = new DataSet();

                    //dbSmartAspects.LoadDataSet(cmd, dataSet, "Task");
                    //DataTable Table = dataSet.Tables["Task"];

                    //taskList = Table.AsEnumerable().Select(r => new SMTask
                    //{
                    //    Id = r.Field<long>("Id"),
                    //    TaskName = r.Field<string>("TaskName"),
                    //    DueDate = r.Field<DateTime>("DueDate"),
                    //    DueTime = r.Field<DateTime>("DueTime"),
                    //    TaskType = r.Field<string>("TaskType"),
                    //    Description = r.Field<string>("Description"),
                    //    AssignTo = r.Field<int>("AssignTo"),
                    //    EmailReminderDate = r.Field<DateTime?>("EmailReminderDate"),
                    //    EmailReminderTime = r.Field<DateTime?>("EmailReminderTime"),
                    //    CreatedBy = r.Field<int>("CreatedBy"),
                    //    IsCompleted = r.Field<bool>("IsCompleted")

                    //}).ToList();
                    totalRecords = Convert.ToInt32(cmd.Parameters["@RecordCount"].Value);
                }
            }


            return taskList;
        }
        public List<SMTask> GetTaskByEmployeeId(int empId)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<SMTask> taskList = new List<SMTask>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAssignTaskByEmployeeId_SP"))
                {

                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, empId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMTask task = new SMTask();
                                task.Id = Convert.ToInt64(reader["Id"]);
                                task.TaskName = reader["TaskName"].ToString();
                                task.TaskDate = Convert.ToDateTime(Convert.ToDateTime(reader["TaskDate"]).ToString("yyyy-MM-dd"));// + " " + reader["DueTime"]);
                                task.EstimatedDoneDate = Convert.ToDateTime(Convert.ToDateTime(reader["EstimatedDoneDate"]).ToString("yyyy-MM-dd"));// + " " + reader["DueTime"]);
                                task.TaskType = reader["TaskType"].ToString();
                                // task.AssignToName = reader["AssignToName"].ToString();
                                task.StartTime = Convert.ToDateTime(currentDate + " " + reader["StartTime"]);
                                task.EndTime = Convert.ToDateTime(currentDate + " " + reader["EndTime"]);
                                task.IsCompleted = Convert.ToBoolean(reader["IsCompleted"]);
                                task.ImplementStatus = Convert.ToBoolean(reader["ImplementStatus"]);
                                task.TaskFor = reader["TaskFor"].ToString();
                                task.TaskType = reader["TaskType"].ToString();
                                taskList.Add(task);
                            }
                        }
                    }

                }
            }


            return taskList;
        }

        public List<SMTask> GetAllTaskByTaskTypeNTaskId(string taskType, long taskId)
        {
            List<SMTask> taskList = new List<SMTask>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllTaskByTaskTypeNTaskId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TaskType", DbType.String, taskType);
                    dbSmartAspects.AddInParameter(cmd, "@TaskId", DbType.Int32, taskId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMTask task = new SMTask();
                                task.Id = Convert.ToInt64(reader["Id"]);
                                task.TaskName = reader["TaskName"].ToString();
                                task.TaskType = reader["TaskType"].ToString();
                                task.TaskStage = Convert.ToInt32(reader["TaskStageId"]);
                                task.TaskDate = Convert.ToDateTime(reader["TaskDate"]);// + " " + reader["DueTime"]);
                                task.EstimatedDoneDate = Convert.ToDateTime(reader["DoneDate"]);// + " " + reader["DueTime"]);
                                                                                                //task.ProjectName = reader["ProjectName"].ToString();
                                                                                                //task.ProjectComplete = Convert.ToDecimal(reader["projectComplete"]);
                                if (reader["Complete"] == DBNull.Value)
                                {
                                    task.Complete = Convert.ToDecimal(0.0);
                                }
                                else
                                {
                                    task.Complete = Convert.ToDecimal(reader["Complete"]);
                                }
                                //task.Complete = Convert.ToDecimal(reader["Complete"]);
                                task.EmployeeName = reader["EmployeeName"].ToString();
                                task.ParentTaskId = Convert.ToInt32(reader["ParentTaskId"]);
                                task.DependentTaskId = Convert.ToInt32(reader["DependentTaskId"]);
                                task.HasChild = Convert.ToInt32(reader["HasChild"]);

                                taskList.Add(task);
                            }
                        }
                    }
                }
            }
            return taskList;
        }

        public List<SMTask> GetReminderByEmployeeId(int empId)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<SMTask> taskList = new List<SMTask>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRemainderForCallToAction_SP"))
                    {

                        dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.Int32, empId);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMTask task = new SMTask();
                                    task.Id = Convert.ToInt64(reader["Id"]);
                                    task.TaskName = reader["TaskName"].ToString();
                                    task.TaskDate = Convert.ToDateTime(Convert.ToDateTime(reader["TaskDate"]).ToString("yyyy-MM-dd"));// + " " + reader["DueTime"]);
                                    task.PerticipentFromClient = reader["PerticipentFromClient"].ToString();
                                    task.StartTime = Convert.ToDateTime(currentDate + " " + reader["StartTime"]);
                                    taskList.Add(task);
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return taskList;
        }

        public List<long> GetTaskAssignedContactsById(long id)
        {
            List<long> ContactList = new List<long>();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            string query = string.Format("SELECT ContactId FROM TaskParticipant WHERE TaskId = {0}", id);

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    long ContactId = Convert.ToInt32(reader["ContactId"]);

                                    ContactList.Add(ContactId);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ContactList;
        }

        public Boolean SaveTaskFeedBackForCRM(SMTaskFeedbackBO feedbackBO, string participantFromCompany, string participantFromClient, out long OutId)
        {
            Boolean status = false;
            bool retVal = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateTaskFeedback_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, feedbackBO.Id);
                            if (feedbackBO.TaskId != 0)
                                dbSmartAspects.AddInParameter(command, "@TaskId", DbType.Int64, feedbackBO.TaskId);
                            else
                                dbSmartAspects.AddInParameter(command, "@TaskId", DbType.Int64, DBNull.Value);

                            if (feedbackBO.EmployeeId != 0)
                                dbSmartAspects.AddInParameter(command, "@EmployeeId", DbType.Int32, feedbackBO.EmployeeId);
                            else
                                dbSmartAspects.AddInParameter(command, "@EmployeeId", DbType.Int32, DBNull.Value);

                            if (!string.IsNullOrEmpty(feedbackBO.ImplementationStatus))
                                dbSmartAspects.AddInParameter(command, "@ImplementationStatus", DbType.String, feedbackBO.ImplementationStatus);
                            else
                                dbSmartAspects.AddInParameter(command, "@ImplementationStatus", DbType.String, DBNull.Value);

                            if (feedbackBO.TaskStage != 0)
                                dbSmartAspects.AddInParameter(command, "@TaskStage", DbType.Int32, feedbackBO.TaskStage);
                            else
                                dbSmartAspects.AddInParameter(command, "@TaskStage", DbType.Int32, feedbackBO.TaskStage);

                            if (feedbackBO.TaskFeedback != null)
                                dbSmartAspects.AddInParameter(command, "@TaskFeedback", DbType.String, feedbackBO.TaskFeedback);
                            else
                                dbSmartAspects.AddInParameter(command, "@TaskFeedback", DbType.String, DBNull.Value);


                            if (feedbackBO.StartDate != null)
                                dbSmartAspects.AddInParameter(command, "@StartDate", DbType.DateTime, feedbackBO.StartDate);
                            else
                                dbSmartAspects.AddInParameter(command, "@StartDate", DbType.DateTime, DBNull.Value);

                            if (feedbackBO.StartTime != null)
                                dbSmartAspects.AddInParameter(command, "@StartTime", DbType.DateTime, feedbackBO.StartTime);
                            else
                                dbSmartAspects.AddInParameter(command, "@StartTime", DbType.DateTime, DBNull.Value);

                            if (feedbackBO.FinishDate != null)
                                dbSmartAspects.AddInParameter(command, "@FinishDate", DbType.DateTime, feedbackBO.FinishDate);
                            else
                                dbSmartAspects.AddInParameter(command, "@FinishDate", DbType.DateTime, DBNull.Value);

                            if (feedbackBO.FinishTime != null)
                                dbSmartAspects.AddInParameter(command, "@FinishTime", DbType.DateTime, feedbackBO.FinishTime);
                            else
                                dbSmartAspects.AddInParameter(command, "@FinishTime", DbType.DateTime, DBNull.Value);

                            if (!string.IsNullOrEmpty(feedbackBO.MeetingAgenda))
                                dbSmartAspects.AddInParameter(command, "@MeetingAgenda", DbType.String, feedbackBO.MeetingAgenda);
                            else
                                dbSmartAspects.AddInParameter(command, "@MeetingAgenda", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(feedbackBO.MeetingLocation))
                                dbSmartAspects.AddInParameter(command, "@MeetingLocation", DbType.String, feedbackBO.MeetingLocation);
                            else
                                dbSmartAspects.AddInParameter(command, "@MeetingLocation", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(feedbackBO.MeetingDiscussion))
                                dbSmartAspects.AddInParameter(command, "@MeetingDiscussion", DbType.String, feedbackBO.MeetingDiscussion);
                            else
                                dbSmartAspects.AddInParameter(command, "@MeetingDiscussion", DbType.String, DBNull.Value);

                            if (!string.IsNullOrEmpty(feedbackBO.CallToAction))
                                dbSmartAspects.AddInParameter(command, "@CallToAction", DbType.String, feedbackBO.CallToAction);
                            else
                                dbSmartAspects.AddInParameter(command, "@CallToAction", DbType.String, DBNull.Value);

                            dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                            OutId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                        }
                        if (status && participantFromCompany != "")
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveUpdateTaskFeedbackAssignedParticipant_SP"))
                            {
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@FeedbackId", DbType.Int64, OutId);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@ParticipantList", DbType.String, participantFromCompany);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@Type", DbType.String, "Office");

                                status = (dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction) > 0);
                            }
                        }
                        if (status && participantFromClient != "")
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveUpdateTaskFeedbackAssignedParticipant_SP"))
                            {
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@FeedbackId", DbType.Int64, OutId);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@ParticipantList", DbType.String, participantFromClient);
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@Type", DbType.String, "Client");

                                status = (dbSmartAspects.ExecuteNonQuery(cmdOutDetails, transction) > 0);
                            }
                        }

                        if (status)
                        {
                            retVal = true;
                            transction.Commit();
                        }
                        else
                        {
                            retVal = false;
                            transction.Rollback();
                            status = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
    }
}
