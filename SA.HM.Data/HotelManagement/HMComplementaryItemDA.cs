using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.HotelManagement
{
    public class HMComplementaryItemDA : BaseService
    {
        public List<HMComplementaryItemBO> GetActiveHMComplementaryItemInfo()
        {
            List<HMComplementaryItemBO> entityBOList = new List<HMComplementaryItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveHMComplementaryItemInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HMComplementaryItemBO entityBO = new HMComplementaryItemBO();

                                entityBO.ComplementaryItemId = Convert.ToInt32(reader["ComplementaryItemId"]);
                                entityBO.ItemName = reader["ItemName"].ToString();
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBO.IsDefaultItem = Convert.ToBoolean(reader["IsDefaultItem"]);
                                entityBO.DefaultItem = reader["DefaultItem"].ToString();
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public HMComplementaryItemBO GetComplementaryItemInfoById(int itemId)
        {
            HMComplementaryItemBO entityBO = new HMComplementaryItemBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetComplementaryItemInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ComplementaryItemId", DbType.Int32, itemId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.ComplementaryItemId = Convert.ToInt32(reader["ComplementaryItemId"]);
                                entityBO.ItemName = reader["ItemName"].ToString();
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBO.IsDefaultItem = Convert.ToBoolean(reader["IsDefaultItem"]);
                                entityBO.DefaultItem = reader["DefaultItem"].ToString();
                            }
                        }
                    }
                }
            }
            return entityBO;
        }
        public List<HMComplementaryItemBO> GetComplementaryItemInfoBySearchCriteria(string ItemName, bool ActiveStat, int isDefault)
        {
            string Where = GenarateWhereConditionstring(ItemName, ActiveStat, isDefault);
            List<HMComplementaryItemBO> itemList = new List<HMComplementaryItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetComplementaryItemInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HMComplementaryItemBO entityBO = new HMComplementaryItemBO();
                                entityBO.ComplementaryItemId = Convert.ToInt32(reader["ComplementaryItemId"]);
                                entityBO.ItemName = reader["ItemName"].ToString();
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]); 
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBO.DefaultItem = reader["DefaultItem"].ToString();
                                entityBO.IsDefaultItem =Convert.ToBoolean(reader["IsDefaultItem"].ToString());
                                itemList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return itemList;
        }


        public string GenarateWhereConditionstring(String ItemName, bool ActiveStat, int isDefault)
        {

            string Where = string.Empty;
            if (!string.IsNullOrEmpty(ItemName.ToString()))
            {
                Where += "  ItemName = '" + ItemName + "'";
            }

            if (!string.IsNullOrEmpty(ActiveStat.ToString()))
            {
                if (!string.IsNullOrEmpty(Where))
                {
                    Where += " AND ActiveStat = '" + ActiveStat + "'";
                }
                else
                {
                    Where += "  ActiveStat = '" + ActiveStat + "'";
                }
            }
            if (isDefault != -1)
            {
                    bool Default = isDefault == 1 ? true : false;
                    if (!string.IsNullOrEmpty(Where))
                    {
                        Where += " AND ISNULL(IsDefaultItem,0) = '" + Default + "'";
                    }
                    else
                    {
                        Where += "  ISNULL(IsDefaultItem,0) = '" + Default + "'";
                    }
            }

            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where;
            }
            return Where;
        }


        public bool SaveHMComplementaryItemInfo(HMComplementaryItemBO comItemBO, out int tempItemId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveHMComplementaryItemInfo_SP"))
                    {

                        dbSmartAspects.AddInParameter(command, "@ItemName", DbType.String, comItemBO.ItemName);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, comItemBO.Description);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, comItemBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, comItemBO.CreatedBy);
                        dbSmartAspects.AddInParameter(command, "@IsDefaultItem", DbType.Boolean, comItemBO.IsDefaultItem);
                        dbSmartAspects.AddOutParameter(command, "@ComplementaryItemId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tempItemId = Convert.ToInt32(command.Parameters["@ComplementaryItemId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public bool UpdateHMComplementaryItemInfo(HMComplementaryItemBO comItemBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateHMComplementaryItemInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@ComplementaryItemId", DbType.String, comItemBO.ComplementaryItemId);
                        dbSmartAspects.AddInParameter(command, "@ItemName", DbType.String, comItemBO.ItemName);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, comItemBO.Description);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, comItemBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, comItemBO.LastModifiedBy);
                        dbSmartAspects.AddInParameter(command, "@IsDefaultItem", DbType.Boolean, comItemBO.IsDefaultItem);

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public List<HMComplementaryItemBO> GetComplementaryItemInfoByReservationId(int reservationId)
        {
            List<HMComplementaryItemBO> entityBOList = new List<HMComplementaryItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetComplementaryItemInfoByReservationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HMComplementaryItemBO entityBO = new HMComplementaryItemBO();

                                entityBO.ComplementaryItemId = Convert.ToInt32(reader["ComplementaryItemId"]);
                                entityBO.ItemName = reader["ItemName"].ToString();
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.RCItemId = Convert.ToInt32(reader["RCItemId"]);
                                entityBO.ReservationId = Convert.ToInt32(reader["ReservationId"].ToString());
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public List<HMComplementaryItemBO> GetComplementaryItemInfoByRegistrationId(int registrationId)
        {
            List<HMComplementaryItemBO> entityBOList = new List<HMComplementaryItemBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetComplementaryItemInfoByRegistrationId_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                HMComplementaryItemBO entityBO = new HMComplementaryItemBO();

                                entityBO.ComplementaryItemId = Convert.ToInt32(reader["ComplementaryItemId"]);
                                entityBO.ItemName = reader["ItemName"].ToString();
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.RCItemId = Convert.ToInt32(reader["RCItemId"]);
                                entityBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"].ToString());
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
    }
}
