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
    public class EmpTypeDA : BaseService
    {
        public List<EmpTypeBO> GetEmpTypeInfo()
        {
            List<EmpTypeBO> boList = new List<EmpTypeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpTypeInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpTypeBO bo = new EmpTypeBO();

                                bo.TypeId = Convert.ToInt32(reader["TypeId"]);
                                bo.Name = reader["Name"].ToString();
                                bo.Code = reader["Code"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();
                                bo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bo.TypeCategory = reader["TypeCategory"].ToString();
                                bo.IsContractualType = Convert.ToBoolean(reader["IsContractualType"]);
                                bo.ActiveStatus = reader["ActiveStatus"].ToString();

                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
        public List<EmpTypeBO> GetActiveGradeInfo()
        {
            List<EmpTypeBO> boList = new List<EmpTypeBO>();

            DataSet empTypeDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveEmpTypeInfo_SP"))
                {

                    dbSmartAspects.LoadDataSet(cmd, empTypeDS, "EmpTypeList");
                    DataTable table = empTypeDS.Tables["EmpTypeList"];

                    boList = table.AsEnumerable().Select(r =>
                                   new EmpTypeBO
                                   {
                                       TypeId = r.Field<int>("TypeId"),
                                       Name = r.Field<string>("Name"),
                                       Code = r.Field<string>("Code"),
                                       Remarks = r.Field<string>("Remarks"),
                                       ActiveStat = r.Field<Boolean>("ActiveStat")

                                   }).ToList();
                }
            }

            return boList;
        }
        public Boolean SaveEmpTypeInfo(EmpTypeBO bo, out int tmpPkId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpTypeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, bo.Name);
                        dbSmartAspects.AddInParameter(command, "@Code", DbType.String, bo.Code);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, bo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bo.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@TypeCategory", DbType.String, bo.TypeCategory);
                        dbSmartAspects.AddInParameter(command, "@IsContractualType", DbType.Boolean, bo.IsContractualType);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bo.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@TypeId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpPkId = Convert.ToInt32(command.Parameters["@TypeId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }        
        public Boolean UpdateEmpTypeInfo(EmpTypeBO bo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpTypeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@TypeId", DbType.Int32, bo.TypeId);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, bo.Name);
                        dbSmartAspects.AddInParameter(command, "@Code", DbType.String, bo.Code);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, bo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bo.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@TypeCategory", DbType.String, bo.TypeCategory);
                        dbSmartAspects.AddInParameter(command, "@IsContractualType", DbType.Boolean, bo.IsContractualType);
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
        public EmpTypeBO GetEmpTypeInfoById(int pkId)
        {
            EmpTypeBO bo = new EmpTypeBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpTypeInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TypeId", DbType.Int32, pkId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bo.TypeId = Convert.ToInt32(reader["TypeId"]);
                                bo.Name = reader["Name"].ToString();
                                bo.Code = reader["Code"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();
                                bo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bo.ActiveStatus = reader["ActiveStatus"].ToString();
                                bo.TypeCategory = reader["TypeCategory"].ToString();
                                bo.IsContractualType = Convert.ToBoolean(reader["IsContractualType"]);
                            }
                        }
                    }
                }
            }
            return bo;
        }

        public List<EmpTypeWiseEmpNoViewBO> GetEmpTypeWiseEmp(int empId)
        {
            List<EmpTypeWiseEmpNoViewBO> boList = new List<EmpTypeWiseEmpNoViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpTypeWiseEmpByDepartment_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmpNo");
                    DataTable Table = ds.Tables["EmpNo"];

                    boList = Table.AsEnumerable().Select(r => new EmpTypeWiseEmpNoViewBO
                    {
                        TypeName = r.Field<string>("TypeName"),
                        NoOfEmp = r.Field<int>("NoOfEmp")
                    }).ToList();
                }
            }
            return boList;
        }

        public List<EmpTypeBO> GetEmpTypeInfoByServiceChargeApplicableFlag()
        {
            List<EmpTypeBO> boList = new List<EmpTypeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpTypeInfoByServiceChargeApplicableFlag_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpTypeBO bo = new EmpTypeBO();

                                bo.TypeId = Convert.ToInt32(reader["TypeId"]);
                                bo.Name = reader["Name"].ToString();
                                bo.Code = reader["Code"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();
                                bo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bo.IsContractualType = Convert.ToBoolean(reader["IsContractualType"]);
                                bo.ActiveStatus = reader["ActiveStatus"].ToString();

                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
    }
}
