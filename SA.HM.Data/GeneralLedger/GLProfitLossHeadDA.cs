using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.GeneralLedger;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.GeneralLedger
{
   public class GLProfitLossHeadDA :BaseService
   {
       public List<GLProfitLossHeadBO> GetLossProfitHeadInfo()
       {
           List<GLProfitLossHeadBO> entityBOList = new List<GLProfitLossHeadBO>();
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLProfitLossHeadInfo_SP"))
               {
                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               GLProfitLossHeadBO entityBO = new GLProfitLossHeadBO();
                               entityBO.PLHeadId = Convert.ToInt32(reader["PLHeadId"]);
                               entityBO.PLHead = reader["PLHead"].ToString();
                               entityBO.CalculationMode = (reader["CalculationMode"]).ToString();

                               entityBO.NotesNumber = reader["NotesNumber"].ToString();
                               entityBO.PLHeadWithNotes = (reader["PLHeadWithNotes"]).ToString();

                               entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                               entityBOList.Add(entityBO);
                           }
                       }
                   }
               }
           }
           return entityBOList;
       }
       public GLProfitLossHeadBO GetGLProfitLossHeadInfoByNodeId(int nodeId)
       {
           GLProfitLossHeadBO entityBO = new GLProfitLossHeadBO();
           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGLProfitLossHeadInfoByNodeId_SP"))
               {
                   dbSmartAspects.AddInParameter(cmd, "@NodeId", DbType.Int32, nodeId);

                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               entityBO.PLHeadId = Convert.ToInt32(reader["PLHeadId"]);
                               entityBO.PLHead = reader["PLHead"].ToString();
                               entityBO.CalculationMode = (reader["CalculationMode"]).ToString();
                               entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                               entityBO.PLSetupId = Convert.ToInt32(reader["PLSetupId"]);                               
                           }
                       }
                   }
               }
           }
           return entityBO;
       }
   }
}
