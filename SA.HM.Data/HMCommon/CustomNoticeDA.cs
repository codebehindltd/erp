using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.HMCommon
{
    public class CustomNoticeDA : BaseService
    {
        public List<CustomNoticeBO> GetNoticeInfoForSearch(string name, string fromDate, string toDate, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<CustomNoticeBO> noticeBOs = new List<CustomNoticeBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCustomNoticeInfoBySearchCriteriaForPaging_SP"))
                    {
                        if (!string.IsNullOrEmpty(name))
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, name);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                        if (!string.IsNullOrEmpty(fromDate))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, Convert.ToDateTime(fromDate));
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, DBNull.Value);
                        }
                        if (!string.IsNullOrEmpty(toDate))
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, Convert.ToDateTime(toDate));
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, DBNull.Value);
                        }

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    CustomNoticeBO noticeBO = new CustomNoticeBO();

                                    noticeBO.Id = Convert.ToInt64(reader["Id"]);
                                    noticeBO.NoticeName = reader["NoticeName"].ToString();

                                    if (reader["CreatedDate"] != DBNull.Value)
                                    {
                                        noticeBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);

                                    }
                                    if (reader["CloseDate"] != DBNull.Value)
                                    {
                                        noticeBO.CloseDate = Convert.ToDateTime(reader["CloseDate"]);

                                    }
                                    noticeBOs.Add(noticeBO);
                                }
                            }
                        }
                        totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return noticeBOs;
        }

        public bool SaveOrUpdateNotice(CustomNoticeBO notice, string empId, out long outId)
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
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateNotice_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, notice.Id);
                            dbSmartAspects.AddInParameter(command, "@NoticeName", DbType.String, notice.NoticeName);
                            dbSmartAspects.AddInParameter(command, "@Content", DbType.String, notice.Content);
                            dbSmartAspects.AddInParameter(command, "@CloseDate", DbType.String, notice.CloseDate);
                            dbSmartAspects.AddInParameter(command, "@UserInfoId", DbType.Int32, notice.CreatedBy);
                            dbSmartAspects.AddInParameter(command, "@AssignType", DbType.String, notice.AssignType);
                            if (notice.EmpDepartment != 0)
                                dbSmartAspects.AddInParameter(command, "@EmpDepartment", DbType.Int32, notice.EmpDepartment);
                            else
                                dbSmartAspects.AddInParameter(command, "@EmpDepartment", DbType.Int32, DBNull.Value);
                            dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                            status = (dbSmartAspects.ExecuteNonQuery(command, transction) > 0);

                            outId = Convert.ToInt64(command.Parameters["@OutId"].Value);
                        }
                        if (status && empId != "")
                        {
                            using (DbCommand cmdOutDetails = dbSmartAspects.GetStoredProcCommand("SaveUpdateNoticeAssignedEmployee_SP"))
                            {
                                dbSmartAspects.AddInParameter(cmdOutDetails, "@NoticeId", DbType.Int64, outId);
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

        public Boolean DeleteNotice(int Id)
        {
            bool status = false;
            try
            {
                string query = string.Format("Delete From CustomNotice WHERE Id = {0} ", Id);

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;

        }

        public List<CustomNoticeBO> GetCustomNoticeByEmpId(int empId)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<CustomNoticeBO> noticeList = new List<CustomNoticeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNoticeByEmpId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmployeeId", DbType.String, empId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CustomNoticeBO noticeBO = new CustomNoticeBO();
                                noticeBO.Id = Convert.ToInt32(reader["Id"]);
                                noticeBO.NoticeName = reader["NoticeName"].ToString();
                                noticeBO.CloseDate = Convert.ToDateTime(Convert.ToDateTime(reader["CloseDate"]).ToString("yyyy-MM-dd"));// + " " + reader["DueTime"]);
                                noticeBO.CreatedDate = Convert.ToDateTime(Convert.ToDateTime(reader["CreatedDate"]).ToString("yyyy-MM-dd"));// + " " + reader["DueTime"]);
                                
                                noticeList.Add(noticeBO);
                            }
                        }
                    }
                   
                }
            }
            return noticeList;
        }
        public CustomNoticeBO GetNoticeInfoById(long Id)
        {
            CustomNoticeBO noticeBO = new CustomNoticeBO();

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNoticeInfoById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, Id);

                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "CustomNotice");

                        noticeBO = ds.Tables[0].AsEnumerable().Select(r => new CustomNoticeBO
                        {
                            Id = r.Field<long>("Id"),
                            NoticeName = r.Field<string>("NoticeName"),
                            Content = r.Field<string>("Content"),
                            CreatedDate = r.Field<DateTime?>("CreatedDate"),
                            CloseDate = r.Field<DateTime?>("CloseDate"),
                            AssignType = r.Field<string>("AssignType"),
                            EmpDepartment = r.Field<int?>("EmpDepartment")
                        }).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return noticeBO;
        }
        //public List<long> GetNoticeAssignedEmployeeById(long id)
        //{
        //    List<long> EmpList = new List<long>();
        //    string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

        //    string query = string.Format("SELECT * FROM CustomNoticeEmployeeMapping WHERE NoticeId = {0}", id);

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
        //                            CustomNoticeEmpMapBO tMTaskAssignedEmployee = new CustomNoticeEmpMapBO();

        //                            tMTaskAssignedEmployee.EmpId = Convert.ToInt32(reader["EmpId"]);

        //                            EmpList.Add(tMTaskAssignedEmployee.EmpId);
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

        public List<EmployeeBO> GetNoticeAssignedEmployeeById(long id)
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            List<EmployeeBO> documentList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAssignedEmployeeByNoticeId"))
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
    }
}
