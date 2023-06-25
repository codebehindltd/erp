using HotelManagement.Entity.HouseKeeping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.HouseKeeping
{
    public class LostFoundDA : BaseService
    {
        public bool SaveLostNFound(LostFoundBO foundBO, out int outId)
        {
            Boolean status = false;
            outId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveLostNFound_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, foundBO.Id);
                        dbSmartAspects.AddInParameter(command, "@TransectionType", DbType.String, foundBO.TransectionType);
                        dbSmartAspects.AddInParameter(command, "@TransectionId", DbType.Int32, foundBO.TransectionId);
                        dbSmartAspects.AddInParameter(command, "@OtherArea", DbType.String, foundBO.OtherArea);
                        dbSmartAspects.AddInParameter(command, "@ItemName", DbType.String, foundBO.ItemName);
                        dbSmartAspects.AddInParameter(command, "@ItemType", DbType.String, foundBO.ItemType);
                        dbSmartAspects.AddInParameter(command, "@Description", DbType.String, foundBO.Description);
                        dbSmartAspects.AddInParameter(command, "@FoundDateTime", DbType.DateTime, foundBO.FoundDateTime);
                        dbSmartAspects.AddInParameter(command, "@WhoFoundIt", DbType.Int32, foundBO.WhoFoundIt);
                        dbSmartAspects.AddInParameter(command, "@WhoFoundItName", DbType.String, foundBO.WhoFoundItName);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, foundBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int32, sizeof(Int32));
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        outId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                    if (status)
                    {
                        transction.Commit();
                    }
                    else
                    {
                        transction.Rollback();
                    }
                }
            }
            return status;
        }

        public LostFoundBO GetLostFoundInfoById(long id)
        {
            LostFoundBO infoBO = new LostFoundBO();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLostFoundInfoById_SP"))
                    {
                        dbSmartAspects.AddInParameter(cmd, "@Id", DbType.Int64, id);
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    infoBO.Id = Convert.ToInt32(reader["Id"]);
                                    if ((reader["TransectionId"]) != DBNull.Value)
                                    {
                                        infoBO.TransectionId = Convert.ToInt32(reader["TransectionId"]);
                                    }
                                    
                                    infoBO.TransectionType = reader["TransectionType"].ToString();
                                    infoBO.OtherArea = Convert.ToString(reader["OtherArea"]);
                                    infoBO.FoundDate = Convert.ToString(reader["FoundDate"]);
                                    infoBO.FoundTime = Convert.ToString(reader["FoundTime"]);
                                    infoBO.ItemName = reader["ItemName"].ToString();
                                    infoBO.ItemType = Convert.ToString(reader["ItemType"]);
                                    if ((reader["WhoFoundIt"])!= DBNull.Value)
                                    {
                                        infoBO.WhoFoundIt = Convert.ToInt32(reader["WhoFoundIt"]);
                                    }
                                    infoBO.WhoFoundItName = reader["WhoFoundItName"].ToString();
                                    infoBO.Description = (reader["Description"]).ToString();
                                    infoBO.ReturnDescription = (reader["ReturnDescription"]).ToString();
                                    if ((reader["ReturnDate"]) != DBNull.Value)
                                        infoBO.ReturnDate = Convert.ToDateTime(reader["ReturnDate"]);
                                    infoBO.HasItemReturned = Convert.ToBoolean(reader["HasItemReturned"]);
                                    infoBO.WhomToReturn = (reader["WhomToReturn"]).ToString();
                                    if ((reader["FoundDateTime"]) != DBNull.Value)
                                    {
                                        infoBO.FoundDateTime = Convert.ToDateTime(reader["FoundDateTime"]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return infoBO;
        }

        public List<LostFoundBO> GetLostFoundInfoGridding(string itemNameSrc, string itemTypeSrc, string transectionTypeSrc, int transectionIdSrc, string foundDateSrc, int foundPersonId, string foundPersonName, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<LostFoundBO> infoBOs = new List<LostFoundBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLostFoundInfoBySearchCriteriaForPaging_SP"))
                    {
                        if (!string.IsNullOrWhiteSpace(itemNameSrc))
                            dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemNameSrc);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, DBNull.Value);

                        if (itemTypeSrc != "0")
                            dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, itemTypeSrc);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, DBNull.Value);

                        if (transectionTypeSrc != "0")
                            dbSmartAspects.AddInParameter(cmd, "@TransectionType", DbType.String, transectionTypeSrc);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@TransectionType", DbType.String, DBNull.Value);

                        if (transectionIdSrc != 0)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@TransectionId", DbType.Int32, transectionIdSrc);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@TransectionId", DbType.Int32, DBNull.Value);
                        }

                        if (foundPersonId != 0)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@WhoFoundIt", DbType.Int32, foundPersonId);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@WhoFoundIt", DbType.Int32, DBNull.Value);
                        }

                        if (!string.IsNullOrWhiteSpace(foundPersonName))
                            dbSmartAspects.AddInParameter(cmd, "@WhoFoundItName", DbType.String, foundPersonName);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@WhoFoundItName", DbType.String, DBNull.Value);

                        if (!string.IsNullOrWhiteSpace(foundDateSrc))
                            dbSmartAspects.AddInParameter(cmd, "@FoundDateTime", DbType.DateTime, foundDateSrc);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@FoundDateTime", DbType.DateTime, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    LostFoundBO bO = new LostFoundBO();
                                    bO.Id = Convert.ToInt64(reader["Id"]);
                                    bO.ItemName = Convert.ToString(reader["ItemName"]);
                                    bO.FoundDateTime = Convert.ToDateTime(reader["FoundDateTime"]);
                                    bO.TransectionType = Convert.ToString(reader["TransectionType"]);
                                    bO.HasItemReturned = Convert.ToBoolean(reader["HasItemReturned"]);

                                    infoBOs.Add(bO);
                                }
                            }
                        }
                        totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return infoBOs;
        }
        public List<LostFoundBO> GetLostFoundInfoForReport(string itemNameSrc, string itemTypeSrc, string hasReturned, string transectionTypeSrc, int transectionIdSrc, DateTime fromDate, DateTime todate, int foundPersonId, string foundPersonName)
        {
            List<LostFoundBO> infoBOs = new List<LostFoundBO>();
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLostFoundInfoForReport_SP"))
                    {
                        if (!string.IsNullOrWhiteSpace(itemNameSrc))
                            dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, itemNameSrc);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ItemName", DbType.String, DBNull.Value);

                        if (itemTypeSrc != "0")
                            dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, itemTypeSrc);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@ItemType", DbType.String, DBNull.Value);

                        if (hasReturned != "0")
                        {
                            if (hasReturned == "Returned")
                            {
                                dbSmartAspects.AddInParameter(cmd, "@HasReturned", DbType.Boolean, "1");

                            }
                            else
                            {
                                dbSmartAspects.AddInParameter(cmd, "@HasReturned", DbType.Boolean, "0");
                            }
                        }
                        else
                            dbSmartAspects.AddInParameter(cmd, "@HasReturned", DbType.String, DBNull.Value);

                        if (transectionTypeSrc != "0")
                            dbSmartAspects.AddInParameter(cmd, "@TransectionType", DbType.String, transectionTypeSrc);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@TransectionType", DbType.String, DBNull.Value);

                        if (transectionIdSrc != 0)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@TransectionId", DbType.Int32, transectionIdSrc);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@TransectionId", DbType.Int32, DBNull.Value);
                        }

                        if (foundPersonId != 0)
                        {
                            dbSmartAspects.AddInParameter(cmd, "@WhoFoundIt", DbType.Int32, foundPersonId);
                        }
                        else
                        {
                            dbSmartAspects.AddInParameter(cmd, "@WhoFoundIt", DbType.Int32, DBNull.Value);
                        }

                        if (!string.IsNullOrWhiteSpace(foundPersonName))
                            dbSmartAspects.AddInParameter(cmd, "@WhoFoundItName", DbType.String, foundPersonName);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@WhoFoundItName", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@FromDate", DbType.DateTime, fromDate);
                        dbSmartAspects.AddInParameter(cmd, "@ToDate", DbType.DateTime, todate);

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    LostFoundBO bO = new LostFoundBO();
                                    bO.Id = Convert.ToInt64(reader["Id"]);
                                    bO.ItemName = Convert.ToString(reader["ItemName"]);
                                    bO.ItemType = Convert.ToString(reader["ItemType"]);
                                    bO.FoundPlace = Convert.ToString(reader["FoundPlace"]);
                                    bO.FoundDateTime = Convert.ToDateTime(reader["FoundDateTime"]);
                                    bO.FoundDateTimeDisplay = Convert.ToString(reader["FoundDateTimeDisplay"]);

                                    bO.WhoFoundItName = Convert.ToString(reader["WhoFoundItName"]);
                                    bO.Description = Convert.ToString(reader["Description"]);

                                    if ((reader["ReturnDate"]) != DBNull.Value)
                                        bO.ReturnDate = Convert.ToDateTime(reader["ReturnDate"]);

                                    bO.ReturnDateDisplay = Convert.ToString(reader["ReturnDateDisplay"]);
                                    bO.ReturnDescription = Convert.ToString(reader["ReturnDescription"]);
                                    bO.WhomToReturn = Convert.ToString(reader["WhomToReturn"]);
                                    bO.HasItemReturned = Convert.ToBoolean(reader["HasItemReturned"]);
                                    
                                    infoBOs.Add(bO);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return infoBOs;
        }
        public bool PerformReturn(LostFoundBO bO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("PerformLostNFoundReturn_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int32, bO.Id);
                        dbSmartAspects.AddInParameter(command, "@ReturnDate", DbType.DateTime, bO.ReturnDate);
                        dbSmartAspects.AddInParameter(command, "@ReturnDescription", DbType.String, bO.ReturnDescription);
                        dbSmartAspects.AddInParameter(command, "@WhomToReturn", DbType.String, bO.WhomToReturn);
                        dbSmartAspects.AddInParameter(command, "@HasItemReturned", DbType.Boolean, bO.HasItemReturned);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Boolean, bO.LastModifiedBy);
                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                    }
                    if (status)
                    {
                        transction.Commit();
                    }
                    else
                    {
                        transction.Rollback();
                    }
                }
            }

           return status;
        }

    }
}
