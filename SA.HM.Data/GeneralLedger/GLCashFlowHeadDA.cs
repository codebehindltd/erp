using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.GeneralLedger;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.GeneralLedger
{
    public class GLCashFlowHeadDA : BaseService
    {
        public List<GLCashFlowHeadBO> GetGLCashFlowHeadInfo()
        {
            List<GLCashFlowHeadBO> entityBOList = new List<GLCashFlowHeadBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLCashFlowHeadInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLCashFlowHeadBO entityBO = new GLCashFlowHeadBO();

                                entityBO.HeadId = Convert.ToInt32(reader["HeadId"]);
                                entityBO.CashFlowHead = reader["CashFlowHead"].ToString();
                                entityBO.GroupId = Convert.ToInt32(reader["GroupId"]);
                                entityBO.GroupHead = reader["GroupHead"].ToString();

                                entityBO.NotesNumber = reader["NotesNumber"].ToString();
                                entityBO.CashFlowHeadWithNotes = reader["CashFlowHeadWithNotes"].ToString();

                                entityBO.CashFlowHeadWithGroup = reader["CashFlowHeadWithGroup"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public GLCashFlowHeadBO GetGLCashFlowHeadInfoByNodeId(int nodeId)
        {
            GLCashFlowHeadBO entityBO = new GLCashFlowHeadBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLCashFlowHeadInfoByNodeId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@NodeId", DbType.Int32, nodeId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.HeadId = Convert.ToInt32(reader["HeadId"]);
                                entityBO.CashFlowHead = reader["CashFlowHead"].ToString();
                                entityBO.GroupId = Convert.ToInt32(reader["GroupId"]);
                                entityBO.GroupHead = reader["GroupHead"].ToString();
                                entityBO.CashFlowHeadWithGroup = reader["CashFlowHeadWithGroup"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBO.CFSetupId = Convert.ToInt32(reader["CFSetupId"]);
                            }
                        }
                    }
                }
            }
            return entityBO;
        }
    }
}
