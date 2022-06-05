using HotelManagement.Entity.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.Payroll
{
    public class EmpWorkStationDA : BaseService
    {
        public Boolean SaveUpdateWorkStationInformation(EmpWorkStationBO EmpWorkStationBO, out long OutId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateWorkStationInformation_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@WorkStationId", DbType.Int64, EmpWorkStationBO.WorkStationId);

                        if (EmpWorkStationBO.WorkStationName != "")
                            dbSmartAspects.AddInParameter(command, "@WorkStationName", DbType.String, EmpWorkStationBO.WorkStationName);
                        else
                            dbSmartAspects.AddInParameter(command, "@WorkStationName", DbType.String, DBNull.Value);

                        if (EmpWorkStationBO.Description != "")
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, EmpWorkStationBO.Description);
                        else
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@Status", DbType.String, EmpWorkStationBO.Status);

                        if (EmpWorkStationBO.WorkStationId == 0)
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, EmpWorkStationBO.CreatedBy);
                        else
                            dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, EmpWorkStationBO.LastModifiedBy);

                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        OutId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public List<EmpWorkStationBO> GetAllWorkStation()
        {
            List<EmpWorkStationBO> boList = new List<EmpWorkStationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllWorkStation_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpWorkStationBO bo = new EmpWorkStationBO();

                                bo.WorkStationId = Convert.ToInt32(reader["WorkStationId"]);
                                bo.WorkStationName = reader["WorkStationName"].ToString();
                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
        public List<EmpWorkStationBO> GetWorkStationInformationPagination(string WorkStationName, bool Status, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<EmpWorkStationBO> WorkStationInformationList = new List<EmpWorkStationBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetWorkStationInformationForPaging_SP"))
                    {

                        if ((WorkStationName) != "")
                            dbSmartAspects.AddInParameter(cmd, "@WorkStationName", DbType.String, WorkStationName);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@WorkStationName", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@Status", DbType.String, Status);


                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    EmpWorkStationBO WorkStationInformation = new EmpWorkStationBO();

                                    WorkStationInformation.WorkStationId = Convert.ToInt32(reader["WorkStationId"]);
                                    WorkStationInformation.WorkStationName = (reader["WorkStationName"].ToString());

                                    WorkStationInformationList.Add(WorkStationInformation);
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
            return WorkStationInformationList;
        }
        public EmpWorkStationBO GetWorkStationInformationById(int id)
        {
            EmpWorkStationBO WorkStationInformation = new EmpWorkStationBO();
            string query = string.Format("SELECT * FROM PayrollEmpWorkStation  WHERE IsDeleted= 0 AND WorkStationId = {0}", id);

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

                                    WorkStationInformation.WorkStationId = Convert.ToInt32(reader["WorkStationId"]);
                                    WorkStationInformation.WorkStationName = (reader["WorkStationName"].ToString());
                                    WorkStationInformation.Description = (reader["Description"].ToString());
                                    if (reader["Status"] != DBNull.Value)
                                    {
                                        WorkStationInformation.Status = Convert.ToBoolean(reader["Status"]);
                                    }

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

            return WorkStationInformation;
        }
        public bool DeleteSource(long Id)
        {
            bool status = false;
            try
            {
                string query = string.Format("DELETE FROM PayrollEmpWorkStation WHERE WorkStationId = {0}", Id);

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
    }
}
