using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class EmpGradeDA : BaseService
    {


        public List<EmpGradeBO> GetGradeInfo()
        {
            List<EmpGradeBO> gradeList = new List<EmpGradeBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGradeInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpGradeBO gradeBO = new EmpGradeBO();
                                gradeBO.GradeId = Convert.ToInt32(reader["GradeId"]);
                                gradeBO.Name = reader["Name"].ToString();
                                gradeBO.ProvisionPeriodId = Convert.ToInt32(reader["ProvisionPeriodId"]);
                                gradeBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                gradeBO.Remarks = reader["Remarks"].ToString();
                                gradeBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                gradeList.Add(gradeBO);
                            }
                        }
                    }
                }
            }
            return gradeList;
        }
        public List<EmpGradeBO> GetActiveGradeInfo()
        {
            List<EmpGradeBO> boList = new List<EmpGradeBO>();

            DataSet gradeDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveGradeInfo_SP"))
                {

                    dbSmartAspects.LoadDataSet(cmd, gradeDS, "GradeList");
                    DataTable table = gradeDS.Tables["GradeList"];

                    boList = table.AsEnumerable().Select(r =>
                                   new EmpGradeBO
                                   {
                                       GradeId = r.Field<int>("GradeId"),
                                       Name = r.Field<string>("Name"),
                                       Remarks = r.Field<string>("Remarks"),
                                       ActiveStat = r.Field<Boolean>("ActiveStat"),
                                       BasicAmount = r.Field<decimal?>("BasicAmount")

                                   }).ToList();
                }
            }

            return boList;
        }
        public EmpGradeBO GetGradeInfoById(int GradeId)
        {
            EmpGradeBO gradeBO = new EmpGradeBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGradeInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GradeId", DbType.Int32, GradeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                gradeBO.GradeId = Convert.ToInt32(reader["GradeId"]);
                                gradeBO.Name = reader["Name"].ToString();
                                gradeBO.ProvisionPeriodMonth = Convert.ToInt32(reader["ProvisionPeriodMonth"]);
                                gradeBO.ProvisionPeriodId = Convert.ToInt32(reader["ProvisionPeriodId"]);
                                gradeBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                gradeBO.Remarks = reader["Remarks"].ToString();
                                gradeBO.IsManagement = Convert.ToBoolean(reader["IsManagement"]);
                            }
                        }
                    }
                }
            }
            return gradeBO;
        }

        public bool SaveGradeInfo(EmpGradeBO gradeBO, out int tmpUserInfoId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGradeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, gradeBO.Name);
                        dbSmartAspects.AddInParameter(command, "@ProvisionPeriodId", DbType.Int32, gradeBO.ProvisionPeriodId);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, gradeBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, gradeBO.Remarks);
                        dbSmartAspects.AddInParameter(command, "@IsManagement", DbType.Boolean, gradeBO.IsManagement);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, gradeBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@GradeId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpUserInfoId = Convert.ToInt32(command.Parameters["@GradeId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public bool UpdateGradeInfo(EmpGradeBO gradeBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGradeInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@GradeId", DbType.Int32, gradeBO.GradeId);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, gradeBO.Name);
                        dbSmartAspects.AddInParameter(command, "@ProvisionPeriodId", DbType.Int32, gradeBO.ProvisionPeriodId);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, gradeBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, gradeBO.Remarks);
                        dbSmartAspects.AddInParameter(command, "@IsManagement", DbType.Boolean, gradeBO.IsManagement);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, gradeBO.LastModifiedBy);
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

        public List<EmpGradeBO> GetGradeInfoBySearchCriteria(string Name, bool ActiveStat)
        {
            List<EmpGradeBO> gradeList = new List<EmpGradeBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGradeInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GradeName", DbType.String, Name);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStatus", DbType.Boolean, ActiveStat);

                    DataSet GradeInfoDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, GradeInfoDS, "GradeInfo");
                    DataTable Table = GradeInfoDS.Tables["GradeInfo"];

                    gradeList = Table.AsEnumerable().Select(r => new EmpGradeBO
                    {
                        GradeId = r.Field<int>("GradeId"),
                        Name = r.Field<string>("Name"),
                        ProvisionPeriodId = r.Field<int>("ProvisionPeriodId"),
                        ActiveStat = r.Field<bool>("ActiveStat"),
                        Remarks = r.Field<string>("Remarks"),
                        IsManagement = r.Field<bool>("IsManagement"),
                        IsManagementText = r.Field<string>("IsManagementText"),
                        CreatedDate = r.Field<DateTime>("CreatedDate")

                    }).ToList();
                }
            }
            return gradeList;
        }

    }
}
