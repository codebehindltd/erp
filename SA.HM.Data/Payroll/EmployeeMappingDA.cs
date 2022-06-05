using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace HotelManagement.Data.Payroll
{
    public class EmployeeMappingDA : BaseService
    {
        public List<EmployeeBO> GetEmployeeByDepartmentAndStatus(long departmentId, int statusId, long deviceId)
        {
            List<EmployeeBO> employeeList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeByDepartmentAndStatus"))
                {
                    if (departmentId > 0)
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int64, departmentId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int64, DBNull.Value);
                    dbSmartAspects.AddInParameter(cmd, "@DeviceId", DbType.Int64, deviceId);
                    dbSmartAspects.AddInParameter(cmd, "@StatusId", DbType.Int32, statusId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO employee = new EmployeeBO();

                                employee.EmpId = Convert.ToInt32(reader["EmpId"]);
                                employee.EmpCode = reader["EmpCode"].ToString();
                                employee.DisplayName = reader["DisplayName"].ToString();
                                if (reader["AttendanceDeviceEmpCode"] != DBNull.Value)
                                    employee.AttendanceDeviceEmpCode = reader["AttendanceDeviceEmpCode"].ToString();
                                employeeList.Add(employee);
                            }
                        }
                    }
                }
            }
            return employeeList;
        }
        public List<DepartmentBO> GetMappingDepartmentInfo()
        {
            List<DepartmentBO> boList = new List<DepartmentBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMappingDepartmentInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                DepartmentBO bo = new DepartmentBO();

                                bo.DepartmentId = Convert.ToInt32(reader["nDepartmentIdn"]);
                                bo.Name = reader["sDepartment"].ToString();
                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
        public List<EmployeeBO> GetMappingEmployeeList()
        {
            List<EmployeeBO> employeeList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllUser"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO employee = new EmployeeBO();

                                employee.EmpId = Convert.ToInt32(reader["nUserIdn"]);
                                employee.EmpCode = reader["nUserIdn"].ToString();
                                employee.DisplayName = reader["sUserName"].ToString();
                                employee.DepartmentId = Convert.ToInt32(reader["nDepartmentIdn"]);
                                employeeList.Add(employee);
                            }
                        }
                    }
                }
            }
            return employeeList;
        }

        public bool UpdateEmployeeMapping(List<EmployeeViewBO> mappingEmployeeList)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmployeeMapping"))
                {
                    try
                    {
                        foreach (var item in mappingEmployeeList)
                        {
                            command.Parameters.Clear();
                            dbSmartAspects.AddInParameter(command, "@DeviceId", DbType.Int32, item.DeviceId);
                            dbSmartAspects.AddInParameter(command, "@EmployeeId", DbType.Int32, item.EmployeeId);
                            dbSmartAspects.AddInParameter(command, "@MappingEmployeeCode", DbType.String, item.MappingEmployeeCode);
                            
                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return status;
        }

    }
}
