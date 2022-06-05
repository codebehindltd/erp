using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HMCommon
{
   public class EmpOverTimeSetupDA: BaseService
   {
       public EmpOverTimeSetupBO GetEmpOverTimeSetupInfo()
       {
           EmpOverTimeSetupBO OverTime = new EmpOverTimeSetupBO();
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpOverTimeSetupInfo_SP"))
               {
                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               OverTime.OverTimeSetupId = Convert.ToInt32(reader["OverTimeSetupId"]);
                               OverTime.SalaryHeadId = Convert.ToInt32(reader["SalaryHeadId"]);
                               OverTime.MonthlyTotalHour = Convert.ToDecimal(reader["MonthlyTotalHour"]);
                           }
                       }
                   }
               }
           }
           return OverTime;
       }
       public bool SaveEmpOverTimeSetupInfo(EmpOverTimeSetupBO overTimeSetupBO, out int tmpCompanyId)
       {
           Boolean status = false;
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpOverTimeSetupInfo_SP"))
               {
                   dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, overTimeSetupBO.SalaryHeadId);
                   dbSmartAspects.AddInParameter(command, "@MonthlyTotalHour", DbType.Decimal, overTimeSetupBO.MonthlyTotalHour);
                   dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, overTimeSetupBO.CreatedBy);
                   dbSmartAspects.AddOutParameter(command, "@OverTimeSetupId", DbType.Int32, sizeof(Int32));

                   status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                   tmpCompanyId = Convert.ToInt32(command.Parameters["@OverTimeSetupId"].Value);
               }
           }
           return status;
       }
       public bool UpdateEmpOverTimeSetupInfo(EmpOverTimeSetupBO empOverTimeSetupBO)
       {
           Boolean status = false;
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpOverTimeSetupInfo_SP"))
               {
                   dbSmartAspects.AddInParameter(command, "@OverTimeSetupId", DbType.Int32, empOverTimeSetupBO.OverTimeSetupId);
                   dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, empOverTimeSetupBO.SalaryHeadId);
                   dbSmartAspects.AddInParameter(command, "@MonthlyTotalHour", DbType.Decimal, empOverTimeSetupBO.MonthlyTotalHour);
                   dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, empOverTimeSetupBO.LastModifiedBy);
                   status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
               }
           }
           return status;
       }
   }
}
