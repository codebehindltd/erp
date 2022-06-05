using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using HotelManagement.Entity.HMCommon;

namespace HotelManagement.Data.HMCommon
{
    public class DesignationDA : BaseService
    {
        public List<DesignationBO> GetDesignationInfo()
        {
            List<DesignationBO> boList = new List<DesignationBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDesignationInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                DesignationBO bo = new DesignationBO();

                                bo.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                                bo.Name = reader["Name"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();
                                bo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bo.ActiveStatus = reader["ActiveStatus"].ToString();

                                boList.Add(bo);
                            }
                        }
                    }
                }
            }
            return boList;
        }
        public List<DesignationBO> GetActiveDesignationInfo()
        {
            List<DesignationBO> boList = new List<DesignationBO>();

            DataSet designationDS = new DataSet();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetActiveDesignationInfo_SP"))
                {

                    dbSmartAspects.LoadDataSet(cmd, designationDS, "DesignationList");
                    DataTable table = designationDS.Tables["DesignationList"];

                    boList = table.AsEnumerable().Select(r =>
                                   new DesignationBO
                                   {
                                       DesignationId = r.Field<int>("DesignationId"),
                                       Name = r.Field<string>("Name"),
                                       Remarks = r.Field<string>("Remarks"),
                                       ActiveStat = r.Field<Boolean>("ActiveStat")                                       

                                   }).ToList();
                }
            }

            return boList;
        }
        public Boolean SaveDesignationInfo(DesignationBO bo, out int tmpBankId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveDesignationInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, bo.Name);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, bo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bo.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, bo.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@DesignationId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpBankId = Convert.ToInt32(command.Parameters["@DesignationId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public Boolean UpdateDesignationInfo(DesignationBO bo)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateDesignationInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@DesignationId", DbType.Int32, bo.DesignationId);
                        dbSmartAspects.AddInParameter(command, "@Name", DbType.String, bo.Name);
                        dbSmartAspects.AddInParameter(command, "@Remarks", DbType.String, bo.Remarks);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, bo.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, bo.LastModifiedBy);

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
        public DesignationBO GetDesignationInfoById(int pkId)
        {
            DesignationBO bo = new DesignationBO();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetDesignationInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@DesignationId", DbType.Int32, pkId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                bo.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                                bo.Name = reader["Name"].ToString();
                                bo.Remarks = reader["Remarks"].ToString();
                                bo.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                bo.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return bo;
        }
    }
}
