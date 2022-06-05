using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using HotelManagement.Entity;
using HotelManagement.Entity.Payroll;

namespace HotelManagement.Data.Payroll
{
    public class PayrollEmpStatusHistoryDA:BaseService
    {
        public bool SavePayrollEmpStatusHistory(PayrollEmpStatusHistoryBO EmpStatus)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SavePayrollEmpStatusHistory_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@EmpId", DbType.Int64, EmpStatus.EmpId);
                        dbSmartAspects.AddInParameter(command, "@EmpStatusId", DbType.Int32, EmpStatus.EmpStatusId);
                        dbSmartAspects.AddInParameter(command, "@ActionDate", DbType.DateTime, EmpStatus.ActionDate);
                        dbSmartAspects.AddInParameter(command, "@EffectiveDate", DbType.DateTime, EmpStatus.EffectiveDate);
                        dbSmartAspects.AddInParameter(command, "@Reason", DbType.String, EmpStatus.Reason);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, EmpStatus.CreatedBy);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    }
                }
            }
            catch(Exception ex )
            {
                throw ex;
            }
             return status;
        }
    }
}
