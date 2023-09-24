using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelManagement.Entity.Payroll;
using System.Data.Common;
using System.Data;

namespace HotelManagement.Data.Payroll
{
    public class SalaryHeadDA : BaseService
    {
        public List<SalaryHeadBO> GetSalaryHeadInfo()
        {
            List<SalaryHeadBO> salaryHeadList = new List<SalaryHeadBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalaryHeadInfo_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalaryHeadBO salaryHeadBO = new SalaryHeadBO();

                                salaryHeadBO.SalaryHeadId = Convert.ToInt32(reader["SalaryHeadId"]);
                                salaryHeadBO.SalaryHead = reader["SalaryHead"].ToString();
                                salaryHeadBO.IsShowOnlyAllownaceDeductionPage = Convert.ToBoolean(reader["IsShowOnlyAllownaceDeductionPage"].ToString());
                                salaryHeadBO.SalaryType = reader["SalaryType"].ToString();
                                salaryHeadBO.TransactionType = reader["TransactionType"].ToString();
                                salaryHeadBO.ContributionType = reader["ContributionType"].ToString();
                                salaryHeadBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                salaryHeadBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                salaryHeadList.Add(salaryHeadBO);
                            }
                        }
                    }
                }
            }
            return salaryHeadList;
        }

        public List<SalaryHeadBO> GetSalaryHeadInfoForDeduction()
        {
            List<SalaryHeadBO> salaryHeadList = new List<SalaryHeadBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalaryHeadInfoForDeduction_SP"))
                {
                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalaryHeadBO salaryHeadBO = new SalaryHeadBO();

                                salaryHeadBO.SalaryHeadId = Convert.ToInt32(reader["SalaryHeadId"]);
                                salaryHeadBO.SalaryHead = reader["SalaryHead"].ToString();
                                salaryHeadBO.IsShowOnlyAllownaceDeductionPage = Convert.ToBoolean(reader["IsShowOnlyAllownaceDeductionPage"].ToString());
                                salaryHeadBO.SalaryType = reader["SalaryType"].ToString();
                                salaryHeadBO.TransactionType = reader["TransactionType"].ToString();
                                //salaryHeadBO.EffectedMonth = Convert.ToDateTime(reader["EffectedMonth"]);
                                salaryHeadBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                salaryHeadBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                salaryHeadList.Add(salaryHeadBO);
                            }
                        }
                    }
                }
            }
            return salaryHeadList;
        }

        public SalaryHeadBO GetSalaryHeadInfoById(int SalaryHeadId)
        {
            SalaryHeadBO salaryHeadBO = new SalaryHeadBO();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalaryHeadInfoById_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SalaryHeadId", DbType.Int32, SalaryHeadId);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                salaryHeadBO.SalaryHeadId = Convert.ToInt32(reader["SalaryHeadId"]);
                                salaryHeadBO.SalaryHead = reader["SalaryHead"].ToString();
                                salaryHeadBO.IsShowOnlyAllownaceDeductionPage = Convert.ToBoolean(reader["IsShowOnlyAllownaceDeductionPage"].ToString());
                                salaryHeadBO.SalaryType = reader["SalaryType"].ToString();
                                salaryHeadBO.NodeId = Convert.ToInt64(reader["NodeId"]);
                                salaryHeadBO.TransactionType = reader["TransactionType"].ToString();
                                salaryHeadBO.ContributionType = reader["ContributionType"].ToString();
                                salaryHeadBO.VoucherMode = reader["VoucherMode"].ToString();
                                salaryHeadBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                salaryHeadBO.ActiveStatus = reader["ActiveStatus"].ToString();
                            }
                        }
                    }
                }
            }
            return salaryHeadBO;
        }
        public List<SalaryHeadBO> GetSalaryHeadInfoByCategory(bool? IsShowOnlyAllownaceDeductionPage)
        {
            List<SalaryHeadBO> salaryHeadBOList = new List<SalaryHeadBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalaryHeadInfoByCategory_SP"))
                {
                    if (IsShowOnlyAllownaceDeductionPage != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsShowOnlyAllownaceDeductionPage", DbType.Boolean, IsShowOnlyAllownaceDeductionPage);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsShowOnlyAllownaceDeductionPage", DbType.Boolean, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalaryHeadBO salaryHeadBO = new SalaryHeadBO();

                                salaryHeadBO.SalaryHeadId = Convert.ToInt32(reader["SalaryHeadId"]);
                                salaryHeadBO.SalaryHead = reader["SalaryHead"].ToString();
                                salaryHeadBO.IsShowOnlyAllownaceDeductionPage = Convert.ToBoolean(reader["IsShowOnlyAllownaceDeductionPage"].ToString());
                                salaryHeadBO.SalaryType = reader["SalaryType"].ToString();
                                salaryHeadBO.TransactionType = reader["TransactionType"].ToString();
                                salaryHeadBO.ContributionType = reader["ContributionType"].ToString();
                                salaryHeadBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                salaryHeadBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                salaryHeadBOList.Add(salaryHeadBO);
                            }
                        }
                    }
                }
            }
            return salaryHeadBOList;
        }

        public List<SalaryHeadBO> GetSalaryHeadInfoByType(string salaryType, bool? IsShowOnlyAllownaceDeductionPage)
        {
            List<SalaryHeadBO> salaryHeadBOList = new List<SalaryHeadBO>();

            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalaryHeadInfoByType_SP"))
                {
                    dbSmartAspects.AddInParameter(cmd, "@SalaryType", DbType.String, salaryType);

                    if (IsShowOnlyAllownaceDeductionPage != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsShowOnlyAllownaceDeductionPage", DbType.Boolean, IsShowOnlyAllownaceDeductionPage);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsShowOnlyAllownaceDeductionPage", DbType.Boolean, DBNull.Value);

                    using (IDataReader reader = dbSmartAspects.ExecuteReader(cmd))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                SalaryHeadBO salaryHeadBO = new SalaryHeadBO();

                                salaryHeadBO.SalaryHeadId = Convert.ToInt32(reader["SalaryHeadId"]);
                                salaryHeadBO.SalaryHead = reader["SalaryHead"].ToString();
                                salaryHeadBO.IsShowOnlyAllownaceDeductionPage = Convert.ToBoolean(reader["IsShowOnlyAllownaceDeductionPage"].ToString());
                                salaryHeadBO.SalaryType = reader["SalaryType"].ToString();
                                salaryHeadBO.TransactionType = reader["TransactionType"].ToString();
                                salaryHeadBO.ActiveStat = Convert.ToBoolean(reader["ActiveStat"]);
                                salaryHeadBO.ActiveStatus = reader["ActiveStatus"].ToString();

                                salaryHeadBOList.Add(salaryHeadBO);
                            }
                        }
                    }
                }
            }
            return salaryHeadBOList;
        }


        public bool SaveSalaryHeadInfo(SalaryHeadBO salaryHeadBO, out int tmpUserInfoId)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("SaveSalaryHeadInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@SalaryHead", DbType.String, salaryHeadBO.SalaryHead);
                        dbSmartAspects.AddInParameter(command, "@IsShowOnlyAllownaceDeductionPage", DbType.Boolean, salaryHeadBO.IsShowOnlyAllownaceDeductionPage);
                        dbSmartAspects.AddInParameter(command, "@SalaryType", DbType.String, salaryHeadBO.SalaryType);
                        dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int64, salaryHeadBO.NodeId);
                        dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, salaryHeadBO.TransactionType);
                        dbSmartAspects.AddInParameter(command, "@ContributionType", DbType.String, salaryHeadBO.ContributionType);
                        dbSmartAspects.AddInParameter(command, "@VoucherMode", DbType.String, salaryHeadBO.VoucherMode);
                        dbSmartAspects.AddInParameter(command, "@EffectedMonth", DbType.DateTime, salaryHeadBO.EffectedMonth);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, salaryHeadBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@CreatedBy", DbType.Int32, salaryHeadBO.CreatedBy);
                        dbSmartAspects.AddOutParameter(command, "@SalaryHeadId", DbType.Int32, sizeof(Int32));

                        status = dbSmartAspects.ExecuteNonQuery(command) > 0 ? true : false;

                        tmpUserInfoId = Convert.ToInt32(command.Parameters["@SalaryHeadId"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        public bool UpdateSalaryHeadInfo(SalaryHeadBO salaryHeadBO)
        {
            Boolean status = false;
            try
            {
                using (DbConnection conn = dbSmartAspects.CreateConnection())
                {
                    using (DbCommand command = dbSmartAspects.GetStoredProcCommand("UpdateSalaryHeadInfo_SP"))
                    {
                        dbSmartAspects.AddInParameter(command, "@SalaryHeadId", DbType.Int32, salaryHeadBO.SalaryHeadId);
                        dbSmartAspects.AddInParameter(command, "@SalaryHead", DbType.String, salaryHeadBO.SalaryHead);
                        dbSmartAspects.AddInParameter(command, "@IsShowOnlyAllownaceDeductionPage", DbType.Boolean, salaryHeadBO.IsShowOnlyAllownaceDeductionPage);
                        dbSmartAspects.AddInParameter(command, "@SalaryType", DbType.String, salaryHeadBO.SalaryType);
                        dbSmartAspects.AddInParameter(command, "@NodeId", DbType.Int64, salaryHeadBO.NodeId);
                        dbSmartAspects.AddInParameter(command, "@TransactionType", DbType.String, salaryHeadBO.TransactionType);
                        dbSmartAspects.AddInParameter(command, "@ContributionType", DbType.String, salaryHeadBO.ContributionType);
                        dbSmartAspects.AddInParameter(command, "@VoucherMode", DbType.String, salaryHeadBO.VoucherMode);
                        dbSmartAspects.AddInParameter(command, "@EffectedMonth", DbType.DateTime, salaryHeadBO.EffectedMonth);
                        dbSmartAspects.AddInParameter(command, "@ActiveStat", DbType.Boolean, salaryHeadBO.ActiveStat);
                        dbSmartAspects.AddInParameter(command, "@LastModifiedBy", DbType.Int32, salaryHeadBO.LastModifiedBy);

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

        public List<SalaryHeadBO> GetSalaryHeadInfoBySearchCriteria(string SalaryHead, int activeStat, bool? isShowOnlyAllownaceDeductionPage, string SalaryType, string TransactionType)
        {
            List<SalaryHeadBO> salaryHeadList = new List<SalaryHeadBO>();
            using (DbConnection conn = dbSmartAspects.CreateConnection())
            {
                using (DbCommand cmd = dbSmartAspects.GetStoredProcCommand("GetSalaryHeadInfoBySearchCriteria_SP"))
                {
                    if (!string.IsNullOrWhiteSpace(SalaryHead))
                        dbSmartAspects.AddInParameter(cmd, "@SalaryHead", DbType.String, SalaryHead);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SalaryHead", DbType.String, DBNull.Value);

                    if (isShowOnlyAllownaceDeductionPage != null)
                        dbSmartAspects.AddInParameter(cmd, "@IsShowOnlyAllownaceDeductionPage", DbType.Boolean, isShowOnlyAllownaceDeductionPage);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@IsShowOnlyAllownaceDeductionPage", DbType.Boolean, DBNull.Value);

                    if (activeStat > -1)
                        dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, activeStat);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@ActiveStat", DbType.Boolean, DBNull.Value);

                    if (SalaryType == "0")
                        dbSmartAspects.AddInParameter(cmd, "@SalaryType", DbType.String, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@SalaryType", DbType.String, SalaryType);

                    if (TransactionType == "0")
                        dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, DBNull.Value);
                    else
                        dbSmartAspects.AddInParameter(cmd, "@TransactionType", DbType.String, TransactionType);

                    DataSet SalaryHeadDS = new DataSet();
                    dbSmartAspects.LoadDataSet(cmd, SalaryHeadDS, "SalaryHead");
                    DataTable Table = SalaryHeadDS.Tables["SalaryHead"];

                    salaryHeadList = Table.AsEnumerable().Select(r => new SalaryHeadBO
                    {
                        SalaryHeadId = r.Field<Int32>("SalaryHeadId"),
                        SalaryHead = r.Field<string>("SalaryHead"),
                        SalaryType = r.Field<string>("SalaryType"),
                        TransactionType = r.Field<string>("TransactionType"),
                        ActiveStat = r.Field<bool>("ActiveStat"),
                        ActiveStatus = r.Field<string>("ActiveStatus")

                    }).ToList();
                }
            }
            return salaryHeadList;
        }
    }
}
