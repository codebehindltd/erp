using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.SalesManagment;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.SalesManagment
{
    public class PMSalesTechnicalInfoDA :BaseService
    {

        public List<PMSalesTechnicalInfoBO> GetAllPMSalesTechnicalInfo()
        {
            List<PMSalesTechnicalInfoBO> list = new List<PMSalesTechnicalInfoBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllPMSalesTechnicalInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                PMSalesTechnicalInfoBO technicalBO = new PMSalesTechnicalInfoBO();
                                technicalBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                technicalBO.CustomerId = Convert.ToInt32(reader["CustomerId"]);
                                technicalBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                                technicalBO.TechnicalContactPerson = Convert.ToString(reader["TechnicalContactPerson"]);
                                technicalBO.TechnicalPersonDepartment = Convert.ToString(reader["TechnicalPersonDepartment"]);
                                technicalBO.TechnicalPersonDesignation = Convert.ToString(reader["TechnicalPersonDesignation"]);
                                technicalBO.TechnicalInfoId = Convert.ToInt32(reader["TechnicalInfoId"]);

                                technicalBO.TechnicalPersonEmail = reader["TechnicalPersonEmail"].ToString();
                                technicalBO.TechnicalPersonPhone = reader["TechnicalPersonPhone"].ToString();
                                list.Add(technicalBO);
                            }
                        }
                    }
                }
            }
            return list;
        }

        public PMSalesTechnicalInfoBO GetPMSalesTechnicalInfoByTechnicalInfoId(int TechnicalInfoId)
        {
            PMSalesTechnicalInfoBO technicalBO = new PMSalesTechnicalInfoBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPMSalesTechnicalInfoByTechnicalInfoId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@TechnicalInfoId", DbType.Int32, TechnicalInfoId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                technicalBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"]);
                                technicalBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                                technicalBO.TechnicalContactPerson = Convert.ToString(reader["TechnicalContactPerson"]);
                                technicalBO.TechnicalPersonDepartment = Convert.ToString(reader["TechnicalPersonDepartment"]);
                                technicalBO.TechnicalPersonDesignation = Convert.ToString(reader["TechnicalPersonDesignation"]);
                                technicalBO.TechnicalInfoId = Convert.ToInt32(reader["TechnicalInfoId"]);

                                technicalBO.TechnicalPersonEmail = reader["TechnicalPersonEmail"].ToString();
                                technicalBO.TechnicalPersonPhone = reader["TechnicalPersonPhone"].ToString();
                            }
                        }
                    }
                }
            }
            return technicalBO;
        }


        public bool SavePMSalesTechnicalInfo(PMSalesTechnicalInfoBO technicalBO, out int tmpCustomerId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePMSalesTechnicalInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@CustomerId", DbType.Int32, technicalBO.CustomerId);
                    dbSmartAspects.AddInParameter(command, "@TechnicalContactPerson", DbType.String, technicalBO.TechnicalContactPerson);
                    dbSmartAspects.AddInParameter(command, "@TechnicalPersonDepartment", DbType.String, technicalBO.TechnicalPersonDepartment);
                    dbSmartAspects.AddInParameter(command, "@TechnicalPersonDesignation", DbType.String, technicalBO.TechnicalPersonDesignation);
                    dbSmartAspects.AddInParameter(command, "@TechnicalPersonPhone", DbType.String, technicalBO.TechnicalPersonPhone);
                    dbSmartAspects.AddInParameter(command, "@TechnicalPersonEmail", DbType.String, technicalBO.TechnicalPersonEmail);

                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, technicalBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@TechnicalInfoId", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    tmpCustomerId = Convert.ToInt32(command.Parameters["@TechnicalInfoId"].Value);
                }
            }
            return status;
        }

        public bool UpdatePMSalesTechnicalInfo(PMSalesTechnicalInfoBO technicalBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePMSalesTechnicalInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@TechnicalInfoId", DbType.Int32, technicalBO.TechnicalInfoId);
                    dbSmartAspects.AddInParameter(command, "@CustomerId", DbType.Int32, technicalBO.CustomerId);
                    dbSmartAspects.AddInParameter(command, "@TechnicalContactPerson", DbType.String, technicalBO.TechnicalContactPerson);
                    dbSmartAspects.AddInParameter(command, "@TechnicalPersonDepartment", DbType.String, technicalBO.TechnicalPersonDepartment);
                    dbSmartAspects.AddInParameter(command, "@TechnicalPersonDesignation", DbType.String, technicalBO.TechnicalPersonDesignation);
                    dbSmartAspects.AddInParameter(command, "@TechnicalPersonPhone", DbType.String, technicalBO.TechnicalPersonPhone);
                    dbSmartAspects.AddInParameter(command, "@TechnicalPersonEmail", DbType.String, technicalBO.TechnicalPersonEmail);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, technicalBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
