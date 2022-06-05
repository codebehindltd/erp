using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using HotelManagement.Entity.Payroll;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class HolidayDA : BaseService
    {
        public List<HolidayBO> GetHolidayInfoByDateRange(DateTime fromDate, DateTime toDate)
        {
            List<HolidayBO> boList = new List<HolidayBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetHolidayInfoByDateRange_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                    dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, toDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HolidayBO bo = new HolidayBO();

                                bo.HolidayId = Convert.ToInt32(reader["HolidayId"]);
                                bo.HolidayDate = Convert.ToDateTime(reader["HolidayDate"]);
                                bo.Description = reader["Description"].ToString();

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
