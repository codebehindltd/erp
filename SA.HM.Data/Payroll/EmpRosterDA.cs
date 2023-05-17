using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpRosterDA : BaseService
    {
        public Boolean SaveEmpRosterInfo(List<EmpRosterBO> rosterBOList)
        {
            bool retVal = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {

                    int count = 0;

                    if (rosterBOList != null)
                    {
                        using (DbCommand commandBO = dbSmartAspects.GetStoredProcCommand("SaveEmpRosterInfo_SP"))
                        {
                            foreach (EmpRosterBO entityBO in rosterBOList)
                            {
                                commandBO.Parameters.Clear();

                                dbSmartAspects.AddInParameter(commandBO, "@EmpId", DbType.Int32, entityBO.EmpId);
                                dbSmartAspects.AddInParameter(commandBO, "@RosterId", DbType.Int32, entityBO.RosterId);
                                dbSmartAspects.AddInParameter(commandBO, "@RosterDate", DbType.DateTime, entityBO.RosterDate);
                                dbSmartAspects.AddInParameter(commandBO, "@TimeSlabId", DbType.Int32, entityBO.TimeSlabId);
                                dbSmartAspects.AddInParameter(commandBO, "@SecondTimeSlabId", DbType.Int32, entityBO.SecondTimeSlabId);
                                dbSmartAspects.AddInParameter(commandBO, "@CreatedBy", DbType.Int32, entityBO.CreatedBy);

                                count += dbSmartAspects.ExecuteNonQuery(commandBO, transction);
                            }
                        }
                    }


                    if (count == rosterBOList.Count)
                    {
                        transction.Commit();
                        retVal = true;
                    }
                    else
                    {
                        retVal = false;
                    }

                }

            }
            return retVal;
        }
        public EmpRosterBO GetEmpRosterInfoById(int rosterId, int empId, DateTime rosterDate)
        {
            EmpRosterBO entityBO = new EmpRosterBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpRosterInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RosterId", DbType.Int32, rosterId);
                    dbSmartAspects.AddInParameter(cmd, "@EmpId", DbType.Int32, empId);
                    dbSmartAspects.AddInParameter(cmd, "@RosterDate", DbType.DateTime, rosterDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.EmpRosterId = Convert.ToInt32(reader["EmpRosterId"]);
                                entityBO.EmpId = Convert.ToInt32(reader["EmpId"]);
                                entityBO.RosterId = Convert.ToInt32(reader["RosterId"]);
                                entityBO.RosterDate = Convert.ToDateTime(reader["RosterDate"]);
                                entityBO.TimeSlabId = Convert.ToInt32(reader["TimeSlabId"]);
                                entityBO.SecondTimeSlabId = Convert.ToInt32(reader["SecondTimeSlabId"]);
                            }
                        }
                    }
                }
            }
            return entityBO;
        }
        public List<EmpRoasterReportViewBO> GetEmpRoasterForReport(int roasterId, int departmentId)
        {
            List<EmpRoasterReportViewBO> empList = new List<EmpRoasterReportViewBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmployeeRosterInformation_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RosterId", DbType.Int32, roasterId);
                    if (departmentId > 0)
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, departmentId);
                    }
                    else
                    {
                        dbSmartAspects.AddInParameter(cmd, "@DepartmentId", DbType.Int32, DBNull.Value);
                    }

                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "EmployeeRoaster");
                    DataTable Table = ds.Tables["EmployeeRoaster"];

                    empList = Table.AsEnumerable().Select(r => new EmpRoasterReportViewBO
                    {
                        RosterName = r.Field<string>("RosterName"),
                        EmpCode = r.Field<string>("EmpCode"),
                        EmployeeName = r.Field<string>("EmployeeName"),
                        DepartmentName = r.Field<string>("DepartmentName"),
                        RosterDate = r.Field<DateTime>("RosterDate"),
                        RosterDateDisplay = r.Field<string>("RosterDateDisplay"),
                        RosterDateDayName = r.Field<string>("RosterDateDayName"),
                        TimeSlabHead = r.Field<string>("TimeSlabHead")
                    }).ToList();
                }
            }

            return empList;
        }
    }
}
