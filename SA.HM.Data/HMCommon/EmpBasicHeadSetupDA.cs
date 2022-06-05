using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HMCommon;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HMCommon
{
    public class EmpBasicHeadSetupDA : BaseService
    {
        public EmpBasicHeadSetupBO GetEmpBasicHeadSetupInfo()
        {
            EmpBasicHeadSetupBO entityBO = new EmpBasicHeadSetupBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetEmpBasicHeadSetupInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.BasicSetupId = Convert.ToInt32(reader["BasicSetupId"]);
                                entityBO.SalaryHeadId = Convert.ToInt32(reader["SalaryHeadId"]);
                            }
                        }
                    }
                }
            }
            return entityBO;
        }
        public bool SaveEmpBasicHeadSetupInfo(EmpBasicHeadSetupBO entityBO, out int pKId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveEmpBasicHeadSetupInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, entityBO.SalaryHeadId);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, entityBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@BasicSetupId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    pKId = Convert.ToInt32(command.Parameters["@BasicSetupId"].Value);
                }
            }
            return status;
        }
        public bool UpdateEmpBasicHeadSetupInfo(EmpBasicHeadSetupBO entityBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateEmpBasicHeadSetupInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@BasicSetupId", DbType.Int32, entityBO.@BasicSetupId);
                    dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, entityBO.SalaryHeadId);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, entityBO.LastModifiedBy);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
