using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class HotelRegPaidServiceDA: BaseService
    {
        public bool UpdateRegistrationServiceInfo(RegistrationServiceInfoBO hrpaidServiceBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRegistrationServiceInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@DetailPaidServiceId", DbType.Int32, hrpaidServiceBO.DetailServiceId);
                    dbSmartAspects.AddInParameter(command, "@IsAchieved", DbType.Boolean, hrpaidServiceBO.IsAchieved);                    

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
