using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.GeneralLedger;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.GeneralLedger
{
    public class GLBalanceSheetHeadDA : BaseService
    {
        public List<GLBalanceSheetHeadBO> GetGLBalanceSheetHeadInfo()
        {
            List<GLBalanceSheetHeadBO> entityBOList = new List<GLBalanceSheetHeadBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLBalanceSheetHeadInfo_SP"))
                {
                    DataSet ds = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, ds, "BalanceSheetHead");
                    DataTable Table = ds.Tables["BalanceSheetHead"];
                    entityBOList = Table.AsEnumerable().Select(r =>
                                new GLBalanceSheetHeadBO
                                {
                                    RCId = r.Field<long>("RCId"),
                                    NodeId = r.Field<int>("NodeId"),
                                    NodeNumber = r.Field<string>("NodeNumber"),
                                    NodeHead = r.Field<string>("NodeHead")                                    
                                }).ToList();
                }
            }
            return entityBOList;
        }
        public GLBalanceSheetHeadBO GetGLBalanceSheetHeadInfoByNodeId(int nodeId)
        {
            GLBalanceSheetHeadBO entityBO = new GLBalanceSheetHeadBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLBalanceSheetHeadInfoByNodeId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeId", DbType.Int32, nodeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.SetupId = Convert.ToInt32(reader["SetupId"]);
                                entityBO.RCId = Convert.ToInt32(reader["RCId"]);
                                entityBO.NodeNumber = reader["NodeNumber"].ToString();
                                entityBO.NodeHead = reader["NodeHead"].ToString();                                
                            }
                        }
                    }
                }
            }
            return entityBO;
        }
    }
}
