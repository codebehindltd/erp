using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.LCManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.LCManagement
{
    public class OverHeadNameDA : BaseService
    {
        public List<OverHeadNameBO> GetLCOverHeadNameInfo()
        {
            List<OverHeadNameBO> guestHouseServiceList = new List<OverHeadNameBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCOverHeadNameInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                OverHeadNameBO guestHouseService = new OverHeadNameBO();

                                guestHouseService.OverHeadId = Convert.ToInt32(reader["OverHeadId"]);
                                guestHouseService.OverHeadName = reader["OverHeadName"].ToString();
                                guestHouseService.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                guestHouseService.ActiveStatus = reader["ActiveStatus"].ToString();

                                guestHouseServiceList.Add(guestHouseService);
                            }
                        }
                    }
                }
            }
            return guestHouseServiceList;
        }
        public List<OverHeadNameBO> GetActiveLCOverHeadNameInfo()
        {
            List<OverHeadNameBO> guestHouseServiceList = new List<OverHeadNameBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveLCOverHeadNameInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                OverHeadNameBO guestHouseService = new OverHeadNameBO();

                                guestHouseService.OverHeadId = Convert.ToInt32(reader["OverHeadId"]);
                                guestHouseService.OverHeadName = reader["OverHeadName"].ToString();
                                guestHouseService.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                guestHouseService.ActiveStatus = reader["ActiveStatus"].ToString();
                                //guestHouseService.VatAmount = Convert.ToDecimal(reader["VatAmount"]);
                                //guestHouseService.ServiceCharge = Convert.ToDecimal(reader["ServiceCharge"]);

                                guestHouseServiceList.Add(guestHouseService);
                            }
                        }
                    }
                }
            }
            return guestHouseServiceList;
        }
        public Boolean SaveLCOverHeadNameInfo(OverHeadNameBO serviceBO, out int tmpserviceId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateLCOverHeadNameInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@OverHeadId", DbType.String, serviceBO.OverHeadId);
                    dbSmartAspects.AddInParameter(command, "@OverHeadName", DbType.String, serviceBO.OverHeadName);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, serviceBO.Description);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int32, serviceBO.NodeId);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, serviceBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@IsCNFHead", DbType.Boolean, serviceBO.IsCNFHead);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, serviceBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tmpserviceId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                }
            }
            return status;
        }
        public Boolean UpdateLCOverHeadNameInfo(OverHeadNameBO serviceBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateLCOverHeadNameInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@OverHeadId", DbType.Int32, serviceBO.OverHeadId);
                    dbSmartAspects.AddInParameter(command, "@OverHeadName", DbType.String, serviceBO.OverHeadName);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, serviceBO.Description);
                    dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int32, serviceBO.NodeId);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, serviceBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, serviceBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        public OverHeadNameBO GetLCOverHeadNameInfoById(int serviceId)
        {
            OverHeadNameBO guestHouseService = new OverHeadNameBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCOverHeadNameInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@OverHeadId", DbType.Int32, serviceId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                guestHouseService.OverHeadId = Convert.ToInt32(reader["OverHeadId"]);
                                guestHouseService.OverHeadName = reader["OverHeadName"].ToString();
                                guestHouseService.NodeId = Int32.Parse(reader["NodeId"].ToString());
                                guestHouseService.Description = reader["Description"].ToString();
                                guestHouseService.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                guestHouseService.IsCNFHead = Convert.ToBoolean(reader["IsCNFHead"]);
                                guestHouseService.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return guestHouseService;
        }
        public List<OverHeadNameBO> GetLCOverHeadNameInfoBySearchCriteria(string overHeadName, bool activeStat)
        {
            List<OverHeadNameBO> guestHouseServiceList = new List<OverHeadNameBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCOverHeadNameInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@OverHeadName", DbType.String, overHeadName);
                    dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, activeStat);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                OverHeadNameBO guestHouseService = new OverHeadNameBO();

                                guestHouseService.OverHeadId = Convert.ToInt32(reader["OverHeadId"]);
                                guestHouseService.OverHeadName = reader["OverHeadName"].ToString();
                                guestHouseService.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                guestHouseService.ActiveStatus = reader["ActiveStatus"].ToString();

                                guestHouseServiceList.Add(guestHouseService);
                            }
                        }
                    }
                }
            }
            return guestHouseServiceList;
        }
        public OverHeadNameBO GetGuestHouseServiceInfoDetailsById(int serviceId)
        {
            OverHeadNameBO guestHouseService = new OverHeadNameBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLCOverHeadNameInfoDetailsById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ServiceId", DbType.Int32, serviceId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                guestHouseService.OverHeadId = Convert.ToInt32(reader["OverHeadId"]);
                                guestHouseService.OverHeadName = reader["OverHeadName"].ToString();
                                guestHouseService.Description = reader["Description"].ToString();
                                guestHouseService.NodeId = Int32.Parse(reader["NodeId"].ToString());
                                guestHouseService.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                guestHouseService.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return guestHouseService;
        }
        public List<OverHeadNameBO> GetOverHeadInfoBySearchCriteriaForPagination(string serviceName, string serviceType, string activeStat, string IsCNFHead, int recordPerPage, int pageIndex, out int totalRecords)
        {
            bool actStat;
            if (activeStat == "0")
            {
                actStat = true;
            }
            else
                actStat = false;
            if (serviceType == "PS")
            {
                serviceType = string.Empty;
            }
            string Where = GenarateWhereCondition(serviceName, serviceType, actStat);
            List<OverHeadNameBO> paidServiceList = new List<OverHeadNameBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {

                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetOverHeadInfoBySearchCriteriaForPagination_SP"))
                {
                    //GetPaidServiceInfoBySearchCriteriaForPagination_SP
                    //dbSmartAspects.AddInParameter(cmd, "@ServiceName", DbType.String, serviceName);
                    //dbSmartAspects.AddInParameter(cmd, "@ServiceType", DbType.String, serviceType);
                    //dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.String, actStat);
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                OverHeadNameBO paidServiceBO = new OverHeadNameBO();
                                paidServiceBO.OverHeadId = Convert.ToInt32(reader["OverHeadId"]);
                                paidServiceBO.OverHeadName = reader["OverHeadName"].ToString();
                                paidServiceBO.Description = reader["Description"].ToString();

                                paidServiceBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                paidServiceBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                string CnfHead = Convert.ToBoolean(reader["IsCNFHead"]) == true ? "1" : "0";
                                if (IsCNFHead == "All" || (CnfHead) == IsCNFHead)
                                {
                                    paidServiceList.Add(paidServiceBO);
                                }
                            }
                        }
                    }
                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return paidServiceList;
        }
        private string GenarateWhereCondition(string serviceName, string serviceType, bool activeStat)
        {

            string Where = string.Empty;
            if (!string.IsNullOrEmpty(serviceName.ToString()))
            {

                Where += "  OverHeadName LIKE \'%" + serviceName + "%\' AND ActiveStat = '" + activeStat + "'";
            }
            else
            {
                if (!string.IsNullOrEmpty(serviceType.ToString()))
                {
                    Where += " ActiveStat = '" + activeStat + "'";

                }
                else
                    Where += " ActiveStat = '" + activeStat + "'";
            }

            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where;
            }
            return Where;
        }
    }
}
