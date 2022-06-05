using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
   public class PayrollHolidayDA :BaseService
   {

       public PayrollHolidayBO GetPayrollHolidayInfoByID(int HolidayId)
       {
           PayrollHolidayBO holiDayBO = new PayrollHolidayBO();
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollHoliDayInfoByID_SP"))
               {
                   dbSmartAspects.AddInParameter(cmd, "@HolidayId", DbType.Int32, HolidayId);

                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               holiDayBO.HolidayId = Convert.ToInt32(reader["HolidayId"]);
                               holiDayBO.HolidayName = Convert.ToString(reader["HolidayName"]);
                               holiDayBO.Description = Convert.ToString(reader["Description"]);
                               holiDayBO.StartDate = Convert.ToDateTime(reader["StartDate"].ToString());
                               holiDayBO.EndDate = Convert.ToDateTime(reader["EndDate"].ToString());
                           }
                       }
                   }
               }
           }
           return holiDayBO;
       }
       public List<PayrollHolidayBO> GetPayrollHolidayInformationBySearchCritaria(DateTime StartDate,DateTime EndDate)
       {
           List<PayrollHolidayBO> List = new List<PayrollHolidayBO>();
           string searchCriteria = string.Empty;
           searchCriteria = GenerateWhereCondition(StartDate, EndDate);

           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetPayrollHolidayInformationBySearchCritaria_SP"))
               {
                   dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, searchCriteria);
                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               PayrollHolidayBO holiDayBO = new PayrollHolidayBO();
                               holiDayBO.HolidayId = Convert.ToInt32(reader["HolidayId"]);
                               holiDayBO.HolidayName = Convert.ToString(reader["HolidayName"]);
                               holiDayBO.Description = Convert.ToString(reader["Description"]);
                               holiDayBO.StartDate = Convert.ToDateTime(reader["StartDate"].ToString());
                               holiDayBO.EndDate = Convert.ToDateTime(reader["EndDate"].ToString());
                               List.Add(holiDayBO);
                           }
                       }
                   }
               }
           }
           return List;
       }
       public bool UpdatePayrollHolidayInfo(PayrollHolidayBO holiDayBO)
       {
           Boolean status = false;
           try
           {
               using (DbConnection conn = dbSmartAspects.CreateConnection())
               {
                   using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdatePayrollHolidayInfo_SP"))
                   {
                       dbSmartAspects.AddInParameter(command, "@HolidayId", DbType.Int32, holiDayBO.HolidayId);
                       dbSmartAspects.AddInParameter(command, "@HolidayName", DbType.String, holiDayBO.HolidayName);
                       dbSmartAspects.AddInParameter(command, "@StartDate", DbType.DateTime, holiDayBO.StartDate);
                       dbSmartAspects.AddInParameter(command, "@EndDate", DbType.DateTime, holiDayBO.EndDate);
                       dbSmartAspects.AddInParameter(command, "@Description", DbType.String, holiDayBO.Description);
                       dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, holiDayBO.LastModifiedBy);
                       status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                   }
               }
           }
           catch(Exception ex)
           {
               throw ex;
           }

           return status;
       }
       public bool SavePayrollHoliDayInfo(PayrollHolidayBO holiDayBO, out int tmpWorkingDayId)
       {
           Boolean status = false;
           try
           {
               using (DbConnection conn = dbSmartAspects.CreateConnection())
               {
                   using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePayrollHoliDayInfo_SP"))
                   {
                       dbSmartAspects.AddInParameter(command, "@HolidayName", DbType.String, holiDayBO.HolidayName);
                       dbSmartAspects.AddInParameter(command, "@StartDate", DbType.DateTime, holiDayBO.StartDate);
                       dbSmartAspects.AddInParameter(command, "@EndDate", DbType.DateTime, holiDayBO.EndDate);
                       dbSmartAspects.AddInParameter(command, "@Description", DbType.String, holiDayBO.Description);
                       dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, holiDayBO.CreatedBy);
                       dbSmartAspects.AddOutParameter(command, "@HolidayId", DbType.Int32, sizeof(Int32));
                       status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                       tmpWorkingDayId = Convert.ToInt32(command.Parameters["@HolidayId"].Value);
                   }
               }
           }
           catch (Exception ex)
            {
               throw ex;
           }
           return status;
       }
       private string GenerateWhereCondition(DateTime FromDate, DateTime ToDate)
       {
           string Where = string.Empty, Condition = string.Empty;

           Condition += "  ( dbo.FnDate(StartDate) >= dbo.FnDate('" + FromDate.ToString("yyyy-MM-dd") + "'";
           Condition += " ) AND dbo.FnDate(StartDate) <= dbo.FnDate('" + ToDate.ToString("yyyy-MM-dd") + "'" + ") )";

           if (!string.IsNullOrEmpty(Condition))
           {
               Where += " WHERE " + Condition;
           }
           return Where;
       }



   }
}
