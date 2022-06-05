using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using HotelManagement.Entity.Inventory;

namespace HotelManagement.Data.Inventory
{
    public class InvItemSpecialRemarksDA : BaseService
    {
        public List<InvItemSpecialRemarksBO> GetActiveInvItemSpecialRemarksInfo()
        {
            List<InvItemSpecialRemarksBO> entityBOList = new List<InvItemSpecialRemarksBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveInvItemSpecialRemarksInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemSpecialRemarksBO entityBO = new InvItemSpecialRemarksBO();

                                entityBO.SpecialRemarksId = Convert.ToInt32(reader["SpecialRemarksId"]);
                                entityBO.SpecialRemarks = reader["SpecialRemarks"].ToString();
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                entityBOList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityBOList;
        }
        public InvItemSpecialRemarksBO GetInvItemSpecialRemarksInfoById(int itemId)
        {
            InvItemSpecialRemarksBO entityBO = new InvItemSpecialRemarksBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemSpecialRemarksInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SpecialRemarksId", DbType.Int32, itemId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entityBO.SpecialRemarksId = Convert.ToInt32(reader["SpecialRemarksId"]);
                                entityBO.SpecialRemarks = reader["SpecialRemarks"].ToString();
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return entityBO;
        }
        public List<InvItemSpecialRemarksBO> GetInvItemSpecialRemarksInfoBySearchCriteria(string ItemName, bool ActiveStat)
        {
            string Where = GenarateWhereConditionstring(ItemName, ActiveStat);
            List<InvItemSpecialRemarksBO> itemList = new List<InvItemSpecialRemarksBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemSpecialRemarksInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SearchCriteria", DbType.String, Where);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                InvItemSpecialRemarksBO entityBO = new InvItemSpecialRemarksBO();
                                entityBO.SpecialRemarksId = Convert.ToInt32(reader["SpecialRemarksId"]);
                                entityBO.SpecialRemarks = reader["SpecialRemarks"].ToString();
                                entityBO.Description = reader["Description"].ToString();
                                entityBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]); 
                                entityBO.ActiveStatus = reader["ActiveStatus"].ToString();
                                itemList.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return itemList;
        }
        public string GenarateWhereConditionstring(String specialRemarks, bool ActiveStat)
        {

            string Where = string.Empty;
            if (!string.IsNullOrEmpty(specialRemarks.ToString()))
            {
                Where += "  SpecialRemarks = '" + specialRemarks + "'";
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

            if (!string.IsNullOrWhiteSpace(Where))
            {
                Where = " WHERE " + Where;
            }
            return Where;
        }
        public bool SaveInvItemSpecialRemarksInfo(InvItemSpecialRemarksBO comItemBO, out int tempItemId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveInvItemSpecialRemarksInfo_SP"))
                {

                    dbSmartAspects.AddInParameter(command, "@SpecialRemarks", DbType.String, comItemBO.SpecialRemarks);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, comItemBO.Description);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, comItemBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, comItemBO.CreatedBy);
                    dbSmartAspects.AddOutParameter(command, "@SpecialRemarksId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                    tempItemId = Convert.ToInt32(command.Parameters["@SpecialRemarksId"].Value);
                }
            }
            return status;
        }
        public bool UpdateInvItemSpecialRemarksInfo(InvItemSpecialRemarksBO comItemBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateInvItemSpecialRemarksInfo_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@SpecialRemarksId", DbType.String, comItemBO.SpecialRemarksId);
                    dbSmartAspects.AddInParameter(command, "@SpecialRemarks", DbType.String, comItemBO.SpecialRemarks);
                    dbSmartAspects.AddInParameter(command, "@Description", DbType.String, comItemBO.Description);
                    dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, comItemBO.ActiveStat);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, comItemBO.LastModifiedBy);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }
        //public List<InvItemSpecialRemarksBO> GetComplementaryItemInfoByReservationId(int reservationId)
        //{
        //    List<InvItemSpecialRemarksBO> entityBOList = new List<InvItemSpecialRemarksBO>();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetComplementaryItemInfoByReservationId_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@ReservationId", DbType.Int32, reservationId);
        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        InvItemSpecialRemarksBO entityBO = new InvItemSpecialRemarksBO();

        //                        entityBO.ComplementaryItemId = Convert.ToInt32(reader["ComplementaryItemId"]);
        //                        entityBO.ItemName = reader["ItemName"].ToString();
        //                        entityBO.Description = reader["Description"].ToString();
        //                        entityBO.RCItemId = Convert.ToInt32(reader["RCItemId"]);
        //                        entityBO.ReservationId = Convert.ToInt32(reader["ReservationId"].ToString());
        //                        entityBOList.Add(entityBO);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return entityBOList;
        //}
        //public List<InvItemSpecialRemarksBO> GetComplementaryItemInfoByRegistrationId(int registrationId)
        //{
        //    List<InvItemSpecialRemarksBO> entityBOList = new List<InvItemSpecialRemarksBO>();
        //    using (DbConnection conn = dbSmartAspects.CreateConnection())
        //    {
        //        using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetComplementaryItemInfoByRegistrationId_SP"))
        //        {
        //            dbSmartAspects.AddInParameter(cmd, "@RegistrationId", DbType.Int32, registrationId);
        //            using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
        //            {
        //                if (reader != null)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        InvItemSpecialRemarksBO entityBO = new InvItemSpecialRemarksBO();

        //                        entityBO.ComplementaryItemId = Convert.ToInt32(reader["ComplementaryItemId"]);
        //                        entityBO.ItemName = reader["ItemName"].ToString();
        //                        entityBO.Description = reader["Description"].ToString();
        //                        entityBO.RCItemId = Convert.ToInt32(reader["RCItemId"]);
        //                        entityBO.RegistrationId = Convert.ToInt32(reader["RegistrationId"].ToString());
        //                        entityBOList.Add(entityBO);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return entityBOList;
        //}
    }
}
