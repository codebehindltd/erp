using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Restaurant;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Restaurant
{
    public class RestaurantKotSpecialRemarksDetailDA : BaseService
    {
        public List<RestaurantKotSpecialRemarksDetailBO> GetRestaurantKotSpecialRemarksDetail()
        {
            List<RestaurantKotSpecialRemarksDetailBO> entityRestaurantKotSpecialRemarks = new List<RestaurantKotSpecialRemarksDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantKotSpecialRemarksDetail_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantKotSpecialRemarksDetailBO entityBO = new RestaurantKotSpecialRemarksDetailBO();

                                entityBO.RemarksDetailId = Convert.ToInt32(reader["RemarksDetailId"]);
                                entityBO.KotId = Convert.ToInt32(reader["KotId"].ToString());
                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"].ToString());
                                entityBO.SpecialRemarksId = Convert.ToInt32(reader["SpecialRemarksId"]);
                                entityBO.CreatedBy = Convert.ToInt32(reader["CreatedBy"].ToString());
                                if (reader["CreatedDate"] != DBNull.Value)
                                    entityBO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                                if (reader["LastModifiedBy"] != DBNull.Value)
                                    entityBO.LastModifiedBy = Convert.ToInt32(reader["LastModifiedBy"].ToString());
                                if (reader["LastModifiedDate"] != DBNull.Value)
                                    entityBO.LastModifiedDate = Convert.ToDateTime(reader["[LastModifiedDate]"].ToString());

                                entityRestaurantKotSpecialRemarks.Add(entityBO);
                            }
                        }
                    }
                }
            }

            return entityRestaurantKotSpecialRemarks;
        }

        public List<RestaurantKotSpecialRemarksDetailBO> GetInvItemSpecialRemarksInfoById(int kotId, int itemId)
        {
            List<RestaurantKotSpecialRemarksDetailBO> entityListBO = new List<RestaurantKotSpecialRemarksDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetRestaurantKotSpecialRemarksDetailById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                RestaurantKotSpecialRemarksDetailBO entityBO = new RestaurantKotSpecialRemarksDetailBO();

                                entityBO.RemarksDetailId = Convert.ToInt32(reader["RemarksDetailId"]);
                                entityBO.KotId = Convert.ToInt32(reader["KotId"].ToString());
                                entityBO.ItemId = Convert.ToInt32(reader["ItemId"].ToString());
                                entityBO.SpecialRemarksId = Convert.ToInt32(reader["SpecialRemarksId"]);
                                entityBO.Remarks = reader["Remarks"].ToString();

                                entityListBO.Add(entityBO);
                            }
                        }
                    }
                }
            }
            return entityListBO;
        }
        public List<RestaurantKotSpecialRemarksDetailBO> GetInvItemSpecialRemarksInfoByIdForPegination(int userInfoId, int kotId, int itemId, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<RestaurantKotSpecialRemarksDetailBO> entityListBO = new List<RestaurantKotSpecialRemarksDetailBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetInvItemSpecialRemarksInfoByIdForPegination_SP"))
                {

                    if (userInfoId != 0)
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, userInfoId);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@UserId", DbType.Int32, DBNull.Value);


                    dbSmartAspects.AddInParameter(cmd, "@KotId", DbType.Int32, kotId);
                    dbSmartAspects.AddInParameter(cmd, "@ItemId", DbType.Int32, itemId);

                    dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                    dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);

                    dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                    DataSet CashDS = new DataSet();

                    dbSmartAspects.LoadDataSet(cmd, CashDS, "Cash");
                    DataTable Table = CashDS.Tables["Cash"];

                    entityListBO = Table.AsEnumerable().Select(r => new RestaurantKotSpecialRemarksDetailBO
                    {
                        RemarksDetailId = r.Field<int>("RemarksDetailId"),
                        KotId = r.Field<int>("KotId"),
                        ItemId = r.Field<int>("ItemId"),
                        SpecialRemarksId = r.Field<int>("SpecialRemarksId"),
                        SpecialRemarks = r.Field<string>("SpecialRemarks"),
                        Remarks = r.Field<string>("Remarks")
                    }).ToList();


                    totalRecords = (int)cmd.Parameters["@RecordCount"].Value;
                }
            }
            return entityListBO;
        }

        public bool SaveRestaurantKotSpecialRemarksDetail(List<RestaurantKotSpecialRemarksDetailBO> remarksList, int waiterId, int kotDetailsId, out int tempItemId)
        {
            Boolean status = false;
            tempItemId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurantKotSpecialRemarksDetail_SP"))
                    {
                        foreach (RestaurantKotSpecialRemarksDetailBO comItemBO in remarksList)
                        {
                            command.Parameters.Clear();

                            dbSmartAspects.AddInParameter(command, "@KotDetailsId", DbType.Int32, kotDetailsId);
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, comItemBO.KotId);
                            dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, comItemBO.ItemId);
                            dbSmartAspects.AddInParameter(command, "@SpecialRemarksId", DbType.Int32, comItemBO.SpecialRemarksId);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, comItemBO.Remarks);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, waiterId);
                            dbSmartAspects.AddOutParameter(command, "@RemarksDetailId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                            tempItemId = Convert.ToInt32(command.Parameters["@RemarksDetailId"].Value);
                        }
                    }

                    if (status)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
            }
            return status;
        }

        public bool UpdateRestaurantKotSpecialRemarksDetail(List<RestaurantKotSpecialRemarksDetailBO> remarksList, List<RestaurantKotSpecialRemarksDetailBO> kotSRemarksDetailTobeDelete, int waiterId, int kotDetailsId)
        {
            Boolean status = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurantKotSpecialRemarksDetail_SP"))
                    {
                        if (remarksList.Count > 0)
                        {
                            foreach (RestaurantKotSpecialRemarksDetailBO comItemBO in remarksList)
                            {
                                command.Parameters.Clear();
                                dbSmartAspects.AddInParameter(command, "@KotDetailsId", DbType.Int32, kotDetailsId);
                                dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, comItemBO.KotId);
                                dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, comItemBO.ItemId);
                                dbSmartAspects.AddInParameter(command, "@SpecialRemarksId", DbType.Int32, comItemBO.SpecialRemarksId);
                                dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, comItemBO.Remarks);
                                dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, waiterId);
                                dbSmartAspects.AddOutParameter(command, "@RemarksDetailId", DbType.Int32, sizeof(Int32));

                                status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                            }
                        }
                    }

                    if (kotSRemarksDetailTobeDelete.Count > 0)
                    {
                        using (DbCommand commandDelete = dbSmartAspects.GetStoredProcCommand("DeleteRestaurantKotSpecialRemarksDetail_SP"))
                        {
                            foreach (RestaurantKotSpecialRemarksDetailBO comItemBO in kotSRemarksDetailTobeDelete)
                            {
                                commandDelete.Parameters.Clear();
                                dbSmartAspects.AddInParameter(commandDelete, "@KotDetailsId", DbType.Int32, kotDetailsId);
                                dbSmartAspects.AddInParameter(commandDelete, "@RemarksDetailId", DbType.Int32, comItemBO.RemarksDetailId);
                                status = dbSmartAspects.ExecuteNonQuery(commandDelete) > 0 ? true : false;
                            }
                        }
                    }

                    if (status)
                    {
                        transaction.Commit();
                    }
                    else if (kotSRemarksDetailTobeDelete.Count > 0 || remarksList.Count > 0)
                        transaction.Rollback();
                }
            }
            return status;
        }

        public bool UpdateRestaurantKotSpecialRemarksDetail(RestaurantKotSpecialRemarksDetailBO comItemBO)
        {
            Boolean status = false;
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantKotSpecialRemarksDetail_SP"))
                {
                    dbSmartAspects.AddInParameter(command, "@RemarksDetailId", DbType.String, comItemBO.RemarksDetailId);
                    dbSmartAspects.AddInParameter(command, "@KotId", DbType.String, comItemBO.KotId);
                    dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Boolean, comItemBO.ItemId);
                    dbSmartAspects.AddInParameter(command, "@SpecialRemarksId", DbType.Int32, comItemBO.SpecialRemarksId);
                    dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.String, comItemBO.CreatedBy);
                    dbSmartAspects.AddInParameter(command, "@CreatedDate", DbType.String, comItemBO.CreatedDate);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Boolean, comItemBO.LastModifiedBy);
                    dbSmartAspects.AddInParameter(command, "@LastModifiedDate", DbType.Int32, comItemBO.LastModifiedDate);
                    dbSmartAspects.AddOutParameter(command, "@RemarksDetailId", DbType.Int32, sizeof(Int32));

                    status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;
                }
            }
            return status;
        }

        public bool SaveRestaurantKotItemWiseRemarksDetail(RestaurantKotSpecialRemarksDetailBO itemWiseRemark, int kotDetailsId, int waiterId, out int remarksDetailsId)
        {
            Boolean status = false;
            remarksDetailsId = 0;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transaction = conn.BeginTransaction())
                {
                    if (itemWiseRemark.RemarksDetailId == 0)
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveRestaurantKotSpecialRemarksDetail_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@KotDetailsId", DbType.Int32, kotDetailsId);
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, itemWiseRemark.KotId);
                            dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, itemWiseRemark.ItemId);
                            dbSmartAspects.AddInParameter(command, "@SpecialRemarksId", DbType.Int32, itemWiseRemark.SpecialRemarksId);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, itemWiseRemark.Remarks);
                            dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, waiterId);
                            dbSmartAspects.AddOutParameter(command, "@RemarksDetailId", DbType.Int32, sizeof(Int32));

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                            remarksDetailsId = Convert.ToInt32(command.Parameters["@RemarksDetailId"].Value);
                        }
                    }
                    else if (itemWiseRemark.RemarksDetailId > 0)
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateRestaurantKotSpecialRemarksDetail_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@RemarksDetailId", DbType.String, itemWiseRemark.RemarksDetailId);
                            dbSmartAspects.AddInParameter(command, "@KotDetailsId", DbType.Int32, kotDetailsId);
                            dbSmartAspects.AddInParameter(command, "@KotId", DbType.Int32, itemWiseRemark.KotId);
                            dbSmartAspects.AddInParameter(command, "@ItemId", DbType.Int32, itemWiseRemark.ItemId);
                            dbSmartAspects.AddInParameter(command, "@SpecialRemarksId", DbType.Int32, itemWiseRemark.SpecialRemarksId);
                            dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, itemWiseRemark.Remarks);
                            dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Boolean, waiterId);

                            status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                            remarksDetailsId = Convert.ToInt32(command.Parameters["@RemarksDetailId"].Value);
                        }
                    }

                    if (status)
                        transaction.Commit();
                    else
                        transaction.Rollback();
                }
            }
            return status;
        }

    }
}
