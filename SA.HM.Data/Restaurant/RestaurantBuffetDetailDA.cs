using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantBuffetDetailDA : BaseService
    {
        public List<RestaurantBuffetDetailBO> GetRestaurantBuffetDetailByBuffetId(int buffetId)
        {
            List<RestaurantBuffetDetailBO> detailList = new List<RestaurantBuffetDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantBuffetDetailByBuffetId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@BuffetId", DbType.Int32, buffetId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantBuffetDetailBO detail = new RestaurantBuffetDetailBO();

                                detail.DetailId = Convert.ToInt32(reader["DetailId"]);
                                detail.BuffetId = Convert.ToInt32(reader["BuffetId"]);
                                detail.ProductId = Convert.ToInt32(reader["ProductId"]);
                                detail.ProductName = reader["ProductName"].ToString();

                                detailList.Add(detail);
                            }
                        }
                    }
                }
            }
            return detailList;
        }
    }
}
