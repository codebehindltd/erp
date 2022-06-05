using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.HotelManagement;
using System.Data.Common;
using System.Data;
using System.Collections;

namespace HotelManagement.Data.HotelManagement
{
    public class RoomOwnerDA : BaseService
    {
        public List<RoomOwnerBO> GetRoomOwnerInfo()
        {
            List<RoomOwnerBO> roomOwnerList = new List<RoomOwnerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomOwnerInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomOwnerBO roomOwner = new RoomOwnerBO();

                                roomOwner.OwnerId = Convert.ToInt32(reader["OwnerId"]);
                                roomOwner.FirstName = reader["FirstName"].ToString();
                                roomOwner.LastName = reader["LastName"].ToString();
                                roomOwner.OwnerName = reader["OwnerName"].ToString();
                                roomOwner.Description = reader["Description"].ToString();
                                roomOwner.Address = reader["Address"].ToString();
                                roomOwner.CityName = reader["CityName"].ToString();
                                roomOwner.ZipCode = reader["ZipCode"].ToString();
                                roomOwner.StateName = reader["StateName"].ToString();
                                roomOwner.Country = reader["Country"].ToString();
                                roomOwner.Phone = reader["Phone"].ToString();
                                roomOwner.Fax = reader["Fax"].ToString();
                                roomOwner.Email = reader["Email"].ToString();

                                roomOwnerList.Add(roomOwner);
                            }
                        }
                    }
                }
            }
            return roomOwnerList;
        }
        public Boolean SaveRoomOwnerInfo(RoomOwnerBO roomOwner, out int tmpOwnerId, List<OwnerDetailBO> detailBO)
        {
            bool retVal = false;
            int status = 0;
            //int tmpRegistrationId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("SaveRoomOwnerInfo_SP"))
                {
                    conn.Open();
                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@FirstName", DbType.String, roomOwner.FirstName);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastName", DbType.String, roomOwner.LastName);
                        dbSmartAspects.AddInParameter(commandMaster, "@Description", DbType.String, roomOwner.Description);
                        dbSmartAspects.AddInParameter(commandMaster, "@Address", DbType.String, roomOwner.Address);
                        dbSmartAspects.AddInParameter(commandMaster, "@CityName", DbType.String, roomOwner.CityName);
                        dbSmartAspects.AddInParameter(commandMaster, "@ZipCode", DbType.String, roomOwner.ZipCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@StateName", DbType.String, roomOwner.StateName);
                        dbSmartAspects.AddInParameter(commandMaster, "@Country", DbType.String, roomOwner.Country);
                        dbSmartAspects.AddInParameter(commandMaster, "@Phone", DbType.String, roomOwner.Phone);
                        dbSmartAspects.AddInParameter(commandMaster, "@Fax", DbType.String, roomOwner.Fax);
                        dbSmartAspects.AddInParameter(commandMaster, "@Email", DbType.String, roomOwner.Email);
                        dbSmartAspects.AddInParameter(commandMaster, "@CreatedBy", DbType.Int32, roomOwner.CreatedBy);
                        dbSmartAspects.AddOutParameter(commandMaster, "@OwnerId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        tmpOwnerId = Convert.ToInt32(commandMaster.Parameters["@OwnerId"].Value);

                        if (status > 0)
                        {
                            int count = 0;

                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveOwnerDetailInfo_SP"))
                            {
                                foreach (OwnerDetailBO ownerDetailBO in detailBO)
                                {
                                    if (ownerDetailBO.OwnerId == 0)
                                    {
                                        commandDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandDetails, "@OwnerId", DbType.Int32, tmpOwnerId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomId", DbType.Int32, ownerDetailBO.RoomId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@CommissionValue", DbType.Int32, ownerDetailBO.CommissionValue);

                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            if (count == detailBO.Count)
                            {
                                transction.Commit();
                                retVal = true;
                            }
                            else
                            {
                                retVal = false;
                            }
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
        public Boolean UpdateRoomOwnerInfo(RoomOwnerBO roomOwner, List<OwnerDetailBO> detailBO, ArrayList arrayDelete)
        {
            bool retVal = false;
            int status = 0;
            int tmpOwnerId = 0;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("UpdateRoomOwnerInfo_SP"))
                {
                    conn.Open();

                    using (DbTransaction transction = conn.BeginTransaction())
                    {
                        dbSmartAspects.AddInParameter(commandMaster, "@OwnerId", DbType.Int32, roomOwner.OwnerId);
                        dbSmartAspects.AddInParameter(commandMaster, "@FirstName", DbType.String, roomOwner.FirstName);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastName", DbType.String, roomOwner.LastName);
                        dbSmartAspects.AddInParameter(commandMaster, "@Description", DbType.String, roomOwner.Description);
                        dbSmartAspects.AddInParameter(commandMaster, "@Address", DbType.String, roomOwner.Address);
                        dbSmartAspects.AddInParameter(commandMaster, "@CityName", DbType.String, roomOwner.CityName);
                        dbSmartAspects.AddInParameter(commandMaster, "@ZipCode", DbType.String, roomOwner.ZipCode);
                        dbSmartAspects.AddInParameter(commandMaster, "@StateName", DbType.String, roomOwner.StateName);
                        dbSmartAspects.AddInParameter(commandMaster, "@Country", DbType.String, roomOwner.Country);
                        dbSmartAspects.AddInParameter(commandMaster, "@Phone", DbType.String, roomOwner.Phone);
                        dbSmartAspects.AddInParameter(commandMaster, "@Fax", DbType.String, roomOwner.Fax);
                        dbSmartAspects.AddInParameter(commandMaster, "@Email", DbType.String, roomOwner.Email);
                        dbSmartAspects.AddInParameter(commandMaster, "@LastModifiedBy", DbType.Int32, roomOwner.LastModifiedBy);

                        status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction);

                        tmpOwnerId = roomOwner.OwnerId;

                        if (status > 0)
                        {
                            int count = 0;

                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveOwnerDetailInfo_SP"))
                            {
                                foreach (OwnerDetailBO ownerDetailBO in detailBO)
                                {
                                    if (ownerDetailBO.OwnerId == 0)
                                    {
                                        commandDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandDetails, "@OwnerId", DbType.Int32, tmpOwnerId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomId", DbType.Int32, ownerDetailBO.RoomId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@CommissionValue", DbType.Int32, ownerDetailBO.CommissionValue);

                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("UpdateOwnerDetailInfo_SP"))
                            {
                                foreach (OwnerDetailBO ownerDetailBO in detailBO)
                                {
                                    if (ownerDetailBO.OwnerId != 0)
                                    {
                                        commandDetails.Parameters.Clear();

                                        dbSmartAspects.AddInParameter(commandDetails, "@DetailId", DbType.Int32, ownerDetailBO.DetailId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@OwnerId", DbType.Int32, tmpOwnerId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@RoomId", DbType.Int32, ownerDetailBO.RoomId);
                                        dbSmartAspects.AddInParameter(commandDetails, "@CommissionValue", DbType.Int32, ownerDetailBO.CommissionValue);

                                        count += dbSmartAspects.ExecuteNonQuery(commandDetails, transction);
                                    }
                                }
                            }

                            if (count == detailBO.Count)
                            {
                                if (arrayDelete.Count > 0)
                                {
                                    foreach (int delId in arrayDelete)
                                    {
                                        using (DbCommand commandEducation = dbSmartAspects.GetStoredProcCommand("DeleteDataDynamically_SP"))
                                        {
                                            dbSmartAspects.AddInParameter(commandEducation, "@TableName", DbType.String, "HotelRoomOwnerDetail");
                                            dbSmartAspects.AddInParameter(commandEducation, "@TablePKField", DbType.String, "DetailId");
                                            dbSmartAspects.AddInParameter(commandEducation, "@TablePKId", DbType.String, delId);

                                            status = dbSmartAspects.ExecuteNonQuery(commandEducation);
                                        }
                                    }
                                }
                                transction.Commit();
                                retVal = true;
                            }
                            else
                            {
                                retVal = false;
                            }
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
        public RoomOwnerBO GetRoomOwnerInfoById(int ownerId)
        {
            RoomOwnerBO roomOwner = new RoomOwnerBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomOwnerInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@OwnerId", DbType.Int32, ownerId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                roomOwner.OwnerId = Convert.ToInt32(reader["OwnerId"]);
                                roomOwner.FirstName = reader["FirstName"].ToString();
                                roomOwner.LastName = reader["LastName"].ToString();
                                roomOwner.OwnerName = reader["OwnerName"].ToString();
                                roomOwner.Description = reader["Description"].ToString();
                                roomOwner.Address = reader["Address"].ToString();
                                roomOwner.CityName = reader["CityName"].ToString();
                                roomOwner.ZipCode = reader["ZipCode"].ToString();
                                roomOwner.StateName = reader["StateName"].ToString();
                                roomOwner.Country = reader["Country"].ToString();
                                roomOwner.Phone = reader["Phone"].ToString();
                                roomOwner.Fax = reader["Fax"].ToString();
                                roomOwner.Email = reader["Email"].ToString();

                            }
                        }
                    }
                }
            }
            return roomOwner;
        }
        public Boolean DeleteOwnerDetailInfoByOwnerId(int roomOwnerId)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteOwnerDetailInfoByOwnerId_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@OwnerId", DbType.Int32, roomOwnerId);

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public List<RoomOwnerBO> GetRoomOwnerInfoBySearchCriteria(string FirstName, string LastName, string Email)
        {
            List<RoomOwnerBO> roomOwnerList = new List<RoomOwnerBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRoomOwnerInfoBySearchCriteria_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@FirstName", DbType.String, FirstName);
                    dbSmartAspects.AddInParameter(cmd, "@LastName", DbType.String, LastName);
                    dbSmartAspects.AddInParameter(cmd, "@Email", DbType.String, Email);
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RoomOwnerBO roomOwner = new RoomOwnerBO();

                                roomOwner.OwnerId = Convert.ToInt32(reader["OwnerId"]);
                                roomOwner.FirstName = reader["FirstName"].ToString();
                                roomOwner.LastName = reader["LastName"].ToString();
                                roomOwner.OwnerName = reader["OwnerName"].ToString();
                                roomOwner.Description = reader["Description"].ToString();
                                roomOwner.Address = reader["Address"].ToString();
                                roomOwner.CityName = reader["CityName"].ToString();
                                roomOwner.ZipCode = reader["ZipCode"].ToString();
                                roomOwner.StateName = reader["StateName"].ToString();
                                roomOwner.Country = reader["Country"].ToString();
                                roomOwner.Phone = reader["Phone"].ToString();
                                roomOwner.Fax = reader["Fax"].ToString();
                                roomOwner.Email = reader["Email"].ToString();

                                roomOwnerList.Add(roomOwner);
                            }
                        }
                    }
                }
            }
            return roomOwnerList;
        }
    }
}
