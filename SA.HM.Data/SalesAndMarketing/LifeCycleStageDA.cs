using HotelManagement.Entity.SalesAndMarketing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.SalesAndMarketing
{
    public class LifeCycleStageDA : BaseService
    {
        public SMLifeCycleStageBO GetDealStageWiseCompanyStatusById(long id)
        {
            SMLifeCycleStageBO StatusBO = new SMLifeCycleStageBO();
            string query = string.Format("select * from SMLifeCycleStage  where IsDeleted= 0 AND Id = {0}", id);

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        DataSet ds = new DataSet();
                        dbSmartAspects.LoadDataSet(cmd, ds, "SalesCall");
                        DataTable Table = ds.Tables["SalesCall"];

                        StatusBO = Table.AsEnumerable().Select(r => new SMLifeCycleStageBO
                        {
                            Id = r.Field<long>("Id"),
                            LifeCycleStage = r.Field<string>("LifeCycleStage"),
                            IsRelatedToDeal = r.Field<bool>("IsRelatedToDeal"),
                            ForcastType = r.Field<string>("ForcastType"),
                            DealType = r.Field<string>("DealType"),
                            Description = r.Field<string>("Description"),
                            DisplaySequence = r.Field<int?>("DisplaySequence"),
                            IsActive = r.Field<bool>("IsActive")

                        }).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return StatusBO;

        }

        public Boolean SaveUpdateDealStage(SMLifeCycleStageBO dealStageWiseCompanyStatusBO, out long OutId)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveUpdateLifeCycleStage_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, dealStageWiseCompanyStatusBO.Id);

                        if (dealStageWiseCompanyStatusBO.LifeCycleStage != "")
                            dbSmartAspects.AddInParameter(command, "@LifeCycleStage", DbType.String, dealStageWiseCompanyStatusBO.LifeCycleStage);
                        else
                            dbSmartAspects.AddInParameter(command, "@LifeCycleStage", DbType.String, DBNull.Value);

                        if (dealStageWiseCompanyStatusBO.DisplaySequence != 0)
                            dbSmartAspects.AddInParameter(command, "@DisplaySequence", DbType.String, dealStageWiseCompanyStatusBO.DisplaySequence);
                        else
                            dbSmartAspects.AddInParameter(command, "@DisplaySequence", DbType.String, DBNull.Value);

                        if (dealStageWiseCompanyStatusBO.Description != "")
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, dealStageWiseCompanyStatusBO.Description);
                        else
                            dbSmartAspects.AddInParameter(command, "@Description", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(command, "@IsRelatedToDeal", DbType.String, dealStageWiseCompanyStatusBO.IsRelatedToDeal);

                        if (dealStageWiseCompanyStatusBO.DealType != "0")
                            dbSmartAspects.AddInParameter(command, "@DealType", DbType.String, dealStageWiseCompanyStatusBO.DealType);
                        else
                            dbSmartAspects.AddInParameter(command, "@DealType", DbType.String, DBNull.Value);

                        if (dealStageWiseCompanyStatusBO.ForcastType != "0")
                            dbSmartAspects.AddInParameter(command, "@ForcastType", DbType.String, dealStageWiseCompanyStatusBO.ForcastType);
                        else
                            dbSmartAspects.AddInParameter(command, "@ForcastType", DbType.String, DBNull.Value);

                        if (dealStageWiseCompanyStatusBO.Id == 0)
                            dbSmartAspects.AddInParameter(command, "@User", DbType.String, dealStageWiseCompanyStatusBO.CreatedBy);
                        else
                            dbSmartAspects.AddInParameter(command, "@User", DbType.String, dealStageWiseCompanyStatusBO.LastModifiedBy);

                        dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        OutId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public List<SMLifeCycleStageBO> GetlifeCycleStagePagination(string lifeCycleStage, int recordPerPage, int pageIndex, out int totalRecords)
        {
            List<SMLifeCycleStageBO> LifeCycleStageList = new List<SMLifeCycleStageBO>();
            try
            {

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLifeCycleStageForPaging_SP"))
                    {

                        if ((lifeCycleStage) != "")
                            dbSmartAspects.AddInParameter(cmd, "@LifeCycleStage", DbType.String, lifeCycleStage);
                        else
                            dbSmartAspects.AddInParameter(cmd, "@LifeCycleStage", DbType.String, DBNull.Value);

                        dbSmartAspects.AddInParameter(cmd, "@RecordPerPage", DbType.Int32, recordPerPage);
                        dbSmartAspects.AddInParameter(cmd, "@PageIndex", DbType.Int32, pageIndex);
                        dbSmartAspects.AddOutParameter(cmd, "@RecordCount", DbType.Int32, sizeof(Int32));

                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    SMLifeCycleStageBO LifeCycle = new SMLifeCycleStageBO();

                                    LifeCycle.Id = Convert.ToInt64(reader["Id"]);
                                    LifeCycle.LifeCycleStage = (reader["LifeCycleStage"].ToString());
                                    LifeCycle.DealType = (reader["DealType"].ToString());
                                    LifeCycle.ForcastType = (reader["ForcastType"].ToString());
                                    LifeCycle.DisplaySequence = (Convert.ToInt32(reader["DisplaySequence"]));

                                    LifeCycleStageList.Add(LifeCycle);
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
            return LifeCycleStageList;
        }

        public bool DeleteStatus(long Id)
        {
            bool status = false;
            try
            {
                string query = string.Format("DELETE FROM SMLifeCycleStage WHERE Id = {0}", Id);

                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        status = dbSmartAspects.ExecuteNonQuery(cmd) > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return status;
        }

        public List<SMLifeCycleStageBO> GetLifeCycleForDdl()
        {
            List<SMLifeCycleStageBO> stageBOs = new List<SMLifeCycleStageBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetLifeCycleStageForDdl_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SMLifeCycleStageBO sMLife = new SMLifeCycleStageBO();

                                sMLife.Id = Convert.ToInt32(reader["Id"]);
                                sMLife.LifeCycleStage = reader["LifeCycleStage"].ToString();
                                sMLife.DisplaySequence = Convert.ToInt32(reader["DisplaySequence"]);

                                stageBOs.Add(sMLife);
                            }
                        }
                    }
                }
            }

            return stageBOs;
        }

        public Boolean DuplicateTypeCheck(SMLifeCycleStageBO sMLifeCycleStage)
        {
            Boolean status = false;

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("CheckDuplicateLifeCycleStageByDealForcastType_SP"))
                    {



                        dbSmartAspects.AddInParameter(command, "@Id", DbType.String, sMLifeCycleStage.Id);

                        if (sMLifeCycleStage.DealType != "0")
                            dbSmartAspects.AddInParameter(command, "@DealType", DbType.String, sMLifeCycleStage.DealType);
                        else
                            dbSmartAspects.AddInParameter(command, "@DealType", DbType.String, DBNull.Value);

                        if (sMLifeCycleStage.ForcastType != "0")
                            dbSmartAspects.AddInParameter(command, "@ForcastType", DbType.String, sMLifeCycleStage.ForcastType);
                        else
                            dbSmartAspects.AddInParameter(command, "@ForcastType", DbType.String, DBNull.Value);


                        using (IDataReader reader = dbSmartAspects.ExecuteReader(command))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    status = Convert.ToInt64(reader["STATUS"]) == 1 ? true : false;
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
            return status;
        }

    }
}
