using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

using HotelManagement.Entity.Payroll;
using System.Collections;

namespace HotelManagement.Data.Payroll
{
    public class EmpTrainingDA : BaseService
    {
        public int SaveEmpTrainingInfo(EmpTrainingBO trainingInfo, out int tmpTrainingId, List<EmpTrainingDetailBO> detailBO)
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpTrainingInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(command, "@Trainer", DbType.String, trainingInfo.Trainer);
                        dbSmartAspects.AddInParameter(command, "@TrainingTypeId", DbType.Int32, trainingInfo.TrainingTypeId);
                        dbSmartAspects.AddInParameter(command, "@OrganizerId", DbType.Int32, trainingInfo.OrganizerId);
                        dbSmartAspects.AddInParameter(command, "@StartDate", DbType.DateTime, trainingInfo.StartDate);
                        dbSmartAspects.AddInParameter(command, "@AttendeeList", DbType.String, trainingInfo.AttendeeList);
                        dbSmartAspects.AddInParameter(command, "@Location", DbType.String, trainingInfo.Location);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, trainingInfo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@Discussed", DbType.String, trainingInfo.Discussed);
                        dbSmartAspects.AddInParameter(command, "@CallToAction", DbType.String, trainingInfo.CallToAction);
                        dbSmartAspects.AddInParameter(command, "@Conclusion", DbType.String, trainingInfo.Conclusion);
                        dbSmartAspects.AddInParameter(command, "@Reminder", DbType.Boolean, trainingInfo.Reminder);
                        dbSmartAspects.AddInParameter(command, "@ReminderHour", DbType.Int32, trainingInfo.ReminderHour);
                        dbSmartAspects.AddInParameter(command, "@EndDate", DbType.DateTime, trainingInfo.EndDate);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, trainingInfo.CreatedBy);

                        //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        dbSmartAspects.AddOutParameter(command, "@TrainingId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        tmpTrainingId = Convert.ToInt32(command.Parameters["@TrainingId"].Value);

                        if (status > 0)
                        {
                            int count = 0;

                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveTrainingDetailInfo_SP"))
                            {
                                foreach (EmpTrainingDetailBO trainingDetailBO in detailBO)
                                {
                                    if (trainingDetailBO.TrainingId == 0)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@TrainingId", DbType.Int32, tmpTrainingId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@EmpId", DbType.Int32, trainingDetailBO.EmpId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@EmpName", DbType.String, trainingDetailBO.EmpName);
                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }
                            if (count == detailBO.Count)
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
                            retVal = false;
                        }

                    }
                }
            }
            return tmpTrainingId;
        }
        public Boolean UpdateEmpTrainingInfo(EmpTrainingBO trainingInfo, List<EmpTrainingDetailBO> detailBO, ArrayList arrayDelete)
        {
            bool retVal = false;
            int status = 0;
            int tmpTrainingId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpTrainingInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(command, "@TrainingId", DbType.String, trainingInfo.TrainingId);
                        dbSmartAspects.AddInParameter(command, "@Trainer", DbType.String, trainingInfo.Trainer);
                        dbSmartAspects.AddInParameter(command, "@TrainingTypeId", DbType.Int32, trainingInfo.TrainingTypeId);
                        dbSmartAspects.AddInParameter(command, "@OrganizerId", DbType.Int32, trainingInfo.OrganizerId);
                        dbSmartAspects.AddInParameter(command, "@StartDate", DbType.DateTime, trainingInfo.StartDate);
                        dbSmartAspects.AddInParameter(command, "@AttendeeList", DbType.String, trainingInfo.AttendeeList);
                        dbSmartAspects.AddInParameter(command, "@Location", DbType.String, trainingInfo.Location);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, trainingInfo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@Discussed", DbType.String, trainingInfo.Discussed);
                        dbSmartAspects.AddInParameter(command, "@CallToAction", DbType.String, trainingInfo.CallToAction);
                        dbSmartAspects.AddInParameter(command, "@Conclusion", DbType.String, trainingInfo.Conclusion);
                        dbSmartAspects.AddInParameter(command, "@Reminder", DbType.Boolean, trainingInfo.Reminder);
                        dbSmartAspects.AddInParameter(command, "@ReminderHour", DbType.Int32, trainingInfo.ReminderHour);
                        dbSmartAspects.AddInParameter(command, "@EndDate", DbType.DateTime, trainingInfo.EndDate);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, trainingInfo.LastModifiedBy);

                        //status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        status = dbSmartAspects.ExecuteNonQuery(command, transction);
                        tmpTrainingId = trainingInfo.TrainingId;

                        if (status > 0)
                        {
                            int count = 0;
                            //using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveTrainingDetailInfo_SP"))
                            //{
                            //    foreach (EmpTrainingDetailBO trainingDetailBO in detailBO)
                            //    {
                            //        if (trainingDetailBO.TrainingDetailId == 0)
                            //        {
                            //            commandDetails.Parameters.Clear();
                            //            dbSmartAspects.AddInParameter(commandDetails, "@TrainingId", DbType.Int32, tmpTrainingId);
                            //            dbSmartAspects.AddInParameter(commandDetails, "@EmpId", DbType.Int32, trainingDetailBO.EmpId);
                            //            dbSmartAspects.AddInParameter(commandDetails, "@EmpName", DbType.String, trainingDetailBO.EmpName);
                            //            dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            //            count++;
                            //        }
                            //    }
                            //}
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateTrainingDetailsInfo_SP"))
                            {
                                foreach (EmpTrainingDetailBO trainingDetailBO in detailBO)
                                {
                                    if (trainingDetailBO.TrainingDetailId != 0)
                                    {
                                        commandDetails.Parameters.Clear();
                                        dbSmartAspects.AddInParameter(commandDetails, "@TrainingDetailId", DbType.Int32, trainingDetailBO.TrainingDetailId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@TrainingId", DbType.Int32, trainingDetailBO.TrainingId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@EmpId", DbType.Int32, trainingDetailBO.EmpId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@EmpName", DbType.String, trainingDetailBO.EmpName);
                                        dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                        count++;
                                    }
                                    else// new add to previous training
                                    {
                                        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveTrainingDetailInfo_SP"))
                                        {
                                            cmd.Parameters.Clear();
                                            dbSmartAspects.AddInParameter(cmd, "@TrainingId", DbType.Int32, tmpTrainingId);
                                            dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, trainingDetailBO.EmpId);
                                            dbSmartAspects.AddInParameter(cmd, "@EmpName", DbType.String, trainingDetailBO.EmpName);
                                            dbSmartAspects.ExecuteNonQuery(cmd, transction);
                                            count++;
                                        }
                                            
                                    }
                                }
                            }
                            //using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateTrainingDetailsInfo_SP"))
                            //{
                            //    foreach (EmpTrainingDetailBO trainingDetailBO in detailBO)
                            //    {
                            //        if (trainingDetailBO.TrainingDetailId != 0)
                            //        {
                            //            commandDetails.Parameters.Clear();
                            //            dbSmartAspects.AddInParameter(commandDetails, "@TrainingDetailId", DbType.Int32, trainingDetailBO.TrainingDetailId);
                            //            dbSmartAspects.AddInParameter(commandDetails, "@TrainingId", DbType.Int32, trainingDetailBO.TrainingId);
                            //            dbSmartAspects.AddInParameter(commandDetails, "@EmpId", DbType.String, trainingDetailBO.EmpId);                                        
                            //            count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                            //        }
                            //    }
                            //}
                            if (count == detailBO.Count)
                            {
                                if (arrayDelete != null)
                                {
                                    if (arrayDelete.Count > 0)
                                    {
                                        foreach (int delId in arrayDelete)
                                        {
                                            using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                            {
                                                dbSmartAspects.AddInParameter(commandEducation, "@TableName", DbType.String, "PayrollEmpTrainingDetail");
                                                dbSmartAspects.AddInParameter(commandEducation, "@TablePKField", DbType.String, "TrainingDetailId");
                                                dbSmartAspects.AddInParameter(commandEducation, "@TablePKId", DbType.String, delId);
                                                status = dbSmartAspects.ExecuteNonQuery(commandEducation);
                                            }
                                        }
                                    }
                                }
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
                            retVal = false;
                        }

                    }
                }
            }
            return retVal;
        }
        public EmpTrainingBO GetEmployeeTrainingById(int trainingId)
        {
            EmpTrainingBO training = new EmpTrainingBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeTrainingById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TrainingId", DbType.Int32, trainingId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpTraining");
                    DataTable Table = ds.Tables["EmpTraining"];

                    training = Table.AsEnumerable().Select(r => new EmpTrainingBO
                    {
                        TrainingId = r.Field<Int32>("TrainingId"),
                        TrainingTypeId = r.Field<Int32>("TrainingTypeId"),
                        TrainingName = r.Field<string>("TrainingName"),
                        Trainer = r.Field<string>("Trainer"),
                        OrganizerId = r.Field<Int32>("OrganizerId"),
                        StartDate = r.Field<DateTime>("StartDate"),
                        AttendeeList = r.Field<string>("AttendeeList"),
                        Location = r.Field<string>("Location"),
                        Remarks = r.Field<string>("Remarks"),
                        Discussed = r.Field<string>("Discussed"),
                        CallToAction = r.Field<string>("CallToAction"),
                        Conclusion = r.Field<string>("Conclusion"),
                        Reminder = r.Field<Boolean?>("Reminder"),
                        ReminderHour = r.Field<Int32?>("ReminderHour"),
                        EndDate = r.Field<DateTime>("EndDate")

                    }).FirstOrDefault();
                }
            }
            return training;
        }
        public List<EmpTrainingBO> GetOrganizerInfoBySearchCriteriaForPagination(string trainer, string trainingName, string organizerId, string location, DateTime? fromDate, DateTime? toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            //        @Trainer VARCHAR(100),
            //@CourseName VARCHAR(100),
            //@OrganizerId INT,
            //@Location VARCHAR(100),
            //@FromDate DATETIME,
            //@ToDate DATETIME,

            List<EmpTrainingBO> training = new List<EmpTrainingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeTrainingWithPagination_SP"))
                {
                    if (!string.IsNullOrEmpty(trainer))
                        dbSmartAspects.AddInParameter(cmd, "@Trainer", DbType.String, trainer);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Trainer", DbType.String, DBNull.Value);

                    if (!string.IsNullOrEmpty(trainingName))
                        dbSmartAspects.AddInParameter(cmd, "@TrainingName", DbType.String, trainingName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TrainingName", DbType.String, DBNull.Value);

                    if (organizerId != "0")
                        dbSmartAspects.AddInParameter(cmd, "@OrganizerId", DbType.Int32, Convert.ToInt32(organizerId));
                    else
                        dbSmartAspects.AddInParameter(cmd, "@OrganizerId", DbType.Int32, DBNull.Value);

                    if (!string.IsNullOrEmpty(location))
                        dbSmartAspects.AddInParameter(cmd, "@Location", DbType.String, location);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Location", DbType.String, DBNull.Value);

                    if (fromDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);

                    if (toDate != null)
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpTraining");
                    DataTable Table = ds.Tables["EmpTraining"];

                    training = Table.AsEnumerable().Select(r => new EmpTrainingBO
                    {
                        TrainingId = r.Field<Int32>("TrainingId"),
                        TrainingTypeId = r.Field<Int32>("TrainingTypeId"),
                        TrainingName = r.Field<string>("TrainingName"),
                        Trainer = r.Field<string>("Trainer"),
                        OrganizerId = r.Field<Int32>("OrganizerId"),
                        Organizer = r.Field<string>("OrganizerName"),
                        StartDate = r.Field<DateTime>("StartDate"),
                        AttendeeList = r.Field<string>("AttendeeList"),
                        Location = r.Field<string>("Location"),
                        Remarks = r.Field<string>("Remarks"),
                        Reminder = r.Field<Boolean>("Reminder"),
                        ReminderHour = r.Field<Int32>("ReminderHour"),
                        EndDate = r.Field<DateTime>("EndDate")

                    }).ToList();

                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return training;
        }
        public List<PayrollEmpTrainingTypeBO> GetTrainingType()
        {
            List<PayrollEmpTrainingTypeBO> training = new List<PayrollEmpTrainingTypeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTrainingType_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpTraining");
                    DataTable Table = ds.Tables["EmpTraining"];

                    training = Table.AsEnumerable().Select(r => new PayrollEmpTrainingTypeBO
                    {
                        TrainingTypeId = r.Field<Int32>("TrainingTypeId"),
                        TrainingName = r.Field<string>("TrainingName"),
                        Remarks = r.Field<string>("Remarks")

                    }).ToList();
                }
            }
            return training;
        }
        public List<EmpTrainingDetailBO> GetTrainingDetailInfoById(int trainingId)
        {
            List<EmpTrainingDetailBO> detailList = new List<EmpTrainingDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTrainingDetailInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TrainingId", DbType.Int32, trainingId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpTrainingDetailBO detail = new EmpTrainingDetailBO();

                                detail.TrainingDetailId = Int32.Parse(reader["TrainingDetailId"].ToString());
                                detail.TrainingId = Int32.Parse(reader["TrainingId"].ToString());
                                detail.EmpId = Int32.Parse(reader["EmpId"].ToString());
                                detail.EmpName = reader["EmpName"].ToString();

                                detailList.Add(detail);
                            }
                        }
                    }
                }
            }
            return detailList;
        }
        public bool DeleteEmpTrainingInfoById(int trainingId)
        {
            bool retVal = false;
            int status = 0;
            int detailStatus = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteEmpTrainingInfoById_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(command, "@TrainingId", DbType.Int32, trainingId);

                        status = dbSmartAspects.ExecuteNonQuery(command, transction);

                        if (status > 0)
                        {
                            // int count = 0;

                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("DeleteEmpTrainingDetailInfoById_SP"))
                            {

                                commandDetails.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandDetails, "@TrainingId", DbType.Int32, trainingId);
                                //dbSmartAspects.AddInParameter(commandDetails, "@TrainingDetailId", DbType.Int32, trainingDetailBO.EmpId);
                                detailStatus = dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                //count++;
                            }
                            if (detailStatus > 0)
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
                            retVal = false;
                        }

                    }
                }
            }
            return retVal;
        }

        //For Report
        public EmpTrainingBO GetEmployeeTrainingByTrainingTypeId(int trainingTypeId, DateTime? fromDate, DateTime? toDate)
        {
            EmpTrainingBO training = new EmpTrainingBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeTrainingByTrainingTypeId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TrainingTypeId", DbType.Int32, trainingTypeId);
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpTraining");
                    DataTable Table = ds.Tables["EmpTraining"];

                    training = Table.AsEnumerable().Select(r => new EmpTrainingBO
                    {
                        TrainingId = r.Field<Int32>("TrainingId"),
                        TrainingTypeId = r.Field<Int32>("TrainingTypeId"),
                        TrainingName = r.Field<string>("TrainingName"),
                        Trainer = r.Field<string>("Trainer"),
                        OrganizerId = r.Field<Int32>("OrganizerId"),
                        Organizer = r.Field<string>("OrganizerName"),
                        StartDate = r.Field<DateTime>("StartDate"),
                        AttendeeList = r.Field<string>("AttendeeList"),
                        Location = r.Field<string>("Location"),
                        Remarks = r.Field<string>("Remarks"),
                        EmpEmail = r.Field<string>("EmpEmail"),
                        Reminder = r.Field<Boolean?>("Reminder"),
                        ReminderHour = r.Field<Int32?>("ReminderHour"),
                        EndDate = r.Field<DateTime>("EndDate")

                    }).FirstOrDefault();
                }
            }
            return training;
        }
        public List<EmpTrainingDetailViewBO> GetTrainingDetailInfoByTrainingId(int trainingId)
        {
            List<EmpTrainingDetailViewBO> detailList = new List<EmpTrainingDetailViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTrainingDetailInfoByTrainingIdForReport_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TrainingId", DbType.Int32, trainingId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpTrainingDetail");
                    DataTable Table = ds.Tables["EmpTrainingDetail"];

                    detailList = Table.AsEnumerable().Select(r => new EmpTrainingDetailViewBO
                    {
                        TrainingId = r.Field<Int32>("TrainingId"),
                        TrainingDetailId = r.Field<Int32>("TrainingDetailId"),
                        EmpId = r.Field<Int32>("EmpId"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmpName = r.Field<string>("EmpName"),
                        EmpDepartment = r.Field<string>("EmpDepartment"),
                        EmpDesignation = r.Field<string>("EmpDesignation"),
                        EmpBranch = r.Field<string>("EmpBranch"),
                        EmpEmail = r.Field<string>("EmpEmail")
                    }).ToList();
                }
            }
            return detailList;
        }

        public List<EmpTrainingBO> GetTrainingListForDepartmentHead(int empId)
        {
            List<EmpTrainingBO> trainingList = new List<EmpTrainingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTrainingListForDepartmentHead_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpTraining");
                    DataTable Table = ds.Tables["EmpTraining"];

                    trainingList = Table.AsEnumerable().Select(r => new EmpTrainingBO
                    {
                        TrainingId = r.Field<Int32>("TrainingId"),
                        TrainingTypeId = r.Field<Int32>("TrainingTypeId"),
                        TrainingName = r.Field<string>("TrainingName"),
                        Trainer = r.Field<string>("Trainer"),
                        OrganizerId = r.Field<Int32>("OrganizerId"),
                        Organizer = r.Field<string>("Organizer"),
                        StartDate = r.Field<DateTime>("StartDate"),
                        AttendeeList = r.Field<string>("AttendeeList"),
                        Location = r.Field<string>("Location"),
                        Remarks = r.Field<string>("Remarks"),
                        Reminder = r.Field<Boolean?>("Reminder"),
                        ReminderHour = r.Field<Int32?>("ReminderHour"),
                        EndDate = r.Field<DateTime>("EndDate"),

                        FromDate = r.Field<string>("FromDate"),
                        ToDate = r.Field<string>("ToDate")
                    }).ToList();
                }
            }
            return trainingList;
        }


        public List<EmpTrainingBO> GetUpcomingProjectsByEmployeeId(int empId)
        {
            List<EmpTrainingBO> trainingList = new List<EmpTrainingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUpcomingProjectsByEmployeeId"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpTraining");
                    DataTable Table = ds.Tables["EmpTraining"];

                    trainingList = Table.AsEnumerable().Select(r => new EmpTrainingBO
                    {
                        TrainingId = r.Field<Int32>("TrainingId"),
                        TrainingName = r.Field<string>("TrainingName"),
                        StartDate = r.Field<DateTime>("StartDate"),
                        EndDate = r.Field<DateTime>("EndDate"),
                        Location = r.Field<string>("Location"),
                        Trainer = r.Field<string>("Trainer")
                    }).ToList();
                }
            }
            return trainingList;
        }

        public List<EmpTrainingBO> GetTrainingListForIndividualEmp(int empId)
        {
            List<EmpTrainingBO> trainingList = new List<EmpTrainingBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTrainingListForIndividualEmp_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpTraining");
                    DataTable Table = ds.Tables["EmpTraining"];

                    trainingList = Table.AsEnumerable().Select(r => new EmpTrainingBO
                    {
                        TrainingId = r.Field<Int32>("TrainingId"),
                        TrainingTypeId = r.Field<Int32>("TrainingTypeId"),
                        TrainingName = r.Field<string>("TrainingName"),
                        Trainer = r.Field<string>("Trainer"),
                        OrganizerId = r.Field<Int32>("OrganizerId"),
                        Organizer = r.Field<string>("Organizer"),
                        StartDate = r.Field<DateTime>("StartDate"),
                        AttendeeList = r.Field<string>("AttendeeList"),
                        Location = r.Field<string>("Location"),
                        Remarks = r.Field<string>("Remarks"),
                        Reminder = r.Field<Boolean?>("Reminder"),
                        ReminderHour = r.Field<Int32?>("ReminderHour"),
                        EndDate = r.Field<DateTime>("EndDate"),

                        FromDate = r.Field<string>("FromDate"),
                        ToDate = r.Field<string>("ToDate")
                    }).ToList();
                }
            }
            return trainingList;
        }
    }
}
