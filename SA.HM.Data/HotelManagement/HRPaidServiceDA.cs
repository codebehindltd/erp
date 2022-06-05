using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class HRPaidServiceDA: BaseService
    {
        public List<HRPaidServiceViewBO> GetPaidServiceByRegId(int registrationId)
        {
            List<HRPaidServiceViewBO> psConfirmList = new List<HRPaidServiceViewBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetFOPaidServiceInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HRPaidServiceViewBO psConfirmBO = new HRPaidServiceViewBO();
                                psConfirmBO.Id = Convert.ToInt32(reader["DetailPaidServiceId"].ToString());
                                psConfirmBO.ServiceName = reader["ServiceName"].ToString();
                                psConfirmBO.ServiceType = reader["ServiceType"].ToString();
                                if (psConfirmBO.ServiceType == "PerStay")
                                {
                                    psConfirmBO.ServiceType = "Per Stay";
                                }
                                psConfirmBO.UnitPrice = Convert.ToDecimal(reader["UnitPrice"].ToString());

                                psConfirmList.Add(psConfirmBO);
                            }
                        }
                    }
                }
            }

            return psConfirmList;
        }
    }
}
