using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.GeneralLedger;

namespace HotelManagement.Data.GeneralLedger
{
   public class GLCommonSetupDA :BaseService
    {
        public bool SaveOrUpdateGLCommonSetup(GLCommonSetupBO setupBO, out int tmpSetupId)
        {
            bool status = false;
            tmpSetupId = 0;

            if (setupBO.SetupId == 0 || setupBO.SetupId.ToString() == "")
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveGLCommonSetup_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@SetupName", DbType.String, setupBO.SetupName);
                        dbSmartAspects.AddInParameter(command, "@TypeName", DbType.String, setupBO.TypeName);
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, setupBO.ProjectId);
                        dbSmartAspects.AddInParameter(command, "@SetupValue", DbType.String, setupBO.SetupValue);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, setupBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@SetupId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpSetupId = Convert.ToInt32(command.Parameters["@SetupId"].Value);
                    }
                }
            }
            else
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateGLCommonSetup_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@SetupId", DbType.Int32, setupBO.SetupId);
                        dbSmartAspects.AddInParameter(command, "@SetupName", DbType.String, setupBO.SetupName);
                        dbSmartAspects.AddInParameter(command, "@ProjectId", DbType.String, setupBO.ProjectId);
                        dbSmartAspects.AddInParameter(command, "@TypeName", DbType.String, setupBO.TypeName);
                        dbSmartAspects.AddInParameter(command, "@SetupValue", DbType.String, setupBO.SetupValue);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, setupBO.LastModifiedBy);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }
        public GLCommonSetupBO GetCommonSetupInfoById(int projectId,string SetupName)
        {
            GLCommonSetupBO setupBo = new GLCommonSetupBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCommonSetupInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ProjectId", DbType.String, projectId);
                    dbSmartAspects.AddInParameter(cmd, "@SetupName", DbType.String, SetupName);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                setupBo.SetupId = Convert.ToInt32(reader["SetupId"]);
                                setupBo.ProjectId = Convert.ToInt32(reader["ProjectId"]);
                                setupBo.TypeName = reader["TypeName"].ToString();
                                setupBo.SetupName = reader["SetupName"].ToString();
                                setupBo.SetupValue = reader["SetupValue"].ToString();
                            }
                        }
                    }
                }
            }
            return setupBo;
        }
        public List<GLNotesConfigurationBO> GetNotesNumberInfo(int isConfigurable)
        {
            List<GLNotesConfigurationBO> entityList = new List<GLNotesConfigurationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetNotesNumberInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@IsConfigurable", DbType.Int32, isConfigurable);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GLNotesConfigurationBO entityBO = new GLNotesConfigurationBO();
                                entityBO.NotesNumber = reader["NotesNumber"].ToString();
                                entityList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityList;
        }
    }
}
