using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class ServiceChargeDA : BaseService
    {
        public List<EmployeeBO> GetEmployeeForServiceChargeConfig(int empTypeId, int departmentId)
        {
            List<EmployeeBO> boList = new List<EmployeeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpForServiceChargeConfig_SP"))
                {
                    if (departmentId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);
                    if (empTypeId != 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@EmpTypeId", DbType.Int32, empTypeId);
                    }
                    else dbSmartAspects.AddInParameter(cmd, "@EmpTypeId", DbType.Int32, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmployeeBO bo = new EmployeeBO();
                                bo.EmpId = Convert.ToInt32(reader["EmpId"]);
                                bo.EmpCode = reader["EmpCode"].ToString();
                                bo.FirstName = reader["FirstName"].ToString();
                                bo.LastName = reader["LastName"].ToString();
                                bo.FullName = bo.FirstName + " " + bo.LastName;
                                bo.DisplayName = reader["DisplayName"].ToString();
                                bo.EmployeeName = reader["EmployeeName"].ToString();
                                bo.JoinDate = Convert.ToDateTime(reader["JoinDate"]);
                                bo.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                                bo.Department = reader["Department"].ToString();
                                bo.EmpTypeId = Convert.ToInt32(reader["EmpTypeId"]);
                                bo.EmpType = reader["EmpType"].ToString();
                                bo.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                                bo.Designation = reader["Designation"].ToString();
                                bo.OfficialEmail = reader["OfficialEmail"].ToString();
                                bo.ReferenceBy = reader["ReferenceBy"].ToString();
                                //bo.PresentPhone = reader["PresentPhone"].ToString();
                                //bo.ResignationDate = reader["ResignationDate"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();
                                bo.NodeId = Convert.ToInt32(reader["NodeId"]);

                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
        public Boolean SaveServiceChargeConfig(ServiceChargeConfigurationBO ServiceCharge, List<ServiceChargeConfigurationDetailsBO> ServiceChargeDetails, List<ServiceChargeConfigurationDetailsBO> deleteList, out int serviceChargeId)
        {
            try
            {
                serviceChargeId = 0;
                Boolean status = false;

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    conn.Open();
                    using (DbTransaction transaction = conn.BeginTransaction())
                    {
                        if (ServiceCharge.ServiceChargeConfigurationId == 0)
                        {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveServiceCharge_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@ServiceAmount", DbType.Decimal, ServiceCharge.ServiceAmount);
                                dbSmartAspects.AddInParameter(command, "@TotalEmployee", DbType.Int16, ServiceCharge.TotalEmployee);
                                dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, ServiceCharge.DepartmentId);
                                dbSmartAspects.AddInParameter(command, "@EmpTypeId", DbType.Int32, ServiceCharge.EmpTypeId);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, ServiceCharge.CreatedBy);

                                dbSmartAspects.AddOutParameter(command, "@ServiceChargeId", DbType.Int32, sizeof(Int32));

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                                serviceChargeId = Convert.ToInt32(command.Parameters["@ServiceChargeId"].Value);
                            }

                            if (status)
                            {
                                status = false;

                                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveServiceChargeDetails_SP"))
                                {
                                    foreach (ServiceChargeConfigurationDetailsBO bo in ServiceChargeDetails)
                                    {
                                        status = false;
                                        cmd.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(cmd, "@ServiceChargeId", DbType.Int64, serviceChargeId);
                                        dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, bo.EmpId);

                                        status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                    }
                                }
                            }
                        }
                        else {
                            using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateServiceCharge_SP"))
                            {
                                dbSmartAspects.AddInParameter(command, "@ServiceChargeId", DbType.Int32, ServiceCharge.ServiceChargeConfigurationId);
                                dbSmartAspects.AddInParameter(command, "@ServiceAmount", DbType.Decimal, ServiceCharge.ServiceAmount);
                                dbSmartAspects.AddInParameter(command, "@TotalEmployee", DbType.Int16, ServiceCharge.TotalEmployee);
                                dbSmartAspects.AddInParameter(command, "@DepartmentId", DbType.Int32, ServiceCharge.DepartmentId);
                                dbSmartAspects.AddInParameter(command, "@EmpTypeId", DbType.Int32, ServiceCharge.EmpTypeId);
                                dbSmartAspects.AddInParameter(command, "@UpdatedBy", DbType.Int32, ServiceCharge.CreatedBy);                                

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;                                
                            }

                            if (status)
                            {
                                if (ServiceChargeDetails.Count > 0)
                                {
                                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("SaveServiceChargeDetails_SP"))
                                    {
                                        foreach (ServiceChargeConfigurationDetailsBO bo in ServiceChargeDetails)
                                        {
                                            status = false;
                                            cmd.Parameters.Clear();

                                            dbSmartAspects.AddInParameter(cmd, "@ServiceChargeId", DbType.Int64, bo.ServiceChargeConfigurationId);
                                            dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, bo.EmpId);

                                            status = dbSmartAspects.ExecuteNonQuery(cmd, transaction) > 0 ? true : false;
                                        }
                                    }
                                }
                            }
                        }
                        if (deleteList.Count > 0)
                        {
                            using (DbCommand commandReference = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                            {
                                foreach (ServiceChargeConfigurationDetailsBO bo in deleteList)
                                {
                                    commandReference.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandReference, "@TableName", DbType.String, "PayrollServiceChargeConfigurationDetails");
                                    dbSmartAspects.AddInParameter(commandReference, "@TablePKField", DbType.String, "ServiceChargeConfigurationDetailsId");
                                    dbSmartAspects.AddInParameter(commandReference, "@TablePKId", DbType.String, bo.ServiceChargeConfigurationDetailsId.ToString());

                                    status = dbSmartAspects.ExecuteNonQuery(commandReference) > 0 ? true : false;
                                }
                            }
                        }


                        if (status)
                            transaction.Commit();
                        else
                            transaction.Rollback();
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                throw ex;
            }
        }

        public List<ServiceChargeConfigurationDetailsBO> GetServiceChargeDetails(int departmentId, int empTypeId)
        {
            List<ServiceChargeConfigurationDetailsBO> boList = new List<ServiceChargeConfigurationDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetServiceChargeDetails_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    dbSmartAspects.AddInParameter(cmd, "@EmpTypeId", DbType.Int32, empTypeId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "ServiceChargeDetails");
                    DataTable Table = ds.Tables["ServiceChargeDetails"];

                    boList = Table.AsEnumerable().Select(r => new ServiceChargeConfigurationDetailsBO
                    {
                        ServiceChargeConfigurationDetailsId = r.Field<Int64>("ServiceChargeConfigurationDetailsId"),
                        ServiceChargeConfigurationId = r.Field<Int64>("ServiceChargeConfigurationId"),
                        EmpId = r.Field<Int32>("EmpId"),
                        DisplayName = r.Field<string>("DisplayName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        Designation = r.Field<string>("Designation"),
                        ServiceAmount = r.Field<decimal?>("ServiceAmount")
                    }).ToList();
                }
            }
            return boList;
        }
    }
}
