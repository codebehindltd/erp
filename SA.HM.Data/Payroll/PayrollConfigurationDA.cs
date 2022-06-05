using HotelManagement.Entity.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace HotelManagement.Data.Payroll
{
    public class PayrollConfigurationDA : BaseService
    {
        public Boolean SaveOrUpdateLeaveDeductionConfiguration(PayrollLeaveDeductionPolicyMasterBO PolicyMaster, List<PayrollLeaveDeductionPolicyDetailBO> DeductionPolicyDetailList,
                                                                List<PayrollLeaveDeductionPolicyDetailBO> deletedTermsNConditions, out long OutId)
        {
            Boolean status = false;
            Boolean retVal = false;

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                conn.Open();
                using (DbTransaction transction = conn.BeginTransaction())
                {
                    try
                    {
                        using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateLeaveDeductionConfiguration_SP"))
                        {
                            dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, PolicyMaster.Id);

                            if (PolicyMaster.NoOfLate != 0)
                                dbSmartAspects.AddInParameter(command, "@NoOfLate", DbType.Int64, PolicyMaster.NoOfLate);
                            else
                                dbSmartAspects.AddInParameter(command, "@NoOfLate", DbType.Int64, 0);

                            if (PolicyMaster.NoOfLeave != 0)
                                dbSmartAspects.AddInParameter(command, "@NoOfLeave", DbType.Int64, PolicyMaster.NoOfLeave);
                            else
                                dbSmartAspects.AddInParameter(command, "@NoOfLeave", DbType.Int64, 0);

                            if (PolicyMaster.Id == 0)
                                dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, PolicyMaster.CreatedBy);
                            else
                                dbSmartAspects.AddInParameter(command, "@UserId", DbType.String, PolicyMaster.LastModifiedBy);

                            dbSmartAspects.AddOutParameter(command, "@OutId", DbType.Int64, sizeof(Int64));

                            status = dbSmartAspects.ExecuteNonQuery(command, transction) > 0 ? true : false; ;

                            OutId = Convert.ToInt32(command.Parameters["@OutId"].Value);
                        }
                        if (status && DeductionPolicyDetailList.Count > 0)
                        {
                            using (DbCommand commandDetails = dbSmartAspects.GetStoredProcCommand("SaveOrUpdateLeaveDeductionDetail_SP"))
                            {
                                foreach (PayrollLeaveDeductionPolicyDetailBO DeductionPolicyDetail in DeductionPolicyDetailList)
                                {
                                    commandDetails.Parameters.Clear();
                                    dbSmartAspects.AddInParameter(commandDetails, "@Id", DbType.Int64, DeductionPolicyDetail.Id);
                                    dbSmartAspects.AddInParameter(commandDetails, "@LeaveId", DbType.Int64, DeductionPolicyDetail.LeaveId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@MasterId", DbType.Int32, OutId);
                                    dbSmartAspects.AddInParameter(commandDetails, "@Sequence", DbType.Int32, DeductionPolicyDetail.Sequence);

                                    status = dbSmartAspects.ExecuteNonQuery(commandDetails, transction) > 0 ? true : false;
                                }
                            }
                        }
                        if (status && deletedTermsNConditions.Count > 0)
                        {
                            using (DbCommand commandMaster = dbSmartAspects.GetStoredProcCommand("DeleteLeaveDeductionPolicyDetails_SP"))
                            {
                                foreach (PayrollLeaveDeductionPolicyDetailBO deletedTtem in deletedTermsNConditions)
                                {
                                    commandMaster.Parameters.Clear();

                                    dbSmartAspects.AddInParameter(commandMaster, "@Id", DbType.Int32, deletedTtem.Id);

                                    status = dbSmartAspects.ExecuteNonQuery(commandMaster, transction) > 0 ? true : false; ;
                                }
                            }
                        }
                        transction.Commit();
                        retVal = true;
                    }
                    catch (Exception ex)
                    {
                        transction.Rollback();
                        retVal = false;
                        throw ex;
                    }
                    return retVal;
                }
            }
        }
        public List<PayrollLeaveDeductionPolicyMasterBO> LeaveDeductionPolicyMaster()
        {
            List<PayrollLeaveDeductionPolicyMasterBO> MasterList = new List<PayrollLeaveDeductionPolicyMasterBO>();
            string query = string.Format("SELECT * FROM PayrollLeaveDeductionPolicyMaster");

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    PayrollLeaveDeductionPolicyMasterBO Master = new PayrollLeaveDeductionPolicyMasterBO();
                                    Master.Id = Convert.ToInt64(reader["Id"]);
                                    Master.NoOfLate = Convert.ToInt64(reader["NoOfLate"]);
                                    Master.NoOfLeave = Convert.ToInt64(reader["NoOfLeave"]);
                                    MasterList.Add(Master);
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

            return MasterList;

        }
        public PayrollLeaveDeductionPolicyMasterBO GetLeaveDeductionPolicyMasterById(long id)
        {
            PayrollLeaveDeductionPolicyMasterBO Master = new PayrollLeaveDeductionPolicyMasterBO();
            string query = string.Format("SELECT * FROM PayrollLeaveDeductionPolicyMaster  WHERE  Id = {0}", id);

            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    Master.Id = Convert.ToInt64(reader["Id"]);
                                    Master.NoOfLate = Convert.ToInt64(reader["NoOfLate"]);
                                    Master.NoOfLeave = Convert.ToInt64(reader["NoOfLeave"]);
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

            return Master;

        }
        public List<PayrollLeaveDeductionPolicyDetailBO> GetLeaveDeductionPolicyDetailsByMasterId(int masterId)
        {
            List<PayrollLeaveDeductionPolicyDetailBO> DetailsList = new List<PayrollLeaveDeductionPolicyDetailBO>();
            string query = string.Format("SELECT * FROM PayrollLeaveDeductionPolicyDetails WHERE MasterId = {0}", masterId);
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand cmd = dbSmartAspects.GetSqlStringCommand(query))
                    {
                        using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    PayrollLeaveDeductionPolicyDetailBO Master = new PayrollLeaveDeductionPolicyDetailBO();
                                    Master.Id = Convert.ToInt64(reader["Id"]);
                                    Master.LeaveId = Convert.ToInt32(reader["LeaveId"]);
                                    Master.Sequence = Convert.ToInt32(reader["Sequence"]);
                                    DetailsList.Add(Master);
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

            return DetailsList;

        }
        public Boolean DeleteLeaveDeductionPolicy(long Id)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("DeleteLeaveDeductionPolicy_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@Id", DbType.Int64, Id);

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
    }
}
