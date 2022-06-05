using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Data.HMCommon
{
    public class CommonProfessionDA : BaseService
    {
        public List<CommonProfessionBO> GetProfessionInfo()
        {
            List<CommonProfessionBO> entityBOList = new List<CommonProfessionBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetProfessionInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                CommonProfessionBO entityBO = new CommonProfessionBO();

                                entityBO.ProfessionId = Convert.ToInt32(reader["ProfessionId"]);
                                entityBO.ProfessionName = reader["ProfessionName"].ToString();
                                entityBO.ProfessionCode = reader["ProfessionCode"].ToString();

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
    }
}
