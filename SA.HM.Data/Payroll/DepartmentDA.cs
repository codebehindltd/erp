using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Data.HMCommon
{
    public class DepartmentDA : BaseService
    {
        public List<DepartmentBO> GetDepartmentInfo()
        {
            List<DepartmentBO> boList = new List<DepartmentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDepartmentInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                DepartmentBO bo = new DepartmentBO();

                                bo.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bo.Name = reader["Name"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();
                                bo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bo.ActiveStatus = reader["ActiveStatus"].ToString();

                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
        public Boolean SaveDepartmentInfo(DepartmentBO bo, out int tmpBankId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDepartmentInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, bo.Name);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, bo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bo.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bo.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@DepartmentId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpBankId = Convert.ToInt32(command.Parameters["@DepartmentId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateDepartmentInfo(DepartmentBO bo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDepartmentInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, bo.DepartmentId);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, bo.Name);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, bo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bo.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, bo.LastModifiedBy);

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
        public DepartmentBO GetDepartmentInfoById(int pkId)
        {
            DepartmentBO bo = new DepartmentBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDepartmentInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, pkId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bo.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bo.Name = reader["Name"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();
                                bo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bo.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return bo;
        }
        public List<DepartmentBO> GetDepartmentInfoBySearchCriteria(string DepartmentName, bool ActiveStat)
        {
            List<DepartmentBO> boList = new List<DepartmentBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDepartmentInfoBySearchCriteria_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(DepartmentName))
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DepartmentName);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);

                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, ActiveStat);

                    DataSet DepartmentInfoDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, DepartmentInfoDS, "DepartmentInfo");
                    DataTable Table = DepartmentInfoDS.Tables["DepartmentInfo"];

                    boList = Table.AsEnumerable().Select(r => new DepartmentBO
                    {
                        DepartmentId = r.Field<int>("DepartmentId"),
                        Name = r.Field<string>("Name"),
                        Remarks = r.Field<string>("Remarks"),
                        ActiveStat = r.Field<bool>("ActiveStat"),
                        ActiveStatus = r.Field<string>("ActiveStatus")

                    }).ToList();
                }
            }
            return boList;
        }
        public List<EmployeeBO> GetEmployeeOfTechnicalDepartment()
        {
            List<EmployeeBO> employeeBOs = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeOfTechnicalDepartment_SP"))
                {
                    
                    DataSet DepartmentInfoDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, DepartmentInfoDS, "DepartmentInfo");
                    DataTable Table = DepartmentInfoDS.Tables["DepartmentInfo"];

                    employeeBOs = Table.AsEnumerable().Select(r => new EmployeeBO
                    {
                        EmpId = r.Field<int>("EmpId"),
                        DisplayName = r.Field<string>("DisplayName")
                    }).ToList();
                }
            }
            return employeeBOs;
        }
        public int CheckDepartmentReference(int id)
        {

            int DepartmentId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("CheckDepartmentReference_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, id);
                    
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                            }
                        }
                    }
                }
            }
            return DepartmentId;
        }

    }
}
