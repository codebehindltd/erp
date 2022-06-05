using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
  public  class HMDayCloseDA:BaseService
    {
        public bool GenerateDayClossing(HMDayCloseBO dayCloseBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DayClosingProcess_SP"))
                {
                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(command, "@DayClossingDate", DbType.DateTime, dayCloseBO.DayClossingDate);
                    dbSmartAspects.AddInParameter(command, "@DayCloseProcessValue", DbType.Int32, dayCloseBO.DayClossingModuleId);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, dayCloseBO.CreatedBy);                    
                    dbSmartAspects.AddOutParameter(command, "@mErr", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    status = true;
                }
            }
            return status;
        }
        public bool ProductReceiveAccountsPostingProcess(HMDayCloseBO dayCloseBO, string receiveIdList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ProductReceiveAccountsPostingProcess_SP"))
                {
                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(command, "@DayClossingDate", DbType.DateTime, dayCloseBO.DayClossingDate);
                    dbSmartAspects.AddInParameter(command, "@ReceivedIdList", DbType.String, receiveIdList);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, dayCloseBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@mErr", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    status = true;
                }
            }
            return status;
        }
        public bool PurchaseProductReturnAccountsPostingProcess(HMDayCloseBO dayCloseBO, string receiveIdList)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("ProductReceiveAccountsPostingProcess_SP"))
                {
                    command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(command, "@DayClossingDate", DbType.DateTime, dayCloseBO.DayClossingDate);
                    dbSmartAspects.AddInParameter(command, "@ReceivedIdList", DbType.String, receiveIdList);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, dayCloseBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@mErr", DbType.Int32, sizeof(Int32));
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    status = true;
                }
            }
            return status;
        }
        public Boolean SaveRoomStatusHistoryInfo()
        {
            bool retVal = false;
            int status = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveRoomStatusHistoryInfo_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        if (status > 0)
                        {
                            transction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                }
            }
            return retVal;
        }
    }
}
