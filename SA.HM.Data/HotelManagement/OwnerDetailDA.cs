using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class OwnerDetailDA : BaseService
    {
        public List<OwnerDetailBO> GetOwnerDetailByOwnerId(int ownerId)
        {
            List<OwnerDetailBO> detailList = new List<OwnerDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOwnerDetailByOwnerId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@OwnerId", DbType.Int32, ownerId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                OwnerDetailBO detail = new OwnerDetailBO();

                                detail.DetailId = Convert.ToInt32(reader["DetailId"]);
                                detail.OwnerId = Convert.ToInt32(reader["OwnerId"]);                                
                                detail.OwnerName = reader["OwnerName"].ToString();
                                detail.RoomId = Convert.ToInt32(reader["RoomId"]);
                                detail.RoomNumber = reader["RoomNumber"].ToString();
                                detail.RoomType = reader["RoomType"].ToString();
                                detail.CommissionValue = Convert.ToDecimal(reader["CommissionValue"]);

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
