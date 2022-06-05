using HotelManagement.Entity.VehicleManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.VehicleManagement
{
    public class VMOverHeadDA : BaseService
    {
        public Boolean SaveVMOverHeadNameInfo(VMOverHeadBO serviceBO, out int tmpserviceId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateVMOverHeadNameInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@OverHeadId", DbType.String, serviceBO.Id);
                    dbSmartAspects.AddInParameter(command, "@OverHeadName", DbType.String, serviceBO.OverheadName);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, serviceBO.Description);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int32, serviceBO.AccountHeadId);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, serviceBO.Status);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, serviceBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpserviceId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateVMOverHeadNameInfo(VMOverHeadBO serviceBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateLCOverHeadNameInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@OverHeadId", DbType.Int32, serviceBO.Id);
                    dbSmartAspects.AddInParameter(command, "@OverHeadName", DbType.String, serviceBO.OverheadName);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, serviceBO.Description);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int32, serviceBO.AccountHeadId);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, serviceBO.Status);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, serviceBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public VMOverHeadBO GetLCOverHeadNameInfoById(int serviceId)
        {
            VMOverHeadBO guestHouseService = new VMOverHeadBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVMOverHeadNameInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@OverHeadId", DbType.Int32, serviceId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                guestHouseService.Id = Convert.ToInt32(reader["Id"]);
                                guestHouseService.OverheadName = reader["OverheadName"].ToString();
                                guestHouseService.AccountHeadId = Int32.Parse(reader["AccountHeadId"].ToString());
                                guestHouseService.Description = reader["Description"].ToString();
                                guestHouseService.Status = Convert.ToBoolean(reader["Status"]);
                            }
                        }
                    }
                }
            }
            return guestHouseService;
        }
        public List<VMOverHeadBO> GetOverHeadInfoBySearchCriteriaForPagination(string serviceName, bool activeStat, int recordPerPage, int pageIndex, out int totalRecords)
        {            
            List<VMOverHeadBO> paidServiceList = new List<VMOverHeadBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOverHeadInfoBySearchCriteriaForPagination_SP"))
                {
                    //GetPaidServiceInfoBySearchCriteriaForPagination_SP
                    dbSmartAspects.AddInParameter(cmd, "@ServiceName", DbType.String, serviceName);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, activeStat);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                VMOverHeadBO paidServiceBO = new VMOverHeadBO();
                                paidServiceBO.Id = Convert.ToInt32(reader["Id"]);
                                paidServiceBO.OverheadName = reader["OverheadName"].ToString();
                                paidServiceBO.Description = reader["Description"].ToString();
                                paidServiceBO.Status = Convert.ToBoolean(reader["Status"]);
                                paidServiceList.Add(paidServiceBO);
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return paidServiceList;
        }
        public List<VMOverHeadBO> GetVMOverHeadNameInfo()
        {
            List<VMOverHeadBO> guestHouseServiceList = new List<VMOverHeadBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetVMOverHeadNameInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                VMOverHeadBO guestHouseService = new VMOverHeadBO();

                                guestHouseService.Id = Convert.ToInt32(reader["Id"]);
                                guestHouseService.OverheadName = reader["OverheadName"].ToString();
                                guestHouseService.Status = Convert.ToBoolean(reader["Status"]);

                                guestHouseServiceList.Add(guestHouseService);
                            }
                        }
                    }
                }
            }
            return guestHouseServiceList;
        }
    }
}
