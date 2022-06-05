using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Restaurant
{
   public class RestaurantTableStatusDA :BaseService
    {

       public List<RestaurantTableStatusBO> GetTableStatusInfo()
        {
      
            List<RestaurantTableStatusBO> statusList = new List<RestaurantTableStatusBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetTableStatusInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantTableStatusBO tableStatusBO = new RestaurantTableStatusBO();
                                tableStatusBO.StatusId = Convert.ToInt32(reader["StatusId"]);
                                tableStatusBO.StatusName = reader["StatusName"].ToString();
                                tableStatusBO.Remarks = reader["Remarks"].ToString();
                                statusList.Add(tableStatusBO);
                            }
                        }
                    }
                }
            }
            return statusList;    

        }
    }
}
