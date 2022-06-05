using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Inventory;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Inventory
{
    public class InvItemDetailsDA : BaseService
    {
        public List<InvItemDetailsBO> GetInvItemDetailsByItemId(int itemId)
        {
            List<InvItemDetailsBO> detailList = new List<InvItemDetailsBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemDetailsByItemId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemDetailsBO detail = new InvItemDetailsBO();

                                detail.DetailId = Convert.ToInt32(reader["DetailId"]);
                                detail.ItemId = Convert.ToInt32(reader["ItemId"]);
                                detail.ItemDetailId = Convert.ToInt32(reader["ItemDetailId"]);
                                detail.ItemName = reader["ItemName"].ToString();
                                detail.ItemUnit = Convert.ToDecimal(reader["ItemUnit"]);

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
