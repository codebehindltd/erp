using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
   public class AllowanceDeductionHeadDA: BaseService
    {


       public List<AllowanceDeductionHeadBO> GetAllowanceDeductionHeadInfo()
       {
           List<AllowanceDeductionHeadBO> AllowDeductList = new List<AllowanceDeductionHeadBO>();

           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllowanceDeductionHeadInfo_SP"))
               {
                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               AllowanceDeductionHeadBO headBO = new AllowanceDeductionHeadBO();
                               headBO.AllowDeductId = Convert.ToInt32(reader["AllowDeductId"]);
                               headBO.AllowDeductName = reader["AllowDeductName"].ToString();
                               headBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                               headBO.ActiveStatus = reader["ActiveStatus"].ToString();
                               headBO.AllowDeductType = reader["AllowDeductType"].ToString();
                               headBO.TransactionType = reader["TransactionType"].ToString();
                               headBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                               AllowDeductList.Add(headBO);
                           }
                       }
                   }
               }
           }
           return AllowDeductList;
       }
       public bool SaveAllowanceDeductionHeadInfo(AllowanceDeductionHeadBO headBO, out int tmpUserInfoId)
       {
           Boolean status = false;
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveAllowanceDeductionHeadInfo_SP"))
               {
                   dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, headBO.TransactionType);
                   dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, headBO.ActiveStat);
                   dbSmartAspects.AddInParameter(command, "@AllowDeductName", DbType.String, headBO.AllowDeductName);
                   dbSmartAspects.AddInParameter(command, "@AllowDeductType", DbType.String, headBO.AllowDeductType);
                   dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, headBO.CreatedBy);
                   dbSmartAspects.AddOutParameter(command, "@AllowDeductId", DbType.Int32, sizeof(Int32));
                   status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                   tmpUserInfoId = Convert.ToInt32(command.Parameters["@AllowDeductId"].Value);
               }
           }
           return status;   
       }
       public bool UpdateAllowanceDeductionHeadInfo(AllowanceDeductionHeadBO headBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateAllowanceDeductionHeadInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@AllowDeductId", DbType.Int32, headBO.AllowDeductId);
                    dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, headBO.TransactionType);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, headBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@AllowDeductName", DbType.String, headBO.AllowDeductName);
                    dbSmartAspects.AddInParameter(command, "@AllowDeductType", DbType.String, headBO.AllowDeductType);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, headBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
       public AllowanceDeductionHeadBO GetAllowanceDeductionHeadInfoByID(int EditId)
        {
            AllowanceDeductionHeadBO headBO = new AllowanceDeductionHeadBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetAllowanceDeductionHeadInfoByID_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@AllowDeductId", DbType.Int32, EditId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                headBO.AllowDeductId = Convert.ToInt32(reader["AllowDeductId"]);
                                headBO.AllowDeductName = reader["AllowDeductName"].ToString();
                                headBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                headBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                headBO.AllowDeductType = reader["AllowDeductType"].ToString();
                                headBO.TransactionType = reader["TransactionType"].ToString();
                                headBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                            }
                        }
                    }
                }
            }
            return headBO;
        }
    }
}
