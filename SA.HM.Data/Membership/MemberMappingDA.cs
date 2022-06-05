using HotelManagement.Entity.HMCommon;
using HotelManagement.Entity.Membership;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.Membership
{
    public class MemberMappingDA : BaseService
    {
        public List<MemMemberBasicsBO> GetMemberByType(int typeId)
        {
            List<MemMemberBasicsBO> memberList = new List<MemMemberBasicsBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetMemberByType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TypeId", DbType.Int32, typeId);

                    //DataSet memberDS = new DataSet();
                    //dbSmartAspects.LoadDataSet(cmd, memberDS, "MemberInfo");
                    //DataTable table = memberDS.Tables["MemberInfo"];
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                MemMemberBasicsBO employee = new MemMemberBasicsBO();

                                employee.MemberId = Convert.ToInt32(reader["MemberId"]);
                                employee.FullName = reader["FullName"].ToString();
                                if (reader["AttendanceDeviceMemberId"] != DBNull.Value)
                                    employee.AttendanceDeviceMemberId = Convert.ToInt32(reader["AttendanceDeviceMemberId"]);
                                memberList.Add(employee);
                            }
                        }
                    }

                    //memberList = table.AsEnumerable().Select(r =>
                    //              new MemMemberBasicsBO
                    //              {
                    //                  MemberId = r.Field<int>("MemberId"),
                    //                  if (reader["AttendanceDeviceEmpId"] != DBNull.Value)
                    //                        = Convert.ToInt32(reader["AttendanceDeviceEmpId"]),
                    //                  FullName = r.Field<string>("FullName"),
                    //              }).ToList();
                }
            }
            return memberList;
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
        public List<MemMemberBasicsBO> GetMappingMemberList()
        {
            List<MemMemberBasicsBO> employeeList = new List<MemMemberBasicsBO>();
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
                                MemMemberBasicsBO employee = new MemMemberBasicsBO();

                                employee.MemberId = Convert.ToInt32(reader["nUserIdn"]);
                                employee.FullName = reader["sUserName"].ToString();
                                employee.DepartmentId = Convert.ToInt32(reader["nDepartmentIdn"]);
                                employeeList.Add(employee);
                            }
                        }
                    }
                }
            }
            return employeeList;
        }
        public bool UpdateMemberMapping(List<MemberViewBO> mappingMemberList)
        {
            bool status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateMemberMapping"))
                {
                    try
                    {
                        foreach (var item in mappingMemberList)
                        {
                            command.Parameters.Clear();
                            dbSmartAspects.AddInParameter(command, "@MemberId", DbType.Int32, item.MemberId);
                            if (item.MappingMemberId > 0)
                                dbSmartAspects.AddInParameter(command, "@MappingMemberId", DbType.Int32, item.MappingMemberId);
                            else
                                dbSmartAspects.AddInParameter(command, "@MappingMemberId", DbType.Int32, DBNull.Value);
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
