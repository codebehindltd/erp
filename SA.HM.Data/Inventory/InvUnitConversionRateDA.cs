using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Inventory;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Inventory
{
    public class InvUnitConversionRateDA:BaseService
    {
        public bool SaveOrUpdateUnitConversionRate(InvUnitConversionRateBO unitConversuionBO, out int tmpConversionId)
        {
            bool status = false;
            tmpConversionId = 0;

            if (unitConversuionBO.ConversionId == 0 || unitConversuionBO.ConversionId.ToString() == "")
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveUnitConversionRate_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@FromUnitHeadId", DbType.Int32, unitConversuionBO.FromUnitHeadId);
                        dbSmartAspects.AddInParameter(command, "@ToUnitHeadId", DbType.Int32, unitConversuionBO.ToUnitHeadId);
                        dbSmartAspects.AddInParameter(command, "@ConversionRate", DbType.Decimal, unitConversuionBO.ConversionRate);                        
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, unitConversuionBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@ConversionId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                        tmpConversionId = Convert.ToInt32(command.Parameters["@ConversionId"].Value);
                    }
                }
            }
            else
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateUnitConversionRate_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ConversionId", DbType.Int32, unitConversuionBO.ConversionId);
                        dbSmartAspects.AddInParameter(command, "@FromUnitHeadId", DbType.Int32, unitConversuionBO.FromUnitHeadId);
                        dbSmartAspects.AddInParameter(command, "@ToUnitHeadId", DbType.Int32, unitConversuionBO.ToUnitHeadId);
                        dbSmartAspects.AddInParameter(command, "@ConversionRate", DbType.Decimal, unitConversuionBO.ConversionRate);                        
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, unitConversuionBO.LastModifiedBy);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            return status;
        }
        public InvUnitConversionRateBO GetUnitConversionRateInfoByHeadId(int FromConversionHeadId, int ToConversionHeadId)
        {
            InvUnitConversionRateBO commonSetupBO = new InvUnitConversionRateBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetUnitConversionRateInfoByHeadId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FromConversionHeadId", DbType.Int32, FromConversionHeadId);
                    dbSmartAspects.AddInParameter(cmd, "@ToConversionHeadId", DbType.Int32, ToConversionHeadId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                commonSetupBO.ConversionRate = Convert.ToDecimal(reader["ConversionRate"].ToString());
                                commonSetupBO.ConversionId = Convert.ToInt32(reader["ConversionId"].ToString());
                                commonSetupBO.FromUnitHeadId = Convert.ToInt32(reader["FromUnitHeadId"].ToString());
                                commonSetupBO.ToUnitHeadId = Convert.ToInt32(reader["ToUnitHeadId"].ToString());
                            }
                        }
                    }
                }
            }
            return commonSetupBO;
        }
    }
}
