using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
   public class EmpPayScaleDA :BaseService
    {
        public List<EmpPayScaleBO> GetAllPayScaleInfo()
        {
            List<EmpPayScaleBO> payScaleList = new List<EmpPayScaleBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllPayScaleInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                EmpPayScaleBO payScaleBO = new EmpPayScaleBO();

                                payScaleBO.GradeId = Convert.ToInt32(reader["GradeId"]);
                                payScaleBO.Grade = reader["Grade"].ToString();
                                payScaleBO.BasicAmount = Convert.ToDecimal(reader["BasicAmount"].ToString());
                                payScaleBO.PayScaleId = Convert.ToInt32(reader["PayScaleId"].ToString());
                                payScaleBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                                payScaleList.Add(payScaleBO);
                            }
                        }
                    }
                }
            }
            return payScaleList;
        }

        public EmpPayScaleBO GetPayScaleByID(int EditId)
        {
            EmpPayScaleBO payScaleBO = new EmpPayScaleBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayScaleByID_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@PayScaleId", DbType.Int32, EditId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                payScaleBO.GradeId = Convert.ToInt32(reader["GradeId"]);
                                payScaleBO.Grade = reader["Grade"].ToString();
                                payScaleBO.BasicAmount = Convert.ToDecimal(reader["BasicAmount"].ToString());
                                payScaleBO.PayScaleId = Convert.ToInt32(reader["PayScaleId"].ToString());
                                payScaleBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                            }
                        }
                    }
                }
            }
            return payScaleBO;
        }

        public EmpPayScaleBO GetPayScaleInfoByGradeId(int gradeID)
        {
            EmpPayScaleBO payScaleBO = new EmpPayScaleBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayScaleInfoByGradeId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@GradeId", DbType.Int32, gradeID);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                payScaleBO.GradeId = Convert.ToInt32(reader["GradeId"]);
                                payScaleBO.Grade = reader["Grade"].ToString();
                                payScaleBO.BasicAmount = Convert.ToDecimal(reader["BasicAmount"].ToString());
                                payScaleBO.PayScaleId = Convert.ToInt32(reader["PayScaleId"].ToString());
                                payScaleBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                            }
                        }
                    }
                }
            }
            return payScaleBO;
        }

        public bool SavePayScaleInfo(EmpPayScaleBO payScaleBO, out int tmpUserInfoId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePayScaleInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@GradeId", DbType.Int32,payScaleBO.GradeId );
                    dbSmartAspects.AddInParameter(command, "@BasicAmount", DbType.Decimal,payScaleBO.BasicAmount);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, payScaleBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@PayScaleId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpUserInfoId = Convert.ToInt32(command.Parameters["@PayScaleId"].Value);
                }
            }
            return status; 
        }

        public bool UpdatePayScaleInfo(EmpPayScaleBO payScaleBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePayScaleInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@PayScaleId", DbType.Int32, payScaleBO.PayScaleId);
                    dbSmartAspects.AddInParameter(command, "@GradeId", DbType.Int32, payScaleBO.GradeId);
                    dbSmartAspects.AddInParameter(command, "@BasicAmount", DbType.Decimal, payScaleBO.BasicAmount);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, payScaleBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
