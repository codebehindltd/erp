using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.HotelManagement;

namespace HotelManagement.Data.HotelManagement
{
    public class GuestBillSplitDA : BaseService
    {
        public List<GuestServiceBillApprovedBO> GetGuestServiceInfoByRegistrationId(string registrationId)
        {
            List<GuestServiceBillApprovedBO> roomNumberList = new List<GuestServiceBillApprovedBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestServiceInfoByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestServiceBillApprovedBO roomNumber = new GuestServiceBillApprovedBO();

                                roomNumber.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                                roomNumber.ServiceName = reader["ServiceName"].ToString();
                                roomNumber.ServiceTotalAmount = Convert.ToDecimal(reader["ServiceTotalAmount"]);
                                roomNumber.ServiceType = reader["ServiceType"].ToString();
                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public List<GuestServiceBillApprovedBO> GetGuestIndividualServiceInfoByRegistrationId(string registrationId)
        {
            List<GuestServiceBillApprovedBO> roomNumberList = new List<GuestServiceBillApprovedBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestIndividualServiceInfoByRegistrationId_SP"))
                {
                    cmd.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SqlCommandTimeOut"]);
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                GuestServiceBillApprovedBO roomNumber = new GuestServiceBillApprovedBO();

                                roomNumber.ApprovedId = Convert.ToInt64(reader["ApprovedId"]);
                                //roomNumber.ServiceDate = Convert.ToDateTime(reader["ServiceDate"]);
                                roomNumber.ServiceName = reader["ServiceName"].ToString();
                                roomNumber.ServiceTotalAmount = Convert.ToDecimal(reader["ServiceTotalAmount"]);
                                roomNumber.ServiceType = reader["ServiceType"].ToString();
                                roomNumberList.Add(roomNumber);
                            }
                        }
                    }
                }
            }
            return roomNumberList;
        }
        public GuestServiceBillApprovedBO GetGuestServiceTotalAmountInfo(string registrationId, string roomIdList, string serviceIdList, string StartDate, string EndDate)
        {
            GuestServiceBillApprovedBO serviceInfoBO = new GuestServiceBillApprovedBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetGuestServiceTotalAmountInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.String, registrationId);
                    dbSmartAspects.AddInParameter(cmd, "@RoomIdList", DbType.String, roomIdList);
                    dbSmartAspects.AddInParameter(cmd, "@ServiceIdList", DbType.String, serviceIdList);
                    dbSmartAspects.AddInParameter(cmd, "@StartDate", DbType.String, StartDate);
                    dbSmartAspects.AddInParameter(cmd, "@EndDate", DbType.String, EndDate);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {

                                serviceInfoBO.ServiceTotalAmount = Convert.ToDecimal(reader["ServiceTotalAmount"]);
                            }
                        }
                    }
                }
            }
            return serviceInfoBO;
        }
        public bool SaveORUpdateBillSpliteInformation(GuestBillSplitBO splitBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveORUpdateBillSpliteInformation_SP"))
                {
                    command.Parameters.Clear();
                    dbSmartAspects.AddInParameter(command, "@RegistrationId", DbType.Int32, splitBO.RegistrationId);
                    dbSmartAspects.AddInParameter(command, "@RoomValue", DbType.Int32, splitBO.RoomValue);
                    dbSmartAspects.AddInParameter(command, "@RoomBillIsCompany", DbType.Boolean, splitBO.RoomBillIsCompany);
                    dbSmartAspects.AddInParameter(command, "@ServiceValue", DbType.String, splitBO.ServiceValue);
                    dbSmartAspects.AddInParameter(command, "@ServiceBillIsCompany", DbType.Boolean, splitBO.ServiceBillIsCompany);
                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
    }
}
