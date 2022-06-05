using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HMCommon
{
    public class SalaryMonthStartDateDA : BaseService
    {
        public SalaryMonthStartDateBO GetSalaryMonthStartDateInfo()
        {
            SalaryMonthStartDateBO entityBO = new SalaryMonthStartDateBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalaryMonthStartDateInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.StartDateId = Convert.ToInt32(reader["StartDateId"]);
                                entityBO.StartDate = Convert.ToInt32(reader["StartDate"]);
                            }
                        }
                    }
                }
            }
            return entityBO;
        }
        public bool UpdateSalaryMonthStartDateInfo(SalaryMonthStartDateBO entityBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSalaryMonthStartDateInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@StartDateId", DbType.Int32, entityBO.StartDateId);
                    dbSmartAspects.AddInParameter(command, "@StartDate", DbType.Int32, entityBO.StartDate);
                    
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, entityBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public bool SaveSalaryMonthStartDateInfo(SalaryMonthStartDateBO entityBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveSalaryMonthStartDateInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@StartDate", DbType.Int32, entityBO.StartDate);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, entityBO.CreatedBy);

                    dbSmartAspects.AddOutParameter(command, "@StartDateId", DbType.Int32, sizeof(Int32));
                    
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
