using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Data.HMCommon
{
   public class CommonCurrencyConversionDA :BaseService
    {
       public CommonCurrencyConversionBO GetCurrencyConversionInfoByCurrencyId(int FromConversionHeadId, int ToConversionHeadId)
       {
           CommonCurrencyConversionBO commonSetupBO = new CommonCurrencyConversionBO();

           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCurrencyConversionInfoByCurrencyId_SP"))
               {
                   dbSmartAspects.AddInParameter(cmd, "@FromCurrencyId", DbType.Int32, FromConversionHeadId);
                   dbSmartAspects.AddInParameter(cmd, "@ToCurrencyId", DbType.Int32, ToConversionHeadId);
                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               commonSetupBO.ConversionRate = Convert.ToDecimal(reader["ConversionRate"].ToString());
                               commonSetupBO.ConversionId = Convert.ToInt32(reader["ConversionId"].ToString());
                               commonSetupBO.FromCurrencyId = Convert.ToInt32(reader["FromCurrencyId"].ToString());
                               commonSetupBO.ToCurrencyId = Convert.ToInt32(reader["ToCurrencyId"].ToString());
                           }
                       }
                   }
               }
           }
           return commonSetupBO;
       }
       public CommonCurrencyConversionBO GetCommonCurrencyConversionInfoById(int ConversionId)
       {
           CommonCurrencyConversionBO commonSetupBO = new CommonCurrencyConversionBO();

           using (DbConnection conn = dbSmartAspects.CreateConnection())
           {
               using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetCommonCurrencyConversionInfoById_SP"))
               {
                   dbSmartAspects.AddInParameter(cmd, "@ConversionId", DbType.Int32, ConversionId);

                   using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                   {
                       if (reader != null)
                       {
                           while (reader.Read())
                           {
                               commonSetupBO.ConversionRate = Convert.ToDecimal(reader["ConversionRate"].ToString());
                               commonSetupBO.ConversionId = Convert.ToInt32(reader["ConversionId"].ToString());
                               commonSetupBO.FromCurrencyId = Convert.ToInt32(reader["FromCurrencyId"].ToString());
                               commonSetupBO.ToCurrencyId = Convert.ToInt32(reader["ToCurrencyId"].ToString());
                           }
                       }
                   }
               }
           }
           return commonSetupBO;
       }
       public bool SaveCommonCurrencyConversion(CommonCurrencyConversionBO commonSetupBO, out int tmpConversionId)
       {
           bool status = false;
           tmpConversionId = 0;

           if (commonSetupBO.ConversionId == 0 || commonSetupBO.ConversionId.ToString() == "")
           {
               using (DbConnection conn = dbSmartAspects.CreateConnection())
               {
                   using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveCommonCurrencyConversion_SP"))
                   {
                       dbSmartAspects.AddInParameter(command, "@FromCurrencyId", DbType.Int32, commonSetupBO.FromCurrencyId);
                       dbSmartAspects.AddInParameter(command, "@ToCurrencyId", DbType.Int32, commonSetupBO.ToCurrencyId);
                       dbSmartAspects.AddInParameter(command, "@ConversionRate", DbType.Decimal, commonSetupBO.ConversionRate);
                       dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, commonSetupBO.ActiveStat);
                       dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, commonSetupBO.CreatedBy);
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
                   using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateCurrencyConversion_SP"))
                   {
                       dbSmartAspects.AddInParameter(command, "@ConversionId", DbType.Int32, commonSetupBO.ConversionId);
                       dbSmartAspects.AddInParameter(command, "@FromCurrencyId", DbType.Int32, commonSetupBO.FromCurrencyId);
                       dbSmartAspects.AddInParameter(command, "@ToCurrencyId", DbType.Int32, commonSetupBO.ToCurrencyId);
                       dbSmartAspects.AddInParameter(command, "@ConversionRate", DbType.Decimal, commonSetupBO.ConversionRate);
                       dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, commonSetupBO.ActiveStat);
                       dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, commonSetupBO.LastModifiedBy);
                       status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                   }
               }
           }
           return status;
       }


    }
}
